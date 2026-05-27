using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poker
{
    [ProtoContract]
    public class CMsgShowHandLostInfo
    {
        public CMsgShowHandLostInfo(PokerTable Table)
        {
            Type = 1;
            TotalPot = Table.TotalPot;
            Serverid_CurrentPlayer = Table.Players[Table.CurrentPlayer].ServerID;
            id_CurrentPlayer = Table.Players[Table.CurrentPlayer].RealUID;


            Serverid_Dealer = Table.Players[Table.Dealer].ServerID;
            id_Dealer = Table.Players[Table.Dealer].RealUID;

            Serverid_BigBlind = Table.Players[Table.BigBlind].ServerID;
            id_BigBlind = Table.Players[Table.BigBlind].RealUID;

            Serverid_SmallBlind = Table.Players[Table.SmallBlind].ServerID;
            id_SmallBlind = Table.Players[Table.SmallBlind].RealUID;


            var Board = Table.Board.Where(p => p != null).ToList();
            TableCards = new Card[Board.Count];
            for (int i = 0; i < Board.Count; i++)
            {
                TableCards[i] = new Card();
                TableCards[i].Value = Board[i].Value;
                TableCards[i].Type = (byte)Board[i].Type;
            }

            var players = Table.Players.Values.Where(px => px.IsPlaying && px.Fold == false).ToList();
            PlayerList = new Player[players.Count];
            for (int i = 0; i < players.Count; i++)
            {
                PlayerList[i] = new Player();
                PlayerList[i].serverid = players[i].ServerID;
                PlayerList[i].UID = players[i].RealUID;
                PlayerList[i].Cards = new PlayerCard[players[i].Pocket.Length];
                for (int x = 0; x < players[i].Pocket.Length; x++)
                {
                    PlayerList[i].Cards[x] = new PlayerCard();
                    PlayerList[i].Cards[x].Value = 13;
                    PlayerList[i].Cards[x].Type = 4;
                }
            }
        }
        [ProtoMember(1, IsRequired = true)]
        public ulong Type = 0;


        [ProtoMember(2, IsRequired = true)]
        public ulong uk1 = 0;
        [ProtoMember(3, IsRequired = true)]
        public ulong uk2 = 0;
        [ProtoMember(4, IsRequired = true)]
        public ulong uk3 = 0;
        [ProtoMember(5, IsRequired = true)]
        public ulong uk4 = 0;


        [ProtoMember(6, IsRequired = true)]
        public ulong TotalPot = 0;


        [ProtoMember(7, IsRequired = true)]
        public ulong Serverid_CurrentPlayer = 0;
        [ProtoMember(8, IsRequired = true)]
        public ulong id_CurrentPlayer = 0;


        [ProtoMember(9, IsRequired = true)]
        public ulong Serverid_Dealer = 0;
        [ProtoMember(10, IsRequired = true)]
        public ulong id_Dealer = 0;


        [ProtoMember(11, IsRequired = true)]
        public ulong Serverid_BigBlind = 0;
        [ProtoMember(12, IsRequired = true)]
        public ulong id_BigBlind = 0;

        [ProtoMember(13, IsRequired = true)]
        public ulong Serverid_SmallBlind = 0;
        [ProtoMember(14, IsRequired = true)]
        public ulong id_SmallBlind = 0;

        [ProtoMember(15, IsRequired = true)]
        public Card[] TableCards;

        [ProtoMember(16, IsRequired = true)]
        public Player[] PlayerList;

        [ProtoContract]
        public class Card
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong Value = 0;
            [ProtoMember(2, IsRequired = true)]
            public ulong Type = 0;
        }
        [ProtoContract]
        public class PlayerCard
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong uk = 0;
            [ProtoMember(2, IsRequired = true)]
            public ulong Value = 0;
            [ProtoMember(3, IsRequired = true)]
            public ulong Type = 0;
        }
        [ProtoContract]
        public class Player
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong serverid = 0;
            [ProtoMember(2, IsRequired = true)]
            public ulong UID = 0;
            [ProtoMember(3, IsRequired = true)]
            public PlayerCard[] Cards;
        }
        public byte[] ToArray()
        {
            return this.TQServer(PacketsID.CMsgShowHandLostInfo);
        }
        public static implicit operator byte[](CMsgShowHandLostInfo Array)
        {
            return Array.ToArray();
        }
    }
}
