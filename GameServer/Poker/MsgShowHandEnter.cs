using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public static class MsgShowHandEnter
    {
        public static unsafe byte[] CreateShowHandEnter(byte Action, Poker.Player player)
        {
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgShowHandEnter);

            
            stream.Write(Action);
            stream.Write((ushort)player.PlayerType);//5
            stream.Write((ushort)player.Seat);//7
            stream.Write((uint)player.Table.Number);//9
            stream.Write(player.ServerID);//13
            stream.Write(player.RealUID);//17
            stream.Write((ulong)0);//21//TotalPot
            stream.Write((uint)player.Table.Id);//29
            stream.Write((uint)0);
            //Server 6
            // 2
            //ShowHand 3
            return GStream.TQServer();
        }
        public static unsafe void GetShowHandEnter(byte[] Packet, out byte Action, out General.PlayerType PlayerType, out ushort Seat, out uint TableNumber, out uint PlayerUid, out General.TableType TableType)
        {
            MemoryStream MS = new MemoryStream(Packet);
            BinaryReader stream = new BinaryReader(MS);
            stream.ReadUInt16();
            stream.ReadUInt16();

            Action = stream.ReadByte();//4
            PlayerType = (General.PlayerType)stream.ReadUInt16();//5
            Seat = stream.ReadUInt16();//7
            TableNumber = stream.ReadUInt32();//9
            stream.ReadUInt32();//SreverID//13
            PlayerUid = stream.ReadUInt32();//17
            stream.ReadUInt64();//21
            TableType = (General.TableType)stream.ReadByte();//29
        }
    }
}
