using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System.Text;

namespace HpkArchiver
{
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Forms;
    using LZ4;

    public class FragmentedFile
    {
        public FileEntry Entry { get; set; }

        public List<Fragment> Fragments { get; set; }
        public Fragment this[int i]
        {
            get { return Fragments[i]; }
            set { Fragments[i] = value; }
        }

        public long Length
        {
            get
            {
                long length = 0;
                for (int i = 0; i < Fragments.Count; i++)
                    length += Fragments[i].Length;
                return length;
            }
        }
        public bool Fragmented
        {
            get
            {
                if (Fragments == null)
                    return false;
                else
                {
                    if (Fragments.Count > 1)
                        return true;
                    else
                        return false;
                }
            }
        }

        public FragmentedFile()
        {
            Fragments = new List<Fragment>();
        }
        public FragmentedFile(List<Fragment> fragments)
        {
            Fragments = fragments;
        }
        public FragmentedFile(ref BinaryReader br, int fragments)
        {
            Fragments = new List<Fragment>();
            for (int i = 0; i < fragments; i++)
            {
                Fragment temp = new Fragment(br.ReadUInt32(), br.ReadInt32());
                if (!temp.IsNull)
                    Fragments.Add(temp);
            }
        }

        /// <summary>
        /// Extracts a fragmented file from the archive to the destination.  Does NOT decompress.
        /// </summary>
        /// <param name="br">Stream to use to extract the file.</param>
        /// <param name="dest">Path to where the extracted file should be saved.</param>
        /// <returns>Number of errors caught.</returns>
        public int ExtractFile(ref BinaryReader br, string dest)
        {
            int errorscaught = 0;
            try
            {
                byte[] buffer = new byte[4096];
                int read;
                FileStream fs = new FileStream(dest, FileMode.Create, FileAccess.Write);
                for (int i = 0; i < Fragments.Count; i++)
                {
                    br.BaseStream.Seek(Fragments[i].Offset, SeekOrigin.Begin);
                    int length = Fragments[i].Length;
                    while (length > 0)
                    {
                        if (length < buffer.Length)
                            read = br.BaseStream.Read(buffer, 0, length);
                        else
                            read = br.BaseStream.Read(buffer, 0, buffer.Length);
                        length -= read;
                        fs.Write(buffer, 0, read);
                        fs.Flush();
                    }
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                errorscaught++;
                StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                sw.WriteLine(dest + "\tCould not extract file: " + ex.Message);
                sw.Flush();
                sw.Close();
            }
            return errorscaught;
        }
        /// <summary>
        /// Extracts a fragmented file from the archive to the destination.  If it is zipped, decompresses it.
        /// </summary>
        /// <param name="br">Stream to use to extract the file.</param>
        /// <param name="dest">Path to where the extracted file should be saved.</param>
        /// <returns>Number of errors caught.</returns>
        public int ExtractZippedFile(ref BinaryReader br, string dest)
        {
            int errorscaught = 0;
            try
            {
                if (Fragments.Count == 0 || Length < 16) // Bug fix for corruption/incomplete/empty file
                {
                    File.Create(dest).Close(); // Make blank file.

                    errorscaught++;
                    StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                    sw.WriteLine(dest + "\tWarning: File too small to decompress, outputting blank file.");
                    sw.Flush();
                    sw.Close();

                    return errorscaught;
                }

                FragmentedBinaryReader fbr = new FragmentedBinaryReader(ref br, Fragments);

                ZlibHeader header = new ZlibHeader();
                byte[] head_bytes = fbr.ReadFragmented(16);
                header.IdentifyingChars = Encoding.ASCII.GetString(head_bytes, 0, 4);
                header.InflatedLength = BitConverter.ToUInt32(head_bytes, 4);
                header.ChunkSize = BitConverter.ToInt32(head_bytes, 8);
                header.OffsetCollection.Add(BitConverter.ToInt32(head_bytes, 12));
                if (header.OffsetCollection[0] != 16)
                {
                    head_bytes = fbr.ReadFragmented(header.OffsetCollection[0] - 16);
                    for (int i = 0; i < head_bytes.Length; i += 4)
                        header.OffsetCollection.Add(BitConverter.ToInt32(head_bytes, i));
                }

                int len = Convert.ToInt32(Length);
                Stack<int> lengths = new Stack<int>(header.OffsetCollection.Count);
                for (int i = header.OffsetCollection.Count - 1; i >= 0; i--)
                {
                    lengths.Push(len - header.OffsetCollection[i]);
                    len -= lengths.Peek();
                }
                len -= header.OffsetCollection[0];


                FileInfo fi = new FileInfo(dest);
                if (!fi.Directory.Exists)
                    fi.Directory.Create();

                byte[] buffer = new byte[4096];
                FileStream compressed_out = new FileStream(dest, FileMode.Create, FileAccess.Write);

                int o = 0;
                int result = 0;
                while (lengths.Count > 0)
                {
                    len = lengths.Pop();

                    byte[] destination = new byte[header.ChunkSize];
                    byte[] source = fbr.ReadFragmented(len);

                    Inflater inflate = new Inflater();

                    result = 0;
                    try
                    {
                        if (source.Length == header.ChunkSize)
                            throw new Exception("Warning: Can't inflate when expected size matches input size."); // Send to catch to be written

                        // attempt to inflate
                        inflate.SetInput(source);
                        result = inflate.Inflate(destination);
                    }
                    catch { result = 0; }; // fall through to the code below
                    if (result == 0)
                    {
                        // dump the chunk to file -- it may not be compressed at all
                        Array.Copy(source, destination, source.Length);
                        result = source.Length;
                    }
                    compressed_out.Write(destination, 0, result);
                    compressed_out.Flush();
                    o++;
                }
                compressed_out.Close();

                if (result == 0)
                {
                    if (header.OffsetCollection.Count == 1 && header.InflatedLength < 32)
                    {
                        StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                        sw.WriteLine(dest + "\tWarning: Could not decompress the file.  Writing raw data.");
                        sw.Flush();
                        sw.Close();

                        FileStream fs = new FileStream(dest, FileMode.Create, FileAccess.Write);
                        br.BaseStream.Seek(Fragments[0].Offset + header.OffsetCollection[0], SeekOrigin.Begin);
                        buffer = br.ReadBytes(Convert.ToInt32(header.InflatedLength));
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Flush();
                        fs.Close();
                    }
                    else
                    {
                        errorscaught++;
                        StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                        sw.WriteLine(dest + "\tCould not decompress the file.  Writing compressed file.");
                        sw.Flush();
                        sw.Close();

                        ExtractFile(ref br, dest);
                    }
                }
            }
            catch (Exception ex)
            {
                errorscaught++;
                StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                sw.WriteLine(dest + "\t" + ex.Message);
                sw.Flush();
                sw.Close();
            }

            return errorscaught;
        }

        /// <summary>
        /// Extracts a fragmented file from the archive to the destination. If it is compressed, decompress it. (LZ4 Compression)
        /// </summary>
        /// <param name="br"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public int ExtractZippedFileLZ4(ref BinaryReader br, string dest)
        {
            var errorsCaught = 0;

#if DEBUG
            Debug.WriteLine(dest);
#endif

            try
            {
                // Read the compressed data..
                br.BaseStream.Position = this.Fragments[0].Offset;
                var compressedData = br.ReadBytes(this.Fragments[0].Length);

                // Read the header data..
                br.BaseStream.Position = this.Fragments[0].Offset + 4;
                var size = br.ReadInt32();
                var chunkSize = br.ReadInt32();

                // Validate the fragments size..
                var frags = this.Fragments.Count;
                if (frags != 1)
                {
                    MessageBox.Show($"Found invalid fragment count while extracting! Frag Count: {frags}");
                    return 1;
                }

                // Determine the number of chunks..
                var temp1 = size % chunkSize;
                var temp2 = temp1 != 0 ? 1 : 0;
                var chunks = (size / chunkSize) + temp2;

                // Process the file chunks..
                var fileData = new List<byte>();
                for (var x = 1; x <= chunks; x++)
                {
                    // Read the current chunk offset..
                    var offset = br.ReadInt32();
                    var zsize = 0;

                    // Store the stream position..
                    var position = br.BaseStream.Position;

                    // Calculate the compressed size..
                    zsize = x == chunks ? this.Fragments[0].Length : br.ReadInt32();
                    zsize -= offset;

#if DEBUG
                    Debug.WriteLine(
                        $"Offset: {offset:X08} // ZSize: {zsize:X08} // NOffset: {(offset + zsize):X08} // Size: {compressedData.Length:X08} // ChunkSize: {chunkSize:X08} // Chunk: {x}/{chunks}"
                        );
#endif

                    // Restore the stream position..
                    br.BaseStream.Position = position;

                    // Handle the data accordingly..
                    if (zsize < chunkSize)
                    {
                        try
                        {
                            // LZ4 decompress the data..
                            var output = new byte[chunkSize];
                            var ret = LZ4Codec.Decode(compressedData, offset, zsize, output, 0, chunkSize, false);
                            if (ret > 0)
                                fileData.AddRange(output.Take(ret));
                            else
                            {
                                errorsCaught++;
                                var sw = new StreamWriter(Static.ErrorFile, true);
                                sw.WriteLine(dest + "\tWarning: Failed to decompress file.");
                                sw.Flush();
                                sw.Close();
                            }
                        }
                        catch
                        {
                            /**
                             * LZ4 decompression failed, at this point from testing it appears
                             * just appending the data that failed seems to be a valid method
                             * of handling this error. Of the few files tested, this always created
                             * valid output files.
                             */
                            var output = new byte[zsize];
                            Array.Copy(compressedData, offset, output, 0, zsize);
                            fileData.AddRange(output);
                        }
                    }
                    else
                    {
                        var output = new byte[zsize];
                        Array.Copy(compressedData, offset, output, 0, zsize);
                        fileData.AddRange(output);
                    }
                }

                // Write the output file..
                File.WriteAllBytes(dest, fileData.ToArray());
            }
            catch (Exception ex)
            {
                errorsCaught++;
                var sw = new StreamWriter(Static.ErrorFile, true);
                sw.WriteLine(dest + "\t" + ex.Message);
                sw.Flush();
                sw.Close();
            }

            return errorsCaught;
        }

        public override string ToString()
        {
            if (Entry == null)
            {
                if (Fragments.Count > 1)
                    return Fragments[0].Offset + "," + Length.ToString() + "*";
                else
                    return Fragments[0].ToString();
            }
            else
            {
                if (Fragments.Count > 1)
                    return Entry.Name + " (" + Fragments[0].Offset + "," + Length.ToString() + "*)";
                else
                    return Entry.Name + " (" + Fragments[0].ToString() + ")";
            }
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(FragmentedFile))
            {
                FragmentedFile ff = (FragmentedFile)obj;
                if (ff.Fragments == Fragments)
                    return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Fragments.GetHashCode();
        }
    }
}
