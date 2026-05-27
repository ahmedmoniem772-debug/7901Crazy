using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerOnline.Game.MsgNpc;
using ConquerOnline.Game.Ai;

namespace ConquerOnline.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet LoginHandlerCreate(this ServerSockets.Packet stream, uint Type, uint Map)
        {
            stream.InitWriter();

            stream.Write(0);
            stream.Write(Type);
            stream.Write(Map);

            stream.Finalize(GamePackets.MapLoading);

            return stream;
        }

    }
    public unsafe struct MsgLoginHandler
    {
        [PacketAttribute(GamePackets.MapLoading)]
        public unsafe static void LoadMap(Client.GameClient client, ServerSockets.Packet packet)
        {
            if ((client.ClientFlag & Client.ServerFlag.AcceptLogin) == Client.ServerFlag.AcceptLogin)
            {
                try
                {
                    client.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                    client.Send(packet.ServerVerCreate());
                client.Send(packet.ServerTimerCreate());
                client.Send(packet.ServerConfig());
                client.Send(packet.HeroInfo(client.Player));
                    foreach (var chipower in client.Player.MyChi)
                        client.Player.MyChi.SendQueryUpdate(client, chipower, packet);
                    MsgChiInfo.MsgHandleChi.SendInfo(client, MsgChiInfo.Action.Upgrade, client);

                    if (client.Player.SecurityPassword != 0)
                    {
                        client.Send(packet.SecondaryPasswordCreate(MsgSecondaryPassword.ActionID.PasswordCorrect, 1, 0));
                    }
                    else
                        client.Player.IsCheckedPass = true;


                    if (client.Player.Achievement != null)
                        client.Player.Achievement.Send(client, packet);
                    if (client.HundredWeapons.Unlocked && client.HundredWeapons.Valid())
                    {
                        client.Send(packet.CreateHundredWeaponsInfo(client, MsgHundredWeaponsInfo.ActionID.RequestInfo));
                        client.Send(packet.ActionCreate(new ActionQuery() { Type = ActionType.OpenDialog, ObjId = client.Player.UID, Timestamp = (uint)Time32.timeGetTime().GetHashCode(), dwParam = DialogCommands.TrojanWeaponArchive, PositionX = client.Player.X, PositionY = client.Player.Y }));
                    }

                    client.Send(packet.NobilityIconCreate(client.Player.Nobility));

                    client.MyNinja.Loading();

                    client.Inventory.ShowALL(packet);

                    client.Equipment.Show(packet);

                    client.Rune.Show(packet);

                    
                    client.Send(packet.CreateXuanBaoOpt(new MsgXuanBaoOpt.XuanBaoOptProto()));

                    client.Player.Send(packet.AutoHuntCreate(0, 341));


                    client.Activeness.CheckTasks(packet);

                    client.Activeness.IncreaseTask(1);
                    client.Activeness.IncreaseTask(13);
                    client.Activeness.IncreaseTask(25);

                    if (client.Player.VipLevel > 1)
                    {
                        client.Activeness.IncreaseTask(2);
                        client.Activeness.IncreaseTask(14);
                        client.Activeness.IncreaseTask(26);
                    }
                    client.Activeness.UpdateActivityPoints(packet);
                    client.Activeness.UpdateClaimRewards(packet);

                    if (client.Player.BlessTime > 0)
                        client.Player.SendUpdate(packet, client.Player.BlessTime, MsgUpdate.DataType.LuckyTimeTimer);

                    client.Send(packet.FlowerCreate(Role.Core.IsBoy(client.Player.Body) ? MsgFlower.FlowerAction.Flower : MsgFlower.FlowerAction.FlowerSender
                        , 0, 0, client.Player.Flowers.RedRoses, client.Player.Flowers.RedRoses.Amount2day
                        , client.Player.Flowers.Lilies, client.Player.Flowers.Lilies.Amount2day
                        , client.Player.Flowers.Orchids, client.Player.Flowers.Orchids.Amount2day
                        , client.Player.Flowers.Tulips, client.Player.Flowers.Tulips.Amount2day));


                    if (client.Player.Flowers.FreeFlowers > 0)
                    {
                        client.Send(packet.FlowerCreate(Role.Core.IsBoy(client.Player.Body)
                            ? MsgFlower.FlowerAction.FlowerSender : MsgFlower.FlowerAction.Flower
                            , 0, 0, client.Player.Flowers.FreeFlowers));
                    }

                 
                    client.Player.CreateHeavenBlessPacket(packet, true);

                    if (client.Player.HitPoints < 1)
                        client.Player.HitPoints = 1;


                    if (MsgTournaments.MsgSchedules.CurrentTournament.Type == MsgTournaments.TournamentType.QuizShow
                        && MsgTournaments.MsgSchedules.CurrentTournament.Process == MsgTournaments.ProcesType.Alive)
                        MsgTournaments.MsgSchedules.CurrentTournament.Join(client, packet);
                        

                    if (client.Player.DExpTime > 0)
                        client.Player.CreateExtraExpPacket(packet);


                    if (client.Player.MyClan != null)
                    {
                        client.Player.MyClan.SendThat(packet, client);

                        foreach (var ally in client.Player.MyClan.Ally.Values)
                            client.Send(packet.ClanRelationCreate(client.Player.MyClan.ID, ally.Name, ally.LeaderName, MsgClan.Info.AddAlly));
                        foreach (var enemy in client.Player.MyClan.Enemy.Values)
                            client.Send(packet.ClanRelationCreate(client.Player.MyClan.ID, enemy.Name, enemy.LeaderName, MsgClan.Info.AddEnemy));
                    }

                    if (client.Player.MyJiangHu != null)
                        client.Player.MyJiangHu.LoginClient(packet, client);
                    else if (client.Player.Reborn == 2)
                    {
                        client.Send(packet.JiangHuInfoCreate(MsgJiangHuInfo.JiangMode.IconBar, "0"));
                    }

                    foreach (var item in client.Confiscator.RedeemContainer.Values)
                    {
                        var Dataitem = item;
                        Dataitem.DaysLeft = (uint)(TimeSpan.FromTicks(DateTime.Now.Ticks).Days - TimeSpan.FromTicks(Role.Instance.Confiscator.GetTimer(item.Date).Ticks).Days);
                        if (Dataitem.DaysLeft > 7)
                        {
                            Dataitem.Action = MsgDetainedItem.ContainerType.RewardCps;
                        }
                        if (Dataitem.Action != MsgDetainedItem.ContainerType.RewardCps)
                        {
                            Dataitem.Action = MsgDetainedItem.ContainerType.DetainPage;
                            Dataitem.Send(client, packet);
                        }
                        if (Dataitem.Action == MsgDetainedItem.ContainerType.RewardCps)
                            client.Confiscator.RedeemContainer.TryRemove(item.UID, out Dataitem);
                    }
                    foreach (var item in client.Confiscator.ClaimContainer.Values)
                    {
                        var Dataitem = item;
                        Dataitem.DaysLeft = (uint)(TimeSpan.FromTicks(DateTime.Now.Ticks).Days - TimeSpan.FromTicks(Role.Instance.Confiscator.GetTimer(item.Date).Ticks).Days);
                        if (Dataitem.RewardConquerPoints != 0)
                        {
                            Dataitem.Action = MsgDetainedItem.ContainerType.RewardCps;
                        }
                        Dataitem.Send(client, packet);
                        client.Confiscator.ClaimContainer[item.UID] = Dataitem;
                    }
                    
                    if (Program.ServerConfig.ConquerorWinner == client.Player.UID)
                        client.Player.AddFlag(Role.Core.IsBoy(client.Player.Body) ? MsgUpdate.Flags.TopMrConquer : MsgUpdate.Flags.TopMrsConquer, Role.StatusFlagsBigVector32.PermanentFlag, false);

                    if (MsgTournaments.MsgSchedules.NEWGUILDWAR.RewardDeputiLeader.Contains(client.Player.UID))
                        client.Player.AddFlag(MsgUpdate.Flags.TopDeputyLeader, Role.StatusFlagsBigVector32.PermanentFlag, false);

                    if (MsgTournaments.MsgSchedules.NEWGUILDWAR.RewardLeader.Contains(client.Player.UID))
                        client.Player.AddFlag(MsgUpdate.Flags.TopGuildLeader, Role.StatusFlagsBigVector32.PermanentFlag, false);

                    if (MsgTournaments.MsgSchedules.SuperGuildWar.RewardDeputiLeader.Contains(client.Player.UID))
                        client.Player.AddFlag(MsgUpdate.Flags.TopSuperGuildWarThreeStars, Role.StatusFlagsBigVector32.PermanentFlag, false);

                    if (MsgTournaments.MsgSchedules.SuperGuildWar.RewardLeader.Contains(client.Player.UID))
                        client.Player.AddFlag(MsgUpdate.Flags.TopSuperGuildWarFiveStars, Role.StatusFlagsBigVector32.PermanentFlag, false);

                    if (client.Player.WinnerBest ==1)
                        client.Player.AddFlag(MsgUpdate.Flags.rygh_hglx1, Role.StatusFlagsBigVector32.PermanentFlag, false);
         
                    if (client.Player.CursedTimer > 0)
                    {
                        client.Player.AddCursed(client.Player.CursedTimer);
                    }

                    MsgTournaments.MsgSchedules.ClassPkWar.LoginClient(client);
                    MsgTournaments.MsgSchedules.ElitePkTournament.GetTitle(client, packet);

                    if (client.Player.VipLevel == 6)
                    {
                        client.Player.AddTitle(Game.MsgTournaments.MsgEliteTournament.top_typ.Vip, true);
                    }
                    if (client.Player.Name.Contains("ConquerOnline[GM]"))
                    {
                        client.Player.AddTitle(Game.MsgTournaments.MsgEliteTournament.top_typ.GM, true);
                    }
                    if (MsgTournaments.MsgBroadcast.CurrentBroadcast.EntityID != 1)
                    {
                        client.Send(new MsgServer.MsgMessage(MsgTournaments.MsgBroadcast.CurrentBroadcast.Message
                            , "ALLUSERS"
                            , MsgTournaments.MsgBroadcast.CurrentBroadcast.EntityName
                            , MsgServer.MsgMessage.MsgColor.red
                            , MsgServer.MsgMessage.ChatMode.BroadcastMessage
                            ).GetArray(packet));
                    }


                    if (client.Player.RacePoints > 0)
                        client.Player.SendUpdate(packet, client.Player.RacePoints, MsgUpdate.DataType.RaceShopPoints);

                    client.Player.InnerPower.AddPotency(packet, client, 0);
                    client.Player.UpdateVip(packet);

                    client.Player.SendUpdate(packet, 255, MsgUpdate.DataType.Merchant);

                    if ((client.Player.MainFlag & Role.Player.MainFlagType.CanClaim) != Role.Player.MainFlagType.CanClaim)
                    {
                        client.Player.MainFlag |= Role.Player.MainFlagType.CanClaim;
                        client.Player.SendUpdate(packet, (uint)client.Player.MainFlag, Game.MsgServer.MsgUpdate.DataType.MainFlag, false);
                    }

                    ActionQuery action = new ActionQuery()
                    {
                        ObjId = client.Player.UID,
                        Type = 157,
                        dwParam = 2
                    };

                    client.Send(packet.ActionCreate(action));

                    Database.TitleStorage.CheckUpUser(client, packet);

                    foreach (var title in client.Player.SpecialTitles)
                        client.Player.AddSpecialTitle(title, packet);
                    foreach (var titles in client.Player.WingsTitles)
                        client.Player.AddSpecialWings(titles, packet);
                    foreach (var titles in client.Player.HaloTitles)
                        client.Player.AddSpecialHalo(titles, packet);
                    client.Player.SendSpecialTitle(packet);
                    client.Player.ActiveSpecialTitles(packet);

                  
                    MsgTournaments.MsgSchedules.PkWar.AddTop(client);

                    client.SendSysMesage("Welcome To " + Program.ServerConfig.ServerName + ".", MsgMessage.ChatMode.Talk);
                    client.SendSysMesage("The guild war this week will start from 20:00 SundDay. and end on 22:00 PM SundDay.", MsgMessage.ChatMode.Talk);

                    client.SendSysMesage("The drop rate  players is 1 To 50  Conquer points in your inventory.", MsgMessage.ChatMode.Talk);
                
                    client.SendSysMesage("Official Site: Www.ConquerOnline.com", MsgMessage.ChatMode.Talk);

                    client.SendSysMesage("Enjoy " + Program.ServerConfig.ServerName + ".", MsgMessage.ChatMode.Talk);

                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Header, "  Our Version 7368:"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "Welcome to " + Program.ServerConfig.ServerName + "."));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- System~Anima"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- ThunderStrike"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- System~Archive~Trojan"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- System~Awkeend~Ninja"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- System~Almighty~Warrior"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- Soaring Archers"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- System~Soaring~Tao"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- New~Poker"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "-New Clan"));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Body, "- AntiCheat protection got some updates in order to stop ilegal changes By~Mego ."));
                    client.Send(packet.StaticGUI(MsgNpc.MsgBuilder.StaticGUIType.Footer, "Thank you. " + Program.ServerConfig.Program + "JoOo "));

                    client.Player.UpdateInventorySash(packet);
                    if (client.Player.ExpProtection > 0)
                        client.Player.CreateExpProtection(packet, 0, false);
                    if (client.Player.VipLevel >= 6)
                    {
                        TimeSpan timer1 = new TimeSpan(client.Player.ExpireVip.Ticks);
                        TimeSpan Now2 = new TimeSpan(DateTime.Now.Ticks);
                        int days_left = (int)(timer1.TotalDays - Now2.TotalDays);
                        int hour_left = (int)(timer1.TotalHours - Now2.TotalHours);
                        int left_minutes = (int)(timer1.TotalMinutes - Now2.TotalMinutes);

                        if (days_left > 0)
                            client.SendSysMesage("Your VIP " + client.Player.VipLevel + " will expire in : " + days_left + " days.", MsgMessage.ChatMode.System);
                        else if (hour_left > 0)
                            client.SendSysMesage("Your VIP " + client.Player.VipLevel + " will expire in : " + hour_left + " hours.", MsgMessage.ChatMode.System);
                        else if (left_minutes > 0)
                            client.SendSysMesage("Your VIP " + client.Player.VipLevel + " will expire in : " + left_minutes + " minutes.", MsgMessage.ChatMode.System);
                    }

                    if (Database.AtributesStatus.IsTrojan(client.Player.Class)
                        || Database.AtributesStatus.IsTrojan(client.Player.FirstClass)
                        || Database.AtributesStatus.IsTrojan(client.Player.SecoundeClass))
                    {
                        if (!client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.Cyclone))
                            client.MySpells.Add(packet, (ushort)Role.Flags.SpellID.Cyclone);
                    }
                    if (Database.AtributesStatus.IsThunderStriker(client.Player.Class)
                        || Database.AtributesStatus.IsThunderStriker(client.Player.FirstClass)
                        || Database.AtributesStatus.IsThunderStriker(client.Player.SecoundeClass))
                    {
                        if (!client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.SparkShield))
                            client.MySpells.Add(packet, (ushort)Role.Flags.SpellID.SparkShield);
                    }


                    if (client.Inventory.HaveSpace(1))
                    {
                        foreach (var item in client.Equipment.ClientItems.Values)
                        {
                            if (item.Position >= (uint)Role.Flags.ConquerItem.Head && item.Position <= (uint)Role.Flags.ConquerItem.Wing)
                            {
                                if (client.Inventory.HaveSpace(1) && item.Position == (uint)Role.Flags.ConquerItem.RightWeapon
                                    && item.Position == (uint)Role.Flags.ConquerItem.LeftWeapon)
                                {
                                    if (!Database.ItemType.IsShield(item.ITEM_ID))
                                    {
                                        if (!Database.ItemType.Equipable(item.ITEM_ID, client))
                                        {
                                            client.Equipment.Remove((Role.Flags.ConquerItem)item.Position, packet);
                                        }
                                    }
                                }
                            }
                            else if (item.Position >= (uint)Role.Flags.ConquerItem.AleternanteHead && item.Position <= (uint)Role.Flags.ConquerItem.AleternanteGarment)
                            {
                                if (client.Inventory.HaveSpace(1) && item.Position == (uint)Role.Flags.ConquerItem.AleternanteRightWeapon
                                    && item.Position == (uint)Role.Flags.ConquerItem.AleternanteLeftWeapon)
                                {
                                    if (!Database.ItemType.IsShield(item.ITEM_ID))
                                    {
                                        if (!Database.ItemType.Equipable(item.ITEM_ID, client))
                                        {
                                            client.Equipment.RemoveAlternante((Role.Flags.ConquerItem)item.Position, packet);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    MsgServer.MsgSameGroupServerList.GroupServer group = new MsgSameGroupServerList.GroupServer();
                    var GroupServers = Database.GroupServerList.GroupServers.Values.ToArray();
                    group.Servers = new MsgSameGroupServerList.Server[Database.GroupServerList.GroupServers.Count];
                    for (int x = 0; x < GroupServers.Length; x++)
                    {
                        var DBServer = GroupServers[x];
                        group.Servers[x] = new MsgSameGroupServerList.Server();
                        group.Servers[x].ServerID = DBServer.ID;
                        group.Servers[x].ServerName = DBServer.Name;
                        group.Servers[x].Type = 1;
                        if (group.Servers[x].Type > 0)
                        {
                            group.Servers[x].MapID = DBServer.MapID;
                            group.Servers[x].X = DBServer.X;
                            group.Servers[x].Y = DBServer.Y;
                            group.Servers[x].Unknown = 0;
                        }
                    }
                    client.Send(packet.CreateGroupServerList(group));
                    var TexasStyle = client.HairfaceStorage.Objects.Where(i => i.Type >= MsgHairfaceStorage.Type.TexasTable).ToArray();
                    if (TexasStyle.Count() > 0)
                    {
                        MsgHairfaceStorage.MsgHairfaceStorageProto Info = new MsgHairfaceStorage.MsgHairfaceStorageProto();
                        Info.Items = new MsgHairfaceStorage.Item[TexasStyle.Count()];
                        Info.Type = MsgHairfaceStorage.Actions.Login;
                        for (int i = 0; i < Info.Items.Length; i++)
                        {
                            Info.Items[i] = new MsgHairfaceStorage.Item();
                            Info.Items[i].ID = TexasStyle[i].ID;
                            Info.Items[i].Type = TexasStyle[i].Type;
                            Info.Items[i].Equipped = TexasStyle[i].Equiped;
                        }
                        client.Send(packet.CreateHairface(Info));
                    }
                    
                    if (Database.RuneRank.RankingList != null && Database.RuneRank.RankingList.ContainsKey(client.Player.UID))
                    {
                        for (int i = 0; i < Database.RuneRank.RankingList.Count; i++)
                            if (Database.RuneRank.RankingList.ToArray()[i].Key == client.Player.UID)
                                client.SendSysMesage("My Rank: " + (i + 1) + "; Current Rune Bag Score: " + client.Rune.Score);
                    }
                    else client.SendSysMesage("My Rank: not ranked; Current Rune Bag Score: " + client.Rune.Score);
                    client.Send(packet.CreateRuneStorage(new MsgRuneStorage.MsgRuneStorageProto() { Type = 2, HPAdd = client.Rune.HPAdd }));

                    if (Database.AtributesStatus.IsThunderStriker(client.Player.Class) && client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.UndyingImprinting))
                    {
                        client.Player.SendUpdate(packet, MsgUpdate.Flags.UndyingImprinting, 60, 0, 0, (MsgUpdate.DataType)111);
                        client.Player.ThunderStrikerUndyingImprinting = 60;
                    }
                    client.Warehouse.SendReturnedItems(packet);
                    if (client.Player.InUnion)
                        client.Player.MyUnion.SendOverheadLeagueInfo(packet, client);
                    client.Player.PKPoints = client.Player.PKPoints;

                    client.Player.MyChi.Login(client);
                    client.DragonSkin.RequestInfo(packet);
                    client.MyArchives.Loading();
                    client.Collection.LoadingCollection();
                    client.MyExchangeShop.Loading();
                    client.MyExchangeShop.LoadingClan();
                    MsgMailNotify.Loading(client);
                    BotSystem.LoadingGathering(client);
                    if (Game.MsgTournaments.MsgArena.ArenaPoll.TryGetValue(client.Player.UID, out client.ArenaStatistic))
                        client.ArenaStatistic.ApplayInfo(client.Player);
                    byte prestigeRank = Convert.ToByte(Database.PrestigeRanking.GetMyRank(Database.PrestigeRanking.GetIndex(client.Player.Class), client.Player.UID));
                    client.Send(packet.CreateProfLevUp(new MsgProfLevUp.MsgProfLevUpProto() { Type = 1, Rank = prestigeRank }));
                   client.Player.ClassExperience = client.Player.ClassExperience;
                    client.ClientFlag &= ~Client.ServerFlag.AcceptLogin;
                    client.ClientFlag |= Client.ServerFlag.LoginFull;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}