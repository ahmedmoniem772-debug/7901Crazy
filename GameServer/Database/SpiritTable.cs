using System;
using System.Collections.Generic;
using System.IO;

namespace VirusX.Database
{
    public class SpiritTable
    {
        public static Dictionary<uint,Spirit> SpiritRates;

        public enum Type : byte
        {
            Failed,
            Upgraded,
            Awkaned
        }
        public class Spirit
        {
            public uint id;
            public uint Type;
            public uint AnimaID;
            public uint rank;
            public uint Prizet1;
            public uint Prizev1;
            public uint Prizet2;
            public uint Prizev2;
        }

        public static void Load()
        {
            SpiritRates = new Dictionary<uint, Spirit>();
            if (File.Exists(Program.ServerConfig.DbLocation + "spirit_rate.txt"))
            {
                string[] Lines = File.ReadAllLines(Program.ServerConfig.DbLocation + "spirit_rate.txt");
                foreach (var line in Lines)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;
                    var spilitline = line.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    Spirit s = new Spirit();
                    s.id = uint.Parse(spilitline[0]);
                    s.Type = uint.Parse(spilitline[1]);
                    s.AnimaID = uint.Parse(spilitline[2]);
                    s.rank = uint.Parse(spilitline[3]);
                    s.Prizet1 = uint.Parse(spilitline[4]);
                    s.Prizev1 = uint.Parse(spilitline[5]);
                    s.Prizet2 = uint.Parse(spilitline[6]);
                    s.Prizev2 = uint.Parse(spilitline[7]);
                    SpiritRates.Add(s.id, s);
                }
            }
        }
    }
}