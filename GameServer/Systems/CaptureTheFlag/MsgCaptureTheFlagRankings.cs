using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CaptureTheFlagRankingsCreate(this ServerSockets.Packet stream, MsgCaptureTheFlagRankings.ActionID Mode, uint Page, uint Length)
        {
            stream.InitWriter();
            stream.Write((ushort)Mode);//4
            stream.Write(Page);//6
            stream.Write(11);//10
            stream.Write(Length);//14
            stream.Write(1);//18
            stream.ZeroFill(12);//20
            return stream;//34
        }
        public static unsafe ServerSockets.Packet AddItemCaptureTheFlagRankings(this ServerSockets.Packet stream,
           string GuildName, string name, uint DwParam1, uint DwParam2, ulong DwParam3, uint DwParam4)
        {
            stream.Write(GuildName, 32);//34
            stream.Write(name, 72);//66
            stream.Write(DwParam1);//138
            stream.Write(DwParam2);//142
            stream.Write((ushort)DwParam3);//146
            return stream;//148
        }
        public static unsafe ServerSockets.Packet CaptureTheFlagRankingsCreate(this ServerSockets.Packet stream, MsgCaptureTheFlagRankings.ActionID Mode
            , uint Page, uint Dwparam1, uint dwparam2, uint dwparam3, ulong dwparam4)
        {
            stream.InitWriter();
            stream.Write((ushort)Mode);//4
            stream.Write(Page);//6
            stream.Write(Dwparam1);//10
            stream.Write(dwparam2);//14
            stream.Write((ushort)1);//18
            stream.Write(dwparam3);//20
            stream.Write(dwparam4);//24

            return stream;//32
        }
        public static unsafe ServerSockets.Packet AddItemCaptureTheFlagRankings(this ServerSockets.Packet stream,
            string name, uint DwParam1, uint DwParam2, ulong DwParam3, uint DwParam4)
        {
            stream.Write(name, 64);//32
            stream.ZeroFill(8);//96
            stream.Write(DwParam1);//104
            stream.Write(DwParam2);//108
            stream.Write(DwParam3);//112
            stream.Write(DwParam4);//124
            stream.Write(0);//120
       
            return stream;//128
        }
        public static unsafe ServerSockets.Packet AddItemCaptureTheFlagRankings(this ServerSockets.Packet stream, uint DwPram1,ulong DwParam2, string Name, uint ID)
        {

            stream.Write(DwPram1);
            stream.Write(DwParam2);
            stream.Write(Name, 32);
            stream.ZeroFill(20);
            stream.Write(ID);
            stream.ZeroFill(20);
            return stream;
        }
        public static unsafe ServerSockets.Packet AddItemCaptureTheFlagRankings(this ServerSockets.Packet stream, uint DwPram1,uint DwParam2, uint DwParam3, ulong DwParam4, uint ID, string Name)
        {
            stream.Write(DwPram1);
            stream.Write(DwParam2);
            stream.Write(DwParam3);
            stream.Write(DwParam4);
            stream.Write(ID);
            stream.Write(Name, 32);
            stream.ZeroFill(50);
            return stream;
        }
        public static unsafe ServerSockets.Packet AddItemCaptureTheFlagRankings(this ServerSockets.Packet stream, string Name, uint DwParam)
        {
            stream.Write(Name, 32);
            stream.Write(DwParam);
            return stream;
        }

        public static unsafe ServerSockets.Packet CaptureTheFlagRankingsFinalize(this ServerSockets.Packet stream)
        {
            stream.Finalize(GamePackets.MsgSelfSynMemAwardRank);

            return stream;
        }
        public static unsafe void GetCaptureTheFlagRankings(this ServerSockets.Packet stream, out  MsgCaptureTheFlagRankings.ActionID mode, out uint Dwparam
            , out uint Dwparam1, out uint dwparam2, out uint dwparam3, out uint dwparam4)
        {
            mode = (MsgCaptureTheFlagRankings.ActionID)stream.ReadUInt16();//4
            Dwparam = stream.ReadUInt32();//6
            Dwparam1 = stream.ReadUInt32();//10
            dwparam2 = stream.ReadUInt32();//14
            stream.ReadUInt16();//18
            dwparam3 = stream.ReadUInt32();//20
            dwparam4 = stream.ReadUInt32();//24
        }
    }
    public class MsgCaptureTheFlagRankings
    {
        public enum ActionID : ushort
        {
            RewardRanking = 0,
            Info = 1,
            SetRewardConquerPoints = 3,
            SetRewardMoney = 4,
            SetRewardAll = 5,
            RankMembers = 8,
            ArenaGui = 9,
            CrossFlag = 10,
            UpdatePoint = 13,
        }
        [PacketAttribute(GamePackets.MsgSelfSynMemAwardRank)]
        private unsafe static void Poroces(Client.GameClient user, ServerSockets.Packet stream)
        {
           
            MsgCaptureTheFlagRankings.ActionID mode; uint Dwparam;
            uint Dwparam1; uint dwparam2; uint dwparam3; uint dwparam4;
            stream.GetCaptureTheFlagRankings(out mode, out Dwparam, out Dwparam1, out dwparam2, out dwparam3, out dwparam4);
            switch (mode)
            {
                case ActionID.CrossFlag:
                    {

                        if (MsgInterServer.Core.IsCrossFlagOpen)
                        {
                            var array = MsgTournaments.MsgSchedules.CaptureTheFlag.RegistredGuilds.Values.Where(p => p.CTF_ExploitsCross != 0).OrderByDescending(p => p.CTF_ExploitsCross).ToArray();


                            stream.CaptureTheFlagRankingsCreate(mode, Dwparam, (uint)Math.Min(array.Length, 8));
                            for (int x = 0; x < Math.Min(array.Length, 8); x++)
                            {
                                var element = array[x];
                                stream.AddItemCaptureTheFlagRankings(Program.ServerConfig.ServerName, element.GuildName, element.CTF_ExploitsCross, (uint)element.Members.Count, (uint)x, 0);
                            }
                            stream.CaptureTheFlagRankingsFinalize();
                            user.Send(stream);
                        }
                        else
                        {
                            var array = Role.Instance.Guild.GuildPoll.Values.Where(p => p.CTF_ExploitsCross != 0).OrderByDescending(p => p.CTF_ExploitsCross).ToArray();

                            stream.CaptureTheFlagRankingsCreate(mode, Dwparam, (uint)Math.Min(array.Length, 8));

                            for (int x = 0; x < Math.Min(array.Length, 8); x++)
                            {
                                var element = array[x];
                                uint[] reward = element.GetLeaderReward();
                                stream.AddItemCaptureTheFlagRankings(Program.ServerConfig.ServerName, element.GuildName, element.CTF_ExploitsCross, (uint)element.Members.Count, (uint)x, 0);
                            }
                            stream.CaptureTheFlagRankingsFinalize();
                            user.Send(stream);
                        }
                        break;
                    }
                case ActionID.SetRewardConquerPoints:
                    {
                        if (user.Player.MyGuild == null || user.Player.MyGuildMember == null)
                            break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;

                        if (user.Player.MyGuild.Info.ConquerPointFund >= dwparam3)
                        {
                            user.Player.MyGuild.Info.ConquerPointFund -= dwparam3;
                            user.Player.MyGuild.CTF_Next_ConquerPoints += dwparam3;
                        }
                        else
                        {

                            user.SendSysMesage("Your guild not have sufficient Conquer Points.");


                        }


                        stream.CaptureTheFlagRankingsCreate(mode, 0, 0, 0, user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_ConquerPoints : 0
                            , user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_Money : 0);
                        stream.CaptureTheFlagRankingsFinalize();
                        user.Send(stream);

                        break;
                    }
                case ActionID.SetRewardMoney:
                    {
                        if (user.Player.MyGuild == null || user.Player.MyGuildMember == null)
                            break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;

                        if (user.Player.MyGuild.Info.SilverFund >= dwparam4)
                        {
                            user.Player.MyGuild.Info.SilverFund -= dwparam4;
                            user.Player.MyGuild.CTF_Next_Money += dwparam4;
                        }
                        else
                        {

                            user.SendSysMesage("Your guild not have sufficient Money.");
                        }

                        stream.CaptureTheFlagRankingsCreate(mode, 0, 0, 0, user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_ConquerPoints : 0
                            , user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_Money : 0);
                        stream.CaptureTheFlagRankingsFinalize();
                        user.Send(stream);

                        break;
                    }
                case ActionID.SetRewardAll:
                    {
                        if (user.Player.MyGuild == null || user.Player.MyGuildMember == null)
                            break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;

                        if (user.Player.MyGuild.Info.ConquerPointFund >= dwparam3)
                        {
                            user.Player.MyGuild.Info.ConquerPointFund -= dwparam3;
                            user.Player.MyGuild.CTF_Next_ConquerPoints += dwparam3;
                        }
                        else
                        {

                            user.SendSysMesage("Your guild not have sufficient Conquer Points.");


                        }
                        if (user.Player.MyGuild.Info.SilverFund >= dwparam4)
                        {
                            user.Player.MyGuild.Info.SilverFund -= dwparam4;
                            user.Player.MyGuild.CTF_Next_Money += dwparam4;
                        }
                        else
                        {
                            user.SendSysMesage("Your guild not have sufficient Money.");

                        }


                        stream.CaptureTheFlagRankingsCreate(mode, 0, 0, 0, user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_ConquerPoints : 0
                            , user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_Money : 0);
                        stream.CaptureTheFlagRankingsFinalize();
                        user.Send(stream);

                        break;
                    }
                case ActionID.Info:
                    {
                        if (user.Player.MyGuild == null || user.Player.MyGuildMember == null)
                            break;

                        var array = user.Player.MyGuild.Members.Values.Where(p => p.CTF_Exploits != 0).ToArray();
                        var Rank = array.OrderByDescending(p => p.CTF_Exploits).ToArray();

                        const int max = 5;
                        int offset = (int)(Dwparam1 - 1) * max;
                        int count = Math.Min(max, Math.Max(0, Rank.Length - offset));

                        stream.CaptureTheFlagRankingsCreate(mode, Dwparam1, (uint)Rank.Length, (uint)count
                               , user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_ConquerPoints : 0
                            , user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_Money : 0);

                        for (int x = 0; x < count; x++)
                        {
                            if (Rank.Length > offset + x)
                            {
                                var element = Rank[offset + x];

                                stream.AddItemCaptureTheFlagRankings((uint)(offset + x), element.CTF_Exploits, element.RewardConquerPoints, element.RewardMoney, element.UID, element.Name);
                            }
                        }
                        stream.CaptureTheFlagRankingsFinalize();
                        user.Send(stream);

                        break;
                    }
                case ActionID.RankMembers:
                    {
                        if (user.Player.MyGuild == null || user.Player.MyGuildMember == null)
                            break;

                        var array = user.Player.MyGuild.Members.Values.Where(p => p.CTF_Exploits != 0).ToArray();
                        var Rank = array.OrderByDescending(p => p.CTF_Exploits).ToArray();

                        const int max = 5;
                        int offset = (int)(Dwparam1 - 1) * max;
                        int count = Math.Min(max, Math.Max(0, Rank.Length - offset));

                        stream.CaptureTheFlagRankingsCreate(mode, 1, (uint)Rank.Length, (uint)count
                            , user.Player.MyGuildMember != null ? user.Player.MyGuildMember.CTF_Exploits : 0, 0);

                        for (int x = 0; x < count; x++)
                        {
                            if (Rank.Length > offset + x)
                            {
                                var element = Rank[offset + x];
                                stream.AddItemCaptureTheFlagRankings(element.Name, element.CTF_Exploits);
                            }
                        }
                        stream.CaptureTheFlagRankingsFinalize();
                        user.Send(stream);

                        break;
                    }
                case ActionID.RewardRanking:
                    {
                        var array = Role.Instance.Guild.GuildPoll.Values.Where(p => p.CTF_Next_ConquerPoints != 0).OrderByDescending(p => p.CTF_Next_ConquerPoints).ToArray();

                        const int max = 5;
                        int offset = (int)(Dwparam1 - 1) * max;
                        int count = Math.Min(max, Math.Max(0, array.Length - offset));

                        stream.CaptureTheFlagRankingsCreate(mode, 1, (uint)array.Length, (uint)(count)
                            , user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_ConquerPoints : 0
                            , user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Next_Money : 0);

                        for (int x = 0; x < count; x++)
                        {
                            if (array.Length > offset + x)
                            {
                                var element = array[offset + x];
                                stream.AddItemCaptureTheFlagRankings(element.CTF_Next_ConquerPoints, element.CTF_Next_Money, element.GuildName, element.Info.GuildID);
                            }
                        }
                        stream.CaptureTheFlagRankingsFinalize();
                        user.Send(stream);


                        break;
                    }
                case ActionID.ArenaGui:
                    {
                        if (MsgTournaments.MsgSchedules.CaptureTheFlag.Proces == MsgTournaments.ProcesType.Alive)
                        {
                            var array = MsgTournaments.MsgSchedules.CaptureTheFlag.RegistredGuilds.Values.Where(p => p.CTF_Exploits != 0).OrderByDescending(p => p.CTF_Exploits).ToArray();

                            stream.CaptureTheFlagRankingsCreate(mode, 1, 0, (uint)Math.Min(array.Length, 8), user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Exploits : 0, 0);
                            for (int x = 0; x < Math.Min(array.Length, 8); x++)
                            {
                                var element = array[x];
                                stream.AddItemCaptureTheFlagRankings(element.GuildName, element.CTF_Exploits, (uint)element.Members.Count, element.CTF_Next_Money, element.CTF_Next_ConquerPoints);
                            }
                            stream.CaptureTheFlagRankingsFinalize();
                            user.Send(stream);
                        }
                        else
                        {
                            var array = Role.Instance.Guild.GuildPoll.Values.Where(p => p.CTF_Exploits != 0).OrderByDescending(p => p.CTF_Exploits).ToArray();

                            stream.CaptureTheFlagRankingsCreate(mode, 1, 0, (uint)Math.Min(array.Length, 8), user.Player.MyGuild != null ? user.Player.MyGuild.CTF_Exploits : 0, 0);

                            for (int x = 0; x < Math.Min(array.Length, 9); x++)
                            {
                                var element = array[x];
                                uint[] reward = element.GetLeaderReward();
                                stream.AddItemCaptureTheFlagRankings(element.GuildName, element.CTF_Exploits, (uint)element.Members.Count, (uint)reward[0], (uint)reward[1]);
                            }
                            stream.CaptureTheFlagRankingsFinalize();
                            user.Send(stream);
                        }
                        break;
                    }
            }

        }

    }
}
