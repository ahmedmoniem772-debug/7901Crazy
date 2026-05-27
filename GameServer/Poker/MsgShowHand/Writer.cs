using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker
{
    public class Writer
    {
        public static void Write(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
#endif
                }
            }
        }
        public static void Write(string arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            byte[] argEncoded = Encoding.Default.GetBytes(arg);
            if (buffer.Length >= offset + arg.Length)
                Array.Copy(argEncoded, 0, buffer, offset, arg.Length);
        }
    }
}
