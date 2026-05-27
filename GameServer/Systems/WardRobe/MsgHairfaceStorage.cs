using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public static class MsgHairfaceStorage
    {
        public enum Type : byte
        {
            Hairstyle = 0,
            Avatar = 1,
            TexasTable = 2,
            CardBack = 3,
            TexasBet = 4,
            Level = 5,
            Map = 6,
            Dealer = 7,
            Carpet = 8,
            Frame = 9,
            Emoji = 10,
            Head = 11,
            ChatFrame = 12,
        }
        public enum Actions : byte
        {
            Login = 0,
            Add = 1,
            Equip = 3,
            Buy = 4,
            AddColor = 5,
            EquipColor = 6,
            BuyColor = 7,
            Avatars = 8,
            HairStyles = 9,
            UnEquip = 10,
            Show = 11,
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
            public List<Item> Items = new List<Item>();
            [ProtoMember(6)]
            public uint PlayerUID;
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
            [ProtoMember(5)]
            public long[] Colors;
            [ProtoMember(6, IsRequired = true)]
            public uint Unkown6;
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
                        var obj = new MsgHairfaceStorageProto()
                        {
                            Type = Actions.Login,
                            PlayerUID = client.Player.UID,
                        };
                        foreach (var item in client.HairfaceStorage.Objects)
                        {
                            var proto = new Item();
                            proto.ID = item.ID;
                            proto.Type = item.Type;
                            if (item.ID == client.Player.Hair % 1000 && item.Type == MsgHairfaceStorage.Type.Hairstyle)
                            {
                                proto.Equipped = true;
                            }
                            else if (item.ID == client.Player.Face && item.Type == MsgHairfaceStorage.Type.Avatar)
                            {
                                proto.Equipped = true;
                            }
                            else if (item.Equiped == true)
                            {
                                proto.Equipped = true;
                            }
                            else
                            {
                                proto.Equipped = false;
                            }

                            if (proto.Type == Type.Hairstyle)
                            {
                                proto.Colors = new long[2];
                                proto.Colors[0] = item.EquippedColor;
                                string bin = "";
                                for (byte z = 0; z < item.Colors.Length; z++)
                                {
                                    bin += Convert.ToString(item.Colors[z]);
                                }
                                proto.Colors[1] = Convert.ToInt64((string)(BaseFunc.ReverseString(bin)), 2);
                            }
                            obj.Items.Add(proto);
                            if (obj.Items.Count == 30)
                            {
                                client.Send(stream.CreateHairface(obj));
                                obj = new MsgHairfaceStorageProto()
                                {
                                    Type = Actions.Login,
                                    PlayerUID = client.Player.UID
                                };
                            }
                        }
                        client.Send(stream.CreateHairface(obj));
                        break;
                    }
                case Actions.HairStyles:
                case Actions.Avatars:
                    {
                        Info.AvatarsOrHairTap = new Item[1];
                        Info.AvatarsOrHairTap[0] = new Item();
                        Info.AvatarsOrHairTap[0].Colors = new long[1] { client.Player.VipLevel };
                        client.Send(stream.CreateHairface(Info));
                        break;
                    }
                case Actions.Buy:
                    {
                        var item = Database.HairfaceStorageType.Hairfaces.Where(i => i.ID == Info.Hair.dwParam && i.Type == (MsgHairfaceStorage.Type)Info.Hair.ID).FirstOrDefault();
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
                        {
                            client.HairfaceStorage.Equip(item);
                        }

                        break;
                    }
                case Actions.EquipColor:
                    {
                        client.HairfaceStorage.EquipColor(Info.HairColor.ID, (byte)Info.HairColor.dwParam);
                        break;
                    }
                case Actions.UnEquip:
                    {
                        var item = client.HairfaceStorage.Objects.Where(i => i.ID == Info.Hair.dwParam && i.Type == (MsgHairfaceStorage.Type)Info.Hair.ID).FirstOrDefault();
                        if (item != null)
                        {
                            client.HairfaceStorage.UnEquip(item);
                        }

                        break;
                    }
                case Actions.BuyColor:
                    {
                        var item = Database.HairColorType.HairColors.Where(i => i.ID == Info.HairColor.ID && i.Color == Info.HairColor.dwParam).FirstOrDefault();
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
                case Actions.Show:
                    {
                        Client.GameClient user;
                        if (Pool.GamePoll.TryGetValue(Info.PlayerUID, out user))
                        {
                            try
                            {

                                Info.Type = Actions.Login;
                                Info.PlayerUID = user.Player.UID;

                                Info.Hair = new Operation();
                                Info.Hair.ID = user.Player.Hair;
                                Info.Hair.dwParam = 0;
                                ///////////////////////////////////////////////////////
                                Info.AvatarsOrHairTap = new Item[1];
                                Info.AvatarsOrHairTap[0] = new Item();
                                Info.AvatarsOrHairTap[0].Colors = new long[1] { user.Player.VipLevel = 6 };
                                Info.AvatarsOrHairTap[0].Unk4 = 0;
                                /////////////////////////////////////////////////////////////
                                Info.HairColor = new Operation();
                                Info.HairColor.ID = user.Player.Hair;
                                Info.HairColor.dwParam = 0;
                                /////////////////////////////////////////////////////


                                Info.Items = new List<Item>();
                                var TexasStyle = client.HairfaceStorage.Objects.Where(i => i.Type >= MsgHairfaceStorage.Type.Hairstyle && i.Type <= MsgHairfaceStorage.Type.Frame).ToArray();
                                var lists = new GenericSplit().Lists<Database.HairfaceStorageType.Hairface>(60, TexasStyle.ToArray());
                                for (int i = 0; i < Info.Items.Count; i++)
                                {
                                    Info.Items[i] = new Item();
                                    Info.Items[i].ID = user.HairfaceStorage.Objects.ToArray()[i].ID;
                                    Info.Items[i].Type = user.HairfaceStorage.Objects.ToArray()[i].Type;
                                    Info.Items[i].Equipped = (user.HairfaceStorage.Objects.ToArray()[i].ID == user.Player.Hair % 1000 && Database.HairfaceStorageType.Hairfaces.ToArray()[i].Type == Type.Hairstyle) || (user.Player.Face == user.HairfaceStorage.Objects.ToArray()[i].ID && Database.HairfaceStorageType.Hairfaces.ToArray()[i].Type == Type.Avatar);
                                    if (Info.Items[i].Type == Type.Hairstyle)
                                    {
                                        Info.Items[i].Colors = new long[2];
                                        Info.Items[i].Colors[0] = user.HairfaceStorage.Objects.ToArray()[i].EquippedColor;
                                        string bin = "";
                                        for (byte z = 0; z < user.HairfaceStorage.Objects.ToArray()[i].Colors.Length; z++)
                                        {
                                            bin += Convert.ToString(user.HairfaceStorage.Objects.ToArray()[i].Colors[z]);
                                        }
                                        if (bin != "")
                                        {
                                            Info.Items[i].Colors[1] = Convert.ToInt64((string)(BaseFunc.ReverseString(bin)), 2);
                                        }
                                    }

                                }
                                client.Send(stream.CreateHairface(Info));

                            }
                            catch (Exception EX)
                            {
                                MyConsole.WriteException(EX);
                            }
                        }
                        break;
                    }
            }
        }
    }
}