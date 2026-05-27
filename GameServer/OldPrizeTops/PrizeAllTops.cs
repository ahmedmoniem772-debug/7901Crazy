using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Game.MsgServer;
using VirusX.MsgInterServer.Packets;
using VirusX.Game.MsgFloorItem;
using VirusX.DBFunctionality;
using VirusX.Game.MsgTournaments;
using System.IO;
using System.Net.Mail;

namespace NewSystem
{

    class PrizeAllTops
    {
        public static int
        ElitePk1St303 = 250,
        ElitePk2St303 = 250,
        ElitePk3St303 = 250,
        ElitePk8St303 = 250,
        ElitePk1St376 = 250,
        ElitePk2St376 = 250,
        ElitePk3St376 = 250,
        ElitePk8St376 = 250,
        ElitePk1St300 = 250,
        ElitePk2St300 = 250,
        ElitePk3St300 = 250,
        ElitePk8St300 = 250,


        #region Prize Skill Team + Team PK Level 130
        SkillTeamPk1St = 200000, // 200k CPs
        SkillTeamPk2nd = 100000, // 100k CPs
        SkillTeamPk3rd = 50000, // 50k CPs
        SkillTeamPk8th = 30000, // 30k CPs

        TeamPk1St = 200000, // 200k CPs
        TeamPk2nd = 100000, // 100k CPs
        TeamPk3rd = 50000, // 50k CPs
        TeamPk8th = 30000, // 30k CPs
        #endregion

        #region Prize Elite PK Level 130
        ElitePk1St = 200000, // 200k CPs
        ElitePk2St = 100000, // 100k CPs
        ElitePk3St = 50000, // 50k CPs
        ElitePk8St = 30000, // 30k CPs
        #endregion

        #region Class PK War Prize
        ClassPkLevel_1_99 = 1000000, // 1m CPs
        ClassPkLevel_100_119 = 1000000, // 1m CPs
        ClassPkLevel_120_130 = 1000000, // 1m CPs
        ClassPkLevel_130_140 = 1000000; // 1m CPs
        #endregion


        public static uint
             AllEvent = 5000, // 5.000 CPs

        GuildWarCPS = 1200000,
        SuperGuildWarCPS = 1000000,
        #region Pole 
        WarOfPlayer = 15000, // Prize = 500.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        WarFighters = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        Emperrors = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        Emperrorsmo = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        GuildWarScore = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        ChampionWar = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        EliteGuildWar = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        EliteGuildWarmo = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        Guild6PoleWar = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        Guild6PoleWarmo = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        UnionWar = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        MsgUnionWarmo = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        GuildWarScoremo = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        ChampionWarmo = 15000, // Prize = 1.000.000 CPs - الجايزة المحطوطة 1 مليون سيبي اس
        #endregion
        ClanWar = 500000,
        ClassPK = 30000,
        FreezeWar = 30000,
        SkillTournament = 30000,
        PkWar = 30000,
        LastManStand = 30000,
        KingOfTheHill = 30000,
        KillerOfElite = 30000,
            ////////////////////
            //////TopFight/////
        King = 250000,
        Prince = 200000,
        Duke = 150000,
        Earl = 100000,

        GuildPoleGl = 500000,
        GuildPoleDL = 300000,
        GuildPoleM = 200000,
        NobilityKing = 500000,
        NobilityPrince = 300000,
        NobilityDuke = 200000,
        NobilityEarl = 100000,
        NobilitySerf = 50000;        

    }
    class DropBigMonster
    {
        public static uint

        Rank1st = 100000,
        Rank2st = 75000,
        Rank3st = 50000;

    }
    
   
}
