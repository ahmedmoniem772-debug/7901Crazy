using System;
using System.Collections.Generic;
using System.IO;

namespace VirusX.Database
{

    public class new_shop_goods
    {
        public class item
        {
            public uint id;
            public uint unk1;
            public uint shopid;
            public uint ItemID;
            public uint CountBuy;
            public byte Bound;
            public uint amount;

        }
        public static Dictionary<uint, new_shop_goods.item> ShopClan;

        public static void Load()
        {
            try
            {
                if (!File.Exists(Program.ServerConfig.DbLocation + "\\new_shop_goods.txt"))
                {
                    Console.WriteLine("!File.Exists: " + Program.ServerConfig.DbLocation + "\\new_shop_goods.txt");
                }
                else
                {
                    string[] strArray1 = File.ReadAllLines(Program.ServerConfig.DbLocation + "\\new_shop_goods.txt");
                    new_shop_goods.ShopClan = new Dictionary<uint, new_shop_goods.item>();
                    for (int index = 0; index < strArray1.Length; ++index)
                    {
                        string[] strArray2 = strArray1[index].Split(new string[1] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
                        new_shop_goods.item obj = new new_shop_goods.item();
                        obj.id = uint.Parse(strArray2[0]);
                        obj.shopid = uint.Parse(strArray2[1]);
                        obj.unk1 = uint.Parse(strArray2[2]);
                        obj.Bound = byte.Parse(strArray2[3]);
                        obj.ItemID = uint.Parse(strArray2[4]);
                        obj.CountBuy = uint.Parse(strArray2[5]);
                        obj.amount = uint.Parse(strArray2[7]);
                        new_shop_goods.ShopClan.Add(obj.id, obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
            }
        }


    }
}