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
    public static class MsgSynForm
    {
        [ProtoContract]
        public class MsgGuildConstructInfo
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2, IsRequired = true)]
            public uint FormType;
            [ProtoMember(3, IsRequired = true)]
            public uint Beast;
            [ProtoMember(4)]
            public uint TypeUpgrade;
            [ProtoMember(5)]
            public ulong Money;
            [ProtoMember(6)]
            public uint TypeMoney;
            [ProtoMember(7)]
            public ulong DonateMaterial;
            [ProtoMember(8)]
            public ulong OK;
            public enum _Type : byte
            {
                Info = 0,
                DonateMaterial = 1,
                Upgrade = 2,
                Magic = 3,
                Enhance = 5,
                Create = 6,
                Load = 7,
            }
            public enum _FormType : byte
            {
                Center = 6,
                Arsenal = 5,
                Beast = 4,
                Gate = 3,
                DrillHall = 2,
                Keeper = 1,
            }
        }
        public static unsafe ServerSockets.Packet CreateGuildConstructInfo(this ServerSockets.Packet stream, MsgGuildConstructInfo obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgSynForm);
            return stream;
        }
        public static unsafe void GetGuildConstructInfo(this ServerSockets.Packet stream, out MsgGuildConstructInfo pQuery)
        {
            pQuery = new MsgGuildConstructInfo();
            pQuery = stream.ProtoBufferDeserialize(pQuery);
        }
        [PacketAttribute(GamePackets.MsgSynForm)]
        private unsafe static void Process5(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (user.PokerPlayer != null)
                return;
           
            MsgGuildConstructInfo Info = null;
        
            stream.GetGuildConstructInfo(out Info);
            int Amount = (int)Info.DonateMaterial;
            if (Amount < 0)
                return;
            switch ((MsgGuildConstructInfo._Type)Info.Type)
                {
                    case MsgGuildConstructInfo._Type.Magic:
                        {
                            if (user.Player.Map != 1038)
                                break;
                            NewPetGuild.Death(user, stream);
                            var Stage = user.Player.MyGuild.Constructs.Values.Where(p => p.ID == Info.FormType && p.Beast == Info.Beast).FirstOrDefault();
                            if (Stage != null)
                            {
                                uint ID = (uint)Stage.Data.Data;
                                switch (Info.Beast)
                                {
                                    case 0:
                                        {
                                            NewPetGuild.AddPet(user, ID, 4, user.Player.X, user.Player.Y, stream);
                                            break;
                                        }
                                    case 1:
                                        {
                                            NewPetGuild.AddPet(user, ID, 5, user.Player.X, user.Player.Y, stream);
                                            user.Player.AddFlag(MsgUpdate.Flags.LotusDemon, StatusFlagsBigVector32.PermanentFlag, false);
                                            break;
                                        }
                                    case 2:
                                        {
                                            NewPetGuild.AddPet(user, ID, 6, user.Player.X, user.Player.Y, stream);
                                            user.Player.AddFlag(MsgUpdate.Flags.SierraBeast, StatusFlagsBigVector32.PermanentFlag, false);
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case MsgGuildConstructInfo._Type.Upgrade:
                        {
                            syn_formtype.formtype obj;
                            var Stage = user.Player.MyGuild.Constructs.Values.Where(p => p.ID == Info.FormType && p.Beast == Info.Beast).FirstOrDefault();
                            if (Stage != null)
                            {
                                if (syn_formtype.TryGetValue(Info.FormType * 100 + Info.Beast, Stage.Level, out obj))
                                {
                                    if (Stage != null)
                                    {
                                        switch (Info.TypeUpgrade)
                                        {
                                            case 0:
                                                {
                                                    if (user.Player.MyGuild.Info.Material >= Info.Money)
                                                    {
                                                        user.Player.MyGuild.Info.Material -= Info.Money;
                                                        Stage.Exp += Info.Money * 100000;
                                                        if (Stage.Exp >= obj.EXP)
                                                        {
                                                            Stage.Level += 1;
                                                            Stage.Exp -= obj.EXP;
                                                            user.Player.MyGuild.AddMessage("STR_SYNDICATE_EVENT_UPGRADE_BUILDING@@" + (MsgGuildConstructInfo._FormType)Info.FormType + "@@" + Stage.Level + "@@");
                                                        }
                                                    }
                                                    break;
                                                }
                                            case 1:
                                                {
                                                    if (user.Player.Money >= (long)Info.Money)
                                                    {
                                                        user.Player.Money -= (long)Info.Money;
                                                        Stage.Exp += Info.Money;
                                                        if (Stage.Exp >= obj.EXP)
                                                        {
                                                            Stage.Level += 1;
                                                            Stage.Exp -= obj.EXP;
                                                            user.Player.MyGuild.AddMessage("STR_SYNDICATE_EVENT_UPGRADE_BUILDING@@" + (MsgGuildConstructInfo._FormType)Info.FormType + "@@" + Stage.Level + "@@");
                                                        }
                                                    }
                                                    break;
                                                }
                                        }
                                        MsgSynFormInfo.MsgGuildConstruct _obj = new MsgSynFormInfo.MsgGuildConstruct();
                                        _obj.Material = user.Player.MyGuild.Info.Material;
                                        _obj.construct = new MsgSynFormInfo.MsgGuildConstruct.Construct[1];
                                        _obj.construct[0] = new MsgSynFormInfo.MsgGuildConstruct.Construct(Stage);
                                        user.Send(stream.CreateGuildConstruct(_obj));
                                        if (Stage.ID == 6)
                                        {
                                            user.Player.MyGuild.Info.Level = Stage.Level;
                                        }
                                        if (Stage.ID == 5)
                                        {
                                            user.Player.MyGuild.Info.ArsenalBP = Stage.Level;
                                            foreach (var m in user.Player.MyGuild.Members.Values)
                                            {
                                                Client.GameClient m_c;
                                                if (Pool.GamePoll.TryGetValue(m.UID, out m_c))
                                                {
                                                    m_c.Player.GuildBattlePower = user.Player.MyGuild.Info.ArsenalBP;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            break;
                        }
                    case MsgGuildConstructInfo._Type.Enhance:
                        {
                           
                            break;
                        }
                    case MsgGuildConstructInfo._Type.DonateMaterial:
                        {
                            if (user.Player.MyGuild == null) break;
                            if (user.Player.MyGuildMember == null) break;
                            if (Info.TypeMoney == 1)
                            {
                                if (user.Player.ConquerPoints >= (int)Info.DonateMaterial)
                                {
                                    user.Player.ConquerPoints -= (long)Info.DonateMaterial;
                                    user.Player.MyGuild.Info.Material += Info.DonateMaterial;
                                    user.Player.MyGuildMember.MoneyDonate += (long)Info.DonateMaterial;
                                    user.Player.MyGuild.SendThat(user.Player);
                                    user.Player.MyGuild.AddMessage("STR_SYNDICATE_EVENT_DONATE_GOODS@@" + user.Player.Name + "@@" + Info.DonateMaterial + "@@");
                                }
                            }
                            else if (Info.TypeMoney == 0)
                            {
                                if (user.Player.Money >= (long)Info.DonateMaterial * 100000)
                                {
                                    user.Player.Money -= (long)Info.DonateMaterial * 100000;
                                    user.Player.MyGuild.Info.Material += Info.DonateMaterial;
                                    user.Player.MyGuildMember.MoneyDonate += (long)Info.DonateMaterial;
                                    user.Player.MyGuild.SendThat(user.Player);
                                    user.Player.MyGuild.AddMessage("STR_SYNDICATE_EVENT_DONATE_GOODS@@" + user.Player.Name + "@@" + Info.DonateMaterial + "@@");
                                }
                            }
                            Info.OK = 1;
                            user.Send(stream.CreateGuildConstructInfo(Info));
                            break;
                        }
                    case MsgGuildConstructInfo._Type.Info:
                        {
                            if (user.Player.MyGuild == null) break;
                            if (user.Player.MyGuildMember == null) break;
                            var Stage = user.Player.MyGuild.Constructs.Values.Where(p => p.ID == Info.FormType && p.Beast == Info.Beast).FirstOrDefault();
                            if (Stage != null)
                            {
                                MsgSynFormInfo.MsgGuildConstruct obj = new MsgSynFormInfo.MsgGuildConstruct();
                                obj.Material = user.Player.MyGuild.Info.Material;
                                obj.construct = new MsgSynFormInfo.MsgGuildConstruct.Construct[1];
                                obj.construct[0] = new MsgSynFormInfo.MsgGuildConstruct.Construct(Stage);
                                user.Send(stream.CreateGuildConstruct(obj));
                            }
                            break;
                        }
                    case MsgGuildConstructInfo._Type.Load:
                        {
                            int Count = user.Player.MyGuild.Constructs.Values.Count;
                            MsgSynFormInfo.MsgGuildConstruct obj = new MsgSynFormInfo.MsgGuildConstruct();
                            obj.Type = 4;
                            obj.Material = user.Player.MyGuild.Info.Material;
                            obj.construct = new MsgSynFormInfo.MsgGuildConstruct.Construct[Count];
                            for (int i = 0; i < Count; i++)
                            {
                                obj.construct[i] = new MsgSynFormInfo.MsgGuildConstruct.Construct(user.Player.MyGuild.Constructs.Values.ToArray()[i]);
                            }
                            user.Send(stream.CreateGuildConstruct(obj));
                            user.Send(stream.CreateGuildConstructInfo(Info));
                            break;
                        }
                }
            
        }
    }
}

            
