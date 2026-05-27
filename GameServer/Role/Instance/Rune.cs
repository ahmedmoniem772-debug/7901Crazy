using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VirusX.Game.MsgServer;

namespace VirusX.Role.Instance
{
    public class Rune
    {
        public Client.GameClient Owner;
        private const byte MaxCount = 100;
        public Dictionary<uint, MsgGameItem> items;
        private MsgGameItem[] objects;
        public Rune(Client.GameClient client)
        {
            Owner = client;
            items = new Dictionary<uint, MsgGameItem>();
            objects = new MsgGameItem[0];
        }
        public uint Score
        {
            get
            {
                uint points = 0;
                foreach (var item in Objects)
                {
                    if (item.Position == 0 || item.Position == (ushort)Role.Flags.ConquerItem.RuneBag) continue;
                    points += 500 + ((item.ITEM_ID / 10000 != 401) ? (300 * (item.ITEM_ID % 100)) : 0);
                }
                return points;
            }
        }
        public bool Add(MsgGameItem item)
        {
            if (!items.ContainsKey(item.UID) && Objects.Where(i => i.Position == (ushort)Role.Flags.ConquerItem.RuneBag).Count() < MaxCount)
            {
                items.Add(item.UID, item);
                objects = items.Values.ToArray();
                return true;
            }
            return false;
        }
        public static Game.MsgServer.MsgSpell GetSkillForRune(Game.MsgServer.MsgGameItem item)
        {

            if (item.SpellID == item.ITEM_ID) return null;
            return new Game.MsgServer.MsgSpell() { ID = (ushort)(Pool.ItemsBase[item.ITEM_ID].RuneSpellID / 100), Level = (byte)(Pool.ItemsBase[item.ITEM_ID].RuneSpellID % 100) };
        }
        public static uint GetRuneFromSkill(Game.MsgServer.MsgSpell skill)
        {
            return Pool.ItemsBase.Values.Where(i => i.RuneSpellID == ((skill.ID * 100) + skill.Level)).FirstOrDefault() != null ? Pool.ItemsBase.Values.Where(i => i.RuneSpellID == ((skill.ID * 100) + skill.Level)).FirstOrDefault().ID : 0;
        }
        public bool Equip(MsgGameItem item, byte pos, bool updade = false)
        {
            if (items.ContainsKey(item.UID))
            {

                if (Database.ItemType.EquipPassJobReq(Pool.ItemsBase[item.ITEM_ID], Owner))
                {
                    items[item.UID].Position = pos;
                    objects = items.Values.ToArray();
                    if (updade)
                    {
                        using (var rec = new ServerSockets.RecycledPacket())
                        {

                            var streamm = rec.GetStream();
                            Owner.Send(streamm.ItemUsageCreate(MsgItemUsuagePacket.ItemUsuageID.EquipRune, item.UID, item.Position, 0, 0, 0, 0, 0));
                        }
                    }
                    var spell = GetSkillForRune(item);
                    if (spell != null)
                    {
                        if (!Owner.MySpells.ClientSpells.ContainsKey(spell.ID))
                        {
                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                            {
                                var streamm = recycledPacket.GetStream();
                                Owner.MySpells.Add(streamm, spell.ID, spell.Level);

                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public bool Unequip(MsgGameItem item, bool updade = false)
        {
            if (items.ContainsKey(item.UID))
            {
                if (updade)
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {

                        var streamm = rec.GetStream();
                        Owner.Send(streamm.ItemUsageCreate(MsgItemUsuagePacket.ItemUsuageID.UnequipRune, item.UID, item.Position, 0, 0, 0, 0, 0));
                    }
                }
                #region Nirvana Reset
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var streamx = rec.GetStream();

                    if (Owner != null)
                    {
                        byte defaultDeathTime = 20;
                        Owner.Send(streamx.DeathTimerCreate(defaultDeathTime));
                    }
                }
                #endregion

                items[item.UID].Position = (ushort)Role.Flags.ConquerItem.RunesCollection;
                objects = items.Values.ToArray();
                var spell = GetSkillForRune(item);
                if (spell != null)
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        #region Remove Skill
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.BloodTide))
                        {
                            Owner.SendSysMesage("Please remove the effect rune BloodTide.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.FlashShield))
                        {
                            Owner.SendSysMesage("Please remove the effect rune FlashShield.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.FuryStrike))
                        {
                            Owner.SendSysMesage("Please remove the effect rune FuryStrike.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.IronGuard))
                        {
                            Owner.SendSysMesage("Please remove the effect rune FuryStrike.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.RiseofTaoism))
                        {
                            Owner.SendSysMesage("Please remove the effect rune RiseofTaoism.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.NeptuneCurse))
                        {
                            Owner.SendSysMesage("Please remove the effect rune NeptuneCurse.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.PortadorRuneDuel))
                        {
                            Owner.SendSysMesage("Please remove the effect rune Duel.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.StarRaid))
                        {
                            Owner.SendSysMesage("Please remove the effect rune StarRaid.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.ShadowFist))
                        {
                            Owner.SendSysMesage("Please remove the effect rune ShadowFist.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.CrackStar))
                        {
                            Owner.SendSysMesage("Please remove the effect rune CrackStar.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.FireCurse))
                        {
                            Owner.SendSysMesage("Please remove the effect rune FireCurse.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.FineRain1))
                        {
                            Owner.SendSysMesage("Please remove the effect rune FineRain.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.FineRain2))
                        {
                            Owner.SendSysMesage("Please remove the effect rune FineRain.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.CounterPunch))
                        {
                            Owner.SendSysMesage("Please remove the effect rune CounterPunch.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.Slayer))
                        {
                            Owner.SendSysMesage("Please remove the effect rune Slayer.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.Absolution))
                        {
                            Owner.SendSysMesage("Please remove the effect rune Absolution.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.Infinity))
                        {
                            Owner.SendSysMesage("Please remove the effect rune Infinity.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.Rampage))
                        {
                            Owner.SendSysMesage("Please remove the effect rune Rampage.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.IronShield))
                        {
                            Owner.SendSysMesage("Please remove the effect rune IronShield.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.Sacrifice))
                        {
                            Owner.SendSysMesage("Please remove the effect rune Sacrifice.");
                            return false;
                        }
                        if (Owner.Player.ContainFlag(MsgUpdate.Flags.FrostArrows))
                        {
                            Owner.SendSysMesage("Please remove the effect rune FrostArrows.");
                            return false;
                        }
                        #endregion
                        var streamm = rec.GetStream();
                        Owner.MySpells.Remove(spell.ID, streamm);
                    }
                }
                return true;
            }
            return false;
        }
        public uint PerfectionRedRune
        {
            get
            {
                var item = WornObjects.Where(i => i.Position == (ushort)Role.Flags.ConquerItem.RedRune || i.Position == (ushort)Role.Flags.ConquerItem.AlternateRedRune).FirstOrDefault();
                if (item == null) return 0;
                return 3000;
            }
        }
        public uint PerfectionBlueRune
        {
            get
            {
                var item = WornObjects.Where(i => i.Position == (ushort)Role.Flags.ConquerItem.BlueRune || i.Position == (ushort)Role.Flags.ConquerItem.AlternateBlueRune).FirstOrDefault();
                if (item == null) return 0;
                switch (item.ITEM_ID % 100)
                {
                    case 2: return 1300;
                    case 3: return 1500;
                    case 4: return 1800;
                    case 5: return 2300;
                    case 6: return 2800;
                    case 7: return 3400;
                    case 8: return 4000;
                    case 9: return 4600;
                    case 10: return 5300;
                    case 11: return 6000;
                    case 12: return 6900;
                    case 13: return 7900;
                    case 14: return 8900;
                    case 15: return 9900;
                    case 16: return 11000;
                    case 17: return 13000;
                    case 18: return 14000;
                    case 19: return 15500;
                    case 20: return 17000;
                    case 21: return 18600;
                    case 22: return 20500;
                    case 23: return 22000;
                    case 24: return 24500;
                    case 25: return 26500;
                    case 26: return 29000;
                    case 27: return 31000;
                    default: return 1100;
                }
            }
        }
        public uint PerfectionYellowRune
        {
            get
            {
                uint sum = 0;
                for (byte x = 0; x < 11; x++)
                {
                    var item = WornObjects.Where(i => i.Position == (ushort)Role.Flags.ConquerItem.YellowRune + x || i.Position == (ushort)Role.Flags.ConquerItem.AlternateYellowRune + x).FirstOrDefault();
                    if (item == null) continue;
                    switch (item.ITEM_ID % 100)
                    {
                        case 2: sum += 1600; break;
                        case 3: sum += 2200; break;
                        case 4: sum += 3000; break;
                        case 5: sum += 4000; break;
                        case 6: sum += 5300; break;
                        case 7: sum += 6900; break;
                        case 8: sum += 8800; break;
                        case 9: sum += 11000; break;
                        default: sum += 1200; break;
                    }
                }
                return sum;
            }
        }
        public uint PerfectionIdelRune
        {
            get
            {
                uint sum = 0;
                for (byte x = 0; x < 11; x++)
                {
                    var item = WornObjects.Where(i => i.Position == (ushort)Role.Flags.ConquerItem.YellowRune + x || i.Position == (ushort)Role.Flags.ConquerItem.AlternateYellowRune + x).FirstOrDefault();
                    if (item == null) continue;
                    switch (item.ITEM_ID % 100)
                    {
                        case 9: sum += 11000; break;
                    }
                }
                return sum;
            }
        }
        //public uint PerfectionIdelRune
        //{
        //    get
        //    {
        //        uint sum = 0;
        //        foreach (var item in Objects)
        //        {
        //            if (item.DBInfo.Description != "ideal rune") continue;
        //            if (item.ITEM_ID / 10000 == 407)
        //            {
        //                switch (item.ITEM_ID % 100)
        //                {
        //                    case 1: sum += 36* 2; break;
        //                    case 2: sum += 48* 2; break;
        //                    case 3: sum += 66* 2; break;
        //                    case 4: sum += 90* 2; break;
        //                    case 5: sum += 120* 2; break;
        //                    case 6: sum += 159* 2; break;
        //                    case 7: sum += 207* 2; break;
        //                    case 8: sum += 264* 2; break;
        //                    case 9: sum += 330* 2; break;
        //                }
        //            }
        //            if (item.ITEM_ID / 10000 == 403)
        //            {
        //                switch (item.ITEM_ID % 100)
        //                {
        //                    case 1: sum += 36 * 2; break;
        //                    case 2: sum += 48* 2; break;
        //                    case 3: sum += 66* 2; break;
        //                    case 4: sum += 90* 2; break;
        //                    case 5: sum += 120* 2; break;
        //                    case 6: sum += 159* 2; break;
        //                    case 7: sum += 207* 2; break;
        //                    case 8: sum += 264* 2; break;
        //                    case 9: sum += 330* 2; break;
        //                }
        //            }
        //        }
        //        for (byte x = 0; x < 18; x++)
        //        {
        //            var item = WornObjects.Where(i => i.Position == (ushort)Role.Flags.ConquerItem.YellowRune + x || i.Position == (ushort)Role.Flags.ConquerItem.AlternateYellowRune + x).FirstOrDefault();
        //            if (item == null) continue;
        //            if (item.DBInfo.Description != "ideal rune") continue;
        //            switch (item.ITEM_ID % 100)
        //            {
        //                default: sum += 11000; break;
        //            }
        //        }
        //        return sum;
        //    }
        //}
        public bool Remove(uint UID, bool Inventory = true)
        {
            if (items.ContainsKey(UID))
            {
                items[UID].Position = 0;
                if (Inventory)
                    Owner.Inventory.ClientItems.TryAdd(UID, items[UID]);
                items.Remove(UID);
                objects = items.Values.ToArray();
                using (var recycledPacket = new ServerSockets.RecycledPacket())
                {
                    var streamm = recycledPacket.GetStream();
                    {
                        Owner.Send(streamm.ItemUsageCreate(MsgItemUsuagePacket.ItemUsuageID.RemoveInventory, UID, 0, 0, 0, 0, 0, 0));

                    }
                }
                return true;
            }
            return false;
        }
        public bool RemoveStackItem(uint UID, ushort count = 1, bool Inventory = true)
        {
            if (items.ContainsKey(UID))
            {
                items[UID].Position = 0;

                if (items[UID].StackSize > count)
                {
                    items[UID].StackSize -= count;

                    if (Inventory)
                        Owner.Inventory.ClientItems.TryAdd(UID, items[UID]);
                    objects = items.Values.ToArray();


                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        items[UID].Mode = Flags.ItemMode.Update;
                        items[UID].Send(Owner, stream);
                    }
                    return true;
                }
                else
                {
                    if (Inventory)
                        Owner.Inventory.ClientItems.TryAdd(UID, items[UID]);
                    items.Remove(UID);
                    objects = items.Values.ToArray();
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var streamm = rec.GetStream();
                        Owner.Send(streamm.ItemUsageCreate(MsgItemUsuagePacket.ItemUsuageID.RemoveInventory, UID, 0, 0, 0, 0, 0, 0));
                    }
                    return true;
                }
            }
            return false;
        }
        public bool IdealCollection(string runeName)
        {
            if (Owner.Player.Reborn < 2 || Owner.Player.Level < 15) return false;
            foreach (var i in Objects)
            {
                if (i == null || !Database.ItemType.EquipPassJobReq(Pool.ItemsBase[i.ITEM_ID], Owner)) continue;
                if (Database.ItemType.isRune(i.ITEM_ID))
                {
                    if (Pool.ItemsBase[i.ITEM_ID].Name.ToLower().Contains(runeName.ToLower()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsEquipped(string runeName, out MsgGameItem item)
        {
            item = null;
            if (Owner.Player.Reborn < 2 || Owner.Player.Level < 15) return false;
            foreach (var i in WornObjects)
            {
                if (i == null || !Database.ItemType.EquipPassJobReq(Pool.ItemsBase[i.ITEM_ID], Owner)) continue;
                if (Database.ItemType.isRune(i.ITEM_ID))
                {
                    if (Pool.ItemsBase[i.ITEM_ID].Name.ToLower().Contains(runeName.ToLower()))
                    {
                        item = i;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsEquipped(string runeName)
        {
            MsgGameItem item;
            return IsEquipped(runeName, out item);
        }
        public bool IsEquipped(string runeName, ref byte itemLevel)
        {
            MsgGameItem item;
            IsEquipped(runeName, out item);
            if (item != null) itemLevel = (byte)(item.ITEM_ID % 100);
            else itemLevel = 0;
            return item != null;
        }
        public MsgGameItem[] Objects
        {
            get
            {
                return objects;
            }
        }
        public void Show(ServerSockets.Packet stream)
        {
            foreach (MsgGameItem item in Objects.OrderBy(i => i.Position))
            {
                item.Mode = Role.Flags.ItemMode.AddItem;
                item.Send(Owner, stream);
            }
        }
        public MsgGameItem[] WornObjects
        {
            get
            {
                List<MsgGameItem> myList = new List<MsgGameItem>();
                foreach (var item in objects)
                {
                    if (item.Position == (ushort)Role.Flags.ConquerItem.RuneBag || item.Position == (ushort)Role.Flags.ConquerItem.Inventory || item.Position == (ushort)Role.Flags.ConquerItem.RunesCollection) continue;
                    if (Owner.Equipment.Alternante)
                        if (item.Position < 120 && objects.Count(i => i.Position == item.Position + 20) > 0)
                            continue;
                    if (!Owner.Equipment.Alternante)
                        if (item.Position > 120)
                            continue;
                    myList.Add(item);
                }
                return myList.ToArray();
            }
        }
        public MsgGameItem[] WornObjectsMinor
        {
            get
            {
                List<MsgGameItem> myList = new List<MsgGameItem>();
                foreach (var item in objects)
                {
                    if (item.Position == (ushort)Role.Flags.ConquerItem.RuneBag || item.Position == (ushort)Role.Flags.ConquerItem.Inventory || item.Position == (ushort)Role.Flags.ConquerItem.RunesCollection) continue;
                    if (Owner.Equipment.Alternante)
                        if (item.Position == 121 && objects.Count(i => i.Position == item.Position + 20) > 0 || item.Position == 122 && objects.Count(i => i.Position == item.Position + 20) > 0)
                            continue;
                    if (Owner.Equipment.Alternante)
                        if (item.Position > 120)
                            continue;
                    myList.Add(item);
                }
                return myList.ToArray();
            }
        }
        public MsgGameItem[] RebornRunesEquip
        {
            get
            {
                List<MsgGameItem> myList = new List<MsgGameItem>();
                foreach (var item in objects)
                {
                    if (item.Position == (ushort)Role.Flags.ConquerItem.RedRune
                        || item.Position == (ushort)Role.Flags.ConquerItem.BlueRune
                        || item.Position == (ushort)Role.Flags.ConquerItem.AlternateRedRune || item.Position == (ushort)Role.Flags.ConquerItem.AlternateBlueRune)
                        myList.Add(item);
                }
                return myList.ToArray();
            }
        }
        public bool Full()
        {
            if (WornObjects.Count() < 9) return false;
            foreach (MsgGameItem i in WornObjects)
                if (Database.ItemType.MaxRuneLevel(i.ITEM_ID) > i.ITEM_ID % 100)
                    return false;
            return true;
        }
        public byte Count
        {
            get
            {
                byte count = 0;
                foreach (MsgGameItem i in objects)
                    if (i != null)
                        count++;
                return count;
            }
        }
        public byte EquippedCount
        {
            get
            {
                byte count = 0;
                foreach (MsgGameItem i in objects)
                    if (!Owner.Equipment.Alternante)
                    {
                        if (i != null && i.Position >= (ushort)Role.Flags.ConquerItem.RedRune && i.Position <= ((ushort)Role.Flags.ConquerItem.YellowRune + 12))
                            count++;
                    }
                    else
                    {
                        if (i != null && i.Position >= (ushort)Role.Flags.ConquerItem.AlternateRedRune && i.Position <= ((ushort)Role.Flags.ConquerItem.AlternateYellowRune + 12))
                            count++;
                    }

                return count;
            }
        }
        public bool TryGetItem(uint UID, out MsgGameItem item)
        {
            foreach (MsgGameItem i in objects)
            {
                if (i != null)
                    if (i.UID == UID)
                    {
                        item = i;
                        return true;
                    }
            }
            item = null;
            return false;
        }
        public bool TryGetItem2(uint Item, out MsgGameItem item)
        {
            foreach (MsgGameItem i in objects)
            {
                if (i != null)
                    if (i.ITEM_ID == Item)
                    {
                        item = i;
                        return true;
                    }
            }
            item = null;
            return false;
        }

        public bool CanIdealRune(uint UID, uint UID2)
        {
            MsgGameItem Item = null;
            MsgGameItem Item2 = null;
            MsgGameItem RuneBag = null;
            if (Owner.Inventory.TryGetItem(UID, out Item) && Owner.Inventory.TryGetItem(UID2, out Item2)
                || Owner.Inventory.TryGetItem(UID, out Item) && Owner.Rune.TryGetItem(UID2, out RuneBag))
            {
                if (RuneBag == null)
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {

                        var stream = rec.GetStream();
                        Owner.Inventory.RemoveStackItem(UID, 1, stream);
                        Owner.Inventory.RemoveStackItem(UID2, 900, stream);
                        return true;
                    }
                }
                else
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {

                        var stream = rec.GetStream();
                        Owner.Inventory.RemoveStackItem(UID, 1, stream);
                        Owner.Rune.RemoveStackItem(UID2, 900);
                        return true;
                    }
                }
            }
            return false;
        }
        public void AddIdealRune(Client.GameClient client, ServerSockets.Packet stream, uint ItemID)
        {
            MsgGameItem Rune = new MsgGameItem();
            Rune.ITEM_ID = ItemID;
            Rune.UID = Pool.ITEM_Counter.Next;
            if (client.Rune.Add(Rune))
            {
                Rune.Position = (ushort)Role.Flags.ConquerItem.RunesCollection;
            }
            Rune.Mode = Role.Flags.ItemMode.AddItem;
            Rune.Send(client, stream);
        }
        public MsgGameItem TryGetItem(byte Position)
        {
            return Objects.Where(i => i.Position == Position).FirstOrDefault();
        }
        public bool Free(ushort Position)
        {
            return (Position == (ushort)Role.Flags.ConquerItem.RuneBag && Count < MaxCount) || Position == (ushort)Role.Flags.ConquerItem.RunesCollection || (Objects.Where(i => i.Position == Position).Count() < 1);
        }
        public MsgGameItem EquippedRedRune
        {
            get
            {
                foreach (var item in WornObjects)
                    if (item.Position == (ushort)Role.Flags.ConquerItem.RedRune || item.Position == (ushort)Role.Flags.ConquerItem.AlternateRedRune)
                        return item;
                return null;
            }
        }

        public MsgGameItem EquippedBlueRune
        {
            get
            {
                foreach (var item in WornObjects)
                    if (item.Position == (ushort)Role.Flags.ConquerItem.BlueRune || item.Position == (ushort)Role.Flags.ConquerItem.AlternateBlueRune)
                        return item;
                return null;
            }
        }

        public uint HPClient
        {
            get
            {
                uint hp = 0;
                var Scores = Database.RuneTable.Attributes.Values.Where(p => p.Score <= Score && p.Type == Database.RuneTable.RuneAttribute.HPAdd).OrderByDescending(p => p.Score).FirstOrDefault();
                if (Scores != null)
                    hp += Scores.Value;
                return hp;
            }
        }
    }
}