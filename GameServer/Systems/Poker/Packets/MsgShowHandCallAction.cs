using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public static class MsgShowHandCallAction
    {
        public static unsafe ServerSockets.Packet CreateShowHandCallAction(this ServerSockets.Packet stream,ushort Action, Role.Instance.Poker.Player player)
        {
            stream.InitWriter();
            stream.Write((ushort)0);//4
            stream.Write(Action);//6
            stream.Write(player.RoundPot);//8
            stream.Write(player.TotalPot);//16
            stream.Write(player.ServerID);//24
            stream.Write(player.Uid);
            stream.Finalize(GamePackets.MsgShowHandCallAction);
            return stream;
        }
        public static unsafe void GetShowHandCallAction(this ServerSockets.Packet stream, out ushort Action, out ulong RoundPot, out ulong TotalPot, out uint UID)
        {
            stream.ReadUInt16();
            Action = stream.ReadUInt16();
            RoundPot = stream.ReadUInt64();
            TotalPot = stream.ReadUInt64();
            stream.ReadUInt32();
            UID = stream.ReadUInt32();
        }
        [PacketAttribute(GamePackets.MsgShowHandCallAction)]
        private static void Process(ConquerOnline.Client.GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {
           
            ushort Action;
            ulong TotalPot, RoundPot;
            uint UID;
            stream.GetShowHandCallAction(out Action, out RoundPot, out TotalPot, out UID);
            Role.Instance.PokerTable Table = client.PokerPlayer.Table;
            if (Table == null) return;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    UID = client.PokerPlayer.Uid;
                    if (!Table.Players.ContainsKey(client.Player.UID)) return;
                    if (Table.CurrentPlayer != client.Player.UID) return;
                    switch (Action)
                    {
                        case 1://Bet
                            {
                                if (!Table.UnLimited)
                                {
                                    if (Table.NumberOfRaise == 0)
                                    {
                                        if (Table.State == Role.Flags.TableState.Pocket || Table.State == Role.Flags.TableState.Flop)
                                        {
                                            RoundPot = Table.MinBet;
                                        }
                                        else
                                        {
                                            RoundPot = Table.MinBet * 2;
                                        }
                                    }
                                    else
                                    {
                                        if (Table.State == Role.Flags.TableState.Pocket)
                                        {
                                            RoundPot = Table.MinBet;
                                        }
                                        else
                                        {
                                            RoundPot = Table.MinBet * 2;
                                        }
                                    }
                                }
                                if (Table.IsCPs ? client.Player.ConquerPoints >= (int)RoundPot : client.Player.Money >= (long)RoundPot)
                                {
                                    if (Table.IsCPs)
                                    {
                                        client.Player.ConquerPoints -= (int)(RoundPot);
                                    }
                                    else
                                    {
                                        client.Player.Money -= (long)(RoundPot);
                                    }
                                    client.PokerPlayer.Decrement(RoundPot);
                                    client.PokerPlayer.PotinThisRound = true;
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var c = Pool.GamePoll[p.Uid];
                                            c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var streamm = recycledPacket.GetStream();
                                                {
                                                    c.Send(streamm.ActionCreate(new ActionQuery()
                                                    {
                                                        Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                        ObjId = Table.Id,
                                                        dwParam3 = (long)Table.TotalPot
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                    if (Table.Next(true))
                                    {
                                        var P3 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                c.Send(P3);
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                    }
                                }
                                break;
                            }
                        case 2://Call
                            {
                                if (Table.IsCPs ? client.Player.ConquerPoints >= (int)Table.RequiredPot : client.Player.Money >= (long)Table.RequiredPot)
                                {
                                    if (Table.IsCPs)
                                    {
                                        client.Player.ConquerPoints -= (int)(Table.RequiredPot);
                                    }
                                    else
                                    {
                                        client.Player.Money -= (long)(Table.RequiredPot);
                                    }
                                    var myTimer = new System.Timers.Timer();
                                    client.PokerPlayer.Decrement(Table.RequiredPot);
                                    client.PokerPlayer.PotinThisRound = true;
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var c = Pool.GamePoll[p.Uid];
                                            c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var streamm = recycledPacket.GetStream();
                                                {
                                                    c.Send(streamm.ActionCreate(new ActionQuery()
                                                    {
                                                        Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                        ObjId = Table.Id,
                                                        dwParam3 = (long)Table.TotalPot
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                    if (Table.Next(true))
                                    {
                                        var P3 =General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                        if (Table.RoundPot != 0)
                                        {
                                            foreach (var p in Table.PlayersOnTable())
                                            {
                                                if (Pool.GamePoll.ContainsKey(p.Uid))
                                                {
                                                    var c = Pool.GamePoll[p.Uid];
                                                    c.Send(P3);
                                                }
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                        myTimer.Elapsed += (o, ea) =>
                                        {
                                            foreach (var p in Table.PlayersOnTable())
                                            {
                                                if (Pool.GamePoll.ContainsKey(p.Uid))
                                                {
                                                    var c = Pool.GamePoll[p.Uid];
                                                    c.Send(P3);
                                                }
                                            }
                                            Table.Time = DateTime.Now.AddSeconds(10);
                                            myTimer.Stop();
                                        };
                                        if (Table.RoundPot == 0)
                                        {
                                            myTimer.Interval = 2000;
                                            myTimer.Start();
                                        }
                                    }
                                }
                                break;
                            }
                        case 4://Fold
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
                                client.PokerPlayer.PotinThisRound = true;
                                client.PokerPlayer.Fold = true;
                                client.PokerPlayer.RoundPot = 0;
                                foreach (var p in Table.PlayersOnTable())
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var c = Pool.GamePoll[p.Uid];
                                        c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                    }
                                }
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var c = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var streamm = recycledPacket.GetStream();
                                            {
                                                c.Send(streamm.ActionCreate(new ActionQuery()
                                                {
                                                    Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                    ObjId = Table.Id,
                                                    dwParam3 = (long)Table.TotalPot
                                                }));
                                            }
                                        }
                                    }
                                }
                                if (Table.Next(false))
                                {
                                    var P3 =General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var c = Pool.GamePoll[p.Uid];
                                            c.Send(P3);
                                        }
                                    }
                                    Table.Time = DateTime.Now.AddSeconds(10);
                                }
                                break;
                            }
                        case 8://Check
                            {
                                client.PokerPlayer.PotinThisRound = true;
                                foreach (var p in Table.PlayersOnTable())
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var c = Pool.GamePoll[p.Uid];
                                        c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                    }
                                }
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var c = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var streamm = recycledPacket.GetStream();
                                            {
                                                c.Send(streamm.ActionCreate(new ActionQuery()
                                                {
                                                    Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                    ObjId = Table.Id,
                                                    dwParam3 = (long)Table.TotalPot
                                                }));
                                            }
                                        }
                                    }
                                }
                                var myTimer = new System.Timers.Timer();
                                if (Table.Next(true))
                                {
                                    var P3 =General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                    Table.Time = DateTime.Now.AddSeconds(10);
                                    myTimer.Elapsed += (o, ea) =>
                                    {
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                c.Send(P3);
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                        myTimer.Stop();
                                    };
                                    myTimer.Interval = 750;
                                    myTimer.Start();
                                }
                                break;
                            }
                        case 16://Raise
                            {
                                if (!Table.UnLimited)
                                {
                                    RoundPot = Table.MinBet;
                                    if (Table.NumberOfRaise == 3)
                                    { return; }
                                }
                                if (Table.IsCPs ? client.Player.ConquerPoints >= (int)Table.RequiredPot + (int)RoundPot : client.Player.Money >= (long)Table.RequiredPot + (long)RoundPot)
                                {
                                    if (!Table.UnLimited)
                                    {
                                        Table.NumberOfRaise++;
                                    }
                                    if (Table.IsCPs)
                                    {
                                        client.Player.ConquerPoints -= (int)(Table.RequiredPot + RoundPot);
                                    }
                                    else
                                    {
                                        client.Player.Money -= ((long)Table.RequiredPot + (long)RoundPot);
                                    }
                                    client.PokerPlayer.Decrement(Table.RequiredPot + RoundPot);
                                    client.PokerPlayer.PotinThisRound = true;
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var c = Pool.GamePoll[p.Uid];
                                            c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var streamm = recycledPacket.GetStream();
                                                {
                                                    c.Send(streamm.ActionCreate(new ActionQuery()
                                                    {
                                                        Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                        ObjId = Table.Id,
                                                        dwParam3 = (long)Table.TotalPot
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                    if (Table.Next(true))
                                    {
                                        var P3 =General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                c.Send(P3);
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                    }
                                }
                                break;
                            }
                        case 32://Allin
                            {
                                if (Table.TableType == Role.Flags.TableType.TexasHoldem)
                                {
                                    if (Table.IsCPs)
                                    {
                                        RoundPot = (ulong)client.Player.ConquerPoints;
                                        client.Player.ConquerPoints = 0;
                                    }
                                    else
                                    {
                                        RoundPot = (ulong)client.Player.Money;
                                        client.Player.Money = 0;
                                    }
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
                                    client.PokerPlayer.Decrement(RoundPot);
                                    client.PokerPlayer.PotinThisRound = true;
                                    client.PokerPlayer.IsPotAllin = true;
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var c = Pool.GamePoll[p.Uid];
                                            c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var streamm = recycledPacket.GetStream();
                                                {
                                                    c.Send(streamm.ActionCreate(new ActionQuery()
                                                    {
                                                        Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                        ObjId = Table.Id,
                                                        dwParam3 = (long)Table.TotalPot
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                    bool H = false;
                                    if (Table.HighestBet(client.PokerPlayer.Uid, client.PokerPlayer.RoundPot))
                                    {
                                        H = Table.Next(true);
                                    }
                                    else
                                    {
                                        H = Table.Next(false);
                                    }
                                    if (H)
                                    {
                                        var P3 =General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                c.Send(P3);
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                    }
                                }
                                else
                                {
                                    if (Table.Showhand == false)
                                    {
                                        if (client.Player.Money > (long)Table.LowestMoney + (long)Table.RequiredPot)
                                        {
                                            RoundPot = (ulong)Table.LowestMoney + Table.RequiredPot;
                                            client.Player.Money -= (long)Table.LowestMoney + (long)Table.RequiredPot;
                                        }
                                        else
                                        {
                                            RoundPot = (ulong)Table.LowestMoney;
                                            client.Player.Money -= (long)Table.LowestMoney;
                                        }
                                        if (Table.Dealer == client.Player.UID)
                                        {
                                            Table.Dealer = Table.NextSeat(client.PokerPlayer.Seat);
                                        }
                                        client.PokerPlayer.Decrement(RoundPot);
                                        client.PokerPlayer.PotinThisRound = true;
                                        client.PokerPlayer.IsPotAllin = true;
                                        Table.Showhand = true;
                                        Table.ShowhandTotalPot = (long)client.PokerPlayer.TotalPot;
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                            }
                                        }
                                        foreach (var p in Table.OnScreen)
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Key))
                                            {
                                                var c = Pool.GamePoll[p.Key];
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var streamm = recycledPacket.GetStream();
                                                    {
                                                        c.Send(streamm.ActionCreate(new ActionQuery()
                                                        {
                                                            Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                            ObjId = Table.Id,
                                                            dwParam3 = (long)Table.TotalPot
                                                        }));
                                                    }
                                                }
                                            }
                                        }
                                        bool H = false;
                                        if (Table.HighestBet(client.PokerPlayer.Uid, client.PokerPlayer.RoundPot))
                                        {
                                            H = Table.Next(true);
                                        }
                                        else
                                        {
                                            H = Table.Next(false);
                                        }
                                        if (H)
                                        {
                                            var P3 =General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                            foreach (var p in Table.PlayersOnTable())
                                            {
                                                if (Pool.GamePoll.ContainsKey(p.Uid))
                                                {
                                                    var c = Pool.GamePoll[p.Uid];
                                                    c.Send(P3);
                                                }
                                            }
                                            Table.Time = DateTime.Now.AddSeconds(10);
                                        }
                                    }
                                    else
                                    {
                                        RoundPot = (ulong)Table.RequiredPot;
                                        client.Player.Money -= (long)Table.RequiredPot;
                                        if (Table.Dealer == client.Player.UID)
                                        {
                                            Table.Dealer = Table.NextSeat(client.PokerPlayer.Seat);
                                        }
                                        client.PokerPlayer.Decrement(RoundPot);
                                        client.PokerPlayer.PotinThisRound = true;
                                        client.PokerPlayer.IsPotAllin = true;
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                c.Send(stream.CreateShowHandCallAction(Action, client.PokerPlayer));
                                            }
                                        }
                                        foreach (var p in Table.OnScreen)
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Key))
                                            {
                                                var c = Pool.GamePoll[p.Key];
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var streamm = recycledPacket.GetStream();
                                                    {
                                                        c.Send(streamm.ActionCreate(new ActionQuery()
                                                        {
                                                            Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                            ObjId = Table.Id,
                                                            dwParam3 = (long)Table.TotalPot
                                                        }));
                                                    }
                                                }
                                            }
                                        }
                                        bool H = false;
                                        if (Table.HighestBet(client.PokerPlayer.Uid, client.PokerPlayer.RoundPot))
                                        {
                                            H = Table.Next(true);
                                        }
                                        else
                                        {
                                            H = Table.Next(false);
                                        }
                                        if (H)
                                        {
                                            var P3 =General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                            foreach (var p in Table.PlayersOnTable())
                                            {
                                                if (Pool.GamePoll.ContainsKey(p.Uid))
                                                {
                                                    var c = Pool.GamePoll[p.Uid];
                                                    c.Send(P3);
                                                }
                                            }
                                            Table.Time = DateTime.Now.AddSeconds(10);
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