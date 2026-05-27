using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using VirusX.Database;
using VirusX.Role.Instance;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe void GetLottery(this ServerSockets.Packet stream, out MsgLottery.Lottery Info)
        {
            Info = new MsgLottery.Lottery();
            Info.Type = (MsgLottery.Types)stream.ReadUInt8();//4
            Info.Count = stream.ReadUInt8();//5
            stream.ReadUInt16();//6
            Info.MeltReward = stream.ReadUInt32();//8
            stream.ReadUInt32();//12
            Info.Id = stream.ReadUInt8();//16
            Info.Bliss = stream.ReadUInt8();//17
            Info.SocketOne = stream.ReadUInt8();//18
            Info.SocketTwo = stream.ReadUInt8();//19
            Info.Plus = stream.ReadUInt32();//20
            Info.Amount = stream.ReadUInt32();//24
        }
        public static unsafe ServerSockets.Packet LotteryCreate(this ServerSockets.Packet stream, MsgLottery.Lottery Info)
        {
            stream.InitWriter();
            stream.Write((byte)Info.Type);//4
            stream.Write((byte)Info.Count);//5
            stream.Write((ushort)0);//6
            stream.Write((uint)Info.MeltReward);//8
            stream.Write((uint)0);//12
            stream.Write(Info.Id);//16
            stream.Write(Info.Bliss);//17
            stream.Write(Info.SocketOne);//18
            stream.Write(Info.SocketTwo);//19
            stream.Write(Info.Plus);//20
            stream.Write(Info.Amount);//24
            foreach (var item in Info.Items)
            {
                stream.Write(item.ItemType);
                stream.Write(item.Bound);
                stream.Write(item.ChancesLeft);
                stream.Write(item.Bliss);
                stream.Write(item.SocketOne);
                stream.Write(item.SocketTwo);
                stream.Write(item.Plus);
                stream.Write(item.Amount);
            }
            stream.Finalize(GamePackets.MsgLottery);
            return stream;
        }
    }

    public class MsgLottery
    {
        public class Lottery
        {
            public List<Item> Items = new List<Item>();
            public Types Type;//4
            public byte Count;//5
            public uint MeltReward;
            public byte Id;//16
            public byte Bliss;//17
            public byte SocketOne;//18
            public byte SocketTwo;//19
            public uint Plus;//20
            public uint Amount;//24
            public class Item
            {
                public uint ItemType;//28
                public uint Bound;//32//Bound
                public byte ChancesLeft;//36 3
                public byte Bliss;//37
                public byte SocketOne;//38
                public byte SocketTwo;//39
                public uint Plus;//40
                public uint Amount;//44
            }
        }
        
        public enum Types : uint
        {
            Accept = 0,
            AddJade = 1,
            Continue = 2,
            Show = 3,
            Buy = 4,
            Luck10 = 5,
            Melt = 6,
        }
        [PacketAttribute(GamePackets.MsgLottery)]
        private static void Procesor(Client.GameClient user, ServerSockets.Packet stream)
        {
            /*user.CreateBoxDialog("This System Is Blocked By GM");
            return;*/
            if (user.InTrade || user.PokerPlayer != null || user.IsVendor)
                return;
            Lottery Info = null;
            stream.GetLottery(out Info);
            switch (Info.Type)
            {
                #region Accept
                case Types.Accept:
                    {
                        if (Info.Bliss == 1)
                        {
                            if (user.Lottery.ID >= 1 && user.Lottery.ID <= 5)
                            {
                                user.Inventory.AddItemWitchStack(711504, 0, 1, stream);
                            }
                            else if (user.Lottery.ID == 11)
                            {
                              
                               user.Inventory.AddItemWitchStack(712019, 0, 1, stream);
                            }
                        }
                        else
                        {
                            foreach (var x in user.Lottery.Items.Values)
                            {
                                if (x.MeltReward == 0)
                                {
                                    if (user.Inventory.HaveSpace(1))
                                    {
                                        Game.MsgServer.MsgGameItem GameItem = new Game.MsgServer.MsgGameItem();
                                        GameItem.ITEM_ID = x.ItemType;
                                        GameItem.Plus = (byte)x.Plus;
                                        GameItem.SocketOne = (Role.Flags.Gem)x.SocketOne;
                                        GameItem.SocketTwo = (Role.Flags.Gem)x.SocketTwo;
                                        GameItem.Bound = (byte)x.Bound;
                                        GameItem.UID = Pool.ITEM_Counter.Next;
                                        GameItem.StackSize = (ushort)x.Amount;
                                        var DBItem = Pool.ItemsBase[x.ItemType];
                                        GameItem.Durability = GameItem.MaximDurability = DBItem.Durability;
                                        user.Inventory.AddItemWitchStack(GameItem, GameItem.StackSize, stream, true);
                                    }
                                    else
                                    {
                                        uint itemid = user.Inventory.AddInbox(stream, x.ItemType, (byte)x.Plus, 0, 0, (Role.Flags.Gem)x.SocketOne, (Role.Flags.Gem)x.SocketTwo, x.Bound > 0);
                                        var Item = Pool.ItemsBase[x.ItemType];

                                        new PrizeInfo(user, Item.Name, "STR_SYSTEM_NAME@@", Item.Name, 30 * 24 * 60 * 60, 0, 0, itemid, 0, 0);

                                    }
                                }
                            }
                        }
                        user.Send(stream.LotteryCreate(Info));
                        user.Lottery.Items.Clear();
                        break;
                    }
                #endregion
                #region AddJade
                case Types.AddJade:
                    {
                        if (user.Lottery.ChancesLeft != 0)
                        {
                            uint Count = 1;
                            if (user.Lottery.ID >= 1 && user.Lottery.ID <= 5)
                            {

                                if (user.Inventory.Contain(711504, Count) || user.Inventory.Contain(711504, Count, 1))
                                {
                                    user.Inventory.Remove(711504, Count, stream);
                                }
                                else break;
                            }
                            else if (user.Lottery.ID == 11)
                            {
                                if (user.Inventory.Contain(712019, Count) || user.Inventory.Contain(712019, Count, 1))
                                {
                                    user.Inventory.Remove(712019, Count, stream);
                                }
                                else break;
                            }
                            user.Lottery.ChancesLeft--;
                            user.Lottery.Items.Clear();
                            user.Lottery.AddItem(1, user.Lottery.ID);
                            foreach (var x in user.Lottery.Items.Values)
                            {
                                Info.Items.Add(new Lottery.Item()
                                {
                                    ItemType = x.ItemType,
                                    ChancesLeft = 3,
                                    Bliss = x.Bliss,
                                    Plus = x.Plus,
                                    Bound = x.Bound,
                                    SocketOne = x.SocketOne,
                                    SocketTwo = x.SocketTwo,
                                });
                            }
                            Info.Count = (byte)(Info.Items.Count);
                            Info.Amount = user.Lottery.ChancesLeft;
                            Info.Type = Types.Show;
                            var item = Info.Items.FirstOrDefault();
                            if (item != null)
                            {
                                Info.Plus = item.Plus;
                                Info.Bliss = item.Bliss;
                                Info.SocketOne = item.SocketOne;
                                Info.SocketTwo = item.SocketTwo;
                            }
                            user.Send(stream.LotteryCreate(Info));
                        }
                        break;
                    }
                #endregion
                #region Continue
                case Types.Continue:
                    {
                        ushort Count = (ushort)(3 * 1);
                        if (Info.Id >= 1 && Info.Id <= 5)
                        {
                            //user.CreateBoxDialog("This System Locked By GM");
                            //break;
                            if (user.Inventory.Contain(711504, Count) || user.Inventory.Contain(711504, Count, 1))
                            {
                                user.Inventory.RemoveStackItem(711504, Count, stream);
                            }
                            else break;
                        }
                        else if (Info.Id == 11)
                        {
                            if (user.Inventory.Contain(712019, Count) || user.Inventory.Contain(712019, Count, 1))
                            {
                                user.Inventory.RemoveStackItem(712019, Count, stream);
                            }
                            else break;
                        }
                        else if (Info.Id == 20)
                        {
                            if (user.Inventory.Contain(3329239, 270) || user.Inventory.Contain(3329239, 270, 1))
                            {
                                user.Inventory.RemoveStackItem(3329239, 270, stream);
                            }
                            else break;
                        }
                        else if (Info.Id == 21)
                        {
                            if (user.Inventory.Contain(3329240, 270) || user.Inventory.Contain(3329239, 270, 1))
                            {
                                user.Inventory.RemoveStackItem(3329240, 270, stream);
                            }
                            else break;
                        }
                        else break;
                        user.Lottery.ChancesLeft = 2;
                        user.Lottery.Items.Clear();
                        user.Lottery.AddItem(1, Info.Id);
                        foreach (var x in user.Lottery.Items.Values)
                        {
                            Info.Items.Add(new Lottery.Item()
                            {
                                ItemType = x.ItemType,
                                ChancesLeft = 3,
                                Bliss = x.Bliss,
                                Plus = x.Plus,
                                Bound = x.Bound,
                                SocketOne = x.SocketOne,
                                SocketTwo = x.SocketTwo,
                            });
                        }
                        Info.Count = (byte)(Info.Items.Count);
                        Info.Amount = user.Lottery.ChancesLeft;
                        Info.Type = Types.Show;
                        var item = Info.Items.FirstOrDefault();
                        if (item != null)
                        {
                            Info.Plus = item.Plus;
                            Info.Bliss = item.Bliss;
                            Info.SocketOne = item.SocketOne;
                            Info.SocketTwo = item.SocketTwo;
                        }
                        user.Send(stream.LotteryCreate(Info));
                        break;
                    }
                #endregion
                #region Buy
                case Types.Buy:
                    {
                        if (Info.Id >= 1 && Info.Id <= 5)
                        {
                            //user.CreateBoxDialog("This System Locked By GM");
                            //break;
                            if (user.Inventory.HaveSpace(2))
                            {
                                if (user.Player.ConquerPoints > (Info.Amount * 400))
                                {
                                    user.Inventory.AddItemWitchStack(3321057, 0, Info.Amount, stream);
                                    user.Inventory.AddItemWitchStack(711504, 0, Info.Amount, stream);
                                    user.Player.ConquerPoints -= (long)Info.Amount * 400;
                                }
                            }
                        }
                        else
                        {
                            if (Info.Id == 11)
                            {
                                if (user.Inventory.HaveSpace(2))
                                {
                                    if (user.Player.ConquerPoints > (Info.Amount * 400))
                                    {
                                        user.Inventory.AddItemWitchStack(3321057, 0, Info.Amount, stream);
                                        user.Inventory.AddItemWitchStack(712019, 0, Info.Amount, stream);
                                        user.Player.ConquerPoints -= (long)Info.Amount * 400;
                                    }
                                }
                            }

                        }
                        user.Send(stream.LotteryCreate(Info));
                        break;
                    }
                #endregion
                #region Luck10
                case Types.Luck10:
                    {
                        ushort Count = (ushort)(3 * 10);
                        if (Info.Id >= 1 && Info.Id <= 5)
                        {
                            //user.CreateBoxDialog("This System Locked By GM");
                            //break;
                            if (user.Inventory.Contain(711504, Count) || user.Inventory.Contain(711504, Count, 1))
                            {
                                user.Inventory.RemoveStackItem(711504, Count, stream);
                            }
                            else break;
                        }
                        else if (Info.Id == 11)
                        {
                            if (user.Inventory.Contain(712019, Count) || user.Inventory.Contain(712019, Count, 1))
                            {
                                user.Inventory.RemoveStackItem(712019, Count, stream);
                            }
                            else break;
                        }
                        else if (Info.Id == 20)
                        {
                            if (user.Inventory.Contain(3329239, 2700) || user.Inventory.Contain(3329239, 2700, 1))
                            {
                                user.Inventory.RemoveStackItem(3329239, 2700, stream);
                            }
                            else break;
                        }
                        else if (Info.Id == 21)
                        {
                            if (user.Inventory.Contain(3329240, 2700) || user.Inventory.Contain(3329240, 2700, 1))
                            {
                                user.Inventory.RemoveStackItem(3329240, 2700, stream);
                            }
                            else break;
                        }
                        else break;
                        user.Lottery.Items.Clear();
                        user.Lottery.AddItem(10, Info.Id);
                        foreach (var x in user.Lottery.Items.Values)
                        {
                            if (Info.Id == 20 || Info.Id == 21)
                            {
                                Info.Items.Add(new Lottery.Item()
                                {
                                    ItemType = x.ItemType,
                                    ChancesLeft = 3,
                                    Bliss = x.Bliss,
                                    Bound = x.Bound,
                                    SocketOne = x.SocketOne,
                                    SocketTwo = x.SocketTwo,
                                });
                            }
                            else
                            {
                                Info.Items.Add(new Lottery.Item()
                                {
                                    ItemType = x.ItemType,
                                    ChancesLeft = 3,
                                    Bliss = x.Bliss,
                                    Plus = x.Plus,
                                    Bound = x.Bound,
                                    SocketOne = x.SocketOne,
                                    SocketTwo = x.SocketTwo,
                                });
                            }
                            
                           
                            if (Info.MeltReward != 0 && x.Bliss >= Info.MeltReward)
                            {
                                uint Amount = LotteryTable.MeltRewardCount(x.ItemType, user.Lottery.ID);
                                if (Amount != 0)
                                {
                                    if (user.Inventory.HaveSpace(1))
                                    {
                                        x.MeltReward = 1;
                                        if (user.Lottery.ID >= 1 && user.Lottery.ID <= 5)
                                            user.Inventory.AddItemWitchStack(3329239, 0, Amount, stream);
                                        else if (user.Lottery.ID == 11)
                                            user.Inventory.AddItemWitchStack(3329240, 0, Amount, stream);
                                    }
                                }
                            }
                        }

                        Info.Count = (byte)(Info.Items.Count);
                        var item = Info.Items.FirstOrDefault();
                        if (item != null)
                        {
                            Info.Plus = item.Plus;
                            Info.Bliss = item.Bliss;
                            Info.SocketOne = item.SocketOne;
                            Info.SocketTwo = item.SocketTwo;
                        }
                        user.Send(stream.LotteryCreate(Info));

                        break;
                    }
                #endregion
                #region Melt
                case Types.Melt:
                    {
                        var Item = user.Lottery.Items[Info.MeltReward];
                        uint Amount = LotteryTable.MeltRewardCount(Item.ItemType, user.Lottery.ID);
                        if (Amount != 0 && Item.MeltReward == 0)
                        {
                            if (user.Inventory.HaveSpace(1))
                            {
                                Item.MeltReward = 1;
                                if (user.Lottery.ID >= 1 && user.Lottery.ID <= 5)
                                    user.Inventory.AddItemWitchStack(3329239, 0, Amount, stream);
                                else if (user.Lottery.ID == 11)
                                    user.Inventory.AddItemWitchStack(3329240, 0, Amount, stream);
                                user.Send(stream.LotteryCreate(Info));
                            }
                            else
                            {
                                Item.MeltReward = 1;
                                if (user.Lottery.ID >= 1 && user.Lottery.ID <= 5)
                                {
                                    uint itemid = user.Inventory.AddInbox(stream, 3329239, (byte)0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, false, Role.Flags.ItemEffect.None, (ushort)Amount);
                                    var Itemss = Pool.ItemsBase[3329239];
                                    new PrizeInfo(user, Itemss.Name, "STR_SYSTEM_NAME@@", Itemss.Name, 30 * 24 * 60 * 60, 0, 0, itemid, 0, 0);
                                    user.Send(stream.LotteryCreate(Info));
                                    
                                }
                                else if (user.Lottery.ID == 11)
                                {
                                    user.Inventory.AddItemWitchStack(3329240, 0, Amount, stream);
                                    uint itemids = user.Inventory.AddInbox(stream, 3329240, (byte)0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, false, Role.Flags.ItemEffect.None, (ushort)Amount);
                                    var Itemsss = Pool.ItemsBase[3329240];
                                    new PrizeInfo(user, Itemsss.Name, "STR_SYSTEM_NAME@@", Itemsss.Name, 30 * 24 * 60 * 60, 0, 0, itemids, 0, 0);
                                    user.Send(stream.LotteryCreate(Info));
                                }
                                    
                               

                               
                            }
                        }
                        break;
                    }
                #endregion
            }
        }
    }
}


