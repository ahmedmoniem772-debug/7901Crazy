using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using System.IO;
using VirusX.Database;
using VirusX.Role;

namespace VirusX.Game.MsgServer
{
    public static class MsgSpiritInteractive
    {
        [ProtoContract]
        public class ProtoStructure
        {
            [ProtoMember(1, IsRequired = true)]
            public ActionID Type;

            [ProtoMember(2, IsRequired = true)]
            public uint[] item;
        }
        public enum ActionID : int
        {
            Upgrade = 0,
            Disassemble = 1,
            OneAnimaUpgrade = 2,
            AnimaForging = 3,
            AnimaCrest = 5,
            AnimaCrestMoney = 6,
        }
        public static unsafe ServerSockets.Packet CreateSpiritInteractive(this ServerSockets.Packet stream, ProtoStructure obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgSpiritInteractive);
            return stream;
        }
        public static unsafe void GetSpiritInteractive(this ServerSockets.Packet stream, out ProtoStructure pQuery)
        {

            pQuery = new ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<ProtoStructure>(pQuery);
        }
        public static uint LoseAnimaWinConsolationBalloon(uint Anima)
        {
            switch (Anima % 100)
            {
                case 12:
                    return 3332017;
                case 11:
                    return 3332016;
                case 10:
                    return 3332015;
                case 9:
                    return 3332014;
                case 8:
                    return 3332013;
                case 7:
                    return 3332012;
                case 6:
                    return 3332011;
                case 5:
                    return 3332010;
                case 4:
                    return 3332009;
                case 3:
                    return 3332008;
                case 2:
                    return 3332007;
                case 1:
                    return 3332006;

            }
            return 0;
        }
        public static byte AnimaUpgradeRate(uint AnimaID)
        {
            switch (AnimaID % 100)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return 50;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    return 100;
                case 18:
                case 19:
                    return 0;
            }
            return 0;
        }
        public static void GetAnimaAwakend(uint ItemID, out uint id)
        {
            id = 0;
            switch (ItemID%100)
            {
                case 1:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 117;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 118;
                        else
                            id = 119;
                        break; 
                    }
                case 2:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 123;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 124;
                        else
                            id = 125;
                        break;
                    }
                case 3:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 129;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 130;
                        else
                            id = 131;
                        break;
                    }
                case 4:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 135;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 136;
                        else
                            id = 137;
                        break;
                    }
                case 5:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 141;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 142;
                        else
                            id = 143;
                        break;
                    }
                case 6:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 147;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 148;
                        else
                            id = 149;
                        break;
                    }
                case 7:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1))
                            id = 153;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 154;
                        else
                            id = 155;
                        break;
                    }
                case 8:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 159;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 160;
                        else
                            id = 161;
                        break;
                    }
                case 9:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 165;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 166;
                        else
                            id = 167;
                        break;
                    }
                case 10:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) )
                            id = 171;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 172;
                        else
                            id = 173;
                        break;
                    }
                case 11:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1) )
                            id = 177;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 178;
                        else
                            id = 179;
                        break;
                    }
                case 12:
                    {
                        if (MyMath.Success(1) && MyMath.Success(1) && MyMath.Success(1))
                            id = 183;
                        else if (MyMath.Success(3) && MyMath.Success(3))
                            id = 184;
                        else
                            id = 185;
                        break;
                    }

            }
        }
        public static bool CanUpdateAnima(MsgGameItem Item, ActionID type)
        {
            if (type == ActionID.Upgrade)
            {
                if (Item.ITEM_ID >= 4200001 && Item.ITEM_ID <= 4200017)
                    return true;
            }
            if (type == ActionID.OneAnimaUpgrade)
            {
                if (Item.ITEM_ID >= 4200001 && Item.ITEM_ID <= 4200017)
                    return true;
            }
            if (type == ActionID.AnimaForging)
            {
                if (Item.ITEM_ID >= 4200001 && Item.ITEM_ID <= 4200017)
                    return true;
            }
            return false;
        }
        public static bool CanUpdateRateAnima(MsgGameItem Item, ActionID type)
        {
            if (type == ActionID.OneAnimaUpgrade)
            {
                if (Item.ITEM_ID >= 4200001 && Item.ITEM_ID <= 4200017)
                    return true;
            }
            return false;
        }
        public static byte RateOneAnimaUpgrade(uint ItemID)
        {
            switch (ItemID % 100)
            {
                case 1:
                    return 50;
                case 2:
                    return 50;
                case 3:
                    return 50;
                case 4:
                    return 50;
                case 5:
                    return 50;
                case 6:
                    return 50;
                case 7:
                    return 50;
                case 8:
                    return 50;
                case 9:
                    return 50;
                case 10:
                    return 50;
                case 11:
                    return 50;
                case 12:
                    return 25;
            }
            return 0;
        }
        [PacketAttribute(GamePackets.MsgSpiritInteractive)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            ProtoStructure Info;
            stream.GetSpiritInteractive(out Info);
            MsgGameItem item;
            if (!client.Player.TREPIN2 && client.Player.CheckPin())
            {
                client.Player.MessageBox("Please Active Pincode", null, null, 60);
                return;
            }
            switch ((ActionID)Info.Type)
            {
                #region Upgrade
                case ActionID.Upgrade:
                    {
                        uint index = 0;
                        if (client.Inventory.TryGetItem(Info.item[0], out item))
                        {
                            if (CanUpdateAnima(item, Info.Type) && client.Inventory.Contain(item.ITEM_ID, 2))
                            {
                                #region Hacked
                                if (!(item.ITEM_ID >= 4200001 && item.ITEM_ID <= 4200018))
                                {
                                    client.Player.MessageBox("You Hacked ?? Good Bay Ya Baby :D #52 ", null, null, 60);
                                    MyConsole.WriteLine("Player " + client.Player.Name + " Banned To Bug Anima.");
                                    Database.SystemBannedAccount.AddBan(client, 99999, "Hack Anima.");
                                    client.Socket.Disconnect();
                                    return;
                                }
                                #endregion
                                #region Success
                                var Array = SpiritTable.SpiritRates.Values.ToArray();
                                if (MyMath.Success(AnimaUpgradeRate(item.ITEM_ID)))
                                {
                                    client.Inventory.Remove(item.ITEM_ID, 2, stream);
                                    Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == item.ITEM_ID && p.rank == 1).ToArray();
                                    if (Array[0].Prizet1 == 1)
                                    {
                                        client.Inventory.AddItemWitchStack(Array.FirstOrDefault().Prizev1, 0, 1, stream);
                                    }
                                    if (Array.FirstOrDefault().rank == 2 || Array.FirstOrDefault().rank == 1)
                                    {
                                        foreach (var user in Pool.GamePoll.Values)
                                        {
                                            user.Send(new MsgMessage(Program.ServerConfig.ServerName, client.Player.Name, "<STR_ITEM_TYPE_" + item.ITEM_ID + "@@>@@<STR_ITEM_TYPE_" + Array.FirstOrDefault().Prizev1 + "@@>@@FF03FA01 </F>NpcPosition_PathFind</N>23735@@", 3).GetArray(stream));
                                        }
                                        #region log Susscful
                                        if (Array.FirstOrDefault().Type == 0)
                                        {
                                            Game.ServerLogs.AnimaUpdateAwakend(client, Array.FirstOrDefault().Prizev1);
                                        }
                                        else
                                        {

                                            Game.ServerLogs.AnimaUpdateSucced(client, Array.FirstOrDefault().Prizev1);
                                        }
                                        #endregion
                                    }
                                    index = Array.FirstOrDefault().id;
                                    Info.item = new uint[3] { Info.item[0], Info.item[1], index };
                                    client.Send(stream.CreateSpiritInteractive(Info));
                                }
                                #endregion
                                #region Failed
                                else
                                {

                                    var Chances = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == item.ITEM_ID && p.rank == 0).ToArray();
                                    if (MyMath.Success(50))
                                    {
                                        client.Inventory.Remove(item.ITEM_ID, 1, stream);
                                        client.GainExpBall(Chances.FirstOrDefault().Prizev1, true);
                                        Game.ServerLogs.AnimaUpdateFail(client, Chances.FirstOrDefault().Prizev1, 1);
                                    }
                                    else
                                    {
                                        client.Inventory.Remove(item.ITEM_ID, 2, stream);
                                        client.GainExpBall(Chances.FirstOrDefault().Prizev1, true);
                                        Game.ServerLogs.AnimaUpdateFail(client, Chances.FirstOrDefault().Prizev1, 2);
                                    }

                                    uint indexx = Chances.FirstOrDefault().id;
                                    Info.item = new uint[3] { Info.item[0], Info.item[1], indexx };
                                    client.Send(stream.CreateSpiritInteractive(Info));
                                }
                                #endregion
                            }
                        }
                        break;
                    }
                #endregion

                #region Disassemble
                case ActionID.Disassemble:
                    {
                        for (int i = 0; i < Info.item.Length; i++)
                        {
                            if (client.Inventory.TryGetItem(Info.item[i], out item))
                            {
                                if (!(item.ITEM_ID >= 4200001 && item.ITEM_ID <= 4200018))
                                {
                                    client.Player.MessageBox("You Hacked ?? Good Bay Ya Baby :D #52 ", null, null, 60);
                                    MyConsole.WriteLine("Player " + client.Player.Name + " Banned To Bug Anima.");
                                    client.Socket.Disconnect();
                                    return;
                                }
                                if (client.Inventory.HaveSpace(1))
                                {
                                    if (client.Inventory.AddItemWitchStack((uint)(item.ITEM_ID - 1), 0, (byte)(item.StackSize * 2), stream, item.Bound > 0) && client.Inventory.RemoveStackItem(item.UID, item.StackSize, stream))
                                        Info.item[i] = 1;
                                    else Info.item[i] = 0;
                                }
                                else
                                {
                                    Info.item[i] = 0;
                                    client.SendSysMesage("Please empty some space in your inventory.");
                                }
                            }
                        }
                        client.Send(stream.CreateSpiritInteractive(Info));
                        break;
                    }
                #endregion

                //#region OneAnimaUpgrade
                //case ActionID.OneAnimaUpgrade:
                //    {
                //        uint index = 0;
                //        MsgGameItem targetedEquipment;
                //        if (client.Equipment.TryGetValue(Info.item[0], out targetedEquipment))
                //        {
                //            if (client.Inventory.TryGetItem(Info.item[1], out item))
                //            {
                //                if (item.ITEM_ID >= 4200016)
                //                    return;
                //                if (CanUpdateAnima(item, Info.Type) && client.Inventory.Contain(item.ITEM_ID, 1))
                //                {
                //                    if (CanUpdateRateAnima(item, Info.Type))
                //                    {
                //                        var Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 0 && p.AnimaID == item.ITEM_ID && p.rank == 0).ToArray();
                //                        if (MyMath.Success(RateOneAnimaUpgrade(item.ITEM_ID)))
                //                        {
                //                            Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == item.ITEM_ID && p.rank > 0).ToArray();
                //                            if (MyMath.Success(80))
                //                            {
                //                                Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == item.ITEM_ID && p.rank == 1).ToArray();
                //                                targetedEquipment.AnimaItemID = Array.FirstOrDefault().Prizev1;
                //                                targetedEquipment.Mode = Role.Flags.ItemMode.Update;
                //                                targetedEquipment.Send(client, stream);
                //                                client.Equipment.QueryEquipment(client.Equipment.Alternante);
                //                                Game.ServerLogs.AnimaUpdateOne(client, Array.FirstOrDefault().Prizev1);
                //                            }
                //                            else
                //                            {
                //                                Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == item.ITEM_ID && p.rank == 2).ToArray();
                //                                targetedEquipment.AnimaItemID = Array.FirstOrDefault().Prizev1;
                //                                targetedEquipment.Mode = Role.Flags.ItemMode.Update;
                //                                targetedEquipment.Send(client, stream);
                //                                client.Equipment.QueryEquipment(client.Equipment.Alternante);
                //                                Game.ServerLogs.AnimaUpdateOne(client, Array.FirstOrDefault().Prizev1);
                //                            }
                //                            foreach (var user in Pool.GamePoll.Values)
                //                            {
                //                                user.Send(new MsgMessage(Program.ServerConfig.ServerName, client.Player.Name, "<STR_ITEM_TYPE_" + Info.item[0] + "@@>@@<STR_ITEM_TYPE_" + Array.FirstOrDefault().Prizev1 + "@@>@@FF03FA01 </F>NpcPosition_PathFind</N>23735@@", 3).GetArray(stream));
                //                            }
                //                            index = Array.FirstOrDefault().id;
                //                            Info.item = new uint[3] { Info.item[0], Info.item[1], index };
                //                            client.Send(stream.CreateSpiritInteractive(Info));
                //                            client.Inventory.Remove(item.ITEM_ID, 1, stream);
                //                        }
                //                        else
                //                        {
                //                            index = Array.FirstOrDefault().id;
                //                            Info.item = new uint[3] { Info.item[0], Info.item[1], index };
                //                            client.Send(stream.CreateSpiritInteractive(Info));
                //                            client.Inventory.Remove(item.ITEM_ID, 1, stream);
                //                            Game.ServerLogs.AnimaUpdateOneFailed(client, Array.FirstOrDefault().Prizev1);
                //                        }
                //                    }
                //                    else
                //                    {
                //                        var Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == item.ITEM_ID && p.rank == 1).ToArray();
                //                        targetedEquipment.AnimaItemID = Array.FirstOrDefault().Prizev1;
                //                        targetedEquipment.Mode = Role.Flags.ItemMode.Update;
                //                        targetedEquipment.Send(client, stream);
                //                        client.Equipment.QueryEquipment(client.Equipment.Alternante);
                //                        Game.ServerLogs.AnimaUpdateOne(client, Array.FirstOrDefault().Prizev1);
                //                        foreach (var user in Pool.GamePoll.Values)
                //                        {
                //                            user.Send(new MsgMessage(Program.ServerConfig.ServerName, client.Player.Name, "<STR_ITEM_TYPE_" + Info.item[0] + "@@>@@<STR_ITEM_TYPE_" + Array.FirstOrDefault().Prizev1 + "@@>@@FF03FA01 </F>NpcPosition_PathFind</N>23735@@", 3).GetArray(stream));
                //                        }
                //                        index = Array.FirstOrDefault().id;
                //                        Info.item = new uint[3] { Info.item[0], Info.item[1], index };
                //                        client.Send(stream.CreateSpiritInteractive(Info));
                //                        client.Inventory.Remove(item.ITEM_ID, 1, stream);
                //                    }

                //                }
                //            }
                //        }
                //        break;
                //    }
                //#endregion

                #region AnimaForging
                case ActionID.AnimaForging:
                    {
                        MsgGameItem Item;
                        uint index = 0;
                        if (client.Inventory.SearchItemByID(Info.item[0], out Item) && client.Inventory.Contain(Info.item[0], 1))
                        {
                            if (CanUpdateAnima(Item, Info.Type))
                            {
                                #region Success
                                var Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == Info.item[0] && p.rank == 0).ToArray();
                                if (Info.item[0] >= 4200003 && Info.item[0] <= 4200005 && MyMath.Success(35)
                                  || Info.item[0] == 4200006 && MyMath.Success(35)
                                  || Info.item[0] == 4200007 && MyMath.Success(35)
                                  || Info.item[0] == 4200008 && MyMath.Success(35)
                                  || Info.item[0] == 4200009 && MyMath.Success(35)
                                  || Info.item[0] == 4200010 && MyMath.Success(35)
                                  || Info.item[0] == 4200011 && MyMath.Success(35)
                                  || Info.item[0] == 4200012 && MyMath.Success(35)
                                  || Info.item[0] == 4200013 && MyMath.Success(35)
                                  || Info.item[0] == 4200014 && MyMath.Success(35)
                                  || Info.item[0] == 4200015 && MyMath.Success(35)
                                  || Info.item[0] == 4200016 && MyMath.Success(35)
                                  || Info.item[0] == 4200017 && MyMath.Success(35))
                                {
                                    Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == Info.item[0] && p.rank > 0).ToArray();
                                    byte rand = (byte)Pool.GetRandom.Next(1, 3);
                                    switch (rand)
                                    {
                                        case 2:
                                            {
                                                Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == Info.item[0] && p.rank == 1).ToArray();
                                                client.Inventory.AddItemWitchStack(Array.FirstOrDefault().Prizev1, 0, 1, stream);
                                                Game.ServerLogs.AnimaUpdateForging(client, Array.FirstOrDefault().Prizev1);
                                                break;
                                            }

                                        case 1:
                                            {
                                                Array = SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == Info.item[0] && p.rank == 2).ToArray();
                                                client.Inventory.AddItemWitchStack(Array.FirstOrDefault().Prizev1, 0, 1, stream);
                                                Game.ServerLogs.AnimaUpdateForging(client, Array.FirstOrDefault().Prizev1);
                                                break;
                                            }
                                    }
                                    foreach (var user in Pool.GamePoll.Values)
                                    {
                                        user.Send(new MsgMessage(Program.ServerConfig.ServerName, client.Player.Name, "<STR_ITEM_TYPE_" + Info.item[0] + "@@>@@<STR_ITEM_TYPE_" + Array.FirstOrDefault().Prizev1 + "@@>@@FF03FA01 </F>NpcPosition_PathFind</N>23735@@", 3).GetArray(stream));
                                    }
                                    index = Array.FirstOrDefault().id;
                                    Info.item = new uint[2] { Info.item[0], index };
                                    client.Send(stream.CreateSpiritInteractive(Info));
                                    client.Inventory.Remove(Info.item[0], 1, stream);
                                }
                                #endregion
                                #region Failed
                                else
                                {

                                    client.GainExpBall(Array.FirstOrDefault().Prizev1, true);
                                    Info.item = new uint[2] { Info.item[0], 0 };
                                    client.Send(stream.CreateSpiritInteractive(Info));
                                    client.Inventory.Remove(Info.item[0], 1, stream);
                                }
                                #endregion
                            }
                        }
                        break;
                    }
                #endregion
                case ActionID.AnimaCrest:
                    {
                        if (client.Inventory.Contain(Info.item[0], Info.item[1]))
                        {
                            client.Inventory.RemoveStackItem(Info.item[0], (ushort)Info.item[1], stream);
                            if (MyMath.Success(50))
                            {
                                var WinLose = new MsgSpiritInteractive.ProtoStructure();
                                WinLose.Type = ActionID.AnimaCrest;
                                WinLose.item = new uint[6];
                                WinLose.item[0] = 5;
                                WinLose.item[1] = Info.item[0];
                                WinLose.item[2] = 5;
                                WinLose.item[3] = 1;
                                WinLose.item[4] = 1;
                                WinLose.item[5] = 1;
                                client.Send(stream.CreateSpiritInteractive(WinLose));
                            }
                            else
                            {
                                var WinLose = new MsgSpiritInteractive.ProtoStructure();
                                WinLose.Type = ActionID.AnimaCrest;
                                WinLose.item = new uint[6];
                                WinLose.item[0] = 5;
                                WinLose.item[1] = Info.item[0];
                                WinLose.item[2] = 5;
                                WinLose.item[3] = 5;
                                WinLose.item[4] = 5;
                                WinLose.item[5] = 1;
                                client.Send(stream.CreateSpiritInteractive(WinLose));
                            }

                        }

                        break;
                    }
                case ActionID.AnimaCrestMoney:
                    {
                        long Money = Info.item[1] * Info.item[2];
                        if (client.Player.Money >= Money)
                        {
                            client.Player.Money -= Money;
                            var WinLose = new MsgSpiritInteractive.ProtoStructure();
                            WinLose.Type = ActionID.AnimaCrestMoney;
                            if (MyMath.Success(50))
                            {
                                client.Player.Money += Money * Info.item[1];
                                WinLose.item = new uint[5];
                                WinLose.item[0] = 0;
                                WinLose.item[1] = (uint)Money;
                                WinLose.item[2] = Info.item[2] * Info.item[1];
                                WinLose.item[3] = 1;
                                WinLose.item[4] = 1;
                                client.Send(stream.CreateSpiritInteractive(WinLose));
                            }
                            else
                            {
                                WinLose.item = new uint[5];
                                WinLose.item[0] = 0;
                                WinLose.item[1] = Info.item[1];
                                WinLose.item[2] = Info.item[2];
                                WinLose.item[3] = 1;
                                WinLose.item[4] = 0;
                                client.Send(stream.CreateSpiritInteractive(WinLose));
                            }

                        }
                        break;
                    }
            }
        }
    }
}