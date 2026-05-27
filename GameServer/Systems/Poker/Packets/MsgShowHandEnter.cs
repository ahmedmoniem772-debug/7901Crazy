using System;
using ConquerOnline;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgShowHandEnter
    {

        public static unsafe ServerSockets.Packet CreateShowHandEnter(this ServerSockets.Packet stream, byte Action, Role.Instance.Poker.Player player)
        {
            stream.InitWriter();

            stream.Write(Action);
            stream.Write((ushort)player.PlayerType);//5
            stream.Write((ushort)player.Seat);//7
            stream.Write(player.Table.Number);//9
            stream.Write(player.ServerID);//13
            stream.Write(player.Uid);//17
            stream.Write((ulong)0);//21
            stream.Write(player.Table.Id);//29
            if (player.Table.TableType == Role.Flags.TableType.ShowHand)
                stream.Write((uint)3);//33
            else
                stream.Write((uint)6);



            stream.Finalize(GamePackets.MsgShowHandEnter);
            return stream;
        }
        public static unsafe void GetShowHandEnter(this ServerSockets.Packet stream, out byte Action, out Role.Flags.PlayerType PlayerType, out ushort Seat, out uint TableNumber, out uint PlayerUid, out Role.Flags.TableType TableType)
        {

            Action = stream.ReadUInt8();
            PlayerType = (Role.Flags.PlayerType)stream.ReadUInt16();
            Seat = stream.ReadUInt16();
            TableNumber = stream.ReadUInt32();
            PlayerUid = stream.ReadUInt32();
            TableType = (Role.Flags.TableType)stream.ReadInt8();
        }

        [PacketAttribute(GamePackets.MsgShowHandEnter)]
        private static void Process(ConquerOnline.Client.GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {
           
            byte Action;
            uint PlayerUid, TableNumber;
            ushort Seat;
            Role.Flags.PlayerType PlayerType;
            Role.Flags.TableType TableType;
            stream.GetShowHandEnter(out Action, out PlayerType, out Seat, out TableNumber, out PlayerUid, out TableType);
            switch (Action)
            {
                case 0:
                    {
                        if (client.PokerPlayer != null)
                        {
                            if (!client.CanPlayPoker()) return;
                            if (client.PokerPlayer.PlayerType == Role.Flags.PlayerType.Watcher)
                            {
                                if (client.PokerPlayer.Table.Number == TableNumber)
                                {
                                    ulong Money = 0;
                                    if (client.PokerPlayer.Table.IsCPs == false)
                                    {
                                        Money = (ulong)client.Player.Money;
                                    }
                                    else if (client.PokerPlayer.Table.IsCPs == true)
                                    {
                                        Money = (ulong)client.Player.ConquerPoints;
                                    }
                                    client.PokerPlayer.Create(Role.Flags.PlayerType.Player, (byte)Seat, client.PokerPlayer.Table, Money);
                                    if (client.PokerPlayer.Table.AddPlayer(client.PokerPlayer))
                                    {
                                        client.Player.View.SendView(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Join, client.PokerPlayer), true);
                                        client.Send(stream.CreateShowHandEnter(1, client.PokerPlayer));
                                        foreach (var P in client.PokerPlayer.Table.PlayersOnTable())
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
                                        foreach (uint key in client.PokerPlayer.Table.OnScreen.Keys)
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
                                                            ObjId = client.PokerPlayer.Table.Id,
                                                            dwParam = (uint)client.PokerPlayer.Table.Players.Count,
                                                            dwParam3 = (long)client.PokerPlayer.Table.Players.Count
                                                        }));
                                                    }
                                                }
                                            }
                                        }
                                        client.Player.PokerTableID = client.PokerPlayer.Table.Id;
                                        client.Player.PokerSeat = client.PokerPlayer.Seat;
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
