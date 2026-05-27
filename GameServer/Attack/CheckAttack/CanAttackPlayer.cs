using VirusX.Role.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler.CheckAttack
{
    public class CanAttackPlayer
    {
        public static bool Verified(Client.GameClient client, Role.Player attacked
      , Database.MagicType.Magic DBSpell, bool flashingname = true, InteractQuery Attack = null)
        {
            if (Attack != null)
            {
                if (Attack.OpponentUID > 0 && Attack.SpellID != 11170 && Attack.SpellID != (ushort)Role.Flags.SpellID.WhirlwindKick && Attack.SpellID != (ushort)Role.Flags.SpellID.InfernalEcho && Attack.SpellID != (ushort)Role.Flags.SpellID.HeavensWonder)
                {
                    if (attacked.UID != Attack.OpponentUID)
                        return false;
                }
            }
            if (client.Player.UID == attacked.UID)
                return false;
            #region ImmortalForce
            if (DBSpell != null && attacked.ContainFlag(MsgUpdate.Flags.ImmortalForce))
            {
                if (!client.Player.ContainFlag(MsgUpdate.Flags.RiseofTaoism))
                {
                    byte itemLevel = 0;
                    byte chanceIgnoreImmortal = 0;
                    if (client.Rune.IsEquipped("Overwhelm", ref itemLevel) || client.Rune.IsEquipped("Shadow Flow", ref itemLevel))
                    {
                        chanceIgnoreImmortal = (byte)(10 + (itemLevel * 5));
                        if (itemLevel == 8) chanceIgnoreImmortal = 60;
                        if (itemLevel == 9) chanceIgnoreImmortal = 70;
                    }
                    if (client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.DivineAnnihilation))
                    {
                        var DivineEmptiness = Pool.Magic[(ushort)Role.Flags.SpellID.DivineAnnihilation][client.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.DivineAnnihilation].Level];
                        chanceIgnoreImmortal = (byte)(DivineEmptiness.Damage2 / 100);
                       
                    }
                    if (!Role.Core.Rate(chanceIgnoreImmortal))
                    {
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            client.Player.SendUpdate(stream, 1, MsgUpdate.DataType.ForbiddenPirate);
                        }
                        return false;
                    }
                }
            }
            #endregion
       
            #region HellVortex

            if (Database.AtributesStatus.IsNinja(attacked.Owner.Player.Class))
            {
                MsgSpell Spell;
                if (attacked.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.HellVortex))
                {
                    if (attacked.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.HellVortex, out Spell))
                    {
                        Database.MagicType.Magic HellVortex = Pool.Magic[Spell.ID][Spell.Level];
                        if (Role.Core.Rate(HellVortex.Rate / 100))
                        {
                            InteractQuery Attacks = new InteractQuery();
                            Attacks.SpellID = Spell.ID;
                            Attacks.SpellLevel = Spell.Level;
                            Attacks.OpponentUID = client.Player.UID;
                            using (ServerSockets.RecycledPacket recycledPacket = new ServerSockets.RecycledPacket())
                            {
                                ServerSockets.Packet stream = recycledPacket.GetStream();
                                MsgAttackPacket.ProcescMagic(attacked.Owner, stream, Attacks, true);
                            }
                            return false;
                        }

                    }
                }
            }
            #endregion

            
         
            #region Infinity
            if (attacked.ContainFlag(MsgUpdate.Flags.Infinity))
            {
                if (DBSpell != null)
                {
                    if ((DBSpell.ID == (ushort)Role.Flags.SpellID.Lightning
                    || DBSpell.ID == (ushort)Role.Flags.SpellID.Vulcano
                    ||
 DBSpell.ID == (ushort)Role.Flags.SpellID.FireCircle || DBSpell.ID == (ushort)Role.Flags.SpellID.FireofHell ||
 DBSpell.ID == (ushort)Role.Flags.SpellID.FlyingMoon || DBSpell.ID == (ushort)Role.Flags.SpellID.Thunder ||
 DBSpell.ID == (ushort)Role.Flags.SpellID.Fire || DBSpell.ID == (ushort)Role.Flags.SpellID.Tornado))
                        return false;
                }
            }
            #endregion
            if (client.Map.ID == 10137)
            {
                if (attacked.X < 145)
                {
                    client.SendSysMesage("You can't PK in the safe zone.", MsgMessage.ChatMode.Whisper, MsgMessage.MsgColor.red);
                    return false;
                }
            }
            if (client.Map.ID == 10250)
            {
                if (attacked.X + attacked.Y >= 2267 && attacked.X < 1100)
                {
                    client.SendSysMesage("You can't PK in the safe zone.", MsgMessage.ChatMode.Whisper, MsgMessage.MsgColor.red);
                    return false;
                }
            }

            if (attacked.Map == 700 && attacked.DynamicID == 0)
                return false;


            #region DodgeRate
            #region ComicFlash
            if (DBSpell != null && attacked.Owner.Rune.IsEquipped("Cosmic Flash"))
            {
                if (DBSpell.ID == (ushort)Role.Flags.SpellID.ScatterFire
                  || DBSpell.ID == (ushort)Role.Flags.SpellID.NebulousHunt 
                  || DBSpell.ID == (ushort)Role.Flags.SpellID.StarburstArrows)
                {
                    byte itemLevel = 0;
                    int ComicFlashRate = 0;
                    if (attacked.Owner.Rune.IsEquipped("Cosmic Flash", ref itemLevel))
                    {
                        switch (itemLevel)
                        {
                            case 1: ComicFlashRate = 10; break;
                            case 2: ComicFlashRate = 20; break;
                            case 3: ComicFlashRate = 30; break;
                            case 4: ComicFlashRate = 40; break;
                            case 5: ComicFlashRate = 45; break;
                            case 6: ComicFlashRate = 50; break;
                            case 7: ComicFlashRate = 55; break;
                            case 8: ComicFlashRate = 65; break;
                            case 9: ComicFlashRate = 80; break;
                        }
                        if (Role.Core.Rate(ComicFlashRate))
                        {
                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                            {
                                var streamm = recycledPacket.GetStream();
                                attacked.SendString(streamm, MsgStringPacket.StringID.Effect, true, new string[1] { "poisonmiss" });
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion
            if (attacked.Owner.Status.DodgeRate > 0)
            {
                byte itemLevel = 0;
                int chanceIgnoreImmortal = 0;
                if (client.Rune.IsEquipped("Slow Pace", ref itemLevel))
                {
                    switch (itemLevel)
                    {
                        case 1: chanceIgnoreImmortal = 5; break;
                        case 2: chanceIgnoreImmortal = 10; break;
                        case 3: chanceIgnoreImmortal = 15; break;
                        case 4: chanceIgnoreImmortal = 20; break;
                        case 5: chanceIgnoreImmortal = 25; break;
                        case 6: chanceIgnoreImmortal = 30; break;
                        case 7: chanceIgnoreImmortal = 35; break;
                        case 8: chanceIgnoreImmortal = 40; break;
                        case 9: chanceIgnoreImmortal = 50; break;
                    }
                }
                if (!Role.Core.Rate(chanceIgnoreImmortal + client.Status.HawkeyeLevel))
                {
                    #region SkySoarer
                    ushort RateHawkeye = 0;
                    byte SkySoarer = 0;
                    if (client.Rune.IsEquipped("Sky Soarer", ref SkySoarer)
                        && client.Player.ContainFlag(MsgUpdate.Flags.SkySoarer))
                    {
                        switch (SkySoarer)
                        {
                            case 1: RateHawkeye = 100; break;
                            case 2: RateHawkeye = 200; break;
                            case 3: RateHawkeye = 300; break;
                            case 4: RateHawkeye = 400; break;
                            case 5: RateHawkeye = 500; break;
                            case 6: RateHawkeye = 800; break;
                            case 7: RateHawkeye = 1000; break;
                            case 8: RateHawkeye = 1200; break;
                            case 9: RateHawkeye = 1500; break;
                        }
                    }
                    #endregion

                    if (Calculate.Base.GetRefinery((attacked.Owner.Status.DodgeRate / 100), (client.Status.HitRate + RateHawkeye) / 100))
                    {
                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                        {
                            var streamm = recycledPacket.GetStream();
                            attacked.SendString(streamm, MsgStringPacket.StringID.Effect, true, new string[1] { "poisonmiss" });
                            return false;
                        }
                    }
                }

            }
            if (attacked.Owner.Player.HitRateFantasy > 0 && attacked.ContainFlag(MsgUpdate.Flags.ShieldofTruth))
            {
                byte itemLevel = 0;
                int chanceIgnoreImmortal = 0;
                if (client.Rune.IsEquipped("Slow Pace", ref itemLevel))
                {
                    switch (itemLevel)
                    {
                        case 1: chanceIgnoreImmortal = 5; break;
                        case 2: chanceIgnoreImmortal = 10; break;
                        case 3: chanceIgnoreImmortal = 15; break;
                        case 4: chanceIgnoreImmortal = 20; break;
                        case 5: chanceIgnoreImmortal = 25; break;
                        case 6: chanceIgnoreImmortal = 30; break;
                        case 7: chanceIgnoreImmortal = 35; break;
                        case 8: chanceIgnoreImmortal = 40; break;
                        case 9: chanceIgnoreImmortal = 50; break;
                    }
                }
                if (!Role.Core.Rate(chanceIgnoreImmortal + client.Status.HawkeyeLevel))
                {

                    if (Calculate.Base.Rate(attacked.Owner.Player.HitRateFantasy))
                    {
                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                        {
                            var streamm = recycledPacket.GetStream();
                            attacked.SendString(streamm, MsgStringPacket.StringID.Effect, true, new string[1] { "poisonmiss" });
                            return false;
                        }
                    }
                }
            }
            #endregion
            #region DeadlyStrike
            if (client.Player.ContainFlag(MsgUpdate.Flags.DeadlyStrike))
            {
                if (Calculate.Base.Rate(20))
                {
                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                    {
                        var streamm = recycledPacket.GetStream();
                        attacked.SendString(streamm, MsgStringPacket.StringID.Effect, true, new string[1] { "poisonmiss" });
                        return false;
                    }
                }
            }
               
            #endregion
            #region Revenge
            if (attacked.ContainFlag(MsgUpdate.Flags.Revenge))
            {
                using (var recycledPacket = new ServerSockets.RecycledPacket())
                {
                    var streamm = recycledPacket.GetStream();
                    attacked.SendString(streamm, MsgStringPacket.StringID.Effect, true, new string[1] { "poisonmiss" });
                    return false;
                }
            }
            #endregion
            #region Hunter
            if (attacked.ContainFlag(MsgUpdate.Flags.Hunter))
            {
                MsgSpell Owner_spells = null;
                if (attacked.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.Hunter, out Owner_spells))
                {
                    Database.MagicType.Magic Hunter = Pool.Magic[Owner_spells.ID][Owner_spells.Level];
                    if (Role.Core.Rate(Hunter.Damage / 100))
                    {
                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                        {
                            var streamm = recycledPacket.GetStream();
                            attacked.SendString(streamm, MsgStringPacket.StringID.Effect, true, new string[1] { "poisonmiss" });
                            return false;
                        }
                    }
                }
            }
            #endregion
            
            #region NeptuneCurse
            if (DBSpell != null && attacked.ContainFlag(MsgUpdate.Flags.NeptuneCurse))
            {
                return false;
            }
            #endregion

            #region Dune

            if (Database.AtributesStatus.IsDune(attacked.Owner.Player.Class))
            {
                MsgSpell Spell;
                #region DreadSlash
                if (attacked.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.DreadSlash))
                {
                    if (attacked.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.DreadSlash, out Spell))
                    {
                        Database.MagicType.Magic DreadSlash = Pool.Magic[Spell.ID][Spell.Level];
                        if (Role.Core.Rate(DreadSlash.Rate / 100))
                        {
                            InteractQuery Attacks = new InteractQuery();
                            Attacks.SpellID = Spell.ID;
                            Attacks.SpellLevel = Spell.Level;
                            Attacks.OpponentUID = client.Player.UID;

                            attacked.AddFlag(MsgUpdate.Flags.DreadSlash, 4, true);

                            using (ServerSockets.RecycledPacket recycledPacket = new ServerSockets.RecycledPacket())
                            {
                                ServerSockets.Packet stream = recycledPacket.GetStream();
                                MsgAttackPacket.ProcescMagic(attacked.Owner, stream, Attacks, true);
                            }
                            return false;
                        }

                    }
                }
                #endregion

            }

            #endregion

            if (!attacked.Alive)
                return false;
            if (attacked.Invisible)
                return false;
            if (client.Player.PkMode == Role.Flags.PKMode.Peace)
                return false;

            if (client.Player.Map == 3935 && client.Player.Map == 10137)
            {
                foreach (var server in Database.GroupServerList.GroupServers.Values)
                {
                    if (Role.Core.GetDistance((ushort)server.X, (ushort)server.Y, attacked.X, attacked.Y) <= 8)
                        return false;
                }
            }
         
           

            if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Type == MsgTournaments.TournamentType.ExtremePk)
            {
                if (Game.MsgTournaments.MsgSchedules.CurrentTournament.InTournament(client))
                    if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Process != MsgTournaments.ProcesType.Alive)
                        return false;
            }





            if (DBSpell != null)
            {
                if (attacked.ContainFlag(MsgUpdate.Flags.Fly) && DBSpell.ID != (ushort)Role.Flags.SpellID.SickleWind)
                {
                    if (DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachment || DBSpell.ID == (ushort)Role.Flags.SpellID.DustDetachmentPassive ||
                        DBSpell.ID == (ushort)Role.Flags.SpellID.SuperTwofoldBlade)
                        return false;
                    if (DBSpell == null || !DBSpell.AttackInFly || DBSpell.ID == client.Player.RandomSpell)
                        return false;
                }
            }


            if (!MsgYuanshen.AirBlocker(client))
            {
                if (attacked.ContainFlag(MsgUpdate.Flags.Fly))
                {
                    if (DBSpell == null || !DBSpell.AttackInFly || DBSpell.ID == client.Player.RandomSpell)
                        return false;
                }

            }
            if (attacked.Owner.IsWatching())
            {
                return false;
            }
            if (client.IsWatching())
            {
                return false;
            }
            if (!attacked.AllowAttack())
                return false;
            if (client.Player.Map == MsgTournaments.MsgCaptureTheFlag.MapID)
            {
                if (!MsgTournaments.MsgSchedules.CaptureTheFlag.Attackable(attacked))
                    return false;
            }
            

            if (client.Player.Map == 4000 || client.Player.Map == 4003 || client.Player.Map == 4006 || client.Player.Map == 4008 || client.Player.Map == 4009
                || client.Player.Map == 4020)
                return false;
            
            if (client.Player.PkMode == Role.Flags.PKMode.Team)
            {
                if (client.Team != null)
                {
                    if (client.Team.Members.ContainsKey(attacked.UID))
                        return false;
                }

                if (client.Player.Associate.Contain(Role.Instance.Associate.Friends, attacked.UID))
                    return false;

                if (client.Player.MyGuild != null)
                {
                    if (client.Player.GuildID == attacked.GuildID)
                        return false;

                    if (attacked.MyGuild != null)
                    {
                        if (client.Player.MyGuild.Ally.ContainsKey(attacked.GuildID))
                            return false;
                    }
                }
                if (client.Player.MyClan != null && client.Player.ClanUID>0)
                {
                    if (client.Player.ClanUID == attacked.ClanUID)
                        return false;

                    if (attacked.MyClan != null)
                    {
                        if (client.Player.MyClan.Ally.ContainsKey(attacked.ClanUID))
                            return false;
                    }
                }
            }
            if (client.Player.PkMode == Role.Flags.PKMode.Guild)
            {
                if (client.Player.MyGuild != null)
                {
                    if (client.Player.GuildID == attacked.GuildID)
                        return false;
                    if (attacked.MyGuild != null)
                    {
                        if (client.Player.MyGuild.Ally.ContainsKey(attacked.GuildID))
                            return false;
                    }
                    
                }
            }
            if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Type == MsgTournaments.TournamentType.HeroOfGame)
            {
                if (Game.MsgTournaments.MsgSchedules.CurrentTournament.InTournament(client))
                    if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Process != MsgTournaments.ProcesType.Alive)
                        return false;
            }
            if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Type == MsgTournaments.TournamentType.BestFight)
            {
                if (Game.MsgTournaments.MsgSchedules.CurrentTournament.InTournament(client))
                    if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Process != MsgTournaments.ProcesType.Alive)
                        return false;
            }
            if (client.Player.PkMode == Role.Flags.PKMode.CS)
            {
                if (client.Player.ServerID == attacked.ServerID)
                    return false;
            }
            if (client.Player.PkMode == Role.Flags.PKMode.Union)
            {
                if (client.Player.InUnion && attacked.InUnion)
                {
                    if (client.Player.MyUnion.UID == attacked.MyUnion.UID)
                        return false;
                }
            }
            if (client.Player.Map == 1002 || client.Player.Map == 1004 || client.Player.Map == 3053)
            {
                return false;
            }
            if (client.InTeamQualifier())
            {
                if (client.Team != null && client.Player.Map == 700)
                {
                    if (client.Team.Members.ContainsKey(attacked.UID))
                        return false;
                }
            }
            if (client.Player.Map == 1002)
            {
                if (!client.Player.OnMyOwnServer) return false;
            }

            if (client.Player.ContainFlag(MsgUpdate.Flags.ChillingSnow))
            {
                MsgServer.AttackHandler.CheckAttack.ProcessColdTime.ProcessAttack.Enqueue(new MsgServer.AttackHandler.CheckAttack.ProcessColdTime.Record()
                {
                    Attacker = client,
                    Attacked = attacked.Owner
                });
            }

            if (client.Player.PkMode == Role.Flags.PKMode.Capture)
            {
                if (attacked.ContainFlag(MsgUpdate.Flags.FlashingName)
                    || attacked.ContainFlag(MsgUpdate.Flags.BlackName))
                    return true;
                else
                    return false;
            }
           
            if (client.Player.PkMode == Role.Flags.PKMode.Jiang)
            {
                if (attacked.JiangHuActive != 0)
                {
                    if (client.Player.Map == 1002 || client.Player.Map == 1000 || client.Player.Map == 1015 || client.Player.Map == 1020 || client.Player.Map == 1011)
                    {
                        if (client.Player.JiangPkFlag != Role.Instance.JiangHu.AttackFlag.None)
                        {
                            if ((client.Player.JiangPkFlag & Role.Instance.JiangHu.AttackFlag.NotHitFriends) == Role.Instance.JiangHu.AttackFlag.NotHitFriends)
                            {
                                return !client.Player.Associate.Contain(Role.Instance.Associate.Friends, attacked.UID);
                            }
                            if ((client.Player.JiangPkFlag & Role.Instance.JiangHu.AttackFlag.NoHitAlliesClan) == Role.Instance.JiangHu.AttackFlag.NoHitAlliesClan)
                            {
                                if (client.Player.MyClan != null)
                                    return !client.Player.MyClan.Ally.ContainsKey(attacked.ClanUID);
                            }
                            if ((client.Player.JiangPkFlag & Role.Instance.JiangHu.AttackFlag.NotHitAlliedGuild) == Role.Instance.JiangHu.AttackFlag.NotHitAlliedGuild)
                            {
                                if (client.Player.MyGuild != null)
                                    return !client.Player.MyGuild.Ally.ContainsKey(attacked.GuildID);
                            }
                            if ((client.Player.JiangPkFlag & Role.Instance.JiangHu.AttackFlag.NotHitClanMembers) == Role.Instance.JiangHu.AttackFlag.NotHitClanMembers)
                            {
                                return !(client.Player.ClanUID == attacked.ClanUID);
                            }
                            if ((client.Player.JiangPkFlag & Role.Instance.JiangHu.AttackFlag.NotHitGuildMembers) == Role.Instance.JiangHu.AttackFlag.NotHitGuildMembers)
                            {
                                return !(client.Player.GuildID == attacked.GuildID);
                            }
                        }
                        return true;
                    }
                }
                else
                    return false;
            }
            if (Pool.Constants.BlockAttackMap.Contains(client.Player.Map) && client.Player.DynamicID == 0)
                return false;
            else if (client.Player.DynamicID != 0)
            {
                if (Pool.Constants.BlockAttackMap.Contains(client.Player.DynamicID))
                    return false;
            }
            if (Program.ServerConfig.IsInterServer == false && flashingname)
            {
                if (client.Player.PkMode != Role.Flags.PKMode.Capture && client.Player.PkMode != Role.Flags.PKMode.Peace)
                {
                    if (!attacked.ContainFlag(MsgUpdate.Flags.FlashingName) && !attacked.ContainFlag(MsgUpdate.Flags.RedName) && !attacked.ContainFlag(MsgUpdate.Flags.BlackName))
                    {
                        if (!Pool.Constants.FreePkMap.Contains(attacked.Map) && attacked.DynamicID == 0)
                        {
                            if (!Pool.Constants.SkillsNotAvailableHere.Contains(attacked.Map) && attacked.DynamicID == 0)
                            {
                                if (!(attacked.Map == 1015 && DateTime.Now.Minute >= 28 && DateTime.Now.Minute <= 35))//Pole Bird
                                    client.Player.AddFlag(MsgUpdate.Flags.FlashingName, 30, true);
                            }
                        }

                    }
                }
            }
        
          
            return true;
        }

        public static bool CanAttack(Client.GameClient client, InteractQuery Attack)
        {
            Role.IMapObj target_;
            if (client.Player.View.TryGetValue(Attack.OpponentUID, out target_, Role.MapObjectType.Player))
            {
                Dictionary<ushort, Database.MagicType.Magic> DBSpells;
                MsgSpell ClientSpell;
                Database.MagicType.Magic DBSpell;
                if (Pool.Magic.TryGetValue(Attack.SpellID, out DBSpells))
                {
                    if (client.MySpells.ClientSpells.TryGetValue(Attack.SpellID, out ClientSpell))
                    {
                        if (DBSpells.TryGetValue(ClientSpell.Level, out DBSpell))
                        {
                            Role.Player Target = target_ as Role.Player;
                            if (!CanAttackPlayer.Verified(client, Target, DBSpell, false, Attack))
                            {
                                client.OnAutoAttack = false;
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}