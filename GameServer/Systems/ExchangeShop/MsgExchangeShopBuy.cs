using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using VirusX.Database;

namespace VirusX.Game.MsgServer
{
    public static class MsgExchange
    {
        public static unsafe ServerSockets.Packet CreateExchangeShopBuy(this ServerSockets.Packet stream, CMsgExchangeShopBuy obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgExchangeShopBuy);
            return stream;
        }
        
        public static unsafe void GetExchangeShopBuy(this ServerSockets.Packet stream, out CMsgExchangeShopBuy pQuery)
        {
            pQuery = new CMsgExchangeShopBuy();
            pQuery = stream.ProtoBufferDeserialize(pQuery);
        }
       
        
    }
    [ProtoContract]
    public class CMsgExchangeShopBuy
    {
        [ProtoMember(1, IsRequired = true)]
        public uint Type;
        [ProtoMember(2, IsRequired = true)]
        public uint NpcID;
        [ProtoMember(3, IsRequired = true)]
        public uint ShopID;
        [ProtoMember(4, IsRequired = true)]
        public uint Count;
        [ProtoMember(5, IsRequired = true)]
        public uint Index;
        [ProtoMember(6, IsRequired = true)]
        public Item[] Items;
        [ProtoContract]
        public class Item
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ID;
            [ProtoMember(2, IsRequired = true)]
            public uint Count;
        }
        [Packet(GamePackets.MsgExchangeShopBuy)]
        private unsafe static void Process1(Client.GameClient client, ServerSockets.Packet stream)
        {
            if (client.InTrade || client.PokerPlayer != null || client.IsVendor)
                return;
            CMsgExchangeShopBuy Info = null;
            stream.GetExchangeShopBuy(out Info);
            switch (Info.Type)
            {
                case 0:
                    {
                        ExchangeShopGoods.item obj;
                        if (ExchangeShopGoods.exchange_shop_goods.TryGetValue(Info.Index, out obj))
                        {
                            if (client.MyExchangeShop.AddItem(Info.ShopID, Info.Index, Info.Count) && client.Inventory.HaveSpace(1))
                            {
                                foreach (var ITEM in Info.Items)
                                {
                                    Game.MsgServer.MsgGameItem invItem;
                                    if (client.Inventory.TryGetItem((uint)ITEM.ID, out invItem))
                                    {
                                        if (invItem.ITEM_ID != obj.material1 && invItem.ITEM_ID != obj.material2 && invItem.ITEM_ID != obj.material3 && invItem.ITEM_ID != obj.material4)
                                        {

                                            return;
                                        }
                                    }
                                }
                                #region FourItem
                                if (obj.material1 != 0 && obj.material2 != 0 && obj.material3 != 0
                                    && obj.material4 != 0)
                                {
                                    if (client.Inventory.Contain(obj.material1, (ushort)(obj.material_amount1 * Info.Count))
                                    && client.Inventory.Contain(obj.material2, (ushort)(obj.material_amount2 * Info.Count))
                                    && client.Inventory.Contain(obj.material3, (ushort)(obj.material_amount3 * Info.Count))
                                    && client.Inventory.Contain(obj.material4, (ushort)(obj.material_amount4 * Info.Count)))
                                    {
                                        client.Inventory.RemoveStackItem(obj.material1, (ushort)(obj.material_amount1 * Info.Count), stream);
                                        client.Inventory.RemoveStackItem(obj.material2, (ushort)(obj.material_amount2 * Info.Count), stream);
                                        client.Inventory.RemoveStackItem(obj.material3, (ushort)(obj.material_amount3 * Info.Count), stream);
                                        client.Inventory.RemoveStackItem(obj.material4, (ushort)(obj.material_amount4 * Info.Count), stream);
                                        client.Inventory.AddItemWitchStack(obj.itemtypeid, 0, (ushort)Info.Count, stream, (bool)(obj.monopoly != 0 ? true : false));
                                        client.Send(stream.CreateExchangeShopBuy(Info));
                                    }
                                }
                                #endregion

                                #region ThreeItem
                                else if (obj.material1 != 0 && obj.material2 != 0 && obj.material3 != 0)
                                {
                                    if (client.Inventory.Contain(obj.material1, (ushort)(obj.material_amount1 * Info.Count))
                                    && client.Inventory.Contain(obj.material2, (ushort)(obj.material_amount2 * Info.Count))
                                    && client.Inventory.Contain(obj.material3, (ushort)(obj.material_amount3 * Info.Count)))
                                    {
                                        client.Inventory.RemoveStackItem(obj.material1, (ushort)(obj.material_amount1 * Info.Count), stream);
                                        client.Inventory.RemoveStackItem(obj.material2, (ushort)(obj.material_amount2 * Info.Count), stream);
                                        client.Inventory.RemoveStackItem(obj.material3, (ushort)(obj.material_amount3 * Info.Count), stream);
                                        client.Inventory.AddItemWitchStack(obj.itemtypeid, 0, (ushort)Info.Count, stream, (bool)(obj.monopoly != 0 ? true : false));
                                        client.Send(stream.CreateExchangeShopBuy(Info));
                                    }
                                }
                                #endregion

                                #region TwoItem
                                else if (obj.material1 != 0 && obj.material2 != 0)
                                {
                                    if (client.Inventory.Contain(obj.material1, (ushort)(obj.material_amount1 * Info.Count))
                                    && client.Inventory.Contain(obj.material2, (ushort)(obj.material_amount2 * Info.Count)))
                                    {
                                        client.Inventory.RemoveStackItem(obj.material1, (ushort)(obj.material_amount1 * Info.Count), stream);
                                        client.Inventory.RemoveStackItem(obj.material2, (ushort)(obj.material_amount2 * Info.Count), stream);
                                        client.Inventory.AddItemWitchStack(obj.itemtypeid, 0, (ushort)Info.Count, stream, (bool)(obj.monopoly != 0 ? true : false));
                                        client.Send(stream.CreateExchangeShopBuy(Info));
                                    }
                                }
                                #endregion

                                #region OneItem
                                else if (obj.material1 != 0)
                                {
                                    if (client.Inventory.Contain(obj.material1, (ushort)(obj.material_amount1 * Info.Count)))
                                    {
                                        foreach (var _items in Info.Items)
                                        {
                                            client.Inventory.RemoveStackItem(_items.ID, (ushort)(_items.Count), stream);
                                        }
                                        client.Inventory.AddItemWitchStack(obj.itemtypeid, 0, (ushort)Info.Count, stream, (bool)(obj.monopoly != 0 ? true : false));
                                        client.Send(stream.CreateExchangeShopBuy(Info));
                                    }
                                }
                                #endregion
                                ServerLogs.AddBuyingExchangeShop(client, Pool.ItemsBase[obj.itemtypeid], Info.Count.ToString(), "" + (obj.monopoly != 0 ? "Bound" : "UnBound") + "");

                            }
                        }
                        break;
                    }
                case 1:
                    {
                        client.CreateBoxDialog("This System Closed By GM");
                        break;
                        ExchangeShopGoodsEx.item obj;
                        if (ExchangeShopGoodsEx.exchange_shop_goods_ex.TryGetValue(Info.Index, out obj))
                        {
                            if (client.MyExchangeShop.AddItemEx(Info.ShopID, Info.Index, Info.Count))
                            {
                                if (obj.cost_type == 4 && client.Player.MyDontion >= obj.cost_value && client.Inventory.HaveSpace(1))
                                {
                                    client.Player.MyDontion -= obj.cost_value;
                                    client.Inventory.AddItemWitchStack(obj.itemtypeid, 0, (ushort)Info.Count, stream, (bool)(obj.monopoly != 0 ? true : false));
                                    ServerLogs.AddBuyingExchangeShop(client, Pool.ItemsBase[obj.itemtypeid], Info.Count.ToString(), "Shop Guild" + (obj.monopoly != 0 ? "Bound" : "UnBound") + "");
                                    client.Send(stream.CreateExchangeShopBuy(Info));
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }
}
