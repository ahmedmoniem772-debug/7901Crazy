using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VirusX.Game
{
    class ServerLogs
    {
       
        public static void AddTradeLog(Role.Instance.Trade first, Client.GameClient client, Role.Instance.Trade second, String secondN)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                    Path = "ServerLogs\\Trade\\",
                    NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("************************************************************************************");
                    file.WriteLine("you traded with {0}", secondN);
                    file.WriteLine("Silver sent: " + first.Money);
                    file.WriteLine("Cps sent: " + first.ConquerPoints);
                    file.WriteLine("Silver received: " + second.Money);
                    file.WriteLine("Cps received: " + second.ConquerPoints);


                    foreach (var i in first.Items.Values)
                    {
                        file.WriteLine("Items sent : " + i.ToLog());
                    }
                    foreach (var i in second.Items.Values)
                    {
                        file.WriteLine("Items received : " + i.ToLog());
                    }
                    file.WriteLine("Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "");
                    file.WriteLine("************************************************************************************");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddChatLog(MsgServer.MsgMessage msg, Client.GameClient client, string bc = "")
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                 Path = "ServerLogs\\Chats\\",
                 NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("************************************************************************************");

                    if (msg != null)
                    {
                        file.WriteLine("your Chat with {0}", msg._To);
                        file.WriteLine("msg: " + msg.__Message);
                    }
                    if (bc != "")
                    {
                        file.WriteLine("your Broadcast");
                        file.WriteLine("msg: " + bc);
                    }
                    file.WriteLine("Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "");
                    file.WriteLine("************************************************************************************");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }

        }
        public static void AddChatGMLog(MsgServer.MsgMessage msg, Client.GameClient client)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                              Path = "ServerLogs\\AddChatGMLog\\",
                 NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("************************************************************************************");

                    if (msg != null)
                    {
                        file.WriteLine("your Chat with {0}", msg._To);
                        file.WriteLine("msg: " + msg.__Message);
                    }
                    file.WriteLine("Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "");
                    file.WriteLine("************************************************************************************");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddChatGMWrongLog(MsgServer.MsgMessage msg, Client.GameClient client)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                                       Path = "ServerLogs\\AddChatGMWrongLog\\",
                 NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("************************************************************************************");

                    if (msg != null)
                    {
                        file.WriteLine("your Chat with {0}", msg._To);
                        file.WriteLine("msg: " + msg.__Message);
                    }
                    file.WriteLine("Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "");
                    file.WriteLine("************************************************************************************");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddDropLog(Client.GameClient client, MsgServer.MsgGameItem Item)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                      Path = "ServerLogs\\AddDropLog\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You dropped a " + file.NewLine + " {0} " + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.ToLog());
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CheckItemsAdd(Client.GameClient client, MsgServer.MsgGameItem Item)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                      Path = "ServerLogs\\CheckItemsAdd\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You CheckItemsAdd a " + file.NewLine + " {0} " + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.ToLog());
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CheckItemsMove(Client.GameClient client, MsgServer.MsgGameItem Item)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                      Path = "ServerLogs\\CheckItemsMove\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You CheckItemsMove a " + file.NewLine + " {0} " + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.ToLog());
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }

        }
        public static void CheckItemsRemove(Client.GameClient client, MsgServer.MsgGameItem Item)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                      Path = "ServerLogs\\CheckItemsRemove\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You CheckItemsRemove a " + file.NewLine + " {0} " + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.ToLog());
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddBuyingLogShopMallCps(Client.GameClient client, Database.ItemType.DBItem Item, string price, string type)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                          Path = "ServerLogs\\ShoppingMallCps\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You BuyItem ShoppingMall " + file.NewLine + " {0} " + file.NewLine + " ID : {1} " + file.NewLine + " Price : {2} {3}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.Name, Item.ID, price, type);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddBuyingLogShopMallBCps(Client.GameClient client, Database.ItemType.DBItem Item, string price, string type)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                          Path = "ServerLogs\\AddBuyingLogShopMallBCps\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You BuyItem ShoppingMall " + file.NewLine + " {0} " + file.NewLine + " ID : {1} " + file.NewLine + " Price : {2} {3}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.Name, Item.ID, price, type);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddBuyingLogShopMallGold(Client.GameClient client, Database.ItemType.DBItem Item, string price, string type)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                          Path = "ServerLogs\\ShoppingMallGold\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You BuyItem ShoppingMall " + file.NewLine + " {0} " + file.NewLine + " ID : {1} " + file.NewLine + " Price : {2} {3}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.Name, Item.ID, price, type);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddBuyingLogShopBooth(Client.GameClient client, Database.ItemType.DBItem Item, string price, string type)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                      Path = "ServerLogs\\ShopBooth\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You BuyItem ShopBooth " + file.NewLine + " {0} " + file.NewLine + " ID : {1} " + file.NewLine + " Price : {2} {3}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.Name, Item.ID, price, type);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddBuyingExchangeShop(Client.GameClient client, Database.ItemType.DBItem Item, string price, string type)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
             Path = "ServerLogs\\BuyingExchangeShop\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You BuyItem BuyingExchangeShop " + file.NewLine + " {0} " + file.NewLine + " ID : {1} " + file.NewLine + " Price : {2} {3}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.Name, Item.ID, price, type);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AddBuyingOtherShop(Client.GameClient client, Database.ItemType.DBItem Item, string price, string type)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                                Path = "ServerLogs\\OtherShop\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");
                    file.WriteLine("You BuyItem OtherShop " + file.NewLine + " {0} " + file.NewLine + " ID : {1} " + file.NewLine + " Price : {2} {3}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.Name, Item.ID, price, type);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void NameChangeLog(Client.GameClient client, string Name, string ChangeName)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                        Path = "ServerLogs\\NameChangeLog\\",
               NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Name " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " NameChange : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Name, ChangeName);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void DemonBoxLog(Client.GameClient client, Database.ItemType.DBItem Item, string price, string type)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                         Path = "ServerLogs\\DemonBox\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Received " + file.NewLine + " {0} " + file.NewLine + " ID : {1} " + file.NewLine + " Price : {2} {3}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Item.Name, Item.ID, price, type);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CpsGet(Client.GameClient client, long Get, long Cps)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                       Path = "ServerLogs\\Check\\CpsGet\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Have Cps : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Get, Cps);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CpsLose(Client.GameClient client, long Lost, long Cps)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                  Path = "ServerLogs\\CpsLose\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Lost " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Have Cps : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Lost, Cps);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CpsBGet(Client.GameClient client, int Get, int Cps)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                       Path = "ServerLogs\\Check\\CpsBGet\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Have Cps : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Get, Cps);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CpsBLose(Client.GameClient client, int Lost, int Cps)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                  Path = "ServerLogs\\CpsBLose\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Lost " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Have Cps : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Lost, Cps);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void DepositWarehouse(Client.GameClient client, long Lost, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                                      Path = "ServerLogs\\DepositWarehouse\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Lost " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Have Money : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Lost, Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void WarehouseWithdraw(Client.GameClient client, long Get, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
          Path = "ServerLogs\\WarehouseWithdraw\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Have Money : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Get, Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void SlotMachine(Client.GameClient client, long Get, long Cps)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                        Path = "ServerLogs\\SlotMachine\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Cps : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Get, Cps);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void SlotMachineMoney(Client.GameClient client, long Get, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                                     Path = "ServerLogs\\SlotMachineMoney\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Money : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Get, Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void NpcChangeMoney(Client.GameClient client, long Money, long Cps)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                       Path = "ServerLogs\\NpcChangeMoney\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Money " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Cps : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Money, Cps);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void NpcChangeCps(Client.GameClient client, int Cps, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\NpcChangeCps\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Cps " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Money : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Cps, Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void StarDragonBall(Client.GameClient client)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.OnLogin.MacAddress.ToString(),
                  Path = "ServerLogs\\StarDragonBall\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You " + client.Player.Name + " Prize");
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void ConquerLeter(Client.GameClient client, uint ItemID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.OnLogin.MacAddress.ToString(),
                  Path = "ServerLogs\\ConquerLeter\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You remove " + Pool.ItemsBase[ItemID].Name + " ");
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AnimaUpdateSucced(Client.GameClient client, uint AnimaID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\AnimaUpdateSucced\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Anima " + file.NewLine + " {0} " + file.NewLine  + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", AnimaID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AnimaUpdateAwakend(Client.GameClient client, uint AnimaID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\AnimaUpdateAwakend\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Anima " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", AnimaID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AnimaUpdateOne(Client.GameClient client, uint AnimaID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\AnimaUpdateOne\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Anima " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", AnimaID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AnimaUpdateOneFailed(Client.GameClient client, uint AnimaID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\AnimaUpdateOneFailed\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You remove Anima " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", AnimaID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AnimaUpdateForging(Client.GameClient client, uint AnimaID)
        {
            try
            {

                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\AnimaUpdateForging\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Anima " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", AnimaID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AnimaUpdateFail(Client.GameClient client, uint AnimaID, uint Count)
        {
            try
            {

                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
              Path = "ServerLogs\\AnimaUpdateFail\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Lose " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " Cps : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", AnimaID, Count);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void NpcChangeAnimaForCps(Client.GameClient client, int ConquerPoints, int AnimaID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\NpcChangeAnimaForCps\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Cps " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " AnimaID : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", ConquerPoints, AnimaID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void NpcChangeRuneForCps(Client.GameClient client, int ConquerPoints, int CountRune)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\NpcChangeRuneForCps\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Cps " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " CountRune : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", ConquerPoints, CountRune);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CpsHigherBank(Client.GameClient client, long ConquerPoints)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\Check\\CpsHigherBank\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Have Cps " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", ConquerPoints);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CpsHigher(Client.GameClient client, long ConquerPoints)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\Check\\CpsHigher\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Have Cps " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", ConquerPoints);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void CpsBHigher(Client.GameClient client, int bConquerPoints)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\Check\\CpsBHigher\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Have Cps " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", bConquerPoints);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void MoneyHigher(Client.GameClient client, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\Check\\MoneyHigher\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Have Money " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void MoneyHigherThanTransferBank(Client.GameClient client, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\Check\\MoneyHigherThan\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Have Transfer Bank " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AnimaAlots(Client.GameClient client, uint AnimaID, int Count)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
               Path = "ServerLogs\\Check\\AnimaAlots\\" + Pool.ItemsBase[AnimaID].Name + "",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Have Anima " + file.NewLine + " {0} " + file.NewLine + file.NewLine + " CountRune : {1}" + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", AnimaID, Count);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void PokerAWin(Client.GameClient client, uint CardID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\PokerAWin\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Card " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", CardID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void PokerBWin(Client.GameClient client, uint CardID)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\PokerBWin\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Card " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", CardID);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void ChangeMoney(Client.GameClient client, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\ChangeMoney\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Money " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void GetMoney(Client.GameClient client, long Money)
        {
            try
            {

                if (Program.ServerConfig.IsInterServer)
                    return;
                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\GetMoney\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Money " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void ChangeMoneyToBag(Client.GameClient client, string Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\ChangeMoneyToBag\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Item " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void WinnerInPokerMoney(Client.GameClient client, long Money)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\WinnerInPokerMoney\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Money " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", Money);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void WinnerInPokerCps(Client.GameClient client, int CPs)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\WinnerInPokerCps\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You Get Money " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", CPs);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void AccountVIP(Client.GameClient client, byte VIPLEVEL)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\AccountVIP\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You  VIPLEVEL " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", VIPLEVEL);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static void OnlineMintues(Client.GameClient client, uint OnlineMintues)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                String folderN = client.Player.Name.RemoveIllegalCharacters(false, true),
                            Path = "ServerLogs\\OnlineMintues\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine("------------------------------------------------------------------------------------");

                    file.WriteLine("You  VIPLEVEL " + file.NewLine + " {0} " + file.NewLine + file.NewLine + "Date = " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "  " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "", OnlineMintues);
                    file.WriteLine("------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                MyConsole.WriteLine(client.Player.Name.ToString().Replace("~", "").Replace("#!", "").Replace("@", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("(", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "").Replace("+", "").Replace("/", "").Replace("*", "").Replace("-", "")
                    .Replace("/", "").Replace("<", "").Replace(">", "").Replace(";", "").Replace(":", ""));

            }
        }
        public static unsafe void PacketHighLength(ServerSockets.Packet stream)
        {
            try
            {
                if (Program.ServerConfig.IsInterServer)
                    return;

                byte[] b = new byte[stream.Size];
                fixed (byte* ptr = b)
                {
                    stream.memcpy(ptr, stream.Memory, stream.Size);
                }
                String folderN = BitConverter.ToUInt16(b, 2).ToString(),
                            Path = "ServerLogs\\PacketHighLength\\",
                NewPath = System.IO.Path.Combine(Path, folderN);
                if (!File.Exists(NewPath + folderN))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                }
                if (!File.Exists(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt"))
                    {
                        fs.Close();
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".txt", true))
                {
                    file.WriteLine(stream.Size);
                }
            }
            catch
            {
            }
        }
    }
}
