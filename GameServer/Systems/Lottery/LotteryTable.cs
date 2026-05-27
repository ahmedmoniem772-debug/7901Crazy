
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public static class LotteryTable
    {
        public static ConcurrentDictionary<uint, Lottery> LotteryList;
        public class Lottery
        {
            public uint Bliss;
            public uint Id;
            public uint Awared;
            public uint u4;
            public string Name;
        }
        public static bool Load = false;
        public static uint[][][] LotteryPrize = new uint[25][][];
        public static double[] AwardRate(uint Id)
        {
            switch (Id)
            {
                case 0:
                    {
                        return new double[] { 0.08, 0.11, 0.12, 0.19, 0.45, 4.2, 41.9, 52.95, };
                    }
                case 1:
                    {
                        return new double[] { 0.13, 0.46, 0.81, 1.06, 3.35, 6.55, 42.33, 45.31, };
                    }
                case 2:
                    {
                        return new double[] { 0.08, 0.09, 0.2, 0.66, 4.35, 0.85, 40.71, 53.06, };
                    }
                case 3:
                    {
                        return new double[] { 0.1, 0.75, 1.12, 2.58, 7.74, 0.53, 42.06, 45.12, };
                    }
                case 4:
                    {
                        return new double[] { 0.1, 0.76, 0.93, 1.56, 2.38, 5.89, 41.45, 46.94, };
                    }
                case 10:
                    {
                        return new double[] { 0.31, 0.8, 1.55, 3.13, 7.8, 11.26, 28.15, 47, };
                    }

                case 19:
                    {
                        return new double[] { 0.38, 2.89 ,3.38 };
                    }
                case 20:
                    {
                        return new double[] { 0.38, 2.89  ,3.38 };
                    }
            }
            return new double[] { 0, 0, 0, 0, 0, 0, 0, 0, };
        }
        public static byte GetBliss(uint Id)
        {
            byte num = 0;
            while (true)
            {
                byte num2 = 0;
                foreach (var x in AwardRate(Id))
                {
                    double r = x * 100;
                    if (Role.Core.Rate((int)r, 10000))
                        return num2;
                    num2++;
                }
                num++;
                if (num >= 30) break;
            }
            return 7;
        }
        public static byte GetBlissMelet(uint Id)
        {
            byte num = 0;
            while (true)
            {
                byte num2 = 0;
                foreach (var x in AwardRate(Id))
                {
                    double r = x * 100;
                    if (Role.Core.Rate((int)r, 10000))
                        return num2;
                    num2++;
                }
                num++;
                if (num >= 30) break;
            }
            return 2;
        }
        public static uint MeltRewardCount(uint awared, uint Id)
        {
            if (Id >= 1 && Id <= 5)
            {
                switch (awared)
                {
                    case 3303166: return 2;
                    case 3303162: return 1;
                    case 3303173: return 2;
                    case 723713: return 6;
                    case 723859: return 2;
                    case 723856: return 2;
                    case 723855: return 2;
                    case 1060039: return 10;
                    case 3303164: return 10;
                    case 723714: return 16;
                    case 723716: return 18;
                    case 3303172: return 3;
                    case 3307146: return 10;
                    case 723725: return 3;
                    case 723727: return 3;
                    case 723715: return 24;
                    case 3303158: return 7;
                    case 3303163: return 10;
                    case 723712: return 2;
                    case 3000120: return 6;
                    case 3001411: return 2;
                    case 3303174: return 7;
                    case 3303154: return 13;
                    case 3000121: return 9;
                    case 3000123: return 21;
                    case 3303159: return 20;
                    case 3303175: return 20;
                    case 3001412: return 4;
                    case 3000122: return 15;
                    case 723717: return 100;
                    case 3001413: return 5;
                    case 3303168: return 100;
                    case 3303177: return 30;
                    case 3303155: return 24;
                    case 3200744: return 24;
                    case 753099: return 17;
                    case 752099: return 17;
                    case 751099: return 17;
                    case 3322219: return 200;
                    case 3001036: return 10;
                    case 3303160: return 60;
                    case 3303176: return 60;
                    case 3303180: return 87;
                    case 723718: return 400;
                    case 3303156: return 120;
                    case 723719: return 500;
                    case 3200739: return 167;
                    case 3303179: return 180;
                    case 3200740: return 250;
                    case 3303167: return 500;
                    case 3001035: return 50;
                    case 3200741: return 333;
                    case 3303178: return 132;
                    case 3303161: return 180;
                    case 3303157: return 360;
                    case 723720: return 1600;
                    case 3303165: return 540;
                    case 3303181: return 540;
                    case 753999: return 165;
                    case 752999: return 165;
                    case 751999: return 165;
                    case 723721: return 2000;
                    case 3001034: return 100;
                    case 3200728: return 833;
                    case 3303182: return 333;
                    case 3303204: return 1800;
                    case 3303183: return 972;
                    case 723722: return 6000;
                    case 3323507: return 333;
                    case 3200743: return 2500;
                    case 3303213: return 4000;
                    case 723723: return 10000;
                    case 3303184: return 1317;
                    case 3303170: return 2916;
                    case 711504: return 3;
                    case 3003132: return 1;
                    case 3003133: return 1;
                    case 3003140: return 2;
                    case 3003146: return 3;
                    case 3003134: return 3;
                    case 3003153: return 6;
                    case 3003147: return 7;
                    case 3003135: return 7;
                    case 3003136: return 13;
                    case 3003148: return 17;
                    case 3200731: return 3;
                    case 3200732: return 4;
                    case 3200733: return 5;
                    case 3200726: return 8;
                    case 3200737: return 42;
                    case 3200738: return 58;
                }
            }
            else if (Id == 11)
            {
                switch (awared)
                {
                    case 3331653: return 9;
                    case 3331670: return 12;
                    case 3330865: return 12;
                    case 3331667: return 7;
                    case 3303175: return 8;
                    case 723717: return 12;
                    case 3390011: return 8;
                    case 3314255: return 50;
                    case 3331654: return 23;
                    case 3325756: return 18;
                    case 3390017: return 33;
                    case 3322774: return 23;
                    case 3331788: return 23;
                    case 3303176: return 25;
                    case 3331655: return 70;
                    case 3331669: return 35;
                    case 3331664: return 37;
                    case 700123: return 53;
                    case 700103: return 53;
                    case 700073: return 53;
                    case 3390018: return 83;
                    case 3331789: return 70;
                    case 3314248: return 70;
                    case 3303161: return 110;
                    case 3331656: return 117;
                    case 3200727: return 70;
                    case 3303179: return 75;
                    case 3331659: return 82;
                    case 3331660: return 82;
                    case 3331661: return 82;
                    case 3318573: return 105;
                    case 3325980: return 180;
                    case 3390019: return 167;
                    case 3314242: return 100;
                    case 3314243: return 100;
                    case 3314244: return 100;
                    case 3331657: return 233;
                    case 3331662: return 210;
                    case 3331663: return 210;
                    case 3314250: return 1000;
                    case 3314251: return 333;
                    case 3330881: return 233;
                    case 3330882: return 233;
                    case 3303181: return 227;
                    case 3331658: return 467;
                    case 3318570: return 330;
                    case 3331665: return 350;
                    case 3318571: return 400;
                    case 3314246: return 2700;
                    case 3325989: return 1080;
                    case 3331790: return 350;
                    case 3314245: return 4500;
                    case 3325992: return 2160;
                    case 3318562: return 887;
                    case 3331666: return 700;
                    case 3318569: return 1000;
                    case 3303170: return 2041;
                    case 3318565: return 2041;
                    case 3318566: return 3000;
                    case 3318567: return 3750;
                    case 3318568: return 4667;
                    case 3326002: return 5400;
                    case 712019: return 30;
                }
            }
            return 1;
        }
        public static void GetReward(uint Id, out uint awared, out byte bliss, out uint Plus, out uint Bound, out byte SocketOne, out byte SocketTwo)
        {

            byte Bliss = 0;
            if (Id == 20 || Id == 21)
            {
                Bliss = GetBlissMelet(Id - 1);
            }
            else
            {
                Bliss = GetBliss(Id - 1);
            }
            var Prize = LotteryPrize[(uint)Id - 1][Bliss];
            bliss = Bliss;
            awared = Prize[Pool.GetRandom.Next(0, Prize.Length)];
            Plus = 0;
            Bound = 0;
            SocketOne = 0;
            SocketTwo = 0;
            switch (awared)
            {
                case 730001:
                case 730002:
                case 730003:
                case 730004:
                case 730005:
                case 730006:
                case 730007:
                case 730008:
                    {
                        Plus = awared % 10;
                        break;

                    }
            }
            if (Id == 20)
            {
                Bound = 1;
            }
            else
            {
                switch (awared)
                {
                    case 0:
                        {
                            Plus = 12;
                            Bound = 1;
                            SocketOne = 13;
                            SocketTwo = 13;
                            break;

                        }
                }
            }
        }
        public static void GenerateLotteryPrize()
        {
            if (Load)
                return;
            LotteryList = new ConcurrentDictionary<uint, Lottery>();
            Load = true;
            LotteryPrize[0] = new uint[8][];
            LotteryPrize[0][0] = new uint[] { 3303213, 3303184, 3303170 };
            LotteryPrize[0][1] = new uint[] { 3303204, 3303183, 3323507, 3200743, 3323507, 3323507, 3323507 };
            LotteryPrize[0][2] = new uint[] { 3303165, 3303181, 753999, 752999, 751999, 3001034, 3200728, 3303182 };
            LotteryPrize[0][3] = new uint[] { 3200741, 3303178, 3303161, 3303157 };
            LotteryPrize[0][4] = new uint[] { 3001036, 3303160, 3303176, 3303180, 3303156, 3200739, 3303179, 3200740, 3303167, 3001035 };
            LotteryPrize[0][5] = new uint[] { 3303174, 3303154, 3000121, 3000123, 3303159, 3303175, 3001412, 3000122, 723717, 3001413, 3303168, 3303177, 3303155, 3200744, 753099, 752099, 751099, 3322219 };
            LotteryPrize[0][6] = new uint[] { 723715, 3303158, 3303163, 723712, 3000120, 3001411 };
            LotteryPrize[0][7] = new uint[] { 3303166, 3303162, 3303173, 723713, 723859, 723856, 723855, 1060039, 3303164, 723714, 723716, 3303172, 3307146, 723725, 723727 };

            LotteryPrize[1] = new uint[8][];
            LotteryPrize[1][0] = new uint[] { 3303204, 723722, 3303213, 723723, 3303184, 3303170 };
            LotteryPrize[1][1] = new uint[] { 3303178, 723720, 753999, 752999, 751999, 723721, 3303182, 3323507 };
            LotteryPrize[1][2] = new uint[] { 723717, 753099, 752099, 751099, 3322219, 3303180, 723718, 723719, 3303167 };
            LotteryPrize[1][3] = new uint[] { 3000121, 3000123, 3000122, 3303168, 3303177 };
            LotteryPrize[1][4] = new uint[] { 723715, 3303163, 3000120, 3003136, 3003148, 3322219 };
            LotteryPrize[1][5] = new uint[] { 3001411, 3001412, 3001413, 3001036, 3001035, 3001034 };
            LotteryPrize[1][6] = new uint[] { 3303164, 723714, 3303172, 723716, 3003147, 3003135, 3307146, 723725, 723727 };
            LotteryPrize[1][7] = new uint[] { 3303166, 3003132, 3303162, 3003133, 3003140, 723713, 3003146, 3003134, 1060039, 3003153 };

            LotteryPrize[2] = new uint[8][];
            LotteryPrize[2][0] = new uint[] { 3303184, 3303170 };
            LotteryPrize[2][1] = new uint[] { 723722, 3303213, 723723, 3323507 };
            LotteryPrize[2][2] = new uint[] { 753999, 752999, 751999, 723721, 3303182, 3303204 };
            LotteryPrize[2][3] = new uint[] { 3303167, 3303178, 723720 };
            LotteryPrize[2][4] = new uint[] { 3000122, 3303168, 723717, 3303177, 753099, 752099, 751099, 3303180, 723718, 723719 };
            LotteryPrize[2][5] = new uint[] { 3303159, 3322219, 3303160, 3303161, 3303165 };
            LotteryPrize[2][6] = new uint[] { 723727, 3303158, 723715, 3303163, 3000120, 3003136, 3003148, 3000121, 3000123 };
            LotteryPrize[2][7] = new uint[] { 3303166, 3003132, 3303162, 3003133, 3003140, 723859, 723856, 723855, 723713, 3003146, 3003134, 1060039, 3003153, 3303164, 723714, 3303172, 723716, 3003147, 3003135, 3307146, 723725 };

            LotteryPrize[3] = new uint[8][];
            LotteryPrize[3][0] = new uint[] { 3303204, 723722, 3303213, 723723, 3303184, 3303170 };
            LotteryPrize[3][1] = new uint[] { 3303178, 723720, 753999, 752999, 751999, 723721, 3323507, 3303182, 3323507 };
            LotteryPrize[3][2] = new uint[] { 723717, 753099, 752099, 751099, 3322219, 3303180, 723718, 723719, 3303167 };
            LotteryPrize[3][3] = new uint[] { 3000121, 3000123, 3000122, 3303168, 3303177 };
            LotteryPrize[3][4] = new uint[] { 723725, 723727, 723715, 3303163, 3000120, 3003136, 3003148, 3322219 };
            LotteryPrize[3][5] = new uint[] { 3200731, 3200732, 3200733, 3200726, 3200737, 3200738, 3200739, 3200740, 3200741, 3200728, 3200743 };
            LotteryPrize[3][6] = new uint[] { 3303164, 723714, 3303172, 723716, 3003147, 3003135, 3307146 };
            LotteryPrize[3][7] = new uint[] { 3303166, 3003132, 3303162, 3003133, 3003140, 723713, 3003146, 3003134, 1060039, 3003153 };

            LotteryPrize[4] = new uint[8][];
            LotteryPrize[4][0] = new uint[] { 3303204, 723722, 3303213, 723723, 3303184, 3303170 };
            LotteryPrize[4][1] = new uint[] { 3303178, 723720, 753999, 752999, 751999, 723721, 3323507, 3303182, 3323507 };
            LotteryPrize[4][2] = new uint[] { 723717, 3322219, 3303180, 723718, 723719, 3303167 };
            LotteryPrize[4][3] = new uint[] { 3000121, 3000123, 3000122, 3303168, 3303177 };
            LotteryPrize[4][4] = new uint[] { 723715, 3303163, 3000120, 3003136, 3003148, 753099, 752099, 751099, 3322219 };
            LotteryPrize[4][5] = new uint[] { 3303173, 723712, 3303174, 3303154, 3303175, 3303155, 3200744, 3303176, 3303156, 3303179, 3303157, 3303181, 3303183 };
            LotteryPrize[4][6] = new uint[] { 3303164, 723714, 3303172, 723716, 3003147, 3003135, 3307146, 723725, 723727 };
            LotteryPrize[4][7] = new uint[] { 3303166, 3003132, 3303162, 3003133, 3003140, 723713, 3003146, 3003134, 1060039, 3003153 };

            LotteryPrize[10] = new uint[8][];
            LotteryPrize[10][0] = new uint[] { 3303170, 3318565, 3318566, 3318567, 3326002 };
            LotteryPrize[10][1] = new uint[] {  3325992, 3331666, 3318569 };
            LotteryPrize[10][2] = new uint[] { 3331658, 3318570, 3331665, 3318571, 3325989 };
            LotteryPrize[10][3] = new uint[] { 3331657, 3331662, 3331663, 3325989, 3330881, 3330882, 3303181 };
            LotteryPrize[10][4] = new uint[] { 3331656, 3200727, 3303179, 3331659, 3331660, 3331661, 3318573, 3325980, 3390019 };
            LotteryPrize[10][5] = new uint[] { 3331655, 3331669, 3331664, 700123, 700103, 700073, 3325980, 3390018, 3303179, 3303161 };
            LotteryPrize[10][6] = new uint[] { 3331654, 3325756, 3322774, 3303176 };
            LotteryPrize[10][7] = new uint[] { 3331653, 3331670, 3330865, 3331667, 3303175, 3325756 };


            LotteryPrize[19] = new uint[3][];
            LotteryPrize[19][0] = new uint[] { 3009003, 3329392, 730007, 730006, 3314208, 3314219, 730005, 3314218,3329302, 3329391, };

            LotteryPrize[19][1] = new uint[] { 3009002, 3009104, 3329390, 3329389, 3009103, 3314207, 730004, 3314217, 3009102, 730003, };

            LotteryPrize[19][2] = new uint[] { 3314216, 3303373, 3009101, 3321077, 3003126, 730002, 3314215, 3009001, 3002926, 3321076, 3009000, 730001, 3314214 };

            LotteryPrize[20] = new uint[3][];
            LotteryPrize[20][0] = new uint[] { 3329392 , 3325208 , 3009003 , 730007 , 3333970 , 3325994 , 3314220 , 3325992 , 730008 , 3314221 , 3325995 , 3314245 , 3326002 ,4300000 , 3326004 , 3326003 ,3329303 , 3326005 };
            LotteryPrize[20][1] = new uint[] { 3314218, 3329302, 3314195, 3329391, 3325993, 3314243, 730006, 3314208, 3314195, 3314219 };
            LotteryPrize[20][2] = new uint[] { 3314193, 3009001, 3002926, 3325756, 3009102, 730003, 3314216, 3303373, 3325755, 3009103, 3314207, 730004, 3314194, 3314217, 3009002, 3009104, 3329390, 3325980, 3314248, 730005, };

        }
    }
    public class Lottery
    {
        public ConcurrentDictionary<uint, Item> Items;
        public Client.GameClient user;
        public Lottery(Client.GameClient _user)
        {
            user = _user;
            Items = new ConcurrentDictionary<uint, Item>();
        }
        public class Item
        {
            public uint ItemType;
            public uint Bound;
            public byte ChancesLeft;
            public byte Bliss;
            public byte SocketOne;
            public byte SocketTwo;
            public uint Plus;
            public uint Amount;
            public uint MeltReward;
        }
        public uint ID = 0;
        public uint ChancesLeft = 0;
        public void AddItem(uint Count, uint Id)
        {
            ID = Id;
            LotteryTable.GenerateLotteryPrize();
            for (uint index = 0; index < Count; index++)
            {
                uint awared;
                byte bliss;
                uint Plus;
                uint Bound;
                byte SocketOne;
                byte SocketTwo;
                LotteryTable.GetReward(Id, out awared, out bliss, out Plus, out Bound, out SocketOne, out SocketTwo);
                var x = new Item
                {
                    ItemType = awared,
                    Bound = Bound,
                    ChancesLeft = 0,
                    Bliss = bliss,
                    SocketOne = SocketOne,
                    SocketTwo = SocketTwo,
                    Plus = Plus,
                    Amount = 1,
                    MeltReward = 0,
                };
                Items.Add(index, x);
                int m_Index = LotteryTable.LotteryList.Values.Count;
                LotteryTable.LotteryList.Add((uint)m_Index, new LotteryTable.Lottery()
                {
                    Awared = awared,
                    Bliss = bliss,
                    Name = user.Player.Name,
                    Id= Id,
                });
            }
        }
    }
 
}

