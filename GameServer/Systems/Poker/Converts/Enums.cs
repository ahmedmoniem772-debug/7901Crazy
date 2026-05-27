using System;
using System.Runtime.InteropServices;

namespace GameServer.Role
{
    public class Enums
    {
        public enum MsgTypesNew : ushort
        {
            MsgTexasNpcInfo = 2458,
            MsgAction = 2099,
            MsgTexasInteractive = 2182,
            MsgShowHandEnter = 2262,
            MsgShowHandExit = 2424,
            MsgShowHandDealtCard = 2414,
            MsgShowHandCallAction = 2040,
            MsgShowHandActivePlayer = 2217,
            MsgShowHandLostInfo = 2359,
            MsgShowHandKick = 2169,
            MsgShowHandGameResult = 2256,
            MsgShowHandLayCard = 2080,
            MsgShowHandTrusteeship = 2172,
            MsgTexasPersonalInfo = 2082,
            MsgTexasExMatchFieldList = 2216,
            MsgTexasExInteractive = 2243
        }
        public enum CardsType : byte
        {
            Heart = 0,
            Spade = 1,
            Club = 2,
            Diamond = 3
        }

        public enum PlayerType : byte
        {
            Player = 1,
            Watcher = 2
        }

        public enum TableInteractiveType : byte
        {
            Join = 0,
            Leave = 1,
            Watch = 4
        }

        public enum TableState : byte
        {
            Unopened = 0,
            Pocket = 1,
            Flop = 2,
            Turn = 3,
            River = 4,
            ShowDown = 5
        }

        public enum TableType : byte
        {
            TexasHoldem = 1,
            ShowHand = 2
        }

        public enum TableUpdate : byte
        {
            Statue = 233,
            Chips = 234,
            PlayerCount = 235
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ActivePlayerTypes
        {
            public static byte Bet = 1;

            public static byte Call = 2;

            public static byte Fold = 4;

            public static byte Check = 8;

            public static byte Raise = 16;

            public static byte Allin = 32;
        }
    }
}