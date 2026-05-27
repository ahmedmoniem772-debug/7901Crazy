using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler
{
    public class PhysicalSpells
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.TripleAttack:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                 , 0, Attack.X, Attack.Y, ClientSpell.ID
                                 , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            uint Experience = 0;
                            Role.IMapObj target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) <= 3)
                                {


                                    for (byte x = 0; x < 3; x++)
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);

                                            Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                    if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                    {
                                        if (user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.WrathoftheEmperor) && Database.ItemType.IsMonkEpicWeapon(user.Player.RightWeaponId) && Database.ItemType.IsMonkEpicWeapon(user.Player.LeftWeaponId))
                                        {
                                            var WrathoftheEmperor = Pool.Magic[(ushort)Role.Flags.SpellID.WrathoftheEmperor][user.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.WrathoftheEmperor].Level];
                                            if (Role.Core.Rate(WrathoftheEmperor.Rate))
                                            {
                                                InteractQuery AttackPaket = new InteractQuery() { X = attacked.X, Y = attacked.Y, AtkType = (ushort)MsgAttackPacket.AttackID.Magic, OpponentUID = attacked.UID, UID = user.Player.UID, SpellID = WrathoftheEmperor.ID };
                                                MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);
                                            }

                                        }
                                        if (attacked.Alive)
                                            MsgServer.MsgAttackPacket.CreateAutoAtack(Attack, user);
                                    }
                                }

                            }
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                            {
                                var attacked = target as Role.Player;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 12)
                                {


                                    for (byte x = 0; x < 3; x++)
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);


                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                            
                                        }
                                    }
                                    if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                    {
                                        if (user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.WrathoftheEmperor) && Database.ItemType.IsMonkEpicWeapon(user.Player.RightWeaponId) && Database.ItemType.IsMonkEpicWeapon(user.Player.LeftWeaponId))
                                        {
                                            var WrathoftheEmperor = Pool.Magic[(ushort)Role.Flags.SpellID.WrathoftheEmperor][user.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.WrathoftheEmperor].Level];
                                            if (Role.Core.Rate(WrathoftheEmperor.Rate))
                                            {
                                                InteractQuery AttackPaket = new InteractQuery() { X = attacked.X, Y = attacked.Y, AtkType = (ushort)MsgAttackPacket.AttackID.Magic, OpponentUID = attacked.UID, UID = user.Player.UID, SpellID = WrathoftheEmperor.ID };
                                                MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);
                                            }

                                        }
                                        if (attacked.Alive)
                                            MsgServer.MsgAttackPacket.CreateAutoAtack(Attack, user);
                                    }
                                }

                            }
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.SobNpc))
                            {
                                var attacked = target as Role.SobNpc;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 3)
                                {


                                    for (byte x = 0; x < 3; x++)
                                    {
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);

                                            Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                    if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                    {
                                        if (user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.WrathoftheEmperor) && Database.ItemType.IsMonkEpicWeapon(user.Player.RightWeaponId) && Database.ItemType.IsMonkEpicWeapon(user.Player.LeftWeaponId))
                                        {
                                            var WrathoftheEmperor = Pool.Magic[(ushort)Role.Flags.SpellID.WrathoftheEmperor][user.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.WrathoftheEmperor].Level];
                                            if (Role.Core.Rate(WrathoftheEmperor.Rate))
                                            {
                                                InteractQuery AttackPaket = new InteractQuery() { X = attacked.X, Y = attacked.Y, AtkType = (ushort)MsgAttackPacket.AttackID.Magic, OpponentUID = attacked.UID, UID = user.Player.UID, SpellID = WrathoftheEmperor.ID };
                                                MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);
                                            }

                                        }
                                        if (attacked.Alive)
                                            MsgServer.MsgAttackPacket.CreateAutoAtack(Attack, user);
                                    }
                                }

                            }
                            Updates.IncreaseExperience.Up(stream, user, Experience);
                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, Experience, DBSpells);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);

                            break;
                        }
                    case (ushort)Role.Flags.SpellID.Windstorm:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                 , 0, Attack.X, Attack.Y, ClientSpell.ID
                                 , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            uint Experience = 0;
                            Role.IMapObj target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) <= 3)
                                {


                                    for (byte x = 0; x < 3; x++)
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
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                            {
                                var attacked = target as Role.Player;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 12)
                                {


                                    for (byte x = 0; x < 3; x++)
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
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.SobNpc))
                            {
                                var attacked = target as Role.SobNpc;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 3)
                                {


                                    for (byte x = 0; x < 3; x++)
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
                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, Experience, DBSpells);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);

                            if (ClientSpell.ID == (ushort)Role.Flags.SpellID.Windstorm && user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BloomofDeath))
                            {
                                Dictionary<ushort, Database.MagicType.Magic> Spells;
                                if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.BloomofDeath, out Spells))
                                {
                                    Database.MagicType.Magic spell;
                                    if (user.nSaveMele)
                                    {
                                        InteractQuery AttackPaket = new InteractQuery()
                                        {
                                            X = user.Player.X,
                                            Y = user.Player.Y,
                                            AtkType = (ushort)MsgAttackPacket.AttackID.Magic,
                                            OpponentUID = Attack.UID,
                                            UID = user.Player.UID,
                                            SpellID = (ushort)Role.Flags.SpellID.BloomofDeath
                                        };
                                        MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);

                                    }
                                    else
                                    {
                                        if (Spells.TryGetValue(Attack.SpellLevel, out spell))
                                        {
                                            InteractQuery AttackPaket = new InteractQuery()
                                            {
                                                X = Attack.X,
                                                Y = Attack.Y,
                                                AtkType = (ushort)MsgAttackPacket.AttackID.Magic,
                                                OpponentUID = Attack.UID,
                                                UID = user.Player.UID,
                                                SpellID = (ushort)Role.Flags.SpellID.BloomofDeath
                                            };
                                            MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);
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