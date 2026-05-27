using VirusX.Client;
using VirusX.Database;
using VirusX.Game.MsgMonster;
using VirusX.Role;
using VirusX.Role.Instance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;


namespace VirusX.Game.MsgServer
{
    public static class MsgSynSkill
    {

        public enum _Type : byte
        {
            Upgrade,
            Show,
        }
        [ProtoContract]
        public class MsgGuildSkill
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2, IsRequired = true)]
            public MsgSynSkill.MsgGuildSkill.Skill[] Skills;

            [ProtoContract]
            public class Skill
            {
                [ProtoMember(1, IsRequired = true)]
                public uint ID;
                [ProtoMember(2, IsRequired = true)]
                public uint Level;
            }
        }
        public static unsafe ServerSockets.Packet CreateGuildSkill(this ServerSockets.Packet stream, MsgSynSkill.MsgGuildSkill obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgSynSkill);
            return stream;
        }
        public static void GetGuildSkill(this ServerSockets.Packet stream, out MsgSynSkill.MsgGuildSkill pQuery)
        {
            pQuery = new MsgSynSkill.MsgGuildSkill();
            pQuery = stream.ProtoBufferDeserialize<MsgSynSkill.MsgGuildSkill>(pQuery);
        }
        [PacketAttribute(GamePackets.MsgSynSkill)]
        private unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            MsgSynSkill.MsgGuildSkill pQuery = (MsgSynSkill.MsgGuildSkill)null;
            stream.GetGuildSkill(out pQuery);
            switch ((MsgSynSkill._Type)pQuery.Type)
            {
                case MsgSynSkill._Type.Upgrade:
                    {
                        uint id = pQuery.Skills[0].ID;
                        uint level = 1;
                        GuildSkill.Skill skill1;
                        if (user.GuildSkill.Skills.TryGetValue(id, out skill1))
                        {
                            id = skill1.ID;
                            level = skill1.Level;
                        }
                        cq_syn_skill_type.skill_type skill_type;
                        if (cq_syn_skill_type.TryGetValue(id, level, out skill_type) && user.Player.MyDontion >= skill_type.DonationPoints && user.Player.Money >= (long)skill_type.Silver)
                        {
                            user.Player.Money -= (long)skill_type.Silver;

                            user.Player.MyDontion -= skill_type.DonationPoints;
                            user.Player.SendUpdate(stream, (long)user.Player.MyDontion, MsgUpdate.DataType.MyDontion);
                            user.Player.SendUpdate(stream, (long)skill_type.DonationPoints, MsgUpdate.DataType.DonationPoints);
                            GuildSkill.Skill skill2;
                            if (user.GuildSkill.Skills.TryGetValue(skill_type.Type, out skill2))
                            {
                                Role.Instance.Guild.Construct construct = user.Player.MyGuild.Constructs.Values.Where<Role.Instance.Guild.Construct>((System.Func<Role.Instance.Guild.Construct, bool>)(p => p.ID == 4)).FirstOrDefault<Role.Instance.Guild.Construct>();
                                if (construct != null && construct.Data.Data > (ulong)skill2.Level)
                                {
                                    ++skill2.Level;
                                    user.GuildSkill.Skills[skill2.ID] = skill2;
                                }
                            }
                            else
                            {
                                skill2 = new GuildSkill.Skill();
                                skill2.ID = skill_type.Type;
                                skill2.Level = (uint)1;
                                user.GuildSkill.Skills.Add<uint, GuildSkill.Skill>(skill2.ID, skill2);
                            }
                            pQuery.Skills[0].Level = skill2.Level;
                            user.Send(stream.CreateGuildSkill(pQuery));
                        }
                        break;

                    }
                case MsgSynSkill._Type.Show:
                    {
                        if (!user.Player.MyGuild.Constructs.ContainsKey(4))
                            break;
                        var Stage = user.Player.MyGuild.Constructs.Values.Where(p => p.ID == 4).FirstOrDefault();
                        if (Stage != null)
                        {
                            VirusX.Game.MsgServer.MsgSynFormInfo.MsgGuildConstruct obj = new VirusX.Game.MsgServer.MsgSynFormInfo.MsgGuildConstruct();
                            obj.Material = 0;
                            obj.construct = new VirusX.Game.MsgServer.MsgSynFormInfo.MsgGuildConstruct.Construct[1];
                            obj.construct[0] = new VirusX.Game.MsgServer.MsgSynFormInfo.MsgGuildConstruct.Construct(Stage);
                            user.Send(stream.CreateGuildConstruct(obj));
                        }
                        int Count = user.GuildSkill.Skills.Values.Count;
                        pQuery.Skills = new MsgGuildSkill.Skill[Count];
                        for (int i = 0; i < Count; i++)
                        {
                            var Skill = user.GuildSkill.Skills.Values.ToArray()[i];
                            pQuery.Skills[i] = new MsgGuildSkill.Skill();
                            pQuery.Skills[i].ID = Skill.ID;
                            pQuery.Skills[i].Level = Skill.Level;
                        }
                        user.Send(stream.CreateGuildSkill(pQuery));
                        break;
                    }
            }
        }
    }
}