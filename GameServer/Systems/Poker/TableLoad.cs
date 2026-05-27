using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ConquerOnline.Game.MsgServer;
using ConquerOnline.Role.Instance;
namespace ConquerOnline.Database
{
    public class Poker
    {
        public static bool Hhh = false;
        public static System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.PokerTable> Tables = new System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.PokerTable>();

        public static void Load()
        {
            
            string[] array = File.ReadAllLines(Program.ServerConfig.DbLocation + "PokerTables.txt");
            foreach (string text in array)
            {
                string[] array3 = text.Split(' ');
                if (array3[0] == "")
                    continue;
                Role.Instance.PokerTable pokerTable = new Role.Instance.PokerTable(Convert.ToUInt32(array3[0]));
                if (Tables.ContainsKey(pokerTable.Id)) continue;
                pokerTable.X = Convert.ToUInt16(array3[1]);
                pokerTable.Y = Convert.ToUInt16(array3[2]);
                pokerTable.Mesh = Convert.ToUInt32(array3[3]);
                pokerTable.Number = Convert.ToUInt32(array3[4]);
                int num = Convert.ToByte(array3[5]);
                if (num == 1)
                {
                    pokerTable.UnLimited = true;
                }
                num = Convert.ToByte(array3[6]);
                if (num == 1)
                {
                    pokerTable.IsCPs = true;
                    pokerTable.MapId = 1860;
                }
                pokerTable.MinBet = Convert.ToUInt32(array3[7]);
                pokerTable.TableType = Role.Flags.TableType.TexasHoldem;
                if (pokerTable.Mesh == (uint)7249527)
                {
                    pokerTable.OMAHA = true;
                }
                if (pokerTable.Mesh == (uint)7578087)
                {
                    pokerTable.TableType = Role.Flags.TableType.ShowHand;
                }

                pokerTable.TableType = (Role.Flags.TableType)((byte)Convert.ToUInt32(array3[8]));
                Tables.Add(pokerTable.Id, pokerTable);

            }
            LoadTablePokerCross();

        }
        public static void LoadTablePokerCross()
        {

            string[] array = File.ReadAllLines(Program.ServerConfig.DbLocation + "PokerTablesCross.txt");
            foreach (string text in array)
            {
                string[] array3 = text.Split(' ');
                if (array3[0] == "")
                    continue;
                Role.Instance.PokerTable pokerTable = new Role.Instance.PokerTable(Convert.ToUInt32(array3[0]));
                if (Tables.ContainsKey(pokerTable.Id)) continue;
                pokerTable.Number = Convert.ToUInt32(array3[0]);
                
                pokerTable.TableType = Role.Flags.TableType.TexasHoldem;
               
                Tables.Add(pokerTable.Id, pokerTable);

            }


        }
        
    }
}