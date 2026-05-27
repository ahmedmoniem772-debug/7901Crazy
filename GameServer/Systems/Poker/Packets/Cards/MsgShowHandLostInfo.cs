using System.Collections.Generic;
using System.Linq;
using ConquerOnline.Role.Instance;
using ConquerOnline;
using ProtoBuf;
using ConquerOnline.Role.Instance.Poker;

namespace ConquerOnline.Game.MsgServer
{
    [ProtoContract]
    public class MsgShowHandLostInfo
    {
        [ProtoContract]
        public class Card
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong Value;

            [ProtoMember(2, IsRequired = true)]
            public ulong Type;

         
        }

        [ProtoContract]
        public class PlayerCard
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong uk;

            [ProtoMember(2, IsRequired = true)]
            public ulong Value;

            [ProtoMember(3, IsRequired = true)]
            public ulong Type;

          
        }

        [ProtoContract]
        public class Player
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong serverid;

            [ProtoMember(2, IsRequired = true)]
            public ulong UID;

            [ProtoMember(3, IsRequired = true)]
            public PlayerCard[] Cards;

          

          
        }

        [ProtoMember(1, IsRequired = true)]
        public ulong Type;

        [ProtoMember(2, IsRequired = true)]
        public ulong uk1;

        [ProtoMember(3, IsRequired = true)]
        public ulong uk2;

        [ProtoMember(4, IsRequired = true)]
        public ulong uk3;

        [ProtoMember(5, IsRequired = true)]
        public ulong uk4;

        [ProtoMember(6, IsRequired = true)]
        public ulong TotalPot;

        [ProtoMember(7, IsRequired = true)]
        public ulong Serverid_CurrentPlayer;

        [ProtoMember(8, IsRequired = true)]
        public ulong id_CurrentPlayer;

        [ProtoMember(9, IsRequired = true)]
        public ulong Serverid_Dealer;

        [ProtoMember(10, IsRequired = true)]
        public ulong id_Dealer;

        [ProtoMember(11, IsRequired = true)]
        public ulong Serverid_BigBlind;

        [ProtoMember(12, IsRequired = true)]
        public ulong id_BigBlind;

        [ProtoMember(13, IsRequired = true)]
        public ulong Serverid_SmallBlind;

        [ProtoMember(14, IsRequired = true)]
        public ulong id_SmallBlind;

        [ProtoMember(15, IsRequired = true)]
        public Card[] TableCards;

        [ProtoMember(16, IsRequired = true)]
        public Player[] PlayerList;
        public ServerSockets.Packet ToArray()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(this);
                stream.Finalize(GamePackets.MsgShowHandLostInfo);
                return stream;
            }
        }
        public static implicit operator ServerSockets.Packet(MsgShowHandLostInfo obj)
        {
            return obj.ToArray();
        }
        public MsgShowHandLostInfo(PokerTable Table)
        {

            Type = (ulong)1;
            TotalPot = Table.TotalPot;
            Role.Instance.Poker.Player player;
            if (Table.Players.TryGetValue(Table.CurrentPlayer, out player))
            {
                this.id_CurrentPlayer = (ulong)player.Uid;
                this.Serverid_CurrentPlayer = (ulong)player.ServerID;
            }
            if (Table.Players.TryGetValue(Table.Dealer, out player))
            {
                this.id_Dealer = (ulong)player.Uid;
                this.Serverid_Dealer = (ulong)player.ServerID;
            }
            if (Table.Players.TryGetValue(Table.BigBlind, out player))
            {
                this.id_BigBlind = (ulong)player.Uid;
                this.Serverid_BigBlind = (ulong)player.ServerID;
            }
            if (Table.Players.TryGetValue(Table.SmallBlind, out player))
            {
                this.id_SmallBlind = (ulong)player.Uid;
                this.Serverid_SmallBlind = (ulong)player.ServerID;
            }
            List<PokerCard> list = Table.Board.Where((PokerCard p) => p != null).ToList();
            TableCards = new Card[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                TableCards[i] = new Card();
                TableCards[i].Value = list[i].Value;
                TableCards[i].Type = (ulong)list[i].Type;
            }
            List<Role.Instance.Poker.Player> list2 = Table.Players.Values.Where((Role.Instance.Poker.Player px) => px.IsPlaying && !px.Fold).ToList();
            PlayerList = new Player[list2.Count];
            for (int j = 0; j < list2.Count; j++)
            {
                PlayerList[j] = new Player();
                PlayerList[j].serverid = list2[j].ServerID;
                PlayerList[j].UID = list2[j].Uid;
                PlayerList[j].Cards = new PlayerCard[list2[j].Pocket.Length];
                for (int k = 0; k < list2[j].Pocket.Length; k++)
                {
                    PlayerList[j].Cards[k] = new PlayerCard();
                    PlayerList[j].Cards[k].Value = (ulong)13;
                    PlayerList[j].Cards[k].Type = (ulong)4;
                }
            }
        }
     

       
    }
}
