using VirusX.Game.MsgTournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler
{
    public class DetachStatus
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.ArcherBane:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                            , 0, Attack.X, Attack.Y, ClientSpell.ID
                            , ClientSpell.Level, ClientSpell.UseSpellSoul);


                            Role.IMapObj target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                            {
                                Role.Player attacked = target as Role.Player;
                                if (attacked.ContainFlag(MsgUpdate.Flags.Fly) && !attacked.ContainFlag(MsgUpdate.Flags.HeavensWrath) && !attacked.ContainFlag(MsgUpdate.Flags.Infinity))
                                {
                                    if (Calculate.Base.Rate(DBSpell.Rate + (user.Player.BattlePower - attacked.BattlePower)))
                                    {
                                        attacked.RemoveFlag(MsgUpdate.Flags.Fly);
                                        uint damage = (uint)((double)attacked.HitPoints * 0.1d);
                                        MsgSpellAnimation.SpellObj AnimationObj;
                                        Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);
                                        ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                        MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(attacked.UID, damage, MsgAttackPacket.AttackEffect.None));
                                    }
                                    else
                                    {
                                        var clientobj = new MsgSpellAnimation.SpellObj(attacked.UID, MsgSpell.SpellID, MsgAttackPacket.AttackEffect.None);
                                        clientobj.Hit = 0;
                                        MsgSpell.Targets.Enqueue(clientobj);
                                    }
                                }
                            }
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);

                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, 250, DBSpells);

                            break;
                        }
                    case (ushort)Role.Flags.SpellID.Revive:
                        {
                            if (user.IsWatching())
                            {

                                user.SendSysMesage("This spell not work on this map..");


                                break;
                            }

                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                            , 0, Attack.X, Attack.Y, ClientSpell.ID
                            , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            bool revive = false;
                            user.Player.RemoveFlag(MsgUpdate.Flags.XPList);

                            Role.IMapObj target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                            {
                                Role.Player attacked = target as Role.Player;
                                if (!attacked.ContainFlag(MsgUpdate.Flags.SoulShackle) || attacked.ContainFlag((MsgUpdate.Flags)438))
                                {
                                    MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(attacked.UID, 0, MsgAttackPacket.AttackEffect.None));
                                    attacked.Revive(stream);
                                    if (MsgSchedules.CaptureTheFlag.Proces == ProcesType.Alive && user.Player.Map == 2057U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(user.Player.UID) && MsgSchedules.CaptureTheFlag.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                    {
                                        ++MsgSchedules.CaptureTheFlag.VlmScoreInfoList[user.Player.UID].Revival;
                                        user.Player.Revive(stream, true);
                                    }
                                    if (MsgSchedules.GuildWar.Proces == ProcesType.Alive && user.Player.Map == 1038U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(user.Player.UID) && MsgSchedules.GuildWar.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                    {
                                        ++MsgSchedules.GuildWar.VlmScoreInfoList[user.Player.UID].Revival;
                                        user.Player.Revive(stream, true);
                                    }
                                    revive = true;
                                }
                            }

                            if (revive)
                            {
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);

                                Updates.UpdateSpell.CheckUpdate(stream, user, Attack, DBSpell.Duration, DBSpells);


                                if (!user.Equipment.FreeEquip((Role.Flags.ConquerItem)4) && user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BlessingTouch))
                                {



                                    byte change = 100;

                                    MsgGameItem hossus;
                                    if (user.Equipment.TryGetEquip(Role.Flags.ConquerItem.LeftWeapon, out hossus))
                                    {
                                        if (Database.ItemType.IsHossu(hossus.ITEM_ID))
                                        {
                                            var dbItem = Pool.ItemsBase[hossus.ITEM_ID];
                                            change += (byte)(dbItem.Level / 3);
                                        }
                                    }

                                    if (AttackHandler.Calculate.Base.Rate(change))
                                    {
                                        if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                                        {
                                            Role.Player attacked = target as Role.Player;

                                            attacked.AddSpellFlag(MsgUpdate.Flags.Stigma, (int)80, true);
                                            attacked.AddSpellFlag(MsgUpdate.Flags.MagicShield, (int)120, true);
                                            attacked.AddSpellFlag(MsgUpdate.Flags.StarOfAccuracy, (int)60, true);


                                            Attack.SpellID = (ushort)Role.Flags.SpellID.BlessingTouch;
                                            var Spell = Pool.Magic[Attack.SpellID];

                                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, (uint)(change * 100), Spell);

                                        }
                                    }

                                }
                            }


                            break;
                        }
                    case (ushort)Role.Flags.SpellID.Pray:
                        {
                            if (user.IsWatching())
                            {
                                user.SendSysMesage("This spell not work on this map..");
                                break;
                            }
                            if (user.Player.Map == 700 && !MsgTournaments.MsgSchedules.PkWar.IsFinished())
                                break;
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                            , 0, Attack.X, Attack.Y, ClientSpell.ID
                            , ClientSpell.Level, ClientSpell.UseSpellSoul);

                            Role.IMapObj target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                            {
                                Role.Player attacked = target as Role.Player;

                                if (attacked.Alive || DateTime.Now < attacked.DeadStamp.AddMilliseconds(300)
                                    || attacked.ContainFlag(MsgUpdate.Flags.SoulShackle)
                                    || attacked.ContainFlag((MsgUpdate.Flags)438))
                                {
                                    user.SendSysMesage("Hold on..");
                                    return;
                                    #region arena by WorldConquer
                                }
                                if (user.Player.Map == 5061 || user.Player.Map == 5062 ||
                                    user.Player.Map == 5063 || user.Player.Map == 5064
                                    || user.Player.Map == 5065 ||
                                    user.Player.Map == 5066 || user.Player.Map == 5051
                                    || user.Player.Map == 5052 || user.Player.Map == 5053
                                    || user.Player.Map == 5054 || user.Player.Map == 5055
                                    || user.Player.Map == 5056 || user.Player.Map == 5057
                                    || user.Player.Map == 5058)
                                {
                                    user.CreateBoxDialog("Mt3mlsh Nafsk Nas7 Yad #4K~WorldConquer Here..");
                                    break;


                                    #endregion
                                }

                                if (user.Player.MyGuild != null)
                                {
                                    if (!user.Player.MyGuild.Enemy.ContainsKey(attacked.MyGuild.Info.GuildID))
                                    {
                                        MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(attacked.UID, 0, MsgAttackPacket.AttackEffect.None));
                                        attacked.Revive(stream);
                                        if (MsgSchedules.CaptureTheFlag.Proces == ProcesType.Alive && user.Player.Map == 2057U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(attacked.UID) && MsgSchedules.CaptureTheFlag.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                        {
                                            ++MsgSchedules.CaptureTheFlag.VlmScoreInfoList[user.Player.UID].Revival;
                                            user.Player.Revive(stream, true);
                                        }
                                        if (MsgSchedules.GuildWar.Proces == ProcesType.Alive && user.Player.Map == 1038U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(attacked.UID) && MsgSchedules.GuildWar.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                        {
                                            ++MsgSchedules.GuildWar.VlmScoreInfoList[user.Player.UID].Revival;
                                            user.Player.Revive(stream, true);
                                        }
                                    }
                                    else return;
                                }
                                else
                                {
                                    MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(attacked.UID, 0, MsgAttackPacket.AttackEffect.None));
                                    attacked.Revive(stream);
                                    if (MsgSchedules.CaptureTheFlag.Proces == ProcesType.Alive && user.Player.Map == 2057U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(attacked.UID) && MsgSchedules.CaptureTheFlag.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                    {
                                        ++MsgSchedules.CaptureTheFlag.VlmScoreInfoList[user.Player.UID].Revival;
                                        user.Player.Revive(stream, true);
                                    }
                                    if (MsgSchedules.GuildWar.Proces == ProcesType.Alive && user.Player.Map == 1038U && user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(attacked.UID) && MsgSchedules.GuildWar.VlmScoreInfoList.ContainsKey(user.Player.UID))
                                    {
                                        ++MsgSchedules.GuildWar.VlmScoreInfoList[user.Player.UID].Revival;
                                        user.Player.Revive(stream, true);
                                    }
                                }

                            }

                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);

                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, DBSpell.Duration, DBSpells);




                            if (!user.Equipment.FreeEquip((Role.Flags.ConquerItem)4) && user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BlessingTouch))
                            {

                                MsgGameItem BackSword;
                                if (user.Equipment.TryGetEquip(Role.Flags.ConquerItem.RightWeapon, out BackSword))
                                {

                                    byte change = 100;

                                    MsgGameItem hossus;
                                    if (user.Equipment.TryGetEquip(Role.Flags.ConquerItem.LeftWeapon, out hossus))
                                    {
                                        if (Database.ItemType.IsHossu(hossus.ITEM_ID))
                                        {
                                            var dbItem = Pool.ItemsBase[hossus.ITEM_ID];
                                            change += (byte)(dbItem.Level / 3);
                                        }
                                    }

                                    if (AttackHandler.Calculate.Base.Rate(change))
                                    {
                                        if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                                        {
                                            Role.Player attacked = target as Role.Player;

                                            attacked.AddSpellFlag(MsgUpdate.Flags.Stigma, (int)80, true);
                                            attacked.AddSpellFlag(MsgUpdate.Flags.MagicShield, (int)120, true);
                                            attacked.AddSpellFlag(MsgUpdate.Flags.StarOfAccuracy, (int)60, true);


                                            Attack.SpellID = (ushort)Role.Flags.SpellID.BlessingTouch;
                                            var Spell = Pool.Magic[Attack.SpellID];

                                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, (uint)(change * 100), Spell);

                                        }
                                    }
                                }
                            }
                            break;
                        }

                }
            }
        }
    }
}

