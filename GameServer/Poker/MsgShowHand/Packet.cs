using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public static class Packet
    {
        public static unsafe byte[] TQServer(this MemoryStream Stream)
        {
            byte[] Array = Stream.ToArray();
            byte[] buffer = new byte[Array.Length + 8];
            Buffer.BlockCopy(Array, 0, buffer, 0, Array.Length);
            Writer.Write((ushort)Array.Length, 0, buffer);
            Writer.Write("TQServer", buffer.Length - 8, buffer);
            return buffer;
        }
        public static unsafe byte[] TQServer(this object Proto, ushort PacketID)
        {
            byte[] ProtoBuffer;
            using (var MemoryStream = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(MemoryStream, Proto);
                ProtoBuffer = MemoryStream.ToArray();
                byte[] buffer = new byte[12 + ProtoBuffer.Length];
                System.Buffer.BlockCopy(ProtoBuffer, 0, buffer, 4, ProtoBuffer.Length);
                Writer.Write((ushort)(buffer.Length - 8), 0, buffer);
                Writer.Write(PacketID, 2, buffer);
                Writer.Write("TQServer", buffer.Length - 8, buffer);
                return buffer;
            }
        }
    }
}
