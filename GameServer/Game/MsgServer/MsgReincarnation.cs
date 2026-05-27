using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgReincarnation
    {

        public static void GetReincarnation(this ServerSockets.Packet msg, out uint ToClass, out uint ToBody)
        {
            //int timerstamp = msg.ReadInt32();

            ToClass = msg.ReadUInt32();
            ToBody = msg.ReadUInt32();
        }
        [PacketAttribute(Game.GamePackets.Reincarnation)]
        public unsafe static void Proces(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (user.Inventory.HaveSpace(5))
            {
                uint ToClass;
                uint ToBody;

                stream.GetReincarnation(out ToClass, out ToBody);


                if (user.Player.Class / 1000 != ToClass / 1000 && user.Rune.WornObjects.Count(i => Role.Instance.Rune.GetSkillForRune(i) != null) > 1)
                {

                    user.CreateBoxDialog("Please unequip red/blue/Yellow runes before changing your class.");
                    user.SendSysMesage("Please unequip red/blue/Yellow runes before changing your class.", MsgMessage.ChatMode.SystemWhisper);
                    return;
                }
                if (ToClass == 1001 || ToClass == 2001 || ToClass == 4001 || ToClass == 5001 || ToClass == 6001 || ToClass == 7001 || ToClass == 8001 || ToClass == 13002 || ToClass == 14002 || ToClass == 16001 || ToClass == 9001)
                {
                    if (user.Inventory.Contain(711083, 1))
                    {
                        if (user.Player.Level >= 110 && user.Player.Reborn == 2)
                        {
                            user.Inventory.Remove(711083, 1, stream);
                          
                            Pool.RebornInfo.Reborn(user.Player, ToClass, stream);
                        }
                        else
                            user.CreateBoxDialog("You have not been Reborn twice or you are not level 110 ++");
                    }
                }
            }
            else
                user.CreateBoxDialog("Your inventory is full, Please remove 5 spots.");
        }
    }
}