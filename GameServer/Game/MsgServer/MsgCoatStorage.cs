using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgCoatStorage
    {

        [ProtoContract]
        public class CoatStorage
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ActionID;
            [ProtoMember(2, IsRequired = false)]
            public uint dwparam1;
            [ProtoMember(3, IsRequired = true)]
            public uint dwpram2;
            [ProtoMember(4, IsRequired = true)]
            public uint dwpram3;
            [ProtoMember(5, IsRequired = false)]
            public ItemStorage[] Item;
            [ProtoMember(6, IsRequired = false)]
            public uint UK;
            [ProtoMember(7, IsRequired = true)]
            public ItemStorage2[] Item2;
        }
        [ProtoContract]
        public class ItemStorage
        {
            public ItemStorage(Game.MsgServer.MsgGameItem item)
            {
                ItemUID = item.UID;
                ItemID = item.ITEM_ID;
                MaxDurability = MinDurability = item.MaximDurability;
                Stack = 1;
                Plus = item.Plus;
                Bless = item.Bless;
                Type1 = item.Bound;
                Type2 = Type1 = 3;

            }
            [ProtoMember(1, IsRequired = true)]
            public uint ItemUID;
            [ProtoMember(2, IsRequired = true)]
            public uint ItemID;
            [ProtoMember(3, IsRequired = true)]
            public uint dwparam3;
            [ProtoMember(4, IsRequired = true)]
            public uint dwparam4;
            [ProtoMember(5, IsRequired = true)]
            public uint dwparam5;
            [ProtoMember(6, IsRequired = true)]
            public uint dwparam6;
            [ProtoMember(7, IsRequired = true)]
            public uint dwparam7;
            [ProtoMember(8, IsRequired = true)]
            public uint Plus;
            [ProtoMember(9, IsRequired = true)]
            public uint Bless;
            [ProtoMember(10, IsRequired = true)]
            public uint Type1;//??
            [ProtoMember(11, IsRequired = true)]
            public uint dwparam11;
            [ProtoMember(12, IsRequired = true)]
            public uint dwparam12;
            [ProtoMember(13, IsRequired = true)]
            public uint dwparam13;
            [ProtoMember(14, IsRequired = true)]
            public uint dwparam14;
            [ProtoMember(15, IsRequired = true)]
            public uint Type2;
            [ProtoMember(16, IsRequired = true)]
            public uint dwparam16;
            [ProtoMember(17, IsRequired = true)]
            public uint dwparam17;
            [ProtoMember(18, IsRequired = true)]
            public uint dwparam18;
            [ProtoMember(19, IsRequired = true)]
            public uint TimeLeft;
            [ProtoMember(20, IsRequired = true)]
            public uint dwparam20;
            [ProtoMember(21, IsRequired = true)]
            public uint Stack;
            [ProtoMember(22, IsRequired = true)]
            public uint MinDurability;
            [ProtoMember(23, IsRequired = true)]
            public uint MaxDurability;
        }
        [ProtoContract]
        public class ItemStorage2
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ItemUID;//408808453 
            [ProtoMember(2, IsRequired = true)]
            public ulong RealTime;//Time Now
            [ProtoMember(3, IsRequired = true)]
            public ulong uk1;//
            [ProtoMember(4, IsRequired = true)]
            public ulong Color;//col
            [ProtoMember(5, IsRequired = true)]
            public ulong Time;//time
        }
        [Flags]
        public enum Action : uint
        {
            Equip = 1,
            Retrive = 2,
            Update = 3,
            EquipMountColor = 14,
            RandomChange = 11,
            AddToWardRobe = 4,
            TakeOff = 5,
            Expire = 8,
            LoadMountColor = 10,

        }
        public static unsafe ServerSockets.Packet CreateCoatStorage(this ServerSockets.Packet stream, CoatStorage obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgCoatStorage);

            return stream;
        }
        public static unsafe void GetCoatStorage(this ServerSockets.Packet stream, out CoatStorage pQuery)
        {
            pQuery = new CoatStorage();
            pQuery = stream.ProtoBufferDeserialize<CoatStorage>(pQuery);
        }
        public static bool CheckGender(Client.GameClient user, uint itemid)
        {
            Database.ItemType.DBItem dbitem;
            if (Pool.ItemsBase.TryGetValue(itemid, out dbitem))
            {
                if (dbitem.Gender == 0)
                    return true;
                if (dbitem.Gender == 1)//boy
                {
                    if (Role.Core.IsBoy(user.Player.Body))
                        return true;
                }
                if (dbitem.Gender == 2)//female
                {
                    if (Role.Core.IsGirl(user.Player.Body))
                        return true;
                }
            }
            return false;
        }
        [PacketAttribute(GamePackets.MsgCoatStorage)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            CoatStorage pQuery;
            stream.GetCoatStorage(out pQuery);
            switch ((Action)pQuery.ActionID)
            {
                case Action.Update:
                    {
                        MsgGameItem item, ExistedItem;
                        if (client.Inventory.TryGetItem(pQuery.dwpram2, out item))
                        {
                            if (client.MyWardrobe.TryGetItem(pQuery.dwparam1, out ExistedItem))
                            {
                                if (item != null && ExistedItem != null)
                                {
                                    if (ExistedItem.RemainingTime > 0 && item.RemainingTime == 0)
                                    {
                                        ExistedItem.EndDate = item.EndDate;
                                        client.Inventory.Update(item, Role.Instance.AddMode.REMOVE, stream);
                                        var store = new CoatStorage();
                                        store.ActionID = (uint)Action.Update;
                                        store.dwparam1 = ExistedItem.UID;
                                        client.Send(stream.CreateCoatStorage(store));
                                    }
                                    if (ExistedItem.RemainingTime > 0 && item.RemainingTime > 0 && ExistedItem.Bound == item.Bound)
                                    {
                                        long Date = ExistedItem.EndDate.ToBinary() + item.EndDate.ToBinary();
                                        ExistedItem.EndDate = DateTime.FromBinary(Date);
                                        client.Inventory.Update(item, Role.Instance.AddMode.REMOVE, stream);
                                        var store = new CoatStorage();
                                        store.ActionID = (uint)Action.Update;
                                        store.dwparam1 = ExistedItem.UID;
                                        client.Send(stream.CreateCoatStorage(store));
                                    }
                                    if (ExistedItem.Bound == 1 && item.Bound == 0)
                                    {
                                        ExistedItem.Bound = item.Bound;
                                        client.Inventory.Update(item, Role.Instance.AddMode.REMOVE, stream);
                                        var store = new CoatStorage();
                                        store.ActionID = (uint)Action.Update;
                                        store.dwparam1 = ExistedItem.UID;
                                        client.Send(stream.CreateCoatStorage(store));
                                    }
                                }
                            }
                        }
                        break;
                    }
                case Action.Equip:
                    {

                        Game.MsgServer.MsgGameItem item;
                        if (client.Inventory.TryGetItem(pQuery.dwparam1, out item))
                        {
                            if (client.MyWardrobe.Contain(item.ITEM_ID))
                                return;
                            ushort Position = Database.ItemType.ItemPosition(item.ITEM_ID);
                            if (Position == (ushort)Role.Flags.ConquerItem.Garment)
                            {
                                if (client.Player.SpecialGarment != 0)
                                {

                                    client.SendSysMesage("Item can't be unequiped during the event.");


                                    return;
                                }
                            }
                            if (Position == (ushort)Role.Flags.ConquerItem.Garment || Position == (ushort)Role.Flags.ConquerItem.SteedMount)
                            {
                                client.MyWardrobe.AddItem(item);
                                client.MyWardrobe.SendItem(stream, item);
                                Game.MsgServer.MsgCoatStorage.CoatStorage store = new Game.MsgServer.MsgCoatStorage.CoatStorage();
                                store.ActionID = (uint)MsgCoatStorage.Action.Equip;
                                store.dwparam1 = item.UID;
                                client.Send(stream.CreateCoatStorage(store));

                            }

                        }
                        break;
                    }
                case Action.AddToWardRobe:
                    {
                        Game.MsgServer.MsgGameItem item;

                        if (client.MyWardrobe.TryGetItem(pQuery.dwparam1, out item))
                        {
                            if (!CheckGender(client, item.ITEM_ID))
                                break;
                            ushort Position = Database.ItemType.ItemPosition(item.ITEM_ID);
                            if (Position == (ushort)Role.Flags.ConquerItem.Garment || Position == (ushort)Role.Flags.ConquerItem.SteedMount)
                            {
                                if (client.Equipment.FreeEquip((Role.Flags.ConquerItem)Position))
                                {
                                    if (client.Inventory.Update(item, Role.Instance.AddMode.REMOVE, stream))
                                    {
                                        item.Position = (ushort)Position;
                                        client.Equipment.Add(item, stream);
                                        item.Mode = Role.Flags.ItemMode.Update;
                                        item.Send(client, stream);
                                    }
                                }
                                else
                                {
                                    if (client.Inventory.Update(item, Role.Instance.AddMode.REMOVE, stream))
                                    {
                                        if (client.Equipment.Remove((Role.Flags.ConquerItem)Position, stream))
                                        {
                                            item.Position = (ushort)Position;
                                            client.Equipment.Add(item, stream);
                                            item.Mode = Role.Flags.ItemMode.Update;
                                            item.Send(client, stream);
                                        }
                                    }
                                }
                            }
                            client.Equipment.QueryEquipment(client.Equipment.Alternante);
                            Game.MsgServer.MsgCoatStorage.CoatStorage store = new Game.MsgServer.MsgCoatStorage.CoatStorage();
                            store.ActionID = (uint)MsgCoatStorage.Action.AddToWardRobe;
                            store.dwparam1 = item.UID;
                            store.dwpram2 = item.ITEM_ID;
                            client.Send(stream.CreateCoatStorage(store));
                        }
                        break;
                    }
                case Action.TakeOff:
                    {
                        Game.MsgServer.MsgGameItem item;
                        if (client.Equipment.TryGetEquip((Role.Flags.ConquerItem)pQuery.dwpram2, out item))
                        {
                            if (item.Position == (ushort)Role.Flags.ConquerItem.Garment)
                            {
                                if (client.Player.SpecialGarment != 0)
                                {

                                    client.SendSysMesage("Item can't be unequiped during the event.");


                                    return;
                                }
                            }
                            if (client.Equipment.Remove((Role.Flags.ConquerItem)item.Position, stream))
                            {
                                Game.MsgServer.MsgCoatStorage.CoatStorage store = new Game.MsgServer.MsgCoatStorage.CoatStorage();
                                store.ActionID = (uint)MsgCoatStorage.Action.TakeOff;
                                store.dwparam1 = item.UID;
                                store.dwpram2 = item.ITEM_ID;
                                client.Send(stream.CreateCoatStorage(store));

                                client.Equipment.QueryEquipment(client.Equipment.Alternante);
                            }
                        }
                        break;
                    }
                case Action.Retrive:
                    {
                        if (!client.Inventory.HaveSpace(1))
                        {
                            client.SendSysMesage("Please make 1 more space in your inventory.");
                            break;
                        }
                        Game.MsgServer.MsgGameItem item2;
                        if (client.Equipment.TryGetValue(pQuery.dwparam1, out item2))
                        {
                            if (item2.UID == pQuery.dwparam1)
                            {
                                if (client.Equipment.Remove((Role.Flags.ConquerItem)item2.Position, stream))
                                {
                                    Game.MsgServer.MsgCoatStorage.CoatStorage store = new Game.MsgServer.MsgCoatStorage.CoatStorage();
                                    store.ActionID = (uint)MsgCoatStorage.Action.TakeOff;
                                    store.dwparam1 = item2.UID;
                                    store.dwpram2 = item2.ITEM_ID;
                                    client.Send(stream.CreateCoatStorage(store));
                                    client.Equipment.QueryEquipment(client.Equipment.Alternante);
                                }
                            }
                        }

                        Game.MsgServer.MsgGameItem item;
                        if (client.MyWardrobe.RemoveItem(pQuery.dwparam1, out item))
                        {
                            client.Inventory.Update(item, Role.Instance.AddMode.ADD, stream);
                            Game.MsgServer.MsgCoatStorage.CoatStorage astore = new Game.MsgServer.MsgCoatStorage.CoatStorage();
                            astore.ActionID = (uint)MsgCoatStorage.Action.Retrive;
                            astore.dwparam1 = pQuery.dwparam1;
                            astore.dwpram2 = item.ITEM_ID;
                            client.Send(stream.CreateCoatStorage(astore));
                            client.Equipment.QueryEquipment(client.Equipment.Alternante);
                        }

                        break;
                    }
                default: MyConsole.WriteLine("Unknown Action Type [" + (uint)pQuery.ActionID + "] MsgCoatStorage."); break;
            }
        }

    }
}
