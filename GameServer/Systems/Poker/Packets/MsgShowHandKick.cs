using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgShowHandKick
    {
        public static unsafe ServerSockets.Packet CreateShowHandKick(this ServerSockets.Packet stream, Role.Instance.Poker.Kick kick, byte Type, uint value, uint T)
        {
            stream.InitWriter();
            stream.Write(Type);
            stream.Write(kick.ServerID_Starter);
            stream.Write(kick.Starter);
            stream.Write(kick.ServerID_Target);
            stream.Write(kick.Target);
            stream.Write(value);
            stream.Write(T);

            stream.Finalize(GamePackets.MsgShowHandKick);
            return stream;
        }
        public static unsafe void GetShowHandKick(this ServerSockets.Packet stream, out byte Type, out uint Target, out byte Accept)
        {
            Type = stream.ReadUInt8();
            stream.ReadUInt32();
            stream.ReadUInt32();
            stream.ReadUInt32();
            Target = stream.ReadUInt32();
            stream.ReadUInt32();
            stream.ReadUInt32();
            Accept = stream.ReadUInt8();
        }
        [PacketAttribute(GamePackets.MsgShowHandKick)]
        public static void ShowHandKick(ConquerOnline.Client.GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {
            
            byte Type;
            uint Target;
            byte Accept;
            stream.GetShowHandKick(out Type, out Target, out Accept);
            var Table = client.PokerPlayer.Table;
            if (Table == null) return;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    switch (Type)
                    {
                        case 0:
                            {
                                if (client.PokerPlayer != null)
                                {
                                    if (!Table.InGame() && Table.CanInterface(client.Player.UID))
                                    {
                                        if (Table.Time > DateTime.Now)
                                        {
                                            if (Table.Kick == null)
                                            {
                                                Table.Kick = new Role.Instance.Poker.Kick();
                                                Table.Kick.Starter = client.Player.UID;
                                                Table.Kick.ServerID_Starter = client.Player.ServerID;
                                                Table.Kick.Target = Target;
                                                Table.Kick.ServerID_Target = Table.Players[Target].ServerID;
                                                Table.Kick.Accept = new System.Collections.Generic.List<uint>();
                                                Table.Kick.Refuse = new System.Collections.Generic.List<uint>();
                                                Table.Kick.Refuse.AddRange(Table.Players.Keys.ToList());
                                                Table.Kick.Total = (byte)Table.Players.Count;
                                                if (Table.Kick.Refuse.Remove(client.Player.UID))
                                                {
                                                    Table.Kick.Accept.Add(client.Player.UID);
                                                    uint Sec = (uint)new TimeSpan((Table.Time - DateTime.Now).Ticks).TotalSeconds;
                                                    Table.Kick.Time = DateTime.Now.AddSeconds(Sec);
                                                    foreach (var P in Table.PlayersOnTable())
                                                    {
                                                        if (Pool.GamePoll.ContainsKey(P.Uid))
                                                        {
                                                            Pool.GamePoll[P.Uid].Send(stream.CreateShowHandKick(Table.Kick, 1, Sec, 0));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case 2: //Accept
                            {
                                if (client.PokerPlayer != null)
                                {
                                    if (!Table.InGame() && Table.CanInterface(client.Player.UID) && client.PokerPlayer.PlayerType == Role.Flags .PlayerType.Player)
                                    {
                                        if (Table.Kick != null)
                                        {
                                            if (Table.Kick.Time > DateTime.Now)
                                            {
                                                if (Accept == 1)
                                                {
                                                    if (Table.Kick.Refuse.Remove(client.Player.UID))
                                                    {
                                                        Table.Kick.Accept.Add(client.Player.UID);
                                                        foreach (var P in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(P.Uid))
                                                            {
                                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                                {
                                                                    var r = recycledPacket.GetStream();
                                                                    {
                                                                        Pool.GamePoll[P.Uid].Send(r.CreateShowHandKick(Table.Kick, 3, 0, 1));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (Table.Kick.Accept.Count > Table.Kick.Refuse.Count)
                                                    {
                                                        foreach (var P in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(P.Uid))
                                                            {
                                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                                {
                                                                    var r = recycledPacket.GetStream();
                                                                    {
                                                                        Pool.GamePoll[P.Uid].Send(r.CreateShowHandKick(Table.Kick, 3, 0, 1));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        var C = Pool.GamePoll[Table.Kick.Target];
                                                        if (C != null)
                                                        {
                                                            if (C.PokerPlayer != null)
                                                            {
                                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                                {
                                                                    var r = recycledPacket.GetStream();
                                                                    {
                                                                        MsgShowHandExit.Process(C, r.CreateShowHandExit(1, C.PokerPlayer));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        Table.Kick = null;
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (var P in Table.PlayersOnTable())
                                                    {
                                                        if (Pool.GamePoll.ContainsKey(P.Uid))
                                                        {
                                                            Pool.GamePoll[P.Uid].Send(stream.CreateShowHandKick(Table.Kick, 2, client.Player.UID, 0));
                                                        }
                                                    }
                                                }
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