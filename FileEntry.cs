using System;
using System.Text;

namespace HpkArchiver
{
    public class FileEntry
    {
        public int FragmentIndex {get; set;}
        public FragmentType Type { get; set; }
        public ushort NameLength { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public int GetSize()
        {
            return 10 + Name.Length;
        }
        public byte[] GetBytes()
        {
            byte[] output = new byte[GetSize()];
            BitConverter.GetBytes(FragmentIndex).CopyTo(output, 0);
            BitConverter.GetBytes((uint)Type).CopyTo(output, 4);
            BitConverter.GetBytes(NameLength).CopyTo(output, 8);
            Encoding.ASCII.GetBytes(Name).CopyTo(output, 10);
            return output;
        }
    }
}
