using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public static class MsgShowHandTable
    {
        public static unsafe byte[] CreateShowHandTable(PokerTable table)
        {
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgTexasNpcInfo);
            stream.Write(table.Id);//4
            stream.Write((ulong)0);//8
            stream.Write(table.X);//16
            stream.Write(table.Y);//18
            stream.Write(table.Mesh);//20
            stream.Write((ushort)0);//24
            stream.Write(table.Number);//26
            stream.Write(table.UnLimited ? 1 : 0);//30
            stream.Write(table.IsCPs ? 1 : 0);//34
            stream.Write(table.MinBet);//38
            stream.Write((byte)table.State);//42
            stream.Write((ulong)table.TotalPot);//43
            stream.Write((ulong)0);//51
            stream.Write((byte)table.Players.Count);//59
            stream.Write(table.OMAHA);//60
            foreach (var p in table.Players.Values)
            {
                stream.Write(p.ServerID);
                stream.Write(p.Uid);
                stream.Write((byte)p.Seat);
                stream.Write((byte)1);
            }
            return GStream.TQServer();
        }
    }
}
