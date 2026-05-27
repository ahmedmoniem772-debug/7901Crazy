using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static class MsgTrade
    {
        [ProtoContract]
        public class Trade
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Action;
            [ProtoMember(2, IsRequired = true)]
            public long Money;
            [ProtoMember(3, IsRequired = true)]
            public uint ItemUID;
            [ProtoMember(4, IsRequired = true)]
            public uint CPS;
            [ProtoMember(5, IsRequired = true)]
            public uint Unknown;
            [ProtoMember(6, IsRequired = true)]
            public uint UID;
        }
        [ProtoContract]
        public class TradeInfo
        {
            [ProtoMember(1, IsRequired = true)]
            public bool un1;
            [ProtoMember(2, IsRequired = true)]
            public uint un2;
            [ProtoMember(3, IsRequired = true)]
            public uint Sender;
            [ProtoMember(4, IsRequired = true)]
            public string Name;
            [ProtoMember(5, IsRequired = true)]
            public uint Receiver;
            [ProtoMember(6, IsRequired = true)]
            public uint Level;
            [ProtoMember(7, IsRequired = true)]
            public uint BattlePower;
            [ProtoMember(8, IsRequired = true)]
            public uint un8;
            [ProtoMember(9, IsRequired = true)]
            public uint un9;
            [ProtoMember(10, IsRequired = true)]
            public bool Spouse;
            [ProtoMember(11, IsRequired = true)]
            public bool Friend;
            [ProtoMember(12, IsRequired = true)]
            public bool TradePartner;
            [ProtoMember(13, IsRequired = true)]
            public bool Mentor;
            [ProtoMember(14, IsRequired = true)]
            public bool Apprentice;
            [ProtoMember(15, IsRequired = true)]
            public bool Teammate;
            [ProtoMember(16, IsRequired = true)]
            public bool GuildMember;
            [ProtoMember(17, IsRequired = true)]
            public bool Enemy;
            [ProtoMember(18, IsRequired = true)]
            public uint un18;
            [ProtoMember(19, IsRequired = true)]
            public uint un19;
            [ProtoMember(20, IsRequired = true)]
            public long un20;

            public ServerSockets.Packet ToArray()
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();

                    stream.InitWriter();
                    stream.ProtoBufferSerialize(this);
                    stream.Finalize(2396);
                    return stream;
                }
            }
        }

        public enum TradeID : uint
        {
            Request = 0,
            AcceptRequest = 1,
            Close = 2,
            ShowTable = 3,
            Finish = 4,
            HideTable = 5,
            AddItem = 6,
            SetMoney = 7,
            ShowMoney = 8,
            Accept = 9,
            RemoveItem = 10,
            SetCPS = 11,
            ShowCPS = 12,
        }

        public static unsafe ServerSockets.Packet CreateTradeInfo(this ServerSockets.Packet stream, Trade obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.Trade);
            return stream;
        }
        public static unsafe void GetTradeInfo(this ServerSockets.Packet stream, out Trade pQuery)
        {
            pQuery = new Trade();
            pQuery = stream.ProtoBufferDeserialize<Trade>(pQuery);
        }

        [PacketAttribute(GamePackets.Trade)]
        private static void HandlerTrade(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (!user.Player.IsCheckedPass || user.PokerPlayer != null /*|| user.Player.Name.Contains("[GM]")*/) return;
            if (user.MyTrade != null && user.MyTrade.Target != null && user.MyTrade.Target.PokerPlayer != null)
                return;

            Trade trade = new Trade();
            stream.GetTradeInfo(out trade);
            switch ((TradeID)trade.Action)
            {
                case TradeID.Request:
                    {
                        if (user.MyTrade == null)
                            user.MyTrade = new Role.Instance.Trade(user);

                        Role.IMapObj obj;
                        if (user.Player.View.TryGetValue((uint)trade.UID, out obj, Role.MapObjectType.Player))
                        {
                            Client.GameClient Partner = (obj as Role.Player).Owner;
                            if (Partner != null)
                            {
                                if (!Partner.InTrade && !user.InTrade)
                                {
                                    user.Player.targetTrade = (uint)Partner.Player.UID;
                                    user.MyTrade.Target = Partner;

                                    user.MyTrade.Target.MyTrade = new Role.Instance.Trade(user.MyTrade.Target) { Target = user };

                                    Partner.MyTrade.Target = user;

                                    user.Send(stream.CreateTradeInfo(trade));
                                    trade.UID = user.Player.UID;
                                    Partner.Send(stream.CreateTradeInfo(trade));

                                    Partner.Send(new TradeInfo()
                                    {
                                        un1 = true,
                                        Sender = user.Player.UID,
                                        Level = user.Player.Level,
                                        Receiver = Partner.Player.UID,
                                        Name = user.Player.Name,
                                        BattlePower = (uint)user.Player.BattlePower,
                                        Spouse = Partner.Player.Name == user.Player.Spouse,
                                        Enemy = user.Player.MyGuild != null && user.Player.MyGuild.Enemy.ContainsKey(Partner.Player.UID),
                                        Friend = user.Player.Associate.Contain(Role.Instance.Associate.Friends, Partner.Player.UID),
                                        TradePartner = user.Player.Associate.Contain(Role.Instance.Associate.Partener, Partner.Player.UID),
                                        Apprentice = user.Player.Associate.Contain(Role.Instance.Associate.Apprentice, Partner.Player.UID),
                                        Mentor = (user.Player.MyMentor != null && user.Player.MyMentor.MyUID == Partner.Player.UID),
                                        Teammate = (user.Team != null && user.Team.Members.ContainsKey(Partner.Player.UID)),
                                        GuildMember = (user.Player.MyGuild != null && user.Player.MyGuild.Members.ContainsKey(Partner.Player.UID)),
                                    }.ToArray());
                             
                                }
                                else
                                {
                                
                                    user.SendSysMesage("Player already in a trade.");
                                }
                            }
                        }
                        break;
                    }
                case TradeID.AcceptRequest:
                    {
                        Role.IMapObj obj;
                        if (user.Player.View.TryGetValue((uint)trade.UID, out obj, Role.MapObjectType.Player))
                        {
                            Client.GameClient Target = (obj as Role.Player).Owner;
                            if (Target != null)
                            {
                                if (!Target.InTrade && !user.InTrade)
                                {
                                    user.Player.targetTrade = (uint)Target.Player.UID;
                                    user.MyTrade.Target = Target;
                                    if (Target.Player.targetTrade == user.Player.UID && user.Player.targetTrade == Target.Player.UID)
                                    {
                                        Target.MyTrade.WindowOpen = user.MyTrade.WindowOpen = true;
                                        Target.MyTrade.Confirmed = user.MyTrade.Confirmed = false;

                                        trade.Action = (uint)TradeID.ShowTable;
                                        user.Send(stream.CreateTradeInfo(trade));

                                        trade.UID = user.Player.UID;
                                        trade.Action = (uint)TradeID.ShowTable;
                                        Target.Send(stream.CreateTradeInfo(trade));
                                    }
                                }
                            }
                        }
                        break;
                    }
                case TradeID.Close:
                    {
                        if (user.InTrade)
                        {
                            user.MyTrade.CloseTrade(trade);
                        }
                        break;
                    }
                case TradeID.AddItem:
                    {
                        if (user.InTrade)
                        {
                            Game.MsgServer.MsgGameItem DataItem;
                            if (user.Inventory.TryGetItem((uint)trade.ItemUID, out DataItem))
                                user.MyTrade.AddItem(stream, trade, DataItem);
                            
                        }
                        break;
                    }
                case TradeID.ShowCPS:
                    {
                        if (user.InTrade)
                            user.MyTrade.AddConquerPoints(trade, stream);
                        break;
                    }
                case TradeID.SetMoney:
                    {
                        if (user.InTrade)
                            user.MyTrade.AddMoney(trade, stream);
                        break;
                    }
                case TradeID.Accept:
                    {
                        try
                        {
                            if (user.MyTrade.Target.MyTrade.Target.Player.UID != user.Player.UID)
                            {
                                trade.Action = (uint)TradeID.Finish;
                                user.Send(stream.CreateTradeInfo(trade));
                                user.MyTrade = null;
                                break;
                            }
                        }
                        catch
                        {
                            trade.Action = (uint)TradeID.Finish;
                            user.Send(stream.CreateTradeInfo(trade));
                            user.MyTrade = null;
                            break;
                        }
                        if (user.InTrade)
                        {
                            if (user.MyTrade.Target.InTrade)
                            {
                                user.MyTrade.Confirmed = true;
                                if (!user.MyTrade.Target.MyTrade.Confirmed)
                                {
                                    trade.Action = (uint)TradeID.Accept;
                                    user.MyTrade.Target.Send(stream.CreateTradeInfo(trade));
                                }
                                else
                                {
                                    bool Acceped = false;
                                    if (user.Inventory.HaveSpace((byte)user.MyTrade.Target.MyTrade.Items.Count))
                                    {
                                        if (user.MyTrade.Target.MyTrade.ValidItems())
                                        {
                                            if (user.MyTrade.Target.Inventory.HaveSpace((byte)user.MyTrade.Items.Count))
                                            {
                                                if (user.MyTrade.ValidItems())
                                                {

                                                    user.Player.ConquerPoints += user.MyTrade.Target.MyTrade.ConquerPoints;
                                                   
                                                   
                                                    user.Player.Money += user.MyTrade.Target.MyTrade.Money;
                                                    user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                                   
                                                    user.MyTrade.Target.Player.ConquerPoints += user.MyTrade.ConquerPoints;
                                                    user.MyTrade.Target.Player.Money += user.MyTrade.Money;
                                                    user.MyTrade.Target.Player.SendUpdate(stream, user.MyTrade.Target.Player.Money, MsgUpdate.DataType.Money);
                                                   
                                                   
                                                    foreach (var item in user.MyTrade.Target.MyTrade.Items.Values)
                                                    {
                                                        user.Inventory.Update(item, Role.Instance.AddMode.MOVE, stream);
                                                        user.MyTrade.Target.Inventory.Update(item, Role.Instance.AddMode.REMOVE, stream, true);
                                                        
                                                    }
                                                    foreach (var item in user.MyTrade.Items.Values)
                                                    {
                                                        user.MyTrade.Target.Inventory.Update(item, Role.Instance.AddMode.MOVE, stream);
                                                        user.Inventory.Update(item, Role.Instance.AddMode.REMOVE, stream, true);
                                                      
                                                    }
                                                }
                                                Acceped = true;
                                            }
                                        }
                                    }
                                    if (!Acceped)
                                    {
                                        user.SendSysMesage("There was an error with the trade", MsgServer.MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
                                        user.MyTrade.Target.SendSysMesage("There was an error with the trade", MsgServer.MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
                                    }
                                    user.Player.targetTrade = 0;
                                    user.MyTrade.Target.Player.targetTrade = 0;
                                    trade.Action = (uint)TradeID.Finish;

                                    trade.UID = user.Player.UID;
                                    user.Send(stream.CreateTradeInfo(trade));

                                    trade.UID = user.MyTrade.Target.Player.UID;
                                    user.MyTrade.Target.Send(stream.CreateTradeInfo(trade));

                                    user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                    user.MyTrade.Target.Player.SendUpdate(stream, user.MyTrade.Target.Player.Money, MsgUpdate.DataType.Money);

                                    user.MyTrade.Target.MyTrade = null;
                                    user.MyTrade = null;
                                }
                            }
                        }
                        break;
                    }

            }
        }
    }
}
