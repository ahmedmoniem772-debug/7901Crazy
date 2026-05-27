using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Client;
using VirusX.Database.DBActions;
using VirusX.Game.MsgServer;
using VirusX.ServerSockets;

namespace VirusX.Game.MsgTournaments
{
    public class MsgCaptureTheFlag
    {
        public Dictionary<uint, MsgCaptureTheFlag.VlmScoreInfo> VlmScoreInfoList = new Dictionary<uint, MsgCaptureTheFlag.VlmScoreInfo>();
        public const ushort MapID = 2057,

            X2CastleMinutes = 15,
            UpScoreBoardSecounds = 6;
        public class VlmScoreInfo
        {
            public uint UID;
            public ulong TotalKills = 0;
            public ulong MaxComboKill = 0;
            public ulong Revival = 0;
            public ulong Shackled = 0;
            public ulong UnShackled = 0;
            public uint UnShackle = 0;
            public ulong TotalDamage = 0;
            public ulong FlagsCaptured = 0;
            public ulong FlagsDelivered = 0;
            public ulong BasesOccupied = 0;
            public int ContributionPts;
            public uint Rating;
            public uint Rank;
            public string Name;
            public string GuildName;
            public uint Deaths;
            public string KMName;
            public uint KMCount;
            public string MKName;
            public uint MKCount;
            public ConcurrentDictionary<uint, MsgCaptureTheFlag.VlmScoreInfo.kiiled> MK = new ConcurrentDictionary<uint, MsgCaptureTheFlag.VlmScoreInfo.kiiled>();
            public ConcurrentDictionary<uint, MsgCaptureTheFlag.VlmScoreInfo.kiiled> KM = new ConcurrentDictionary<uint, MsgCaptureTheFlag.VlmScoreInfo.kiiled>();

            public class kiiled
            {
                public uint Count;
                public string Name;
            }
        }
        public void LoadVlmScore()
        {
            using (Read read = new Read("VlmScore.txt"))
            {
                if (!read.Reader())
                    return;
                int count = read.Count;
                for (int index = 0; index < count; ++index)
                {
                    ReadLine readLine = new ReadLine(read.ReadString("/"), '/');
                    MsgCaptureTheFlag.VlmScoreInfo vlmScoreInfo = new MsgCaptureTheFlag.VlmScoreInfo()
                    {
                        UID = readLine.Read(0U),
                        Name = readLine.Read(""),
                        ContributionPts = readLine.Read(0),
                        Deaths = readLine.Read(0U),
                        BasesOccupied = (ulong)readLine.Read(0U),
                        FlagsCaptured = (ulong)readLine.Read((byte)0),
                        FlagsDelivered = (ulong)readLine.Read(0U),
                        Rank = readLine.Read(0U),
                        Rating = readLine.Read(0U),
                        Revival = (ulong)readLine.Read(0U),
                        Shackled = (ulong)readLine.Read(0U),
                        UnShackled = (ulong)readLine.Read(0U)
                    };
                    vlmScoreInfo.Revival = (ulong)readLine.Read(0U);
                    vlmScoreInfo.TotalKills = (ulong)readLine.Read(0U);
                    vlmScoreInfo.TotalDamage = (ulong)readLine.Read(0U);
                    vlmScoreInfo.MaxComboKill = (ulong)readLine.Read(0U);
                    vlmScoreInfo.MKCount = readLine.Read(0U);
                    vlmScoreInfo.MKName = readLine.Read("");
                    vlmScoreInfo.KMCount = readLine.Read(0U);
                    vlmScoreInfo.KMName = readLine.Read("");
                    vlmScoreInfo.GuildName = readLine.Read("");
                    this.VlmScoreInfoList.Add(vlmScoreInfo.UID, vlmScoreInfo);
                }
            }
        }

        public void SaveVlmScore()
        {
            using (Write write = new Write("VlmScore.txt"))
            {
                foreach (MsgCaptureTheFlag.VlmScoreInfo vlmScoreInfo in this.VlmScoreInfoList.Values)
                {
                    WriteLine writeLine = new WriteLine('/');
                    writeLine.Add(vlmScoreInfo.UID).Add(vlmScoreInfo.Name).Add(vlmScoreInfo.ContributionPts).Add(vlmScoreInfo.Deaths).Add(vlmScoreInfo.BasesOccupied).Add(vlmScoreInfo.FlagsCaptured).Add(vlmScoreInfo.FlagsDelivered).Add(vlmScoreInfo.Rank).Add(vlmScoreInfo.Rating).Add(vlmScoreInfo.Revival).Add(vlmScoreInfo.Shackled).Add(vlmScoreInfo.UnShackled).Add(vlmScoreInfo.TotalKills).Add(vlmScoreInfo.TotalDamage).Add(vlmScoreInfo.MaxComboKill).Add(vlmScoreInfo.MKCount).Add(vlmScoreInfo.MKName == null ? "" : vlmScoreInfo.MKName).Add(vlmScoreInfo.KMCount).Add(vlmScoreInfo.KMName == null ? "" : vlmScoreInfo.KMName).Add(vlmScoreInfo.GuildName);
                    write.Add(writeLine.Close());
                }
                write.Execute(VirusX.Database.DBActions.Mode.Open);
            }
        }

        public void OrderList()
        {
            lock (this)
            {
                foreach (MsgCaptureTheFlag.VlmScoreInfo vlmScoreInfo in this.VlmScoreInfoList.Values.ToArray<MsgCaptureTheFlag.VlmScoreInfo>())
                {
                    GameClient gameClient;
                    if (Pool.GamePoll.TryGetValue(vlmScoreInfo.UID, out gameClient))
                        vlmScoreInfo.ContributionPts = (int)gameClient.Player.MyGuildMember.CTF_Exploits;
                }
                this.VlmScoreInfoList = ((IEnumerable<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>>)this.VlmScoreInfoList.Where<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>>((Func<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>, bool>)(i => i.Value.ContributionPts != 0)).ToArray<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>>()).OrderByDescending<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>, int>((Func<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>, int>)(p => p.Value.ContributionPts)).ToDictionary<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>, uint, MsgCaptureTheFlag.VlmScoreInfo>((Func<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>, uint>)(pair => pair.Key), (Func<KeyValuePair<uint, MsgCaptureTheFlag.VlmScoreInfo>, MsgCaptureTheFlag.VlmScoreInfo>)(pair => pair.Value));
                foreach (MsgCaptureTheFlag.VlmScoreInfo vlmScoreInfo in this.VlmScoreInfoList.Values.ToArray<MsgCaptureTheFlag.VlmScoreInfo>())
                {
                    vlmScoreInfo.Rank = (uint)(this.VlmScoreInfoList.Values.ToList<MsgCaptureTheFlag.VlmScoreInfo>().IndexOf(vlmScoreInfo) + 1);
                    if (vlmScoreInfo.Rank >= 1U && vlmScoreInfo.Rank <= 3U)
                        vlmScoreInfo.Rating = 0U;
                    else if (vlmScoreInfo.Rank >= 4U && vlmScoreInfo.Rank <= 8U)
                        vlmScoreInfo.Rating = 1U;
                    else if (vlmScoreInfo.Rank >= 9U && vlmScoreInfo.Rank <= 16U)
                        vlmScoreInfo.Rating = 2U;
                    else if (vlmScoreInfo.Rank >= 17U && vlmScoreInfo.Rank <= 31U)
                    {
                        vlmScoreInfo.Rating = 3U;
                    }
                    else
                    {
                        int num = vlmScoreInfo.Rank < 32U ? 0 : (vlmScoreInfo.Rank <= 54U ? 1 : 0);
                        vlmScoreInfo.Rating = num == 0 ? 5U : 4U;
                    }
                    MsgCaptureTheFlag.VlmScoreInfo.kiiled kiiled1 = ((IEnumerable<MsgCaptureTheFlag.VlmScoreInfo.kiiled>)vlmScoreInfo.MK.Values.OrderByDescending<MsgCaptureTheFlag.VlmScoreInfo.kiiled, uint>((Func<MsgCaptureTheFlag.VlmScoreInfo.kiiled, uint>)(p => p.Count)).ToArray<MsgCaptureTheFlag.VlmScoreInfo.kiiled>()).FirstOrDefault<MsgCaptureTheFlag.VlmScoreInfo.kiiled>();
                    if (kiiled1 != null)
                    {
                        vlmScoreInfo.MKName = kiiled1.Name;
                        vlmScoreInfo.MKCount = kiiled1.Count;
                    }
                    MsgCaptureTheFlag.VlmScoreInfo.kiiled kiiled2 = ((IEnumerable<MsgCaptureTheFlag.VlmScoreInfo.kiiled>)vlmScoreInfo.KM.Values.OrderByDescending<MsgCaptureTheFlag.VlmScoreInfo.kiiled, uint>((Func<MsgCaptureTheFlag.VlmScoreInfo.kiiled, uint>)(p => p.Count)).ToArray<MsgCaptureTheFlag.VlmScoreInfo.kiiled>()).FirstOrDefault<MsgCaptureTheFlag.VlmScoreInfo.kiiled>();
                    if (kiiled2 != null)
                    {
                        vlmScoreInfo.KMName = kiiled2.Name;
                        vlmScoreInfo.KMCount = kiiled2.Count;
                    }
                    if (Pool.GamePoll.ContainsKey(vlmScoreInfo.UID))
                        Pool.GamePoll[vlmScoreInfo.UID].Send(new RecycledPacket().GetStream().CreateMsgVlmScoreInfo(new MsgVlmScoreInfo.MsgVlmScoreInfoProto()
                        {
                            Type = 2U,
                            uk5 = 1U,
                            PersonalInfo = new MsgVlmScoreInfo.Personal()
                            {
                                Name = vlmScoreInfo.Name,
                                Rank = vlmScoreInfo.Rank,
                                Rating = vlmScoreInfo.Rating,
                                Deaths = vlmScoreInfo.Deaths,
                                GuildName = vlmScoreInfo.GuildName,
                                Flags = vlmScoreInfo.Rank == 1U ? uint.MaxValue : 0U,
                                CaptureTheFlagInfo = new ulong[14]
                            {
            (ulong) vlmScoreInfo.ContributionPts,
            vlmScoreInfo.TotalKills,
            vlmScoreInfo.MaxComboKill,
            vlmScoreInfo.Revival,
            vlmScoreInfo.Shackled,
            vlmScoreInfo.UnShackled,
            vlmScoreInfo.TotalDamage,
            vlmScoreInfo.FlagsCaptured,
            vlmScoreInfo.BasesOccupied,
            vlmScoreInfo.FlagsDelivered,
            0UL,
            0UL,
            0UL,
            0UL
                            }
                            }
                        }));
                }
                this.SaveVlmScore();
            }
        }
        public class Basse
        {
            public Role.SobNpc Npc;
            public System.SafeDictionary<uint, WarScrore> Scores = new System.SafeDictionary<uint, WarScrore>();
            public uint CapturerID = 0;
            public bool IsX2 = false;

            public class WarScrore
            {
                public uint GuildID;
                public string Name;
                public uint Score;
            }

            internal void UpdateScore(Role.Player client, uint Damage)
            {
                if (client.MyGuild == null)
                    return;
                if (!Scores.ContainsKey(client.GuildID))
                {
                    Scores.Add(client.GuildID, new WarScrore()
                    {
                        GuildID = client.MyGuild.Info.GuildID,
                        Name = client.MyGuild.GuildName,
                        Score = Damage
                    });
                }
                else
                {
                    Scores[client.MyGuild.Info.GuildID].Score += Damage;
                }
            }
        }

        public Role.GameMap Map;
        public uint X2Castle = 0;


        public ProcesType Proces;
        public System.SafeDictionary<uint, Basse> Bases;
        public DateTime UpdateStampScore = new DateTime();
        public DateTime SendX2LoctionStamp = new DateTime();
        public DateTime TournamentStamp = new DateTime();

        public MsgCaptureTheFlag()
        {
            Proces = ProcesType.Dead;
            Bases = new System.SafeDictionary<uint, Basse>();
            Pool.Constants.FreePkMap.Add(MapID);
            this.LoadVlmScore();
        }

        public System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.Guild> RegistredGuilds = new System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.Guild>();
        public void Start()
        {
            if (Proces == ProcesType.Dead)
            {
                TournamentStamp = DateTime.Now;

                Bases.Clear();
                CreateBases();
                if (Program.ServerConfig.IsInterServer)
                {
                    foreach (var guild in Role.Instance.Guild.GuildPoll.Values)
                    {
                        guild.CTF_ExploitsCross = 0;

                        foreach (var user in guild.Members.Values)
                        {
                            user.CTF_ExploitsCross = 0;
                        }
                    }
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        MsgInterServer.PipeServer.Send(new MsgMessage("[Cross Capture The Flag] begins at 22:00. Get yourself prepared for it!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.BroadcastMessage).GetArray(stream));
                    }
                }
                else
                {
                    foreach (var guild in Role.Instance.Guild.GuildPoll.Values)
                    {
                        guild.ClaimCtfReward = 0;
                        guild.CTF_Exploits = 0;
                        guild.CTF_Rank = 0;

                        foreach (var user in guild.Members.Values)
                        {
                            user.CTF_Exploits = 0;
                            user.RewardConquerPoints = 0;
                            user.RewardMoney = 0;
                            user.CTF_Claimed = 0;
                        }
                    }
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        foreach (var user in Pool.GamePoll.Values)
                            user.Player.MessageBox("", new Action<Client.GameClient>(p =>
                            {
                                p.Teleport(329, 322, 1002);
                            }), null
                            , 60, MsgServer.MsgStaticMessage.Messages.CapturetheFlag);
                    }
                }
                RegistredGuilds = new System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.Guild>();
                VlmScoreInfoList = new Dictionary<uint, MsgCaptureTheFlag.VlmScoreInfo>();
                GenerateX2Castle();
                Proces = ProcesType.Alive;


            }
        }
        public void CheckFinish()
        {
            if (Proces == ProcesType.Alive)
            {
                Proces = ProcesType.Dead;

                var array = Role.Instance.Guild.GuildPoll.Values.Where(p => p.CTF_Exploits != 0).ToArray();
                var ranks = array.OrderByDescending(p => p.CTF_Exploits).ToArray();
                for (int x = 0; x < Math.Min(9, ranks.Length); x++)
                    ranks[x].CTF_Rank = (byte)(x + 1);

                foreach (var guild in Role.Instance.Guild.GuildPoll.Values)
                {

                    if (RegistredGuilds.ContainsKey(guild.Info.GuildID))
                    {
                        var array_members = guild.Members.Values.Where(p => p.CTF_Exploits != 0).ToArray();
                        var Ranks_members = array_members.OrderByDescending(p => p.CTF_Exploits).ToArray();
                        for (int x = 0; x < Ranks_members.Length; x++)
                        {
                            var rank = CalculateMemberRewardCTF((uint)(x + 1), guild);
                            Ranks_members[x].RewardConquerPoints = rank[0];
                            Ranks_members[x].RewardMoney = rank[1];
                        }
                    }
                    guild.CTF_Next_ConquerPoints = 0;
                    guild.CTF_Next_Money = 0;
                }
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
#if Arabic
                        Server.SendGlobalPacket(new MsgMessage("Capture The Flag has finished.", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
#else
                    Server.SendGlobalPacket(new MsgMessage("Capture The Flag has finished.", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
#endif

                }

                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user.Player.Map == MapID)
                        user.Teleport(410, 354, 1002);
                }
            }
            this.OrderList();
        }
        private uint[] CalculateMemberRewardCTF(uint Rank, Role.Instance.Guild guild)
        {
            uint[] rew = new uint[2];
            rew[0] = (guild.CTF_Next_ConquerPoints / (Rank + 1));
            rew[1] = (guild.CTF_Next_Money / (Rank + 1));
            return rew;
        }
        public void GenerateX2Castle()
        {
            int random = Pool.GetRandom.Next(0, Bases.Count);
            var basse = Bases.Values.ToArray()[X2Castle];
            basse.IsX2 = false;
            X2Castle = (uint)random;
            UpdateMapX2Location();
        }
        public void UpdateMapX2Location()
        {
            var basse = Bases.Values.ToArray()[X2Castle];
            basse.IsX2 = true;
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.X2Location, X2Castle + 1, 1);
                stream.AddX2LocationCaptureTheFlagUpdate(basse.Npc.X, basse.Npc.Y);
                stream.CaptureTheFlagUpdateFinalize();
                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user.Player.Map == MapID && user.Player.DynamicID == 0)
                    {
                        user.Send(stream);
                    }
                }
            }
        }
        public bool Join(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (Proces == ProcesType.Alive)
            {
                if (!RegistredGuilds.ContainsKey(user.Player.GuildID))
                {
                    RegistredGuilds.TryAdd(user.Player.GuildID, user.Player.MyGuild);
                }

                if (user.Player.MyGuild != null && !this.VlmScoreInfoList.ContainsKey(user.Player.UID))
                    this.VlmScoreInfoList.Add(user.Player.UID, new MsgCaptureTheFlag.VlmScoreInfo()
                    {
                        UID = user.Player.UID,
                        GuildName = user.Player.MyGuild.GuildName,
                        Name = user.Player.Name
                    });
                stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.InitializeCTF, 0, 0);
                stream.CaptureTheFlagUpdateFinalize();
                user.Send(stream);


                var basse = Bases.Values.ToArray()[X2Castle];
                stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.X2Location, X2Castle + 1, 1);
                stream.AddX2LocationCaptureTheFlagUpdate(basse.Npc.X, basse.Npc.Y);
                stream.CaptureTheFlagUpdateFinalize();
                user.Send(stream);

                user.Teleport(478, 373, MapID);
                return true;
            }
            return false;
        }
        public void CreateBases()
        {
            Map = Pool.ServerMaps[MapID];

            foreach (var npc in Map.View.GetAllMapRoles(Role.MapObjectType.SobNpc))
                Bases.Add(npc.UID, new Basse() { Npc = npc as Role.SobNpc });

            SpawnFlags();
        }
        public void CheckUpX2()
        {
            if (Proces == ProcesType.Alive)
            {
                if (DateTime.Now > SendX2LoctionStamp.AddMinutes(X2CastleMinutes))
                {
                    GenerateX2Castle();
                    
                    SendX2LoctionStamp = DateTime.Now;
                    foreach (var Bot in Pool.GamePoll.Values)
                    {
                        if (Bot.MyBot != null)
                        {
                            if (Bot.MyBot.Type == BotType.EventBots)
                            {
                                if (Bot.Player.EndTeleCaptureTheFlag)
                                    Bot.Player.EndTeleCaptureTheFlag = false;
                            }
                        }
                    }
                }
            }
        }

        public void SpawnFlags()
        {
            for (int i = 40 - Map.View.GetAllMapRolesCount(Role.MapObjectType.Npc, x => x.UID == (uint)(x.X * 1000 + x.Y)); i > 0; i--)
            {
                ushort x = 0; ushort y = 0;
                Map.GetRandCoord(ref x, ref y);
                if (!InMainCastle(x, y))
                {
                    var npc = Game.MsgNpc.Npc.Create();
                    npc.UID = (uint)(x * 1000 + y);
                    npc.Mesh = 8910;
                    npc.Map = MapID;
                    npc.X = x;
                    npc.Y = y;
                    npc.NpcType = Role.Flags.NpcType.Flag;

                    Map.AddNpc(npc);
                }
            }
        }
        public void UpdateMapScore()
        {
            if (Proces == ProcesType.Alive)
            {
                if (DateTime.Now > UpdateStampScore.AddSeconds(UpScoreBoardSecounds))
                {
                    SendUpdateBoardScore();
                    UpdateStampScore = DateTime.Now;
                }
            }
        }
        public void SendUpdateBoardScore()
        {

            var array = RegistredGuilds.Values.Where(p => p.CTF_Exploits != 0).ToArray();
            var rank = array.OrderByDescending(p => p.CTF_Exploits).ToArray();
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.InitializeCTF, 0, 0);
                stream.CaptureTheFlagUpdateFinalize();
                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user.Player.Map == MapID && user.Player.DynamicID == 0)
                        user.Send(stream);
                }



                stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.ScoreUpdate, 2, (uint)Math.Min(rank.Length, 5));

                for (uint x = 0; x < rank.Length; x++)
                {
                    if (x == 4)
                        break;
                    var element = rank[x];

                    stream.AddItemCaptureTheFlagUpdate(x, element.CTF_Exploits, element.GuildName);
                }

                stream.CaptureTheFlagUpdateFinalize();
                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user.Player.Map == MapID && user.Player.DynamicID == 0)
                        user.Send(stream);
                }

                //send base score.
                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user.Player.Map == MapID && user.Player.DynamicID == 0)
                    {
                        Basse flag_base;
                        if (TryGetBase(user, out flag_base))
                        {
                            var array_scorebasse = flag_base.Scores.Values.OrderByDescending(p => p.Score).ToArray();
                            stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.ScoreBase, 0, (uint)Math.Min(array_scorebasse.Length, 5));
                            for (uint x = 0; x < array_scorebasse.Length; x++)
                            {
                                if (x == 4)
                                    break;
                                var element = array_scorebasse[x];
                                stream.AddItemCaptureTheFlagUpdate(x, element.Score, element.Name);
                            }
                            stream.CaptureTheFlagUpdateFinalize();
                            user.Send(stream);
                        }
                    }
                }
            }

        }
        public bool TryGetBase(Client.GameClient user, out Basse bas)
        {
            if (user.Player.Map == MapID && user.Player.DynamicID == 0)
            {
                foreach (var flag_base in Bases.Values)
                {
                    if (Role.Core.GetDistance(user.Player.X, user.Player.Y, flag_base.Npc.X, flag_base.Npc.Y) <= 11)
                    {
                        bas = flag_base;
                        return true;
                    }
                }
            }
            bas = null;
            return false;
        }
        public void Location()
        {
            foreach (var user in Pool.GamePoll.Values)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    if (user.Player.Map == 2057 && user.Player.DynamicID == 0)
                    {
                        if (user != null && this.Bases != null)
                        {
                            Basse[] array = this.Bases.Values.Where((p => p.Npc.Name == user.Player.MyGuild.GuildName)).ToArray();
                            stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.Location, user.Player.UID, (uint)array.Length);
                            for (int index = 0; index < array.Length; ++index)
                            {
                                Basse basse = array[index];
                                stream.AddX2LocationCaptureTheFlagUpdate(basse.Npc.X, basse.Npc.Y);
                            }
                            user.Send(stream.CaptureTheFlagUpdateFinalize());
                        }
                    }
                }
            }
        }
        public void UpdateFlagScore(Role.Player client, Role.SobNpc Attacked, uint Damage, ServerSockets.Packet stream)
        {
            if (Proces != ProcesType.Alive)
                return;
            if (client.MyGuild == null)
                return;
            Basse Bas;
            if (Bases.TryGetValue(Attacked.UID, out Bas))
            {
                Bas.UpdateScore(client, Damage);
                if (this.VlmScoreInfoList.ContainsKey(client.UID))
                    this.VlmScoreInfoList[client.UID].TotalDamage += (ulong)Damage;
                if (Bas.Npc.HitPoints == 0)
                {
                    if (this.VlmScoreInfoList.ContainsKey(client.UID))
                        ++this.VlmScoreInfoList[client.UID].BasesOccupied;
                    var array = Bas.Scores.Values.OrderByDescending(p => p.Score).ToArray();
                    var GuildWinner = array.First();

                    Bas.CapturerID = GuildWinner.GuildID;

                    Bas.Scores.Clear();
                    Bas.Npc.HitPoints = Bas.Npc.MaxHitPoints;
                    Bas.Npc.Name = GuildWinner.Name;


                    foreach (var user in Pool.GamePoll.Values)
                    {
                        if (user.Player.Map == MapID && user.Player.DynamicID == 0)
                        {
                            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, Bas.Npc.X, Bas.Npc.Y) <= 9)
                            {
                                MsgServer.MsgUpdate upd = new MsgServer.MsgUpdate(stream, Bas.Npc.UID, 2);
                                stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.Mesh, (long)Bas.Npc.Mesh);
                                stream = upd.GetArray(stream);
                                user.Send(stream);
                                stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.Hitpoints, Bas.Npc.HitPoints);
                                stream = upd.GetArray(stream);
                                user.Send(stream);
                                user.Send(Bas.Npc.GetArray(stream, false));
                            
                                  
                            }
                        }
                    }
                    this.Location();
                    stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.OccupiedBase, (byte)(Bas.Npc.UID % 100), 0);
                    stream.CaptureTheFlagUpdateFinalize();
                    SendMapPacket(stream);
                }
            }
        }
        public void SendMapPacket(ServerSockets.Packet stream)
        {
            foreach (var user in Pool.GamePoll.Values)
            {
                if (user.Player.Map == MapID && user.Player.DynamicID == 0)
                    user.Send(stream);
            }
        }
        public bool Attackable(Role.Player user)
        {
            return !InMainCastle(user.X, user.Y);
        }
        public bool InMainCastle(ushort X, ushort Y)
        {
            return Role.Core.GetDistance(X, Y, 482, 367) < 32;
        }
        public void PlantTheFlag(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (Proces != ProcesType.Alive)
                return;
            if (user.Player.Map == MapID)
            {
                if (user.Player.MyGuild == null)
                    return;

                if (user.Player.ContainFlag(MsgUpdate.Flags.CTF_Flag))
                {
                    Basse flag_base;
                    if (this.VlmScoreInfoList.ContainsKey(user.Player.UID))
                        ++this.VlmScoreInfoList[user.Player.UID].FlagsDelivered;
                    if (TryGetBase(user, out flag_base))
                    {
                        if (flag_base.CapturerID == user.Player.GuildID)
                        {
                            user.Player.RemoveFlag(MsgUpdate.Flags.CTF_Flag);

                            uint exploits = 15;

                            if (flag_base.IsX2)
                                exploits *= 2;

                            user.Player.MyGuild.CTF_Exploits += exploits;
                            user.Player.MyGuildMember.CTF_Exploits += exploits;


                            stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.GenerateTimer, 0, user.Player.UID);
                            stream.CaptureTheFlagUpdateFinalize();
                            user.Send(stream);

                            stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.GenerateEffect, user.Player.UID);
                            stream.CaptureTheFlagUpdateFinalize();
                            user.Send(stream);

                            stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.RemoveFlagEffect, user.Player.UID);
                            stream.CaptureTheFlagUpdateFinalize();
                            user.Send(stream);
                        }
                    }
                }
            }
        }
        public void ChechMoveFlag(Client.GameClient user)
        {
            if (Proces != ProcesType.Alive)
                return;
            if (user.Player.Map == MapID)
            {
                if (user.Player.MyGuild == null)
                    return;
                if (!user.Player.ContainFlag(MsgUpdate.Flags.CTF_Flag))
                {
                    foreach (var flag in user.Map.View.Roles(Role.MapObjectType.Npc, user.Player.X, user.Player.Y))
                    {
                        if (Role.Core.GetDistance(user.Player.X, user.Player.Y, flag.X, flag.Y) < 2)
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                if (this.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                    ++this.VlmScoreInfoList[user.Player.UID].FlagsCaptured;
                                user.Player.AddFlag(MsgServer.MsgUpdate.Flags.CTF_Flag, 60, true);

                                stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.GenerateTimer, 60, user.Player.UID);
                                stream.CaptureTheFlagUpdateFinalize();
                                user.Send(stream);

                                user.Player.MyGuild.CTF_Exploits += 3;
                                user.Player.MyGuildMember.CTF_Exploits += 3;


                                stream.CaptureTheFlagUpdateCreate(MsgCaptureTheFlagUpdate.Mode.GenerateEffect, user.Player.UID);
                                stream.CaptureTheFlagUpdateFinalize();
                                user.Send(stream);


                                user.Map.View.LeaveMap<Role.IMapObj>(flag);

                                ActionQuery action;

                                action = new ActionQuery()
                                {
                                    ObjId = flag.UID,
                                    Type = ActionType.RemoveEntity
                                };
                                unsafe
                                {
                                    user.Player.View.SendView(stream.ActionCreate(action), true);
                                }

                            }

                            break;
                        }
                    }
                }
            }
        }
    }
}
