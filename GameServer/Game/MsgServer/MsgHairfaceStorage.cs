using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgHairfaceStorage
    {
        public enum Type : byte
        {
            Hairstyle,
            Avatar,
            TexasTable,
            CardBack,
            TexasBet,
            Level,
            Map,
        }
        public enum Actions : byte
        {
            Login,
            Add,
            Equip = 3,
            Buy,
            AddColor,
            EquipColor,
            BuyColor,
            Avatars,
            HairStyles,
            UnEquip,
        }
        [ProtoContract]
        public class MsgHairfaceStorageProto
        {
            [ProtoMember(1, IsRequired = true)]
            public Actions Type;
            [ProtoMember(2)]
            public Operation Hair;
            [ProtoMember(3)]
            public Operation HairColor;
            [ProtoMember(4)]
            public Item[] AvatarsOrHairTap;
            [ProtoMember(5)]
            public Item[] Items;
        }
        [ProtoContract]
        public class Item
        {
            [ProtoMember(1, IsRequired = true)]
            public Type Type;
            [ProtoMember(2, IsRequired = true)]
            public uint ID;
            [ProtoMember(3, IsRequired = true)]
            public bool Equipped;
            [ProtoMember(4, IsRequired = true)]
            public uint Unk4;
            [ProtoMember(5, IsRequired = true)]
            public long[] Colors;
        }
        [ProtoContract]
        public class Operation
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ID;
            [ProtoMember(2, IsRequired = true)]
            public uint dwParam;
        }
        public static unsafe ServerSockets.Packet CreateHairface(this ServerSockets.Packet stream, MsgHairfaceStorageProto obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgHairFaceStorage);

            return stream;
        }
        public static unsafe void GetHairface(this ServerSockets.Packet stream, out MsgHairfaceStorageProto pQuery)
        {
            pQuery = new MsgHairfaceStorageProto();
            pQuery = stream.ProtoBufferDeserialize<MsgHairfaceStorageProto>(pQuery);
        }
        [PacketAttribute(GamePackets.MsgHairFaceStorage)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {

            MsgHairfaceStorageProto Info;
            stream.GetHairface(out Info);
            switch (Info.Type)
            {
                case Actions.Login:
                    {
                        Info.Items = new Item[client.HairfaceStorage.Objects.Count()];
                        if (Info.Items.Length > 0)
                        {
                            for (int i = 0; i < Info.Items.Length; i++)
                            {
                                Info.Items[i] = new Item();
                                Info.Items[i].ID = client.HairfaceStorage.Objects.ToArray()[i].ID;
                                Info.Items[i].Type = client.HairfaceStorage.Objects.ToArray()[i].Type;
                                Info.Items[i].Equipped = (client.HairfaceStorage.Objects.ToArray()[i].ID == client.Player.Hair % 1000 && Database.HairfaceStorageTable.Hairfaces.ToArray()[i].Type == Type.Hairstyle) || (client.Player.Face == client.HairfaceStorage.Objects.ToArray()[i].ID && Database.HairfaceStorageTable.Hairfaces.ToArray()[i].Type == Type.Avatar);
                                if (Info.Items[i].Type == Type.Hairstyle)
                                {
                                    Info.Items[i].Colors = new long[2];
                                    Info.Items[i].Colors[0] = client.HairfaceStorage.Objects.ToArray()[i].EquippedColor;
                                    string bin = "";
                                    for (byte z = 0; z < client.HairfaceStorage.Objects.ToArray()[i].Colors.Length; z++)
                                    {
                                        bin += Convert.ToString(client.HairfaceStorage.Objects.ToArray()[i].Colors[z]);
                                    }
                                    Info.Items[i].Colors[1] = Convert.ToInt64((string)(BaseFunc.ReverseString(bin)), 2);
                                }
                            }
                            client.Send(stream.CreateHairface(Info));
                        }
                        
                        break;
                    }
                case Actions.HairStyles:
                case Actions.Avatars:
                    {
                        Info.AvatarsOrHairTap = new Item[1];
                        Info.AvatarsOrHairTap[0] = new Item();
                        Info.AvatarsOrHairTap[0].Colors = new long[1];
                        client.Send(stream.CreateHairface(Info));
                        break;
                    }
                case Actions.Buy:
                    {
                        var item = Database.HairfaceStorageTable.Hairfaces.Where(i => i.ID == Info.Hair.dwParam && i.Type == (MsgHairfaceStorage.Type)Info.Hair.ID).FirstOrDefault();
                        if (item != null)
                        {
                            if (client.Player.Money >= item.Cost)
                            {
                                client.Player.Money -= item.Cost;
                                client.HairfaceStorage.Add(item);
                            }
                        }
                        break;
                    }
                case Actions.Equip:
                    {
                        var item = client.HairfaceStorage.Objects.Where(i => i.ID == Info.Hair.dwParam && i.Type == (MsgHairfaceStorage.Type)Info.Hair.ID).FirstOrDefault();
                        if (item != null)
                            client.HairfaceStorage.Equip(item);
                        break;
                    }
                case Actions.UnEquip:
                    {
                        var item = client.HairfaceStorage.Objects.Where(i => i.ID == Info.Hair.dwParam && i.Type == (MsgHairfaceStorage.Type)Info.Hair.ID).FirstOrDefault();
                        if (item != null)
                        client.HairfaceStorage.UnEquip(item);
                        break;
                    }
                case Actions.EquipColor:
                    {
                        client.HairfaceStorage.EquipColor(Info.HairColor.ID, (byte)Info.HairColor.dwParam);
                        break;
                    }
                case Actions.BuyColor:
                    {
                        var item = Database.HairfaceStorageTable.HairColors.Where(i => i.ID == Info.HairColor.ID && i.Color == Info.HairColor.dwParam).FirstOrDefault();
                        if (item != null)
                        {
                            if (client.Player.Money >= item.Cost)
                            {
                                client.Player.Money -= item.Cost;
                                client.HairfaceStorage.AddColor(item);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
