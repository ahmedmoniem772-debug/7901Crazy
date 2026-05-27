using System;
using System.Collections.Generic;
using System.Linq;
using ConquerOnline.Game.MsgServer;
namespace ConquerOnline
{
    public unsafe class PokerHandler
    {
        public static void ShowHand(Role.Instance.PokerTable Table)
        {
            switch (Table.State)
            {
                #region Unopened
                case Role.Flags.TableState.Unopened:
                    {
                        if (Table.Time < Table.ThreadTime)
                        {
                            #region Kick
                            if (Table.Kick != null)
                            {
                                try
                                {
                                    if (Table.Kick.Accept.Count > Table.Kick.Refuse.Count)
                                    {
                                        foreach (var P in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(P.Uid))
                                            {
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = recycledPacket.GetStream();
                                                    {
                                                        Pool.GamePoll[P.Uid].Send(stream.CreateShowHandKick(Table.Kick, 3, 0, 2));
                                                    }
                                                }
                                            }
                                        }
                                        var C = Pool.GamePoll[Table.Kick.Target];
                                        if (Table.Players.ContainsKey(Table.Kick.Target))
                                        {
                                            if (C != null)
                                            {
                                                if (C.PokerPlayer != null)
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        {
                                                            MsgShowHandExit.Process(C, stream.CreateShowHandExit(1, C.PokerPlayer));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var P in Table.PlayersOnTable())
                                        {
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    Pool.GamePoll[P.Uid].Send(stream.CreateShowHandKick(Table.Kick, 4, 0, 2));
                                                }
                                            }
                                        }
                                    }
                                    Table.Kick = null;
                                }
                                catch
                                {
                                    Table.Kick = null;
                                }
                            }
                            #endregion
                        }
                        if (Table.Players.Count > 1)
                        {
                            if (Table.Time < Table.ThreadTime)
                            {
                                Table.StartNewRound();
                                foreach (var p in Table.Players.Values.Where(p => p.IsPlaying == true))
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var client = Pool.GamePoll[p.Uid];
                                        if (Table.IsCPs)
                                        {
                                            client.Player.ConquerPoints -= (int)Table.MinBet / 2;
                                        }
                                        else
                                        {
                                            client.Player.Money -= Table.MinBet / 2;
                                        }
                                    }
                                }
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                            }
                                        }
                                    }
                                }
                                if (Table.TableIsChange == true)
                                {
                                    Table.TableIsChange = false;
                                }
                                Table.RoundState = 0;
                                Table.StartPocket();

                            }
                        }
                        break;
                    }
                #endregion
                #region Poket
                case Role.Flags.TableState.Pocket:
                    {
                        if (Table.Players.Count > 1)
                        {
                            if (Table.Time < Table.ThreadTime)
                            {
                                if (Table.RoundState == 0)
                                {
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var client = Pool.GamePoll[p.Key];
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    client.Send(stream.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));
                                                }
                                            }
                                        }
                                    }
                                    var p2 = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 1, MsgShowHandDealtCard.HandDealtCard.CardUp, 0);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            client.Send(MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.CardDown, client.Player.UID));
                                            client.Send(p2);
                                        }
                                    }
                                    Table.Time = DateTime.Now.AddSeconds(Table.Players.Values.Where(p => p.IsPlaying).ToList().Count * 1.5);
                                    Table.RoundState = 1;
                                }
                                else if (Table.RoundState == 1)
                                {
                                    var P1 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            client.Send(P1);
                                        }
                                    }
                                    Table.RoundState = 2;
                                    Table.Time = DateTime.Now.AddSeconds(10);
                                }
                                else if (Table.RoundState == 2)
                                {
                                    var player = Table.Players[Table.CurrentPlayer];
                                    player.PotinThisRound = true;
                                    player.Fold = true;
                                    player.RoundPot = 0;
                                    if (Table.Dealer == player.Uid)
                                    {
                                        Table.Dealer = Table.NextSeat(player.Seat);
                                    }
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var c = Pool.GamePoll[p.Uid];
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    c.Send(stream.CreateShowHandCallAction(4, player));
                                                }
                                            }
                                        }
                                    }
                                    if (Table.Next(false))
                                    {
                                        var P2 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var client = Pool.GamePoll[p.Uid];
                                                client.Send(P2);
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                    }
                                }
                            }
                        }
                        break;
                    }
                #endregion
                #region Flop<Turn<River
                case Role.Flags.TableState.Flop:
                case Role.Flags.TableState.Turn:
                case Role.Flags.TableState.River:
                    {
                        if (Table.Players.Count > 1)
                        {
                            if (Table.RoundState == 0)
                            {
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                            }
                                        }
                                    }
                                }
                                var P2 = Table.State == Role.Flags.TableState.Flop ? MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 1, MsgShowHandDealtCard.HandDealtCard.CardUp, 0) : Table.State == Role.Flags.TableState.Turn ? MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 1, MsgShowHandDealtCard.HandDealtCard.CardUp, 0) : Table.State == Role.Flags.TableState.River ? MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 1, MsgShowHandDealtCard.HandDealtCard.CardUp, 0) : null;
                                foreach (var p in Table.PlayersOnTable())
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var client = Pool.GamePoll[p.Uid];
                                        client.Send(P2);
                                    }
                                }
                                Table.RoundState = 1;
                            }
                            if (Table.Time < Table.ThreadTime)
                            {
                                if (Table.RoundState == 1)
                                {
                                    var player = Table.Players[Table.CurrentPlayer];
                                    if (player.TotalPot > 10000)
                                    {
                                        player.PotinThisRound = true;
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = recycledPacket.GetStream();
                                                    {
                                                        c.Send(stream.CreateShowHandCallAction(8, player));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        player.PotinThisRound = true;
                                        player.Fold = true;
                                        player.RoundPot = 0;
                                        if (Table.SmallBlind == player.Uid)
                                        {
                                            Table.SmallBlind = Table.NextSeat(player.Seat);
                                        }
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = recycledPacket.GetStream();
                                                    {
                                                        c.Send(stream.CreateShowHandCallAction(4, player));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Table.Next(false))
                                    {
                                        var P2 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var client = Pool.GamePoll[p.Uid];
                                                client.Send(P2);
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                    }
                                }
                            }
                        }
                        break;
                    }
                #endregion
                #region ShowDown
                case Role.Flags.TableState.ShowDown:
                    {

                        MsgShowHandDealtCard P1x = null;
                        MsgShowHandDealtCard P2x = null;
                        MsgShowHandDealtCard P3x = null;
                        if (Table.RoundState == 0)
                        {
                            foreach (var p in Table.OnScreen)
                            {
                                if (Pool.GamePoll.ContainsKey(p.Key))
                                {
                                    var client = Pool.GamePoll[p.Key];
                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = recycledPacket.GetStream();
                                        {
                                            client.Send(stream.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));

                                        }
                                    }
                                }
                            }
                            if (Table.PreviousState > 0)
                            {
                                if (Table.PreviousState < Role.Flags.TableState.Flop)
                                {
                                    P3x = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 3, MsgShowHandDealtCard.HandDealtCard.CardUp, 0);
                                }
                                if (Table.PreviousState == Role.Flags.TableState.Flop)
                                {
                                    P2x = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 2, MsgShowHandDealtCard.HandDealtCard.CardUp, 0);
                                }
                                if (Table.PreviousState == Role.Flags.TableState.Turn)
                                {
                                    P1x = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 1, MsgShowHandDealtCard.HandDealtCard.CardUp, 0);
                                }

                            }
                            Table.RoundState = 1;
                        }
                        if (Table.RoundState == 1)
                        {
                            try
                            {
                                Table.GetWinners();
                                Table.RoundState = 2;
                                var Result = General.MsgShowHandGameResult(Table);
                                var ShowCards = MsgShowHandLayCard.CreateShowHandLayCard(Table);
                                Table.TotalPot = 0;
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                            }
                                        }
                                    }
                                }
                                if (Table.PreviousState > 0)
                                {
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            if (Table.PreviousState < Role.Flags.TableState.Flop)
                                            {
                                                client.Send(P3x);
                                            }
                                            if (Table.PreviousState == Role.Flags.TableState.Flop)
                                            {
                                                client.Send(P2x);
                                            }
                                            if (Table.PreviousState == Role.Flags.TableState.Turn)
                                            {
                                                client.Send(P1x);
                                            }
                                        }
                                    }
                                }
                                foreach (var p in Table.PlayersOnTable())
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var client = Pool.GamePoll[p.Uid];
                                        client.Send(ShowCards);
                                        client.Send(Result);
                                        if (client.PokerPlayer.PlayerType == Role.Flags.PlayerType.Player)
                                        {
                                            if (Table.IsCPs)
                                            {
                                                client.Player.ConquerPoints = (int)p.CurrentMoney;
                                                if (client.Player.ConquerPoints < Table.MinBet * 10)
                                                {
                                                    if (Table.PlayerLeave(client.PokerPlayer))
                                                    {
                                                        foreach (var px in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(px.Uid))
                                                            {
                                                                var c = Pool.GamePoll[px.Uid];
                                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                                {
                                                                    var stream = recycledPacket.GetStream();
                                                                    {
                                                                        c.Send(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                        {
                                                            var stream = recycledPacket.GetStream();
                                                            {
                                                                client.Player.View.SendView(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                            }
                                                        }
                                                        client.PokerPlayer.Create(Role.Flags.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
                                                        Table.AddWatcher(client.PokerPlayer);
                                                        client.Player.PokerTableID = 0;
                                                        client.Player.PokerSeat = 0;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                client.Player.Money = (long)p.CurrentMoney;
                                                if (client.Player.Money < Table.MinBet * 10)
                                                {
                                                    if (Table.PlayerLeave(client.PokerPlayer))
                                                    {
                                                        foreach (var px in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(px.Uid))
                                                            {
                                                                var c = Pool.GamePoll[px.Uid];
                                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                                {
                                                                    var stream = recycledPacket.GetStream();
                                                                    {
                                                                        c.Send(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                        {
                                                            var stream = recycledPacket.GetStream();
                                                            {
                                                                client.Player.View.SendView(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                            }
                                                        }
                                                        client.PokerPlayer.Create(Role.Flags.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
                                                        Table.AddWatcher(client.PokerPlayer);
                                                        client.Player.PokerTableID = 0;
                                                        client.Player.PokerSeat = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                Table.RoundState = 2;
                            }

                        }
                        if (Table.RoundState == 2)
                        {
                            Table.State = Role.Flags.TableState.Unopened;
                            Table.RoundState = 0;
                            Table.Clear();
                            foreach (var p in Table.OnScreen)
                            {
                                if (Pool.GamePoll.ContainsKey(p.Key))
                                {
                                    var client = Pool.GamePoll[p.Key];
                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = recycledPacket.GetStream();
                                        {
                                            client.Send(stream.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                            client.Send(stream.ActionCreate(new ActionQuery() { Type = (ushort)Role.Flags.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));
                                        }
                                    }
                                }
                            }
                            Table.Time = DateTime.Now.AddSeconds(10);
                        }
                        break;
                    }
                #endregion
            }
        }
        public static void TexasHoldem(Role.Instance.PokerTable Table)
        {
            switch (Table.State)
            {
                #region Unopened
                case Role.Flags.TableState.Unopened:
                    {
                        if (Table.Time < Table.ThreadTime)
                        {
                            #region Kick
                            if (Table.Kick != null)
                            {
                                try
                                {
                                    if (Table.Kick.Accept.Count > Table.Kick.Refuse.Count)
                                    {
                                        foreach (var P in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(P.Uid))
                                            {
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = recycledPacket.GetStream();
                                                    {
                                                        Pool.GamePoll[P.Uid].Send(stream.CreateShowHandKick(Table.Kick, 3, 0, 2));
                                                    }
                                                }
                                                }
                                        }
                                        var C = Pool.GamePoll[Table.Kick.Target];
                                        if (Table.Players.ContainsKey(Table.Kick.Target))
                                        {
                                            if (C != null)
                                            {
                                                if (C.PokerPlayer != null)
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        {
                                                            MsgShowHandExit.Process(C, stream.CreateShowHandExit(1, C.PokerPlayer));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var P in Table.PlayersOnTable())
                                        {
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    Pool.GamePoll[P.Uid].Send(stream.CreateShowHandKick(Table.Kick, 4, 0, 2));
                                                }
                                            }
                                        }
                                    }
                                    Table.Kick = null;
                                }
                                catch
                                {
                                    Table.Kick = null;
                                }
                            }
                            #endregion
                        }
                        if (Table.Players.Count > 1)
                        {
                            if (Table.Time < Table.ThreadTime)
                            {
                                Table.StartNewRound();
                                foreach (var p in Table.Players.Values.Where(p => p.IsPlaying == true))
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var client = Pool.GamePoll[p.Uid];
                                        if (Table.IsCPs)
                                        {
                                            client.Player.ConquerPoints -= (int)Table.MinBet / 2;
                                            if (client.Player.UID == Table.BigBlind)
                                                client.Player.ConquerPoints -= (int)Table.MinBet;
                                            if (client.Player.UID == Table.SmallBlind)
                                                client.Player.ConquerPoints -= (int)Table.MinBet / 2;
                                        }
                                        else
                                        {
                                            client.Player.Money -= Table.MinBet / 2;
                                            if (client.Player.UID == Table.BigBlind)
                                                client.Player.Money -= Table.MinBet;
                                            if (client.Player.UID == Table.SmallBlind)
                                                client.Player.Money -= Table.MinBet / 2;
                                        }
                                    }
                                }
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.ActionCreate(new ActionQuery()
                                                {
                                                    Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                    ObjId = Table.Id,
                                                    dwParam3 = (long)Table.TotalPot
                                                }));
                                            }
                                        }
                                    }
                                }
                                if (Table.TableIsChange == true)
                                {
                                    var P3 = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 7, MsgShowHandDealtCard.HandDealtCard.OneCardDraw);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            client.Send(P3);
                                        }
                                    }
                                    Table.TableIsChange = false;
                                    Table.Time = DateTime.Now.AddSeconds(7);
                                }
                                Table.RoundState = 0;
                                Table.StartPocket();

                            }
                        }
                        break;
                    }
                #endregion
                #region Poket
                case Role.Flags.TableState.Pocket:
                    {
                        if (Table.Players.Count > 1)
                        {
                            if (Table.Time < Table.ThreadTime)
                            {
                                if (Table.RoundState == 0)
                                {
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var client = Pool.GamePoll[p.Key]; 
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    client.Send(stream.ActionCreate(new ActionQuery()
                                                    {
                                                        Type = (ushort)Role.Flags.TableUpdate.Statue,
                                                        ObjId = Table.Id,
                                                        dwParam = (uint)Table.State,
                                                        dwParam3 = (long)Table.State
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            client.Send(MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.TwoCardDraw, p.Uid));
                                        }
                                    }
                                    Table.Time = DateTime.Now.AddSeconds(Table.Players.Values.Where(p => p.IsPlaying).ToList().Count * 1.5);
                                    Table.RoundState = 1;
                                }
                                else if (Table.RoundState == 1)
                                {
                                    var P1 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            client.Send(P1);
                                        }
                                    }
                                    Table.RoundState = 2;
                                    Table.Time = DateTime.Now.AddSeconds(10);
                                }
                                else if (Table.RoundState == 2)
                                {
                                    var player = Table.Players[Table.CurrentPlayer];
                                    player.PotinThisRound = true;
                                    player.Fold = true;
                                    player.RoundPot = 0;
                                    if (Table.SmallBlind == player.Uid)
                                    {
                                        Table.SmallBlind = Table.NextSeat(player.Seat);
                                    }
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var c = Pool.GamePoll[p.Uid];
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    c.Send(stream.CreateShowHandCallAction(4, player));
                                                }
                                            }
                                        }
                                    }
                                    if (Table.Next(false))
                                    {
                                        var P2 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var client = Pool.GamePoll[p.Uid];
                                                client.Send(P2);
                                            }
                                        }
                                        Table.Time = DateTime.Now.AddSeconds(10);
                                    }
                                }
                            }
                        }
                        break;
                    }
                #endregion
                #region Flop<Turn<River
                case Role.Flags.TableState.Flop:
                case Role.Flags.TableState.Turn:
                case Role.Flags.TableState.River:
                    {
                        if (Table.Players.Count > 1)
                        {
                            if (Table.RoundState == 0)
                            {
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.ActionCreate(new ActionQuery()
                                                {
                                                    Type = (ushort)Role.Flags.TableUpdate.Statue,
                                                    ObjId = Table.Id,
                                                    dwParam = (uint)Table.State,
                                                    dwParam3 = (long)Table.State
                                                }));
                                            }
                                        }
                                    }
                                }
                                var P2 = Table.State == Role.Flags.TableState.Flop ? MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.ThreeCardDraw, 0) : Table.State == Role.Flags.TableState.Turn ? MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.FourCardDraw, 0) : Table.State == Role.Flags.TableState.River ? MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.FiveCardDraw, 0) : MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.FiveCardDraw, 0);
                                foreach (var p in Table.PlayersOnTable())
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var client = Pool.GamePoll[p.Uid];
                                        client.Send(P2);
                                    }
                                }
                                Table.RoundState = 1;
                            }
                            if (Table.Time < Table.ThreadTime)
                            {
                                if (Table.RoundState == 1)
                                {
                                    var player = Table.Players[Table.CurrentPlayer];
                                    if (Table.RoundPot == player.RoundPot)
                                    {
                                        
                                        player.PotinThisRound = true;
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = recycledPacket.GetStream();
                                                    {
                                                        c.Send(stream.CreateShowHandCallAction(8, player));
                                                    }
                                                }
                                            }
                                        }
                                        foreach (var p in Table.OnScreen)
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Key))
                                            {
                                                var c = Pool.GamePoll[p.Key];
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = recycledPacket.GetStream();
                                                    {
                                                        c.Send(stream.ActionCreate(new ActionQuery()
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
                                            var P3 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
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
                                    }
                                    else
                                    {
                                        player.PotinThisRound = true;
                                        player.Fold = true;
                                        player.RoundPot = 0;
                                        if (Table.SmallBlind == player.Uid)
                                        {
                                            Table.SmallBlind = Table.NextSeat(player.Seat);
                                        }
                                        foreach (var p in Table.PlayersOnTable())
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Uid))
                                            {
                                                var c = Pool.GamePoll[p.Uid];
                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = recycledPacket.GetStream();
                                                    {
                                                        c.Send(stream.CreateShowHandCallAction(4, player));
                                                    }
                                                }
                                            }
                                        }
                                        if (Table.Next(false))
                                        {
                                            var P2 = General.MsgShowHandActivePlayer(Table, 10, Table.CurrentPlayer);
                                            foreach (var p in Table.PlayersOnTable())
                                            {
                                                if (Pool.GamePoll.ContainsKey(p.Uid))
                                                {
                                                    var client = Pool.GamePoll[p.Uid];
                                                    client.Send(P2);
                                                }
                                            }
                                            Table.Time = DateTime.Now.AddSeconds(10);
                                        }
                                    }
                                   
                                    
                                }
                            }
                        }
                        break;
                    }
                #endregion
                #region ShowDown
                case Role.Flags.TableState.ShowDown:
                    {
                        if (Table.RoundState == 0)
                        {
                            try
                            {
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.ActionCreate(new ActionQuery()
                                                {
                                                    Type = (ushort)Role.Flags.TableUpdate.Statue,
                                                    ObjId = Table.Id,
                                                    dwParam = (uint)Table.State,
                                                    dwParam3 = (long)Table.State
                                                }));
                                            }
                                        }
                                    }
                                }
                                if (Table.PreviousState > 0)
                                {
                                    var P1 = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.ThreeCardDraw, 0);
                                    var P2 = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.FourCardDraw, 0);
                                    var P3 = MsgShowHandDealtCard.CreateShowHandDealtCard(Table, 0, MsgShowHandDealtCard.HandDealtCard.FiveCardDraw, 0);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            if (Table.PreviousState < Role.Flags.TableState.Flop)
                                            {
                                                client.Send(P1);
                                                client.Send(P2);
                                                client.Send(P3);
                                            }
                                            if (Table.PreviousState == Role.Flags.TableState.Flop)
                                            {
                                                client.Send(P2);
                                                client.Send(P3);
                                            }
                                            if (Table.PreviousState == Role.Flags.TableState.Turn)
                                            {
                                                client.Send(P3);
                                            }
                                        }
                                    }
                                }
                                Table.RoundState = 1;
                            }
                            catch
                            {
                                Table.RoundState = 1;
                            }
                        }
                        if (Table.RoundState == 1)
                        {
                            try
                            {
                                Table.GetWinners();
                                var Result = General.MsgShowHandGameResult(Table);
                                var ShowCards = MsgShowHandLayCard.CreateShowHandLayCard(Table);
                                Table.TotalPot = 0;
                                Table.RoundPot = 0;
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.ActionCreate(new ActionQuery()
                                                {
                                                    Type = (ushort)Role.Flags.TableUpdate.Chips,
                                                    ObjId = Table.Id,
                                                    dwParam3 = (long)Table.TotalPot
                                                }));
                                            }
                                        }
                                    }
                                }
                                foreach (var p in Table.PlayersOnTable())
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var client = Pool.GamePoll[p.Uid];
                                        client.Send(ShowCards);
                                        client.Send(Result);
                                        if (client.PokerPlayer.PlayerType == Role.Flags.PlayerType.Player || client.PokerPlayer.PlayerType == Role.Flags.PlayerType.CrossPoker)
                                        {
                                            if (Table.IsCPs)
                                            {
                                                client.Player.ConquerPoints = (int)p.CurrentMoney;
                                                if (client.Player.ConquerPoints < Table.MinBet * 10)
                                                {

                                                    if (Table.PlayerLeave(client.PokerPlayer))
                                                    {
                                                        foreach (var px in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(px.Uid))
                                                            {
                                                                var c = Pool.GamePoll[px.Uid];
                                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                                {
                                                                    var stream = recycledPacket.GetStream();
                                                                    {
                                                                        c.Send(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                        {
                                                            var stream = recycledPacket.GetStream();
                                                            {
                                                                client.Player.View.SendView(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                            }
                                                        }
                                                        client.PokerPlayer.Create(Role.Flags.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
                                                        Table.AddWatcher(client.PokerPlayer);
                                                        client.Player.PokerTableID = 0;
                                                        client.Player.PokerSeat = 0;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                client.Player.Money = (long)p.CurrentMoney;
                                                
                                                if (client.Player.Money < Table.MinBet * 10)
                                                {
                                                    if (Table.PlayerLeave(client.PokerPlayer))
                                                    {
                                                        foreach (var px in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(px.Uid))
                                                            {
                                                                var c = Pool.GamePoll[px.Uid];
                                                                using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                                {
                                                                    var stream = recycledPacket.GetStream();
                                                                    {
                                                                        c.Send(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                        {
                                                            var stream = recycledPacket.GetStream();
                                                            {
                                                                client.Player.View.SendView(stream.CreateTexasInteractive(Role.Flags.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                            }
                                                        }
                                                        client.PokerPlayer.Create(Role.Flags.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
                                                        Table.AddWatcher(client.PokerPlayer);
                                                        client.Player.PokerTableID = 0;
                                                        client.Player.PokerSeat = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Table.RoundState = 2;
                            }
                            catch
                            {
                                Table.RoundState = 2;
                            }

                        }
                        if (Table.RoundState == 2)
                        {
                            Table.State = Role.Flags.TableState.Unopened;
                            Table.RoundState = 0;
                            Table.Clear();
                            foreach (var p in Table.OnScreen)
                            {
                                if (Pool.GamePoll.ContainsKey(p.Key))
                                {
                                    var client = Pool.GamePoll[p.Key];
                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = recycledPacket.GetStream();
                                        {
                                            client.Send(stream.ActionCreate(new ActionQuery()
                                            {
                                                Type = (ushort)Role.Flags.TableUpdate.Statue,
                                                ObjId = Table.Id,
                                                dwParam = (uint)Table.State,
                                                dwParam3 = (long)Table.State
                                            }));
                                        }
                                    }
                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = recycledPacket.GetStream();
                                        {
                                            client.Send(stream.ActionCreate(new ActionQuery()
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
                            Table.Time = DateTime.Now.AddSeconds(10);
                        }
                        foreach (var px in Table.PlayersOnTable())
                        {
                            if (Pool.GamePoll.ContainsKey(px.Uid))
                            {
                                var c = Pool.GamePoll[px.Uid];
                                if (c.Fake)
                                {
                                    c.Player.Money = c.Player.RelodMoney;
                                }
                            }
                        }
                        break;
                    }
                #endregion
                default:
                    {
                        Console.WriteLine("Unhandle Poker Table State: " + Table.State);
                        break;
                    }
            }
        }
        
        public static void PokerTablesCallback(Role.Instance.PokerTable Table, int time)
        {
            Table.ThreadTime = DateTime.Now;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    switch (Table.TableType)
                    {
                        case Role.Flags.TableType.ShowHand:
                            {
                                ShowHand(Table);
                                break;
                            }
                        case Role.Flags.TableType.TexasHoldem:
                            {
                                try
                                {
                                    TexasHoldem(Table);
                                }
                                catch
                                {
                                    MyConsole.WriteLine(Table.Id.ToString());
                                }
                                break;
                            }
                    }
                }
            }
        }
    }
}