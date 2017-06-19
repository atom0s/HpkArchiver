using System;
using System.Text;

namespace HpkArchiver
{
    public class Header
    {
        public const int LENGTH = 36;

        public string IdentifyingChars;
        public uint DataOffset;
        public int FragmentsPerFile; // 1
        public int Unknown2; // -1
        public uint FragmentsResidualOffset;
        public int FragmentsResidualCount;
        public int Unknown5; // 1
        public uint FragmentedFileSystemOffset;
        public int FragmentedFileSystemLength;

        public Header()
        {
            IdentifyingChars = "BPUL";
        }
        public Header(byte[] data)
        {
            IdentifyingChars = Encoding.ASCII.GetString(data, 0, 4);
            DataOffset = BitConverter.ToUInt32(data, 4);
            FragmentsPerFile = BitConverter.ToInt32(data, 8);
            Unknown2 = BitConverter.ToInt32(data, 12);
            FragmentsResidualOffset = BitConverter.ToUInt32(data, 16);
            FragmentsResidualCount = BitConverter.ToInt32(data, 20);
            Unknown5 = BitConverter.ToInt32(data, 24);
            FragmentedFileSystemOffset = BitConverter.ToUInt32(data, 28);
            FragmentedFileSystemLength = BitConverter.ToInt32(data, 32);
        }

        public byte[] GetBytes()
        {
            byte[] output = new byte[LENGTH];
            Encoding.ASCII.GetBytes(IdentifyingChars).CopyTo(output, 0);
            BitConverter.GetBytes(DataOffset).CopyTo(output, 4);
            BitConverter.GetBytes(FragmentsPerFile).CopyTo(output, 8);
            BitConverter.GetBytes(Unknown2).CopyTo(output, 12);
            BitConverter.GetBytes(FragmentsResidualOffset).CopyTo(output, 16);
            BitConverter.GetBytes(FragmentsResidualCount).CopyTo(output, 20);
            BitConverter.GetBytes(Unknown5).CopyTo(output, 24);
            BitConverter.GetBytes(FragmentedFileSystemOffset).CopyTo(output, 28);
            BitConverter.GetBytes(FragmentedFileSystemLength).CopyTo(output, 32);
            return output;
        }

        public override string ToString()
        {
            return FragmentsPerFile.ToString();
        }
    }
}
