using System;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgShowHandExit
    {
       
        public static unsafe ServerSockets.Packet CreateShowHandExit(this ServerSockets.Packet stream, byte Action, Role.Instance.Poker.Player player)
        {
            stream.InitWriter();
            stream.Write((uint)Action);
            stream.Write(player.Table.Number);
            stream.Write(player.Uid);
            stream.Write(player.ServerID);
            stream.Finalize(GamePackets.MsgShowHandExit);
            return stream;
        }
        public static unsafe void GetShowHandExit(this ServerSockets.Packet stream, out uint Action, out uint TableNumber, out uint PlayerUid)
        {
            
            Action = stream.ReadUInt32();
            TableNumber = stream.ReadUInt32();
            PlayerUid = stream.ReadUInt32();
        }
        [PacketAttribute(GamePackets.MsgShowHandExit)]
        public static void Process(ConquerOnline.Client.GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {
           
            uint Action;
            uint PlayerUid, TableNumber;
            stream.GetShowHandExit(out Action, out TableNumber, out PlayerUid);
            if (client.PokerPlayer == null) return;
            var Table = client.PokerPlayer.Table;
            if (Table == null) return;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    switch (Action)
                    {
                        case 1:
                            {
                                if (client.PokerPlayer != null)
                                {
                                    if (Table != null)
                                    {
                                        if (client.PokerPlayer.PlayerType == Role.Flags.PlayerType.Player || client.PokerPlayer.PlayerType == Role.Flags.PlayerType.CrossPoker)
                                        {
                                            if (!Table.Players.ContainsKey(client.Player.UID)) break;
                                            if (Table.PlayerLeave(client.PokerPlayer))
                                            {
                                                if (Table.TableType == Role.Flags.TableType.TexasHoldem)
                                                {
                                                    if (Table.SmallBlind == client.Player.UID)
                                                    {
                                                        Table.SmallBlind = Table.NextSeat(client.PokerPlayer.Seat);
                                                    }
                                                }
                                                else
                                                {
                                                    if (Table.Dealer == client.Player.UID)
                                                    {
                                                        Table.Dealer = Table.NextSeat(client.PokerPlayer.Seat);
                                                    }
                                                }
                                                client.Send(stream.CreateShowHandExit(1, client.PokerPlayer));
                                                client.Player.View.SendView(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    if (Pool.GamePoll.ContainsKey(P.Uid))
                                                    {
                                                        Pool.GamePoll[P.Uid].Send(stream.CreateShowHandExit(1, client.PokerPlayer));
                                                    }
                                                }
                                                foreach (uint key in Table.OnScreen.Keys)
                                                {
                                                    if (Pool.GamePoll.ContainsKey(key))
                                                    {
                                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                        {
                                                            var streamm = recycledPacket.GetStream();
                                                            {
                                                                Pool.GamePoll[key].Send(streamm.ActionCreate(new ActionQuery()
                                                                {
                                                                    Type = (ushort)Role.Flags.TableUpdate.PlayerCount,
                                                                    ObjId = Table.Id,
                                                                    dwParam = (uint)Table.Players.Count,
                                                                    dwParam3 = (long)Table.Players.Count
                                                                }));
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Table.InGame())
                                                {
                                                    if (Table.CurrentPlayer == client.Player.UID)
                                                    {
                                                        if (client.PokerPlayer.Table.Next(false))
                                                        {
                                                            var P3 = General.MsgShowHandActivePlayer(client.PokerPlayer.Table, 10, client.PokerPlayer.Table.CurrentPlayer);
                                                            foreach (var p in client.PokerPlayer.Table.PlayersOnTable())
                                                            {
                                                                if (Pool.GamePoll.ContainsKey(p.Uid))
                                                                {
                                                                    var c = Pool.GamePoll[p.Uid];
                                                                    c.Send(P3);
                                                                }
                                                            }
                                                            client.PokerPlayer.Table.Time = DateTime.Now.AddSeconds(10);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Table.NextRound();
                                                    }
                                                }
                                                client.PokerPlayer = null;
                                                client.Player.PokerTableID = 0;
                                                client.Player.PokerSeat = 0;
                                            }

                                        }
                                        else if (client.PokerPlayer.PlayerType == Role.Flags.PlayerType.Watcher)
                                        {
                                            if (!Table.Watchers.ContainsKey(client.Player.UID))
                                                return;
                                            if (Table.WatcherLeave(client.PokerPlayer))
                                            {
                                                client.Send(stream.CreateShowHandExit(1, client.PokerPlayer));
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    if (Pool.GamePoll.ContainsKey(P.Uid))
                                                    {
                                                        Pool.GamePoll[P.Uid].Send(stream.CreateShowHandExit(1, client.PokerPlayer));
                                                    }
                                                }
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