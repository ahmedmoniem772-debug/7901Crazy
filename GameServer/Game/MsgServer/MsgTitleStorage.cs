using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ConquerOnline.Game.MsgServer
{
   public static class MsgTitleStorage
    {
       public enum TitleType : uint
       {
           #region Title
           Overlord = 1,
           ChosenOne = 9,
           RisingStar = 2001,
           Victor = 2002,
           Conqueror = 2003,
           Talent = 2004,
           Fashionist = 2005,
           SwiftChaser = 2006,
           MonkeyRider = 2013,
           SolarRider = 2014,
           LunarRider = 2015,
           SaintRider = 2016,
           Grandmaster = 2018,
           Fairy = 2020,
           Goddess = 2021,
           Beauty = 2022,
           Scholarz = 2023,
           Handsome = 2024,
           Wise = 2025,
           Superman = 2026,
           Scholar = 2027,
           EarthKnight = 2028,
           GloryKnight = 2029,
           SkyKnight = 2030,
           Paladin = 2031,
           BigFan = 2032,
           EuroCollector = 2033,
           Invincible = 2034,
           Legendary = 2035,
           Peerless = 2036,
           Outstanding = 2037,
           Expert = 2038,
           PeerlessBeauty = 2040,
           SuperIdol = 2041,
           MostRadiant = 2044,
           MostAttractive = 2045,
           PokerKing = 2046,
           PokerLord = 2047,
           PokerMaster = 2048,
           PokerStar = 2049,
           ImmortalFate = 2050,
           SuccessortoLege = 2051,
           FashionIcon = 2052,
           FashionIdol = 2053,
           FashionMaster = 2054,
           SlamDunkLeade = 2056,
           HonoredHero = 2057,
           KingTeam = 2059,
           DominatorTeam = 2060,
           PowerTeam = 2061,
           EliteTeam = 2062,
           LoveJoker = 2063,
           SweetFairy = 2064,
           LovesMadness = 2065,
           MirrorMaster = 2077,
           TripodMaster = 2078,
           BeadsMaster = 2079,
           SealMaster = 2080,
           BellMaster = 2081,
           Professional = 2082,
           EliteShooter = 2083,
           KingofFootball = 2084,
           HansomeBoy = 2085,
           AttractiveGentleman = 2086,
           BeautifulLady = 2087,
           BrightestKing = 2088,
           CharmingPrince = 2089,
           BrightestQueen = 2090,
           CharmingPrincess = 2091,
           FairyGirl = 2092,
           Sweetness = 2100,
           Handsomeness = 2101,
           SuperBeauty = 2102,
           SuperBrilliance = 2103,
           MasterofJustice = 2123,
           HeroicPride = 2124,
           TopMaster = 2125,
           WorldExplorer = 2126,
           Trainee = 2127,
           ExpertNew = 2128,
           Elite = 2129,
           Master = 2130,
           GrandmasterNew = 2131,
           LegendaryNew = 2132,
           NoblePionner = 2133,
           Tycoon = 2134,
           Magnifico = 2135,
           Millionaire = 2136,
           LuckyGirl = 2138,
           BrightBoy = 2139,
           StunningBeauty = 2140,
           BrilliantMan = 2141,
           WhyAmISoBeautiful = 2142,
           WhyAmISoHandsome = 2143,
           OverlordofConquer = 2145,
           WorldDiceMaster = 2148,
           DiceChampionChina = 2149,
           DiceChampionEnglish = 2150,
           ReyEspaoldeDados = 2151,
           Unk = 2152,
           NewRuler = 2158,
           TurkeyTerminator = 2162,
           Unstoppable = 2163,
           WidelyKnown = 2164,
           SacredDragon = 2165,
           SacredPhoenix = 2166,
           SacredTiger = 2167,
           SacredTortoise = 2168,
           Matchless = 2169,
           DivineSage = 2170,
           ForeverBroke = 2174,
           ToxicKingKirby = 2175,
           RatTitle = 2176,
           FairLady = 2179,
           FineGentleman = 2180,
           LoveIgniter = 2181,
           Heartthrob = 2182,
           HeartStealer = 2183,
           UniqueBeauty = 2184,
           FoolFun = 2185,
           CapsuleMaster = 2188,
           SupremeGoldVIP = 2189,
           SupremePlatinumVIP = 2190,
           SupremeDiamondVIP = 2191,
           ExclusiveLove = 2194,
           ImpressiveCharm = 2196,
           EnduringLove = 2195,
           SereneGrace = 2197,
           FreeAsWind = 2198,
           CharmingAsSunglow = 2199,
           DragonLord = 2200,
           DragonSlayer = 2201,
           CyberPrivilege = 2203,
           IllusionMaster = 2204,
           SuperbWarrior = 2206,
           OxFortune = 2210,
           ThornFighter = 2211,
           RoseMaiden = 2212,
           KamenRider = 2213,
           FancyPrincess = 2214,
           DarknightDuke = 2215,
           PerfectPeak = 2216,
           AzureQueen = 2217,
           SupremeTycoon = 2218,
           ClownMe = 2220,
           AnniversaryTitle = 2221,
           SuperiorClan = 2224,
           CuteHero = 2225,
           CuteConqueror = 2226,
           SilverDistributor = 2227,
           BraveExplorer = 2232,
           Adventurer = 2233,
           PromiseofSky = 2235,
           LoveofSky = 2236,
           AquilaSong = 2237,
           LyraMelody = 2238,
           OdeofStar = 2239,
           VowofStar = 2241,
           BrightHop = 2246,
           DuskyGlory = 2247,
           ThanksgivingGourmet = 2252,
           TigerMinded = 2254,
           TalentedMonopoly = 2255,
           BeaPear = 2264,
           Pearoff = 2265,
           LikeYouBerryMuch = 2266,
           MyBerryGirl = 2267,
           SweetGrapefruit = 2268,
           SweetFool = 2269,
           EarthConqueror = 2270,
           BloomofYouth = 2273,
           #endregion
          
          
       }
       public enum WingsType : uint
       {
           #region Wings
           WingsofSolarDra = 4001,
           GlitterFeather = 6000,
           WingsofInfernal = 6001,
           RadiantWings = 6002,
           StarlightWings = 6003,
           MoonlightWings = 6004,
           FairyWings = 6005,
           VioletCloudWing = 6007,
           VioletLightning = 6008,
           WingsofPlanet = 6009,
           Supreme = 6011,
           WingsofRomance = 6012,
           EmeraldGlowWing = 6013,
           OrangeGlowWings = 6014,
           FlameGlowWings = 6015,
           BrightGlowWings = 6016,
           SirenSong = 6017,
           WingsofHeart = 6020,
           ButterflyWing = 6021,
           LoveWings = 6022,
           VioletWings = 6023,
           FragrantWings = 6025,
           WingsofSlaughter = 6030,
           CelestialWings = 6031,
           HollyBlueWings = 6032,
           SpringFestivalWings = 6033,
           NewYearWings = 6034,
           LastingLove = 6036,
           DestinedLove = 6037,
           FlutteringFlag2 = 6038,
           FlutteringFlag = 6039,
           FrostPhoenixWings = 6040,
           MegaPhoenixWings = 6041,
           FloralWings = 6043,
           SilverButterfly = 6044,
           TipsyDreamGold = 6045,
           TipsyDream = 6046,
           SpotlessFeather = 6047,
           ShimmeringStarBrilliant = 6048,
           ShimmeringStar = 6049,
           HolyWings = 6051,
           FrostyHeartPlume = 6052,
           FrostyPlume = 6053,
           FrostyBlossomPlume = 6054,
           FlamePapilio = 6056,
           FlamePapilioGolden = 6058,
           LoveSoundHeaven = 6073,
           LoveSound = 6074,
           #endregion
       }
       public enum HaloType : uint
       { 
           #region Halo
           WeeklyPKChampion = 8002,
           MostValuableNewcomer = 8004,
           Warrior = 8005,
           Trojan = 8006,
           Archer = 8007,
           WaterTaoist = 8008,
           FireTaoist = 8009,
           Ninja = 8010,
           Monk = 8011,
           Pirate = 8012,
           DragonWarrior = 8013,
           Windwalker = 8014,
           Thunderstriker = 8015,
           MrUniverseChampion = 8016,
           MrUniverseRunnerup = 8017,
           MrUniverse2ndRunnerUp = 8018,
           MsUniverseChampion1 = 8019,
           MsUniverseRunnerup2 = 8020,
           MsUniverse2ndRunnerup = 8021,
           StrongestClan = 8022,
           GlobalLoveStar = 8023,
           GlobalLoveNumen = 8024,
           GlobalLoveAngel = 8025,
           GiftedBeauty = 8026,
           GiftedScholar = 8027,
           WorldsBestGuildLeader = 8028,
           WorldsBestDeputyLeader = 8029,
           CSPKL120GroupChampion = 8030,
           CSPKL120GroupRunnerup = 8031,
           CSPKL120Group2ndRunnerup = 8032,
           CSPKL120GroupTop8 = 8033,
           CSPKL130GroupChampion = 8034,
           CSPKL130GroupRunnerup  = 8035,
           CSPKL130Group2ndRunnerup = 8036,
           CSPKL130GroupTop8 = 8037,

           FirstLoveHalo = 8038,
           GalaxySkyHalo = 8039,
           ArrowofLoveHalo = 8040,
           PhoenixFlyHalo = 8041,
           CSTeamPKChampionHalo = 8043,
           Cillionaire = 8044,
           #endregion
       }
       [ProtoContract]
       public class TitleStorage
       {
           [ProtoMember(1, IsRequired = true)]
           public Action ActionID;
           [ProtoMember(2, IsRequired = true)]
           public uint dwparam1;
           [ProtoMember(3, IsRequired = true)]
           public uint dwparam2;
           [ProtoMember(4, IsRequired = true)]
           public uint dwparam3;
           [ProtoMember(5, IsRequired = true)]
           public Title Title;
       }
       [ProtoContract]
       public class Title
       {
           [ProtoMember(1, IsRequired = true)]
           public uint ID;
           [ProtoMember(2, IsRequired = true)]
           public uint SubId;
           [ProtoMember(3, IsRequired = true)]
           public uint dwparam1;
           [ProtoMember(4, IsRequired = true)]
           public uint dwparam2;
       }
       [Flags]
       public enum Action : uint
       {
           UpdateScore = 0,
           UseTitle =1,
           RemoveTitle = 3,
           Equip = 4,
           UnEquip = 5,
           FullLoad = 6
       }
       public static unsafe ServerSockets.Packet CreateTitleStorage(this ServerSockets.Packet stream, TitleStorage obj)
       {
           stream.InitWriter();
           stream.ProtoBufferSerialize(obj);
           stream.Finalize(GamePackets.MsgTitleStorage);
           return stream;
       }
       public static unsafe void GetTitleStorage(this ServerSockets.Packet stream, out TitleStorage pQuery)
       {
           pQuery = new TitleStorage();
           pQuery = stream.ProtoBufferDeserialize<TitleStorage>(pQuery);
       }
       [PacketAttribute(GamePackets.MsgTitleStorage)]
       private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
       {
           TitleStorage pQuery;
           stream.GetTitleStorage(out pQuery);
         
           switch (pQuery.ActionID)
           {
               case Action.Equip:
                   {
                       if (client.Player.SpecialTitles.Contains((TitleType)pQuery.dwparam2))
                       {
                           Database.TitleStorage dbtitle;
                           if (Database.TitleStorage.Titles.TryGetValue(pQuery.dwparam2, out dbtitle))
                           {
                               client.Player.SpecialTitleScore = dbtitle.Score;
                               client.Player.SpecialTitleID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);
                               client.Send(stream.CreateTitleStorage(pQuery));
                               pQuery.ActionID = Action.UseTitle;
                               pQuery.Title = new Title();
                               pQuery.Title.ID = pQuery.dwparam2;
                               pQuery.Title.SubId = pQuery.dwparam3;
                               if (dbtitle.ID >= 6058)
                                   pQuery.Title.dwparam1 = 1;
                               else
                                   pQuery.Title.dwparam1 = dbtitle.Score;
                               client.Send(stream.CreateTitleStorage(pQuery));
                               client.Player.View.SendView(client.Player.GetArray(stream, false),false);
                           }
                       }
                       else if (client.Player.WingsTitles.Contains((WingsType)pQuery.dwparam2))
                       {
                           Database.TitleStorage dbtitle;
                           if (Database.TitleStorage.Titles.TryGetValue(pQuery.dwparam2, out dbtitle))
                           {
                               client.Player.SpecialWingID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);
                               client.Send(stream.CreateTitleStorage(pQuery));
                               pQuery.ActionID = Action.UseTitle;
                               pQuery.Title = new Title();
                               pQuery.Title.ID = pQuery.dwparam2;
                               pQuery.Title.SubId = pQuery.dwparam3;
                               pQuery.Title.dwparam1 = 1;
                               client.Send(stream.CreateTitleStorage(pQuery));
                               client.Player.View.SendView(client.Player.GetArray(stream, false), false);
                           }
                       }
                       else if (client.Player.HaloTitles.Contains((HaloType)pQuery.dwparam2))
                       {
                           Database.TitleStorage dbtitle;
                           if (Database.TitleStorage.Titles.TryGetValue(pQuery.dwparam2, out dbtitle))
                           {

                               client.Player.SpecialTitleScore = dbtitle.Score;
                               client.Player.SpecialHaloID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);

                               client.Send(stream.CreateTitleStorage(pQuery));
                               pQuery.ActionID = Action.UseTitle;
                               pQuery.Title = new Title();
                               pQuery.Title.ID = pQuery.dwparam2;
                               pQuery.Title.SubId = pQuery.dwparam3;
                               pQuery.Title.dwparam1 = 1;
                               client.Send(stream.CreateTitleStorage(pQuery));
                               client.Player.View.SendView(client.Player.GetArray(stream, false), false);
                           }

                       }
                      
                       break;
                   }
               case Action.UnEquip:
                   {
                       if (client.Player.SpecialTitles.Contains((TitleType)pQuery.dwparam2))
                       {
                           Database.TitleStorage dbtitle;
                           if (Database.TitleStorage.Titles.TryGetValue(pQuery.dwparam2, out dbtitle))
                           {
                               client.Player.SpecialTitleScore = client.Player.SpecialTitleID = 0;
                               client.Send(stream.CreateTitleStorage(pQuery));
                               client.Player.View.Clear(stream);
                               client.Player.View.Role(false);
                               client.Player.View.SendView(client.Player.GetArray(stream, false), false);
                           }
                       }
                       else if (client.Player.WingsTitles.Contains((WingsType)pQuery.dwparam2))
                       {
                           Database.TitleStorage dbtitle;
                           if (Database.TitleStorage.Titles.TryGetValue(pQuery.dwparam2, out dbtitle))
                           {

                               client.Player.SpecialWingID = 0;
                               client.Send(stream.CreateTitleStorage(pQuery));
                               client.Player.View.Clear(stream);
                               client.Player.View.Role(false);
                               client.Player.View.SendView(client.Player.GetArray(stream, false), false);
                           }
                       }
                       else if (client.Player.HaloTitles.Contains((HaloType)pQuery.dwparam2))
                       {
                           Database.TitleStorage dbtitle;
                           if (Database.TitleStorage.Titles.TryGetValue(pQuery.dwparam2, out dbtitle))
                           {
                               client.Player.SpecialTitleScore = client.Player.SpecialHaloID = 0;
                               client.Send(stream.CreateTitleStorage(pQuery));
                               client.Player.View.Clear(stream);
                               client.Player.View.Role(false);
                               client.Player.View.SendView(client.Player.GetArray(stream, false), false);
                           }
                       }

                       break;
                   }
           }
       }

    }
}
