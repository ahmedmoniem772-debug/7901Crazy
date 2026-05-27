using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VirusX.Game.MsgServer;
using System.Windows.Forms;
using VirusX.DBFunctionality;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace VirusX.Database
{
    public class ServerDatabase
    {
        public static ArenaTable Arena = new ArenaTable();
        public static object TeamArena = new object();
        public static void ResetingEveryDay(Client.GameClient client)
        {
            try
            {
                if (DateTime.Now.DayOfYear != client.Player.Day)
                {
                    client.Player.UseChiToken = 0;

                    client.Player.ChangeEpicTrojan = client.Player.ChangeArrayEpicTrojan =
                        client.Player.ChangeMr_MirrorEpicTrojan = client.Player.ChangeGeneralPakEpicTrojan = 0;
                    client.Player.CanChangeEpicMaterial = client.Player.CanChangeArrayEpicMaterial =
                        client.Player.CanChangeMr_MirrorEpicMaterial = client.Player.CanChangGeneralPakMaterial = 1;

                    client.Player.TodayChampionPoints = 0;
                  
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
                    client.Player.MisShoot = client.Player.HitShoot = 0;
                    client.Player.ArenaDeads = client.Player.ArenaKills = 0;

                    client.Player.TowerOfMysterychallenge = 3;
                    client.Player.TOMChallengeToday = 0;
                    client.Player.TowerOfMysteryChallengeFlag = 0;
                    client.Player.TOMSelectChallengeToday = 0;
                    client.Player.ClaimTowerAmulets = 0;
                    client.Player.TOMClaimTeamReward = 0;
                    client.Player.TOMRefreshReward = 0;
                    client.Player.QuestGUI.RemoveQuest(6126);
                    client.Player.ClaimPointsArena = 0;

                    client.Player.OpenHousePack = 0;
                    client.MyExchangeShop.Reset();

                    client.Player.DbTry = false;
                    client.Player.LotteryEntries = 0;
                    client.Player.BDExp = 0;
                    client.Player.ExpBallUsed = 0;
                    client.Player.TCCaptainTimes = 0;
                    client.DemonExterminator.FinishToday = 0;
                    client.Player.EpicQuestChance = 0;
                    client.Player.EpicNinjaQuestChance = 0;
                    client.Player.EpicPirateQuestChance = 0;
                    if (client.Player.MyChi != null && DateTime.Now.DayOfYear > client.Player.Day)
                        client.Player.MyChi.ChiPoints = (int)(client.Player.MyChi.ChiPoints + Math.Min(((DateTime.Now.DayOfYear - client.Player.Day) * 300), client.Player.MyChi.MaxChiPoints));
                    else
                        client.Player.MyChi.ChiPoints = client.Player.MyChi.ChiPoints + 300;

                    if (client.Player.Flowers != null)
                    {
                        client.Player.Flowers.FreeFlowers = 1;
                        client.Player.Flowers.RedRoses.FlowerFree = 0;
                        foreach (var flower in client.Player.Flowers)
                            flower.Amount2day = 0;

                    }

                    if (client.Player.Level >= 90)
                    {
                        client.Player.Enilghten = CalculateEnlighten(client.Player);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            client.Player.SendUpdate(stream, client.Player.Enilghten, Game.MsgServer.MsgUpdate.DataType.EnlightPoints);
                        }
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
                    client.Player.Day = DateTime.Now.DayOfYear;
                }
            }
            catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
        }
        public static ushort CalculateEnlighten(Role.Player player)
        {
            if (player.Level < 90)
                return 0;
            ushort val = 100;
            if (player.NobilityRank == Role.Instance.Nobility.NobilityRank.Knight || player.NobilityRank == Role.Instance.Nobility.NobilityRank.Baron)
                val += 100;
            if (player.NobilityRank == Role.Instance.Nobility.NobilityRank.Earl || player.NobilityRank == Role.Instance.Nobility.NobilityRank.Duke)
                val += 200;
            if (player.NobilityRank == Role.Instance.Nobility.NobilityRank.Prince)
                val += 300;
            if (player.NobilityRank == Role.Instance.Nobility.NobilityRank.King)
                val += 400;
            if (player.VipLevel <= 3)
                val += 100;
            if (player.VipLevel > 3 && player.VipLevel <= 5)
                val += 200;
            if (player.VipLevel > 5)
                val += 300;

            return val;
        }
        public static void SaveClient(Client.GameClient client)
        {

            try
            {
                WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + client.Player.UID + ".ini");

                if ((client.ClientFlag & Client.ServerFlag.LoginFull) != Client.ServerFlag.LoginFull)
                {

                    if (client.Map != null)
                        client.Map.Denquer(client);

                }

                if (HouseTable.InHouse(client.Player.Map) && client.Player.DynamicID != 0 || client.Player.DynamicID != 0)
                {
                    if (client.Socket != null && client.Socket.Alive == false)
                    {
                        client.Player.Map = 1002;
                        client.Player.X = 410;
                        client.Player.Y = 354;
                    }
                }
                if ((client.ClientFlag & Client.ServerFlag.Disconnect) == Client.ServerFlag.Disconnect)
                {
                    if (client.Player.Map == 1017 || client.Player.Map == 1081 || client.Player.Map == 2060 || client.Player.Map == 9972
                        || client.Player.Map == 1080 || client.Player.Map == 3820 || client.Player.Map == 3954
                         || client.Player.Map == 22330 || client.Player.Map == 22332 || client.Player.Map == 22334 || client.Player.Map == 22331 || client.Player.Map == 22333 || client.Player.Map == 22335
                    || client.Player.Map == 1806
                        
                        
                || client.Player.Map == 1768
                || client.Player.Map == 7 || client.Player.Map == 8 || client.Player.Map == 11 || client.Player.Map == 10760 || client.Player.Map == 10503 || client.Player.Map == 12
             || client.Player.Map == 14 || client.Player.Map == 15 || client.Player.Map == 5 || client.Player.Map == 20 || client.Player.Map == 10760 || client.Player.Map == 10503


                || client.Player.Map == 1505 || client.Player.Map == 1506 || client.Player.Map == 12022 || client.Player.Map == 1508 || client.Player.Map == 1507
                 || client.Player.Map == 1801 || client.Player.Map == 1780 || client.Player.Map == 1779 || client.Player.Map == 3071 || client.Player.Map == 11447 || client.Player.Map == 1068

                        || client.Player.Map == 3830 || client.Player.Map == 3831 || client.Player.Map == 3832 || client.Player.Map == 3834
                        || client.Player.Map == 3826 || client.Player.Map == 3827 || client.Player.Map == 3828 || client.Player.Map == 3829
                        || client.Player.Map == 10 || client.Player.Map == 3825
                        || client.Player.Map == 2353
                       || client.Player.Map == 20081 || client.Player.Map == 20082 || client.Player.Map == 20083 || client.Player.Map == 20084
                        || client.Player.Map == 1518 || client.Player.Map == 1508
                    #region Room
 || client.Player.Map == 5050
                        || client.Player.Map == 5051
                        || client.Player.Map == 5052
                        || client.Player.Map == 5053
                        || client.Player.Map == 5054
                        || client.Player.Map == 5055
                        || client.Player.Map == 5056
                        || client.Player.Map == 5057
                        || client.Player.Map == 5058
                        || client.Player.Map == 5059
                        || client.Player.Map == 5060
 || client.Player.Map == 5061
                        || client.Player.Map == 5062
                        || client.Player.Map == 5063
                        || client.Player.Map == 5064
                        || client.Player.Map == 5065
                        || client.Player.Map == 5066
                        || client.Player.Map == 5067
                        || client.Player.Map == 5068
                        || client.Player.Map == 5069
                        || client.Player.Map == 5070
                        || client.Player.Map == 5071
                    #endregion
 || client.Player.Map == 6011 || client.Player.Map == 1138 //1138
                        || client.Player.Map == 10088 || client.Player.Map == 5342 || client.Player.Map == 1038 || client.Player.Map == 8009 || client.Player.Map == 3825 || client.Player.Map == 10089 || client.Player.Map == 10090
                        || client.Player.Map == 44455 || client.Player.Map == 44456 || client.Player.Map == 44457
                         || client.Player.Map == 44460 || client.Player.Map == 10133 || client.Player.Map == 10134 || client.Player.Map == 44461 || client.Player.Map == 44462 || client.Player.Map == 10478 || client.Player.Map == 1138 || client.Player.Map == 44463 || client.Player.Map == 10428

)
                    {

                        client.Player.Map = 1002;
                        client.Player.X = 410;
                        client.Player.Y = 354;
                    }

                    if (client.Player.Map == 3053)
                    {
                        client.Player.Map = 1002;
                        client.Player.X = 343;
                        client.Player.Y = 434;
                    }
                    if (client.Player.Map == 3852)
                    {
                        client.Player.Map = 1002;
                        client.Player.X = 410;
                        client.Player.Y = 354;
                    }


                    if (Pool.Constants.SkillsNotAvailableHere.Contains(client.Player.Map) 
                        || client.Player.Map == 4000 
                        || client.Player.Map == 4003 || client.Player.Map == 4006 || client.Player.Map == 4008 || client.Player.Map == 4009)
                    {
                        client.Player.Map = 1002;
                        client.Player.X = 410;
                        client.Player.Y = 354;
                    }
                }

                if (!client.FullLoading)
                    return;
                write.WriteString("Character", "HairfaceStorage", client.HairfaceStorage.ToString());
                write.Write<uint>("Character", "CollectionID", client.Player.CollectionID);
                write.Write<uint>("Character", "PandaID", client.Player.PandaID);
                write.WriteString("Character", "PinCodeAnima", client.Player.PinCodeAnima.ToString());
                write.Write<bool>("Character", "RelicSpirit", client.Player.RelicSpirit);
                write.Write<bool>("Character", "TailedBeastActivated", client.Beasts.Activated);
                write.Write<uint>("Character", "TailedBeastTotalPoints", client.Beasts.TotalPoints);
                write.Write<uint>("Character", "FruitToday", client.Beasts.FruitToday);
                write.Write<uint>("Character", "DeityLandLuckyPoints", client.Player.DeityLandLuckyPoints);
                write.Write<uint>("Character", "HeroPoints", client.Player.HeroPoints);
                write.Write<uint>("Character", "ClassExperience", client.Player.ClassExperience);
                write.Write<uint>("Character", "SavePromote", client.Player.SavePromote);
                write.Write<bool>("Character", "UnLockedSystem", client.Player.UnLockedSystem);
                write.Write<uint>("Character", "UID", client.Player.UID);
                write.Write<ushort>("Character", "Body", client.Player.Body);
                write.Write<ushort>("Character", "Face", client.Player.Face);
                write.WriteString("Character", "Name", client.Player.Name);
                write.WriteString("Character", "Spouse", client.Player.Spouse);
                write.WriteString("Character", "Description", client.Player.Description);
                write.Write<uint>("Character", "Class", client.Player.Class);
                write.Write<uint>("Character", "FirstClass", client.Player.FirstClass);
                write.Write<uint>("Character", "SecoundeClass", client.Player.SecoundeClass);
                write.Write<uint>("Character", "ExchangehighAvaliability", client.Player.ExchangehighAvaliability);
                write.Write<bool>("Character", "IsBannedChat", client.Player.IsBannedChat);
                write.Write<bool>("Character", "PermenantBannedChat", client.Player.PermenantBannedChat);
                write.Write<long>("Character", "BannedChatStamp", client.Player.BannedChatStamp.ToBinary());
                write.Write<byte>("Character", "RebornItem", client.Player.RebornItem);
                write.Write<ushort>("Character", "Avatar", client.Player.Avatar);
                write.Write<uint>("Character", "Map", client.Player.Map);
                write.Write<ushort>("Character", "X", client.Player.X);
                write.Write<ushort>("Character", "Y", client.Player.Y);
                write.Write<uint>("Character", "ResinanceRuneOne", client.Player.ResinanceRuneOne);
                write.Write<uint>("Character", "ResinanceRuneTwo", client.Player.ResinanceRuneTwo);
                write.Write<uint>("Character", "ResinanceRelicOne", client.Player.ResinanceRelicOne);
                write.Write<uint>("Character", "ResinanceRelicTwo", client.Player.ResinanceRelicTwo);
                write.Write<uint>("Character", "ResinanceRunethree", client.Player.ResinanceRunethree);
                write.Write<uint>("Character", "ResinanceRunefour", client.Player.ResinanceRunefour);
                write.Write<uint>("Character", "ResinanceRunefive", client.Player.ResinanceRunefive);
                write.Write<uint>("Character", "PMap", client.Player.PMap);

                write.Write<byte>("Character", "BlueRune", client.Player.BlueRune);
                write.Write<byte>("Character", "RedRune", client.Player.RedRunes);

                write.Write<ushort>("Character", "PMapX", client.Player.PMapX);
                write.Write<ushort>("Character", "PMapY", client.Player.PMapY);

                write.Write<ushort>("Character", "Agility", client.Player.Agility);
                write.Write<ushort>("Character", "Strength", client.Player.Strength);
                write.Write<ushort>("Character", "Vitaliti", client.Player.Vitality);
                write.Write<ushort>("Character", "Spirit", client.Player.Spirit);
                write.Write<ushort>("Character", "Atributes", client.Player.Atributes);

                write.Write<byte>("Character", "Reborn", client.Player.Reborn);
                write.Write<ushort>("Character", "Level", client.Player.Level);
                write.Write<uint>("Character", "Haire", client.Player.Hair);
                write.Write<ulong>("Character", "Experience", client.Player.Experience);
                write.Write<int>("Character", "MinHitPoints", client.Player.HitPoints);
                write.Write<ushort>("Character", "MinMana", client.Player.Mana);

                write.Write<ulong>("Character", "DominoCode", client.Player.DominoCode);
                write.Write<uint>("Character", "WarDropeFull", client.Player.WarDropeFull);
                write.Write<long>("Character", "DominoCoins", client.Player.DominoCoins);
                write.Write<uint>("Character", "ConquerPoints", (uint)client.Player.ConquerPoints);
                write.Write<int>("Character", "BoundConquerPoints", client.Player.BoundConquerPoints);
                write.Write<long>("Character", "Money", client.Player.Money);
                write.Write<uint>("Character", "VirtutePoints", client.Player.VirtutePoints);

                write.Write<ushort>("Character", "PkPoints", client.Player.PKPoints);
                write.Write<uint>("Character", "QuizPoints", client.Player.QuizPoints);
                write.Write<string>("Character", "MemoryAgate", client.Player.AgatesString());
                write.Write<ushort>("Character", "Enilghten", client.Player.Enilghten);
                write.Write<ushort>("Character", "EnlightenReceive", client.Player.EnlightenReceive);
                write.Write<ulong>("Character", "DailySignUpDays", client.Player.DailySignUpDays);
                write.Write<byte>("Character", "DailyMonth", client.Player.DailyMonth);
                write.Write<byte>("Character", "DailySignUpRewards", client.Player.DailySignUpRewards);
                write.Write<int>("Character", "FairyForm", client.Player.FairyForm);
                write.Write<byte>("Character", "VipLevel", client.Player.VipLevel);
                write.Write<long>("Character", "VipTime", client.Player.ExpireVip.Ticks);
                write.Write<bool>("Character", "VipTimeback", client.Player.ExpireVipback);
                client.Player.Achievement.Save(client.Achievement);
                write.WriteString("Character", "Achivement", client.Achievement.ToString());

                write.Write<long>("Character", "WHMoney", client.Player.WHMoney);
                write.Write<long>("Character", "CpsBank", client.Player.CpsBank);

                write.Write<uint>("Character", "BlessTime", client.Player.BlessTime);

                write.Write<uint>("Character", "SpouseUID", client.Player.SpouseUID);

                write.Write<int>("Character", "HeavenBlessing", client.Player.HeavenBlessing);
                write.Write<long>("Character", "LostTimeBlessing", client.Player.HeavenBlessTime.ToBinary());

                write.Write<uint>("Character", "HuntingBlessing", client.Player.HuntingBlessing);
                write.Write<uint>("Character", "OnlineTrainingPoints", client.Player.OnlineTrainingPoints);
                write.Write<long>("Character", "JoinOnflineTG", client.Player.JoinOnflineTG.Ticks);

                write.WriteString("Character", "HundredWeapons", client.HundredWeapons.ToString());

                write.WriteString("Character", "NinjaNinpo", client.MyNinja.ToString());

                write.Write<int>("Character", "Day", client.Player.Day);
                write.Write<byte>("Character", "BDExp", client.Player.BDExp);

                write.Write<uint>("Character", "RateExp", client.Player.RateExp);
                write.Write<uint>("Character", "DExpTime", client.Player.DExpTime);
                write.Write<byte>("Character", "ExpBallUsed", client.Player.ExpBallUsed);

                write.WriteString("Character", "SubProfInfo", client.Player.SubClass.ToString());

                write.WriteString("Character", "Dragon", client.Player.MyChi.Dragon.ToString());
                write.WriteString("Character", "Pheonix", client.Player.MyChi.Phoenix.ToString());
                write.WriteString("Character", "Turtle", client.Player.MyChi.Turtle.ToString());
                write.WriteString("Character", "Tiger", client.Player.MyChi.Tiger.ToString());
                write.Write<int>("Character", "ChiPoints", client.Player.MyChi.ChiPoints);
                
                write.WriteString("Character", "Flowers", client.Player.Flowers.ToString());
                write.Write<ulong>("Character", "DonationNobility", client.Player.Nobility.Donation);

                write.Write<uint>("Character", "GuildID", client.Player.GuildID);
                write.Write<ushort>("Character", "GuildRank", (ushort)client.Player.GuildRank);
                if (client.Player.MyGuildMember != null)
                {
                    client.Player.MyGuildMember.LastLogin = DateTime.Now.Ticks;
                    write.Write<uint>("Character", "CpsDonate", client.Player.MyGuildMember.CpsDonate);
                    write.Write<long>("Character", "MoneyDonate", client.Player.MyGuildMember.MoneyDonate);
                    write.Write<uint>("Character", "PkDonation", client.Player.MyGuildMember.PkDonation);
                    write.Write<long>("Character", "LastLogin", client.Player.MyGuildMember.LastLogin);

                    write.Write<uint>("Character", "CTF_Exploits", client.Player.MyGuildMember.CTF_Exploits);
                    write.Write<uint>("Character", "CTF_RCPS", client.Player.MyGuildMember.RewardConquerPoints);
                    write.Write<uint>("Character", "CTF_RM", client.Player.MyGuildMember.RewardMoney);
                    write.Write<byte>("Character", "CTF_R", client.Player.MyGuildMember.CTF_Claimed);
                }
                if (client.Player.MyClan != null)
                {
                    write.Write<uint>("Character", "ClanID", client.Player.MyClan.ID);
                    write.Write<ushort>("Character", "ClanRank", client.Player.ClanRank);
                    if (client.Player.MyClanMember != null)
                        write.Write<uint>("Character", "ClanDonation", client.Player.MyClanMember.Donation);
                }
                if (client.Player.InUnion)
                {
                    write.Write<uint>("Character", "UnionUID", client.Player.MyUnion.UID);
                    write.Write<uint>("Character", "UnionRank", (uint)client.Player.UnionMemeber.Rank);

                    write.Write<uint>("Character", "Treasury", client.Player.UnionMemeber.MyTreasury);

                }
                else
                {
                    write.Write<uint>("Character", "UnionUID", 0);
                    write.Write<uint>("Character", "UnionRank", 0);
                    write.Write<uint>("Character", "UnionExploits", 0);
                    write.Write<uint>("Character", "UnionGoldBrick", 0);
                }


                write.Write<uint>("Character", "KingDomExploits", client.Player.KingDomExploits);
                write.Write<byte>("Character", "FRL", client.Player.FirstRebornLevel);
                write.Write<byte>("Character", "SRL", client.Player.SecoundeRebornLevel);
                write.Write<bool>("Character", "Reincanation", client.Player.Reincarnation);
                write.Write<byte>("Character", "LotteryEntries", client.Player.LotteryEntries);
                write.Write<bool>("Character", "DbTry", client.Player.DbTry);
                write.WriteString("Character", "DemonEx", client.DemonExterminator.ToString());
                write.WriteString("Character", "PkName", client.Player.MyKillerName);
                write.Write<uint>("Character", "PkUID", client.Player.MyKillerUID);
                write.Write<int>("Character", "Cursed", client.Player.CursedTimer);
                write.WriteString("Character", "HeroRewards", client.HeroRewards.ToString());
                write.WriteString("Character", "Activeness", client.Activeness.ToString());
                write.Write<uint>("Character", "AparenceType", client.Player.AparenceType);
                write.Write<uint>("Character", " TableBetDice", client.Player.TableBetDice);
                write.Write<uint>("Character", "HitShoot", client.Player.HitShoot);
                write.Write<uint>("Character", "MisShoot", client.Player.MisShoot);
                write.Write<uint>("Character", "ArenaDeads", client.Player.ArenaDeads);
                write.Write<uint>("Character", "ArenaKills", client.Player.ArenaKills);

                write.Write<uint>("Character", "EpicQuestChance", client.Player.EpicQuestChance);
                write.Write<uint>("Character", "EpicNinjaQuestChance", client.Player.EpicNinjaQuestChance);
                write.Write<uint>("Character", "EpicPirateQuestChance", client.Player.EpicPirateQuestChance);
                write.Write<uint>("Character", "TKills", client.Player.TournamentKills);
                
                write.Write<uint>("Character", "OnlineMinutes", client.Player.OnlineMinutes);
                write.Write<uint>("Character", "WorldPoints", client.Player.WorldPoints);
                write.Write<uint>("Character", "CastlePoint", client.Player.CastlePoint);
                write.Write<uint>("Character", "ScoreHuntCoins", client.Player.ScoreHuntCoins);
                write.Write<uint>("Character", "OnlinePointsPK", client.Player.OnlinePointsPK);

                write.Write<uint>("Character", "HistoryChampionPoints", client.Player.HistoryChampionPoints);
                write.Write<uint>("Character", "TodayChampionPoints", client.Player.TodayChampionPoints);

                write.Write<uint>("Character", "ChampionPoints", client.Player.ChampionPoints);
                write.Write<uint>("Character", "DailySpiritBeadItem", client.Player.DailySpiritBeadItem);
                write.WriteString("Character", "SpecialTitles", GetSpecialTitles(client));
                write.WriteString("Character", "SpecialHalo", GetHaloTitles(client));
                write.WriteString("Character", "TitleTime", GetTitleWithTime(client));
                write.WriteString("Character", "SpecialWings", GetSpecialWings(client));

                write.WriteString("Character", "HaloAction", GetSpecialHalosAction(client));
                write.WriteString("Character", "SpecialFoot", GetSpecialFootFinger(client));
                write.WriteString("Character", "SecurityPass", GetSecurityPassword(client));
                write.Write<byte>("Character", "TCT", (byte)client.Player.TCCaptainTimes);
                write.Write<uint>("Character", "RacePoints", client.Player.RacePoints);
                write.Write<double>("Character", "DonatePoints", client.Player.DonatePoints);
               
                write.Write<uint>("Character", "Online", client.Player.Online);
                write.Write<uint>("Character", "RamdanBag", client.Player.RamdanBag);
                write.Write<uint>("Character", "itemRamdanBag", client.Player.itemRamdanBag);
                write.Write<uint>("Character", "RewardPoints", client.Player.RewardPoints);
                write.Write<uint>("Character", "ItemRewordChristmas", client.Player.ItemRewordChristmas);
                write.Write<ushort>("Character", "NameEditCount", client.Player.NameEditCount);
                write.Write<uint>("Character", "ClaimStateGift", (uint)client.Player.MainFlag);
                write.Write<uint>("Character", "enervant", client.Player.AtiveQuestApe);
                write.Write<ushort>("Character", "InventorySashCount", client.Player.InventorySashCount);

                write.Write<ushort>("Character", "CountryID", client.Player.CountryID);
                write.Write<uint>("Character", "MyFootBallPoints", client.Player.MyFootBallPoints);
                write.Write<uint>("Character", "ExpProtection", client.Player.ExpProtection);
                write.Write<uint>("Character", "PrestigePoints", client.MyPrestigePoints);
                write.Write<uint>("Character", "BanCount", client.BanCount);


                write.Write<byte>("Character", "BuyKingdomDeeds", client.Player.BuyKingdomDeeds);
                write.Write<uint>("Character", "KingDomDeeds", client.Player.KingDomDeeds);
              

                write.WriteString("Character", "ExchangeShop", client.MyExchangeShop.ToString());

                write.Write<ushort>("Character", "ExtraAtributes", client.Player.ExtraAtributes);
              
                write.Write<byte>("Character", "OpenHousePack", client.Player.OpenHousePack);
                write.Write<byte>("Character", "botjail", client.Player.botjail);

                write.Write<byte>("Character", "ClaimTowerAmulets", client.Player.ClaimTowerAmulets);
                write.Write<byte>("Character", "TOMClaimTeamReward", client.Player.TOMClaimTeamReward);
                write.Write<byte>("Character", "MyTowerOfMysteryLayer", client.Player.MyTowerOfMysteryLayer);
                write.Write<byte>("Character", "MyTowerOfMysteryLayerElite", client.Player.MyTowerOfMysteryLayerElite);
                write.Write<byte>("Character", "TowerOfMysterychallenge", client.Player.TowerOfMysterychallenge);
                write.Write<uint>("Character", "TowerOfMysteryChallengeFlag", client.Player.TowerOfMysteryChallengeFlag);
                write.Write<byte>("Character", "TOMSelectChallengeToday", client.Player.TOMSelectChallengeToday);
                write.Write<byte>("Character", "TOMChallengeToday", client.Player.TOMChallengeToday);
                write.Write<uint>("Character", "TOMRefreshReward", client.Player.TOMRefreshReward);
                write.Write<byte>("Character", "TOM_Reward", (byte)client.Player.TOM_Reward);
                write.Write<long>("Character", "JPAStamp", client.Player.JoinPowerArenaStamp.Ticks);
                write.Write<uint>("Character", "KillCount", client.Player.KillCount);
                write.WriteString("Character", "EpicTrojan", client.Player.SaveEpicTrojan());
                write.WriteString("Character", "EnergyPoints", client.Player.SaveEpicArcher());
                write.WriteString("Character", "QuestWarrior", client.Player.SaveArchiveWarrior());
                write.Write<int>("Character", "GiveFlowersToPerformer", client.Player.GiveFlowersToPerformer);
                write.Write<byte>("Character", "UseChiToken", client.Player.UseChiToken);
                write.Write<ushort>("Character", "ClaimPointsArena", client.Player.ClaimPointsArena);
                write.Write<byte>("Character", "ChiToken", client.Player.ChiToken);
                write.Write<byte>("Character", "Vote", client.Player.Vote);
                write.Write<uint>("Character", "RuneFlag", client.Beasts.Flag);
                write.Write<byte>("Character", "JiangToken", client.Player.JiangToken);
                write.Write<long>("Character", "CanChangeWindWalkerFree", client.Player.CanChangeWindWalkerFree.Ticks);

                write.Write<uint>("Character", "questtwin", client.Player.questtwin);
                write.Write<uint>("Character", "questphoinex", client.Player.questphoinex);
                write.Write<uint>("Character", "questape", client.Player.questape);
                write.Write<uint>("Character", "questdeserst", client.Player.questdeserst);
                write.Write<uint>("Character", "questbird", client.Player.questbird);


                write.Write<uint>("Character", "questone", client.Player.questone);
                write.Write<uint>("Character", "questtwo", client.Player.questtwo);
                write.Write<uint>("Character", "questthree", client.Player.questthree);
                write.Write<uint>("Character", "questfour", client.Player.questfour);
                write.Write<uint>("Character", "FootprintOpen", client.Player.FootprintOpen);
                write.Write<uint>("Character", "HaloActionOpen", client.Player.HaloActionOpen);
                write.WriteString("Character", "MedalStorage", client.MedalStorage.ToString());
                write.WriteString("Character", "AnimaSkin", client.DragonSkin.ToString());
                write.WriteString("Character", "MyArchives", client.MyArchives.ToString());
                write.WriteString("Character", "Collection", client.Collection.ToString());
                write.WriteString("Character", "MyAstredge", client.MyAstredge.ToString());
                write.WriteString("Character", "GuildSkill", client.GuildSkill.ToString());
                write.Write<uint>("Character", "GuildDontion", client.Player.MyDontion);
                write.Write<uint>("Character", "CyanJadeRing", client.Player.CyanJadeRing);
                write.Write<uint>("Character", "MountArmorColor", client.Player.MountArmorColor);
                write.Write<ushort>("Character", "FrameID", client.Player.FrameID);
                write.WriteString("Character", "EnemyInvade", client.EnemyInvade.ToString());
                #region Retreated
                if (client.Player.MyChi.DragonTime != 0)
                {
                    write.Write<long>("Character", "DragonTime", client.Player.MyChi.DragonTime);
                    write.WriteString("Character", "RetreatedDragon", ChiTable.PowersToString(client.Player.MyChi.DragonPowers));
                }
                if (client.Player.MyChi.PhoenixTime != 0)
                {
                    write.Write<long>("Character", "PhoenixTime", client.Player.MyChi.PhoenixTime);
                    write.WriteString("Character", "RetreatedPhoenix", ChiTable.PowersToString(client.Player.MyChi.PhoenixPowers));
                }
                if (client.Player.MyChi.TurtleTime != 0)
                {
                    write.Write<long>("Character", "TurtleTime", client.Player.MyChi.TurtleTime);
                    write.WriteString("Character", "RetreatedTurtle", ChiTable.PowersToString(client.Player.MyChi.TigerPowers));
                }
                if (client.Player.MyChi.TigerTime != 0)
                {
                    write.Write<long>("Character", "TigerTime", client.Player.MyChi.TigerTime);
                    write.WriteString("Character", "RetreatedTiger", ChiTable.PowersToString(client.Player.MyChi.TurtlePowers));
                }
                #endregion
                write.Write<byte>("Character", "VotePoints", client.Player.VotePoints);

                client.CoatColorRule.Save(client.Player.UID);
                SaveClientItems(client);
                SaveClientSpells(client);
                SaveClientProfs(client);
                SaveClientMails(client);
                RoleQuests.Save(client);
                client.HairfaceStorage.Save(client.Player.UID);
                Role.Instance.House.Save(client);
               
                if ((client.ClientFlag & Client.ServerFlag.Disconnect) == Client.ServerFlag.Disconnect)
                {
                    Client.GameClient user;
                    Pool.GamePoll.TryRemove(client.Player.UID, out user);
                    Pool.DisconnectPool.TryRemove(client.Player.UID, out user);
                    write.Write<byte>("Character", "Online", 0);
                    client.Player.Online -= 1;
                }
            }
            catch (Exception e) { MyConsole.WriteLine(e.ToString()); }

        }
        public static string GetSecurityPassword(Client.GameClient user)
        {
            Database.DBActions.WriteLine writer = new DBActions.WriteLine(',');
            writer.Add(user.Player.SecurityPassword);
            writer.Add(user.Player.OnReset);
            writer.Add(user.Player.ResetSecurityPassowrd.Ticks);
            return writer.Close();
        }
        public static void LoadSecurityPassword(string line, Client.GameClient user)
        {
            Database.DBActions.ReadLine reader = new DBActions.ReadLine(line, ',');
            user.Player.SecurityPassword = reader.Read((uint)0);
            user.Player.OnReset = reader.Read((uint)0);
            if (user.Player.OnReset == 1)
            {
                user.Player.ResetSecurityPassowrd = DateTime.FromBinary(reader.Read((long)0));
                if (DateTime.Now > user.Player.ResetSecurityPassowrd)
                {
                    user.Player.OnReset = 0;
                    user.Player.SecurityPassword = 0;
                }
            }

        }
        public static string GetSpecialTitles(Client.GameClient user)
        {
            Database.DBActions.WriteLine writer = new DBActions.WriteLine(',');
            writer.Add((uint)user.Player.SpecialTitles.Count);
            foreach (var title in user.Player.SpecialTitles)
            {
                writer.Add((uint)title);
                if (user.Player.SpecialTitleID / 10000 == (uint)title && user.Player.SpecialTitleID / 10000 != 0)
                {
                    writer.Add((byte)1);
                }

                else
                {
                    writer.Add((byte)0);
                }

            }
            return writer.Close();
        }
        public static string GetSpecialWings(Client.GameClient user)
        {
            Database.DBActions.WriteLine writer = new DBActions.WriteLine(',');
            writer.Add((uint)user.Player.WingsTitles.Count);
            foreach (var title in user.Player.WingsTitles)
            {
                writer.Add((uint)title);
                if (user.Player.SpecialWingID / 10000 == (uint)title && user.Player.SpecialWingID / 10000 != 0)
                {
                    writer.Add((byte)1);
                }

                else
                {
                    writer.Add((byte)0);
                }

            }
            return writer.Close();
        }
        public static string GetSpecialHalosAction(Client.GameClient user)
        {
            Database.DBActions.WriteLine writer = new DBActions.WriteLine(',');
            writer.Add((uint)user.Player.HaloAction.Count);
            foreach (var title in user.Player.HaloAction)
            {
                writer.Add((uint)title);
                if (user.Player.SpecialHaloAction / 10000 == (uint)title && user.Player.SpecialHaloAction / 10000 != 0)
                {
                    writer.Add((byte)1);
                }

                else
                {
                    writer.Add((byte)0);
                }

            }
            return writer.Close();
        }
        public static string GetSpecialFootFinger(Client.GameClient user)
        {
            Database.DBActions.WriteLine writer = new DBActions.WriteLine(',');
            writer.Add((uint)user.Player.Footprint.Count);
            foreach (var title in user.Player.Footprint)
            {
                writer.Add((uint)title);
                if (user.Player.SpecialFootprintID / 10000 == (uint)title && user.Player.SpecialFootprintID / 10000 != 0)
                {
                    writer.Add((byte)1);
                }

                else
                {
                    writer.Add((byte)0);
                }

            }
            return writer.Close();
        }
        public static string GetTitleWithTime(Client.GameClient user)
        {
            Database.DBActions.WriteLine writer = new DBActions.WriteLine(',');
            writer.Add((uint)user.Player.TitleWithTime.Count);
            foreach (var title in user.Player.TitleWithTime.Values)
            {
                writer.Add((uint)title.titleID);
                writer.Add(title.TotalSeconds);
                writer.Add((long)title.DateStamp.Ticks);
            }
            return writer.Close();
        }
        public static void LoadTitleWithTime(Client.GameClient user, string line)
        {
            Database.DBActions.ReadLine reader = new DBActions.ReadLine(line, ',');
            uint count = reader.Read((uint)0);
            for (int x = 0; x < count; x++)
            {
                uint titleID = reader.Read((uint)0);
                ulong TotalSeconds = reader.Read((uint)0);
                long Tick = reader.Read((long)0);
                var TimeTitle = new Role.Player.WardRobeTitle();
                TimeTitle.titleID = titleID;
                TimeTitle.DateStamp = DateTime.FromBinary(Tick);
                TimeSpan timeSpan = TimeTitle.DateStamp - DateTime.Now;
                if (TimeTitle.TotalSeconds > 0)
                    TimeTitle.TotalSeconds = (uint)timeSpan.TotalSeconds;
                else
                    TimeTitle.TotalSeconds = (uint)0;
                user.Player.TitleWithTime.Add(titleID, TimeTitle);
                Database.TitleStorage dbtitle;
                if (Database.TitleStorage.Titles.TryGetValue(titleID, out dbtitle))
                {
                    user.Player.SpecialTitleScore = dbtitle.Score;
                    user.Player.SpecialHaloID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);

                }
            }
        }
        public static string GetHaloTitles(Client.GameClient user)
        {
            Database.DBActions.WriteLine writer = new DBActions.WriteLine(',');
            writer.Add((uint)user.Player.HaloTitles.Count);
            foreach (var title in user.Player.HaloTitles)
            {
                writer.Add((uint)title);
                if (user.Player.SpecialHaloID / 10000 == (uint)title && user.Player.SpecialHaloID / 10000 != 0)
                {
                    writer.Add((byte)1);
                }

                else
                {
                    writer.Add((byte)0);
                }

            }
            return writer.Close();
        }
        public static void LoadHaloTitles(Client.GameClient user, string line)
        {
            Database.DBActions.ReadLine reader = new DBActions.ReadLine(line, ',');
            uint count = reader.Read((uint)0);
            for (int x = 0; x < count; x++)
            {
                uint Title = reader.Read((uint)0);
                uint Active = reader.Read((uint)0);
                user.Player.HaloTitles.Add((Game.MsgServer.MsgTitleStorage.HaloType)Title);
                if (Active == 1)
                {
                    Database.TitleStorage dbtitle;
                    if (Database.TitleStorage.Titles.TryGetValue(Title, out dbtitle))
                    {
                        user.Player.SpecialTitleScore = dbtitle.Score;
                        user.Player.SpecialHaloID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);

                    }
                }
            }
        }
        public static void LoadSpecialTitles(Client.GameClient user, string line)
        {
            Database.DBActions.ReadLine reader = new DBActions.ReadLine(line, ',');
            uint count = reader.Read((uint)0);
            for (int x = 0; x < count; x++)
            {
                uint Title = reader.Read((uint)0);
                uint Active = reader.Read((uint)0);
                user.Player.SpecialTitles.Add((Game.MsgServer.MsgTitleStorage.TitleType)Title);
                if (Active == 1)
                {
                    Database.TitleStorage dbtitle;
                    if (Database.TitleStorage.Titles.TryGetValue(Title, out dbtitle))
                    {
                        if (dbtitle.ID >= 1 && dbtitle.ID <= 2456)
                        {
                            user.Player.SpecialTitleScore = dbtitle.Score;
                            user.Player.SpecialTitleID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);
                        }

                    }
                }
            }
        }
        public static void LoadSpecialWings(Client.GameClient user, string line)
        {
            Database.DBActions.ReadLine reader = new DBActions.ReadLine(line, ',');
            uint count = reader.Read((uint)0);
            for (int x = 0; x < count; x++)
            {
                uint Title = reader.Read((uint)0);
                uint Active = reader.Read((uint)0);
                user.Player.WingsTitles.Add((Game.MsgServer.MsgTitleStorage.WingsType)Title);
                if (Active == 1)
                {
                    Database.TitleStorage dbtitle;
                    if (Database.TitleStorage.Titles.TryGetValue(Title, out dbtitle))
                    {
                        if (dbtitle.ID >= 4001 && dbtitle.ID <= 6137)
                        {
                            user.Player.SpecialWingID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);
                        }
                    }
                }
            }
        }
        public static void LoadSpecialFoot(Client.GameClient user, string line)
        {
            Database.DBActions.ReadLine reader = new DBActions.ReadLine(line, ',');
            uint count = reader.Read((uint)0);
            for (int x = 0; x < count; x++)
            {
                uint Title = reader.Read((uint)0);
                uint Active = reader.Read((uint)0);
                user.Player.Footprint.Add((Game.MsgServer.MsgTitleStorage.Footprint)Title);
                if (Active == 1)
                {
                    Database.TitleStorage dbtitle;
                    if (Database.TitleStorage.Titles.TryGetValue(Title, out dbtitle))
                    {
                        if (dbtitle.ID >= 9001 && dbtitle.ID <= 9010)
                        {
                            user.Player.SpecialFootprintID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);
                        }
                    }
                }
            }
        }
        public static void LoadSpecialHaloAction(Client.GameClient user, string line)
        {
            Database.DBActions.ReadLine reader = new DBActions.ReadLine(line, ',');
            uint count = reader.Read((uint)0);
            for (int x = 0; x < count; x++)
            {
                uint Title = reader.Read((uint)0);
                uint Active = reader.Read((uint)0);
                user.Player.HaloAction.Add((Game.MsgServer.MsgTitleStorage.HaloAction)Title);
                if (Active == 1)
                {
                    Database.TitleStorage dbtitle;
                    if (Database.TitleStorage.Titles.TryGetValue(Title, out dbtitle))
                    {
                        if (dbtitle.ID >= 9500 && dbtitle.ID <= 9508)
                        {
                            user.Player.SpecialHaloAction = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);
                        }
                    }
                }
            }
        }
        
        public static void LoadCharacter(Client.GameClient client, uint UID)
        {
            
            client.Player.UID = UID;
            WindowsAPI.IniFile reader = new WindowsAPI.IniFile("\\Users\\" + UID + ".ini");
            reader.Write<byte>("Character", "Online", 1);
                //new MySqlCommand(MySqlCommandType.UPDATE).Update("playersonline").Set("Online", client.Player.Online).Execute();
            client.Player.Online += 1;
            client.Player.RelicSpirit = reader.ReadBool("Character", "RelicSpirit", false);
            if (client.Beasts == null)//For new accounts who has created the instance before
                client.Beasts = new Role.Instance.Beasts(client);
            if (client.HairfaceStorage == null)//For new accounts who has created the instance before
                client.HairfaceStorage = new Role.Instance.HairfaceStorage(client);
            client.Player.WarDropeFull = reader.ReadUInt32("Character", "WarDropeFull", 0);
            client.Player.Name = reader.ReadString("Character", "Name", "None");


            client.HairfaceStorage.Load(client.Player.UID);
            client.Beasts.Activated = reader.ReadBool("Character", "TailedBeastActivated", false);
            client.Player.CollectionID = reader.ReadUInt32("Character", "CollectionID", 0);
            client.Player.PandaID = reader.ReadUInt32("Character", "PandaID", 0);
            client.Beasts.TotalPoints = reader.ReadUInt32("Character", "TailedBeastTotalPoints", 0);
            client.Beasts.FruitToday = reader.ReadUInt32("Character", "FruitToday", 0);
            client.Beasts.Flag = reader.ReadUInt32("Character", "RuneFlag", 0);
            client.Player.DeityLandLuckyPoints = reader.ReadUInt32("Character", "DeityLandLuckyPoints", 0);
            client.Player.HeroPoints = reader.ReadUInt32("Character", "HeroPoints", 0);
            client.Player.ClassExperience = reader.ReadUInt32("Character", "ClassExperience", 0);
            client.Player.SavePromote = reader.ReadUInt32("Character", "SavePromote", 0);
            client.Player.Body = reader.ReadUInt16("Character", "Body", 1002);
            client.Player.Face = reader.ReadUInt16("Character", "Face", 0);
            client.Player.Spouse = reader.ReadString("Character", "Spouse", "None");
            client.Player.Description = reader.ReadString("Character", "Description", "None");
            client.Player.Class = reader.ReadUInt32("Character", "Class", 0);
            client.Player.FirstClass = reader.ReadUInt32("Character", "FirstClass", 0);
            client.Player.SecoundeClass = reader.ReadUInt32("Character", "SecoundeClass", 0);
            client.Player.Avatar = reader.ReadUInt16("Character", "Avatar", 0);
            client.Player.Map = reader.ReadUInt32("Character", "Map", 1002);
            client.Player.X = reader.ReadUInt16("Character", "X", 308);
            client.Player.Y = reader.ReadUInt16("Character", "Y", 288);

            client.Player.PMap = reader.ReadUInt32("Character", "PMap", 1002);
            client.Player.PMapX = reader.ReadUInt16("Character", "PMapX", 308);
            client.Player.PMapY = reader.ReadUInt16("Character", "PMapY", 288);

            client.Player.BlueRune = reader.ReadByte("Character", "BlueRune", 0);
            client.Player.RedRunes = reader.ReadByte("Character", "RedRunes", 0);
            client.Player.UnLockedSystem = reader.ReadBool("Character", "UnLockedSystem", false);
            client.Player.Agility = reader.ReadUInt16("Character", "Agility", 0);
            client.Player.Strength = reader.ReadUInt16("Character", "Strength", 0);
            client.Player.Spirit = reader.ReadUInt16("Character", "Spirit", 0);
            client.Player.Vitality = reader.ReadUInt16("Character", "Vitaliti", 0);
            client.Player.Atributes = reader.ReadUInt16("Character", "Atributes", 0);
            client.Player.PermenantBannedChat = reader.ReadBool("Character", "PermenantBannedChat", false);
            client.Player.IsBannedChat = reader.ReadBool("Character", "IsBannedChat", false);
            client.Player.BannedChatStamp = DateTime.FromBinary(reader.ReadInt64("Character", "BannedChatStamp", 0));
            client.Player.Reborn = reader.ReadByte("Character", "Reborn", 0);
            client.Player.Level = reader.ReadUInt16("Character", "Level", 0);
            client.Player.Hair = reader.ReadUInt32("Character", "Haire", 0);
            client.Player.RebornItem = reader.ReadByte("Character", "RebornItem", 0);
            client.Player.Experience = reader.ReadUInt64("Character", "Experience", 0);
            client.Player.HitPoints = reader.ReadInt32("Character", "MinHitPoints", 0);
            client.Player.Mana = reader.ReadUInt16("Character", "MinMana", 0);
            client.Player.ConquerPoints = (int)reader.ReadUInt32("Character", "ConquerPoints", 0);
            client.Player.DominoCoins = reader.ReadInt64("Character", "DominoCoins", 0);
            client.Player.DominoCode = reader.ReadUInt64("Character", "DominoCode", 0);
            client.Player.BoundConquerPoints = reader.ReadInt32("Character", "BoundConquerPoints", 0);
            client.Player.Money = reader.ReadInt64("Character", "Money", 0);
            client.Player.VirtutePoints = reader.ReadUInt32("Character", "VirtutePoints", 0);
            client.Player.PKPoints = reader.ReadUInt16("Character", "PkPoints", 0);
            client.Player.QuizPoints = reader.ReadUInt32("Character", "QuizPoints", 0);
            client.Player.Enilghten = reader.ReadUInt16("Character", "Enilghten", 0);
            client.Player.EnlightenReceive = reader.ReadUInt16("Character", "EnlightenReceive", 0);

            client.Player.questtwin = reader.ReadByte("Character", "questtwin", 0);
            client.Player.questphoinex = reader.ReadByte("Character", "questphoinex", 0);
            client.Player.questape = reader.ReadByte("Character", "questape", 0);
            client.Player.questdeserst = reader.ReadByte("Character", "questdeserst", 0);
            client.Player.questbird = reader.ReadByte("Character", "questbird", 0);

            client.Player.FootprintOpen = reader.ReadByte("Character", "FootprintOpen", 0);
            client.Player.HaloActionOpen = reader.ReadByte("Character", "HaloActionOpen", 0);

            client.Player.questone = reader.ReadByte("Character", "questone", 0);
            client.Player.questtwo = reader.ReadByte("Character", "questtwo", 0);
            client.Player.questthree = reader.ReadByte("Character", "questthree", 0);
            client.Player.questfour = reader.ReadByte("Character", "questfour", 0);


            client.Player.DailySignUpDays = reader.ReadUInt64("Character", "DailySignUpDays", 0);
            client.Player.DailyMonth = reader.ReadByte("Character", "DailyMonth", 0);
            client.Player.DailySignUpRewards = reader.ReadByte("Character", "DailySignUpRewards", 0);
            client.Player.VipLevel = reader.ReadByte("Character", "VipLevel", 0);
            client.Player.ExpireVipback = reader.ReadBool("Character", "VipTimeback", false);
            client.Player.ExpireVip = DateTime.FromBinary(reader.ReadInt64("Character", "VipTime", 0));
            if (DateTime.Now > client.Player.ExpireVip && !client.Player.ExpireVipback)
            {
                if (client.Player.VipLevel > 1)
                    client.Player.VipLevel = 0;
            }
            if (client.Player.VipLevel >= 4)
            {
                Game.ServerLogs.AccountVIP(client, client.Player.VipLevel);
            }
            client.Achievement = new AchievementCollection();
            client.Achievement.Load(reader.ReadString("Character", "Achivement", ""));

            client.Player.WHMoney = reader.ReadInt64("Character", "WHMoney", 0);
            client.Player.CpsBank = reader.ReadInt64("Character", "CpsBank", 0);
            client.Player.ResinanceRuneOne = reader.ReadUInt32("Character", "ResinanceRuneOne", 0);
            client.Player.ResinanceRelicTwo = reader.ReadUInt32("Character", "ResinanceRelicTwo", 0);
            client.Player.ResinanceRelicOne = reader.ReadUInt32("Character", "ResinanceRelicOne", 0);
            client.Player.ResinanceRuneTwo = reader.ReadUInt32("Character", "ResinanceRuneTwo", 0);
            client.Player.ResinanceRunethree = reader.ReadUInt32("Character", "ResinanceRunethree", 0);
            client.Player.ResinanceRunethree = reader.ReadUInt32("Character", "ResinanceRunefour", 0);
            client.Player.BlessTime = reader.ReadUInt32("Character", "BlessTime", 0);
            client.Player.SpouseUID = reader.ReadUInt32("Character", "SpouseUID", 0);
            client.Player.HeavenBlessing = reader.ReadInt32("Character", "HeavenBlessing", 0);
            client.Player.HeavenBlessTime = DateTime.FromBinary(reader.ReadInt64("Character", "LostTimeBlessing", 0));
            client.Player.HuntingBlessing = reader.ReadUInt32("Character", "HuntingBlessing", 0);
            client.Player.OnlineTrainingPoints = reader.ReadUInt32("Character", "OnlineTrainingPoints", 0);
            client.Player.JoinOnflineTG = DateTime.FromBinary(reader.ReadInt64("Character", "JoinOnflineTG", 0));
            client.Player.RateExp = reader.ReadUInt32("Character", "RateExp", 0);
            client.Player.DExpTime = reader.ReadUInt32("Character", "DExpTime", 0);
            client.Player.VotePoints = reader.ReadByte("Character", "VotePoints", 0);
            client.Player.Day = reader.ReadInt32("Character", "Day", 0);
            client.Player.BDExp = reader.ReadByte("Character", "BDExp", 0);
            client.Player.ExpBallUsed = reader.ReadByte("Character", "ExpBallUsed", 0);
            client.Player.ExchangehighAvaliability = reader.ReadUInt32("Character", "ExchangehighAvaliability", 0);
            client.EnemyInvade.Load(reader.ReadString("Character", "EnemyInvade", "0/"));
            if (client.Twisted == null)
                client.Twisted = new MsgTwistedFututr();
            
            MsgTwistedFututr.Load(client);



            DataCore.LoadClient(client.Player);

            client.Player.GuildID = reader.ReadUInt32("Character", "GuildID", 0);
            client.Player.GuildRank = (Role.Flags.GuildMemberRank)reader.ReadUInt32("Character", "GuildRank", 200);
            if (client.Player.GuildID != 0)
            {
                Role.Instance.Guild myguild;
                if (Role.Instance.Guild.GuildPoll.TryGetValue(client.Player.GuildID, out myguild))
                {
                    client.Player.MyGuild = myguild;
                    Role.Instance.Guild.Member member;
                    if (myguild.Members.TryGetValue(client.Player.UID, out member))
                    {
                        member.IsOnline = true;
                        client.Player.GuildID = (ushort)myguild.Info.GuildID;
                        client.Player.MyGuildMember = member;
                        client.Player.GuildRank = member.Rank;
                        client.Player.GuildBattlePower = myguild.ShareMemberPotency(member.Rank);


                    }
                    else
                    {
                        client.Player.MyGuild = null;
                        client.Player.GuildID = 0;
                        client.Player.GuildRank = (Role.Flags.GuildMemberRank)0;
                    }
                }
                else
                {
                    client.Player.MyGuild = null;
                    client.Player.GuildID = 0;
                    client.Player.GuildRank = (Role.Flags.GuildMemberRank)0;
                }
            }

            uint UnionID = reader.ReadUInt32("Character", "UnionUID", 0);
            if (UnionID != 0 && client.Player.GuildID == 0)
            {
                Role.Instance.Union union;
                if (Role.Instance.Union.UnionPoll.TryGetValue(UnionID, out union))
                {
                    Role.Instance.Union.Member Member;
                    if (union.Members.TryGetValue(client.Player.UID, out Member))
                    {
                        client.Player.MyUnion = union;
                        client.Player.UnionMemeber = Member;
                        client.Player.UnionMemeber.Owner = client;
                    }
                }
            }
            else if (client.Player.GuildID != 0 && client.Player.MyGuild != null && client.Player.MyGuild.UnionID != 0)
            {
                var union = client.Player.MyGuild.GetUnion;
                if (union != null)
                {
                    Role.Instance.Guild.Member Member;
                    if (client.Player.MyGuild.Members.TryGetValue(client.Player.UID, out Member))
                    {
                        client.Player.UnionMemeber = Member.UnionMem;
                        client.Player.UnionMemeber.Owner = client;
                        client.Player.MyUnion = union;
                    }
                }
            }

            if (client.Player.InUnion)
            {
                if (client.Player.UnionMemeber.Rank == Role.Instance.Union.Member.MilitaryRanks.Emperor)
                {
                    if (client.Player.MyUnion.EmperrorUID != client.Player.UID)
                        client.Player.UnionMemeber.Rank = Role.Instance.Union.Member.MilitaryRanks.Member;
                }
            }


            client.Player.SubClass = new Role.Instance.SubClass();
            client.Player.SubClass.Load(reader.ReadString("Character", "SubProfInfo", ""));
            client.Player.SubClass.CreateSpawn(client);

            if (Role.Instance.Chi.ChiPool.ContainsKey(UID))
            {
                client.Player.MyChi = Role.Instance.Chi.ChiPool[UID];
                Role.Instance.Chi.ComputeStatus(client.Player.MyChi);
            }
            else
                client.Player.MyChi = new Role.Instance.Chi(UID);

            if (Role.Instance.Flowers.ClientPoll.ContainsKey(UID))
                client.Player.Flowers = Role.Instance.Flowers.ClientPoll[UID];
            else
                client.Player.Flowers = new Role.Instance.Flowers(UID, client.Player.Name);
            string flowerStr = reader.ReadString("Character", "Flowers", "");
            Database.DBActions.ReadLine Linereader = new DBActions.ReadLine(flowerStr, '/');
            client.Player.Flowers.FreeFlowers = Linereader.Read((uint)0);

            Role.Instance.Nobility nobility;
            if (Pool.NobilityRanking.TryGetValue(UID, out nobility))
            {
                client.Player.Nobility = nobility;
                client.Player.NobilityRank = client.Player.Nobility.Rank;
            }
            else
            {
                client.Player.Nobility = new Role.Instance.Nobility(client);
                client.Player.Nobility.Donation = reader.ReadUInt64("Character", "DonationNobility", 0);
                client.Player.NobilityRank = client.Player.Nobility.Rank;
            }
            


            Role.Instance.JiangHu Jiang;
            if (Role.Instance.JiangHu.Poll.TryGetValue(client.Player.UID, out Jiang))
            {
                client.Player.MyJiangHu = Jiang;
                client.Player.MyJiangHu.Level = (byte)client.Player.Level;
                client.Player.MyJiangHu.CountDownMode = DateTime.Now;
            }


            Role.Instance.Associate.MyAsociats Associate;
            if (Role.Instance.Associate.Associates.TryGetValue(client.Player.UID, out Associate))
            {
                client.Player.Associate = Associate;
                client.Player.Associate.MyClient = client;
                client.Player.Associate.Online = true;
                if (client.Player.Associate.Associat.ContainsKey(Role.Instance.Associate.Mentor))
                {
                    foreach (var member in client.Player.Associate.Associat[Role.Instance.Associate.Mentor].Values)
                    {
                        if (member.UID != 0)
                        {
                            Role.Instance.Associate.MyAsociats mentor;
                            if (Role.Instance.Associate.Associates.TryGetValue(member.UID, out mentor))
                            {
                                client.Player.MyMentor = mentor;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                client.Player.Associate = new Role.Instance.Associate.MyAsociats(client.Player.UID);
                client.Player.Associate.MyClient = client;
                client.Player.Associate.Online = true;
            }
            client.Player.ClanUID = reader.ReadUInt32("Character", "ClanID", 0);
            if (client.Player.ClanUID != 0)
            {
                Role.Instance.Clan myclan;
                if (Role.Instance.Clan.Clans.TryGetValue(client.Player.ClanUID, out myclan))
                {
                    client.Player.MyClan = myclan;
                    Role.Instance.Clan.Member member;
                    if (myclan.Members.TryGetValue(client.Player.UID, out member))
                    {
                        member.Online = true;
                        client.Player.ClanName = myclan.Name;
                        client.Player.MyClanMember = member;
                        client.Player.ClanRank = (ushort)member.Rank;
                    }
                    else
                    {
                        client.Player.MyClan = null;
                        client.Player.ClanUID = 0;
                        client.Player.ClanRank = 0;
                    }
                }
                else
                    client.Player.ClanUID = 0;
            }

            client.Player.FirstRebornLevel = reader.ReadByte("Character", "FRL", 0);
            client.Player.SecoundeRebornLevel = reader.ReadByte("Character", "SRL", 0);
            client.Player.Reincarnation = reader.ReadBool("Character", "Reincanation", false);
            client.Player.LotteryEntries = reader.ReadByte("Character", "LotteryEntries", 0);
            client.Player.DbTry = reader.ReadBool("Character", "DbTry", false);
            client.DemonExterminator.ReadLine(reader.ReadString("Character", "DemonEx", "0/0/"));
            client.Player.FairyForm = reader.ReadInt32("Character", "FairyForm", -1);
            client.Player.MyKillerUID = reader.ReadUInt32("Character", "PkName", 0);
            client.Player.MyKillerName = reader.ReadString("Character", "PkName", "None");
            client.Player.PinCodeAnima = reader.ReadString("Character", "PinCodeAnima", "");
            client.Player.CursedTimer = reader.ReadInt32("Character", "Cursed", 0);
            client.HeroRewards.Load(reader.ReadBigString("Character", "HeroRewards", ""));
            client.Activeness.Load(reader.ReadBigString("Character", "Activeness", ""));
            client.Player.AtiveQuestApe = reader.ReadUInt32("Character", "enervant", 0);

            client.Player.AparenceType = reader.ReadUInt32("Character", "AparenceType", 0);
            client.Player.TableBetDice = reader.ReadUInt32("Character", "TableBetDice", 0);
            client.Player.HitShoot = reader.ReadUInt32("Character", "HitShoot", 0);
            client.Player.MisShoot = reader.ReadUInt32("Character", "MisShoot", 0);
            client.Player.ArenaKills = reader.ReadUInt32("Character", "ArenaKills", 0);
            client.Player.ArenaDeads = reader.ReadUInt32("Character", "ArenaDeads", 0);

            client.Player.TournamentKills = reader.ReadUInt32("Character", "TKills", 0);

            client.Player.WorldPoints = reader.ReadUInt32("Character", "WorldPoints", 0);
            client.Player.OnlineMinutes = reader.ReadUInt32("Character", "OnlineMinutes", 0);
            client.Player.ScoreHuntCoins = reader.ReadUInt32("Character", "ScoreHuntCoins", 0);
            client.Player.CastlePoint = reader.ReadUInt32("Character", "CastlePoint", 0);
            client.Player.OnlinePointsPK = reader.ReadUInt32("Character", "OnlinePointsPK", 0);
            client.Player.EpicQuestChance = reader.ReadUInt32("Character", "EpicQuestChance", 0);
            client.Player.EpicNinjaQuestChance = reader.ReadUInt32("Character", "EpicNinjaQuestChance", 0);
            client.Player.EpicPirateQuestChance = reader.ReadUInt32("Character", "EpicPirateQuestChance", 0);
            client.Player.HistoryChampionPoints = reader.ReadUInt32("Character", "HistoryChampionPoints", 0);

            client.Player.AddChampionPoints(reader.ReadUInt32("Character", "ChampionPoints", 0), false);
            client.Player.TodayChampionPoints = reader.ReadUInt32("Character", "TodayChampionPoints", 0);
            client.Player.DailySpiritBeadItem = reader.ReadUInt32("Character", "DailySpiritBeadItem", 0);
            LoadSpecialTitles(client, reader.ReadBigString("Character", "SpecialTitles", "0/"));
            LoadSpecialWings(client, reader.ReadBigString("Character", "SpecialWings", "0/"));
            LoadSpecialFoot(client, reader.ReadBigString("Character", "SpecialFoot", "0/"));
            LoadSpecialHaloAction(client, reader.ReadBigString("Character", "HaloAction", "0/"));

            LoadHaloTitles(client, reader.ReadBigString("Character", "SpecialHalo", "0/"));
            LoadTitleWithTime(client, reader.ReadBigString("Character", "TitleTime", "0/"));
            LoadSecurityPassword(reader.ReadString("Character", "SecurityPass", "0,0,0"), client);
            client.Player.TCCaptainTimes = reader.ReadByte("Character", "TCT", 0);
            client.Player.RacePoints = reader.ReadUInt32("Character", "RacePoints", 0);
            client.Player.DonatePoints = reader.ReadDouble("Character", "DonatePoints", 0);
            client.Player.RamdanBag = reader.ReadUInt32("Character", "RamdanBag", 0);
            client.Player.itemRamdanBag = reader.ReadUInt32("Character", "itemRamdanBag", 0);
            client.Player.RewardPoints = reader.ReadUInt32("Character", "RewardPoints", 0);
            client.Player.ItemRewordChristmas = reader.ReadUInt32("Character", "ItemRewordChristmas", 0);
            client.Player.NameEditCount = reader.ReadUInt16("Character", "NameEditCount", 0);
            client.Player.MainFlag = (Role.Player.MainFlagType)reader.ReadUInt32("Character", "ClaimStateGift", 0);
            client.Player.CountryID = reader.ReadUInt16("Character", "CountryID", 0);

            client.Player.InventorySashCount = reader.ReadUInt16("Character", "InventorySashCount", 0);
            client.Player.MyFootBallPoints = reader.ReadUInt32("Character", "MyFootBallPoints", 0);
            client.Player.ExpProtection = reader.ReadUInt32("Character", "ExpProtection", 0);
            client.BanCount = reader.ReadByte("Character", "BanCount", 0);
            client.Player.KingDomExploits = reader.ReadUInt32("Character", "KingDomExploits", 0);


            client.Player.BuyKingdomDeeds = reader.ReadByte("Character", "BuyKingdomDeeds", 0);
            client.Player.KingDomDeeds = reader.ReadUInt32("Character", "KingDomDeeds", 0);


            client.MyExchangeShop.Load(reader.ReadBigString("Character", "ExchangeShop", "0"));
            client.DragonSkin.Load(reader.ReadBigString("Character", "AnimaSkin", "0"));
            client.GuildSkill.Load(reader.ReadBigString("Character", "GuildSkill", "0"));
            client.Player.MyDontion = reader.ReadUInt32("Character", "GuildDontion", 0);
            client.Player.CyanJadeRing = reader.ReadUInt32("Character", "CyanJadeRing", 0);
            client.Player.ExtraAtributes = reader.ReadUInt16("Character", "ExtraAtributes", 0);
            //botjail
            client.Player.OpenHousePack = reader.ReadByte("Character", "OpenHousePack", 0);
         
            client.Player.botjail = reader.ReadByte("Character", "botjail", 0);
          
            client.Player.Online = reader.ReadByte("Character", "Online", 0);
            client.Player.MyTowerOfMysteryLayer = reader.ReadByte("Character", "MyTowerOfMysteryLayer", 0);

            client.Player.ClaimTowerAmulets = reader.ReadByte("Character", "ClaimTowerAmulets", 0);
            client.Player.TOMClaimTeamReward = reader.ReadByte("Character", "TOMClaimTeamReward", 0);
            client.Player.MyTowerOfMysteryLayerElite = reader.ReadByte("Character", "MyTowerOfMysteryLayerElite", 0);
            client.Player.TowerOfMysterychallenge = reader.ReadByte("Character", "TowerOfMysterychallenge", 0);
            client.Player.TowerOfMysteryChallengeFlag = reader.ReadUInt32("Character", "TowerOfMysteryChallengeFlag", 0);
            client.Player.TOMSelectChallengeToday = reader.ReadByte("Character", "TOMSelectChallengeToday", 0);
            client.Player.TOMChallengeToday = reader.ReadByte("Character", "TOMChallengeToday", 0);
            client.Player.TOMRefreshReward = reader.ReadUInt32("Character", "TOMRefreshReward", 0);
            client.Player.JoinPowerArenaStamp = DateTime.FromBinary(reader.ReadInt64("Character", "JPAStamp", 0));
            client.Player.TOM_Reward = (Game.MsgTournaments.MsgTowerOfMystery.RewardTypes)reader.ReadByte("Character", "TOM_Reward", 0);

            client.Player.LoadEpicTrojan(reader.ReadString("Character", "EpicTrojan", "0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0"));
            client.Player.LoadEpicArcher(reader.ReadString("Character", "EnergyPoints", "0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0"));
            client.Player.LoadArchiveWarrior(reader.ReadString("Character", "QuestWarrior", "0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0/0"));
           
            client.Player.GiveFlowersToPerformer = reader.ReadInt32("Character", "GiveFlowersToPerformer", 0);
            client.Player.ClaimPointsArena = reader.ReadUInt16("Character", "ClaimPointsArena", 0);
            client.Player.UseChiToken = reader.ReadByte("Character", "UseChiToken", 0);
            client.Player.ChiToken = reader.ReadByte("Character", "ChiToken", 0);
            client.Player.Vote = reader.ReadByte("Character", "Vote", 0);
            client.Player.KillCount = reader.ReadByte("Character", "KillCount", 0);
            client.Player.JiangToken = reader.ReadByte("Character", "JiangToken", 0);
            client.Player.CanChangeWindWalkerFree = DateTime.FromBinary(reader.ReadInt64("Character", "CanChangeWindWalkerFree", DateTime.Now.Ticks));

            LoadClientItems(client);
            if (client.HundredWeapons == null)
                client.HundredWeapons = new Role.Instance.HundredWeapons(client);

            if (client.MyNinja == null)
                client.MyNinja = new Role.Instance.Ninja(client);
            if (client.MyArchives == null)
                client.MyArchives = new Role.Instance.Archives(client);
               if (client.MyAstredge == null)
                client.MyAstredge = new Role.Instance.Astredge(client);
            client.MyAstredge.Load(reader.ReadBigString("Character", "MyAstredge", ""));
            client.MyArchives.Load(reader.ReadBigString("Character", "MyArchives", ""));
            client.Collection.Load(reader.ReadBigString("Character", "Collection", ""));
            client.Player.LoadAgates(reader.ReadBigString("Character", "MemoryAgate", ""));
            client.CoatColorRule.Load(client.Player.UID);
            LoadClientSpells(client);
            LoadClientProfs(client);
            LoadClientMails(client);
            RoleQuests.Load(client);
            Role.Instance.House.Load(client);
            client.HundredWeapons.Load(reader.ReadBigString("Character", "HundredWeapons", "0"));
            client.MyNinja.Load(reader.ReadBigString("Character", "NinjaNinpo", "0"));
            client.MedalStorage.Load(reader.ReadBigString("Character", "MedalStorage", "0"));
            client.Player.MountArmorColor = reader.ReadUInt32("Character", "MountArmorColor", 0);
            client.Player.FrameID = reader.ReadUInt16("Character", "FrameID", 0);
            ResetingEveryDay(client);


            Role.Instance.Confiscator Container;
            if (Pool.QueueContainer.PollContainers.TryGetValue(client.Player.UID, out Container))
                client.Confiscator = Container;
            try
            {
                client.Player.Associate.OnLoading(client);
            }
            catch (Exception e) { MyConsole.WriteLine(e.ToString()); }

            if (!Role.Instance.InnerPower.InnerPowerPolle.TryGetValue(client.Player.UID, out client.Player.InnerPower))
                client.Player.InnerPower = new Role.Instance.InnerPower(client.Player.Name, client.Player.UID);

            client.Player.InnerPower.UpdateStatus();

            if (Game.MsgTournaments.MsgArena.ArenaPoll.TryGetValue(client.Player.UID, out client.ArenaStatistic))
            {
                client.ArenaStatistic.ApplayInfo(client.Player);
            }
            else
            {
                client.ArenaStatistic = new Game.MsgTournaments.MsgArena.User();
                client.ArenaStatistic.ApplayInfo(client.Player);
                client.ArenaStatistic.Info.ArenaPoints = 1000;
                Game.MsgTournaments.MsgArena.ArenaPoll.TryAdd(client.Player.UID, client.ArenaStatistic);
            }

           
            client.FullLoading = true;
        }

        public unsafe static void LoadClientItems(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersItems\\" + client.Player.UID + ".bin", FileMode.Open))
            {
                ClientItems.DBItem Item;
                int ItemCount;
                binary.Read(&ItemCount, sizeof(int));
                Dictionary<uint, MsgGameItem> InContainer = new Dictionary<uint, MsgGameItem>();
                for (int x = 0; x < ItemCount; x++)
                {
                    binary.Read(&Item, sizeof(ClientItems.DBItem));
                    if (Item.ITEM_ID == 750000)
                        client.DemonExterminator.ItemUID = Item.UID;
                    if (Item.ITEM_ID / 1000 == 203)
                    {
                        if (Item.Enchant > 0)
                            Item.Enchant = 0;
                        if (Item.Bless > 1)
                            Item.Bless = 0;
                    }
                    if (Item.Bless > 7)
                        Item.Bless = 7;
                    if (Item.Plus > 12)
                        Item.Plus = 12;
                    if (Item.Enchant > 255)
                        Item.Enchant = 255;

                    if (Item.ITEM_ID / 1000 == 201 || Item.ITEM_ID / 1000 == 202 || Item.ITEM_ID / 1000 == 203)
                    {
                        if (Item.Bless > 1)
                            Item.Bless = 1;

                        if (Item.Enchant > 0)
                            Item.Enchant = 0;
                    }

                    if (Item.ITEM_ID / 1000 == 204)
                    {
                        if (Item.Bless > 0)
                            Item.Bless = 0;

                        if (Item.Enchant > 0)
                            Item.Enchant = 0;
                    }

                    if (Item.ITEM_ID >= 4100001 && Item.ITEM_ID <= 4100005)
                    {
                        if (Item.AnimaItemID > 0)
                            Item.AnimaItemID = 0;
                    }

                    Game.MsgServer.MsgGameItem ClienItem = Item.GetDataItem();
                    if (ClienItem.Delete)
                        continue;
                    if (Item.DepositeCount != 0)
                    {
                        uint DepositeCount = Item.DepositeCount;
                        for (int i = 0; i < DepositeCount; i++)
                        {
                            binary.Read(&Item, sizeof(ClientItems.DBItem));
                            if (Item.ITEM_ID == 750000)
                                client.DemonExterminator.ItemUID = Item.ITEM_ID;

                            Game.MsgServer.MsgGameItem DepositeItem = Item.GetDataItem();
                            if (client.Player.GuildID == 0)
                                DepositeItem.Inscribed = 0;
                            ClienItem.Deposite.TryAdd(DepositeItem.UID, DepositeItem);
                            if (!InContainer.ContainsKey(DepositeItem.UID))
                                InContainer.Add(DepositeItem.UID, DepositeItem);
                        }
                    }
                    if (Item.WH_ID != 0)
                    {
                       
                        if (Item.WH_ID == 100)
                        {
                            client.MyWardrobe.AddItem(ClienItem);
                            if (Item.Position > 0 && Item.Position <= (ushort)Role.Flags.ConquerItem.AleternanteGarment)
                            {
                                client.Equipment.ClientItems.TryAdd(Item.UID, ClienItem);
                            }
                        }
                        else
                        {
                            if (!client.Warehouse.ClientItems.ContainsKey(Item.WH_ID))
                                client.Warehouse.ClientItems.TryAdd(Item.WH_ID, new System.Collections.Concurrent.ConcurrentDictionary<uint, Game.MsgServer.MsgGameItem>());
                           
                            if (client.Player.GuildID == 0)
                                ClienItem.Inscribed = 0;
                            client.Warehouse.ClientItems[Item.WH_ID].TryAdd(Item.UID, ClienItem);
                            
                        }
                    }
                    else
                    {
                      
                        if (Item.Position > (ushort)0 && Item.Position <= (ushort)59)
                            client.Equipment.ClientItems.TryAdd(Item.UID, ClienItem);
                        if (Item.Position == (ushort)Role.Flags.ConquerItem.MythSoulBag)
                        {
                            client.MythSoulBag.Add(ClienItem.UID, ClienItem);
                        }


                        else if (Item.Position == (ushort)Role.Flags.ConquerItem.RelicResonance)
                        {
                            client.Relics.Add(ClienItem.UID, ClienItem);
                        }
                        else if (Item.Position == 0 )
                        {
                            client.Inventory.AddDBItem(ClienItem);
                        }
                        if (Item.Position > 0 && Item.Position <= (ushort)Role.Flags.ConquerItem.AleternanteRelics)
                        {
                            client.Equipment.ClientItems.TryAdd(Item.UID, ClienItem);
                        }
                       
                        else if ((Item.Position == (ushort)Role.Flags.ConquerItem.RuneBag || Item.Position == (ushort)Role.Flags.ConquerItem.RunesCollection) || (Item.Position >= (ushort)Role.Flags.ConquerItem.RedRune && Item.Position <= (ushort)Role.Flags.ConquerItem.AlternateYellowRune + 11))
                        {
                            try
                            {
                                if (!Database.ItemType.EquipPassJobReq(Pool.ItemsBase[ClienItem.ITEM_ID], client))
                                    Item.Position = (ushort)Role.Flags.ConquerItem.RunesCollection;
                            }
                            catch
                            {
                            }
                            if (client.Rune.Free(Item.Position))
                            {
                                client.Rune.Add(ClienItem);
                            }
                            
                        }
                        
                    }
                    if (Item.ITEM_ID >= 4200012 && Item.ITEM_ID <= 4200018)
                    {
                        int Count = client.Inventory.GetCountItem(Item.ITEM_ID);
                        Game.ServerLogs.AnimaAlots(client, Item.ITEM_ID, Count);
                    }
                    else if ((Item.Position >= (ushort)VirusX.Role.Enums.EonspiritPosition.EonspiritInventory && Item.Position <= (ushort)VirusX.Role.Enums.EonspiritPosition.EonspiritUnActive) && Database.ItemType.EonspiritItem.Contains(Item.ITEM_ID))
                    {
                        client.EonspiritSystem.Add(ClienItem.UID, ClienItem);
                    }
                }
                binary.Read(&ItemCount, sizeof(int));
                for (int x = 0; x < ItemCount; x++)
                {
                    ClientItems.Perfection info = new ClientItems.Perfection();
                    binary.Read(&info, sizeof(ClientItems.Perfection));
                    if (InContainer.ContainsKey(info.ItemUID))
                    {
                        var item = InContainer[info.ItemUID];
                        item.PerfectionLevel = info.Level;
                        item.OwnerUID = info.OwnerUID;
                        item.OwnerName = info.OwnerName;
                        item.PerfectionProgress = info.Progres;
                        item.Signature = info.SpecialText;
                        continue;
                    }
                    else if (client.Equipment.ClientItems.ContainsKey(info.ItemUID))
                    {
                        var item = client.Equipment.ClientItems[info.ItemUID];
                        item.PerfectionLevel = info.Level;
                        item.OwnerUID = info.OwnerUID;
                        item.OwnerName = info.OwnerName;
                        item.PerfectionProgress = info.Progres;
                        item.Signature = info.SpecialText;
                        continue;
                    }
                    else if (client.Inventory.ClientItems.ContainsKey(info.ItemUID))
                    {
                        var item = client.Inventory.ClientItems[info.ItemUID];
                        item.PerfectionLevel = info.Level;
                        item.OwnerUID = info.OwnerUID;
                        item.OwnerName = info.OwnerName;
                        item.PerfectionProgress = info.Progres;
                        item.Signature = info.SpecialText;
                        continue;
                    }
                    foreach (var WH in client.Warehouse.ClientItems.Values)
                    {
                        if (WH.ContainsKey(info.ItemUID))
                        {
                            var item = WH[info.ItemUID];
                            item.PerfectionLevel = info.Level;
                            item.OwnerUID = info.OwnerUID;
                            item.OwnerName = info.OwnerName;
                            item.PerfectionProgress = info.Progres;
                            item.Signature = info.SpecialText;
                            break;
                        }

                    }
                }

                int CountRelics = 0;
                if (binary.Read(&CountRelics, sizeof(int)))
                {
                    for (int x = 0; x < CountRelics; x++)
                    {
                        Role.Instance.RelicAttribute[] RelicAttributes = new Role.Instance.RelicAttribute[5];
                        uint RelicUID = 0;
                        binary.Read(&RelicUID, sizeof(uint));
                        int AmountAttributes = 0;
                        binary.Read(&AmountAttributes, sizeof(uint));
                        for (int i = 0; i < AmountAttributes; i++)
                        {
                            uint attribut = 0;
                            binary.Read(&attribut, sizeof(uint));
                            RelicAttributes[i] = new Role.Instance.RelicAttribute(attribut);
                        }

                        MsgGameItem ClientItem;
                        if (client.TryGetGlobalClientItem(RelicUID, out ClientItem))
                        {
                            if (ClientItem.ITEM_ID >= 4100001 && ClientItem.ITEM_ID <= 4100005 && ClientItem.Plus > 0)
                                continue;
                            if (ClientItem.ITEM_ID >= 4100001 && ClientItem.ITEM_ID <= 4100005 && ClientItem.Bless > 0)
                                continue;
                            ClientItem.RelicAttributes = RelicAttributes;
                        }
                        else if (InContainer.ContainsKey(RelicUID))
                        {
                            var item = InContainer[RelicUID];
                            item.RelicAttributes = RelicAttributes;
                        }
                    }

                }
               
                binary.Close();

            }
        }

        public unsafe static void SaveClientItems(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersItems\\" + client.Player.UID + ".bin", FileMode.Create))
            {
                Dictionary<uint, MsgGameItem> Relics = new Dictionary<uint, MsgGameItem>();
                Dictionary<uint, MsgGameItem> InContainer = new Dictionary<uint, MsgGameItem>();
                ClientItems.DBItem DBItem = new ClientItems.DBItem();
                var items = client.AllMyItems().ToArray();
                int ItemCount = items.Length;
                binary.Write(&ItemCount, sizeof(int));
                foreach (var item in items)
                {
                    if (Database.ItemType.ItemPosition(item.ITEM_ID) == (ushort)Role.Flags.ConquerItem.RelicResonance)
                    {
                        if (!Relics.ContainsKey(item.UID))
                        {
                            Relics.Add(item.UID, item);
                        }
                    }
                    DBItem.GetDBItem(item);
                    if (!binary.Write(&DBItem, sizeof(ClientItems.DBItem)))
                        MyConsole.WriteLine("Unable to save an item!");
                    if (item.Deposite.Count > 0)
                    {
                        foreach (var DepositItem in item.Deposite.Values)
                        {
                            DBItem.GetDBItem(DepositItem);
                            binary.Write(&DBItem, sizeof(ClientItems.DBItem));
                            if ((DepositItem.PerfectionLevel > 0 || DepositItem.PerfectionProgress > 0) && DepositItem.IsEquip)
                                if (!InContainer.ContainsKey(DepositItem.UID))
                                    InContainer.Add(DepositItem.UID, DepositItem);
                        }
                    }
                }
                var perfectionItems = client.AllPerfectionItems().ToArray();
                ItemCount = perfectionItems.Length + InContainer.Count;
                binary.Write(&ItemCount, sizeof(int));
                foreach (var item in perfectionItems)
                {
                    var info = DBItem.GetPerfectionInfo(item);
                    if (!binary.Write(&info, sizeof(ClientItems.Perfection)))
                        MyConsole.WriteLine("Unable to save an item!");
                }
                foreach (var item in InContainer.Values)
                {
                    var info = DBItem.GetPerfectionInfo(item);
                    if (!binary.Write(&info, sizeof(ClientItems.Perfection)))
                        MyConsole.WriteLine("Unable to save an item!");
                }
                int RelicsCount = Relics.Count;
                binary.Write(&RelicsCount, sizeof(int));
                foreach (var item in Relics.Values)
                {
                    uint Relic_UID = item.UID;
                    binary.Write(&Relic_UID, sizeof(uint));
                    uint AmountAttributes = (uint)item.RelicAttributes.Count();
                    binary.Write(&AmountAttributes, sizeof(uint));
                    for (int x = 0; x < item.RelicAttributes.Count(); x++)
                    {
                        uint attribut = item.RelicAttributes[x];
                        binary.Write(&attribut, sizeof(uint));
                    }

                }
                binary.Close();
            }
        }
        public unsafe static void SaveClientMails(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersMail\\" + client.Player.UID + ".bin", FileMode.Create))
            {
                ClientMail.DBPrize DBprize = new ClientMail.DBPrize();
                int CountPrizes;
                CountPrizes = client.PrizeInfo.Count;
                binary.Write(&CountPrizes, sizeof(int));
                foreach (var prize in client.PrizeInfo.Values)
                {
                    if (prize.id == 0)
                        continue;
                    DBprize.GetDBPrize(prize);
                    binary.Write(&DBprize, sizeof(ClientMail.DBPrize));
                }
                binary.Close();
            }
        }
        public unsafe static void LoadClientMails(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersMail\\" + client.Player.UID + ".bin", FileMode.Open))
            {
                ClientMail.DBPrize DBprize;
                int CountPrizes;
                binary.Read(&CountPrizes, sizeof(int));
                for (int x = 0; x < CountPrizes; x++)
                {
                    binary.Read(&DBprize, sizeof(ClientMail.DBPrize));
                    var ClientPrize = DBprize.GetClientPrize();
                    if (ClientPrize != null)
                        client.PrizeInfo.Add(ClientPrize.id, ClientPrize);
                }
                binary.Close();
            }
        }
        public unsafe static void LoadClientProfs(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersProfs\\" + client.Player.UID + ".bin", FileMode.Open))
            {
                ClientProficiency.DBProf DBProf;
                int CountProf;
                binary.Read(&CountProf, sizeof(int));
                for (int x = 0; x < CountProf; x++)
                {
                    binary.Read(&DBProf, sizeof(ClientProficiency.DBProf));
                    var ClientProf = DBProf.GetClientProf();
                    client.MyProfs.ClientProf.TryAdd(ClientProf.ID, ClientProf);
                }
                binary.Close();
            }
        }
        public unsafe static void SaveClientProfs(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersProfs\\" + client.Player.UID + ".bin", FileMode.Create))
            {
                ClientProficiency.DBProf DBProf = new ClientProficiency.DBProf();
                int CountProf;
                CountProf = client.MyProfs.ClientProf.Count;
                binary.Write(&CountProf, sizeof(int));
                foreach (var prof in client.MyProfs.ClientProf.Values)
                {
                    DBProf.GetDBSpell(prof);
                    binary.Write(&DBProf, sizeof(ClientProficiency.DBProf));
                }
                binary.Close();
            }
        }

        public unsafe static void LoadClientSpells(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersSpells\\" + client.Player.UID + ".bin", FileMode.Open))
            {
                ClientSpells.DBSpell DBSpell;
                int CountSpell;
                binary.Read(&CountSpell, sizeof(int));
                for (int x = 0; x < CountSpell; x++)
                {
                    binary.Read(&DBSpell, sizeof(ClientSpells.DBSpell));
                    var clientSpell = DBSpell.GetClientSpell();
                    Dictionary<ushort, Database.MagicType.Magic> Spells;
                    if (Pool.Magic.TryGetValue(clientSpell.ID, out Spells))
                    {
                        if (Spells.Count > clientSpell.Level)
                        {
                            try
                            {

                                MagicType.Magic Magic = Pool.Magic[clientSpell.ID][clientSpell.Level];
                                if (Magic != null && Magic.isRuneSkill)
                                {
                                    var item = Role.Instance.Rune.GetRuneFromSkill(clientSpell);
                                    if (!Database.ItemType.EquipPassJobReq(Pool.ItemsBase[item], client)) continue;
                                }
                            }
                            catch
                            { }
                        }
                    }
                    client.MySpells.ClientSpells.TryAdd(clientSpell.ID, clientSpell);
                }
                binary.Close();
            }
        }
        public unsafe static void SaveClientSpells(Client.GameClient client)
        {
            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
            if (binary.Open(Program.ServerConfig.DbLocation + "\\PlayersSpells\\" + client.Player.UID + ".bin", FileMode.Create))
            {
                ClientSpells.DBSpell DBSpell = new ClientSpells.DBSpell();
                int SpellCount;
                SpellCount = client.MySpells.ClientSpells.Count;
                binary.Write(&SpellCount, sizeof(int));
                foreach (var spell in client.MySpells.ClientSpells.Values)
                {
                    DBSpell.GetDBSpell(spell);
                    binary.Write(&DBSpell, sizeof(ClientSpells.DBSpell));
                }
                binary.Close();
            }
        }
        public static void CreateCharacte(Client.GameClient client)
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + client.Player.UID + ".ini");
            write.Write<uint>("Character", "UID", client.Player.UID);
            write.Write<ushort>("Character", "Body", client.Player.Body);
            write.Write<ushort>("Character", "Face", client.Player.Face);
            write.WriteString("Character", "Name", client.Player.Name);
            write.Write<uint>("Character", "Class", client.Player.Class);
            write.Write<uint>("Character", "Map", client.Player.Map);
            write.Write<ushort>("Character", "X", client.Player.X);
            write.Write<ushort>("Character", "Y", client.Player.Y);

            client.ArenaStatistic = new Game.MsgTournaments.MsgArena.User();
            client.ArenaStatistic.ApplayInfo(client.Player);
            client.ArenaStatistic.Info.ArenaPoints = 1000;
            Game.MsgTournaments.MsgArena.ArenaPoll.TryAdd(client.Player.UID, client.ArenaStatistic);

            client.Player.Nobility = new Role.Instance.Nobility(client);

          

            client.Player.Associate = new Role.Instance.Associate.MyAsociats(client.Player.UID);
            client.Player.Associate.MyClient = client;
            client.Player.Associate.Online = true;


            client.Player.Flowers = new Role.Instance.Flowers(client.Player.UID, client.Player.Name);
            client.Player.SubClass = new Role.Instance.SubClass();
            client.Player.MyChi = new Role.Instance.Chi(client.Player.UID);
            client.Achievement = new AchievementCollection();

            client.FullLoading = true;

        }

        public static bool AllowCreate(uint UID)
        {
            return !File.Exists(Program.ServerConfig.DbLocation + "\\Users\\" + UID + ".ini");
            // WindowsAPI.IniFile reader = new WindowsAPI.IniFile("\\Accounts\\" + Account + ".ini");
            // return reader.ReadUInt32("Account", "UID", 1000000) == 1000000;
        }
        public static void UpdateGuildMember(Role.Instance.Guild.Member Member)
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + Member.UID + ".ini");
            //  write.Write<uint>("Character", "GuildID", 0);
            write.Write<ushort>("Character", "GuildRank", 0);
        }
        public static void UpdateGuildMember(Role.Instance.Guild.UpdateDB Member)
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + Member.UID + ".ini");
            write.Write<ushort>("Character", "GuildRank", 0);
            write.Write<ushort>("Character", "GuildID", 0);
        }
        public static void UpdateUnionMember(Role.Instance.Union.Member Member)
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + Member.UID + ".ini");
            if (Member.ReceiveKick == 0)
            {

                write.Write<uint>("Character", "UnionUID", Member.UID);
                write.Write<uint>("Character", "UnionRank", (uint)Member.Rank);
                write.Write<uint>("Character", "UnionExploits", Member.Exploits);
                write.Write<uint>("Character", "Treasury", Member.MyTreasury);
            }
            else
            {
                write.Write<uint>("Character", "UnionUID", 0);
                write.Write<uint>("Character", "UnionRank", 0);
                write.Write<uint>("Character", "UnionExploits", 0);
                write.Write<uint>("Character", "UnionGoldBrick", 0);
            }
        }
        public static void UpdateMapRace(Role.GameMap map)
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\maps\\" + map.ID + ".ini");
            write.Write<uint>("info", "race_record", map.RecordSteedRace);
        }
        public static void UpdateClanMember(Role.Instance.Clan.Member Member)
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + Member.UID + ".ini");
            write.Write<uint>("Character", "ClanID", 0);
            write.Write<ushort>("Character", "ClanRank", 0);
            write.Write<uint>("Character", "ClanDonation", 0);
        }
        public static void DestroySpouse(Client.GameClient client)
        {
            if (client.Player.SpouseUID != 0)
            {
                WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + client.Player.SpouseUID + ".ini");
                write.Write<uint>("Character", "SpouseUID", 0);
                write.WriteString("Character", "Spouse", "None");

                client.Player.SpouseUID = 0;
            }

            client.Player.Spouse = "None";
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Player.SendString(stream, Game.MsgServer.MsgStringPacket.StringID.Spouse, false, new string[1] { "None" });
            }
        }
        public static string GenerateDate()
        {
            DateTime now = DateTime.Now;
            return now.Year.ToString() + "and" + now.Month.ToString() + "and" + now.Day.ToString() + " and " + now.Hour.ToString() + " and " + now.Minute.ToString() + " and " + now.Second.ToString();
        }
        public static void UpdateSpouse(Client.GameClient client)
        {
            if (client.Player.SpouseUID != 0)
            {
                WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + client.Player.SpouseUID + ".ini");
                write.WriteString("Character", "Spouse", client.Player.Name);
            }
        }

        public static object SavingObj = new object();
        public static ExecuteLogin LoginQueue = new ExecuteLogin();

        public class ExecuteLogin : ConcurrentSmartThreadQueue<object>
        {
            public object SynRoot = new object();
            public ExecuteLogin()
                : base(5)
            {
                Start(50);
            }
            public void TryEnqueue(object obj)
            {
                {

                    base.Enqueue(obj);
                }
            }
            #region Test
            private static string SafeDate()
            {
                return DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }

            private static void EnsureDir(string path)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            private static void BackupFile(string source, string backupDir, string backupName)
            {
                if (!File.Exists(source))
                    return;

                EnsureDir(backupDir);
                File.Copy(source, Path.Combine(backupDir, backupName), true);
            }

            #endregion
            protected unsafe override void OnDequeue(object obj, int time)
            {
                try
                {
                    if (obj is string)
                    {

                    }
                    else if (obj is Role.GameMap)
                    {
                        UpdateMapRace(obj as Role.GameMap);
                    }
                    else if (obj is Role.Instance.Guild.Member)
                    {
                        UpdateGuildMember(obj as Role.Instance.Guild.Member);
                    }
                    else if (obj is Role.Instance.Guild.UpdateDB)
                    {
                        UpdateGuildMember(obj as Role.Instance.Guild.UpdateDB);
                    }
                    else if (obj is Role.Instance.Union.Member)
                    {
                        UpdateUnionMember(obj as Role.Instance.Union.Member);
                    }
                    else if (obj is Role.Instance.Clan.Member)
                    {
                        UpdateClanMember(obj as Role.Instance.Clan.Member);
                    }
                    else
                    {
                        Client.GameClient client = obj as Client.GameClient;

                        if (client.Player.Delete)
                        {
                            client.IsDeleting = true;
                            client.ClientFlag &= ~Client.ServerFlag.QueuesSave;

                            if (client.Map != null)
                                client.Map.View.LeaveMap<Role.Player>(client.Player);

                            string date = SafeDate();
                            string baseDb = Program.ServerConfig.DbLocation;

                            MyConsole.WriteLine("Client " + client.Player.Name + " delete he account.", ConsoleColor.Magenta);

                            // ===== USERS =====
                            BackupFile(
                                baseDb + "\\Users\\" + client.Player.UID + ".ini",
                                baseDb + "\\BackUp\\Users\\",
                                client.Player.UID + "_date_" + date + ".ini"
                            );

                            // ===== SPELLS =====
                            BackupFile(
                                baseDb + "\\PlayersSpells\\" + client.Player.UID + ".bin",
                                baseDb + "\\BackUp\\PlayersSpells\\",
                                client.Player.UID + "_date_" + date + ".bin"
                            );

                            // ===== PROFS =====
                            BackupFile(
                                baseDb + "\\PlayersProfs\\" + client.Player.UID + ".bin",
                                baseDb + "\\BackUp\\PlayersProfs\\",
                                client.Player.UID + "_date_" + date + ".bin"
                            );

                            // ===== ITEMS =====
                            BackupFile(
                                baseDb + "\\PlayersItems\\" + client.Player.UID + ".bin",
                                baseDb + "\\BackUp\\PlayersItems\\",
                                client.Player.UID + "_date_" + date + ".bin"
                            );

                            // ===== DELETE ORIGINAL FILES =====
                            string[] deleteFiles =
    {
        baseDb + "\\Users\\" + client.Player.UID + ".ini",
        baseDb + "\\PlayersSpells\\" + client.Player.UID + ".bin",
        baseDb + "\\PlayersProfs\\" + client.Player.UID + ".bin",
        baseDb + "\\PlayersItems\\" + client.Player.UID + ".bin",
        baseDb + "\\Quests\\" + client.Player.UID + ".bin",
        baseDb + "\\Houses\\" + client.Player.UID + ".bin"
    };

                            foreach (var file in deleteFiles)
                                if (File.Exists(file))
                                    File.Delete(file);

                            // ===== HOUSE =====
                            Role.Instance.House house;
                            if (client.MyHouse != null && Role.Instance.House.HousePoll.ContainsKey(client.Player.UID))
                                Role.Instance.House.HousePoll.TryRemove(client.Player.UID, out house);

                            // ===== CHI =====
                            Role.Instance.Chi chi;
                            if (Role.Instance.Chi.ChiPool.TryRemove(client.Player.UID, out chi))
                            {
                                EnsureDir(baseDb + "\\BackUp\\");
                                WindowsAPI.IniFile write = new WindowsAPI.IniFile(baseDb + "\\BackUp\\ChiInfo.txt");

                                string sec = client.Player.UID + "_date_" + date;
                                write.WriteString(sec, "Dragon", chi.Dragon.ToString());
                                write.WriteString(sec, "Phoenix", chi.Phoenix.ToString());
                                write.WriteString(sec, "Turtle", chi.Turtle.ToString());
                                write.WriteString(sec, "Tiger", chi.Tiger.ToString());
                            }

                            // ===== INNER POWER =====
                            Role.Instance.InnerPower inner;
                            if (Role.Instance.InnerPower.InnerPowerPolle.TryRemove(client.Player.UID, out inner))
                            {
                                WindowsAPI.IniFile write = new WindowsAPI.IniFile(baseDb + "\\BackUp\\InnerPower.txt");
                                write.WriteString(client.Player.UID + "_date_" + date, "InnerPower", inner.ToString());
                            }

                            // ===== JIANG HU =====
                            Role.Instance.JiangHu jiang;
                            if (Role.Instance.JiangHu.Poll.TryRemove(client.Player.UID, out jiang))
                            {
                                WindowsAPI.IniFile write = new WindowsAPI.IniFile(baseDb + "\\BackUp\\JiangHuInfo.txt");
                                write.WriteString(client.Player.UID + "_date_" + date, "JiangHu", jiang.ToString());
                            }

                            // ===== FLOWERS =====
                            Role.Instance.Flowers flow;
                            Role.Instance.Flowers.ClientPoll.TryRemove(client.Player.UID, out flow);

                            // ===== NOBILITY =====
                            Pool.NobilityRanking.RemoveAndUpdateTheRest(client.Player.UID);

                            // ===== ASSOCIATES =====
                            Role.Instance.Associate.MyAsociats assoc;
                            Role.Instance.Associate.Associates.TryRemove(client.Player.UID, out assoc);

                            // ===== RESET RANKINGS =====
                            client.HundredWeapons.Unlocked = false;
                            client.HundredWeapons.UpdateRank();

                            client.MyNinja.Unlocked = false;
                            client.MyNinja.UpdateRank();

                            client.MyArchives.UpdateRank();
                            client.MyAstredge.UpdateRank();

                            // ===== REMOVE CLIENT =====
                            Client.GameClient user;
                            Pool.GamePoll.TryRemove(client.Player.UID, out user);

                            if (user != null && Pool.NameUsed.Contains(user.Player.Name.GetHashCode()))
                            {
                                lock (Pool.NameUsed)
                                    Pool.NameUsed.Remove(user.Player.Name.GetHashCode());
                            }

                            client.Player.Delete = true;
                            return;
                        }


                        if ((client.ClientFlag & Client.ServerFlag.RemoveSpouse) == Client.ServerFlag.RemoveSpouse)
                        {
                            DestroySpouse(client);
                            client.ClientFlag &= ~Client.ServerFlag.RemoveSpouse;
                            return;
                        }
                        if ((client.ClientFlag & Client.ServerFlag.UpdateSpouse) == Client.ServerFlag.UpdateSpouse)
                        {
                            UpdateSpouse(client);
                            client.ClientFlag &= ~Client.ServerFlag.UpdateSpouse;
                            return;
                        }
                        if ((client.ClientFlag & Client.ServerFlag.SetLocation) != Client.ServerFlag.SetLocation && (client.ClientFlag & Client.ServerFlag.OnLoggion) == Client.ServerFlag.OnLoggion)
                        {
                            Game.MsgServer.MsgLoginClient.LoginHandler(client, client.OnLogin);
                        }
                        else if ((client.ClientFlag & Client.ServerFlag.QueuesSave) == Client.ServerFlag.QueuesSave)
                        {
                            if (client.Player.OnTransform)
                            {
                                client.Player.HitPoints = Math.Min(client.Player.HitPoints, (int)client.Status.MaxHitpoints);

                            }
                            if (client.IsDeleting)
                                return;

                            SaveClient(client);

                        }
                    }
                }
                catch (Exception e) { MyConsole.SaveException(e); }
            }
        }

    }
}