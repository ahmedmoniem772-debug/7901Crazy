using VirusX.Role;
using System.Collections.Generic;

namespace VirusX.Game.MsgServer.AttackHandler
{
    public class TripleAttack
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.TripleAttackDragonhowl:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                , 0, Attack.X, Attack.Y, ClientSpell.ID
                                , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);

                            uint Experience = 0;
                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {

                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if ((attacked.Family.Settings & MsgMonster.MonsterSettings.Guard) == MsgMonster.MonsterSettings.Guard)
                                    continue;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < 18)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {


                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                           
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                var attacked = targer as Role.Player;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y, false) < DBSpell.Range || user.nSaveMele)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2,false, user.nSaveMele))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {
                                           
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);

                                           
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                            {
                                var attacked = targer as Role.SobNpc;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2, true, true))
                                    {
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                }
                            }
                            if (MsgSpell.Targets.Count == 0)
                                break;
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.TripleAttackBloodlust:
                         {
                            Role.IMapObj curTarget;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out curTarget, Role.MapObjectType.Player) || user.Player.View.TryGetValue(Attack.OpponentUID, out curTarget, Role.MapObjectType.Monster))
                            {
                                Attack.X = curTarget.X;
                                Attack.Y = curTarget.Y;
                            }

                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                , 0, Attack.X, Attack.Y, ClientSpell.ID
                                , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);



                            uint Experience = 0;
                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if ((attacked.Family.Settings & MsgMonster.MonsterSettings.Guard) == MsgMonster.MonsterSettings.Guard)
                                    continue;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < 7)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2, true, true))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                          
                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                var attacked = targer as Role.Player;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y, false) < 7 || user.nSaveMele)
                                {
                                   
                                    if (Line.InLine(attacked.X, attacked.Y, 2,false,user.nSaveMele))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {
                                            if (attacked.UID != Attack.OpponentUID)
                                                continue;
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                            {
                                var attacked = targer as Role.SobNpc;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y, true) < 7)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2,true,true))
                                    {
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                           
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                }
                            }
                           
                            Updates.IncreaseExperience.Up(stream, user, Experience);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.TripleAttackRedcurse:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                               , 0, Attack.X, Attack.Y, ClientSpell.ID
                               , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);

                            uint Experience = 0;
                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {

                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if ((attacked.Family.Settings & MsgMonster.MonsterSettings.Guard) == MsgMonster.MonsterSettings.Guard)
                                    continue;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < 7)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {


                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                           
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                var attacked = targer as Role.Player;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y, false) < 7 || user.nSaveMele)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2, false, user.nSaveMele))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {
                                            user.Player.SendUpdate(stream, user.Player.Stamina, Game.MsgServer.MsgUpdate.DataType.Stamina);
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);

                                           
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                            {
                                var attacked = targer as Role.SobNpc;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y, true) < 7)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2, true, true))
                                    {
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                }
                            }
                            if (MsgSpell.Targets.Count == 0)
                                break;
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                }
            }
        }
    }
}