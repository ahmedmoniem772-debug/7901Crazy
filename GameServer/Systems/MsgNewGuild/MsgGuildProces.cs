using VirusX.Database;
using VirusX.Role;
using VirusX.Role.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public unsafe static class MsgGuildProces
    {
        public enum GuildAction : uint
        {
            Accept = 1,
            Invite = 2,
            Quit = 3,
            InfoName = 6,
            Allied = 7,
            RemoveAlly = 8,
            Enemy = 9,
            RemoveEnemy = 10,
            SilverDonate = 11,
            Show = 12,
            Disband = 19,
            CpDonate = 20,
            RequestAllied = 23,
            Requirements = 24,
            Bulletin = 27,
            Promote = 28,
            ConfirmPromote = 29,
            Discharge = 30,
            Resign = 32,
            RequestPromote = 37,
            UpdatePromote = 38,
            Create = 51,
            Appoint = 53,
            Retire = 54,
            Join = 55,
        }

        public static void GuildRequest(this ServerSockets.Packet stream, out GuildAction requesttype, out uint UID, out int[] args, out string[] strlist)
        {
            requesttype = (GuildAction)stream.ReadInt32();
            UID = stream.ReadUInt32();
            args = new int[4];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = stream.ReadInt32();
            }
            strlist = stream.ReadStringList();
            stream.ReadBytes(3); //unknown
        }

        public static unsafe ServerSockets.Packet GuildRequestCreate(this ServerSockets.Packet stream, GuildAction requesttype, uint UID, int[] args, params string[] strlist)
        {
            stream.InitWriter();
            stream.Write((uint)requesttype);
            stream.Write(UID);
            stream.Write(args[0]);
            stream.Write(args[1]);
            stream.Write(args[2]);
            stream.Write(args[3]);
            stream.Write(strlist);
            stream.Finalize(GamePackets.MsgSyndicate);
            return stream;
        }

        [PacketAttribute(GamePackets.MsgSyndicate)]
        private static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (!user.Player.OnMyOwnServer)
                return;
            if (user.PokerPlayer != null)
                return;
            GuildAction Action;
            uint UID;
            int[] args;
            string[] strlist;
            stream.GuildRequest(out Action, out UID, out args, out strlist);
            if (user.PokerPlayer != null)
                return;
            int Amount = (int)UID;
            if (Amount < 0)
                return;
            switch (Action)
            {
                case GuildAction.Accept:
                    {
                        if (user.Player.MyGuild != null) break;
                        Role.IMapObj obj;
                        if (user.Player.View.TryGetValue(UID, out obj, Role.MapObjectType.Player))
                        {
                            Client.GameClient Target = null;
                            Target = (obj as Role.Player).Owner;

                            if (Target == null) break;
                            if (Target.Player.MyGuild == null) break;
                            if (Target.Player.MyGuildMember == null) break;
                            if (Target.Player.TargetGuild == user.Player.UID)
                            {
                                Target.Player.MyGuild.AddPlayer(user.Player, stream);
                            }
                        }
                        break;
                    }
                case GuildAction.Invite:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader
                            && user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.DeputyLeader) break;
                        Role.IMapObj obj;
                        if (user.Player.View.TryGetValue(UID, out obj, Role.MapObjectType.Player))
                        {
                            Client.GameClient Target = null;
                            Target = (obj as Role.Player).Owner;
                            if (Target == null) break;

                            if (Target == null) break;
                            if (Target.Player.MyGuild != null) break;
                            if (Target.Player.MyGuildMember != null) break;
                            user.Player.TargetGuild = Target.Player.UID;
                            Target.Send(stream.RelationCreate(user.Player, Target.Player));
                            Target.Send(stream.GuildRequestCreate(GuildAction.Invite, user.Player.UID, args, strlist));
                        }
                        break;
                    }
                case GuildAction.Join:
                    {
                        if (user.Player.MyGuild != null) break;
                        if (user.Player.MyGuildMember != null) break;
                        Guild Guild;
                        if (Guild.GuildPoll.TryGetValue(UID, out Guild))
                        {
                            Guild.AddPlayer(user.Player, stream);
                        }
                        break;
                    }
                case GuildAction.Create:
                    {
                        if (user.Player.NobilityRank >= Role.Instance.Nobility.NobilityRank.Earl && user.Player.Money >= 10000000)
                        {
                            if (Role.Instance.Guild.AllowToCreate(strlist[0]))
                            {
                                user.Player.Money -= 10000000;
                                new Role.Instance.Guild(user, strlist[0], stream);
                            }
                        }
                        break;
                    }
                case GuildAction.Retire:
                    {
                        Guild.Member Member;
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader) break;
                        if (user.Player.MyGuild.Members.TryGetValue(UID, out Member))
                        {
                            user.Player.MyGuild.CommandID = 0;
                            Member.Command = 0;
                            user.Send(stream.GuildRequestCreate(Action, 0, new int[4], strlist.ToArray()));

                        }
                        break;
                    }
                case GuildAction.Appoint:
                    {
                        Guild.Member Member;
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader) break;
                        if (user.Player.MyGuild.Members.TryGetValue(UID, out Member))
                        {
                            Guild.Member Member2;
                            if (user.Player.MyGuild.CommandID != 0)
                            {
                                if (user.Player.MyGuild.Members.TryGetValue(user.Player.MyGuild.CommandID, out Member2))
                                    Member2.Command = 0;
                            }
                            user.Player.MyGuild.CommandID = Member.UID;
                            Member.Command = 1;
                            user.Send(stream.GuildRequestCreate(Action, 0, new int[4], strlist.ToArray()));
                        }
                        break;
                    }
                case GuildAction.Resign:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;

                        user.Player.MyGuild.Promote((uint)Role.Flags.GuildMemberRank.Member, user.Player, user.Player.Name, stream);
                        break;
                    }
                case GuildAction.InfoName:
                    {
                        if (user.OnInterServer)
                            break;
                        Role.Instance.Guild Guild;
                        if (Role.Instance.Guild.GuildPoll.TryGetValue(UID, out Guild))
                        {

                            user.Player.SendString(stream, Game.MsgServer.MsgStringPacket.StringID.GuildName, Guild.Info.GuildID, true
                           , new string[1] { Guild.GuildName + " " + Guild.Info.LeaderName + " " + Guild.Info.Level + " " + Guild.Members.Count });
                        }
                        break;
                    }
                case GuildAction.SilverDonate:
                    {
                        if (UID >= 10000)
                        {
                            if (user.InTrade)
                                return;
                            if (user.PokerPlayer != null)
                                return;
                          
                            if (user.Player.Money < UID)
                                break;
                            if (user.Player.MyGuild != null && user.Player.MyGuildMember != null)
                            {
                                user.Player.Money -= (int)UID;
                                user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                user.Player.MyGuildMember.MoneyDonate += UID;
                                user.Player.MyGuild.Info.SilverFund += UID;
                                user.Player.MyGuild.SendThat(user.Player);
                                user.Player.MyGuild.AddMessage("STR_SYNDICATE_EVENT_DONATE_MONEY@@" + user.Player.Name + "@@" + UID + "@@");
                            }
                        }
                        break;
                    }
                case GuildAction.CpDonate:
                    {
                        if (UID >= 1)
                        {
                            if (user.InTrade)
                                return;
                          
                                if (user.Player.ConquerPoints < UID)
                                break;
                            if (user.Player.MyGuild != null && user.Player.MyGuildMember != null)
                            {
                                user.Player.ConquerPoints -= (long)UID;

                                user.Player.MyGuildMember.CpsDonate += UID;
                                user.Player.MyGuild.Info.ConquerPointFund += UID;
                                user.Player.MyGuild.SendThat(user.Player);
                                user.Player.MyGuild.AddMessage("STR_SYNDICATE_EVENT_DONATE_EMONEY@@" + user.Player.Name + "@@" + UID + "@@");
                            }
                        }
                        break;
                    }
                case GuildAction.Show:
                    {
                        if (user.Player.MyGuild != null)
                        {
                            user.Player.MyGuild.SendThat(user.Player);
                        }
                        break;
                    }
                case GuildAction.Bulletin:
                    {
                        if (user.Player.MyGuild != null)
                        {
                            if (user.Player.Name != user.Player.MyGuild.Info.LeaderName)
                                break;
                            if (strlist.Length > 0 && strlist[0] != null)
                            {
                                if (BaseFunc.NameStrCheck(strlist[0], false))
                                {
                                    user.Player.MyGuild.CreateBuletinTime();
                                    user.Player.MyGuild.Bulletin = strlist[0];
                                    user.Player.MyGuild.SendThat(user.Player);
                                }
                                else
                                {
#if Arabic
                                     user.SendSysMesage("Invalid Charasters in Bulletin.");
#else
                                    user.SendSysMesage("Invalid Charasters in Bulletin.");
#endif

                                }
                            }
                        }
                        break;
                    }
                case GuildAction.Quit:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                        {
                            user.Player.MyGuild.Quit(user.Player.Name, false, stream);

                        }
                        break;
                    }
                case GuildAction.RequestPromote:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        SendPromote(stream, user, Action);
                        break;
                    }
                case GuildAction.Promote:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        if (strlist.Length > 0 && strlist[0] != null)
                        {
                            user.Player.MyGuild.Promote(UID, user.Player, strlist[0], stream);
                        }
                        break;
                    }
                case GuildAction.RemoveAlly:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        if (strlist.Length > 0 && strlist[0] != null)
                        {
                            user.Player.MyGuild.RemoveAlly(strlist[0], stream);
                        }
                        break;
                    }
                case GuildAction.RequestAllied:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        if (strlist.Length > 0 && strlist[0] != null)
                        {
                            string name = strlist[0];
                            if (name == user.Player.MyGuild.GuildName)
                                break;
                            if (!user.Player.MyGuild.IsEnemy(name))
                                user.Player.MyGuild.AddAlly(stream, name);
                            else
                            {
                                user.SendSysMesage("Soory, this guild is in ennemy`s list");
                            }
                        }
                        user.Player.MyGuild.SendGuildAlly(stream, true, user);
                        break;
                    }
                case GuildAction.Allied:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        if (strlist.Length > 0 && strlist[0] != null)
                        {
                            string name = strlist[0];
                            if (name == user.Player.MyGuild.GuildName)
                                break;
                            if (!user.Player.MyGuild.IsEnemy(name))
                            {
                                var leader = Guild.GetLeaderGuild(name);
                                if (leader != null && leader.IsOnline)
                                {
                                    Client.GameClient LeaderClient;
                                    if (Pool.GamePoll.TryGetValue(leader.UID, out LeaderClient))
                                    {
                                        LeaderClient.Send(stream.GuildRequestCreate(GuildAction.RequestAllied, 0, new int[4], user.Player.MyGuild.GuildName));
                                        user.Player.MyGuild.AddAlly(stream, name);
                                    }
                                }
                            }
                            else
                            {
#if Arabic
                                  user.SendSysMesage("Soory, this guild is in ennemy`s list");
#else
                                user.SendSysMesage("Soory, this guild is in ennemy`s list");
#endif

                            }
                        }
                        break;
                    }
                case GuildAction.Enemy:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        if (strlist.Length > 0 && strlist[0] != null)
                        {
                            string name = strlist[0];
                            if (name == user.Player.MyGuild.GuildName)
                                break;
                            if (user.Player.MyGuild.AllowAddAlly(name))
                            {
                                user.Player.MyGuild.AddEnemy(stream, name);
#if Arabic
                                user.Player.MyGuild.SendMessajGuild("Guild Leader " + user.Player.Name + " has added Guild " + name + " to the enemy list!");
#else
                                user.Player.MyGuild.SendMessajGuild("Guild Leader " + user.Player.Name + " has added Guild " + name + " to the enemy list!");
#endif

                            }
                            else
                            {
#if Arabic
                                  user.SendSysMesage("Soory, this guild is in Ally`s list");
#else
                                user.SendSysMesage("Soory, this guild is in Ally`s list");
#endif

                            }
                        }
                        break;
                    }
                case GuildAction.RemoveEnemy:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        if (strlist.Length > 0 && strlist[0] != null)
                        {
                            user.Player.MyGuild.RemoveEnemy(strlist[0], stream);
                        }
                        break;
                    }
                case GuildAction.Discharge:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        if (user.Player.InUnion)
                        {
                            user.CreateBoxDialog("Please quit union first time.");

                            break;
                        }
                        user.Player.MyGuild.Dismis(user, stream);
                        break;
                    }

                case GuildAction.Requirements:
                    {
                        if (user.Player.MyGuild == null) break;
                        if (user.Player.MyGuildMember == null) break;
                        if (user.Player.MyGuildMember.Rank != Role.Flags.GuildMemberRank.GuildLeader)
                            break;
                        byte[] packet = new byte[stream.Size];
                        fixed (byte* ptr = packet)
                        {
                            stream.memcpy(ptr, stream.Memory, stream.Size);
                        }
                        user.Player.MyGuild.Info.Recruitment_Battle_Power = BitConverter.ToUInt32(packet, 12);
                        user.Player.MyGuild.Info.Recruitment_Flag = (Guild.ClassFlag)BitConverter.ToUInt32(packet, 16);
                        user.Player.MyGuild.Info.RecruitmentOFF = (packet[20] == 1 ? true : false);
                        user.Player.MyGuild.Info.RecruitmentON = (packet[21] == 1 ? true : false);
                        break;
                    }
      

            }
        }
        public static unsafe void SendPromote(ServerSockets.Packet stream, Client.GameClient client, GuildAction typ)
        {
            if (client.Player.MyGuild == null) return;
            if (client.Player.MyGuildMember == null) return;
            List<string> list = new List<string>();
            StringBuilder builder = new StringBuilder();
            var guild = client.Player.MyGuild;
            var ArsenalBP = guild.Info.ArsenalBP;
            var Level = guild.Info.Level;
            syndicate_level.Level _level;
            if (syndicate_level._syndicate_level.TryGetValue(Level, out _level))
            {
                #region GuildLeader
                if (client.Player.MyGuildMember.Rank == Role.Flags.GuildMemberRank.GuildLeader)
                {
                    list.Add(CreatePromotion(builder, Role.Flags.GuildMemberRank.GuildLeader, 1, 1, ArsenalBP, 0));
                    list.Add(CreatePromotion(builder, Role.Flags.GuildMemberRank.DeputyLeader, guild.DeputyLeader_Count, _level.Max_DeputyLeader, ArsenalBP, 0));
                    list.Add(CreatePromotion(builder, Role.Flags.GuildMemberRank.Manager, guild.Manager_Count, _level.Max_Manager, ArsenalBP, 0));
                    list.Add(CreatePromotion(builder, Role.Flags.GuildMemberRank.Steward, guild.Steward_Count, _level.Max_Steward, ArsenalBP, 0));
                }
                #endregion
                #region DeputyLeader
                if (client.Player.MyGuildMember.Rank == Role.Flags.GuildMemberRank.DeputyLeader)
                {
                    list.Add(CreatePromotion(builder, Role.Flags.GuildMemberRank.Manager, guild.Manager_Count, _level.Max_Manager, ArsenalBP, 0));
                    list.Add(CreatePromotion(builder, Role.Flags.GuildMemberRank.Steward, guild.Steward_Count, _level.Max_Steward, ArsenalBP, 0));
                }
                #endregion
            }
            int extraLength = 0;
            foreach (var str in list) extraLength += str.Length + 1;
            client.Send(stream.GuildRequestCreate(typ, 0, new int[4], list.ToArray()));
        }
        public static string CreatePromotion(StringBuilder builder, Role.Flags.GuildMemberRank rank, int occupants, uint maxOccupants, uint extraBattlePower, uint conquerPoints)
        {
            builder.Remove(0, builder.Length);
            builder.Append((int)rank);
            builder.Append(" ");
            builder.Append(occupants);
            builder.Append(" ");
            builder.Append(maxOccupants);
            builder.Append(" ");
            builder.Append(extraBattlePower);
            builder.Append(" ");
            builder.Append(conquerPoints);
            builder.Append(" ");
            return builder.ToString();
        }
    }
}
