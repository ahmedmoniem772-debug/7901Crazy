using ConquerOnline.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerOnline.Game.MsgServer;
using ConquerOnline.Database.DBActions;
using System.Collections.Concurrent;
using ConquerOnline.Game;

namespace ConquerOnline
{
    public class EnemyInvade
    {
        public static Counter Counter = new Counter(1);
        public enum MatchStatus : byte
        {
            None,
            Winner,
            Loser
        }
        public static ConcurrentDictionary<uint, Match> MatchesRegistered = new ConcurrentDictionary<uint, Match>();
        public static ConcurrentDictionary<uint, GameClient> Players = new ConcurrentDictionary<uint, GameClient>();
        public class User
        {
            public bool inFight = false;
            public uint Damage;
            private uint _Chance;
            private uint _MyPoint;
            public uint Chance
            {
                get
                {
                    return _Chance;
                }
                set
                {
                    _Chance = value;
                }
            }
            public uint MyPoint
            {
                get { return _MyPoint; }
                set
                {
                    _MyPoint = value;
                }
            }
            public uint MyRank;
            public uint UserID;
            public uint Class;
            public uint Level;
            public uint Mesh;
            public string Name = "";
            public uint GetRecovery
            {
                get
                {
                    return (uint)(TimeStamp - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                }
            }
            public uint del_time;
            public DateTime TimeStamp;
            public MatchStatus State = MatchStatus.None;
            public void Recovery(uint Time)
            {
                if (DateTime.Now > TimeStamp)
                {
                    del_time = Time;
                    TimeStamp = DateTime.Now.AddSeconds(del_time);
                }
            }
            public void Ck()
            {
                if (DateTime.Now >= TimeStamp)
                {
                    if (Chance < 10)
                        Chance++;
                    if (Chance < 10)
                        Recovery(2 * 60 * 60);
                }
            }
            public void Reseting()
            {
                Chance = 10;
            }
            public void UpdateRank()
            {
                if (UserID == 0)
                    return;
                EnemyInvadeRank.Entry entry = new EnemyInvadeRank.Entry();
                entry.TotalPoints = MyPoint;
                entry.Type = EnemyInvadeRank.Type.Gathering;
                entry.Level = (byte)Level;
                entry.Mesh = Mesh;
                entry.Class = Class;
                entry.Name = Name;
                entry.UID = UserID;
                EnemyInvadeRank.Ranks[entry.Type].UpdateItem(entry);
                entry = new EnemyInvadeRank.Entry();
                entry.TotalPoints = MyPoint;
                entry.Type = (EnemyInvadeRank.Type)(Class / 1000);
                entry.Level = (byte)Level;
                entry.Mesh = Mesh;
                entry.Class = Class;
                entry.Name = Name;
                entry.UID = UserID;
                EnemyInvadeRank.Ranks[entry.Type].UpdateItem(entry);
            }
            public ConquerOnline.ServerSockets.Packet AddPoint(uint point)
            {
                MsgEnemyInvadeArenic obj = new MsgEnemyInvadeArenic();
                obj.Type = (uint)MsgEnemyInvadeArenic.TypeID.AddPoint;
                obj.Type2 = 1;
                obj.Point = point;
                MyPoint += point;
                UpdateRank();
                return obj;
            }
            public override string ToString()
            {
                var file = new WriteLine('/');
                file.Add(MyPoint);
                file.Add(Chance);
                file.Add(TimeStamp.Ticks);
                return file.Close();
            }
            public void Load(string line)
            {
                if (line != "")
                {
                    ReadLine reader = new ReadLine(line, '/');
                    MyPoint = reader.Read((uint)0);
                    Chance = reader.Read((uint)0);
                    TimeStamp = DateTime.FromBinary(reader.Read((long)0));
                    TimeSpan timeSpan = TimeStamp - DateTime.Now;
                    del_time = (uint)timeSpan.TotalMinutes * 60;
                    Chance = 10;
                }
            }
            public List<GameClient> Players;
            private Client.GameClient user;
            public Match GET_Match;
            public User(Client.GameClient _user)
            {
                user = _user;
                Players = new List<GameClient>();
            }
            public void UserInfo()
            {
                UserID = user.Player.UID;
                Level = user.Player.Level;
                Mesh = user.Player.Mesh;
                Class = user.Player.Class;
                Name = user.Player.Name;
            }
            public void LoadingGathering(GameClient user)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    MsgEnemyInvadeOpt Info = new MsgEnemyInvadeOpt();
                    user.EnemyInvade.UserInfo();
                    Info.Type = (uint)MsgEnemyInvadeOpt.TypeID.Login;
                    Info.UserID = user.Player.UID;
                    Info.MyPoint = user.EnemyInvade.MyPoint;
                    Info.StartTime = 1619172392;
                    Info.Chance = user.EnemyInvade.Chance;
                    Info.Recovery = user.EnemyInvade.GetRecovery;//Recovery
                    Info.uk7 = 1619172393;//Now
                    Info.uk8 = 0;
                    Info.uk9 = 0;
                    Info.uk10 = 1;
                    user.Send(Info);
                }
            }
            public void SetBot()
            {
                foreach (var bot in EnemyInvade.Players.Values)
                {
                    if (bot.EnemyInvadeId == user.Player.UID)
                        user.EnemyInvade.Players.Add(bot);
                }
                if (user.EnemyInvade.Players.Count != 4)
                    RandomBot();
            }
            public void ClearBot()
            {
                //foreach (var bot in user.EnemyInvade.Players)
                //    bot.EnemyInvadeId = 0;
                user.EnemyInvade.Players.Clear();
            }
            public void RandomBot()
            {
                ClearBot();
                foreach (var bot in EnemyInvade.Players.Values)
                {
                    Client.GameClient p;
                    if (bot.EnemyInvadeId != 0 && Pool.GamePoll.TryGetValue(bot.EnemyInvadeId, out p))
                        if (p.Socket != null && p.Socket.Alive)
                            continue;
                    user.EnemyInvade.Players.Add(bot);
                    bot.EnemyInvadeId = user.Player.UID;
                    if (user.EnemyInvade.Players.Count == 4)
                        break;
                }
                if (user.EnemyInvade.Players.Count != 4)
                    user.Player.MessageBox("Please wait, places are reserved");
            }
            public void SendEnemy()
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    if (user.EnemyInvade.Players.Count == 4)
                    {
                        MsgEnemyInvadeOpt obj = new MsgEnemyInvadeOpt();
                        obj.Type = (uint)MsgEnemyInvadeOpt.TypeID.Show;
                        obj.Players = new List<MsgEnemyInvadeOpt.Player>();
                        foreach (var Enemy in user.EnemyInvade.Players)
                        {
                            Client.GameClient bot;
                            if (Pool.GamePoll.TryGetValue(Enemy.Player.UID, out bot))
                            {
                                bot.Player.BWindowsView = 3;
                                user.Send(bot.Player.GetArray(stream, false));
                                bot.Player.BWindowsView = 0;

                                obj.Players.Add(new MsgEnemyInvadeOpt.Player()
                                {
                                    ID = bot.Player.UID,
                                    Point = 150,
                                });
                            }
                        }
                        user.Send(obj);
                    }
                }
            }
        }
        public class Match
        {
            public List<GameClient> Fights;
            public uint ID;
            public bool Done;
            public DateTime DoneStamp;
            public DateTime StartTimer;
            public Match(uint id)
            {
                Fights = new List<GameClient>();
                StartTimer = DateTime.Now;
                ID = id;
            }
            public Client.GameClient Winner()
            {
                var client = Fights.Where(p => p.EnemyInvade.State != MatchStatus.Loser && p.EnemyInvade.State != MatchStatus.None).FirstOrDefault();
                if (client == null)
                {
                    Console.WriteLine("ERROR Winner");
                }
                return client;
            }
            public Client.GameClient Loser()
            {
                var client = Fights.Where(p => p.EnemyInvade.State == MatchStatus.Loser).FirstOrDefault();
                if (client == null)
                {
                    Console.WriteLine("ERROR Loser");
                }
                return client;
            }
            public void SendScore()
            {
                MsgEnemyInvadeArenic obj = new MsgEnemyInvadeArenic();
                obj.Type = (uint)MsgEnemyInvadeArenic.TypeID.SendScore;
                obj.Players = new MsgEnemyInvadeArenic.Player[Fights.Count];
                for (int i = 0; i < Fights.Count; i++)
                {
                    var GET_user = Fights[i];
                    obj.Players[i] = new MsgEnemyInvadeArenic.Player();
                    obj.Players[i].UID = GET_user.Player.UID;
                    obj.Players[i].ServerID = GET_user.Player.ServerID;
                    obj.Players[i].Name = GET_user.Player.Name;
                    obj.Players[i].Damage = GET_user.EnemyInvade.Damage;
                }
                foreach (var GET_user in Fights)
                    GET_user.Send(obj);

            }
            public void End(Client.GameClient loser)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    if (Done)
                        return;
                    Done = true;
                    foreach (var GET_user in Fights)
                    {
                        if (GET_user.Player.UID == loser.Player.UID)
                            GET_user.EnemyInvade.State = MatchStatus.Loser;
                        else
                            GET_user.EnemyInvade.State = MatchStatus.Winner;
                    }
                    if (Loser() != null)
                    {
                        GameClient GET_Loser = Loser();
                        GET_Loser.Send(stream.ArenaSignupCreate(MsgArenaSignup.DialogType.Dialog, MsgArenaSignup.DialogButton.Lose, loser));
                    }
                    if (Winner() != null)
                    {
                        GameClient GET_Winner = Winner();
                        GET_Winner.Send(stream.ArenaSignupCreate(MsgArenaSignup.DialogType.Dialog, MsgArenaSignup.DialogButton.Win, Winner()));
                    }
                    MsgEnemyInvadeArenic obj = new MsgEnemyInvadeArenic();
                    obj.Type = (uint)MsgEnemyInvadeArenic.TypeID.Status;
                    obj.Type2 = 3;
                    Winner().Send(obj);
                    Loser().Send(obj);
                    DoneStamp = DateTime.Now;
                }
            }
            public void EndExport(Client.GameClient GET_user)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    GET_user.TeleportCallBack();
                    GET_user.Player.RestorePkMode();
                    GET_user.EnemyInvade.GET_Match = null;
                    GET_user.EnemyInvade.inFight = false;
                    GET_user.EnemyInvade.State = MatchStatus.None;
                    GET_user.Player.Revive(stream);
                }
            }
            public void Export()
            {
                if (Fights.Count == 2)
                {
                    foreach (var GET_user in Fights)
                    {
                        if (GET_user.EnemyInvade.inFight)
                        {
                            EndExport(GET_user);
                        }
                    }
                }
            }
            public void Import(Client.GameClient User)
            {
                try
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        if (Fights.Count == 2)
                        {
                            var map = Pool.ServerMaps[700];
                            uint dinamicID = map.GenerateDynamicID();
                            foreach (var GET_user in Fights)
                            {
                                if (GET_user.Player.UID != User.Player.UID)
                                {
                                    MsgEnemyInvadeArenic obj = new MsgEnemyInvadeArenic();
                                    obj.Type = (uint)MsgEnemyInvadeArenic.TypeID.Status;
                                    obj.Type2 = 1;
                                    User.Send(obj);
                                    obj = new MsgEnemyInvadeArenic();
                                    obj.Type = (uint)MsgEnemyInvadeArenic.TypeID.Info;
                                    obj.Players = new MsgEnemyInvadeArenic.Player[1];
                                    obj.Players[0] = new MsgEnemyInvadeArenic.Player();
                                    obj.Players[0].UID = GET_user.Player.RealUID;
                                    obj.Players[0].ServerID = GET_user.Player.ServerID;
                                    obj.Players[0].Class = GET_user.Player.Class;
                                    obj.Players[0].Level = GET_user.Player.Level;
                                    obj.Players[0].u5 = 1;
                                    obj.Players[0].u6 = 1;
                                    obj.Players[0].Name = GET_user.Player.Name;
                                    User.Send(obj);
                                    obj = new MsgEnemyInvadeArenic();
                                    obj.Type = (uint)MsgEnemyInvadeArenic.TypeID.Status;
                                    obj.Type2 = 2;
                                    User.Send(obj);
                                }
                                GET_user.Player.Revive(stream);

                                ushort x = 0;
                                ushort y = 0;
                                map.GetRandCoord(ref x, ref y);
                                GET_user.Teleport(x, y, 700, dinamicID);

                                GET_user.Player.ProtectJumpAttack(10);
                                GET_user.Player.SetPkMode(Role.Flags.PKMode.PK);
                                GET_user.EnemyInvade.inFight = true;
                                GET_user.EnemyInvade.Damage = 0;
                                GET_user.EnemyInvade.State = MatchStatus.None;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            public void End()
            {
                End((Fights[0].EnemyInvade.Damage > Fights[1].EnemyInvade.Damage) ? Fights[1] : Fights[0]);
            }
            public void Win(Client.GameClient winner, Client.GameClient loser)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    if (winner != null)
                    {
                        winner.Send(new MsgEnemyInvadeArenic() { Type = (uint)MsgEnemyInvadeArenic.TypeID.Export, Type2 = 1 });
                        winner.Send(winner.EnemyInvade.AddPoint(150));
                    }
                    if (loser != null)
                    {
                        loser.Send(new MsgEnemyInvadeArenic() { Type = (uint)MsgEnemyInvadeArenic.TypeID.Export, Type2 = 0 });

                    }
                }
            }
        }
        public static void CheckGroups()
        {
            if (MatchesRegistered.Count > 0)
            {
                DateTime Now = DateTime.Now;
                foreach (var group in MatchesRegistered.Values)
                {
                    if (Now > group.StartTimer.AddSeconds(5))
                    {
                        if (!group.Done)
                        {
                            if (Now > group.StartTimer.AddMinutes(3))
                            {
                                group.End();
                            }
                        }
                        else
                        {
                            if (Now > group.DoneStamp.AddSeconds(4))
                            {
                                group.DoneStamp = DateTime.Now.AddDays(1);
                                group.Win(group.Winner(), group.Loser());
                                group.Export();
                                MatchesRegistered.Remove(group.ID);
                            }
                        }
                    }
                }
            }
        }
        public static void Load()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                if (Players.Count == 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        var bot = ConquerOnline.AIB.ConquerAIHandle.CreateBots(stream, (AIB.BotLevel)6, 5000, 0, 0, true);
                        Players.Add(bot.Player.UID, bot);
                    }
                }
            }
        }

        [PacketAttribute(GamePackets.MsgEnemyInvadeOpt)]
        private unsafe static void Process1(Client.GameClient user, ServerSockets.Packet stream)
        {
            Load();
            MsgEnemyInvadeOpt Info = stream;
            switch ((MsgEnemyInvadeOpt.TypeID)Info.Type)
            {
                case MsgEnemyInvadeOpt.TypeID.Fight:
                    {
                        if (user.EnemyInvade.Chance >= 1)
                        {
                            Client.GameClient bot;
                            if (Pool.GamePoll.TryGetValue(Info.UserID, out bot))
                            {
                                EnemyInvade.Match obj = new EnemyInvade.Match(Counter.Next);
                                obj.Fights = new List<GameClient>();
                                obj.Fights.Add(user);
                                obj.Fights.Add(bot);
                                obj.Import(user);
                                user.EnemyInvade.GET_Match = obj;
                                bot.EnemyInvade.GET_Match = obj;
                                MatchesRegistered.Add(obj.ID, obj);
                                user.Send(Info);
                                user.EnemyInvade.Chance--;
                                user.EnemyInvade.Recovery(2 * 60 * 60);
                            }
                        }
                        break;
                    }
                case MsgEnemyInvadeOpt.TypeID.Info:
                    {
                        Info.UserID = user.Player.UID;
                        Info.MyPoint = user.EnemyInvade.MyPoint;
                        Info.StartTime = 1619172392;
                        Info.Chance = user.EnemyInvade.Chance;
                        Info.Recovery = user.EnemyInvade.GetRecovery;//Recovery
                        Info.uk7 = 0;//Now
                        Info.uk8 = 1;
                        Info.uk9 = 0;
                        Info.uk10 = 1;
                        user.Send(Info);
                        user.EnemyInvade.SetBot();
                        user.EnemyInvade.SendEnemy();
                        break;
                    }
                case MsgEnemyInvadeOpt.TypeID.Change:
                    {
                        user.EnemyInvade.RandomBot();
                        Info.UserID = user.Player.UID;
                        Info.MyPoint = user.EnemyInvade.MyPoint;
                        Info.StartTime = 1619172392;
                        Info.Chance = user.EnemyInvade.Chance;
                        Info.Recovery = user.EnemyInvade.GetRecovery;
                        Info.uk7 = 0;//Now
                        Info.uk8 = 1;
                        Info.uk9 = 0;
                        Info.uk10 = 1;
                        user.Send(Info);
                        user.EnemyInvade.SendEnemy();
                        break;
                    }

                default:
                    Console.WriteLine(" HeroGatheringInfo Info.Type: " + Info.Type);
                    break;
            }

        }
        [PacketAttribute(GamePackets.MsgEnemyInvadeArenic)]
        private unsafe static void Process2(Client.GameClient user, ServerSockets.Packet stream)
        {
            MsgEnemyInvadeArenic Info = new MsgEnemyInvadeArenic();
            Info = stream.ProtoBufferDeserialize<MsgEnemyInvadeArenic>(Info);
            switch ((MsgEnemyInvadeArenic.TypeID)Info.Type)
            {
                default:
                    Console.WriteLine(" EnemyInvade Info.Type: " + Info.Type);
                    break;
                case MsgEnemyInvadeArenic.TypeID.Quit:
                    {
                        if (user.EnemyInvade.GET_Match != null)
                            user.EnemyInvade.GET_Match.End(user);
                        break;
                    }
            }
        }
    }
}
