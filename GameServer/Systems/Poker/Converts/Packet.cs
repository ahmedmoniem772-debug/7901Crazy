using ProtoBuf;
using System;
using System.IO;

namespace Converts
{
    public static class Packet
    {
        public static byte[] ConvertOffset(this MemoryStream Stream)
        {
            byte[] array = Stream.ToArray();
            byte[] array2 = new byte[array.Length + 8];
            Buffer.BlockCopy(array, 0, array2, 0, array.Length);
            Writer.Write((ushort)array.Length, 0, array2);
            Writer.Write("TQServer", array2.Length - 8, array2);
            return array2;
        }
        public static byte[] ConvertProto(this object Proto, ushort PacketID)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize<object>((Stream)(object)memoryStream, Proto);
                byte[] array = memoryStream.ToArray();
                byte[] array2 = new byte[12 + array.Length];
                Buffer.BlockCopy(array, 0, array2, 4, array.Length);
                Writer.Write((ushort)(array2.Length - 8), 0, array2);
                Writer.Write(PacketID, 2, array2);
                Writer.Write("TQServer", array2.Length - 8, array2);
                return array2;
            }
        }
    }
}