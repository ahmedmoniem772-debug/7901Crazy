using HoldemHand;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public class PokerTable
    {
        public Poker.Kick Kick;
        public List<uint> ToSend = new List<uint>();
        public uint BigBlind;
        internal Poker.PokerCard[] Board = new Poker.PokerCard[5];
        internal List<Poker.PokerCard> Cards;
        public uint CurrentPlayer;
        public bool OMAHA;
        public uint Dealer;
        public object dr;
        public ushort MapId = 1858;
        public ConcurrentDictionary<uint, uint> OnScreen;
        public ConcurrentDictionary<uint, Poker.Player> Players;
        private Dictionary<uint, ulong> TempPlayers;
        public ConcurrentDictionary<uint, Poker.Player> Watchers;
        public Poker.Player PreviousPlayer;
        public ulong RequiredPot;
        public ulong RoundPot;
        public uint SmallBlind;
        public bool TableIsChange;
        public object TableSyncRoot;
        public General.TableType TableType;
        public DateTime ThreadTime;
        public DateTime Time;
        public byte RoundState;
        public General.TableState PreviousState;
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
        public General.TableState State;
        public ulong TotalPot;
        public long LowestMoney
        {
            get
            {
                long num = 0L;
                if (!Showhand)
                {
                    foreach (Poker.Player value in Players.Values)
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
            get;
            set;
        }
        public PokerTable(uint id)
        {
            TableSyncRoot = new object();
            Id = id;
            Players = new ConcurrentDictionary<uint, Poker.Player>();
            Watchers = new ConcurrentDictionary<uint, Poker.Player>();
            TempPlayers = new Dictionary<uint, ulong>();
            RoundState = 0;
            Kick = null;
            PreviousState = General.TableState.Unopened;
            RequiredPot = 0uL;
            NumberOfRaise = 0;
            TotalPot = 0uL;
            RoundPot = 0uL;
            CurrentPlayer = 0u;
            PreviousPlayer = null;
            Board = new Poker.PokerCard[5];
            ReloadCards();
            dr = new object();
        }
        public bool CanInterface(uint UID)
        {
            if (OnScreen.ContainsKey(UID))
            {
                return true;
            }
            return false;
        }
        public bool IsSeatAvailable(byte seat)
        {
            try
            {
                foreach (Poker.Player value in Players.Values)
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
        public bool AddWatcher(Poker.Player player)
        {
            try
            {
                if (player.PlayerType == General.PlayerType.Watcher && !Watchers.ContainsKey(player.Uid))
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
            if (State == General.TableState.Unopened)
            {
                return false;
            }
            if (State == General.TableState.ShowDown)
            {
                return false;
            }
            return true;
        }
        public bool AddPlayer(Poker.Player player)
        {
            try
            {
                if (player.CurrentMoney < MinBet * 10)
                {
                    return false;
                }
                WatcherLeave(player);
                if (player.PlayerType == General.PlayerType.Player && IsSeatAvailable(player.Seat) && !Players.ContainsKey(player.Uid))
                {
                    if (TableType == General.TableType.TexasHoldem && Players.Count < 9 || Players.Count < 5)
                    {
                        Players.TryAdd(player.Uid, player);
                        TableIsChange = true;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

     

        public bool PlayerLeave(Poker.Player player)
        {
            try
            {
                if (!IsSeatAvailable(player.Seat) && Players.ContainsKey(player.Uid) && Players.Count > 0)
                {
                    if (InGame() && player.IsPlaying)
                    {
                        TempPlayers.Add(player.Uid, player.TotalPot);
                    }
                    ((IDictionary<uint, Poker.Player>)Players).Remove(player.Uid);
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
        public bool WatcherLeave(Poker.Player player)
        {
            try
            {
                if (Watchers.ContainsKey(player.Uid))
                {
                    ((IDictionary<uint, Poker.Player>)Watchers).Remove(player.Uid);
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
        private void ReloadCards()
        {
            while (true)
            {
                Cards = new List<Poker.PokerCard>();
                byte b = 0;
                if (Mesh == 7247567 || Mesh == 7255787 || TableType == General.TableType.ShowHand)
                {
                    b = 5;
                }
                for (byte b2 = 0; b2 < 4; b2 = (byte)(b2 + 1))
                {
                    General.CardsType type = General.CardsType.Heart;
                    if (b2 == 1)
                    {
                        type = General.CardsType.Spade;
                    }
                    if (b2 == 2)
                    {
                        type = General.CardsType.Club;
                    }
                    if (b2 == 3)
                    {
                        type = General.CardsType.Diamond;
                    }
                    for (byte b3 = b; b3 < 13; b3 = (byte)(b3 + 1))
                    {
                        Poker.PokerCard pokerCard = new Poker.PokerCard();
                        pokerCard.Type = type;
                        pokerCard.Value = b3;
                        Poker.PokerCard item = pokerCard;
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
        private Poker.PokerCard Draw()
        {
            Poker.PokerCard pokerCard = null;
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
        private void AddCardsToPlayers(int count)
        {
            if (TableType == General.TableType.ShowHand)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (Poker.Player item in from p in Players.Values
                                                                where p.IsPlaying
                                                                select p)
                    {
                        if (item.Pocket[i] == null)
                        {
                            int num = 0;
                            while (true)
                            {
                                item.Pocket[i] = Draw();
                                if (item.Pocket[i] != null || Cards.Count <= 1 || num >= 30)
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
                    foreach (Poker.Player item2 in from p in Players.Values
                                                                 where p.IsPlaying
                                                                 select p)
                    {
                        int num = 0;
                        while (true)
                        {
                            item2.Pocket[i] = Draw();
                            if (item2.Pocket[i] != null || Cards.Count <= 1 || num >= 30)
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
            for (byte b = 0; b < count; b = (byte)(b + 1))
            {
                if (Board[b] == null)
                {
                    int num = 0;
                    while (true)
                    {
                        Board[b] = Draw();
                        if (Board[b] != null || Cards.Count <= 1 || num >= 30)
                        {
                            break;
                        }
                        num++;
                    }
                }
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
            PreviousState = General.TableState.Unopened;
            RequiredPot = 0uL;
            NumberOfRaise = 0;
            TotalPot = 0uL;
            RoundPot = 0uL;
            CurrentPlayer = 0u;
            PreviousPlayer = null;
            Board = new Poker.PokerCard[5];
            foreach (Poker.Player value in Players.Values)
            {
                value.Create(value.PlayerType, value.Seat, this, (ulong)value.CurrentMoney);
            }
            ReloadCards();
        }
        public void StartNewRound()
        {
            if (TableType == General.TableType.TexasHoldem)
            {
                ShowHand = 0;
                ReloadCards();
                foreach (Poker.Player value in Players.Values)
                {
                    value.IsPlaying = true;
                }
                if (TableIsChange)
                {
                    AddCardsToPlayers(1);
                    ReloadCards();
                }
                GetDealer();
                TotalPot = (ulong)((from p in Players.Values
                                    where p.IsPlaying
                                    select p).ToList().Count * (MinBet / 2u));
                foreach (Poker.Player item in from p in Players.Values
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
                State = General.TableState.Pocket;
            }
            else
            {
                ReloadCards();
                foreach (Poker.Player value2 in Players.Values)
                {
                    value2.IsPlaying = true;
                }
                AddCardsToPlayers(2);
                GetDealer();
                PreviousPlayer = Players[PreviousSeat(Players[Dealer].Seat)];
                TotalPot = (ulong)((from p in Players.Values
                                    where p.IsPlaying
                                    select p).ToList().Count * (MinBet / 2u));
                foreach (Poker.Player item2 in from p in Players.Values
                                                             where p.IsPlaying
                                                             select p)
                {
                    item2.CurrentMoney -= MinBet / 2u;
                    item2.TotalPot = MinBet / 2u;
                }
                State = General.TableState.Pocket;
            }
        }
        public void StartPocket()
        {
            if (TableType == General.TableType.TexasHoldem)
            {
                CurrentPlayer = NextSeat(Players[BigBlind].Seat);
                if (this.OMAHA)
                {
                    AddCardsToPlayers(4);
                }
                else
                {
                    AddCardsToPlayers(2);
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
            if (TableType == General.TableType.TexasHoldem)
            {
                if (!Players.ContainsKey(CurrentPlayer))
                {
                    return 0;
                }
                if (!UnLimited)
                {
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet && State == General.TableState.Pocket && NumberOfRaise < 3)
                    {
                        return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet && State == General.TableState.Pocket && NumberOfRaise >= 3)
                    {
                        return (ushort)(General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
                    }
                    if (RequiredPot > 0L && (ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet && State != General.TableState.Pocket)
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
                        if (State == General.TableState.Pocket || State == General.TableState.Flop)
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
                        if (State == General.TableState.Pocket)
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
            foreach (Poker.Player item in from p in Players.Values
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
            if (TableType == General.TableType.TexasHoldem)
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
                if (State == General.TableState.ShowDown)
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
            if (State == General.TableState.ShowDown)
            {
                return false;
            }
            return true;
        }
        public void NextRound()
        {
            if (TableType == General.TableType.TexasHoldem)
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
                    State = General.TableState.ShowDown;
                }
                else if (b == 2)
                {
                    if (State + 1 == General.TableState.Flop)
                    {
                        AddCardsToBoard(3);
                    }
                    if (State + 1 == General.TableState.Turn)
                    {
                        AddCardsToBoard(4);
                    }
                    if (State + 1 == General.TableState.River)
                    {
                        AddCardsToBoard(5);
                    }
                    foreach (Poker.Player item in from p in Players.Values
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
                    if (State == General.TableState.ShowDown)
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
                    State = General.TableState.ShowDown;
                }
                else if (b == 2)
                {
                    if (State + 1 == General.TableState.Flop)
                    {
                        AddCardsToPlayers(3);
                    }
                    if (State + 1 == General.TableState.Turn)
                    {
                        AddCardsToPlayers(4);
                    }
                    if (State + 1 == General.TableState.River)
                    {
                        AddCardsToPlayers(5);
                    }
                    foreach (Poker.Player item2 in from p in Players.Values
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
                    if (State == General.TableState.ShowDown)
                    {
                        CurrentPlayer = 0u;
                        PreviousPlayer = null;
                    }
                }
            }
        }
        internal void GetDealer()
        {
            if (TableType == General.TableType.TexasHoldem)
            {
                if (TableIsChange)
                {
                    Dealer = 0u;
                }
                if (Dealer == 0)
                {
                    foreach (Poker.Player item in from p in Players.Values
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
                    foreach (Poker.Player item2 in from p in Players.Values
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
                            Poker.PokerCard[] pocket = item2.Pocket;
                            foreach (Poker.PokerCard pokerCard in pocket)
                            {
                                if (item2.Pocket[0] != null && item2.Pocket[0] != pokerCard && pokerCard != null)
                                {
                                    text = text + " " + pokerCard.ToString();
                                    num++;
                                }
                            }
                            pocket = Players[Dealer].Pocket;
                            foreach (Poker.PokerCard pokerCard in pocket)
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
            List<Poker.Player> list = (from p in Players.Values
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
                Poker.Player player = (from x in list
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
            List<Poker.Player> list;
            uint num;
            int num2;
            if (TableType == General.TableType.TexasHoldem)
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
                    Poker.Player player = (from x in list
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
            if (State == General.TableState.ShowDown)
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
                Poker.Player player = (from x in list
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
            foreach (Poker.Player item in from p in Players.Values
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
        public IEnumerable<Poker.Player> PlayersOnTable()
        {
            List<Poker.Player> list = new List<Poker.Player>();
            list.AddRange(Players.Values);
            list.AddRange(Watchers.Values);
            return list;
        }
        public void GetWinners()
        {
            int loob1 = 0;
            int loob2 = 0;
            int loob3 = 0;
            int loob4 = 0;
            try
            {
                if (TableType == Poker.General.TableType.TexasHoldem)
                {

                    TableBusy = true;
                    var xPlayer = Players.Values.Where(p => p.IsPlaying);
                    var Hands = new Dictionary<uint, Hand>();
                    var Winners = new Dictionary<uint, Hand>();
                    foreach (var p in xPlayer)
                    {
                        if (TempPlayers.ContainsKey(p.Uid))
                        {
                            TempPlayers[p.Uid] = p.TotalPot;
                        }
                        else
                        {
                            TempPlayers.Add(p.Uid, p.TotalPot);
                        }
                        if (p.Fold)
                        {
                            p.Lose -= (long)p.TotalPot;
                        }
                        if (!p.Fold)
                        {
                            Hand hand;
                            if (this.OMAHA)
                            {
                                hand = p.FourCard(this.ToString());
                            }
                            else
                            {
                                hand = new Hand(p.ToString(), this.ToString());
                            }
                            Winners.Add(p.Uid, hand);
                            Hands.Add(p.Uid, hand);
                        }
                    }
                    var Pots = CalcaluteSidePots();
                    var RemovesPot = new List<uint>();
                    foreach (var pot in Pots)
                    {
                        foreach (var p in Hands)
                        {
                            if (pot.Value.Players.Count == 1)
                            {
                                if (pot.Value.Players.Contains(p.Key))
                                {
                                    Players[p.Key].CurrentMoney += (long)pot.Value.Money;
                                    Players[p.Key].TotalPot -= (ulong)pot.Value.Money;
                                    if (!RemovesPot.Contains(pot.Key))
                                    {
                                        RemovesPot.Add(pot.Key);
                                    }
                                }
                            }
                        }
                    }
                    foreach (var I in RemovesPot)
                    {
                        if (Pots.ContainsKey(I))
                        {
                            ((IDictionary<uint, SidePot>)Pots).Remove(I);
                        }
                    }
                    RemovesPot.Clear();
                Lable_X:
                    var X = CalcaluteBestHands(Hands);
                    bool Exp = false;
                Lable_X1:
                    var RemovesWinner = new List<uint>();
                    foreach (var p in X)
                    {
                        var player = xPlayer.Where(x => x.Uid == p.Key).FirstOrDefault();
                        if (player != null)
                        {
                            foreach (var pot in Pots)
                            {
                                long div = 0;
                                foreach (var W in pot.Value.Players)
                                {
                                    if (X.ContainsKey(W))
                                    {
                                        div += 1;
                                    }
                                }
                                if (pot.Value.Players.Contains(player.Uid) || Exp)
                                {
                                    if (Exp == false)
                                    {
                                        player.Lose += (long)pot.Value.Money / (long)div;
                                    }
                                    else
                                    {
                                        player.Lose += (long)pot.Value.Money / (long)X.Count;
                                    }
                                    if (!RemovesPot.Contains(pot.Key))
                                    {
                                        RemovesPot.Add(pot.Key);
                                    }
                                }
                            }
                        }
                        if (!RemovesWinner.Contains(p.Key))
                        {
                            RemovesWinner.Add(p.Key);
                        }

                    }
                    foreach (var I in RemovesWinner)
                    {
                        if (Hands.ContainsKey(I))
                        {
                            Hands.Remove(I);
                        }
                    }
                    foreach (var I in RemovesPot)
                    {
                        if (Pots.ContainsKey(I))
                        {
                            ((IDictionary<uint, SidePot>)Pots).Remove(I);
                        }
                    }
                    if (Pots.Count > 0)
                    {
                        foreach (var pot in Pots.Values)
                        {
                            foreach (var p in Hands)
                            {
                                if (pot.Players.Contains(p.Key))
                                {
                                    if (Players.ContainsKey(p.Key))
                                    {
                                        if (Players[p.Key].Fold == false)
                                        {
                                            if (loob1 < 30)
                                            {
                                                loob1++;
                                                goto Lable_X;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Pots.Count > 0)
                    {
                        foreach (var pot in Pots.Values)
                        {
                            X = CalcaluteBestHands(Winners);
                            Exp = true;
                            if (X.Count >= 1)
                            {
                                bool xxxx = false;
                                foreach (var xx in X)
                                {
                                    if (!Players.ContainsKey(xx.Key))
                                    {
                                        xxxx = true;
                                    }
                                }
                                if (xxxx == false)
                                {
                                    if (loob2 < 30)
                                    {
                                        loob2++;
                                        goto Lable_X1;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var p in Winners)
                    {
                        Players[p.Key].CurrentMoney += (long)Players[p.Key].Lose;
                        if (Players[p.Key].CurrentMoney < 0)
                        {
                            Players[p.Key].CurrentMoney = 0;
                        }
                        if (Players[p.Key].Lose >= (long)Players[p.Key].TotalPot)
                        {
                            Players[p.Key].Lose = (long)Players[p.Key].Lose - (long)((Players[p.Key].TotalPot * 99) / 100);
                        }
                        else
                        {
                            Players[p.Key].Lose = (long)Players[p.Key].Lose - (long)(Players[p.Key].TotalPot);
                        }
                    }
                }
                else
                {
                    TableBusy = true;
                    var xPlayer = Players.Values.Where(p => p.IsPlaying);
                    var Hands = new Dictionary<uint, ulong>();
                    var Winners = new Dictionary<uint, ulong>();
                    foreach (var p in xPlayer)
                    {
                        if (TempPlayers.ContainsKey(p.Uid))
                        {
                            TempPlayers[p.Uid] = p.TotalPot;
                        }
                        else
                        {
                            TempPlayers.Add(p.Uid, p.TotalPot);
                        }
                        if (p.Fold)
                        {
                            p.Lose -= (long)p.TotalPot;
                        }
                        if (!p.Fold)
                        {
                            int count1 = 0;
                            string hand1 = "";
                            foreach (var Card in p.Pocket)
                            {
                                if (p.Pocket[0] != null)
                                {
                                    if (p.Pocket[0] != Card)
                                    {
                                        if (Card != null)
                                        {
                                            hand1 += " " + Card.ToString();
                                            count1 += 1;
                                        }
                                    }
                                }
                            }
                            ulong playerMask1 = Hand.ParseHand(hand1);
                            var CompirsHand1 = Hand.Evaluate(playerMask1, count1);
                            Winners.Add(p.Uid, CompirsHand1);
                            Hands.Add(p.Uid, CompirsHand1);
                        }

                    }
                    var Pots = CalcaluteSidePots();
                    var RemovesPot = new List<uint>();
                    foreach (var pot in Pots)
                    {
                        foreach (var p in Hands)
                        {
                            if (pot.Value.Players.Count == 1)
                            {
                                if (pot.Value.Players.Contains(p.Key))
                                {
                                    Players[p.Key].CurrentMoney += (long)pot.Value.Money;
                                    Players[p.Key].TotalPot -= (ulong)pot.Value.Money;
                                    if (!RemovesPot.Contains(pot.Key))
                                    {
                                        RemovesPot.Add(pot.Key);
                                    }
                                }
                            }
                        }
                    }
                    foreach (var I in RemovesPot)
                    {
                        if (Pots.ContainsKey(I))
                        {
                            ((IDictionary<uint, SidePot>)Pots).Remove(I);
                        }
                    }
                    RemovesPot.Clear();
                Lable_X:
                    var X = CalcaluteBestHands(Hands);
                    bool Exp = false;
                Lable_X1:
                    var RemovesWinner = new List<uint>();
                    foreach (var p in X)
                    {
                        var player = xPlayer.Where(x => x.Uid == p.Key).FirstOrDefault();
                        if (player != null)
                        {
                            foreach (var pot in Pots)
                            {
                                long div = 0;
                                foreach (var W in pot.Value.Players)
                                {
                                    if (X.ContainsKey(W))
                                    {
                                        div += 1;
                                    }
                                }
                                if (pot.Value.Players.Contains(player.Uid) || Exp)
                                {
                                    if (Exp == false)
                                    {
                                        player.Lose += (long)pot.Value.Money / (long)div;
                                    }
                                    else
                                    {
                                        player.Lose += (long)pot.Value.Money / (long)X.Count;
                                    }
                                    if (!RemovesPot.Contains(pot.Key))
                                    {
                                        RemovesPot.Add(pot.Key);
                                    }
                                }
                            }
                        }
                        if (!RemovesWinner.Contains(p.Key))
                        {
                            RemovesWinner.Add(p.Key);
                        }

                    }
                    foreach (var I in RemovesWinner)
                    {
                        if (Hands.ContainsKey(I))
                        {
                            Hands.Remove(I);
                        }
                    }
                    foreach (var I in RemovesPot)
                    {
                        if (Pots.ContainsKey(I))
                        {
                            ((IDictionary<uint, SidePot>)Pots).Remove(I);
                        }
                    }
                    if (Pots.Count > 0)
                    {
                        foreach (var pot in Pots.Values)
                        {
                            foreach (var p in Hands)
                            {
                                if (pot.Players.Contains(p.Key))
                                {
                                    if (Players.ContainsKey(p.Key))
                                    {
                                        if (Players[p.Key].Fold == false)
                                        {
                                            if (loob3 < 30)
                                            {
                                                loob3++;
                                                goto Lable_X;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Pots.Count > 0)
                    {
                        foreach (var pot in Pots.Values)
                        {
                            X = CalcaluteBestHands(Winners);
                            Exp = true;
                            if (X.Count >= 1)
                            {
                                bool xxxx = false;
                                foreach (var xx in X)
                                {
                                    if (!Players.ContainsKey(xx.Key))
                                    {
                                        xxxx = true;
                                    }
                                }
                                if (xxxx == false)
                                {
                                    if (loob4 < 30)
                                    {
                                        loob4++;
                                        goto Lable_X1;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var p in Winners)
                    {
                        Players[p.Key].CurrentMoney += (long)Players[p.Key].Lose;
                        if (Players[p.Key].Lose >= (long)Players[p.Key].TotalPot)
                        {
                            Players[p.Key].Lose = (long)Players[p.Key].Lose - (long)((Players[p.Key].TotalPot * 99) / 100);
                        }
                        else
                        {
                            Players[p.Key].Lose = (long)Players[p.Key].Lose - (long)(Players[p.Key].TotalPot);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                TableBusy = false;
            }
            finally
            {
                TableBusy = false;
            }
        }
        internal ConcurrentDictionary<uint, Poker.SidePot> CalcaluteSidePots()
        {
            uint num = 0u;
            ConcurrentDictionary<uint, Poker.SidePot> concurrentDictionary = new ConcurrentDictionary<uint, Poker.SidePot>();
            Dictionary<uint, ulong> dictionary = (from x in TempPlayers
                                                  orderby x.Value
                                                  select x).ToDictionary((KeyValuePair<uint, ulong> x) => x.Key, (KeyValuePair<uint, ulong> x) => x.Value);
            int num2 = 0;
            while (dictionary.Count > 0 && num2 < 30)
            {
                num2++;
                num++;
                Poker.SidePot sidePot = new Poker.SidePot();
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
            Poker.PokerCard[] board = Board;
            foreach (Poker.PokerCard pokerCard in board)
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
