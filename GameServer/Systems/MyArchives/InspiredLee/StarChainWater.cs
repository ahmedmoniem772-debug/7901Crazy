using VirusX.Database;
using VirusX.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Game.MsgFloorItem;
using VirusX.Role.Instance;
namespace VirusX.Game.MsgServer.AttackHandler
{
    public class FiveStarLianzhuWater
    {

        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.StarChainWater:
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

                            ushort CountRevive = 0;
                            foreach (var targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                Role.Player attacked = targer as Role.Player;
                                if (!attacked.Alive)
                                {

                                    bool isRecentlyDead = DateTime.Now < attacked.DeadStamp.AddMilliseconds(300);
                                    bool hasSoulShackle = attacked.ContainFlag(MsgUpdate.Flags.SoulShackle);
                                    bool hasSpecificFlag = attacked.ContainFlag((MsgUpdate.Flags)438);

                                    // استخدام الشروط المنفصلة
                                    if ( isRecentlyDead || hasSoulShackle || hasSpecificFlag)
                                    {
                                        // قم بتنفيذ الشيفرة عندما يكون أي من الشروط صحيحًا
                                        user.SendSysMesage("Hold on..");
                                        break;
                                        #region arena by WorldConquer
                                    }
                                    if (user.Player.Map == 5061 || user.Player.Map == 5062 
                                        || user.Player.Map == 5063 || user.Player.Map == 5064 
                                        || user.Player.Map == 5065 || user.Player.Map == 5066 
                                        || user.Player.Map == 5051 || user.Player.Map == 5052
                                        || user.Player.Map == 5053 || user.Player.Map == 5054 
                                        || user.Player.Map == 5055 || user.Player.Map == 5056 
                                        || user.Player.Map == 5057 || user.Player.Map == 5058 
                                       )
                                    {
                                        user.CreateBoxDialog("Mt3mlsh Nafsk Nas7 Yad #4K~WorldConquer Here..");
                                        break;


                                        #endregion
                                    }

                                    if (attacked.MyGuild != null && user.Player.MyGuild != null)
                                    {
                                        if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) <= 5 && CountRevive < 5)
                                        {
                                            if (attacked.MyGuild.Info.GuildID == user.Player.MyGuild.Info.GuildID
                                                || user.Player.MyGuild.Ally.ContainsKey(attacked.MyGuild.Info.GuildID))
                                            {
                                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(attacked.UID, 0, MsgAttackPacket.AttackEffect.None));
                                                attacked.Revive(stream);
                                                CountRevive++;
                                               
                                            }
                                        }
                                        else return;
                                    }
                                    else return;
                                   
                                }
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                                Updates.UpdateSpell.CheckUpdate(stream, user, Attack, DBSpell.Duration, DBSpells);
                              
                            }
                            break;
                        }



                }
            }
        }


    }
}