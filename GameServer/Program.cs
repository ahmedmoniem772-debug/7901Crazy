using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using VirusX.DBFunctionality;
using VirusX.Game.MsgServer;
using VirusX.Cryptography;
using System.Net.Http;
using VirusX.Database;
using VirusX.Panels;
using System.Windows.Forms;
using VirusX.ServerSockets;
using VirusX.Game;
using VirusX.MsgInterServer;
using System.Net;
using System.Collections.Specialized;
//using Newtonsoft.Json;
using COServer;
using VirusX;
using VirusX.Game.MsgTournaments;
using VirusX.Game.Ai;
using VirusX.Systems.ChangeSystem.SystemBanned.Panels;

namespace VirusX
{
    class Program
    {


        public static List<uint> NoHP = new List<uint>() { 5051, 5053, 5054, 5055, 5056, 5057, 5058 };
        public static List<uint> FreePkMap = new List<uint>() { 2353, 22330, 22331, 22332, 22333, 22334, 22335, 22336, 22337, 22338, 26391, 26392, 26393, 2515, 3954, 6525, 6521, 10550, 26394, 26395, 26396, 26397, 26398, 26399, 10137, 11447, 3998, 3071, 5051, 3691, 3692, 3693, 3694, 3695, 3696, 2581, 2582, 2583, 2584, 2585, 6000, 6001, 1505, 1005, 1038, 700, 1508/*PkWar*/, 1357, Game.MsgTournaments.MsgCaptureTheFlag.MapID };
        public static List<uint> TreadeOrShop = new List<uint>() { 5050, 5051, 5052, 5053, 5054, 5055, 5056, 5057, 5058, 5059, 5060, 5061, 5062, 5063, 5064, 5065, 5066, 5067, 5068, 5069, 5070 };
        public static List<uint> FBMap = new List<uint>() { 5051, 5053, 5054, 5055, 5056, 5057, 5058 };
        public static List<uint> NoAgateMap = new List<uint>() { 3301, 3302, 3303, 3304, 3305, 3306, 3307, 3308, 3309, 3310, 3311, 3312, 3313, 700, 1038, 1764, 2057, 1081, 2060, 1080, 6521, 1138, 3954, 3820, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        public static SafeRandom GetRandom = new SafeRandom();
        public static Packets packetsForm;
        public static SpawnMonster Spawn;
        public static bool ExitRequested = false;
        public static ServerSockets.ServerSocket VirusX;
        [DllImport("kernel32.dll")]
 
        
        private static extern bool SetConsoleCtrlHandler(ConsoleHandlerDelegate handler, bool add);
        private delegate bool ConsoleHandlerDelegate(int type);
        public static unsafe void PrintPacket(ServerSockets.Packet stream)
        {
            byte[] b = new byte[stream.Size];
            fixed (byte* ptr = b)
            {
                stream.memcpy(ptr, stream.Memory, stream.Size);
            }
            foreach (byte D in b)
            {
                System.Console.Write((Convert.ToString(D, 16)).PadLeft(2, '0').ToUpper() + " ");
            }
            System.Console.Write("\n\n");

        }
        private static ConsoleHandlerDelegate handlerKeepAlive;
        public static bool ProcessConsoleEvent(int type)
        {
            lock (Database.ServerDatabase.SavingObj)
            {
                try
                {
                    ExitRequested = true;
                    if (ServerConfig.IsInterServer)
                    {
                        foreach (var client in Pool.GamePoll.Values)
                        {
                            try
                            {
                                if (client.Socket != null)
                                    client.Socket.Disconnect();
                            }
                            catch (Exception e)
                            {
                                MyConsole.WriteLine(e.ToString());
                            }
                        }
                        return true;
                    }
                    try
                    {

                        
                       
                    }
                    catch (Exception e) { MyConsole.SaveException(e); }

                    MyConsole.WriteLine("Saving Online Clients Information..." );
                    Server.SaveDBPayers();
                    foreach (var client in Pool.GamePoll.Values)
                    {
                        try
                        {
                            if (client.Socket != null)
                                client.Socket.Disconnect();
                        }
                        catch (Exception e)
                        {
                            MyConsole.WriteLine(e.ToString());
                        }
                    }
                    
                    MyConsole.WriteLine("Saving Global Information..." );
                    if (!Program.ServerConfig.IsInterServer)
                        Server.SaveDatabase();
                    MyConsole.WriteLine("Database has been saved successfully." );
                    MyConsole.WriteLine("Waiting for online players information to be saved..." );
                    if (Database.ServerDatabase.LoginQueue.Finish())
                    {
                        MyConsole.WriteLine("Online players information saved successfully..." );
                        System.Threading.Thread.Sleep(2000);
                    }
                    if (VirusX != null)
                        VirusX.Close();
                    
                }
                catch (Exception e)
                {
                    MyConsole.SaveException(e);
                }
                return true;
            }
        }
        public static class ServerConfig
        {
            public static string LogginKey = "R3Xx97ra5j8D6uZz";

            public static string CO2Folder = "";

            public static string Website = "";
            public static string XtremeTopLink = "";
            public static uint ServerID = 0;

            public static uint ConquerorWinner = 0;
            public static string IPAddres = "192.168.1.116";
            public static ushort AuthPort = 0;
            public static ushort GamePort = 0;
            public static string ServerName = "VirusX";
            public static string Program = "";
            //InternetPort
            public static ushort Port_BackLog;
            public static ushort Port_ReceiveSize = 16384;
            public static ushort Port_SendSize = 16384;

            //Database
            public static string DbLocation = "";


            public static uint ExpRateSpell = 2;
            public static uint ExpRateProf = 2;
            public static uint ExpHunt = 100;
            public static uint UserExpRate = 10;
            public static uint PhysicalDamage = 100;

            public static bool IsInterServer = false;
        }
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
        public static void Maintenance()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (5 Minutes). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (5 Minutes). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                //DiscordAPI.Enqueue("The server will be brought down for maintenance in (5 Minutes). Please log off immediately to avoid data loss.");
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {

                MyConsole.WriteLine("The server will be brought down for maintenance in (4 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (4 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (4 Minutes & 00 Seconds). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (4 Minutes & 00 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (3 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (3 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (3 Minutes). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (3 Minutes). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (2 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (2 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (2 Minutes). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (2 Minutes ). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (1 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (1 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (1 Minutes). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (1 Minutes). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
            Thread.Sleep(1000 * 30);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("The server will be brought down for maintenance in (30 Seconds). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in (30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
                Server.SaveDatabase();
                if (Server.FullLoading && !Program.ServerConfig.IsInterServer)
                {
                    foreach (var user in Pool.GamePoll.Values)
                    {
                        if (user.OnInterServer)
                            continue;
                        if ((user.ClientFlag & Client.ServerFlag.LoginFull) == Client.ServerFlag.LoginFull)
                        {
                            user.ClientFlag |= Client.ServerFlag.QueuesSave;
                            Database.ServerDatabase.LoginQueue.TryEnqueue(user);
                        }
                    }

                    MyConsole.WriteLine("All online clients have been saved successfully." );
                }
                if (Database.ServerDatabase.LoginQueue.Finish())
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            Thread.Sleep(1000 * 20);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                MyConsole.WriteLine("Server maintenance(few minutes). Please log off immediately to avoid data loss.");
                MsgMessage msg = new MsgMessage("Server maintenance(few minutes). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                Server.SendGlobalPacket(msg.GetArray(new ServerSockets.RecycledPacket().GetStream()));
            }
         
            Thread.Sleep(1000 * 10);
            ProcessConsoleEvent(0);
            new MySqlCommand(MySqlCommandType.UPDATE).Update("online").Set("OnlineCount", 0).Execute();
            Application.Restart();
            Environment.Exit(0);
            //DiscordAPI.Enqueue("Server maintenance(few minutes). Please log off immediately to avoid data loss.");
        }
        [STAThread]
        public static unsafe void Main(string[] args)
        {
            try
            {
                DisableConsoleQuickEdit.Go();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                ServerSockets.Packet.SealString = "TQServer";
                //MsgTQProtect.Load(true);
                handlerKeepAlive = ProcessConsoleEvent;
                SetConsoleCtrlHandler(handlerKeepAlive, true);
                Server.Initialize();
                Cryptography.DHKeyExchange.KeyExchange.CreateKeys();
                Cryptography.AuthCryptography.PrepareAuthCryptography();
                #region Open Sockets
                TransferCipher.Key = Encoding.ASCII.GetBytes("958KhLvYJ3zdLCTyz9Ak8RAgM78tY5F32b7CUXDuLDJDFBH8H67BWy9QThmaN5VS");
                TransferCipher.Salt = Encoding.ASCII.GetBytes("859VgBf3ytALHWLXbJxSUX4uFEu3Xmz2UAY9sTTm8AScB7Kk2uwqDSnuNJske4BJ");
                Pool.transferCipher = new TransferCipher("127.0.0.1"); if (Program.ServerConfig.IsInterServer == false)
                if (Program.ServerConfig.IsInterServer == false)
                {
                    VirusX = new ServerSockets.ServerSocket(
                    new Action<ServerSockets.SecuritySocket>(p => new Client.GameClient(p))
                  , Game_Receive, Game_Disconnect);
                    VirusX.Initilize(ServerConfig.Port_SendSize, ServerConfig.Port_ReceiveSize, 1, 3);
                    VirusX.Open(Program.ServerConfig.IPAddres, Program.ServerConfig.GamePort, Program.ServerConfig.Port_BackLog);
                    MyConsole.WriteLine("");
                    MyConsole.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
                    MyConsole.WriteLine($"║                         SERVER STARTED - PORT {Program.ServerConfig.GamePort}                            ║");
                    MyConsole.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");

                    }
                Database.NpcServer.LoadServerTraps();
                MsgInterServer.PipeServer.Initialize();
                ThreadPoll.Create();
                #endregion
                Server.FullLoading = true;
                SocketThread.Creates("ConquerSockets", VirusX, MsgInterServer.PipeServer.Server);
                MsgInterServer.StaticConnexion.Create();
                new MapGroupThread(100, "ConquerServer3").Start();             
                for (; ; )
                    ConsoleCMD(Console.ReadLine());


            }
            catch (Exception e) { MyConsole.WriteException(e); }

        }      
        public unsafe static void ConsoleCMD(string cmd)
        {
            try
            {
                string[] line = cmd.Split(' ');

                switch (line[0])
                {
                    case "Info":
                        {
                            var Info = new Panel();
                            Info.ShowDialog();
                            break;
                        }
                    //case "1":
                    //    {
                    //        Server.LoadMonstersTest();
                    //        break;
                    //    }
                    case "Mon":
                        {
                            Program.Spawn = new SpawnMonster();
                            Program.Spawn.ShowDialog();
                            break;
                        }
                    case "pac":
                        {
                            Program.packetsForm = new Packets();
                            Program.packetsForm.ShowDialog();
                            break;
                        }
                    #region Load
                    case "loadsobnpc":
                        {
                            NpcServer.LoadSobNpcs();
                            MyConsole.WriteLine("Loaded SobNpc Done");
                            break;
                        }
                  
                    case "loadnpc":
                        {
                            NpcServer.LoadNpcs();
                            MyConsole.WriteLine("Loaded Npc Done");
                            break;
                        }
                    #endregion
                    #region item
                    case "rss":
                        {
                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                client.MyNinja = new Role.Instance.Ninja(client);
                                Database.ServerDatabase.LoadClientItems(client);
                                var item = client.MyNinja.Items.Values.Where(p => p.ItemID == 600).FirstOrDefault();
                                if (item != null)
                                {

                                }
                            }
                            break;

                        }
                    case "return":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                ini.FileName = fname;
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                client.Player.Name = ini.ReadString("Character", "Name", "None");
                                Database.ServerDatabase.LoadClientItems(client);
                                foreach (var a in client.Warehouse.ClientItems)
                                {
                                    foreach (var b in a.Value.Values)
                                    {
                                        if (b.WH_ID == 65535)
                                        {
                                            if (b.StackSize > 0)
                                                b.StackSize = 0;
                                            client.Warehouse.ClientItems.Remove(b.UID);
                                            MyConsole.WriteLine("Done Restart Items");
                                        }

                                    }
                                }
                                Database.ServerDatabase.SaveClientItems(client);
                            }
                            break;
                        }
                    case "rperf":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                ini.FileName = fname;
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                client.Player.Name = ini.ReadString("Character", "Name", "None");
                                Database.ServerDatabase.LoadClientItems(client);
                                var list = client.Equipment.ClientItems.Values.ToList();
                                foreach (var item in list)
                                {
                                    if (item.PerfectionLevel >= 27)
                                        item.PerfectionLevel = 0;
                                }
                                foreach (var a in client.Warehouse.ClientItems)
                                {
                                    foreach (var b in a.Value.Values)
                                    {
                                        if (b.PerfectionLevel >= 27)
                                            b.PerfectionLevel = 0;
                                    }
                                }
                                var listr = client.Inventory.ClientItems.Values.ToList();
                                foreach (var item in listr)
                                {
                                    if (item.PerfectionLevel >= 27)
                                        item.PerfectionLevel = 0;
                            
                                }
                                Database.ServerDatabase.SaveClientItems(client);
                                MyConsole.WriteLine("Done Restart Items");
                            }
                            break;
                        }
                    case "rPlus":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                ini.FileName = fname;
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                client.Player.Name = ini.ReadString("Character", "Name", "None");
                                Database.ServerDatabase.LoadClientItems(client);
                                foreach (var a in client.Warehouse.ClientItems)
                                {
                                    foreach (var b in a.Value.Values)
                                    {
                                        if (b.Bound > 0)
                                        {
                                            if (b.Plus == 8)
                                                b.Plus = 0;
                                        }
                                       
                                    }
                                }
                                var listr = client.Inventory.ClientItems.Values.ToList();
                                foreach (var item in listr)
                                {
                                    if (item.Bound > 0)
                                    {
                                        if (item.Plus == 8)
                                            item.Plus = 0;
                                    }

                                }
                                Database.ServerDatabase.SaveClientItems(client);
                                MyConsole.WriteLine("Done Restart Items");
                            }
                            break;
                        }
                    case "rPokerCard":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                ini.FileName = fname;
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                client.Player.Name = ini.ReadString("Character", "Name", "None");
                                Database.ServerDatabase.LoadClientItems(client);
                                foreach (var a in client.Warehouse.ClientItems)
                                {
                                    foreach (var b in a.Value.Values)
                                    {
                                        if (b.ITEM_ID >= 3390072 && b.ITEM_ID <= 3390084)
                                            client.Warehouse.ClientItems.Remove(b.UID);
                                    }
                                }
                                var listr = client.Inventory.ClientItems.Values.ToList();
                                foreach (var item in listr)
                                {
                                    if (item.ITEM_ID >= 3390072 && item.ITEM_ID <= 3390084)
                                        client.Inventory.ClientItems.Remove(item.UID);
                                }
                                Database.ServerDatabase.SaveClientItems(client);
                                MyConsole.WriteLine("Done Restart Items");
                            }
                            break;
                        }
                    case "removeAnima":
                        {
                            string Name = "";
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;
                                Name = ini.ReadString("Character", "Name", "None");
                            }

                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                client.Player.Name = Name;
                                Database.ServerDatabase.LoadClientItems(client);
                                var list = client.Equipment.ClientItems.Values.ToList();
                                foreach (var item in list)
                                {
                                    if (item.AnimaItemID >= 4200012)
                                    {
                                        Game.MsgServer.MsgGameItem ItemDat = new Game.MsgServer.MsgGameItem();
                                        ItemDat.UID = Pool.ITEM_Counter.Next;
                                        item.AnimaItemID = 0;
                                        Game.MsgServer.MsgGameItem ItemDatS = new Game.MsgServer.MsgGameItem();
                                        ItemDatS.UID = Pool.ITEM_Counter.Next;
                                        ItemDatS.ITEM_ID = item.AnimaItemID;
                                        ItemDatS.Mode = Role.Flags.ItemMode.AddItemReturned;
                                        ItemDatS.WH_ID = ushort.MaxValue;
                                        MyConsole.WriteLine("Done Restart Items " + clientUID + "");

                                    }
                                }
                                //foreach (var a in client.Warehouse.ClientItems)
                                //{
                                //    foreach (var b in a.Value.Values)
                                //    {
                                //        if (b.ITEM_ID >= 4200001 && b.ITEM_ID <= 4200018)
                                //        {
                                //            b.Position = 255;
                                //            b.WH_ID = 0;
                                //            client.Warehouse.ClientItems.Remove(b.UID);
                                //        }
                                //        if (b.AnimaItemID >= 4200001 && b.AnimaItemID <= 4200018)
                                //        {
                                //            b.AnimaItemID = 0;
                                //        }
                                //        MyConsole.WriteLine("Done Restart Items" + clientUID + "");

                                //    }
                                //}
                                //var listr = client.Inventory.ClientItems.Values.ToList();
                                //foreach (var item in listr)
                                //{
                                //    if (item.ITEM_ID >= 4200001 && item.ITEM_ID <= 4200018)
                                //    {
                                //        client.Inventory.ClientItems.Remove(item.UID);
                                //    }
                                //    if (item.AnimaItemID >= 4200001 && item.AnimaItemID <= 4200018)
                                //        item.AnimaItemID = 0;
                                //    MyConsole.WriteLine("Done Restart Items" + clientUID + "");

                                //}
                                Database.ServerDatabase.SaveClientItems(client);
                            }
                            break;
                        }
                    case "removeMythS":
                        {
                            string Name = "";
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;
                                Name = ini.ReadString("Character", "Name", "None");
                            }

                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                client.Player.Name = Name;
                                Database.ServerDatabase.LoadClientItems(client);
                                var list = client.Equipment.ClientItems.Values.ToList();
                                foreach (var item in list)
                                {
                                    if (item.MythSoulID > 0)
                                    {
                                        Game.MsgServer.MsgGameItem ItemDat = new Game.MsgServer.MsgGameItem();
                                        item.MythSoulID = 0;
                                        item.Mutacion = 0;
                                        item.MythsoulEffect = 0;
                                    }
                                }
                                foreach (var a in client.Warehouse.ClientItems)
                                {
                                    foreach (var b in a.Value.Values)
                                    {
                                        if (b.MythSoulID > 0)
                                        {
                                            b.MythSoulID = 0;
                                            b.Mutacion = 0;
                                            b.MythsoulEffect = 0;
                                        }
                                    }
                                }
                                var listr = client.Inventory.ClientItems.Values.ToList();
                                foreach (var item in listr)
                                {
                                    if (item.MythSoulID > 0)
                                    {
                                        item.MythSoulID = 0;
                                        item.Mutacion = 0;
                                        item.MythsoulEffect = 0;
                                    }
                                }
                                Database.ServerDatabase.SaveClientItems(client);
                                MyConsole.WriteLine("Done Restart Items");
                            }
                            break;
                        }
                    case "ra":
                        {
                            WindowsAPI.BinaryFile binary = new WindowsAPI.BinaryFile();
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\PlayersItems\\"))
                            {
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".bin", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                client.Inventory = new Role.Instance.Inventory(client);
                                client.Equipment = new Role.Instance.Equip(client);
                                client.Warehouse = new Role.Instance.Warehouse(client);
                                client.MyWardrobe = new Role.Instance.Wardrobe(client);
                                client.Rune = new Role.Instance.Rune(client);
                                Database.ServerDatabase.LoadClientItems(client);
                                
                                foreach (var a in client.Warehouse.ClientItems)
                                {
                                    foreach (var b in a.Value.Values)
                                    {
                                        if (b.PerfectionLevel > 0)
                                            b.PerfectionLevel = 0;
                                     
                                    }
                                }
                                var list = client.Inventory.ClientItems.Values.ToList();
                                foreach (var item in list)
                                {
                                    if (item.PerfectionLevel > 0)
                                        item.PerfectionLevel = 0;
                                    if (item.OwnerName != string.Empty)
                                        client.Inventory.ClientItems.Remove(item.UID);
                                }
                                Database.ServerDatabase.SaveClientItems(client);
                                MyConsole.WriteLine("Done Restart Items" );
                            }
                            break;

                        }
                    #endregion
                 
                    #region commandsConsle
                    case "exit":
                        {
                            ProcessConsoleEvent(0);
                            Environment.Exit(0);
                            break;
                        }
                    case "down":
                        {
                            new Thread(new ThreadStart(Maintenance)).Start();
                            break;
                        }
                    case "save":
                        {
                            Server.SaveDatabase();
                            if (Server.FullLoading && !Program.ServerConfig.IsInterServer)
                            {
                                foreach (var user in Pool.GamePoll.Values)
                                {
                                    if (user.Fake || user.OnInterServer || user.Socket == null || !user.Socket.Alive)
                                        continue;
                                    if ((user.ClientFlag & Client.ServerFlag.LoginFull) == Client.ServerFlag.LoginFull)
                                    {
                                        user.ClientFlag |= Client.ServerFlag.QueuesSave;
                                        Database.ServerDatabase.LoginQueue.TryEnqueue(user);
                                    }
                                }
                                Server.SaveDatabase();
                                MyConsole.WriteLine("Database Has Been Saved Successfully ." );
                            }
                            if (Database.ServerDatabase.LoginQueue.Finish())
                            {
                                System.Threading.Thread.Sleep(1000);
                                MyConsole.WriteLine("Database Saved Successfully .");
                            }
                            break;
                        }
                    case "kick":
                        {

                            foreach (var user in Pool.GamePoll.Values)
                            {
                                if (user.Player.Name.Contains(line[1]))
                                {
                                    user.EndQualifier();
                                }
                            }
                            break;
                        }
                    #endregion
                    #region Panels
                    case "packets":
                        {
                            Program.packetsForm = new Packets();
                            Program.packetsForm.ShowDialog();
                            break;
                        }
                    case "chi":
                        {
                            Chi cp = new Chi();
                            cp.ShowDialog();
                            break;
                        }
                        
                  case "10":
                        {
                            Mahmoud cp = new Mahmoud();
                            cp.ShowDialog();
                            break;
                        }
                    case "jiang":
                        {
                            JiangHu cp = new JiangHu();
                            cp.ShowDialog();
                            break;
                        }
                    case "clear":
                        {
                            Console.Clear();
                            break;
                        }
                    case "cp":
                        {
                            Controlpanel cp = new Controlpanel();
                            cp.ShowDialog();
                            break;
                        }
                    case "USD":
                        {
                            PanelCharge cp = new PanelCharge();
                            cp.ShowDialog();
                            break;
                        }

                    case "Acc":
                        {
                            AccountsForm cp = new AccountsForm();
                            cp.ShowDialog();
                            break;

                        }
                    case "cp1":
                        {
                            NightMareFo cp = new NightMareFo();
                            cp.ShowDialog();
                            break;
                        }
                    case "ChatNew":
                        {
                            ChatPanal cp = new ChatPanal();
                            cp.ShowDialog();
                            break;
                        }
                    #endregion
                    #region Events
                    case "hunts":
                        {
                            Game.MsgTournaments.MsgSchedules.SquidWardOctopus.Start();
                            break;
                        }
                    case "MsgEmperorWar":
                        {
                            Game.MsgTournaments.MsgSchedules.EmperorWar.Start();
                            break;
                        }
                    case "EndEmperorWar":
                        {
                            Game.MsgTournaments.MsgSchedules.EmperorWar.End();
                            break;
                        }

                        
                    case "ChampionsOfWar":
                        {
                    Game.MsgTournaments.MsgSchedules.ChampionsOfWarr.Start();
                    break;
                }
                    case "ChampionsOfWarrEnd":
                        {
                    Game.MsgTournaments.MsgSchedules.ChampionsOfWarr.End();
                    break;
                }

                    case "UnionWar":
                        {
                            Game.MsgTournaments.MsgSchedules.UnionWar.Start();
                            break;
                        }
                    case "UnionWarEnd":
                        {
                            Game.MsgTournaments.MsgSchedules.UnionWar.FinishRound();
                            break;
                        }

                    /////
                    case "ScoreGuildWar":
                        {
                            Game.MsgTournaments.MsgSchedules.TopWarScore.Start();
                            break;
                        }
                    case "ScoreGuildWarEnd":
                        {
                            Game.MsgTournaments.MsgSchedules.TopWarScore.End();
                            break;
                        }
                    case "Guild6PoleWar6":
                        {
                    Game.MsgTournaments.MsgSchedules.Guild6PoleWar6.Start();
                    break;
                }
                    case "Guild6PoleWarend6":
                        {
                    Game.MsgTournaments.MsgSchedules.Guild6PoleWar6.End();
                    break;
                }
                   
                    case "WarOfPlayers":
                        {
                    Game.MsgTournaments.MsgWarOfPlayers.Start();

                    break;
                }
                    case "WarOfPlayersEnd":
                        {
                    Game.MsgTournaments.MsgWarOfPlayers.End();

                    break;
                }
               
                    case "eliteEnd":
                        {
                    Game.MsgTournaments.MsgSchedules.EliteGuildWar.End();
                    break;
                }
                    case "StartSuper":
                        {
                    Game.MsgTournaments.MsgSchedules.SuperGuildWar.Start();
                    break;
                }
                    case "finishsSuper":
                        {
                    Game.MsgTournaments.MsgSchedules.SuperGuildWar.Proces = Game.MsgTournaments.ProcesType.Dead;
                    Game.MsgTournaments.MsgSchedules.SuperGuildWar.CompleteEndGuildWar();
                    break;
                }
               
                    case "5555":
                        {
                    Game.MsgTournaments.MsgSchedules.CpsCastle.Start();
                    break;
                }
                    case "55555":
                        {
                    Game.MsgTournaments.MsgSchedules.CpsCastle.Finish();
                    break;
                }
                  
                   



                    case "cpscastle":
                        {
                            Game.MsgTournaments.MsgSchedules.CpsCastle.Start();
                            break;
                        }

                    case "Clan":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanWar.Open();
                            break;
                        }



                    case "ctfon":
                        {
                            Game.MsgTournaments.MsgSchedules.CaptureTheFlag.Start();
                            break;
                        }
                    case "ctfonCheckFinish":
                        {
                            Game.MsgTournaments.MsgSchedules.CaptureTheFlag.CheckFinish();
                            break;
                        }
                    case "pk":
                        {
                            Game.MsgTournaments.MsgSchedules.ElitePkTournament.Start();

                            break;
                        }
                    case "teampk":
                        {
                            Game.MsgTournaments.MsgSchedules.SkillTeamPkTournament.Start();


                           
                            break;
                        }
                    case "teampk2":
                        {
                            Game.MsgTournaments.MsgSchedules.TeamPkTournament.Start();


                           
                            break;

                        }
                    case "gw":
                        {
                            Game.MsgTournaments.MsgSchedules.GuildWar.Proces = Game.MsgTournaments.ProcesType.Alive;
                            Game.MsgTournaments.MsgSchedules.GuildWar.Start();
                            break;
                        }
                    case "finishgw":
                        {
                            Game.MsgTournaments.MsgSchedules.GuildWar.Proces = Game.MsgTournaments.ProcesType.Dead;
                            Game.MsgTournaments.MsgSchedules.GuildWar.CompleteEndGuildWar();
                            break;
                        }
                    case "sgw":
                        {
                            Game.MsgTournaments.MsgSchedules.SuperGuildWar.Start();
                            break;
                        }
                    case "offsgw":
                        {
                            Game.MsgTournaments.MsgSchedules.SuperGuildWar.Proces = Game.MsgTournaments.ProcesType.Dead;
                            Game.MsgTournaments.MsgSchedules.SuperGuildWar.CompleteEndGuildWar();
                            break;
                        }

                    case "elite":
                        {
                            Game.MsgTournaments.MsgSchedules.EliteGuildWar.Start();
                            break;
                        }
                    case "Strike":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");

                                foreach (string fmap in System.IO.Directory.GetDirectories(Program.ServerConfig.DbLocation + "\\Test\\"))
                                {
                                    foreach (string fmobtype in System.IO.Directory.GetDirectories(fmap))
                                    {
                                        foreach (string ffile in System.IO.Directory.GetFiles(fmobtype))
                                        {
                                            ini.FileName = ffile;
                                            uint Price = 1;
                                            var runes = Pool.ItemsBase.Values.Where((p => Database.ItemType.ItemPosition(p.ID) == (ushort)Role.Flags.ConquerItem.Garment || Database.ItemType.ItemPosition(p.ID) == (ushort)Role.Flags.ConquerItem.SteedMount)).ToArray();
                                            for (var i = 0; i < runes.Length; i++)
                                            {
                                                if (runes[i].Imunity > 0 || runes[i].Crytical > 0 || runes[i].SCrytical > 0 || runes[i].ItemHP > 0)
                                                    ini.WriteString("cq_generator", "Item" + i + "", "" + runes[i].ID.ToString() + "@@" + Price + "@@0@@0@0@0@0@@1");
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "Garments":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");

                                foreach (string fmap in System.IO.Directory.GetDirectories(Program.ServerConfig.DbLocation + "\\Test\\"))
                                {
                                    foreach (string fmobtype in System.IO.Directory.GetDirectories(fmap))
                                    {
                                        foreach (string ffile in System.IO.Directory.GetFiles(fmobtype))
                                        {
                                            ini.FileName = ffile;
                                            uint Price = 1;
                                            var runes = Database.CoatStorage.GarmentsBig5Star.Values.ToArray();
                                            for (var i = 0; i < runes.Length; i++)
                                            {
                                                ini.WriteString("cq_generator", "Item" + i + "", "" + runes[i].ID.ToString() + "@@" + Price + "@@0@@0@0@0@0@@1");
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "Mounts":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");

                                foreach (string fmap in System.IO.Directory.GetDirectories(Program.ServerConfig.DbLocation + "\\Test\\"))
                                {
                                    foreach (string fmobtype in System.IO.Directory.GetDirectories(fmap))
                                    {
                                        foreach (string ffile in System.IO.Directory.GetFiles(fmobtype))
                                        {
                                            ini.FileName = ffile;
                                            uint Price = 1;
                                            var runes = Database.CoatStorage.MountsBig5Star.Values.ToArray();
                                            for (var i = 0; i < runes.Length; i++)
                                            {
                                                ini.WriteString("cq_generator", "Item" + i + "", "" + runes[i].ID.ToString() + "@@" + Price + "@@0@@0@0@0@0@@1");
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "RuneY":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");

                                foreach (string fmap in System.IO.Directory.GetDirectories(Program.ServerConfig.DbLocation + "\\Test\\"))
                                {
                                    foreach (string fmobtype in System.IO.Directory.GetDirectories(fmap))
                                    {
                                        foreach (string ffile in System.IO.Directory.GetFiles(fmobtype))
                                        {
                                            ini.FileName = ffile;
                                            uint Price = 1;
                                            var runes = Pool.ItemsBase.Values.Where((p => p.ID >= 4035009 && p.ID <= 4070009)).ToArray();
                                            for (var i = 0; i < runes.Length; i++)
                                            {
                                                if (runes[i].ID % 10 == 9)
                                                    ini.WriteString("cq_generator", "Item" + i + "", "" + runes[i].ID.ToString() + "@@" + Price + "@@0@@0@0@0@0@@1");
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "Clan1":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanTwin.Start();
                            break;
                        }
                    case "Clan2":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanPhoenix.Start();
                            break;
                        }

                    case "Clan3":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanApe.Start();
                            break;
                        }
                    case "Clan4":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanDesert.Start();
                            break;
                        }
                    case "Clan5":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanBird.Start();
                            break;
                        }
                    case "Clans1":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanTwin.End();
                            break;
                        }
                    case "Clans2":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanPhoenix.End();
                            break;
                        }
                    case "Clans3":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanApe.End();
                            break;
                        }
                    case "Clans4":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanDesert.End();
                            break;
                        }
                    case "Clans5":
                        {
                            Game.MsgTournaments.MsgSchedules.ClanBird.End();
                            break;
                        }
                    #endregion
                    #region Commands
                    case "search":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                string Name = ini.ReadString("Character", "Name", "None");
                                if (Name.ToLower() == line[1].ToLower() || Name.Contains(line[1]))
                                {
                                    Console.WriteLine(ini.ReadUInt32("Character", "UID", 0));
                                    break;
                                }

                            }
                            break;
                        }
                    case "resetavatar":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                string Name = ini.ReadString("Character", "Name", "None");
                                ini.WriteString("Character", "HairfaceStorage", "0");
                            }

                            break;
                        }
                    case "resetnobility":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong nobility = ini.ReadUInt64("Character", "DonationNobility", 0);
                                nobility = 0;
                                ini.Write<ulong>("Character", "DonationNobility", nobility);
                                MyConsole.WriteLine("Done Restart DonationNobility");
                            }
                           
                            break;
                        }
                    case "ResstLevel":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ushort Level = ini.ReadUInt16("Character", "Level", 0);
                                ushort Level1 = ini.ReadUInt16("Character", "FRL", 0);
                                ushort Level2 = ini.ReadUInt16("Character", "SRL", 0);
                                if (Level > 129 || Level1 > 129 || Level2 > 129)
                                {
                                    ini.Write<ulong>("Character", "Level", 129);
                                    ini.Write<ulong>("Character", "FRL", 129);
                                    ini.Write<ulong>("Character", "SRL", 129);
                                }
                                MyConsole.WriteLine("Done Restart Level");
                            }

                            break;
                        }
                    case "RestFlower":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                string Flowers = ini.ReadBigString("Character", "Flowers", "");
                                Flowers = "";
                                ini.WriteString("Character", "Flowers", Flowers);
                                MyConsole.WriteLine("Done Restart Flowers");
                            }

                            break;
                        }
                    case "restmoney":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong Money = ini.ReadUInt64("Character", "Money", 0);
                                Money = 0;
                                ini.Write<ulong>("Character", "Money", Money);
                                ini.Write<ulong>("Character", "WHMoney", Money);

                            }
                            MyConsole.WriteLine("Done Restart Money");
                            break;
                        }
                    case "restcps":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong ConquerPoints = ini.ReadUInt64("Character", "ConquerPoints", 0);
                                ConquerPoints = 0;
                                ini.Write<ulong>("Character", "ConquerPoints", ConquerPoints);
                               
                            }
                            MyConsole.WriteLine("Done Restart CPS" );
                            break;
                        }
                    case "Any":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;
                                ini.WriteString("Character", "Flowers", "");

                            }
                            MyConsole.WriteLine("Done Flowers");
                            break;
                        }
                    case "rp":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                uint clientUID = uint.Parse(System.IO.Path.GetFileName(fname).Replace(".ini", ""));
                                Client.GameClient client = new Client.GameClient(null);
                                client.Player.UID = clientUID;
                                Database.ServerDatabase.LoadCharacter(client, (uint)clientUID);
                                if (client.MyArchives.UniversalFragment > 0)
                                {
                                    client.MyArchives.UniversalFragment = 0;
                                    Database.ServerDatabase.SaveClient(client);
                                    Console.WriteLine("Done Pirate Rest !");
                                }
                            }
                            break;
                        }
                    case "rop":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong OnlineMinutes = ini.ReadUInt64("Character", "OnlineMinutes", 0);
                                OnlineMinutes = 0;
                                ini.Write<ulong>("Character", "OnlineMinutes", OnlineMinutes);
                                MyConsole.WriteLine("Done Restart CPS" );
                            }

                            break;
                        }
                    case "bancount":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong BanCount = ini.ReadUInt64("Character", "BanCount", 0);
                                BanCount = 0;
                                ini.Write<ulong>("Character", "BanCount", BanCount);
                                MyConsole.WriteLine("Done !" );
                            }

                            break;
                        }
                   
                    case "restvip":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;
                                ini.Write<ulong>("Character", "VipLevel", 1);
                                ini.Write<long>("Character", "VipTime", 0);
                                MyConsole.WriteLine("Done Restart Money" );
                            }

                            break;
                        }
                    case "wh":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;


                                ini.Write<int>("Character", "CpsBank", 0);
                                MyConsole.WriteLine("Done Restart Money" );
                            }

                            break;
                        }
                    case "restartchi":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong Dragon = ini.ReadUInt64("Character", "Dragon", 0);
                                ulong Pheonix = ini.ReadUInt64("Character", "Pheonix", 0);
                                ulong Turtle = ini.ReadUInt64("Character", "Turtle", 0);
                                ulong Tiger = ini.ReadUInt64("Character", "Tiger", 0);
                                Dragon = 0;
                                Pheonix = 0;
                                Turtle = 0;
                                Tiger = 0;
                                ini.Write<ulong>("Character", "Dragon", Dragon);
                                ini.Write<ulong>("Character", "Dragon", Dragon);
                                ini.Write<ulong>("Character", "Dragon", Dragon);
                                ini.Write<ulong>("Character", "Dragon", Dragon);
                                MyConsole.WriteLine("Done Restart chi");
                            }

                            break;
                        }
                    case "twin":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong Map = ini.ReadUInt64("Character", "Map", 0);
                                Map = 1002;
                                ini.Write<ulong>("Character", "Map", Map);
                                ini.Write<ulong>("Character", "X", 410);
                                ini.Write<ulong>("Character", "Y", 354);
                                MyConsole.WriteLine("Done Restart Map" );
                            }
                            break;
                        }
                    case "restartnobility":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong nobility = ini.ReadUInt64("Character", "DonationNobility", 0);
                                ulong LastNobilityDonation = ini.ReadUInt64("Character", "LastNobilityDonation", 0);
                                nobility = nobility * 0 / 100;
                                LastNobilityDonation = 0;
                                ini.Write<ulong>("Character", "DonationNobility", nobility);

                                ini.Write<ulong>("Character", "LastNobilityDonation", LastNobilityDonation);
                                MyConsole.WriteLine("Done Restart Donation" );
                            }

                            break;
                        }
                    case "sendvip":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;
                                byte LevelVIP = ini.ReadByte("Character", "VipLevel", 0);
                                if (LevelVIP < 4 && LevelVIP != 6 && LevelVIP != 7)
                                {
                                    ini.Write<byte>("Character", "VipLevel", 4);
                                      System.DateTime Timeeee = DateTime.FromBinary(ini.ReadInt64("Character", "VipTime", 0));
                                Timeeee = Timeeee.AddDays(1);
                                ini.Write<long>("Character", "VipTime", Timeeee.Ticks);
                                MyConsole.WriteLine("Done Give Vip" + fname + "");
                                }
                              
                              
                            }
                            break;
                        }
                    case "restartnobility2":
                        {
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                ulong LastNobilityDonation = ini.ReadUInt64("Character", "LastNobilityDonation", 0);
                                LastNobilityDonation = LastNobilityDonation * 0 / 100;

                                ini.Write<ulong>("Character", "LastNobilityDonation", LastNobilityDonation);
                                MyConsole.WriteLine("Done Restart Donation" );
                            }

                            break;
                        }

                    case "restart":
                        {
                            ProcessConsoleEvent(0);

                            System.Diagnostics.Process hproces = new System.Diagnostics.Process();
                            hproces.StartInfo.FileName = "VirusX.exe";
                            hproces.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                            hproces.Start();

                            Environment.Exit(0);

                            break;
                        }
                    #endregion
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
        public static List<uint> PacketsReceiveOnPoker = new List<uint>() { 2082, 2182, 2195, 2216, 2230, 2332, 2458, 2459, 2040, 2080, 2152, 2169, 2172, 2217, 2256, 2262, 2359, 2401, 2414, 2424, 2046, 2292, 2427, 2361 };//FixAllBugPoker
        public unsafe static void Game_Receive(ServerSockets.SecuritySocket obj, ServerSockets.Packet stream)
        {
            if (!obj.SetDHKey)
                CreateDHKey(obj, stream);
            else
            {
                try
                {
                    if (obj.Game == null)
                        return;
                    ushort PacketID = stream.ReadUInt16();
                    if (obj.Game.Player.CheckTransfer)
                        goto jmp;
                    if (obj.Game.PipeClient != null && PacketID != Game.GamePackets.MsgAchievement)
                    {
                        if (PacketID == (ushort)Game.GamePackets.MsgOsShop
             || PacketID == (ushort)Game.GamePackets.Msg2ndPsw
                  || PacketID == (ushort)Game.GamePackets.MsgLeagueOpt
                  || PacketID == (ushort)Game.GamePackets.MsgLeagueConcubines
                  || PacketID == (ushort)Game.GamePackets.MsgLeagueAllegianceList
                      || PacketID == (ushort)Game.GamePackets.MsgTokenUpdate
                          || PacketID == (ushort)Game.GamePackets.MsgLeagueImperialCourtList
                              || PacketID == (ushort)Game.GamePackets.MsgLeagueInfo
                                  || PacketID == (ushort)Game.GamePackets.MsgOverheadLeagueInfo
                                      || PacketID == (ushort)Game.GamePackets.MsgLeagueMemList
                                          || PacketID == (ushort)Game.GamePackets.MsgLeaguePalaceGuardsList
                                             || PacketID == (ushort)Game.GamePackets.MsgLeagueRank
                                                || PacketID == (ushort)Game.GamePackets.MsgLeagueSynList
                                                   || PacketID == (ushort)Game.GamePackets.MsgLeagueToken
                      || PacketID == (ushort)Game.GamePackets.MsgLeagueRobOpt)
                            goto jmp;

                        stream.Seek(stream.Size);
                        obj.Game.PipeClient.Send(stream);

                        if (PacketID != Game.GamePackets.MsgItem)
                        {

                            return;
                        }
                        stream.Seek(4);
                    }

#if TEST
                    MyConsole.WriteLine("Receive -> PacketID: " + PacketID);
#endif

                    Database.ServerDatabase.LoginQueue.Enqueue("[CallStack]" + MyConsole.log1(obj.Game.Player.Name, stream.Memory, stream.Size));
                jmp:

                    Action<Client.GameClient, ServerSockets.Packet> hinvoker;
                    if (Pool.MsgInvoker.TryGetInvoker(PacketID, out hinvoker))
                    {
                        hinvoker(obj.Game, stream);
                    }
                }
                catch (Exception e) { MyConsole.WriteException(e); }
                finally
                {
                    ServerSockets.PacketRecycle.Reuse(stream);
                }
            }

        }
        public static List<ushort> DHBlockPacket = new List<ushort>();
        public static bool isblockeddhkey = false;
        public unsafe static void CreateDHKey(ServerSockets.SecuritySocket obj, ServerSockets.Packet Stream)
        {
            try
            {
                bool IsBlock = false;
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var streamssss = rec.GetStream();
                    var BlockStream = rec.GetStream();
                    BlockStream = Stream;
                    streamssss = Stream;
                    ushort id = streamssss.ReadUInt16();
                    if (id == 2562)
                    {
                        return;
                    }

                    byte[] buffer = new byte[60];
                    bool extra = false;
                    string text = System.Text.ASCIIEncoding.ASCII.GetString(obj.DHKeyBuffer.buffer, 0, obj.DHKeyBuffer.Length());
                    if (!text.EndsWith("TQClient"))
                    {
                        System.Buffer.BlockCopy(obj.EncryptedDHKeyBuffer.buffer, obj.EncryptedDHKeyBuffer.Length() - 60, buffer, 0, 60);
                        extra = true;
                    }
                    if (DHBlockPacket.Contains(id))
                    {
                        IsBlock = true;
                    }
                    string key;
                    if (isblockeddhkey ? BlockStream.GetHandshakeReplyKey(IsBlock, out key) : Stream.GetHandshakeReplyKey(IsBlock, out key))
                    {
                        obj.SetDHKey = true;
                        obj.Game.DHKey.HandleResponse(key);
                        var compute_key = obj.Game.DHKeyExchance.PostProcessDHKey(obj.Game.DHKey.ToBytes());
                        obj.Game.Crypto.GenerateKey(compute_key);
                        obj.Game.Crypto.Reset();
                        if (isblockeddhkey)
                            isblockeddhkey = false;
                    }
                    else
                    {
                        DHBlockPacket.Add(id);
                        Console.WriteLine("New Dh Key Block Packet Is Add =>>>> [" + id + "]");
                        isblockeddhkey = true;
                        obj.Disconnect();
                        return;
                    }
                    if (extra)
                    {
                        Stream.Seek(0);
                        obj.Game.Crypto.Decrypt(buffer, 0, Stream.Memory, 0, 60);
                        Stream.Size = buffer.Length;
                        Stream.Seek(2);
                        ushort PacketID = Stream.ReadUInt16();
                        if (PacketID == 2429)
                            return;
                        Action<Client.GameClient, ServerSockets.Packet> hinvoker;
                        if (Pool.MsgInvoker.TryGetInvoker(PacketID, out hinvoker))
                        {
                            hinvoker(obj.Game, Stream);
                        }
                        else
                        {
                            obj.Disconnect();
                            MyConsole.WriteLine("DH KEY Not found the packet ----> " + PacketID);
                        }
                    }
                }
            }
            catch (Exception e) { MyConsole.WriteException(e); }
        }
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
            CopyAll(diSource, diTarget);
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        public unsafe static void Game_Disconnect(ServerSockets.SecuritySocket obj)
        {
            if (obj.Game != null && obj.Game.Player != null)
            {
                try
                {
                    Client.GameClient client;
                    if (Pool.GamePoll.TryGetValue(obj.Game.Player.UID, out client))
                    {
                        Pool.DisconnectPool.TryAdd(client.Player.UID, client);
                       
                        if (client.OnInterServer)
                            return;
                        if ((client.ClientFlag & Client.ServerFlag.LoginFull) == Client.ServerFlag.LoginFull)
                        {
                            if (obj.Game.PipeClient != null)
                                obj.Game.PipeClient.Disconnect();
                            obj.Game.Player.MapGuildWar = false;
                            MsgSchedules.GuildWar.RemoveUSER(obj.Game);
                            
                            client.LoginIP = client.Socket.RemoteIp;
                            client.LoginMAC = client.OnLogin.MacAddress;
                            MyConsole.WriteLine($"[-] {client.Player.Name} logged out | {client.LoginIP} | {client.LoginMAC}", ConsoleColor.Red);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                try
                                {
                                    if (client.Player.InUnion)
                                    {
                                        client.Player.UnionMemeber.Owner = null;
                                    }
                                    client.Player.ClearFloor();
                                    client.CheckRouletteDisconnect();
                                    client.EndQualifier();
                                    if (client.Team != null)
                                        client.Team.Remove(client, true);
                                    if (client.Player.MyClanMember != null)
                                        client.Player.MyClanMember.Online = false;
                                    if (client.IsVendor)
                                        client.MyVendor.StopVending(stream);
                                    if (client.InTrade)
                                        client.MyTrade.CloseTrade(new MsgTrade.Trade() { Action = 5 });
                                    if (client.Player.MyGuildMember != null)
                                        client.Player.MyGuildMember.IsOnline = false;

                                    if (client.Player.ObjInteraction != null)
                                    {
                                        client.Player.InteractionEffect.AtkType = (ushort)Game.MsgServer.MsgAttackPacket.AttackID.InteractionStopEffect;

                                        InteractQuery action = InteractQuery.ShallowCopy(client.Player.InteractionEffect);

                                        client.Send(stream.InteractionCreate(action));

                                        client.Player.ObjInteraction.Player.OnInteractionEffect = false;
                                        client.Player.ObjInteraction.Player.ObjInteraction = null;
                                    }


                                    client.Player.View.Clear(stream);


                                }
                                catch (Exception e)
                                {
                                    MyConsole.WriteException(e);
                                    client.Player.View.Clear(stream);
                                }
                                finally
                                {
                                    client.ClientFlag &= ~Client.ServerFlag.LoginFull;
                                    client.ClientFlag |= Client.ServerFlag.Disconnect;
                                    client.ClientFlag |= Client.ServerFlag.QueuesSave;
                                    Database.ServerDatabase.LoginQueue.TryEnqueue(client);
                                }

                                try
                                {
                                    client.Player.Associate.OnDisconnect(stream, client);


                                    if (client.Player.MyMentor != null)
                                    {
                                        Client.GameClient me;
                                        client.Player.MyMentor.OnlineApprentice.TryRemove(client.Player.UID, out me);
                                        client.Player.MyMentor = null;
                                    }
                                    client.Player.Associate.Online = false;
                                    lock (client.Player.Associate.MyClient)
                                        client.Player.Associate.MyClient = null;
                                    foreach (var clien in client.Player.Associate.OnlineApprentice.Values)
                                        clien.Player.SetMentorBattlePowers(0, 0);
                                    client.Player.Associate.OnlineApprentice.Clear();

                                }
                                catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
                            }
                        }
                    }
                }
                catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
            }
            else if (obj.Game != null)
            {
                if (obj.Game.ConnectionUID != 0)
                {
                    Client.GameClient client;
                    Pool.GamePoll.TryRemove(obj.Game.ConnectionUID, out client);
                }
            }
        }
    }
}