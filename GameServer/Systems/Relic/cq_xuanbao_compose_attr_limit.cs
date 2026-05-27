using ConquerOnline.Role.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerOnline
{
    public class cq_xuanbao_compose_attr_limit
    {

        public static Dictionary<uint, attr_limit> xuanbao_compose_attr_limit;
        public class attr_limit
        {
            public uint id;
            public uint uk;
            public uint Score;
            public uint limit;
        }
        public static void Load()
        {
            try
            {
                if (!System.IO.File.Exists(Program.ServerConfig.DbLocation + "xuanbao_compose_attr_limit.txt"))
                {
                    Console.WriteLine("!File.Exists: " + Program.ServerConfig.DbLocation + "xuanbao_compose_attr_limit.txt");
                    return;
                }
                xuanbao_compose_attr_limit = new Dictionary<uint, attr_limit>();
                var _rate = System.IO.File.ReadAllLines(Program.ServerConfig.DbLocation + "xuanbao_compose_attr_limit.txt");
                foreach (string Line in _rate)
                {
                    string[] Data = Line.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                    attr_limit obj = new attr_limit();
                    obj.id = Convert.ToUInt32(Data[0]);
                    obj.uk = Convert.ToUInt32(Data[1]);
                    obj.Score = Convert.ToUInt32(Data[2]);
                    obj.limit = Convert.ToUInt32(Data[3]);
                    xuanbao_compose_attr_limit.Add(obj.id, obj);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }
    }
}
