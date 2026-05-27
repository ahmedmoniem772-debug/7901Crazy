using VirusX.Database;
using VirusX.DBFunctionality;
using VirusX.Game.MsgMonster;
using VirusX.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VirusX
{
    public class ThreadPlayer
    {
        public const int

            AI_Buffers = 500,

            AI_Guards = 700,

            AI_Monsters = 700,

            User_Stamina = 1000,

            User_Buffers = 700,

            User_StampXPCount = 1000,

            User_AutoAttack = 500,

            User_CheckSecounds = 1000,

            User_CheckItemsTime = 1000,

            User_SaveMele = 1000,

            User_FloorSpell = 100,

            User_ItemTIme = 1000,

           User_GenertMap = 1000;
    }
    public class ThreadFunctions
    {
        public const int TournamentsStamp = 1000,
            RouletteStamp = 1000,
            TeamArena_CreateMatches = 900,
            TeamArena_VerifyMatches = 980,
            TeamArena_CheckGroups = 960,
            Arena_CreateMatches = 1100,
            Arena_VerifyMatches = 1200,
            Arena_CheckGroups = 1150,
            TeamPkStamp = 1000,
            ElitePkStamp = 1000,
            BroadCastStamp = 1000,
            ResetDayStamp = 6000;
    }
}
