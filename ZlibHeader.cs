using System;
using System.Collections.Generic;
using System.Text;

namespace HpkArchiver
{
    public class ZlibHeader
    {
        public string IdentifyingChars;
        public uint InflatedLength;
        public int ChunkSize;
        public List<int> OffsetCollection;

        public ZlibHeader()
        {
            IdentifyingChars = "ZLIB";
            OffsetCollection = new List<int>();
        }

        public int GetSize()
        {
            return 12 + (OffsetCollection.Count * 4); 
        }
        public byte[] GetBytes()
        {
            byte[] output = new byte[GetSize()];
            Encoding.ASCII.GetBytes(IdentifyingChars).CopyTo(output, 0);
            BitConverter.GetBytes(InflatedLength).CopyTo(output, 4);
            BitConverter.GetBytes(ChunkSize).CopyTo(output, 8);
            for (int i = 0; i < OffsetCollection.Count; i++)
                BitConverter.GetBytes(OffsetCollection[i]).CopyTo(output, 12 + (i * 4));
            return output;
        }
    }
}
