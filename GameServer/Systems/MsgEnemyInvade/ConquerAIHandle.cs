using ConquerOnline.Client;
using ConquerOnline.Database;
using ConquerOnline.Game.MsgServer;
using ConquerOnline.Game.MsgTournaments;
using ConquerOnline.Role;
using ConquerOnline.Role.Instance;
using ConquerOnline.ServerSockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet = ConquerOnline.ServerSockets.Packet;

namespace ConquerOnline.AIB
{
    public unsafe class ConquerAIHandle
    {
        public static void Action()
        {
            foreach (var Bot in ConquerAI.Pool.Values)
            {
                if (Bot != null)
                {
                    Bot.MyBot.Buscar();
                    Bot.MyBot.Brincar();
                    Bot.MyBot.Atacar();
                    Bot.MyBot.Remover();
                }
            }
        }
        public static string[] AvailableNames = { "Jack", "MT", "Fighter", "Naymer", "Messi" };

        public static BotType Type;
        public static Counter UID1 = new Counter(3999950000);
        public static Counter UID2 = new Counter(9000000);
        public static GameClient CreateBots(Packet stream
      , BotLevel Level
      , uint Map, ushort X, ushort Y, bool HeroGathering = false)
        {
            GameClient Bot = Clases();
            Bot.MyBot = new ConquerAI();
            Bot.MyBot.Bot = Bot;
            Bot.MyBot.SetLevel(Level);
            string name = "[" + Level.ToString() + "]" + Pool.GetRandom.Next(0, 255) + "" + +Pool.GetRandom.Next(0, 255) + "" + +Pool.GetRandom.Next(0, 255);
            Bot.Player.Name = name;
            Bot.MyBot.HeroGathering = HeroGathering;
            Bot.MyBot.Level = Level;
            switch (Type)
            {
                case BotType.MyAi:
                    {
                        Bot.Player.UID = UID1.Next;
                        Bot.Player.RealUID = UID2.Next;
                        switch (Pool.GetRandom.Next(1, 12))
                        {
                            case 1: Bot.Player.Class = 1005; break;
                            case 2: Bot.Player.Class = 2005; break;
                            case 3: Bot.Player.Class = 4005; break;
                            case 4: Bot.Player.Class = 5005; break;
                            case 5: Bot.Player.Class = 6005; break;
                            case 6: Bot.Player.Class = 7005; break;
                            case 7: Bot.Player.Class = 8005; break;
                            case 8: Bot.Player.Class = 9005; break;
                            case 9: Bot.Player.Class = 13005; break;
                            case 10: Bot.Player.Class = 14005; break;
                            case 11: Bot.Player.Class = 16005; break;
                        }
                        Bot.Player.DynamicID = Map;
                        Bot.Player.X = (ushort)X;
                        Bot.Player.Y = (ushort)Y;
                        Bot.Player.Map = (ushort)Map;
                        Bot.Map = Pool.ServerMaps[Map];
                        Bot.Player.Level = (ushort)140;
                        Bot.Player.Face = (ushort)Pool.GetRandom.Next(134, 154);
                        Bot.Player.Hair = (ushort)Pool.GetRandom.Next(1, 76);
                        Bot = RellenarInformacion(Bot, stream);
                        Bot.Player.View.SendView(Bot.Player.GetArray(stream, false), false);
                        Bot.FullLoading = true;
                        Bot.Player.CompleteLogin = true;
                        Bot.MyBot.Bot = Bot;
                        Bot.Map.AddAI(Bot);
                        Bot.Player.Strength = (ushort)160;
                        Bot.Player.Vitality = (ushort)900;
                        Bot.Player.Agility = (ushort)120;
                        Bot.Player.Spirit = (ushort)105;
                        LoadChi(Bot);
                        LoadJiangHu(Bot);
                        BotsEquipment(stream, Bot);



                        Pool.GamePoll.TryAdd(Bot.Player.UID, Bot);
                        Bot.Equipment.QueryEquipment(Bot.Equipment.Alternante);
                        Bot.Player.HitPoints = (int)Bot.Status.MaxHitpoints;
                        Bot.Equipment.OnDequeue();
                        Role.GameMap.EnterMap((int)Bot.Player.Map);//???
                        break;
                    }


            }

            return Bot;
        }
        public static void BotsEquipment(Packet stream, GameClient pclient)
        {
            uint PerfectionLevel = 54;
            uint SoulArmor = 822071;
            uint SoulH = 820073;
            var FiveStar = Database.CoatStorage.Garments.Values.Where(p => p.Stars == 5).ToArray();
            var rand = (ushort)(Pool.GetRandom.Next() % 1000);
            int Garment = Pool.GetRandom.Next(0, FiveStar.Length);
            pclient.Equipment.AddEx(stream, (uint)FiveStar[Garment].ID, Flags.ConquerItem.Garment, 0, 1, 0, (Flags.Gem)0, (Flags.Gem)0);//
            pclient.Equipment.AddEx(stream, (uint)360354, Flags.ConquerItem.LeftWeaponAccessory, 0, 1, 0, (Flags.Gem)0, (Flags.Gem)0);//
            pclient.Equipment.AddEx(stream, (uint)360355, Flags.ConquerItem.RightWeaponAccessory, 0, 1, 0, (Flags.Gem)0, (Flags.Gem)0);//
            pclient.Equipment.AddEx(stream, (uint)2100075, Flags.ConquerItem.Bottle, 0, 1, 0, (Flags.Gem)0, (Flags.Gem)0);//
            pclient.Equipment.AddEx(stream, (uint)200001, Flags.ConquerItem.SteedMount, 0, 1, 0, (Flags.Gem)0, (Flags.Gem)0);//
            pclient.Equipment.AddEx(stream, 300000, Flags.ConquerItem.Steed, 12,0,0, (Flags.Gem)0, (Flags.Gem)0,false, Role.Flags.ItemEffect.None, PerfectionLevel);//Steed
            pclient.Equipment.AddEx(stream, 203009, Flags.ConquerItem.RidingCrop, 12, 0, 0, (Flags.Gem)0, (Flags.Gem)0, false, Role.Flags.ItemEffect.None, PerfectionLevel);//Crop
            pclient.Equipment.AddEx(stream, 201009, Flags.ConquerItem.Fan, 12, 1, 0, (Flags.Gem)103, (Flags.Gem)103, false, Role.Flags.ItemEffect.None, PerfectionLevel);//Fan
            pclient.Equipment.AddEx(stream, 202009, Flags.ConquerItem.Tower, 12, 1, 0, (Flags.Gem)123, (Flags.Gem)123, false, Role.Flags.ItemEffect.None, PerfectionLevel);//Tower
            pclient.Equipment.AddEx(stream, 204009, Flags.ConquerItem.Wing, 12, 1, 0, (Flags.Gem)103, (Flags.Gem)123, false, Role.Flags.ItemEffect.None, PerfectionLevel);//Wing
            pclient.Equipment.AddEx(stream, 120269, Flags.ConquerItem.Necklace, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Necklace
            pclient.Equipment.AddEx(stream, (uint)150269, Flags.ConquerItem.Ring, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Ring
            pclient.Equipment.AddEx(stream, (uint)160249, Flags.ConquerItem.Boots, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Boot
            if (AtributesStatus.IsTrojan(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)130309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018,SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//PeerlessCoronet
            }
            else if (AtributesStatus.IsWarrior(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)560439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Spear
                pclient.Equipment.AddEx(stream, (uint)900309, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Shield
                pclient.Equipment.AddEx(stream, (uint)131309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)111309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//Head
            }
            if (AtributesStatus.IsArcher(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)500429, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Bow
                pclient.Equipment.AddEx(stream, (uint)1050000, Flags.ConquerItem.LeftWeapon);//Arrow
                pclient.Equipment.AddEx(stream, (uint)133309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)113309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//Head
            }
            else if (AtributesStatus.IsNinja(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)601439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Katana
                pclient.Equipment.AddEx(stream, (uint)601439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Katana
                pclient.Equipment.AddEx(stream, (uint)135309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)112309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//Head
            }
            if (AtributesStatus.IsMonk(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)610439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Beads
                pclient.Equipment.AddEx(stream, (uint)610439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//Beads
                pclient.Equipment.AddEx(stream, (uint)136309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)143309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//Head
            }
            else if (AtributesStatus.IsPirate(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)130309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//PeerlessCoronet
            }
            if (AtributesStatus.IsLee(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)130309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//PeerlessCoronet
            }
            else if (AtributesStatus.IsWater(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)421439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//BackSword
                pclient.Equipment.AddEx(stream, (uint)134309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)114309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//Head
            }
            if (AtributesStatus.IsFire(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)421439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//BackSword
                pclient.Equipment.AddEx(stream, (uint)134309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)114309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//Head
            }
            else if (AtributesStatus.IsWindWalker(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)130309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//PeerlessCoronet
            }
            if (AtributesStatus.IsThunderStriker(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)130309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulH);//PeerlessCoronet
            }
        }
        public static GameClient Clases()
        {
            GameClient pclient = new GameClient(null);

            pclient.Fake = true;
            pclient.Player = new Role.Player(pclient);
            pclient.Inventory = new Inventory(pclient);
            pclient.Equipment = new Equip(pclient);
            pclient.Warehouse = new Warehouse(pclient);
            pclient.MySpells = new Spell(pclient);
            pclient.MyProfs = new Proficiency(pclient);
            pclient.Status = new MsgStatus();
            pclient.MyVendor = new Vendor(pclient);
            pclient.Player.SubClass = new SubClass();
            pclient.Player.Nobility = new Nobility(pclient);
            pclient.Player.Away = 0;
            pclient.Player.View = new RoleView(pclient);
            pclient.Player.Associate = new Associate.MyAsociats(pclient.Player.UID);
            pclient.Player.MyClan = new Clan();
            pclient.Player.QuestGUI = new Quests(pclient.Player);
            pclient.Team = new Team(pclient);
            pclient.MyTrade = new Trade(pclient);
            pclient.Confiscator = new Confiscator();
            pclient.ExtraStatus = new ConcurrentDictionary<RoleStatus.StatuTyp, RoleStatus>();
            pclient.ArenaStatistic = new MsgArena.User();
            pclient.DemonExterminator = new DemonExterminator();
            pclient.Player.MyChi = new Role.Instance.Chi(pclient.Player.UID);
            pclient.Beasts = new Beasts(pclient);
            pclient.HairfaceStorage = new HairfaceStorage(pclient);
            pclient.HundredWeapons = new Role.Instance.HundredWeapons(pclient);
            pclient.MyNinja = new Role.Instance.Ninja(pclient);
            pclient.MyArchives = new Role.Instance.Archives(pclient);
            pclient.Rune = new Rune(pclient);
            pclient.Player.MyUnion = new Union();


            return pclient;
        }
        public static GameClient RellenarInformacion(GameClient pclient, Packet stream)
        {
            pclient.Player.Body = (ushort)ConquerAI.Body[Pool.GetRandom.Next(0, ConquerAI.Body.Length)];
            pclient.Player.VipLevel = 4;
            pclient.Player.NobilityRank = Nobility.NobilityRank.Earl;
            pclient.Player.Angle = Flags.ConquerAngle.SouthEast;
            pclient.Player.InnerPower = new InnerPower(pclient.Player.Name, pclient.Player.UID);
    
            DataCore.AtributeStatus.GetStatus(pclient.Player);
            pclient.Player.CountryID = (ushort)Pool.GetRandom.Next(1, 50);
            pclient.Player.ServerID = 1;
            pclient.Player.Reborn = 2;
            pclient.Player.FirstClass = 13005;
            pclient.Player.SecoundeClass = 2005;
            pclient.Player.Flowers = new Flowers(pclient.Player.UID, pclient.Player.Name);
            pclient.MyHouse = new House(pclient.Player.UID);
            pclient.Player.Action = Flags.ConquerAction.Sit;

            return pclient;
        }

        public static void LoadChi(Client.GameClient user)
        {
            for (int i = 1; i < 5; i++)
            {
                var Power = user.Player.MyChi.Where(p => (int)p.Type == i).FirstOrDefault();
                if (Power == null) continue;
                if (!Power.UnLocked)
                {
                    Power.UnLocked = true;
                    Power.UID = user.Player.UID;
                    Power.Name = user.Player.Name;
                }
                Power.Attributes[0] = new ConquerOnline.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[0].Type = MsgChiInfo.ChiAttribute.HPAdd;
                Power.Attributes[0].Value = 5000;
                Power.Attributes[1] = new ConquerOnline.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[1].Type = MsgChiInfo.ChiAttribute.HPAdd;
                Power.Attributes[1].Value = 5000;
                Power.Attributes[2] = new ConquerOnline.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[2].Type = MsgChiInfo.ChiAttribute.HPAdd;
                Power.Attributes[2].Value = 5000;
                Power.Attributes[3] = new ConquerOnline.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[3].Type = MsgChiInfo.ChiAttribute.HPAdd;
                Power.Attributes[3].Value = 5000;
                Power.Exp = 600000;
               
            }
        }
        public static void LoadJiangHu(Client.GameClient user)
        {
            Role.Instance.JiangHu jiang = new Role.Instance.JiangHu(user.Player.UID);
            jiang.Name = user.Player.Name;
            jiang.CustomizedName = user.Player.Name;
            jiang.Level = (byte)user.Player.Level;
            jiang.Talent = 0;
            jiang.FreeTimeToday = 0;
            jiang.OnJiangMode = false;
            jiang.FreeCourse = 0;
            jiang.StartCountDwon = DateTime.Now;
            jiang.CountDownEnd = DateTime.Now;
            jiang.RoundBuyPoints = 0;
            uint _Stage = 0;
            byte Level = 0;

            Role.Instance.JiangHu.Stage.AtributesType Type = Role.Instance.JiangHu.Stage.AtributesType.None;
            foreach (var Stage in jiang.ArrayStages)
            {
                Stage.Activate = true;
                foreach (var Star in Stage.ArrayStars)
                {

                    switch (_Stage)
                    {
                        case 0:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.Breakthrough;
                                Level = 6;
                                break;
                            }
                        case 1:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.Immunity;
                                Level = 6;
                                break;
                            }
                        case 2:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.PAttack;
                                Level = 6;
                                break;
                            }
                        case 3:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.CriticalStrike;
                                Level = 6;
                                break;
                            }
                        case 4:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.MaxLife;
                                Level = 6;
                                break;
                            }
                        case 5:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.Immunity;
                                Level = 6;
                                break;
                            }
                        case 6:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.PAttack;
                                Level = 6;
                                break;
                            }
                        case 7:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.CriticalStrike;
                                Level = 6;
                                break;
                            }
                        case 8:
                            {
                                Type = Role.Instance.JiangHu.Stage.AtributesType.MaxLife;
                                Level = 6;
                                break;
                            }
                    }
                    Star.Activate = true;
                    Star.UID = jiang.ValueToRoll(Type, Level);
                    Star.Typ = jiang.GetValueType(Star.UID);
                    Star.Level = jiang.GetValueLevel(Star.UID);
                }
                _Stage++;
            }
            jiang.CreateStatusAtributes(user);
            user.Player.MyJiangHu = jiang;
        }
    }
}
