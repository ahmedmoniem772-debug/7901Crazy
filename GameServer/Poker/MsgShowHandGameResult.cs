using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public class CMsgShowHandGameResult
    {
        public static byte[] Create(PokerTable Table)
        {
            var Players = Table.Players.Values.Where(p => p.IsPlaying).ToList();
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgShowHandGameResult);
            stream.Write((ushort)10);//4
            stream.Write((ushort)Players.Count);//6
            foreach (var Player in Players.OrderBy(p => p.Lose))
            {
                if (Player.CurrentMoney < Table.MinBet * 10)
                    stream.Write((byte)1);
                else
                    stream.Write((byte)0);
                if (Player.Fold)
                    stream.Write((ushort)3);
                else if (Player.Lose < 0)
                    stream.Write((ushort)255);
                else if (Player.Lose > 0)
                    stream.Write((ushort)0);
                else
                    stream.Write((ushort)0);
                stream.Write(Player.ServerID);
                stream.Write(Player.RealUID);
                stream.Write(Player.Lose);
                stream.Write(0L);
                stream.Write(0U);
            }
            return GStream.TQServer();
        }
    }
}
