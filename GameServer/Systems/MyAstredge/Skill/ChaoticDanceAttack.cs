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
    public class ChaoticDanceAttack
    {
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            if (CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell))
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Role.Flags.SpellID.ChaoticDanceAttack:
                        {
                            var MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul, 1);
                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                            {
                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, user.Player.X, user.Player.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                            }
                            var FloorItem = new Role.FloorSpell((uint)Game.MsgFloorItem.MsgItemPacket.ChaoticDance, (ushort)Attack.X, (ushort)Attack.Y, 34, DBSpell, 8000, 1000);
                            FloorItem.FloorPacket.m_X = Attack.X;
                            FloorItem.FloorPacket.m_Y = Attack.Y;
                            FloorItem.FloorPacket.ItemOwnerUID = user.Player.UID;
                            FloorItem.FloorPacket.m_Color = 34;
                            FloorItem.FloorPacket.Name = "STR_TRAP_ID_@@" + 3534;
                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);
                            user.Player.View.SendView(stream.ItemPacketCreate(FloorItem.FloorPacket), true);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                }
            }
        }
    }
}
