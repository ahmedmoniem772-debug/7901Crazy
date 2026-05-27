using System;
using System.Collections.Generic;
using System.IO;

namespace VirusX.Database
{
    public class HairfaceStorageType
    {
        public static System.Collections.Generic.List<Hairface> Hairfaces = new System.Collections.Generic.List<Hairface>();
        public class Hairface
        {
            public Game.MsgServer.MsgHairfaceStorage.Type Type;
            public uint ID;
            public string Name;
            public byte RareLevel;
            public byte RequiredVIPLevel;
            public uint Cost;
            public List<byte> Classes;
            public byte Sex;


            public byte EquippedColor;
            public byte EquippedHair;
            public byte EquippedFace;
            public byte EquippedTable;
            public byte EquippedCardBack;
            public byte EquippedBet;
            public byte Equippedlevel;
            public byte EquippedMap;
            public byte EquippedDealer;
            public byte EquippedCarpet;
            public byte EquippedFrame;
            public byte[] Colors;
            public bool Equiped;
        }
        public static void Load()
        {
            Hairfaces.Clear();

            var path = Program.ServerConfig.DbLocation + "hairface_storage_type.txt";
            if (!File.Exists(path))
                return;

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var s = line.Split(new[] { "@@" }, StringSplitOptions.None);
                if (s.Length < 4)
                    continue;

                var hf = new Hairface();

                // Type
                byte type;
                if (!byte.TryParse(s[0], out type))
                    continue;
                hf.Type = (Game.MsgServer.MsgHairfaceStorage.Type)type;

                // ID
                uint id;
                if (!uint.TryParse(s[1], out id))
                    continue;
                hf.ID = id;

                // Name
                hf.Name = s[3];

                // RareLevel
                byte rare;
                hf.RareLevel = (s.Length > 4 && byte.TryParse(s[4], out rare)) ? rare : (byte)0;

                // VIP
                byte vip;
                hf.RequiredVIPLevel = (s.Length > 5 && byte.TryParse(s[5], out vip)) ? vip : (byte)0;

                // Cost
                uint cost;
                hf.Cost = (s.Length > 7 && uint.TryParse(s[7], out cost)) ? cost : 0;

                // Classes
                hf.Classes = new List<byte>();
                if (s.Length > 9 && !string.IsNullOrWhiteSpace(s[9]))
                {
                    var classParts = s[9].Split(',');
                    foreach (var c in classParts)
                    {
                        byte cls;
                        if (byte.TryParse(c, out cls))
                            hf.Classes.Add(cls);
                    }
                }

                // Sex
                byte sex;
                hf.Sex = (s.Length > 10 && byte.TryParse(s[10], out sex)) ? sex : (byte)0;

                // Avatar & Hairstyle → no class restriction
                if (hf.Type == Game.MsgServer.MsgHairfaceStorage.Type.Avatar ||
                    hf.Type == Game.MsgServer.MsgHairfaceStorage.Type.Hairstyle)
                {
                    hf.Classes.Clear();
                }

                hf.Colors = new byte[7] { 1, 0, 0, 0, 0, 0, 0 };

                Hairfaces.Add(hf);
            }
        }
        public static void LoadX()
        {
            if (System.IO.File.Exists(Program.ServerConfig.DbLocation + "hairface_storage_type.txt"))
            {
                string[] Lines = System.IO.File.ReadAllLines((Program.ServerConfig.DbLocation + "hairface_storage_type.txt"));
                foreach (var line in Lines)
                {
                    var spilitline = line.Split(new string[] { "@@", "@", "@@@@" }, System.StringSplitOptions.RemoveEmptyEntries);
                    var Hf = new Hairface();
                    Hf.Type = (Game.MsgServer.MsgHairfaceStorage.Type)System.Convert.ToByte(spilitline[0]);
                    Hf.ID = System.Convert.ToUInt32(spilitline[1]);
                    Hf.Name = spilitline[3];
                    Hf.RareLevel = System.Convert.ToByte(spilitline[4]);
                    Hf.RequiredVIPLevel = System.Convert.ToByte(spilitline[5]);
                    Hf.Cost = System.Convert.ToUInt32(spilitline[7]);
                    Hf.Classes = new System.Collections.Generic.List<byte>();
                    if (spilitline.Length > 9)
                    {
                        var spilit = spilitline[9].Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in spilit)
                            Hf.Classes.Add(byte.Parse(item));
                        if (spilitline.Length > 10)
                            Hf.Sex = System.Convert.ToByte(spilitline[10]);
                        else
                            Hf.Sex = 0;
                    }
                    Hf.Colors = new byte[7] { 1, 0, 0, 0, 0, 0, 0 };
                    Hairfaces.Add(Hf);
                }
            }
        }
    }
}