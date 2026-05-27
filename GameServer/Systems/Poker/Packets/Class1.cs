//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using ConquerOnline.Game.MsgServer;
//using ConquerOnline.Game.MsgServer.HoldemHand;
//using ConquerOnline.Role.Instance.Poker;

//namespace ConquerOnline.Role.Instance
//{
//    // Token: 0x020000F0 RID: 240
//    public class PokerTableddd
//    {
//        // Token: 0x1700002B RID: 43
//        // (get) Token: 0x06000514 RID: 1300 RVA: 0x000926C4 File Offset: 0x000908C4
//        public long LowestMoney
//        {
//            get
//            {
//                long num = 0L;
//                if (!this.Showhand)
//                {
//                    foreach (Player player in this.Players.Values)
//                    {
//                        if (player.IsPlaying && !player.Fold)
//                        {
//                            if (num == 0L)
//                            {
//                                num = player.CurrentMoney;
//                            }
//                            else if (player.CurrentMoney < num)
//                            {
//                                num = player.CurrentMoney;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    num = this.ShowhandTotalPot;
//                }
//                return num;
//            }
//        }

//        // Token: 0x1700002C RID: 44
//        // (get) Token: 0x06000515 RID: 1301 RVA: 0x0009278C File Offset: 0x0009098C
//        // (set) Token: 0x06000516 RID: 1302 RVA: 0x000927A4 File Offset: 0x000909A4
//        public int ShowHand
//        {
//            [CompilerGenerated]
//            get
//            {
//                return this.ShowhandB;
//            }
//            [CompilerGenerated]
//            set
//            {
//                this.ShowhandB = value;
//            }
//        }

//        // Token: 0x06000517 RID: 1303 RVA: 0x000927B0 File Offset: 0x000909B0
//        public PokerTable(uint id)
//        {
//            this.ToSend = new List<uint>();
//            this.Board = new PokerCard[5];
//            this.MapId = 3053;
//            this.NumberOfRaise = 0;
//            this.Showhand = false;
//            this.ShowhandTotalPot = 0L;
//            this.TableBusy = false;
//            this.TableSyncRoot = new object();
//            this.Id = id;
//            this.Players = new ConcurrentDictionary<uint, Player>();
//            this.Watchers = new ConcurrentDictionary<uint, Player>();
//            this.TempPlayers = new Dictionary<uint, ulong>();
//            this.RoundState = 0;
//            this.Kick = null;
//            this.PreviousState = Flags.TableState.Unopened;
//            this.RequiredPot = 0UL;
//            this.NumberOfRaise = 0;
//            this.TotalPot = 0UL;
//            this.RoundPot = 0UL;
//            this.CurrentPlayer = 0U;
//            this.PreviousPlayer = null;
//            this.Board = new PokerCard[5];
//            this.ReloadCards();
//            this.dr = new object();
//        }

//        // Token: 0x06000518 RID: 1304 RVA: 0x000928C0 File Offset: 0x00090AC0
//        public bool CanInterface(uint UID)
//        {
//            return this.OnScreen.ContainsKey(UID);
//        }

//        // Token: 0x06000519 RID: 1305 RVA: 0x000928E8 File Offset: 0x00090AE8
//        public bool IsSeatAvailable(byte seat)
//        {
//            bool result;
//            try
//            {
//                foreach (Player player in this.Players.Values)
//                {
//                    if (seat == player.Seat)
//                    {
//                        return false;
//                    }
//                }
//                result = true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                result = false;
//            }
//            return result;
//        }

//        // Token: 0x0600051A RID: 1306 RVA: 0x00092980 File Offset: 0x00090B80
//        public bool AddWatcher(Player player)
//        {
//            bool result;
//            try
//            {
//                if (player.PlayerType == Flags.PlayerType.Watcher && !this.Watchers.ContainsKey(player.Uid))
//                {
//                    this.Watchers.TryAdd(player.Uid, player);
//                    result = true;
//                }
//                else
//                {
//                    result = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                result = false;
//            }
//            return result;
//        }

//        // Token: 0x0600051B RID: 1307 RVA: 0x000929F4 File Offset: 0x00090BF4
//        public bool InGame()
//        {
//            return this.State != Flags.TableState.Unopened && this.State != Flags.TableState.ShowDown;
//        }

//        // Token: 0x0600051C RID: 1308 RVA: 0x00092A34 File Offset: 0x00090C34
//        public bool AddPlayer(Player player)
//        {
//            bool result;
//            try
//            {
//                if (player.CurrentMoney >= (long)((ulong)(this.MinBet * 10U)))
//                {
//                    this.WatcherLeave(player);
//                    if (player.PlayerType == Flags.PlayerType.Player && this.IsSeatAvailable(player.Seat) && !this.Players.ContainsKey(player.Uid) && ((this.TableType == Flags.TableType.TexasHoldem && this.Players.Count < 9) || this.Players.Count < 5))
//                    {
//                        this.Players.TryAdd(player.Uid, player);
//                        this.TableIsChange = true;
//                        result = true;
//                    }
//                    else
//                    {
//                        result = false;
//                    }
//                }
//                else
//                {
//                    result = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                result = false;
//            }
//            return result;
//        }

//        // Token: 0x0600051D RID: 1309 RVA: 0x00092B10 File Offset: 0x00090D10
//        public bool AddPlayerCross(Player player)
//        {
//            bool result;
//            try
//            {
//                if (player.CurrentMoney >= (long)((ulong)(this.MinBet * 10U)))
//                {
//                    this.WatcherLeave(player);
//                    if (player.PlayerType == Flags.PlayerType.CrossPoker && this.IsSeatAvailable(player.Seat) && !this.Players.ContainsKey(player.Uid) && ((this.TableType == Flags.TableType.TexasHoldem && this.Players.Count < 9) || this.Players.Count < 5))
//                    {
//                        this.Players.TryAdd(player.Uid, player);
//                        this.TableIsChange = true;
//                        result = true;
//                    }
//                    else
//                    {
//                        result = false;
//                    }
//                }
//                else
//                {
//                    result = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                result = false;
//            }
//            return result;
//        }

//        // Token: 0x0600051E RID: 1310 RVA: 0x00092BEC File Offset: 0x00090DEC
//        public bool PlayerLeave(Player player)
//        {
//            bool result;
//            try
//            {
//                if (!this.IsSeatAvailable(player.Seat) && this.Players.ContainsKey(player.Uid) && this.Players.Count > 0)
//                {
//                    if (this.InGame() && player.IsPlaying)
//                    {
//                        this.TempPlayers.Add(player.Uid, player.TotalPot);
//                    }
//                    ((IDictionary<uint, Player>)this.Players).Remove(player.Uid);
//                    this.TableIsChange = true;
//                    result = true;
//                }
//                else
//                {
//                    result = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                result = false;
//            }
//            return result;
//        }

//        // Token: 0x0600051F RID: 1311 RVA: 0x00092CB0 File Offset: 0x00090EB0
//        public bool WatcherLeave(Player player)
//        {
//            bool result;
//            try
//            {
//                if (this.Watchers.ContainsKey(player.Uid))
//                {
//                    ((IDictionary<uint, Player>)this.Watchers).Remove(player.Uid);
//                    result = true;
//                }
//                else
//                {
//                    result = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                result = false;
//            }
//            return result;
//        }

//        // Token: 0x06000520 RID: 1312 RVA: 0x00092D18 File Offset: 0x00090F18
//        private void ReloadCards()
//        {
//            for (;;)
//            {
//                this.CardNewRemove = new List<PokerCard>();
//                this.Cards = new List<PokerCard>();
//                byte b = 0;
//                if (this.Mesh == 7210797U || this.Mesh == 7247567U || this.Mesh == 7255787U || this.TableType == Flags.TableType.ShowHand)
//                {
//                    b = 5;
//                }
//                for (byte b2 = 0; b2 < 4; b2 += 1)
//                {
//                    Flags.CardsType type = Flags.CardsType.Heart;
//                    if (b2 == 1)
//                    {
//                        type = Flags.CardsType.Spade;
//                    }
//                    if (b2 == 2)
//                    {
//                        type = Flags.CardsType.Club;
//                    }
//                    if (b2 == 3)
//                    {
//                        type = Flags.CardsType.Diamond;
//                    }
//                    for (byte b3 = b; b3 < 13; b3 += 1)
//                    {
//                        PokerCard item = new PokerCard
//                        {
//                            Type = type,
//                            Value = b3
//                        };
//                        this.Cards.Add(item);
//                    }
//                }
//                if (b == 5)
//                {
//                    if (this.Cards.Count >= 32)
//                    {
//                        break;
//                    }
//                }
//                else if (this.Cards.Count >= 52)
//                {
//                    break;
//                }
//            }
//        }

//        // Token: 0x06000521 RID: 1313 RVA: 0x00092E5C File Offset: 0x0009105C
//        private PokerCard Draw()
//        {
//            PokerCard result2;
//            lock (this.dr)
//            {
//                Random random = new Random(Guid.NewGuid().GetHashCode());
//                Random random2 = new Random(Guid.NewGuid().GetHashCode() * random.Next(1, 100));
//                int num = random2.Next(0, this.Cards.Count);
//                if (this.Cards.Count > 0)
//                {
//                    if (this.Cards.Count > num)
//                    {
//                        if (this.Cards[num] != null)
//                        {
//                            PokerCard result = this.Cards[num];
//                            this.CardNewRemove.Add(this.Cards[num]);
//                            this.Cards.RemoveAt(num);
//                            return result;
//                        }
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Error In Card Draw");
//                }
//                result2 = null;
//            }
//            return result2;
//        }

//        // Token: 0x06000522 RID: 1314 RVA: 0x000933F4 File Offset: 0x000915F4
//        private PokerCard DrawWinnerBoard()
//        {
//            PokerCard result2;
//            lock (this.dr)
//            {
//                Random random = new Random(Guid.NewGuid().GetHashCode());
//                Random random2 = new Random(Guid.NewGuid().GetHashCode() * random.Next(1, 100));
//                Player player = this.Players.Values.FirstOrDefault((Player p) => p.Winall && p.WinnerBot > (Flags.PlayerTypeWinner)0);
//                if (player != null)
//                {
//                    if (((player.Pocket[0].Value == 12 && player.Pocket[1].Value == 8) || (player.Pocket[0].Value == 8 && player.Pocket[1].Value == 12)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass26 CS$<>8__locals1 = new PokerTable.<>c__DisplayClass26();
//                        this.Cards.Clear();
//                        CS$<>8__locals1.HighCards = new List<int>
//                        {
//                            10,
//                            11,
//                            9,
//                            7,
//                            5
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals1.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals1.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals1.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj2 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj2;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 12 && player.Pocket[1].Value == 10) || (player.Pocket[0].Value == 10 && player.Pocket[1].Value == 12)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass2b CS$<>8__locals3 = new PokerTable.<>c__DisplayClass2b();
//                        this.Cards.Clear();
//                        CS$<>8__locals3.HighCards = new List<int>
//                        {
//                            11,
//                            9,
//                            6,
//                            7,
//                            8
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals3.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals3.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals3.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj3 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj3;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 12 && player.Pocket[1].Value == 11) || (player.Pocket[0].Value == 11 && player.Pocket[1].Value == 12)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass30 CS$<>8__locals5 = new PokerTable.<>c__DisplayClass30();
//                        this.Cards.Clear();
//                        CS$<>8__locals5.HighCards = new List<int>
//                        {
//                            9,
//                            6,
//                            8,
//                            10,
//                            5
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals5.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals5.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals5.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj4 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj4;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 12 && player.Pocket[1].Value == 9) || (player.Pocket[0].Value == 9 && player.Pocket[1].Value == 12)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass35 CS$<>8__locals7 = new PokerTable.<>c__DisplayClass35();
//                        this.Cards.Clear();
//                        CS$<>8__locals7.HighCards = new List<int>
//                        {
//                            5,
//                            7,
//                            11,
//                            8,
//                            10
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals7.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals7.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals7.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj5 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj5;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 8 && player.Pocket[1].Value == 10) || (player.Pocket[0].Value == 10 && player.Pocket[1].Value == 8)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass3a CS$<>8__locals9 = new PokerTable.<>c__DisplayClass3a();
//                        this.Cards.Clear();
//                        CS$<>8__locals9.HighCards = new List<int>
//                        {
//                            6,
//                            7,
//                            9,
//                            11,
//                            12
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals9.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals9.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals9.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj6 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj6;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 8 && player.Pocket[1].Value == 11) || (player.Pocket[0].Value == 11 && player.Pocket[1].Value == 8)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass3f CS$<>8__locals11 = new PokerTable.<>c__DisplayClass3f();
//                        this.Cards.Clear();
//                        CS$<>8__locals11.HighCards = new List<int>
//                        {
//                            12,
//                            10,
//                            9,
//                            5,
//                            6
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals11.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals11.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals11.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj7 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj7;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 8 && player.Pocket[1].Value == 9) || (player.Pocket[0].Value == 9 && player.Pocket[1].Value == 8)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass44 CS$<>8__locals13 = new PokerTable.<>c__DisplayClass44();
//                        this.Cards.Clear();
//                        CS$<>8__locals13.HighCards = new List<int>
//                        {
//                            12,
//                            10,
//                            11,
//                            7,
//                            5
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals13.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals13.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals13.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj8 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj8;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 10 && player.Pocket[1].Value == 11) || (player.Pocket[0].Value == 11 && player.Pocket[1].Value == 10)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass49 CS$<>8__locals15 = new PokerTable.<>c__DisplayClass49();
//                        this.Cards.Clear();
//                        CS$<>8__locals15.HighCards = new List<int>
//                        {
//                            5,
//                            6,
//                            8,
//                            9,
//                            12
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals15.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals15.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals15.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj9 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj9;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 10 && player.Pocket[1].Value == 9) || (player.Pocket[0].Value == 9 && player.Pocket[1].Value == 10)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass4e CS$<>8__locals17 = new PokerTable.<>c__DisplayClass4e();
//                        this.Cards.Clear();
//                        CS$<>8__locals17.HighCards = new List<int>
//                        {
//                            5,
//                            6,
//                            7,
//                            8,
//                            11
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals17.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals17.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals17.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj10 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj10;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 11 && player.Pocket[1].Value == 9) || (player.Pocket[0].Value == 9 && player.Pocket[1].Value == 11)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.stright)
//                    {
//                        PokerTable.<>c__DisplayClass53 CS$<>8__locals19 = new PokerTable.<>c__DisplayClass53();
//                        this.Cards.Clear();
//                        CS$<>8__locals19.HighCards = new List<int>
//                        {
//                            8,
//                            8,
//                            8,
//                            7,
//                            9
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals19.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals19.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals19.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj11 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj11;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 8 && player.Pocket[1].Value == 10) || (player.Pocket[0].Value == 10 && player.Pocket[1].Value == 8)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.FourOfKind)
//                    {
//                        PokerTable.<>c__DisplayClass58 CS$<>8__locals21 = new PokerTable.<>c__DisplayClass58();
//                        this.Cards.Clear();
//                        CS$<>8__locals21.HighCards = new List<int>
//                        {
//                            10,
//                            10,
//                            10,
//                            11,
//                            7
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals21.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals21.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals21.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj12 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj12;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (((player.Pocket[0].Value == 9 && player.Pocket[1].Value == 11) || (player.Pocket[0].Value == 11 && player.Pocket[1].Value == 9)) && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.FourOfKind)
//                    {
//                        PokerTable.<>c__DisplayClass5d CS$<>8__locals23 = new PokerTable.<>c__DisplayClass5d();
//                        this.Cards.Clear();
//                        CS$<>8__locals23.HighCards = new List<int>
//                        {
//                            9,
//                            9,
//                            9,
//                            11,
//                            5
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals23.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals23.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals23.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj13 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj13;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (player.Pocket[0].Value == 12 && player.Pocket[1].Value == 12 && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.fullhouse)
//                    {
//                        PokerTable.<>c__DisplayClass62 CS$<>8__locals25 = new PokerTable.<>c__DisplayClass62();
//                        this.Cards.Clear();
//                        CS$<>8__locals25.HighCards = new List<int>
//                        {
//                            9,
//                            9,
//                            10,
//                            7,
//                            12
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals25.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals25.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals25.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj14 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj14;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (player.Pocket[0].Value == 11 && player.Pocket[1].Value == 11 && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.fullhouse)
//                    {
//                        PokerTable.<>c__DisplayClass67 CS$<>8__locals27 = new PokerTable.<>c__DisplayClass67();
//                        this.Cards.Clear();
//                        CS$<>8__locals27.HighCards = new List<int>
//                        {
//                            9,
//                            10,
//                            11,
//                            10,
//                            7
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals27.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals27.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals27.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj15 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj15;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (player.Pocket[0].Value == 10 && player.Pocket[1].Value == 10 && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.fullhouse)
//                    {
//                        PokerTable.<>c__DisplayClass6c CS$<>8__locals29 = new PokerTable.<>c__DisplayClass6c();
//                        this.Cards.Clear();
//                        CS$<>8__locals29.HighCards = new List<int>
//                        {
//                            6,
//                            5,
//                            8,
//                            10,
//                            6
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals29.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals29.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals29.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj16 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj16;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (player.Pocket[0].Value == 9 && player.Pocket[1].Value == 9 && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.fullhouse)
//                    {
//                        PokerTable.<>c__DisplayClass71 CS$<>8__locals31 = new PokerTable.<>c__DisplayClass71();
//                        this.Cards.Clear();
//                        CS$<>8__locals31.HighCards = new List<int>
//                        {
//                            9,
//                            6,
//                            7,
//                            7,
//                            8
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals31.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals31.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals31.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj17 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj17;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                    if (player.Pocket[0].Value == 8 && player.Pocket[1].Value == 8 && !player.ClearCard && player.WinnerBot == Flags.PlayerTypeWinner.fullhouse)
//                    {
//                        PokerTable.<>c__DisplayClass76 CS$<>8__locals33 = new PokerTable.<>c__DisplayClass76();
//                        this.Cards.Clear();
//                        CS$<>8__locals33.HighCards = new List<int>
//                        {
//                            8,
//                            5,
//                            5,
//                            6,
//                            7
//                        };
//                        int cardnum;
//                        for (cardnum = 0; cardnum < CS$<>8__locals33.HighCards.Count; cardnum++)
//                        {
//                            PokerCard[] array = (from p in this.CardNewRemove
//                            where (int)p.Value == CS$<>8__locals33.HighCards[cardnum]
//                            select p).ToArray<PokerCard>();
//                            PokerCard pokerCard = new PokerCard();
//                            pokerCard.Value = (byte)CS$<>8__locals33.HighCards[cardnum];
//                            List<Flags.CardsType> list = new List<Flags.CardsType>();
//                            for (int i = 0; i < array.Length; i++)
//                            {
//                                list.Add(array[i].Type);
//                            }
//                            foreach (object obj18 in Enum.GetValues(typeof(Flags.CardsType)))
//                            {
//                                Flags.CardsType cardsType = (Flags.CardsType)obj18;
//                                if (!list.Contains(cardsType))
//                                {
//                                    pokerCard.Type = cardsType;
//                                }
//                            }
//                            this.Cards.Add(pokerCard);
//                        }
//                        player.ClearCard = true;
//                    }
//                }
//                int num = random2.Next(0, this.Cards.Count);
//                if (this.Cards.Count > 0)
//                {
//                    if (this.Cards.Count > num)
//                    {
//                        if (this.Cards[num] != null)
//                        {
//                            PokerCard result = this.Cards[num];
//                            this.Cards.Remove(this.Cards[num]);
//                            return result;
//                        }
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Error In Card Draw");
//                }
//                result2 = null;
//            }
//            return result2;
//        }

//        // Token: 0x06000523 RID: 1315 RVA: 0x00095974 File Offset: 0x00093B74
//        private PokerCard DrawWinner()
//        {
//            PokerCard result2;
//            lock (this.dr)
//            {
//                Random random = new Random(Guid.NewGuid().GetHashCode());
//                Random random2 = new Random(Guid.NewGuid().GetHashCode() * random.Next(1, 100));
//                int num = random2.Next(0, this.Cards.Count);
//                PokerCard[] array = (from z in this.Cards
//                where z.Value == 6 || z.Value == 7 || z.Value == 8 || z.Value == 9 || z.Value == 10 || z.Value == 11 || z.Value == 12
//                select z).ToArray<PokerCard>();
//                if (array.Length > 0)
//                {
//                    if (array.Length > num)
//                    {
//                        if (array[num] != null)
//                        {
//                            PokerCard result = array[num];
//                            this.Cards.Remove(array[num]);
//                            return result;
//                        }
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Error In Card Draw");
//                }
//                result2 = null;
//            }
//            return result2;
//        }

//        // Token: 0x06000524 RID: 1316 RVA: 0x00095B08 File Offset: 0x00093D08
//        private void AddCardsToPlayers(int count)
//        {
//            if (this.TableType == Flags.TableType.ShowHand)
//            {
//                for (int i = 0; i < count; i++)
//                {
//                    foreach (Player player in from p in this.Players.Values
//                    where p.IsPlaying
//                    select p)
//                    {
//                        if (player.Pocket[i] == null)
//                        {
//                            int num = 0;
//                            for (;;)
//                            {
//                                player.Pocket[i] = this.Draw();
//                                if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
//                                {
//                                    break;
//                                }
//                                num++;
//                            }
//                        }
//                    }
//                }
//            }
//            else
//            {
//                for (int i = 0; i < count; i++)
//                {
//                    foreach (Player player in from p in this.Players.Values
//                    where p.IsPlaying && p.Winall
//                    select p)
//                    {
//                        int num = 0;
//                        for (;;)
//                        {
//                            player.Pocket[i] = this.DrawWinner();
//                            if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
//                            {
//                                break;
//                            }
//                            num++;
//                        }
//                    }
//                }
//                for (int i = 0; i < count; i++)
//                {
//                    foreach (Player player in from p in this.Players.Values
//                    where p.IsPlaying && !p.Winall
//                    select p)
//                    {
//                        int num = 0;
//                        for (;;)
//                        {
//                            player.Pocket[i] = this.Draw();
//                            if (player.Pocket[i] != null || this.Cards.Count <= 1 || num >= 30)
//                            {
//                                break;
//                            }
//                            num++;
//                        }
//                    }
//                }
//            }
//        }

//        // Token: 0x06000525 RID: 1317 RVA: 0x00095DCC File Offset: 0x00093FCC
//        public void AddCardsToBoard(int count)
//        {
//            Player player = this.Players.Values.FirstOrDefault((Player p) => p.Winall && p.IsPlaying && !p.Fold);
//            if (player != null)
//            {
//                byte b = 0;
//                while ((int)b < count)
//                {
//                    if (this.Board[(int)b] == null)
//                    {
//                        int num = 0;
//                        for (;;)
//                        {
//                            this.Board[(int)b] = this.DrawWinnerBoard();
//                            if (this.Board[(int)b] != null || this.Cards.Count <= 1 || num >= 30)
//                            {
//                                break;
//                            }
//                            num++;
//                        }
//                    }
//                    b += 1;
//                }
//            }
//            else
//            {
//                byte b = 0;
//                while ((int)b < count)
//                {
//                    if (this.Board[(int)b] == null)
//                    {
//                        int num = 0;
//                        for (;;)
//                        {
//                            this.Board[(int)b] = this.Draw();
//                            if (this.Board[(int)b] != null || this.Cards.Count <= 1 || num >= 30)
//                            {
//                                break;
//                            }
//                            num++;
//                        }
//                    }
//                    b += 1;
//                }
//            }
//        }

//        // Token: 0x06000526 RID: 1318 RVA: 0x00095EE4 File Offset: 0x000940E4
//        public void Clear()
//        {
//            this.Showhand = false;
//            this.ShowhandTotalPot = 0L;
//            this.ShowHand = 0;
//            this.TempPlayers.Clear();
//            this.RoundState = 0;
//            this.Kick = null;
//            this.PreviousState = Flags.TableState.Unopened;
//            this.RequiredPot = 0UL;
//            this.NumberOfRaise = 0;
//            this.TotalPot = 0UL;
//            this.RoundPot = 0UL;
//            this.CurrentPlayer = 0U;
//            this.PreviousPlayer = null;
//            this.Board = new PokerCard[5];
//            foreach (Player player in this.Players.Values)
//            {
//                player.Create(player.PlayerType, player.Seat, this, (ulong)player.CurrentMoney);
//            }
//            this.ReloadCards();
//        }

//        // Token: 0x06000527 RID: 1319 RVA: 0x00096048 File Offset: 0x00094248
//        public void StartNewRound()
//        {
//            Player player = this.Players.Values.FirstOrDefault((Player p) => p.Winall);
//            if (player != null)
//            {
//                player.WinnerBot = Flags.PlayerTypeWinner.stright;
//                player.ClearCard = false;
//            }
//            if (this.TableType == Flags.TableType.TexasHoldem)
//            {
//                this.ShowHand = 0;
//                this.ReloadCards();
//                foreach (Player player2 in this.Players.Values)
//                {
//                    player2.IsPlaying = true;
//                }
//                if (this.TableIsChange)
//                {
//                    this.AddCardsToPlayers(1);
//                    this.ReloadCards();
//                }
//                this.GetDealer();
//                this.TotalPot = (ulong)((long)(from p in this.Players.Values
//                where p.IsPlaying
//                select p).ToList<Player>().Count * (long)((ulong)(this.MinBet / 2U)));
//                foreach (Player player3 in from p in this.Players.Values
//                where p.IsPlaying
//                select p)
//                {
//                    player3.CurrentMoney -= (long)((ulong)(this.MinBet / 2U));
//                    player3.TotalPot = (ulong)(this.MinBet / 2U);
//                }
//                this.Players[this.SmallBlind].CurrentMoney -= (long)((ulong)(this.MinBet / 2U));
//                this.Players[this.SmallBlind].RoundPot += (ulong)(this.MinBet / 2U);
//                this.Players[this.SmallBlind].TotalPot += (ulong)(this.MinBet / 2U);
//                this.TotalPot += (ulong)(this.MinBet / 2U);
//                this.RoundPot += (ulong)(this.MinBet / 2U);
//                this.Players[this.BigBlind].CurrentMoney -= (long)((ulong)this.MinBet);
//                this.Players[this.BigBlind].RoundPot += (ulong)this.MinBet;
//                this.Players[this.BigBlind].TotalPot += (ulong)this.MinBet;
//                this.TotalPot += (ulong)this.MinBet;
//                this.RoundPot += (ulong)this.MinBet;
//                this.State = Flags.TableState.Pocket;
//            }
//            else
//            {
//                this.ReloadCards();
//                foreach (Player player4 in this.Players.Values)
//                {
//                    player4.IsPlaying = true;
//                }
//                this.AddCardsToPlayers(2);
//                this.GetDealer();
//                this.PreviousPlayer = this.Players[this.PreviousSeat((int)this.Players[this.Dealer].Seat)];
//                this.TotalPot = (ulong)((long)(from p in this.Players.Values
//                where p.IsPlaying
//                select p).ToList<Player>().Count * (long)((ulong)(this.MinBet / 2U)));
//                foreach (Player player5 in from p in this.Players.Values
//                where p.IsPlaying
//                select p)
//                {
//                    player5.CurrentMoney -= (long)((ulong)(this.MinBet / 2U));
//                    player5.TotalPot = (ulong)(this.MinBet / 2U);
//                }
//                this.State = Flags.TableState.Pocket;
//            }
//        }

//        // Token: 0x06000528 RID: 1320 RVA: 0x000964F4 File Offset: 0x000946F4
//        public void StartPocket()
//        {
//            if (this.TableType == Flags.TableType.TexasHoldem)
//            {
//                this.CurrentPlayer = this.NextSeat(this.Players[this.BigBlind].Seat);
//                if (!this.OMAHA)
//                {
//                    this.AddCardsToPlayers(2);
//                }
//                else
//                {
//                    this.AddCardsToPlayers(4);
//                }
//                this.GetRequiredBet();
//            }
//            else
//            {
//                this.CurrentPlayer = this.Dealer;
//                this.GetRequiredBet();
//            }
//        }

//        // Token: 0x06000529 RID: 1321 RVA: 0x00096574 File Offset: 0x00094774
//        private void GetRequiredBet()
//        {
//            if (this.PreviousPlayer != null && this.CurrentPlayer != 0U)
//            {
//                this.RequiredPot = this.PreviousPlayer.RoundPot - this.Players[this.CurrentPlayer].RoundPot;
//            }
//            else
//            {
//                this.RequiredPot = 0UL;
//            }
//        }

//        // Token: 0x0600052A RID: 1322 RVA: 0x000965D4 File Offset: 0x000947D4
//        internal ushort GetRequiredAction()
//        {
//            if (this.TableType == Flags.TableType.TexasHoldem)
//            {
//                if (!this.Players.ContainsKey(this.CurrentPlayer))
//                {
//                    return 0;
//                }
//                if (!this.UnLimited)
//                {
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)(this.RequiredPot + (ulong)this.MinBet) && this.State == Flags.TableState.Pocket && this.NumberOfRaise < 3)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)(this.RequiredPot + (ulong)this.MinBet) && this.State == Flags.TableState.Pocket && this.NumberOfRaise >= 3)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)(this.RequiredPot + (ulong)this.MinBet) && this.State != Flags.TableState.Pocket)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)(this.RequiredPot + 10UL) && this.Players[this.CurrentPlayer].CurrentMoney > (long)this.RequiredPot)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)this.RequiredPot)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)(this.MinBet * 2U)) && this.RoundPot > 0UL)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)(this.MinBet * 2U)) && this.RoundPot > 0UL)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.NumberOfRaise == 0)
//                    {
//                        if (this.State == Flags.TableState.Pocket || this.State == Flags.TableState.Flop)
//                        {
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)this.MinBet) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)this.MinBet) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                        }
//                        else
//                        {
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)(this.MinBet * 2U)) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)(this.MinBet * 2U)) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                        }
//                    }
//                    else if (this.NumberOfRaise > 0)
//                    {
//                        if (this.State == Flags.TableState.Pocket)
//                        {
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)this.MinBet) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)this.MinBet) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                        }
//                        else
//                        {
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)(this.MinBet * 2U)) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                            if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)(this.MinBet * 2U)) && this.RoundPot == 0UL)
//                            {
//                                return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                            }
//                        }
//                    }
//                    Console.WriteLine("Unhandle RequiredBet: " + this.RequiredPot);
//                    return 0;
//                }
//                else
//                {
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)(this.PreviousPlayer.RoundPot + this.RequiredPot))
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)(this.PreviousPlayer.RoundPot + this.RequiredPot) && this.Players[this.CurrentPlayer].CurrentMoney > (long)this.RequiredPot)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)this.RequiredPot)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)this.MinBet) && this.RoundPot > 0UL)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)this.MinBet) && this.RoundPot > 0UL)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)this.MinBet) && this.RoundPot == 0UL)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                    }
//                    if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)this.MinBet) && this.RoundPot == 0UL)
//                    {
//                        return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                    }
//                    Console.WriteLine("Unhandle RequiredBet: " + this.RequiredPot);
//                }
//            }
//            else
//            {
//                if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)(this.PreviousPlayer.RoundPot + this.RequiredPot) && !this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                }
//                if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)(this.PreviousPlayer.RoundPot + this.RequiredPot) && this.Players[this.CurrentPlayer].CurrentMoney > (long)this.RequiredPot && !this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Call + General.ActivePlayerTypes.Fold);
//                }
//                if (this.RequiredPot > 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)this.RequiredPot && !this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
//                }
//                if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)this.MinBet) && this.RoundPot > 0UL && !this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Raise + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                }
//                if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)this.MinBet) && this.RoundPot > 0UL && !this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                }
//                if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney > (long)((ulong)this.MinBet) && this.RoundPot == 0UL && !this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Bet + General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                }
//                if (this.RequiredPot == 0UL && this.Players[this.CurrentPlayer].CurrentMoney <= (long)((ulong)this.MinBet) && this.RoundPot == 0UL && !this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Check + General.ActivePlayerTypes.Fold);
//                }
//                if (this.Showhand)
//                {
//                    return (ushort)(General.ActivePlayerTypes.Allin + General.ActivePlayerTypes.Fold);
//                }
//                Console.WriteLine("Unhandle RequiredBet: " + this.RequiredPot);
//            }
//            return 0;
//        }

//        // Token: 0x0600052B RID: 1323 RVA: 0x00097248 File Offset: 0x00095448
//        private byte CheckRound()
//        {
//            ulong num = 0UL;
//            bool flag = true;
//            bool flag2 = true;
//            foreach (Player player in from p in this.Players.Values
//            where p.IsPlaying && !p.IsPotAllin && !p.Fold
//            select p)
//            {
//                if (num == 0UL)
//                {
//                    num = player.RoundPot;
//                }
//                if (num != player.RoundPot)
//                {
//                    flag = false;
//                    break;
//                }
//                if (!player.PotinThisRound)
//                {
//                    flag2 = false;
//                    break;
//                }
//            }
//            byte result;
//            if ((from p in this.Players.Values
//            where p.IsPlaying && !p.Fold
//            select p).ToList<Player>().Count == 1)
//            {
//                result = 3;
//            }
//            else
//            {
//                bool flag3;
//                if (flag2 && flag)
//                {
//                    flag3 = ((from p in this.Players.Values
//                    where p.IsPlaying && !p.IsPotAllin && !p.Fold
//                    select p).ToList<Player>().Count != 0);
//                }
//                else
//                {
//                    flag3 = true;
//                }
//                if (!flag3)
//                {
//                    result = 1;
//                }
//                else
//                {
//                    bool flag4;
//                    if (flag2 && flag)
//                    {
//                        if ((from p in this.Players.Values
//                        where p.IsPlaying && !p.IsPotAllin && !p.Fold
//                        select p).ToList<Player>().Count == 1)
//                        {
//                            flag4 = (this.RequiredPot != 0UL);
//                            goto IL_1A3;
//                        }
//                    }
//                    flag4 = true;
//                    IL_1A3:
//                    if (!flag4)
//                    {
//                        result = 1;
//                    }
//                    else
//                    {
//                        bool flag5;
//                        if (flag2 && flag)
//                        {
//                            if ((from p in this.Players.Values
//                            where p.IsPlaying && !p.IsPotAllin && !p.Fold
//                            select p).ToList<Player>().Count > 1)
//                            {
//                                flag5 = (this.RequiredPot != 0UL);
//                                goto IL_202;
//                            }
//                        }
//                        flag5 = true;
//                        IL_202:
//                        if (!flag5)
//                        {
//                            result = 2;
//                        }
//                        else
//                        {
//                            result = 0;
//                        }
//                    }
//                }
//            }
//            return result;
//        }

//        // Token: 0x0600052C RID: 1324 RVA: 0x0009747C File Offset: 0x0009567C
//        public bool Next(bool c = false)
//        {
//            bool result;
//            if (this.TableType == Flags.TableType.TexasHoldem)
//            {
//                if (c)
//                {
//                    this.PreviousPlayer = this.Players[this.CurrentPlayer];
//                }
//                if (this.PreviousPlayer == null)
//                {
//                    this.CurrentPlayer = this.NextSeat(this.Players[this.SmallBlind].Seat);
//                }
//                else
//                {
//                    this.CurrentPlayer = this.NextSeat(this.PreviousPlayer.Seat);
//                }
//                this.GetRequiredBet();
//                this.NextRound();
//                result = (this.State != Flags.TableState.ShowDown);
//            }
//            else
//            {
//                if (c)
//                {
//                    this.PreviousPlayer = this.Players[this.CurrentPlayer];
//                }
//                if (this.PreviousPlayer == null)
//                {
//                    this.CurrentPlayer = this.NextSeat(this.Players[this.Dealer].Seat);
//                }
//                else
//                {
//                    this.CurrentPlayer = this.NextSeat(this.PreviousPlayer.Seat);
//                }
//                this.GetRequiredBet();
//                this.NextRound();
//                result = (this.State != Flags.TableState.ShowDown);
//            }
//            return result;
//        }

//        // Token: 0x0600052D RID: 1325 RVA: 0x00097604 File Offset: 0x00095804
//        public void NextRound()
//        {
//            if (this.TableType == Flags.TableType.TexasHoldem)
//            {
//                byte b = this.CheckRound();
//                if (b == 1 || b == 3)
//                {
//                    this.AddCardsToBoard(5);
//                    if (b == 1)
//                    {
//                        this.PreviousState = this.State;
//                    }
//                    this.RoundPot = 0UL;
//                    this.CurrentPlayer = 0U;
//                    this.PreviousPlayer = null;
//                    this.RoundState = 0;
//                    this.State = Flags.TableState.ShowDown;
//                }
//                else if (b == 2)
//                {
//                    if ((byte)(this.State + 1) == 2)
//                    {
//                        this.AddCardsToBoard(3);
//                    }
//                    if ((byte)(this.State + 1) == 3)
//                    {
//                        this.AddCardsToBoard(4);
//                    }
//                    if ((byte)(this.State + 1) == 4)
//                    {
//                        this.AddCardsToBoard(5);
//                    }
//                    foreach (Player player in from p in this.Players.Values
//                    where p.IsPlaying
//                    select p)
//                    {
//                        player.PotinThisRound = false;
//                        player.RoundPot = 0UL;
//                    }
//                    this.CurrentPlayer = this.SmallBlind;
//                    this.RoundPot = 0UL;
//                    this.RoundState = 0;
//                    this.State += 1;
//                    if (this.State == Flags.TableState.ShowDown)
//                    {
//                        this.CurrentPlayer = 0U;
//                        this.PreviousPlayer = null;
//                    }
//                }
//            }
//            else
//            {
//                byte b = this.CheckRound();
//                if (b == 1 || b == 3)
//                {
//                    this.AddCardsToPlayers(5);
//                    if (b == 1)
//                    {
//                        this.PreviousState = this.State;
//                    }
//                    this.RoundPot = 0UL;
//                    this.CurrentPlayer = 0U;
//                    this.PreviousPlayer = null;
//                    this.RoundState = 0;
//                    this.GetDealer();
//                    this.State = Flags.TableState.ShowDown;
//                }
//                else if (b == 2)
//                {
//                    if ((byte)(this.State + 1) == 2)
//                    {
//                        this.AddCardsToPlayers(3);
//                    }
//                    if ((byte)(this.State + 1) == 3)
//                    {
//                        this.AddCardsToPlayers(4);
//                    }
//                    if ((byte)(this.State + 1) == 4)
//                    {
//                        this.AddCardsToPlayers(5);
//                    }
//                    foreach (Player player2 in from p in this.Players.Values
//                    where p.IsPlaying
//                    select p)
//                    {
//                        player2.PotinThisRound = false;
//                        player2.RoundPot = 0UL;
//                    }
//                    this.GetDealer();
//                    this.CurrentPlayer = this.Dealer;
//                    this.RoundPot = 0UL;
//                    this.RoundState = 0;
//                    this.State += 1;
//                    if (this.State == Flags.TableState.ShowDown)
//                    {
//                        this.CurrentPlayer = 0U;
//                        this.PreviousPlayer = null;
//                    }
//                }
//            }
//            if (this.Players.ContainsKey(this.CurrentPlayer))
//            {
//                this.Players[this.CurrentPlayer].TimeAllin = Time32.Now.AddSeconds(3);
//            }
//        }

//        // Token: 0x0600052E RID: 1326 RVA: 0x000979D8 File Offset: 0x00095BD8
//        internal void GetDealer()
//        {
//            if (this.TableType == Flags.TableType.TexasHoldem)
//            {
//                if (this.TableIsChange)
//                {
//                    this.Dealer = 0U;
//                }
//                if (this.Dealer == 0U)
//                {
//                    foreach (Player player in from p in this.Players.Values
//                    where p.IsPlaying
//                    select p)
//                    {
//                        if (this.Dealer == 0U)
//                        {
//                            this.Dealer = player.Uid;
//                        }
//                        else if (player.Pocket[0] > this.Players[this.Dealer].Pocket[0])
//                        {
//                            this.Dealer = player.Uid;
//                        }
//                    }
//                }
//                else
//                {
//                    this.Dealer = this.NextSeat(this.Players[this.Dealer].Seat);
//                }
//                this.SmallBlind = this.NextSeat(this.Players[this.Dealer].Seat);
//                this.BigBlind = this.NextSeat(this.Players[this.SmallBlind].Seat);
//                this.PreviousPlayer = this.Players[this.BigBlind];
//                this.CurrentPlayer = 0U;
//            }
//            else
//            {
//                this.Dealer = 0U;
//                if (this.Dealer == 0U)
//                {
//                    foreach (Player player2 in from p in this.Players.Values
//                    where p.IsPlaying
//                    select p)
//                    {
//                        if (this.Dealer == 0U)
//                        {
//                            this.Dealer = player2.Uid;
//                        }
//                        else
//                        {
//                            int num = 0;
//                            int num2 = 0;
//                            string text = "";
//                            string text2 = "";
//                            PokerCard[] pocket = player2.Pocket;
//                            foreach (PokerCard pokerCard in pocket)
//                            {
//                                if (player2.Pocket[0] != null && player2.Pocket[0] != pokerCard && pokerCard != null)
//                                {
//                                    text = text + " " + pokerCard.ToString();
//                                    num++;
//                                }
//                            }
//                            pocket = this.Players[this.Dealer].Pocket;
//                            foreach (PokerCard pokerCard in pocket)
//                            {
//                                if (this.Players[this.Dealer].Pocket[0] != null && this.Players[this.Dealer].Pocket[0] != pokerCard && pokerCard != null)
//                                {
//                                    text2 = text2 + " " + pokerCard.ToString();
//                                    num2++;
//                                }
//                            }
//                            ulong cards = Hand.ParseHand(text);
//                            ulong cards2 = Hand.ParseHand(text2);
//                            uint num3 = Hand.Evaluate(cards, num);
//                            uint num4 = Hand.Evaluate(cards2, num2);
//                            Hand.DescriptionFromHandValueInternal(num3);
//                            Hand.DescriptionFromHandValueInternal(num4);
//                            if (num3 > num4)
//                            {
//                                this.Dealer = player2.Uid;
//                            }
//                        }
//                    }
//                }
//                this.CurrentPlayer = 0U;
//            }
//        }

//        // Token: 0x0600052F RID: 1327 RVA: 0x00097E38 File Offset: 0x00096038
//        public uint PreviousSeat(int seat)
//        {
//            List<Player> list = (from p in this.Players.Values
//            where p.IsPlaying && !p.IsPotAllin && !p.Fold
//            select p).ToList<Player>();
//            uint result;
//            if (list.Count == 0)
//            {
//                result = 0U;
//            }
//            else if (list.Count == 1)
//            {
//                result = list.FirstOrDefault<Player>().Uid;
//            }
//            else
//            {
//                uint num = 0U;
//                int num2 = 0;
//                while (num == 0U && num2 < 20)
//                {
//                    num2++;
//                    seat--;
//                    if (seat < 0)
//                    {
//                        seat = 4;
//                    }
//                    Player player = (from x in list
//                    where (int)x.Seat == seat
//                    select x).FirstOrDefault<Player>();
//                    if (player != null)
//                    {
//                        num = player.Uid;
//                    }
//                }
//                result = num;
//            }
//            return result;
//        }

//        // Token: 0x06000530 RID: 1328 RVA: 0x00098024 File Offset: 0x00096224
//        public uint NextSeat(byte seat)
//        {
//            uint result;
//            if (this.TableType == Flags.TableType.TexasHoldem)
//            {
//                List<Player> list = (from p in this.Players.Values
//                where p.IsPlaying && !p.IsPotAllin && !p.Fold
//                select p).ToList<Player>();
//                if (list.Count == 0)
//                {
//                    result = 0U;
//                }
//                else if (list.Count == 1)
//                {
//                    result = list.FirstOrDefault<Player>().Uid;
//                }
//                else
//                {
//                    uint num = 0U;
//                    int num2 = 0;
//                    while (num == 0U && num2 < 20)
//                    {
//                        num2++;
//                        seat += 1;
//                        if (seat > 8)
//                        {
//                            seat = 0;
//                        }
//                        Player player = (from x in list
//                        where x.Seat == seat
//                        select x).FirstOrDefault<Player>();
//                        if (player != null)
//                        {
//                            num = player.Uid;
//                        }
//                    }
//                    result = num;
//                }
//            }
//            else
//            {
//                List<Player> list = (from p in this.Players.Values
//                where p.IsPlaying && !p.IsPotAllin && !p.Fold
//                select p).ToList<Player>();
//                if (this.State == Flags.TableState.ShowDown)
//                {
//                    list = (from p in this.Players.Values
//                    where p.IsPlaying && !p.Fold
//                    select p).ToList<Player>();
//                }
//                if (list.Count == 0)
//                {
//                    result = 0U;
//                }
//                else if (list.Count == 1)
//                {
//                    result = list.FirstOrDefault<Player>().Uid;
//                }
//                else
//                {
//                    uint num = 0U;
//                    int num2 = 0;
//                    while (num == 0U && num2 < 20)
//                    {
//                        num2++;
//                        seat += 1;
//                        if (seat > 5)
//                        {
//                            seat = 0;
//                        }
//                        Player player = (from x in list
//                        where x.Seat == seat
//                        select x).FirstOrDefault<Player>();
//                        if (player != null)
//                        {
//                            num = player.Uid;
//                        }
//                    }
//                    result = num;
//                }
//            }
//            return result;
//        }

//        // Token: 0x06000531 RID: 1329 RVA: 0x000982BC File Offset: 0x000964BC
//        public bool HighestBet(uint uid, ulong bet)
//        {
//            bool result = true;
//            foreach (Player player in from p in this.Players.Values
//            where p.IsPlaying
//            select p)
//            {
//                if (player.Uid != uid && player.RoundPot > bet)
//                {
//                    result = false;
//                    break;
//                }
//            }
//            foreach (KeyValuePair<uint, ulong> keyValuePair in this.TempPlayers)
//            {
//                if (keyValuePair.Value > this.Players[uid].TotalPot)
//                {
//                    return false;
//                }
//            }
//            return result;
//        }

//        // Token: 0x06000532 RID: 1330 RVA: 0x000983DC File Offset: 0x000965DC
//        public IEnumerable<Player> PlayersOnTable()
//        {
//            List<Player> list = new List<Player>();
//            list.AddRange(this.Players.Values);
//            list.AddRange(this.Watchers.Values);
//            return list;
//        }

//        // Token: 0x06000533 RID: 1331 RVA: 0x000984B4 File Offset: 0x000966B4
//        public void GetWinners()
//        {
//            int num = 0;
//            int num2 = 0;
//            int num3 = 0;
//            int num4 = 0;
//            try
//            {
//                if (this.TableType == Flags.TableType.TexasHoldem)
//                {
//                    this.TableBusy = true;
//                    IEnumerable<Player> enumerable = from p in this.Players.Values
//                    where p.IsPlaying
//                    select p;
//                    Dictionary<uint, Hand> dictionary = new Dictionary<uint, Hand>();
//                    Dictionary<uint, Hand> dictionary2 = new Dictionary<uint, Hand>();
//                    foreach (Player player in enumerable)
//                    {
//                        if (this.TempPlayers.ContainsKey(player.Uid))
//                        {
//                            this.TempPlayers[player.Uid] = player.TotalPot;
//                        }
//                        else
//                        {
//                            this.TempPlayers.Add(player.Uid, player.TotalPot);
//                        }
//                        if (player.Fold)
//                        {
//                            player.Lose -= (long)player.TotalPot;
//                        }
//                        if (!player.Fold)
//                        {
//                            Hand value = (!this.OMAHA) ? new Hand(player.ToString(), this.ToString()) : player.FourCard(this.ToString());
//                            dictionary2.Add(player.Uid, value);
//                            dictionary.Add(player.Uid, value);
//                        }
//                    }
//                    ConcurrentDictionary<uint, SidePot> concurrentDictionary = this.CalcaluteSidePots();
//                    List<uint> list = new List<uint>();
//                    foreach (KeyValuePair<uint, SidePot> keyValuePair in concurrentDictionary)
//                    {
//                        foreach (KeyValuePair<uint, Hand> keyValuePair2 in dictionary)
//                        {
//                            if (keyValuePair.Value.Players.Count == 1 && keyValuePair.Value.Players.Contains(keyValuePair2.Key))
//                            {
//                                this.Players[keyValuePair2.Key].CurrentMoney += keyValuePair.Value.Money;
//                                this.Players[keyValuePair2.Key].TotalPot -= (ulong)keyValuePair.Value.Money;
//                                if (!list.Contains(keyValuePair.Key))
//                                {
//                                    list.Add(keyValuePair.Key);
//                                }
//                            }
//                        }
//                    }
//                    foreach (uint key in list)
//                    {
//                        if (concurrentDictionary.ContainsKey(key))
//                        {
//                            ((IDictionary<uint, SidePot>)concurrentDictionary).Remove(key);
//                        }
//                    }
//                    list.Clear();
//                    for (;;)
//                    {
//                        IL_323:
//                        Dictionary<uint, Hand> dictionary3 = this.CalcaluteBestHands(dictionary);
//                        bool flag = false;
//                        for (;;)
//                        {
//                            IL_337:
//                            List<uint> list2 = new List<uint>();
//                            using (Dictionary<uint, Hand>.Enumerator enumerator5 = dictionary3.GetEnumerator())
//                            {
//                                while (enumerator5.MoveNext())
//                                {
//                                    KeyValuePair<uint, Hand> p2 = enumerator5.Current;
//                                    IEnumerable<Player> source = enumerable;
//                                    Func<Player, bool> predicate = delegate(Player x)
//                                    {
//                                        uint uid = x.Uid;
//                                        KeyValuePair<uint, Hand> p4 = p2;
//                                        return uid == p4.Key;
//                                    };
//                                    Player player2 = source.Where(predicate).FirstOrDefault<Player>();
//                                    if (player2 != null)
//                                    {
//                                        foreach (KeyValuePair<uint, SidePot> keyValuePair3 in concurrentDictionary)
//                                        {
//                                            long num5 = 0L;
//                                            foreach (uint key2 in keyValuePair3.Value.Players)
//                                            {
//                                                if (dictionary3.ContainsKey(key2))
//                                                {
//                                                    num5 += 1L;
//                                                }
//                                            }
//                                            if (keyValuePair3.Value.Players.Contains(player2.Uid) || flag)
//                                            {
//                                                if (!flag)
//                                                {
//                                                    player2.Lose += keyValuePair3.Value.Money / num5;
//                                                }
//                                                else
//                                                {
//                                                    player2.Lose += keyValuePair3.Value.Money / (long)dictionary3.Count;
//                                                }
//                                                if (!list.Contains(keyValuePair3.Key))
//                                                {
//                                                    list.Add(keyValuePair3.Key);
//                                                }
//                                            }
//                                        }
//                                    }
//                                    List<uint> list3 = list2;
//                                    KeyValuePair<uint, Hand> p7 = p2;
//                                    if (!list3.Contains(p7.Key))
//                                    {
//                                        List<uint> list4 = list2;
//                                        p7 = p2;
//                                        list4.Add(p7.Key);
//                                    }
//                                }
//                            }
//                            foreach (uint key3 in list2)
//                            {
//                                if (dictionary.ContainsKey(key3))
//                                {
//                                    dictionary.Remove(key3);
//                                }
//                            }
//                            foreach (uint key4 in list)
//                            {
//                                if (concurrentDictionary.ContainsKey(key4))
//                                {
//                                    ((IDictionary<uint, SidePot>)concurrentDictionary).Remove(key4);
//                                }
//                            }
//                            if (concurrentDictionary.Count > 0)
//                            {
//                                foreach (SidePot sidePot in concurrentDictionary.Values)
//                                {
//                                    foreach (KeyValuePair<uint, Hand> keyValuePair4 in dictionary)
//                                    {
//                                        if (sidePot.Players.Contains(keyValuePair4.Key) && this.Players.ContainsKey(keyValuePair4.Key) && !this.Players[keyValuePair4.Key].Fold && num < 30)
//                                        {
//                                            num++;
//                                            goto IL_323;
//                                        }
//                                    }
//                                }
//                            }
//                            if (concurrentDictionary.Count > 0)
//                            {
//                                foreach (SidePot sidePot2 in concurrentDictionary.Values)
//                                {
//                                    dictionary3 = this.CalcaluteBestHands(dictionary2);
//                                    flag = true;
//                                    if (dictionary3.Count >= 1)
//                                    {
//                                        bool flag2 = false;
//                                        foreach (KeyValuePair<uint, Hand> keyValuePair5 in dictionary3)
//                                        {
//                                            if (!this.Players.ContainsKey(keyValuePair5.Key))
//                                            {
//                                                flag2 = true;
//                                            }
//                                        }
//                                        if (!flag2 && num2 < 30)
//                                        {
//                                            num2++;
//                                            goto IL_337;
//                                        }
//                                    }
//                                }
//                                goto IL_7C6;
//                            }
//                            goto IL_7C8;
//                        }
//                    }
//                    IL_7C6:
//                    IL_7C8:
//                    foreach (KeyValuePair<uint, Hand> keyValuePair6 in dictionary2)
//                    {
//                        this.Players[keyValuePair6.Key].CurrentMoney += this.Players[keyValuePair6.Key].Lose;
//                        if (this.Players[keyValuePair6.Key].CurrentMoney < 0L)
//                        {
//                            this.Players[keyValuePair6.Key].CurrentMoney = 0L;
//                        }
//                        if (this.Players[keyValuePair6.Key].Lose >= (long)this.Players[keyValuePair6.Key].TotalPot)
//                        {
//                            this.Players[keyValuePair6.Key].Lose = this.Players[keyValuePair6.Key].Lose - (long)(this.Players[keyValuePair6.Key].TotalPot * 99UL / 100UL);
//                        }
//                        else
//                        {
//                            this.Players[keyValuePair6.Key].Lose = this.Players[keyValuePair6.Key].Lose - (long)this.Players[keyValuePair6.Key].TotalPot;
//                        }
//                    }
//                }
//                else
//                {
//                    this.TableBusy = true;
//                    IEnumerable<Player> enumerable = from p in this.Players.Values
//                    where p.IsPlaying
//                    select p;
//                    Dictionary<uint, ulong> dictionary4 = new Dictionary<uint, ulong>();
//                    Dictionary<uint, ulong> dictionary5 = new Dictionary<uint, ulong>();
//                    foreach (Player player3 in enumerable)
//                    {
//                        if (this.TempPlayers.ContainsKey(player3.Uid))
//                        {
//                            this.TempPlayers[player3.Uid] = player3.TotalPot;
//                        }
//                        else
//                        {
//                            this.TempPlayers.Add(player3.Uid, player3.TotalPot);
//                        }
//                        if (player3.Fold)
//                        {
//                            player3.Lose -= (long)player3.TotalPot;
//                        }
//                        if (!player3.Fold)
//                        {
//                            int num6 = 0;
//                            string text = "";
//                            PokerCard[] pocket = player3.Pocket;
//                            foreach (PokerCard pokerCard in pocket)
//                            {
//                                if (player3.Pocket[0] != null && player3.Pocket[0] != pokerCard && pokerCard != null)
//                                {
//                                    text = text + " " + pokerCard.ToString();
//                                    num6++;
//                                }
//                            }
//                            ulong cards = Hand.ParseHand(text);
//                            uint num7 = Hand.Evaluate(cards, num6);
//                            dictionary5.Add(player3.Uid, (ulong)num7);
//                            dictionary4.Add(player3.Uid, (ulong)num7);
//                        }
//                    }
//                    ConcurrentDictionary<uint, SidePot> concurrentDictionary = this.CalcaluteSidePots();
//                    List<uint> list = new List<uint>();
//                    foreach (KeyValuePair<uint, SidePot> keyValuePair7 in concurrentDictionary)
//                    {
//                        foreach (KeyValuePair<uint, ulong> keyValuePair8 in dictionary4)
//                        {
//                            if (keyValuePair7.Value.Players.Count == 1 && keyValuePair7.Value.Players.Contains(keyValuePair8.Key))
//                            {
//                                this.Players[keyValuePair8.Key].CurrentMoney += keyValuePair7.Value.Money;
//                                this.Players[keyValuePair8.Key].TotalPot -= (ulong)keyValuePair7.Value.Money;
//                                if (!list.Contains(keyValuePair7.Key))
//                                {
//                                    list.Add(keyValuePair7.Key);
//                                }
//                            }
//                        }
//                    }
//                    foreach (uint key5 in list)
//                    {
//                        if (concurrentDictionary.ContainsKey(key5))
//                        {
//                            ((IDictionary<uint, SidePot>)concurrentDictionary).Remove(key5);
//                        }
//                    }
//                    list.Clear();
//                    for (;;)
//                    {
//                        IL_CD3:
//                        Dictionary<uint, ulong> dictionary6 = this.CalcaluteBestHands(dictionary4);
//                        bool flag = false;
//                        for (;;)
//                        {
//                            IL_CE7:
//                            List<uint> list2 = new List<uint>();
//                            using (Dictionary<uint, ulong>.Enumerator enumerator8 = dictionary6.GetEnumerator())
//                            {
//                                while (enumerator8.MoveNext())
//                                {
//                                    KeyValuePair<uint, ulong> p3 = enumerator8.Current;
//                                    IEnumerable<Player> source2 = enumerable;
//                                    Func<Player, bool> predicate2 = delegate(Player x)
//                                    {
//                                        uint uid = x.Uid;
//                                        KeyValuePair<uint, ulong> p6 = p3;
//                                        return uid == p6.Key;
//                                    };
//                                    Player player2 = source2.Where(predicate2).FirstOrDefault<Player>();
//                                    if (player2 != null)
//                                    {
//                                        foreach (KeyValuePair<uint, SidePot> keyValuePair9 in concurrentDictionary)
//                                        {
//                                            long num5 = 0L;
//                                            foreach (uint key6 in keyValuePair9.Value.Players)
//                                            {
//                                                if (dictionary6.ContainsKey(key6))
//                                                {
//                                                    num5 += 1L;
//                                                }
//                                            }
//                                            if (keyValuePair9.Value.Players.Contains(player2.Uid) || flag)
//                                            {
//                                                if (!flag)
//                                                {
//                                                    player2.Lose += keyValuePair9.Value.Money / num5;
//                                                }
//                                                else
//                                                {
//                                                    player2.Lose += keyValuePair9.Value.Money / (long)dictionary6.Count;
//                                                }
//                                                if (!list.Contains(keyValuePair9.Key))
//                                                {
//                                                    list.Add(keyValuePair9.Key);
//                                                }
//                                            }
//                                        }
//                                    }
//                                    List<uint> list5 = list2;
//                                    KeyValuePair<uint, ulong> p5 = p3;
//                                    if (!list5.Contains(p5.Key))
//                                    {
//                                        List<uint> list6 = list2;
//                                        p5 = p3;
//                                        list6.Add(p5.Key);
//                                    }
//                                }
//                            }
//                            foreach (uint key7 in list2)
//                            {
//                                if (dictionary4.ContainsKey(key7))
//                                {
//                                    dictionary4.Remove(key7);
//                                }
//                            }
//                            foreach (uint key8 in list)
//                            {
//                                if (concurrentDictionary.ContainsKey(key8))
//                                {
//                                    ((IDictionary<uint, SidePot>)concurrentDictionary).Remove(key8);
//                                }
//                            }
//                            if (concurrentDictionary.Count > 0)
//                            {
//                                foreach (SidePot sidePot3 in concurrentDictionary.Values)
//                                {
//                                    foreach (KeyValuePair<uint, ulong> keyValuePair10 in dictionary4)
//                                    {
//                                        if (sidePot3.Players.Contains(keyValuePair10.Key) && this.Players.ContainsKey(keyValuePair10.Key) && !this.Players[keyValuePair10.Key].Fold && num3 < 30)
//                                        {
//                                            num3++;
//                                            goto IL_CD3;
//                                        }
//                                    }
//                                }
//                            }
//                            if (concurrentDictionary.Count > 0)
//                            {
//                                foreach (SidePot sidePot4 in concurrentDictionary.Values)
//                                {
//                                    dictionary6 = this.CalcaluteBestHands(dictionary5);
//                                    flag = true;
//                                    if (dictionary6.Count >= 1)
//                                    {
//                                        bool flag2 = false;
//                                        foreach (KeyValuePair<uint, ulong> keyValuePair11 in dictionary6)
//                                        {
//                                            if (!this.Players.ContainsKey(keyValuePair11.Key))
//                                            {
//                                                flag2 = true;
//                                            }
//                                        }
//                                        if (!flag2 && num4 < 30)
//                                        {
//                                            num4++;
//                                            goto IL_CE7;
//                                        }
//                                    }
//                                }
//                                goto IL_1176;
//                            }
//                            goto IL_1178;
//                        }
//                    }
//                    IL_1176:
//                    IL_1178:
//                    foreach (KeyValuePair<uint, ulong> keyValuePair12 in dictionary5)
//                    {
//                        this.Players[keyValuePair12.Key].CurrentMoney += this.Players[keyValuePair12.Key].Lose;
//                        if (this.Players[keyValuePair12.Key].Lose >= (long)this.Players[keyValuePair12.Key].TotalPot)
//                        {
//                            this.Players[keyValuePair12.Key].Lose = this.Players[keyValuePair12.Key].Lose - (long)(this.Players[keyValuePair12.Key].TotalPot * 99UL / 100UL);
//                        }
//                        else
//                        {
//                            this.Players[keyValuePair12.Key].Lose = this.Players[keyValuePair12.Key].Lose - (long)this.Players[keyValuePair12.Key].TotalPot;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                this.TableBusy = false;
//            }
//            finally
//            {
//                this.TableBusy = false;
//            }
//        }

//        // Token: 0x06000534 RID: 1332 RVA: 0x00099B20 File Offset: 0x00097D20
//        internal ConcurrentDictionary<uint, SidePot> CalcaluteSidePots()
//        {
//            uint num = 0U;
//            ConcurrentDictionary<uint, SidePot> concurrentDictionary = new ConcurrentDictionary<uint, SidePot>();
//            Dictionary<uint, ulong> dictionary = (from x in this.TempPlayers
//            orderby x.Value
//            select x).ToDictionary((KeyValuePair<uint, ulong> x) => x.Key, (KeyValuePair<uint, ulong> x) => x.Value);
//            int num2 = 0;
//            while (dictionary.Count > 0 && num2 < 30)
//            {
//                num2++;
//                num += 1U;
//                SidePot sidePot = new SidePot();
//                KeyValuePair<uint, ulong> keyValuePair = dictionary.FirstOrDefault<KeyValuePair<uint, ulong>>();
//                sidePot.Money += (long)(keyValuePair.Value * (ulong)((long)dictionary.Count));
//                foreach (KeyValuePair<uint, ulong> keyValuePair2 in dictionary)
//                {
//                    Dictionary<uint, ulong> tempPlayers;
//                    uint key;
//                    (tempPlayers = this.TempPlayers)[key = keyValuePair2.Key] = tempPlayers[key] - keyValuePair.Value;
//                    if (this.TempPlayers[keyValuePair2.Key] == 0UL)
//                    {
//                        this.TempPlayers.Remove(keyValuePair2.Key);
//                    }
//                    sidePot.Players.Add(keyValuePair2.Key);
//                }
//                if (sidePot.Players.Count > 1)
//                {
//                    sidePot.Money = sidePot.Money * 99L / 100L;
//                }
//                concurrentDictionary.TryAdd(num, sidePot);
//                dictionary = new Dictionary<uint, ulong>((from x in this.TempPlayers
//                orderby x.Value
//                select x).ToDictionary((KeyValuePair<uint, ulong> x) => x.Key, (KeyValuePair<uint, ulong> x) => x.Value));
//            }
//            this.TempPlayers.Clear();
//            return concurrentDictionary;
//        }

//        // Token: 0x06000535 RID: 1333 RVA: 0x00099D78 File Offset: 0x00097F78
//        internal Dictionary<uint, ulong> CalcaluteBestHands(Dictionary<uint, ulong> Hands)
//        {
//            Dictionary<uint, ulong> dictionary = new Dictionary<uint, ulong>();
//            foreach (KeyValuePair<uint, ulong> keyValuePair in Hands)
//            {
//                if (dictionary.Count == 0)
//                {
//                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
//                }
//                else if (keyValuePair.Value > dictionary.FirstOrDefault<KeyValuePair<uint, ulong>>().Value)
//                {
//                    dictionary.Clear();
//                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
//                }
//                else if (keyValuePair.Value == dictionary.FirstOrDefault<KeyValuePair<uint, ulong>>().Value)
//                {
//                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
//                }
//            }
//            return dictionary;
//        }

//        // Token: 0x06000536 RID: 1334 RVA: 0x00099E80 File Offset: 0x00098080
//        internal Dictionary<uint, Hand> CalcaluteBestHands(Dictionary<uint, Hand> Hands)
//        {
//            Dictionary<uint, Hand> dictionary = new Dictionary<uint, Hand>();
//            foreach (KeyValuePair<uint, Hand> keyValuePair in Hands)
//            {
//                if (dictionary.Count == 0)
//                {
//                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
//                }
//                else if (keyValuePair.Value > dictionary.FirstOrDefault<KeyValuePair<uint, Hand>>().Value)
//                {
//                    dictionary.Clear();
//                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
//                }
//                else if (keyValuePair.Value == dictionary.FirstOrDefault<KeyValuePair<uint, Hand>>().Value)
//                {
//                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
//                }
//            }
//            return dictionary;
//        }

//        // Token: 0x06000537 RID: 1335 RVA: 0x00099F8C File Offset: 0x0009818C
//        public override string ToString()
//        {
//            string text = "";
//            PokerCard[] board = this.Board;
//            foreach (PokerCard pokerCard in board)
//            {
//                if (pokerCard != null)
//                {
//                    text = (string.IsNullOrEmpty(text) ? pokerCard.ToString() : (text + " " + pokerCard));
//                }
//            }
//            return text;
//        }

//        // Token: 0x04000544 RID: 1348
//        public Kick Kick;

//        // Token: 0x04000545 RID: 1349
//        public List<uint> ToSend;

//        // Token: 0x04000546 RID: 1350
//        public uint BigBlind;

//        // Token: 0x04000547 RID: 1351
//        internal PokerCard[] Board;

//        // Token: 0x04000548 RID: 1352
//        internal List<PokerCard> Cards;

//        // Token: 0x04000549 RID: 1353
//        internal List<PokerCard> CardNewRemove;

//        // Token: 0x0400054A RID: 1354
//        public uint CurrentPlayer;

//        // Token: 0x0400054B RID: 1355
//        public uint Dealer;

//        // Token: 0x0400054C RID: 1356
//        public object dr;

//        // Token: 0x0400054D RID: 1357
//        public ushort MapId = 3053;

//        // Token: 0x0400054E RID: 1358
//        public ConcurrentDictionary<uint, uint> OnScreen;

//        // Token: 0x0400054F RID: 1359
//        public ConcurrentDictionary<uint, Player> Players;

//        // Token: 0x04000550 RID: 1360
//        private Dictionary<uint, ulong> TempPlayers;

//        // Token: 0x04000551 RID: 1361
//        public ConcurrentDictionary<uint, Player> Watchers;

//        // Token: 0x04000552 RID: 1362
//        public Player PreviousPlayer;

//        // Token: 0x04000553 RID: 1363
//        public ulong RequiredPot;

//        // Token: 0x04000554 RID: 1364
//        public ulong RoundPot;

//        // Token: 0x04000555 RID: 1365
//        public uint SmallBlind;

//        // Token: 0x04000556 RID: 1366
//        public bool TableIsChange;

//        // Token: 0x04000557 RID: 1367
//        public object TableSyncRoot;

//        // Token: 0x04000558 RID: 1368
//        public Flags.TableType TableType;

//        // Token: 0x04000559 RID: 1369
//        public DateTime ThreadTime;

//        // Token: 0x0400055A RID: 1370
//        public DateTime Time;

//        // Token: 0x0400055B RID: 1371
//        public byte RoundState;

//        // Token: 0x0400055C RID: 1372
//        private int ShowhandB;

//        // Token: 0x0400055D RID: 1373
//        public Flags.TableState PreviousState;

//        // Token: 0x0400055E RID: 1374
//        public byte NumberOfRaise = 0;

//        // Token: 0x0400055F RID: 1375
//        public bool Showhand = false;

//        // Token: 0x04000560 RID: 1376
//        public long ShowhandTotalPot = 0L;

//        // Token: 0x04000561 RID: 1377
//        public bool TableBusy = false;

//        // Token: 0x04000562 RID: 1378
//        public uint Id;

//        // Token: 0x04000563 RID: 1379
//        public ushort X;

//        // Token: 0x04000564 RID: 1380
//        public ushort Y;

//        // Token: 0x04000565 RID: 1381
//        internal uint Mesh;

//        // Token: 0x04000566 RID: 1382
//        public uint Number;

//        // Token: 0x04000567 RID: 1383
//        public bool UnLimited;

//        // Token: 0x04000568 RID: 1384
//        public bool IsCPs;

//        // Token: 0x04000569 RID: 1385
//        public uint MinBet;

//        // Token: 0x0400056A RID: 1386
//        public Flags.TableState State;

//        // Token: 0x0400056B RID: 1387
//        public bool OMAHA;

//        // Token: 0x0400056C RID: 1388
//        public ulong TotalPot;
//    }
//}
