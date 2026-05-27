using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VirusX;

namespace Poker
{
    public class PokerLoad
    {
        public static Dictionary<uint, PokerTable> Tables;
        public static void Load()
        {
            try
            {
                Tables = new Dictionary<uint, PokerTable>();
                string[] array = File.ReadAllLines(Program.ServerConfig.DbLocation + "PokerTables.ini");
                foreach (string text in array)
                {

                    string[] array3 = text.Split(',');
                    PokerTable pokerTable = new PokerTable(Convert.ToUInt32(array3[0]));
                    if (Tables.ContainsKey(pokerTable.Id))
                        continue;
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
                    pokerTable.TableType = Poker.General.TableType.TexasHoldem;
                    if (pokerTable.Mesh == 7249527)
                        pokerTable.OMAHA = true;
                    if (pokerTable.Mesh == 7578087)
                        pokerTable.TableType = Poker.General.TableType.ShowHand;
                
                    pokerTable.MapId = Convert.ToUInt16(array3[8]);
                    Tables.Add(pokerTable.Id, pokerTable);

                }
               //VirusX.MyConsole.WriteLine("Poker Loading Count: " + Tables.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
