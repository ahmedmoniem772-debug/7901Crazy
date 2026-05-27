using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poker
{
    [ProtoContract]
    public class CMsgShowHandLayCard
    {
        [ProtoMember(1, IsRequired = true)]
        public ulong Type = 0;
        [ProtoMember(2, IsRequired = true)]
        public Player[] PlayerList;
        [ProtoContract]
        public class Player
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong serverid = 0;
            [ProtoMember(2, IsRequired = true)]
            public ulong UID = 0;
            [ProtoMember(3, IsRequired = true)]
            public Card[] Cards;
        }
        [ProtoContract]
        public class Card
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong Value = 0;
            [ProtoMember(2, IsRequired = true)]
            public ulong Type = 0;
        }
        public byte[] ToArray()
        {
            return this.TQServer(PacketsID.CMsgShowHandLayCard);
        }
        public static implicit operator byte[](CMsgShowHandLayCard Array)
        {
            return Array.ToArray();
        }
        public static byte[] Create(PokerTable table)
        {
            if (table.TableType == General.TableType.TexasHoldem)
            {
                CMsgShowHandLayCard ShowHandLayCard = new CMsgShowHandLayCard();
                ShowHandLayCard.Type = 1;
                var list = table.Players.Values.Where(p => p.IsPlaying && p.Fold == false).ToList();
                ShowHandLayCard.PlayerList = new Player[list.Count];
                for (int i = 0; i < ShowHandLayCard.PlayerList.Length; i++)
                {
                    ShowHandLayCard.PlayerList[i] = new Player();
                    ShowHandLayCard.PlayerList[i].UID = list[i].RealUID;
                    ShowHandLayCard.PlayerList[i].serverid = list[i].ServerID;
                    ShowHandLayCard.PlayerList[i].Cards = new Card[list[i].Pocket.Length];
                    for (int x = 0; x < ShowHandLayCard.PlayerList[i].Cards.Length; x++)
                    {
                        ShowHandLayCard.PlayerList[i].Cards[x] = new Card();
                        ShowHandLayCard.PlayerList[i].Cards[x].Value = (ulong)list[i].Pocket[x].Value;
                        ShowHandLayCard.PlayerList[i].Cards[x].Type = (ulong)list[i].Pocket[x].Type;
                    }
                }
                return ShowHandLayCard;
            }
            else
            {
                CMsgShowHandLayCard ShowHandLayCard = new CMsgShowHandLayCard();
                ShowHandLayCard.Type = 1;
                var list = table.Players.Values.Where(p => p.IsPlaying && p.Fold == false).ToList();
                ShowHandLayCard.PlayerList = new Player[list.Count];
                for (int i = 0; i < ShowHandLayCard.PlayerList.Length; i++)
                {
                    ShowHandLayCard.PlayerList[i] = new Player();
                    ShowHandLayCard.PlayerList[i].UID = list[i].RealUID;
                    ShowHandLayCard.PlayerList[i].serverid = list[i].ServerID;
                    ShowHandLayCard.PlayerList[i].Cards = new Card[1];
                    ShowHandLayCard.PlayerList[i].Cards[0] = new Card();
                    ShowHandLayCard.PlayerList[i].Cards[0].Value = (ulong)list[i].Pocket[0].Value;
                    ShowHandLayCard.PlayerList[i].Cards[0].Type = (ulong)list[i].Pocket[0].Type;
                }
                return ShowHandLayCard;
            }
        }
    }
}
