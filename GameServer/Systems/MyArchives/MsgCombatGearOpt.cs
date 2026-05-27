using VirusX.Client;
using VirusX.Database;
using VirusX.Role.Instance;
using VirusX.ServerSockets;
using ProtoBuf;
using System;
using System.Linq;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateArchviesInfo(this ServerSockets.Packet stream, MsgCombatGearOpt.ProtoStructure obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgCombatGearOpt);
            return stream;
        }
        public static unsafe void GetArchviesInfo(this ServerSockets.Packet stream, out MsgCombatGearOpt.ProtoStructure pQuery)
        {
            pQuery = new MsgCombatGearOpt.ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<MsgCombatGearOpt.ProtoStructure>(pQuery);
        }
    }
    public static class MsgCombatGearOpt
    {
        [ProtoContract]
        public class ProtoStructure
        {
            [Flags]
            public enum TypeID
            {
                Upgrade = 0,
                Open = 1,
                Close = 2,
                Transform = 3,
                Cancel = 4,
                Add = 5,
                Remove = 6,
                New = 7,
                UpgradeTaoist = 8,
                IncreaseMastery = 9,
                UpgradeAllTaoist = 10,
                NewRuneSolts = 11,
                AddScore = 14,

                UpgradeDune = 15,
                BuyDunePoints = 16,
            }
            [Flags]
            public enum TypeExp : uint
            {
                item = 0x0,
                Plus = 0x1,
                Super = 0x2
            }
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2, IsRequired = true)]
            public Archives.TypeID ID;
            [ProtoMember(3, IsRequired = true)]
            public uint Transform = 0;
            [ProtoMember(4, IsRequired = true)]
            public uint UID = 0;
            [ProtoMember(5, IsRequired = true)]
            public uint Exp = 0;
            [ProtoMember(6, IsRequired = true)]
            public uint[] U;
            [ProtoMember(7)]
            public ItemProto[] SelectMaterial;
            [ProtoContract]
            public class ItemProto
            {
                [ProtoMember(1, IsRequired = true)]
                public uint UID = 0;
                [ProtoMember(2, IsRequired = true)]
                public uint Size = 0;
            }
            public static readonly uint[] Solt = new uint[] { 0, 2400, 7200, 14400 };
            [PacketAttribute(GamePackets.MsgCombatGearOpt)]
            private static void Process(Client.GameClient user, ServerSockets.Packet stream)
            {

                if (user.InTrade || user.PokerPlayer != null || user.IsVendor)
                    return;
                ProtoStructure obj;
              
                stream.GetArchviesInfo(out obj);
                //Console.WriteLine($"[CombatGearOpt] Type={obj.Type} | ID={obj.ID} | Transform={obj.Transform} | UID={obj.UID}");

                switch (obj.Type)
                {
                    #region Upgrade
                    case (uint)TypeID.Upgrade:
                        {
                            Archives.Item OBJ;
                            uint Progress = 0;
                            if (user.MyArchives.Items.TryGetValue(obj.ID, out OBJ))
                            {
                                MsgGameItem ITEM;
                                foreach (var items in obj.SelectMaterial)
                                {
                                    if (user.Inventory.TryGetItem(items.UID, out ITEM))
                                    {
                                        if (user.Inventory.Contain(ITEM.ITEM_ID, (ushort)items.Size)
                                            || user.Inventory.Contain(ITEM.ITEM_ID, (ushort)items.Size, 1))
                                        {
                                            Progress += GetExp(TypeExp.item, ITEM.ITEM_ID, items.Size);
                                            if (Progress == 0)
                                            {
                                                user.CreateBoxDialog("This Item Can't Use Here");
                                                break;
                                            }
                                            if (ITEM.SocketTwo != 0)
                                                Progress += 400;
                                            else if (ITEM.SocketOne != 0)
                                                Progress += 100;
                                            user.Inventory.RemoveStackItem(ITEM.ITEM_ID, (ushort)items.Size, stream);
                                        }
                                    }
                                }
                                obj.UID = 1;
                                OBJ.Progress += Progress;
                                byte level = 0;
                                if (OBJ.ItemID >= Role.Instance.Archives.TypeID.Dragonhowl && OBJ.ItemID <= Role.Instance.Archives.TypeID.Redcurse)
                                {
                                    level = 50;
                                }
                                if (OBJ.ItemID >= Role.Instance.Archives.TypeID.StoneCracker && OBJ.ItemID <= Role.Instance.Archives.TypeID.ThornCutter)
                                {
                                    level = 60;
                                }
                                if (OBJ.ItemID >= Role.Instance.Archives.TypeID.HeavenlyTiger && OBJ.ItemID <= Role.Instance.Archives.TypeID.CosmicRoc)
                                {
                                    level = 60;
                                }
                                if (OBJ.ItemID >= Role.Instance.Archives.TypeID.Dragon && OBJ.ItemID <= Role.Instance.Archives.TypeID.Suanni)
                                {
                                    level = 60;
                                }

                                if (OBJ.ItemID >= Role.Instance.Archives.TypeID.Conception && OBJ.ItemID <= Role.Instance.Archives.TypeID.Belt)
                                {
                                    level = 60;
                                }

                                while (OBJ.Progress >= OBJ.DBItem.Exp && OBJ.Level < level)
                                {
                                    ++OBJ.Level;
                                }
                                if (OBJ.Progress >= 1000000000)
                                    OBJ.Progress = 1000000000;
                                MsgCombatGear.ProtoStructure.Create(user, OBJ, MsgCombatGear.ProtoStructure.Action.Update);
                                user.Send(stream.CreateArchviesInfo(obj));
                                if (user.MyArchives.isOpen() != null)
                                    user.MyArchives.Open(user.MyArchives.isOpen().ItemID);
                                user.MyArchives.LoadingCombatHeart();
                                user.MyArchives.UpdateRank();
                            }
                            break;
                        }
                    #endregion
                    #region Open
                    case (uint)TypeID.Open:
                        {
                            #region Warrior
                            if (AtributesStatus.IsWarrior(user.Player.Class))
                            {
                                user.MyArchives.Open(obj.ID);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                                Archives.Item ItemWarrior = user.MyArchives.isOpen();
                                if (ItemWarrior != null)
                                {
                                    obj.Type = (uint)TypeID.New;
                                    obj.UID = user.Player.UID;
                                    obj.Exp = ItemWarrior.Progress;
                                    obj.Transform = ItemWarrior.dwParam;
                                    user.Player.View.SendView(stream.CreateArchviesInfo(obj), false);
                                }
                            }
                            #endregion
                            #region Archer
                            if (AtributesStatus.IsArcher(user.Player.Class))
                            {

                                user.MyArchives.Open(obj.ID);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                                Archives.Item ItemArcher = user.MyArchives.isOpen();
                                if (ItemArcher != null)
                                {
                                    obj.Type = (uint)TypeID.New;
                                    obj.UID = user.Player.UID;
                                    obj.Exp = ItemArcher.Progress;
                                    obj.Transform = ItemArcher.dwParam;
                                    user.Player.View.SendView(stream.CreateArchviesInfo(obj), false);
                                }
                            }

                            #endregion
                            #region Tao
                            if (AtributesStatus.IsTaoist(user.Player.Class))
                            {

                                user.MyArchives.Open(obj.ID);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                                Archives.Item ItemTao = user.MyArchives.isOpen();
                                if (ItemTao != null)
                                {
                                    obj.Type = (uint)TypeID.New;
                                    obj.UID = user.Player.UID;
                                    obj.Exp = ItemTao.Progress;
                                    obj.Transform = ItemTao.dwParam;
                                    user.Player.View.SendView(stream.CreateArchviesInfo(obj), false);
                                }
                            }

                            #endregion

                            #region IsMonk
                            if (AtributesStatus.IsMonk(user.Player.Class))
                            {
                                user.MyArchives.Open(obj.ID);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                                Archives.Item ItemMonk = user.MyArchives.isOpen();
                                if (ItemMonk != null)
                                {
                                    obj.Type = (uint)TypeID.New;
                                    obj.UID = user.Player.UID;
                                    obj.Exp = ItemMonk.Progress;
                                    obj.Transform = ItemMonk.dwParam;
                                    user.Player.View.SendView(stream.CreateArchviesInfo(obj), false);
                                }
                            }

                            #endregion
                            #region Pirate
                            if (AtributesStatus.IsPirate(user.Player.Class))
                            {

                                user.MyArchives.Open(obj.ID);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                                Archives.Item ItemTao = user.MyArchives.isOpen();
                                if (ItemTao != null)
                                {
                                    obj.Type = (uint)TypeID.New;
                                    obj.UID = user.Player.UID;
                                    obj.Exp = ItemTao.Progress;
                                    obj.Transform = ItemTao.dwParam;
                                    user.Player.View.SendView(stream.CreateArchviesInfo(obj), false);
                                }
                            }

                            #endregion

                            #region Dune
                            if (AtributesStatus.IsDune(user.Player.Class))
                            {

                                user.MyArchives.Open(obj.ID);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                                Archives.Item ItemDune = user.MyArchives.isOpen();
                                if (ItemDune != null)
                                {
                                    obj.Type = (uint)TypeID.New;
                                    obj.UID = user.Player.UID;
                                    obj.Exp = ItemDune.Progress;
                                    obj.Transform = ItemDune.dwParam;
                                    user.Player.View.SendView(stream.CreateArchviesInfo(obj), false);
                                }
                            }

                            #endregion
                            user.Equipment.QueryEquipment(user.Equipment.Alternante);
                            
                            break;
                        }
                    #endregion

                    #region Transform
                    case (uint)TypeID.Transform:
                        {
                            Archives.Item obj5;
                            if (AtributesStatus.IsWarrior(user.Player.Class) && user.MyArchives.Items.TryGetValue(obj.ID, out obj5) && obj5.Level == 50)
                            {
                                obj5.dwParam = obj.Transform;
                                
                                MsgCombatGear.ProtoStructure.Create(user, obj5, MsgCombatGear.ProtoStructure.Action.Update);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                            }
                            break;
                        }
                    #endregion

                    #region Cancel
                    case (uint)TypeID.Cancel:
                        {
                            Archives.Item obj7;
                            if (AtributesStatus.IsWarrior(user.Player.Class) && user.MyArchives.Items.TryGetValue(obj.ID, out obj7) && obj7.Level == 50)
                            {
                                obj7.dwParam = 0;
                                
                                MsgCombatGear.ProtoStructure.Create(user, obj7, MsgCombatGear.ProtoStructure.Action.Update);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                            }
                            break;
                        }
                    #endregion

                    #region Add
                    case (uint)TypeID.Add:
                        {
                            Archives.Item obj10;
                            if (AtributesStatus.IsWarrior(user.Player.Class) && user.MyArchives.Items.TryGetValue(obj.ID, out obj10) && obj10.Level == 50)
                            {
                                foreach (ItemProto items in obj.SelectMaterial)
                                {
                                    MsgGameItem Item;
                                    if (user.Inventory.TryGetItem(items.UID, out Item))
                                    {
                                        if (items.Size <= 7)
                                        {
                                            if ((long)obj10.Animas.Length >= (long)(obj.Transform - 1))
                                                obj10.Animas[(int)obj.Transform - 1].AnimaID[(int)items.Size - 1] = Item.ITEM_ID;
                                            user.Inventory.Remove(Item.ITEM_ID, 1, stream);
                                            MsgCombatGearSkin.Skin.Create(user, obj10);
                                        }
                                        else
                                            break;
                                    }
                                }
                                
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                            }
                            break;
                        }
                    #endregion

                    #region Remove
                    case (uint)TypeID.Remove:
                        {
                            Archives.Item obj12;
                            if (AtributesStatus.IsWarrior(user.Player.Class) && user.MyArchives.Items.TryGetValue(obj.ID, out obj12) && obj12.Level == 50)
                            {
                                foreach (ItemProto items in obj.SelectMaterial)
                                {
                                    if (items.Size <= 7)
                                    {
                                        if ((long)obj12.Animas.Length >= (long)(obj.Transform - 1))
                                        {
                                            uint ID = obj12.Animas[(int)obj.Transform - 1].AnimaID[(int)items.Size - 1];
                                            obj12.Animas[(int)obj.Transform - 1].AnimaID[(int)items.Size - 1] = 0;
                                            user.Inventory.AddItemWitchStack(ID, (byte)0, (ushort)1, stream);
                                        }
                                    }
                                    else
                                        break;
                                }
                               
                                MsgCombatGearSkin.Skin.Create(user, obj12);
                                obj.UID = 1;
                                user.Send(stream.CreateArchviesInfo(obj));
                            }
                            break;
                        }
                    #endregion

                    #region UpgradeTao
                    case (uint)TypeID.UpgradeTaoist:
                        {
                            Archives.Item weapon;
                            if (user.MyArchives.Items.TryGetValue(obj.ID, out weapon))
                            {
                                CombatGear.Items combat;
                                if (CombatGear.TryGetValue((uint)weapon.ItemID, weapon.Level, out combat))
                                {
                                    uint cost = combat.Exp - weapon.Progress;
                                    if (user.MyArchives.Reiki >= cost && weapon.Level < 60)
                                    {
                                        user.MyArchives.Reiki -= cost;
                                        user.MyArchives.AddReiki(0);

                                        weapon.Progress = combat.Exp;
                                        weapon.Level++;
                                    }
                                    MsgCombatGear.ProtoStructure.Create(user, weapon, MsgCombatGear.ProtoStructure.Action.Update);
                                    user.Send(stream.CreateArchviesInfo(obj));
                                }
                                user.MyArchives.UpdateRank();
                                if (user.MyArchives.isOpen() != null)
                                    user.MyArchives.Open(user.MyArchives.isOpen().ItemID);
                            }
                            break;
                        }
                    case (uint)TypeID.IncreaseMastery:
                        {
                            Archives.Item weapon;
                            if (user.MyArchives.MasteryPoints >= obj.Transform && user.MyArchives.Items.TryGetValue(obj.ID, out  weapon))
                            {
                                user.MyArchives.MasteryPoints -= obj.Transform;
                                weapon.MasteryPoints += obj.Transform;
                                obj.Transform = weapon.MasteryPoints;
                                obj.UID = 1;
                            }
                            user.Send(stream.CreateArchviesInfo(obj));
                            if (user.MyArchives.isOpen() != null)
                                user.MyArchives.Open(user.MyArchives.isOpen().ItemID);
                            break;
                        }
                    case (uint)TypeID.UpgradeAllTaoist:
                        {
                            Archives.Item weapon;
                            if (user.MyArchives.Items.TryGetValue(obj.ID, out  weapon))
                            {
                                CombatGear.Items combat;
                                if (CombatGear.TryGetValue((uint)weapon.ItemID, weapon.Level, out combat))
                                {
                                    uint cost = combat.Exp - weapon.Progress;
                                    while (user.MyArchives.Reiki >= cost && weapon.Level < 60)
                                    {
                                        user.MyArchives.Reiki -= cost;
                                        user.MyArchives.AddReiki(0);
                                        weapon.Level++;
                                        weapon.Progress = combat.Exp;

                                        CombatGear.TryGetValue((uint)weapon.ItemID, weapon.Level, out combat);
                                        cost = combat.Exp - weapon.Progress;
                                        obj.UID = 1;
                                    }
                                    MsgCombatGear.ProtoStructure.Create(user, weapon, MsgCombatGear.ProtoStructure.Action.Update);
                                    user.Send(stream.CreateArchviesInfo(obj));
                                }
                                user.MyArchives.UpdateRank();

                            }
                            break;
                        }
                    #endregion

                    #region Pirate New Packet Rune Plus++
                    case (uint)TypeID.NewRuneSolts:
                        {
                            Archives.Item weapon;
                            if (AtributesStatus.IsPirate(user.Player.Class) && user.MyArchives.Items.TryGetValue(obj.ID, out weapon))
                            {
                                obj.UID = 1;
                                if (user.MyArchives.UniversalFragment >= Solt[weapon.DBItem.Level] && weapon.Level < 4)
                                {
                                    user.MyArchives.UniversalFragment -= Solt[weapon.DBItem.Level];
                                    MsgCombatGearTao.AddFragemnt(user);
                                    weapon.Level++;
                                    user.Send(stream.CreateTao(new MsgCombatGearTao.ProtoStructure() { Action = MsgCombatGearTao.Action.Into, UniversalFragment = user.MyArchives.UniversalFragment }));
                                    MsgCombatGear.ProtoStructure.Create(user, weapon, MsgCombatGear.ProtoStructure.Action.Update);
                                    user.Send(stream.CreateArchviesInfo(obj));
                                }
                            }
                            user.MyArchives.UpdateNut();
                            break;
                        }
                    #endregion

                    #region PirateNewAddScore
                    case (uint)TypeID.AddScore:
                        {
                            Archives.Item weapon;
                            if (AtributesStatus.IsPirate(user.Player.Class) && user.MyArchives.Items.TryGetValue(obj.ID, out weapon))
                            {
                                if (obj.Exp == 0)
                                {
                                    if (user.MyArchives.UniversalFragment > 0)
                                    {
                                        user.MyArchives.UniversalFragment -= 1;
                                        weapon.MasteryPoints += 15;
                                    }
                                }
                                else if (obj.Exp == 1)
                                {
                                    if (user.MyArchives.UniversalFragment > 0)
                                    {
                                        weapon.MasteryPoints += user.MyArchives.UniversalFragment;
                                        user.MyArchives.UniversalFragment = 0;
                                    }

                                }
                                obj.UID = 1;
                                user.Send(stream.CreateTao(new MsgCombatGearTao.ProtoStructure() { Action = MsgCombatGearTao.Action.Into, UniversalFragment = user.MyArchives.UniversalFragment }));
                                MsgCombatGear.ProtoStructure.Create(user, weapon, MsgCombatGear.ProtoStructure.Action.Update);
                                user.Send(stream.CreateArchviesInfo(obj));
                                user.MyArchives.UpdateRank();
                                MsgCombatGearTao.AddFragemnt(user);
                            }
                            break;
                        }
                    #endregion

                    #region Upgrade Dune
                    case (uint)TypeID.UpgradeDune:
                        {
                            Archives.Item weapon;
                            if (user.MyArchives.Items.TryGetValue(obj.ID, out weapon))
                            {
                                CombatGear.Items combat;
                                if (CombatGear.TryGetValue((uint)weapon.ItemID, weapon.Level, out combat))
                                {
                                    if (weapon.Level > 27)
                                        weapon.Level = 27;

                                    uint cost = combat.Exp - weapon.Progress;
                                    if (weapon.Level < 27)
                                    {
                                        if (user.MyArchives.DunePts >= cost && weapon.Level < 27)
                                        {
                                            user.MyArchives.DunePts -= cost;
                                            user.MyArchives.UpdateDunePoints(user.MyArchives.DunePts);

                                            weapon.Progress = combat.Exp;
                                            weapon.Level++;
                                        }
                                        else
                                        {
                                            //user.MyArchives.DunePts -= cost;
                                            weapon.Progress += user.MyArchives.DunePts;
                                            user.MyArchives.DunePts = 0;
                                            user.MyArchives.UpdateDunePoints(0);
                                            //weapon.Level++;
                                        }
                                    }

                                    MsgCombatGear.ProtoStructure.Create(user, weapon, MsgCombatGear.ProtoStructure.Action.Update);
                                    user.Send(stream.CreateArchviesInfo(obj));
                                }
                                user.MyArchives.UpdateRank();
                                if (user.MyArchives.isOpen() != null)
                                {
                                    user.MyArchives.Open(user.MyArchives.isOpen().ItemID);
                                }
                            }
                            break;
                        }
                    case (uint)TypeID.BuyDunePoints:
                        {
                            //user.Player.MessageBox("Sorry, This Item isn't available from here.", null, null, 60);
                            uint Cost = 1;
                            uint count = obj.Transform * 15;
                            long totalCost = Cost * count;
                            int Amount = (int)totalCost;
                            if (Amount < 0)
                            {
                                return;
                            }

                            if (user.Player.ConquerPoints >= (totalCost))
                            {
                                user.MyArchives.AddDunePoints(count);
                                user.Player.ConquerPoints -= (uint)(totalCost);
                            }
                            else
                            {
                                totalCost = Cost * count / 15;
                                if (user.Player.ConquerPoints >= (totalCost))
                                {
                                    user.MyArchives.AddDunePoints(count);
                                    user.Player.ConquerPoints -= (uint)(totalCost);
                                }

                            }
                            break;
                        }
                        #endregion

                }

            }
            public static uint GetExp(TypeExp type, uint ID = 0, uint Size = 0)
            {

                uint Progress = 0;
                switch (type)
                {
                    default:
                        return Progress;
                    case TypeExp.item:
                        {
                            switch (ID)
                            {
                                case 3335125:
                                    Progress += 120 * Size;
                                    break;
                                case 3335126:
                                    Progress += 480 * Size;
                                    break;
                                case 3335127:
                                    Progress += 1440 * Size;
                                    break;
                                case 3335128:
                                    Progress += 4320 * Size;
                                    break;
                                case 3333400:
                                    Progress += 120 * Size;
                                    break;
                                case 3333401:
                                    Progress += 480 * Size;
                                    break;
                                case 3333402:
                                    Progress += 1440 * Size;
                                    break;
                                case 3333403:
                                    Progress += 4320 * Size;
                                    break;
                                case 3339453:
                                    Progress += 120 * Size;
                                    break;
                                case 3339454:
                                    Progress += 480 * Size;
                                    break;
                                case 3339455:
                                    Progress += 1440 * Size;
                                    break;
                                case 3339456:
                                    Progress += 4320 * Size;
                                    break;


                                case 3351308:
                                    Progress += 120 * Size;
                                    break;
                                case 3351309:
                                    Progress += 480 * Size;
                                    break;
                                case 3351310:
                                    Progress += 1440 * Size;
                                    break;
                                case 3351311:
                                    Progress += 4320 * Size;
                                    break;

                                case 3354543://Ironheart
                                    Progress += 120 * Size;
                                    break;
                                case 3354544://Crimson Heart
                                    Progress += 480 * Size;
                                    break;
                                case 3354545://Stalwart Heart
                                    Progress += 1440 * Size;
                                    break;
                                case 3354546://Gallant Heart
                                    Progress += 4320 * Size;
                                    break;

                                    //case 730001:
                                    //    Progress += 40 * Size;
                                    //    break;
                                    //case 730002:
                                    //    Progress += 160 * Size;
                                    //    break;
                                    //case 730003:
                                    //    Progress += 400 * Size;
                                    //    break;
                                    //case 730004:
                                    //    Progress += 1440 * Size;
                                    //    break;
                                    //case 730005:
                                    //    Progress += 4320 * Size;
                                    //    break;
                                    //case 730006:
                                    //    Progress += 12960 * Size;
                                    //    break;
                                    //case 730007:
                                    //    Progress += 38880 * Size;
                                    //    break;
                                    //case 730008:
                                    //    Progress += 116640 * Size;
                                    //break;
                            }
                            return Progress;
                        }
                    case TypeExp.Plus:
                        {
                            switch (ID)
                            {
                                case 1:
                                    Progress += 40;
                                    break;
                                case 2:
                                    Progress += 160;
                                    break;
                                case 3:
                                    Progress += 400;
                                    break;
                                case 4:
                                    Progress += 1440;
                                    break;
                                case 5:
                                    Progress += 4320;
                                    break;
                                case 6:
                                    Progress += 12960;
                                    break;
                                case 7:
                                    Progress += 38880;
                                    break;
                                case 8:
                                    Progress += 116640;
                                    break;
                            }
                            return Progress;
                        }
                    case TypeExp.Super:
                        {
                            switch ((byte)(ID % 10))
                            {
                                case 6:
                                    Progress++;
                                    break;
                                case 7:
                                    Progress += 2;
                                    break;
                                case 8:
                                    Progress += 6;
                                    break;
                                case 9:
                                    Progress += 20;
                                    break;
                            }

                            return Progress;
                        }
                }

            }
        }
    }
}
