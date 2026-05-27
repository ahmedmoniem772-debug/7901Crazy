using ConquerOnline.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConquerOnline.Game.MsgServer.AttackHandler
{
    public class SkillPets
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            if (user.Pet != null)
            {
                user.Pet.DeadPet(stream);
            }
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.SummonGuard:
                    case (ushort)Role.Flags.SpellID.SummonBat:
                    case (ushort)Role.Flags.SpellID.SummonBatBoss:
                    case (ushort)Role.Flags.SpellID.BloodyBat:
                    case (ushort)Role.Flags.SpellID.FireEvil:
                    case (ushort)Role.Flags.SpellID.Skeleton:
                        {
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                   , 0, Attack.X, Attack.Y, ClientSpell.ID
                                   , ClientSpell.Level, ClientSpell.UseSpellSoul);

                           
                            new PetInfo(user.Player, (uint)DBSpell.Damage, stream);
                            user.Pet.RevivePet(stream);
                            Updates.UpdateSpell.CheckUpdate(stream, user, Attack, 1, DBSpells);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                }
            }
        }
    }
}