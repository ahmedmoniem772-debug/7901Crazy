using System.Collections.Generic;
using System.Linq;
using ConquerOnline.Role;
using ConquerOnline.Role.Instance;
using ProtoBuf;
using ConquerOnline.Role.Instance.Poker;

namespace ConquerOnline.Game.MsgServer
{

    [ProtoContract]
    public class MsgShowHandLayCard
    {
        [ProtoContract]
        public class Player
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong serverid;

            [ProtoMember(2, IsRequired = true)]
            public ulong UID;

            [ProtoMember(3, IsRequired = true)]
            public Card[] Cards;
        }
        [ProtoContract]
        public class Card
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong Value;

            [ProtoMember(2, IsRequired = true)]
            public ulong Type;
        }

        [ProtoMember(1, IsRequired = true)]
        public ulong Type;

        [ProtoMember(2, IsRequired = true)]
        public Player[] PlayerList;

        public ServerSockets.Packet ToArray()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(this);
                stream.Finalize(GamePackets.MsgShowHandLayCard);
                return stream;
            }
        }
        public static implicit operator ServerSockets.Packet(MsgShowHandLayCard obj)
        {
            return obj.ToArray();
        }



        public static MsgShowHandLayCard CreateShowHandLayCard(PokerTable table)
        {
            if (table.TableType != Flags.TableType.TexasHoldem)
            {
                MsgShowHandLayCard Card1 = new MsgShowHandLayCard();
                Card1.Type = 1uL;
                List<ConquerOnline.Role.Instance.Poker.Player> list = table.Players.Values.Where((ConquerOnline.Role.Instance.Poker.Player p) => p.IsPlaying && !p.Fold).ToList();
                Card1.PlayerList = new Player[list.Count];
                for (int i = 0; i < Card1.PlayerList.Length; i++)
                {
                    Card1.PlayerList[i] = new Player();
                    Card1.PlayerList[i].UID = list[i].Uid;
                    Card1.PlayerList[i].serverid = list[i].ServerID;
                    Card1.PlayerList[i].Cards = new Card[1];
                    Card1.PlayerList[i].Cards[0] = new Card();
                    Card1.PlayerList[i].Cards[0].Value = list[i].Pocket[0].Value;
                    Card1.PlayerList[i].Cards[0].Type = (ulong)list[i].Pocket[0].Type;
                }
                return Card1;
            }
            MsgShowHandLayCard Card2 = new MsgShowHandLayCard();
            Card2.Type = 1uL;
            List<ConquerOnline.Role.Instance.Poker.Player> list2 = table.Players.Values.Where((ConquerOnline.Role.Instance.Poker.Player p) => p.IsPlaying && !p.Fold).ToList();
            Card2.PlayerList = new Player[list2.Count];
            for (int j = 0; j < Card2.PlayerList.Length; j++)
            {
                Card2.PlayerList[j] = new Player();
                Card2.PlayerList[j].UID = list2[j].Uid;
                Card2.PlayerList[j].serverid = list2[j].ServerID;
                Card2.PlayerList[j].Cards = new Card[list2[j].Pocket.Length];
                for (int k = 0; k < Card2.PlayerList[j].Cards.Length; k++)
                {
                    Card2.PlayerList[j].Cards[k] = new Card();
                    Card2.PlayerList[j].Cards[k].Value = list2[j].Pocket[k].Value;
                    Card2.PlayerList[j].Cards[k].Type = (ulong)list2[j].Pocket[k].Type;
                }
            }
            return Card2;
        }



    }
}
