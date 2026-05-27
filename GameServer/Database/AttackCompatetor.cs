using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConquerOnline.Database
{
    public static class AttackCompatetor
    {
       
        public static Dictionary<ushort, byte> Skills = new Dictionary<ushort, byte>();
      
        public static void Load()
        {
            if (!File.Exists(Program.ServerConfig.DbLocation + "\\Comatetor.ini"))
            {
                File.Create(Program.ServerConfig.DbLocation + "\\Comatetor.ini").Dispose();
            }
            WindowsAPI.IniFile reader = new WindowsAPI.IniFile("\\Comatetor.ini");
            ushort Count = reader.ReadUInt16("System", "Count", 0);
            if (Count > 0)
            {
                for (ushort x = 0; x < Count; x++)
                {
                    ushort ID = reader.ReadUInt16("Skill" + x, "ID", 0);
                    byte Value = reader.ReadByte("Skill" + x, "Value", 0);
                    if (!Skills.ContainsKey(ID))
                    {
                        Skills.Add(ID, Value);
                    }
                }
            }
        }
       
        public static void Insert(ushort ID, byte Value)
        {
            if (Skills.ContainsKey(ID))
            {
                Skills[ID] = Value;
            }
            else
                Skills.Add(ID, Value);
        }
        
        public static bool CheckDmg(ushort ID, out int Damage)
        {
            if (Skills.ContainsKey(ID))
            {
                Damage = Skills[ID];
                return true;
            }
            Damage = 0;
            return false;
        }

        private static int Percentage(int number, int numerator, int denominator)
        {
            return (number * numerator) / denominator;
        }

        public static void Save()
        {
            try
            {
                if (!File.Exists(Program.ServerConfig.DbLocation + "\\Comatetor.ini"))
                {
                    File.Create(Program.ServerConfig.DbLocation + "\\Comatetor.ini").Dispose();
                }
                WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Comatetor.ini");
                write.Write<ushort>("System", "Count", (ushort)Skills.Count);
                if (Skills.Count > 0)
                {
                    int z = 0;
                    foreach (var query in Skills)
                    {
                        write.Write<ushort>("Skill" + z, "ID", query.Key);
                        write.Write<byte>("Skill" + z, "Value", query.Value);
                        z += 1;
                    }
                }
            }
            catch
            {
                MyConsole.WriteLine("Error in saving attack compatetor.");
            }
        }
    }
}
