using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public static class MsgMelterOpt
    {
        [ProtoContract]
        public class MsgMelterOptProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2)]
            public uint EntityID;
            [ProtoMember(3)]
            public string EntityName;
            [ProtoMember(4, IsRequired = true)]
            public Item[] Items;
            [ProtoMember(5, IsRequired = true)]
            public byte unk;//1
            [ProtoMember(6, IsRequired = true)]
            public byte unk2;//0
        }
        [ProtoContract]
        public class Item
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2, IsRequired = true)] 
            public uint ID;
            [ProtoMember(3, IsRequired = true)]
            public bool Bound;
            [ProtoMember(4, IsRequired = true)]
            public bool AnnounceMessage;
            [ProtoMember(5, IsRequired = true)]
            public byte unk3;
        }
        public static unsafe ServerSockets.Packet CreateMelterOpt(this ServerSockets.Packet stream, MsgMelterOptProto obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgMelterOpt);
            return stream;
        }
        public static unsafe void GetMelterOpt(this ServerSockets.Packet stream, out MsgMelterOptProto pQuery)
        {
            pQuery = new MsgMelterOptProto();
            pQuery = stream.ProtoBufferDeserialize<MsgMelterOptProto>(pQuery);
        }
        public static readonly List<uint> Harditem = new List<uint>() { 3009100 ,3009101 ,3009102 ,3009103 ,3009104, 3311746 };
        public static void GettargetedItemID(uint targetedItemID, out uint targetedItemIDs)
        {
            targetedItemIDs = 0;
            if (targetedItemID == 3009100 && !Role.Core.Rate(30))
                targetedItemIDs = 3001036;
            if (targetedItemID == 3311740 && !Role.Core.Rate(15))
                targetedItemIDs = 1088000;
            else if (targetedItemID == 3009101 && !Role.Core.Rate(20))
                targetedItemIDs = 3001036;

            else if (targetedItemID == 3009102 && !Role.Core.Rate(10))
                targetedItemIDs = 3001036;

            else if (targetedItemID == 3009103 && !Role.Core.Rate(7))
                targetedItemIDs = 3001036;

            else if (targetedItemID == 3009104 && !Role.Core.Rate(5))
                targetedItemIDs = 3001036;

            else if (targetedItemID == 3009104 && !Role.Core.Rate(5))
                targetedItemIDs = 3001036;

            else if (targetedItemID == 3311746 && !Role.Core.Rate(5))
            {
                uint[] Items = new uint[] { 3001035, 3310998 };
                targetedItemIDs = Items[(uint)Pool.GetRandom.Next(0, Items.Length)];
            }
            if (targetedItemIDs == 0)
                targetedItemIDs = targetedItemID;
        }

        [PacketAttribute(GamePackets.MsgMelterOpt)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            /*client.CreateBoxDialog("This System Is Blocked By GM");
            return;*/
            MsgMelterOptProto Info;
            stream.GetMelterOpt(out Info);
            
            if (Info.Type == 0)
            {
                if (Info.Items != null)
                {
                    if (Info.Items.Length > 0)
                    {
                        var item = Database.MeltingTypeTable.MeltingTypes.Where(i => i.Type == Database.MeltingTypeTable.Type.MeltingItem && i.ItemID == Info.Items[0].ID).FirstOrDefault();
                        if (item != null)
                        {
                            var prizeItems = Database.MeltingTypeTable.MeltingTypes.Where(i => i.Type == Database.MeltingTypeTable.Type.PrizePool && i.PrizeType == item.PrizeType).FirstOrDefault().Prizes;
                            var targetedItemID = prizeItems[Pool.GetRandom.Next(0, prizeItems.Count)];
                            if (Harditem.Contains(targetedItemID))
                            {
                                uint targetedItemIDs;
                                GettargetedItemID(targetedItemID, out targetedItemIDs);
                                targetedItemID = targetedItemIDs;
                            }

                            MsgGameItem msgItem;
                            if (client.Inventory.SearchItemByID(item.ItemID, out msgItem) && client.Inventory.Remove(msgItem.ITEM_ID, 1, stream))
                            {
                                client.Inventory.AddItemWitchStack(targetedItemID, 0, 1, stream, true);
                               
                                var packet = new MsgMelterOptProto() { EntityID = client.Player.UID, EntityName = client.Player.Name, Type = 0, unk = 1, unk2 = 0 };
                                packet.Items = new Item[1];
                                packet.Items[0] = new Item() { ID = targetedItemID, Type = 8, Bound = msgItem.Bound != 0, AnnounceMessage = false, unk3 = 1 };
                                client.Send(stream.CreateMelterOpt(packet));
                                packet.Items[0].AnnounceMessage = true;
                                foreach (var player in Pool.GamePoll.Values)
                                {
                                    client.Send(new MsgMessage("STR_ID_tMelter[BroadCast][Result]@@" + client.Player.Name + "@@<" + Pool.ItemsBase[msgItem.ITEM_ID].Name + "@@>@@<" + Pool.ItemsBase[targetedItemID].Name + "", MsgMessage.MsgColor.white, MsgMessage.ChatMode.Melting).GetArray(stream));
                                    if (player.Player.UID != client.Player.UID)
                                        player.Send(stream.CreateMelterOpt(packet));
                                }
                                

                                Pool.MelterRankList.Add(new MsgMelterRankList.Instance() { Name = client.Player.Name, Bound = packet.Items[0].Bound, Type = 8, ItemID = targetedItemID, Unknown3 = (uint)Pool.GetRandom.Next(1559853103, 1559853332) });
                             
                            }
                        }
                    }
                }
                else
                {
                    client.Send(stream.CreateMelterOpt(Info));
                    
                }
                
            }
            else if (Info.Type == 1)
            {
                if (Info.Items.Length > 0)
                {
                    var item = Database.MeltingTypeTable.MeltingTypes.Where(i => i.Type == Database.MeltingTypeTable.Type.MeltingItem && i.ItemID == Info.Items[0].ID).FirstOrDefault();
                    if (item != null)
                    {
                        var packet = new MsgMelterOptProto() { EntityID = client.Player.UID, EntityName = client.Player.Name, Type = 1, unk = 1, unk2 = 0 };
                        packet.Items = new Item[10];
                        for (int i = 0; i < 10; i++)
                        {
                            var prizeItems = Database.MeltingTypeTable.MeltingTypes.Where(z => z.Type == Database.MeltingTypeTable.Type.PrizePool && z.PrizeType == item.PrizeType).FirstOrDefault().Prizes;
                            var targetedItemID = prizeItems[Pool.GetRandom.Next(0, prizeItems.Count)];
                            if (Harditem.Contains(targetedItemID))
                            {
                                uint targetedItemIDs;
                                GettargetedItemID(targetedItemID, out  targetedItemIDs);
                                targetedItemID = targetedItemIDs;
                            }
                            MsgGameItem msgItem;
                            if (client.Inventory.SearchItemByID(item.ItemID, out msgItem) && client.Inventory.Remove(msgItem.ITEM_ID, 1, stream))
                            {
                                client.Inventory.AddItemWitchStack(targetedItemID, 0, 1, stream, true);
                               
                                packet.Items[i] = new Item() { ID = targetedItemID, Type = 8, Bound = msgItem.Bound != 0, AnnounceMessage = false, unk3 = 1 };
                                client.Send(stream.CreateMelterOpt(packet));
                                packet.Items[i].AnnounceMessage = true;
                                foreach (var player in Pool.GamePoll.Values)
                                {
                                    client.Send(new MsgMessage("STR_ID_tMelter[BroadCast][Result]@@" + client.Player.Name + "@@<" + Pool.ItemsBase[msgItem.ITEM_ID].Name + "@@>@@<" + Pool.ItemsBase[targetedItemID].Name + "", MsgMessage.MsgColor.white, MsgMessage.ChatMode.Melting).GetArray(stream));
                                    if (player.Player.UID != client.Player.UID)
                                        player.Send(stream.CreateMelterOpt(packet));
                                }
                                
                                Pool.MelterRankList.Add(new MsgMelterRankList.Instance() { Name = client.Player.Name, Bound = packet.Items[i].Bound, Type = 8, ItemID = targetedItemID, Unknown3 = (uint)Pool.GetRandom.Next(1559853103, 1559853332) });
                            }
                        }

                    }
                }
            }
        }
    }
}