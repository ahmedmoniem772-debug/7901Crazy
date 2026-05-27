using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.IO;

namespace VirusX.Database
{
    public class ClientItems
    {
        public struct Perfection
        {
            public uint ItemUID;
            public uint Level;
            public uint Progres;
            public uint OwnerUID;
            public unsafe fixed sbyte _OwnerName[32];
            public unsafe fixed sbyte _SpecialText[32];

            public unsafe string OwnerName
            {
                get { fixed (sbyte* bp = _OwnerName) { return new string(bp); } }
                set
                {
                    string text = value;
                    fixed (sbyte* bp = _OwnerName)
                    {
                        for (int i = 0; i < text.Length; i++)
                            bp[i] = (sbyte)text[i];
                    }
                }
            }
            public unsafe string SpecialText
            {
                get { fixed (sbyte* bp = _SpecialText) { return new string(bp); } }
                set
                {
                    string text = value;
                    fixed (sbyte* bp = _SpecialText)
                    {
                        for (int i = 0; i < text.Length; i++)
                            bp[i] = (sbyte)text[i];
                    }
                }
            }
        }
        public struct DBItem
        {
            public uint UID;
            public uint ITEM_ID;
            public ushort Durability;
            public ushort MaximDurability;
            public ushort Position;
            public uint SocketProgress;
            public Role.Flags.Gem SocketOne;
            public Role.Flags.Gem SocketTwo;
            public Role.Flags.ItemEffect Effect;
            public byte Plus;
            public byte Bless;
            public byte Bound;
            public ushort Enchant;
            public byte Suspicious;
            public byte Locked;
            public uint PlusProgress;
            public uint Activate;
            public ushort StackSize;
            public uint WH_ID;
            public Role.Flags.Color Color;
            public int UnLockTimer;
            public uint ItemPoints;
            public uint TimeLeftInMinutes;
            public uint RemainingTime;
            //Artefacts 
            public uint PurificationItemID;
            public uint PurificationLevel;
            public uint PurificationDuration;
            public long PurificationAddedOn;
            //Refinary
            public uint EffectID;
            public uint EffectLevel;
            public uint EffectPercent;
            public uint EffectPercent2;
            public uint EffectDuration;
            public long EffectAddedOn;
            public long Expiration;

            public uint AnimaItemID;
            public uint RuneEXP;
            public uint SpellID;
            public byte RuneView;
            public uint MythsoulEffect;
            public uint MythSoul;
            public uint MythSoulPoints;
            public uint Attribute1;
            public uint Attribute2;
            public uint Attribute3;
            public uint Attribute4;
            public uint Attribute5;
            public long Timer;
            public uint ItemUID;
            public uint Level;
            public uint Progres;
            public uint OwnerUID;
            public uint PerfectionLevel;
            public uint PerfectionProgress;

            public unsafe fixed sbyte _OwnerName[32];
            public unsafe fixed sbyte _SpecialText[64];
            public unsafe string OwnerName
            {
                get { fixed (sbyte* bp = _OwnerName) { return new string(bp); } }
                set
                {
                    string text = value;
                    fixed (sbyte* bp = _OwnerName)
                    {
                        for (int i = 0; i < text.Length; i++)
                            bp[i] = (sbyte)text[i];
                    }
                }
            }
            public unsafe string SpecialText
            {
                get { fixed (sbyte* bp = _SpecialText) { return new string(bp); } }
                set
                {
                    string text = value;
                    fixed (sbyte* bp = _SpecialText)
                    {
                        for (int i = 0; i < text.Length; i++)
                            bp[i] = (sbyte)text[i];
                    }
                }
            }
            public uint DepositeCount;
            public uint Mutacion;
            public uint Mutacion2;
            public uint YuanshenIndex;
            public ulong YuanshenTime;
            public Perfection GetPerfectionInfo(Game.MsgServer.MsgGameItem DataItem)
            {
                Perfection info = new Perfection();
                info.ItemUID = DataItem.UID;
                info.Level = DataItem.PerfectionLevel;
                info.Progres = DataItem.PerfectionProgress;
                info.OwnerUID = DataItem.OwnerUID;
                info.OwnerName = DataItem.OwnerName;
                info.SpecialText = DataItem.Signature;
                return info;
            }
            public DBItem GetDBItem(Game.MsgServer.MsgGameItem DataItem)
            {
                UID = DataItem.UID;
                ITEM_ID = DataItem.ITEM_ID;
                Durability = DataItem.Durability;
                MaximDurability = DataItem.MaximDurability;
                Position = DataItem.Position;
                SocketProgress = DataItem.SocketProgress;
                SocketOne = DataItem.SocketOne;
                SocketTwo = DataItem.SocketTwo;
                Effect = DataItem.Effect;
                Plus = DataItem.Plus;
                Bless = DataItem.Bless;
                Bound = DataItem.Bound;
                Enchant = DataItem.Enchant;
                Suspicious = DataItem.Suspicious;
                Locked = DataItem.Locked;
                PlusProgress = DataItem.PlusProgress;
                Activate = DataItem.Activate;
                StackSize = DataItem.StackSize;
                WH_ID = DataItem.WH_ID;
                Color = DataItem.Color;
                ItemPoints = DataItem.ItemPoints;
                PurificationItemID = DataItem.Purification.PurificationItemID;
                PurificationAddedOn = DataItem.Purification.AddedOn.Ticks;
                PurificationDuration = DataItem.Purification.PurificationDuration;
                PurificationLevel = DataItem.Purification.PurificationLevel;
                EffectAddedOn = DataItem.Refinary.AddedOn.Ticks;
                EffectDuration = DataItem.Refinary.EffectDuration;
                EffectID = DataItem.Refinary.EffectID;
                EffectLevel = DataItem.Refinary.EffectLevel;
                EffectPercent = DataItem.Refinary.EffectPercent;
                EffectPercent2 = DataItem.Refinary.EffectPercent2;

                UnLockTimer = DataItem.UnLockTimer;
                RemainingTime = DataItem.RemainingTime;
                Expiration = DataItem.EndDate == DateTime.FromBinary(0) ? 0 : DataItem.EndDate.ToBinary();
                AnimaItemID = DataItem.AnimaItemID;
                RuneEXP = DataItem.RuneEXP;
                SpellID = DataItem.SpellID;
                RuneView = DataItem.RuneView;
                MythsoulEffect = DataItem.MythsoulEffect;
                MythSoul = DataItem.MythSoulID;
                MythSoulPoints = DataItem.MythSoulProgress;
                if (DataItem.RelicAttributes != null)
                {
                    Attribute1 = DataItem.RelicAttributes[0];
                    Attribute2 = DataItem.RelicAttributes[1];
                    Attribute3 = DataItem.RelicAttributes[2];
                    Attribute4 = DataItem.RelicAttributes[3];
                    Attribute5 = DataItem.RelicAttributes[4];
                }
                this.Mutacion = DataItem.Mutacion;

                PerfectionLevel = DataItem.PerfectionLevel;
                PerfectionProgress = DataItem.PerfectionProgress;
                OwnerUID = DataItem.OwnerUID;
                OwnerName = DataItem.OwnerName;
                SpecialText = DataItem.Signature;
                Mutacion2 = DataItem.Mutacion2;

                YuanshenIndex = DataItem.YuanshenIndex;
                YuanshenTime = DataItem.YuanshenTime;
                return this;
            }
            public Game.MsgServer.MsgGameItem GetDataItem()
            {
                var DataItem = new Game.MsgServer.MsgGameItem();
                DataItem.UID = UID;
                DataItem.ITEM_ID = ITEM_ID;
                DataItem.Durability = Durability;
                DataItem.MaximDurability = MaximDurability;
                DataItem.Position = Position;
                DataItem.SocketProgress = SocketProgress;
                DataItem.SocketOne = SocketOne;
                DataItem.SocketTwo = SocketTwo;
                DataItem.Effect = Effect;
                if (Plus > 12) Plus = 0;//do
                DataItem.Plus = Plus;
                if (Bless > 7) Bless = 0;//do
                DataItem.Bless = Bless;
                DataItem.Bound = Bound;
                DataItem.Enchant = (byte)Enchant;
                DataItem.Suspicious = Suspicious;
                DataItem.Locked = Locked;
                DataItem.PlusProgress = PlusProgress;
                DataItem.Activate = Activate;
                DataItem.TimeLeftInMinutes = this.TimeLeftInMinutes;
                DataItem.StackSize = StackSize;
                DataItem.ItemPoints = ItemPoints;
                DataItem.UnLockTimer = UnLockTimer;
                DataItem.WH_ID = WH_ID;
                DataItem.Color = Color;
                DataItem.Purification.ItemUID = UID;
                DataItem.Purification.PurificationItemID = PurificationItemID;
                DataItem.Purification.PurificationLevel = PurificationLevel;
                DataItem.Purification.PurificationDuration = PurificationDuration;
                DataItem.Purification.AddedOn = DateTime.FromBinary(PurificationAddedOn);
                if (!DataItem.Purification.InLife)
                    DataItem.Purification = new Game.MsgServer.MsgItemExtra.Purification();
                DataItem.Refinary.ItemUID = UID;
                DataItem.Refinary.EffectID = EffectID;
                DataItem.Refinary.EffectLevel = EffectLevel;
                DataItem.Refinary.EffectPercent = EffectPercent;
                DataItem.Refinary.EffectPercent2 = EffectPercent2;
                DataItem.Refinary.EffectDuration = EffectDuration;
                DataItem.Refinary.AddedOn = DateTime.FromBinary(EffectAddedOn);
                DataItem.EndDate = DateTime.FromBinary(Expiration);
                if (!DataItem.Refinary.InLife)
                    DataItem.Refinary = new Game.MsgServer.MsgItemExtra.Refinery();
                DataItem.AnimaItemID = AnimaItemID;
                DataItem.RuneEXP = RuneEXP;
                DataItem.SpellID = SpellID;
                DataItem.RuneView = RuneView;
                DataItem.MythsoulEffect = MythsoulEffect;
                DataItem.MythSoulID = MythSoul;
                DataItem.MythSoulProgress = MythSoulPoints;
                DataItem.RelicAttributes = new Role.Instance.RelicAttribute[5];
                DataItem.RelicAttributes[0] = new Role.Instance.RelicAttribute(Attribute1);
                DataItem.RelicAttributes[1] = new Role.Instance.RelicAttribute(Attribute2);
                DataItem.RelicAttributes[2] = new Role.Instance.RelicAttribute(Attribute3);
                DataItem.RelicAttributes[3] = new Role.Instance.RelicAttribute(Attribute4);
                DataItem.RelicAttributes[4] = new Role.Instance.RelicAttribute(Attribute5);
                if (Timer > 0)
                {
                    DataItem.TimeStamp = DateTime.FromBinary(Timer);
                    if (DateTime.Now >= DataItem.TimeStamp)
                    {

                        DataItem.Delete = true;
                    }
                    else
                    {
                        TimeSpan timeSpan = DataItem.TimeStamp - DateTime.Now;
                        DataItem.TimeLeftInMinutes = (uint)timeSpan.TotalMinutes * 60;
                    }
                }


                DataItem.PerfectionLevel = PerfectionLevel;
                DataItem.PerfectionProgress = PerfectionProgress;
                DataItem.OwnerUID = OwnerUID;
                DataItem.OwnerName = OwnerName;
                DataItem.Signature = SpecialText;

                DataItem.Mutacion = this.Mutacion;
                DataItem.Mutacion2 = Mutacion2;

                DataItem.YuanshenIndex = YuanshenIndex;
                DataItem.YuanshenTime = YuanshenTime;
                return DataItem;
            }
        }
    }
}