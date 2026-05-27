using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Game.MsgFloorItem;

namespace VirusX.Game.MsgServer.AttackHandler
{
    public class Line
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                if (VirusX.Pool.Constants.MapCounterHits.Contains(user.Player.Map))
                    user.Player.MisShoot += 1;

                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.HookMoon:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                , 0, Attack.X, Attack.Y, ClientSpell.ID
                                , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, 11, 0, ClientSpell.ID);


                            uint Experience = 0;
                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {

                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if ((attacked.Family.Settings & MsgMonster.MonsterSettings.Guard) == MsgMonster.MonsterSettings.Guard)
                                    continue;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range || user.nSaveMele)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2) || user.nSaveMele)
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);

                                           
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                            if (attacked.AddFlag(MsgUpdate.Flags.HookMoon, (int)DBSpell.Duration, true))
                                            {
                                                attacked.HookMoonAttackedPower = (uint)(100 - (DBSpell.Level >= 7 ? 50 : 30));
                                                attacked.SendUpdate(stream, MsgUpdate.Flags.HookMoon, DBSpell.Duration, 0, 0, MsgUpdate.DataType.ArchiveSkill);
                                            }
                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                            {
                                var attacked = targer as Role.SobNpc;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2))
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
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.RageofWar:
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

                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < 11)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {

                                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, Attack.X, Attack.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.RageofWar, (ushort)Attack.X, (ushort)Attack.Y, 14, DBSpell, 1500);
                                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);

                                            MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj() { Damage = 1, MoveX = user.Player.X, Hit = 1, MoveY = user.Player.Y, UID = FloorItem.FloorPacket.m_UID });
                                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);


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

                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < 11)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {

                                           

                                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, Attack.X, Attack.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.RageofWar, (ushort)Attack.X, (ushort)Attack.Y, 14, DBSpell, 1500);
                                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);

                                            MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj() { Damage = 1, MoveX = user.Player.X, Hit = 1, MoveY = user.Player.Y, UID = FloorItem.FloorPacket.m_UID });
                                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);


                                            if (VirusX.Pool.Constants.MapCounterHits.Contains(user.Player.Map))
                                                user.Player.HitShoot += 1;
                                           

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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < 11)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
                                    {
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, Attack.X, Attack.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.RageofWar, (ushort)Attack.X, (ushort)Attack.Y, 14, DBSpell, 1500);
                                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);

                                            MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj() { Damage = 1, MoveX = user.Player.X, Hit = 1, MoveY = user.Player.Y, UID = FloorItem.FloorPacket.m_UID });
                                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);


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
                            break;
                        }

                    //case (ushort)Role.Flags.SpellID.FastBlader:
                    //    {

                    //        MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                    //            , 0, Attack.X, Attack.Y, ClientSpell.ID
                    //            , ClientSpell.Level, ClientSpell.UseSpellSoul);
                    //        Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);

                    //        uint Experience = 0;
                    //        foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                    //        {

                    //            MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                    //            if ((attacked.Family.Settings & MsgMonster.MonsterSettings.Guard) == MsgMonster.MonsterSettings.Guard)
                    //                continue;
                    //            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                    //            {
                    //                if (Line.InLine(attacked.X, attacked.Y, 0))
                    //                {
                    //                    if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);
                    //                        Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);

                    //                    }
                    //                }
                    //                else if (Line.InLine(attacked.X, attacked.Y, 2))
                    //                {
                    //                    if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj, 1);
                    //                        Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);

                    //                    }
                    //                }
                    //            }
                    //        }
                    //        foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                    //        {
                    //            var attacked = targer as Role.Player;
                    //            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                    //            {
                    //                if (Line.InLine(attacked.X, attacked.Y, 0))
                    //                {
                    //                    if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);
                    //                        ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);
                    //                    }
                    //                }
                    //                else if (Line.InLine(attacked.X, attacked.Y, 2))
                    //                {
                    //                    if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj, false, 1);
                    //                        ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);
                    //                    }
                    //                }

                    //            }
                    //        }
                    //        foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                    //        {
                    //            var attacked = targer as Role.SobNpc;
                    //            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                    //            {
                    //                if (Line.InLine(attacked.X, attacked.Y, 0))
                    //                {
                    //                    if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);
                    //                        Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);
                    //                    }
                    //                }
                    //                else if (Line.InLine(attacked.X, attacked.Y, 2))
                    //                {
                    //                    if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj, 1);
                    //                        Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);
                    //                    }
                    //                }

                    //            }
                    //        }
                    //        Updates.IncreaseExperience.Up(stream, user, Experience);
                    //        Updates.UpdateSpell.CheckUpdate(stream, user, Attack, Experience, DBSpells);
                    //        MsgSpell.SetStream(stream);
                    //        MsgSpell.Send(user);
                    //        break;
                    //    }
                    case (ushort)Role.Flags.SpellID.FastBlader:
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                    else if (Line.InLine(attacked.X, attacked.Y, 2))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj, false, 1);
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
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
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
                            break;
                        }
                    //case (ushort)Role.Flags.SpellID.ScrenSword:
                    //    {


                    //        MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                    //            , 0, Attack.X, Attack.Y, ClientSpell.ID
                    //            , ClientSpell.Level, ClientSpell.UseSpellSoul);
                    //        Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);

                    //        uint Experience = 0;
                    //        foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                    //        {

                    //            MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                    //            if ((attacked.Family.Settings & MsgMonster.MonsterSettings.Guard) == MsgMonster.MonsterSettings.Guard)
                    //                continue;
                    //            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                    //            {
                    //                if (Line.InLine(attacked.X, attacked.Y, 0))
                    //                {
                    //                    if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);
                    //                        Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);

                    //                    }
                    //                }
                    //                else if (Line.InLine(attacked.X, attacked.Y, 2))
                    //                {
                    //                    if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj, 1);
                    //                        Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);

                    //                    }
                    //                }
                    //            }
                    //        }
                    //        foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                    //        {
                    //            var attacked = targer as Role.Player;
                    //            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                    //            {
                    //                if (Line.InLine(attacked.X, attacked.Y, 0))
                    //                {
                    //                    if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);
                    //                        ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);
                    //                    }
                    //                }
                    //                else if (Line.InLine(attacked.X, attacked.Y, 2))
                    //                {
                    //                    if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj, false, 1);
                    //                        ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        foreach (var targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                    //        {
                    //            var attacked = targer as Role.SobNpc;
                    //            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                    //            {
                    //                if (Line.InLine(attacked.X, attacked.Y, 2))
                    //                {
                    //                    if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                    //                    {
                    //                        MsgSpellAnimation.SpellObj AnimationObj;
                    //                        Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);
                    //                        Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                    //                        MsgSpell.Targets.Enqueue(AnimationObj);
                    //                    }
                    //                }

                    //            }
                    //        }
                    //        Updates.IncreaseExperience.Up(stream, user, Experience);
                    //        Updates.UpdateSpell.CheckUpdate(stream, user, Attack, Experience, DBSpells);
                    //        MsgSpell.SetStream(stream);
                    //        MsgSpell.Send(user);
                    //        break;
                    //    }
                    case (ushort)Role.Flags.SpellID.ScrenSword:
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);
                                            Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                    else if (Line.InLine(attacked.X, attacked.Y, 2))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj, 1);
                                            Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                var attacked = targer as Role.Player;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 0))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                    else if (Line.InLine(attacked.X, attacked.Y, 2))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj, false, 1);
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                }
                            }
                            foreach (var targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                            {
                                var attacked = targer as Role.SobNpc;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 2))
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
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.ThunderBlast:
                        {

                            if (!Database.AtributesStatus.IsThunderStriker(user.Player.Class)) break;
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 1))
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 1))
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, 1))
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
                            break;
                      
                        }
                   
                    case (ushort)Role.Flags.SpellID.SpeedGun:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                , 0, Attack.X, Attack.Y, ClientSpell.ID
                                , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);
                            bool hit = false;
                            byte LineRange = 1;
                            uint Experience = 0;
                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);

                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                            hit = true;
                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                var attacked = targer as Role.Player;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < 3)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {
                                            if (VirusX.Pool.Constants.MapCounterHits.Contains(user.Player.Map))
                                                user.Player.HitShoot += 1;
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            hit = true;
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
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
                                    {
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);
                                           
                                            Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                            hit = true;
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                        }
                                    }
                                }
                            }
                            Updates.IncreaseExperience.Up(stream, user, Experience);
                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, Experience, DBSpells);
                            if (hit)
                            {
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                            }
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.DragonTail:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                , 0, Attack.X, Attack.Y, ClientSpell.ID
                                , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);

                            byte LineRange = 0;
                            uint Experience = 0;

                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
                                    {
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            if (VirusX.Pool.Constants.MapCounterHits.Contains(user.Player.Map))
                                                user.Player.HitShoot += 1;
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
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
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
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
                            break;
                        }
                   
                    case (ushort)Role.Flags.SpellID.ViperFang:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                , 0, Attack.X, Attack.Y, ClientSpell.ID
                                , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0, ClientSpell.ID);

                            byte LineRange = 1;
                            uint Experience = 0;

                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, attacked.X, attacked.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
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
                                if (Role.Core.GetDistance(user.Player.X, user.Player.Y, targer.X, targer.Y) < DBSpell.Range)
                                {
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
                                    {
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                        {
                                            if (VirusX.Pool.Constants.MapCounterHits.Contains(user.Player.Map))
                                                user.Player.HitShoot += 1;
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
                                    if (Line.InLine(attacked.X, attacked.Y, LineRange))
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
                            break;
                        }
                }
            }
        }

       
    }
}
