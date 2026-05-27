using VirusX.Role;
using HoldemHand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker
{
    public class Kick
    {
        public DateTime Time;
        public List<uint> Accept;
        public List<uint> Refuse;
        public byte Total;
        public byte TotalRefuse;
        public uint ServerID_Target;
        public uint Target;
        public uint ServerID_Starter;
        public uint Starter;
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
        internal General.CardsType Type = General.CardsType.Heart;
        internal byte Value = 0;
        public override string ToString()
        {
            string text = (Value + 2).ToString();
            string name = Enum.GetName(typeof(General.CardsType), Type);
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
        public General.PlayerType PlayerType;
        internal PokerCard[] Pocket;
        public long _CurrentMoney;
        public long Lose;
        public ulong RoundPot;
        public byte Seat;
        public PokerTable Table;
        public ulong TotalPot;
        public uint Uid;
        public ulong TempMoney;
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
        public Player(string name, uint uid, uint _RealUID, uint serverId)
        {
            Name = name;
            Uid = uid;
            RealUID = _RealUID;
            ServerID = 0;
        }
        public uint RealUID;
        public uint ServerID;
        public void Create(General.PlayerType pType, byte seat, PokerTable table, ulong money)
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
            if (Table.OMAHA)
            {
                Pocket = new PokerCard[4];
            }
            else if (Table.TableType == General.TableType.ShowHand)
            {
                Pocket = new PokerCard[5];
            }
            else
            {
                Pocket = new PokerCard[2];
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
                foreach (var Card in Pocket)
                {
                    OneCard = Card.ToString();
                    for (int x = 0; x < 4; x++)
                    {
                        if (Pocket[x].ToString() != Card.ToString())
                        {
                            TwoCard = Pocket[x].ToString();
                            PocketCards = OneCard + " " + TwoCard;
                            Hand hand = new Hand(PocketCards, Board);
                            Hands.Add(hand);
                        }
                    }
                }
                return Hands.OrderByDescending(p => (uint)p.HandValue).FirstOrDefault();
            }

        internal void Create(Flags.PlayerType crossPoker, byte seat, PokerTable table, ulong money)
        {
            throw new NotImplementedException();
        }
    }
}
