using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public static class MsgTexasInteractive
    {
        public static unsafe byte[] CreateTexasInteractive(General.TableInteractiveType InteractiveType, Poker.Player player)
        {
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgTexasInteractiveo);
            stream.Write((uint)InteractiveType);//4
            stream.Write((uint)player.Table.Id);//8
            stream.Write((uint)player.RealUID);//12
            stream.Write((uint)player.ServerID);//16
            stream.Write((uint)player.Seat);//20
            stream.Write((uint)0);//20
            return GStream.TQServer();
        }
        public static unsafe void GetTexasInteractive(byte[] Packet, out General.TableInteractiveType InteractiveType, out uint TableId, out uint PlayerUid, out byte Seat, out uint TableType)
        {
            MemoryStream MS = new MemoryStream(Packet);
            BinaryReader stream = new BinaryReader(MS);
            stream.ReadUInt16();
            stream.ReadUInt16();
            InteractiveType = (General.TableInteractiveType)stream.ReadUInt32();//4
            TableId = stream.ReadUInt32();//8
            PlayerUid = stream.ReadUInt32();//12
            stream.ReadUInt32();//ServerID
            Seat = (byte)stream.ReadUInt32();//20
            TableType = stream.ReadUInt32();//16

        }
    }
}
