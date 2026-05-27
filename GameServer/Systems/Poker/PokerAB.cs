using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ConquerOnline.Game.MsgServer;
using System.Threading.Generic;
using ConquerOnline.Game.MsgFloorItem;
using ConquerOnline.Game.MsgServer.AttackHandler;
using ConquerOnline.Game.MsgTournaments;
using ConquerOnline.Client;

namespace ConquerOnline
{
    public class PokerAB
    {
        public static int PokerABRate(uint Card)
        {
            switch (Card % 100)
            {
                case 83:
                    return 5;
                case 82:
                    return 10;
                case 81:
                    return 15;
                case 80:
                    return 20;
                case 79:
                    return 25;
                case 78:
                    return 30;
                case 77:
                    return 35;
                case 76:
                    return 40;
                case 75:
                    return 45;
                case 74:
                    return 50;
               
            }
            return 0;
        }
        public static bool PokerABActive = false;
        public static void SmeltingPokerAB()
        {
            if (Server.FullLoading)
            {
                if (Pool.ServerMaps.ContainsKey(1002))
                {
                    var map = Pool.ServerMaps[1002];
                    if (map.View.GetAllMapRolesCount(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2317) > 0)
                    {
                        if (Time32.Now >= Pool.smeltFloorStamp.AddSeconds(30))
                        {
                            Pool.smeltFloorStamp = Time32.Now;
                            var floor = map.View.GetAllMapRoles(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2317).FirstOrDefault();
                            map.RemoveTrap(floor.X, floor.Y, floor);
                            foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                            {
                                if (client.Player.Map == map.ID && (client.Player.DynamicID == 0 || client.Player.DynamicID == map.ID))
                                {
                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = recycledPacket.GetStream();
                                        {
                                            client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = floor.X, Y = floor.Y, Strings = new string[1] { "DragonSoul_djs" } }));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (map.View.GetAllMapRolesCount(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2318) > 0)
                    {
                        if (Time32.Now >= Pool.smeltFloorStamp.AddSeconds(2))
                        {
                            Pool.smeltFloorStamp = Time32.Now;
                            var floor = map.View.GetAllMapRoles(Role.MapObjectType.Item, i => (i as MsgItem).MsgFloor.m_ID == 2318).FirstOrDefault() as MsgItem;
                            map.RemoveTrap(floor.X, floor.Y, floor);
                            floor.UID = MsgItem.UIDS.Next;
                            floor.MsgFloor.DropType = MsgDropID.Effect;
                            floor.MsgFloor.m_ID = 2317;
                            map.EnqueueItem(floor);
                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                            {
                                var stream = recycledPacket.GetStream();
                                {
                                    floor.SendAll(stream, MsgDropID.Effect);
                                }
                            }
                            PokerABActive = false;
                            //Smelting
                            if (ConquerOnline.Game.MsgServer.AttackHandler.Calculate.Base.Rate(50))//PokerA
                            {
                                foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                                {
                                    if (client.Player.Map == map.ID && (client.Player.DynamicID == 0 || client.Player.DynamicID == map.ID))
                                    {
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 350, Y = 469, Strings = new string[1] { "DragonSoul_ylsb" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 341, Y = 474, Strings = new string[1] { "DragonSoul_ylcg" } }));

                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 350, Y = 469, Strings = new string[1] { "qfail" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 341, Y = 474, Strings = new string[1] { "qsuccess" } }));
                                            }
                                        }
                                        if (client.Player.PokerB > 0)
                                        {
                                            client.CreateBoxDialog("┌────────You Lose────────┐\n" +
                                                        "             The Poker Card vanished into " + 20 * (client.Player.PokerB % 100) + "-min EXP.\n" +
                                                     "└──────────────────────┘");
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    client.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (client.Player.PokerA > 0 && client.Player.PokerB == 0)
                                            {
                                                if (ConquerOnline.Game.MsgServer.AttackHandler.Calculate.Base.Rate(PokerABRate(client.Player.PokerA)))
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        {
                                                            client.Inventory.Add(stream, client.Player.PokerA + 1, 1);
                                                        }
                                                    }
                                                    ConquerOnline.Game.ServerLogs.PokerAWin(client, client.Player.PokerA + 1);
                                                    client.CreateBoxDialog("┌────────You Win────────┐\n" +
                                                            "             Congratulations! You received a " + Pool.ItemsBase[client.Player.PokerA + 1].Name + "!\n" +
                                                         "└──────────────────────┘");
                                                }
                                                else 
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        {
                                                            client.Inventory.Add(stream, client.Player.PokerA, 1);
                                                        }
                                                    }
                                                    client.CreateBoxDialog("┌────────You Win────────┐\n" +
                                                            "             The Poker Card is not upgraded,\n     but you received an extra " + Pool.ItemsBase[client.Player.PokerA].Name + "!\n" +
                                                         "└──────────────────────┘");
                                                }
                                                
                                            }
                                        }
                                        client.Player.PokerA = 0;
                                        client.Player.PokerB = 0;
                                    }
                                }
                                    
                            }
                            else//PokerB
                            {
                                foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                                {
                                    if (client.Player.Map == map.ID && (client.Player.DynamicID == 0 || client.Player.DynamicID == map.ID))
                                    {
                                        using (var recycledPacket = new ServerSockets.RecycledPacket())
                                        {
                                            var stream = recycledPacket.GetStream();
                                            {
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 341, Y = 474, Strings = new string[1] { "DragonSoul_ylsb" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 350, Y = 469, Strings = new string[1] { "DragonSoul_ylcg" } }));

                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 341, Y = 474, Strings = new string[1] { "qfail" } }));
                                                client.Send(stream.StringPacketCreate(new Game.MsgServer.MsgStringPacket() { ID = MsgStringPacket.StringID.LocationEffect, X = 350, Y = 469, Strings = new string[1] { "qsuccess" } }));
                                            }
                                        }
                                        if (client.Player.PokerA > 0)
                                        {
                                            client.CreateBoxDialog("┌────────You Lose────────┐\n" +
                                                      "             The Poker Card vanished into " + 20 * (client.Player.PokerA % 100) + "--min EXP.\n" +
                                                   "└──────────────────────┘");
                                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = recycledPacket.GetStream();
                                                {
                                                    client.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (client.Player.PokerB > 0 && client.Player.PokerA == 0)
                                            {

                                                if (ConquerOnline.Game.MsgServer.AttackHandler.Calculate.Base.Rate(PokerABRate(client.Player.PokerB)))
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        {
                                                            client.Inventory.Add(stream, client.Player.PokerB + 1, 1);
                                                            ConquerOnline.Game.ServerLogs.PokerBWin(client, client.Player.PokerB + 1);
                                                        }
                                                    }
                                                    client.CreateBoxDialog("┌────────You Win────────┐\n" +
                                                            "             Congratulations! You received a " + Pool.ItemsBase[client.Player.PokerB + 1].Name + "!\n" +
                                                         "└──────────────────────┘");
                                                }
                                                else
                                                {
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = recycledPacket.GetStream();
                                                        {
                                                            client.Inventory.Add(stream, client.Player.PokerB, 1);
                                                            client.CreateBoxDialog("┌────────You Win────────┐\n" +
                                                                    "             The Poker Card is not upgraded,\n     but you received an extra " + Pool.ItemsBase[client.Player.PokerB].Name + "!\n" +
                                                                 "└──────────────────────┘");
                                                        }
                                                    }
                                                }
                                            }
                                            

                                        }
                                        client.Player.PokerA = 0;
                                        client.Player.PokerB = 0;
                                        
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Time32.Now >= Pool.smeltFloorStamp.AddSeconds(5))
                        {
                            Pool.smeltFloorStamp = Time32.Now;
                            var item = new MsgItem(null, 347, 473, MsgItem.ItemType.Effect, 0, 0, map.ID, 0, false, map, 60 * 60 * 1000);
                            item.MsgFloor.m_ID = 2318;
                            item.MsgFloor.m_Color = 1;
                            item.MsgFloor.DropType = MsgDropID.Effect;
                            map.EnqueueItem(item);
                            using (var recycledPacket = new ServerSockets.RecycledPacket())
                            {
                                var stream = recycledPacket.GetStream();
                                {
                                    item.SendAll(stream, MsgDropID.Effect);
                                }
                            }
                            PokerABActive = true;

                        }
                    }
                    
                   
                }
            }
        }

    }
}
