using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Poker
{
    public class General
    {
        public enum CardsType : byte
        {
            Heart,
            Spade,
            Club,
            Diamond
        }
        public enum PlayerType : byte
        {
            Player = 1,
            CrossPoker = 3,
            Watcher
        }

        public enum TableInteractiveType : byte
        {
            Join = 0,
            Leave = 1,
            Watch = 4
        }

        public enum TableState : byte
        {
            Unopened,
            Pocket,
            Flop,
            Turn,
            River,
            ShowDown
        }

        public enum TableType : byte
        {
            TexasHoldem = 1,
            ShowHand = 2,
            Domino

        }

        public enum TableUpdate : byte
        {
            Statue = 233,
            Chips,
            PlayerCount
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
