using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {

        public static void GetItemPacketPacket(this ServerSockets.Packet stream, out MsgGameItem item)
        {
            item = new MsgGameItem();
            item.UID = stream.ReadUInt32();//4
            item.ITEM_ID = stream.ReadUInt32();//8
            item.Durability = stream.ReadUInt16();//12
            item.MaximDurability = stream.ReadUInt16();//14
            item.Mode = (Role.Flags.ItemMode)stream.ReadUInt16();//16
            item.Position = stream.ReadUInt8();//18
            item.SocketProgress = stream.ReadUInt32();//19
            item.SocketOne = (Role.Flags.Gem)stream.ReadUInt8();//23
            item.SocketTwo = (Role.Flags.Gem)stream.ReadUInt8();//24
            if (Database.ItemType.isRune(item.ITEM_ID))
            {
                item.SpellID = stream.ReadUInt32();//25
                item.RuneView = stream.ReadUInt8();//29
            }
            else
            {
                item.Effect = (Role.Flags.ItemEffect)stream.ReadUInt16();//25
                stream.SeekForward(sizeof(ushort) + sizeof(byte));//27
            }
            item.Plus = stream.ReadUInt8();//30
            item.Bless = stream.ReadUInt8();//31
            item.Bound = stream.ReadUInt8();//32
            item.Enchant = stream.ReadUInt8();//33
            item.ProgresGreen = stream.ReadUInt8();//34
            stream.SeekForward(sizeof(ushort) + 1);//35
            item.Suspicious = stream.ReadUInt8();//38
            stream.SeekForward(sizeof(byte));//39
            item.Locked = stream.ReadUInt8();//40
            stream.SeekForward(sizeof(byte));//41
            item.PlusProgress = stream.ReadUInt32();//42
            stream.ReadUInt32();//46 RemainingTime
            stream.ReadUInt32();//50 Unknow
            item.StackSize = stream.ReadUInt16();//54
            item.PerfectionLevel = stream.ReadUInt32();//56
            item.PerfectionProgress = stream.ReadUInt32();//60
            item.OwnerUID = stream.ReadUInt32();//64
            item.OwnerName = stream.ReadCString(32);//68
            item.Signature = stream.ReadCString(64);//100
            stream.SeekForward(sizeof(ushort));//164
            item.RuneEXP = stream.ReadUInt32();//166
            if (item.RelicAttributes != null)
                for (int i = 0; i < item.RelicAttributes.Length; i++)
                    item.RelicAttributes[i] = new Role.Instance.RelicAttribute(stream.ReadUInt32());//170 174 178 182 186
            else
                stream.SeekForward(sizeof(ulong) * 3);
            item.AnimaItemID = stream.ReadUInt32();//190
            stream.ReadUInt32();
            item.MythSoulID = stream.ReadUInt32();
            item.MythSoulProgress = stream.ReadUInt32();
            stream.ReadUInt32();
            stream.ReadUInt32();
            item.Mutacion = stream.ReadUInt32();
           
        }
    }
    public class MsgGameItem
    {
        public Database.ItemType.DBItem DBInfo
        {
            get
            {
                if (Pool.ItemsBase.ContainsKey(ITEM_ID))
                    return Pool.ItemsBase[ITEM_ID];
                return new Database.ItemType.DBItem();
            }
        }
        public string ToLog()
        {
            return "Name: " + Pool.ItemsBase[ITEM_ID].Name + ": UID: " + UID.ToString() + " | " + "ID: " + ITEM_ID.ToString() + " | " + "Plus: " + Plus.ToString() + " | " + "Bless: " + Bless.ToString() + " | " + "Anima: " + AnimaItemID.ToString() + " | " + "Perfection Level: " + PerfectionLevel.ToString() + " | " + "S1: " + SocketOne.ToString() + " | " + "S2: " + SocketTwo.ToString();
        }
        public MsgItemExtra.Purification Purification;
        public MsgItemExtra.Refinery Refinary;

        public ConcurrentDictionary<uint, MsgGameItem> Deposite = new ConcurrentDictionary<uint, MsgGameItem>();



        public void Send(MsgInterServer.PipeClient user, ServerSockets.Packet stream)
        {
            user.Send(ItemCreate(stream, this));
            if (Purification.ItemUID != 0 || Refinary.ItemUID != 0)
            {
                MsgItemExtra extra = new MsgItemExtra();
                if (Purification.InLife)
                {
                    if (Purification.SecoundsLeft == 0)
                        Purification.Typ = MsgItemExtra.Typing.Stabilization;
                    else
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
                user.Send(extra.CreateArray(stream, true));
            }
        }
        public static Counter ItemUID = new Counter(0);
        public unsafe Role.Instance.Inventory Send(Client.GameClient client, ServerSockets.Packet stream)
        {
            if (Mode == Role.Flags.ItemMode.Update)
            {
                Database.ServerDatabase.LoginQueue.Enqueue("[Item] Player Name: " + client.Player.Name + " " + ToLog());
            }
            if (MaximDurability == 0)
            {
                Database.ItemType.DBItem DBItem;
                if (Pool.ItemsBase.TryGetValue(ITEM_ID, out DBItem))
                    MaximDurability = DBItem.Durability;
            }
            ushort position = Database.ItemType.ItemPosition(ITEM_ID);
            if (position == (ushort)Role.Flags.ConquerItem.RightWeaponAccessory || position == (ushort)Role.Flags.ConquerItem.LeftWeaponAccessory)
            {
                Activate = 1;
                TimeLeftInMinutes = uint.MaxValue;
            }
            if (Plus > 0)
            {
                if (position == 0)
                    Plus = 0;
            }
            if (ITEM_ID >= 730001 && ITEM_ID <= 730008)
                Plus = (byte)(ITEM_ID % 10);
            Database.ItemType.DBItem DBItems;

            if (Pool.ItemsBase.TryGetValue(ITEM_ID, out DBItems))
                RuneView = DBItems.RuneView;

            client.Send(ItemCreate(stream, this));
            SendItemExtra(client, stream);
            SendItemLocked(client, stream);

            return client.Inventory;
        }
        public void SendItemExtra(Client.GameClient client, ServerSockets.Packet stream)
        {
            
            if (Purification.ItemUID != 0 || Refinary.ItemUID != 0)
            {
                MsgItemExtra extra = new MsgItemExtra();
                if (Purification.InLife)
                {
                    if (Purification.SecoundsLeft == 0)
                        Purification.Typ = MsgItemExtra.Typing.Stabilization;
                    else
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

        }
        public void SendItemLocked(Client.GameClient client, ServerSockets.Packet stream)
        {
            if (Locked == 2)
            {
                if (client.Player.OnMyOwnServer)
                {
                    if (UnLockTimer == 0)
                    {
                        Locked = 0;
                        Mode = Role.Flags.ItemMode.Update;
                        client.Send(ItemCreate(stream, this));
                    }
                    else
                    {
                        if (DateTime.Now > Role.Core.GetTimer(UnLockTimer))
                        {
                            Locked = 0;
                            Mode = Role.Flags.ItemMode.Update;
                            client.Send(ItemCreate(stream, this));
                        }
                        else
                        {
                            client.Send(stream.ItemLockCreate(UID, MsgItemLock.TypeLock.UnlockDate, 0, (uint)UnLockTimer));
                        }
                    }
                }
            }
        }
        public DateTime TimeStamp;
        public bool Delete { get; set; }
        public ServerSockets.Packet ItemCreate(ServerSockets.Packet stream, MsgGameItem item)
        {
            stream.InitWriter();
            stream.Write(item.UID);//4
            stream.Write(item.ITEM_ID);//8
            stream.Write(item.Durability);//12
            stream.Write(item.MaximDurability);//14
            stream.Write((ushort)item.Mode);//16
            stream.Write((byte)item.Position);//18
            stream.Write(item.SocketProgress);//19
            stream.Write((byte)item.SocketOne);//23
            stream.Write((byte)item.SocketTwo);//24
            if (Database.ItemType.isRune(ITEM_ID))
            {
                stream.Write(item.SpellID);//25
                stream.Write(item.RuneView);//29
            }
            else
            {
                stream.Write((ushort)item.Effect);//25
                stream.ZeroFill(sizeof(ushort) + sizeof(byte));
            }
            stream.Write(item.Plus);//30
            stream.Write(item.Bless);//31
            stream.Write(item.Bound);//32
            stream.Write((byte)item.Enchant);//33
            stream.Write(item.ProgresGreen);//34
            stream.Write(item.Suspicious);//38
            stream.ZeroFill(sizeof(byte));//39
            stream.Write(item.Locked);//40
            stream.ZeroFill(sizeof(byte));//41
            if (!Database.ItemType.isRune(item.ITEM_ID))
                stream.Write(item.PlusProgress);//42
            else
                stream.Write(0);
            if (!Database.ItemType.Relic.Contains(item.ITEM_ID))
            {
                if (item.Activate == 1 && item.TimeLeftInMinutes > 0 && item.TimeLeftInMinutes < uint.MaxValue)
                {
                    stream.Write(item.TimeLeftInMinutes);//46 TimeIn
                }
                else
                    stream.Write(item.TimeLeftInMinutes);//uint.MaxValue);//active 
            }
            else
                stream.Write(0);
            stream.Write(0);//50
            stream.Write(item.StackSize);//54
            stream.Write(item.PerfectionLevel);//56
            stream.Write(item.PerfectionProgress);//60

            stream.Write(item.OwnerUID);//64
            stream.Write(item.OwnerName, 32);//68
            stream.Write(item.Signature, 64);//100
            stream.ZeroFill(sizeof(ushort));//164
            if (Database.ItemType.isRune(item.ITEM_ID))
                stream.Write(item.RuneEXP);//166   
            else
                stream.Write(0);
            if (item.RelicAttributes != null)
                for (int i = 0; i < item.RelicAttributes.Length; i++)
                    stream.Write(item.RelicAttributes[i]);
            else
                stream.ZeroFill(sizeof(ulong) * 3);
            stream.Write(item.AnimaItemID);//190   
            stream.Write(0);//194 Null
            stream.Write(item.MythSoulID);//198
            stream.Write(item.MythSoulProgress);//202
            stream.Write(0);
            stream.Write(0);
            stream.Write(item.Mutacion);
            stream.Finalize(GamePackets.Item);
            return stream;
        }


        public bool IsWeapon
        {
            get
            {
                return (Database.ItemType.ItemPosition(ITEM_ID) == (ushort)Role.Flags.ConquerItem.RightWeapon
                    || Database.ItemType.ItemPosition(ITEM_ID) == (ushort)Role.Flags.ConquerItem.LeftWeapon) && !Database.ItemType.IsArrow(ITEM_ID);
            }
        }
        public bool IsEquip
        {
            get
            {
                return Database.ItemType.ItemPosition(ITEM_ID) != 0;
            }
        }
        public uint ItemPoints;
        public int GetPerfectionPosition
        {
            get
            {
                ushort DBPosotion = Database.ItemType.ItemPosition(ITEM_ID);
                if (DBPosotion <= 5)
                    return DBPosotion;
                if (Database.ItemType.IsShield(ITEM_ID))
                    return 4;
                switch (DBPosotion)
                {
                    case (ushort)Role.Flags.ConquerItem.Wing:
                        return 5;
                    case (ushort)Role.Flags.ConquerItem.Ring:
                        return 6;
                    case (ushort)Role.Flags.ConquerItem.RidingCrop:
                        return 7;
                    case (ushort)Role.Flags.ConquerItem.Boots:
                        return 8;
                    case (ushort)Role.Flags.ConquerItem.Steed:
                        return 9;
                    case (ushort)Role.Flags.ConquerItem.Fan:
                        return 10;
                    case (ushort)Role.Flags.ConquerItem.Tower:
                        return 11;
                }
                return -1;

            }
        }
        public uint PerfectionStage
        {
            get
            {
                return PerfectionLevel / 9;
            }
        }
        public uint PerfectionStageStars
        {
            get
            {
                return PerfectionLevel % 9;
            }
        }
        public uint GetPrestigeScore
        {
            get
            {
                return ItemPoints;
            }
        }
        public bool IsTwoHander()
        {
            uint item_type = ITEM_ID / 1000;
            bool check = ((UInt16)item_type >= 500 && (UInt16)item_type <= 580);
            if (check)
            {
                check = (item_type != 900);
                if (check)
                {
                    check = (item_type != 601);
                    if (check)
                    {
                        check = (item_type != 610);
                    }
                }
            }
            return check;
        }

        public ushort Leng;
        public ushort PacketID;
        public uint UID;
        private uint _id;
        public uint ITEM_ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if (Database.ItemType.isRune(value))
                {
                    if (Pool.ItemsBase[value].RuneSpellID == value)
                    {
                        SpellID = Pool.ItemsBase[value].RuneSpellID - Pool.ItemsBase[value].RuneSpellID % 100 + 1;
                        Effect = Role.Flags.ItemEffect.RuneEffect;
                    }
                    else
                    {
                        SpellID = Pool.ItemsBase[value].RuneSpellID;
                        Effect = Role.Flags.ItemEffect.None;
                    }
                }
            }
        }
        public ushort Durability;
        public ushort MaximDurability;
        public Role.Flags.ItemMode Mode;
        public ushort Position;
        public uint SocketProgress;
        public uint RemainingTime2;
        public uint RemainingTime;
        public Role.Flags.Gem SocketOne;
        public Role.Flags.Gem SocketTwo;
        public uint SpellID;
        public Role.Flags.ItemEffect Effect;
        public byte Plus;
        public byte Bless;
        public byte Bound;
        public byte Enchant;//36 // Steed  -> ProgresBlue 
        public uint ProgresGreen;//39 // for steed
        public byte Suspicious;
        public byte Locked;
        public Role.Flags.Color Color;
        public uint PlusProgress;//52
        public ushort Inscribed;
        public uint Activate;
        public uint TimeLeftInMinutes;//64
        public ushort StackSize;//68
        public ushort UnKnow;
        public uint WH_ID;

        public uint OwnerUID = 0;
        public uint PerfectionLevel = 0;
        public ushort PerfectionRank = 0;
        public uint PerfectionProgress = 0;
        public string OwnerName = "";
        public string Signature = "";
        public DateTime EndDate = DateTime.FromBinary(0);
        public uint RuneEXP;
        public Role.Instance.RelicAttribute[] RelicAttributes = new Role.Instance.RelicAttribute[5];
        public uint AnimaItemID;
        public uint MythSoulID;
        public uint MythSoulProgress;
        public uint MutacionID2;
        public uint Mutacion;
        public int UnLockTimer;
        public byte RuneView;
        public Dictionary<uint, string> Agate_map { get; set; }
        public MsgGameItem()
        {
            this.Agate_map = new Dictionary<uint, string>(10);
        }

        public MsgGameItem FakeChatItem(uint ItemID, ushort Durability)
        {
            Game.MsgServer.MsgGameItem GameItem = new Game.MsgServer.MsgGameItem();
            GameItem.UID = 1;
            GameItem.Durability = GameItem.MaximDurability = Durability;
            GameItem.ITEM_ID = ItemID;
            GameItem.Mode = Role.Flags.ItemMode.ChatItem;
            return GameItem;
        }
        public void SendAgate(Client.GameClient client)
        {
            using (ServerSockets.RecycledPacket rec = new ServerSockets.RecycledPacket())
            {
                ServerSockets.Packet stream = rec.GetStream();
                stream.InitWriter();
                stream.Write(0U);
                stream.Write(this.UID);
                stream.Write((uint)this.Agate_map.Count);
                stream.Write((ulong)this.Agate_map.Count);
                stream.Write((uint)this.Durability);
                stream.Write((uint)this.Agate_map.Count);
                if (this.Agate_map.Count > 0)
                {
                    for (uint key = 0; (long)key < (long)this.Agate_map.Count; ++key)
                    {
                        stream.Write(key);
                        stream.Write(uint.Parse(this.Agate_map[key].Split(new char[1]
            {
              '~'
            })[0].ToString()));
                        stream.Write(uint.Parse(this.Agate_map[key].Split(new char[1]
            {
              '~'
            })[1].ToString()));
                        stream.Write(uint.Parse(this.Agate_map[key].Split(new char[1]
            {
              '~'
            })[2].ToString()));
                        stream.ZeroFill(32);
                    }
                }
                stream.Finalize(GamePackets.MsgSuperFlag);
                client.Send(stream);
            }
        }
      
       
    }
}
