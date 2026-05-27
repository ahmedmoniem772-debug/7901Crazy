using System.Linq;
using ProtoBuf;

namespace ConquerOnline.Game.MsgServer
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
        [PacketAttribute(GamePackets.MsgMelterOpt)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            MsgMelterOptProto Info;
            stream.GetMelterOpt(out Info);
            if (Info.Type == 0)
            {
                if (Info.Items.Length > 0)
                {
                    var item = Database.MeltingTypeTable.MeltingTypes.Where(i => i.Type == Database.MeltingTypeTable.Type.MeltingItem && i.ItemID == Info.Items[0].ID).FirstOrDefault();
                    if (item != null)
                    {
                        var prizeItems = Database.MeltingTypeTable.MeltingTypes.Where(i => i.Type == Database.MeltingTypeTable.Type.PrizePool && i.PrizeType == item.PrizeType).FirstOrDefault().Prizes;
                        var targetedItemID = prizeItems[Pool.GetRandom.Next(0, prizeItems.Count)];
                        MsgGameItem msgItem;
                        if (client.Inventory.SearchItemByID(item.ItemID, out msgItem) && client.Inventory.Remove(msgItem.ITEM_ID, 1, stream))
                        {
                            client.Inventory.Add(stream, targetedItemID, 1, 0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, true);
                            var packet = new MsgMelterOptProto(){ EntityID = client.Player.UID, EntityName = client.Player.Name,Type = 0};
                            packet.Items = new Item[1];
                            packet.Items[0] = new Item(){ ID = targetedItemID, Type = 8, Bound = msgItem.Bound != 0,AnnounceMessage = false};
                            client.Send(stream.CreateMelterOpt(packet));
                            packet.Items[0].AnnounceMessage = true;
                            foreach (var player in Pool.GamePoll.Values)
                                if (player.Player.UID != client.Player.UID)
                                    player.Send(stream.CreateMelterOpt(packet));
                            Pool.MelterRankList.Add(new MsgMelterRankList.Instance() { Name = client.Player.Name, Bound = packet.Items[0].Bound, Type = 8, ItemID = targetedItemID, Unknown3 = (uint)Pool.GetRandom.Next(1559853103, 1559853332) });
                        }
                    }
                }
            }
            else if (Info.Type == 1)
            {
                if (Info.Items.Length > 0)
                {
                    var item = Database.MeltingTypeTable.MeltingTypes.Where(i => i.Type == Database.MeltingTypeTable.Type.MeltingItem && i.ItemID == Info.Items[0].ID).FirstOrDefault();
                    if (item != null)
                    {
                        var packet = new MsgMelterOptProto(){EntityID = client.Player.UID,EntityName = client.Player.Name,Type = 1};
                        packet.Items = new Item[10];
                        for (int i = 0; i < 10; i++)
                        {
                            var prizeItems = Database.MeltingTypeTable.MeltingTypes.Where(z => z.Type == Database.MeltingTypeTable.Type.PrizePool && z.PrizeType == item.PrizeType).FirstOrDefault().Prizes;
                            var targetedItemID = prizeItems[Pool.GetRandom.Next(0, prizeItems.Count)];
                            MsgGameItem msgItem;
                            if (client.Inventory.SearchItemByID(item.ItemID, out msgItem) && client.Inventory.Remove(msgItem.ITEM_ID, 1, stream))
                            {
                                client.Inventory.Add(stream, targetedItemID, 1, 0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, true);
                                packet.Items[i] = new Item() { ID = targetedItemID, Type = 8, Bound = msgItem.Bound != 0, AnnounceMessage = false };
                                client.Send(stream.CreateMelterOpt(packet));
                                packet.Items[i].AnnounceMessage = true;
                                foreach (var player in Pool.GamePoll.Values)
                                    if (player.Player.UID != client.Player.UID)
                                        player.Send(stream.CreateMelterOpt(packet));
                                Pool.MelterRankList.Add(new MsgMelterRankList.Instance() { Name = client.Player.Name, Bound = packet.Items[i].Bound, Type = 8, ItemID = targetedItemID, Unknown3 = (uint)Pool.GetRandom.Next(1559853103, 1559853332) });
                            }
                        }
                    }
                }
            }
        }
    }
}