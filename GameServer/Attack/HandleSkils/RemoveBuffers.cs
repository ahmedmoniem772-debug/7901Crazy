using VirusX.Game.MsgTournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler
{
    public class RemoveBuffers
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack,user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.Compassion:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                            , 0, Attack.X, Attack.Y, ClientSpell.ID
                            , ClientSpell.Level, ClientSpell.UseSpellSoul);

                            if (user.Team != null)
                            {
                                foreach (var target in user.Team.GetMembers())
                                {
                                    if (Role.Core.GetDistance(user.Player.X, user.Player.Y, target.Player.X, target.Player.Y) < 18)
                                    {
                                       // target.Player.RemoveFlag(MsgUpdate.Flags.SoulShackle);
                                        target.Player.RemoveFlag(MsgUpdate.Flags.Poisoned);
                                        target.Player.RemoveFlag(MsgUpdate.Flags.PoisonStar);
                                        MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(target.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                    }
                                }
                            }

                            MsgSpell.SetStream(stream); MsgSpell.Send(user);

                            Updates.UpdateSpell.CheckUpdate(stream,user,Attack, 1000, DBSpells);
                  
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.Tranquility:
                        {

                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                           , 0, Attack.X, Attack.Y, ClientSpell.ID
                           , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            if (user.Player.UID == Attack.OpponentUID)
                            {

                                user.Player.RemoveFlag(MsgUpdate.Flags.SoulShackle);
                                user.Player.RemoveFlag(MsgUpdate.Flags.Poisoned);
                                user.Player.RemoveFlag(MsgUpdate.Flags.PoisonStar);
                                user.Player.RemoveFlag(MsgUpdate.Flags.ChillingSnow);
                                
                                if (MsgSchedules.CaptureTheFlag.Proces == ProcesType.Alive && user.Player.Map == 2057U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(user.Player.UID) && MsgSchedules.CaptureTheFlag.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                {
                                    ++MsgSchedules.CaptureTheFlag.VlmScoreInfoList[user.Player.UID].Shackled;
                                    user.Player.UnShackle(stream, true);
                                }
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                            }
                            else
                            {
                                Role.IMapObj target;
                                if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                                {
                                    Role.Player attacked = target as Role.Player;
                                    if (user.Player.PkMode == Role.Flags.PKMode.Team || user.Player.PkMode == Role.Flags.PKMode.Guild)
                                    {
                                        if ((user.Player.MyGuild != null &&
                                            user.Player.MyGuild.Ally.ContainsKey(attacked.MyGuild.Info.GuildID)) || user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(attacked.MyGuild.Info.GuildID))
                                        {

                                        }
                                    }
                                    else if (user.Player.PkMode == Role.Flags.PKMode.PK)
                                    {
                                        if (attacked.RemoveFlag(MsgUpdate.Flags.SoulShackle))
                                        {
                                            if (user.Player.MyGuild != null)
                                            {
                                                if (user.Player.Map == 2057 && Game.MsgTournaments.MsgSchedules.CaptureTheFlag.Proces == Game.MsgTournaments.ProcesType.Alive)
                                                {
                                                    user.Player.MyGuild.CTF_Exploits += 2;
                                                    user.Player.MyGuildMember.CTF_Exploits += 2;

                                                }

                                            }

                                        }
                                    }
                                    attacked.RemoveFlag(MsgUpdate.Flags.SoulShackle);
                                    attacked.RemoveFlag(MsgUpdate.Flags.Poisoned);
                                    attacked.RemoveFlag(MsgUpdate.Flags.PoisonStar);
                                    attacked.RemoveFlag(MsgUpdate.Flags.ChillingSnow);
                                    
                                    if (MsgSchedules.CaptureTheFlag.Proces == ProcesType.Alive && user.Player.Map == 2057U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(user.Player.UID) && MsgSchedules.CaptureTheFlag.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                    {
                                        ++MsgSchedules.CaptureTheFlag.VlmScoreInfoList[user.Player.UID].Shackled;
                                        user.Player.UnShackle(stream, true);
                                    }
                                    if (MsgSchedules.GuildWar.Proces == ProcesType.Alive && user.Player.Map == 1038U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(user.Player.UID) && MsgSchedules.GuildWar.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                    {
                                        ++MsgSchedules.GuildWar.VlmScoreInfoList[user.Player.UID].Shackled;
                                        user.Player.UnShackle(stream, true);
                                    }
                                    MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(attacked.UID, 0, MsgAttackPacket.AttackEffect.None));
                                }
                            }


                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);

                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, 1000, DBSpells);


                            MsgSpell GraceofHeaven;
                            if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.GraceofHeaven, out GraceofHeaven))
                            {
                                if (user.Team != null)
                                {
                                    MsgSpellAnimation MsgSpell2 = new MsgSpellAnimation(user.Player.UID
                        , 0, Attack.X, Attack.Y, GraceofHeaven.ID
                        , GraceofHeaven.Level, GraceofHeaven.UseSpellSoul);

                                    foreach (var member in user.Team.Temates)
                                    {
                                        if (Role.Core.GetDistance(user.Player.X, user.Player.Y, member.client.Player.X, member.client.Player.Y) < 18)
                                        {

                                            member.client.Player.RemoveFlag(MsgUpdate.Flags.Poisoned);
                                            member.client.Player.RemoveFlag(MsgUpdate.Flags.PoisonStar);
                                            MsgSpell2.Targets.Enqueue(new MsgSpellAnimation.SpellObj(member.client.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                        }
                                    }

                                    MsgSpell2.SetStream(stream);
                                    MsgSpell2.Send(user);

                                    Attack.SpellID = GraceofHeaven.ID;
                                    Updates.UpdateSpell.CheckUpdate(stream, user, Attack, 1000, DBSpells);
                                }
                            }

                            break;
                        }
                    case (ushort)Role.Flags.SpellID.Serenity:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                            , 0, Attack.X, Attack.Y, ClientSpell.ID
                            , ClientSpell.Level, ClientSpell.UseSpellSoul);

                            if (user.Player.UID == Attack.OpponentUID)
                            {


                                user.Player.RemoveFlag(MsgUpdate.Flags.Poisoned);
                                user.Player.RemoveFlag(MsgUpdate.Flags.PoisonStar);
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                            }
                            else
                            {
                                Role.IMapObj target;
                                if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                                {
                                    Role.Player attacked = target as Role.Player;

                                    attacked.RemoveFlag(MsgUpdate.Flags.Poisoned);
                                    attacked.RemoveFlag(MsgUpdate.Flags.PoisonStar);
                                    MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(attacked.UID, 0, MsgAttackPacket.AttackEffect.None));

                                }
                            }

                            MsgSpell.SetStream(stream); MsgSpell.Send(user);

                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, 1000, DBSpells);
                            break;
                        }
                }
            }
        }
    }
}
