using System;
using ConquerOnline;
using System.IO;
using ConquerOnline.Role;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgTexasInteractive
    {
        public static unsafe ServerSockets.Packet CreateTexasInteractive(this ServerSockets.Packet stream, Flags.TableInteractiveType InteractiveType, Role.Instance.Poker.Player player)
        {
            stream.InitWriter();

            stream.Write((uint)InteractiveType);//4
            stream.Write(player.Table.Id);//8
            stream.Write(player.Uid);//12
            stream.Write(0);//16
            stream.Write((uint)player.Seat);//20
            stream.Write(0);//24
            stream.Finalize(GamePackets.MsgTexasInteractive);
            return stream;
        }
        public static unsafe void GetTexasInteractive(this ServerSockets.Packet stream, out Flags.TableInteractiveType InteractiveType, out uint TableId, out uint PlayerUid, out byte Seat, out uint TableType)
        {


            InteractiveType = (Role.Flags.TableInteractiveType)stream.ReadUInt32();//4
            TableId = stream.ReadUInt32();//8
            PlayerUid = stream.ReadUInt32();//12
            stream.ReadUInt32();//16
            Seat = (byte)stream.ReadUInt32();//20
            TableType = stream.ReadUInt32();//24
        }
        [PacketAttribute(GamePackets.MsgTexasInteractive)]
        private static void Process(ConquerOnline.Client.GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {

            Role.Flags.TableInteractiveType InteractiveType;
            uint PlayerUid, TableId, TableType;
            byte Seat;
            stream.GetTexasInteractive(out InteractiveType, out TableId, out PlayerUid, out Seat, out TableType);
            var Table = Database.Poker.Tables[TableId];
            if (Table == null)
                return;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    switch (InteractiveType)
                    {
                        case Role.Flags.TableInteractiveType.Join:
                            {
                                if (!client.CanPlayPoker())
                                    return;
                                if (PlayerUid == client.Player.UID)
                                {
                                    if (client.PokerPlayer == null)
                                    {

                                        if (Table.OnScreen != null)
                                        {
                                            if (!Table.OnScreen.ContainsKey(client.Player.UID))
                                            { Table.OnScreen.TryAdd(client.Player.UID, client.Player.UID); }
                                        }
                                        if (Table != null)
                                        {
                                            if (!Table.CanInterface(client.Player.UID))
                                                break;
                                            ulong Money = 0;
                                            if (Table.IsCPs == false)
                                            {
                                                Money = (ulong)client.Player.Money;
                                            }
                                            else if (Table.IsCPs == true)
                                            {
                                                Money = (ulong)client.Player.ConquerPoints;
                                            }
                                            client.PokerPlayer = new Role.Instance.Poker.Player(client.Player.Name, client.Player.UID, client.Player.ServerID, client.Player.winnerall);
                                            client.PokerPlayer.Create(Role.Flags.PlayerType.Player, Seat, Table, Money);
                                            if (Table.AddPlayer(client.PokerPlayer))
                                            {
                                                client.Player.View.SendView(stream.CreateTexasInteractive(InteractiveType, client.PokerPlayer), true);
                                                client.Send(stream.CreateShowHandEnter(1, client.PokerPlayer));
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    client.Send(stream.CreateShowHandEnter(1, P));
                                                    if (P.Uid != client.Player.UID)
                                                    {
                                                        if (Pool.GamePoll.ContainsKey(P.Uid))
                                                        {
                                                            Pool.GamePoll[P.Uid].Send(stream.CreateShowHandEnter(1, client.PokerPlayer));
                                                        }
                                                    }
                                                }
                                                foreach (uint key in Table.OnScreen.Keys)
                                                {
                                                    if (Pool.GamePoll.ContainsKey(key))
                                                    {
                                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                        {
                                                            var r = recycledPacket.GetStream();
                                                            {
                                                                Pool.GamePoll[key].Send(r.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Table.InGame())
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var r = recycledPacket.GetStream();
                                                        {
                                                            client.Send(r.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                                            client.Send(new MsgShowHandLostInfo(Table));
                                                        }
                                                    }
                                                }
                                                client.Player.PokerTableID = client.PokerPlayer.Table.Id;
                                                client.Player.PokerSeat = client.PokerPlayer.Seat;
                                            }
                                            else
                                            {
                                                client.Player.PokerTableID = 0;
                                                client.Player.PokerSeat = 0;
                                                client.PokerPlayer = null;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case Role.Flags.TableInteractiveType.Watch:
                            {
                                if (PlayerUid == client.Player.UID)
                                {
                                    if (client.PokerPlayer == null)
                                    {
                                        if (Table.OnScreen != null)
                                        {
                                            if (!Table.OnScreen.ContainsKey(client.Player.UID))
                                            { Table.OnScreen.TryAdd(client.Player.UID, client.Player.UID); }
                                        }
                                        if (Table != null)
                                        {
                                            if (!Table.CanInterface(client.Player.UID))
                                                break;
                                            ulong Money = 0;
                                            if (Table.IsCPs == false)
                                            {
                                                Money = (ulong)client.Player.Money;
                                            }
                                            else if (Table.IsCPs == true)
                                            {
                                                Money = (ulong)client.Player.ConquerPoints;
                                            }
                                            client.PokerPlayer = new Role.Instance.Poker.Player(client.Player.Name, client.Player.UID, client.Player.ServerID, client.Player.winnerall);
                                            client.PokerPlayer.Create(Role.Flags.PlayerType.Watcher, Seat, Table, Money);
                                            if (Table.AddWatcher(client.PokerPlayer))
                                            {
                                                client.Send(stream.CreateShowHandEnter(1, client.PokerPlayer));
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    client.Send(stream.CreateShowHandEnter(1, P));
                                                    if (P.Uid != client.Player.UID)
                                                    {
                                                        if (Pool.GamePoll.ContainsKey(P.Uid))
                                                        {
                                                            Pool.GamePoll[P.Uid].Send(stream.CreateShowHandEnter(1, client.PokerPlayer));
                                                        }
                                                    }
                                                }
                                                if (Table.InGame())
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var r = recycledPacket.GetStream();
                                                        {
                                                            client.Send(r.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                                            client.Send(new MsgShowHandLostInfo(Table));
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                client.PokerPlayer = null;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }
    }
}
