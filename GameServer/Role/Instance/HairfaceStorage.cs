using VirusX.Client;
using VirusX.Database;
using VirusX.Game.MsgServer;
using VirusX.ServerSockets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirusX.Role.Instance
{
    public class HairfaceStorage
    {
        private List<Database.HairfaceStorageType.Hairface> items;
        private Database.HairfaceStorageType.Hairface[] objects;
        private Client.GameClient Owner;
        public HairfaceStorage(Client.GameClient client)
        {
            Owner = client;
            items = new List<Database.HairfaceStorageType.Hairface>();
            objects = new Database.HairfaceStorageType.Hairface[0];
        }
        public bool Add(Database.HairfaceStorageType.Hairface item, bool Send = true)
        {
            if (item == null) return false;
            if (!Exists(item.ID, item.Type))
            {
                items.Add(item);
                objects = items.ToArray();
                if (Send)
                {
                    var obj = new MsgHairfaceStorage.MsgHairfaceStorageProto()
                    {
                        Type = MsgHairfaceStorage.Actions.Add,
                    };
                    obj.Items.Add(new MsgHairfaceStorage.Item()
                    {
                        Type = item.Type,
                        ID = item.ID,
                        Colors = new long[2] { 0, 1 }
                    });
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var streamm = rec.GetStream();
                        Owner.Send(streamm.CreateHairface(obj));
                    }
                }
                return true;
            }
            return false;
        }
        public bool AddColor(Database.HairColorType.HairColor item)
        {
            if (Exists(item.ID, MsgHairfaceStorage.Type.Hairstyle))
            {
                items.Where(i => i.ID == item.ID && i.Type == MsgHairfaceStorage.Type.Hairstyle).FirstOrDefault().Colors[item.Color] = 1;
                objects = items.ToArray();
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var streamm = rec.GetStream();
                    Owner.Send(
                           streamm.CreateHairface(
                           new MsgHairfaceStorage.MsgHairfaceStorageProto()
                           {
                               Type = MsgHairfaceStorage.Actions.AddColor,
                               HairColor = new MsgHairfaceStorage.Operation()
                               {
                                   ID = item.ID,
                                   dwParam = item.Color
                               }
                           }));
                }
                return true;
            }
            return false;
        }
        public bool CanEquip(Database.HairfaceStorageType.Hairface hair)
        {
            if (hair.Type >= MsgHairfaceStorage.Type.TexasTable)
                return true;

            if (hair.RequiredVIPLevel > Owner.Player.VipLevel)
                return false;

            if (hair.Sex != 0)
            {
                if (hair.Sex == 1 && !Role.Core.IsBoy(Owner.Player.Body))
                    return false;
                if (hair.Sex == 2 && !Role.Core.IsGirl(Owner.Player.Body))
                    return false;
            }

            if (hair.Classes.Count > 0 &&
                !hair.Classes.Contains((byte)(Owner.Player.Class / 1000)))
                return false;

            return true;
        }
        public bool Equip(Database.HairfaceStorageType.Hairface hair)
        {
            if (Exists(hair.ID, hair.Type) && CanEquip(hair))
            {
                if (hair.Type >= MsgHairfaceStorage.Type.TexasTable && hair.Type <= MsgHairfaceStorage.Type.Carpet)
                {
                    var ItemsEquped = items.Where(i => i.ID != hair.ID && i.Type == hair.Type && i.Equiped).FirstOrDefault();
                    if (ItemsEquped != null)
                        UnEquip(ItemsEquped);
                    items.Where(i => i.ID == hair.ID && i.Type == hair.Type).FirstOrDefault().Equiped = true;
                    objects = items.ToArray();
                }
                else if (hair.Type == MsgHairfaceStorage.Type.Head)
                {
                    var ItemsEquped = items.Where(i => i.ID != hair.ID && i.Type == hair.Type && i.Equiped).FirstOrDefault();
                    if (ItemsEquped != null)
                        UnEquip(ItemsEquped);
                    items.Where(i => i.ID == hair.ID && i.Type == hair.Type).FirstOrDefault().Equiped = true;
                    objects = items.ToArray();
                    Owner.Player.Head = (ushort)hair.ID;
                }
                else if (hair.Type == MsgHairfaceStorage.Type.Frame)
                {
                    var ItemsEquped = items.Where(i => i.ID != hair.ID && i.Type == hair.Type && i.Equiped).FirstOrDefault();
                    if (ItemsEquped != null)
                        UnEquip(ItemsEquped);
                    items.Where(i => i.ID == hair.ID && i.Type == hair.Type).FirstOrDefault().Equiped = true;
                    objects = items.ToArray();
                    Owner.Player.FrameID = (ushort)hair.ID;
                }
                else if (hair.Type == MsgHairfaceStorage.Type.Hairstyle)
                {
                    Owner.Player.Hair = (ushort)hair.ID;
                    Owner.Player.HairColor = hair.EquippedColor;
                }
                else Owner.Player.Face = (ushort)hair.ID;
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var streamm = rec.GetStream();
                    Owner.Send(
                         streamm.CreateHairface(
                         new MsgHairfaceStorage.MsgHairfaceStorageProto()
                         {
                             Type = MsgHairfaceStorage.Actions.Equip,
                             Hair = new MsgHairfaceStorage.Operation() { ID = (byte)hair.Type, dwParam = hair.ID }
                         }));
                }
                return true;
            }
            return false;
        }
        public bool UnEquip(Database.HairfaceStorageType.Hairface hair)
        {

            if (Exists(hair.ID, hair.Type) && CanEquip(hair))
            {
                items.Where(i => i.ID == hair.ID && i.Type == hair.Type).FirstOrDefault().Equiped = false;
                objects = items.ToArray();
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var streamm = rec.GetStream();
                    Owner.Send(
                         streamm.CreateHairface(
                         new MsgHairfaceStorage.MsgHairfaceStorageProto()
                         {
                             Type = MsgHairfaceStorage.Actions.UnEquip,
                             Hair = new MsgHairfaceStorage.Operation() { ID = (byte)hair.Type, dwParam = hair.ID }
                         }));
                }
                return true;
            }
            return false;
        }
        public bool EquipColor(uint ID, byte Color)
        {
            if (Exists(ID, MsgHairfaceStorage.Type.Hairstyle) && items.Where(i => i.ID == ID && i.Type == MsgHairfaceStorage.Type.Hairstyle).FirstOrDefault().Colors[Color] == 1 && Owner.Player.Hair % 1000 == ID)
            {
                items.Where(i => i.ID == ID && i.Type == MsgHairfaceStorage.Type.Hairstyle).FirstOrDefault().EquippedColor = Color;
                objects = items.ToArray();

                Owner.Player.HairColor = (byte)Color;
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var streamm = rec.GetStream();
                    Owner.Send(
                        streamm.CreateHairface(
                        new MsgHairfaceStorage.MsgHairfaceStorageProto()
                        {
                            Type = MsgHairfaceStorage.Actions.EquipColor,
                            HairColor = new MsgHairfaceStorage.Operation() { ID = ID, dwParam = Color }
                        }));
                }
                return true;
            }
            return false;
        }
        public bool Exists(uint ID, MsgHairfaceStorage.Type type)
        {
            return Objects.Where(i => i.ID == ID && i.Type == type).Count() > 0;
        }
        public Database.HairfaceStorageType.Hairface[] Objects
        {
            get
            {
                return objects;
            }
        }
        public bool GetFirst = false;
        public unsafe void Loading()
        {
            var obj = new MsgHairfaceStorage.MsgHairfaceStorageProto()
            {
                Type = MsgHairfaceStorage.Actions.Login,
            };
            foreach (var item in Objects)
            {
                var proto = new MsgHairfaceStorage.Item();
                proto.ID = item.ID;
                proto.Type = item.Type;
                if (item.EquippedHair == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedFace == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedTable == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedCardBack == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedBet == 1)
                {
                    proto.Equipped = true;
                }
                if (item.Equippedlevel == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedMap == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedDealer == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedCarpet == 1)
                {
                    proto.Equipped = true;
                }
                if (item.EquippedFrame == 1)
                {
                    proto.Equipped = true;
                }
                if (proto.Type == MsgHairfaceStorage.Type.Hairstyle)
                {
                    proto.Colors = new long[2];
                    proto.Colors[0] = item.EquippedColor;
                    string bin = "";
                    for (byte z = 0; z < item.Colors.Length; z++)
                    {
                        bin += System.Convert.ToString(item.Colors[z]);
                    }
                    if (item.Colors.Length < bin.Length)
                        proto.Colors[1] = System.Convert.ToInt64((string)(BaseFunc.ReverseString(bin)), 2);
                }
                items.Add(item);


                if (items.Count == 30)
                {
                    Owner.Send(new ServerSockets.RecycledPacket().GetStream().CreateHairface(obj));
                    obj = new MsgHairfaceStorage.MsgHairfaceStorageProto()
                    {
                        Type = MsgHairfaceStorage.Actions.Login,
                    };
                }
            }
            Owner.Send(new ServerSockets.RecycledPacket().GetStream().CreateHairface(obj));
        }
        public void Load(uint UID)
        {
            var iniFile = new WindowsAPI.IniFile("\\HairfaceStoragePlayer\\" + UID + ".ini");
            GetFirst = iniFile.ReadBool("Hair", "GetFirst", false);
            int count = iniFile.ReadInt32("Hair", "Count", 0);
            for (int i = 0; i < count; i++)
            {
                var item = new Database.HairfaceStorageType.Hairface();
                item.Type = (MsgHairfaceStorage.Type)iniFile.ReadUInt32("Hair" + i.ToString(), "Type", 0);
                item.ID = iniFile.ReadUInt32("Hair" + i.ToString(), "ID", 0);
                item.EquippedColor = iniFile.ReadByte("Hair" + i.ToString(), "EquippedColor", 0);
                item.EquippedHair = iniFile.ReadByte("Hair" + i.ToString(), "EquippedHair", 0);
                item.EquippedFace = iniFile.ReadByte("Hair" + i.ToString(), "EquippedFace", 0);
                item.EquippedTable = iniFile.ReadByte("Hair" + i.ToString(), "EquippedTable", 0);
                item.EquippedCardBack = iniFile.ReadByte("Hair" + i.ToString(), "EquippedCardBack", 0);
                item.EquippedBet = iniFile.ReadByte("Hair" + i.ToString(), "EquippedBet", 0);
                item.Equippedlevel = iniFile.ReadByte("Hair" + i.ToString(), "Equippedlevel", 0);
                item.EquippedMap = iniFile.ReadByte("Hair" + i.ToString(), "EquippedMap", 0);
                item.EquippedDealer = iniFile.ReadByte("Hair" + i.ToString(), "EquippedDealer", 0);
                item.EquippedCarpet = iniFile.ReadByte("Hair" + i.ToString(), "EquippedCarpet", 0);
                item.EquippedFrame = iniFile.ReadByte("Hair" + i.ToString(), "EquippedFrame", 0);
                item.Colors = new byte[7];
                for (int l = 0; l < item.Colors.Length; l++)
                {
                    item.Colors[l] = iniFile.ReadByte("Hair" + i.ToString(), "Colors" + l.ToString(), 0);
                }
                if (Database.HairfaceStorageType.Hairfaces.Where(z => z.ID == item.ID).Count() > 0)
                {
                    var baseItem = Database.HairfaceStorageType.Hairfaces.OrderByDescending(f => f.Type == item.Type).Where(o => o.ID == item.ID).FirstOrDefault();
                    item.Classes = baseItem.Classes;
                    item.Cost = baseItem.Cost;
                    item.Name = baseItem.Name;
                    item.RareLevel = baseItem.RareLevel;
                    item.RequiredVIPLevel = baseItem.RequiredVIPLevel;
                    item.Sex = baseItem.Sex;
                    Add(item, false);
                }
            }
        }
        public void Save(uint UID)
        {
            var iniFile = new WindowsAPI.IniFile("\\HairfaceStoragePlayer\\" + UID + ".ini");
            iniFile.Write<bool>("Hair", "GetFirst", GetFirst);
            iniFile.Write<int>("Hair", "Count", objects.Length);
            int count = 0;
            foreach (var item in objects)
            {
                iniFile.Write<uint>("Hair" + count.ToString(), "Type", (uint)item.Type);
                iniFile.Write<uint>("Hair" + count.ToString(), "ID", item.ID);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedColor", item.EquippedColor);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedHair", item.EquippedHair);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedFace", item.EquippedFace);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedTable", item.EquippedTable);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedCardBack", item.EquippedCardBack);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedBet", item.EquippedBet);
                iniFile.Write<uint>("Hair" + count.ToString(), "Equippedlevel", item.Equippedlevel);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedMap", item.EquippedMap);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedDealer", item.EquippedDealer);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedCarpet", item.EquippedCarpet);
                iniFile.Write<uint>("Hair" + count.ToString(), "EquippedFrame", item.EquippedFrame);
                for (int k = 0; k < item.Colors.Length; k++)
                {
                    iniFile.Write("Hair" + count, "Colors" + k, item.Colors[k]);
                }
                count++;
            }
            return;
        }
    }
}