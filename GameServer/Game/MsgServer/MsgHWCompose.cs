using ProtoBuf;
using System;
using System.Linq;

namespace MortalConquer.Game.MsgServer
{
    public static class MsgHWCompose
    {
        public const uint ImproveItemID = 3321098, ImproveEXP = 10;
        public const uint SpiritStone_ID = 3321107, ChaosJade_ID = SpiritStone_ID + 1;
        [ProtoContract]
        public class MsgHWComposeProto
        {
            [ProtoMember(1, IsRequired = true)]
            public ActionID Type;
            [ProtoMember(2, IsRequired = true)]
            public Database.MagicType.WeaponsType SubType;
            [ProtoMember(3, IsRequired = true)]
            public uint dwParam;
            [ProtoMember(4)]
            public byte AppearancePosition;
            [ProtoMember(5)]
            public Item[] Items;
        }
        [ProtoContract]
        public class Item
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ItemUID;
            [ProtoMember(2, IsRequired = true)]
            public int dwParam;
        }

        [Flags]
        public enum ActionID
        {
            Improve,
            Baptize,
            ConfirmBaptize,
            Guardian
        }
        public static unsafe ServerSockets.Packet CreateHWCompose(this ServerSockets.Packet stream, MsgHWComposeProto msg)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(msg);
            stream.Finalize(GamePackets.MsgHWCompose);
            return stream;
        }
        [PacketAttribute(GamePackets.MsgHWCompose)]
        public static unsafe void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            MsgHWComposeProto msg = new MsgHWComposeProto();
            msg = stream.ProtoBufferDeserialize<MsgHWComposeProto>(msg);
            if ((msg.Items == null && (byte)msg.Type < (byte)ActionID.Baptize) || !client.HundredWeapons.Unlocked || !client.HundredWeapons.Valid() || !client.HundredWeapons.ContainsWeapon(msg.SubType)) return;
            var myWeapon = client.HundredWeapons.Objects[(Database.MagicType.WeaponsType)msg.SubType];

            switch (msg.Type)
            {
                case ActionID.Improve:
                    {
                        MsgGameItem ItemPlus = null;
                        for (int i = 0; i < msg.Items.Length; i++)
                        {
                            if (!client.Inventory.TryGetItem(msg.Items[i].ItemUID, out ItemPlus))
                                continue;
                            if (ItemPlus.ITEM_ID != ImproveItemID)
                                continue;
                            if (ItemPlus.StackSize < (ushort)msg.Items[i].dwParam)
                            {
                                msg.Items[i].dwParam = ItemPlus.StackSize;
                                ItemPlus.Send(client, stream);
                            }
                           // if (!client.MyProfs.CheckProf((ushort)msg.SubType, Database.HundredWeapons.HundredWeaponsList.Values.Where(x => x.WeaponSubtype == msg.SubType && x.Level == myWeapon.Level).FirstOrDefault().WeaponProf))
                               // break;
                            if (myWeapon.Level >= 9)
                                break;

                            myWeapon.Progress += (uint)(ImproveEXP * msg.Items[i].dwParam);
                            while (myWeapon.Progress >= myWeapon.DBInfo.Progress)
                            {
                                myWeapon.Progress -= myWeapon.DBInfo.Progress;
                                myWeapon.Level++;
                                if (myWeapon.Level >= 9)
                                {
                                    myWeapon.Progress = 0;
                                    break;
                                }
                            }
                            client.Inventory.RemoveStackItem(ItemPlus.UID, (ushort)msg.Items[i].dwParam, stream);
                            //Database.ServerDatabase.LoginQueue.Enqueue("[Archive] Name: " + client.Player.Name + " UID :" + client.Player.UID + " IP: " + client.Socket.RemoteIp + " MacAddress " + client.Socket.GetMACAddress() +  " Weapon "+ myWeapon.Progress +" Time " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                              
                        }
                        break;
                    
                    }
                case ActionID.Baptize:
                    {
                        if (msg.Items.Length < 1 || myWeapon.Level < 3) break;
                        if (!client.Inventory.Remove(msg.Items[0].ItemUID, myWeapon.DBInfo.ReqItemCount, stream)) break;
                        var attributes = myWeapon.Attributes.ToArray();
                        foreach (var attribute in attributes)
                        {
                            if (attribute.Key == Database.HundredWeapons.AttributeType.MagicAttack || attribute.Key == Database.HundredWeapons.AttributeType.MagicDefense)
                                continue;//Disabled Temporary
                        again:
                            int max = myWeapon.DBInfo.Attributes[attribute.Key];
                            if (msg.Items[0].ItemUID == SpiritStone_ID)
                            {
                                int value = myWeapon.Attributes[attribute.Key] = Pool.GetRandom.Next(0, max * Pool.GetRandom.Next(1, 70) / 100);
                                while (100 * value / max >= 70 && Role.Core.Rate(60) || 100 * value / max >= 90 && Role.Core.Rate(95))
                                    goto again;
                            }
                            else if (msg.Items[0].ItemUID == ChaosJade_ID)
                            {
                                int value = myWeapon.Attributes[attribute.Key] = Pool.GetRandom.Next(max * Pool.GetRandom.Next(30, 70) / 100, max);
                                while (100 * value / max < 70 && Role.Core.Rate(50) || 100 * value / max < 90 && Role.Core.Rate(10))
                                    goto again;
                            }
                            myWeapon.Attributes[attribute.Key] += myWeapon.DBInfo.Attributes[attribute.Key];
                        }
                        break;
                    }
                case ActionID.Guardian:
                    {
                        if (msg.AppearancePosition > 0)
                        {
                            if (client.HundredWeapons.FreeAppearance(msg.AppearancePosition) || client.HundredWeapons.Hide(msg.AppearancePosition))
                            {
                                byte duplicatedPosition;
                                if (!client.HundredWeapons.checkDuplicatedAppearance(msg.SubType, msg.AppearancePosition, out duplicatedPosition) || client.HundredWeapons.Hide(duplicatedPosition))
                                    client.HundredWeapons.Show(msg.SubType, msg.AppearancePosition);
                            }
                        }
                        else
                        {
                            if (myWeapon.AppearancePosition > 0)
                                client.HundredWeapons.Hide(msg.SubType);
                        }
                        break;
                    }
            }
            msg.Items = null;
            client.Send(stream.CreateHundredWeaponsInfo(client, MsgHundredWeaponsInfo.ActionID.Update));
            client.Send(stream.CreateHWCompose(msg));
            client.HundredWeapons.UpdateRank();
        }
    }
}