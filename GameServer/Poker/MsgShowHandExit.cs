using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public static class MsgShowHandExit
    {
        public static unsafe byte[] CreateShowHandExit(byte Action, Poker.Player player)
        {
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgShowHandExit);
            stream.Write((uint)Action);
            stream.Write((uint)player.Table.Number);//8
            stream.Write((uint)player.RealUID);//12
            stream.Write((uint)player.ServerID);//16
            return GStream.TQServer();
        }
        public static unsafe void GetShowHandExit(byte[] Packet, out uint Action, out uint TableNumber, out uint PlayerUid)
        {
            MemoryStream MS = new MemoryStream(Packet);
            BinaryReader stream = new BinaryReader(MS);
            stream.ReadUInt16();
            stream.ReadUInt16();

            Action = stream.ReadUInt32();
            TableNumber = stream.ReadUInt32();
            PlayerUid = stream.ReadUInt32();
        }


    }
}
