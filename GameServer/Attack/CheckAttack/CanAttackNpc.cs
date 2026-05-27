using VirusX.Game.MsgTournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler.CheckAttack
{
    public class CanAttackNpc
    {
        public static bool Verified(Client.GameClient client, Role.SobNpc attacked
     , Database.MagicType.Magic DBSpell)
        {
          
           
            if (attacked.HitPoints == 0)
                return false;
            if (!Game.MsgTournaments.MsgSchedules.GuildWar.Verified(client.Player, attacked))
                return false;
            if (attacked.IsStatue)
            {
                if (attacked.HitPoints == 0)
                    return false;
                if (client.Player.PkMode == Role.Flags.PKMode.PK)
                    return true;
                else
                    return false;
            }
            //if (attacked.UID == 890)
            //{
            //    if (client.Player.MyClan == null)
            //        return false;
            //    var tournament = Game.MsgTournaments.MsgSchedules.ClanWar.CurentWar;
            //    if (tournament == null)
            //        return false;

            //    if (!tournament.InWar(client))
            //        return false;
            //    if (tournament.Winner == null)
            //        return false;
            //    if (tournament.Winner.ClainID == client.Player.ClanUID)
            //        return false;

            //}
            if (attacked.UID == MsgSchedules.TopWarScore.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            {
                if (client.Player.MyGuild == null)
                    return false;

                if (MsgSchedules.TopWarScore.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;

                if (MsgSchedules.TopWarScore.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
            }
            if (attacked.UID == 22348)
            {
                if (client.Player.MyUnion == null)
                {
                    client.CreateBoxDialog("Sorry you not have Union.");
                    return false;
                }
                if (Game.MsgTournaments.MsgSchedules.UnionWar.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;
                if (client.Player.MyGuild == null)
                    return false;
                if (client.Player.MyGuild.UnionID == Game.MsgTournaments.MsgSchedules.UnionWar.Winner.UnionID)
                    return false;
                if (Game.MsgTournaments.MsgSchedules.UnionWar.Proces == MsgTournaments.ProcesType.Dead || Game.MsgTournaments.MsgSchedules.UnionWar.Proces == MsgTournaments.ProcesType.Idle)
                    return false;
            }
            if (attacked.UID == Game.MsgTournaments.MsgSchedules.SuperGuildWar.Furnitures[MsgTournaments.MsgSuperGuildWar.FurnituresType.Pole].UID)
            {
                if (client.Player.MyGuild == null)
                    return false;
                if (Game.MsgTournaments.MsgSchedules.SuperGuildWar.Furnitures[MsgTournaments.MsgSuperGuildWar.FurnituresType.Pole].HitPoints == 0)
                    return false;
                if (client.Player.GuildID == Game.MsgTournaments.MsgSchedules.SuperGuildWar.Winner.GuildID)
                    return false;
                if (Game.MsgTournaments.MsgSchedules.SuperGuildWar.Proces == MsgTournaments.ProcesType.Dead || Game.MsgTournaments.MsgSchedules.SuperGuildWar.Proces == MsgTournaments.ProcesType.Idle)
                    return false;
            }
            if (attacked.UID == MsgSchedules.EliteGuildWar.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            {
                if (client.Player.MyGuild == null)
                    return false;
                if (MsgSchedules.EliteGuildWar.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;
                if (client.Player.MyGuild.GuildName == MsgSchedules.EliteGuildWar.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
                    return false;
                if (MsgSchedules.EliteGuildWar.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
            }
            if (attacked.UID == MsgSchedules.Guild6PoleWar6.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            {
                if (client.Player.MyGuild == null)
                    return false;
                if (MsgSchedules.Guild6PoleWar6.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;
                if (client.Player.MyGuild.GuildName == MsgSchedules.Guild6PoleWar6.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
                    return false;
                if (MsgSchedules.Guild6PoleWar6.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
            }

            if (attacked.UID == MsgSchedules.ChampionsOfWarr.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            {
                if (client.Player.MyGuild == null)
                    return false;
                if (MsgSchedules.ChampionsOfWarr.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;
                if (client.Player.MyGuild.GuildName == MsgSchedules.ChampionsOfWarr.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
                    return false;
                if (MsgSchedules.ChampionsOfWarr.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
            }
            if (attacked.UID == MsgSchedules.EmperorWar.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            {
                if (client.Player.MyGuild == null)
                    return false;
                if (MsgSchedules.EmperorWar.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;
                if (client.Player.MyGuild.GuildName == MsgSchedules.EmperorWar.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
                    return false;
                if (MsgSchedules.EmperorWar.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
            }
            if (attacked.UID == MsgSchedules.UnionWar.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            {
                if (client.Player.MyGuild == null)
                    return false;
                if (MsgSchedules.UnionWar.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;
                if (client.Player.MyUnion.Name == MsgSchedules.UnionWar.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
                    return false;
                if (MsgSchedules.UnionWar.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
            }


            if (attacked.UID == MsgTournaments.MsgWarOfPlayers.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            {

                if (MsgTournaments.MsgWarOfPlayers.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
                    return false;
                if (client.Player.Name == MsgTournaments.MsgWarOfPlayers.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
                    return false;
                if (MsgTournaments.MsgWarOfPlayers.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
            }


            //if (attacked.UID == MsgSchedules.ScoreGuildWar.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            //{
            //    if (client.Player.MyGuild == null)
            //        return false;
            //    if (MsgSchedules.ScoreGuildWar.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
            //        return false;
            //    if (client.Player.MyGuild.GuildName == MsgSchedules.ScoreGuildWar.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
            //        return false;
            //    if (MsgSchedules.ScoreGuildWar.Proces != MsgTournaments.ProcesType.Alive)
            //        return false;
            //}


            //if (attacked.UID == MsgSchedules.ClanTwin.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            //{
            //    if (client.Player.MyClan == null)
            //        return false;
            //    if (MsgSchedules.ClanTwin.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
            //        return false;
            //    if (client.Player.MyClan.Name == MsgSchedules.ClanTwin.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
            //        return false;
            //    if (MsgSchedules.ClanTwin.Proces != MsgTournaments.ProcesType.Alive)
            //        return false;
            //}


            //if (attacked.UID == MsgSchedules.ClanApe.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            //{
            //    if (client.Player.MyClan == null)
            //        return false;
            //    if (MsgSchedules.ClanApe.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
            //        return false;
            //    if (client.Player.MyClan.Name == MsgSchedules.ClanApe.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
            //        return false;
            //    if (MsgSchedules.ClanApe.Proces != MsgTournaments.ProcesType.Alive)
            //        return false;
            //}
            //if (attacked.UID == MsgSchedules.ClanDesert.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            //{
            //    if (client.Player.MyClan == null)
            //        return false;
            //    if (MsgSchedules.ClanDesert.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
            //        return false;
            //    if (client.Player.MyClan.Name == MsgSchedules.ClanDesert.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
            //        return false;
            //    if (MsgSchedules.ClanDesert.Proces != MsgTournaments.ProcesType.Alive)
            //        return false;
            //}
            //if (attacked.UID == MsgSchedules.ClanPhoenix.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            //{
            //    if (client.Player.MyClan == null)
            //        return false;
            //    if (MsgSchedules.ClanPhoenix.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
            //        return false;
            //    if (client.Player.MyClan.Name == MsgSchedules.ClanPhoenix.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
            //        return false;
            //    if (MsgSchedules.ClanPhoenix.Proces != MsgTournaments.ProcesType.Alive)
            //        return false;
            //}
            //if (attacked.UID == MsgSchedules.ClanBird.Furnitures[Role.SobNpc.StaticMesh.Pole].UID)
            //{
            //    if (client.Player.MyClan == null)
            //        return false;
            //    if (MsgSchedules.ClanBird.Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints == 0)
            //        return false;
            //    if (client.Player.MyClan.Name == MsgSchedules.ClanBird.Furnitures[Role.SobNpc.StaticMesh.Pole].Name)
            //        return false;
            //    if (MsgSchedules.ClanBird.Proces != MsgTournaments.ProcesType.Alive)
            //        return false;
            //}




            MsgTournaments.MsgCaptureTheFlag.Basse Bas;
            if (MsgTournaments.MsgSchedules.CaptureTheFlag.Bases.TryGetValue(attacked.UID, out Bas))
            {
                if (MsgTournaments.MsgSchedules.CaptureTheFlag.Proces != MsgTournaments.ProcesType.Alive)
                    return false;
                if (client.Player.MyGuild == null)
                    return false;
                if (Bas.Npc.HitPoints == 0)
                    return false;
                if (Bas.CapturerID == client.Player.GuildID)
                    return false;

            }
            return true;
        }

        public static bool CanAttack(Client.GameClient client, InteractQuery Attack)
        {
            Role.IMapObj target_;
            if (client.Player.View.TryGetValue(Attack.OpponentUID, out target_, Role.MapObjectType.SobNpc))
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
                            Role.SobNpc Target = target_ as Role.SobNpc;
                            if (!CanAttackNpc.Verified(client, Target, DBSpell))
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
