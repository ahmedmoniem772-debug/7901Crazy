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
    public class DatabaseLoad
    {
        public static void Loading()
        {
            Time32 now = Time32.Now;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                    LOADING DATABASE                            ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine("");

            int totalItems = 0;
            System.Diagnostics.Stopwatch totalSw = System.Diagnostics.Stopwatch.StartNew();

            // Helper method for logging
            void LogStart(string systemName)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"[{DateTime.Now:HH:mm:ss}] Loading {systemName}... ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }

            void LogComplete(string systemName, int count = -1, long ms = -1, string label = "items")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (count >= 0 && ms >= 0)
                    Console.WriteLine($"DONE ({ms}ms) - {count} {label}");
                else if (count >= 0)
                    Console.WriteLine($"DONE - {count} {label}");
                else if (ms >= 0)
                    Console.WriteLine($"DONE ({ms}ms)");
                else
                    Console.WriteLine("DONE");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }

            void LogError(string systemName, Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR loading {systemName}: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }

            // ========================================================================
            // INITIALIZATION
            // ========================================================================
            LogStart("Pool.MsgInvoker");
            try { Pool.MsgInvoker = new VirusX.CachedAttributeInvocation<System.Action<VirusX.Client.GameClient, VirusX.ServerSockets.Packet>, VirusX.PacketAttribute, ushort>(PacketAttribute.Translator); LogComplete("Pool.MsgInvoker"); }
            catch (Exception ex) { LogError("Pool.MsgInvoker", ex); }

            // ========================================================================
            // REBORN & ITEMS
            // ========================================================================
            LogStart("Pool.RebornInfo");
            try { Pool.RebornInfo = new RebornInfomations(); Pool.RebornInfo.Load(); LogComplete("Pool.RebornInfo"); }
            catch (Exception ex) { LogError("Pool.RebornInfo", ex); }

            LogStart("Pool.ItemsBase");
            try { Pool.ItemsBase = new ItemType(); LogComplete("Pool.ItemsBase"); }
            catch (Exception ex) { LogError("Pool.ItemsBase", ex); }

            LogStart("Pool.RifineryItems");
            try { Pool.RifineryItems = new Rifinery(); LogComplete("Pool.RifineryItems"); }
            catch (Exception ex) { LogError("Pool.RifineryItems", ex); }

            LogStart("Pool.DBRerinaryBoxes");
            try { Pool.DBRerinaryBoxes = new RefinaryBoxes(); LogComplete("Pool.DBRerinaryBoxes"); }
            catch (Exception ex) { LogError("Pool.DBRerinaryBoxes", ex); }

            LogStart("ItemsBase.Loading");
            try { Pool.ItemsBase.Loading(); totalItems += Pool.ItemsBase.Count; LogComplete("ItemsBase.Loading", Pool.ItemsBase.Count); }
            catch (Exception ex) { LogError("ItemsBase.Loading", ex); }

            LogStart("ActionHelper.Create");
            try { ActionHelper.Create(); LogComplete("ActionHelper.Create"); }
            catch (Exception ex) { LogError("ActionHelper.Create", ex); }

            // ========================================================================
            // SHOPS
            // ========================================================================
            LogStart("Shops.ChampionShop");
            try { Database.Shops.ChampionShop.Load(); LogComplete("Shops.ChampionShop"); }
            catch (Exception ex) { LogError("Shops.ChampionShop", ex); }

            LogStart("Shops.EShopFile");
            try { Database.Shops.EShopFile.Load(); LogComplete("Shops.EShopFile"); }
            catch (Exception ex) { LogError("Shops.EShopFile", ex); }

            LogStart("Shops.EShopV2File");
            try { Database.Shops.EShopV2File.Load(); LogComplete("Shops.EShopV2File"); }
            catch (Exception ex) { LogError("Shops.EShopV2File", ex); }

            LogStart("Shops.HonorShop");
            try { Database.Shops.HonorShop.Load(); LogComplete("Shops.HonorShop"); }
            catch (Exception ex) { LogError("Shops.HonorShop", ex); }

            LogStart("Shops.RacePointShop");
            try { Database.Shops.RacePointShop.Load(); LogComplete("Shops.RacePointShop"); }
            catch (Exception ex) { LogError("Shops.RacePointShop", ex); }

            LogStart("Shops.ShopFile");
            try { Database.Shops.ShopFile.Load(); LogComplete("Shops.ShopFile"); }
            catch (Exception ex) { LogError("Shops.ShopFile", ex); }

            // ========================================================================
            // RANKINGS
            // ========================================================================
            LogStart("Roulettes");
            try { Database.Roulettes.Load(); LogComplete("Roulettes"); }
            catch (Exception ex) { LogError("Roulettes", ex); }

            LogStart("NinjaRank");
            try { Database.NinjaRank.Load(); LogComplete("NinjaRank"); }
            catch (Exception ex) { LogError("NinjaRank", ex); }

            LogStart("WarriorRank");
            try { Database.WarriorRank.Load(); LogComplete("WarriorRank"); }
            catch (Exception ex) { LogError("WarriorRank", ex); }

            LogStart("ArcherRank");
            try { Database.ArcherRank.Load(); LogComplete("ArcherRank"); }
            catch (Exception ex) { LogError("ArcherRank", ex); }

            LogStart("LeeLongRank");
            try { Database.LeeLongRank.Load(); LogComplete("LeeLongRank"); }
            catch (Exception ex) { LogError("LeeLongRank", ex); }

            LogStart("DuneWandererRank");
            try { Database.DuneWandererRank.Load(); LogComplete("DuneWandererRank"); }
            catch (Exception ex) { LogError("DuneWandererRank", ex); }

            LogStart("MonkRanks");
            try { Database.MonkRanks.Load(); LogComplete("MonkRanks"); }
            catch (Exception ex) { LogError("MonkRanks", ex); }

            LogStart("WaterRank");
            try { Database.WaterRank.Load(); LogComplete("WaterRank"); }
            catch (Exception ex) { LogError("WaterRank", ex); }

            LogStart("AstredgeRank");
            try { Database.AstredgeRank.Load(); LogComplete("AstredgeRank"); }
            catch (Exception ex) { LogError("AstredgeRank", ex); }

            LogStart("FireRank");
            try { Database.FireRank.Load(); LogComplete("FireRank"); }
            catch (Exception ex) { LogError("FireRank", ex); }

            LogStart("PirateRank");
            try { Database.PirateRank.Load(); LogComplete("PirateRank"); }
            catch (Exception ex) { LogError("PirateRank", ex); }

            LogStart("RuneRank");
            try { Database.RuneRank.Load(); LogComplete("RuneRank"); }
            catch (Exception ex) { LogError("RuneRank", ex); }

            LogStart("HWRank");
            try { Database.HWRank.Load(); LogComplete("HWRank"); }
            catch (Exception ex) { LogError("HWRank", ex); }

            LogStart("PrestigeRanking");
            try { Database.PrestigeRanking.Load(); LogComplete("PrestigeRanking"); }
            catch (Exception ex) { LogError("PrestigeRanking", ex); }

            LogStart("HeroGatheringRank");
            try { HeroGatheringRank.Load(); LogComplete("HeroGatheringRank"); }
            catch (Exception ex) { LogError("HeroGatheringRank", ex); }

            LogStart("KOBoardRanking");
            try { Role.KOBoard.KOBoardRanking.Load(); LogComplete("KOBoardRanking"); }
            catch (Exception ex) { LogError("KOBoardRanking", ex); }

            LogStart("TableBetDiceRank");
            try { Database.TableBetDiceRank.Load(); LogComplete("TableBetDiceRank"); }
            catch (Exception ex) { LogError("TableBetDiceRank", ex); }

            LogStart("RankItems");
            try { RankItems.LoadAllItems(); LogComplete("RankItems"); }
            catch (Exception ex) { LogError("RankItems", ex); }

            // ========================================================================
            // SYNDICATE & GUILD
            // ========================================================================
            LogStart("syndicate_level");
            try { syndicate_level.Load(); LogComplete("syndicate_level"); }
            catch (Exception ex) { LogError("syndicate_level", ex); }

            LogStart("syn_formtype");
            try { syn_formtype.Load(); LogComplete("syn_formtype"); }
            catch (Exception ex) { LogError("syn_formtype", ex); }

            LogStart("cq_syn_skill_type");
            try { cq_syn_skill_type.Load(); LogComplete("cq_syn_skill_type"); }
            catch (Exception ex) { LogError("cq_syn_skill_type", ex); }

            // ========================================================================
            // BANS & VIP
            // ========================================================================
            LogStart("SystemBanned");
            try { SystemBanned.Load(); LogComplete("SystemBanned"); }
            catch (Exception ex) { LogError("SystemBanned", ex); }

            LogStart("SystemBannedAccount");
            try { SystemBannedAccount.Load(); LogComplete("SystemBannedAccount"); }
            catch (Exception ex) { LogError("SystemBannedAccount", ex); }

            LogStart("SystemBannedPC");
            try { SystemBannedPC.Load(); LogComplete("SystemBannedPC"); }
            catch (Exception ex) { LogError("SystemBannedPC", ex); }

            LogStart("ShareVIP");
            try { Database.ShareVIP.Load(); LogComplete("ShareVIP"); }
            catch (Exception ex) { LogError("ShareVIP", ex); }

            // ========================================================================
            // SERVER & MAPS
            // ========================================================================
            LogStart("Server.LoadExpInfo");
            try { Server.LoadExpInfo(); LogComplete("Server.LoadExpInfo"); }
            catch (Exception ex) { LogError("Server.LoadExpInfo", ex); }

            LogStart("DataCore.AtributeStatus");
            try { DataCore.AtributeStatus.Load(); LogComplete("DataCore.AtributeStatus"); }
            catch (Exception ex) { LogError("DataCore.AtributeStatus", ex); }

            LogStart("GameMap.LoadMaps");
            try { Role.GameMap.LoadMaps(); LogComplete("GameMap.LoadMaps", Pool.ServerMaps.Count, -1, "maps"); }
            catch (Exception ex) { LogError("GameMap.LoadMaps", ex); }

            LogStart("YuanshenAttr");
            try { YuanshenAttr.Load(); LogComplete("YuanshenAttr"); }
            catch (Exception ex) { LogError("YuanshenAttr", ex); }

            LogStart("YuanshenLevUP");
            try { YuanshenLevUP.Load(); LogComplete("YuanshenLevUP"); }
            catch (Exception ex) { LogError("YuanshenLevUP", ex); }

            LogStart("YuanshenLottery");
            try { YuanshenLottery.Load(); LogComplete("YuanshenLottery"); }
            catch (Exception ex) { LogError("YuanshenLottery", ex); }

            LogStart("YuanshenRank");
            try { YuanshenRank.Load(); LogComplete("YuanshenRank"); }
            catch (Exception ex) { LogError("YuanshenRank", ex); }

            LogStart("PokerLoad");
            try { Poker.PokerLoad.Load(); LogComplete("PokerLoad", Poker.PokerLoad.Tables?.Count ?? 0, -1, "tables"); }
            catch (Exception ex) { LogError("PokerLoad", ex); }

            LogStart("Server.LoadMonsters");
            try { Server.LoadMonsters(); LogComplete("Server.LoadMonsters"); }
            catch (Exception ex) { LogError("Server.LoadMonsters", ex); }

            LogStart("Tranformation.Int");
            try { Tranformation.Int(); LogComplete("Tranformation.Int"); }
            catch (Exception ex) { LogError("Tranformation.Int", ex); }

            LogStart("Pool.Magic");
            try { Pool.Magic.Load(); LogComplete("Pool.Magic", Pool.Magic.Count); }
            catch (Exception ex) { LogError("Pool.Magic", ex); }

            LogStart("QuestInfo.Init");
            try { QuestInfo.Init(); LogComplete("QuestInfo.Init"); }
            catch (Exception ex) { LogError("QuestInfo.Init", ex); }

            LogStart("Pool.SubClassInfo");
            try { Pool.SubClassInfo.Load(); LogComplete("Pool.SubClassInfo"); }
            catch (Exception ex) { LogError("Pool.SubClassInfo", ex); }

            LogStart("BeastsAtrribute");
            try { BeastsAtrribute.Load(); LogComplete("BeastsAtrribute"); }
            catch (Exception ex) { LogError("BeastsAtrribute", ex); }

            LogStart("HairfaceStorageType");
            try { Database.HairfaceStorageType.Load(); LogComplete("HairfaceStorageType"); }
            catch (Exception ex) { LogError("HairfaceStorageType", ex); }

            LogStart("RuneTable");
            try { RuneTable.Load(); LogComplete("RuneTable"); }
            catch (Exception ex) { LogError("RuneTable", ex); }

            LogStart("MythTable");
            try { MythTable.LoadMyth(); LogComplete("MythTable"); }
            catch (Exception ex) { LogError("MythTable", ex); }

            LogStart("dict_collection");
            try { dict_collection.Load(); LogComplete("dict_collection"); }
            catch (Exception ex) { LogError("dict_collection", ex); }

            LogStart("dict_lottery");
            try { dict_lottery.Load(); LogComplete("dict_lottery"); }
            catch (Exception ex) { LogError("dict_lottery", ex); }

            LogStart("HundredWeapons");
            try { Database.HundredWeapons.Load(); LogComplete("HundredWeapons"); }
            catch (Exception ex) { LogError("HundredWeapons", ex); }

            LogStart("NinjaFile");
            try { NinjaFile.Load(); LogComplete("NinjaFile"); }
            catch (Exception ex) { LogError("NinjaFile", ex); }

            LogStart("Gouyuxuzuotype");
            try { Gouyuxuzuotype.Load(); LogComplete("Gouyuxuzuotype"); }
            catch (Exception ex) { LogError("Gouyuxuzuotype", ex); }

            LogStart("MeltingTypeTable");
            try { MeltingTypeTable.Load(); LogComplete("MeltingTypeTable"); }
            catch (Exception ex) { LogError("MeltingTypeTable", ex); }

            LogStart("CombatGear");
            try { CombatGear.Load(); LogComplete("CombatGear"); }
            catch (Exception ex) { LogError("CombatGear", ex); }

            LogStart("god_weapons_type");
            try { god_weapons_type.Load(); LogComplete("god_weapons_type"); }
            catch (Exception ex) { LogError("god_weapons_type", ex); }

            LogStart("god_weapons_material");
            try { god_weapons_material.Load(); LogComplete("god_weapons_material"); }
            catch (Exception ex) { LogError("god_weapons_material", ex); }

            LogStart("god_weapons_exp");
            try { god_weapons_exp.Load(); LogComplete("god_weapons_exp"); }
            catch (Exception ex) { LogError("god_weapons_exp", ex); }

            LogStart("daoqi_dict_type");
            try { daoqi_dict_type.Load(); LogComplete("daoqi_dict_type"); }
            catch (Exception ex) { LogError("daoqi_dict_type", ex); }

            LogStart("FateExpTable");
            try { FateExpTable.Load(); LogComplete("FateExpTable"); }
            catch (Exception ex) { LogError("FateExpTable", ex); }

            LogStart("SpiritTable");
            try { SpiritTable.Load(); LogComplete("SpiritTable"); }
            catch (Exception ex) { LogError("SpiritTable", ex); }

            LogStart("SwordAncestorType");
            try { SwordAncestorType.Load(); LogComplete("SwordAncestorType"); }
            catch (Exception ex) { LogError("SwordAncestorType", ex); }

            LogStart("ChiTable");
            try { ChiTable.Load(); LogComplete("ChiTable"); }
            catch (Exception ex) { LogError("ChiTable", ex); }

            LogStart("FlowersTable");
            try { FlowersTable.Load(); LogComplete("FlowersTable"); }
            catch (Exception ex) { LogError("FlowersTable", ex); }

            LogStart("NobilityTable");
            try { NobilityTable.Load(); LogComplete("NobilityTable"); }
            catch (Exception ex) { LogError("NobilityTable", ex); }

            LogStart("Associate.Load");
            try { Role.Instance.Associate.Load(); LogComplete("Associate.Load"); }
            catch (Exception ex) { LogError("Associate.Load", ex); }

            LogStart("CoatStorage");
            try { CoatStorage.Load(); LogComplete("CoatStorage"); }
            catch (Exception ex) { LogError("CoatStorage", ex); }

            // ========================================================================
            // TOURNAMENTS & WARS
            // ========================================================================
            LogStart("GuildWar.CreateFurnitures");
            try { Game.MsgTournaments.MsgSchedules.GuildWar.CreateFurnitures(); LogComplete("GuildWar.CreateFurnitures"); }
            catch (Exception ex) { LogError("GuildWar.CreateFurnitures", ex); }

            LogStart("GuildWar.Load");
            try { Game.MsgTournaments.MsgSchedules.GuildWar.Load(); LogComplete("GuildWar.Load"); }
            catch (Exception ex) { LogError("GuildWar.Load", ex); }

            LogStart("SuperGuildWar.CreateFurnitures");
            try { Game.MsgTournaments.MsgSchedules.SuperGuildWar.CreateFurnitures(); LogComplete("SuperGuildWar.CreateFurnitures"); }
            catch (Exception ex) { LogError("SuperGuildWar.CreateFurnitures", ex); }

            LogStart("SuperGuildWar.Load");
            try { Game.MsgTournaments.MsgSchedules.SuperGuildWar.Load(); LogComplete("SuperGuildWar.Load"); }
            catch (Exception ex) { LogError("SuperGuildWar.Load", ex); }

            LogStart("EliteGuildWar.Load");
            try { Game.MsgTournaments.MsgSchedules.EliteGuildWar.Load(); LogComplete("EliteGuildWar.Load"); }
            catch (Exception ex) { LogError("EliteGuildWar.Load", ex); }

            LogStart("MsgWarOfPlayers.Load");
            try { Game.MsgTournaments.MsgWarOfPlayers.Load(); LogComplete("MsgWarOfPlayers.Load"); }
            catch (Exception ex) { LogError("MsgWarOfPlayers.Load", ex); }

            LogStart("ChampionsOfWarr.Load");
            try { Game.MsgTournaments.MsgSchedules.ChampionsOfWarr.Load(); LogComplete("ChampionsOfWarr.Load"); }
            catch (Exception ex) { LogError("ChampionsOfWarr.Load", ex); }

            LogStart("UnionWar.Create");
            try { Game.MsgTournaments.MsgSchedules.UnionWar.Create(); LogComplete("UnionWar.Create"); }
            catch (Exception ex) { LogError("UnionWar.Create", ex); }

            LogStart("EmperorWar.Load");
            try { Game.MsgTournaments.MsgSchedules.EmperorWar.Load(); LogComplete("EmperorWar.Load"); }
            catch (Exception ex) { LogError("EmperorWar.Load", ex); }

            LogStart("Guild6PoleWar6.Load");
            try { Game.MsgTournaments.MsgSchedules.Guild6PoleWar6.Load(); LogComplete("Guild6PoleWar6.Load"); }
            catch (Exception ex) { LogError("Guild6PoleWar6.Load", ex); }

            LogStart("TopWarScore.Load");
            try { Game.MsgTournaments.MsgSchedules.TopWarScore.Load(); LogComplete("TopWarScore.Load"); }
            catch (Exception ex) { LogError("TopWarScore.Load", ex); }

            // ========================================================================
            // COSMETICS & GUILDS
            // ========================================================================
            LogStart("Coat_Color_Rule");
            try { Coat_Color_Rule.Load(); LogComplete("Coat_Color_Rule"); }
            catch (Exception ex) { LogError("Coat_Color_Rule", ex); }

            LogStart("LeagueTable");
            try { LeagueTable.Load(); LogComplete("LeagueTable"); }
            catch (Exception ex) { LogError("LeagueTable", ex); }

            LogStart("GuildTable");
            try { GuildTable.Load(); LogComplete("GuildTable"); }
            catch (Exception ex) { LogError("GuildTable", ex); }

            LogStart("ClanTable");
            try { Database.ClanTable.Load(); LogComplete("ClanTable"); }
            catch (Exception ex) { LogError("ClanTable", ex); }

            LogStart("JianHuTable");
            try { JianHuTable.LoadStatus(); JianHuTable.LoadJiangHu(); LogComplete("JianHuTable"); }
            catch (Exception ex) { LogError("JianHuTable", ex); }

            // ========================================================================
            // REWARDS & SHOPS
            // ========================================================================
            LogStart("TaskRewards");
            try { TaskRewards.Load(); LogComplete("TaskRewards"); }
            catch (Exception ex) { LogError("TaskRewards", ex); }

            LogStart("InnerPowerTable.LoadDBInformation");
            try { InnerPowerTable.LoadDBInformation(); LogComplete("InnerPowerTable.LoadDBInformation"); }
            catch (Exception ex) { LogError("InnerPowerTable.LoadDBInformation", ex); }

            LogStart("ExchangeShopGoods");
            try { ExchangeShopGoods.Load(); LogComplete("ExchangeShopGoods"); }
            catch (Exception ex) { LogError("ExchangeShopGoods", ex); }

            LogStart("ExchangeShopGoodsEx");
            try { ExchangeShopGoodsEx.Load(); LogComplete("ExchangeShopGoodsEx"); }
            catch (Exception ex) { LogError("ExchangeShopGoodsEx", ex); }

            LogStart("new_shop_goods");
            try { new_shop_goods.Load(); LogComplete("new_shop_goods"); }
            catch (Exception ex) { LogError("new_shop_goods", ex); }

            LogStart("QuizShow");
            try { QuizShow.Load(); LogComplete("QuizShow"); }
            catch (Exception ex) { LogError("QuizShow", ex); }

            LogStart("ClassPkWar");
            try { Game.MsgTournaments.MsgSchedules.ClassPkWar.Load(); LogComplete("ClassPkWar"); }
            catch (Exception ex) { LogError("ClassPkWar", ex); }

            LogStart("ElitePkTournament");
            try { Game.MsgTournaments.MsgSchedules.ElitePkTournament.Load(); LogComplete("ElitePkTournament"); }
            catch (Exception ex) { LogError("ElitePkTournament", ex); }

            LogStart("TeamPkTournament");
            try { Game.MsgTournaments.MsgSchedules.TeamPkTournament.Load(); LogComplete("TeamPkTournament"); }
            catch (Exception ex) { LogError("TeamPkTournament", ex); }

            LogStart("SkillTeamPkTournament");
            try { Game.MsgTournaments.MsgSchedules.SkillTeamPkTournament.Load(); LogComplete("SkillTeamPkTournament"); }
            catch (Exception ex) { LogError("SkillTeamPkTournament", ex); }

            LogStart("TitleStorage");
            try { TitleStorage.LoadDBInformation(); LogComplete("TitleStorage"); }
            catch (Exception ex) { LogError("TitleStorage", ex); }

            LogStart("ItemRefineUpgrade");
            try { ItemRefineUpgrade.Load(); LogComplete("ItemRefineUpgrade"); }
            catch (Exception ex) { LogError("ItemRefineUpgrade", ex); }

            LogStart("ProfessionTable");
            try { Database.ProfessionTable.Load(); LogComplete("ProfessionTable"); }
            catch (Exception ex) { LogError("ProfessionTable", ex); }

            LogStart("TheCrimeTable");
            try { TheCrimeTable.Load(); LogComplete("TheCrimeTable"); }
            catch (Exception ex) { LogError("TheCrimeTable", ex); }

            LogStart("ActivityTasks");
            try { Pool.ActivityTasks.Load(); LogComplete("ActivityTasks"); }
            catch (Exception ex) { LogError("ActivityTasks", ex); }

            LogStart("Statue");
            try { Role.Statue.Load(); LogComplete("Statue"); }
            catch (Exception ex) { LogError("Statue", ex); }

            LogStart("TableHeroRewards");
            try { Pool.TableHeroRewards.LoadInformations(); LogComplete("TableHeroRewards"); }
            catch (Exception ex) { LogError("TableHeroRewards", ex); }

            LogStart("Disdain");
            try { Database.Disdain.Load(); LogComplete("Disdain"); }
            catch (Exception ex) { LogError("Disdain", ex); }

            LogStart("RechargeShop");
            try { RechargeShop.Load(); LogComplete("RechargeShop"); }
            catch (Exception ex) { LogError("RechargeShop", ex); }

            LogStart("Booths");
            try {Booths.Load(); LogComplete("Booths"); }
            catch (Exception ex) { LogError("Booths", ex); }

            LogStart("Arena");
            try { Pool.Arena.Load(); LogComplete("Arena"); }
            catch (Exception ex) { LogError("Arena", ex); }

            LogStart("InnerPowerTable.Load");
            try { InnerPowerTable.Load(); LogComplete("InnerPowerTable.Load"); }
            catch (Exception ex) { LogError("InnerPowerTable.Load", ex); }

            LogStart("TutorInfo");
            try { Database.TutorInfo.Load(); LogComplete("TutorInfo"); }
            catch (Exception ex) { LogError("TutorInfo", ex); }

            LogStart("InfoDemonExterminators");
            try { InfoDemonExterminators.Create(); LogComplete("InfoDemonExterminators"); }
            catch (Exception ex) { LogError("InfoDemonExterminators", ex); }

            LogStart("QueueContainer");
            try { Pool.QueueContainer.Load(); LogComplete("QueueContainer"); }
            catch (Exception ex) { LogError("QueueContainer", ex); }

            LogStart("GroupServerList");
            try { GroupServerList.Load(); LogComplete("GroupServerList"); }
            catch (Exception ex) { LogError("GroupServerList", ex); }

            LogStart("VoteSystem");
            try { VoteSystem.Load(); LogComplete("VoteSystem"); }
            catch (Exception ex) { LogError("VoteSystem", ex); }

            LogStart("BotJail");
            try { BotJail.Load(); LogComplete("BotJail"); }
            catch (Exception ex) { LogError("BotJail", ex); }

            LogStart("StarDragonBall");
            try { Database.StarDragonBall.Load(); LogComplete("StarDragonBall"); }
            catch (Exception ex) { LogError("StarDragonBall", ex); }

            LogStart("DeityAltar");
            try { DeityAltar.Load(); LogComplete("DeityAltar", DeityAltar.DeityAltarS?.Count ?? 0, -1, "deities"); }
            catch (Exception ex) { LogError("DeityAltar", ex); }

            LogStart("RanksTable");
            try { Database.RanksTable.Initialize(); LogComplete("RanksTable"); }
            catch (Exception ex) { LogError("RanksTable", ex); }

            LogStart("Server.LoadDatabase");
            try { Server.LoadDatabase(); LogComplete("Server.LoadDatabase"); }
            catch (Exception ex) { LogError("Server.LoadDatabase", ex); }

            LogStart("RankMonster");
            try { RankMonster.Load(); LogComplete("RankMonster"); }
            catch (Exception ex) { LogError("RankMonster", ex); }

            LogStart("Pool.Insults");
            try { Pool.Insults.Load(); LogComplete("Pool.Insults"); }
            catch (Exception ex) { LogError("Pool.Insults", ex); }

            totalSw.Stop();

            // ========================================================================
            // FINAL SUMMARY
            // ========================================================================
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("");
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                              DATABASE LOAD COMPLETE                           ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║  Total time: {totalSw.ElapsedMilliseconds} ms ({totalSw.ElapsedMilliseconds / 1000.0:F2} seconds)                              ║");
            Console.WriteLine($"║  Items loaded: {totalItems:N0}                                                              ║");
            Console.WriteLine($"║  Maps loaded: {Pool.ServerMaps?.Count ?? 0}                                                               ║");
            Console.WriteLine($"║  Booths loaded: {Booths.Boooths?.Count ?? 0}                                                             ║");
            Console.WriteLine($"║  Poker Tables: {Poker.PokerLoad.Tables?.Count ?? 0}                                                             ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Database Loaded in " + (Time32.Now - now).AllSeconds() + " Seconds ...");
            Console.ResetColor();
        }
    }
}
