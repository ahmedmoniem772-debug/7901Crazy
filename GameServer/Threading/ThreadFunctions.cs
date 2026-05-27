using ConquerOnline.Database;
using ConquerOnline.DBFunctionality;
using ConquerOnline.Game.MsgMonster;
using ConquerOnline.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConquerOnline
{
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
