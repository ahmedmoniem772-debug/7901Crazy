using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ConquerOnline.Game.MsgServer;
using System.Threading.Generic;
using ConquerOnline.Game.MsgFloorItem;
using ConquerOnline.Game.MsgServer.AttackHandler;
using ConquerOnline.Game.MsgTournaments;
using ConquerOnline.Client;
using System.IO;
using ConquerOnline.Role.Instance;

namespace ConquerOnline
{
    //public static class ThreadPool
    //{
    //    public static int Online
    //    {
    //        get
    //        {
    //            int current = Pool.GamePoll.Values.Count(p => p.Fake == false);
    //            return current;
    //        }
    //    }
    //    public static int MaxOnline;
    //    private static DateTime ServerStamp = DateTime.Now;
    //    public static DateTime ArenaStamp = DateTime.Now;
    //    public static DateTime LastUpdate = DateTime.Now;
    //    private static DateTime savestamp = DateTime.Now;
    //    public static void ServerCallBack()
    //    {
    //        try
    //        {
    //            DateTime DateNow = DateTime.Now;

              

    //            if (Program.ExitRequested)
    //                return;
    //            if (DateNow > ServerStamp)
    //            {
    //                ServerFunctions();
    //                ServerStamp = DateNow.AddMilliseconds(1000);
    //            }
    //            if (DateNow > ArenaStamp)
    //            {
    //                ArenaFunctions();
    //                TeamArenaFunctions();
    //                ArenaStamp = DateNow.AddMilliseconds(1000);
    //            }
    //            Game.MsgTournaments.MsgBroadcast.Work();
    //            WorldTournaments();
               
    //        }
    //        catch (Exception e)
    //        {
    //            MyConsole.SaveException(e);
    //        }
    //    }
    //    private static void WorldTournaments()
    //    {
    //        if (!Server.FullLoading)
    //            return;
    //        #region BotsCheakUp
    //        Game.Ai.BotSystem.CheakUp();
    //        #endregion
           

    //        DateTime DateNow = DateTime.Now;

    //        #region Console Title
    //        if (Online > MaxOnline)
    //            MaxOnline = Online;
    //        MyConsole.Title = Program.ServerConfig.ServerName + " - [ Online / " + Online + " ] - [ MaxOnline / " + MaxOnline + " ] #ConquerOnline .";
    //        if (DateNow > LastUpdate.AddMinutes(3))
    //        {
    //            var InfCount = Pool.GamePoll.Values.Count(p => p.Fake == false);
    //            new DBFunctionality.MySqlCommand(DBFunctionality.MySqlCommandType.UPDATE).Select("playersonline").Set("Online", InfCount).Execute();

    //            LastUpdate = DateNow;
    //        }
    //        #endregion

         
            
    //        #region Tournaments
    //        Game.MsgTournaments.MsgSchedules.CheckUp();
    //        #endregion
    //        #region Roulettes Tables
    //        foreach (var roullet in Database.Roulettes.RoulettesPoll.Values)
    //            roullet.work();
    //        #endregion
    //        #region Poker Tables
    //        foreach (var t in Database.Poker.Tables.Values)
    //            PokerHandler.PokerTablesCallback(t, 0);
    //        #endregion

    //        #region Team Pk Now
    //        foreach (var teamGroup in Game.MsgTournaments.MsgTeamPkTournament.EliteGroups)
    //            teamGroup.timerCallback();
    //        #endregion

    //        #region Skill Pk Now
    //        foreach (var sTeamGroup in Game.MsgTournaments.MsgSkillTeamPkTournament.EliteGroups)
    //            sTeamGroup.timerCallback();
    //        #endregion

    //        #region Eite Pk Now
    //        foreach (var elitegroup in Game.MsgTournaments.MsgEliteTournament.EliteGroups)
    //            elitegroup.timerCallback();
    //        #endregion

    //        #region DragonIsland

    //        #region NemesisTyrant
    //        if ((DateNow.Minute == 45) && DateNow.Second == 0)
    //        {

    //            var map = Pool.ServerMaps[10137];

    //            if (!map.ContainMobID(4220))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();

    //                    Server.AddMapMonster(stream, map, 4220, 568, 372, 18, 18, 1);

    //                    string Messaj = "The NemesisTyrant have spawned in the DragonIsland on (568, 372) ! Hurry to kill them. Drop [SavageBone, Garment[5]Star, item Check MythSoul ,etc].";

    //                    Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Messaj, Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.Center).GetArray(stream));


    //                    foreach (var user in Pool.GamePoll.Values)

    //                        user.Player.MessageBox(Messaj, new Action<Client.GameClient>(p => { p.Teleport(94, 409, 10137); }), null, 60);

    //                }
    //            }
    //        }
    //        #endregion

    //        #region Snow Banshee
    //        if ((DateNow.Minute == 57) && DateNow.Second == 0)
    //        {
    //            var map = Pool.ServerMaps[10137];
    //            if (!map.ContainMobID(4171))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();
    //                    Server.AddMapMonster(stream, map, 4171, 658, 718, 18, 18, 1);

    //                    string Messaj = "The Snow Banshee appeared in DragonIsland(658,718)! Hurry to kill them. Drop [SavageBone,MoonFruit,SoulFruit,etc].";


    //                    Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Messaj, Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.Center).GetArray(stream));
    //                    foreach (var user in Pool.GamePoll.Values)
    //                        user.Player.MessageBox(Messaj, new Action<Client.GameClient>(p =>
    //                        {
    //                            p.Teleport(94, 409, 10137);
    //                        }
    //                                              ), null, 60);
    //                }
    //            }
    //        }
    //        #endregion

    //        #region ThrillingSpook
    //        if ((DateNow.Minute == 28) && DateNow.Second == 0)
    //        {
    //            var map = Pool.ServerMaps[10137];
    //            if (!map.ContainMobID(4212))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();

    //                    Server.AddMapMonster(stream, map, 4212, 349, 635, 18, 18, 1);

    //                    string Messaj = "The ThrillingSpook have spawned in the DragonIsland on (349,635) ! Hurry to kill them. Drop [SavageBone,MoonFruit,SoulFruit,etc].";

    //                    Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Messaj, Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.Center).GetArray(stream));

    //                    foreach (var user in Pool.GamePoll.Values)

    //                        user.Player.MessageBox(Messaj, new Action<Client.GameClient>(p => { p.Teleport(94, 409, 10137); }), null, 60);

    //                }
    //            }
    //        }
    //        #endregion

    //        #endregion

    //        #region DeityLand

    //        #region QueenofEvil
    //        if ((DateNow.Minute == 10) && DateNow.Second == 3)
    //        {
    //            var map = Pool.ServerMaps[10250];

    //            if (!map.ContainMobID(3970))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();

    //                    ushort X = 640;
    //                    ushort Y = 600;

    //                    Server.AddMapMonster(stream, map, 3970, X, Y, 18, 18, 1);

    //                    if (Pool.MonsterFamilies.ContainsKey(3970))
    //                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3970].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));
    //                    foreach (var user in Pool.GamePoll.Values)

    //                        user.Player.MessageBox(Pool.MonsterFamilies[3970].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", new Action<Client.GameClient>(p => { p.Teleport(1008, 1287, 10250); }), null, 60);

    //                }
    //            }
    //        }
    //        #endregion            

    //        #region NetherTyrant
    //        if ((DateNow.Minute == 50) && DateNow.Second == 3)
    //        {
    //            var map = Pool.ServerMaps[10250];

    //            if (!map.ContainMobID(3978))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();

    //                    ushort X = 872;
    //                    ushort Y = 1135;

    //                    Server.AddMapMonster(stream, map, 3978, X, Y, 18, 18, 1);

    //                    if (Pool.MonsterFamilies.ContainsKey(3978))
    //                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3978].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));
    //                    foreach (var user in Pool.GamePoll.Values)

    //                        user.Player.MessageBox(Pool.MonsterFamilies[3978].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", new Action<Client.GameClient>(p => { p.Teleport(1008, 1287, 10250); }), null, 60);

    //                }
    //            }
    //        }
    //        #endregion

    //        #region BloodyBanshee
    //        if ((DateNow.Minute == 10) && DateNow.Second == 3)
    //        {
    //            var map = Pool.ServerMaps[10250];

    //            if (!map.ContainMobID(3976))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();

    //                    ushort X = 484;
    //                    ushort Y = 176;

    //                    Server.AddMapMonster(stream, map, 3976, X, Y, 18, 18, 1);

    //                    if (Pool.MonsterFamilies.ContainsKey(3976))
    //                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3976].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));
    //                    foreach (var user in Pool.GamePoll.Values)

    //                        user.Player.MessageBox(Pool.MonsterFamilies[3976].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", new Action<Client.GameClient>(p => { p.Teleport(1008, 1287, 10250); }), null, 60);

    //                }
    //            }
    //        }
    //        #endregion

    //        #region ChillingSpook
    //        if ((DateNow.Minute == 35) && DateNow.Second == 3)
    //        {
    //            var map = Pool.ServerMaps[10250];

    //            if (!map.ContainMobID(3977))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();

    //                    ushort X = 1020;
    //                    ushort Y = 698;

    //                    Server.AddMapMonster(stream, map, 3977, X, Y, 18, 18, 1);

    //                    if (Pool.MonsterFamilies.ContainsKey(3977))
    //                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3977].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));
    //                    foreach (var user in Pool.GamePoll.Values)

    //                        user.Player.MessageBox(Pool.MonsterFamilies[3977].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", new Action<Client.GameClient>(p => { p.Teleport(1008, 1287, 10250); }), null, 60);

    //                }
    //            }
    //        }
    //        #endregion

    //        #region DragonWraith
    //        if ((DateNow.Minute == 05) && DateNow.Second == 3)
    //        {
    //            var map = Pool.ServerMaps[10250];

    //            if (!map.ContainMobID(3971))
    //            {

    //                using (var rec = new ServerSockets.RecycledPacket())
    //                {
    //                    var stream = rec.GetStream();

    //                     ushort X = 163;
    //                     ushort Y = 415;

    //                    Server.AddMapMonster(stream, map, 3971, X, Y, 18, 18, 1);

    //                   if (Pool.MonsterFamilies.ContainsKey(3971))
    //                                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Pool.MonsterFamilies[3971].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.TopLeft).GetArray(stream));
    //                            foreach (var user in Pool.GamePoll.Values)

    //                                user.Player.MessageBox(Pool.MonsterFamilies[3971].Name + " has appeared at (" + X + "," + Y + ") in the Deityland. Hurry and go kill the beast!", new Action<Client.GameClient>(p => { p.Teleport(1008, 1287, 10250); }), null, 60);

    //                }
    //            }
    //        }
    //        #endregion
    //        #endregion

           
    //    }
    //    private static void ServerFunctions()
    //    {
    //        DateTime DateNow = DateTime.Now;
    //        #region Restart
    //        if (DateNow.Hour == 00 && DateNow.Minute == 00 && DateNow.Second == 1)
    //        {
    //            Program.ConsoleCMD("down");

    //        }
    //        #endregion
    //        #region Random Seed
    //        if (DateNow > Pool.ResetRandom)
    //        {
    //            Pool.GetRandom.SetSeed(Environment.TickCount);
    //            Pool.ResetRandom = DateTime.Now.AddMinutes(30);
    //        }
    //        #endregion
    //        #region Reset Server
    //        Server.Reset();
    //        #endregion
    //        #region Back UP
    //        if (DateNow.Hour == 12 && DateNow.Minute == 0 && DateNow.Second == 1)//BackUP
    //        {
    //            try
    //            {
    //                string create = Program.ServerConfig.DbLocation + "\\AABackUP\\" + DateTime.Now.Year + " - " + DateTime.Now.Month + " - " + DateTime.Now.Day + " ";
    //                string createUsers = create + "\\Users";
    //                string createspells = create + "\\PlayersSpells";
    //                string createprofs = create + "\\PlayersProfs";
    //                string createitems = create + "\\PlayersItems";
    //                string createquests = create + "\\Quests";
    //                string createhouses = create + "\\Houses";
    //                string createclans = create + "\\Clans";
    //                string createguilds = create + "\\Guilds";
    //                string createunions = create + "\\Unions";
    //                string all = createUsers + createspells + createprofs + createitems + createquests + createhouses + createclans + createguilds + createunions;
    //                try
    //                {
    //                    if (!Directory.Exists(create))
    //                    {
    //                        DirectoryInfo di = Directory.CreateDirectory(create);
    //                        DirectoryInfo di2 = Directory.CreateDirectory(createUsers);
    //                        DirectoryInfo di3 = Directory.CreateDirectory(createspells);
    //                        DirectoryInfo di4 = Directory.CreateDirectory(createprofs);
    //                        DirectoryInfo di5 = Directory.CreateDirectory(createitems);
    //                        DirectoryInfo di6 = Directory.CreateDirectory(createquests);
    //                        DirectoryInfo di7 = Directory.CreateDirectory(createhouses);
    //                        DirectoryInfo di8 = Directory.CreateDirectory(createclans);
    //                        DirectoryInfo di9 = Directory.CreateDirectory(createguilds);
    //                        DirectoryInfo di0 = Directory.CreateDirectory(createunions);
    //                        MyConsole.WriteLine("Folders Created at " + create + "");
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\JiangHu.txt", create + "\\JiangHu.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\PrestigeRanking.txt", create + "\\PrestigeRanking.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\InnerPower.txt", create + "\\InnerPower.txt", true);

    //                        File.Copy(Program.ServerConfig.DbLocation + "\\Arena.ini", create + "\\Arena.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\BanIp.txt", create + "\\BanIp.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\BanUID.txt", create + "\\BanUID.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\ClaimContainer.bin", create + "\\ClaimContainer.bin", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\ClassPkWar.ini", create + "\\ClassPkWar.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\ElitePk.ini", create + "\\ElitePk.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\GuildWarInfo.ini", create + "\\GuildWarInfo.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\HWRanking.txt", create + "\\HWRanking.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\Insults.bin", create + "\\Insults.bin", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\Npcs.txt", create + "\\Npcs.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\PerfectionRanking.bin", create + "\\PerfectionRanking.bin", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\RedeemContainer.bin", create + "\\RedeemContainer.bin", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\RuneRanking.txt", create + "\\RuneRanking.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\SkillTeamPK.ini", create + "\\SkillTeamPK.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\SobNpcs.txt", create + "\\SobNpcs.txt", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\SuperGuildWarInfo.ini", create + "\\SuperGuildWarInfo.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\TeamArena.ini", create + "\\TeamArena.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\TeamElitePK.ini", create + "\\TeamElitePK.ini", true);
    //                        File.Copy(Program.ServerConfig.DbLocation + "\\Votes.txt", create + "\\Votes.txt", true);

    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\Users", createUsers);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\PlayersSpells", createspells);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\PlayersProfs", createprofs);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\PlayersItems", createitems);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\Quests", createquests);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\Houses", createhouses);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\Clans", createclans);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\Guilds", createguilds);
    //                        Program.Copy(Program.ServerConfig.DbLocation + "\\Unions", createunions);
    //                        MyConsole.WriteLine("Done BackUp Database For Today ( " + DateTime.Now.Year + " - " + DateTime.Now.Month + " - " + DateTime.Now.Day + " ) ");
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        MyConsole.WriteLine("" + create + " Already Backed Up !");
    //                        return;
    //                    }
    //                }
    //                catch (IOException ioex)
    //                {
    //                    Console.WriteLine(ioex.Message);
    //                }
    //            }
    //            catch (Exception e) { Console.WriteLine(e.ToString()); }
    //        }
    //        #endregion
    //        #region Save Server
    //        if (DateNow.Minute == 30 && DateNow.Second < 1)
    //        {
    //            Program.ConsoleCMD("save");
    //        }
    //        #endregion
    //        ThreadPool.savestamp = DateNow.AddSeconds(180);
    //    }
    //    private static void ArenaFunctions()
    //    {
    //        HeroGathering.CheckGroups();
    //        BotHandle.ThreadAction();
    //        Game.MsgTournaments.MsgSchedules.Arena.CheckGroups();
    //        Game.MsgTournaments.MsgSchedules.Arena.CreateMatches();
    //        Game.MsgTournaments.MsgSchedules.Arena.VerifyMatches();
                
    //    }
    //    private static void TeamArenaFunctions()
    //    {
    //        Game.MsgTournaments.MsgSchedules.TeamArena.CheckGroups();
    //        Game.MsgTournaments.MsgSchedules.TeamArena.CreateMatches();
    //        Game.MsgTournaments.MsgSchedules.TeamArena.VerifyMatches();
    //    }
    //}
}