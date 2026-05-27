using VirusX.Game.MsgFloorItem;
using VirusX.Game.MsgServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using VirusX.Database;
using VirusX.Insults;
using VirusX.Game.Ai;

namespace VirusX
{
    public static class Pool
    {
        public static string RewardName(int i)
        {
            if (i == 1)
            {

                return "3 " + Pool.ItemsBase[3335933].Name + ".";
            }

            else if (i == 2)
            {
                return "2 " + Pool.ItemsBase[3335933].Name + ".";
            }

            else if (i == 3)
            {
                return "1 " + Pool.ItemsBase[3335933].Name + ".";
            }
            return "";
        }
        public static string RewardNameAnima(int i)
        {
            if (i == 1)
            {

                return "3 " + Pool.ItemsBase[4200016].Name + " and 100 M  Money.";
            }
            else if (i == 2)
            {
                return "2 " + Pool.ItemsBase[4200015].Name + "  and 50 M  Money.";
            }
            else if (i == 3)
            {
                return "1 " + Pool.ItemsBase[4200014].Name + "  and 25 M  Money.";
            }
            else if (i == 4)
            {
                return "1 " + Pool.ItemsBase[4200013].Name + "  and 15 M  Money.";
            }
            else if (i == 5)
            {
                return "1 " + Pool.ItemsBase[4200012].Name + "  and  5 M  Money.";
            }
            return "";
        }
        public static class Constants
        {
            public static readonly List<uint> ProtectMapSpells = new List<uint>() { };
            public static readonly ushort[] PriceUpdatePorf = new ushort[] { 36000, 36000, 36000, 36000, 36000, 36000, 18367, 12328, 7377, 6164, 3688, 3082, 3082, 3082, 3082, 2670, 1825, 1251, 866, 704 };
            public static readonly List<uint> MapCounterHits = new List<uint>() { 5052, 5061, 5062, 5063, 5064, 5065, 5066, 5053, 5054, 5055, 5056, 5057, 5058, 1005, 6000 };
            public static readonly List<uint> NoDropItems = new List<uint>() {1138 ,1764, 700, 3954, 3820 };
            public static readonly List<uint> SkillsNotAvailableHere = new List<uint>()
{
    5051, 5053, 5054, 5055, 5056, 5066,
    5057, 5058, 5059, 5060, 5061, 5062,
    5062, 5063, 5064, 5065, 22330, 22331,
    22332, 22333, 22334, 22335, 1518, 1508,
    6011, 20083, 20082, 20084, 20086
};
            public static readonly List<uint> RemoveRide = new List<uint>()
{
    5051, 5053, 5054, 5055, 5056, 5066,
    5057, 5058, 5059, 5060, 5061, 5062,
    5062, 5063, 5064, 5065, 22330, 22331,
    22332, 22333, 22334, 22335, 1518, 1508,
    6011, 20083, 20082, 20084, 20086
}.Concat(new List<uint>()
{
    20081, 20082, 20083, 20084, 3825, 6011, 5342, 3825, 6521, 9988, 
    6891, 5599, 8521, 1005, 700, 3053, 10230, 1860, 1511, 22330, 22331, 
    22332, 22333, 22334, 1860, 1858, 8881, 8880, 700
}).Distinct().ToList();
            public static readonly List<uint> FreePkMap = new List<uint>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 1138,12020, 12021, 12022, 12023, 12024, 12025,10001, 15757, 1483, 1518, 5052, 5061, 5062, 5063, 5064, 5065, 5066, 5051, 5053, 5054, 5055, 5056, 5057, 5058, 5053, 5054, 5055, 5056, 5057, 5058, 5052, 5061, 5062, 5063, 5064, 5065, 5066, 5053, 5054, 5055, 5056, 5057, 5058, 6011, 3998, 6784, 5342, 8009, 2353, 5339, 6964, 3071, 11447, 6546, 3935, 8437, 9988, 5599, 2071, 7357, 6521, 3825, 5342, 6891, 8521, 6525, 6729, 6000, 6001, 1505, 1005, 1038, 700, 1508/*PkWar*/, 1357, Game.MsgTournaments.MsgCaptureTheFlag.MapID };
            public static readonly List<uint> BlockAttackMap = new List<uint>() { 10001, 11447, 10445, 10446, 10447, 10448, 10449, 10450, 10451, 10452, 10453, 10454, 10455, 15757, 15758, 15759, 2353, 3825, 6072, 3830, 5040, 3820, 1004, 3831, 3832, 3834, 3826, 3827, 3828, 3829, 10, 9995, 1068, 4020, 4000, 4003, 4006, 4008, 4009, 1860, 1858, 1801, 1780, 1779/*Ghost Map*/, 9972, 1806, 3954, 3081, 1036, 1008, 601, 1006, 1511, 1039, 700, Game.MsgTournaments.MsgEliteGroup.WaitingAreaID, (uint)Game.MsgTournaments.MsgSteedRace.Maps.DungeonRace, (uint)Game.MsgTournaments.MsgSteedRace.Maps.IceRace, (uint)Game.MsgTournaments.MsgSteedRace.Maps.IslandRace, (uint)Game.MsgTournaments.MsgSteedRace.Maps.LavaRace, (uint)Game.MsgTournaments.MsgSteedRace.Maps.MarketRace };
            public static readonly List<uint> BlockTeleportMap = new List<uint>() { 10001, 22330, 22331, 22333, 22334, 22335, 5050, 5051, 5052, 5053, 5054, 5055, 5056, 5057, 5058, 5059, 5060, 5061, 5062, 5063, 5064, 5065, 5066, 5067, 5068, 5069, 5070, 601, 6000, 5040, 6001, 1005, 700, 1858, 1860, 3852, Game.MsgTournaments.MsgEliteGroup.WaitingAreaID, 1768 };
        }
        public unsafe class SendGlobalPacket
        {
            public unsafe void Enqueue(ServerSockets.Packet data)
            {

                var array = Pool.GamePoll.Values.ToArray();
                foreach (var user in Pool.GamePoll.Values)
                {
                    user.Send(data);
                }
            }
        }
        public static ConcurrentDictionary<uint, BotSystem> AIAutoHuntingPoll;
        public class LvLottery
        {
            public uint ITEMID;
            public string Name;
            public ulong HWID;
            public ulong LvBliss;
        }
        public static List<LvLottery> LotteryList = new List<LvLottery>(100);
        public static List<Smelt> SmeltingSuccesses = new List<Smelt>(10);
        public class Smelt
        {
            public string Prize, Name;
            public byte Furnace;
        }

        public static List<byte> SmeltingSessions = new List<byte>(10);
        public static string ReverseString(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = "";
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
        public static Dictionary<uint, HuntCoins> HuntCoinsMap = new Dictionary<uint, HuntCoins>();
        public class HuntCoins
        {
            public string  Name;
            public uint Coins;
            public int Rank;
        }
        public static Dictionary<uint, DeitylandSacrificeRanking> DeitylandSacrificeRankings = new Dictionary<uint, DeitylandSacrificeRanking>();
        public class DeitylandSacrificeRanking
        {
            public string Name;
            public uint Jades;
            public uint Rank;
            public uint UID;
        }
      
        public static Dictionary<string, DaityRanking> DaityRankings = new Dictionary<string, DaityRanking>();
        public class DaityRanking
        {
            public string Name;
            public uint JadeOFaaith;
        }
        public static string DaityRankingRewardName(int i)
        {
            switch (i)
            {
                case 1:
                    return "Random Yellow Rune && Random Blue Rune && 1000 Universal Rune Essence ";
                case 2:
                    return "900 Universal Rune Essence";
                case 3:
                    return "800 Universal Rune Essence";
                case 4:
                    return "500 Universal Rune Essence";
                case 5:
                    return "500 Universal Rune Essence";
                case 6:
                    return "500 Universal Rune Essence";
                case 7:
                    return "500 Universal Rune Essence";
                case 8:
                    return "500 Universal Rune Essence";
                case 9:
                    return "500 Universal Rune Essence";
                case 10:
                    return "500 Universal Rune Essence";
            }
           
            return "";
        }
        public static DateTime GoldenTreeExpirationDate = DateTime.FromBinary(0);
        public static uint GoldenTreeClaimed = 0;
        public static uint MaxAvaliableGoldenTreeClaim = 0;
        public static Dictionary<uint, Dictionary<uint, Dictionary<uint, Tuple<uint, uint, uint, string>>>> ClientAgates = new Dictionary<uint, Dictionary<uint, Dictionary<uint, Tuple<uint, uint, uint, string>>>>();
        public static Time32 smeltFloorStamp;
        public static uint lostlady = 0;
        public static List<Game.MsgServer.MsgMelterRankList.Instance> MelterRankList = new List<Game.MsgServer.MsgMelterRankList.Instance>();

        public static Cryptography.TransferCipher transferCipher;
        public static Role.Instance.Nobility.NobilityRanking NobilityRanking = new Role.Instance.Nobility.NobilityRanking();
        public static Role.Instance.ChiRank ChiRanking = new Role.Instance.ChiRank();
        public static Role.Instance.Flowers.FlowersRankingToday FlowersRankToday = new Role.Instance.Flowers.FlowersRankingToday();
        public static Role.Instance.Flowers.FlowerRanking GirlsFlowersRanking = new Role.Instance.Flowers.FlowerRanking();
        public static Role.Instance.Flowers.FlowerRanking BoysFlowersRanking = new Role.Instance.Flowers.FlowerRanking(false);
        public static DateTime ResetRandom = new DateTime();
        public static System.FastRandom Random = new System.FastRandom();
        public static System.SafeRandom GetRandom = new System.SafeRandom();
        public static System.RandomLite LiteRandom = new System.RandomLite();
        public static DateTime DragonIslandBansheeHour, DragonIslandSpookHour, DragonIslandNemsisHour;
        public static Dictionary<DBLevExp.Sort, Dictionary<byte, DBLevExp>> LevelInfo = new Dictionary<DBLevExp.Sort, Dictionary<byte, DBLevExp>>();
        public static ConcurrentDictionary<uint, TheCrimeTable> TheCrimePoll = new ConcurrentDictionary<uint, TheCrimeTable>();
        public static Database.ActivityTask ActivityTasks = new ActivityTask();
        public static InfoHeroReward TableHeroRewards = new InfoHeroReward();
        public static List<uint> RedeemActivated = new List<uint>();
        public static Dictionary<ushort, List<ushort>> WeaponSpells = new Dictionary<ushort, List<ushort>>();
        public static MagicType Magic = new MagicType();
        public static SubProfessionInfo SubClassInfo = new SubProfessionInfo();
        public static Dictionary<uint, Game.MsgMonster.MonsterFamily> MonsterFamilies = new Dictionary<uint, Game.MsgMonster.MonsterFamily>();
        public static ConcurrentDictionary<uint, List<string>> MonsterMap = new ConcurrentDictionary<uint, List<string>>();
        public static System.Counter ITEM_Counter = new System.Counter(1);
        public static System.Counter Inbox_Counter = new System.Counter(1);
        public static Rifinery RifineryItems;
        public static VirusX.CachedAttributeInvocation<System.Action<VirusX.Client.GameClient, VirusX.ServerSockets.Packet>, VirusX.PacketAttribute, ushort> MsgInvoker;
        public static InsultManager Insults = new InsultManager();
        public static RefinaryBoxes DBRerinaryBoxes;
        public static ItemType ItemsBase;
        public static MythTable.MythSoulEXP Item;
        public static MapDictionary<uint, Role.GameMap> ServerMaps;
        public static ConcurrentDictionary<uint, Client.GameClient> GamePoll; 
       
        public static ConcurrentDictionary<uint, Client.GameClient> DisconnectPool = new ConcurrentDictionary<uint, Client.GameClient>();
        public static ConcurrentDictionary<uint, Game.MsgNpc.Npc> NpcSever = new ConcurrentDictionary<uint, Game.MsgNpc.Npc>();
        public static ConcurrentDictionary<uint, Role.SobNpc> SobNpcSever = new ConcurrentDictionary<uint, Role.SobNpc>();
       
        public static List<int> NameUsed;
        public static RebornInfomations RebornInfo;
        public static ArenaTable Arena = new ArenaTable();
        public static SendGlobalPacket SendGlobalPackets;
        public static object TeamArena = new object();
        public static System.Counter ClientCounter = new System.Counter(1000000);
        public static System.Counter DominoCounter = new System.Counter(300000000);
        public static ConfiscatorTable QueueContainer = new ConfiscatorTable();
        public static System.SafeDictionary<uint, Game.MsgServer.MsgGameItem> ChatItems = new System.SafeDictionary<uint, Game.MsgServer.MsgGameItem>();
        public static int RandFromGivingNums(params int[] nums)
        {
            return nums[GetRandom.Next(0, nums.Length)];
        }
        public static uint RandFromGivingNums(uint[] nums)
        {
            if (nums == null || nums.Length == 0) return 0;
            return nums[GetRandom.Next(0, nums.Length)];
        }
    }
    public class Server
    {
        public static int DB = 300, Plus3Stone = 300, Parement = 300, ChiPack = 300, ToughDrill = 300, StarDrill = 300,
            RedRuneSelectionPack = 300, BlueRuneSelectionPack = 300, YellowRuneEssence = 300,
            BrightStarStone = 300,
              RadintStarStone = 300,
            DrainingTouchBooster = 300,
            BloodSpawnBooster = 300,
            Sturdiness = 300,
               FreeSoulBooster = 300,
               MPMaster = 300,
                  BossKiller = 300,
                            NoMercy = 300,
                            TortoiseBreaker = 300,
                            Healer = 300,
                            XPKiller = 300,
            Plus8Stone = 300,
              Grabber = 300,
              PotencyPtsBag = 300,
         YellowRuneEssence2 = 300,
                                     MeteorScrolls = 300,
                             FrozenChiPill = 300,
                             ArcaneEssence = 300,
                             NormalSoulStone = 300,
                              RefinedSoulStone = 300,
                             EliteSoulStone = 300,
                             SuperSoulStone = 300,
                           XPBooster = 300,
                           Plus6Steed = 300,
                           Plus6Stone = 300,
                           RefinedThunderGem = 300,
                           RefinedGloryGem = 300,
                           SuperThunderGem = 300,
                           SuperGloryGem = 300,
                          ExemptionToken = 300,
                           MiraculousGourd = 300,
                           WhiteFlower = 300,
                           RedFlower = 300,
                YellowRuneWitchery = 300,
                                         Bomb = 300,
                                         YinYangFruit = 300,
                                         LuckyAmulet = 300,
                                         MeteorTear = 300,
                                         PerformerCard = 300,
                                         Saddle = 300,
                                        Emerald = 300,
                                         HealthWine = 300,
                                      SuperProtectionPill = 300,
                            SeniorTrainingPill = 300,
                            StudyPoints = 300,
            YellowRuneFragment = 300;
        public static Extensions.Time32 ResetStamp = Extensions.Time32.Now.AddMilliseconds(ThreadFunctions.ResetDayStamp);

        public static bool FullLoading = false;
        public static WindowsAPI.IniFile Pets;
        public static void SendGlobalPacket(ServerSockets.Packet data)
        {
            var array = Pool.GamePoll.Values.ToArray();
            foreach (var user in Pool.GamePoll.Values)
            {
                user.Send(data);
            }
        }

        public static uint ResetServerDay = 0;

        public static unsafe void Reset(Extensions.Time32 Clock)
        {
            if (Clock > ResetStamp)
            {
                if (DateTime.Now.DayOfYear != ResetServerDay)
                {
                    try
                    {
                        Pool.Arena.ResetArena();
                        foreach (var flowerclient in Role.Instance.Flowers.ClientPoll.Values)
                        {
                            foreach (var flower in flowerclient)
                                flower.Amount2day = 0;
                        }
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                        {
                            foreach (var User in Database.RankMonster.HuntCoinsMap.Values)
                            {
                                Database.RankMonster.HuntCoinsMap.Remove(User.UID);
                            }
                        }
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            var array = Database.DeityAltar.DeityAltarS.Where(i => i.Value.Jades >= 30).OrderByDescending(i => i.Value.Jades).Take(10).ToArray();
                            for (int i = 0; i < array.Length; i++)
                            {
                                Client.GameClient client;
                                if (Pool.GamePoll.TryGetValue(array[i].Value.UID, out  client))
                                {
                                    switch (array[i].Value.Rank)
                                    {
                                        case 1:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 1", 30 * 24 * 60 * 60, 0, 0, 0, 30, 0);
                                                break;
                                            }
                                        case 2:
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 2", 30 * 24 * 60 * 60, 0, 0, 0, 31, 0);
                                            {
                                                break;
                                            }
                                        case 3:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 3", 30 * 24 * 60 * 60, 0, 0, 0, 32, 0);
                                                break;
                                            }
                                        case 4:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 4", 30 * 24 * 60 * 60, 0, 0, 0, 33, 0);
                                                break;
                                            }
                                        case 5:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 5", 30 * 24 * 60 * 60, 0, 0, 0, 34, 0);
                                                break;
                                            }
                                        case 6:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 6", 30 * 24 * 60 * 60, 0, 0, 0, 35, 0);
                                                break;
                                            }
                                        case 7:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 7", 30 * 24 * 60 * 60, 0, 0, 0, 36, 0);
                                                break;
                                            }
                                        case 8:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 8", 30 * 24 * 60 * 60, 0, 0, 0, 37, 0);
                                                break;
                                            }
                                        case 9:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 9", 30 * 24 * 60 * 60, 0, 0, 0, 38, 0);
                                                break;
                                            }
                                        case 10:
                                            {
                                                new PrizeInfo(client, "DeityAltar Reward", "Prize Rank", "You Rank " + array[i].Value.Rank + " This Prize For Rank 10", 30 * 24 * 60 * 60, 0, 0, 0, 39, 0);
                                                break;
                                            }
                                    }
                                }
                                else
                                {

                                }
                            }
                            Database.DeityAltar.DeityAltarS.Clear();
                            foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                            {
                                
                                
                                client.Player.TodayChampionPoints = 0;
                              
                                    client.Player.ChangeEpicTrojan = client.Player.ChangeArrayEpicTrojan =
                        client.Player.ChangeMr_MirrorEpicTrojan = client.Player.ChangeGeneralPakEpicTrojan = 0;
                                client.Player.CanChangeEpicMaterial = client.Player.CanChangeArrayEpicMaterial =
                                    client.Player.CanChangeMr_MirrorEpicMaterial = client.Player.CanChangGeneralPakMaterial = 1;

                                client.Player.UseChiToken = 0;
                                client.MyExchangeShop.Reset();
                                client.Player.RamdanBag = 0;
                                client.Player.TowerOfMysterychallenge = 3;
                                client.Player.TOMChallengeToday = 0;
                                client.Player.TowerOfMysteryChallengeFlag = 0;
                                client.Player.TOMSelectChallengeToday = 0;
                                client.Player.ClaimTowerAmulets = 0;
                                client.Player.TOMClaimTeamReward = 0;
                                client.Player.TOMRefreshReward = 0;
                                client.Player.QuestGUI.RemoveQuest(6126);




                                client.Player.OpenHousePack = 0;

                                if (client.Player.DailyMonth == 0)
                                    client.Player.DailyMonth = (byte)DateTime.Now.Month;
                                if (client.Player.DailyMonth != DateTime.Now.Month)
                                {
                                    client.Player.DailySignUpRewards = 0;
                                    client.Player.DailySignUpDays = 0;
                                    client.Player.DailyMonth = (byte)DateTime.Now.Month;
                                }
                                if (client.Player.MyJiangHu != null)
                                {
                                    client.Player.MyJiangHu.FreeCourse = 30000;
                                    client.Player.MyJiangHu.FreeTimeToday = 0;
                                    client.Player.MyJiangHu.RoundBuyPoints = 0;
                                }
                                client.Player.ArenaKills = client.Player.ArenaDeads = 0;
                                client.Player.HitShoot = client.Player.MisShoot = 0;

                                client.Player.DbTry = false;
                                client.Player.LotteryEntries = 0;
                                client.Player.Day = DateTime.Now.DayOfYear;
                                client.Player.BDExp = 0;
                                client.Player.TCCaptainTimes = 0;
                                client.Player.ExpBallUsed = 0;
                                client.DemonExterminator.FinishToday = 0;

                                if (client.Player.MyChi != null)
                                {
                                    client.Player.MyChi.ChiPoints = client.Player.MyChi.ChiPoints + 300;
                                    Game.MsgServer.MsgChiInfo.MsgHandleChi.SendInfo(client, Game.MsgServer.MsgChiInfo.Action.Upgrade);
                                }
                                client.Player.Flowers.FreeFlowers = 1;
                                foreach (var flower in client.Player.Flowers)
                                    flower.Amount2day = 0;


                                if (client.Player.Flowers.FreeFlowers > 0)
                                {
                                    client.Send(stream.FlowerCreate(Role.Core.IsBoy(client.Player.Body)
                                        ? Game.MsgServer.MsgFlower.FlowerAction.FlowerSender
                                        : Game.MsgServer.MsgFlower.FlowerAction.Flower
                                        , 0, 0, client.Player.Flowers.FreeFlowers));
                                }

                                if (client.Player.Level >= 90)
                                {
                                    client.Player.Enilghten = ServerDatabase.CalculateEnlighten(client.Player);
                                    client.Player.SendUpdate(stream, client.Player.Enilghten, Game.MsgServer.MsgUpdate.DataType.EnlightPoints);
                                }

                                client.Player.BuyKingdomDeeds = 0;
                                client.Player.QuestGUI.RemoveQuest(35024);
                                client.Player.QuestGUI.RemoveQuest(35007);
                                client.Player.QuestGUI.RemoveQuest(35025);
                                client.Player.QuestGUI.RemoveQuest(35028);
                                client.Player.QuestGUI.RemoveQuest(35034);

                                //---- reset Quests
                                client.Player.QuestGUI.RemoveQuest(6390);
                                client.Player.QuestGUI.RemoveQuest(6329);
                                client.Player.QuestGUI.RemoveQuest(6245);
                                client.Player.QuestGUI.RemoveQuest(6049);
                                client.Player.QuestGUI.RemoveQuest(6366);
                                client.Player.QuestGUI.RemoveQuest(6014);
                                client.Player.QuestGUI.RemoveQuest(2375);
                                client.Player.QuestGUI.RemoveQuest(6126);
                                client.Player.DailyHeavenChance = client.Player.DailyMagnoliaChance
                                    = client.Player.DailyMagnoliaItemId
                                    = client.Player.DailyHeavenChance = client.Player.DailySpiritBeadCount = client.Player.DailyRareChance = 0;
                                //
                            }
                            foreach (var User in Pool.DeitylandSacrificeRankings.Values)
                            {
                                Pool.DeitylandSacrificeRankings.Clear();
                            }
                        }
                        ResetServerDay = (uint)DateTime.Now.DayOfYear;
                    }
                    catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
                }
                ResetStamp.Value = Clock.Value + ThreadFunctions.ResetDayStamp;
            }
        }

        public static void SaveDBPayers()
        {
            if (!Program.ServerConfig.IsInterServer)
            {
                SaveDatabase();
            }
        }

        public static void Initialize()
        {
          
            Pool.AIAutoHuntingPoll = new ConcurrentDictionary<uint, Game.Ai.BotSystem>();
            Pool.ServerMaps = new MapDictionary<uint, Role.GameMap>();
            Pool.GamePoll = new ConcurrentDictionary<uint, Client.GameClient>();
            
            Pool.NameUsed = new List<int>();

            WindowsAPI.IniFile IniFile = new WindowsAPI.IniFile(System.IO.Directory.GetCurrentDirectory() + "\\shell.ini", true);
            Program.ServerConfig.IPAddres = IniFile.ReadString("ServerInfo", "AddresIP", "");
            Program.ServerConfig.GamePort = IniFile.ReadUInt16("ServerInfo", "Game_Port", 0);
            string DatabaseName = IniFile.ReadString("ServerInfo", "DatabaseName", "");
            string DatabasePass = IniFile.ReadString("ServerInfo", "DatabasePass", "");
            Program.ServerConfig.ServerName = IniFile.ReadString("ServerInfo", "ServerName", "");
            Program.ServerConfig.IsInterServer = IniFile.ReadBool("ServerInfo", "IsInterServer", false);

            Program.ServerConfig.Port_BackLog = IniFile.ReadUInt16("InternetPort", "BackLog", 100);
            Program.ServerConfig.Port_ReceiveSize = IniFile.ReadUInt16("InternetPort", "ReceiveSize", 4096);
            Program.ServerConfig.Port_SendSize = IniFile.ReadUInt16("InternetPort", "SendSize", 2048);

            Program.ServerConfig.DbLocation = IniFile.ReadString("Database", "Location", "");
            Program.ServerConfig.CO2Folder = IniFile.ReadString("Database", "CO2FOLDER", "");
            VirusX.DBFunctionality.DataHolder.CreateConnection(DatabaseName, DatabasePass);
            Pool.ITEM_Counter.Set(IniFile.ReadUInt32("Database", "ItemUID", 0));
            DB = IniFile.ReadInt32("CPFestival", "DB", 0);
            Plus3Stone = IniFile.ReadInt32("CPFestival", "Plus3Stone", 0);
            Parement = IniFile.ReadInt32("CPFestival", "Parement", 0);
            ChiPack = IniFile.ReadInt32("CPFestival", "ChiPack", 0);
            ToughDrill = IniFile.ReadInt32("CPFestival", "ToughDrill", 0);
            StarDrill = IniFile.ReadInt32("CPFestival", "StarDrill", 0);
            Pool.ClientCounter.Set(IniFile.ReadUInt32("Database", "ClientUID", 1000000));
            ResetServerDay = IniFile.ReadUInt32("Database", "Day", 0);
            Pool.DominoCounter.Set(IniFile.ReadUInt32("Database", "DominoUID", 300000000));
            Game.MsgTournaments.MsgSchedules.Create();
            Game.MsgTournaments.MsgSchedules.PkWar.WinnerUID = IniFile.ReadUInt32("Tournaments", "PkWarWinner", 0);
            Pets = new WindowsAPI.IniFile("Pets.ini");
            DatabaseLoad.Loading();
        }
        public static void SaveFestival()
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile(System.IO.Directory.GetCurrentDirectory() + "\\shell.ini");
            write.Write<int>("CPFestival", "DB", DB);
            write.Write<int>("CPFestival", "Plus3Stone", Plus3Stone);
            write.Write<int>("CPFestival", "Parement", Parement);
            write.Write<int>("CPFestival", "ChiPack", ChiPack);
            write.Write<int>("CPFestival", "ToughDrill", ToughDrill);
            write.Write<int>("CPFestival", "StarDrill", StarDrill);

        }

        public static void LoadMapName(uint id)
        {

            if (System.IO.File.Exists(Program.ServerConfig.DbLocation + "GameMapEx.ini"))
            {
                WindowsAPI.IniFile ini = new WindowsAPI.IniFile("GameMapEx.ini");
                Pool.ServerMaps[id].Name = ini.ReadString(id.ToString(), "Name", Program.ServerConfig.ServerName);
            }
        }

        
        public static bool Checkline(string m, string o)
        {
            if (System.IO.File.Exists(m))
            {
                if (new System.IO.FileInfo(m).Length != 0)
                {
                    string[] lines = System.IO.File.ReadAllLines(m);
                    foreach (var line in lines)
                    {
                        if (line == o)
                            return false;
                    }
                }
                return true;
            }
            return true;
        }
        public static void LoadExpInfo()
        {
            if (System.IO.File.Exists(Program.ServerConfig.DbLocation + "levexp.txt"))
            {
                using (System.IO.StreamReader read = System.IO.File.OpenText(Program.ServerConfig.DbLocation + "levexp.txt"))
                {
                    while (true)
                    {
                        string GetLine = read.ReadLine();
                        if (GetLine == null) return;
                        string[] line = GetLine.Split(' ');
                        DBLevExp exp = new DBLevExp();
                        exp.Action = (DBLevExp.Sort)byte.Parse(line[0]);
                        exp.Level = byte.Parse(line[1]);
                        exp.Experience = ulong.Parse(line[2]);
                        exp.UpLevTime = int.Parse(line[3]);
                        exp.MentorUpLevTime = int.Parse(line[4]);
                        if (!Pool.LevelInfo.ContainsKey(exp.Action))
                            Pool.LevelInfo.Add(exp.Action, new Dictionary<byte, DBLevExp>());
                        Pool.LevelInfo[exp.Action].Add(exp.Level, exp);
                    }
                }
            }
            GC.Collect();
        }
        public static void LoadMonsters()
        {
            try
            {

                WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Monsters\\"))
                {
                    ini.FileName = fname;
                    Game.MsgMonster.MonsterFamily Family = new Game.MsgMonster.MonsterFamily();
                    Family.ID = ini.ReadUInt32("cq_monstertype", "id", 0);
                    Family.Name = ini.ReadString("cq_monstertype", "name", "INVALID_MOB");

                    Family.Level = ini.ReadUInt16("cq_monstertype", "level", 0);
                    Family.MaxAttack = ini.ReadInt32("cq_monstertype", "attack_max", 0);
                    Family.MinAttack = ini.ReadInt32("cq_monstertype", "attack_min", 0);
                    if (Family.Name == "INVALID_MOB" || Family.Level == 0 || Family.ID == 0 || Family.MinAttack > Family.MaxAttack)
                    {
                        MyConsole.WriteLine("MONSTER FILE CORRUPT: \r\n" + fname + "\r\n");
                        continue;
                    }
                    Family.Defense = ini.ReadUInt16("cq_monstertype", "defence", 0);
                    Family.Mesh = ini.ReadUInt16("cq_monstertype", "lookface", 0);
                    Family.MaxHealth = ini.ReadInt32("cq_monstertype", "life", 0);
                    Family.ViewRange = 16;
                    Family.AttackRange = ini.ReadSByte("cq_monstertype", "attack_range", 0);
                    Family.Dodge = ini.ReadByte("cq_monstertype", "dodge", 0);
                    Family.DropBoots = ini.ReadByte("cq_monstertype", "drop_shoes", 0);
                    Family.DropNecklace = ini.ReadByte("cq_monstertype", "drop_necklace", 0);
                    Family.DropRing = ini.ReadByte("cq_monstertype", "drop_ring", 0);
                    Family.DropArmet = ini.ReadByte("cq_monstertype", "drop_armet", 0);
                    Family.DropArmor = ini.ReadByte("cq_monstertype", "drop_armor", 0);
                    Family.DropShield = ini.ReadByte("cq_monstertype", "drop_shield", 0);
                    Family.DropWeapon = ini.ReadByte("cq_monstertype", "drop_weapon", 0);
                    Family.DropMoney = ini.ReadUInt16("cq_monstertype", "drop_money", 0);
                    Family.DropHPItem = ini.ReadUInt32("cq_monstertype", "drop_hp", 0);
                    Family.DropMPItem = ini.ReadUInt32("cq_monstertype", "drop_mp", 0);
                    Family.Boss = ini.ReadByte("cq_monstertype", "Boss", 0);
                    Family.Defense2 = ini.ReadInt32("cq_monstertype", "defence2", 0);
                    if (Family.Boss != 0)
                        Family.AttackRange = 3;

                    Family.MoveSpeed = ini.ReadInt32("cq_monstertype", "move_speed", 0);
                    Family.AttackSpeed = ini.ReadInt32("cq_monstertype", "attack_speed", 0);
                    Family.SpellId = ini.ReadUInt32("cq_monstertype", "magic_type", 0);

                    Family.ExtraCritical = ini.ReadUInt32("cq_monstertype", "critical", 0);
                    Family.ExtraBreack = ini.ReadUInt32("cq_monstertype", "break", 0);

                    Family.extra_battlelev = ini.ReadInt32("cq_monstertype", "extra_battlelev", 0);
                    Family.extra_exp = ini.ReadInt32("cq_monstertype", "extra_exp", 0);
                    Family.extra_damage = ini.ReadInt32("cq_monstertype", "extra_damage", 0);


                    if (Family.Boss == 0 && Family.MaxAttack > 3000)
                    {
                        Family.MaxAttack = Family.MaxAttack / 2;
                        Family.MinAttack = Family.MinAttack / 2;
                    }

                    Family.DropSpecials = new Game.MsgMonster.SpecialItemWatcher[ini.ReadInt32("SpecialDrop", "Count", 0)];
                    for (int i = 0; i < Family.DropSpecials.Length; i++)
                    {
                        string[] Data = ini.ReadString("SpecialDrop", i.ToString(), "", 32).Split(',');

                        Family.DropSpecials[i] = new Game.MsgMonster.SpecialItemWatcher(uint.Parse(Data[0]), int.Parse(Data[1]));
                    }

                    Family.CreateItemGenerator();
                    Family.CreateMonsterSettings();
                    try
                    {
                        Pool.MonsterFamilies.Add(Family.ID, Family);
                    }
                    catch { MyConsole.WriteLine("Error In File " + fname); }
                }
                foreach (string fmap in System.IO.Directory.GetDirectories(Program.ServerConfig.DbLocation + "\\MonsterSpawns\\"))
                {
                    uint tMapID;
                    if (!uint.TryParse(fmap.Remove(0, (Program.ServerConfig.DbLocation + "\\MonsterSpawns\\").Length), out tMapID))
                        continue;
                    if (tMapID == 1038 || tMapID == 3935 || tMapID == 10137 || tMapID == 10250)
                        continue;
                    Role.GameMap.EnterMap((int)tMapID);
                    Game.MsgMonster.MobCollection colletion = new Game.MsgMonster.MobCollection(tMapID);
                    if (colletion.ReadMap())
                    {
                        foreach (string fmobtype in System.IO.Directory.GetDirectories(fmap))
                        {
                            foreach (string ffile in System.IO.Directory.GetFiles(fmobtype))
                            {
                                ini.FileName = ffile;
                                colletion.LocationSpawn = ffile;

                                uint ID = ini.ReadUInt32("cq_generator", "npctype", 0);
                                Game.MsgMonster.MonsterFamily famil;
                                if (!Pool.MonsterFamilies.TryGetValue(ID, out famil))
                                {
                                    continue;
                                }
                                if (Game.MsgMonster.MonsterRole.SpecialMonsters.Contains(famil.ID))
                                    continue;
                                Game.MsgMonster.MonsterFamily Monster = famil.Copy();

                                Monster.SpawnX = ini.ReadUInt16("cq_generator", "bound_x", 0);
                                Monster.SpawnY = ini.ReadUInt16("cq_generator", "bound_y", 0);
                                Monster.MaxSpawnX = (ushort)(Monster.SpawnX + ini.ReadUInt16("cq_generator", "bound_cx", 0));
                                Monster.MaxSpawnY = (ushort)(Monster.SpawnY + ini.ReadUInt16("cq_generator", "bound_cy", 0));
                                Monster.MapID = ini.ReadUInt32("cq_generator", "mapid", 0);
                                Monster.SpawnCount = ini.ReadByte("cq_generator", "max_per_gen", 0);
                                Monster.rest_secs = ini.ReadInt32("cq_generator", "rest_secs", 0);

                                if (Monster.MapID == 10133 || Monster.MapID == 10134)
                                    Monster.SpawnCount += 50;

                                if (!Pool.MonsterMap.ContainsKey(Monster.MapID))
                                    Pool.MonsterMap.Add(Monster.MapID, new List<string>());

                                if (!Pool.MonsterMap[Monster.MapID].Contains(Monster.Name))
                                {
                                    if (Monster.Settings != Game.MsgMonster.MonsterSettings.Guard)
                                        Pool.MonsterMap[Monster.MapID].Add(Monster.Name);
                                }
                                colletion.Add(Monster);

                            }
                        }
                    }
                }
                LoadMapMonsters("Monsters1002.txt");
                LoadMapMonsters("Monsters3998.txt");
                LoadMapMonsters("Monsters1011.txt");
                LoadMapMonsters("Monsters1015.txt");
                LoadMapMonsters("Monsters1020.txt");
                LoadMapMonsters("Monsters10137.txt");
                LoadMapMonsters("Monsters10250.txt");
                LoadMapMonsters("Monsters3935.txt");

                GC.Collect();
            }
            catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
        }
        public static void LoadMapMonsters(string file)
        {
            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
            if (System.IO.File.Exists(Program.ServerConfig.DbLocation + "MonsterSpawns\\" + file + ""))
            {
                using (System.IO.StreamReader read = System.IO.File.OpenText(Program.ServerConfig.DbLocation + "MonsterSpawns\\" + file + ""))
                {
                    while (true)
                    {

                        string aline = read.ReadLine();
                        if (aline != null && aline != "")
                        {
                            try
                            {

                                string[] line = aline.Split(',');

                                uint body = uint.Parse(line[1]);
                                string name = line[2];

                                ushort X = ushort.Parse(line[3]);
                                ushort Y = ushort.Parse(line[4]);
                                uint Map = uint.Parse(line[5]);


                                if (Pool.ServerMaps.ContainsKey(Map))
                                {
                                    var GMap = Pool.ServerMaps[Map];
                                    if (GMap.MonstersColletion == null)
                                    {
                                        GMap.MonstersColletion = new Game.MsgMonster.MobCollection(GMap.ID);
                                    }
                                    else if (GMap.MonstersColletion.DMap == null)
                                        GMap.MonstersColletion.DMap = GMap;
                                    foreach (var _monster in Pool.MonsterFamilies.Values)
                                    {
                                        if (_monster.Name == name)
                                        {



                                            if (Game.MsgMonster.MonsterRole.SpecialMonsters.Contains(_monster.ID))
                                                break;
                                            Game.MsgMonster.MonsterFamily Monster = _monster.Copy();

                                            Monster.SpawnX = X;
                                            Monster.SpawnY = Y;
                                            Monster.MaxSpawnX = (ushort)(X + 1);
                                            Monster.MaxSpawnY = (ushort)(Y + 1);
                                            Monster.MapID = GMap.ID;
                                            Monster.SpawnCount = 1;
                                            if (!Pool.MonsterMap.ContainsKey(Monster.MapID))
                                                Pool.MonsterMap.Add(Monster.MapID, new List<string>());

                                            if (!Pool.MonsterMap[Monster.MapID].Contains(Monster.Name))
                                            {
                                                if (Monster.Settings != Game.MsgMonster.MonsterSettings.Guard)
                                                    Pool.MonsterMap[Monster.MapID].Add(Monster.Name);
                                            }
                                            Game.MsgMonster.MonsterRole rolemonster = GMap.MonstersColletion.Add(Monster, false, 0, Map != 10166);
                                            break;
                                        }

                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                                break;
                            }
                        }
                        else
                            break;

                    }
                }
            }
        }
        public static void LoadMonstersTest()
        {
            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Monsters\\"))
            {
                ini.FileName = fname;
                Game.MsgMonster.MonsterFamily Family = new Game.MsgMonster.MonsterFamily();
                Family.Name = ini.ReadString("cq_monstertype", "name", "INVALID_MOB");
                uint ID = Game.MsgMonster.MonsterID.GetID(Family.Name);
                ini.Write<uint>("cq_monstertype", "id", ID);
            }

        }
        public static void LoadNewMonsters(uint id)
        {
            try
            {
                Game.MsgMonster.MobCollection colletion = new Game.MsgMonster.MobCollection(id);
                if (colletion.ReadMap())
                {
                    string[] text = System.IO.File.ReadAllLines(Program.ServerConfig.DbLocation + "MonsterSpawns\\NewMonster.txt");
                    foreach (string line in text)
                    {
                        string[] data = line.Split(' ');
                        try
                        {
                            ushort MapID = ushort.Parse(data[1]);
                            if (id != MapID) continue;
                            ushort X = (ushort)long.Parse(data[2]);
                            ushort Y = (ushort)long.Parse(data[3]);
                            ushort XPlus = (ushort)long.Parse(data[4]);
                            ushort YPlus = (ushort)long.Parse(data[5]);
                            uint respawn = (uint)long.Parse(data[6]);
                            int Amount = (int)long.Parse(data[7]);
                            uint monsterID = (uint)long.Parse(data[8]);
                            Game.MsgMonster.MonsterFamily famil;
                            if (!Pool.MonsterFamilies.TryGetValue(monsterID, out famil))
                            {
                                continue;
                            }
                            if (Game.MsgMonster.MonsterRole.SpecialMonsters.Contains(famil.ID))
                                continue;
                            Game.MsgMonster.MonsterFamily Monster = famil.Copy();
                            Monster.SpawnX = X;
                            Monster.SpawnY = Y;
                            Monster.MaxSpawnX = (ushort)(X + XPlus);
                            Monster.MaxSpawnY = (ushort)(Y + YPlus);
                            Monster.MapID = MapID;
                            Monster.SpawnCount = (byte)Amount;
                            Monster.rest_secs = (int)respawn;
                            colletion.Add(Monster);
                        }
                        catch
                        {
                        }


                    }
                }

                GC.Collect();
            }
            catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
        }
        public static void LoadPortals(uint id = 0)
        {

         if (System.IO.File.Exists(Program.ServerConfig.DbLocation + "portals.ini"))
            {
                using (System.IO.StreamReader read = System.IO.File.OpenText(Program.ServerConfig.DbLocation + "portals.ini"))
                {
                    ushort count = 0;
                    while (true)
                    {
                        string lines = read.ReadLine();
                        if (lines == null)
                            break;
                        ushort Map = ushort.Parse(lines.Split('[')[1].ToString().Split(']')[0]);
                        ushort Count = ushort.Parse(read.ReadLine().Split('=')[1]);
                        for (ushort x = 0; x < Count; x++)
                        {
                            Role.Portal portal = new Role.Portal();
                            string[] line = read.ReadLine().Split('=')[1].Split(' ');
                            portal.MapID = ushort.Parse(line[0]);
                            portal.X = ushort.Parse(line[1]);
                            portal.Y = ushort.Parse(line[2]);

                            string[] dline = read.ReadLine().Split('=')[1].Split(' ');
                            portal.Destiantion_MapID = ushort.Parse(dline[0]);
                            portal.Destiantion_X = ushort.Parse(dline[1]);
                            portal.Destiantion_Y = ushort.Parse(dline[2]);
                            if (id != 0 && id != Map) continue;
                            if (Pool.ServerMaps.ContainsKey(portal.MapID))
                                Pool.ServerMaps[portal.MapID].Portals.Add(portal);
                            count++;
                        }
                    }
                }
            }
            GC.Collect();
        }

        public unsafe static void AddMapMonster(ServerSockets.Packet stream, Role.GameMap map, uint ID, ushort x, ushort y, ushort max_x, ushort max_y, byte count, uint DinamicID = 0, bool RemoveOnDead = true
        , Game.MsgFloorItem.MsgItemPacket.EffectMonsters m_effect = Game.MsgFloorItem.MsgItemPacket.EffectMonsters.None, string streffect = "")
        {
            if (map.MonstersColletion == null)
            {
                map.MonstersColletion = new Game.MsgMonster.MobCollection(map.ID);
            }
            if (map.MonstersColletion.ReadMap())
            {

                Game.MsgMonster.MonsterFamily famil;
                if (Pool.MonsterFamilies.TryGetValue(ID, out famil))
                {
                    Game.MsgMonster.MonsterFamily Monster = famil.Copy();

                    Monster.SpawnX = x;
                    Monster.SpawnY = y;
                    Monster.MaxSpawnX = (ushort)(x + max_x);
                    Monster.MaxSpawnY = (ushort)(y + max_y);
                    Monster.MapID = map.ID;
                    Monster.SpawnCount = count;
                    Game.MsgMonster.MonsterRole rolemonster = map.MonstersColletion.Add(Monster, RemoveOnDead, DinamicID, true);

                    if (rolemonster == null)
                    {
                        Console.WriteLine("Eror monster spawn. Server.");
                        return;
                    }

                    Game.MsgServer.ActionQuery action = new Game.MsgServer.ActionQuery()
                    {
                        ObjId = rolemonster.UID,
                        Type = Game.MsgServer.ActionType.RemoveEntity
                    };
                    rolemonster.Send(stream.ActionCreate(action));
                    rolemonster.Send(rolemonster.GetArray(stream, false));

                    if (streffect != null)
                    {
                        rolemonster.SendString(stream, MsgStringPacket.StringID.Effect, streffect);
                    }



                    if (m_effect != Game.MsgFloorItem.MsgItemPacket.EffectMonsters.None && rolemonster != null)
                    {
                        Game.MsgFloorItem.MsgItemPacket effect = Game.MsgFloorItem.MsgItemPacket.Create();
                        effect.m_UID = (uint)m_effect;
                        effect.m_X = rolemonster.X;
                        effect.m_Y = rolemonster.Y;
                        effect.DropType = MsgDropID.Earth;
                        rolemonster.Send(stream.ItemPacketCreate(effect));
                        rolemonster.SendString(stream, Game.MsgServer.MsgStringPacket.StringID.Effect, "glebesword");
                    }
                    if (rolemonster.HitPoints > 65535)
                    {
                        Game.MsgServer.MsgUpdate Upd = new Game.MsgServer.MsgUpdate(stream, rolemonster.UID, 2);
                        stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.MaxHitpoints, rolemonster.Family.MaxHealth);
                        stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.Hitpoints, rolemonster.HitPoints);
                        stream = Upd.GetArray(stream);
                        rolemonster.Send(stream);
                    }
                }
            }
        }
        public unsafe static void TestMonster(ServerSockets.Packet stream, Role.GameMap map,ushort Mesh, uint ID, ushort x, ushort y, ushort max_x, ushort max_y, byte count, uint DinamicID = 0, bool RemoveOnDead = true
       , Game.MsgFloorItem.MsgItemPacket.EffectMonsters m_effect = Game.MsgFloorItem.MsgItemPacket.EffectMonsters.None, string streffect = "")
        {
            if (map.MonstersColletion == null)
            {
                map.MonstersColletion = new Game.MsgMonster.MobCollection(map.ID);
            }
            if (map.MonstersColletion.ReadMap())
            {

                Game.MsgMonster.MonsterFamily famil;
                if (Pool.MonsterFamilies.TryGetValue(ID, out famil))
                {
                    Game.MsgMonster.MonsterFamily Monster = famil.Copy();

                    Monster.SpawnX = x;
                    Monster.SpawnY = y;
                    Monster.MaxSpawnX = (ushort)(x + max_x);
                    Monster.MaxSpawnY = (ushort)(y + max_y);
                    Monster.MapID = map.ID;
                    Monster.SpawnCount = count;
                    Monster.Mesh = Mesh;
                    Game.MsgMonster.MonsterRole rolemonster = map.MonstersColletion.Add(Monster, RemoveOnDead, DinamicID, true);

                    if (rolemonster == null)
                    {
                        Console.WriteLine("Eror monster spawn. Server.");
                        return;
                    }

                    Game.MsgServer.ActionQuery action = new Game.MsgServer.ActionQuery()
                    {
                        ObjId = rolemonster.UID,
                        Type = Game.MsgServer.ActionType.RemoveEntity
                    };
                    rolemonster.Send(stream.ActionCreate(action));
                    rolemonster.Send(rolemonster.GetArray(stream, false));

                    if (streffect != null)
                    {
                        rolemonster.SendString(stream, MsgStringPacket.StringID.Effect, streffect);
                    }



                    if (m_effect != Game.MsgFloorItem.MsgItemPacket.EffectMonsters.None && rolemonster != null)
                    {
                        Game.MsgFloorItem.MsgItemPacket effect = Game.MsgFloorItem.MsgItemPacket.Create();
                        effect.m_UID = (uint)m_effect;
                        effect.m_X = rolemonster.X;
                        effect.m_Y = rolemonster.Y;
                        effect.DropType = MsgDropID.Earth;
                        rolemonster.Send(stream.ItemPacketCreate(effect));
                        rolemonster.SendString(stream, Game.MsgServer.MsgStringPacket.StringID.Effect, "glebesword");
                    }
                    if (rolemonster.HitPoints > 65535)
                    {
                        Game.MsgServer.MsgUpdate Upd = new Game.MsgServer.MsgUpdate(stream, rolemonster.UID, 2);
                        stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.MaxHitpoints, rolemonster.Family.MaxHealth);
                        stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.Hitpoints, rolemonster.HitPoints);
                        stream = Upd.GetArray(stream);
                        rolemonster.Send(stream);
                    }
                }
            }
        }
        public unsafe static bool AddFloor(ServerSockets.Packet stream, Role.GameMap map, uint ID, ushort x, ushort y, ushort spelllevel, Database.MagicType.Magic dbspell, Client.GameClient Owner, uint GuildID, uint OwnerUID, uint DinamicID = 0, string Name = "", bool RemoveOnDead = true)
        {
            try
            {
                if (map.MonstersColletion == null)
                {
                    map.MonstersColletion = new Game.MsgMonster.MobCollection(map.ID);
                }
                if (map.MonstersColletion.ReadMap())
                {

                    Game.MsgMonster.MonsterFamily famil;
                    if (Pool.MonsterFamilies.TryGetValue(1, out famil))
                    {
                        Game.MsgMonster.MonsterFamily Monster = famil.Copy();

                        Monster.SpawnX = x;
                        Monster.SpawnY = y;
                        Monster.MaxSpawnX = (ushort)(x + 1);
                        Monster.MaxSpawnY = (ushort)(y + 1);
                        Monster.MapID = map.ID;
                        Monster.SpawnCount = 1;
                        Game.MsgMonster.MonsterRole rolemonster = map.MonstersColletion.Add(Monster, RemoveOnDead, DinamicID, true);
                        if (rolemonster == null)
                        {
                           
                            return false;
                        }
                        rolemonster.Family.ID = ID;
                        rolemonster.IsFloor = true;
                        rolemonster.FloorStampTimer = DateTime.Now.AddSeconds(7);
                        rolemonster.Family.Settings = Game.MsgMonster.MonsterSettings.Lottus;

                        rolemonster.FloorPacket = new MsgItemPacket();
                        rolemonster.FloorPacket.m_UID = rolemonster.UID;
                        rolemonster.FloorPacket.m_ID = ID;
                        rolemonster.FloorPacket.m_X = x;
                        rolemonster.FloorPacket.m_Y = y;
                        rolemonster.FloorPacket.MaxLife = 25;
                        rolemonster.FloorPacket.Life = 25;
                        rolemonster.FloorPacket.DropType = MsgDropID.Effect;
                        rolemonster.FloorPacket.m_Color = 13;
                        rolemonster.FloorPacket.ItemOwnerUID = OwnerUID;
                        rolemonster.FloorPacket.GuildID = GuildID;
                        rolemonster.FloorPacket.FlowerType = 2;
                        rolemonster.FloorPacket.Timer = Role.Core.TqTimer(rolemonster.FloorStampTimer);
                        rolemonster.FloorPacket.Name = Name;

                        rolemonster.DBSpell = dbspell;
                        rolemonster.Family.MaxHealth = 25;
                        rolemonster.HitPoints = 25;
                        rolemonster.OwnerFloor = Owner;
                        rolemonster.SpellLevel = spelllevel;

                        if (rolemonster == null)
                        {
                            Console.WriteLine("Eror monster spawn. Server.");
                            return false;
                        }
                        map.View.EnterMap<Role.IMapObj>(rolemonster);
                        rolemonster.Send(rolemonster.GetArray(stream, false));
                        return true;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return false;

        }

        public unsafe static void LoadDatabase()
        {
            try
            {
                foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                {
                    WindowsAPI.IniFile IniFile = new WindowsAPI.IniFile(fname);
                    IniFile.FileName = fname;
                    string name = IniFile.ReadString("Character", "Name", "");
                    Pool.NameUsed.Add(name.GetHashCode());
                }
            }
            catch (Exception e)
            {
                MyConsole.WriteException(e);
            }
        }
       
        public unsafe static void SaveDatabase()
        {
           //213.186.60.100
            if (!FullLoading)
                return;
            try
            {

                Role.Instance.Clan.ProcessChangeNames();
                Role.Instance.Guild.ProcessChangeNames();
              
                Save(new Action(Database.JianHuTable.SaveJiangHu));
                Save(new Action(Role.Instance.Associate.Save));
                Save(new Action(Database.GuildTable.Save));
                Save(new Action(Database.ClanTable.Save));
                Save(new Action(Pool.QueueContainer.Save));
                Save(new Action(Game.MsgTournaments.MsgSchedules.GuildWar.Save));
                Save(new Action(Game.MsgTournaments.MsgSchedules.SuperGuildWar.Save));
                Save(new Action(TheCrimeTable.Save));
                Save(new Action(Pool.Arena.Save));
                Save(new Action(YuanshenRank.Save));
                Save(new Action(HWRank.Save));
                Save(new Action(NinjaRank.Save));
                Save(new Action(WarriorRank.Save));
                Save(new Action(ArcherRank.Save));
                Save(new Action(WaterRank.Save));
                Save(new Action(MonkRanks.Save));
                Save(new Action(FireRank.Save));
                Save(new Action(PirateRank.Save));
                Save(new Action(LeeLongRank.Save));
                Save(new Action(DuneWandererRank.Save));
                Save(new Action(AstredgeRank.Save));
                Save(new Action(HeroGatheringRank.Save));
                Save(new Action(Game.MsgTournaments.MsgSchedules.ClassPkWar.Save));
                Save(new Action(Game.MsgTournaments.MsgSchedules.ElitePkTournament.Save));
                Save(new Action(Game.MsgTournaments.MsgSchedules.TeamPkTournament.Save));
                Save(new Action(Game.MsgTournaments.MsgSchedules.SkillTeamPkTournament.Save));
                Save(new Action((Game.MsgTournaments.MsgSchedules.Tournaments[Game.MsgTournaments.TournamentType.BattleField]
                    as Game.MsgTournaments.MsgBattleField).Save));
                Save(new Action(SystemBanned.Save));
                Save(new Action(TableBetDiceRank.Save));
                Save(new Action(SystemBannedPC.Save));
                Save(new Action(RankMonster.Save));
                Save(new Action(SystemBannedAccount.Save));
                Save(new Action(InnerPowerTable.Save));
                Save(new Action(ShareVIP.Save));
                Save(new Action(VoteSystem.Save));
                Save(new Action(BotJail.Save));
                Save(new Action(Database.StarDragonBall.Save));
                Save(new Action(LeagueTable.Save));
                Save(new Action(RechargeShop.Save));
                Save(new Action(DeityAltar.Save));
                //Save(new Action(Game.MsgTournaments.MsgSchedules.ClanWar.Save));
                Save(new Action(RankItems.SaveRanks));
                Save(new Action(Role.Statue.Save));
                Save(new Action(PrestigeRanking.Save));
                Save(new Action(Role.KOBoard.KOBoardRanking.Save));
                Save(new Action(MsgInterServer.Instance.CrossElitePKTournament.Save));
                Save(new Action(Pool.Insults.Save));
                Save(new Action(RuneRank.Save));
                Save(new Action(SaveFestival));
                WindowsAPI.IniFile IniFile = new WindowsAPI.IniFile("");
                IniFile.FileName = System.IO.Directory.GetCurrentDirectory() + "\\shell.ini";
                IniFile.Write<uint>("Database", "ItemUID", Pool.ITEM_Counter.Count);
                IniFile.Write<uint>("Database", "DominoUID", Pool.DominoCounter.Count);
                IniFile.Write<uint>("Database", "ClientUID", Pool.ClientCounter.Count);
                IniFile.Write<uint>("Database", "Day", ResetServerDay);
                IniFile.Write<uint>("Tournaments", "PkWarWinner", Game.MsgTournaments.MsgSchedules.PkWar.WinnerUID);
               
              
            }
            catch (Exception e) { MyConsole.WriteException(e); }
        }

        public static void Save(Action obj)
        {
            try
            {
                obj.Invoke();
            }
            catch (Exception e) { MyConsole.SaveException(e); }
        }

    }
}