using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using MortalConquer.Role.Instance;

namespace MortalConquer.Game.MsgServer
{
    public static class MsgXuanBaoOpt
    {
        [ProtoContract]
        public class XuanBaoOptProto
        {
            [ProtoMember(1, IsRequired = true)]
            public byte Type;
            [ProtoMember(2)]
            public uint Relic1;
            [ProtoMember(3)]
            public uint Relic2;
            [ProtoMember(4)]
            public uint Result;
        }
        public static unsafe ServerSockets.Packet CreateXuanBaoOpt(this ServerSockets.Packet stream, XuanBaoOptProto obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgXuanBaoOpt);

            return stream;
        }
        public static unsafe void GetXuanBaoOpt(this ServerSockets.Packet stream, out XuanBaoOptProto pQuery)
        {
            pQuery = new XuanBaoOptProto();
            pQuery = stream.ProtoBufferDeserialize<XuanBaoOptProto>(pQuery);
        }
        [PacketAttribute(GamePackets.MsgXuanBaoOpt)]
        private unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {

            XuanBaoOptProto Info;
            stream.GetXuanBaoOpt(out Info);
            switch (Info.Type)
            {
                case 0:
                    {
                        MsgGameItem Item_1;
                        MsgGameItem Item_2;
                        Random R = new Random();
                        if (user.Inventory.TryGetItem(Info.Relic1, out Item_1) && user.Inventory.TryGetItem(Info.Relic2, out Item_2))
                        {
                            if (user.Player.Money >= 500000)
                            {
                                Item_1.Position = 213;
                                Item_2.Position = 214;

                                MsgGameItem Item_3 = new MsgGameItem();
                                Item_3.UID = Pool.ITEM_Counter.Next;
                                Item_3.ITEM_ID = Item_1.ITEM_ID;
                                Item_3.Durability = Item_3.MaximDurability = (ushort)(R.Next(30, 101) * 100);
                                Item_3.Mode = Role.Flags.ItemMode.AddItem;
                                Item_3.Position = 215;
                                Item_3.StackSize = 1;
                                Item_3.RelicAttributes = new RelicAttribute[Item_1.RelicAttributes.Length];
                                for (byte i = 0; i < Item_1.RelicAttributes.Length; i++)
                                {

                                    if (Role.Core.Rate(30))
                                    {
                                        RelicAttribute.Attribute attr;
                                        uint value;
                                        cq_xuanbao_rand_attr.GetRandom(Item_3.ITEM_ID, out attr, out value, true);
                                        Item_3.RelicAttributes[i] = new RelicAttribute();
                                        Item_3.RelicAttributes[i].Type = attr;
                                        Item_3.RelicAttributes[i].Value = value;
                                    }
                                    else
                                    {
                                        Item_3.RelicAttributes[i] = new RelicAttribute();
                                        Item_3.RelicAttributes[i].Type = Item_1.RelicAttributes[i].Type;
                                        Item_3.RelicAttributes[i].Value = Item_1.RelicAttributes[i].Value;
                                    }
                                    if (Item_3.RelicAttributes[i].Type == Item_1.RelicAttributes[i].Type)
                                    {
                                        if (Item_1.RelicAttributes[i].Type == RelicAttribute.Attribute.LuckyStrike)
                                        {
                                            Item_3.RelicAttributes[i].Value = 100;
                                            Item_3.RelicAttributes[i].Epic = true;
                                        }
                                        else
                                        {
                                            if (Item_1.RelicAttributes[i].Type != 0)
                                            {
                                                int limit = (int)cq_xuanbao_compose_attr_limit.xuanbao_compose_attr_limit[(uint)Item_1.RelicAttributes[i].Type].limit;

                                                Item_3.RelicAttributes[i].Value = (uint)(Item_1.RelicAttributes[i].Value + (limit * 1 / 100));
                                                if (Item_3.RelicAttributes[i].Value > limit)
                                                { Item_3.RelicAttributes[i].Value = (uint)limit; }
                                            }
                                        }
                                    }
                                }

                                Item_3.Send(user, stream);

                                user.RelicFuse.Add(Item_1.UID, Item_1);
                                user.RelicFuse.Add(Item_2.UID, Item_2);
                                user.RelicFuse.Add(Item_3.UID, Item_3);
                                user.Inventory.ClientItems.Remove(Item_1.UID);
                                user.Inventory.ClientItems.Remove(Item_2.UID);
                                user.Inventory.ClientItems.Remove(Item_3.UID);
                                Info.Relic1 = Item_1.UID;
                                Info.Relic2 = Item_2.UID;
                                Info.Result = Item_3.UID;
                                user.Send(stream.CreateXuanBaoOpt(Info));
                                user.Player.Money -= 500000;
                            }
                            else
                            {
                                user.SendSysMesage("500000");
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        MsgGameItem GetRelic;
                        if (user.RelicFuse.TryGetValue(Info.Relic1, out GetRelic))
                        {
                            foreach (var i in user.RelicFuse.Values)
                            {
                                if (i.UID == Info.Relic1)
                                {
                                    GetRelic.Position = 0;
                                    GetRelic.Mode = Role.Flags.ItemMode.Update;
                                    GetRelic.Send(user, stream);
                                    Info.Relic1 = GetRelic.UID;
                                    user.Inventory.ClientItems.TryAdd(GetRelic.UID, GetRelic);
                                }
                            }
                            user.RelicFuse.Clear();
                        }
                        user.Send(stream.CreateXuanBaoOpt(Info));
                        break;
                    }
                case 4:
                    {
                        if (Info.Relic1 == 1)
                        {
                            if (user.Player.ConquerPoints >= 99 * Info.Relic2)
                            {
                                if (user.Inventory.HaveSpace((byte)Info.Relic2))
                                {
                                    for (int i = 0; i < Info.Relic2; i++)
                                        user.Inventory.AddRandomRelic(new ServerSockets.RecycledPacket().GetStream(), false, false);
                                }
                                user.Player.ConquerPoints -= 99 * Info.Relic2;
                            }
                        }
                        else
                        {
                            if (user.Player.ConquerPoints >= 10 * Info.Relic2)
                            {
                                if (user.Inventory.HaveSpace((byte)Info.Relic2))
                                {
                                    for (int i = 0; i < Info.Relic2; i++)
                                        user.Inventory.AddRandomRelic(new ServerSockets.RecycledPacket().GetStream(), false, false);
                                }
                                user.Player.ConquerPoints -= 10 * Info.Relic2;
                            }

                        }
                        break;
                    }
            }
        }
    }
}
