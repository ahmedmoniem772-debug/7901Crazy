using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerOnline.Game.MsgServer;
using ConquerOnline.Game.MsgServer.HoldemHand;

namespace ConquerOnline.Role.Instance.Poker
{
    public class Kick
    {
        public DateTime Time;

        public List<uint> Accept;

        public List<uint> Refuse;

        public byte Total;

        public byte TotalRefuse;

        public uint Target;

        public uint Starter;

        public uint ServerID_Starter;

        public uint ServerID_Target;
    }

    internal class SidePot
    {
        private long _Money = 0L;

        internal List<uint> Players = new List<uint>();

        internal long Money
        {
            get
            {
                return _Money;
            }
            set
            {
                if (value < 0L)
                {
                    value = 0L;
                }
                _Money = value;
            }
        }
    }

    internal class PokerCard
    {
        internal ConquerOnline.Role.Flags.CardsType Type = ConquerOnline.Role.Flags.CardsType.Heart;

        internal byte Value = 0;

        public override string ToString()
        {
            string text = (Value + 2).ToString();
            string name = Enum.GetName(typeof(ConquerOnline.Role.Flags.CardsType), Type);
            if (name != null)
            {
                string str = name.Substring(0, 1).ToLower();
                if (text == "10")
                {
                    text = "t";
                }
                if (text == "11")
                {
                    text = "j";
                }
                if (text == "12")
                {
                    text = "q";
                }
                if (text == "13")
                {
                    text = "k";
                }
                if (text == "14")
                {
                    text = "a";
                }
                return text + str;
            }
            return "";
        }

        public static bool operator >(PokerCard pc1, PokerCard pc2)
        {
            if (pc1.Value == pc2.Value)
            {
                return (int)pc1.Type > (int)pc2.Type;
            }
            return pc1.Value > pc2.Value;
        }

        public static bool operator <(PokerCard pc1, PokerCard pc2)
        {
            if (pc1.Value == pc2.Value)
            {
                return (int)pc1.Type < (int)pc2.Type;
            }
            return pc1.Value < pc2.Value;
        }
    }

    public class Player
    {
        public bool IsPlaying;

        public bool IsPotAllin;

        public bool PotinThisRound;

        public bool Fold;

        public string Name;

        public ConquerOnline.Role.Flags.PlayerType PlayerType;

        internal PokerCard[] Pocket;

        public long _CurrentMoney;

        public long Lose;

        public Time32 TimeAllin;

        public bool Winall;

        public bool ClearCard;
        public bool ClearCard2;

        public ConquerOnline.Role.Flags.PlayerTypeWinner WinnerBot;

        public ulong RoundPot;

        public byte Seat;

        public PokerTable Table;

        public ulong TotalPot;

        public uint Uid;

        public ulong TempMoney;
        public uint ServerID = 0;
        public long CurrentMoney
        {
            get
            {
                return _CurrentMoney;
            }
            set
            {
                if (value < 0L)
                {
                    value = 0L;
                }
                _CurrentMoney = value;
            }
        }

        public Player(string name, uint uid, uint Serverid, bool winnerall = false)
        {
            Name = name;
            Uid = uid;
            ServerID = Serverid;
            Winall = winnerall;

        }

        public void Create(ConquerOnline.Role.Flags.PlayerType pType, byte seat, PokerTable table, ulong money)
        {
            PotinThisRound = false;
            IsPlaying = false;
            IsPotAllin = false;
            Fold = false;
            PlayerType = pType;
            TotalPot = 0uL;
            RoundPot = 0uL;
            Seat = seat;
            Lose = 0L;
            CurrentMoney = (long)money;
            TempMoney = 0uL;
            Table = table;
            if (!Table.OMAHA)
            {
                if (Table.TableType == Flags.TableType.ShowHand)
                {
                    Pocket = new PokerCard[5];
                }
                else
                {
                    Pocket = new PokerCard[2];
                }
            }
            else
            {
                Pocket = new PokerCard[4];
            }
        }

        public void Increment(ulong money)
        {
            CurrentMoney += (long)money;
        }

        public void Decrement(ulong money)
        {
            CurrentMoney -= (long)money;
            RoundPot += money;
            TotalPot += money;
            Table.TotalPot += money;
            Table.RoundPot += money;
        }

        public override string ToString()
        {
            string text = "";
            PokerCard[] pocket = Pocket;
            foreach (PokerCard pokerCard in pocket)
            {
                if (pokerCard != null)
                {
                    text = (string.IsNullOrEmpty(text) ? pokerCard.ToString() : (text + " " + pokerCard));
                }
            }
            return text;
        }
        internal Hand FourCard(string Board)
        {
            List<Hand> Hands = new List<Hand>();
            string OneCard = "";
            string TwoCard = "";
            string PocketCards = "";
            foreach (var Cards in Pocket)
            {
                OneCard = Cards.ToString();
                for (int x = 0; x < 4; x++)
                {
                    if (Pocket[x].ToString() != Cards.ToString())
                    {
                        TwoCard = Pocket[x].ToString();
                        PocketCards = OneCard + " " + TwoCard;
                        Hand Hand = new Hand(PocketCards, Board);
                        Hands.Add(Hand);
                    }
                }
            }

            return Hands.OrderByDescending(p => (uint)p.HandValue).FirstOrDefault();
        }

    }
}
