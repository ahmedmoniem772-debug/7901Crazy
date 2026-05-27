using System;
using HoldemHand;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace GameServer.Role.Instance
{
    public class PokerTable
    {
        public Kick Kick;
        public List<uint> ToSend = new List<uint>();
        public uint BigBlind;
        internal PokerCard[] Board = new PokerCard[5];
        internal List<PokerCard> Cards;
        public uint CurrentPlayer;
        public bool OMAHA;
        public uint Dealer;
        public object dr;
        public ushort MapId = 3053;
        public ConcurrentDictionary<uint, uint> OnScreen;
        public ConcurrentDictionary<uint, Player> Players;
        private Dictionary<uint, ulong> TempPlayers;
        public ConcurrentDictionary<uint, Player> Watchers;
        public Player PreviousPlayer;
        public ulong RequiredPot;
        public ulong RoundPot;
        public uint SmallBlind;
        public bool TableIsChange;
        public object TableSyncRoot;
        public Enums.TableType TableType;
        public DateTime ThreadTime;
        public DateTime Time;
        public byte RoundState;
        public Enums.TableState PreviousState;
        public byte NumberOfRaise = 0;
        public bool Showhand = false;
        public long ShowhandTotalPot = 0;
        public bool TableBusy = false;
        public uint Id;
        public ushort X;
        public ushort Y;
        internal uint Mesh;
        public uint Number;
        public bool UnLimited;
        public bool IsCPs;
        public uint MinBet;
        public Enums.TableState State;
        public ulong TotalPot;
        public long LowestMoney
        {
            get
            {

                long M = 0;
                if (Showhand == false)
                {
                    foreach (var P in Players.Values)
                    {
                        if (P.IsPlaying)
                        {
                            if (!P.Fold)
                            {
                                if (M == 0)
                                {
                                    M = P.CurrentMoney;
                                }
                                else
                                {
                                    if (P.CurrentMoney < M)
                                    {
                                        M = P.CurrentMoney;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    M = ShowhandTotalPot;
                }
                return M;
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
            Players = new ConcurrentDictionary<uint, Player>();
            Watchers = new ConcurrentDictionary<uint, Player>();
            TempPlayers = new Dictionary<uint, ulong>();
            RoundState = 0;
            Kick = null;
            PreviousState = 0;
            RequiredPot = 0;
            NumberOfRaise = 0;
            TotalPot = 0;
            RoundPot = 0;
            CurrentPlayer = 0;
            PreviousPlayer = null;
            Board = new PokerCard[5];
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
                foreach (var p in Players.Values)
                    if (seat == p.Seat)
                        return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool AddWatcher(Player player)
        {
            try
            {
                if (player.PlayerType == Enums.PlayerType.Watcher)
                    if (!Watchers.ContainsKey(player.Uid))
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
            if (State == Enums.TableState.Unopened)
                return false;
            if (State == Enums.TableState.ShowDown)
                return false;
            return true;
        }
        public bool AddPlayer(Player player)
        {
            try
            {
                if (player.CurrentMoney < MinBet * 10)
                {
                    return false;
                }
                WatcherLeave(player);
                if (player.PlayerType == Enums.PlayerType.Player && IsSeatAvailable(player.Seat) && !Players.ContainsKey(player.Uid))
                {
                    if (TableType == Enums.TableType.TexasHoldem && Players.Count < 9 || Players.Count < 5)
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
        public bool PlayerLeave(Player player)
        {
            try
            {
                if (!IsSeatAvailable(player.Seat) && Players.ContainsKey(player.Uid) && Players.Count > 0)
                {
                    if (InGame() && player.IsPlaying)
                    {
                        TempPlayers.Add(player.Uid, player.TotalPot);
                    }
                    ((IDictionary<uint, Player>)Players).Remove(player.Uid);
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
        public bool WatcherLeave(Player player)
        {
            try
            {
                if (Watchers.ContainsKey(player.Uid))
                {
                    ((IDictionary<uint, Player>)Watchers).Remove(player.Uid);
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
        lablex:
            Cards = new List<PokerCard>();
            byte S = 0;
            if (Mesh == 7247567 || Mesh == 7255787 || TableType == Enums.TableType.ShowHand)
            {
                S = 5;
            }
            for (byte y = 0; y < 4; y++)
            {
                var T = Enums.CardsType.Heart;
                if (y == 1)
                    T = Enums.CardsType.Spade;
                if (y == 2)
                    T = Enums.CardsType.Club;
                if (y == 3)
                    T = Enums.CardsType.Diamond;
                for (byte x = S; x < 13; x++)
                {
                    var c = new PokerCard
                    {
                        Type = T,
                        Value = x
                    };
                    Cards.Add(c);
                }
            }
            if (S == 5)
            {
                if (Cards.Count < 32)
                {
                    goto lablex;
                }
            }
            else
            {
                if (Cards.Count < 52)
                {
                    goto lablex;
                }
            }
        }
        private PokerCard Draw()
        {
            PokerCard card = null;
            lock (dr)
            {
                var rand2 = new Random(Guid.NewGuid().GetHashCode());
                var rand = new Random(Guid.NewGuid().GetHashCode() * rand2.Next(1, 100));
                var I = rand.Next(0, Cards.Count);
                if (Cards.Count > 0)
                {
                    if (Cards.Count > I)
                    {
                        if (Cards[I] != null)
                        {
                            card = Cards[I];
                            Cards.RemoveAt(I);
                            return card;
                        }
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
            if (TableType == Enums.TableType.ShowHand)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var p in Players.Values.Where(p => p.IsPlaying))
                    {
                        if (p.Pocket[i] == null)
                        {
                            int loob = 0;
                        lablex:
                            p.Pocket[i] = Draw();
                            if (p.Pocket[i] == null && Cards.Count > 1 && loob < 30)
                            {
                                loob += 1;
                                goto lablex;
                            }
                        }
                    }
                }
            }
            else
            {

                for (int i = 0; i < count; i++)
                {
                    foreach (var p in Players.Values.Where(p => p.IsPlaying))
                    {
                        int loob = 0;
                    lablex:
                        p.Pocket[i] = Draw();
                        if (p.Pocket[i] == null && Cards.Count > 1 && loob < 30)
                        {
                            loob += 1;
                            goto lablex;
                        }
                    }
                }
            }
        }
        public void AddCardsToBoard(int count)
        {
            var Winner = Players.Values.FirstOrDefault(p => p.Winall);
            if (Winner != null)
            {
                var Card = Cards.Where(p => p.Value == Winner.Pocket[0].Value).ToArray();
                byte num = 0;
                for (byte b = 0; b < Card.Length; b++)
                {
                    Board[b] = Card[b];
                    Cards.Remove(Card[b]);
                    num++;
                }

                var Card2 = Cards.Where(p => p.Value == Winner.Pocket[1].Value).ToArray();

                for (byte b = 0; b < Card2.Length; b++)
                {
                    if (num == 5)
                        break;
                    Board[num] = Card2[b];
                    Cards.Remove(Card2[b]);
                    num++;
                }
                int PokerCard = 5 - num;
                for (byte i = 0; i < PokerCard; i++)
                {
                    if (Board[i] == null)
                    {
                        int loob = 0;
                    lablex:
                        Board[i] = Draw();
                        if (Board[i] == null && Cards.Count > 1 && loob < 30)
                        {
                            loob += 1;
                            goto lablex;
                        }
                    }
                }
            }
            else
            {
                for (byte i = 0; i < count; i++)
                {
                    if (Board[i] == null)
                    {
                        int loob = 0;
                    lablex:
                        Board[i] = Draw();
                        if (Board[i] == null && Cards.Count > 1 && loob < 30)
                        {
                            loob += 1;
                            goto lablex;
                        }
                    }
                }
            }
        }
        public void Clear()
        {
            Showhand = false;
            ShowhandTotalPot = 0;
            ShowHand = 0;
            TempPlayers.Clear();
            RoundState = 0;
            Kick = null;
            PreviousState = 0;
            RequiredPot = 0;
            NumberOfRaise = 0;
            TotalPot = 0;
            RoundPot = 0;
            CurrentPlayer = 0;
            PreviousPlayer = null;
            Board = new PokerCard[5];
            foreach (var p in Players.Values)
            {
                p.Create(p.PlayerType, p.Seat, this, (ulong)p.CurrentMoney);
            }
            ReloadCards();
        }
        public void StartNewRound()
        {
            if (TableType == Enums.TableType.TexasHoldem)
            {
                ShowHand = 0;
                ReloadCards();
                foreach (var p in Players.Values)
                {
                    p.IsPlaying = true;
                }
                if (TableIsChange)
                {
                    AddCardsToPlayers(1);
                    ReloadCards();
                }
                GetDealer();
                TotalPot = (ulong)(Players.Values.Where(p => p.IsPlaying).ToList().Count * (MinBet / 2));
                foreach (var p in Players.Values.Where(p => p.IsPlaying))
                {
                    p.CurrentMoney -= MinBet / 2;
                    p.TotalPot = MinBet / 2;
                }
                Players[SmallBlind].CurrentMoney -= MinBet / 2;
                Players[SmallBlind].RoundPot += MinBet / 2;
                Players[SmallBlind].TotalPot += MinBet / 2;
                TotalPot += MinBet / 2;
                RoundPot += MinBet / 2;

                Players[BigBlind].CurrentMoney -= MinBet;
                Players[BigBlind].RoundPot += MinBet;
                Players[BigBlind].TotalPot += MinBet;
                TotalPot += MinBet;
                RoundPot += MinBet;
                State = Enums.TableState.Pocket;
            }
            else
            {
                ReloadCards();
                foreach (var p in Players.Values)
                {
                    p.IsPlaying = true;
                }
                AddCardsToPlayers(2);
                GetDealer();
                PreviousPlayer = Players[PreviousSeat(Players[Dealer].Seat)];
                TotalPot = (ulong)(Players.Values.Where(p => p.IsPlaying).ToList().Count * (MinBet / 2));
                foreach (var p in Players.Values.Where(p => p.IsPlaying))
                {
                    p.CurrentMoney -= MinBet / 2;
                    p.TotalPot = MinBet / 2;
                }
                State = Enums.TableState.Pocket;
            }
        }
        public void StartPocket()
        {
            if (TableType == Enums.TableType.TexasHoldem)
            {
                CurrentPlayer = NextSeat(Players[BigBlind].Seat);
                if (OMAHA)
                    AddCardsToPlayers(4);
                else
                    AddCardsToPlayers(2);
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
                RequiredPot = 0;
            }
        }
        internal ushort GetRequiredAction()
        {
            if (TableType == Enums.TableType.TexasHoldem)
            {
                if (!Players.ContainsKey(CurrentPlayer))
                    return 0;
                if (!UnLimited)
                {
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet) && State == Enums.TableState.Pocket && NumberOfRaise < 3)
                        return (ushort)
                        (Enums.ActivePlayerTypes.Raise +
                         Enums.ActivePlayerTypes.Call +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet) && State == Enums.TableState.Pocket && NumberOfRaise >= 3)
                        return (ushort)
                        (Enums.ActivePlayerTypes.Call +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot + MinBet) && State != Enums.TableState.Pocket)
                        return (ushort)
                        (Enums.ActivePlayerTypes.Call +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot + 10) &&
                        ((ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Call +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet * 2) && (RoundPot > 0))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Raise +
                         Enums.ActivePlayerTypes.Check +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet * 2) && (RoundPot > 0))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Check +
                         Enums.ActivePlayerTypes.Fold);
                    if (NumberOfRaise == 0)
                    {
                        if (State == Enums.TableState.Pocket || State == Enums.TableState.Flop)
                        {
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Bet +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Allin +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                        }
                        else
                        {
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet * 2) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Bet +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet * 2) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Allin +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                        }
                    }
                    else if (NumberOfRaise > 0)
                    {
                        if (State == Enums.TableState.Pocket)
                        {
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Bet +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Allin +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                        }
                        else
                        {
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet * 2) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Bet +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                            if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet * 2) && (RoundPot == 0))
                                return (ushort)
                                (Enums.ActivePlayerTypes.Allin +
                                 Enums.ActivePlayerTypes.Check +
                                 Enums.ActivePlayerTypes.Fold);
                        }
                    }
                    Console.WriteLine("Unhandle RequiredBet: " + RequiredPot);
                    return 0;
                }
                else
                {
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney > PreviousPlayer.RoundPot + RequiredPot))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Raise +
                         Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Call +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney <= PreviousPlayer.RoundPot + RequiredPot) &&
                        ((ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Call +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet) && (RoundPot > 0))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Raise +
                         Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Check +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet) && (RoundPot > 0))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Check +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet) && (RoundPot == 0))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Bet +
                         Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Check +
                         Enums.ActivePlayerTypes.Fold);
                    if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet) && (RoundPot == 0))
                        return (ushort)
                        (Enums.ActivePlayerTypes.Allin +
                         Enums.ActivePlayerTypes.Check +
                         Enums.ActivePlayerTypes.Fold);
                    Console.WriteLine("Unhandle RequiredBet: " + RequiredPot);

                }
            }
            else
            {
                if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney > PreviousPlayer.RoundPot + RequiredPot) && Showhand == false)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Raise +
                     Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Call +
                     Enums.ActivePlayerTypes.Fold);
                if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney <= PreviousPlayer.RoundPot + RequiredPot) &&
                    ((ulong)Players[CurrentPlayer].CurrentMoney > RequiredPot) && Showhand == false)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Call +
                     Enums.ActivePlayerTypes.Fold);
                if ((RequiredPot > 0) && ((ulong)Players[CurrentPlayer].CurrentMoney <= RequiredPot) && Showhand == false)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Fold);
                if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet) && (RoundPot > 0) && Showhand == false)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Raise +
                     Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Check +
                     Enums.ActivePlayerTypes.Fold);
                if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet) && (RoundPot > 0) && Showhand == false)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Check +
                     Enums.ActivePlayerTypes.Fold);
                if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney > MinBet) && (RoundPot == 0) && Showhand == false)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Bet +
                     Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Check +
                     Enums.ActivePlayerTypes.Fold);
                if ((RequiredPot == 0) && (Players[CurrentPlayer].CurrentMoney <= MinBet) && (RoundPot == 0) && Showhand == false)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Check +
                     Enums.ActivePlayerTypes.Fold);
                if (Showhand == true)
                    return (ushort)
                    (Enums.ActivePlayerTypes.Allin +
                     Enums.ActivePlayerTypes.Fold);
                Console.WriteLine("Unhandle RequiredBet: " + RequiredPot);
            }
            return 0;
        }
        private byte CheckRound()
        {
            ulong hight = 0;
            var equil = true;
            var allPot = true;
            foreach (var p in Players.Values.Where(p => p.IsPlaying && p.IsPotAllin == false && p.Fold == false))
            {
                if (hight == 0)
                    hight = p.RoundPot;
                if (hight != p.RoundPot)
                {
                    equil = false;
                    break;
                }
                if (p.PotinThisRound == false)
                {
                    allPot = false;
                    break;
                }
            }
            if (Players.Values.Where(p => p.IsPlaying && p.Fold == false).ToList().Count == 1)
                return 3;//End AllFold
            if (allPot && equil && Players.Values.Where(p => p.IsPlaying && p.IsPotAllin == false && p.Fold == false).ToList().Count == 0)
                return 1; //End Allin
            if (allPot && equil && Players.Values.Where(p => p.IsPlaying && p.IsPotAllin == false && p.Fold == false).ToList().Count == 1 && RequiredPot == 0)
                return 1; //End Allin
            if (allPot && equil && Players.Values.Where(p => p.IsPlaying && p.IsPotAllin == false && p.Fold == false).ToList().Count > 1 && RequiredPot == 0)
                return 2; //NextRound
            return 0;
        }
        public bool Next(bool c = false)
        {
            if (TableType == Enums.TableType.TexasHoldem)
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
                if (State == Enums.TableState.ShowDown)
                {
                    return false;
                }
                return true;
            }
            else
            {
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
                if (State == Enums.TableState.ShowDown)
                {
                    return false;
                }
                return true;
            }
        }
        public void NextRound()
        {
            if (TableType == Enums.TableType.TexasHoldem)
            {

                var check = CheckRound();
                if (check == 1 || check == 3)
                {
                    AddCardsToBoard(5);
                    if (check == 1)
                    {
                        PreviousState = State;
                    }
                    RoundPot = 0;
                    CurrentPlayer = 0;
                    PreviousPlayer = null;
                    RoundState = 0;
                    State = Enums.TableState.ShowDown;

                }
                else if (check == 2)
                {
                    if (State + 1 == Enums.TableState.Flop)
                        AddCardsToBoard(3);
                    if (State + 1 == Enums.TableState.Turn)
                        AddCardsToBoard(4);
                    if (State + 1 == Enums.TableState.River)
                        AddCardsToBoard(5);
                    foreach (var p in Players.Values.Where(p => p.IsPlaying))
                    {
                        p.PotinThisRound = false;
                        p.RoundPot = 0;
                    }
                    CurrentPlayer = SmallBlind;
                    RoundPot = 0;
                    RoundState = 0;
                    State += 1;
                    if (State == Enums.TableState.ShowDown)
                    {
                        CurrentPlayer = 0;
                        PreviousPlayer = null;
                    }
                }
            }
            else
            {

                var check = CheckRound();
                if (check == 1 || check == 3)
                {
                    AddCardsToPlayers(5);
                    if (check == 1)
                    {
                        PreviousState = State;
                    }
                    RoundPot = 0;
                    CurrentPlayer = 0;
                    PreviousPlayer = null;
                    RoundState = 0;
                    GetDealer();
                    State = Enums.TableState.ShowDown;

                }
                else if (check == 2)
                {
                    if (State + 1 == Enums.TableState.Flop)
                        AddCardsToPlayers(3);
                    if (State + 1 == Enums.TableState.Turn)
                        AddCardsToPlayers(4);
                    if (State + 1 == Enums.TableState.River)
                        AddCardsToPlayers(5);
                    foreach (var p in Players.Values.Where(p => p.IsPlaying))
                    {
                        p.PotinThisRound = false;
                        p.RoundPot = 0;
                    }
                    GetDealer();
                    CurrentPlayer = Dealer;
                    RoundPot = 0;
                    RoundState = 0;
                    State += 1;
                    if (State == Enums.TableState.ShowDown)
                    {
                        CurrentPlayer = 0;
                        PreviousPlayer = null;
                    }
                }
            }
            if (Players.ContainsKey(CurrentPlayer))
                Players[CurrentPlayer].TimeAllin = Time32.Now.AddSeconds(3);
        }
        internal void GetDealer()
        {
            if (TableType == Enums.TableType.TexasHoldem)
            {
                if (TableIsChange)
                    Dealer = 0;
                if (Dealer == 0)
                {
                    foreach (var p in Players.Values.Where(p => p.IsPlaying))
                    {
                        if (Dealer == 0)
                        {
                            Dealer = p.Uid;
                        }
                        else
                        {
                            if (p.Pocket[0] > Players[Dealer].Pocket[0])
                                Dealer = p.Uid;
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
                CurrentPlayer = 0;
            }
            else
            {
                Dealer = 0;
                if (Dealer == 0)
                {
                    foreach (var p in Players.Values.Where(p => p.IsPlaying))
                    {
                        if (Dealer == 0)
                        {
                            Dealer = p.Uid;
                        }
                        else
                        {
                            int count1 = 0;
                            int count2 = 0;
                            string hand1 = "";
                            string hand2 = "";
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
                            foreach (var Card in Players[Dealer].Pocket)
                            {
                                if (Players[Dealer].Pocket[0] != null)
                                {
                                    if (Players[Dealer].Pocket[0] != Card)
                                    {
                                        if (Card != null)
                                        {
                                            hand2 += " " + Card.ToString();
                                            count2 += 1;
                                        }
                                    }
                                }
                            }
                            ulong playerMask1 = Hand.ParseHand(hand1);
                            ulong playerMask2 = Hand.ParseHand(hand2);
                            var CompirsHand1 = Hand.Evaluate(playerMask1, count1);
                            var CompirsHand2 = Hand.Evaluate(playerMask2, count2);
                            var X1 = Hand.DescriptionFromHandValueInternal(CompirsHand1);
                            var X2 = Hand.DescriptionFromHandValueInternal(CompirsHand2);
                            if (CompirsHand1 > CompirsHand2)
                                Dealer = p.Uid;
                        }
                    }
                }
                CurrentPlayer = 0;
            }
        }
        public uint PreviousSeat(int seat)
        {
            var List = Players.Values.Where(p => p.IsPlaying && p.IsPotAllin == false && p.Fold == false).ToList();
            if (List.Count == 0)
                return 0;
            if (List.Count == 1)
                return List.FirstOrDefault().Uid;
            uint uid = 0;
            int I = 0;
            while (uid == 0 && I < 20)
            {
                I++;
                seat -= 1;
                if (seat < 0)
                    seat = 4;
                var p = List.Where(x => x.Seat == seat).FirstOrDefault();
                if (p != null)
                    uid = p.Uid;
            }
            return uid;
        }
        public uint NextSeat(byte seat)
        {
            if (TableType == Enums.TableType.TexasHoldem)
            {
                var List = Players.Values.Where(p => p.IsPlaying && p.IsPotAllin == false && p.Fold == false).ToList();
                if (List.Count == 0)
                    return 0;
                if (List.Count == 1)
                    return List.FirstOrDefault().Uid;
                uint uid = 0;
                int I = 0;
                while (uid == 0 && I < 20)
                {
                    I++;
                    seat += 1;
                    if (seat > 8)
                        seat = 0;
                    var p = List.Where(x => x.Seat == seat).FirstOrDefault();
                    if (p != null)
                        uid = p.Uid;
                }
                return uid;
            }
            else
            {
                var List = Players.Values.Where(p => p.IsPlaying && p.IsPotAllin == false && p.Fold == false).ToList();
                if (State == Enums.TableState.ShowDown)
                {
                    List = Players.Values.Where(p => p.IsPlaying && p.Fold == false).ToList();
                }
                if (List.Count == 0)
                    return 0;
                if (List.Count == 1)
                    return List.FirstOrDefault().Uid;
                uint uid = 0;
                int I = 0;
                while (uid == 0 && I < 20)
                {
                    I++;
                    seat += 1;
                    if (seat > 5)
                        seat = 0;
                    var p = List.Where(x => x.Seat == seat).FirstOrDefault();
                    if (p != null)
                        uid = p.Uid;
                }
                return uid;
            }
        }
        public bool HighestBet(uint uid, ulong bet)
        {
            var high = true;
            foreach (var p in Players.Values.Where(p => p.IsPlaying))
            {
                if (p.Uid != uid)
                {
                    if (p.RoundPot > bet)
                    {
                        high = false;
                        break;
                    }
                }
            }
            foreach (var p in TempPlayers)
            {
                if (p.Value > Players[uid].TotalPot)
                {
                    high = false;
                    break;
                }
            }
            return high;
        }
        public IEnumerable<Player> PlayersOnTable()
        {
            var list = new List<Player>();
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
                if (TableType == Enums.TableType.TexasHoldem)
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
                                hand = p.FourCard(ToString());
                            }
                            else
                            {
                                hand = new Hand(p.ToString(), ToString());
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
                                    Players[p.Key].CurrentMoney += pot.Value.Money;
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
                                        player.Lose += pot.Value.Money / div;
                                    }
                                    else
                                    {
                                        player.Lose += pot.Value.Money / X.Count;
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
                        Players[p.Key].CurrentMoney += Players[p.Key].Lose;
                        if (Players[p.Key].CurrentMoney < 0)
                        {
                            Players[p.Key].CurrentMoney = 0;
                        }
                        if (Players[p.Key].Lose >= (long)Players[p.Key].TotalPot)
                        {
                            Players[p.Key].Lose = Players[p.Key].Lose - (long)((Players[p.Key].TotalPot * 99) / 100);
                        }
                        else
                        {
                            Players[p.Key].Lose = Players[p.Key].Lose - (long)(Players[p.Key].TotalPot);
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
                                    Players[p.Key].CurrentMoney += pot.Value.Money;
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
                                        player.Lose += pot.Value.Money / div;
                                    }
                                    else
                                    {
                                        player.Lose += pot.Value.Money / X.Count;
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
                        Players[p.Key].CurrentMoney += Players[p.Key].Lose;
                        if (Players[p.Key].Lose >= (long)Players[p.Key].TotalPot)
                        {
                            Players[p.Key].Lose = Players[p.Key].Lose - (long)((Players[p.Key].TotalPot * 99) / 100);
                        }
                        else
                        {
                            Players[p.Key].Lose = Players[p.Key].Lose - (long)(Players[p.Key].TotalPot);
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
        internal ConcurrentDictionary<uint, SidePot> CalcaluteSidePots()
        {
            uint count = 0;
            var dic = new ConcurrentDictionary<uint, SidePot>();
            var s2 = TempPlayers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            int I = 0;
            while (s2.Count > 0 && I < 30)
            {
                I++;
                count += 1;
                var SidePot = new SidePot();
                var Lowest = s2.FirstOrDefault();
                SidePot.Money += ((long)Lowest.Value * s2.Count);
                foreach (var p in s2)
                {
                    TempPlayers[p.Key] -= Lowest.Value;
                    if (TempPlayers[p.Key] == 0)
                    {
                        TempPlayers.Remove(p.Key);
                    }
                    SidePot.Players.Add(p.Key);
                }
                if (SidePot.Players.Count > 1)
                {
                    SidePot.Money = SidePot.Money * 99 / 100;
                }
                dic.TryAdd(count, SidePot);
                s2 = new Dictionary<uint, ulong>(TempPlayers.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value));
            }
            TempPlayers.Clear();
            return dic;
        }
        internal Dictionary<uint, ulong> CalcaluteBestHands(Dictionary<uint, ulong> Hands)
        {
            var dic = new Dictionary<uint, ulong>();
            foreach (var p in Hands)
            {
                if (dic.Count == 0)
                {
                    dic.Add(p.Key, p.Value);
                }
                else if (p.Value > dic.FirstOrDefault().Value)
                {
                    dic.Clear();
                    dic.Add(p.Key, p.Value);
                }
                else if (p.Value == dic.FirstOrDefault().Value)
                {

                    dic.Add(p.Key, p.Value);

                }
            }
            return dic;
        }
        internal Dictionary<uint, Hand> CalcaluteBestHands(Dictionary<uint, Hand> Hands)
        {
            var dic = new Dictionary<uint, Hand>();
            foreach (var p in Hands)
            {
                if (dic.Count == 0)
                {
                    dic.Add(p.Key, p.Value);
                }
                else if (p.Value > dic.FirstOrDefault().Value)
                {
                    dic.Clear();
                    dic.Add(p.Key, p.Value);
                }
                else if (p.Value == dic.FirstOrDefault().Value)
                {

                    dic.Add(p.Key, p.Value);

                }
            }
            return dic;
        }
        public override string ToString()
        {
            var board = "";
            foreach (var c in Board)
                if (c != null)
                    if (!string.IsNullOrEmpty(board))
                        board += " " + c;
                    else
                        board = c.ToString();
            return board;
        }
    }
}