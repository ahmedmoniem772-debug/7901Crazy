using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using ProtoBuf;
using VirusX.Role.Instance;
using VirusX.Game.MsgTournaments;
using VirusX.Role;
using VirusX.Game.MsgServer;
using VirusX.Client;
using VirusX.Game.MsgNpc;
using VirusX.Database.DBActions;
using VirusX.ServerSockets;

namespace VirusX
{
  
    public class MsgGuildWar
    {
        public class DataFlameQuest
        {
            public List<uint> Registred;
            public bool ActiveFlame10;

            public DataFlameQuest()
            {
                Registred = new List<uint>();
                ActiveFlame10 = false;
            }
        }
        public DataFlameQuest FlamesQuest;
        public Dictionary<uint, MsgGuildWar.VlmScoreInfo> VlmScoreInfoList = new Dictionary<uint, MsgGuildWar.VlmScoreInfo>();
        public class VlmScoreInfo
        {
            public uint UID;
            public ulong TotalKills = 0;
            public ulong Revival = 0;
            public ulong Shackled = 0;
            public ulong UnShackled = 0;
            public ulong TotalDamage = 0;
            public ulong TotalDamageBuffPillar = 0;
            public ulong TotalDamageGates = 0;
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
            public ConcurrentDictionary<uint, MsgGuildWar.VlmScoreInfo.kiiled> MK = new ConcurrentDictionary<uint, MsgGuildWar.VlmScoreInfo.kiiled>();
            public ConcurrentDictionary<uint, MsgGuildWar.VlmScoreInfo.kiiled> KM = new ConcurrentDictionary<uint, MsgGuildWar.VlmScoreInfo.kiiled>();

            public class kiiled
            {
                public uint Count;
                public string Name;
            }
        }
        public void LoadVlmScore()
        {
            using (Read read = new Read("VlmGuildwar.txt"))
            {
                if (!read.Reader())
                    return;
                int count = read.Count;
                for (int index = 0; index < count; ++index)
                {
                    ReadLine readLine = new ReadLine(read.ReadString("/"), '/');
                    MsgGuildWar.VlmScoreInfo vlmScoreInfo = new MsgGuildWar.VlmScoreInfo();
                    vlmScoreInfo.UID = readLine.Read(0U);
                    vlmScoreInfo.Name = readLine.Read("");
                    vlmScoreInfo.ContributionPts = readLine.Read(0);
                    vlmScoreInfo.Deaths = readLine.Read(0U);
                    vlmScoreInfo.Rank = readLine.Read(0U);
                    vlmScoreInfo.Rating = readLine.Read(0U);
                    vlmScoreInfo.Revival = (ulong)readLine.Read(0U);
                    vlmScoreInfo.Shackled = (ulong)readLine.Read(0U);
                    vlmScoreInfo.TotalDamageBuffPillar = (ulong)readLine.Read(0U);
                    vlmScoreInfo.TotalDamageGates = (ulong)readLine.Read(0U);
                    vlmScoreInfo.UnShackled = (ulong)readLine.Read(0U);
                    vlmScoreInfo.TotalKills = (ulong)readLine.Read(0U);
                    vlmScoreInfo.TotalDamage = (ulong)readLine.Read(0U);
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
            using (Write write = new Write("VlmGuildwar.txt"))
            {
                foreach (MsgGuildWar.VlmScoreInfo vlmScoreInfo in this.VlmScoreInfoList.Values)
                {
                    WriteLine writeLine = new WriteLine('/');
                    writeLine.Add(vlmScoreInfo.UID).Add(vlmScoreInfo.Name).Add(vlmScoreInfo.ContributionPts).Add(vlmScoreInfo.Deaths).Add(vlmScoreInfo.Rank).Add(vlmScoreInfo.Rating).Add(vlmScoreInfo.Revival).Add(vlmScoreInfo.Shackled).Add(vlmScoreInfo.UnShackled).Add(vlmScoreInfo.TotalKills).Add(vlmScoreInfo.TotalDamage).Add(vlmScoreInfo.TotalDamageBuffPillar).Add(vlmScoreInfo.TotalDamageGates).Add(vlmScoreInfo.MKCount).Add(vlmScoreInfo.MKName == null ? "" : vlmScoreInfo.MKName).Add(vlmScoreInfo.KMCount).Add(vlmScoreInfo.KMName == null ? "" : vlmScoreInfo.KMName).Add(vlmScoreInfo.GuildName);
                    write.Add(writeLine.Close());
                }
                write.Execute(VirusX.Database.DBActions.Mode.Open);
            }
        }

        public void OrderList()
        {
            lock (this)
            {
                foreach (MsgGuildWar.VlmScoreInfo vlmScoreInfo in this.VlmScoreInfoList.Values)
                {
                    GameClient gameClient;
                    if (Pool.GamePoll.TryGetValue(vlmScoreInfo.UID, out gameClient) && gameClient.Player.MyGuildMember.GuildWarPoints > 0U)
                        vlmScoreInfo.ContributionPts = (int)gameClient.Player.MyGuildMember.GuildWarPoints;
                }
                this.VlmScoreInfoList = ((IEnumerable<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>>)this.VlmScoreInfoList.Where<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>>((Func<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>, bool>)(i => i.Value.ContributionPts != 0)).ToArray<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>>()).OrderByDescending<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>, int>((Func<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>, int>)(p => p.Value.ContributionPts)).ToDictionary<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>, uint, MsgGuildWar.VlmScoreInfo>((Func<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>, uint>)(pair => pair.Key), (Func<KeyValuePair<uint, MsgGuildWar.VlmScoreInfo>, MsgGuildWar.VlmScoreInfo>)(pair => pair.Value));
                foreach (MsgGuildWar.VlmScoreInfo vlmScoreInfo in this.VlmScoreInfoList.Values)
                {
                    vlmScoreInfo.Rank = (uint)(this.VlmScoreInfoList.Values.ToList<MsgGuildWar.VlmScoreInfo>().IndexOf(vlmScoreInfo) + 1);
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
                    MsgGuildWar.VlmScoreInfo.kiiled kiiled1 = ((IEnumerable<MsgGuildWar.VlmScoreInfo.kiiled>)vlmScoreInfo.MK.Values.OrderByDescending<MsgGuildWar.VlmScoreInfo.kiiled, uint>((Func<MsgGuildWar.VlmScoreInfo.kiiled, uint>)(p => p.Count)).ToArray<MsgGuildWar.VlmScoreInfo.kiiled>()).FirstOrDefault<MsgGuildWar.VlmScoreInfo.kiiled>();
                    if (kiiled1 != null)
                    {
                        vlmScoreInfo.MKName = kiiled1.Name;
                        vlmScoreInfo.MKCount = kiiled1.Count;
                    }
                    MsgGuildWar.VlmScoreInfo.kiiled kiiled2 = ((IEnumerable<MsgGuildWar.VlmScoreInfo.kiiled>)vlmScoreInfo.KM.Values.OrderByDescending<MsgGuildWar.VlmScoreInfo.kiiled, uint>((Func<MsgGuildWar.VlmScoreInfo.kiiled, uint>)(p => p.Count)).ToArray<MsgGuildWar.VlmScoreInfo.kiiled>()).FirstOrDefault<MsgGuildWar.VlmScoreInfo.kiiled>();
                    if (kiiled2 != null)
                    {
                        vlmScoreInfo.KMName = kiiled2.Name;
                        vlmScoreInfo.KMCount = kiiled2.Count;
                    }
                    if (Pool.GamePoll.ContainsKey(vlmScoreInfo.UID))
                        Pool.GamePoll[vlmScoreInfo.UID].Send(new RecycledPacket().GetStream().CreateMsgVlmScoreInfo(new MsgVlmScoreInfo.MsgVlmScoreInfoProto()
                        {
                            Type = 9U,
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
           1UL,
           vlmScoreInfo.Revival,
           vlmScoreInfo.Shackled,
           vlmScoreInfo.UnShackled,
           vlmScoreInfo.TotalDamage,
           2UL,
           3UL,
           4UL,
           vlmScoreInfo.TotalDamageGates,
           vlmScoreInfo.TotalDamage,
           vlmScoreInfo.TotalDamageBuffPillar,
           0UL
                            }
                            }
                        }));
                }
                this.SaveVlmScore();
            }
        }
        public Dictionary<Role.SobNpc.StaticMesh, Role.SobNpc> Furnitures { get; set; }
        internal ushort[][] StatueCoords = { new ushort[] { 140, 134 }, new ushort[] { 144, 124 }, new ushort[] { 130, 138 }, new ushort[] { 153, 124 }, new ushort[] { 161, 124 }, new ushort[] { 130, 147 }, new ushort[] { 130, 155 } };
        [Flags]
        public enum StaticMesh : uint
        {
            Pole = 1137,
            RightGate = 1000,
            LeftGate = 1001,
            RightGate_Close_1 = 1467,
            RightGate_Open_1 = 1477,
            RightGate_Demolition_1 = 1487,
            LeftGate_Close_1 = 1491,
            LeftGate_Open_1 = 1501,
            LeftGate_Demolition_1 = 1511,
            RightGate_Close_2 = 1527,
            RightGate_Open_2 = 1537,
            RightGate_Demolition_2 = 1547,
            LeftGate_Close_2 = 1551,
            LeftGate_Open_2 = 1561,
            LeftGate_Demolition_2 = 1571,
            RightGate_Close_3 = 1587,
            RightGate_Open_3 = 1597,
            RightGate_Demolition_3 = 1607,
            LeftGate_Close_3 = 1611,
            LeftGate_Open_3 = 1621,
            LeftGate_Demolition_3 = 1631,
            RightGate_Close_4 = 1647,
            RightGate_Open_4 = 1657,
            RightGate_Demolition_4 = 1667,
            LeftGate_Close_4 = 1671,
            LeftGate_Open_4 = 1681,
            LeftGate_Demolition_4 = 1691,
        }
        [Flags]
        public enum NPCID : uint
        {
            RightGate = 0,
            LeftGate,
            TenacityPillar,

            ExorcimPillar,
            GuildPillar,
            GuildWithHonorPillar
        }
        public class GuildRank
        {
            public uint GuildID;
            public string Name;
            public uint Points;
            public uint Rank;
        }
        public class GuildRankTop4
        {
            public uint GuildID;
            public string Name;
            public uint Points;
            public uint Rank;
        }
        public class UserRank
        {
            public string Name;
            public uint Points;
            public uint UID;
            public uint Rank;
        }
        public class NpcInfo
        {
            public NPCID type;
            public uint GuildID;
            public Role.SobNpc Npc;
            public ConcurrentDictionary<uint, uint> SCORE_LIST;
        }
        public class ScoreList
        {
            public const int ConquerPointsReward = 30000000;
            public uint GuildID;
            public string Name;
            public ulong Score;
            public int LeaderReward = 1;
            public int LeaderRewardTop = 1;
            public int DeputiLeaderReward = 7;
        }
        public class GuildConductor
        {
            public static List<uint> BlockMaps = new List<uint>()
                    {
                        1038
                    };
            public Game.MsgNpc.Npc Npc;
            public uint ToMap;
            public ushort ToX, ToY;
            public override string ToString()
            {
                Database.DBActions.WriteLine Line = new Database.DBActions.WriteLine(',');
                Line.Add(Npc.UID).Add(Npc.X).Add(Npc.Y).Add(Npc.Map).Add(Npc.Mesh).Add((ushort)Npc.NpcType)
                    .Add(ToX).Add(ToY).Add(ToMap);
                return Line.Close();
            }
            public void Load(string Line, Game.MsgNpc.NpcID UID)
            {
                Npc = Game.MsgNpc.Npc.Create();
                if (Line == "")
                {
                    Npc.UID = (uint)UID;
                    return;
                }
                Database.DBActions.ReadLine Reader = new Database.DBActions.ReadLine(Line, ',');
                Npc.UID = Reader.Read((uint)0);
                Npc.X = Reader.Read((ushort)0);
                Npc.Y = Reader.Read((ushort)0);
                Npc.Map = Reader.Read((ushort)0);
                Npc.Mesh = Reader.Read((ushort)0);
                Npc.NpcType = (Role.Flags.NpcType)Reader.Read((ushort)0);
                ToX = Reader.Read((ushort)0);
                ToY = Reader.Read((ushort)0);
                ToMap = Reader.Read((ushort)0);
            }
            public static bool ChangeNpcLocation(Role.GameMap map, ref ushort X, ref ushort Y, ref uint Map)
            {
                return false;
            }
            public void GetCoords(out ushort x, out ushort y, out uint map)
            {
                if (ToMap != 0 && ToX != 0 && ToY != 0)
                {
                    x = ToX;
                    y = ToY;
                    map = ToMap;
                    return;
                }
                x = 300;
                y = 278;
                map = 1002;
            }
        }
        public ConcurrentDictionary<uint, GuildRankTop4> GUILD_RANK_TOP4;
        public ConcurrentDictionary<uint, GuildRank> GUILD_RANK;
        public ConcurrentDictionary<uint, UserRank> USER_RANK;
        public ConcurrentDictionary<NPCID, NpcInfo> NPC_INFO;
        public ConcurrentDictionary<uint, ScoreList> SCORE_LIST;
        public ConcurrentDictionary<Game.MsgNpc.NpcID, GuildConductor> GuildConductors;
        public GameMap Map;
        public ScoreList Winner;
        public ProcesType Proces { get; set; }
        public DateTime StampUpdate = new DateTime();
        public DateTime Stamp = new DateTime();
        public Time32 TimeLeft;
        public List<uint> RewardLeader = new List<uint>();
        public List<uint> RewardDeputiLeader = new List<uint>();
        public ushort CountDown
        {
            get
            {
                int value = (int)((TimeLeft.TotalMilliseconds - Time32.Now.TotalMilliseconds) / 1000);
                if (value <= 0)
                    return 0;
                return (ushort)value;
            }
        }
        public const uint MapID = 1038;
        public bool S_30 = false;
        public bool TotalPointsEnd = false;
        public unsafe void Began()
        {
            if (Proces == ProcesType.Idle)
            {
                Proces = ProcesType.Alive;
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
#if Arabic
                         Server.SendGlobalPacket(new MsgServer.MsgMessage("Guild war has began!", MsgServer.MsgMessage.MsgColor.white, MsgServer.MsgMessage.ChatMode.System).GetArray(stream));
               
#else
                    Server.SendGlobalPacket(new VirusX.Game.MsgServer.MsgMessage("Guild war has began!", VirusX.Game.MsgServer.MsgMessage.MsgColor.white, VirusX.Game.MsgServer.MsgMessage.ChatMode.System).GetArray(stream));

#endif
                }
            }
        }
        public MsgGuildWar()
        {
            Winner = new ScoreList();
            GUILD_RANK_TOP4 = new ConcurrentDictionary<uint, GuildRankTop4>();
            SCORE_LIST = new ConcurrentDictionary<uint, ScoreList>();
            GUILD_RANK = new ConcurrentDictionary<uint, GuildRank>();
            USER_RANK = new ConcurrentDictionary<uint, UserRank>();
            NPC_INFO = new ConcurrentDictionary<NPCID, NpcInfo>();
            GuildConductors = new ConcurrentDictionary<Game.MsgNpc.NpcID, GuildConductor>();
            this.LoadVlmScore();
        }
        public void CreateFurnitures()
        {
            //101315,LeftGate,26,251,1038,165,210,10000000,10000000,21,0
            //101314,RightGate,26,277,1038,223,183,10000000,10000000,21,0
            //101097,ExorcimPillar,64,2797,1038,186,167,120000000,120000000,21,0
            //100942,TenacityPillar,64,1427,1038,231,278,120000000,120000000,21,0
            Map = Pool.ServerMaps[1038];
            AddNpc(NPCID.GuildPillar, 810);
            AddNpc(NPCID.LeftGate, 101315);
            AddNpc(NPCID.RightGate, 101314);
            AddNpc(NPCID.ExorcimPillar, 101097);

            AddNpc(NPCID.TenacityPillar, 100942);
        }
        public void AddHonorPillar()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                Role.SobNpc npc = new Role.SobNpc();
                npc.ObjType = Role.MapObjectType.SobNpc;
                npc.UID = 100900;
                npc.Name = "HonorPillar";
                npc.Type = (Role.Flags.NpcType)10;
                npc.Mesh = (Role.SobNpc.StaticMesh)3457;
                npc.Map = MapID;
                npc.X = 80;
                npc.Y = 82;
                npc.Sort = 17;
                npc.MaxHitPoints = npc.HitPoints = 120000000;
                if (Pool.ServerMaps.ContainsKey(npc.Map))
                {
                    Map.View.EnterMap<Role.IMapObj>(npc);
                    Map.SetFlagNpc(npc.X, npc.Y);
                }
                npc.SendView(npc.GetArray(stream, false), Map);
                AddNpc(NPCID.GuildWithHonorPillar, 100900);
            }
        }
        public void AddNpc(NPCID type, uint npcid)
        {
            try
            {
                NpcInfo obj = new NpcInfo();
                obj.type = type;
                obj.Npc = Map.View.GetMapObject<Role.SobNpc>(MapObjectType.SobNpc, npcid);
                NPC_INFO.TryAdd(obj.type, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void Start()
        {
            try
            {

                if (Program.ServerConfig.IsInterServer)
                {
                    S_30 = true;
                    Winner = new ScoreList();
                    GUILD_RANK_TOP4 = new ConcurrentDictionary<uint, GuildRankTop4>();
                    SCORE_LIST = new ConcurrentDictionary<uint, ScoreList>();
                    GUILD_RANK = new ConcurrentDictionary<uint, GuildRank>();
                    USER_RANK = new ConcurrentDictionary<uint, UserRank>();
                    StampUpdate = DateTime.Now.AddSeconds(10);
                    Stamp = DateTime.Now.AddSeconds(1);
                    Proces = ProcesType.Alive;
                    NPC_INFO[NPCID.GuildPillar].Npc.Name = "GuildPillar";
                    ResetNpc(true);
               
                    TimeLeft = Time32.Now.AddMinutes(60);
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_SYN_WAR_START@@", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_SYN_WAR_START@@", MsgMessage.MsgColor.red, MsgMessage.ChatMode.System).GetArray(stream));

                        MsgInterServer.PipeServer.Send(new MsgMessage("[Cross Guild War] begins at 21:00. Get yourself prepared for it!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.BroadcastMessage).GetArray(stream));
                    }
                }
                else
                {
                    if (S_30)
                        return;
                    if (DateTime.Now.Minute == 60)
                        return;
                    S_30 = true;

                    Winner = new ScoreList();
                    GUILD_RANK_TOP4 = new ConcurrentDictionary<uint, GuildRankTop4>();
                    SCORE_LIST = new ConcurrentDictionary<uint, ScoreList>();
                    GUILD_RANK = new ConcurrentDictionary<uint, GuildRank>();
                    USER_RANK = new ConcurrentDictionary<uint, UserRank>();
                    StampUpdate = DateTime.Now.AddSeconds(10);
                    Stamp = DateTime.Now.AddSeconds(1);
                    Proces = ProcesType.Alive;
                    NPC_INFO[NPCID.GuildPillar].Npc.Name = "GuildPillar";
                    ResetNpc(true);
                    TimeLeft = Time32.Now.AddSeconds((60 - DateTime.Now.Minute) * 60);
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        NpcInfo nps;
                        if (NPC_INFO.TryRemove(NPCID.GuildWithHonorPillar, out nps))
                            Map.RemoveNpc(nps.Npc, stream);
                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_SYN_WAR_START@@", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                        Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_SYN_WAR_START@@", MsgMessage.MsgColor.red, MsgMessage.ChatMode.System).GetArray(stream));
                        foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                        {
                            if (client != null && client.Socket != null && client.Socket.Alive && client.Player != null && client.Player.Map == 1038)
                                MsgNewSynWar.Process(client, new MsgNewSynWar() { Type = 5 });
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void FinishRound()
        {
            var array = SCORE_LIST.Values.ToArray().OrderByDescending(P => P.Score).ToArray().FirstOrDefault();
            if (array != null && array.GuildID != Winner.GuildID)
            {
                UpdateRankScore(true);
                NPC_INFO[NPCID.GuildPillar].Npc.Name = Winner.Name;
                NPC_INFO[NPCID.GuildPillar].Npc.GuildID = Winner.GuildID;
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    NPC_INFO[NPCID.GuildPillar].Npc.SendView(NPC_INFO[NPCID.GuildPillar].Npc.GetArray(stream, false), Map);
                    Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_GOT_WIN_s@@" + Winner.Name + "@@", MsgMessage.MsgColor.white, MsgMessage.ChatMode.System).GetArray(stream));
                    Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_GOT_WIN_s@@" + Winner.Name + "@@", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                }
                ResetNpc();
            }
        }
        public void CompleteEndGuildWar()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                S_30 = false;
                Proces = ProcesType.Dead;
                RemoveMapItem();
                UpdateRankScore();
                UpdateRank();
                var GETWinner = GUILD_RANK.Values.OrderByDescending(p => p.Points).FirstOrDefault();
                if (GETWinner != null && GETWinner.Points>0)
                {
                    Winner.GuildID = GETWinner.GuildID;
                    Winner.Name = GETWinner.Name;
                    Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_ID_tNewGuildWars[Broadcast][RankWin]@@" + GETWinner.Name + "@@", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                }
                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("STR_SYN_WAR_END@@", MsgMessage.MsgColor.red, MsgMessage.ChatMode.System).GetArray(stream));
   

                RewardDeputiLeader.Clear();
                RewardLeader.Clear();
                Winner.DeputiLeaderReward = 7;
                Winner.LeaderReward = 1;
                Winner.LeaderRewardTop = 1;
                this.OrderList();
            }
        }
        public void UpdateScore(VirusX.Role.Player client, uint Damage, Role.SobNpc attacked)
        {
            if (client.MyGuild == null)
                return;
            if (Proces == ProcesType.Alive)
            {
                if (attacked.UID == 100900)
                {
                    NpcInfo npcInfo;
                    if (this.NPC_INFO[NPCID.GuildPillar].Npc.HitPoints >= 0 && this.VlmScoreInfoList.ContainsKey(client.UID))
                        this.VlmScoreInfoList[client.UID].TotalDamage += (ulong)Damage;
                    if (NPC_INFO.TryGetValue(NPCID.GuildWithHonorPillar, out npcInfo))
                    {
                        if (npcInfo.SCORE_LIST.ContainsKey(client.GuildID))
                            npcInfo.SCORE_LIST[client.GuildID] += Damage;
                        else
                            npcInfo.SCORE_LIST.Add(client.GuildID, Damage);
                    }
                    return;
                }
                Damage = Damage / 10;
                uint Damage2 = Damage;
                foreach (var X in SCORE_LIST.Values)
                {
                    if (X.GuildID != client.GuildID)
                    {
                        if (X.Score >= Damage2)
                        {
                            X.Score -= Damage2;
                            Damage2 = 0;
                        }
                        else
                        {
                            Damage2 -= (uint)X.Score;
                            X.Score = 0;
                        }
                    }
                }
                if (!SCORE_LIST.ContainsKey(client.GuildID))
                {
                    ScoreList obj = new ScoreList();
                    obj.GuildID = client.GuildID;
                    obj.Name = client.MyGuild.GuildName;
                    obj.Score = Damage;
                    SCORE_LIST.TryAdd(client.GuildID, obj);
                }
                else
                {
                    SCORE_LIST[client.GuildID].Score += Damage;
                    if (SCORE_LIST[client.GuildID].Score > (ulong)NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints )
                        SCORE_LIST[client.GuildID].Score = (ulong)NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints;
                }
                if (NPC_INFO[NPCID.GuildPillar].Npc.UID == 810)
                {
                    var GuildPillar = SCORE_LIST.Values.ToArray().OrderByDescending(p => p.Score).ToArray().FirstOrDefault();
                    var PillarChange = (double)((GuildPillar.Score * 100) / (uint)NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints);
                    if ((SCORE_LIST.Count < 2 || PillarChange > 50) && GuildPillar.GuildID != Winner.GuildID)
                    {
                        FinishRound();
                    }
                }
                UpdateRankScore();
                UpdatePoints(client);
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    Game.MsgServer.MsgUpdate upd = new MsgUpdate(stream, NPC_INFO[NPCID.GuildPillar].Npc.UID,2);
                    stream = upd.Append(stream, MsgUpdate.DataType.Hitpoints, (long)NPC_INFO[NPCID.GuildPillar].Npc.HitPoints);
                    stream = upd.Append(stream, MsgUpdate.DataType.MaxHitpoints, (long)NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints);
                    stream = upd.GetArray(stream);
                    client.View.SendView(stream, true);
                }
                MsgNewSynWar.Process(client.Owner, new MsgNewSynWar() { Type = 4 });
            }
        }
        public void UpdateRank()
        {
            try
            {
                if (S_30)
                {
                    MsgNewSynWar.Process(null, new MsgNewSynWar() { Type = 0 });
                    MsgNewSynWar.Process(null, new MsgNewSynWar() { Type = 1 });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void Join(Client.GameClient user)
        {
            if (S_30)
            {
                MsgNewSynWar.Process(user, new MsgNewSynWar() { Type = 6 });
                MsgNewSynWar.Process(user, new MsgNewSynWar() { Type = 5 });
            }
        }

        public void Update()
        {
            if (S_30)
            {
                if (DateTime.Now > Stamp)
                {
                    if (DateTime.Now > StampUpdate)
                    {
                        UpdateRankScore();
                        UpdateRank();
                        StampUpdate = DateTime.Now.AddSeconds(10);
                    }


                    this.VlmScoreInfoList = new Dictionary<uint, MsgGuildWar.VlmScoreInfo>();
                    if (CountDown == 0)
                    {
                        CompleteEndGuildWar();
                    }
                    else if (60 * 30 >= CountDown)
                    {
                        if (AvailablePoints + TotalAvailablePoints < 3000)
                        {
                            LoadMapItem((uint)(3000 - (AvailablePoints + TotalAvailablePoints)));

                        }
                    }
                    
                    else if (60 * 5 >= CountDown)
                    {
                        if (!MsgSchedules.GuildWar.NPC_INFO.ContainsKey(MsgGuildWar.NPCID.GuildWithHonorPillar))
                            MsgSchedules.GuildWar.AddHonorPillar();
                    }
                    Stamp = DateTime.Now.AddSeconds(1);
                }
            }
        }
        public void Update(GameClient user)
        {
            if (user.Player.Map == 1038 && MsgSchedules.GuildWar.Proces == ProcesType.Alive)
            {
                if (!user.Player.Alive)
                {
                    if (DateTime.Now > user.Player.DeadStamp.AddSeconds(60))
                    {
                        if (user.Player.MyGuildMember != null)
                            user.Player.MyGuildMember.GuildWarPoints = 0;
                    }
                }
            }
            if (user.Player.MapGuildWar == false && user.Player.Map == 1038)
            {
                if (MsgSchedules.GuildWar.Proces == ProcesType.Alive)
                {
                    user.Player.MapGuildWar = true;
                    MsgSchedules.GuildWar.Join(user);
                }
            }
            if (user.Player.MapGuildWar)
            {
                if (user.Player.Map != 1038 || MsgSchedules.GuildWar.Proces != ProcesType.Alive)
                {
                    user.Player.MapGuildWar = false;
                    MsgSchedules.GuildWar.RemoveUSER(user);
                }
            }
        }
        public uint AvailablePoints
        {
            get
            {
                return (uint)((Map.ContainItem(2619) * 10) + (Map.ContainItem(2620) * 5) + (Map.ContainItem(2621) * 3) + (Map.ContainItem(2622) * 1));
            }
            
        }
        public uint TotalAvailablePoints
        {
            get
            {
                uint point = 0;
                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user != null
                        && user.Socket != null
                        && user.Socket.Alive
                        && user.Player != null
                        && user.Player.Map == MapID
                        && user.Player.MyGuildMember != null
                        && user.Player.MyGuildMember.GuildWarPoints != 0)
                    {
                        point += user.Player.MyGuildMember.GuildWarPoints;
                    }
                }
                return point;
            }
        }
        public void LoadMapItem(uint count)
        {
            try
            {
                if (S_30 && !TotalPointsEnd)
                {
                    for (int i = 0; i < count; )
                    {
                        #region STR_TRAP_ID_2619
                        if (Role.Core.Rate(15) && i + 10 <= count)
                        {
                            Map.GuildWarItem(2619);
                            i += 10;
                        }
                        #endregion
                        #region STR_TRAP_ID_2620
                        if (Role.Core.Rate(15) && i + 5 <= count)
                        {
                            Map.GuildWarItem(2620);
                            i += 5;
                        }
                        #endregion
                        #region STR_TRAP_ID_2621
                        if (Role.Core.Rate(30) && i + 3 <= count)
                        {
                            Map.GuildWarItem(2621);
                            i += 3;
                        }
                        #endregion
                        #region STR_TRAP_ID_2622
                        if (Role.Core.Rate(30) && i + 1 <= count)
                        {
                            Map.GuildWarItem(2622);
                            i += 1;
                        }
                        #endregion
                        if (AvailablePoints == 3000)
                        {
                            TotalPointsEnd = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void RemoveMapItem()
        {
           
                foreach (var obj in Map.View.GetAllMapRoles(MapObjectType.Item))
                {
                    Game.MsgFloorItem.MsgItem Item = obj as Game.MsgFloorItem.MsgItem;
                    if (Item != null)
                    {
                        if (Item.MsgFloor.m_ID >= 2619 && Item.MsgFloor.m_ID <= 2622)
                        {

                            Map.RemoveTrapGuild(Item.X, Item.Y, Item);
                        }
                    }
                }
        }
        public void ChechMove(GameClient user)
        {
            if (user.Player.Map != 1038)
                return;
            if (user.Player.MyGuild == null)
                return;
            if (user.Player.MyGuildMember == null)
                return;
            foreach (var item in user.Player.View.Roles(Role.MapObjectType.Item))
            {
                Game.MsgFloorItem.MsgItem Item = item as Game.MsgFloorItem.MsgItem;
                if (Core.GetDistance(user.Player.X, user.Player.Y, Item.X, Item.Y) <= 1)
                {
                    if (Item.MsgFloor.m_ID == 2619)
                    {
                        user.Player.MyGuildMember.GuildWarPoints += 10;
                        Map.RemoveTrap(Item.X, Item.Y, Item);
                        UpdatePoints(user.Player);
                    }
                    if (Item.MsgFloor.m_ID == 2620)
                    {
                        user.Player.MyGuildMember.GuildWarPoints += 5;
                        Map.RemoveTrap(Item.X, Item.Y, Item);
                        UpdatePoints(user.Player);
                    }
                    if (Item.MsgFloor.m_ID == 2621)
                    {
                        user.Player.MyGuildMember.GuildWarPoints += 3;
                        Map.RemoveTrap(Item.X, Item.Y, Item);
                        UpdatePoints(user.Player);
                    }
                    if (Item.MsgFloor.m_ID == 2622)
                    {
                        user.Player.MyGuildMember.GuildWarPoints += 1;
                        Map.RemoveTrap(Item.X, Item.Y, Item);
                        UpdatePoints(user.Player);
                    }
                    MsgNewSynWar.Process(user, new MsgNewSynWar() { Type = 3 });
                    user.SendSysMesage("STR_ID_tNewGuildWars[Talk][UserScore]@@" + user.Player.MyGuildMember.GuildWarPoints + "@@");
                }
            }
        }
        
        public void UpdatePoints(VirusX.Role.Player player)
        {
            if (S_30)
            {
                if (!GUILD_RANK.ContainsKey(player.GuildID))
                {
                    GuildRank obj = new GuildRank();
                    obj.GuildID = player.GuildID;
                    obj.Name = player.MyGuild.GuildName;
                    obj.Points = player.MyGuild.GuildWarPoints;
                    GUILD_RANK.TryAdd(player.GuildID, obj);
                }
                else
                {
                    GUILD_RANK[player.GuildID].Points = player.MyGuild.GuildWarPoints;
                }
                if (!USER_RANK.ContainsKey(player.UID))
                {
                    UserRank obj = new UserRank();
                    obj.UID = player.UID;
                    obj.Name = player.Name;
                    obj.Points = (uint)player.MyGuildMember.GuildWarPoints;
                    USER_RANK.TryAdd(player.UID, obj);
                }
                else
                {
                    USER_RANK[player.UID].Points = (uint)player.MyGuildMember.GuildWarPoints;
                }
            }
        }
        public void UpdateRankScore(bool createWinned = false)
        {
            if (Proces != ProcesType.Dead)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    var Array = SCORE_LIST.Values.OrderByDescending(p => p.Score).Take(4).ToArray();
                    var DescendingList = Array;
                    for (int x = 0; x < DescendingList.Length; x++)
                    {
                        var element = DescendingList[x];
                        if (x == 0 && createWinned)
                            Winner = element;
                        Game.MsgServer.MsgMessage msg = new Game.MsgServer.MsgMessage("STR_TOP_LIST_dsd@@"
                            + (x + 1).ToString()
                            + "@@" + element.Name
                            + "@@" + (double)((element.Score * 100) / (uint)NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints)
                            + "%@@" + ((double)((element.Score * 100) / (uint)NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints)) * 2000 / 100
                            + "@@", MsgMessage.MsgColor.yellow, x == 0 ? MsgMessage.ChatMode.FirstRightCorner : MsgMessage.ChatMode.ContinueRightCorner);
                        SendMapPacket(msg.GetArray(stream));
                       
                    }
                }
            }
        }
        public void ResetNpc(bool Start = false)
        {
            try
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    Guild GetWinner;
                    NPC_INFO[NPCID.LeftGate].Npc.Mesh = (SobNpc.StaticMesh)StaticMesh.LeftGate;
                    NPC_INFO[NPCID.RightGate].Npc.Mesh = (SobNpc.StaticMesh)StaticMesh.RightGate;
                    NPC_INFO[NPCID.LeftGate].Npc.MaxHitPoints = 10000000;
                    NPC_INFO[NPCID.RightGate].Npc.MaxHitPoints = 10000000;
                    if (Guild.GuildPoll.TryGetValue(Winner.GuildID, out GetWinner))
                    {
                        foreach (var c_guild in GetWinner.Constructs.Values)
                        {
                            if (c_guild.ID == 1)
                            {
                                NPC_INFO[NPCID.LeftGate].Npc.MaxHitPoints = (int)c_guild.Data.Data;
                                NPC_INFO[NPCID.RightGate].Npc.MaxHitPoints = (int)c_guild.Data.Data;
                                NPC_INFO[NPCID.LeftGate].Npc.GuildID = Winner.GuildID;
                                NPC_INFO[NPCID.RightGate].Npc.GuildID = Winner.GuildID;
                                NPC_INFO[NPCID.LeftGate].Npc.Mesh = (SobNpc.StaticMesh)GateClose(c_guild.Level, false);
                                NPC_INFO[NPCID.RightGate].Npc.Mesh = (SobNpc.StaticMesh)GateClose(c_guild.Level, true);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (Start)
                        {
                            NPC_INFO[NPCID.LeftGate].Npc.Mesh = (SobNpc.StaticMesh)GateClose(0, false);
                            NPC_INFO[NPCID.RightGate].Npc.Mesh = (SobNpc.StaticMesh)GateClose(0, true);
                        }
                        else
                        {
                            NPC_INFO[NPCID.LeftGate].Npc.Mesh = (SobNpc.StaticMesh)GateOpen(0, false);
                            NPC_INFO[NPCID.RightGate].Npc.Mesh = (SobNpc.StaticMesh)GateOpen(0, true);
                        }
                    }
                    NPC_INFO[NPCID.GuildPillar].Npc.HitPoints = NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints;
                    NPC_INFO[NPCID.LeftGate].Npc.HitPoints = NPC_INFO[NPCID.LeftGate].Npc.MaxHitPoints;
                    NPC_INFO[NPCID.RightGate].Npc.HitPoints = NPC_INFO[NPCID.RightGate].Npc.MaxHitPoints;
                    if (Start)
                    {
                        NPC_INFO[NPCID.TenacityPillar].Npc.HitPoints = NPC_INFO[NPCID.TenacityPillar].Npc.MaxHitPoints;
                        NPC_INFO[NPCID.ExorcimPillar].Npc.HitPoints = NPC_INFO[NPCID.ExorcimPillar].Npc.MaxHitPoints;
                        NPC_INFO[NPCID.TenacityPillar].Npc.SendView(NPC_INFO[NPCID.TenacityPillar].Npc.GetArray(stream, false), Map);
                        NPC_INFO[NPCID.ExorcimPillar].Npc.SendView(NPC_INFO[NPCID.ExorcimPillar].Npc.GetArray(stream, false), Map);

                    }
                    NPC_INFO[NPCID.LeftGate].Npc.SendView(NPC_INFO[NPCID.LeftGate].Npc.GetArray(stream, false), Map);
                    NPC_INFO[NPCID.RightGate].Npc.SendView(NPC_INFO[NPCID.RightGate].Npc.GetArray(stream, false), Map);
                    NPC_INFO[NPCID.GuildPillar].Npc.SendView(NPC_INFO[NPCID.GuildPillar].Npc.GetArray(stream, false), Map);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public bool Verified(Role.Player attacker, Role.SobNpc target)
        {
            if (target.UID == NPC_INFO[NPCID.GuildPillar].Npc.UID)
            {
                if (MsgSchedules.GuildWar.NPC_INFO.ContainsKey(MsgGuildWar.NPCID.GuildWithHonorPillar))
                    return false;
                if (attacker.MyGuild == null)
                    return false;
                if (NPC_INFO[NPCID.GuildPillar].Npc.HitPoints == 0)
                    return false;

                if (Proces != ProcesType.Alive)
                    return false;
            }

            if (NPC_INFO.ContainsKey(NPCID.ExorcimPillar))
            {
                if (target.UID == NPC_INFO[NPCID.ExorcimPillar].Npc.UID)
                {
                    if (attacker.MyGuild == null)
                        return false;
                    if (attacker.GuildID == NPC_INFO[NPCID.ExorcimPillar].Npc.GuildID)
                        return false;
                    if (Proces != ProcesType.Alive)
                        return false;
                }
            }

            if (NPC_INFO.ContainsKey(NPCID.TenacityPillar))
            {
                if (target.UID == NPC_INFO[NPCID.TenacityPillar].Npc.UID)
                {
                    if (attacker.MyGuild == null)
                        return false;
                    if (attacker.GuildID == NPC_INFO[NPCID.TenacityPillar].Npc.GuildID)
                        return false;
                    if (Proces != ProcesType.Alive)
                        return false;
                }
            }
            if (NPC_INFO.ContainsKey(NPCID.GuildWithHonorPillar))
            {
                if (target.UID == NPC_INFO[NPCID.GuildWithHonorPillar].Npc.UID)
                {
                    if (attacker.MyGuild == null)
                        return false;
                    if (attacker.GuildID == NPC_INFO[NPCID.GuildWithHonorPillar].Npc.GuildID)
                        return false;
                    if (Proces != ProcesType.Alive)
                        return false;
                }
            }
            return true;
        }
        public void RemoveUSER(Client.GameClient user)
        {
            if (user.Player.MyGuildMember != null)
                user.Player.MyGuildMember.GuildWarPoints = 0;
            if (USER_RANK.ContainsKey(user.Player.UID))
                USER_RANK.Remove(user.Player.UID);
        }
        public void Death(Role.Player killer, Role.SobNpc Npc)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                Guild GET_Winner;
                #region GuildPillar
                if (Npc.UID == NPC_INFO[NPCID.GuildPillar].Npc.UID)
                {
                    uint Damage = (uint)Npc.HitPoints;
                    if (Npc.HitPoints > 0)
                        Npc.HitPoints = 0;

                    UpdateScore(killer, Damage, Npc);
                    NPC_INFO[NPCID.GuildPillar].Npc.HitPoints = NPC_INFO[NPCID.GuildPillar].Npc.MaxHitPoints;
                }
                #endregion
                #region GuildWithHonorPillar
                if (NPC_INFO.ContainsKey(NPCID.GuildWithHonorPillar))
                {
                    if (Npc.UID == NPC_INFO[NPCID.GuildWithHonorPillar].Npc.UID)
                    {
                        uint Damage = (uint)Npc.HitPoints;
                        if (Npc.HitPoints > 0)
                            Npc.HitPoints = 0;
                    }
                }
                #endregion

                #region ExorcimPillar
                if (NPC_INFO.ContainsKey(NPCID.ExorcimPillar))
                {
                    if (Npc.UID == NPC_INFO[NPCID.ExorcimPillar].Npc.UID)
                    {
                        uint Damage = (uint)Npc.HitPoints;
                        if (Npc.HitPoints > 0)
                            Npc.HitPoints = 0;
                    }
                }
                #endregion
                #region TenacityPillar
                if (NPC_INFO.ContainsKey(NPCID.TenacityPillar))
                {
                    if (Npc.UID == NPC_INFO[NPCID.TenacityPillar].Npc.UID)
                    {
                        uint Damage = (uint)Npc.HitPoints;
                        if (Npc.HitPoints > 0)
                            Npc.HitPoints = 0;
                    }
                }
                #endregion
                if (killer.MyGuild != null && killer.Map == MapID)
                {
                    #region ExorcimPillar
                    if (Npc.UID == NPC_INFO[NPCID.ExorcimPillar].Npc.UID)
                    {
                        NPC_INFO[NPCID.ExorcimPillar].Npc.Name = killer.MyGuild.GuildName;
                        NPC_INFO[NPCID.ExorcimPillar].Npc.GuildID = killer.GuildID;
                        NPC_INFO[NPCID.ExorcimPillar].Npc.HitPoints = NPC_INFO[NPCID.ExorcimPillar].Npc.MaxHitPoints;
                        NPC_INFO[NPCID.ExorcimPillar].Npc.SendView(NPC_INFO[NPCID.ExorcimPillar].Npc.GetArray(stream, false), Map);
                        MsgNewSynWar.Process(null, new MsgNewSynWar() { Type = 9, dwParam = 100009, SyndicateID = Npc.GuildID });
                    }
                    #endregion

                    #region TenacityPillar
                    if (Npc.UID == NPC_INFO[NPCID.TenacityPillar].Npc.UID)
                    {
                        NPC_INFO[NPCID.TenacityPillar].Npc.Name = killer.MyGuild.GuildName;
                        NPC_INFO[NPCID.TenacityPillar].Npc.GuildID = killer.GuildID;
                        NPC_INFO[NPCID.TenacityPillar].Npc.HitPoints = NPC_INFO[NPCID.TenacityPillar].Npc.MaxHitPoints;
                        NPC_INFO[NPCID.TenacityPillar].Npc.SendView(NPC_INFO[NPCID.TenacityPillar].Npc.GetArray(stream, false), Map);
                        MsgNewSynWar.Process(null, new MsgNewSynWar() { Type = 9, dwParam = 100008, SyndicateID = Npc.GuildID });
                    }
                    #endregion
                    #region GuildWithHonorPillar
                    if (NPC_INFO.ContainsKey(NPCID.GuildWithHonorPillar))
                    {
                        if (Npc.UID == NPC_INFO[NPCID.GuildWithHonorPillar].Npc.UID)
                        {
                            NPC_INFO[NPCID.GuildWithHonorPillar].Npc.Name = killer.MyGuild.GuildName;
                            NPC_INFO[NPCID.GuildWithHonorPillar].Npc.GuildID = killer.GuildID;
                            NPC_INFO[NPCID.GuildWithHonorPillar].Npc.HitPoints = NPC_INFO[NPCID.ExorcimPillar].Npc.MaxHitPoints;
                            NPC_INFO[NPCID.GuildWithHonorPillar].Npc.SendView(NPC_INFO[NPCID.GuildWithHonorPillar].Npc.GetArray(stream, false), Map);
                            killer.MyGuild.Info.HonorPillar++;
                        }
                    }
                    #endregion
                    #region LeftGate
                    if (Npc.UID == NPC_INFO[NPCID.LeftGate].Npc.UID)
                    {
                        if (Winner != null)
                        {
                            if (!LeftGateDemolition)
                                MsgNewSynWar.Process(null, new MsgNewSynWar() { Type = 10, dwParam = 3000, SyndicateID = Npc.GuildID });
                            Npc.MaxHitPoints = 10000000;
                            Npc.Mesh = (SobNpc.StaticMesh)GateDemolition(0, false);
                            if (Guild.GuildPoll.TryGetValue(Winner.GuildID, out GET_Winner))
                            {
                                GET_Winner.SendMessajGuild("[MsgGuildWar] The left gate has been breached!");
                                foreach (var c_guild in GET_Winner.Constructs.Values)
                                {
                                    if (c_guild.ID == 1)
                                    {
                                        Npc.MaxHitPoints = (int)c_guild.Data.Data;
                                        Npc.Mesh = (SobNpc.StaticMesh)GateDemolition(c_guild.Level, false);
                                    }
                                }
                            }
                            NPC_INFO[NPCID.LeftGate].Npc.GuildID = killer.GuildID;
                            NPC_INFO[NPCID.LeftGate].Npc.HitPoints = NPC_INFO[NPCID.LeftGate].Npc.MaxHitPoints;
                            Npc.SendView(Npc.GetArray(stream, false), Map);

                        }

                    }
                    #endregion
                    #region RightGate
                    if (Npc.UID == NPC_INFO[NPCID.RightGate].Npc.UID)
                    {
                        if (Winner != null)
                        {
                            if (!RightGateDemolition)
                                MsgNewSynWar.Process(null, new MsgNewSynWar() { Type = 10, dwParam = 3001, SyndicateID = Npc.GuildID });
                            Npc.MaxHitPoints = 10000000;
                            Npc.Mesh = (SobNpc.StaticMesh)GateDemolition(0, true);
                            if (Guild.GuildPoll.TryGetValue(Winner.GuildID, out GET_Winner))
                            {
                                GET_Winner.SendMessajGuild("[MsgGuildWar] The right gate has been breached!");
                                Npc.MaxHitPoints = 10000000;
                                foreach (var c_guild in GET_Winner.Constructs.Values)
                                {
                                    if (c_guild.ID == 1)
                                    {
                                        Npc.MaxHitPoints = (int)c_guild.Data.Data;
                                        Npc.Mesh = (SobNpc.StaticMesh)GateDemolition(c_guild.Level, true);
                                    }
                                }
                            }
                            NPC_INFO[NPCID.RightGate].Npc.GuildID = killer.GuildID;
                            NPC_INFO[NPCID.RightGate].Npc.HitPoints = NPC_INFO[NPCID.RightGate].Npc.MaxHitPoints;
                            Npc.SendView(Npc.GetArray(stream, false), Map);

                        }
                    }
                    #endregion
                    UpdatePoints(killer);
                }
            }
        }
        public void SendMapPacket(ServerSockets.Packet packet)
        {
            foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
            {
                if (client.Player.Map == 1038 || client.Player.Map == 6001)
                {
                    client.Send(packet);
                }
            }
        }
        public StaticMesh GateClose(uint Level, bool Gate)
        {
            if (Gate)
            {
                if (Level >= 20)
                    return StaticMesh.RightGate_Close_4;
                else if (Level >= 15)
                    return StaticMesh.RightGate_Close_3;
                else if (Level >= 10)
                    return StaticMesh.RightGate_Close_4;
                return StaticMesh.RightGate_Close_1;
            }
            else
            {
                if (Level >= 20)
                    return StaticMesh.LeftGate_Close_4;
                else if (Level >= 15)
                    return StaticMesh.LeftGate_Close_3;
                else if (Level >= 10)
                    return StaticMesh.LeftGate_Close_2;
                return StaticMesh.LeftGate_Close_1;
            }
        }
        public StaticMesh GateDemolition(uint Level, bool Gate)
        {
            if (Gate)
            {
                if (Level >= 20)
                    return StaticMesh.RightGate_Demolition_4;
                else if (Level >= 15)
                    return StaticMesh.RightGate_Demolition_3;
                else if (Level >= 10)
                    return StaticMesh.RightGate_Demolition_2;
                return StaticMesh.RightGate_Demolition_1;
            }
            else
            {
                if (Level >= 20)
                    return StaticMesh.LeftGate_Demolition_4;
                else if (Level >= 15)
                    return StaticMesh.LeftGate_Demolition_3;
                else if (Level >= 10)
                    return StaticMesh.LeftGate_Demolition_2;
                return StaticMesh.LeftGate_Demolition_1;
            }
        }
        public StaticMesh GateOpen(uint Level, bool Gate)
        {
            if (Gate)
            {
                if (Level >= 20)
                    return StaticMesh.RightGate_Open_4;
                else if (Level >= 15)
                    return StaticMesh.RightGate_Open_3;
                else if (Level >= 10)
                    return StaticMesh.RightGate_Open_2;
                return StaticMesh.RightGate_Open_1;
            }
            else
            {
                if (Level >= 20)
                    return StaticMesh.LeftGate_Open_4;
                else if (Level >= 15)
                    return StaticMesh.LeftGate_Open_3;
                else if (Level >= 10)
                    return StaticMesh.LeftGate_Open_2;
                return StaticMesh.LeftGate_Open_1;
            }
        }
        public unsafe bool Bomb(ServerSockets.Packet stream, Client.GameClient client, NPCID gate)
        {
            if (NPC_INFO[gate].Npc.HitPoints > 3000000)
            {
                NPC_INFO[gate].Npc.HitPoints -= 2000000;

                Game.MsgServer.MsgUpdate upd = new Game.MsgServer.MsgUpdate(stream, NPC_INFO[gate].Npc.UID, 1);
                stream = upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.Hitpoints, NPC_INFO[gate].Npc.HitPoints);
                NPC_INFO[gate].Npc.SendScrennPacket(upd.GetArray(stream));

                client.Player.Dead(null, client.Player.X, client.Player.Y, client.Player.UID);
                NPC_INFO[gate].Npc.SendString(stream, Game.MsgServer.MsgStringPacket.StringID.Effect, "firemagic");
                NPC_INFO[gate].Npc.SendString(stream, Game.MsgServer.MsgStringPacket.StringID.Effect, "bombarrow");

                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage("" + client.Player.Name + " from " + (client.Player.MyGuild != null ? client.Player.MyGuild.GuildName.ToString() : "None".ToString()).ToString() + " detonated the Bomb and killed herself/himself. But the " + gate.ToString() + " was blown up!"
                    , Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.Center).GetArray(stream));

                return true;
            }
            return false;
        }
        public bool ValidJump(int Current, out int New, ushort X, ushort Y)
        {
            if (Role.Core.GetDistance(217, 177, X, Y) <= 3)
            {
                New = 0;
                return true;
            }
            New = Current;
            int new_FloorType = Map.FloorType[X, Y];
            if (Current == 3)
            {
                if (new_FloorType == 0 || new_FloorType == 9 || new_FloorType == 13)
                {
                    if (Role.Core.GetDistance(X, Y, 164, 209) <= 20)
                    {
                        if (LeftGateOpen)
                        {
                            New = new_FloorType;
                            return true;
                        }
                    }
                    if (Role.Core.GetDistance(X, Y, 222, 177) <= 15)
                    {
                        if (RightGateOpen)
                        {
                            New = new_FloorType;
                            return true;
                        }
                    }
                    return false;
                }
            }
            New = new_FloorType;
            return true;
        }
        public bool ValidWalk(int Current, out int New, ushort X, ushort Y)
        {
            if (Role.Core.GetDistance(217, 177, X, Y) <= 3)
            {
                New = 0;
                return true;
            }
            New = Current;
            int new_mask = Map.FloorType[X, Y];
            if (Current == 3)
            {
                if (new_mask == 0 || new_mask == 9 || new_mask == 13)
                {
                    if (Y == 209 || Y == 208)
                    {
                        if (Role.Core.GetDistance(X, Y, 164, 209) <= 3)
                        {
                            if (LeftGateOpen)
                            {
                                New = new_mask;
                                return true;
                            }
                        }
                    }
                    else if (X == 216)
                    {
                        if (Role.Core.GetDistance(X, Y, 216, 177) <= 4)
                        {
                            if (RightGateOpen)
                            {
                                New = new_mask;
                                return true;
                            }
                        }
                    }

                    return false;
                }
            }
            New = new_mask;
            return true;
        }
        public bool LeftGateOpen
        {
            get
            {
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Open_1)
                    return true;
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Open_2)
                    return true;
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Open_3)
                    return true;
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Open_4)
                    return true;
                return false;
            }
        }
        public bool RightGateOpen
        {
            get
            {
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Open_1)
                    return true;
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Open_2)
                    return true;
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Open_3)
                    return true;
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Open_4)
                    return true;
                return false;
            }
        }
        public bool LeftGateDemolition
        {
            get
            {
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Demolition_1)
                    return true;
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Demolition_2)
                    return true;
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Demolition_3)
                    return true;
                if (NPC_INFO[NPCID.LeftGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.LeftGate_Demolition_4)
                    return true;
                return false;
            }
        }
        public bool RightGateDemolition
        {
            get
            {
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Demolition_1)
                    return true;
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Demolition_2)
                    return true;
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Demolition_3)
                    return true;
                if (NPC_INFO[NPCID.RightGate].Npc.Mesh == (SobNpc.StaticMesh)StaticMesh.RightGate_Demolition_4)
                    return true;

                return false;
            }
        }



        public void Load()
        {
            WindowsAPI.IniFile reader = new WindowsAPI.IniFile("\\GuildWarInfo.ini");
            Winner.GuildID = reader.ReadUInt32("Info", "ID", 0);
            Winner.Name = reader.ReadString("Info", "Name", "None");
            Winner.LeaderReward = reader.ReadInt32("Info", "LeaderReward", 0);
            Winner.LeaderRewardTop = reader.ReadInt32("Info", "LeaderRewardTop", 0);
            Winner.DeputiLeaderReward = reader.ReadInt32("Info", "DeputiLeaderReward", 0);

            RewardLeader.Add(reader.ReadUInt32("Info", "LeaderTop0", 0));
            for (int x = 0; x < 7; x++)
            {
                RewardDeputiLeader.Add(reader.ReadUInt32("Info", "DeputiTop" + x.ToString() + "", 0));
            }
            ResetNpc(true);
            for (int x = 0; x < 4; x++)
            {
                GuildConductor conductor = new GuildConductor();
                conductor.Load(reader.ReadString("Condutors", "GuildConductor" + (x + 1).ToString() + "", ""), (Game.MsgNpc.NpcID)(101614 + x * 2));
                GuildConductors.Add((Game.MsgNpc.NpcID)(101614 + x * 2), conductor);
                if (conductor.Npc.Map != 0)
                {
                    if (Pool.ServerMaps.ContainsKey(conductor.Npc.Map))
                        Pool.ServerMaps[conductor.Npc.Map].AddNpc(conductor.Npc);
                }
            }

        }
        public void Save()
        {
            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\GuildWarInfo.ini");
            if (Proces == ProcesType.Dead)
            {
                write.Write<uint>("Info", "ID", Winner.GuildID);
                write.WriteString("Info", "Name", Winner.Name);
                write.Write<int>("Info", "LeaderReward", Winner.LeaderReward);
                write.Write<int>("Info", "LeaderRewardTop", Winner.LeaderRewardTop);
                write.Write<int>("Info", "DeputiLeaderReward", Winner.DeputiLeaderReward);

                for (int x = 0; x < RewardLeader.Count; x++)
                    write.Write<uint>("Info", "LeaderTop" + x.ToString() + "", RewardLeader[x]);
                for (int x = 0; x < 7; x++)
                {
                    if (x >= RewardDeputiLeader.Count)
                        break;
                    write.Write<uint>("Info", "DeputiTop" + x.ToString() + "", RewardDeputiLeader[x]);
                }
            }

            write.WriteString("Condutors", "GuildConductor1", GuildConductors[Game.MsgNpc.NpcID.TeleGuild1].ToString());
            write.WriteString("Condutors", "GuildConductor2", GuildConductors[Game.MsgNpc.NpcID.TeleGuild2].ToString());
            write.WriteString("Condutors", "GuildConductor3", GuildConductors[Game.MsgNpc.NpcID.TeleGuild3].ToString());
            write.WriteString("Condutors", "GuildConductor4", GuildConductors[Game.MsgNpc.NpcID.TeleGuild4].ToString());
        }
       
       

    }
}
