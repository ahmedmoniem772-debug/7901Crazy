using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public static class MsgShowHandKick
    {
        public static unsafe byte[] CreateShowHandKick(Poker.Kick kick, byte Type, uint value, uint Time)
        {
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgShowHandKick);
            stream.Write((byte)Type);//4
            stream.Write((uint)kick.ServerID_Starter);//5
            stream.Write((uint)kick.Starter);//9
            stream.Write((uint)kick.ServerID_Target);//13
            stream.Write((uint)kick.Target);//17
            stream.Write((uint)value);//21
            stream.Write((uint)Time);//25
            return GStream.TQServer();
        }
        public static unsafe void GetShowHandKick(byte[] Packet, out byte Type, out uint Target, out byte Accept)
        {
            MemoryStream MS = new MemoryStream(Packet);
            BinaryReader stream = new BinaryReader(MS);
            stream.ReadUInt16();
            stream.ReadUInt16();

            Type = stream.ReadByte();//4
            stream.ReadUInt32();//5 ServerID_Starter
            stream.ReadUInt32();//9 Starter
            stream.ReadUInt32();//13 ServerID_Target
            Target = stream.ReadUInt32();//17
            stream.ReadUInt32();//21
            stream.ReadUInt32();//25
            Accept = stream.ReadByte();//29
        }


    }
}
