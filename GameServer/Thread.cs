using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VirusX.ServerSockets;
using VirusX.Game.MsgTournaments;
using System.IO;
using VirusX.Threading;
using System.Data;
using VirusX.Game.MsgFloorItem;
using VirusX.Game.MsgServer;
using VirusX.Game.MsgServer.AttackHandler;
using VirusX.Client;
namespace VirusX
{
    public static class ThreadPoll
    {
        #region Funcs
        public static void Execute(Action action, uint timeOut = 0, ThreadPriority priority = ThreadPriority.Normal)
        {
            GenericThreadPool.Subscribe(new VirusX.Threading.LazyDelegate(action, timeOut, priority));
        }
        public static void Execute<T>(Action<T> action, T param, uint timeOut = 0, ThreadPriority priority = ThreadPriority.Normal)
        {
            GenericThreadPool.Subscribe<T>(new VirusX.Threading.Generic.LazyDelegate<T>(action, timeOut, priority), param);
        }
        public static IDisposable Subscribe(Action action, uint period = 1, ThreadPriority priority = ThreadPriority.Normal)
        {
            return GenericThreadPool.Subscribe(new VirusX.Threading.TimerRule(action, period, priority));
        }
        public static IDisposable Subscribe<T>(Action<T> action, T param, uint timeOut = 0, ThreadPriority priority = ThreadPriority.Normal)
        {
            return GenericThreadPool.Subscribe<T>(new VirusX.Threading.Generic.TimerRule<T>(action, timeOut, priority), param);
        }
        public static IDisposable Subscribe<T>(VirusX.Threading.Generic.TimerRule<T> rule, T param, VirusX.Threading.StaticPool pool)
        {
            return pool.Subscribe<T>(rule, param);
        }
        public static IDisposable Subscribe<T>(VirusX.Threading.Generic.TimerRule<T> rule, T param)
        {
            return GenericThreadPool.Subscribe<T>(rule, param);
        }
        #endregion
       
        public static int Online
        {
            get
            {
                int current = Pool.GamePoll.Values.Count(p => p.Fake == false);
                return current;
            }
        }
        public static List<string> Insults = new List<string> {
            "k o s", "3ars", "زنيه", "كسه", "ksomk","بتنك", "ابن مرة", "3rs","ابن مرة", "ابن مرة", "/scale", "scale", "fuck", "abn", "naka",
            "Dick", "head", "mother", "fucker", "Kick", "Fuck ur self", "كسمك", "كسمين امك", "ك س م ك ام ك", "كسمين", "يا عرص", "ياعرص", "يا ابن الوسخه", "الوسخه", "يا ابن الشرموطه", "شرموطه",
            "امك", "ماما", "دين امك", " ينعال دين امك", "يا ابن دين الكلب", "دين الكلب", "يا ابن", "5od yad", "abok", "omak", "فجرة", "ابن فجرة", "يا ابن الفجرة", "cock", "M3rsen", "pussy",
            "son of bitch", "kos", "omk", " k o s", "mtnak", "sharmot", "5owl", "5awl", "zanya", "3rs", "hanekak", "Dana hanekak", "Den", "Sharmota", "Kosomen omak", "Kosomen", "/", "*", "@", "!", "/scale", "scale", " / ",
            "Mayten", "a7a", "a7eh", "fuck", "a 7 a", "خول", "متناك", "يمعرص ", "يبن الاحبه", "بتتناك", "انت بتتناك", "يبن العرص ", "يبن شرموطه", "يبن الاحبه", "يبن دين كلب", "يبن الشرموطه", "امك زانيه فيك" , "هبعبصك" , "تعريص", "Ta3res", "هحطو في طيزك", "التعريص دا ", "التعريص "
         };
        private static ProcessFactory processFactory;
        public static int MaxOnline;
        public static Extensions.Time32 UpdateServerStatus = Extensions.Time32.Now;
        public static DateTime SaveDBStamp = DateTime.Now.AddMinutes(30);
        public static DateTime SaveDBStamp2 = DateTime.Now.AddMinutes(5);
        public static DateTime BackUp = DateTime.Now.AddHours(1);
        public static VirusX.Threading.Generic.TimerRule<ServerSockets.SecuritySocket> ConnectionReceive, ConnectionSend, ConnectionReview;
        public static VirusX.Threading.StaticPool GenericThreadPool;
        public static VirusX.Threading.StaticPool ReceivePool, SendPool;

        public static void Create()
        {
            GenericThreadPool = new VirusX.Threading.StaticPool(36).Run();
            ReceivePool = new VirusX.Threading.StaticPool(36).Run();
            SendPool = new VirusX.Threading.StaticPool(36).Run();
            ThreadCollection.StartThreads();
            ConnectionReceive = new VirusX.Threading.Generic.TimerRule<ServerSockets.SecuritySocket>(connectionReceive, 1);
            ConnectionSend = new VirusX.Threading.Generic.TimerRule<ServerSockets.SecuritySocket>(connectionSend, 1);
            ConnectionReview = new VirusX.Threading.Generic.TimerRule<ServerSockets.SecuritySocket>(_ConnectionReview, 300000, ThreadPriority.Lowest);
            Subscribe(ServerFunctions, 60000);
            Subscribe(WorldTournaments, 1000);
            Subscribe(ArenaFunctions, 1000);
            Subscribe(SaveServer, 1000);
        }
        private static void ServerFunctions()
        {
            if (Program.ExitRequested)
                return;
            if (!Server.FullLoading)
                return;
            DateTime DateNow = DateTime.Now;
            Extensions.Time32 clock = Extensions.Time32.Now;

            #region Save Database
            lock (Database.ServerDatabase.SavingObj)
            {
                Server.SaveDBPayers();
                foreach (var client in Pool.GamePoll.Values)
                {
                    if (client.OnInterServer || client.Socket == null || !client.Socket.Alive)
                        continue;
                    if ((client.ClientFlag & Client.ServerFlag.LoginFull) == Client.ServerFlag.LoginFull)
                    {
                        client.ClientFlag |= Client.ServerFlag.QueuesSave;
                        Database.ServerDatabase.LoginQueue.TryEnqueue(client);
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            #endregion
            #region Random Seed
            if (DateNow > Pool.ResetRandom)
            {
                Pool.GetRandom.SetSeed(Environment.TickCount);
                Pool.ResetRandom = DateTime.Now.AddMinutes(30);
            }
            #endregion
            #region Reset Server
            Server.Reset(clock);
            #endregion
            #region Back UP [New]
            if (DateNow >= BackUp)
            {
                try
                {
                    string basePath = Program.ServerConfig.DbLocation + "BackUPS\\";
                    string backupPath = basePath +
                        DateNow.Year + " - " +
                        DateNow.Month + " - " +
                        DateNow.Day + " - " +
                        DateNow.Hour;

                    if (!Directory.Exists(backupPath))
                    {
                        Directory.CreateDirectory(backupPath);

                        string createUsers = backupPath + "\\Users\\";
                        string createspells = backupPath + "\\PlayersSpells\\";
                        string createprofs = backupPath + "\\PlayersProfs\\";
                        string createitems = backupPath + "\\PlayersItems\\";
                        string createquests = backupPath + "\\Quests\\";
                        string createhouses = backupPath + "\\Houses\\";
                        string createclans = backupPath + "\\Clans\\";
                        string createguilds = backupPath + "\\Guilds\\";
                        string createunions = backupPath + "\\Unions\\";
                        string coatColorRule = backupPath + "\\CoatColorRule\\";

                        Directory.CreateDirectory(createUsers);
                        Directory.CreateDirectory(createspells);
                        Directory.CreateDirectory(createprofs);
                        Directory.CreateDirectory(createitems);
                        Directory.CreateDirectory(createquests);
                        Directory.CreateDirectory(createhouses);
                        Directory.CreateDirectory(createclans);
                        Directory.CreateDirectory(createguilds);
                        Directory.CreateDirectory(createunions);
                        Directory.CreateDirectory(coatColorRule);

                        // Files
                        string db = Program.ServerConfig.DbLocation;
                        File.Copy(db + "\\JiangHu.txt", backupPath + "\\JiangHu.txt", true);
                        File.Copy(db + "\\PrestigeRanking.txt", backupPath + "\\PrestigeRanking.txt", true);
                        File.Copy(db + "\\InnerPower.txt", backupPath + "\\InnerPower.txt", true);

                        File.Copy(db + "\\Arena.ini", backupPath + "\\Arena.ini", true);
                        File.Copy(db + "\\BanIp.txt", backupPath + "\\BanIp.txt", true);
                        File.Copy(db + "\\BanUID.txt", backupPath + "\\BanUID.txt", true);
                        File.Copy(db + "\\ClaimContainer.bin", backupPath + "\\ClaimContainer.bin", true);
                        File.Copy(db + "\\ClassPkWar.ini", backupPath + "\\ClassPkWar.ini", true);
                        File.Copy(db + "\\ElitePk.ini", backupPath + "\\ElitePk.ini", true);
                        File.Copy(db + "\\GuildWarInfo.ini", backupPath + "\\GuildWarInfo.ini", true);
                        File.Copy(db + "\\HWRanking.txt", backupPath + "\\HWRanking.txt", true);

                        Program.Copy(db + "\\Users", createUsers);
                        Program.Copy(db + "\\PlayersSpells", createspells);
                        Program.Copy(db + "\\PlayersProfs", createprofs);
                        Program.Copy(db + "\\PlayersItems", createitems);
                        Program.Copy(db + "\\Quests", createquests);
                        Program.Copy(db + "\\Houses", createhouses);
                        Program.Copy(db + "\\Clans", createclans);
                        Program.Copy(db + "\\Guilds", createguilds);
                        Program.Copy(db + "\\Unions", createunions);
                        Program.Copy(db + "\\CoatColorRule", coatColorRule);

                        MyConsole.WriteLine("Done BackUp Database For Today ( " + DateTime.Now.Year + " - " + DateTime.Now.Month + " - " + DateTime.Now.Day + " - " + DateTime.Now.Hour + " ) ");
                    }

                    // next backup after 12 hours
                    BackUp = DateNow.AddHours(1);
                }
                catch (Exception e)
                {
                    MyConsole.WriteException(e);
                    BackUp = DateNow.AddMinutes(30); // retry later
                }
            }
            #endregion

        }
        private static void SaveServer()
        {
            if (Program.ExitRequested)
                return;
            if (!Server.FullLoading)
                return;
            DateTime DateNow = DateTime.Now;
            Extensions.Time32 clock = Extensions.Time32.Now;
            #region Save Server
         
            if (DateNow > SaveDBStamp)
            {
                Program.ConsoleCMD("save");
                SaveDBStamp = DateNow.AddMinutes(30);
            }
            #endregion
        }
        private static void WorldTournaments()
        {
            if (Program.ExitRequested)
                return;
           
            DateTime DateNow = DateTime.Now;
            DateTime Now64 = DateTime.Now;
            Extensions.Time32 clock = Extensions.Time32.Now;
            try
            {
                if (!Server.FullLoading)
                    return;
                #region Console Title
                if (Online > MaxOnline)
                    MaxOnline = Online;
                MyConsole.Title = Program.ServerConfig.ServerName + " - [ Online / " + Online + " ] - [ MaxOnline / " + MaxOnline + " ] Hossny .";
                if (clock > UpdateServerStatus.AddSeconds(5))
                {
                    var InfCount = Pool.GamePoll.Values.Count(p => p.Fake == false);
                    //new DBFunctionality.MySqlCommand(DBFunctionality.MySqlCommandType.UPDATE).Select("playersonline").Set("Online", InfCount).Execute();
                    UpdateServerStatus = Extensions.Time32.Now.AddSeconds(5);
                }
                #endregion
               
                #region BotsCheakUp
                Game.Ai.BotSystem.CheakUp();
                #endregion
                #region Anima Furnaces (Smelting)
                if (Server.FullLoading)
                {
                    if (Pool.ServerMaps.ContainsKey(10428))
                    {
                        var map = Pool.ServerMaps[10428];
                        if (map.View.GetAllMapRolesCount(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2284) > 0)
                        {
                            if (Time32.Now >= Pool.smeltFloorStamp.AddSeconds(30))
                            {
                                Pool.smeltFloorStamp = Time32.Now;
                                var floor = map.View.GetAllMapRoles(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2284).FirstOrDefault();
                                map.RemoveTrap(floor.X, floor.Y, floor);

                                foreach (var client in Pool.GamePoll.Values)
                                {
                                    if (client.Player.Map == map.ID && (client.Player.DynamicID == 0 || client.Player.DynamicID == map.ID))
                                    {
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = floor.X, Y = floor.Y, Strings = new string[1] { "DragonSoul_djs" } }));
                                        }
                                    }
                                }

                            }
                        }
                        else if (map.View.GetAllMapRolesCount(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2285) > 0)
                        {
                            if (Time32.Now >= Pool.smeltFloorStamp.AddSeconds(2))
                            {
                                Pool.smeltFloorStamp = Time32.Now;
                                var floor = map.View.GetAllMapRoles(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2285).FirstOrDefault() as MsgItem;
                                map.RemoveTrap(floor.X, floor.Y, floor);

                                floor.UID = MsgItem.UIDS.Next;
                                floor.MsgFloor.DropType = MsgDropID.Effect;
                                floor.MsgFloor.m_ID = 2284;
                                map.EnqueueItem(floor);
                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                {
                                    var stream = recycledPacket.GetStream();
                                    floor.SendAll(stream, MsgDropID.Effect);

                                }

                                //Result
                                if (Role.Core.Rate(50))//Lunar
                                {
                                    Pool.SmeltingSessions.Add(1);
                                    foreach (var client in Pool.GamePoll.Values)
                                        if (client.Player.Map == map.ID && (client.Player.DynamicID == 0 || client.Player.DynamicID == map.ID))
                                        {
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 42, Y = 55, Strings = new string[1] { "DragonSoul_ylsb" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 50, Y = 47, Strings = new string[1] { "DragonSoul_ylcg" } }));

                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 60, Y = 57, Strings = new string[1] { "npc_liandanlu_1" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 60, Y = 57, Strings = new string[1] { "glebesword" } }));
                                            }
                                            if (client.Player.DragonFurnace > 0 && !client.Player.DragonFurnace.ToString().StartsWith("5"))
                                            {

                                                if (Role.Core.Rate(BaseFunc.AnimaUpgradeRate(client.Player.DragonFurnace)))
                                                {
                                                    if ((client.Player.DragonFurnace + 1) % 100 >= 14)
                                                        Pool.SmeltingSuccesses.Add(new Pool.Smelt() { Furnace = 1, Name = client.Player.Name, Prize = Pool.ItemsBase[client.Player.DragonFurnace + 1].Name });
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        client.Inventory.Add(stream, client.Player.DragonFurnace + 1, 1);
                                                    }
                                                    client.CreateBoxDialog("+------- Smelting Succeeded -------+\n" +
                                                           "              Congratulations! \n" +
                                                        "        You received a " + Pool.ItemsBase[client.Player.DragonFurnace + 1].Name + "!\n" +
                                         "+----------------------------------+");
                                                }
                                                else
                                                {
                                                    if ((client.Player.DragonFurnace - 1) % 100 >= 14)
                                                        Pool.SmeltingSuccesses.Add(new Pool.Smelt() { Furnace = 1, Name = client.Player.Name, Prize = Pool.ItemsBase[client.Player.DragonFurnace - 1].Name });
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        client.Inventory.Add(stream, client.Player.DragonFurnace, 1);
                                                        client.Inventory.Add(stream, client.Player.DragonFurnace - 1, 1);
                                                    }
                                                    client.CreateBoxDialog("+------- Smelting Succeeded -------+\n" +
                                                           "  The Anima is not upgraded,! \n" +
                                                        "        but you received an extra " + Pool.ItemsBase[client.Player.DragonFurnace - 1].Name + "!\n" +
                                         "+----------------------------------+");
                                                }
                                            }
                                            client.Player.DragonFurnace = 0;
                                        }
                                }
                                else
                                {
                                    Pool.SmeltingSessions.Add(0);
                                    foreach (var client in Pool.GamePoll.Values)
                                    {
                                        if (client.Player.Map == map.ID && (client.Player.DynamicID == 0 || client.Player.DynamicID == map.ID))
                                        {
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 50, Y = 47, Strings = new string[1] { "DragonSoul_ylsb" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 42, Y = 55, Strings = new string[1] { "DragonSoul_ylcg" } }));

                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 52, Y = 65, Strings = new string[1] { "npc_liandanlu_1" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 52, Y = 65, Strings = new string[1] { "glebesword" } }));
                                            }
                                            if (client.Player.DragonFurnace > 0 && client.Player.DragonFurnace.ToString().StartsWith("5"))
                                            {
                                                client.Player.DragonFurnace -= 1000000;
                                                if (Role.Core.Rate(BaseFunc.AnimaUpgradeRate(client.Player.DragonFurnace)))
                                                {
                                                    if ((client.Player.DragonFurnace + 1) % 100 >= 14)
                                                        Pool.SmeltingSuccesses.Add(new Pool.Smelt() { Furnace = 0, Name = client.Player.Name, Prize = Pool.ItemsBase[client.Player.DragonFurnace + 1].Name });
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        client.Inventory.Add(stream, client.Player.DragonFurnace + 1, 1);
                                                    }
                                                    client.CreateBoxDialog("+------- Smelting Succeeded -------+\n" +
                                                           "              Congratulations! \n" +
                                                        "        You received a " + Pool.ItemsBase[client.Player.DragonFurnace + 1].Name + "!\n" +
                                         "+----------------------------------+");
                                                }
                                                else
                                                {
                                                    if ((client.Player.DragonFurnace - 1) % 100 >= 14)
                                                        Pool.SmeltingSuccesses.Add(new Pool.Smelt() { Furnace = 0, Name = client.Player.Name, Prize = Pool.ItemsBase[client.Player.DragonFurnace - 1].Name });
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        client.Inventory.Add(stream, client.Player.DragonFurnace, 1);
                                                        client.Inventory.Add(stream, client.Player.DragonFurnace - 1, 1);
                                                    }
                                                    client.CreateBoxDialog("+------- Smelting Succeeded -------+\n" +
                                                           "  The Anima is not upgraded,! \n" +
                                                        "        but you received an extra " + Pool.ItemsBase[client.Player.DragonFurnace - 1].Name + "!\n" +
                                         "+----------------------------------+");
                                                }
                                            }
                                            client.Player.DragonFurnace = 0;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Time32.Now >= Pool.smeltFloorStamp.AddSeconds(5))
                            {
                                Pool.smeltFloorStamp = Time32.Now;
                                var item = new MsgItem(null, 50, 55, MsgItem.ItemType.Effect, 0, 0, map.ID, 0, false, map, 60 * 60 * 1000);
                                item.MsgFloor.m_ID = 2285;
                                item.MsgFloor.m_Color = 1;
                                item.MsgFloor.DropType = MsgDropID.Effect;
                                map.EnqueueItem(item);
                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                {
                                    var stream = recycledPacket.GetStream();
                                    item.SendAll(stream, MsgDropID.Effect);
                                }

                            }
                        }
                    }
                }
                #endregion

                Game.MsgTournaments.MsgBroadcast.Work(clock);

                #region Tournaments
                Game.MsgTournaments.MsgSchedules.CheckUp();
                #endregion

           

                #region Team Pk Now
                foreach (var teamGroup in Game.MsgTournaments.MsgTeamPkTournament.EliteGroups)
                    teamGroup.timerCallback();
                #endregion

                #region Skill Pk Now
                foreach (var sTeamGroup in Game.MsgTournaments.MsgSkillTeamPkTournament.EliteGroups)
                    sTeamGroup.timerCallback();
                #endregion

                #region Eite Pk Now
                foreach (var elitegroup in Game.MsgTournaments.MsgEliteTournament.EliteGroups)
                    elitegroup.timerCallback();
                #endregion

                #region DragonIsland
                #region ThrillingSpook
                if ((DateNow.Minute == 00) && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[10137];
                    if (!map.ContainMobID(4212))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            Server.AddMapMonster(stream, map, 4212, 349, 635, 18, 18, 1);
                            if (Pool.MonsterFamilies.ContainsKey(4212))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[4212].Name
                                    + " has appeared at (" + 349 + "," + 635 + ") in the DragonIsland. Hurry and go kill the beast!",
                                    Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));


                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[4212].Name
                                      + " has appeared at (" + 349 + "," + 635 + ") in the DragonIsland. Hurry and go kill the beast!", "Alot Prize", 093, 410, 10137, 0, 60);
                            }
                        }
                    }
                }
                #endregion

                #region NemesisTyrant
                if ( (DateNow.Minute == 15) && DateNow.Second < 3)
                {

                    var map = Pool.ServerMaps[10137];

                    if (!map.ContainMobID(4220))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            Server.AddMapMonster(stream, map, 4220, 568, 372, 18, 18, 1);
                            if (Pool.MonsterFamilies.ContainsKey(4220))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[4220].Name
                                    + " has appeared at (" + 568 + "," + 372 + ") in the DragonIsland. Hurry and go kill the beast!",
                                    Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));

                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[4220].Name
                                        + " has appeared at (" + 568 + "," + 372 + ") in the DragonIsland. Hurry and go kill the beast!", "Alot Prize", 093, 410, 10137, 0, 60);
                            }
                        }
                    }
                }
                #endregion

                #region Snow Banshee
                if ( (DateNow.Minute == 27) && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[10137];
                    if (!map.ContainMobID(4171))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            Server.AddMapMonster(stream, map, 4171, 658, 718, 18, 18, 1);
                            if (Pool.MonsterFamilies.ContainsKey(4171))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[4171].Name
                                    + " has appeared at (" + 658 + "," + 718 + ") in the DragonIsland. Hurry and go kill the beast!",
                                    Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));

                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[4171].Name
                                    + " has appeared at (" + 658 + "," + 718 + ") in the DragonIsland. Hurry and go kill the beast!", "Alot Prize", 093, 410, 10137, 0, 60);
                            }
                        }
                    }
                }
                #endregion

                #endregion

                
                #region DeityLand

                #region DragonWraith
                if ( (DateNow.Minute == 05) && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[10250];

                    if (!map.ContainMobID(3971))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            ushort X = 163;
                            ushort Y = 415;

                            Server.AddMapMonster(stream, map, 3971, X, Y, 18, 18, 1);

                            if (Pool.MonsterFamilies.ContainsKey(3971))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3971].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));

                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[3971].Name
                                   + " has appeared at (" + 163 + "," + 415 + ")  has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", "Alot Prize", 1014, 1288, 10250, 0, 60);
                            }
                        }
                    }
                }
                #endregion

                #region ChillingSpook
                if ((DateNow.Minute == 30) && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[10250];

                    if (!map.ContainMobID(3977))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            ushort X = 1020;
                            ushort Y = 698;

                            Server.AddMapMonster(stream, map, 3977, X, Y, 18, 18, 1);

                            if (Pool.MonsterFamilies.ContainsKey(3977))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3977].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));
                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[3977].Name
                                 + " has appeared at (" + 1020 + "," + 698 + ")  has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", "Alot Prize", 1014, 1288, 10250, 0, 60);
                            }
                        }
                    }
                }
                #endregion

                #region BloodyBanshee
                if ( (DateNow.Minute == 57) && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[10250];

                    if (!map.ContainMobID(3976))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            ushort X = 484;
                            ushort Y = 176;

                            Server.AddMapMonster(stream, map, 3976, X, Y, 18, 18, 1);

                            if (Pool.MonsterFamilies.ContainsKey(3976))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3976].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));


                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[3976].Name
                                + " has appeared at (" + 484 + "," + 176 + ")  has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", "Alot Prize", 1014, 1288, 10250, 0, 60);
                            }
                        }
                    }
                }
                #endregion

                #region NetherTyrant
                if ( (DateNow.Minute == 45) && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[10250];

                    if (!map.ContainMobID(3978))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            ushort X = 917;
                            ushort Y = 1173;

                            Server.AddMapMonster(stream, map, 3978, X, Y, 18, 18, 1);

                            if (Pool.MonsterFamilies.ContainsKey(3978))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3978].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));
                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[3978].Name
                              + " has appeared at (" + 917 + "," + 1173 + ")  has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", "Alot Prize", 1014, 1288, 10250, 0, 60);
                            }
                        }
                    }
                }
                #endregion

                #region QueenofEvil
                if (((DateNow.Hour == 20) && DateNow.Minute == 10 && DateNow.Second < 3 )||( (DateNow.Hour == 13) && DateNow.Minute == 10 && DateNow.Second < 3))
                {
                    var map = Pool.ServerMaps[10250];

                    if (!map.ContainMobID(3970))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            ushort X = 625;
                            ushort Y = 623;

                            Server.AddMapMonster(stream, map, 3970, X, Y, 18, 18, 1);

                            if (Pool.MonsterFamilies.ContainsKey(3970))
                            {
                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3970].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));

                                MsgSchedules.SendInvitation(Pool.MonsterFamilies[3970].Name
                             + " has appeared at (" + 625 + "," + 623 + ")  has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", "Alot Prize", 1014, 1288, 10250, 0, 60);
                            }
                        }
                    }
                }
                #endregion
                #region NewQuest
                if ((DateNow.Hour == 12 || DateNow.Hour == 14
                    || DateNow.Hour == 16 || DateNow.Hour == 18
                    || DateNow.Hour == 20 || DateNow.Hour == 22) && DateNow.Minute == 00 && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[8822];

                    if (!map.ContainMobID(7985))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            ushort X = 191;
                            ushort Y = 198;

                            Server.AddMapMonster(stream, map, 7985, X, Y, 1, 1, 1, 0);
                        }
                    }
                }
                #endregion
                #region NewQuest
                if ((DateNow.Hour == 12 || DateNow.Hour == 13
                    || DateNow.Hour == 14 || DateNow.Hour == 15
                     || DateNow.Hour == 16|| DateNow.Hour == 17
                      || DateNow.Hour == 18 || DateNow.Hour == 19
                       || DateNow.Hour == 20 || DateNow.Hour == 21
                        || DateNow.Hour == 22 || DateNow.Hour == 23
                        ) && DateNow.Minute == 6 && DateNow.Second < 3)
                {
                    var map = Pool.ServerMaps[1063];

                    if (!map.ContainMobID(7483))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            ushort X = 84;
                            ushort Y = 64;

                            Server.AddMapMonster(stream, map, 7483, X, Y, 1, 1, 1, 0, true);
                        }
                    }
                }
                #endregion
                #endregion

            }
            catch (Exception e) { MyConsole.WriteException(e); }
        }
        private static void ArenaFunctions()
        {
            HeroGathering.CheckGroups();
            BotHandle.ThreadAction();
            Game.MsgTournaments.MsgSchedules.Arena.CheckGroups();
            Game.MsgTournaments.MsgSchedules.Arena.CreateMatches();
            Game.MsgTournaments.MsgSchedules.Arena.VerifyMatches();
        }
        private static void connectionReceive(ServerSockets.SecuritySocket wrapper)
        {
            try
            {
                if (wrapper.ReceiveBuffer())
                {
                    wrapper.HandlerBuffer();
                }
            }
            catch (Exception e)
            {
                MyConsole.WriteException(e);
            }
        }
        private static void connectionSend(ServerSockets.SecuritySocket obj)
        {
            try
            {
                while (SecuritySocket.TrySend(obj)) ;
            }
            catch (Exception e)
            {
                MyConsole.WriteException(e);
            }
        }
        private static void _ConnectionReview(ServerSockets.SecuritySocket wrapper)
        {
            try
            {
                wrapper.CheckUp();
            }
            catch (Exception e)
            {
                MyConsole.WriteException(e);
            }
        }
    }
}
