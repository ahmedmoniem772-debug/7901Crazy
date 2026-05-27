using ConquerOnline.Game.MsgServer.HoldemHand;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ConquerOnline.Game.MsgServer;
using System.Runtime.CompilerServices;

namespace ConquerOnline.Role.Instance
{
    public class PokerTable
    {
        public Role.Instance.Poker.Kick Kick;

        public List<uint> ToSend;

        public uint BigBlind;

        internal Role.Instance.Poker.PokerCard[] Board;

        internal List<Role.Instance.Poker.PokerCard> Cards;
        internal List<Role.Instance.Poker.PokerCard> CardNewRemove;
        internal List<Role.Instance.Poker.PokerCard> get5cards;
        public uint CurrentPlayer;

        public uint Dealer;

        public object dr;

        public ushort MapId = 3053;

        public ConcurrentDictionary<uint, uint> OnScreen;

        public ConcurrentDictionary<uint, Role.Instance.Poker.Player> Players;

        private Dictionary<uint, ulong> TempPlayers;

        public ConcurrentDictionary<uint, Role.Instance.Poker.Player> Watchers;

        public Role.Instance.Poker.Player PreviousPlayer;

        public ulong RequiredPot;

        public ulong RoundPot;

        public uint SmallBlind;

        public bool TableIsChange;

        public object TableSyncRoot;

        public Role.Flags.TableType TableType;

        public DateTime ThreadTime;

        public DateTime Time;

        public byte RoundState;

        private int ShowhandB;

        public ConquerOnline.Role.Flags.TableState PreviousState;

        public byte NumberOfRaise = 0;

        public bool Showhand = false;

        public long ShowhandTotalPot = 0L;

        public bool TableBusy = false;

        public uint Id;

        public ushort X;

        public ushort Y;

        internal uint Mesh;

        public uint Number;

        public bool UnLimited;

        public bool IsCPs;

        public uint MinBet;

        public ConquerOnline.Role.Flags.TableState State;

        public bool OMAHA;

        public ulong TotalPot;

        public long LowestMoney
        {
            get
            {
                long num = 0L;
                if (!Showhand)
                {
                    foreach (Role.Instance.Poker.Player value in Players.Values)
                    {
                        if (value.IsPlaying && !value.Fold)
                        {
                            if (num == 0L)
                            {
                                num = value.CurrentMoney;
                            }
                            else if (value.CurrentMoney < num)
                            {
                                num = value.CurrentMoney;
                            }
                        }
                    }
                }
                else
                {
                    num = ShowhandTotalPot;
                }
                return num;
            }
        }

        public int ShowHand
        {
            get
            {
                return ShowhandB;
            }
            set
            {
                ShowhandB = value;
            }
        }

        public PokerTable(uint id)
        {
            this.ToSend = new List<uint>();
            this.Board = new Role.Instance.Poker.PokerCard[5];
            this.MapId = 3053;
            this.NumberOfRaise = 0;
            this.Showhand = false;
            this.ShowhandTotalPot = (long)0;
            this.TableBusy = false;
            TableSyncRoot = new object();
            Id = id;
            Players = new ConcurrentDictionary<uint, Role.Instance.Poker.Player>();
            Watchers = new ConcurrentDictionary<uint, Role.Instance.Poker.Player>();
            TempPlayers = new Dictionary<uint, ulong>();
            RoundState = 0;
            Kick = null;
            PreviousState = Role.Flags.TableState.Unopened;
            RequiredPot = 0uL;
            NumberOfRaise = 0;
            TotalPot = (ulong)0;
            RoundPot = (ulong)0;
            CurrentPlayer = (uint)0;
            PreviousPlayer = null;
            Board = new Role.Instance.Poker.PokerCard[5];
            ReloadCards();
            dr = new object();
        }

        public bool CanInterface(uint UID)
        {
            if (!OnScreen.ContainsKey(UID))
            {
                return false;
            }
            return true;
        }

        public bool IsSeatAvailable(byte seat)
        {
            try
            {
                foreach (Role.Instance.Poker.Player value in Players.Values)
                {
                    if (seat == value.Seat)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool AddWatcher(Role.Instance.Poker.Player player)
        {
            try
            {
                if (player.PlayerType == Flags.PlayerType.Watcher && !Watchers.ContainsKey(player.Uid))
                {
                    Watchers.TryAdd(player.Uid, player);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool InGame()
        {
            if (State == Flags.TableState.Unopened)
            {
                return false;
            }
            if (State == Flags.TableState.ShowDown)
            {
                return false;
            }
            return true;
        }

        public bool AddPlayer(Role.Instance.Poker.Player player)
        {
            try
            {
                if (player.CurrentMoney >= MinBet * 10)
                {
                    WatcherLeave(player);
                    if (player.PlayerType == Flags.PlayerType.Player && IsSeatAvailable(player.Seat) && !Players.ContainsKey(player.Uid) && ((TableType == Flags.TableType.TexasHoldem && Players.Count < 9) || Players.Count < 5))
                    {
                        Players.TryAdd(player.Uid, player);
                        TableIsChange = true;
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool AddPlayerCross(Role.Instance.Poker.Player player)
        {
            try
            {
                if (player.CurrentMoney >= MinBet * 10)
                {
                    WatcherLeave(player);
                    if (player.PlayerType == Flags.PlayerType.CrossPoker && IsSeatAvailable(player.Seat) && !Players.ContainsKey(player.Uid) && ((TableType == Flags.TableType.TexasHoldem && Players.Count < 9) || Players.Count < 5))
                    {
                        Players.TryAdd(player.Uid, player);
                        TableIsChange = true;
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool PlayerLeave(Role.Instance.Poker.Player player)
        {
            try
            {
                if (!IsSeatAvailable(player.Seat) && Players.ContainsKey(player.Uid) && Players.Count > 0)
                {
                    if (InGame() && player.IsPlaying)
                    {
                        TempPlayers.Add(player.Uid, player.TotalPot);
                    }
                    ((IDictionary<uint, Role.Instance.Poker.Player>)Players).Remove(player.Uid);
                    TableIsChange = true;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool WatcherLeave(Role.Instance.Poker.Player player)
        {
            try
            {
                if (Watchers.ContainsKey(player.Uid))
                {
                    ((IDictionary<uint, Role.Instance.Poker.Player>)Watchers).Remove(player.Uid);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        private Role.Instance.Poker.PokerCard get5card()
        {
            lock (this.dr)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                Random random2 = new Random(Guid.NewGuid().GetHashCode() * random.Next(1, 100));
                var player = this.Players.Values.FirstOrDefault(p => p.Winall && p.WinnerBot > (Flags.PlayerTypeWinner)0);
                if (player != null && !player.Fold)
                {
                    #region A&&K
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 13 - 2) || (player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 12 - 2, 11 - 2, 10 - 2, 9 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 12 - 2) || (player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 13 - 2, 11 - 2, 10 - 2, 8 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 11 - 2) || (player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 12 - 2, 13 - 2, 10 - 2, 9 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 10 - 2) || (player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 12 - 2, 13 - 2, 11 - 2, 9 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 9 - 2) || (player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 12 - 2, 11 - 2, 13 - 2, 10 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 8 - 2) || (player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 12 - 2, 11 - 2, 13 - 2, 10 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 12 - 2, 11 - 2, 13 - 2, 10 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 5 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 11 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 5 - 2, 9 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 5 - 2, 4 - 2, 13 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 5 - 2, 3 - 2, 4 - 2, 11 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 14 - 2) || (player.Pocket[0].Value == 14 - 2 && player.Pocket[1].Value == 14 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 13 - 2, 12 - 2, 11 - 2, 10 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 13 - 2) || (player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 12 - 2, 11 - 2, 10 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 12 - 2) || (player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 10 - 2, 11 - 2, 8 - 2, 9 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 11 - 2) || (player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 12 - 2, 8 - 2, 10 - 2, 9 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 10 - 2) || (player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 12 - 2, 11 - 2, 8 - 2, 9 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 9 - 2) || (player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 12 - 2, 11 - 2, 10 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 8 - 2) || (player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 12 - 2, 11 - 2, 10 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 12 - 2, 11 - 2, 10 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 2 - 2, 3 - 2, 4 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 2 - 2, 3 - 2, 5 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 2 - 2, 5 - 2, 4 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 13 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 13 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 5 - 2, 3 - 2, 4 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 12 - 2) || (player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 10 - 2, 13 - 2, 11 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 11 - 2) || (player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 13 - 2, 10 - 2, 8 - 2, 9 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 10 - 2) || (player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 13 - 2, 11 - 2, 8 - 2, 9 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 9 - 2) || (player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 10 - 2, 13 - 2, 11 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 8 - 2) || (player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 10 - 2, 13 - 2, 11 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }

                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 10 - 2, 13 - 2, 11 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 6 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 6 - 2, 5 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 2 - 2, 4 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 12 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 12 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 3 - 2, 4 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 11 - 2) || (player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 9 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 10 - 2) || (player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 12 - 2, 13 - 2, 8 - 2, 9 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 9 - 2) || (player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 8 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 8 - 2) || (player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 9 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 9 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 6 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 6 - 2, 5 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 14 - 2, 4 - 2, 5 - 2, 13 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 11 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 11 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 3 - 2, 4 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 10 - 2) || (player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 11 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 9 - 2) || (player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 11 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 8 - 2) || (player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 11 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 13 - 2, 12 - 2, 11 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 5 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 6 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 14 - 2, 5 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 14 - 2, 4 - 2, 5 - 2, 13 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 10 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 10 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 3 - 2, 4 - 2, 5 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 9 - 2) || (player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 7 - 2, 8 - 2, 10 - 2, 11 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 8 - 2) || (player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 7 - 2, 14 - 2, 10 - 2, 11 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 13 - 2, 8 - 2, 10 - 2, 11 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 5 - 2, 7 - 2, 8 - 2, 14 - 2, 13 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 6 - 2, 7 - 2, 8 - 2, 14 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 5 - 2, 6 - 2, 7 - 2, 8 - 2, 3 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 5 - 2, 6 - 2, 7 - 2, 8 - 2, 4 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 9 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 9 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 5 - 2, 6 - 2, 7 - 2, 8 - 2, 4 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 8 - 2) || (player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 8 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 7 - 2, 9 - 2, 10 - 2, 12 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 8 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 9 - 2, 10 - 2, 12 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 8 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 4 - 2, 5 - 2, 7 - 2, 14 - 2, 13 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 8 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 4 - 2, 6 - 2, 7 - 2, 12 - 2, 13 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }

                    if (((player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 8 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 6 - 2, 5 - 2, 7 - 2, 14 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 8 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 4 - 2, 5 - 2, 6 - 2, 7 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 8 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 8 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 4 - 2, 5 - 2, 6 - 2, 7 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 7 - 2) || (player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 7 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> {8 - 2, 9 - 2, 10 - 2, 12 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }

                    if (((player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 7 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 3 - 2, 4 - 2, 5 - 2, 10 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 7 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 3 - 2, 4 - 2, 6 - 2, 10 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 7 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 3 - 2, 6 - 2, 5 - 2, 10 - 2, 11 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 7 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 4 - 2, 5 - 2, 6 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 7 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 7 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 3 - 2, 4 - 2, 5 - 2, 6 - 2, 13 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 6 - 2) || (player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 6 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 5 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 6 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 4 - 2, 14 - 2, 7 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 6 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 3 - 2, 5 - 2, 13 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 6 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 2 - 2, 5 - 2, 4 - 2, 11 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 6 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 6 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 3 - 2, 4 - 2, 5 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 5 - 2) || (player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 5 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 4 - 2, 3 - 2, 9 - 2, 2 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 5 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 7 - 2, 3 - 2, 9 - 2, 2 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 5 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 2 - 2, 4 - 2, 9 - 2, 10 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 5 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 5 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {

                        var HighCards = new List<int> { 14 - 2, 3 - 2, 4 - 2, 8 - 2, 13 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 4 - 2) || (player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 4 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 2 - 2, 3 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 4 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 2 - 2, 5 - 2, 11 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 4 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 4 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 3 - 2, 5 - 2, 13 - 2, 8 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 3 - 2) || (player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 3 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 2 - 2, 4 - 2, 5 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    if (((player.Pocket[0].Value == 3 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 3 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 4 - 2, 5 - 2, 10 - 2, 9 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }

                    if (((player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 2 - 2) || (player.Pocket[0].Value == 2 - 2 && player.Pocket[1].Value == 2 - 2)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
                    {
                         
                        var HighCards = new List<int> { 14 - 2, 3 - 2, 4 - 2, 5 - 2, 12 - 2 };
                        int cardnum;
                        for (cardnum = 0; cardnum < HighCards.Count; cardnum++)
                        {
                            var array = CardNewRemove.Where(p => p.Value == HighCards[cardnum]).ToArray();

                            Role.Instance.Poker.PokerCard pokerCards = new ConquerOnline.Role.Instance.Poker.PokerCard();
                            pokerCards.Value = (byte)HighCards[cardnum];
                            var list = new List<Flags.CardsType>();
                            for (int i = 0; i < array.Length; i++)
                            {
                                list.Add(array[i].Type);
                            }
                            foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                            {
                                Flags.CardsType cardsType = (Flags.CardsType)obj;
                                if (!list.Contains(cardsType))
                                {
                                    pokerCards.Type = cardsType;
                                }
                            }
                            this.get5cards.Add(pokerCards);
                        }
                        player.ClearCard = true;
                    }
                    #endregion
                    var listt = new List<Flags.CardsType>();
                    for (int x = 0; x < get5cards.Count; x++)
                    {
                        foreach (var obj in Enum.GetValues(typeof(Flags.CardsType)))
                        {
                            Flags.CardsType cardsType = (Flags.CardsType)obj;
                            if (!listt.Contains(cardsType))
                            {
                                get5cards[x].Type = cardsType;
                                listt.Add(cardsType);
                                break;
                            }
                        }
                    }
                }
                foreach (var card in get5cards)
                {
                    Cards.RemoveAll(p => p.Value == card.Value && p.Type == card.Type);
                }
                return null;
            }
        }


        private void ReloadCards()
        {
            while (true)
            {
                Cards = new List<Role.Instance.Poker.PokerCard>();
                CardNewRemove = new List<Role.Instance.Poker.PokerCard>();
                get5cards = new List<Role.Instance.Poker.PokerCard>();
                byte b = 0;
                if (Mesh == 7210797 || Mesh == 7247567 || Mesh == 7255787 || TableType == Role.Flags.TableType.ShowHand)
                {
                    b = 5;
                }
                for (byte b2 = 0; b2 < 4; b2 = (byte)(b2 + 1))
                {
                    Role.Flags.CardsType type = Role.Flags.CardsType.Heart;
                    if (b2 == 1)
                    {
                        type = Role.Flags.CardsType.Spade;
                    }
                    if (b2 == 2)
                    {
                        type = Role.Flags.CardsType.Club;
                    }
                    if (b2 == 3)
                    {
                        type = Role.Flags.CardsType.Diamond;
                    }
                    for (byte b3 = b; b3 < 13; b3 = (byte)(b3 + 1))
                    {
                        Role.Instance.Poker.PokerCard pokerCard = new Role.Instance.Poker.PokerCard();
                        pokerCard.Type = type;
                        pokerCard.Value = b3;
                        Role.Instance.Poker.PokerCard item = pokerCard;
                        Cards.Add(item);
                    }
                }
                if (b == 5)
                {
                    if (Cards.Count >= 32)
                    {
                        break;
                    }
                }
                else if (Cards.Count >= 52)
                {
                    break;
                }
            }
        }

        private Role.Instance.Poker.PokerCard Draw()
        {
            Role.Instance.Poker.PokerCard pokerCard = null;
            lock (dr)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                Random random2 = new Random(Guid.NewGuid().GetHashCode() * random.Next(1, 100));
                int num = random2.Next(0, Cards.Count);
                if (Cards.Count > 0)
                {
                    if (Cards.Count > num && Cards[num] != null)
                    {
                        pokerCard = Cards[num];
                        CardNewRemove.Add(Cards[num]);
                        Cards.RemoveAt(num);
                        return pokerCard;
                    }
                }
                else
                {
                    Console.WriteLine("Error In Card Draw");
                }
                return null;
            }
        }
        private void AddCardsToPlayersOne(int count)
        {
            if (this.TableType == Flags.TableType.ShowHand)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var player in from p in this.Players.Values
                                           where p.IsPlaying
                                           select p)
                    {
                        if (player.Pocket[i] == null)
                        {
                            int num = 0;
                            for (; ; )
                            {
                                player.Pocket[i] = this.Draw();
                                if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
                                {
                                    break;
                                }
                                num++;
                            }
                        }
                    }
                }
            }
            else
            {
                
                for (int i = 0; i < count; i++)
                {
                    foreach (var player in from p in this.Players.Values
                                           where p.IsPlaying
                                           select p)
                    {
                        int num = 0;
                        for (; ; )
                        {
                            player.Pocket[i] = this.Draw();
                            if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
                            {
                                break;
                            }
                            num++;
                        }
                    }
                }
            }
        }
        private void AddCardsToPlayers(int count)
        {
            if (this.TableType == Flags.TableType.ShowHand)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var player in from p in this.Players.Values
                                           where p.IsPlaying
                                           select p)
                    {
                        if (player.Pocket[i] == null)
                        {
                            int num = 0;
                            for (; ; )
                            {
                                player.Pocket[i] = this.Draw();
                                if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
                                {
                                    break;
                                }
                                num++;
                            }
                        }
                    }
                }
            }
            else
            {
                var players = this.Players.Values.FirstOrDefault(p => p.Winall && p.WinnerBot > (Flags.PlayerTypeWinner)0);
                if (players != null && !players.Fold)
                {
                    for (int i = 0; i < count; i++)
                    {
                        foreach (var player in from p in this.Players.Values
                                               where p.IsPlaying && p.Winall
                                               select p)
                        {
                            int num = 0;
                            for (; ; )
                            {
                                player.Pocket[i] = this.Draw();
                                if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
                                {
                                    break;
                                }
                                num++;
                            }
                        }
                    }
                    get5card();
                }
                for (int i = 0; i < count; i++)
                {
                    foreach (var player in from p in this.Players.Values
                                           where p.IsPlaying && !p.Winall
                                           select p)
                    {
                        int num = 0;
                        for (; ; )
                        {
                            player.Pocket[i] = this.Draw();
                            if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
                            {
                                break;
                            }
                            num++;
                        }
                    }
                }
            }
        }

        public void AddCardsToBoard(int count)
        {   
            var player = this.Players.Values.FirstOrDefault(p => p.Winall && p.WinnerBot > (Flags.PlayerTypeWinner)0);
            if (player != null && !player.Fold)
            {
                if (!player.ClearCard2)
                {
                    Cards.Clear();
                    foreach (var card in get5cards)
                    {
                        Cards.Add(card);
                    }
                    player.ClearCard2 = true;
                }
            }
            byte b = 0;
            while ((int)b < count)
            {
                if (this.Board[(int)b] == null)
                {
                    int num = 0;
                    for (; ; )
                    {
                        this.Board[(int)b] = this.Draw();
                        if (this.Board[(int)b] != null || this.Cards.Count <= 1 || num >= 30)
                        {
                            break;
                        }
                        num++;
                    }
                }
                b += 1;
            }
        }

        public void Clear()
        {
            Showhand = false;
            ShowhandTotalPot = 0L;
            ShowHand = 0;
            TempPlayers.Clear();
            RoundState = 0;
            Kick = null;
            PreviousState = Role.Flags.TableState.Unopened;
            RequiredPot = 0uL;
            NumberOfRaise = 0;
            TotalPot = 0uL;
            RoundPot = 0uL;
            CurrentPlayer = 0u;
            PreviousPlayer = null;
            Board = new Role.Instance.Poker.PokerCard[5];
            foreach (Role.Instance.Poker.Player value in Players.Values)
            {
                value.Create(value.PlayerType, value.Seat, this, (ulong)value.CurrentMoney);
            }
            ReloadCards();
        }

        public void StartNewRound()
        {
            var player = this.Players.Values.FirstOrDefault(p => p.Winall);
            if (player != null)
            {
                player.WinnerBot = Flags.PlayerTypeWinner.stright;
                player.ClearCard = false;
                player.ClearCard2 = false;
            }
            if (TableType == Role.Flags.TableType.TexasHoldem)
            {
                ShowHand = 0;
                ReloadCards();
                foreach (Role.Instance.Poker.Player value in Players.Values)
                {
                    value.IsPlaying = true;
                }
                if (TableIsChange)
                {
                    AddCardsToPlayersOne(1);
                    ReloadCards();
                }
                GetDealer();
                TotalPot = (ulong)((from p in Players.Values
                                    where p.IsPlaying
                                    select p).ToList().Count * (MinBet / 2u));
                foreach (Role.Instance.Poker.Player item in from p in Players.Values
                                                            where p.IsPlaying
                                                            select p)
                {
                    item.CurrentMoney -= MinBet / 2u;
                    item.TotalPot = MinBet / 2u;
                }
                Players[SmallBlind].CurrentMoney -= MinBet / 2u;
                Players[SmallBlind].RoundPot += MinBet / 2u;
                Players[SmallBlind].TotalPot += MinBet / 2u;
                TotalPot += MinBet / 2u;
                RoundPot += MinBet / 2u;
                Players[BigBlind].CurrentMoney -= MinBet;
                Players[BigBlind].RoundPot += MinBet;
                Players[BigBlind].TotalPot += MinBet;
                TotalPot += MinBet;
                RoundPot += MinBet;
                State = Role.Flags.TableState.Pocket;
            }
            else
            {
                ReloadCards();
                foreach (Role.Instance.Poker.Player value2 in Players.Values)
                {
                    value2.IsPlaying = true;
                }
                AddCardsToPlayers(2);
                GetDealer();
                PreviousPlayer = Players[PreviousSeat(Players[Dealer].Seat)];
                TotalPot = (ulong)((from p in Players.Values
                                    where p.IsPlaying
                                    select p).ToList().Count * (MinBet / 2u));
                foreach (Role.Instance.Poker.Player item2 in from p in Players.Values
                                                             where p.IsPlaying
                                                             select p)
                {
                    item2.CurrentMoney -= MinBet / 2u;
                    item2.TotalPot = MinBet / 2u;
                }
                State = Role.Flags.TableState.Pocket;
            }
        }

        public void StartPocket()
        {
            if (TableType == Flags.TableType.TexasHoldem)
            {
                CurrentPlayer = NextSeat(Players[BigBlind].Seat);
                if (!OMAHA)
                {
                    AddCardsToPlayers(2);
                }
                else
                {
                    AddCardsToPlayers(4);
                }
                GetRequiredBet();
            }
            else
            {
                CurrentPlayer = Dealer;
                GetRequiredBet();
            }
        }

        private void GetRequiredBet()
        {
            if (PreviousPlayer != null && CurrentPlayer != 0)
            {
                RequiredPot = PreviousPlayer.RoundPot - Players[CurrentPlayer].RoundPot;
            }
            else
            {
                RequiredPot = 0uL;
            }
        }

        internal ushort GetRequiredAction()
        {
            if (TableType == Role.Flags.TableType.TexasHoldem)
            {
                if (!Players.ContainsKey(CurrentPlayer))
                {
                    return 0;
                }
                if (!UnLimited)
                {
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet && State == Role.Flags.TableState.Pocket && NumberOfRaise < 3)
                    {
                        return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet && State == Role.Flags.TableState.Pocket && NumberOfRaise >= 3)
                    {
                        return (ushort)(General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet && State != Role.Flags.TableState.Pocket)
                    {
                        return (ushort)(General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot + 10L && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot)
                    {
                        return (ushort)(General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot)
                    {
                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet * 2 && RoundPot > 0L)
                    {
                        return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet * 2 && RoundPot > 0L)
                    {
                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                    }
                    if (NumberOfRaise == 0)
                    {
                        if (State == Role.Flags.TableState.Pocket || State == Role.Flags.TableState.Flop)
                        {
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                        }
                        else
                        {
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet * 2 && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet * 2 && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                        }
                    }
                    else if (NumberOfRaise > 0)
                    {
                        if (State == Role.Flags.TableState.Pocket)
                        {
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                        }
                        else
                        {
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet * 2 && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                            if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet * 2 && RoundPot == 0L)
                            {
                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                            }
                        }
                    }
                    Console.WriteLine("Unhandle RequiredBet: " + RequiredPot);
                    return 0;
                }
                if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > PreviousPlayer.RoundPot + RequiredPot)
                {
                    return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney <= PreviousPlayer.RoundPot + RequiredPot && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet && RoundPot > 0L)
                {
                    return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet && RoundPot > 0L)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet && RoundPot == 0L)
                {
                    return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet && RoundPot == 0L)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                Console.WriteLine("Unhandle RequiredBet: " + RequiredPot);
            }
            else
            {
                if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > PreviousPlayer.RoundPot + RequiredPot && !Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney <= PreviousPlayer.RoundPot + RequiredPot && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot && !Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot && !Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet && RoundPot > 0L && !Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet && RoundPot > 0L && !Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney > MinBet && RoundPot == 0L && !Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                if (RequiredPot == 0L && Players[CurrentPlayer].CurrentMoney <= MinBet && RoundPot == 0L && !Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
                }
                if (Showhand)
                {
                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
                }
                Console.WriteLine("Unhandle RequiredBet: " + RequiredPot);
            }
            return 0;
        }

        private byte CheckRound()
        {
            ulong num = 0uL;
            bool flag = true;
            bool flag2 = true;
            foreach (Role.Instance.Poker.Player item in from p in Players.Values
                                                        where p.IsPlaying && !p.IsPotAllin && !p.Fold
                                                        select p)
            {
                if (num == 0L)
                {
                    num = item.RoundPot;
                }
                if (num != item.RoundPot)
                {
                    flag = false;
                    break;
                }
                if (!item.PotinThisRound)
                {
                    flag2 = false;
                    break;
                }
            }
            if ((from p in Players.Values
                 where p.IsPlaying && !p.Fold
                 select p).ToList().Count == 1)
            {
                return 3;
            }
            if (flag2 && flag && (from p in Players.Values
                                  where p.IsPlaying && !p.IsPotAllin && !p.Fold
                                  select p).ToList().Count == 0)
            {
                return 1;
            }
            if (flag2 && flag && (from p in Players.Values
                                  where p.IsPlaying && !p.IsPotAllin && !p.Fold
                                  select p).ToList().Count == 1 && RequiredPot == 0L)
            {
                return 1;
            }
            if (flag2 && flag && (from p in Players.Values
                                  where p.IsPlaying && !p.IsPotAllin && !p.Fold
                                  select p).ToList().Count > 1 && RequiredPot == 0L)
            {
                return 2;
            }
            return 0;
        }

        public bool Next(bool c = false)
        {
            if (TableType == Role.Flags.TableType.TexasHoldem)
            {
                if (c)
                {
                    PreviousPlayer = Players[CurrentPlayer];
                }
                if (PreviousPlayer == null)
                {
                    CurrentPlayer = NextSeat(Players[SmallBlind].Seat);
                }
                else
                {
                    CurrentPlayer = NextSeat(PreviousPlayer.Seat);
                }
                GetRequiredBet();
                NextRound();
                if (State == Role.Flags.TableState.ShowDown)
                {
                    return false;
                }
                return true;
            }
            if (c)
            {
                PreviousPlayer = Players[CurrentPlayer];
            }
            if (PreviousPlayer == null)
            {
                CurrentPlayer = NextSeat(Players[Dealer].Seat);
            }
            else
            {
                CurrentPlayer = NextSeat(PreviousPlayer.Seat);
            }
            GetRequiredBet();
            NextRound();
            if (State == Role.Flags.TableState.ShowDown)
            {
                return false;
            }
            return true;
        }

        public void NextRound()
        {
            if (TableType == Role.Flags.TableType.TexasHoldem)
            {
                byte b = CheckRound();
                if (b == 1 || b == 3)
                {
                    AddCardsToBoard(5);
                    if (b == 1)
                    {
                        PreviousState = State;
                    }
                    RoundPot = 0uL;
                    CurrentPlayer = 0u;
                    PreviousPlayer = null;
                    RoundState = 0;
                    State = Role.Flags.TableState.ShowDown;
                }
                else if (b == 2)
                {
                    if (State + 1 == Role.Flags.TableState.Flop)
                    {
                        AddCardsToBoard(3);
                    }
                    if (State + 1 == Role.Flags.TableState.Turn)
                    {
                        AddCardsToBoard(4);
                    }
                    if (State + 1 == Role.Flags.TableState.River)
                    {
                        AddCardsToBoard(5);
                    }
                    foreach (Role.Instance.Poker.Player item in from p in Players.Values
                                                                where p.IsPlaying
                                                                select p)
                    {
                        item.PotinThisRound = false;
                        item.RoundPot = 0uL;
                    }
                    CurrentPlayer = SmallBlind;
                    RoundPot = 0uL;
                    RoundState = 0;
                    State++;
                    if (State == Role.Flags.TableState.ShowDown)
                    {
                        CurrentPlayer = 0u;
                        PreviousPlayer = null;
                    }
                }
            }
            else
            {
                byte b = CheckRound();
                if (b == 1 || b == 3)
                {
                    AddCardsToPlayers(5);
                    if (b == 1)
                    {
                        PreviousState = State;
                    }
                    RoundPot = 0uL;
                    CurrentPlayer = 0u;
                    PreviousPlayer = null;
                    RoundState = 0;
                    GetDealer();
                    State = Role.Flags.TableState.ShowDown;
                }
                else if (b == 2)
                {
                    if (State + 1 == Role.Flags.TableState.Flop)
                    {
                        AddCardsToPlayers(3);
                    }
                    if (State + 1 == Role.Flags.TableState.Turn)
                    {
                        AddCardsToPlayers(4);
                    }
                    if (State + 1 == Role.Flags.TableState.River)
                    {
                        AddCardsToPlayers(5);
                    }
                    foreach (Role.Instance.Poker.Player item2 in from p in Players.Values
                                                                 where p.IsPlaying
                                                                 select p)
                    {
                        item2.PotinThisRound = false;
                        item2.RoundPot = 0uL;
                    }
                    GetDealer();
                    CurrentPlayer = Dealer;
                    RoundPot = 0uL;
                    RoundState = 0;
                    State++;
                    if (State == Role.Flags.TableState.ShowDown)
                    {
                        CurrentPlayer = 0u;
                        PreviousPlayer = null;
                    }
                }
            }
            if (Players.ContainsKey(CurrentPlayer))
                Players[CurrentPlayer].TimeAllin = Time32.Now.AddSeconds(3);
        }

        internal void GetDealer()
        {
            if (TableType == Role.Flags.TableType.TexasHoldem)
            {
                if (TableIsChange)
                {
                    Dealer = 0u;
                }
                if (Dealer == 0)
                {
                    foreach (Role.Instance.Poker.Player item in from p in Players.Values
                                                                where p.IsPlaying
                                                                select p)
                    {
                        if (Dealer == 0)
                        {
                            Dealer = item.Uid;
                        }
                        else if (item.Pocket[0] > Players[Dealer].Pocket[0])
                        {
                            Dealer = item.Uid;
                        }
                    }
                }
                else
                {
                    Dealer = NextSeat(Players[Dealer].Seat);
                }
                SmallBlind = NextSeat(Players[Dealer].Seat);
                BigBlind = NextSeat(Players[SmallBlind].Seat);
                PreviousPlayer = Players[BigBlind];
                CurrentPlayer = 0u;
            }
            else
            {
                Dealer = 0u;
                if (Dealer == 0)
                {
                    foreach (Role.Instance.Poker.Player item2 in from p in Players.Values
                                                                 where p.IsPlaying
                                                                 select p)
                    {
                        if (Dealer == 0)
                        {
                            Dealer = item2.Uid;
                        }
                        else
                        {
                            int num = 0;
                            int num2 = 0;
                            string text = "";
                            string text2 = "";
                            Role.Instance.Poker.PokerCard[] pocket = item2.Pocket;
                            foreach (Role.Instance.Poker.PokerCard pokerCard in pocket)
                            {
                                if (item2.Pocket[0] != null && item2.Pocket[0] != pokerCard && pokerCard != null)
                                {
                                    text = text + " " + pokerCard.ToString();
                                    num++;
                                }
                            }
                            pocket = Players[Dealer].Pocket;
                            foreach (Role.Instance.Poker.PokerCard pokerCard in pocket)
                            {
                                if (Players[Dealer].Pocket[0] != null && Players[Dealer].Pocket[0] != pokerCard && pokerCard != null)
                                {
                                    text2 = text2 + " " + pokerCard.ToString();
                                    num2++;
                                }
                            }
                            ulong cards = Hand.ParseHand(text);
                            ulong cards2 = Hand.ParseHand(text2);
                            uint num3 = Hand.Evaluate(cards, num);
                            uint num4 = Hand.Evaluate(cards2, num2);
                            Hand.DescriptionFromHandValueInternal(num3);
                            Hand.DescriptionFromHandValueInternal(num4);
                            if (num3 > num4)
                            {
                                Dealer = item2.Uid;
                            }
                        }
                    }
                }
                CurrentPlayer = 0u;
            }
        }

        public uint PreviousSeat(int seat)
        {
            List<Role.Instance.Poker.Player> list = (from p in Players.Values
                                                     where p.IsPlaying && !p.IsPotAllin && !p.Fold
                                                     select p).ToList();
            if (list.Count == 0)
            {
                return 0u;
            }
            if (list.Count == 1)
            {
                return list.FirstOrDefault().Uid;
            }
            uint num = 0u;
            int num2 = 0;
            while (num == 0 && num2 < 20)
            {
                num2++;
                seat--;
                if (seat < 0)
                {
                    seat = 4;
                }
                Role.Instance.Poker.Player player = (from x in list
                                                     where x.Seat == seat
                                                     select x).FirstOrDefault();
                if (player != null)
                {
                    num = player.Uid;
                }
            }
            return num;
        }

        public uint NextSeat(byte seat)
        {
            List<Role.Instance.Poker.Player> list;
            uint num;
            int num2;
            if (TableType == Role.Flags.TableType.TexasHoldem)
            {
                list = (from p in Players.Values
                        where p.IsPlaying && !p.IsPotAllin && !p.Fold
                        select p).ToList();
                if (list.Count == 0)
                {
                    return 0u;
                }
                if (list.Count == 1)
                {
                    return list.FirstOrDefault().Uid;
                }
                num = 0u;
                num2 = 0;
                while (num == 0 && num2 < 20)
                {
                    num2++;
                    seat = (byte)(seat + 1);
                    if (seat > 8)
                    {
                        seat = 0;
                    }
                    Role.Instance.Poker.Player player = (from x in list
                                                         where x.Seat == seat
                                                         select x).FirstOrDefault();
                    if (player != null)
                    {
                        num = player.Uid;
                    }
                }
                return num;
            }
            list = (from p in Players.Values
                    where p.IsPlaying && !p.IsPotAllin && !p.Fold
                    select p).ToList();
            if (State == Role.Flags.TableState.ShowDown)
            {
                list = (from p in Players.Values
                        where p.IsPlaying && !p.Fold
                        select p).ToList();
            }
            if (list.Count == 0)
            {
                return 0u;
            }
            if (list.Count == 1)
            {
                return list.FirstOrDefault().Uid;
            }
            num = 0u;
            num2 = 0;
            while (num == 0 && num2 < 20)
            {
                num2++;
                seat = (byte)(seat + 1);
                if (seat > 5)
                {
                    seat = 0;
                }
                Role.Instance.Poker.Player player = (from x in list
                                                     where x.Seat == seat
                                                     select x).FirstOrDefault();
                if (player != null)
                {
                    num = player.Uid;
                }
            }
            return num;
        }

        public bool HighestBet(uint uid, ulong bet)
        {
            bool result = true;
            foreach (Role.Instance.Poker.Player item in from p in Players.Values
                                                        where p.IsPlaying
                                                        select p)
            {
                if (item.Uid != uid && item.RoundPot > bet)
                {
                    result = false;
                    break;
                }
            }
            foreach (KeyValuePair<uint, ulong> tempPlayer in TempPlayers)
            {
                if (tempPlayer.Value > Players[uid].TotalPot)
                {
                    return false;
                }
            }
            return result;
        }

        public IEnumerable<Role.Instance.Poker.Player> PlayersOnTable()
        {
            List<Role.Instance.Poker.Player> list = new List<Role.Instance.Poker.Player>();
            list.AddRange(Players.Values);
            list.AddRange(Watchers.Values);
            return list;
        }

        public void GetWinners()
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            try
            {
                if (TableType == Role.Flags.TableType.TexasHoldem)
                {
                    TableBusy = true;
                    IEnumerable<Role.Instance.Poker.Player> enumerable = from p in Players.Values
                                                                         where p.IsPlaying
                                                                         select p;
                    Dictionary<uint, Hand> dictionary = new Dictionary<uint, Hand>();
                    Dictionary<uint, Hand> dictionary2 = new Dictionary<uint, Hand>();

                    foreach (Role.Instance.Poker.Player item in enumerable)
                    {
                        if (TempPlayers.ContainsKey(item.Uid))
                        {
                            TempPlayers[item.Uid] = item.TotalPot;
                        }
                        else
                        {
                            TempPlayers.Add(item.Uid, item.TotalPot);
                        }
                        if (item.Fold)
                        {
                            item.Lose -= (long)item.TotalPot;
                        }
                        if (!item.Fold)
                        {
                            Hand value = ((!OMAHA) ? new Hand(item.ToString(), ToString()) : item.FourCard(ToString()));
                            dictionary2.Add(item.Uid, value);
                            dictionary.Add(item.Uid, value);
                        }
                    }
                    ConcurrentDictionary<uint, Role.Instance.Poker.SidePot> concurrentDictionary = CalcaluteSidePots();
                    List<uint> list = new List<uint>();
                    foreach (KeyValuePair<uint, Role.Instance.Poker.SidePot> item2 in concurrentDictionary)
                    {
                        foreach (KeyValuePair<uint, Hand> item3 in dictionary)
                        {
                            if (item2.Value.Players.Count == 1 && item2.Value.Players.Contains(item3.Key))
                            {
                                Players[item3.Key].CurrentMoney += item2.Value.Money;
                                Players[item3.Key].TotalPot -= (ulong)item2.Value.Money;
                                if (!list.Contains(item2.Key))
                                {
                                    list.Add(item2.Key);
                                }
                            }
                        }
                    }
                    foreach (uint item4 in list)
                    {
                        if (concurrentDictionary.ContainsKey(item4))
                        {
                            ((IDictionary<uint, Role.Instance.Poker.SidePot>)concurrentDictionary).Remove(item4);
                        }
                    }
                    list.Clear();
                    while (true)
                    {
                    IL_0693:
                        Dictionary<uint, Hand> dictionary3 = CalcaluteBestHands(dictionary);
                        bool flag = false;
                        while (true)
                        {
                        IL_067e:
                            List<uint> list2 = new List<uint>();
                            using (Dictionary<uint, Hand>.Enumerator enumerator3 = dictionary3.GetEnumerator())
                            {
                                while (enumerator3.MoveNext())
                                {
                                    Func<Role.Instance.Poker.Player, bool> func = null;
                                    KeyValuePair<uint, Hand> p2 = enumerator3.Current;
                                    IEnumerable<Role.Instance.Poker.Player> source = enumerable;
                                    func = delegate(Role.Instance.Poker.Player x)
                                    {
                                        uint uid2 = x.Uid;
                                        KeyValuePair<uint, Hand> keyValuePair4 = p2;
                                        return uid2 == keyValuePair4.Key;
                                    };
                                    Role.Instance.Poker.Player player = source.Where(func).FirstOrDefault();
                                    if (player != null)
                                    {
                                        foreach (KeyValuePair<uint, Role.Instance.Poker.SidePot> item5 in concurrentDictionary)
                                        {
                                            long num5 = 0L;
                                            foreach (uint player2 in item5.Value.Players)
                                            {
                                                if (dictionary3.ContainsKey(player2))
                                                {
                                                    num5++;
                                                }
                                            }
                                            if (item5.Value.Players.Contains(player.Uid) || flag)
                                            {
                                                if (!flag)
                                                {
                                                    player.Lose += item5.Value.Money / num5;
                                                }
                                                else
                                                {
                                                    player.Lose += item5.Value.Money / dictionary3.Count;
                                                }
                                                if (!list.Contains(item5.Key))
                                                {
                                                    list.Add(item5.Key);
                                                }
                                            }
                                        }
                                    }
                                    List<uint> list3 = list2;
                                    KeyValuePair<uint, Hand> keyValuePair = p2;
                                    if (!list3.Contains(keyValuePair.Key))
                                    {
                                        List<uint> list4 = list2;
                                        keyValuePair = p2;
                                        list4.Add(keyValuePair.Key);
                                    }
                                }
                            }
                            foreach (uint item6 in list2)
                            {
                                if (dictionary.ContainsKey(item6))
                                {
                                    dictionary.Remove(item6);
                                }
                            }
                            foreach (uint item7 in list)
                            {
                                if (concurrentDictionary.ContainsKey(item7))
                                {
                                    ((IDictionary<uint, Role.Instance.Poker.SidePot>)concurrentDictionary).Remove(item7);
                                }
                            }
                            if (concurrentDictionary.Count > 0)
                            {
                                foreach (Role.Instance.Poker.SidePot value2 in concurrentDictionary.Values)
                                {
                                    foreach (KeyValuePair<uint, Hand> item8 in dictionary)
                                    {
                                        if (!value2.Players.Contains(item8.Key) || !Players.ContainsKey(item8.Key) || Players[item8.Key].Fold || num >= 30)
                                        {
                                            continue;
                                        }
                                        num++;
                                        goto IL_0693;
                                    }
                                }
                            }
                            if (concurrentDictionary.Count > 0)
                            {
                                foreach (Role.Instance.Poker.SidePot value3 in concurrentDictionary.Values)
                                {
                                    dictionary3 = CalcaluteBestHands(dictionary2);
                                    flag = true;
                                    if (dictionary3.Count < 1)
                                    {
                                        continue;
                                    }
                                    bool flag2 = false;
                                    foreach (KeyValuePair<uint, Hand> item9 in dictionary3)
                                    {
                                        if (!Players.ContainsKey(item9.Key))
                                        {
                                            flag2 = true;
                                        }
                                    }
                                    if (flag2 || num2 >= 30)
                                    {
                                        continue;
                                    }
                                    num2++;
                                    goto IL_067e;
                                }
                            }
                            foreach (KeyValuePair<uint, Hand> item10 in dictionary2)
                            {
                                Players[item10.Key].CurrentMoney += Players[item10.Key].Lose;
                                if (Players[item10.Key].CurrentMoney < 0L)
                                {
                                    Players[item10.Key].CurrentMoney = 0L;
                                }
                                if (Players[item10.Key].Lose >= (long)Players[item10.Key].TotalPot)
                                {
                                    Players[item10.Key].Lose = Players[item10.Key].Lose - (long)(Players[item10.Key].TotalPot * 99L / 100uL);
                                }
                                else
                                {
                                    Players[item10.Key].Lose = Players[item10.Key].Lose - (long)Players[item10.Key].TotalPot;
                                }

                            }
                            break;
                        }
                        break;
                    }
                }
                else
                {
                    TableBusy = true;
                    IEnumerable<Role.Instance.Poker.Player> enumerable = from p in Players.Values
                                                                         where p.IsPlaying
                                                                         select p;
                    Dictionary<uint, ulong> dictionary4 = new Dictionary<uint, ulong>();
                    Dictionary<uint, ulong> dictionary5 = new Dictionary<uint, ulong>();
                    foreach (Role.Instance.Poker.Player item11 in enumerable)
                    {
                        if (TempPlayers.ContainsKey(item11.Uid))
                        {
                            TempPlayers[item11.Uid] = item11.TotalPot;
                        }
                        else
                        {
                            TempPlayers.Add(item11.Uid, item11.TotalPot);
                        }
                        if (item11.Fold)
                        {
                            item11.Lose -= (long)item11.TotalPot;
                        }
                        if (!item11.Fold)
                        {
                            int num6 = 0;
                            string text = "";
                            Role.Instance.Poker.PokerCard[] pocket = item11.Pocket;
                            foreach (Role.Instance.Poker.PokerCard pokerCard in pocket)
                            {
                                if (item11.Pocket[0] != null && item11.Pocket[0] != pokerCard && pokerCard != null)
                                {
                                    text = text + " " + pokerCard.ToString();
                                    num6++;
                                }
                            }
                            ulong cards = Hand.ParseHand(text);
                            uint num7 = Hand.Evaluate(cards, num6);
                            dictionary5.Add(item11.Uid, num7);
                            dictionary4.Add(item11.Uid, num7);
                        }
                    }
                    ConcurrentDictionary<uint, Role.Instance.Poker.SidePot> concurrentDictionary = CalcaluteSidePots();
                    List<uint> list = new List<uint>();
                    foreach (KeyValuePair<uint, Role.Instance.Poker.SidePot> item12 in concurrentDictionary)
                    {
                        foreach (KeyValuePair<uint, ulong> item13 in dictionary4)
                        {
                            if (item12.Value.Players.Count == 1 && item12.Value.Players.Contains(item13.Key))
                            {
                                Players[item13.Key].CurrentMoney += item12.Value.Money;
                                Players[item13.Key].TotalPot -= (ulong)item12.Value.Money;
                                if (!list.Contains(item12.Key))
                                {
                                    list.Add(item12.Key);
                                }
                            }
                        }
                    }
                    foreach (uint item14 in list)
                    {
                        if (concurrentDictionary.ContainsKey(item14))
                        {
                            ((IDictionary<uint, Role.Instance.Poker.SidePot>)concurrentDictionary).Remove(item14);
                        }
                    }
                    list.Clear();
                    while (true)
                    {
                    IL_0f16:
                        Dictionary<uint, ulong> dictionary6 = CalcaluteBestHands(dictionary4);
                        bool flag = false;
                        while (true)
                        {
                        IL_0f01:
                            List<uint> list2 = new List<uint>();
                            using (Dictionary<uint, ulong>.Enumerator enumerator6 = dictionary6.GetEnumerator())
                            {
                                while (enumerator6.MoveNext())
                                {
                                    Func<Role.Instance.Poker.Player, bool> func2 = null;
                                    KeyValuePair<uint, ulong> p3 = enumerator6.Current;
                                    IEnumerable<Role.Instance.Poker.Player> source2 = enumerable;
                                    func2 = delegate(Role.Instance.Poker.Player x)
                                    {
                                        uint uid = x.Uid;
                                        KeyValuePair<uint, ulong> keyValuePair3 = p3;
                                        return uid == keyValuePair3.Key;
                                    };
                                    Role.Instance.Poker.Player player = source2.Where(func2).FirstOrDefault();
                                    if (player != null)
                                    {
                                        foreach (KeyValuePair<uint, Role.Instance.Poker.SidePot> item15 in concurrentDictionary)
                                        {
                                            long num5 = 0L;
                                            foreach (uint player3 in item15.Value.Players)
                                            {
                                                if (dictionary6.ContainsKey(player3))
                                                {
                                                    num5++;
                                                }
                                            }
                                            if (item15.Value.Players.Contains(player.Uid) || flag)
                                            {
                                                if (!flag)
                                                {
                                                    player.Lose += item15.Value.Money / num5;
                                                }
                                                else
                                                {
                                                    player.Lose += item15.Value.Money / dictionary6.Count;
                                                }
                                                if (!list.Contains(item15.Key))
                                                {
                                                    list.Add(item15.Key);
                                                }
                                            }
                                        }
                                    }
                                    List<uint> list5 = list2;
                                    KeyValuePair<uint, ulong> keyValuePair2 = p3;
                                    if (!list5.Contains(keyValuePair2.Key))
                                    {
                                        List<uint> list6 = list2;
                                        keyValuePair2 = p3;
                                        list6.Add(keyValuePair2.Key);
                                    }
                                }
                            }
                            foreach (uint item16 in list2)
                            {
                                if (dictionary4.ContainsKey(item16))
                                {
                                    dictionary4.Remove(item16);
                                }
                            }
                            foreach (uint item17 in list)
                            {
                                if (concurrentDictionary.ContainsKey(item17))
                                {
                                    ((IDictionary<uint, Role.Instance.Poker.SidePot>)concurrentDictionary).Remove(item17);
                                }
                            }
                            if (concurrentDictionary.Count > 0)
                            {
                                foreach (Role.Instance.Poker.SidePot value4 in concurrentDictionary.Values)
                                {
                                    foreach (KeyValuePair<uint, ulong> item18 in dictionary4)
                                    {
                                        if (!value4.Players.Contains(item18.Key) || !Players.ContainsKey(item18.Key) || Players[item18.Key].Fold || num3 >= 30)
                                        {
                                            continue;
                                        }
                                        num3++;
                                        goto IL_0f16;
                                    }
                                }
                            }
                            if (concurrentDictionary.Count > 0)
                            {
                                foreach (Role.Instance.Poker.SidePot value5 in concurrentDictionary.Values)
                                {
                                    dictionary6 = CalcaluteBestHands(dictionary5);
                                    flag = true;
                                    if (dictionary6.Count < 1)
                                    {
                                        continue;
                                    }
                                    bool flag2 = false;
                                    foreach (KeyValuePair<uint, ulong> item19 in dictionary6)
                                    {
                                        if (!Players.ContainsKey(item19.Key))
                                        {
                                            flag2 = true;
                                        }
                                    }
                                    if (flag2 || num4 >= 30)
                                    {
                                        continue;
                                    }
                                    num4++;
                                    goto IL_0f01;
                                }
                            }
                            foreach (KeyValuePair<uint, ulong> item20 in dictionary5)
                            {
                                Players[item20.Key].CurrentMoney += Players[item20.Key].Lose;
                                if (Players[item20.Key].Lose >= (long)Players[item20.Key].TotalPot)
                                {
                                    Players[item20.Key].Lose = Players[item20.Key].Lose - (long)(Players[item20.Key].TotalPot * 99L / 100uL);
                                }
                                else
                                {
                                    Players[item20.Key].Lose = Players[item20.Key].Lose - (long)Players[item20.Key].TotalPot;
                                }
                            }
                            break;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TableBusy = false;
            }
            finally
            {
                TableBusy = false;
            }
        }

        internal ConcurrentDictionary<uint, Role.Instance.Poker.SidePot> CalcaluteSidePots()
        {
            uint num = 0u;
            ConcurrentDictionary<uint, Role.Instance.Poker.SidePot> concurrentDictionary = new ConcurrentDictionary<uint, Role.Instance.Poker.SidePot>();
            Dictionary<uint, ulong> dictionary = (from x in TempPlayers
                                                  orderby x.Value
                                                  select x).ToDictionary((KeyValuePair<uint, ulong> x) => x.Key, (KeyValuePair<uint, ulong> x) => x.Value);
            int num2 = 0;
            while (dictionary.Count > 0 && num2 < 30)
            {
                num2++;
                num++;
                Role.Instance.Poker.SidePot sidePot = new Role.Instance.Poker.SidePot();
                KeyValuePair<uint, ulong> keyValuePair = dictionary.FirstOrDefault();
                sidePot.Money += (long)keyValuePair.Value * (long)dictionary.Count;
                foreach (KeyValuePair<uint, ulong> item in dictionary)
                {
                    Dictionary<uint, ulong> tempPlayers;
                    uint key;
                    (tempPlayers = TempPlayers)[key = item.Key] = tempPlayers[key] - keyValuePair.Value;
                    if (TempPlayers[item.Key] == 0L)
                    {
                        TempPlayers.Remove(item.Key);
                    }
                    sidePot.Players.Add(item.Key);
                }
                if (sidePot.Players.Count > 1)
                {
                    sidePot.Money = sidePot.Money * 99L / 100L;
                }
                concurrentDictionary.TryAdd(num, sidePot);
                dictionary = new Dictionary<uint, ulong>((from x in TempPlayers
                                                          orderby x.Value
                                                          select x).ToDictionary((KeyValuePair<uint, ulong> x) => x.Key, (KeyValuePair<uint, ulong> x) => x.Value));
            }
            TempPlayers.Clear();
            return concurrentDictionary;
        }

        internal Dictionary<uint, ulong> CalcaluteBestHands(Dictionary<uint, ulong> Hands)
        {
            Dictionary<uint, ulong> dictionary = new Dictionary<uint, ulong>();
            foreach (KeyValuePair<uint, ulong> Hand in Hands)
            {
                if (dictionary.Count == 0)
                {
                    dictionary.Add(Hand.Key, Hand.Value);
                }
                else if (Hand.Value > dictionary.FirstOrDefault().Value)
                {
                    dictionary.Clear();
                    dictionary.Add(Hand.Key, Hand.Value);
                }
                else if (Hand.Value == dictionary.FirstOrDefault().Value)
                {
                    dictionary.Add(Hand.Key, Hand.Value);
                }
            }
            return dictionary;
        }

        internal Dictionary<uint, Hand> CalcaluteBestHands(Dictionary<uint, Hand> Hands)
        {
            Dictionary<uint, Hand> dictionary = new Dictionary<uint, Hand>();
            foreach (KeyValuePair<uint, Hand> Hand in Hands)
            {
                if (dictionary.Count == 0)
                {
                    dictionary.Add(Hand.Key, Hand.Value);
                }
                else if (Hand.Value > dictionary.FirstOrDefault().Value)
                {
                    dictionary.Clear();
                    dictionary.Add(Hand.Key, Hand.Value);
                }
                else if (Hand.Value == dictionary.FirstOrDefault().Value)
                {
                    dictionary.Add(Hand.Key, Hand.Value);
                }
            }
            return dictionary;
        }

        public override string ToString()
        {
            string text = "";
            Role.Instance.Poker.PokerCard[] board = Board;
            foreach (Role.Instance.Poker.PokerCard pokerCard in board)
            {
                if (pokerCard != null)
                {
                    text = (string.IsNullOrEmpty(text) ? pokerCard.ToString() : (text + " " + pokerCard));
                }
            }
            return text;
        }
    }
}
