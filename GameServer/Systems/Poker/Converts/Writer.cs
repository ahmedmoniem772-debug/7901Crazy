using System;
using System.Text;

public class Writer
{
    public static void Write(ushort arg, int offset, byte[] buffer)
    {
        if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + 2)
        {
            buffer[offset] = (byte)arg;
            buffer[offset + 1] = (byte)(arg >> 8);
        }
    }

    public static void Write(string arg, int offset, byte[] buffer)
    {
        if (buffer != null && offset <= buffer.Length - 1)
        {
            byte[] bytes = Encoding.Default.GetBytes(arg);
            if (buffer.Length >= offset + arg.Length)
            {
                Array.Copy(bytes, 0, buffer, offset, arg.Length);
            }
        }
    }
}