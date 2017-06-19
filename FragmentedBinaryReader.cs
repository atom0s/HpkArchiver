using System;
using System.Collections.Generic;
using System.IO;

namespace HpkArchiver
{
    public class Fragment
    {
        public long Offset { get; set; }
        public int Length { get; set; }

        public Fragment()
        {
        }
        public Fragment(long offset)
        {
            Offset = offset;
        }
        public Fragment(long offset, int length)
        {
            Offset = offset;
            Length = length;
        }
        public Fragment(byte[] data)
        {
            if (data.Length == 8)
            {
                Offset = BitConverter.ToUInt32(data, 0);
                Length = BitConverter.ToInt32(data, 4);
            }
            else if (data.Length == 12)
            {
                Offset = BitConverter.ToInt64(data, 0);
                Length = BitConverter.ToInt32(data, 8);
            }
        }

        public bool IsNull
        {
            get
            {
                if (Offset == 0 && Length == 0)
                    return true;
                else
                    return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Fragment))
            {
                Fragment fragment = (Fragment)obj;
                if (fragment.Offset == Offset && fragment.Length == Length)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return Offset.GetHashCode() * 17 + Length.GetHashCode();
        }
        public override string ToString()
        {
            return Offset + "," + Length;
        }
    }
    public class FragmentedBinaryReader
    {
        public List<Fragment> Fragments { get; set; }
        public BinaryReader Reader { get; set; }

        public FragmentedBinaryReader(ref BinaryReader br)
        {
            Fragments = new List<Fragment>();
            Reader = br;
        }
        public FragmentedBinaryReader(ref BinaryReader br, List<Fragment> fragments)
        {
            Reader = br;
            Fragments = fragments;
        }

        private int _CurrentFragment = 0;
        private long _CurrentFragmentPosition = 0;
        private int _CurrentFragmentLength = 0;
        public byte[] ReadFragmented(int length)
        {
            if (_CurrentFragmentLength == 0) // This should only get hit once when the method is first called.  It initializes the above variables.
            {
                _CurrentFragmentPosition = Fragments[_CurrentFragment].Offset;
                _CurrentFragmentLength = Fragments[_CurrentFragmentLength].Length;
            }

            byte[] buffer = new byte[length];
            int position = 0;
            int required = buffer.Length;
            while (position < buffer.Length) // Loop until buffer is full.
            {
                Reader.BaseStream.Seek(_CurrentFragmentPosition, SeekOrigin.Begin); // Seek on every iteration in case the current fragment changed.
                if (required < _CurrentFragmentLength) // We have enough data in the current fragment to fill the buffer.
                {
                    Reader.Read(buffer, position, required);
                    _CurrentFragmentPosition = Reader.BaseStream.Position;
                    _CurrentFragmentLength -= required;
                    position += required;
                    required -= required;
                }
                else // We need more data from the next fragment.
                {
                    Reader.Read(buffer, position, _CurrentFragmentLength); // Finish current fragment.
                    position += _CurrentFragmentLength;
                    required -= _CurrentFragmentLength;
                    _CurrentFragment++;
                    if (_CurrentFragment == Fragments.Count) // No more fragments to read, return what you have.
                    {
                        byte[] output = new byte[position];
                        Array.Copy(buffer, 0, output, 0, output.Length);
                        return output;
                    }
                    _CurrentFragmentPosition = Fragments[_CurrentFragment].Offset;
                    _CurrentFragmentLength = Fragments[_CurrentFragment].Length;
                }
            }
            return buffer;
        }
    }
}
