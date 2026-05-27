using System;
using System.Collections.Generic;
using System.Linq;
using VirusX.Game.MsgServer;
using VirusX.ServerSockets;
using VirusX;
using Poker;
namespace VirusX
{
    public unsafe class PokerHandler
    {
        public static void ShowHand(PokerTable Table)
        {
            switch (Table.State)
            {
                #region Unopened
                case Poker.General.TableState.Unopened:
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
                                                Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 3, 0, 2));
                                            }
                                        }
                                        var C = Pool.GamePoll[Table.Kick.Target];
                                        if (Table.Players.ContainsKey(Table.Kick.Target))
                                        {
                                            if (C != null)
                                            {
                                                if (C.PokerPlayer != null)
                                                {
                                                    ServerSockets.Packet stream = new ServerSockets.Packet(Poker.MsgShowHandExit.CreateShowHandExit(1, C.PokerPlayer));
                                                    MsgShowHandExit(C, stream);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var P in Table.PlayersOnTable())
                                        {
                                            Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 4, 0, 2));
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
                                            client.Player.ConquerPoints -= (uint)(Table.MinBet / 2);
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
                                        client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
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
                case Poker.General.TableState.Pocket:
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
                                            client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));
                                        }
                                    }
                                    var p2 = CMsgShowHandDealtCard.Create(Table, 1, CMsgShowHandDealtCard.HandDealtCard.CardUp, 0);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            client.Send(CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.CardDown, client.Player.UID));
                                            client.Send(p2);
                                        }
                                    }
                                    Table.Time = DateTime.Now.AddSeconds(Table.Players.Values.Where(p => p.IsPlaying).ToList().Count * 1.5);
                                    Table.RoundState = 1;
                                }
                                else if (Table.RoundState == 1)
                                {
                                    var P1 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(4, player));
                                        }
                                    }
                                    if (Table.Next(false))
                                    {
                                        var P2 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                case Poker.General.TableState.Flop:
                case Poker.General.TableState.Turn:
                case Poker.General.TableState.River:
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

                                        client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                    }
                                }
                                var P2 = Table.State == Poker.General.TableState.Flop ? CMsgShowHandDealtCard.Create(Table, 1, CMsgShowHandDealtCard.HandDealtCard.CardUp, 0) : Table.State == Poker.General.TableState.Turn ? CMsgShowHandDealtCard.Create(Table, 1, CMsgShowHandDealtCard.HandDealtCard.CardUp, 0) : Table.State == Poker.General.TableState.River ? CMsgShowHandDealtCard.Create(Table, 1, CMsgShowHandDealtCard.HandDealtCard.CardUp, 0) : null;
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(4, player));
                                        }
                                    }
                                    if (Table.Next(false))
                                    {
                                        var P2 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                case Poker.General.TableState.ShowDown:
                    {
                        byte[] P1x = null;
                        byte[] P2x = null;
                        byte[] P3x = null;
                        if (Table.RoundState == 0)
                        {
                            foreach (var p in Table.OnScreen)
                            {
                                if (Pool.GamePoll.ContainsKey(p.Key))
                                {
                                    var client = Pool.GamePoll[p.Key];
                                    client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));

                                }
                            }
                            if (Table.PreviousState > 0)
                            {
                                if (Table.PreviousState < Poker.General.TableState.Flop)
                                {
                                    P3x = CMsgShowHandDealtCard.Create(Table, 3, CMsgShowHandDealtCard.HandDealtCard.CardUp, 0);
                                }
                                if (Table.PreviousState == Poker.General.TableState.Flop)
                                {
                                    P2x = CMsgShowHandDealtCard.Create(Table, 2, CMsgShowHandDealtCard.HandDealtCard.CardUp, 0);
                                }
                                if (Table.PreviousState == Poker.General.TableState.Turn)
                                {
                                    P1x = CMsgShowHandDealtCard.Create(Table, 1, CMsgShowHandDealtCard.HandDealtCard.CardUp, 0);
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
                                var Result = CMsgShowHandGameResult.Create(Table);
                                var ShowCards = CMsgShowHandLayCard.Create(Table);
                                Table.TotalPot = 0;
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                    }
                                }
                                if (Table.PreviousState > 0)
                                {
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            if (Table.PreviousState < Poker.General.TableState.Flop)
                                            {
                                                client.Send(P3x);
                                            }
                                            if (Table.PreviousState == Poker.General.TableState.Flop)
                                            {
                                                client.Send(P2x);
                                            }
                                            if (Table.PreviousState == Poker.General.TableState.Turn)
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
                                        if (client.PokerPlayer.PlayerType == Poker.General.PlayerType.Player)
                                        {
                                            if (Table.IsCPs)
                                            {
                                                client.Player.ConquerPoints = (uint)p.CurrentMoney;
                                                if (client.Player.ConquerPoints < Table.MinBet * 10)
                                                {
                                                    if (Table.PlayerLeave(client.PokerPlayer))
                                                    {
                                                        foreach (var px in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(px.Uid))
                                                            {
                                                                var c = Pool.GamePoll[px.Uid];
                                                                c.Send(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer));
                                                            }
                                                        }
                                                        client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer), true);

                                                        client.PokerPlayer.Create(Poker.General.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
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
                                                                c.Send(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer));
                                                            }
                                                        }
                                                        client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                        client.PokerPlayer.Create(Poker.General.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
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
                            Table.State = Poker.General.TableState.Unopened;
                            Table.RoundState = 0;
                            Table.Clear();
                            foreach (var p in Table.OnScreen)
                            {
                                if (Pool.GamePoll.ContainsKey(p.Key))
                                {
                                    var client = Pool.GamePoll[p.Key];

                                    client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                    client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));
                                }
                            }
                            Table.Time = DateTime.Now.AddSeconds(10);
                        }
                        break;
                    }
                    #endregion
            }
        }
        public static void TexasHoldem(PokerTable Table)
        {
            switch (Table.State)
            {
                #region Unopened
                case Poker.General.TableState.Unopened:
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
                                                Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 3, 0, 2));
                                            }
                                        }
                                        var C = Pool.GamePoll[Table.Kick.Target];
                                        if (Table.Players.ContainsKey(Table.Kick.Target))
                                        {
                                            if (C != null)
                                            {
                                                if (C.PokerPlayer != null)
                                                {
                                                    ServerSockets.Packet stream = new ServerSockets.Packet(Poker.MsgShowHandExit.CreateShowHandExit(1, C.PokerPlayer));
                                                    MsgShowHandExit(C, stream);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var P in Table.PlayersOnTable())
                                        {
                                            Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 4, 0, 2));
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
                                            client.Player.ConquerPoints -= (uint)Table.MinBet / 2;
                                            if (client.Player.UID == Table.BigBlind)
                                                client.Player.ConquerPoints -= (uint)Table.MinBet;
                                            if (client.Player.UID == Table.SmallBlind)
                                                client.Player.ConquerPoints -= (uint)Table.MinBet / 2;
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
                                        client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                    }
                                }
                                if (Table.TableIsChange == true)
                                {

                                    var P3 = CMsgShowHandDealtCard.Create(Table, 7, CMsgShowHandDealtCard.HandDealtCard.OneCardDraw);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            //    client.Send(P2);
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
                case Poker.General.TableState.Pocket:
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
                                            client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                        }
                                    }
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            client.Send(CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.TwoCardDraw, p.Uid));
                                        }
                                    }
                                    Table.Time = DateTime.Now.AddSeconds(Table.Players.Values.Where(p => p.IsPlaying).ToList().Count * 1.5);
                                    Table.RoundState = 1;
                                }
                                else if (Table.RoundState == 1)
                                {
                                    var P1 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(4, player));
                                        }
                                    }
                                    if (Table.Next(false))
                                    {
                                        var P2 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                case Poker.General.TableState.Flop:
                case Poker.General.TableState.Turn:
                case Poker.General.TableState.River:
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

                                        client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                    }
                                }
                                var P2 = Table.State == Poker.General.TableState.Flop ? CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.ThreeCardDraw, 0) : Table.State == Poker.General.TableState.Turn ? CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.FourCardDraw, 0) : Table.State == Poker.General.TableState.River ? CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.FiveCardDraw, 0) : CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.FiveCardDraw, 0);
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(4, player));
                                        }
                                    }
                                    if (Table.Next(false))
                                    {
                                        var P2 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                case Poker.General.TableState.ShowDown:
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
                                        client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                    }
                                }
                                if (Table.PreviousState > 0)
                                {
                                    var P1 = CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.ThreeCardDraw, 0);
                                    var P2 = CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.FourCardDraw, 0);
                                    var P3 = CMsgShowHandDealtCard.Create(Table, 0, CMsgShowHandDealtCard.HandDealtCard.FiveCardDraw, 0);
                                    foreach (var p in Table.PlayersOnTable())
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Uid))
                                        {
                                            var client = Pool.GamePoll[p.Uid];
                                            if (Table.PreviousState < Poker.General.TableState.Flop)
                                            {
                                                client.Send(P1);
                                                client.Send(P2);
                                                client.Send(P3);
                                            }
                                            if (Table.PreviousState == Poker.General.TableState.Flop)
                                            {
                                                client.Send(P2);
                                                client.Send(P3);
                                            }
                                            if (Table.PreviousState == Poker.General.TableState.Turn)
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
                                var Result = CMsgShowHandGameResult.Create(Table);
                                var ShowCards = CMsgShowHandLayCard.Create(Table);
                                Table.TotalPot = 0;
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var client = Pool.GamePoll[p.Key];
                                        client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                    }
                                }
                                foreach (var p in Table.PlayersOnTable())
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Uid))
                                    {
                                        var client = Pool.GamePoll[p.Uid];
                                        client.Send(ShowCards);
                                        client.Send(Result);
                                        if (client.PokerPlayer.PlayerType == Poker.General.PlayerType.Player)
                                        {
                                            if (Table.IsCPs)
                                            {
                                                client.Player.ConquerPoints = (uint)p.CurrentMoney;
                                                if (client.Player.ConquerPoints < Table.MinBet * 10)
                                                {
                                                    if (Table.PlayerLeave(client.PokerPlayer))
                                                    {
                                                        foreach (var px in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(px.Uid))
                                                            {
                                                                var c = Pool.GamePoll[px.Uid];
                                                                c.Send(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer));
                                                            }
                                                        }
                                                        client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                        client.PokerPlayer.Create(Poker.General.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
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
                                                                c.Send(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer));
                                                            }
                                                        }
                                                        client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                        client.PokerPlayer.Create(Poker.General.PlayerType.Watcher, client.PokerPlayer.Seat, Table, (ulong)client.PokerPlayer.CurrentMoney);
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
                            Table.State = Poker.General.TableState.Unopened;
                            Table.RoundState = 0;
                            Table.Clear();
                            foreach (var p in Table.OnScreen)
                            {
                                if (Pool.GamePoll.ContainsKey(p.Key))
                                {
                                    var client = Pool.GamePoll[p.Key];
                                    client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Statue, ObjId = Table.Id, dwParam = (uint)Table.State, dwParam3 = (long)Table.State }));
                                    client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));

                                }
                            }
                            Table.Time = DateTime.Now.AddSeconds(10);
                        }
                        break;
                    }
                    #endregion
            }
        }
        public static void PokerTablesCallback(PokerTable Table, int time)
        {
            Table.ThreadTime = DateTime.Now;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    switch (Table.TableType)
                    {
                        case Poker.General.TableType.ShowHand:
                            {
                                ShowHand(Table);
                                break;
                            }
                        case Poker.General.TableType.TexasHoldem:
                            {
                                TexasHoldem(Table);
                                break;
                            }
                    }
                }
            }
        }
        [PacketAttribute(PacketsID.CMsgShowHandKick)]
        private static void MsgShowHandKick(Client.GameClient client, ServerSockets.Packet stream)
        {
            byte[] Packet = new byte[stream.Size];
            fixed (byte* ptr = Packet)
            {
                stream.memcpy(ptr, stream.Memory, stream.Size);
            }
            byte Type;
            uint Target;
            byte Accept;
            Poker.MsgShowHandKick.GetShowHandKick(Packet, out Type, out Target, out Accept);
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
                                                Table.Kick = new Poker.Kick();
                                                Table.Kick.Starter = client.Player.UID;
                                                Table.Kick.ServerID_Starter = client.Player.ServerID;
                                                Table.Kick.Target = Target;
                                                Table.Kick.ServerID_Target = Table.Players[Target].ServerID;
                                                Table.Kick.Accept = new List<uint>();
                                                Table.Kick.Refuse = new List<uint>();
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
                                                            Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 1, Sec, 0));
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
                                    if (!Table.InGame() && Table.CanInterface(client.Player.UID) && client.PokerPlayer.PlayerType == Poker.General.PlayerType.Player)
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
                                                                Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 2, client.Player.UID, 1));
                                                            }
                                                        }
                                                    }
                                                    if (Table.Kick.Accept.Count > Table.Kick.Refuse.Count)
                                                    {
                                                        foreach (var P in Table.PlayersOnTable())
                                                        {
                                                            if (Pool.GamePoll.ContainsKey(P.Uid))
                                                            {
                                                                Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 3, 0, 1));
                                                            }
                                                        }
                                                        var C = Pool.GamePoll[Table.Kick.Target];
                                                        if (C != null)
                                                        {
                                                            if (C.PokerPlayer != null)
                                                            {
                                                                ServerSockets.Packet stream_1 = new ServerSockets.Packet(Poker.MsgShowHandExit.CreateShowHandExit(1, C.PokerPlayer));
                                                                MsgShowHandExit(C, stream_1);


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
                                                            Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandKick.CreateShowHandKick(Table.Kick, 2, client.Player.UID, 0));
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
        [PacketAttribute(PacketsID.CMsgShowHandExit)]
        public static void MsgShowHandExit(Client.GameClient client, ServerSockets.Packet stream)
        {

            byte[] Packet = new byte[stream.Size];
            fixed (byte* ptr = Packet)
            {
                stream.memcpy(ptr, stream.Memory, stream.Size);
            }
            uint Action;
            uint PlayerUid, TableNumber;
            Poker.MsgShowHandExit.GetShowHandExit(Packet, out Action, out TableNumber, out PlayerUid);
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
                                        if (client.PokerPlayer.PlayerType == Poker.General.PlayerType.Player)
                                        {
                                            if (!Table.Players.ContainsKey(client.Player.UID))
                                                break;
                                            if (Table.PlayerLeave(client.PokerPlayer))
                                            {
                                                if (Table.TableType == Poker.General.TableType.TexasHoldem)
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
                                                client.Send(Poker.MsgShowHandExit.CreateShowHandExit(1, client.PokerPlayer));
                                                client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Leave, client.PokerPlayer), true);
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    if (Pool.GamePoll.ContainsKey(P.Uid))
                                                    {
                                                        Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandExit.CreateShowHandExit(1, client.PokerPlayer));
                                                    }
                                                }
                                                foreach (uint key in Table.OnScreen.Keys)
                                                {
                                                    if (Pool.GamePoll.ContainsKey(key))
                                                    {
                                                        Pool.GamePoll[key].Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));
                                                    }
                                                }
                                                if (Table.InGame())
                                                {
                                                    if (Table.CurrentPlayer == client.Player.UID)
                                                    {
                                                        if (client.PokerPlayer.Table.Next(false))
                                                        {
                                                            var P3 = CMsgShowHandActivePlayer.Create(client.PokerPlayer.Table, 10, client.PokerPlayer.Table.CurrentPlayer);
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
                                        else if (client.PokerPlayer.PlayerType == Poker.General.PlayerType.Watcher)
                                        {
                                            if (!Table.Watchers.ContainsKey(client.Player.UID))
                                                return;
                                            if (Table.WatcherLeave(client.PokerPlayer))
                                            {
                                                client.Send(Poker.MsgShowHandExit.CreateShowHandExit(1, client.PokerPlayer));
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    if (Pool.GamePoll.ContainsKey(P.Uid))
                                                    {
                                                        Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandExit.CreateShowHandExit(1, client.PokerPlayer));
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
        [PacketAttribute(PacketsID.CMsgShowHandEnter)]
        private static void MsgShowHandEnter(Client.GameClient client, ServerSockets.Packet stream)
        {
            byte[] Packet = new byte[stream.Size];
            fixed (byte* ptr = Packet)
            {
                stream.memcpy(ptr, stream.Memory, stream.Size);
            }
            byte Action;
            uint PlayerUid, TableNumber;
            ushort Seat;
            Poker.General.PlayerType PlayerType;
            Poker.General.TableType TableType;
            Poker.MsgShowHandEnter.GetShowHandEnter(Packet, out Action, out PlayerType, out Seat, out TableNumber, out PlayerUid, out TableType);
            switch (Action)
            {
                case 0:
                    {
                        if (client.PokerPlayer != null)
                        {
                            if (!client.CanPlayPoker())
                                return;
                            if (client.PokerPlayer.PlayerType == Poker.General.PlayerType.Watcher)
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
                                    client.PokerPlayer.Create(Poker.General.PlayerType.Player, (byte)Seat, client.PokerPlayer.Table, Money);
                                    if (client.PokerPlayer.Table.AddPlayer(client.PokerPlayer))
                                    {
                                        client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(Poker.General.TableInteractiveType.Join, client.PokerPlayer), true);
                                        client.Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, client.PokerPlayer));
                                        foreach (var P in client.PokerPlayer.Table.PlayersOnTable())
                                        {
                                            client.Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, P));
                                            if (P.Uid != client.Player.UID)
                                            {
                                                if (Pool.GamePoll.ContainsKey(P.Uid))
                                                {
                                                    Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, client.PokerPlayer));
                                                }
                                            }
                                        }
                                        foreach (uint key in client.PokerPlayer.Table.OnScreen.Keys)
                                        {
                                            if (Pool.GamePoll.ContainsKey(key))
                                            {
                                                Pool.GamePoll[key].Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.PlayerCount, ObjId = client.PokerPlayer.Table.Id, dwParam = (uint)client.PokerPlayer.Table.Players.Count, dwParam3 = (long)client.PokerPlayer.Table.Players.Count }));
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
        [PacketAttribute(PacketsID.CMsgShowHandCallAction)]
        private static void MsgShowHandCallAction(Client.GameClient client, ServerSockets.Packet stream)
        {
            byte[] Packet = new byte[stream.Size];
            fixed (byte* ptr = Packet)
            {
                stream.memcpy(ptr, stream.Memory, stream.Size);
            }
            ushort Action;
            ulong TotalPot, RoundPot;
            uint UID;
            Poker.MsgShowHandCallAction.GetShowHandCallAction(Packet, out Action, out RoundPot, out TotalPot, out UID);
            PokerTable Table = client.PokerPlayer.Table;
            if (Table == null)
                return;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    UID = client.PokerPlayer.Uid;
                    if (!Table.Players.ContainsKey(client.Player.UID))
                        return;
                    if (Table.CurrentPlayer != client.Player.UID)
                        return;
                    switch (Action)
                    {
                        case 1://Bet
                            {
                                if (!Table.UnLimited)
                                {
                                    if (Table.NumberOfRaise == 0)
                                    {
                                        if (Table.State == Poker.General.TableState.Pocket || Table.State == Poker.General.TableState.Flop)
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
                                        if (Table.State == Poker.General.TableState.Pocket)
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
                                        client.Player.ConquerPoints -= (uint)(RoundPot);
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                        }
                                    }
                                    if (Table.Next(true))
                                    {
                                        var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                        client.Player.ConquerPoints -= (uint)(Table.RequiredPot);
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                        }
                                    }
                                    if (Table.Next(true))
                                    {
                                        var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                if (Table.TableType == Poker.General.TableType.TexasHoldem)
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
                                        c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));
                                    }
                                }
                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var c = Pool.GamePoll[p.Key];
                                        c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                    }
                                }
                                if (Table.Next(false))
                                {
                                    var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                        c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));
                                    }
                                }

                                foreach (var p in Table.OnScreen)
                                {
                                    if (Pool.GamePoll.ContainsKey(p.Key))
                                    {
                                        var c = Pool.GamePoll[p.Key];
                                        c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                    }
                                }
                                var myTimer = new System.Timers.Timer();
                                if (Table.Next(true))
                                {
                                    var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                if (Table.IsCPs ? client.Player.ConquerPoints >= (int)(Table.RequiredPot + RoundPot) : client.Player.Money >= (long)Table.RequiredPot + (long)RoundPot)
                                {
                                    if (!Table.UnLimited)
                                    {
                                        Table.NumberOfRaise++;
                                    }
                                    if (Table.IsCPs)
                                    {
                                        client.Player.ConquerPoints -= (uint)(Table.RequiredPot + RoundPot);
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                        }
                                    }
                                    if (Table.Next(true))
                                    {
                                        var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                if (Table.TableType == Poker.General.TableType.TexasHoldem)
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
                                    if (Table.TableType == Poker.General.TableType.TexasHoldem)
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
                                            c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));
                                        }
                                    }
                                    foreach (var p in Table.OnScreen)
                                    {
                                        if (Pool.GamePoll.ContainsKey(p.Key))
                                        {
                                            var c = Pool.GamePoll[p.Key];
                                            c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
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
                                        var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                                c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));

                                            }
                                        }
                                        foreach (var p in Table.OnScreen)
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Key))
                                            {
                                                var c = Pool.GamePoll[p.Key];
                                                c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
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
                                            var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
                                                c.Send(Poker.MsgShowHandCallAction.CreateShowHandCallAction(Action, client.PokerPlayer));
                                            }
                                        }
                                        foreach (var p in Table.OnScreen)
                                        {
                                            if (Pool.GamePoll.ContainsKey(p.Key))
                                            {
                                                var c = Pool.GamePoll[p.Key];
                                                c.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
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
                                            var P3 = CMsgShowHandActivePlayer.Create(Table, 10, Table.CurrentPlayer);
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
        [PacketAttribute(PacketsID.CMsgTexasInteractiveo)]
        private static void CMsgTexasInteractiveo(Client.GameClient client, ServerSockets.Packet stream)
        {
            byte[] Packet = new byte[stream.Size];
            fixed (byte* ptr = Packet)
            {
                stream.memcpy(ptr, stream.Memory, stream.Size);
            }
            Poker.General.TableInteractiveType InteractiveType;
            uint PlayerUid, TableId, TableType;
            byte Seat;
            MsgTexasInteractive.GetTexasInteractive(Packet, out InteractiveType, out TableId, out PlayerUid, out Seat, out TableType);

            PokerTable Table = Poker.PokerLoad.Tables[TableId];
            if (Table == null)
                return;
            if (!Table.TableBusy)
            {
                lock (Table.TableSyncRoot)
                {
                    switch (InteractiveType)
                    {
                        case Poker.General.TableInteractiveType.Join:
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

                                            client.PokerPlayer = new Poker.Player(client.Player.Name, client.Player.UID, client.Player.UID, client.Player.ServerID);
                                            client.PokerPlayer.Create(Poker.General.PlayerType.Player, Seat, Table, Money);
                                            if (Table.AddPlayer(client.PokerPlayer))
                                            {
                                                client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(InteractiveType, client.PokerPlayer), true);
                                                client.Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, client.PokerPlayer));
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    client.Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, P));
                                                    if (P.Uid != client.Player.UID)
                                                    {
                                                        if (Pool.GamePoll.ContainsKey(P.Uid))
                                                        {
                                                            Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, client.PokerPlayer));
                                                        }
                                                    }
                                                }
                                                foreach (uint key in Table.OnScreen.Keys)
                                                {
                                                    if (Pool.GamePoll.ContainsKey(key))
                                                    {
                                                        Pool.GamePoll[key].Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.PlayerCount, ObjId = Table.Id, dwParam = (uint)Table.Players.Count, dwParam3 = (long)Table.Players.Count }));
                                                    }
                                                }
                                                if (Table.InGame())
                                                {
                                                    client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                                    client.Send(new CMsgShowHandLostInfo(Table));
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
                        case Poker.General.TableInteractiveType.Watch:
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
                                            client.PokerPlayer = new Poker.Player(client.Player.Name, client.Player.UID, client.Player.UID, client.Player.ServerID);
                                            client.PokerPlayer.Create(Poker.General.PlayerType.Watcher, Seat, Table, Money);
                                            if (Table.AddWatcher(client.PokerPlayer))
                                            {
                                                client.Player.View.SendView(MsgTexasInteractive.CreateTexasInteractive(InteractiveType, client.PokerPlayer), true);
                                                client.Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, client.PokerPlayer));
                                                foreach (var P in Table.PlayersOnTable())
                                                {
                                                    if (P.Uid != client.Player.UID)
                                                    {
                                                        client.Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, P));
                                                        if (Pool.GamePoll.ContainsKey(P.Uid))
                                                        {
                                                            Pool.GamePoll[P.Uid].Send(Poker.MsgShowHandEnter.CreateShowHandEnter(1, client.PokerPlayer));
                                                        }
                                                    }
                                                }
                                                if (Table.InGame())
                                                {
                                                    client.Send(new ServerSockets.RecycledPacket().GetStream().ActionCreate(new ActionQuery() { Type = (ushort)Poker.General.TableUpdate.Chips, ObjId = Table.Id, dwParam3 = (long)Table.TotalPot }));
                                                    client.Send(new CMsgShowHandLostInfo(Table));
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
