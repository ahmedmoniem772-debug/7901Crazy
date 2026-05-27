using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Poker
{
    [ProtoContract]
    public class CMsgShowHandDealtCard
    {
        public enum HandDealtCard : ushort
        {
            TwoCardDraw = 0,
            ThreeCardDraw = 1,
            FourCardDraw = 2,
            FiveCardDraw = 3,
            OneCardDraw = 4,
            CardDown = 5,
            CardUp = 6
        }
        [ProtoMember(1, IsRequired = true)]
        public ulong Counter = 0;
        [ProtoMember(2, IsRequired = true)]
        public HandDealtCard Type = 0;
        [ProtoMember(3, IsRequired = true)]
        public ulong ServerID_Dealer = 0;
        [ProtoMember(4, IsRequired = true)]
        public ulong Dealer = 0;
        [ProtoMember(5, IsRequired = true)]
        public ulong ServerID_SmallBlind = 0;
        [ProtoMember(6, IsRequired = true)]
        public ulong SmallBlind = 0;
        [ProtoMember(7, IsRequired = true)]
        public ulong ServerID_BigBlind = 0;
        [ProtoMember(8, IsRequired = true)]
        public ulong BigBlind = 0;
        [ProtoMember(9, IsRequired = true)]
        public MyCard[] MyCards;
        [ProtoMember(10, IsRequired = true)]
        public Player[] PlayerList;
        [ProtoContract]
        public class MyCard
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong Value = 0;
            [ProtoMember(2, IsRequired = true)]
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
            public MyCard[] MyCards;
        }
        public byte[] ToArray()
        {
            return this.TQServer(PacketsID.CMsgShowHandDealtCard);
        }
        public static implicit operator byte[](CMsgShowHandDealtCard Array)
        {
            return Array.ToArray();
        }
        public static byte[] Create(PokerTable table, ushort counter, HandDealtCard Type, uint uid = 0)
        {
            switch (Type)
            {
                #region CardDown
                case HandDealtCard.CardDown:
                    {
                        CMsgShowHandDealtCard ShowHandDealtCard = new CMsgShowHandDealtCard();
                        ShowHandDealtCard.Type = Type;
                        ShowHandDealtCard.Counter = counter;
                        Poker.Player _Player;
                        if (table.Players.TryGetValue(table.Dealer, out _Player))
                        {
                            ShowHandDealtCard.ServerID_Dealer = _Player.ServerID;
                            ShowHandDealtCard.Dealer = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_SmallBlind = _Player.ServerID;
                            ShowHandDealtCard.SmallBlind = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_BigBlind = _Player.ServerID;
                            ShowHandDealtCard.BigBlind = _Player.RealUID;
                        }
                        ShowHandDealtCard.MyCards = new MyCard[1];
                        ShowHandDealtCard.MyCards[0] = new MyCard();
                        ShowHandDealtCard.MyCards[0].Value = (ulong)table.Players[uid].Pocket[0].Value;
                        ShowHandDealtCard.MyCards[0].Type = (ulong)table.Players[uid].Pocket[0].Type;
                        int i = 0;
                        ShowHandDealtCard.PlayerList = new Player[table.Players.Values.Where(p => p.IsPlaying).OrderByDescending(p => p.Seat).ToList().Count];
                        foreach (var x in table.Players.Values.Where(p => p.IsPlaying).OrderByDescending(p => p.Seat))
                        {
                            ShowHandDealtCard.PlayerList[i] = new Player();
                            ShowHandDealtCard.PlayerList[i].UID = x.RealUID;
                            ShowHandDealtCard.PlayerList[i].serverid = x.ServerID;
                            ShowHandDealtCard.PlayerList[i].MyCards = new MyCard[1];
                            ShowHandDealtCard.PlayerList[i].MyCards[0] = new MyCard();
                            ShowHandDealtCard.PlayerList[i].MyCards[0].Value = (ulong)13;
                            ShowHandDealtCard.PlayerList[i].MyCards[0].Type = (ulong)4;
                            i++;
                        }
                        return ShowHandDealtCard;
                    }
                #endregion
                #region CardUp
                case HandDealtCard.CardUp:
                    {
                        CMsgShowHandDealtCard ShowHandDealtCard = new CMsgShowHandDealtCard();
                        ShowHandDealtCard.Type = Type;
                        ShowHandDealtCard.Counter = counter;
                        Poker.Player _Player;
                        if (table.Players.TryGetValue(table.Dealer, out _Player))
                        {
                            ShowHandDealtCard.ServerID_Dealer = _Player.ServerID;
                            ShowHandDealtCard.Dealer = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_SmallBlind = _Player.ServerID;
                            ShowHandDealtCard.SmallBlind = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_BigBlind = _Player.ServerID;
                            ShowHandDealtCard.BigBlind = _Player.RealUID;
                        }
                        ShowHandDealtCard.PlayerList = new Player[(table.Players.Values.Where(p => p.IsPlaying).ToList().Count * counter)];
                        int i_i = 0;
                        for (int i = 0; i < counter; i++)
                        {
                            table.ShowHand += 1;
                            int seat = table.Players[table.Dealer].Seat;
                            uint px = table.Dealer;
                            for (int x = 0; x < table.Players.Values.Where(p => p.IsPlaying).ToList().Count; x++)
                            {
                                ShowHandDealtCard.PlayerList[i_i] = new Player();
                                ShowHandDealtCard.PlayerList[i_i].serverid = table.Players[px].ServerID;
                                ShowHandDealtCard.PlayerList[i_i].UID = table.Players[px].RealUID;
                                ShowHandDealtCard.PlayerList[i_i].MyCards = new MyCard[1];
                                ShowHandDealtCard.PlayerList[i_i].MyCards[0] = new MyCard();
                                ShowHandDealtCard.PlayerList[i_i].MyCards[0].Value = (ulong)table.Players[px].Pocket[table.ShowHand].Value;
                                ShowHandDealtCard.PlayerList[i_i].MyCards[0].Type = (ulong)table.Players[px].Pocket[table.ShowHand].Type;
                                i_i++;
                                px = table.NextSeat((byte)seat);
                                seat = table.Players[px].Seat;
                            }
                        }
                        return ShowHandDealtCard;
                    }
                #endregion
                #region TwoCardDraw
                case HandDealtCard.TwoCardDraw:
                    {
                        CMsgShowHandDealtCard ShowHandDealtCard = new CMsgShowHandDealtCard();
                        ShowHandDealtCard.Type = Type;
                        ShowHandDealtCard.Counter = counter;
                        Poker.Player _Player;
                        if (table.Players.TryGetValue(table.Dealer, out _Player))
                        {
                            ShowHandDealtCard.ServerID_Dealer = _Player.ServerID;
                            ShowHandDealtCard.Dealer = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_SmallBlind = _Player.ServerID;
                            ShowHandDealtCard.SmallBlind = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_BigBlind = _Player.ServerID;
                            ShowHandDealtCard.BigBlind = _Player.RealUID;
                        }
                        if (table.Players.ContainsKey(uid))
                        {
                            if (table.Players[uid].IsPlaying)
                            {
                                ShowHandDealtCard.MyCards = new MyCard[table.Players[uid].Pocket.Length];
                                for (int i = 0; i < ShowHandDealtCard.MyCards.Length; i++)
                                {
                                    ShowHandDealtCard.MyCards[i] = new MyCard();
                                    ShowHandDealtCard.MyCards[i].Value = (ulong)table.Players[uid].Pocket[i].Value;
                                    ShowHandDealtCard.MyCards[i].Type = (ulong)table.Players[uid].Pocket[i].Type;
                                }
                            }
                        }
                        int Seat = table.Players[table.CurrentPlayer].Seat;
                        uint CurrentPlayer = table.CurrentPlayer;
                        ShowHandDealtCard.PlayerList = new Player[table.Players.Values.Where(p => p.IsPlaying).ToList().Count];
                        for (int x = 0; x < table.Players.Values.Where(p => p.IsPlaying).ToList().Count; x++)
                        {
                            ShowHandDealtCard.PlayerList[x] = new Player();
                            ShowHandDealtCard.PlayerList[x].serverid = table.Players[CurrentPlayer].ServerID;
                            ShowHandDealtCard.PlayerList[x].UID = table.Players[CurrentPlayer].RealUID;
                            ShowHandDealtCard.PlayerList[x].MyCards = new MyCard[1];
                            ShowHandDealtCard.PlayerList[x].MyCards[0] = new MyCard();
                            ShowHandDealtCard.PlayerList[x].MyCards[0].Value = (ulong)13;
                            ShowHandDealtCard.PlayerList[x].MyCards[0].Type = (ulong)4;
                            CurrentPlayer = table.NextSeat((byte)Seat);
                            Seat = table.Players[CurrentPlayer].Seat;
                        }
                        return ShowHandDealtCard;
                    }
                #endregion
                #region OneCardDraw
                case HandDealtCard.OneCardDraw:
                    {
                        CMsgShowHandDealtCard ShowHandDealtCard = new CMsgShowHandDealtCard();
                        ShowHandDealtCard.Type = Type;
                        ShowHandDealtCard.Counter = counter;
                        Poker.Player _Player;
                        if (table.Players.TryGetValue(table.Dealer, out _Player))
                        {
                            ShowHandDealtCard.ServerID_Dealer = _Player.ServerID;
                            ShowHandDealtCard.Dealer = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_SmallBlind = _Player.ServerID;
                            ShowHandDealtCard.SmallBlind = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_BigBlind = _Player.ServerID;
                            ShowHandDealtCard.BigBlind = _Player.RealUID;
                        }
                        ShowHandDealtCard.PlayerList = new Player[table.Players.Values.Where(p => p.IsPlaying).OrderByDescending(p => p.Seat).ToArray().Length];
                        int i = 0;
                        foreach (var Player in table.Players.Values.Where(p => p.IsPlaying).OrderByDescending(p => p.Seat))
                        {
                            ShowHandDealtCard.PlayerList[i] = new Player();
                            ShowHandDealtCard.PlayerList[i].serverid = Player.ServerID;
                            ShowHandDealtCard.PlayerList[i].UID = Player.RealUID;
                            ShowHandDealtCard.PlayerList[i].MyCards = new MyCard[1];
                            ShowHandDealtCard.PlayerList[i].MyCards[0] = new MyCard();
                            ShowHandDealtCard.PlayerList[i].MyCards[0].Value = (ulong)Player.Pocket[0].Value;
                            ShowHandDealtCard.PlayerList[i].MyCards[0].Type = (ulong)Player.Pocket[0].Type;
                            i++;
                        }
                        return ShowHandDealtCard;
                    }
                #endregion
                #region ThreeCardDraw|FourCardDraw|FiveCardDraw
                case HandDealtCard.ThreeCardDraw:
                case HandDealtCard.FourCardDraw:
                case HandDealtCard.FiveCardDraw:
                    {
                        CMsgShowHandDealtCard ShowHandDealtCard = new CMsgShowHandDealtCard();
                        ShowHandDealtCard.Type = Type;
                        ShowHandDealtCard.Counter = counter;
                        Poker.Player _Player;
                        if (table.Players.TryGetValue(table.Dealer, out _Player))
                        {
                            ShowHandDealtCard.ServerID_Dealer = _Player.ServerID;
                            ShowHandDealtCard.Dealer = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_SmallBlind = _Player.ServerID;
                            ShowHandDealtCard.SmallBlind = _Player.RealUID;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out _Player))
                        {
                            ShowHandDealtCard.ServerID_BigBlind = _Player.ServerID;
                            ShowHandDealtCard.BigBlind = _Player.RealUID;
                        }
                        if (Type == HandDealtCard.ThreeCardDraw)
                        {
                            ShowHandDealtCard.MyCards = new MyCard[3];
                            for (int i = 0; i < 3; i++)
                            {
                                if (table.Board[i] != null)
                                {
                                    ShowHandDealtCard.MyCards[i] = new MyCard();
                                    ShowHandDealtCard.MyCards[i].Value = (ulong)table.Board[i].Value;
                                    ShowHandDealtCard.MyCards[i].Type = (ulong)table.Board[i].Type;
                                }
                            }
                        }
                        if (Type == HandDealtCard.FourCardDraw)
                        {
                            ShowHandDealtCard.MyCards = new MyCard[1];
                            ShowHandDealtCard.MyCards[0] = new MyCard();
                            ShowHandDealtCard.MyCards[0].Value = (ulong)table.Board[3].Value;
                            ShowHandDealtCard.MyCards[0].Type = (ulong)table.Board[3].Type;
                        }
                        if (Type == HandDealtCard.FiveCardDraw)
                        {
                            ShowHandDealtCard.MyCards = new MyCard[1];
                            ShowHandDealtCard.MyCards[0] = new MyCard();
                            ShowHandDealtCard.MyCards[0].Value = (ulong)table.Board[4].Value;
                            ShowHandDealtCard.MyCards[0].Type = (ulong)table.Board[4].Type;
                        }
                        return ShowHandDealtCard;
                    }
                #endregion
            }
            return null;
        }
    }
}
