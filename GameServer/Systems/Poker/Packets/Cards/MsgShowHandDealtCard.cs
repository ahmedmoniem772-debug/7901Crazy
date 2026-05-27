
using ConquerOnline.Database;
using ConquerOnline.Role.Instance.Poker;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConquerOnline.Game.MsgServer
{
    [ProtoContract]
    public class MsgShowHandDealtCard
    {
        public enum HandDealtCard : ushort
        {
            TwoCardDraw,
            ThreeCardDraw,
            FourCardDraw,
            FiveCardDraw,
            OneCardDraw,
            CardDown,
            CardUp,
        }
        [ProtoMember(1, IsRequired = true)]
        public ulong Counter;

        [ProtoMember(2, IsRequired = true)]
        public HandDealtCard Type;

        [ProtoMember(3, IsRequired = true)]
        public ulong ServerID_Dealer;

        [ProtoMember(4, IsRequired = true)]
        public ulong Dealer;

        [ProtoMember(5, IsRequired = true)]
        public ulong ServerID_SmallBlind;

        [ProtoMember(6, IsRequired = true)]
        public ulong SmallBlind;

        [ProtoMember(7, IsRequired = true)]
        public ulong ServerID_BigBlind;

        [ProtoMember(8, IsRequired = true)]
        public ulong BigBlind;

        [ProtoMember(9, IsRequired = true)]
        public MyCard[] MyCards;

        [ProtoMember(10, IsRequired = true)]
        public Player[] PlayerList;

       
        [ProtoContract]
        public class Player
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong serverid;

            [ProtoMember(2, IsRequired = true)]
            public ulong UID;

            [ProtoMember(3, IsRequired = true)]
            public MyCard[] MyCards;

        }
        [ProtoContract]
        public class MyCard
        {
            [ProtoMember(1, IsRequired = true)]
            public ulong Value;
            [ProtoMember(2, IsRequired = true)]
            public ulong Type;

        }

    
       
        public ServerSockets.Packet ToArray()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(this);
                stream.Finalize(GamePackets.MsgShowHandDealtCard);
                return stream;
            }
        }
        public static implicit operator ServerSockets.Packet(MsgShowHandDealtCard obj)
        {
            return obj.ToArray();
        }
        public static MsgShowHandDealtCard CreateShowHandDealtCard(Role.Instance.PokerTable table, ushort counter, MsgShowHandDealtCard.HandDealtCard Type, uint uid = 0)
        {
            
            MsgShowHandDealtCard showHandDealtCard1 = new MsgShowHandDealtCard();
            switch (Type)
            {
                case MsgShowHandDealtCard.HandDealtCard.OneCardDraw:
                    {
                        MsgShowHandDealtCard OneCardDraw = new MsgShowHandDealtCard();
                        OneCardDraw.Type = Type;
                        OneCardDraw.Counter = (ulong)counter;
                        ConquerOnline.Role.Instance.Poker.Player player3;
                        if (table.Players.TryGetValue(table.Dealer, out player3))
                        {
                            OneCardDraw.ServerID_Dealer = (ulong)player3.ServerID;
                            OneCardDraw.Dealer = (ulong)player3.Uid;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out player3))
                        {
                            OneCardDraw.ServerID_SmallBlind = (ulong)player3.ServerID;
                            OneCardDraw.SmallBlind = (ulong)player3.Uid;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out player3))
                        {
                            OneCardDraw.ServerID_BigBlind = (ulong)player3.ServerID;
                            OneCardDraw.BigBlind = (ulong)player3.Uid;
                        }
                        OneCardDraw.PlayerList = new MsgShowHandDealtCard.Player[table.Players.Values.Where<Role.Instance.Poker.Player>((Func<Role.Instance.Poker.Player, bool>)(p => p.IsPlaying)).OrderByDescending<Role.Instance.Poker.Player, byte>((Func<Role.Instance.Poker.Player, byte>)(p => p.Seat)).ToArray<Role.Instance.Poker.Player>().Length];
                        int index3 = 0;
                        foreach (Role.Instance.Poker.Player player4 in (IEnumerable<Role.Instance.Poker.Player>)table.Players.Values.Where<Role.Instance.Poker.Player>((Func<Role.Instance.Poker.Player, bool>)(p => p.IsPlaying)).OrderByDescending<Role.Instance.Poker.Player, byte>((Func<Role.Instance.Poker.Player, byte>)(p => p.Seat)))
                        {
                            OneCardDraw.PlayerList[index3] = new MsgShowHandDealtCard.Player();
                            OneCardDraw.PlayerList[index3].serverid = (ulong)player4.ServerID;
                            OneCardDraw.PlayerList[index3].UID = (ulong)player4.Uid;
                            OneCardDraw.PlayerList[index3].MyCards = new MsgShowHandDealtCard.MyCard[1];
                            OneCardDraw.PlayerList[index3].MyCards[0] = new MsgShowHandDealtCard.MyCard();
                            OneCardDraw.PlayerList[index3].MyCards[0].Value = (ulong)player4.Pocket[0].Value;
                            OneCardDraw.PlayerList[index3].MyCards[0].Type = (ulong)player4.Pocket[0].Type;
                            ++index3;
                        }

                       return OneCardDraw;

                        
                    }
                case MsgShowHandDealtCard.HandDealtCard.TwoCardDraw:
                    {

                        MsgShowHandDealtCard TwoCardDraw = new MsgShowHandDealtCard();
                        TwoCardDraw.Type = Type;
                        TwoCardDraw.Counter = counter;
                        Role.Instance.Poker.Player value3;
                        if (table.Players.TryGetValue(table.Dealer, out  value3))
                        {
                            TwoCardDraw.ServerID_Dealer = value3.ServerID;
                            TwoCardDraw.Dealer = value3.Uid;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out value3))
                        {
                            TwoCardDraw.ServerID_SmallBlind = value3.ServerID;
                            TwoCardDraw.SmallBlind = value3.Uid;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out value3))
                        {
                            TwoCardDraw.ServerID_BigBlind = value3.ServerID;
                            TwoCardDraw.BigBlind = value3.Uid;
                        }
                        if (table.Players.ContainsKey(uid) && table.Players[uid].IsPlaying)
                        {
                            TwoCardDraw.MyCards = new MyCard[table.Players[uid].Pocket.Length];
                            for (int l = 0; l < TwoCardDraw.MyCards.Length; l++)
                            {
                                TwoCardDraw.MyCards[l] = new MyCard();
                                TwoCardDraw.MyCards[l].Value = table.Players[uid].Pocket[l].Value;
                                TwoCardDraw.MyCards[l].Type = (ulong)table.Players[uid].Pocket[l].Type;
                            }
                        }
                        else
                        {
                            TwoCardDraw.Counter = 2UL;
                            TwoCardDraw.MyCards = new MsgShowHandDealtCard.MyCard[2];
                            for (int j = 0; j < TwoCardDraw.MyCards.Length; j++)
                            {
                                TwoCardDraw.MyCards[j] = new MsgShowHandDealtCard.MyCard
                                {
                                    Value = 13UL,
                                    Type = 4UL
                                };
                            }
                        }
                        int seat2 = table.Players[table.CurrentPlayer].Seat;
                        uint key2 = table.CurrentPlayer;
                        TwoCardDraw.PlayerList = new Player[table.Players.Values.Where((Role.Instance.Poker.Player p) => p.IsPlaying).ToList().Count];
                        for (int m = 0; m < table.Players.Values.Where((Role.Instance.Poker.Player p) => p.IsPlaying).ToList().Count; m++)
                        {
                            TwoCardDraw.PlayerList[m] = new Player();
                            TwoCardDraw.PlayerList[m].serverid = table.Players[key2].ServerID;
                            TwoCardDraw.PlayerList[m].UID = table.Players[key2].Uid;
                            TwoCardDraw.PlayerList[m].MyCards = new MyCard[1];
                            TwoCardDraw.PlayerList[m].MyCards[0] = new MyCard();
                            TwoCardDraw.PlayerList[m].MyCards[0].Value = (ulong)13;
                            TwoCardDraw.PlayerList[m].MyCards[0].Type = (ulong)4;
                            key2 = table.NextSeat((byte)seat2);
                            seat2 = table.Players[key2].Seat;
                        }
                        return TwoCardDraw;
                       
                    }

                case MsgShowHandDealtCard.HandDealtCard.ThreeCardDraw:
                case MsgShowHandDealtCard.HandDealtCard.FourCardDraw:
                case MsgShowHandDealtCard.HandDealtCard.FiveCardDraw:
                    {
                        MsgShowHandDealtCard showHandDealtCard3 = new MsgShowHandDealtCard();
                        showHandDealtCard3.Type = Type;
                        showHandDealtCard3.Counter = (ulong)counter;
                        ConquerOnline.Role.Instance.Poker.Player player2;
                        if (table.Players.TryGetValue(table.Dealer, out player2))
                        {
                            showHandDealtCard3.ServerID_Dealer = (ulong)player2.ServerID;
                            showHandDealtCard3.Dealer = (ulong)player2.Uid;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out player2))
                        {
                            showHandDealtCard3.ServerID_SmallBlind = (ulong)player2.ServerID;
                            showHandDealtCard3.SmallBlind = (ulong)player2.Uid;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out player2))
                        {
                            showHandDealtCard3.ServerID_BigBlind = (ulong)player2.ServerID;
                            showHandDealtCard3.BigBlind = (ulong)player2.Uid;
                        }
                        if (Type == MsgShowHandDealtCard.HandDealtCard.ThreeCardDraw)
                        {
                            showHandDealtCard3.Counter = (ulong)3;
                            showHandDealtCard3.MyCards = new MsgShowHandDealtCard.MyCard[3];
                            for (int index2 = 0; index2 < 3; ++index2)
                            {
                                if (table.Board[index2] != null)
                                {
                                    showHandDealtCard3.MyCards[index2] = new MsgShowHandDealtCard.MyCard();
                                    showHandDealtCard3.MyCards[index2].Value = (ulong)table.Board[index2].Value;
                                    showHandDealtCard3.MyCards[index2].Type = (ulong)table.Board[index2].Type;
                                }
                            }
                        }
                        if (Type == MsgShowHandDealtCard.HandDealtCard.FourCardDraw)
                        {
                            showHandDealtCard3.Counter = (ulong)1;
                            showHandDealtCard3.MyCards = new MsgShowHandDealtCard.MyCard[1];
                            showHandDealtCard3.MyCards[0] = new MsgShowHandDealtCard.MyCard();
                            showHandDealtCard3.MyCards[0].Value = (ulong)table.Board[3].Value;
                            showHandDealtCard3.MyCards[0].Type = (ulong)table.Board[3].Type;
                        }
                        if (Type == MsgShowHandDealtCard.HandDealtCard.FiveCardDraw)
                        {
                            showHandDealtCard3.Counter = (ulong)1;
                            showHandDealtCard3.MyCards = new MsgShowHandDealtCard.MyCard[1];
                            showHandDealtCard3.MyCards[0] = new MsgShowHandDealtCard.MyCard();
                            showHandDealtCard3.MyCards[0].Value = (ulong)table.Board[4].Value;
                            showHandDealtCard3.MyCards[0].Type = (ulong)table.Board[4].Type;
                        }
                        return showHandDealtCard3;
                        
                    }

                case MsgShowHandDealtCard.HandDealtCard.CardDown:
                    {
                        MsgShowHandDealtCard showHandDealtCard5 = new MsgShowHandDealtCard();
                        showHandDealtCard5.Type = Type;
                        showHandDealtCard5.Counter = (ulong)1;
                        ConquerOnline.Role.Instance.Poker.Player player5;
                        if (table.Players.TryGetValue(table.Dealer, out player5))
                        {
                            showHandDealtCard5.ServerID_Dealer = (ulong)player5.ServerID;
                            showHandDealtCard5.Dealer = (ulong)player5.Uid;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out player5))
                        {
                            showHandDealtCard5.ServerID_SmallBlind = (ulong)player5.ServerID;
                            showHandDealtCard5.SmallBlind = (ulong)player5.Uid;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out player5))
                        {
                            showHandDealtCard5.ServerID_BigBlind = (ulong)player5.ServerID;
                            showHandDealtCard5.BigBlind = (ulong)player5.Uid;
                        }
                        showHandDealtCard5.MyCards = new MsgShowHandDealtCard.MyCard[1];
                        showHandDealtCard5.MyCards[0] = new MsgShowHandDealtCard.MyCard();
                        showHandDealtCard5.MyCards[0].Value = (ulong)table.Players[uid].Pocket[0].Value;
                        showHandDealtCard5.MyCards[0].Type = (ulong)table.Players[uid].Pocket[0].Type;
                        int index4 = 0;
                        showHandDealtCard5.PlayerList = new MsgShowHandDealtCard.Player[table.Players.Values.Where<ConquerOnline.Role.Instance.Poker.Player>((Func<ConquerOnline.Role.Instance.Poker.Player, bool>)(p => p.IsPlaying)).OrderByDescending<ConquerOnline.Role.Instance.Poker.Player, byte>((Func<ConquerOnline.Role.Instance.Poker.Player, byte>)(p => p.Seat)).ToList<ConquerOnline.Role.Instance.Poker.Player>().Count];
                        foreach (ConquerOnline.Role.Instance.Poker.Player player6 in (IEnumerable<ConquerOnline.Role.Instance.Poker.Player>)table.Players.Values.Where<ConquerOnline.Role.Instance.Poker.Player>((Func<ConquerOnline.Role.Instance.Poker.Player, bool>)(p => p.IsPlaying)).OrderByDescending<ConquerOnline.Role.Instance.Poker.Player, byte>((Func<ConquerOnline.Role.Instance.Poker.Player, byte>)(p => p.Seat)))
                        {
                            showHandDealtCard5.PlayerList[index4] = new MsgShowHandDealtCard.Player();
                            showHandDealtCard5.PlayerList[index4].UID = (ulong)player6.Uid;
                            showHandDealtCard5.PlayerList[index4].serverid = (ulong)player6.ServerID;
                            showHandDealtCard5.PlayerList[index4].MyCards = new MsgShowHandDealtCard.MyCard[1];
                            showHandDealtCard5.PlayerList[index4].MyCards[0] = new MsgShowHandDealtCard.MyCard();
                            showHandDealtCard5.PlayerList[index4].MyCards[0].Value = (ulong)13;
                            showHandDealtCard5.PlayerList[index4].MyCards[0].Type = (ulong)4;
                            ++index4;
                        }
                        return showHandDealtCard5;
                        
                    }
                case MsgShowHandDealtCard.HandDealtCard.CardUp:
                    {
                        MsgShowHandDealtCard msgShowHandDealtCard = new MsgShowHandDealtCard();
                        msgShowHandDealtCard.Type = Type;
                        msgShowHandDealtCard.Counter = counter;
                        ConquerOnline.Role.Instance.Poker.Player value;
                        if (table.Players.TryGetValue(table.Dealer, out  value))
                        {
                            msgShowHandDealtCard.ServerID_Dealer = value.ServerID;
                            msgShowHandDealtCard.Dealer = value.Uid;
                        }
                        if (table.Players.TryGetValue(table.SmallBlind, out value))
                        {
                            msgShowHandDealtCard.ServerID_SmallBlind = value.ServerID;
                            msgShowHandDealtCard.SmallBlind = value.Uid;
                        }
                        if (table.Players.TryGetValue(table.BigBlind, out value))
                        {
                            msgShowHandDealtCard.ServerID_BigBlind = value.ServerID;
                            msgShowHandDealtCard.BigBlind = value.Uid;
                        }
                        msgShowHandDealtCard.PlayerList = new Player[table.Players.Values.Where((Role.Instance.Poker.Player p) => p.IsPlaying).ToList().Count * counter];
                        int num = 0;
                        for (int i = 0; i < counter; i++)
                        {
                            table.ShowHand++;
                            int seat = table.Players[table.Dealer].Seat;
                            uint key = table.Dealer;
                            for (int j = 0; j < table.Players.Values.Where((Role.Instance.Poker.Player p) => p.IsPlaying).ToList().Count; j++)
                            {
                                msgShowHandDealtCard.PlayerList[num] = new Player();
                                msgShowHandDealtCard.PlayerList[num].serverid = table.Players[key].ServerID;
                                msgShowHandDealtCard.PlayerList[num].UID = table.Players[key].Uid;
                                msgShowHandDealtCard.PlayerList[num].MyCards = new MyCard[1];
                                msgShowHandDealtCard.PlayerList[num].MyCards[0] = new MyCard();
                                msgShowHandDealtCard.PlayerList[num].MyCards[0].Value = table.Players[key].Pocket[table.ShowHand].Value;
                                msgShowHandDealtCard.PlayerList[num].MyCards[0].Type = (ulong)table.Players[key].Pocket[table.ShowHand].Type;
                                num++;
                                key = table.NextSeat((byte)seat);
                                seat = table.Players[key].Seat;
                            }
                        }
                        return msgShowHandDealtCard;
                       
                       
                    }
                    
                default:
                    showHandDealtCard1 = null;
                    break;
            }
            return showHandDealtCard1;
        }

   

    }
}
