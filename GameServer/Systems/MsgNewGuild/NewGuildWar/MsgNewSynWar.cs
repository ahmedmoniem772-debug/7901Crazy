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
using VirusX.Game;

namespace VirusX
{
    [ProtoContract]
    public class MsgNewSynWar
    {
        public enum TypeID : byte
        {
            GuildScore = 0,//100%
            AvailablePoints = 1,//100%
            Report = 2,//100%
            UserScore = 3,//100%
            GulidWarBegan = 5,
            Join = 6,//100%
            Repaired = 8,//100%
            OwnershipofTenacityPillarischanged = 9,//100%
            Destroyed = 10,//100%
        }
        [ProtoMember(1, IsRequired = true)]
        public uint Type;
        [ProtoMember(2)]
        public uint uk2;
        [ProtoMember(3)]
        public uint dwParam;// CountDown 1800 = 30 or //Remaining Available  Points , NpcID = 9
        [ProtoMember(4)]
        public string RightGate = "";
        [ProtoMember(5)]
        public uint USER_ID;
        [ProtoMember(6)]
        public uint uk6;//0
        [ProtoMember(7)]
        public string LeftGate = "";
        [ProtoMember(8)]
        public uint uk8;//0
        [ProtoMember(9)]
        public string TenacityPillar = "";
        [ProtoMember(10)]
        public uint uk10;//100008
        [ProtoMember(11)]
        public string ExorcimPillar;//0
        [ProtoMember(12)]
        public string GuildPillar = "";
        [ProtoMember(13)]
        public uint Refreshed;//0
        [ProtoMember(14)]
        public string GuildWithHonorPillar;//0
        [ProtoMember(15)]
        public uint SyndicateID;
        [ProtoMember(16)]
        public uint uk16;
        [ProtoMember(17)]
        public uint uk17;
        [ProtoMember(18)]
        public uint uk18;
        [ProtoMember(19)]
        public GuildData[] items;
        [ProtoContract]
        public class GuildData
        {
            [ProtoMember(1)]
            public uint Rank;
            [ProtoMember(2)]
            public uint Point;
            [ProtoMember(3)]
            public string Name;
        }
        public ServerSockets.Packet ToArray()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(this);
                stream.Finalize(Game.GamePackets.MsgNewSynWar);
                return stream;
            }
        }
        public static implicit operator ServerSockets.Packet(MsgNewSynWar obj)
        {
            return obj.ToArray();
        }
        [PacketAttribute(Game.GamePackets.MsgNewSynWar)]
        public unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            MsgNewSynWar Info = null;
            Info = new MsgNewSynWar();
            Info = stream.ProtoBufferDeserialize<MsgNewSynWar>(Info);
            switch ((TypeID)Info.Type)
            {
                case TypeID.GuildScore:
                    {
                        Guild GUILD;
                        foreach (var g in MsgSchedules.GuildWar.GUILD_RANK.Values)
                        {
                            if (Guild.GuildPoll.TryGetValue(g.GuildID, out GUILD))
                            {
                                g.Points = GUILD.GuildWarPoints;
                            }
                        }
                        MsgNewSynWar obj = new MsgNewSynWar();
                        obj.Type = (uint)MsgNewSynWar.TypeID.GuildScore;
                        obj.uk16 = 1;
                        obj.uk17 = 1;
                        int unm = 0;
                        var Array = MsgSchedules.GuildWar.GUILD_RANK.Values.Where(p => p.Points != 0).OrderByDescending(p => p.Points).ToArray();
                        obj.items = new GuildData[Array.Length];
                        foreach (var guild in Array)
                        {
                            if (unm == 3)
                                break;
                            obj.items[unm] = new GuildData();
                            obj.items[unm].Point = guild.Points;
                            obj.items[unm].Name = guild.Name;
                            unm++;
                        }
                        if (user != null)
                            user.Send(obj);
                        else
                            MsgSchedules.GuildWar.SendMapPacket(obj);
                        break;
                    }
                case TypeID.AvailablePoints:
                    {
                        MsgNewSynWar obj = new MsgNewSynWar();
                        obj.Type = (uint)MsgNewSynWar.TypeID.AvailablePoints;
                        obj.dwParam = MsgSchedules.GuildWar.AvailablePoints;
                        obj.uk16 = 1;
                        uint unm = 0;
                        var _Array = MsgSchedules.GuildWar.USER_RANK.Values.Where(p => p.Points != 0).OrderByDescending(p => p.Points).ToArray();
                        obj.items = new GuildData[_Array.Length];
                        foreach (var players in _Array)
                        {
                            if (unm < 2)
                            {
                                obj.items[unm] = new GuildData();
                                obj.items[unm].Rank = (uint)(unm + 1);
                                obj.items[unm].Point = players.Points;
                                obj.items[unm].Name = players.Name;
                            }
                            players.Rank = (uint)(unm + 1);
                            unm++;
                        }
                        if (user != null)
                            user.Send(obj);
                        else
                            MsgSchedules.GuildWar.SendMapPacket(obj);
                        break;
                    }
                case TypeID.Report:
                    {
                        string RightGate = "", LeftGate = "", TenacityPillar = "", GuildPillar = "", ExorcimPillar = "", GuildWithHonorPillar = "";
                        Guild GETGuild = null;
                        if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.GuildID, out GETGuild))
                            RightGate = GETGuild.GuildName;
                        if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.GuildID, out GETGuild))
                            LeftGate = GETGuild.GuildName;
                        if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.TenacityPillar].Npc.GuildID, out GETGuild))
                            TenacityPillar = GETGuild.GuildName;

                        if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.GuildPillar].Npc.GuildID, out GETGuild))
                            GuildPillar = GETGuild.GuildName;
                        if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.ExorcimPillar].Npc.GuildID, out GETGuild))
                            ExorcimPillar = GETGuild.GuildName;
                        if (MsgSchedules.GuildWar.NPC_INFO.ContainsKey(MsgGuildWar.NPCID.GuildWithHonorPillar))
                        {
                            if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.GuildWithHonorPillar].Npc.GuildID, out GETGuild))
                                GuildWithHonorPillar = GETGuild.GuildName;
                            Info.Refreshed = 1;
                        }
                        Info.USER_ID = 3001;
                        Info.dwParam = 3000; //Remaining Available  Points
                        Info.RightGate = RightGate;
                        Info.LeftGate = LeftGate;
                        Info.TenacityPillar = TenacityPillar;

                        Info.GuildPillar = GuildPillar;
                        Info.ExorcimPillar = ExorcimPillar;
                        Info.GuildWithHonorPillar = GuildWithHonorPillar;

                        Info.uk10 = 100008;
                        Info.uk16 = 1;
                        Info.uk17 = 1;
                        Info.uk18 = 0;
                        var unm = 0;
                        var Array = MsgSchedules.GuildWar.GUILD_RANK.Values.Where(p => p.Points != 0).OrderByDescending(p => p.Points).ToArray();
                        Info.items = new GuildData[Array.Length];
                        foreach (var guild in Array)
                        {
                            Info.items[unm] = new GuildData();
                            Info.items[unm].Point = guild.Points;
                            Info.items[unm].Name = guild.Name;
                            unm++;
                        }
                        user.Send(Info);
                        break;
                    }
                case TypeID.Repaired:
                    {
                        if (Info.dwParam == 3000)
                        {
                            if (MsgSchedules.GuildWar.LeftGateDemolition)
                            {
                                if ((user.Player.GuildRank == Flags.GuildMemberRank.DeputyLeader || user.Player.GuildRank == Flags.GuildMemberRank.GuildLeader) && MsgSchedules.GuildWar.Winner.GuildID == user.Player.GuildID)
                                {
                                    Guild GetWinner;
                                    if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.Winner.GuildID, out GetWinner))
                                    {
                                        bool f = true;
                                        foreach (var c_guild in GetWinner.Constructs.Values)
                                        {
                                            if (c_guild.ID == 1)
                                            {
                                                if (GetWinner.Info.Material >= c_guild.Data.GateRepair)
                                                {
                                                    GetWinner.Info.Material -= c_guild.Data.GateRepair;
                                                }
                                                else
                                                    f = false;
                                            }
                                        }
                                        if (!f) break;
                                        MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.Mesh = (SobNpc.StaticMesh)MsgGuildWar.StaticMesh.LeftGate;
                                        MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.MaxHitPoints = 10000000;
                                        foreach (var c_guild in GetWinner.Constructs.Values)
                                        {
                                            if (c_guild.ID == 1)
                                            {
                                                MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.MaxHitPoints = (int)c_guild.Data.Data;
                                                MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.GuildID = MsgSchedules.GuildWar.Winner.GuildID;
                                                MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.Mesh = (SobNpc.StaticMesh)MsgSchedules.GuildWar.GateClose(c_guild.Level, false);
                                            }
                                        }
                                    }
                                    MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.HitPoints = MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.MaxHitPoints;
                                    MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.SendView(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.LeftGate].Npc.GetArray(stream, false), MsgSchedules.GuildWar.Map);
                                    MsgSchedules.GuildWar.SendMapPacket(Info);
                                }
                            }
                        }
                        if (Info.dwParam == 3001)
                        {
                            if (MsgSchedules.GuildWar.RightGateDemolition)
                            {
                                if ((user.Player.GuildRank == Flags.GuildMemberRank.DeputyLeader || user.Player.GuildRank == Flags.GuildMemberRank.GuildLeader) && MsgSchedules.GuildWar.Winner.GuildID == user.Player.GuildID)
                                {
                                    Guild GetWinner;
                                    if (Guild.GuildPoll.TryGetValue(MsgSchedules.GuildWar.Winner.GuildID, out GetWinner))
                                    {
                                        bool f = true;
                                        foreach (var c_guild in GetWinner.Constructs.Values)
                                        {
                                            if (c_guild.ID == 1)
                                            {
                                                if (GetWinner.Info.Material >= c_guild.Data.GateRepair)
                                                {
                                                    GetWinner.Info.Material -= c_guild.Data.GateRepair;
                                                }
                                                else
                                                    f = false;
                                            }
                                        }
                                        if (!f) break;
                                        MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.Mesh = (SobNpc.StaticMesh)MsgGuildWar.StaticMesh.RightGate;
                                        MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.MaxHitPoints = 10000000;
                                        foreach (var c_guild in GetWinner.Constructs.Values)
                                        {
                                            if (c_guild.ID == 1)
                                            {
                                                MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.MaxHitPoints = (int)c_guild.Data.Data;
                                                MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.GuildID = MsgSchedules.GuildWar.Winner.GuildID;
                                                MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.Mesh = (SobNpc.StaticMesh)MsgSchedules.GuildWar.GateClose(c_guild.Level, true);
                                            }
                                        }
                                    }
                                    MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.HitPoints = MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.MaxHitPoints;
                                    MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.SendView(MsgSchedules.GuildWar.NPC_INFO[MsgGuildWar.NPCID.RightGate].Npc.GetArray(stream, false), MsgSchedules.GuildWar.Map);
                                   
                                }
                            }
                        }
                        break;
                    }
                case TypeID.Join:
                    {
                        MsgNewSynWar obj = new MsgNewSynWar();
                        obj.Type = (uint)MsgNewSynWar.TypeID.Join;
                        obj.dwParam = MsgSchedules.GuildWar.CountDown;
                        obj.SyndicateID = user.Player.GuildID;
                        user.Send(obj);
                        break;
                    }
                case TypeID.GulidWarBegan:
                    {
                        MsgNewSynWar obj = new MsgNewSynWar();
                        obj.Type = (uint)TypeID.GulidWarBegan;
                        obj.dwParam = MsgSchedules.GuildWar.CountDown;
                        obj.USER_ID = 0;//??
                        obj.uk10 = 0;//??
                        obj.SyndicateID = user.Player.GuildID;
                        obj.uk18 = 0;//???
                        user.Send(obj);
                        break;
                    }
                case TypeID.UserScore:
                    {
                        MsgNewSynWar obj = new MsgNewSynWar();
                        obj.Type = (uint)MsgNewSynWar.TypeID.UserScore;
                        obj.dwParam = user.Player.MyGuildMember.GuildWarPoints;
                        obj.USER_ID = user.Player.UID;
                        user.Player.View.SendView(obj, true);
                        break;
                    }
                case TypeID.OwnershipofTenacityPillarischanged:
                    {
                        if (MsgSchedules.GuildWar.S_30)
                        {
                            if (Info.SyndicateID != 0)
                            {
                                if (user != null)
                                    user.Send(Info);
                                else
                                    MsgSchedules.GuildWar.SendMapPacket(Info);
                            }
                        }
                        break;
                    }
                case TypeID.Destroyed:
                    {
                        if (MsgSchedules.GuildWar.S_30)
                        {
                            if (Info.SyndicateID != 0)
                            {
                                if (user != null)
                                    user.Send(Info);
                                else
                                    MsgSchedules.GuildWar.SendMapPacket(Info);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
