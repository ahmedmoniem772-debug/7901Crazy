using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Role.Instance;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public static class MsgRelicFuse
    {
        [Flags]
        public enum ActionIds
        {
            FuseRelic = 0,
            SelectRelic = 1,
            BuyRelic = 4,
            Synergy = 5,
            Action7 = 7,//Remove Reconace Relic
            Action8 = 8,//Reconace Relic Attrebiute View
            UnLockSynergyTwo = 9,//Unlock 2nd Reconance
            Action10 = 10,//??
        }

        [ProtoContract]
        public class RelicFuse
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ActionID;

            [ProtoMember(2, IsRequired = true)]
            public uint MainRelic;

            [ProtoMember(3, IsRequired = true)]
            public uint FuseRelic;

            [ProtoMember(4)]
            public uint BigRateRelic;

            [ProtoMember(5)]
            public uint Member5;

            [ProtoMember(6)]
            public uint Member6;//all rate

            [ProtoMember(7)]
            public uint Member7;//rate

            [ProtoMember(8)]
            public uint Member8; //

            [ProtoMember(9)]
            public List<ItemData> Items = new List<ItemData>();//

            [ProtoMember(10)]
            public uint Member10; //
        }
        [ProtoContract]
        public class ItemData
        {
            [ProtoMember(1)]
            public uint ItemID = 0;

            [ProtoMember(2)]
            public uint Count = 0;

            //[ProtoMember(3)]
            //public bool Epic { get; set; }
        }
        private static readonly uint[][] FuseCost = new[]
         {
            new uint[] { 300000 },
            new uint[] { 300000, 500000},
            new uint[] { 300000, 500000, 1000000},
            new uint[] { 500000, 1000000, 2000000, 5000000},
            new uint[] { 1000000, 2000000, 5000000, 10000000, 20000000},
        };
        private unsafe static uint GetXuanBaoFuseCost(uint totalnum, uint samenum, uint _default)
        {
            if (FuseCost.Length > totalnum && FuseCost[totalnum].Length > samenum)
            {
                return FuseCost[totalnum][samenum];
            }

            return _default;
        }
        public static uint GetMaxAttributte(uint type)
        {
            var ComposeLimitDB = Database.RuneTable.composelimit.Where(i => i.Attribute == (Game.MsgServer.MsgChiInfo.ChiAttribute)type).ToArray().FirstOrDefault();
            if (ComposeLimitDB != null)
            {
                return ComposeLimitDB.Max;
            }

            return 0;
        }
        public static uint GetMinxAttributte(uint type)
        {
            var ComposeLimitDB = Database.RuneTable.composelimit.Where(i => i.Attribute == (Game.MsgServer.MsgChiInfo.ChiAttribute)type).ToArray().FirstOrDefault();
            if (ComposeLimitDB != null)
            {
                return ComposeLimitDB.Min;
            }

            return 0;
        }
        public static uint GetAttributID(uint value, Game.MsgServer.MsgChiInfo.ChiAttribute type, bool new_type = false)
        {
            if (new_type)
            {
                var max_attribute = GetMaxAttributte((uint)type);
                if (max_attribute / 3 < value)
                {
                    return (uint)(value * 1000 + (uint)(((uint)type % 100) + 100));
                }
            }

            return (uint)(value * 1000 + (uint)type);
        }
        private static int GetRelicAttributesCount(Role.Instance.RelicAttribute[] MainRelic, Role.Instance.RelicAttribute[] FuseRelic)
        {
            int L1Count = MainRelic.Count(i => i.Type != MsgChiInfo.ChiAttribute.None);
            int L2Count = FuseRelic.Count(i => i.Type != MsgChiInfo.ChiAttribute.None);
            if (L1Count == L2Count)
            {
                return L1Count;
            }

            int ResultCount = L1Count;
            if (L2Count > L1Count && L2Count == 5)
            {
                ResultCount = Role.Core.Rate(50) ? Math.Min(Pool.GetRandom.Next(L1Count, L2Count + 1), L2Count) : ((L1Count + L2Count) / 2);
                return ResultCount;
            }
            return ((L1Count + L2Count) / 2);
        }
        public static bool RelicAttribute(uint ID, Game.MsgServer.MsgChiInfo.ChiAttribute attribute)
        {
            if (attribute == MsgChiInfo.ChiAttribute.None)
            {
                return false;
            }

            switch (ID)
            {
                case 4100001://Featured (M-Attack)
                    {
                        if (attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.SkillCriticalStrike
                         || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.CriticalStrike
                         || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.HPAdd
                         || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddAttack)
                        {
                            return false;
                        }

                        break;
                    }
                case 4100002://Featured (Max-HP)
                    {
                        if (attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.SkillCriticalStrike
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.CriticalStrike
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddMagicAttack
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddAttack)
                        {
                            return false;
                        }

                        break;
                    }
                case 4100003://Featured (M-Strike)
                    {
                        if (attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.HPAdd
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.CriticalStrike
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddMagicAttack
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddAttack)
                        {
                            return false;
                        }

                        break;
                    }
                case 4100004://Featured (P-Attack)
                    {
                        if (attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.HPAdd
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.CriticalStrike
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddMagicAttack
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.SkillCriticalStrike)
                        {
                            return false;
                        }

                        break;
                    }
                case 4100005://Featured (P-Strike)
                    {
                        if (attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.HPAdd
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddAttack
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.AddMagicAttack
                            || attribute == Game.MsgServer.MsgChiInfo.ChiAttribute.SkillCriticalStrike)
                        {
                            return false;
                        }

                        break;
                    }
            }
            return true;
        }
        public static List<uint> XRelicFuse(Game.MsgServer.MsgGameItem MainRelic, Game.MsgServer.MsgGameItem FuseRelic)
        {
            List<Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>> Attributes = new List<Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>>();
            foreach (var atrribute in MainRelic.RelicAttributes.Where(i => RelicAttribute(MainRelic.ITEM_ID, i.Type)))
            {
                var new_type = atrribute.Type;
                var new_power = atrribute / 1000;

                int CountX = MainRelic.RelicAttributes.Count(p => p.Type == atrribute.Type);
                var xuanbaoadditionattr = Database.RuneTable.xuanbaoadditionattr.FirstOrDefault(i => i.Type == (byte)atrribute.Type && i.Num == CountX);
                if (xuanbaoadditionattr != null)
                {
                    if (new_power > xuanbaoadditionattr.Value)
                    {
                        new_power -= (ushort)xuanbaoadditionattr.Value;
                    }
                }

                Attributes.Add(new Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>(new_type, new_power));
            }
            foreach (var atrribute in FuseRelic.RelicAttributes.Where(i => RelicAttribute(MainRelic.ITEM_ID, i.Type)))
            {
                var new_type = atrribute.Type;
                var new_power = atrribute / 1000;

                int CountX = FuseRelic.RelicAttributes.Count(p => p.Type == atrribute.Type);
                var xuanbaoadditionattr = Database.RuneTable.xuanbaoadditionattr.FirstOrDefault(i => i.Type == (byte)atrribute.Type && i.Num == CountX);
                if (xuanbaoadditionattr != null)
                {
                    if (new_power > xuanbaoadditionattr.Value)
                    {
                        new_power -= (ushort)xuanbaoadditionattr.Value;
                    }
                }


                if (!CheckUpAttributes(Attributes, new Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>(new_type, new_power)))
                {
                    Attributes.Add(new Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>(new_type, new_power));
                }
            }

            if (Role.Core.Rate(1))
            {
                var new_type = Game.MsgServer.MsgChiInfo.ChiAttribute.LuckyStrike;
                uint new_power = 100;
                Attributes.Add(new Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>(new_type, new_power));
            }
            List<uint> GenerateAttributes = new List<uint>();
            int Count = GetRelicAttributesCount(MainRelic.RelicAttributes.ToArray(), FuseRelic.RelicAttributes.ToArray());
            for (int x = 0; x < Count; x++)
            {

                var PickAttribut = Attributes.Pick();
                int Duplicate = GenerateAttributes.Count(i => i % 100 == (int)PickAttribut.Item1);
                while (Duplicate > 0 && !Role.Core.Rate(35 / Duplicate == 0 ? 1 : Duplicate))
                {
                    Duplicate = GenerateAttributes.Count(i => i % 100 == (int)PickAttribut.Item1);
                    PickAttribut = Attributes.Pick();
                }
                var attvalue = (ushort)Math.Min(PickAttribut.Item2 + Math.Min((int)Pool.GetRandom.Next(0, 10), (int)GetMinxAttributte((uint)PickAttribut.Item1)), GetMaxAttributte((uint)PickAttribut.Item1));
                GenerateAttributes.Add(GetAttributID(attvalue, PickAttribut.Item1));




            }
            return GenerateAttributes;
        }
        private static bool CheckUpAttributes(List<Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>> attributes, Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint> attribut)
        {
            Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint> ToRemove = null;
            Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint> ToAdd = null;
            bool Found = false;
            foreach (var oldattr in attributes)
            {
                if (oldattr.Item1 == attribut.Item1)
                {
                    uint min = Math.Min(oldattr.Item2, attribut.Item2);
                    uint max = Math.Max(oldattr.Item2, attribut.Item2);
                    ToAdd = new Tuple<Game.MsgServer.MsgChiInfo.ChiAttribute, uint>(oldattr.Item1, (uint)Pool.GetRandom.Next((int)min, (int)max));
                    ToRemove = oldattr;
                    Found = true;
                    break;
                }
            }
            if (Found)
            {
                attributes.Remove(ToRemove);
                attributes.Add(ToAdd);
            }
            return Found;
        }
        public static unsafe ServerSockets.Packet CreategRelicFuse(this ServerSockets.Packet stream, RelicFuse obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgXuanBaoOpt);

            return stream;
        }
        public static unsafe void GetRelicFuse(this ServerSockets.Packet stream, out RelicFuse pQuery)
        {
            pQuery = new RelicFuse();
            pQuery = stream.ProtoBufferDeserialize<RelicFuse>(pQuery);
        }
        public static void xuanbaoadditionattr(MsgGameItem relic)
        {
            for (int x = 0; x < relic.RelicAttributes.Length; x++)
            {
                int Count = relic.RelicAttributes.Count(p => p.Type == relic.RelicAttributes[x].Type);
                var xuanbaoadditionattr = Database.RuneTable.xuanbaoadditionattr.FirstOrDefault(i => i.Type == (byte)relic.RelicAttributes[x].Type && i.Num == Count);
                if (xuanbaoadditionattr != null)
                {
                    relic.RelicAttributes[x].Value += (ushort)xuanbaoadditionattr.Value;
                }
                if (relic.RelicAttributes[x].Type == MsgChiInfo.ChiAttribute.LuckyStrike)
                {
                    relic.RelicAttributes[x].Value = 100;
                    relic.RelicAttributes[x].Epic = true;
                }
            }
        }
        [PacketAttribute(GamePackets.MsgXuanBaoOpt)]
        private unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {

            RelicFuse pQuery;
            stream.GetRelicFuse(out pQuery);

            switch (pQuery.ActionID)
            {
                case (uint)ActionIds.Action7://Remove
                    {
                        var c_relic = user.Relics.Values.FirstOrDefault(p => p.RelicResonance == pQuery.BigRateRelic);
                        if (c_relic != null)
                        {
                            c_relic.Position = 0;
                            c_relic.Mode = Role.Flags.ItemMode.AddItem;
                            c_relic.RelicResonance = 0;
                            c_relic.Send(user, stream);
                            user.Relics.Remove(c_relic.UID);
                            user.Inventory.ClientItems.TryAdd(c_relic.UID, c_relic);
                            pQuery.MainRelic = c_relic.UID;
                            user.Equipment.QueryEquipment(user.Equipment.Alternante);
                            user.Send(stream.CreategRelicFuse(pQuery));
                        }
                        break;
                    }
                case (uint)ActionIds.Action8://Relic Minor 1 View
                    {
                        pQuery.FuseRelic = user.Player.RelicResonance;//
                        pQuery.BigRateRelic = 2;
                        user.Send(stream.CreategRelicFuse(pQuery));
                        break;
                    }
                
                case (uint)ActionIds.UnLockSynergyTwo://unlock >> Done
                    {
                        pQuery.Member6 = user.Player.RelicsAllRate;
                        pQuery.Member7 = user.Player.RelicsRate;
                        pQuery.FuseRelic = user.Player.RelicResonance;

                        if (user.Player.RelicResonanceTwoUnlock == 1)
                        {
                            pQuery.FuseRelic = 300;
                            user.Player.RelicResonance = 300;
                            user.Player.RelicResonanceTwoUnlock = 1;
                            pQuery.BigRateRelic = 2;
                            user.Send(stream.CreategRelicFuse(pQuery));
                            break;
                        }

                        else if (pQuery.Items != null)
                        {
                            foreach (var item in pQuery.Items)
                            {
                                MsgGameItem PermStones;
                                if (user.Inventory.TryGetItem(item.ItemID, out PermStones))
                                {
                                    if (PermStones.ITEM_ID == 723694)//Small Perm Stone
                                    {
                                        user.Player.RelicResonance += 10;
                                        pQuery.FuseRelic = user.Player.RelicResonance;
                                        
                                        user.Inventory.RemoveStackItem(item.ItemID, item.Count, stream);

                                        if (user.Player.RelicResonance >= 300)
                                        {
                                            pQuery.FuseRelic = 300;
                                            user.Player.RelicResonance = 300;
                                            user.Player.RelicResonanceTwoUnlock = 1;
                                            pQuery.BigRateRelic = 2;
                                        }

                                        user.Send(stream.CreategRelicFuse(pQuery));
                                    }
                                    else if (PermStones.ITEM_ID == 723695)//Pig Perm Stone
                                    {
                                        user.Player.RelicResonance += 100;
                                        pQuery.FuseRelic = user.Player.RelicResonance;
                                        
                                        user.Inventory.RemoveStackItem(item.ItemID, item.Count, stream);

                                        if (user.Player.RelicResonance >= 300)
                                        {
                                            pQuery.FuseRelic = 300;
                                            user.Player.RelicResonance = 300;
                                            user.Player.RelicResonanceTwoUnlock = 1;
                                            pQuery.BigRateRelic = 2;
                                        }

                                        user.Send(stream.CreategRelicFuse(pQuery));

                                    }
                                }
                            }
                        }
                        user.Send(stream.CreategRelicFuse(pQuery));
                        break;
                    }
                case (uint)ActionIds.Action10://Relic Minor 2 View
                    {
                        
                        pQuery.FuseRelic = user.Player.RelicResonance;//
                        pQuery.BigRateRelic = 2;
                        user.Send(stream.CreategRelicFuse(pQuery));
                        break;
                    }
                case (uint)ActionIds.Synergy://Synergy
                    {
                        pQuery.Member6 = user.Player.RelicsAllRate;
                        pQuery.Member7 = user.Player.RelicsRate;
                        MsgGameItem MainRelic;
                        if (user.Equipment.TryGetEquip(Role.Flags.ConquerItem.Relic/*pQuery.MainRelic*/, out MainRelic))
                        {
                            MsgGameItem FuseRelic;
                            if (user.Inventory.TryGetItem(pQuery.FuseRelic, out FuseRelic))
                            {
                                if (Database.ItemType.ItemPosition(MainRelic.ITEM_ID) != (ushort)Role.Flags.ConquerItem.Relic
                                   || Database.ItemType.ItemPosition(FuseRelic.ITEM_ID) != (ushort)Role.Flags.ConquerItem.Relic)
                                {
                                    return;
                                }
                                if (user.Inventory.Contain(3353634, 10))//Prism Stone
                                {
                                    user.Inventory.RemoveStackItem(3353634, (ushort)10, stream);

                                    if (user.Player.RelicsRate >= 100 && user.Player.RelicsAllRate >= 1)
                                    {
                                        user.Player.RelicsAllRate -= 1;
                                        user.Player.RelicsRate += 1;

                                        pQuery.Member6 = user.Player.RelicsAllRate;
                                        pQuery.Member7 = user.Player.RelicsRate;
                                    }
                                    //xxx
                                    //FuseRelic.RelicAttributes

                                    if (user.Inventory.Update(FuseRelic, Role.Instance.AddMode.REMOVE, stream))
                                    {
                                        pQuery.ActionID = (uint)ActionIds.Synergy;
                                        FuseRelic.Position = (ushort)Role.Flags.ConquerItem.RelicResonance;
                                        //FuseRelic.Mode = Role.Flags.ItemMode.Remove;
                                        FuseRelic.RelicResonance = 2;
                                        FuseRelic.Send(user, stream);
                                        user.Relics.TryAdd(FuseRelic.UID, FuseRelic);
                                        //user.Inventory.Remove(FuseRelic.UID, 1, stream);
                                        pQuery.MainRelic = FuseRelic.UID;
                                        user.Equipment.QueryEquipment(user.Equipment.Alternante);

                                        user.Send(stream.CreategRelicFuse(pQuery));
                                    }
                                }
                            }
                        }

                        break;
                    }
                case 4://buy Relic
                    {
                        //user.Player.MessageBox("Shop Mall is Blocked by [ [GM] ] .", null, null, 60);
                        var amount = Math.Min(pQuery.FuseRelic, 100);
                        if (pQuery.MainRelic == 1)//Advanced
                        {
                            for (int i = 0; i < amount; i++)
                            {
                                if (user.Player.ConquerPoints >= 20000)
                                {
                                    user.Player.ConquerPoints -= 20000;
                                    user.Inventory.AddRandomRelicL5(stream, bound: false);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < amount; i++)
                            {
                                if (user.Player.ConquerPoints >= 10000)
                                {
                                    user.Player.ConquerPoints -= 10000;
                                    user.Inventory.AddRandomRelicL1(stream, false);
                                }
                            }
                        }
                        break;
                    }
                case (uint)ActionIds.FuseRelic:
                    {
                        MsgGameItem MainRelic;
                        if (user.Inventory.TryGetItem(pQuery.MainRelic, out MainRelic))
                        {
                            MsgGameItem FuseRelic;
                            if (user.Inventory.TryGetItem(pQuery.FuseRelic, out FuseRelic))
                            {
                                if (Database.ItemType.ItemPosition(MainRelic.ITEM_ID) != (ushort)Role.Flags.ConquerItem.Relic
                                   || Database.ItemType.ItemPosition(FuseRelic.ITEM_ID) != (ushort)Role.Flags.ConquerItem.Relic)
                                {
                                    return;
                                }
                                uint Cost = GetXuanBaoFuseCost((uint)(Math.Max((uint)MainRelic.RelicAttributes.Count(), (uint)FuseRelic.RelicAttributes.Count()) - 1), 0, 100000);
                                if (user.Player.Money >= Cost)
                                {
                                    user.Player.Money -= Cost;
                                    user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);


                                    user.Warehouse.Clear((uint)Game.MsgNpc.NpcID.RelicFuse);

                                    var Attributes = XRelicFuse(MainRelic, FuseRelic);
                                    var new_relic = new Game.MsgServer.MsgGameItem() { UID = pQuery.BigRateRelic = Pool.ITEM_Counter.Next, StackSize = 1, Position = (ushort)Role.Flags.ConquerItem.RelicFuse3, ITEM_ID = MainRelic.ITEM_ID, RelicAttributes = new RelicAttribute[5], Mode = Role.Flags.ItemMode.AddItem };
                                    new_relic.Durability = new_relic.MaximDurability = (ushort)(Pool.GetRandom.Next(30, 101) * 100);
                                    if (Attributes.Count > 0)
                                    {
                                        for (int O = 0; O < Attributes.Count; O++)
                                        {
                                            new_relic.RelicAttributes[O] = new RelicAttribute(Attributes[O]);
                                            var ComposeLimitDB = Database.RuneTable.composelimit.Where(i => i.Attribute == new_relic.RelicAttributes[O].Type).ToArray().FirstOrDefault();
                                            if (new_relic.RelicAttributes[O].Value >= ComposeLimitDB.Max)
                                            {
                                                new_relic.RelicAttributes[O].Value = (ushort)ComposeLimitDB.Max;
                                                new_relic.RelicAttributes[O].Epic = true;
                                            }
                                        }
                                        xuanbaoadditionattr(new_relic);

                                    }

                                    MainRelic.Position = (ushort)Role.Flags.ConquerItem.RelicFuse1;
                                    FuseRelic.Position = (ushort)Role.Flags.ConquerItem.RelicFuse2;
                                    user.Warehouse.AddItem(new_relic, (uint)Game.MsgNpc.NpcID.RelicFuse);
                                    user.Warehouse.AddItem(MainRelic, (uint)Game.MsgNpc.NpcID.RelicFuse);
                                    user.Warehouse.AddItem(FuseRelic, (uint)Game.MsgNpc.NpcID.RelicFuse);
                                    new_relic.Send(user, stream);

                                    MsgGameItem BestRelic = new_relic;
                                    pQuery.BigRateRelic = BestRelic.UID;
                                    user.Send(stream.CreategRelicFuse(pQuery));

                                    user.Inventory.Update(MainRelic, Role.Instance.AddMode.REMOVE, stream);
                                    user.Inventory.Update(FuseRelic, Role.Instance.AddMode.REMOVE, stream);
                                }
                                else
                                {
                                    user.SendSysMesage("You don`t have enough Money to FuseRelics");
                                }
                            }
                        }
                        break;
                    }
                case (uint)ActionIds.SelectRelic:
                    {
                        MsgGameItem item;
                        if (user.Warehouse.TryGetItem((uint)Game.MsgNpc.NpcID.RelicFuse, pQuery.MainRelic, out item))
                        {
                            if (!user.Inventory.HaveSpace(1))
                            {
                                user.SendSysMesage("Please make one more space in your inventory.");
                                break;
                            }

                            item.WH_ID = 0;
                            user.Inventory.Update(item, Role.Instance.AddMode.ADD, stream);
                            user.Warehouse.Clear((uint)Game.MsgNpc.NpcID.RelicFuse);
                            user.Send(stream.CreategRelicFuse(pQuery));

                        }
                        break;
                    }
                default:
                    {
                        MyConsole.WriteLine("MsgRelic Can't Find Action >> "+pQuery.ActionID+"");
                        break;
                    }
            }
        }
    }
}