using System;
using System.Collections.Generic;

namespace HpkArchiver
{
    public class FileEntryExtended : FileEntry
    {
        public string FullName { get; set; }
        public bool Compress { get; set; }
        public int Length { get; set; }
        public uint Offset { get; set; }
        public long FileTime { get; set; }
        public List<int> DirIndices { get; set; }
        public List<int> FileIndices { get; set; }

        public FileEntryExtended()
        {
            Compress = false;
            DirIndices = new List<Int32>();
            FileIndices = new List<Int32>();
        }
    }
}
