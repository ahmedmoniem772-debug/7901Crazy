using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public static class MsgShowHandCallAction
    {
        public static unsafe byte[] CreateShowHandCallAction(ushort Action, Poker.Player player)
        {
        
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgShowHandCallAction);
            stream.Write((ushort)0);
            stream.Write(Action);
            stream.Write(player.RoundPot);
            stream.Write(player.TotalPot);
            stream.Write((uint)player.ServerID);
            stream.Write((uint)player.RealUID);
            return GStream.TQServer();
        }
        public static unsafe void GetShowHandCallAction(byte[] Packet, out ushort Action, out ulong RoundPot, out ulong TotalPot, out uint UID)
        {

            MemoryStream MS = new MemoryStream(Packet);
            BinaryReader stream = new BinaryReader(MS);
            stream.ReadUInt16();
            stream.ReadUInt16();

            stream.ReadUInt16();
            Action = stream.ReadUInt16();
            RoundPot = stream.ReadUInt64();
            TotalPot = stream.ReadUInt64();
            stream.ReadUInt32();//ServerID
            UID = stream.ReadUInt32();
        }
    }
}
