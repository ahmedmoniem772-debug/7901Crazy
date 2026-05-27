//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler.CheckAttack
{
    public class CanUseSpell
    {
        public unsafe static bool Verified(InteractQuery Attack, Client.GameClient client, Dictionary<ushort, Database.MagicType.Magic> DBSpells
            , out MsgSpell ClientSpell, out  Database.MagicType.Magic Spell)
        {
            try
            {
                foreach (var sub in Pool.WeaponSpells)
                    if (sub.Value.Contains(Attack.SpellID))
                    {
                        if (client.Player.RandomSpell != Attack.SpellID)
                        {
                            ClientSpell = default(MsgSpell);
                            Spell = default(Database.MagicType.Magic);
                            return false;
                        }
                        client.Player.RandomSpell = 0;
                        break;
                    }
                if (client.MySpells.ClientSpells.TryGetValue(Attack.SpellID, out ClientSpell))
                {
                    if (DBSpells.TryGetValue(ClientSpell.Level, out Spell))
                    {
                        if (Spell.Type == Database.MagicType.MagicSort.DirectAttack || Spell.Type == Database.MagicType.MagicSort.Attack)
                        {

                            if (!client.IsInSpellRange(Attack.OpponentUID, Spell.Range))
                            {
                                ClientSpell = default(MsgSpell);
                                Spell = default(Database.MagicType.Magic);
                                return false;
                            }
                        }
                      
                    
                        int IncreaseSpellStamina = 0;//constant
                       
                        if (client.Player.ContainFlag(MsgUpdate.Flags.ScurvyBomb))
                            IncreaseSpellStamina += 6;
                        if (client.Player.ContainFlag(MsgUpdate.Flags.DragonPierceStamina))
                        {
                            if (client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.DragonPierce))
                            {
                                Database.MagicType.Magic DragonPierces = Pool.Magic[(ushort)Role.Flags.SpellID.DragonPierce][client.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.DragonPierce].Level];
                                IncreaseSpellStamina += (int)DragonPierces.Damage3;
                            }
                        }
                        if (ClientSpell.UseSpellSoul > 0)
                            client.OnSoulSpell = ClientSpell.ID;
                        else
                            client.OnSoulSpell = 0;
                        bool Grace = false;
                        if (Spell.ID == (ushort)Role.Flags.SpellID.StarChainWater)
                        {
                            IncreaseSpellStamina += (int)Spell.Duration;
                        }
                        //if (Spell.ID == (ushort)Role.Flags.SpellID.Bless||Spell.ID == (ushort)Role.Flags.SpellID.DivineHare)
                        //{
                        //    if (client.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.HeavenBlessing))
                        //    {
                        //        IncreaseSpellStamina = 50;
                        //    }
                        //    else
                        //    {
                        //        IncreaseSpellStamina = 80;
                        //    }
                        //}
                        //if (Spell.ID == (ushort)Role.Flags.SpellID.Riding)
                        //{
                        //    IncreaseSpellStamina = 20; 
                        //}
                        if (client.Player.Map != 1039)
                        {
                            if (Spell.ID != (ushort)Role.Flags.SpellID.IronbonePassive
                            && Spell.ID != (ushort)Role.Flags.SpellID.WarSuit201NormalATK1
                            && Spell.ID != (ushort)Role.Flags.SpellID.WarSuit201NormalATK2
                            && Spell.ID != (ushort)Role.Flags.SpellID.WarSuit201NormalATK3
                            && Spell.ID != (ushort)Role.Flags.SpellID.TripleAttackDragonhowl)
                            {
                                if (Spell.UseStamina + IncreaseSpellStamina > client.Player.Stamina)
                                    return false;
                                else
                                {
                                    #region Redcurse
                                    if (client.MyArchives.isOpen(Role.Instance.Archives.TypeID.Redcurse) && client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.PowerDash))
                                    {
                                        Database.MagicType.Magic PowerDash = Pool.Magic[(ushort)Role.Flags.SpellID.PowerDash][client.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.PowerDash].Level];
                                        IncreaseSpellStamina -= (int)PowerDash.Damage2;
                                    }
                                    #endregion
                                    if ((ushort)(Spell.UseStamina + IncreaseSpellStamina) > 0)
                                    {
                                        client.Player.Stamina -= (ushort)(Spell.UseStamina + IncreaseSpellStamina);
                                        using (var rec = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = rec.GetStream();
                                            client.Player.SendUpdate(stream, client.Player.Stamina, MsgUpdate.DataType.Stamina);
                                        }
                                    }
                                }
                                int useMana = Spell.UseMana;
                                byte itemLevel = 0;
                                if (client.Rune.IsEquipped("MPMaster", ref itemLevel))
                                {
                                    double percent = itemLevel * 10;
                                    if (itemLevel == 2) percent = 15;
                                    else if (itemLevel == 3) percent = 25;
                                    else if (itemLevel == 7) percent = 75;
                                    else if (itemLevel == 8) percent = 85;
                                    useMana -= (ushort)((double)useMana * percent / 100d);
                                }


                                if (useMana > client.Player.Mana)
                                    return false;
                                else
                                {

                                    if (Database.ItemType.IsMonkEpicWeapon(client.Player.RightWeaponId) && Database.ItemType.IsMonkEpicWeapon(client.Player.LeftWeaponId) && client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.GraceofHeaven))
                                    {
                                        Grace = true;
                                    }
                                    if (useMana > 0 && !Grace)
                                    {
                                        client.Player.Mana -= (ushort)useMana;
                                    }
                                }
                            }
                         
                        }
                        else
                        {
                            return true;
                        }
                        if (client.Fake||(Spell.IsSpellWithColdTime && !Grace && Spell.ID != (ushort)Role.Flags.SpellID.HeavensWrath && !client.Player.ContainFlag(MsgUpdate.Flags.CannonBarrage) && !client.Player.ContainFlag(MsgUpdate.Flags.BlackbeardsRage) && !client.Player.ContainFlag(MsgUpdate.Flags.HeavensWrath) || Spell.ID == (ushort)Role.Flags.SpellID.CrackingSwipe))
                        {
                            DateTime now = DateTime.Now;
                           
                            if (ClientSpell.ColdTime > now)
                                return false;
                            else
                            {
                                #region Reduce
                                ushort[] Spellsss = new ushort[] { 16520, 16530, 16540, 16550, 16400, 16410, 16310, 16320, 16440, 16450, 16460, 16470, 16500, 16510, 16420, 16430, 16330, 16340, 16480, 16490, 16360, 16370, 16380, 16350, 16390 };
                                if (Spellsss.Contains(Spell.ID))
                                {
                                    #region (Ninja)
                                    Role.Instance.Ninja.Item item;
                                    if (Spell.ID == (ushort)Role.Flags.SpellID.WildFireball)
                                    {
                                        if (client.MyNinja.TryGetValueEquip(Role.Instance.Ninja.ItemType.WildSigilRapid, out item))
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime - item.DBItem.Power);
                                        else
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime);
                                    }
                                    else if (Spell.ID == (ushort)Role.Flags.SpellID.FlameofDestruction)
                                    {
                                        if (client.MyNinja.TryGetValueEquip(Role.Instance.Ninja.ItemType.FlameSigilScorch, out item))
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime - item.DBItem.Power);
                                        else
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime);
                                    }
                                    else if (Spell.ID == (ushort)Role.Flags.SpellID.DustDetachment)
                                    {
                                        if (client.MyNinja.TryGetValueEquip(Role.Instance.Ninja.ItemType.DustSigilExtinction, out item))
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime - item.DBItem.Power);
                                        else
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime);
                                    }
                                    else if (Spell.ID == (ushort)Role.Flags.SpellID.SickleWind)
                                    {
                                        if (client.MyNinja.TryGetValueEquip(Role.Instance.Ninja.ItemType.SickleSigilGallop, out item))
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime - item.DBItem.Power);
                                        else
                                            ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime);
                                    }
                                    else ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime);
                                    #endregion
                                }
                                else if (client.Player.ContainFlag(MsgUpdate.Flags.WaveBreak))
                                {
                                    if (Spell.ID == 22040 || Spell.ID == 22050 || Spell.ID == 22060)
                                    {
                                        ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime - 1000);
                                    }
                                }
                                else if (Spell.ID == (ushort)Role.Flags.SpellID.CrackingSwipe)
                                {
                                    if (client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.SuanniDominance))
                                    {
                                        Database.MagicType.Magic SuanniDominance = Pool.Magic[(ushort)Role.Flags.SpellID.SuanniDominance][client.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.SuanniDominance].Level];
                                        if (Role.Core.Rate(SuanniDominance.Rate))
                                        {
                                            using (var rec = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = rec.GetStream();
                                                var AttackPaket = new InteractQuery()
                                                {
                                                    X = client.Player.X,
                                                    Y = client.Player.Y,
                                                    AtkType = (ushort)MsgAttackPacket.AttackID.Magic,
                                                    UID = client.Player.UID,
                                                    SpellID = (ushort)Role.Flags.SpellID.DragonRoar,
                                                };
                                                MsgAttackPacket.ProcescMagic(client, stream, AttackPaket, true);
                                                Game.MsgServer.MsgSpell ClientSpells;
                                                if (client.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.SuanniDominance, out ClientSpells))
                                                {
                                                    var Magic = Pool.Magic[ClientSpells.ID][ClientSpells.Level];
                                                    if (Spell.ID == (ushort)Role.Flags.SpellID.SuanniRoar)
                                                    {
                                                        ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime - Magic.Duration * 1000);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                                else
                                {
                                    ClientSpell.IsSpellWithColdTime = true;
                                    ClientSpell.ColdTime = now.AddMilliseconds(Spell.ColdTime);
                                }
                            }

                        }
                        return true;
                    }
                }

                ClientSpell = default(MsgSpell);
                Spell = default(Database.MagicType.Magic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ClientSpell = default(MsgSpell);
                Spell = default(Database.MagicType.Magic);
                return false;
            }
            return false;
        }
    }
}
