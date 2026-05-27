using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler.Calculate
{
 public   class BackDmg
    {
     public unsafe static bool  Calculate(Role.Player player, Role.Player target, Database.MagicType.Magic DBSpell, uint Damage , out MsgSpellAnimation.SpellObj SpellObj, bool ranged = false)
     {
        
         if (player.Alive == false)
         {
             SpellObj = default(MsgSpellAnimation.SpellObj);
             return false;
         }
         
        
         //if (Base.Rate(55))
         {
             if (target.ContainFlag(MsgUpdate.Flags.RevengeTail))
             {
                 if (target.RevengeTailChange > 0)
                 {
                     MsgSpell ClientSpell;
                     if (target.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.RevengeTail, out ClientSpell))
                     {
                         Dictionary<ushort, Database.MagicType.Magic> DBSpells;
                         if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.RevengeTail, out DBSpells))
                         {
                             if (DBSpells.TryGetValue(ClientSpell.Level, out DBSpell))
                             {
                                 if (Damage < (uint)DBSpell.Damage)
                                 {
                                     MsgSpellAnimation MsgSpell = new MsgSpellAnimation(target.UID
                                  , 0, target.X, target.Y, ClientSpell.ID
                                  , ClientSpell.Level, ClientSpell.UseSpellSoul);

                                     MsgSpell.bomb = 1;
                                     if (CheckAttack.CanAttackPlayer.Verified(player.Owner, target, DBSpell))
                                     {
                                         MsgSpellAnimation.SpellObj AnimationObj = new MsgSpellAnimation.SpellObj()
                                         {
                                             UID = player.UID,
                                             Hit = 1
                                         };
                                         AnimationObj.Damage = (uint)DBSpell.Damage;
                                         ReceiveAttack.Player.Execute(AnimationObj, target.Owner, player);
                                         MsgSpell.Targets.Enqueue(AnimationObj);
                                     }
                                     using (var rec = new ServerSockets.RecycledPacket())
                                     {
                                         var stream = rec.GetStream();


                                         MsgSpell.SetStream(stream);
                                         MsgSpell.Send(target.Owner);


                                     }

                                     target.RevengeTailChange -= 1;

                                     if (target.RevengeTailChange == 0)
                                         target.RemoveFlag(MsgUpdate.Flags.RevengeTail);
                                 }
                             }

                         }
                     }
                 }
             }
             if (Damage > 0 && !ranged && target.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.CounterPunch))
             {
                 var mySpell = Pool.Magic[(ushort)Role.Flags.SpellID.CounterPunch][target.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.CounterPunch].Level];
                 if (Role.Core.Rate(15))
                 {
                     byte percent = 30;
                     if (mySpell.Level >= 4 && mySpell.Level <= 9) percent = 40;
                     else if (mySpell.Level >= 10 && mySpell.Level <= 14) percent = 50;
                     else if (mySpell.Level >= 15 && mySpell.Level <= 19) percent = 60;
                     else if (mySpell.Level >= 20 && mySpell.Level <= 24) percent = 65;
                     else if (mySpell.Level == 25) percent = 70;
                     else if (mySpell.Level == 26) percent = 80;
                     SpellObj = new MsgSpellAnimation.SpellObj() { Damage = Damage };

                     MsgSpellAnimation MsgSpell = new MsgSpellAnimation(target.UID
                                , player.UID, 0, 0, mySpell.ID
                                , mySpell.Level, 0);

                     if (CheckAttack.CanAttackPlayer.Verified(target.Owner, player, DBSpell))
                     {
                         MsgSpellAnimation.SpellObj AnimationObj = new MsgSpellAnimation.SpellObj()
                         {
                             UID = player.UID,
                             Hit = 1,
                             Damage = (uint)(Damage * percent / 100)
                         };
                         ReceiveAttack.Player.Execute(AnimationObj, target.Owner, player);
                         MsgSpell.Targets.Enqueue(AnimationObj);
                     }
                     using (var rec = new ServerSockets.RecycledPacket())
                     {
                         var stream = rec.GetStream();


                         MsgSpell.SetStream(stream);
                         MsgSpell.Send(target.Owner);


                     }
                     return true;
                 }
             }
             #region MagneticLight
             MsgSpell msgspell;
             if (target.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.MagneticLight, out msgspell))
             {
                 if (DBSpell == null || DBSpell.Passive && player.Stamina >= 10 || Game.MsgServer.AttackHandler.Calculate.Base.GetDistance(target.X, target.Y, player.X, player.Y) <= 1)
                 {
                     
                     Dictionary<ushort, Database.MagicType.Magic> DBSpells;
                     if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.MagneticLight, out DBSpells))
                     {
                         var LuzMagnetica = Pool.Magic[msgspell.ID][msgspell.Level];

                         int Rate = LuzMagnetica.Rate / 100;
                         //if (player.BattlePower > target.BattlePower)
                         //{
                         //    int Bp = target.BattlePower - player.BattlePower;
                         //    Rate = Math.Max(0, Bp * (int)(LuzMagnetica.Rate / 100));
                         //}
                         //else if (target.BattlePower > player.BattlePower)
                         //{
                         //    int Bp = player.BattlePower - target.BattlePower;
                         //    Rate = Math.Max(100, Bp * (int)(LuzMagnetica.Rate / 100));
                         //}
                         if (Role.Core.Rate(Rate))
                         {
                             using (var rec = new ServerSockets.RecycledPacket())
                             {
                                 if (DBSpells.TryGetValue(msgspell.Level, out DBSpell))
                                 {
                                     SpellObj = new MsgSpellAnimation.SpellObj();
                                     SpellObj.Damage = 0;
                                     SpellObj.UID = target.UID;

                                     MsgSpellAnimation MsgSpell = new MsgSpellAnimation(target.UID
                                  , 0, target.X, target.Y, msgspell.ID
                                  , msgspell.Level, msgspell.UseSpellSoul);
                                     MsgSpell.bomb = 1;
                                     if (CheckAttack.CanAttackPlayer.Verified(player.Owner, target, DBSpell))
                                     {
                                         Game.MsgServer.MsgSpellAnimation.SpellObj DmgObj = new Game.MsgServer.MsgSpellAnimation.SpellObj();
                                         DmgObj.Damage = (uint)LuzMagnetica.GDamage;
                                         if (DmgObj.Damage == 0)
                                             DmgObj.Damage = 1;
                                         InteractQuery action = new InteractQuery()
                                         {
                                             Damage = (int)DmgObj.Damage,
                                             ResponseDamage = DmgObj.Damage,
                                             AtkType = (ushort)(MsgAttackPacket.AttackID)58,
                                             X = player.X,
                                             Y = player.Y,
                                             OpponentUID = player.UID,
                                             UID = player.UID,
                                             Effect = (uint)DmgObj.Effect
                                         };
                                         player.View.SendView(rec.GetStream().InteractionCreate(action), true);
                                         player.AddFlag(MsgUpdate.Flags.MagneticLight, 2, true);
                                         ReceiveAttack.Player.Execute(DmgObj, target.Owner, player);
                                         MsgSpell.Targets.Enqueue(DmgObj);
                                     }
                                     MsgSpell.SetStream(rec.GetStream());
                                     MsgSpell.Send(target.Owner);
                                     byte level = 0;
                                     int Rates = 0;
                                     if (target.Owner.Rune.IsEquipped("Consolidation", ref level))
                                     {
                                         switch (level)
                                         {
                                             case 1: Rates = 30; break;
                                             case 2: Rates = 35; break;
                                             case 3: Rates = 40; break;
                                             case 4: Rates = 45; break;
                                             case 5: Rates = 55; break;
                                             case 6: Rates = 60; break;
                                             case 7: Rates = 65; break;
                                             case 8: Rates = 70; break;
                                             case 9: Rates = 80; break;
                                         }
                                         if (!Role.Core.Rate(Rates))
                                         {
                                             target.Stamina -= 10;
                                             target.SendUpdate(rec.GetStream(), target.Stamina, Game.MsgServer.MsgUpdate.DataType.Stamina);
                                         }
                                     }
                                   
                                     return true;
                                 }
                             }
                         }
                     }
                 }
             }
                #endregion
                bool CanUseBackfire = true;
                if (player.Owner.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.DivineAnnihilation))
                {
                    var DivineEmptiness = Pool.Magic[(ushort)Role.Flags.SpellID.DivineAnnihilation][player.Owner.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.DivineAnnihilation].Level];
                    if (Role.Core.Rate(DivineEmptiness.Rate))
                    {
                        CanUseBackfire = false;
                    }
                }
                if (target.ContainFlag(MsgUpdate.Flags.Backfire)&& CanUseBackfire)
             {
                 using (var rec = new ServerSockets.RecycledPacket())
                 {
                     var stream = rec.GetStream();
                     MsgSpell ClientSpell;
                     if (target.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.Backfire, out ClientSpell))
                     {
                         Dictionary<ushort, Database.MagicType.Magic> DBSpells;
                         if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.Backfire, out DBSpells))
                         {
                             if (DBSpells.TryGetValue(ClientSpell.Level, out DBSpell))
                             {
                                 SpellObj = new MsgSpellAnimation.SpellObj();
                                 SpellObj.Damage = Damage;
                                 if (target.HitPoints > Damage)//if the target will be alive
                                 {
                                     target.RemoveFlag(MsgUpdate.Flags.Backfire);

                                     MsgSpellAnimation.SpellObj DmgObj = new MsgSpellAnimation.SpellObj();
                                     DmgObj.Damage = Math.Min((uint)DBSpell.DamageOnMonster, (uint)Base.MulDiv((int)(target.HitPoints - Damage), (int)DBSpell.Damage, 10));

                                     //update spell
                                     if (ClientSpell.Level < DBSpells.Count - 1)
                                     {
                                         ClientSpell.Experience += (int)(DmgObj.Damage * Program.ServerConfig.ExpRateSpell);
                                         if (ClientSpell.Experience > DBSpells[ClientSpell.Level].Experience)
                                         {
                                             ClientSpell.Level++;
                                             ClientSpell.Experience = 0;
                                             player.Owner.SendSysMesage("You increased the spell level!", MsgMessage.ChatMode.TopLeftSystem, MsgMessage.MsgColor.red);
                                         }
                                         target.Send(stream.SpellCreate(ClientSpell));
                                         target.Owner.MySpells.ClientSpells[ClientSpell.ID] = ClientSpell;
                                     }


                                     InteractQuery action = new InteractQuery()
                                     {
                                         Damage = (int)DmgObj.Damage,
                                         X = target.X,
                                         Y = target.Y,
                                         OpponentUID = player.UID,
                                         UID = target.UID,
                                         Effect = (uint)DmgObj.Effect,
                                         AtkType = (ushort)MsgAttackPacket.AttackID.BackFire

                                     };

                                     target.View.SendView(stream.InteractionCreate(action), true);
                                     action = new InteractQuery()
                                     {
                                         Damage = (int)Damage,
                                         X = player.X,
                                         Y = player.Y,
                                         UID = player.UID,
                                         OpponentUID = target.UID,
                                         Effect = (uint)DmgObj.Effect,
                                         AtkType = (ushort)MsgAttackPacket.AttackID.Physical

                                     };

                                     player.View.SendView(stream.InteractionCreate(action), true);

                                     ReceiveAttack.Player.Execute(DmgObj, target.Owner, player);


                                     byte MaxStamina = (byte)(target.HeavenBlessing > 0 ? 150 : 100);
                                     target.Stamina = (ushort)Math.Min((int)target.Stamina + (uint)DBSpell.DamageOnHuman, MaxStamina);

                                     target.SendUpdate(stream, target.Stamina, Game.MsgServer.MsgUpdate.DataType.Stamina);


                                     return true;
                                 }
                             }
                         }
                     }
                 }
             }
         }
         #region HeavenWonder
         using (var rec = new ServerSockets.RecycledPacket())
         {
             var stream = rec.GetStream();
             MsgSpell ClientSpell;
             if (target.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.HeavensWonder, out ClientSpell))
             {
                 Dictionary<ushort, Database.MagicType.Magic> DBSpells;
                 if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.HeavensWonder, out DBSpells))
                 {
                     var HeavensWonder = Pool.Magic[ClientSpell.ID][ClientSpell.Level];
                     if (Role.Core.Rate(HeavensWonder.Rate/100))
                     {
                         InteractQuery InteractQuery = new InteractQuery();
                         InteractQuery.SpellID = ClientSpell.ID;
                         InteractQuery.SpellLevel = ClientSpell.Level;
                         InteractQuery.X = target.X;
                         InteractQuery.Y = target.Y;
                         InteractQuery.UID = target.UID;
                         InteractQuery.OpponentUID = player.UID;
                         MsgAttackPacket.ProcescMagic(target.Owner, stream, InteractQuery, true);
                       
                     }
                 }
             }
         }
         #endregion
         if (Base.Rate(15))
         {

            
             if (target.ActivateCounterKill)
             {
                 using (var rec = new ServerSockets.RecycledPacket())
                 {
                     var stream = rec.GetStream();
                     MsgSpell ClientSpell;
                     if (target.Owner.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.CounterKill, out ClientSpell))
                     {
                         Dictionary<ushort, Database.MagicType.Magic> DBSpells;
                         if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.CounterKill, out DBSpells))
                         {
                             if (DBSpells.TryGetValue(ClientSpell.Level, out DBSpell))
                             {
                                 SpellObj = new MsgSpellAnimation.SpellObj();
                                 SpellObj.Damage = 0;

                                 MsgSpellAnimation.SpellObj DmgObj = new MsgSpellAnimation.SpellObj();
                                 Physical.OnPlayer(target, player, DBSpell, out DmgObj, true);
                                 DmgObj.Damage /= 2;

                                 //update spell
                                 if (ClientSpell.Level < DBSpells.Count - 1)
                                 {
                                     ClientSpell.Experience += (int)(DmgObj.Damage * Program.ServerConfig.ExpRateSpell);
                                     if (ClientSpell.Experience > DBSpells[ClientSpell.Level].Experience)
                                     {
                                         ClientSpell.Level++;
                                         ClientSpell.Experience = 0;
                                     }
                                     target.Send(stream.SpellCreate(ClientSpell));
                                     target.Owner.MySpells.ClientSpells[ClientSpell.ID] = ClientSpell;
                                 }


                                 InteractQuery action = new InteractQuery()
                                 {
                                     ResponseDamage = DmgObj.Damage,
                                     X = player.X,
                                     Y = player.Y,
                                     OpponentUID = player.UID,
                                     UID = target.UID,
                                     Effect = (uint)DmgObj.Effect,
                                     AtkType = (ushort)MsgAttackPacket.AttackID.Scapegoat
                                 };

                                 target.View.SendView(stream.InteractionCreate(action), true);



                                 ReceiveAttack.Player.Execute(DmgObj, target.Owner, player);

                                 return true;
                             }
                         }
                     }
                 }
             }
             if (target.ContainReflect)
             {
                 int addition = 0;
                 int additionalDamage = 0;
                 byte itemLevel = 0;
                 if (target.Owner.Rune.IsEquipped("Retaliation", ref itemLevel))
                     addition += 20;
                 switch (itemLevel)
                 {
                     case 1: additionalDamage = 9000; break;
                     case 2: additionalDamage = 10000; break;
                     case 3: additionalDamage = 10500; break;
                     case 4: additionalDamage = 11000; break;
                     case 5: additionalDamage = 11500; break;
                     case 6: additionalDamage = 12000; break;
                     case 7: additionalDamage = 12500; break;
                     case 8: additionalDamage = 13000; break;
                     case 9: additionalDamage = 13500; break;
                     case 10: additionalDamage = 14000; break;
                     case 11: additionalDamage = 14500; break;
                     case 12: additionalDamage = 15000; break;
                     case 13: additionalDamage = 15500; break;
                     case 14: additionalDamage = 16000; break;
                     case 15: additionalDamage = 16500; break;
                     case 16: additionalDamage = 17000; break;
                     case 17: additionalDamage = 17500; break;
                     case 18: additionalDamage = 18000; break;
                     case 19: additionalDamage = 18500; break;
                     case 20: additionalDamage = 19000; break;
                     case 21: additionalDamage = 19500; break;
                     case 22: additionalDamage = 20000; break;
                     case 23: additionalDamage = 20500; break;
                     case 24: additionalDamage = 21000; break;
                     case 25: additionalDamage = 22000; break;
                     case 26: additionalDamage = 23000; break;
                     case 27: additionalDamage = 25000; break;
                 }
                 if (Role.Core.Rate(20 + addition + (30)))
                 {
                     uint damage = (uint)additionalDamage;
                     if (damage <= 0)
                         damage = 1;
                     if (damage > 10000 + additionalDamage) damage = (uint)(10000 + additionalDamage);

                     using (var rec = new ServerSockets.RecycledPacket())
                     {
                         var stream = rec.GetStream();
                         SpellObj = new MsgSpellAnimation.SpellObj();
                         SpellObj.Damage = 0;
                         SpellObj.UID = target.UID;

                         MsgSpellAnimation.SpellObj DmgObj = new MsgSpellAnimation.SpellObj();
                        
                         DmgObj.Damage = damage;
                         DmgObj.UID = player.UID;


                         InteractQuery action = new InteractQuery()
                         {
                             ResponseDamage = DmgObj.Damage,
                             Damage = (int)DmgObj.Damage,
                             AtkType = (ushort)MsgAttackPacket.AttackID.Reflect,
                             X = player.X,
                             Y = player.Y,
                             OpponentUID = player.UID,
                             UID = target.UID,
                             Effect = (uint)DmgObj.Effect
                         };

                         target.View.SendView(stream.InteractionCreate(action), true);

                         ReceiveAttack.Player.Execute(DmgObj, target.Owner, player);

                     }
                     return true;
                 }
             }
         }
         SpellObj = default(MsgSpellAnimation.SpellObj);
         return false;
     }

    }
}
