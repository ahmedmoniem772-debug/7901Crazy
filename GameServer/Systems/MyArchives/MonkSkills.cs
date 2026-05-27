using System;
using System.Collections.Generic;
using VirusX.Game.MsgFloorItem;
namespace VirusX.Game.MsgServer.AttackHandler
{
    public class MonkSkills
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.ClapBomb:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                 , 0, Attack.X, Attack.Y, ClientSpell.ID
                                 , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            int num = 0;

                            switch (ClientSpell.Level)
                            {
                                case 0:
                                case 1:
                                    num = 3;
                                    break;
                                case 2:
                                case 3:
                                    num = 4;
                                    break;
                                default:
                                    num = 5;
                                    break;
                            }
                            uint Experience = 0;
                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) <= 8)
                                {
                                    if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                    {
                                        if (num < 1) break;
                                        num--;
                                        MsgSpellAnimation.SpellObj AnimationObj;
                                        Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);

                                        Experience += ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                        AnimationObj.Hit = 1;
                                        AnimationObj.MoveX = target.X;
                                        AnimationObj.MoveY = target.Y;
                                        Pool.ServerMaps[attacked.Map].Pushback(ref AnimationObj.MoveX, ref AnimationObj.MoveY, user.Player.Angle, 8);

                                        user.Map.View.MoveTo<Role.IMapObj>(attacked, (ushort)AnimationObj.MoveX, (ushort)AnimationObj.MoveY);
                                        attacked.X = (ushort)AnimationObj.MoveX;
                                        attacked.Y = (ushort)AnimationObj.MoveY;

                                        MsgSpell.Targets.Enqueue(AnimationObj);

                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                var attacked = targer as Role.Player;
                                if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) <= 8)
                                {
                                    if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                    {
                                        if (num < 1) break;
                                        num--;
                                        MsgSpellAnimation.SpellObj AnimationObj;
                                        Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);


                                        ReceiveAttack.Player.Execute(AnimationObj, user, attacked);


                                        AnimationObj.Hit = 1;
                                        AnimationObj.MoveX = targer.X;
                                        AnimationObj.MoveY = targer.Y;
                                        if (!CheckAttack.CheckFloors.CheckGuildWar(attacked.Owner, (ushort)AnimationObj.MoveX, (ushort)AnimationObj.MoveY))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            attacked.Owner.Map.Pushback(ref AnimationObj.MoveX, ref AnimationObj.MoveY, user.Player.Angle, 8);



                                            user.Map.View.MoveTo<Role.IMapObj>(attacked, (ushort)AnimationObj.MoveX, (ushort)AnimationObj.MoveY);
                                            attacked.X = (ushort)AnimationObj.MoveX;
                                            attacked.Y = (ushort)AnimationObj.MoveY;
                                            MsgSpell.Targets.Enqueue(AnimationObj);
                                            attacked.View.Role(false, null);
                                            user.Player.AttackStamp = DateTime.Now.AddSeconds(2);

                                        }
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                            {
                                var attacked = targer as Role.SobNpc;
                                if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) <= 8)
                                {
                                    if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                    {
                                        if (num < 1) break;
                                        num--;

                                        MsgSpellAnimation.SpellObj AnimationObj;
                                        Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);

                                        Experience += ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                        MsgSpell.Targets.Enqueue(AnimationObj);
                                    }
                                }
                            }
                            Updates.IncreaseExperience.Up(stream, user, Experience);
                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, Experience, DBSpells);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.VajraPalm:
                        {
                            if ((int)Attack.X != (int)user.Player.X && Attack.X != (ushort)0 && user.Map.ValidLocation(Attack.X, Attack.Y))
                            {

                                Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, (byte)DBSpell.Range, 0);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                                {
                                    if ((int)Calculate.Base.GetDistance(Attack.X, Attack.Y, target.X, target.Y) <= DBSpell.MaxTargets && Line.InLine(target.X, target.Y, 2))
                                    {
                                        MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);

                                            ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                                foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Player))
                                {
                                    if ((int)Calculate.Base.GetDistance(Attack.X, Attack.Y, target.X, target.Y) <= DBSpell.MaxTargets && Line.InLine(target.X, target.Y, 2))
                                    {
                                        var attacked = target as Role.Player;
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);

                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                                foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                                {
                                    if ((int)Calculate.Base.GetDistance(Attack.X, Attack.Y, target.X, target.Y) <= DBSpell.MaxTargets && Line.InLine(target.X, target.Y, 2))
                                    {
                                        var attacked = target as Role.SobNpc;
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);

                                            ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                                MsgSpell.SetStream(stream);
                                MsgSpell.SendRole(user);
                                MsgSpell.Send(user);
                                user.Shift(Attack.X, Attack.Y, stream, false);
                            }
                            break;

                        }
                    case (ushort)Role.Flags.SpellID.VajraRing://
                        {
                            
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                            {
                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, user.Player.X, user.Player.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                            }
                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.VajraRing, (ushort)Attack.X, (ushort)Attack.Y, 11, DBSpell, 3000, 500);
                            FloorItem.FloorPacket.m_X = Attack.X;
                            FloorItem.FloorPacket.m_Y = Attack.Y;
                            FloorItem.FloorPacket.OwnerX = user.Player.X;
                            FloorItem.FloorPacket.OwnerY = user.Player.Y;
                            FloorItem.FloorPacket.GuildID = user.Player.GuildID;
                            FloorItem.FloorPacket.ItemOwnerUID = user.Player.UID;
                            FloorItem.FloorPacket.Name = "STR_TRAP_ID_4879@@";
                            FloorItem.FloorPacket.m_Color = 11;
                            FloorItem.FloorPacket.Timer = 5;
                            FloorItem.FloorPacket.MapID = user.Player.Map;
                         
                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);
                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);
                            user.Player.AddFlag(MsgUpdate.Flags.VajraRing, (int)DBSpell.Second, true);
                            user.Player.SendUpdate(stream, MsgUpdate.Flags.VajraRing, (uint)DBSpell.Second, (uint)DBSpell.Damage, 3000, MsgUpdate.DataType.ArchiveSkill);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;


                        }
                    case (ushort)Role.Flags.SpellID.ClawStrike:
                        {
                            if ((int)Attack.X != (int)user.Player.X && Attack.X != (ushort)0 && user.Map.ValidLocation(Attack.X, Attack.Y))
                            {

                                Algoritms.InLineAlgorithm Line = new Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                                {
                                    if ((int)Calculate.Base.GetDistance(Attack.X, Attack.Y, target.X, target.Y) <= DBSpell.MaxTargets && Line.InLine(target.X, target.Y, 2))
                                    {
                                        MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                        if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);

                                            ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                                foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Player))
                                {
                                    if ((int)Calculate.Base.GetDistance(Attack.X, Attack.Y, target.X, target.Y) <= DBSpell.MaxTargets && Line.InLine(target.X, target.Y, 2))
                                    {
                                        var attacked = target as Role.Player;
                                        if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);

                                            ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                                foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                                {
                                    if ((int)Calculate.Base.GetDistance(Attack.X, Attack.Y, target.X, target.Y) <= DBSpell.MaxTargets && Line.InLine(target.X, target.Y, 2))
                                    {
                                        var attacked = target as Role.SobNpc;
                                        if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                        {
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);

                                            ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                            MsgSpell.Targets.Enqueue(AnimationObj);

                                        }
                                    }
                                }
                                MsgSpell.SetStream(stream);
                                MsgSpell.SendRole(user);
                                MsgSpell.Send(user);
                                user.Shift(Attack.X, Attack.Y, stream, false);
                            }
                            break;

                        }
                    case (ushort)Role.Flags.SpellID.ZenStaff:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                            {
                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, user.Player.X, user.Player.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                            }
                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.ZenStaff, (ushort)Attack.X, (ushort)Attack.Y, 23, DBSpell, 3000, 900);

                            FloorItem.FloorPacket.m_X = Attack.X;
                            FloorItem.FloorPacket.m_Y = Attack.Y;
                            FloorItem.FloorPacket.OwnerX = user.Player.X;
                            FloorItem.FloorPacket.OwnerY = user.Player.Y;
                            FloorItem.FloorPacket.GuildID = user.Player.GuildID;
                            FloorItem.FloorPacket.ItemOwnerUID = user.Player.UID;
                            FloorItem.FloorPacket.Name = "STR_TRAP_ID_@@" + (uint)FloorItem.FloorPacket.m_ID;
                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);
                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);

                            user.Player.AddFlag(MsgUpdate.Flags.ZenStaff, 7, true);
                            user.Player.SendUpdate(stream, MsgUpdate.Flags.ZenStaff, (uint)DBSpell.Duration, (uint)DBSpell.Damage, 3000, MsgUpdate.DataType.ArchiveSkill);

                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.BellShield:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                , 0, Attack.X, Attack.Y, ClientSpell.ID
                                , ClientSpell.Level, ClientSpell.UseSpellSoul);
                            foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                            {
                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) <= 8)
                                {
                                    if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                    {
                                        MsgSpellAnimation.SpellObj AnimationObj;
                                        Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);

                                        ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                        MsgSpell.Targets.Enqueue(AnimationObj);
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                            {
                                var attacked = targer as Role.Player;
                                if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) <= 8)
                                {
                                    if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                    {
                                        MsgSpellAnimation.SpellObj AnimationObj;
                                        Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);

                                        ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                        attacked.Owner.Player.FutilityDg = (uint)DBSpell.DamageOnHuman;

                                        if (attacked.AddFlag(MsgUpdate.Flags.BellShield, (int)DBSpell.Duration, true))
                                            attacked.SendUpdate(stream, MsgUpdate.Flags.BellShield, DBSpell.Duration, (uint)attacked.Owner.Player.FutilityDg, (uint)attacked.Owner.Player.FutilityDg, MsgUpdate.DataType.ArchiveSkill, true);
                                        MsgSpell.Targets.Enqueue(AnimationObj);
                                    }
                                }
                            }
                            foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                            {
                                var attacked = targer as Role.SobNpc;
                                if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) <= 8)
                                {
                                    if (CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                    {
                                        MsgSpellAnimation.SpellObj AnimationObj;
                                        Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);


                                        ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                        MsgSpell.Targets.Enqueue(AnimationObj);
                                    }
                                }
                            }
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            user.Player.AddFlag(MsgUpdate.Flags.BellShield, 6, true);
                            user.Player.SendUpdate(stream, MsgUpdate.Flags.BellShield, 6, (int)DBSpell.Damage, 120000, (int)DBSpell.Damage3, 8, MsgUpdate.DataType.ArchiveSkill, true);

                            user.Player.AddSpellFlag(MsgUpdate.Flags.BellShield, (int)DBSpell.Duration, false);
                        


                            break;
                        }
                    case (ushort)Role.Flags.SpellID.QuellingRobe:
                        {
                            
                         
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                            {
                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, user.Player.X, user.Player.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                            }
                            
                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.QuellingRobe, (ushort)Attack.X, (ushort)Attack.Y, 14, DBSpell, 3000, 900);
                            FloorItem.FloorPacket.m_X = Attack.X;
                            FloorItem.FloorPacket.m_Y = Attack.Y;
                            FloorItem.FloorPacket.OwnerX = user.Player.X;
                            FloorItem.FloorPacket.OwnerY = user.Player.Y;
                            FloorItem.FloorPacket.GuildID = user.Player.GuildID;
                            FloorItem.FloorPacket.ItemOwnerUID = user.Player.UID;
                            FloorItem.FloorPacket.Name = "STR_TRAP_ID_4861@@";
                            FloorItem.FloorPacket.m_Color = 14; 
                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);
                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            
                            break;
                        }
                    case (ushort)(Role.Flags.SpellID.VioletBowl):
                        {
                            user.Send(stream.InteractionCreate(Attack));
                            if (!user.Player.ContainFlag(MsgUpdate.Flags.VioletBowl))
                            {
                                user.Player.AddSpellFlag(MsgUpdate.Flags.VioletBowl, (int)DBSpell.Duration, true);
                                user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage / 10, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, user.Player.UID, 0, 0, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                            }
                            break;
                        }
                    case (ushort)(Role.Flags.SpellID.BoneForging):
                        {
                            user.Send(stream.InteractionCreate(Attack));
                            if (!user.Player.ContainFlag(MsgUpdate.Flags.BoneForging))
                            {
                                user.Player.AddSpellFlag(MsgUpdate.Flags.BoneForging, (int)DBSpell.Duration, true);
                                user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage / 10, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, user.Player.UID, 0, 0, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                            }
                            break;
                        }
                    case (ushort)(Role.Flags.SpellID.SilentBreath):
                        {
                            user.Send(stream.InteractionCreate(Attack));
                            if (!user.Player.ContainFlag(MsgUpdate.Flags.SilentBreath))
                            {
                                user.Player.AddSpellFlag(MsgUpdate.Flags.SilentBreath, (int)DBSpell.Duration, true);
                                user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage / 10, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, user.Player.UID, 0, 0, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                            }
                            break;
                        }
                    case (ushort)(Role.Flags.SpellID.OnefingerZen):
                        {
                            user.Send(stream.InteractionCreate(Attack));
                            if (!user.Player.ContainFlag((MsgUpdate.Flags)DBSpell.Status))
                            {
                                user.Player.AddSpellFlag((MsgUpdate.Flags)DBSpell.Status, (int)DBSpell.Duration, true);
                                user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, user.Player.UID, 0, 0, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                            }
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.FlowerTouch:
                        {
                            user.Send(stream.InteractionCreate(Attack));
                            if (!user.Player.ContainFlag(MsgUpdate.Flags.FlowerTouch))
                            {
                                user.Player.AddSpellFlag(MsgUpdate.Flags.FlowerTouch, (int)DBSpell.Duration, true);
                                user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage / 10, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, user.Player.UID, 0, 0, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                            }
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.FlowerTouchATK:
                        {

                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                            {
                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, user.Player.X, user.Player.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                            }
                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.FlowerTouch
                                , (ushort)Attack.X, (ushort)Attack.Y, 23, DBSpell, 3000, 900);

                            FloorItem.FloorPacket.m_X = Attack.X;
                            FloorItem.FloorPacket.m_Y = Attack.Y;
                            FloorItem.FloorPacket.OwnerX = user.Player.X;
                            FloorItem.FloorPacket.OwnerY = user.Player.Y;
                            FloorItem.FloorPacket.GuildID = user.Player.GuildID;
                            FloorItem.FloorPacket.ItemOwnerUID = user.Player.UID;
                            FloorItem.FloorPacket.Name = "STR_TRAP_ID_@@" + (uint)FloorItem.FloorPacket.m_ID;
                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);
                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);
                            user.Send(stream.InteractionCreate(Attack));
                            user.Player.AddSpellFlag(MsgUpdate.Flags.FlowerTouch, (int)DBSpell.Duration, true);
                            user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage / 10, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);

                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, 10000, DBSpells);

                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                   
                    
                }
            }
        }

    }
}