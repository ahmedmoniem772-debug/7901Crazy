using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe void GetMachine(this ServerSockets.Packet stream, out MsgMachine.SlotMachineSubType Mode, out byte BetMultiplier, out uint NpcID)
        {
            Mode = (MsgMachine.SlotMachineSubType)stream.ReadUInt8();
            BetMultiplier = stream.ReadUInt8();
            stream.ReadUInt16();
            NpcID = stream.ReadUInt32();
        }

    }
    public struct MsgMachine
    {
        public enum SlotMachineSubType : byte
        {
            StartSpin = 0,
            StopSpin = 1,
            ClientFinishSpin = 2
        }

        [PacketAttribute(Game.GamePackets.MsgSlotAction)]
        public unsafe static void Handler(Client.GameClient user, ServerSockets.Packet stream)
        {
            MsgMachine.SlotMachineSubType Mode;
            byte BetMultiplier;
            uint NpcID;
            return;
            stream.GetMachine(out Mode, out BetMultiplier, out NpcID);
            if (user.InTrade || user.PokerPlayer != null || user.IsVendor)
                return;
            switch (Mode)
            {/*
              // من 10 لي 30
              * 22711 
              * من 100 لي 300
              * 22712
              * 22713
              * 22714
              */
                case SlotMachineSubType.StartSpin:
                    {


                        MsgNpc.Npc Npc;
                        if (Pool.NpcSever.TryGetValue(NpcID, out Npc))
                        {
                            if (Npc.Mesh / 10 >= 1977 && Npc.Mesh / 10 <= 1980)
                            {
                                int id = (int)Npc.Mesh / 10 - 1977;
                                uint cost = 10000;
                                bool cps = id != 0;
                                if (id == 1) cost = 3;
                                if (id == 2) cost = 10;
                                if (id == 3) cost = 100;
                                cost *= BetMultiplier;
                                if (cost < 0)
                                    return;
                                if ((cps && user.Player.ConquerPoints >= cost) || (!cps && user.Player.Money >= cost))
                                {
                                    if (user.InTrade) return;
                                    if (cps)
                                    {
                                        user.Player.ConquerPoints -= (long)cost;

                                    }
                                    else
                                    {
                                        user.Player.Money -= cost;
                                        user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                    }
                                    user.SlotMachine = new Role.Instance.SlotMachine(NpcID, (uint)cost, cps);
                                    user.SlotMachine.SpinTheWheels();
                                    user.SlotMachine.SendWheelsToClient(user, stream);
                                }

                            }
                            else
                            {
                                if (Npc.Mesh / 10 >= 2313 && Npc.Mesh / 10 <= 2316)
                                {
                                    int id = (int)Npc.Mesh / 10 - 2313;
                                    uint cost = 10000;
                                    bool cps = id != 0;
                                    if (id == 1) cost = 3;
                                    if (id == 2) cost = 10;
                                    if (id == 3) cost = 100;
                                    cost *= BetMultiplier;
                                    if (cost < 0)
                                        return;
                                    if ((cps && user.Player.ConquerPoints >= cost) || (!cps && user.Player.Money >= cost))
                                    {
                                        if (user.InTrade) return;
                                        if (cps)
                                        {
                                            user.Player.ConquerPoints -= (long)cost;

                                        }
                                        else
                                        {
                                            user.Player.Money -= cost;
                                            user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                        }
                                        user.SlotMachine = new Role.Instance.SlotMachine(NpcID, (uint)cost, cps);
                                        user.SlotMachine.SpinTheWheels();
                                        user.SlotMachine.SendWheelsToClient(user, stream);
                                    }

                                }
                            }
                        }
                            
                        else 
                        {
                            if (NpcID >= 22711 && NpcID <= 22714 || NpcID == 15460 || NpcID == 15461 || NpcID == 15462 || NpcID == 23020)
                            {
                                uint cost = 0;
                                bool cps = false;
                                switch (NpcID)
                                {
                                    case 22711:
                                        cost = 10000;
                                        break;
                                    case 22712:
                                        cost = 100000;
                                        break;
                                    case 22713:
                                        cost = 1000000;
                                        break;
                                    case 22714:
                                        cost = 5000000;
                                        break;
                                    case 15460:
                                        cost = 3;
                                        cps = true;
                                        break;
                                    case 15461:
                                        cost = 10;
                                        cps = true;
                                        break;
                                    case 15462:
                                        cost = 100;
                                        cps = true;
                                        break;
                                    case 23020:
                                        cost = 1000;
                                        cps = true;
                                        break;
                                }
                                cost *= BetMultiplier;

                                if ((cps && user.Player.ConquerPoints >= cost) || (!cps && user.Player.Money >= cost))
                                {
                                    if (user.InTrade) return;
                                    if (cps)
                                    {
                                        user.Player.ConquerPoints -= (long)cost;

                                    }
                                    else
                                    {
                                        user.Player.Money -= cost;
                                        user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                    }
                                    user.SlotMachine = new Role.Instance.SlotMachine(NpcID, (uint)cost, cps);
                                    user.SlotMachine.SpinTheWheels();
                                    user.SlotMachine.SendWheelsToClient(user, stream);
                                }
                            }
                        }
                        break;
                    }
                case SlotMachineSubType.ClientFinishSpin:
                    {
                        if (user.PokerPlayer != null)
                            return;
                        if (user.SlotMachine != null)
                        {
                            uint reward = user.SlotMachine.GetRewardAmount(user,stream);

                            if (user.SlotMachine.Cps)
                            {
                                user.Player.ConquerPoints -= (long)reward;
                                if (reward > 0)
                                {
                                    VirusX.Game.ServerLogs.SlotMachine(user, (long)reward, user.Player.ConquerPoints);
                                    foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                                    {
                                        client.Send(new MsgMessage(Program.ServerConfig.ServerName, user.Player.Name, "<" + reward + "@@>@@FF03FA01 </F>NpcPosition_PathFind</N>6297@@", 3, true, true).GetArray(stream));
                                    }
                                }
                            }
                            else
                            {
                                user.Player.Money += reward;
                                user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                if (reward > 0)
                              
                                {
                                    VirusX.Game.ServerLogs.SlotMachineMoney(user, (long)reward, (long)user.Player.Money);
                                    foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                                    {
                                        client.Send(new MsgMessage(Program.ServerConfig.ServerName, user.Player.Name, "<" + reward + "@@>@@FF03FA01 </F>NpcPosition_PathFind</N>6297@@", 3, true).GetArray(stream));
                                    }
                                }
                             
                            }

                            user.Send(stream.MachineResponseCreate(SlotMachineSubType.StopSpin, (byte)user.SlotMachine.Wheels[0], (byte)user.SlotMachine.Wheels[1], (byte)user.SlotMachine.Wheels[2], NpcID));
                            user.SlotMachine = null;
                        }
                        break;
                    }
            }
        }   
    }
}
