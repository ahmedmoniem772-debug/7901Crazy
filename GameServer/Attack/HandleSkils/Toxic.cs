using VirusX.Game.MsgServer.AttackHandler.CheckAttack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer.AttackHandler
{
  public  class Toxic
    {
      public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
      {
          Database.MagicType.Magic DBSpell;
          MsgSpell ClientSpell;
          if (CheckAttack.CanUseSpell.Verified(Attack,user, DBSpells, out ClientSpell, out DBSpell))
          {
              switch (ClientSpell.ID)
              {
                  case (ushort)Role.Flags.SpellID.ToxicFog:
                      {
                          MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                              , 0, Attack.X, Attack.Y, ClientSpell.ID
                              , ClientSpell.Level, ClientSpell.UseSpellSoul);
                          uint Damage = 0;
                          foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                          {
                              MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                              if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) < 5)
                              {
                                  if (CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                  {
                                      if (attacked.Boss == 0)
                                      {
                                          attacked.PoisonLevel = (byte)(ClientSpell.Level + ClientSpell.UseSpellSoul);
                                          Damage = 1000;
                                          attacked.AddSpellFlag(MsgUpdate.Flags.Poisoned, (int)DBSpell.Duration, true, 1);
                                      }
                                  }
                              }
                          }
                          foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                          {
                              var attacked = targer as Role.Player;
                              if (Calculate.Base.GetDistance(Attack.X, Attack.Y, attacked.X, attacked.Y) < 5)
                              {
                                  if (CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell, true, Attack))
                                  {
                                      byte itemLevel = 0;
                                      if (attacked.Owner.Rune.IsEquipped("Sturdiness", ref itemLevel))
                                      {
                                          itemLevel = (byte)(5 + (itemLevel * 5));
                                          if (Role.Core.Rate(itemLevel)) continue;
                                      }
                                      if (attacked.Alive)
                                      {
                                          attacked.Owner.PerfectionStatus.PerfectionLevel = (ushort)user.PrestigeLevel;
                                          attacked.PoisonLevel = (byte)(ClientSpell.Level + ClientSpell.UseSpellSoul);
                                          Damage = 1000;
                                          if (attacked.AddFlag(MsgUpdate.Flags.Poisoned, (int)DBSpell.Duration, true, 2))
                                          {
                                              byte rate = 0;
                                              byte Solidness = 0;
                                              if (attacked.Owner.Rune.IsEquipped("Solidness", ref Solidness) || attacked.Owner.Rune.IsEquipped("Unbreakable", ref Solidness))
                                              {

                                                  switch (Solidness)
                                                  {
                                                      case 1: rate = 10; break;
                                                      case 2: rate = 20; break;
                                                      case 3: rate = 30; break;
                                                      case 4: rate = 40; break;
                                                      case 5: rate = 50; break;
                                                      case 6: rate = 60; break;
                                                      case 7: rate = 70; break;
                                                      case 8: rate = 85; break;
                                                      case 9: rate = 100; break;
                                                  }

                                              }
                                              if (!Role.Core.Rate(rate))
                                              {

                                                  int damage = attacked.Owner.Player.HitPoints / 2;
                                                  if (damage > 1)
                                                  {
                                                      #region ToxinEraser
                                                      bool ToxinEraserActive = false;
                                                      if (attacked.Owner.PerfectionStatus.PerfectionLevel < attacked.Owner.PrestigeLevel)
                                                          ToxinEraserActive = true;
                                                      if (ToxinEraserActive && attacked.Owner.PerfectionStatus.ToxinEraser > 0 && VirusX.Game.MsgServer.AttackHandler.Calculate.Base.Rate(attacked.Owner.PerfectionStatus.ToxinEraser))
                                                      {

                                                          attacked.Owner.Player.View.SendView(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                                                          {
                                                              Effect = MsgRefineEffect.RefineEffects.ToxinEraserLevel,
                                                              Id = attacked.Owner.Player.UID,
                                                              dwParam = attacked.Owner.Player.UID
                                                          }), true);
                                                          return;

                                                      }
                                                      #endregion
                                                  }
                                                  if (attacked.Owner.Player.HitPoints == 1)
                                                  {
                                                      damage = 0;
                                                      goto jump;
                                                  }
                                                  damage -= (int)((damage * Math.Min(attacked.Owner.Status.Detoxication, 90)) / 100);
                                                  attacked.Owner.Player.HitPoints = Math.Max(1, (int)(attacked.Owner.Player.HitPoints - damage));

                                              jump:
                                                  InteractQuery action = new InteractQuery()
                                                  {
                                                      Damage = damage,
                                                      AtkType = (ushort)MsgAttackPacket.AttackID.Physical,
                                                      X = attacked.Owner.Player.X,
                                                      Y = attacked.Owner.Player.Y,
                                                      OpponentUID = attacked.Owner.Player.UID
                                                  };
                                                  attacked.Owner.Player.View.SendView(stream.InteractionCreate(action), true);

                                              }
                                          }
                                      }

                                  }
                              }
                          }

                          Updates.UpdateSpell.CheckUpdate(stream,user,Attack, Damage, DBSpells);
                          MsgSpell.SetStream(stream);
                          MsgSpell.Send(user);

                          break;
                      }

              }
          }
      }
    }
}
