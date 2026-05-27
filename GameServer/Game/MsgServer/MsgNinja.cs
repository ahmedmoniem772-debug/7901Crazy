using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using MortalConquer.Client;
using MortalConquer.Role.Instance;

namespace MortalConquer.Game.MsgServer
{
    public static class MsgNinja
    {
        public const ushort MsgNinjaItem_ID = 2393;
        public const ushort MsgNinjaInfo_ID = 2394;
        public const ushort MsgNinjaUser_ID = 2395;
        [ProtoContract]
        public class MsgNinjaUser//2395
        {
            public enum TypeID
            {
                Loading = 0,
                Flag = 1,
                Points = 2,
            }
            [ProtoMember(1, IsRequired = true)]
            public uint Type = 0;
            [ProtoMember(2, IsRequired = true)]
            public uint UID = 0;
            [ProtoMember(3, IsRequired = true)]
            public ulong Flag = 0;
            [ProtoMember(4, IsRequired = true)]
            public uint Points = 0;
            [ProtoMember(5, IsRequired = true)]
            public uint Fire = 0;
            [ProtoMember(6, IsRequired = true)]
            public uint Water = 0;
            [ProtoMember(7, IsRequired = true)]
            public uint Earth = 0;
            [ProtoMember(8, IsRequired = true)]
            public uint Wind = 0;
            [ProtoMember(9, IsRequired = true)]
            public uint Lightning = 0;
            [ProtoMember(10, IsRequired = true)]
            public uint FireNew = 0;
            [ProtoMember(11, IsRequired = true)]
            public uint WaterNew = 0;
            [ProtoMember(12, IsRequired = true)]
            public uint EarthNew = 0;
            [ProtoMember(13, IsRequired = true)]
            public uint WindNew = 0;
            [ProtoMember(14, IsRequired = true)]
            public uint LightningNew = 0;
            [ProtoMember(15, IsRequired = true)]
            public uint Score = 0;
            [ProtoMember(16, IsRequired = true)]
            public uint FireMastery = 0;
            [ProtoMember(17, IsRequired = true)]
            public uint WaterMastery = 0;
            [ProtoMember(18, IsRequired = true)]
            public uint EarthMastery = 0;
            [ProtoMember(19, IsRequired = true)]
            public uint WindMastery = 0;
            [ProtoMember(20, IsRequired = true)]
            public uint LightningMastery = 0;
            [ProtoMember(21, IsRequired = true)]
            public uint UnlockMastery = 0;
            public static void Create(GameClient user, Ninja Ninja, uint UID)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    MsgNinjaUser obj = new MsgNinjaUser();
                    obj.Type = (uint)MsgNinjaUser.TypeID.Loading;
                    obj.UID = UID;
                    obj.Points = Ninja.Exp;
                    obj.Flag = Ninja.Flag;
                    obj.Fire = Ninja.Fire;
                    obj.Water = Ninja.Water;
                    obj.Earth = Ninja.Earth;
                    obj.Wind = Ninja.Wind;
                    obj.Lightning = Ninja.Lightning;
                    obj.FireNew = Ninja.tmp_Fire;
                    obj.WaterNew = Ninja.tmp_Water;
                    obj.EarthNew = Ninja.tmp_Earth;
                    obj.WindNew = Ninja.tmp_Wind;
                    obj.LightningNew = Ninja.tmp_Lightning;
                    obj.Score = Ninja.Score;
                    obj.FireMastery = Ninja.FireMastery;
                    obj.WaterMastery = Ninja.WaterMastery;
                    obj.EarthMastery = Ninja.EarthMastery;
                    obj.WindMastery = Ninja.WindMastery;
                    obj.LightningMastery = Ninja.LightningMastery;
                    obj.UnlockMastery = Ninja.UnlockMastery;
                    user.Send(stream.CreateNinjaUser(obj));
                }
            }
        }
        [ProtoContract]
        public class MsgNinjaItem//2393
        {
            [Flags]
            public enum TypeID
            {
                Load = 0,
                View = 1,
                Add = 2,
                Update = 3,
            }
            [ProtoMember(1, IsRequired = true)]
            public TypeID Type = 0;
            [ProtoMember(2, IsRequired = true)]
            public uint UID = 0;
            [ProtoMember(3, IsRequired = true)]
            public Item[] Items;
            [ProtoContract]
            public class Item
            {
                [ProtoMember(1, IsRequired = true)]
                public uint ID = 0;
                [ProtoMember(2, IsRequired = true)]
                public uint Level = 0;
                [ProtoMember(3, IsRequired = true)]
                public uint Position = 0;
                [ProtoMember(4, IsRequired = true)]
                public uint Points = 0;
                [ProtoMember(5, IsRequired = true)]
                public uint UK = 0;
            }
            public static void Create(GameClient user, Ninja _NinjaSprint, TypeID Type, uint UID)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    MsgNinjaItem obj = new MsgNinjaItem();
                    obj.Type = Type;
                    obj.UID = UID;
                    obj.Items = new MsgNinjaItem.Item[_NinjaSprint.Items.Count];
                    int i = 0; foreach (var Item in _NinjaSprint.Items.Values)
                    {
                        obj.Items[i] = new MsgNinjaItem.Item();
                        obj.Items[i].ID = Item.ItemID;
                        obj.Items[i].Level = Item.Level;
                        obj.Items[i].Position = Item.Position;
                        obj.Items[i].Points = Item.Exp;
                        obj.Items[i].UK = Item.UK; i++;
                    }
                    user.Send(stream.CreateNinjaItem(obj));
                }
            }
            public static void Create(GameClient user, Ninja.Item Item, TypeID Type)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    MsgNinjaItem obj = new MsgNinjaItem();
                    obj.Type = Type;
                    obj.UID = user.Player.UID;
                    obj.Items = new MsgNinjaItem.Item[1];
                    obj.Items[0] = new MsgNinjaItem.Item();
                    obj.Items[0].ID = Item.ItemID;
                    obj.Items[0].Level = Item.Level;
                    obj.Items[0].Position = Item.Position;
                    obj.Items[0].Points = Item.Exp;
                    user.Send(stream.CreateNinjaItem(obj));
                }

            }
            [PacketAttribute(MsgNinjaItem_ID)]
            private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
            {
                MsgNinjaItem Info;
                stream.GetNinjaItem(out Info);
                switch (Info.Type)
                {
                    case TypeID.View:
                        {
                            Client.GameClient Target;
                            if (Pool.GamePoll.TryGetValue((uint)Info.UID, out Target))
                            {
                                MsgNinjaItem.Create(client, Target.MyNinja, MsgNinjaItem.TypeID.View, Target.Player.UID);
                                MsgNinjaUser.Create(client, Target.MyNinja, Target.Player.UID);
                            }
                            break;
                        }
                }
            }
        }
        [ProtoContract]
        public class MsgNinjaInfo//2394
        {
            [Flags]
            public enum TypeID
            {
                Equip = 0,
                Unequip = 1,
                UpLevel = 2,
                BoostAptitude = 4,
                SaveDiscard = 5,
                Hat = 6,
                AddPoints = 7,
                AddMastery = 8,
                InstantUpgrade = 9,
                UnlockMastery = 11,
            }
            [ProtoMember(1, IsRequired = true)]
            public TypeID Type = 0;
            [ProtoMember(2, IsRequired = true)]
            public uint ID = 0;
            [ProtoMember(3, IsRequired = true)]
            public uint Points = 0;
            [ProtoMember(4, IsRequired = true)]
            public uint Position = 0;
            public static void PrintPacket(byte[] packet)
            {
                foreach (byte D in packet)
                {
                    System.Console.Write((Convert.ToString(D, 16)).PadLeft(2, '0').ToUpper() + " ");
                }
                System.Console.Write("\n\n");
            }
            [PacketAttribute(MsgNinjaInfo_ID)]
            private unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
            {
                Random Random = new Random();
                MsgNinjaInfo Info;
                stream.GetNinjaInfo(out Info);
                switch (Info.Type)
                {
                    case TypeID.Equip:
                        {
                            if (Info.Position == 1 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Dawn)
                                || Info.Position == 2 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Rest)
                                || Info.Position == 3 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Life)
                                || Info.Position == 4 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Pain)
                                || Info.Position == 5 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Limlt)
                                || Info.Position == 6 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_View)
                                || Info.Position == 7 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Shock)
                                || Info.Position == 8 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Death)
                                || Info.Position == 9 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Rest_2)
                                || Info.Position == 10 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Life_2)
                                || Info.Position == 11 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Pain_2)
                                || Info.Position == 12 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Limlt_2)
                                || Info.Position == 13 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_View_2)
                                || Info.Position == 14 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Shock_2)
                                || Info.Position == 15 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Death_2)
                                || Info.Position == 16 && user.MyNinja.ContainsFlag((ulong)Ninja.Flags.Gate_of_Death_2))
                            {
                                Ninja.Item item;
                                if (user.MyNinja.TryGetValue(Info.ID, out item))
                                {
                                    var Unequip = user.MyNinja.Items.Values.Where(p => p.Position == Info.Position).FirstOrDefault();
                                    if (Unequip != null)
                                    {
                                        Info.Type = TypeID.Unequip;
                                        Info.ID = Unequip.ItemID;
                                        Info.Position = Unequip.Position;
                                        Info.Points = Unequip.Level;
                                        user.Send(stream.CreateNinjaInfo(Info));
                                        Unequip.Position = 0;
                                        Unequip.UPDATE();
                                    }
                                    Info.Type = TypeID.Equip;
                                    item.Position = Info.Position;
                                    Info.ID = item.ItemID;
                                    Info.Position = item.Position;
                                    Info.Points = item.Level;
                                    user.Send(stream.CreateNinjaInfo(Info));
                                    item.UPDATE();
                                    user.MyNinja.GetLevel();
                                    user.MyNinja.Alternate();
                                    user.Equipment.QueryEquipment(user.Equipment.Alternante);
                                }
                            }
                            break;
                        }
                    case TypeID.Unequip:
                        {
                            var item = user.MyNinja.Items.Values.Where(p => p.Position == Info.Position).FirstOrDefault();
                            if (item != null)
                            {
                                item.Position = 0;
                                Info.ID = item.ItemID;
                                Info.Points = item.Level;
                                user.Send(stream.CreateNinjaInfo(Info));
                                item.UPDATE();
                            }
                            user.MyNinja.GetLevel();
                            user.MyNinja.Alternate();
                            user.Equipment.QueryEquipment(user.Equipment.Alternante);
                            break;
                        }
                    case TypeID.Hat:
                        {

                            if (Info.Points == 1)
                            {
                                if (Info.ID != (uint)Game.MsgServer.MsgUpdate.Flags.Overall)
                                {
                                    user.Player.RemoveFlag(Game.MsgServer.MsgUpdate.Flags.Fire);
                                    user.Player.RemoveFlag(Game.MsgServer.MsgUpdate.Flags.Water);
                                    user.Player.RemoveFlag(Game.MsgServer.MsgUpdate.Flags.Earth);
                                    user.Player.RemoveFlag(Game.MsgServer.MsgUpdate.Flags.Wind);
                                    user.Player.RemoveFlag(Game.MsgServer.MsgUpdate.Flags.Lightning);
                                }
                                user.Player.AddFlag((Game.MsgServer.MsgUpdate.Flags)Info.ID, Role.StatusFlagsBigVector32.PermanentFlag, false);
                            }
                            else if (Info.Points == 0)
                            {
                                user.Player.RemoveFlag((Game.MsgServer.MsgUpdate.Flags)Info.ID);
                            }
                            break;
                        }
                    case TypeID.UnlockMastery:
                        {
                            switch (Info.ID)
                            {
                                case 1: if (user.MyNinja.Fire_Score >= 14000) user.MyNinja.AddFlagMastery(Ninja.UnlockMasterys.FireMastery); break;
                                case 2: if (user.MyNinja.Water_Score >= 14000) user.MyNinja.AddFlagMastery(Ninja.UnlockMasterys.WaterMastery); break;
                                case 3: if (user.MyNinja.Earth_Score >= 14000) user.MyNinja.AddFlagMastery(Ninja.UnlockMasterys.EarthMastery); break;
                                case 4: if (user.MyNinja.Wind_Score >= 14000) user.MyNinja.AddFlagMastery(Ninja.UnlockMasterys.WindMastery); break;
                                case 5: if (user.MyNinja.Lightning_Score >= 14000) user.MyNinja.AddFlagMastery(Ninja.UnlockMasterys.LightningMastery); break;
                            }
                            MsgNinjaUser.Create(user, user.MyNinja, user.Player.UID);
                            user.Send(stream.CreateNinjaInfo(Info));
                            break;
                        }
                    case TypeID.AddMastery:
                        {
                            if (user.MyNinja.Exp >= Info.Points)
                            {
                                switch (Info.ID)
                                {
                                    case 1: user.MyNinja.FireMastery += Info.Points; break;
                                    case 2: user.MyNinja.WaterMastery += Info.Points; break;
                                    case 3: user.MyNinja.EarthMastery += Info.Points; break;
                                    case 4: user.MyNinja.WindMastery += Info.Points; break;
                                    case 5: user.MyNinja.LightningMastery += Info.Points; break;
                                }
                                user.MyNinja.Exp -= Info.Points;
                                user.MyNinja.AddPoints(0);
                                Info.Type = TypeID.UnlockMastery;
                                user.Send(stream.CreateNinjaInfo(Info));
                                user.MyNinja.UpdateRank();
                            }
                            break;
                        }
                    case TypeID.UpLevel:
                        {
                            Ninja.Item item;
                            if (user.MyNinja.TryGetValue(Info.ID, out item))
                            {
                                if (user.MyNinja.Exp >= Info.Points)
                                {
                                    user.MyNinja.Exp -= Info.Points;
                                    item.Exp += Info.Points;
                                    if (item.Exp >= item.DBItem.Exp)
                                    {
                                        item.Exp -= item.DBItem.Exp;
                                        item.Level++;
                                    }
                                    item.UPDATE();
                                    user.MyNinja.AddPoints(0);
                                    MsgNinjaItem.Create(user, item, MsgNinjaItem.TypeID.Update);
                                }
                            }
                            user.MyNinja.UpdateRank();
                            user.MyNinja.GetLevel();
                            user.MyNinja.Alternate();
                            user.Equipment.QueryEquipment(user.Equipment.Alternante);
                            break;
                        }
                    case TypeID.BoostAptitude:
                        {

                            if (user.MyNinja.Fire == 0 || user.MyNinja.Water == 0 || user.MyNinja.Earth == 0 || user.MyNinja.Wind == 0 || user.MyNinja.Lightning == 0)
                            {
                                user.MyNinja.Fire = 1;
                                user.MyNinja.Water = 1;
                                user.MyNinja.Earth = 1;
                                user.MyNinja.Wind = 1;
                                user.MyNinja.Lightning = 1;
                            }
                            else
                            {
                                var n = user.MyNinja;
                                uint t = n.Fire + n.Water + n.Earth + n.Wind + n.Lightning;
                                uint _i = 0;
                                uint _a = 0;
                                if (t < 150)
                                {
                                    _i = 3327203;
                                    _a = 1;
                                }
                                if (t > 150 && t <= 299)
                                {
                                    _i = 3327203;
                                    _a = 2;
                                }
                                if (t >= 300 && t <= 400)
                                {
                                    _i = 3330609;
                                    _a = 1;
                                }
                                if (t > 400)
                                {
                                    _i = 3330608;
                                    _a = 1;
                                }
                                if (_i != 0 && _a != 0)
                                {
                                    if (user.Inventory.Contain(_i, _a))
                                    {
                                        user.MyNinja.tmp_Fire = (uint)Random.Next(1, 5) + user.MyNinja.Fire;
                                        user.MyNinja.tmp_Water = (uint)Random.Next(1, 5) + user.MyNinja.Water;
                                        user.MyNinja.tmp_Earth = (uint)Random.Next(1, 5) + user.MyNinja.Earth;
                                        user.MyNinja.tmp_Wind = (uint)Random.Next(1, 5) + user.MyNinja.Wind;
                                        user.MyNinja.tmp_Lightning = (uint)Random.Next(1, 5) + user.MyNinja.Lightning;
                                        user.Inventory.Remove(_i, _a, stream);
                                        if (n.tmp_Fire > 100)
                                            n.tmp_Fire = 100;
                                        if (n.tmp_Water > 100)
                                            n.tmp_Water = 100;
                                        if (n.tmp_Earth > 100)
                                            n.tmp_Earth = 100;
                                        if (n.tmp_Wind > 100)
                                            n.tmp_Wind = 100;
                                        if (n.tmp_Lightning > 100)
                                            n.tmp_Lightning = 100;
                                    }
                                }
                            }
                            MsgNinjaUser.Create(user, user.MyNinja, user.Player.UID);
                            user.Send(stream.CreateNinjaInfo(Info));
                            break;
                        }
                    case TypeID.SaveDiscard:
                        {
                            switch (Info.ID)
                            {
                                case 1://Save
                                    {
                                        user.MyNinja.Fire = user.MyNinja.tmp_Fire;
                                        user.MyNinja.Water = user.MyNinja.tmp_Water;
                                        user.MyNinja.Earth = user.MyNinja.tmp_Earth;
                                        user.MyNinja.Wind = user.MyNinja.tmp_Wind;
                                        user.MyNinja.Lightning = user.MyNinja.tmp_Lightning;
                                        user.MyNinja.tmp_Fire = 0;
                                        user.MyNinja.tmp_Water = 0;
                                        user.MyNinja.tmp_Earth = 0;
                                        user.MyNinja.tmp_Wind = 0;
                                        user.MyNinja.tmp_Lightning = 0;
                                        break;
                                    }
                                case 0://Discard
                                    {
                                        user.MyNinja.tmp_Fire = 0;
                                        user.MyNinja.tmp_Water = 0;
                                        user.MyNinja.tmp_Earth = 0;
                                        user.MyNinja.tmp_Wind = 0;
                                        user.MyNinja.tmp_Lightning = 0;
                                        break;
                                    }

                            }
                            MsgNinjaUser.Create(user, user.MyNinja, user.Player.UID);
                            user.Send(stream.CreateNinjaInfo(Info));
                            user.MyNinja.UpdateRank();
                            break;
                        }
                    case TypeID.AddPoints:
                        {
                            if (user.Player.ConquerPoints >= (Info.Points * 10))
                            {
                                user.MyNinja.AddPoints(Info.Points);
                                user.Player.ConquerPoints -= (Info.Points * 10);
                            }
                            break;
                        }
                    case TypeID.InstantUpgrade:
                        {
                            Ninja.Item item;
                            if (user.MyNinja.TryGetValue(Info.ID, out item))
                            {
                                if ((item.Exp * 100 / item.DBItem.Exp) > Random.Next() % 100)
                                {
                                    item.Exp = 0;
                                    item.Level++;
                                }
                                else
                                {
                                    item.Exp = 0;
                                }
                                item.UPDATE();
                                MsgNinjaItem.Create(user, item, MsgNinjaItem.TypeID.Update);
                            }
                            user.MyNinja.UpdateRank();
                            user.MyNinja.GetLevel();
                            user.MyNinja.Alternate();
                            user.Equipment.QueryEquipment(user.Equipment.Alternante);
                            break;
                        }
                    default:
                        {
                            byte[] b = new byte[stream.Size];
                            fixed (byte* ptr = b)
                            {
                                stream.memcpy(ptr, stream.Memory, stream.Size);
                            }
                            PrintPacket(b);
                            break;
                        }

                }
            }
        }
        public static unsafe ServerSockets.Packet CreateNinjaUser(this ServerSockets.Packet stream, MsgNinjaUser obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(MsgNinja.MsgNinjaUser_ID);
            return stream;
        }
        public static unsafe ServerSockets.Packet CreateNinjaInfo(this ServerSockets.Packet stream, MsgNinjaInfo obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(MsgNinja.MsgNinjaInfo_ID);
            return stream;
        }
        public static unsafe ServerSockets.Packet CreateNinjaItem(this ServerSockets.Packet stream, MsgNinjaItem obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(MsgNinja.MsgNinjaItem_ID);
            return stream;
        }
        public static unsafe void GetNinjaItem(this ServerSockets.Packet stream, out MsgNinjaItem pQuery)
        {
            pQuery = new MsgNinjaItem();
            pQuery = stream.ProtoBufferDeserialize<MsgNinjaItem>(pQuery);
        }
        public static unsafe void GetNinjaInfo(this ServerSockets.Packet stream, out MsgNinjaInfo pQuery)
        {
            pQuery = new MsgNinjaInfo();
            pQuery = stream.ProtoBufferDeserialize<MsgNinjaInfo>(pQuery);
        }
    }
}
