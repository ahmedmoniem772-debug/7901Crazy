using VirusX.Game.MsgServer.AttackHandler.CheckAttack;
using VirusX.Role.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler.Calculate
{
    public class Physical   
    {
       
        public static void OnMonster(Role.Player player, MsgMonster.MonsterRole monster, Database.MagicType.Magic DBSpell, out MsgSpellAnimation.SpellObj SpellObj, byte MultipleDamage = 0, int IncreaseAttack = 0)
        {
            SpellObj = new MsgSpellAnimation.SpellObj(monster.UID, 0, MsgAttackPacket.AttackEffect.None);

            #region FloorItem
            if (monster.IsFloor)
            {
                SpellObj.Damage = 2;
                return;
            }
            #endregion

            #region Calculator(Attack)
            int Damage = (int)Base.GetDamage(player.Owner.Status.MaxAttack, player.Owner.Status.MinAttack);

            Damage = (int)player.Owner.AjustAttack((uint)Damage);
            Damage = (int)player.Owner.AjustMaxAttack((uint)Damage);
            #endregion

            #region Defense(Monster)
            var rawDefense = monster.Family.Defense;

            Damage = Math.Max(0, Damage - rawDefense);

            #endregion

            #region MTD
            bool Effect = false;
            bool DeathSigh = false;
            bool BonePlus = false;
            if (MultipleDamage != 0)
            {
                Damage = Damage * MultipleDamage;
            }
            else
            {
                int IncDamagee = 0;
                if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.WaveBreak))
                {
                    IncDamagee += 250;
                }
                #region BonePluse
                //using (var rec = new ServerSockets.RecycledPacket())
                //{
                //    var stream = rec.GetStream();
                //    MsgSpell Owner_spell = null;
                //    if (DBSpell != null)
                //    {
                //        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                //        {
                //            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.BonePulse))
                //            {
                //                if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BonePulse))
                //                {
                //                    if (player.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.BonePulse, out Owner_spell))
                //                    {

                //                        Database.MagicType.Magic BonePulse = Pool.Magic[Owner_spell.ID][Owner_spell.Level];
                //                        int IncDg = BonePulse.GDamage % 1000;
                //                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                //                            || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                //                            || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive)
                //                        {
                //                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000, 100);
                //                            IncDg -= DBSpell.GDamage % 1000;
                //                        }


                //                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                //                            || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                //                        {
                //                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000, 100);
                //                            IncDg -= DBSpell.GDamage % 1000;
                //                        }


                //                        Damage += (int)(Damage * (IncDg) / 100);
                //                        Effect = true;
                //                        BonePlus = true;
                //                    }
                //                }
                //            }
                //        }
                //    }
                    
                //}
                
                #endregion
                #region BossKiller
                byte BossKillerL = 0;
                if (player.Owner.Rune.IsEquipped("BossKiller", ref BossKillerL) || player.Owner.Rune.IsEquipped("Battle Reaper", ref BossKillerL))
                {
                    if (monster.Boss > 0)
                    {
                        double percent = 0;
                        percent = (double)(210 + (BossKillerL * 10));
                        IncDamagee += (int)percent;
                        if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.Superman))
                            SpellObj.Damage = (uint)(SpellObj.Damage * 20) / 100;

                    }

                }

                #endregion
                if (DBSpell != null && !DeathSigh)
                {
                    #region DBSpell(Edit)

                    if (DBSpell != null && !BonePlus)
                    {
                        #region ThunderStrike
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.CrackingShock)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderBlast)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Megabolt)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DevouringStrike)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WindstormBattleaxe)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.HeavensWrath)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderRampage)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        #endregion

                        #region WindWalker
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleBlasts)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnMonster % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ShadowofChaser)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SwirlingStorm)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.RageofWar)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.BurntFrost)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.AngerofStomper)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.HorrorofStomper)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.PeaceofStomper)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Omnipotence)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage2 % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        #endregion

                        #region Monk
                      




                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.RadiantPalm)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlwindKick)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.InfernalEcho)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WrathoftheEmperor)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
 Effect = true;
                        }

                        #endregion

                        #region Archer
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ArrowBlades)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.BlisteringWave)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ScatterFire)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.MortalWound)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.StarburstArrows)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.RevengeAttack)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.CrackShot)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ElementalArrow)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        #endregion

                        #region Trojan
                        if (IncreaseAttack == 1)
                        {
                            if (DBSpell.ID == (ushort)Role.Flags.SpellID.ScrenSword)
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 1000 + IncDamagee + DBSpell.Damage2 % 1000), 100);
     Effect = true;
                            }

                            if (DBSpell.ID == (ushort)Role.Flags.SpellID.FastBlader)
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 1000 + IncDamagee + DBSpell.Damage2 % 1000), 100);
     Effect = true;
                            }
                        }
                        if (IncreaseAttack == 0)
                        {
                            if (DBSpell.ID == (ushort)Role.Flags.SpellID.ScrenSword)
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.Damage2 % 1000), 100);
     Effect = true;
                            }
                            if (DBSpell.ID == (ushort)Role.Flags.SpellID.FastBlader)
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.Damage2 % 1000), 100);
     Effect = true;
                            }
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Hercules)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Celestial)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Roamer)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Penetration)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Phoenix)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Rage)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.AxeShadow)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.DamageOnHuman % 1000), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.CleanSweep)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.DamageOnHuman % 1000), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SongofPhoenix)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.DamageOnHuman % 1000), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.HookMoon)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.Damage3 % 1000), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderStrike)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.DamageOnHuman % 1000), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DeadlyStrike)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.DamageOnHuman % 1000), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SacredBlessing)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee + DBSpell.DamageOnHuman % 1000), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DeathSigh)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        #endregion

                        #region Warrior
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaveofBlood)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Pounce)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ChargingVortex)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Ironbone)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.IronbonePassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleAttackDragonhowl)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)((DBSpell.GDamage % 1000) * 3 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleAttackBloodlust)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)((DBSpell.GDamage % 1000) * 3 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WildDashAttack)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)((DBSpell.GDamage % 1000) + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleAttackRedcurse)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)((DBSpell.GDamage % 1000) * 3 + IncDamagee), 100);
 Effect = true;
                        }
                        #endregion

                        #region Pirate
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.BladeTempest)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.EagleEye)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.GaleBomb)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (IncreaseAttack == 0 && DBSpell.ID == (ushort)Role.Flags.SpellID.SeaBurial)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (IncreaseAttack == 1 && DBSpell.ID == (ushort)Role.Flags.SpellID.SeaBurial)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.BloomofDeath)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.Drukyle || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.DrukylePassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.SandMist)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.CaptiveArrow)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.HolySanction || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.HolySanctionPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.TwospiendSpear || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.TwospiendSpearpassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
                            Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.PheasantBeak || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.PheasantBeakPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
                            Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.GiantGun || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.GiantGunPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
                            Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.IceAge || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.IceAgePassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
                            Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.LavaSea || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.LavaSeaPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
                            Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.StarVolCano || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.StarVolcanoPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
                            Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.Spitfire || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.SpitfirePassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
                            Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.CannonBarrage)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.ThunderlordAttack)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.BlackbeardsRage)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.NeedLevel % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        #endregion

                        #region Ninja
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SuperTwofoldBlade)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.GDamage % 1000 + IncDamagee), 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.TwilightDance)
                        {
                            if (IncreaseAttack == 1)
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage3 % 1000 + IncDamagee, 100);
     Effect = true;
                            }
                            if (IncreaseAttack == 2)
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.NewDamage % 1000 + IncDamagee, 100);
     Effect = true;
                            }
                            if (IncreaseAttack == 3)
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.NewDamage2 % 1000 + IncDamagee, 100);
     Effect = true;
                            }
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.BloodyScythe)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;

                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.FatalSpin)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireball || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrison || DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachment || DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShuriken || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlash || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ShurikenVortex)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        #endregion

                        #region DW DragonWarrior
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonSlash)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.CrackingSwipe)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SplittingSwipe)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.AirKick)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SpeedKick)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonTransformationPassive)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SuanniRoar)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonRising)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.GDamage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack1 ||
                            DBSpell.ID == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack2 ||
                            DBSpell.ID == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack3)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonCyclone)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Rate % 1000 + IncDamagee, 100);
 Effect = true;
                        }
                        #endregion
                    }
                    #endregion
                }
                #region MonsterHunter
                if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.MonsterHunter))
                {
                    var MonsterHunter = Pool.Magic[(ushort)Role.Flags.SpellID.MonsterHunter][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.MonsterHunter].Level];
                    IncDamagee += MonsterHunter.GDamage % 1000;
                }
                #endregion
                if (!Effect && !BonePlus && !DeathSigh)
                {
                    Damage = Base.MulDiv((int)Damage, (int)((DBSpell != null) ? (uint)(DBSpell.Damage + IncDamagee) : Program.ServerConfig.PhysicalDamage + IncDamagee), 100);
                }
                
            }
            #endregion
            #region DeathSigh
            if (player.ContainFlag(MsgUpdate.Flags.DeathSigh))
            {
                #region VioletShield
                if (player.ContainFlag(MsgUpdate.Flags.DeathSigh))
                {
                    if (DBSpell == null || DBSpell.Passive)
                    {
                        uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                        Damage = Base.MulDiv((int)Damage, (int)(player.DeathSighPassive), 100);
                        DeathSigh = true;
                    }
                    else
                    {
                        uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                        Damage = Base.MulDiv((int)Damage, (int)(player.DeathSighActive), 100);
                        DeathSigh = true;
                    }
                }
                #endregion

            }
            #endregion
            #region Damage(INC)
            Damage += player.getFan(false);

            Damage = Base.AdjustMinDamageUser2Monster(Damage, player.Owner);

            Damage = Base.CalcDamageUser2Monster(Damage, monster.Family.Defense, player.Level, monster.Level, false);

            Damage = (int)Base.BigMulDiv(Damage, monster.Family.Defense2, Client.GameClient.DefaultDefense2);

            if (monster.Family.Defense2 > 0)
                Damage = (int)Calculate.Base.CalculateExtraAttack((uint)Damage, player.Owner.AjustPhysicalDamageIncrease(), 0);
            #endregion
            
            SpellObj.Damage = (uint)Math.Max(1, Damage);

            bool update = false;

            #region CriticalStrike
            if (Role.Core.Rate((int)(player.Owner.Status.CriticalStrike / 100) / 3))
            {
                if (player.Owner.AjustCriticalStrike() / 100 > 0)
                {
                    SpellObj.Effect |= MsgAttackPacket.AttackEffect.CriticalStrike;
                    SpellObj.Damage = (uint)(SpellObj.Damage * 1.5);
                    update = true;
                }
            }
            #endregion

            #region LuckyStrike(X2)
            if (!update && Base.Rate(player.Owner.PerfectionStatus.LuckyStrike / 100))
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    player.View.SendView(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                    {
                        Effect = MsgRefineEffect.RefineEffects.LuckyStrike,
                        Id = player.UID
                    }), true);
                }
                SpellObj.Effect |= MsgAttackPacket.AttackEffect.LuckyStrike;
                SpellObj.Damage = (uint)(SpellObj.Damage * 2);
            }
            #endregion

            #region DashRate
            if (player.Owner.Status.DashRate > 0 && !update)
            {
                if (Base.Rate((int)player.Owner.Status.DashRate / 100))
                {
                    SpellObj.Effect |= MsgAttackPacket.AttackEffect.DashRate;
                    Damage = Base.MulDiv((int)Damage, 125, 100);
                }

            }
            #endregion
            #region Destroy
           
            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.DragonPierceCrit)||player.Owner.MyArchives.isOpen(Archives.TypeID.Dragonhowl) && player.ContainFlag(MsgUpdate.Flags.DragonPierceCrit) && (SpellObj.Effect & MsgAttackPacket.AttackEffect.CriticalStrike) == MsgAttackPacket.AttackEffect.CriticalStrike)
            {
                SpellObj.Effect |= MsgAttackPacket.AttackEffect.DestroyWarrior;
            }
            #endregion

            #region Monster(Guard)
            if ((monster.Family.Settings & MsgMonster.MonsterSettings.Guard) == MsgMonster.MonsterSettings.Guard)
                SpellObj.Damage /= 10;
            #endregion

            #region XpSkill
            if (player.ContainFlag(MsgUpdate.Flags.ManiacDance))
            {
                if (monster.Boss == 0)
                    Damage *= 10;
                else
                    Damage *= 3;
            }

            if (player.ContainFlag(MsgUpdate.Flags.Oblivion))
                Damage = Base.MulDiv((int)Damage, 200, 100);
            if (monster.Boss == 1)
            {
                if (player.ContainFlag(MsgUpdate.Flags.Superman))
                    SpellObj.Damage *= 10;
            }
            if (monster.Boss == 1)
            {
                if (player.ContainFlag(MsgUpdate.Flags.FatalStrike))
                    SpellObj.Damage *= 3;
            }
            #endregion

            #region SuanNiHeart
            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.SuanNiHeart))
            {
                if (DBSpell == null || DBSpell.Passive || player.RandomSpell > 0)
                {

                    Database.MagicType.Magic SuanniHeart = Pool.Magic[(ushort)Role.Flags.SpellID.SuanniHeart][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.SuanniHeart].Level];
                    SpellObj.Damage += (uint)(SpellObj.Damage * SuanniHeart.Damage2) / 100;
                }
                if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.HookMoonCaster))
                    SpellObj.Damage = (uint)(SpellObj.Damage * player.Owner.Player.HookMoonPower / 100);
            }
            #endregion


            #region Monster(MyPrestigePoints)
            if (monster.Boss == 1)
            {
                if (player.Owner.MyPrestigePoints < 50000000)
                {
                    SpellObj.Damage /= 4;
                }
            }
            #endregion

            #region Monster(Damage)//1
            if (monster.Family.Defense2 == 0)
                SpellObj.Damage = 1;
            if (monster.Family.ID == 5385)
                SpellObj.Damage = 0;
            if (monster.Family.ID == 2700)
                SpellObj.Damage = 10;
            if (monster.Name == "AncientDragon") SpellObj.Damage = 1;
            else if (monster.Name == "WarDevil") SpellObj.Damage = 1;
            else if (monster.Name.Contains("Sprite")) SpellObj.Damage = 1;
            if (monster.Name.Contains("GhostReaver") || monster.Name.Contains("ThunderApeKing"))
            {
                SpellObj.Damage = 10000;
            }
            #endregion

            MsgAttackPacket.HitInMele(player, DBSpell);

        }
        public static void OnPlayer(Role.Player player, Role.Player target, Database.MagicType.Magic DBSpell, out MsgSpellAnimation.SpellObj SpellObj, bool StackOver = false, int IncreaseAttack = 0)
        {
            
                SpellObj = new MsgSpellAnimation.SpellObj(target.UID, 0, MsgAttackPacket.AttackEffect.None);
                MsgYuanshen.SwordBody(target.Owner, SpellObj);
                #region ShurikenVortex
                #region YellowRune(Megaquake)
                byte RateMegaquake = 0;
                byte Megaquake = 0;
                if (player.Owner.Rune.IsEquipped("Megaquake", ref Megaquake))
                {
                    switch (Megaquake)
                    {
                        case 1: RateMegaquake = 1; break;
                        case 2: RateMegaquake = 2; break;
                        case 3: RateMegaquake = 3; break;
                        case 4: RateMegaquake = 4; break;
                        case 5: RateMegaquake = 5; break;
                        case 6: RateMegaquake = 6; break;
                        case 7: RateMegaquake = 7; break;
                        case 8: RateMegaquake = 8; break;
                        case 9: RateMegaquake = 9; break;
                        case 10: RateMegaquake = 10; break;
                        case 11: RateMegaquake = 12; break;
                        case 12: RateMegaquake = 14; break;
                        case 13: RateMegaquake = 16; break;
                        case 14: RateMegaquake = 18; break;
                        case 15: RateMegaquake = 20; break;
                        case 16: RateMegaquake = 22; break;
                        case 17: RateMegaquake = 24; break;
                        case 18: RateMegaquake = 26; break;
                        case 19: RateMegaquake = 28; break;
                        case 20: RateMegaquake = 30; break;
                        case 21: RateMegaquake = 33; break;
                        case 22: RateMegaquake = 36; break;
                        case 23: RateMegaquake = 39; break;
                        case 24: RateMegaquake = 42; break;
                        case 25: RateMegaquake = 45; break;
                        case 26: RateMegaquake = 50; break;
                        case 27: RateMegaquake = 55; break;
                    }
                }
                #endregion
                if (!Base.Rate(RateMegaquake))
                {
                    if (target.ContainFlag(MsgUpdate.Flags.ShurikenVortex) || target.ContainFlag(MsgUpdate.Flags.ManiacDance))
                    {
                        SpellObj.Damage = 1;
                        return;
                    }
                }
                #endregion

                #region DragonSwing
                if (target.ContainFlag(MsgUpdate.Flags.DragonSwing))
                {
                    byte additionalRate = 0;
                    if (target.Owner.Rune.IsEquipped("SwingingTail") && DBSpell != null && (DBSpell.ID == (ushort)Role.Flags.SpellID.EagleEye || DBSpell.ID == (ushort)Role.Flags.SpellID.BlackbeardsRage || DBSpell.ID == (ushort)Role.Flags.SpellID.KineticSpark || DBSpell.ID == (ushort)Role.Flags.SpellID.DaggerStorm || DBSpell.ID == (ushort)Role.Flags.SpellID.BlisteringWave || DBSpell.ID == (ushort)Role.Flags.SpellID.MortalWound))//SwingingTail(Dragon Warrior Rune)
                        additionalRate += 30;
                    if (Calculate.Base.Rate(target.DragonSwingChance + additionalRate))
                    {
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            target.SendString(stream, MsgStringPacket.StringID.Effect, true, new string[1] { "poisonmiss" });
                        }
                        SpellObj.Damage = 0;
                        return;
                    }
                }
                #endregion

                #region FreezingPelter
                if (player.ContainFlag(MsgUpdate.Flags.FreezingPelter) && Calculate.Base.Rate(30 + (int)(player.BattlePower - (player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.DeathSigh) && target.BattlePower > player.BattlePower ? player.BattlePower : target.BattlePower) * 5)))
                {
                    target.AddFlag(MsgUpdate.Flags.xFreezingPelter, 10, true);
                }
                #endregion

                bool update = false;

                #region Dodged
                if (DBSpell == null)
                {
                    if (Base.Dodged(player.Owner, target.Owner))
                    {
                        SpellObj.Damage = 0;
                        return;
                    }
                }
                #endregion

                #region HideGui
                unsafe
                {
                    using (var rect = new ServerSockets.RecycledPacket())
                    {
                        var stream = rect.GetStream();
                        ActionQuery action = new ActionQuery()
                        {
                            ObjId = target.UID,
                            Type = ActionType.HideGui,
                        };
                        target.Owner.Send(stream.ActionCreate(action));
                    }
                }
                #endregion

                #region Calculator(Attack)
                int Damage = (int)Base.GetDamage(player.Owner.Status.MaxAttack, player.Owner.Status.MinAttack);

            Damage = (int)player.Owner.AjustAttack((uint)Damage);
            Damage = (int)player.Owner.AjustMaxAttack((uint)Damage);
            #endregion

                #region AjustDefense
            var rawDefense = target.Owner.AjustDefense;
                if (target.ContainFlag(MsgUpdate.Flags.DefensiveStance) && target.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.DefensiveStance))
                {
                    Database.MagicType.Magic DefensiveStance = Pool.Magic[(ushort)Role.Flags.SpellID.DefensiveStance][target.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.DefensiveStance].Level];
                    rawDefense = (rawDefense * (uint)(DefensiveStance.GDamage % 1000)) / 100;
                }
           
                if (player.Owner.Rune.IsEquipped("Wild Cleave"))
                {
                    MsgGameItem Shield;
                    if (target.Owner.Equipment.TryGetEquip(Role.Flags.ConquerItem.LeftWeapon, out Shield))
                    {
                        if (Database.ItemType.IsShield(Shield.ITEM_ID))
                        {
                            int Extra = 0;
                            #region SoulExtra
                            if (Shield.Purification.InLife)
                            {
                                var purificare = Pool.ItemsBase[Shield.Purification.PurificationItemID];
                                Extra += purificare.PhysicalDefence;
                            }
                            #endregion
                            #region ItemExtra&&ItemBounce
                            var DBItem = Pool.ItemsBase[Shield.ITEM_ID];
                            byte AnimaPercent = (byte)(Shield.AnimaItemID > 0 ? Pool.ItemsBase[Shield.AnimaItemID].ItemHP / 100 : 0);
                            var extraitematributes = DBItem.Plus[Shield.Plus];
                            Extra += ((extraitematributes.PhysicalDefence + (extraitematributes.PhysicalDefence * AnimaPercent / 100)) + DBItem.PhysicalDefence);
                            #endregion
                            rawDefense -= (uint)Extra;
                        }
                    }
                }
                if (Damage > rawDefense)
                    Damage -= (int)rawDefense;
                else
                    Damage = 1;
                #endregion

                bool Effect = false;

                int ReduceWarAegis = 0;

                #region War Aegis
                if (target.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.WarAegis))
                {
                    var WarAgeis = Pool.Magic[(ushort)Role.Flags.SpellID.WarAegis][target.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.WarAegis].Level];
                    if (WarAgeis != null)
                    {
                        ReduceWarAegis = (int)WarAgeis.Damage2 / 10;
                    }
                }
                #endregion

                #region BonePluse
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    MsgSpell Owner_spell = null;
                    if (DBSpell != null)
                    {
                        //if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                        //                || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                        //                || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive
                        //                || DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                        //                || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                        //{
                        //    if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.BonePulse))
                        //    {
                        //        if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BonePulse))
                        //        {
                        //            if (player.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.BonePulse, out Owner_spell))
                        //            {

                        //                Database.MagicType.Magic BonePulse = Pool.Magic[Owner_spell.ID][Owner_spell.Level];
                        //                int IncDg = BonePulse.Damage2 % 1000;
                        //                if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                        //                    || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                        //                    || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive)
                        //                {
                        //                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000) - ReduceWarAegis, 100);
                        //                    IncDg -= DBSpell.Damage2 % 1000;
                        //                }


                        //                if (DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                        //                    || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                        //                {
                        //                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 1000) - ReduceWarAegis, 100);
                        //                    IncDg -= DBSpell.DamageOnHuman % 1000;
                        //                }


                        //                Damage += (int)(Damage * (IncDg) / 100);
                              
                        //                Effect = true;
                        //            }
                        //        }
                        //    }
                        //}
                        if (player.ContainFlag(MsgUpdate.Flags.DeathSigh))
                        {
                            #region VioletShield
                            if (player.ContainFlag(MsgUpdate.Flags.DeathSigh))
                            {
                                byte VioletShieldL = 0, chance = 0;
                                if (target.Owner.Rune.IsEquipped("VioletShield", ref VioletShieldL))
                                {
                                    chance = (byte)(VioletShieldL * 10);
                                    if (VioletShieldL == 8) chance = 85;
                                    if (VioletShieldL == 9) chance = 100;
                                }
                                if (!Base.Rate(chance))
                                {
                                    if (DBSpell == null || DBSpell.Passive)
                                    {
                                        uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                                        Damage = Base.MulDiv((int)Damage, (int)(DBSpellDG + player.DeathSighPassive) - ReduceWarAegis, 100);
                                        Effect = true;
                                    }
                                    else
                                    {
                                        uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                                        Damage = Base.MulDiv((int)Damage, (int)(DBSpellDG + player.DeathSighActive) - ReduceWarAegis, 100);
                                        Effect = true;
                                    }
                                }
                            }
                            #endregion

                        }
                       
                    if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.Commanding) && player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.SuanniCommand))
                    {
                        if (DBSpell != null)
                        {

                            if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonRising || DBSpell.ID == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack1 || DBSpell.ID == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack2 || DBSpell.ID == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack3)
                            {
                                Database.MagicType.Magic SuanniCommand = Pool.Magic[(ushort)Role.Flags.SpellID.SuanniCommand][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.SuanniCommand].Level];
                                uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                     
                                Effect = true;
                                if (player.SuanniCommandCount == 5)
                                    Damage = Base.MulDiv((int)Damage, (int)(DBSpellDG + 30) - ReduceWarAegis, 100);
                                else
                                    Damage = Base.MulDiv((int)Damage, (int)(DBSpellDG + (player.SuanniCommandCount * 5)) - ReduceWarAegis, 100);
                            }
                        }

                    }
                }
                }
                #endregion

                #region Edit Damage(MT)
                if (DBSpell != null && !Effect)
                {
                  
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleAttackBloodlust
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.TripleAttackRedcurse)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage * 2 - ReduceWarAegis, 100);
                        update = true;
                    }
                    #region Edit Damage(Index)[10]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleBlasts)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage / 2 - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.BladeTempest)
                    {
                        MsgGameItem RightWeapon;
                        MsgGameItem LeftWeapon;
                        if (player.Owner.Equipment.TryGetEquip(Role.Flags.ConquerItem.RightWeapon, out RightWeapon)
                            && player.Owner.Equipment.TryGetEquip(Role.Flags.ConquerItem.LeftWeapon, out LeftWeapon))
                        {
                            if (Database.ItemType.IsPirateEpicWeapon(RightWeapon.ITEM_ID) && Database.ItemType.IsPirateEpicWeapon(LeftWeapon.ITEM_ID))
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                                update = true;
                            }
                        }

                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.SwirlingStorm)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage / 4 - ReduceWarAegis, 100);
                        update = true;
                    }
                #endregion
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.VajraRing)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.ZenStaff)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.ClawStrike)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.ClapBomb)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.FlowerTouch)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.VajraPalm)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.VioletBowl)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.QuellingRobe)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.BellShield)
                {
                    Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                    update = true;
                }
              
                #region DamageOnHuman(Index)[35]
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderStrike
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.CleanSweep
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SongofPhoenix
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.AxeShadow
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DeadlyStrike
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SacredBlessing
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DeathSigh
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.Celestial
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.Roamer
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWind
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachment
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.BloodyScythe
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WildDashAttack
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.ArrowBlades
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.RevengeAttack
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.Megabolt
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.SkyFall
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderBlast
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.StarburstArrows
                         || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.Drukyle
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.StarVolCano
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.Spitfire
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.SandMist
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.CaptiveArrow
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.SpitfirePassive
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.StarVolcanoPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.LavaSeaPassive
                                         || DBSpell.ID == (ushort)Role.Flags.SpellID.DragonTransformationPassive

                         || DBSpell.ID == (ushort)Role.Flags.SpellID.SuanniRoar
                               || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.DrukylePassive
                                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.LavaSea
                           || DBSpell.ID == (ushort)Role.Flags.SpellID.DragonRising
                     )
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                        update = true;
                    }

                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.StarFlow)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                        update = true;
                    }



                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.ElementalArrow)
                    {
                        uint DamageInc = 0;
                        Database.MagicType.Magic ElementalArrow = Pool.Magic[(ushort)Role.Flags.SpellID.ElementalArrow][(ushort)player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.ElementalArrow].Level];
                        if (player.ContainFlag(MsgUpdate.Flags.FireArrow)
                               || player.ContainFlag(MsgUpdate.Flags.IceArrow)
                               || player.ContainFlag(MsgUpdate.Flags.PoisonArrow)
                               || player.ContainFlag(MsgUpdate.Flags.ThunderArrow)
                               || player.ContainFlag(MsgUpdate.Flags.WindArrow))
                        {
                            if (player.ContainFlag(MsgUpdate.Flags.FireArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.FireArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.IceArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.IceArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.PoisonArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.PoisonArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.ThunderArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.ThunderArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.WindArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.WindArrow);
                                DamageInc += 20;
                            }


                        }
                        Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000 + DamageInc) - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.FireArrow)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) - ReduceWarAegis, 100);
                        update = true;


                    }

                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.CrackShot)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + DBSpell.Damage3 % 1000 - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (IncreaseAttack == 1)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonSlash)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ScrenSword)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.FastBlader)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                    }
                    if (IncreaseAttack == 0)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonSlash)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)((DBSpell.DamageOnHuman % 1000) + DBSpell.Damage3) - ReduceWarAegis, 100);
                            update = true;
                        }
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.DevouringStrike)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 100 - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonCyclone)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 100 - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderRampage)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)50 - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                        update = true;
                    }
                    #endregion

                    #region Damage2(Index)[36]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireball
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShuriken
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlash
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrison
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.BonePulse
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.Ironbone
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.IronbonePassive
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.MortalWound
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.BlisteringWave
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.PheasantBeak
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.IceAge
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.GiantGun
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.ThunderlordAttack
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.GiantGunPassive
            || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.HolySanction
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.HolySanctionPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.TwospiendSpearpassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.PheasantBeakPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.IceAgePassive)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage2 % 1000 - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (IncreaseAttack == 1)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SeaBurial)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage2 % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.TwospiendSpear)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000 + DBSpell.DamageOnHuman) - ReduceWarAegis, 100);
                        update = true;
                    }
                    #endregion

                    #region Damage3(Index)[37]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterShockwave
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.HookMoon
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.HellVortex)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage3 % 1000 - ReduceWarAegis, 100);
                        update = true;

                    }

                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.TwilightDance)
                    {
                        if (IncreaseAttack == 1)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage3 % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                        if (IncreaseAttack == 2)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.NewDamage % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                        if (IncreaseAttack == 3)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.NewDamage2 % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }


                    }
                    #endregion

                    #region DamageOnMonster(Index)[44]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.NormalATK1
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.WarSuit401NormalATK1
                      || DBSpell.ID == (ushort)Role.Flags.SpellID.WarSuit402NormalATK1
                                    || DBSpell.ID == (ushort)Role.Flags.SpellID.WarSuit402NormalATK2
                                                  || DBSpell.ID == (ushort)Role.Flags.SpellID.WarSuit402NormalATK3
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.NormalATK2
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.WarSuit401NormalATK2
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.NormalATK3
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.CrackingShock
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WindstormBattleaxe
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderRampage
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.CannonBarrage
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.Wildwind || DBSpell.ID == (ushort)Role.Flags.SpellID.HeavensWrath)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnMonster % 1000 - ReduceWarAegis, 100);
                        update = true;
                    }
                    if (IncreaseAttack == 1)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SuperTwofoldBlade)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnMonster % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                    }

                   
                    #endregion
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.ChaoticDanceAttack)
                    {
                        #region SilentBlade

                        double RateSilentBlade= 0;
                        int reduceDMG = 0;
                        byte SilentBlade = 0;
                        if (target.Owner.Rune.IsEquipped("Silent Blade", ref SilentBlade))
                        {
                            switch (SilentBlade)
                            {
                                case 1: RateSilentBlade = 25; break;
                                case 2: RateSilentBlade = 27; break;
                                case 3: RateSilentBlade = 29; break;
                                case 4: RateSilentBlade = 31; break;
                                case 5: RateSilentBlade = 33; break;
                                case 6: RateSilentBlade = 36; break;
                                case 7: RateSilentBlade = 39; break;
                                case 8: RateSilentBlade = 42; break;
                                case 9: RateSilentBlade = 45; break;
                            }
                           
                        }
                        #endregion
                        if (IncreaseAttack == 1)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage3 % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                        if (IncreaseAttack == 2)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage2 % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                        if (IncreaseAttack == 3)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 - ReduceWarAegis, 100);
                            update = true;
                        }
                        if (RateSilentBlade > 0)
                        {
                            reduceDMG = (int)Damage * (int)RateSilentBlade / 100;
                            Damage -= reduceDMG;

                        }


                    }
                }

                #endregion

                #region Damage(Index)[10]

                if (DBSpell != null)
                {
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.HeavensWonder)
                    {
                        if (DBSpell.Damage > 0)
                            DBSpell.Damage *= 2;
                    }
                }
                if (!update && !Effect)
                {
                    if (DBSpell != null)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.HeavensWonder)
                        {
                            if (DBSpell.Damage > 0)
                                DBSpell.Damage *= 2;
                        }
                    }
                    if (IncreaseAttack == 0)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)((DBSpell != null && DBSpell.Damage > 0) ? DBSpell.Damage : Program.ServerConfig.PhysicalDamage) - ReduceWarAegis, 100);
                    }
                }
                #endregion
                
                bool onbreak = false;

                update = false;

                bool Strike = false;

                #region TailedBeast
                if (target.Owner.Beasts.Activated && Base.Rate(1))
                    SpellObj.Effect |= MsgAttackPacket.AttackEffect.TailedBeast;
                #endregion

                #region CriticalStrike
                if (player.Owner.AjustCriticalStrike() > 0 && !Effect)
                {
                    uint WhirlSigilSage1 = 0;
                    uint WhirlSigilSage2 = 0;
                    if (DBSpell != null)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShuriken)
                        {
                            Ninja.Item item;
                            if (player.Owner.MyNinja.TryGetValueEquip(Ninja.ItemType.WhirlSigilSage, out item))
                            {
                                WhirlSigilSage1 = (uint)item.DBItem.Power / 100;
                                WhirlSigilSage2 = (uint)item.DBItem.Damage;
                            }
                        }
                    }
                    if (Base.GetRefinery((player.Owner.AjustCriticalStrike() / 100) + WhirlSigilSage1, target.Owner.AjustImunity() / 100))
                    {
                        SpellObj.Effect |= MsgAttackPacket.AttackEffect.CriticalStrike;
                        if (player.Owner.Rune.IsEquipped("StrikeBooster"))
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(120 + WhirlSigilSage2), 100);
                        }
                        else
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(100 + WhirlSigilSage2), 100);
                        }


                        update = true;
                    }
                }
                #endregion

                #region LuckyStrike(X2)
                #region Parry
                int Parry = target.Owner.PerfectionStatus.Parry / 100;
                if (Parry > (player.Owner.PerfectionStatus.LuckyStrike / 100))
                    Effect = true;
                #endregion
                if (!update && Base.Rate(player.Owner.PerfectionStatus.LuckyStrike / 100) && !Effect && player.Owner.PrestigeLevel > target.Owner.PrestigeLevel)
                    if (Base.Rate(player.Owner.PerfectionStatus.LuckyStrike / 100))
                    {
                        if (Base.Rate(target.Owner.PerfectionStatus.StrikeLock))
                        {
                            if (!(Base.Rate(player.Owner.PerfectionStatus.AbsoluteLuck) && player.Owner.PrestigeLevel > target.Owner.PrestigeLevel))
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    target.View.SendView(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                                    {
                                        Effect = MsgRefineEffect.RefineEffects.StrikeLockLevel,
                                        Id = player.UID
                                    }), true);
                                }
                            }
                            else
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    target.View.SendView(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                                    {
                                        Effect = MsgRefineEffect.RefineEffects.AbsoluteLuck,
                                        Id = player.UID
                                    }), true);
                                }
                                SpellObj.Effect |= MsgAttackPacket.AttackEffect.LuckyStrike;
                                Damage = (int)(Damage * 2);
                                update = true;
                            }
                        }
                        else
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                target.View.SendView(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                                {
                                    Effect = MsgRefineEffect.RefineEffects.LuckyStrike,
                                    Id = player.UID
                                }), true);
                            }
                            SpellObj.Effect |= MsgAttackPacket.AttackEffect.LuckyStrike;
                            Damage = (int)(Damage * 2);
                            update = true;
                        }
                    }
                #endregion

                #region Breakthrough
                if (!update && player.Owner.AjustBreakthrough() > 0 )
                {
                    int breaks = (int)player.Owner.AjustBreakthrough();
                    #region DragonPierceBreak
                    int DragonBreak = 0;
                    if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.DragonPierce))
                    {
                        Database.MagicType.Magic DragonPierces = Pool.Magic[(ushort)Role.Flags.SpellID.DragonPierce][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.DragonPierce].Level];
             
                        if (player.ContainFlag(MsgUpdate.Flags.DragonPierceBreak))
                        {
                            if (player.Owner.MyArchives.isOpen(Role.Instance.Archives.TypeID.Dragonhowl))
                                DragonBreak = (DragonPierces.Damage2 / 100);
                        }
                    }
                    #endregion
                    #region Front Break
                    byte FrontBreakL = 0;
                    ushort INCB = 0;
                    if (player.Owner.Rune.IsEquipped("Front Break", ref FrontBreakL) && player.Owner.Player.ContainFlag(MsgUpdate.Flags.FrontBreak))
                    {
                        switch (FrontBreakL)
                        {
                            case 1: INCB = 10; break;
                            case 2: INCB = 20; break;
                            case 3: INCB = 30; break;
                            case 4: INCB = 40; break;
                            case 5: INCB = 50; break;
                            case 6: INCB = 60; break;
                            case 7: INCB = 70; break;
                            case 8: INCB = 80; break;
                            case 9: INCB = 100; break;
                        }
                    }
                    #endregion
                    if (DBSpell != null)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlash)
                        {
                            breaks += 150;
                        }
                        Ninja.Item item;
                        if (player.Owner.MyNinja.TryGetValueEquip(Ninja.ItemType.SlashSealFlash, out item))
                        {
                            if (!player.Owner.Player.ContainFlag((MsgUpdate.Flags)290))
                                player.Owner.Player.AddFlag((MsgUpdate.Flags)290, 20, true);
                            if (player.Owner.Player.ContainFlag((MsgUpdate.Flags)290))
                            {
                                Role.StatusFlagsBigVector32.Flag Flags;
                                if (player.Owner.Player.BitVector.ArrayFlags.TryGetValue(290, out Flags))
                                {
                                    if (Flags.InvokerSecouds == 0)
                                    {
                                        breaks += 350;

                                    }
                                }
                            }

                        }
                    }
                    int rateBreakthrough = ((int)((breaks + INCB) - (target.Owner.Status.Counteraction)) / 10);
                    if (player.BattlePower < target.BattlePower)
                    {
                        if (Role.Core.Rate(rateBreakthrough - (int)DragonBreak))
                    {
                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                        {
                            var streamm = recycledPacket.GetStream();
                            target.Owner.Send(new MsgServer.MsgMessage("Breakthrough Has a chance to exceed Max Attack when the enemy has a higher BP.", MsgMessage.MsgColor.red, MsgMessage.ChatMode.TopLeft).GetArray(streamm));
                            SpellObj.Effect |= MsgAttackPacket.AttackEffect.Break;
                            onbreak = true;
                        }
                    }
                }
                }
                #endregion

                #region DashRate
                if (!Strike)
                {
                    if (Base.GetRefinery((player.Owner.Status.DashRate / 100), target.Owner.Status.Resist / 100))
                    {
                        SpellObj.Effect |= MsgAttackPacket.AttackEffect.DashRate;
                        Damage = Base.MulDiv((int)Damage, 125, 100);
                    }
                }
                #endregion

                #region EditDamage!Break
                if (onbreak == false)
                {
                    var olddamage = Damage;
                    Damage = Database.Disdain.UserAttackUser(player, target, Damage);
                    if (Damage < olddamage)
                    {
                        Damage += olddamage / 4;
                    }
                }
                #endregion

                #region GetDefence
                if (target.Reborn > 0)
                    Damage = (int)Base.BigMulDiv((int)Damage, 6000, Client.GameClient.DefaultDefense2);
                #endregion

                #region Blessed
                Damage -= (int)(Damage * target.Owner.Status.ItemBless / 100);
                #endregion

                Damage = (int)Calculate.Base.CalculateExtraAttack((uint)Damage, player.Owner.Status.PhysicalDamageIncrease, target.Owner.Status.PhysicalDamageDecrease);

                #region UniversalShield
                byte UniversalShieldL = 0;
                uint FinalPD = 0;
                if ((target.Owner.Rune.IsEquipped("UniversalShield", ref UniversalShieldL) || target.Owner.Rune.IsEquipped("Indestructible Balance", ref UniversalShieldL)) && target.Owner.Status.PhysicalDamageDecrease < player.Owner.Status.PhysicalDamageIncrease)
                {
                    if (player.BattlePower < target.BattlePower && !player.ContainFlag(MsgUpdate.Flags.DeathSigh))
                    {
                        switch (UniversalShieldL)
                        {
                            case 1: FinalPD = 5000; break;
                            case 2: FinalPD = 6000; break;
                            case 3: FinalPD = 6500; break;
                            case 4: FinalPD = 7000; break;
                            case 5: FinalPD = 7500; break;
                            case 6: FinalPD = 8000; break;
                            case 7: FinalPD = 8500; break;
                            case 8: FinalPD = 9000; break;
                            case 9: FinalPD = 10000; break;
                        }
                    }
                    else if (player.BattlePower > target.BattlePower)
                    {
                        switch (UniversalShieldL)
                        {
                            case 1: FinalPD = 5000; break;
                            case 2: FinalPD = 5000; break;
                            case 3: FinalPD = 7000; break;
                            case 4: FinalPD = 8000; break;
                            case 5: FinalPD = 9000; break;
                            case 6: FinalPD = 10000; break;
                            case 7: FinalPD = 11000; break;
                            case 8: FinalPD = 12000; break;
                            case 9: FinalPD = 15000; break;
                        }
                    }
                    uint FinalPDM = (player.Owner.Status.PhysicalDamageIncrease - target.Owner.Status.PhysicalDamageDecrease);
                    if (FinalPDM > FinalPD)
                        FinalPDM = FinalPD;
                    Damage -= (int)FinalPDM;
                    SpellObj.Effect |= MsgAttackPacket.AttackEffect.UniversalShieldBarrier;
                    
                }
                #endregion

                #region TortoisePercent %100
                var TortoisePercent = target.Owner.GemValues(Role.Flags.Gem.NormalTortoiseGem);
                if (TortoisePercent > 0)
                {
                    if (TortoisePercent > 46)
                        TortoisePercent = 46;
                    #region TortoiseSmasher
                    byte TortoiseSmasherL = 0;
                    int ignores = 0;
                    if (player.Owner.Rune.IsEquipped("TortoiseSmasher", ref TortoiseSmasherL) || player.Owner.Rune.IsEquipped("Tortoise Predator", ref TortoiseSmasherL))
                    {
                        switch (TortoiseSmasherL)
                        {
                            case 1: ignores = 3; break;
                            case 2: ignores = 4; break;
                            case 3: ignores = 5; break;
                            case 4: ignores = 6; break;
                            case 5: ignores = 7; break;
                            case 6: ignores = 8; break;
                            case 7: ignores = 9; break;
                            case 8: ignores = 10; break;
                            case 9: ignores = 12; break;
                        }
                        TortoisePercent -= (uint)ignores;
                    }
                    #endregion
                    if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.TenFistSword))
                    {
                        var DBSpells = Pool.Magic[(ushort)Role.Flags.SpellID.TenFistSword][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.TenFistSword].Level];
                        if (target.Owner.Status.Damage > 50)
                        {
                            TortoisePercent -= (uint)(TortoisePercent * DBSpells.DamageOnHuman) / 100;
                        }
                    }
                    #region TortoiseBreaker

                    byte TortoiseBreakerL = 0;
                    int decrease = 0;
                    if (player.Owner.Rune.IsEquipped("TortoiseBreaker", ref TortoiseBreakerL) || player.Owner.Rune.IsEquipped("Tortoise Predator", ref TortoiseBreakerL))
                    {
                        switch (TortoiseBreakerL)
                        {
                            case 1: decrease = 500; break;
                            case 2: decrease = 1000; break;
                            case 3: decrease = 1500; break;
                            case 4: decrease = 2000; break;
                            case 5: decrease = 2500; break;
                            case 6: decrease = 3000; break;
                            case 7: decrease = 3500; break;
                            case 8: decrease = 4000; break;
                            case 9: decrease = 5000; break;
                        }
                        SpellObj.Effect |= MsgAttackPacket.AttackEffect.TortoiseBreaker;
                    }
                    Damage += decrease;
                    #endregion
     
                    #region BonePluse
                    //using (var rec = new ServerSockets.RecycledPacket())
                    //{
                    //    var stream = rec.GetStream();
                    //    MsgSpell Owner_spell = null;
                    //    if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.BonePulse))
                    //    {
                    //        if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BonePulse))
                    //        {
                    //            if (player.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.BonePulse, out Owner_spell))
                    //            {
                    //                Database.MagicType.Magic BonePulse = Pool.Magic[Owner_spell.ID][Owner_spell.Level];
                    //                TortoisePercent -= (uint)(TortoisePercent * BonePulse.DamageOnHuman) / 100;
                                
                    //            }
                    //        }
                    //    }
                    //}
                #endregion
                #region DragonPierceTortoise
                MsgSpell ClientSpell;
                if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.DragonPierceTortoise))
                {
                    if (player.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.DragonPierce, out ClientSpell))
                    {
                        Dictionary<ushort, Database.MagicType.Magic> DBSpells;
                        if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.DragonPierce, out DBSpells))
                        {
                            if (DBSpells.TryGetValue(ClientSpell.Level, out DBSpell))
                            {
                                TortoisePercent -= (uint)(TortoisePercent * DBSpell.DamageOnHuman) / 100;
                            }
                        }
                    }

                }
                #endregion
                Damage -= (int)(Damage * TortoisePercent) / 100;
                }
                if (target.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.EightSpanMirror))
                {
                    var DBSpells = Pool.Magic[(ushort)Role.Flags.SpellID.EightSpanMirror][target.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.EightSpanMirror].Level];
                    if (target.Owner.Status.Damage < 55)
                    {
                        int Damagev = DBSpells.DamageOnHuman;
                        #region TortoiseSmasher
                        byte TortoiseSmasherL = 0;
                        int ignores = 0;
                        if (player.Owner.Rune.IsEquipped("TortoiseSmasher", ref TortoiseSmasherL) || player.Owner.Rune.IsEquipped("Tortoise Predator", ref TortoiseSmasherL))
                        {
                            switch (TortoiseSmasherL)
                            {
                                case 1: ignores = 3; break;
                                case 2: ignores = 4; break;
                                case 3: ignores = 5; break;
                                case 4: ignores = 6; break;
                                case 5: ignores = 7; break;
                                case 6: ignores = 8; break;
                                case 7: ignores = 9; break;
                                case 8: ignores = 10; break;
                                case 9: ignores = 12; break;
                            }
                            Damagev -= (int)ignores;
                        }
                        #endregion
                        Damage -= (int)(Damage * Damagev) / 100;
                    }
                }
                #endregion
                #region Slayer
                if (player.ContainFlag(MsgUpdate.Flags.Slayer) && player.Owner.Rune.IsEquipped("Slayer"))
                {
                    if (DBSpell != null)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.Windstorm || DBSpell.ID == (ushort)Role.Flags.SpellID.BloomofDeath)
                        {

                            Damage += Base.MulDiv((int)Damage, 30, 100);
                        }
                        else
                        {
                            uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                            #region Slayer
                            if (player.ContainFlag(MsgUpdate.Flags.Slayer) && player.Owner.Rune.IsEquipped("Slayer"))
                            {
                                if (DBSpellDG < 110)
                                {
                                    Damage += Base.MulDiv((int)Damage, 10, 100);
                                }
                            }
                            #endregion

                        }
                    }
                }
                #endregion
                SpellObj.Damage = (uint)Math.Max(1, Damage);

                #region Judgment
                byte itemLevell = 0;
                byte chanceIgnoreAzure5 = 0;
                if (player.Owner.Rune.IsEquipped("Judgment", ref itemLevell))
                {
                    chanceIgnoreAzure5 = (byte)(10 + (itemLevell * 5));
                    if (itemLevell == 9) chanceIgnoreAzure5 = 60;
                }
                #endregion

                #region Crack
                bool Crack = false;
                if (player.Owner.Status.Crack > 0)
                {
                    Database.MythSoulAttributes.Attribute MythInfo;
                    if (Database.MythSoulAttributes.Attributes[Database.MythSoulAttributes.Type.Crack].TryGetValue(player.Owner.Status.Crack, out MythInfo))
                    {
                        double IncRate = 0;
                    Database.MythSoulAttributes.Attribute SuperPower;
                    if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.SuperPower))
                        {
                            if (Database.MythSoulAttributes.Attributes[Database.MythSoulAttributes.Type.Superpower].TryGetValue(player.Owner.Status.Superpower, out SuperPower))
                            {
                                IncRate = (double)SuperPower.Damage / 100;
                            }
                        }
                        double IncRatee = 0;
                    Database.MythSoulAttributes.Attribute Oracle;
                    if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.Oracle))
                        {
                            if (Database.MythSoulAttributes.Attributes[Database.MythSoulAttributes.Type.Oracle].TryGetValue(player.Owner.Status.Oracle, out Oracle))
                            {
                                IncRatee = (double)Oracle.Damage / 100;
                            }
                        }
                        if (Calculate.Base.Rate((int)MythInfo.Rate + (int)IncRate - (int)IncRatee))
                        {
                            Crack = true;
                        }
                    }
                }
            #endregion

            #region AzureShield
            bool Fusing = false;
            if (player.Owner.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.Fusing) && player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellIDPirate.Fusing))
            {
                var Fusings = Pool.Magic[(ushort)Role.Flags.SpellIDPirate.Fusing][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellIDPirate.Fusing].Level];
                if (Calculate.Base.Rate(Fusings.GDamage / 100))
                {
                    Fusing = true;
                }
            }
            if (DBSpell != null)
            {
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.FlameofDestruction)
                {
                    Fusing = true;
                }

            }
       
            if (target.ContainFlag(MsgUpdate.Flags.AzureShield))
            {
                if (!Fusing)
                {
                    if (!Crack)
                    {
                        if (!Calculate.Base.Rate(chanceIgnoreAzure5))
                        {

                            #region AzureShieldDefence

                            if (SpellObj.Damage > target.AzureShieldDefence)
                            {
                                Calculate.AzureShield.CreateDmg(player, target, target.AzureShieldDefence);
                                target.RemoveFlag(MsgUpdate.Flags.AzureShield);
                                SpellObj.Damage -= target.AzureShieldDefence;

                            }
                            else
                            {
                                target.AzureShieldDefence -= (ushort)SpellObj.Damage;
                                Calculate.AzureShield.CreateDmg(player, target, SpellObj.Damage);
                                SpellObj.Damage = 1;
                            }
                            #endregion

                        }
                        else
                        {
                            SpellObj.Effect |= MsgAttackPacket.AttackEffect.BreakDown;
                        }
                    }
                }
                else
                {
                    SpellObj.Effect = MsgAttackPacket.AttackEffect.CrackMyth;
                }
            }

            #endregion

            #region Block
            int Block = (int)(target.Owner.Status.Block / 100);
            Block += (int)((target.ShieldBlockDamage * Block) / 100);
            uint Change = (uint)Math.Min(70, Block / 2);
            if (player.Owner.PerfectionStatus.ShieldBreak > 0)
            {
                if (Base.Rate(player.Owner.PerfectionStatus.ShieldBreak))
                {
                    if (!target.ContainFlag(MsgUpdate.Flags.ShieldBreak))
                    {
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            target.SendUpdate(stream, MsgUpdate.Flags.ShieldBreak, 15, 0, 0, MsgUpdate.DataType.AzureShield);
                        }
                        target.AddFlag(MsgUpdate.Flags.ShieldBreak, 15, true);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            player.View.SendView(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                            {
                                Effect = MsgRefineEffect.RefineEffects.ShiledBreak,
                                Id = player.UID,
                                dwParam = player.UID
                            }), true);
                        }
                    }
                }
            }
            #endregion

            #region ShieldBreak
            if (target.ContainFlag(MsgUpdate.Flags.ShieldBreak))
            {
                if (Change > 20)
                {
                    Change -= 20;
                }
                else
                {
                    Change = 0;
                }

            }
            #endregion

            if (!player.ContainFlag(MsgUpdate.Flags.DisableBlock))
            {
                if (Base.Rate((byte)Change))
                {
                    SpellObj.Effect |= MsgAttackPacket.AttackEffect.Block;
                    SpellObj.Damage /= 2;

                }
            }

            #region MyClones
            if (target.MyClones.Count != 0)
            {
                foreach (var clone in target.MyClones.GetValues())
                    clone.RemoveThat(target.Owner);

                target.MyClones.Clear();
            }
            #endregion

            #region Infinity
            if (player.ContainFlag(MsgUpdate.Flags.Infinity) && player.Owner.Rune.IsEquipped("Infinity"))
            {
                uint InfinityDamage = (uint)(SpellObj.Damage * player.InfinityDamage / 100);
                SpellObj.Damage = InfinityDamage;
            }
            #endregion

            #region Assassin
            if (player.Owner.Rune.IsEquipped("Assassin"))
            {
                if (DBSpell == null || DBSpell.Passive)
                {
                    if (target.Action == Role.Flags.ConquerAction.Sit || target.OnAutoHunt)
                    {
                        SpellObj.Damage += 30000;
                        SpellObj.Effect |= MsgAttackPacket.AttackEffect.Assasssin;
                    }

                }

            }
            #endregion

            if (onbreak)
            {
                #region Barrier&&TacitStrike
                byte Level = 0;
                if (target.Owner.Rune.IsEquipped("Barrier", ref Level))
                {
                    int sub = (int)((2500 + Level) * 500);
                    if (Level == 5) sub = 6000;
                    else if (Level == 6) sub = 7000;
                    else if (Level == 7) sub = 9000;
                    else if (Level == 8) sub = 11000;
                    else if (Level == 9) sub = 20000;
                    Level = 0;
                    if (player.Owner.Rune.IsEquipped("TacitStrike", ref Level))
                    {
                        int add = (int)((2500 + Level) * 500);
                        if (Level == 5) add = 6000;
                        else if (Level == 6) add = 7000;
                        else if (Level == 7) add = 9000;
                        else if (Level == 8) add = 11000;
                        else if (Level == 9) add = 15000;
                        if (sub >= add)
                            sub -= add;
                        else sub = 0;
                        SpellObj.Effect |= MsgAttackPacket.AttackEffect.TacitStrike;
                    }
                    else
                    {
                        SpellObj.Effect |= MsgAttackPacket.AttackEffect.UniversalShieldBarrier;
                    }
                    if (SpellObj.Damage >= sub)
                        SpellObj.Damage -= (uint)sub;
                }
                #endregion
            }

            #region NoMercy
            byte NoMercyL = 0;
            if (SpellObj.Damage < 22000 && player.Owner.Rune.IsEquipped("NoMercy", ref NoMercyL))
            {
                uint IncD = 0;

                IncD = (uint)(4500 + NoMercyL * 500);

                if (NoMercyL == 9) IncD = 10000;
                else if (NoMercyL == 8) IncD = 9000;

                SpellObj.Damage += IncD;

                if (SpellObj.Damage > 22000)
                    SpellObj.Damage = 22000;
            }
            #endregion

            #region BloodFeast
            if (Pool.Constants.FreePkMap.Contains(player.Map))
            {
                byte BloodFeastL = 0;
                if (player.Owner.Rune.IsEquipped("BloodFeast", ref BloodFeastL))
                {
                    int points = (int)((BloodFeastL + 1) * 5000);
                    if (Calculate.Base.Rate(20))
                    {
                        player.HitPoints += points;
                        if (player.HitPoints > player.Owner.AjustMaxHitpoints())
                            player.HitPoints = (int)player.Owner.AjustMaxHitpoints();
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            player.SendString(stream, (MsgStringPacket.StringID)30, true, "hxdf_hf");
                        }
                    }
                }
            }
            #endregion

            #region Adamant
            if (DBSpell != null)
            {
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.MortalWound || DBSpell.ID == (ushort)Role.Flags.SpellID.RapidFire)
                {
                    byte AdamantL = 0;
                    byte Rate = 0;
                    if (target.Owner.Rune.IsEquipped("Adamant", ref AdamantL))
                    {
                        switch (AdamantL)
                        {
                            case 1: Rate = 50; break;
                            case 2: Rate = 55; break;
                            case 3: Rate = 60; break;
                            case 4: Rate = 65; break;
                            case 5: Rate = 70; break;
                            case 6: Rate = 75; break;
                            case 7: Rate = 80; break;
                            case 8: Rate = 90; break;
                            case 9: Rate = 100; break;
                        }
                        if (Base.Rate(Rate))
                        {
                            uint decreaseDamage = SpellObj.Damage * 30 / 100;
                            if (SpellObj.Damage > decreaseDamage)
                                SpellObj.Damage -= decreaseDamage;
                        }

                    }
                }
            }

            #endregion


            #region Iron Bone
            if (DBSpell != null)
            {
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.StarburstArrows || DBSpell.ID == (ushort)Role.Flags.SpellID.NebulousHunt
                    || DBSpell.ID == (ushort)Role.Flags.SpellID.FireCurse
                    || DBSpell.ID == (ushort)Role.Flags.SpellID.WeepStorm
                    || DBSpell.ID == (ushort)Role.Flags.SpellID.FlameofDestruction
                     || DBSpell.ID == (ushort)Role.Flags.SpellID.WaterShockwave)
                {
                    byte AdamantL = 0;
                    byte Rate = 0;
                    if (player.Owner.Rune.IsEquipped("Iron Bone", ref AdamantL) || player.Owner.Rune.IsEquipped("Invincible Resolve", ref AdamantL))
                    {
                        switch (AdamantL)
                        {
                            case 1: Rate = 20; break;
                            case 2: Rate = 25; break;
                            case 3: Rate = 30; break;
                            case 4: Rate = 35; break;
                            case 5: Rate = 40; break;
                            case 6: Rate = 45; break;
                            case 7: Rate = 50; break;
                            case 8: Rate = 55; break;
                            case 9: Rate = 60; break;
                        }
                        if (Base.Rate(Rate))
                        {
                            uint decreaseDamage = SpellObj.Damage * 40 / 100;
                            if (SpellObj.Damage > decreaseDamage)
                                SpellObj.Damage -= decreaseDamage;
                        }

                    }
                }
            }

            #endregion

            #region Reflect
            if (CheckAttack.BlockRefect.CanUseReflect(player.Owner))
            {
                if (!StackOver)
                {
                    MsgSpellAnimation.SpellObj InRedirect;
                    if (BackDmg.Calculate(player, target, DBSpell, SpellObj.Damage, out InRedirect))
                        SpellObj = InRedirect;
                }
            }
            #endregion



            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.HookMoonCaster))
                SpellObj.Damage = (uint)(SpellObj.Damage * player.Owner.Player.HookMoonPower / 100);

            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.SuanNiHeart))
            {
                if (DBSpell == null || DBSpell.Passive || player.RandomSpell > 0)
                {

                    Database.MagicType.Magic SuanniHeart = Pool.Magic[(ushort)Role.Flags.SpellID.SuanniHeart][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.SuanniHeart].Level];
                    SpellObj.Damage += (uint)(SpellObj.Damage * SuanniHeart.Damage2) / 100;
                }
                if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.HookMoonCaster))
                    SpellObj.Damage = (uint)(SpellObj.Damage * player.Owner.Player.HookMoonPower / 100);
            }

            #region Redcurse
            if (player.Owner.MyArchives.isOpen(Role.Instance.Archives.TypeID.Redcurse) && player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.PowerDash))
            {
                if (DBSpell != null)
                {
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.Pounce)
                    {
                        Database.MagicType.Magic PowerDash = Pool.Magic[(ushort)Role.Flags.SpellID.PowerDash][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.PowerDash].Level];
                        SpellObj.Damage += (uint)((SpellObj.Damage * PowerDash.DamageOnHuman) / 100);

                        bool CanAttack = player.Owner.Equipment.SteedPlus > target.Owner.Equipment.SteedPlus;
                        if (player.Owner.Equipment.SteedPlus == target.Owner.Equipment.SteedPlus)
                            CanAttack = player.Owner.Equipment.SteedPlusPorgres > target.Owner.Equipment.SteedPlusPorgres;

                        if (CanAttack)
                        {
                            target.Owner.Player.RemoveFlag(MsgUpdate.Flags.Ride);
                        }
                    }
                }
            }
            #endregion
            MsgAttackPacket.HitInMele(player, DBSpell);

        }
        public static void OnNpcs(Role.Player player, Role.SobNpc target, Database.MagicType.Magic DBSpell, out MsgSpellAnimation.SpellObj SpellObj, int IncreaseAttack = 0)
        {
            SpellObj = new MsgSpellAnimation.SpellObj(target.UID, 0, MsgAttackPacket.AttackEffect.None);

            bool update = false;

            bool Effect = false;

            bool BonePlus = false;

            bool DeathSigh = false;

            #region Calculator(Attack)
            int Damage = (int)Base.GetDamage(player.Owner.Status.MaxAttack, player.Owner.Status.MinAttack);

            Damage = (int)player.Owner.AjustAttack((uint)Damage);
            #endregion

            int IncDamagee = 0;

            #region Breakdown
            byte BreakdownL = 0;

            if (player.Owner.Rune.IsEquipped("Breakdown", ref BreakdownL) || player.Owner.Rune.IsEquipped("Battle Reaper", ref BreakdownL))
            {
                IncDamagee += (int)(110 + BreakdownL * 10);
            }
            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.WaveBreak))
            {
                IncDamagee += 100;

            }
            #endregion

            #region TenacityPillar
            if (player.Map == 1038 && Game.MsgTournaments.MsgSchedules.GuildWar.Proces != MsgTournaments.ProcesType.Dead)
            {
                if (player.GuildID == Game.MsgTournaments.MsgSchedules.GuildWar.NPC_INFO[VirusX.MsgGuildWar.NPCID.TenacityPillar].Npc.GuildID)
                {
                    IncDamagee += 200;
                }

            }
            #endregion

            #region DeathSigh
            if (player.ContainFlag(MsgUpdate.Flags.DeathSigh))
            {
                #region VioletShield
                if (player.ContainFlag(MsgUpdate.Flags.DeathSigh))
                {
                    if (DBSpell == null || DBSpell.Passive)
                    {
                        uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                        Damage = Base.MulDiv((int)Damage, (int)(DBSpellDG + player.DeathSighPassive), 100);
                        DeathSigh = true;
                    }
                    else
                    {
                        uint DBSpellDG = VirusX.Game.MsgServer.AttackHandler.Calculate.DBSpell.GetDBSpellDG(player, DBSpell, IncreaseAttack);
                        Damage = Base.MulDiv((int)Damage, (int)(DBSpellDG + player.DeathSighActive), 100);
                        DeathSigh = true;
                    }
                }
                #endregion

            }
            #endregion

            #region DBSpell(Edit)
            if (DBSpell != null && !DeathSigh)
            {
                //#region BonePluse
                //using (var rec = new ServerSockets.RecycledPacket())
                //{
                //    var stream = rec.GetStream();
                //    MsgSpell Owner_spell = null;
                //    if (DBSpell != null)
                //    {
                //        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                //                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                //        {
                //            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.BonePulse))
                //            {
                //                if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BonePulse))
                //                {
                //                    if (player.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.BonePulse, out Owner_spell))
                //                    {

                //                        Database.MagicType.Magic BonePulse = Pool.Magic[Owner_spell.ID][Owner_spell.Level];
                //                        int IncDg = BonePulse.Damage2 % 1000;
                //                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                //                            || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                //                            || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive)
                //                        {
                //                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000) , 100);
                //                            IncDg -= DBSpell.Damage2 % 1000;
                //                        }


                //                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                //                            || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                //                        {
                //                            Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 1000), 100);
                //                            IncDg -= DBSpell.DamageOnHuman % 1000;
                //                        }

                //                        Damage += (int)(Damage * (IncDg) / 100);
                                        
                //                        BonePlus = true;
                //                    }
                //                }
                //            }
                //        }
                //    }
                   
                //}
                //#endregion
                if (!BonePlus)
                {
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleAttackBloodlust
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.TripleAttackRedcurse)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage * 3 + IncDamagee, 100);
                        update = true;
                    }

                    #region Edit Damage(Index)[10]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.TripleBlasts)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage / 2 + IncDamagee, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.BladeTempest)
                    {
                        MsgGameItem RightWeapon;
                        MsgGameItem LeftWeapon;
                        if (player.Owner.Equipment.TryGetEquip(Role.Flags.ConquerItem.RightWeapon, out RightWeapon)
                            && player.Owner.Equipment.TryGetEquip(Role.Flags.ConquerItem.LeftWeapon, out LeftWeapon))
                        {
                            if (Database.ItemType.IsPirateEpicWeapon(RightWeapon.ITEM_ID) && Database.ItemType.IsPirateEpicWeapon(LeftWeapon.ITEM_ID))
                            {
                                Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
                                update = true;
                            }
                        }

                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.SwirlingStorm)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage / 4 + IncDamagee, 100);
                        update = true;
                    }
                    #endregion

                    #region DamageOnHuman(Index)[35]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderStrike
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.CleanSweep
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SongofPhoenix
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.AxeShadow
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DeadlyStrike
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SacredBlessing
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DeathSigh
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.Celestial
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.Roamer
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWind
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachment
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.BloodyScythe
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WildDashAttack
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.ArrowBlades
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.RevengeAttack
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.Megabolt
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.SkyFall
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderBlast
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.StarburstArrows
                         || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.Drukyle
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.StarVolCano
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.Spitfire
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.SandMist
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.CaptiveArrow
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.SpitfirePassive
                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.StarVolcanoPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.LavaSeaPassive
                                         || DBSpell.ID == (ushort)Role.Flags.SpellID.DragonTransformationPassive

                         || DBSpell.ID == (ushort)Role.Flags.SpellID.SuanniRoar
                               || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.DrukylePassive
                                || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.LavaSea
                           || DBSpell.ID == (ushort)Role.Flags.SpellID.DragonRising
                     )
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
                        update = true;
                    }

                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.StarFlow)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
                        update = true;
                    }



                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.ElementalArrow)
                    {
                        uint DamageInc = 0;
                        Database.MagicType.Magic ElementalArrow = Pool.Magic[(ushort)Role.Flags.SpellID.ElementalArrow][(ushort)player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.ElementalArrow].Level];
                        if (player.ContainFlag(MsgUpdate.Flags.FireArrow)
                               || player.ContainFlag(MsgUpdate.Flags.IceArrow)
                               || player.ContainFlag(MsgUpdate.Flags.PoisonArrow)
                               || player.ContainFlag(MsgUpdate.Flags.ThunderArrow)
                               || player.ContainFlag(MsgUpdate.Flags.WindArrow))
                        {
                            if (player.ContainFlag(MsgUpdate.Flags.FireArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.FireArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.IceArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.IceArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.PoisonArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.PoisonArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.ThunderArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.ThunderArrow);
                                DamageInc += 20;
                            }
                            if (player.ContainFlag(MsgUpdate.Flags.WindArrow))
                            {
                                player.RemoveFlag(MsgUpdate.Flags.WindArrow);
                                DamageInc += 20;
                            }


                        }
                        Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000 + DamageInc) + IncDamagee, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.FireArrow)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.DamageOnHuman % 100) + IncDamagee, 100);
                        update = true;


                    }

                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.CrackShot)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + DBSpell.Damage3 % 1000 + IncDamagee, 100);
                        update = true;
                    }
                    if (IncreaseAttack == 1)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonSlash)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
                            update = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.ScrenSword)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
                            update = true;
                        }
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.FastBlader)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
                            update = true;
                        }
                    }
                    if (IncreaseAttack == 0)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonSlash)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)((DBSpell.DamageOnHuman % 1000) + DBSpell.Damage3) + IncDamagee, 100);
                            update = true;
                        }
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.DevouringStrike)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 100 + IncDamagee, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.DragonCyclone)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 100 + IncDamagee, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderRampage)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)50 + IncDamagee, 100);
                        update = true;
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.SickleWindPassive)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnHuman % 1000 + IncDamagee, 100);
                        update = true;
                    }
                    #endregion

                    #region Damage2(Index)[36]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireball
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShuriken
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlash
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrison
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.BonePulse
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.Ironbone
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.IronbonePassive
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.MortalWound
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.BlisteringWave
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.WildFireballPassive
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.LightningSlashPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.PheasantBeak
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.IceAge
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.GiantGun
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.ThunderlordAttack
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.GiantGunPassive
            || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.HolySanction
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.HolySanctionPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.TwospiendSpearpassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.PheasantBeakPassive
                 || DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.IceAgePassive)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage2 % 1000 + IncDamagee, 100);
                        update = true;
                    }
                    if (IncreaseAttack == 1)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SeaBurial)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage2 % 1000 + IncDamagee, 100);
                            update = true;
                        }
                    }
                    if (DBSpell.ID == (ushort)Role.Flags.SpellIDPirate.TwospiendSpear)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)(DBSpell.Damage2 % 1000 + DBSpell.DamageOnHuman) + IncDamagee, 100);
                        update = true;
                    }
                    #endregion

                    #region Damage3(Index)[37]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.WaterShockwave
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.HookMoon
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.HellVortex)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage3 % 1000 + IncDamagee, 100);
                        update = true;

                    }

                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.TwilightDance)
                    {
                        if (IncreaseAttack == 1)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.Damage3 % 1000 + IncDamagee, 100);
                            update = true;
                        }
                        if (IncreaseAttack == 2)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.NewDamage % 1000 + IncDamagee, 100);
                            update = true;
                        }
                        if (IncreaseAttack == 3)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.NewDamage2 % 1000 + IncDamagee, 100);
                            update = true;
                        }


                    }
                    #endregion

                    #region DamageOnMonster(Index)[44]
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.NormalATK1
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.WarSuit401NormalATK1
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.NormalATK2
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.WarSuit401NormalATK2
                       || DBSpell.ID == (ushort)Role.Flags.SpellID.NormalATK3
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.CrackingShock
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.WindstormBattleaxe
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.ThunderRampage
                        || DBSpell.ID == (ushort)Role.Flags.SpellID.CannonBarrage
                         || DBSpell.ID == (ushort)Role.Flags.SpellID.Wildwind || DBSpell.ID == (ushort)Role.Flags.SpellID.HeavensWrath)
                    {
                        Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnMonster % 1000 + IncDamagee, 100);
                        update = true;
                    }
                    if (IncreaseAttack == 1)
                    {
                        if (DBSpell.ID == (ushort)Role.Flags.SpellID.SuperTwofoldBlade)
                        {
                            Damage = Base.MulDiv((int)Damage, (int)(uint)DBSpell.DamageOnMonster % 1000 + IncDamagee, 100);
                            update = true;
                        }
                    }


                    #endregion
                }
            }
            #endregion

            #region MTD
            if (!update && !BonePlus && !DeathSigh)
            {
                Damage = Base.MulDiv((int)Damage, (int)((DBSpell != null) ? (uint)DBSpell.Damage + IncDamagee : Program.ServerConfig.PhysicalDamage + IncDamagee), 100);
            }
            #endregion

            #region CriticalStrike
            if (Role.Core.Rate((int)(player.Owner.Status.CriticalStrike / 100) / 3))
            {
                if (player.Owner.AjustCriticalStrike() > 0)
                {
                    SpellObj.Effect |= MsgAttackPacket.AttackEffect.CriticalStrike;
                    Damage = Base.MulDiv((int)Damage, (int)(150), 100);
                    Effect = true;
                }
            }
            #endregion

            #region LuckyStrike(X2)
            if (Base.Rate(player.Owner.PerfectionStatus.LuckyStrike / 100))
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    player.View.SendView(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                    {
                        Effect = MsgRefineEffect.RefineEffects.LuckyStrike,
                        Id = player.UID
                    }), true);
                }
                SpellObj.Effect |= MsgAttackPacket.AttackEffect.LuckyStrike;
                Damage = Base.MulDiv((int)Damage, 200, 100);
            }
            #endregion

            #region DashRate
            if (player.Owner.Status.DashRate > 0 && !Effect)
            {
                if (Base.Rate((int)player.Owner.Status.DashRate / 100))
                {
                    SpellObj.Effect |= MsgAttackPacket.AttackEffect.DashRate;
                    Damage = Base.MulDiv((int)Damage, 200, 100);
                }

            }
            #endregion

            SpellObj.Damage = (uint)Math.Max(1, Damage);

           

            SpellObj.Damage = Calculate.Base.CalculateExtraAttack(SpellObj.Damage, player.Owner.Status.PhysicalDamageIncrease, 0);

            if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.SuanNiHeart))
            {
                if (DBSpell == null || DBSpell.Passive || player.RandomSpell > 0)
                {

                    Database.MagicType.Magic SuanniHeart = Pool.Magic[(ushort)Role.Flags.SpellID.SuanniHeart][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.SuanniHeart].Level];
                    SpellObj.Damage += (uint)(SpellObj.Damage * SuanniHeart.Damage2) / 100;
                }
                if (player.Owner.Player.ContainFlag(MsgUpdate.Flags.HookMoonCaster))
                    SpellObj.Damage = (uint)(SpellObj.Damage * player.Owner.Player.HookMoonPower / 100);
            }

            MsgAttackPacket.HitInMele(player, DBSpell);

        }
    }
}

              