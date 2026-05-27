using System;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgShowHandTable
    {
        public static unsafe ServerSockets.Packet CreateShowHandTable(this ServerSockets.Packet stream, Role.Instance.PokerTable table)
        {


            stream.Write((ushort)table.Id);//4
            stream.Write((ushort)1);//6
            stream.Write((ulong)0);//8
            stream.Write(table.X);//16
            stream.Write(table.Y);//18
            stream.Write((ushort)table.Mesh);//20
            stream.Write((ushort)110);//22
            stream.Write((ushort)0);//24
            stream.Write(table.Number);//26
            stream.Write(table.UnLimited ? 1 : 0);//30
            stream.Write(table.IsCPs ? 1 : 0);//34
            stream.Write(table.MinBet);//38
            stream.Write((byte)table.State);//42
            stream.Write(table.TotalPot);//43
            stream.Write((ulong)0);//51
            stream.Write((byte)table.Players.Count);//59
            stream.Write(table.OMAHA);//60
            foreach (var player in table.Players.Values)
            {
                stream.Write(player.ServerID);
                stream.Write(player.Uid);
                stream.Write(player.Seat);
                stream.Write((byte)1);
            }


            stream.Finalize(GamePackets.MsgTexasNpcInfo);
            return stream;
        }
    }

}