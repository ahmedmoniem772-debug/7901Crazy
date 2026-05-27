using ConquerOnline.Role.Instance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerOnline
{
    public class cq_xuanbao_rand_attr
    {
        public static List<RandomAttribute> xuanbao_rand_attr;
        public class RandomAttribute
        {
            public uint ItemID;
            public RelicAttribute.Attribute Attribute;
            public uint Min, Max;
            public bool dwParam;
        }
        public static void Load()
        {
            try
            {
                if (File.Exists(Program.ServerConfig.DbLocation + "xuanbao_rand_attr.txt"))
                {
                    xuanbao_rand_attr = new List<RandomAttribute>();
                    string[] Lines = File.ReadAllLines((Program.ServerConfig.DbLocation + "xuanbao_rand_attr.txt"));
                    foreach (var line in Lines)
                    {
                        var spilitline = line.Split(new string[] { "@@", " " }, StringSplitOptions.RemoveEmptyEntries);
                        RandomAttribute obj = new RandomAttribute();
                        obj.ItemID = Convert.ToUInt32(spilitline[0]);
                        obj.Attribute = (RelicAttribute.Attribute)Convert.ToByte(spilitline[1]);
                        obj.Min = Convert.ToUInt32(spilitline[2]);
                        obj.Max = Convert.ToUInt32(spilitline[3]);
                        if (obj.Max % 10 == 1)
                        {
                            obj.Max--;
                            obj.dwParam = true;
                        }
                        xuanbao_rand_attr.Add(obj);

                    }
                }
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }
        public static void GetRandom(uint ItemID, out RelicAttribute.Attribute Attribute, out uint value, bool LukyStrike = false)
        {
            Attribute = RelicAttribute.Attribute.None;
            value = 0;
            try
            {
            again:
                RelicAttribute.Attribute Attr;
                if (LukyStrike)
                    Attr = (RelicAttribute.Attribute)Pool.GetRandom.Next(1, 15);
                else
                    Attr = (RelicAttribute.Attribute)Pool.GetRandom.Next(1, 14);
                switch (ItemID)
                {
                    case 4100001://Featured (M-Attack)
                        {
                            if (Attr == RelicAttribute.Attribute.SkillCriticalStrike || Attr == RelicAttribute.Attribute.CriticalStrike || Attr == RelicAttribute.Attribute.HPAdd || Attr == RelicAttribute.Attribute.AddAttack)
                                goto again;
                            break;
                        }
                    case 4100002://Featured (Max-HP)
                        {
                            if (Attr == RelicAttribute.Attribute.SkillCriticalStrike || Attr == RelicAttribute.Attribute.CriticalStrike || Attr == RelicAttribute.Attribute.AddMagicAttack || Attr == RelicAttribute.Attribute.AddAttack)
                                goto again;
                            break;
                        }
                    case 4100003://Featured (M-Strike)
                        {
                            if (Attr == RelicAttribute.Attribute.HPAdd || Attr == RelicAttribute.Attribute.CriticalStrike || Attr == RelicAttribute.Attribute.AddMagicAttack || Attr == RelicAttribute.Attribute.AddAttack)
                                goto again;
                            break;
                        }
                    case 4100004://Featured (P-Attack)
                        {
                            if (Attr == RelicAttribute.Attribute.HPAdd || Attr == RelicAttribute.Attribute.CriticalStrike || Attr == RelicAttribute.Attribute.AddMagicAttack || Attr == RelicAttribute.Attribute.SkillCriticalStrike)
                                goto again;
                            break;
                        }
                    case 4100005://Featured (P-Strike)
                        {
                            if (Attr == RelicAttribute.Attribute.HPAdd || Attr == RelicAttribute.Attribute.AddAttack || Attr == RelicAttribute.Attribute.AddMagicAttack || Attr == RelicAttribute.Attribute.SkillCriticalStrike)
                                goto again;
                            break;
                        }
                }
                Attribute = Attr;
                if (Attr == RelicAttribute.Attribute.LuckyStrike)
                {
                    value = 100;
                }
                else
                {
                    var GET_value = xuanbao_rand_attr.Where(p => p.ItemID == ItemID && p.Attribute == Attr).FirstOrDefault();
                    if (GET_value != null)
                    {
                        value = (uint)Pool.GetRandom.Next((int)GET_value.Min, (int)GET_value.Max);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
