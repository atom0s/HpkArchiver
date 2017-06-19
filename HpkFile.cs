using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System.Runtime.InteropServices;

namespace HpkArchiver
{
    public delegate void FileSystemEntryProcessedHandler(FragmentType type);
    public delegate void FileSystemCountHandler(int count);
    public delegate void ProcessingFileDatesHandler();
    public delegate void CompletedHandler(TimeSpan duration, string description);

    public class HpkFile
    {
        public event FileSystemEntryProcessedHandler FileSystemEntryProcessed;
        public event FileSystemCountHandler FileSystemCount;
        public event ProcessingFileDatesHandler ProcessingFileDates;
        public event CompletedHandler Completed;

        private int _ErrorsCaught = 0;
        public int ErrorsCaught
        {
            get { return _ErrorsCaught; }
        }

        private int _FilesHandled = 0;
        public int FilesHandled
        {
            get { return _FilesHandled; }
        }

        private int _DirectoriesHandled = 0;
        public int DirectoriesHandled
        {
            get { return _DirectoriesHandled; }
        }

        private Thread _Thread = null;
        public void Abort()
        {
            if (_Thread != null)
                _Thread.Abort();
        }

        private Game _Game = Game.Tropico3;
        public Game Game
        {
            get { return _Game; }
            set { _Game = value; }
        }

        private const int FRAGMENT_LENGTH = 8; // The length, in bytes, of how long each fragment header (offset/length pairs) appears in the file.
        private const int BUFFER_SIZE = 4096;
        private const int COMPRESS_BUFFER_SIZE = 32768;
        private readonly string[] COMPRESS_EXTENSIONS = new string[] { ".lua", ".xml", ".tga", ".dds", ".xtex", ".bin", ".csv" };

        public HpkFile()
        {
        }

        private Header _Header;
        private List<Fragment> _ResidualCollection;
        public long ResidualBytes
        {
            get
            {
                long output = 0;
                for (int i = 0; i < _ResidualCollection.Count; i++)
                    output += _ResidualCollection[i].Length;
                return output;
            }
        }
        private List<FragmentedFile> _FragmentedFileCollection;
        private List<FileEntryExtended> _FileSystemExtendedCollection;

        public void Clear()
        {
            _ErrorsCaught = 0;
            _FilesHandled = 0;
            _DirectoriesHandled = 0;
            _Header = new Header();
            _ResidualCollection = new List<Fragment>();
            _FragmentedFileCollection = new List<FragmentedFile>();
            _FileSystemExtendedCollection = new List<FileEntryExtended>();
        }

        #region " Extract "
        public void StartExtract(string source, string dest)
        {
            _Thread = new Thread(Extract);
            _Thread.Name = "HpkExtract";
            _Thread.Start(new HpkExtractArgs(source, dest));
        }
        /// <summary>
        /// Extracts contained directory hive out of an HPK archive.
        /// </summary>
        /// <param name="args">Must be an instance of HpkExtractArgs.</param>
        public void Extract(object args)
        {
            Clear();

            HpkExtractArgs arguments = (HpkExtractArgs)args;

            DateTime start = DateTime.Now;

            FileStream fs = new FileStream(arguments.SourceFileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            #region " Unzip file if zipped "
            string unpacked = ""; // temp file.
            if (Encoding.ASCII.GetString(br.ReadBytes(4)) == "ZLIB") // yo zipped!
            {
                unpacked = Static.TempZippedFile;
                FragmentedFile hpk = new FragmentedFile();
                hpk.Fragments.Add(new Fragment(0, Convert.ToInt32(fs.Length)));
                _ErrorsCaught += hpk.ExtractZippedFile(ref br, unpacked);
                br.Close();
                fs.Close();
                fs = new FileStream(unpacked, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
            }
            #endregion

            #region " Header "
            fs.Seek(0, SeekOrigin.Begin); // reset to beginning of file
            _Header = new Header(br.ReadBytes(Header.LENGTH));
            int filesystementries = _Header.FragmentedFileSystemLength / (FRAGMENT_LENGTH * _Header.FragmentsPerFile);
            #endregion

            #region " Residual "
            fs.Seek(_Header.FragmentsResidualOffset, SeekOrigin.Begin);
            for (int i = 0; i < _Header.FragmentsResidualCount; i++)
                _ResidualCollection.Add(new Fragment(br.ReadBytes(FRAGMENT_LENGTH)));
            #endregion

            FileSystemCountHandler counted = FileSystemCount;
            if (counted != null)
                counted(filesystementries);

            #region " File System "
            fs.Seek(_Header.FragmentedFileSystemOffset, SeekOrigin.Begin);
            for (int i = 0; i < filesystementries; i++)
                _FragmentedFileCollection.Add(new FragmentedFile(ref br, _Header.FragmentsPerFile));

            ReadDirectory(0, arguments.DestinationFolderName, ref br);
            #endregion

            br.Close();
            fs.Close();

            if ((unpacked.Length > 0) && File.Exists(unpacked)) // remove temp file
                Static.TryDeleteFile(unpacked);

            ProcessFileDates(arguments.DestinationFolderName + Static.PlatformSlash() + "_filedates");

            CompletedHandler completed = Completed;
            if (completed != null)
                completed(DateTime.Now - start, "extracted");
        }

        private void ReadDirectory(int entrylink, string workingdir, ref BinaryReader br)
        {
            if (_FragmentedFileCollection[entrylink].Fragments.Count == 0)
            {
                StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                sw.WriteLine("Warning: File at index " + entrylink.ToString() + " skipped due to not having any attached data.");
                sw.Flush();
                sw.Close();
            }
            else
            {
                br.BaseStream.Seek(_FragmentedFileCollection[entrylink][0].Offset, SeekOrigin.Begin);
                byte[] temp = br.ReadBytes(_FragmentedFileCollection[entrylink][0].Length);

                int position = 0;
                while (position < _FragmentedFileCollection[entrylink][0].Length)
                {
                    br.BaseStream.Seek(_FragmentedFileCollection[entrylink][0].Offset + position, SeekOrigin.Begin);

                    FileEntry entry = new FileEntry();

                    entry.FragmentIndex = BitConverter.ToInt32(temp, position);
                    position += 4;

                    entry.Type = (FragmentType)BitConverter.ToUInt32(temp, position);
                    position += 4;

                    entry.NameLength = BitConverter.ToUInt16(temp, position);
                    position += 2;

                    entry.Name = Encoding.ASCII.GetString(temp, position, entry.NameLength);
                    position += entry.NameLength;

                    string dir = workingdir;
                    if (entry.Type == FragmentType.Directory)
                    {
                        dir += Static.PlatformSlash() + entry.Name;
                        Directory.CreateDirectory(dir);
                        ReadDirectory(entry.FragmentIndex - 1, dir, ref br);

                        _DirectoriesHandled++;
                        FileSystemEntryProcessedHandler h = FileSystemEntryProcessed;
                        if (h != null)
                            h(FragmentType.Directory);
                    }
                    else
                    {
                        string dest = dir + Static.PlatformSlash() + entry.Name;
                        if (_FragmentedFileCollection[entry.FragmentIndex - 1].Fragments.Count == 0)
                        {
                            File.Create(dest).Close(); // Make blank file.

                            _ErrorsCaught++;
                            StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                            sw.WriteLine(dest + "\tNo fragments, outputting blank file.");
                            sw.Flush();
                            sw.Close();
                        }
                        else
                        {
                            br.BaseStream.Seek(_FragmentedFileCollection[entry.FragmentIndex - 1][0].Offset, SeekOrigin.Begin);

                            var data = br.ReadBytes(4);
                            if (Encoding.ASCII.GetString(data) == "ZLIB")
                                _ErrorsCaught += _FragmentedFileCollection[entry.FragmentIndex - 1].ExtractZippedFile(ref br, dest);
                            else
                            {
                                if (Encoding.ASCII.GetString(data).Contains("LZ4"))
                                    _ErrorsCaught += _FragmentedFileCollection[entry.FragmentIndex - 1].ExtractZippedFileLZ4(ref br, dest);
                                else
                                    _ErrorsCaught += _FragmentedFileCollection[entry.FragmentIndex - 1].ExtractFile(ref br, dest);
                            }

                            _FilesHandled++;
                            FileSystemEntryProcessedHandler h = FileSystemEntryProcessed;
                            if (h != null)
                                h(FragmentType.File);
                        }
                    }
                    _FragmentedFileCollection[entry.FragmentIndex - 1].Entry = entry;
                }
            }
        }

        private void ProcessFileDates(string filedates)
        {
            FileInfo src = new FileInfo(filedates);
            if (src.Exists)
            {
                ProcessingFileDatesHandler h = ProcessingFileDates;
                if (h != null)
                    h();

                string path = src.DirectoryName + Static.PlatformSlash();

                StreamReader sr = new StreamReader(filedates);
                string a, b;
                switch (_Game)
                {
                    case Game.Tropico4:
                    case Game.Tropico5:
                        while (sr.Peek() != -1)
                        {
                            Static.SplitOnce(sr.ReadLine(), '=', out a, out b);
                            try { File.SetLastWriteTimeUtc(path + Static.PlatformPathReverse(a), DateTime.FromFileTime(Convert.ToInt64(b) * 2000)); }
                            catch { } // it is a directory and setting the timestamp on a directory is forbidden
                        }
                        break;
                    default:
                        while (sr.Peek() != -1)
                        {
                            Static.SplitOnce(sr.ReadLine(), '=', out a, out b);
                            try { File.SetLastWriteTimeUtc(path + Static.PlatformPathReverse(a), DateTime.FromFileTime(Convert.ToInt64(b))); }
                            catch { } // it is a directory and setting the timestamp on a directory is forbidden
                        }
                    break;
                }
                sr.Close();

                // This file is now irrelevant.
                src.Delete();
            }
        }
        #endregion

        #region " Archive "
        public void StartArchive(string source, string dest, bool zip)
        {
            _Thread = new Thread(Archive);
            _Thread.Name = "HpkArchive";
            _Thread.Start(new HpkArchiveArgs(source, dest, zip));
        }
        /// <summary>
        /// Packs contained directory hive into an HPK archive.
        /// </summary>
        /// <param name="args">An instance of HpkArchiveArgs which tells it what to do.</param>
        public void Archive(object args)
        {
            Clear();

            HpkArchiveArgs arguments = (HpkArchiveArgs)args;

            DateTime start = DateTime.Now;

            DiscoverDirectories(new DirectoryInfo(arguments.SourceFolderName));

            FileEntryExtended fse = new FileEntryExtended();
            fse.FragmentIndex = _FileSystemExtendedCollection.Count + 1;
            fse.Name = "_filedates";
            fse.NameLength = Convert.ToUInt16(fse.Name.Length);
            fse.Type = FragmentType.File;
            _FileSystemExtendedCollection[0].FileIndices.Add(_FileSystemExtendedCollection.Count);
            _FileSystemExtendedCollection.Add(fse);

            FileSystemCountHandler counted = FileSystemCount;
            if (counted != null)
                counted(_FileSystemExtendedCollection.Count - 1);

            FileStream output;
            string packed = "";
            if (arguments.ZipIt)
            {
                packed = Static.TempZippedFile;
                output = new FileStream(packed, FileMode.Create, FileAccess.ReadWrite);
            }
            else
                output = new FileStream(arguments.DestinationFileName, FileMode.Create, FileAccess.ReadWrite);
            output.Seek(Header.LENGTH, SeekOrigin.Begin); // reserve room for the header which will be written there later
            BinaryWriter bw = new BinaryWriter(output);

            #region " Write Data "
            for (int i = 0; i < _FileSystemExtendedCollection.Count - 1; i++)
            {
                if (_FileSystemExtendedCollection[i].Type == FragmentType.Directory)
                {
                    _FileSystemExtendedCollection[i].Offset = Convert.ToUInt32(output.Position);

                    for (int j = 0; j < _FileSystemExtendedCollection[i].DirIndices.Count; j++)
                        bw.Write(_FileSystemExtendedCollection[_FileSystemExtendedCollection[i].DirIndices[j]].GetBytes());
                    for (int j = 0; j < _FileSystemExtendedCollection[i].FileIndices.Count; j++)
                        bw.Write(_FileSystemExtendedCollection[_FileSystemExtendedCollection[i].FileIndices[j]].GetBytes());
                    bw.Flush();

                    _FileSystemExtendedCollection[i].Length = Convert.ToInt32(output.Position - _FileSystemExtendedCollection[i].Offset);

                    _DirectoriesHandled++;
                    FileSystemEntryProcessedHandler h = FileSystemEntryProcessed;
                    if (h != null)
                        h(FragmentType.Directory);
                }
                else
                {
                    if (_FileSystemExtendedCollection[i].Compress)
                        ArchiveZippedFile(ref bw, i);
                    else
                        ArchiveFile(ref bw, i);

                    _FilesHandled++;
                    FileSystemEntryProcessedHandler h = FileSystemEntryProcessed;
                    if (h != null)
                        h(FragmentType.File);
                }
            }
            #endregion

            #region " Write _filedates"
            int filedates_index = _FileSystemExtendedCollection[0].FileIndices[_FileSystemExtendedCollection[0].FileIndices.Count - 1];
            _FileSystemExtendedCollection[filedates_index].Offset = Convert.ToUInt32(output.Position);
            StreamWriter sw = new StreamWriter(output);
            switch (_Game)
            {
                case Game.Tropico4:
                case Game.Tropico5:
                    for (int i = 1; i < _FileSystemExtendedCollection.Count - 1; i++)
                    {
                        sw.WriteLine(Static.PlatformPathToUnix(_FileSystemExtendedCollection[i].FullName.Substring(_FileSystemExtendedCollection[0].FullName.Length + 1)) + "=" + Convert.ToInt64(Math.Round((decimal)_FileSystemExtendedCollection[i].FileTime / (decimal)2000, 0)));
                        if (i % 10 == 0)
                            sw.Flush();
                    }
                    break;
                default:
                    for (int i = 1; i < _FileSystemExtendedCollection.Count - 1; i++)
                    {
                        sw.WriteLine(Static.PlatformPathToUnix(_FileSystemExtendedCollection[i].FullName.Substring(_FileSystemExtendedCollection[0].FullName.Length + 1)) + "=" + _FileSystemExtendedCollection[i].FileTime);
                        if (i % 10 == 0)
                            sw.Flush();
                    }
                    break;
            }
            sw.Flush();
            _FileSystemExtendedCollection[filedates_index].Length = Convert.ToInt32(output.Position - _FileSystemExtendedCollection[_FileSystemExtendedCollection.Count - 1].Offset);
            #endregion

            #region " Write Header "
            Header header = new Header();
            header.DataOffset = Header.LENGTH;
            header.FragmentsPerFile = 1; // Easiest to work with so simplify all down to one.
            header.Unknown2 = -1;
            header.FragmentsResidualOffset = 0;
            header.FragmentsResidualCount = 0;
            header.Unknown5 = 1;

            header.FragmentedFileSystemOffset = Convert.ToUInt32(output.Position); 
            for (int i = 0; i < _FileSystemExtendedCollection.Count; i++)
            {
                bw.Write(_FileSystemExtendedCollection[i].Offset);
                bw.Write(_FileSystemExtendedCollection[i].Length);
            }
            bw.Flush();
            header.FragmentedFileSystemLength = Convert.ToInt32(output.Position - header.FragmentedFileSystemOffset);

            output.Seek(0, SeekOrigin.Begin);
            bw.Write(header.GetBytes());
            bw.Flush();
            sw.Close();
            bw.Close();
            output.Close();
            #endregion

            Static.TryDeleteFile(Static.TempFile);

            if ((packed.Length > 0) && File.Exists(packed))
            {
                ZipFile(packed, arguments.DestinationFileName);
                Static.TryDeleteFile(packed);
            }

            CompletedHandler completed = Completed;
            if (completed != null)
                completed(DateTime.Now - start, "archived");
        }

        private int DiscoverDirectories(DirectoryInfo directory)
        {
            int index = _FileSystemExtendedCollection.Count;
            FileEntryExtended dir_fse = new FileEntryExtended();
            dir_fse.FullName = directory.FullName;
            dir_fse.FragmentIndex = _FileSystemExtendedCollection.Count + 1;
            dir_fse.Name = directory.Name;
            dir_fse.NameLength = Convert.ToUInt16(directory.Name.Length);
            dir_fse.Type = FragmentType.Directory;
            dir_fse.FileTime = directory.LastWriteTimeUtc.ToFileTime();
            _FileSystemExtendedCollection.Add(dir_fse);

            foreach (DirectoryInfo dir in directory.GetDirectories())
                _FileSystemExtendedCollection[index].DirIndices.Add(DiscoverDirectories(dir));

            foreach (FileInfo file in directory.GetFiles())
            {
                FileEntryExtended fse = new FileEntryExtended();
                fse.FullName = file.FullName;
                fse.FragmentIndex = _FileSystemExtendedCollection.Count + 1;
                fse.Name = file.Name;
                fse.NameLength = Convert.ToUInt16(file.Name.Length);
                fse.Type = FragmentType.File;
                fse.Length = Convert.ToInt32(file.Length);
                if ((Array.IndexOf<string>(COMPRESS_EXTENSIONS, file.Extension) != -1) && (fse.Length > 255))
                    fse.Compress = true;
                fse.FileTime = file.LastWriteTimeUtc.ToFileTime();
                _FileSystemExtendedCollection[index].FileIndices.Add(_FileSystemExtendedCollection.Count);
                _FileSystemExtendedCollection.Add(fse);
            }

            return index;
        }

        private void ArchiveFile(ref BinaryWriter bw, int index)
        {
            _FileSystemExtendedCollection[index].Offset = Convert.ToUInt32(bw.BaseStream.Position);

            FileStream input = new FileStream(_FileSystemExtendedCollection[index].FullName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(input);
            int length = _FileSystemExtendedCollection[index].Length;
            byte[] buffer;
            while (length > 0)
            {
                if (length < BUFFER_SIZE)
                {
                    buffer = br.ReadBytes(length);
                    length -= length;
                }
                else
                {
                    buffer = br.ReadBytes(BUFFER_SIZE);
                    length -= BUFFER_SIZE;
                }

                bw.Write(buffer);
                bw.Flush();
            }
            br.Close();
            input.Close();

            _FileSystemExtendedCollection[index].Length = Convert.ToInt32(bw.BaseStream.Position - _FileSystemExtendedCollection[index].Offset);
        }
        private void ArchiveZippedFile(ref BinaryWriter bw, int index)
        {

            ZlibHeader header = new ZlibHeader();
            header.ChunkSize = COMPRESS_BUFFER_SIZE;
            header.InflatedLength = Convert.ToUInt32(_FileSystemExtendedCollection[index].Length);

            byte[] buffer;
            byte[] output_buffer = new byte[header.ChunkSize];
            FileStream input = new FileStream(_FileSystemExtendedCollection[index].FullName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(input);
            int length = _FileSystemExtendedCollection[index].Length;

            FileStream temp = new FileStream(Static.TempFile, FileMode.Create, FileAccess.ReadWrite);

            int result = 0;
            while (length > 0)
            {
                header.OffsetCollection.Add(Convert.ToInt32(temp.Position));

                if (length < header.ChunkSize)
                {
                    buffer = br.ReadBytes(length);
                    length -= length;
                }
                else
                {
                    buffer = br.ReadBytes(header.ChunkSize);
                    length -= header.ChunkSize;
                }

                Deflater deflate = new Deflater(Deflater.DEFAULT_COMPRESSION, false);
                result = 0;
                try
                {
                    deflate.SetInput(buffer);
                    deflate.Finish();
                    result = deflate.Deflate(output_buffer);
                }
                catch { };
                if (result == 0)
                {
                    _ErrorsCaught++;
                    StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                    sw.WriteLine(_FileSystemExtendedCollection[index].FullName + "\tCould not compress the file.");
                    sw.Flush();
                    sw.Close();
                    break;
                }
                else if (result == header.ChunkSize) // Fragmentted extract won't attempt to decompress this chunk so we have to make sure it is copied as-is.
                    temp.Write(buffer, 0, result);
                else
                    temp.Write(output_buffer, 0, result);
                temp.Flush();
            }
            br.Close();
            input.Close();

            input = null;
            br = null;

            if (result == 0)
                ArchiveFile(ref bw, index);
            else
            {
                _FileSystemExtendedCollection[index].Offset = Convert.ToUInt32(bw.BaseStream.Position);

                int size = header.GetSize();
                for (int i = 0; i < header.OffsetCollection.Count; i++)
                    header.OffsetCollection[i] += size;
                bw.Write(header.GetBytes());

                length = Convert.ToInt32(temp.Length);
                temp.Seek(0, SeekOrigin.Begin);

                br = new BinaryReader(temp);

                while (length > 0)
                {
                    if (length < BUFFER_SIZE)
                    {
                        buffer = br.ReadBytes(length);
                        length -= length;
                    }
                    else
                    {
                        buffer = br.ReadBytes(BUFFER_SIZE);
                        length -= BUFFER_SIZE;
                    }

                    bw.Write(buffer);
                    bw.Flush();
                }
                br.Close();
                temp.Close();

                _FileSystemExtendedCollection[index].Length = Convert.ToInt32(bw.BaseStream.Position - _FileSystemExtendedCollection[index].Offset);
            }
        }
        private void ZipFile(string src, string dest)
        {
            FileInfo fi_src = new FileInfo(src);

            ZlibHeader header = new ZlibHeader();
            header.ChunkSize = COMPRESS_BUFFER_SIZE;
            header.InflatedLength = Convert.ToUInt32(fi_src.Length);

            byte[] buffer;
            byte[] output_buffer = new byte[header.ChunkSize];
            FileStream input = new FileStream(fi_src.FullName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(input);
            int length = Convert.ToInt32(fi_src.Length);

            FileStream temp = new FileStream(Static.TempFile, FileMode.Create, FileAccess.ReadWrite);

            int result = 0;
            while (length > 0)
            {
                header.OffsetCollection.Add(Convert.ToInt32(temp.Position));

                if (length < header.ChunkSize)
                {
                    buffer = br.ReadBytes(length);
                    length -= length;
                }
                else
                {
                    buffer = br.ReadBytes(header.ChunkSize);
                    length -= header.ChunkSize;
                }

                Deflater deflate = new Deflater(Deflater.DEFAULT_COMPRESSION, false);
                result = 0;
                try
                {
                    deflate.SetInput(buffer);
                    deflate.Finish();
                    result = deflate.Deflate(output_buffer);
                }
                catch { };
                if (result == 0)
                {
                    _ErrorsCaught++;
                    StreamWriter sw = new StreamWriter(Static.ErrorFile, true);
                    sw.WriteLine(fi_src.FullName + "\tCould not compress the file.");
                    sw.Flush();
                    sw.Close();
                    break;
                }
                else if (result == header.ChunkSize) // Fragmentted extract won't attempt to decompress this chunk so we have to make sure it is copied as-is.
                    temp.Write(buffer, 0, result);
                else
                    temp.Write(output_buffer, 0, result);
                temp.Flush();
            }
            br.Close();
            input.Close();

            input = null;
            br = null;

            if (result != 0)
            {
                FileStream output = new FileStream(dest, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(output);

                int size = header.GetSize();
                for (int i = 0; i < header.OffsetCollection.Count; i++)
                    header.OffsetCollection[i] += size;
                bw.Write(header.GetBytes());

                length = Convert.ToInt32(temp.Length);
                temp.Seek(0, SeekOrigin.Begin);

                br = new BinaryReader(temp);

                while (length > 0)
                {
                    if (length < BUFFER_SIZE)
                    {
                        buffer = br.ReadBytes(length);
                        length -= length;
                    }
                    else
                    {
                        buffer = br.ReadBytes(BUFFER_SIZE);
                        length -= BUFFER_SIZE;
                    }

                    bw.Write(buffer);
                    bw.Flush();
                }
                br.Close();
                temp.Close();
                output.Close();
            }

            Static.TryDeleteFile(Static.TempFile);
        }
        #endregion
    }

    #region " Helper Classes "
    public struct HpkExtractArgs
    {
        public string SourceFileName;
        public string DestinationFolderName;

        public HpkExtractArgs(string source, string dest)
        {
            SourceFileName = source;
            DestinationFolderName = dest;
        }
    }
    public struct HpkArchiveArgs
    {
        public string SourceFolderName;
        public string DestinationFileName;
        public bool ZipIt;

        public HpkArchiveArgs(string source, string dest, bool zip)
        {
            SourceFolderName = source;
            DestinationFileName = dest;
            ZipIt = zip;
        }
    }
    #endregion
}
