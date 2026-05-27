using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet DetainedItemCreate(this ServerSockets.Packet stream, MsgDetainedItem item)
        {
            stream.InitWriter();
            stream.Write(item.UID);
            stream.Write(item.ItemUID);
            stream.Write(item.ItemID);
            stream.Write(item.Durability);
            stream.Write(item.MaximDurability);
            stream.Write((uint)item.Action);
            stream.Write(item.SocketProgress);//24
            stream.Write((byte)item.SocketOne);
            stream.Write((byte)item.SocketTwo);
            stream.Write((ushort)0);
            stream.Write((uint)item.Effect);
            stream.Write((byte)0);
            stream.Write(item.Plus);//37
            stream.Write(item.Bless);//38
            stream.Write((byte)(item.Bound ? 1 : 0));//39
            stream.Write((uint)item.Enchant);//40
            stream.Write((uint)item.Suspicious);//48
            stream.Write(item.Lock);//48
            stream.Write((byte)0);
            stream.Write((byte)0);
            stream.Write((uint)item.PerfectionLevel);//52
            stream.Write((uint)item.PerfectionProgres);//56
            stream.Write(item.OwnerUID);//60
            stream.Write(item.OwnerName, 32);//64
            stream.ZeroFill(4);
            stream.Write(item.Signature, 60);//96
            stream.Write(item.OwnerUID);
            stream.Write(item.OwnerName, 32);
            stream.Write(item.GainerUID);
            stream.Write(item.GainerName, 32);
            stream.Write(item.Date);
            stream.Write(item.RewardConquerPoints);
            stream.Write(item.ConquerPointsCost);
            stream.Write(item.DaysLeft);

            stream.Finalize(GamePackets.MsgDetainItemInfo);
            return stream;
        }
    }
    [StructLayout(LayoutKind.Explicit, Size = 322)]
    public unsafe struct MsgDetainedItem
    {
        public enum ContainerType : uint
        {
            DetainPage = 13369344,//0, 
            ClaimPage = 13369345,//1,
            RewardCps = 13369346 //2
        }
        [FieldOffset(0)]
        public uint UID;
        [FieldOffset(4)]
        public uint ItemUID;
        [FieldOffset(8)]
        public uint ItemID;
        [FieldOffset(12)]
        public ushort Durability;
        [FieldOffset(14)]
        public ushort MaximDurability;
        [FieldOffset(16)]
        public ContainerType Action;
        [FieldOffset(20)]
        public uint SocketProgress;
        [FieldOffset(24)]
        public Role.Flags.Gem SocketOne;
        [FieldOffset(25)]
        public Role.Flags.Gem SocketTwo;
        [FieldOffset(26)]
        public Role.Flags.ItemEffect Effect;
        [FieldOffset(30)]//32
        public byte Plus;
        [FieldOffset(31)]//33
        public byte Bless;
        [FieldOffset(32)]//34
        public bool Bound;
        [FieldOffset(33)]//35
        public byte Enchant;
        [FieldOffset(34)]//36
        public uint PlusProgres;
        [FieldOffset(38)]//40
        public ushort Suspicious;
        [FieldOffset(40)]//42
        public ushort Lock;
        [FieldOffset(42)]
        public uint OwnerUID;
        [FieldOffset(46)]
        private fixed sbyte szOwnerName[32];
        [FieldOffset(78)]
        public uint GainerUID;
        [FieldOffset(82)]
        private fixed sbyte szGainerName[32];
        [FieldOffset(114)]
        public int Date;//time when lose item
        [FieldOffset(118)]
        public int RewardConquerPoints;
        [FieldOffset(122)]
        public int ConquerPointsCost;
        [FieldOffset(126)]
        public uint DaysLeft;
        [FieldOffset(130)]
        public uint PerfectionLevel;
        [FieldOffset(134)]
        public uint PerfectionProgres;
        [FieldOffset(138)]
        private fixed sbyte _OwnerName[32];
        [FieldOffset(170)]
        private fixed sbyte _Signature[36];
        public static MsgDetainedItem Create(MsgGameItem GameItem)
        {
            var item = new MsgDetainedItem();
            item.ItemUID = GameItem.UID;
            item.ItemID = GameItem.ITEM_ID;
            item.Durability = GameItem.Durability;
            item.MaximDurability = GameItem.MaximDurability;
            item.SocketProgress = GameItem.SocketProgress;
            item.SocketOne = GameItem.SocketOne;
            item.SocketTwo = GameItem.SocketTwo;
            item.Effect = GameItem.Effect;
            item.Plus = GameItem.Plus;
            item.Bless = GameItem.Bless;
            item.Bound = GameItem.Bound == 1;
            item.Enchant = GameItem.Enchant;
            item.Suspicious = GameItem.Suspicious;
            item.Lock = GameItem.Locked;
            item.PlusProgres = GameItem.PlusProgress;
            item.PerfectionLevel = GameItem.PerfectionLevel;
            item.PerfectionProgres = GameItem.PerfectionProgress;
            item.Signature = GameItem.Signature;
            item.PerfectionOwnerName = GameItem.OwnerName;
            item.RuneEXP = GameItem.RuneEXP;
            item.AnimaItemID = GameItem.AnimaItemID;
            item.MythsoulEffect = GameItem.MythsoulEffect;
            item.MythSoulID = GameItem.MythSoulID;
            item.MythSoulProgress = GameItem.MythSoulProgress;
            item.Attribute1 = GameItem.RelicAttributes[0];
            item.Attribute2 = GameItem.RelicAttributes[1];
            item.Attribute3 = GameItem.RelicAttributes[2];
            item.Attribute4 = GameItem.RelicAttributes[3];
            item.Attribute5 = GameItem.RelicAttributes[4];
            return item;
        }
        public static MsgGameItem CopyTo(MsgDetainedItem Item)
        {
            var GameItem = new MsgGameItem();
            GameItem.Purification = MsgItemExtra.Purification.ShallowCopy(Item.Purification);
            GameItem.Refinary = MsgItemExtra.Refinery.ShallowCopy(Item.Refinary);
            GameItem.UID = Item.ItemUID;
            GameItem.ITEM_ID = Item.ItemID;
            GameItem.Bless = Item.Bless;
            GameItem.Bound = (byte)(Item.Bound ? 1 : 0);
            GameItem.Durability = Item.Durability;
            GameItem.Effect = Item.Effect;
            GameItem.Enchant = Item.Enchant;
            GameItem.MaximDurability = Item.MaximDurability;
            GameItem.Plus = Item.Plus;
            GameItem.PlusProgress = Item.PlusProgres;
            GameItem.SocketOne = Item.SocketOne;
            GameItem.SocketProgress = Item.SocketProgress;
            GameItem.SocketTwo = Item.SocketTwo;
            GameItem.Suspicious = (byte)Item.Suspicious;
            GameItem.PerfectionLevel = Item.PerfectionLevel;
            GameItem.PerfectionProgress = Item.PerfectionProgres;
            GameItem.OwnerUID = Item.OwnerUID;
            GameItem.OwnerName = Item.PerfectionOwnerName;
            GameItem.Signature = Item.Signature;
            GameItem.RuneEXP = Item.RuneEXP;
            GameItem.AnimaItemID = Item.AnimaItemID;
            GameItem.MythsoulEffect = Item.MythsoulEffect;
            GameItem.MythSoulID = Item.MythSoulID;
            GameItem.MythSoulProgress = Item.MythSoulProgress;
            GameItem.RelicAttributes = new Role.Instance.RelicAttribute[5];
            GameItem.RelicAttributes[0] = new Role.Instance.RelicAttribute(Item.Attribute1);
            GameItem.RelicAttributes[1] = new Role.Instance.RelicAttribute(Item.Attribute2);
            GameItem.RelicAttributes[2] = new Role.Instance.RelicAttribute(Item.Attribute3);
            GameItem.RelicAttributes[3] = new Role.Instance.RelicAttribute(Item.Attribute4);
            GameItem.RelicAttributes[4] = new Role.Instance.RelicAttribute(Item.Attribute5);
            return GameItem;
        }
        [FieldOffset(206)]
        public MsgItemExtra.Purification Purification;
        [FieldOffset(246)]
        public MsgItemExtra.Refinery Refinary;
        [FieldOffset(294)]
        public uint Attribute1;
        [FieldOffset(298)]
        public uint Attribute2;
        [FieldOffset(302)]
        public uint Attribute3;
        [FieldOffset(306)]
        public uint Attribute4;
        [FieldOffset(310)]
        public uint Attribute5;
        [FieldOffset(314)]
        public uint RuneEXP;
        [FieldOffset(318)]
        public uint AnimaItemID;
        [FieldOffset(322)]
        public uint MythsoulEffect;
        [FieldOffset(326)]
        public uint MythSoulID;
        [FieldOffset(330)]
        public uint MythSoulProgress;
        public unsafe string Signature
        {
            get { fixed (sbyte* bp = _Signature) { return new string(bp); } }
            set
            {
                string ip = value;
                fixed (sbyte* bp = _Signature)
                {
                    for (int i = 0; i < ip.Length; i++)
                        bp[i] = (sbyte)ip[i];
                }
            }
        }
        public unsafe string PerfectionOwnerName
        {
            get { fixed (sbyte* bp = _OwnerName) { return new string(bp); } }
            set
            {
                string ip = value;
                fixed (sbyte* bp = _OwnerName)
                {
                    for (int i = 0; i < ip.Length; i++)
                        bp[i] = (sbyte)ip[i];
                }
            }
        }
        public unsafe string OwnerName
        {
            get { fixed (sbyte* bp = szOwnerName) { return new string(bp); } }
            set
            {
                string ip = value;
                fixed (sbyte* bp = szOwnerName)
                {
                    for (int i = 0; i < ip.Length; i++)
                        bp[i] = (sbyte)ip[i];
                }
            }
        }
        public unsafe string GainerName
        {
            get { fixed (sbyte* bp = szGainerName) { return new string(bp); } }
            set
            {
                string ip = value;
                fixed (sbyte* bp = szGainerName)
                {
                    for (int i = 0; i < ip.Length; i++)
                        bp[i] = (sbyte)ip[i];
                }
            }
        }
        public unsafe Role.Instance.Inventory Send(Client.GameClient client, ServerSockets.Packet stream)
        {
            if (Action == ContainerType.RewardCps)
            {
                OwnerUID = 500;
                ItemID = 0;
                Durability = 0;
                MaximDurability = 0;
                SocketOne = Role.Flags.Gem.NoSocket;
                SocketTwo = Role.Flags.Gem.NoSocket;
                Effect = Role.Flags.ItemEffect.None;
                Plus = 0;
                Bless = 0;
                Enchant = 0;
                SocketProgress = 0;
                Bound = false;
                Lock = 0;
                client.Send(stream.DetainedItemCreate(this));
                return client.Inventory;
            }
            var item = this;
            client.Send(stream.DetainedItemCreate(this));

            if (Purification.ItemUID != 0 || Refinary.ItemUID != 0)
            {
                MsgItemExtra extra = new MsgItemExtra();
                if (Purification.InLife)
                {
                    Purification.Typ = MsgItemExtra.Typing.PurificationAdding;
                    extra.Purifications.Add(Purification);
                }
                if (Refinary.InLife)
                {
                    Refinary.Typ = MsgItemExtra.Typing.RefinaryAdding;
                    if (Refinary.EffectDuration == 0)
                        Refinary.Typ = MsgItemExtra.Typing.PermanentRefinery;
                    extra.Refinerys.Add(Refinary);
                }
                client.Send(extra.CreateArray(stream));
            }
            return client.Inventory;
        }
    }
}
