using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public static class MsgAutoHunt
    {
        [Flags]
        public enum Mode : byte
        {
            Start = 1,
            EndAuto = 2,
            EXPGained = 3,
           
        }
        public static unsafe ServerSockets.Packet AutoHuntCreate(this ServerSockets.Packet stream, ushort type, ulong Icon, ulong Exp = 0, string KillerName = null)
        {
            stream.InitWriter();

            stream.Write(type);
            stream.Write(Icon);
            stream.Write(Exp);
            stream.Write(KillerName);
            stream.Finalize(GamePackets.MsgHangUp);
            return stream;
        }
        public static unsafe void GetAutoHunt(this ServerSockets.Packet stream, out ushort Act)
        {
            Act = stream.ReadUInt16();
        }
        [PacketAttribute(GamePackets.MsgHangUp)]
        private static unsafe void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (user.Player.Map == 10133 || user.Player.Map == 10134|| user.Player.Map == 10250 || user.Player.Map == 11447 || user.Player.Map == 1700 || user.Player.Map == 10134 || user.Player.Map == 10133 || user.Player.Map == 700 || user.Inventory.Contain(750000, 1))
            {
                user.SendSysMesage("You cannot use Auto Hunt in here.");
                return;
            }
            //if (user.Player.Map == 15757 || user.Player.Map == 15758 || user.Player.Map == 15759 || user.Player.Map == 15760 || user.Player.Map == 15761 || user.Player.Map == 15762 || user.Player.Map == 15763 || user.Player.Map == 15764 || user.Player.Map == 15765 || user.Player.Map == 15766 || user.Player.Map == 15767 || user.Player.Map == 15768)
            //{
            //    user.CreateBoxDialog("Sorry You Cannot Use Auto Hunt Here in This Map.");
            //    return;
            //}
            ushort Action;
            stream.GetAutoHunt(out Action);
            switch ((Mode)Action)
            {
                case Mode.Start:
                    {
                        if (user.Player.OnAutoHunt == false)
                        {
                            if (user.Player.OnXPSkill() != MsgUpdate.Flags.Normal)
                                user.Player.RemoveFlag(user.Player.OnXPSkill());
                            user.Send(stream.AutoHuntCreate(0, 341));
                            user.Send(stream.AutoHuntCreate(1, 341));
                            user.Player.OnAutoHunt = true;
                        }
                       
                        break;
                    }
                case Mode.EndAuto:
                    {
                        
                        if (user.Player.AutoHuntExp > 0)
                        {
                            
                            user.IncreaseAutoExperience(stream, user.Player.AutoHuntExp);
                        }
                        user.Send(stream.AutoHuntCreate(3, 0));
                        user.Player.OnAutoHunt = false;
                        user.Player.AutoHuntExp = 0;
                        break;
                    }
                case Mode.EXPGained:
                    {
                        if (user.Player.AutoHuntExp > 0)
                        {
                            user.IncreaseAutoExperience(stream, user.Player.AutoHuntExp);
                        }
                        user.Send(stream.AutoHuntCreate(3, 0, user.Player.AutoHuntExp));
                        user.Player.AutoHuntExp = 0;
                        break;
                    }
                default:
                    {
                        MyConsole.WriteLine("[AutoHunt] Unknown Action: " + Action + ""); 
                        break;
                    }
            }

            
        }
    }
}