using ProtoBuf;
using VirusX.Client;
using System.Collections.Generic;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateShopClan(this ServerSockets.Packet stream, MsgShopClan.ProtoStructure pQuery)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgNewShopGoods);
            return stream;
        }
        public static void GetExchangeShopGoodsInfo(this ServerSockets.Packet stream, out MsgShopClan.ProtoStructure pQuery)
        {
            pQuery = new MsgShopClan.ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<MsgShopClan.ProtoStructure>(pQuery);
        }

    }
  
    public class MsgShopClan
    {
        [ProtoContract]
        public class ProtoStructure
        {
            public enum Action : byte
            {
                Show = 0,
                Buy = 1,
                Refersh = 4,
                BuyShop = 6,
            }
            [ProtoMember(1, IsRequired = true)]
            public Action ACTion;
            [ProtoMember(2, IsRequired = true)]
            public uint ID;
            [ProtoMember(3, IsRequired = true)]
            public uint Level;
            [ProtoMember(4, IsRequired = true)]
            public uint Unk1;
            [ProtoMember(5, IsRequired = true)]
            public ulong FreeTime;
            [ProtoMember(6, IsRequired = true)]
            public uint PayNum;
            [ProtoMember(7, IsRequired = true)]
            public uint Ret;
            [ProtoMember(8, IsRequired = true)]
            public List<ITemProto> Items;
            [ProtoMember(9, IsRequired = true)]
            public uint Ret9;
        }
        [ProtoContract]
        public class ITemProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint IDShop;
            [ProtoMember(2, IsRequired = true)]
            public uint Count;
            [ProtoMember(3, IsRequired = true)]
            public ulong Time;
        }
        [PacketAttribute(GamePackets.MsgNewShopGoods)]
        private static void Process(GameClient user, ServerSockets.Packet stream)
        {
            if (user.InTrade || user.PokerPlayer != null || user.IsVendor)
                return;
            MsgShopClan.ProtoStructure pQuery;
            stream.GetExchangeShopGoodsInfo(out pQuery);
            switch (pQuery.ACTion)
            {
                case ProtoStructure.Action.Show:
                    {
                        if (pQuery.ID == 27950
                            || pQuery.ID == 23561
                            || pQuery.ID == 22372
                            || pQuery.ID == 23127)
                        {
                            user.Send(stream.CreateShopClan(pQuery));
                        }
                        else
                        {
                            user.MyExchangeShop.OpenClan(stream, 1);
                        }
                        break;
                    }
                case ProtoStructure.Action.Buy:
                    {
                       
                        Database.new_shop_goods.item obj;
                        
                        if (Database.new_shop_goods.ShopClan.TryGetValue(pQuery.Items[0].IDShop, out obj))
                        {
                            if (user.MyExchangeShop.AddItemClan(pQuery.ID, pQuery.Items[0].IDShop, (ushort)pQuery.Items[0].Count))
                            {
                                if (obj.shopid == 1)
                                {
                                    if (user.Player.CyanJadeRing >= obj.CountBuy && user.Inventory.HaveSpace(1))
                                    {
                                        user.Player.CyanJadeRing -= obj.CountBuy;
                                        user.Inventory.AddItemWitchStack(obj.ItemID, 0, (ushort)pQuery.Items[0].Count, stream, (bool)(obj.Bound != 0 ? true : false));
                                        ServerLogs.AddBuyingOtherShop(user, Pool.ItemsBase[obj.ItemID], pQuery.Items[0].Count.ToString(), "ShopClan" + (obj.Bound != 0 ? "Bound" : "UnBound") + "");
                                        pQuery.Ret = 1;
                                        user.Send(stream.CreateShopClan(pQuery));
                                    }
                                }
                                if (obj.unk1 == 2)
                                {
                                    int Amount = (int)obj.CountBuy * (ushort)pQuery.Items[0].Count;
                                    if (Amount < 0)
                                        return;
                                    if (user.Player.BoundConquerPoints >= obj.CountBuy * (ushort)pQuery.Items[0].Count && user.Inventory.HaveSpace(1))
                                    {
                                        user.Player.BoundConquerPoints -= (int)obj.CountBuy * (ushort)pQuery.Items[0].Count;
                                        user.Inventory.AddItemWitchStack(obj.ItemID, 0, (ushort)pQuery.Items[0].Count, stream, (bool)(obj.Bound != 1 ? true : false));
                                        ServerLogs.AddBuyingOtherShop(user, Pool.ItemsBase[obj.ItemID], pQuery.Items[0].Count.ToString(), "ShopClan" + (obj.Bound != 0 ? "Bound" : "UnBound") + "");
                                        pQuery.Ret = 1;
                                        user.Send(stream.CreateShopClan(pQuery));
                                    }
                                }
                            }
                        }
                        break;
                    }
                case ProtoStructure.Action.BuyShop:
                    {
                        Database.new_shop_goods.item obj;
                        for (int x = 0; x < pQuery.Items.Count; x++)
                        {
                            if (Database.new_shop_goods.ShopClan.TryGetValue(pQuery.Items[x].IDShop, out obj))
                            {
                                if (user.MyExchangeShop.AddItemClan(pQuery.ID, pQuery.Items[x].IDShop, (ushort)pQuery.Items[x].Count))
                                {
                                    if (obj.unk1 == 1)
                                    {
                                        int Amount = (int)obj.CountBuy * (ushort)pQuery.Items[x].Count;
                                        if (Amount < 0)
                                            return;
                                        if (user.Player.ConquerPoints >= obj.CountBuy * (ushort)pQuery.Items[x].Count && user.Inventory.HaveSpace(1))
                                        {
                                            user.Player.ConquerPoints -= (long)obj.CountBuy * (ushort)pQuery.Items[x].Count;
                                            user.Inventory.AddItemWitchStack(obj.ItemID, 0, (ushort)pQuery.Items[x].Count, stream, (bool)(obj.Bound != 0 ? true : false));
                                            ServerLogs.AddBuyingOtherShop(user, Pool.ItemsBase[obj.ItemID], pQuery.Items[x].Count.ToString(), "" + (obj.Bound != 0 ? "Bound" : "UnBound") + "");
                                            pQuery.Ret = 1;

                                            user.Send(stream.CreateShopClan(pQuery));
                                        }
                                    }
                                    else if (obj.unk1 == 2)
                                    {
                                        int Amount = (int)obj.CountBuy * (ushort)pQuery.Items[x].Count;
                                        if (Amount < 0)
                                            return;
                                        if (user.Player.BoundConquerPoints >= obj.CountBuy * (ushort)pQuery.Items[x].Count && user.Inventory.HaveSpace(1))
                                        {
                                            user.Player.BoundConquerPoints -= (int)obj.CountBuy * (ushort)pQuery.Items[x].Count;
                                            user.Inventory.AddItemWitchStack(obj.ItemID, 0, (ushort)pQuery.Items[x].Count, stream, (bool)(obj.Bound != 0 ? true : false));
                                            ServerLogs.AddBuyingOtherShop(user, Pool.ItemsBase[obj.ItemID], pQuery.Items[x].Count.ToString(), "" + (obj.Bound != 0 ? "Bound" : "UnBound") + "");
                                            pQuery.Ret = 1;
                                            user.Send(stream.CreateShopClan(pQuery));
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                case ProtoStructure.Action.Refersh:
                    {
                        if (user.Player.CyanJadeRing >= 50)
                        {
                            user.MyExchangeShop.GetRandomClan(1, 8);
                            user.MyExchangeShop.OpenClan(stream, 1);
                            user.Player.CyanJadeRing -= 50;
                        }
                        break;
                    }
                default: MyConsole.WriteLine("Unknown Action Type [" + (uint)pQuery.ACTion + "] MsgShopClan."); break;
            }
        }

    }
}
