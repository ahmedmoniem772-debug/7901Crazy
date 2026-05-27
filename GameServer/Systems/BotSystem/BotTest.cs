
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Generic;
using VirusX;
using VirusX.Client;
using VirusX.Game.MsgServer;
using VirusX.Game.MsgTournaments;
using VirusX.Role;
using VirusX.Role.Instance;
using VirusX.ServerSockets;
using VirusX.Database;
using VirusX.Game.MsgMonster;

namespace VirusX.Entities.Character
{
    public static class AiBot
    {
        public static ConcurrentDictionary<uint, AI> Pool = new ConcurrentDictionary<uint, AI>();
        public static Counter AiUID = new Counter(60000000);
        private static TimerRule<AI> AIAction = new TimerRule<AI>(TimerCallBack, 100);
        public static string[] AvailableNames = { "Ares", "Apolo", "Asclepio", "Baco", "Cupido", "Dionisio", "Eros", "Febo", "Forcis", "Hades", "Hefesto", "Hércules", "Hermes", "Helios", "Marte", "Mercurio", "Morfeo", "Neptuno", "Perseo", "Poseidón", "Silvano", "Teseo", "Ulises", "Vulcano", "Zeus", "Aamon", "Belia", "Paimon", "Agares", "Sidragaso", "Balban", "Baphomet", "Bael", "Baalzephon", "Alouqua", "Abrahel", "Abyzou ", "Aradia", "Astartea", "Habondia", "Lilith", "Nahama", "Noctiluca", "Perséfone", "Perisas", "Tamar", "Zalir", "Zemunín", "Asmodeo ", "Behemoth", "Lucifer", "Mammón", "Leviatán", "Astaroth", "Diablo", "Baal", "Dante", "Asta", "Megicula", "Lumier", "Lier", "Deku", "Bakugo", "Rafael", "Odin", "Thor", "Kratos", "Agumon", "Valhalla", "Arcangel", "Meliodas", "Zeldris", "Escanor", "King", "Ban", "Estarossa", "Mael", "LightYagami", "Kira", "RiasGremory", "Bastardo", "TheNorth", "Winter", "NightKing", "Cain", "Athena", "Yui", "Yagoromo", "Indra", "Naruto", "Sasuke", "Luffy", "RoronoaZoro", "TrafalgarLaw", "RossMaria", "LaPerversa", "FelizSanchez", "DaniloLadron", "BlackClover", "Julios", "Luck", "Noel", "Yami", "Lie", "Luffy", "Name", "Zoro", "Usopp", "Franki", "Robin", "Brook", "Enel", "ElRaro", "BocaDePiano", "Alofoke", "ElMeloso", "Ditamae", "Pistola", "YQueFue", "Ranchero", "TheEnd", "TheFinal", "FinalSpace", "TheLostCity", "Indra", "Digimon", "Pokemon", "JetEd", "Blue", "Red", "Green", "Online", "Fake", "True", "False", "Int", "String", "Float", "Uint", "ushort", "Byte", "Ulong", "Long", "Array", "Koneko", "Isee", "ElAbusador", "TeCogi", "Peligrosa", "Danger", "FlowFactory", "RD", "Anuel", "MykeTowers", "Mozart", "RochyRD", "Mace", "Falchion", "Montante", "Battleaxe", "Zweihander", "Hatchet", "Billhook", "Club", "Hammer", "Caltrop", "Maul", "Sledgehammer", "Longbow", "Bludgeon", "Harpoon", "Crossbow", "Lance", "Angon", "Pike", "Tiger Claw", "Fire Lance", "Poleaxe", "Brass Knuckle", "Matchlock", "Quarterstaff", "Gauntlet", "Bullwhip", "War Hammer", "Katar", "Flying Claw", "Spear", "Dagger", "Slungshot", "Katana", "Gladius", "Aspis", "Saber", "Cutlass", "Blade", "Broadsword", "Scimitar", "Lockback", "Claymore", "Espada", "Machete", "Grizzly", "Wolverine", "Deathstalker", "Snake", "Wolf", "Scorpion", "Vulture", "Claw", "Boomslang", "Falcon", "Fang", "Viper", "Ram", "Grip", "Sting", "Boar", "Black Mamba", "Lash", "Tusk", "Goshawk", "Gnaw", "Amazon", "Majesty", "Anomoly", "Malice", "Banshee", "Mannequin", "Belladonna", "Minx", "Beretta", "Mirage", "Black Beauty", "Nightmare", "Calypso", "Nova", "Carbon", "Pumps", "Cascade", "Raven", "Colada", "Resin", "Cosma", "Riveter", "Cougar", "Rogue", "Countess", "Roulette", "Enchantress", "Shadow", "Enigma", "Siren", "Femme Fatale", "Stiletto", "Firecracker", "Tattoo", "Geisha", "T-Back", "Goddess", "Temperance", "Half Pint", "Tequila", "Harlem", "Terror", "Heroin", "Thunderbird", "Infinity", "Ultra", "Insomnia", "Vanity", "Ivy", "Velvet", "Legacy", "Vixen", "Lithium", "Voodoo", "Lolita", "Wicked", "Lotus", "Widow", "Mademoiselle", "Xenon", "Kahina", "Teuta", "Isis", "Dihya", "Artemis", "Nefertiti", "RunningEagle", "Atalanta", "Sekhmet", "Colestah", "Athena", "Ishtar", "Calamity Jane", "Enyo", "Ashtart", "Pearl Heart", "Bellona", "Juno", "Belle Starr", "White Tights", "Tanit", "Hua Mulan", "Shieldmaiden", "Devi", "Boudica", "Valkyrie", "Selkie", "Medb", "Cleo", "Venus", "Fate", "Beguile", "Deviant", "Illusion", "Crafty", "Variance", "Delusion", "Deceit", "Caprice", "Deception", "Waylay", "Aberr", "Myth", "Ambush", "Variant", "Daydream", "Feint", "Hero", "Night Terror", "Catch-22", "Villain", "Figment", "Puzzler", "Daredevil", "Virtual", "Curio", "Mercenary", "Chicanery", "Prodigy", "Voyager", "Trick", "Breach", "Wanderer", "Vile", "Miss Fortune", "Audacity", "Horror", "Vex", "Swagger", "Dismay", "Grudge", "Nerve", "Phobia", "Enmity", "Egomania", "Fright", "Animus", "Scheme", "Panic", "Hostility", "Paramour", "Agony", "Rancor", "X-hibit", "Inferno", "Malevolence", "Charade", "Blaze", "Poison", "Hauteur", "Crucible", "Spite", "Vainglory", "Haunter", "Spitefulness", "Narcissus", "Bane", "Venom", "Brass", "Volcano", "Vampire", "Hulk" };
        public static List<ushort> HunterAvailableMaps = new List<ushort>() { };
        public static uint[] Body = { 1007, 2008 };
        public static uint[] Class = { 1000, 2000, 4000, 5000, 6000, 7000, 10000, 8000, 16000, 9000 };
        public enum BotLevel
        {
            Noob = 0,
            Easy = 1,
            Normal = 2,
            Medium = 3,
            Hard = 4,
            Insane = 5,
            GodEater = 6,
        }
        public enum BotType
        {
            PVM,
            PVP,
            Market,
            Qualifier
        }
        public enum BotState
        {
            Find = 0,
            Jump = 1,
            Attacking = 2,
            Chat = 3,
            Qualifier = 4,
        }
        public static void TimerCallBack(AI aI, int time)
        {
            switch (aI.State)
            {
                case BotState.Find: aI.CheckTarget(); break;
                case BotState.Jump: aI.SearchTarget(); break;
                case BotState.Attacking: aI.AttackingTarget(); break;
            }
        }
        public static AI CreateAi(VirusX.ServerSockets.Packet stream, GameClient client, BotType type, BotLevel level, ushort MapID, ushort X, ushort Y, ushort SpellID = 0)
        {
            var AI = new AI();
            AI.Bot = new GameClient(null);

            #region Instance
            AI.Bot.Fake = true;
            AI.Bot.Player = new Player(AI.Bot);
            AI.Bot.Inventory = new Inventory(AI.Bot);
            AI.Bot.Equipment = new Equip(AI.Bot);
            AI.Bot.Warehouse = new Warehouse(AI.Bot);
            AI.Bot.MySpells = new Spell(AI.Bot);
            AI.Bot.MyProfs = new Proficiency(AI.Bot);
            AI.Bot.Status = new MsgStatus();
            AI.Bot.MyVendor = new Vendor(AI.Bot);
            AI.Bot.Player.SubClass = new SubClass();
            AI.Bot.Player.Nobility = new Nobility(AI.Bot);
            AI.Bot.Player.View = new RoleView(AI.Bot);
            AI.Bot.Player.Associate = new Associate.MyAsociats(AI.Bot.Player.UID);
            AI.Bot.Player.MyClan = new Clan();
            AI.Bot.Player.QuestGUI = new Quests(AI.Bot.Player);
            AI.Bot.MyTrade = new Trade(AI.Bot);
            AI.Bot.Confiscator = new Confiscator();
            AI.Bot.ExtraStatus = new ConcurrentDictionary<RoleStatus.StatuTyp, RoleStatus>();
            AI.Bot.ArenaStatistic = new MsgArena.User();
            AI.Bot.DemonExterminator = new DemonExterminator();
            AI.Bot.Player.MyChi = new VirusX.Role.Instance.Chi(AI.Bot.Player.UID);
            AI.Bot.Beasts = new Beasts(AI.Bot);
            AI.Bot.Rune = new Rune(AI.Bot);
            AI.Bot.Player.MyUnion = new Union();
            AI.Bot.MyArchives = new Archives(AI.Bot);
            AI.Bot.MyNinja = new Ninja(AI.Bot);
            AI.Bot.HundredWeapons = new VirusX.Role.Instance.HundredWeapons(AI.Bot);
            #endregion

            #region Information
        TryOtherName:
            string name = AvailableNames[Program.GetRandom.Next(0, AvailableNames.Length)];
            foreach (var value in Pool.Values)
            {
                if (value.Bot.Player.Name == name)
                    goto TryOtherName;
            }
            AI.Bot.Player.Name = name;
            AI.Bot.Player.Body = (ushort)Body[Program.GetRandom.Next(0, Body.Length)];
            AI.Bot.Player.Class = type == BotType.PVM ? 4049 : type == BotType.PVP ? 1049 : type == BotType.Qualifier ? (uint)Class[Program.GetRandom.Next(0, Class.Length)] : 1049;
            AI.Bot.Player.HitPoints = 80000;
            AI.Bot.Status.MaxHitpoints = 80000;
            AI.Bot.Player.VipLevel = 6;
            AI.Bot.Player.Face = AI.Bot.Player.Body == 2008 ? (ushort)Program.GetRandom.Next(1, 178) : (ushort)Program.GetRandom.Next(201, 369);
            client.Player.Hair = (ushort)((ushort)(Program.GetRandom.Next(3, 9) * 100) + (ushort)Program.GetRandom.Next(10, 36));
            AI.Bot.Player.NobilityRank = Nobility.NobilityRank.King;
            AI.Bot.Player.Angle = VirusX.Role.Flags.ConquerAngle.SouthEast;
            AI.Bot.Player.ServerID = (ushort)GroupServerList.MyServerInfo.ID;
            AI.Bot.Player.UID = VirusX.Pool.ClientCounter.Next;
            AI.Bot.Player.Reborn = 2;
            AI.Bot.Player.FirstClass = 1005;
            AI.Bot.Player.SecoundeClass = 2005;
            AI.Bot.Player.Flowers = new Flowers(AI.Bot.Player.UID, AI.Bot.Player.Name);
            AI.Bot.MyHouse = new House(AI.Bot.Player.UID);
            AI.Bot.Player.InnerPower = new InnerPower(AI.Bot.Player.Name, AI.Bot.Player.UID);
            AI.Bot.Player.Action = VirusX.Role.Flags.ConquerAction.Sit;
            AI.Bot.Map = client.Map;
            AI.Bot.Player.DynamicID = client.Player.DynamicID;
            AI.Bot.Player.X = X;
            AI.Bot.Player.Y = Y;
            AI.Bot.Player.Map = (ushort)MapID;
            AI.Bot.Player.Level = (ushort)client.Player.Level;
            client.Send(AI.Bot.Player.GetArray(stream, false));
            AI.Bot.FullLoading = true;
            AI.Bot.Player.CompleteLogin = true;
            AI.Bot.Map.Enquer(AI.Bot);
            AI.OwnerUID = client.Player.UID;
            if (type == BotType.Qualifier)
            {
                AI.Bot.ArenaStatistic.ApplayInfo(AI.Bot.Player);
                AI.Bot.ArenaStatistic.Info.ArenaPoints = 4000;
                MsgArena.ArenaPoll.TryAdd(AI.Bot.Player.UID, AI.Bot.ArenaStatistic);
                MsgSchedules.Arena.DoSignup(stream, AI.Bot);
            }

            #endregion

            #region Equipment
            Equipment(stream, AI.Bot);
            AI.Bot.Equipment.OnDequeue();
            #endregion

            #region Level
            switch (level)
            {
                case BotLevel.Noob:
                    AI.configuration.AttackStamp = 1000;
                    AI.configuration.JumpStamp = 1000;
                    AI.configuration.HitRate = 40;
                    break;
                case BotLevel.Easy:
                    AI.configuration.AttackStamp = 900;
                    AI.configuration.JumpStamp = 900;
                    AI.configuration.HitRate = 50;
                    break;
                case BotLevel.Normal:
                    AI.configuration.AttackStamp = 800;
                    AI.configuration.JumpStamp = 800;
                    AI.configuration.HitRate = 60;
                    break;
                case BotLevel.Medium:
                    AI.configuration.AttackStamp = 700;
                    AI.configuration.JumpStamp = 700;
                    AI.configuration.HitRate = 70;
                    break;
                case BotLevel.Hard:
                    AI.configuration.AttackStamp = 600;
                    AI.configuration.JumpStamp = 600;
                    AI.configuration.HitRate = 80;
                    break;
                case BotLevel.Insane:
                    AI.configuration.AttackStamp = 500;
                    AI.configuration.JumpStamp = 500;
                    AI.configuration.HitRate = 90;
                    break;
                case BotLevel.GodEater:
                    AI.configuration.AttackStamp = 300;
                    AI.configuration.JumpStamp = 300;
                    AI.configuration.HitRate = 100;
                    break;
            }
            #endregion

            //#region Spell
            //ServerAutoCheck.ClassSpell(AI.Bot);
            //#endregion

            AI.Type = type;
            if (type == BotType.PVP)
            {
                AI.Target = client.Player;
            }
            //AI.Timer = new IDisposable[] { AIAction.Add(AI.Timer) };
            AI.DisposalSyncRoot = new object();
            Pool.Add(AI.Bot.Player.UID, AI);
            return AI;
        }
        public static uint GetSpell(GameClient AI)
        {
            #region Trojan
            if (AtributesStatus.IsTrojan(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.SuperCyclone, (ushort)VirusX.Role.Flags.SpellID.ScrenSword, (ushort)VirusX.Role.Flags.SpellID.FastBlader };
                return Spell[VirusX.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Warrior
            if (AtributesStatus.IsWarrior(AI.Player.Class))
            {
                if (!AI.MyArchives.Items.ContainsKey(Archives.TypeID.Dragonhowl))
                {
                    AI.MyArchives.AddItem(Archives.TypeID.Dragonhowl, 1, 0, 1, 0);

                }
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.WaveofBlood, (ushort)VirusX.Role.Flags.SpellID.Ironbone };
                return Spell[VirusX.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Archer
            if (AtributesStatus.IsArcher(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.ScatterFire };
                return Spell[VirusX.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Ninja
            if (AtributesStatus.IsNinja(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.FatalStrike, (ushort)VirusX.Role.Flags.SpellID.TwilightDance, (ushort)VirusX.Role.Flags.SpellID.FatalSpin, (ushort)VirusX.Role.Flags.SpellID.SuperTwofoldBlade };
                return Spell[VirusX.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Monk
            if (AtributesStatus.IsMonk(AI.Player.Class))
            {
                uint[] Spell = { (uint)(ushort)VirusX.Role.Flags.SpellID.RadiantPalm };
                return Spell[VirusX.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Pirate
            if (AtributesStatus.IsPirate(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.EagleEye, (ushort)VirusX.Role.Flags.SpellID.GaleBomb, (ushort)VirusX.Role.Flags.SpellID.BladeTempest, (ushort)VirusX.Role.Flags.SpellID.Windstorm, (ushort)VirusX.Role.Flags.SpellID.Gunfire, (ushort)VirusX.Role.Flags.SpellID.SeaBurial, (ushort)VirusX.Role.Flags.SpellID.ImmortalForce };
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Dragon-Warrior
            if (AtributesStatus.IsLee(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.DragonCyclone, (ushort)VirusX.Role.Flags.SpellID.SpeedKick, (ushort)VirusX.Role.Flags.SpellID.ViolentKick, (ushort)VirusX.Role.Flags.SpellID.StormKick,
                    (ushort)VirusX.Role.Flags.SpellID.CrackingSwipe, (ushort)VirusX.Role.Flags.SpellID.SplittingSwipe, (ushort)VirusX.Role.Flags.SpellID.DragonPunch , (ushort)VirusX.Role.Flags.SpellID.DragonSlash};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Water
            if (AtributesStatus.IsWater(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.ChainBolt, (ushort)VirusX.Role.Flags.SpellID.Thunder };
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Fire
            if (AtributesStatus.IsFire(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.ChainBolt, (ushort)VirusX.Role.Flags.SpellID.FireRing, (ushort)VirusX.Role.Flags.SpellID.FireMeteor, (ushort)VirusX.Role.Flags.SpellID.FireCircle,
                    (ushort)VirusX.Role.Flags.SpellID.Tornado, (ushort)VirusX.Role.Flags.SpellID.Bomb, (ushort)VirusX.Role.Flags.SpellID.FireofHell , (ushort)VirusX.Role.Flags.SpellID.FlameLotus};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Windwalker
            if (AtributesStatus.IsWindWalker(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.Omnipotence, (ushort)VirusX.Role.Flags.SpellID.RageofWar, (ushort)VirusX.Role.Flags.SpellID.AngerofStomper, (ushort)VirusX.Role.Flags.SpellID.HorrorofStomper,
                    (ushort)VirusX.Role.Flags.SpellID.PeaceofStomper, (ushort)VirusX.Role.Flags.SpellID.Thundercloud, (ushort)VirusX.Role.Flags.SpellID.SwirlingStorm};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region ThunderStrike
            if (AtributesStatus.IsThunderStriker(AI.Player.Class))
            {
                uint[] Spell = { (uint)VirusX.Role.Flags.SpellID.WindstormBattleaxe, (ushort)VirusX.Role.Flags.SpellID.DevouringStrike, (ushort)VirusX.Role.Flags.SpellID.SkyFall, (ushort)VirusX.Role.Flags.SpellID.HeavensWrath,
                    (ushort)VirusX.Role.Flags.SpellID.CrackingShock, (ushort)VirusX.Role.Flags.SpellID.ThunderBlast, (ushort)VirusX.Role.Flags.SpellID.Megabolt , (ushort)VirusX.Role.Flags.SpellID.ThunderRampage};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            return 0;
        }
        public static void Equipment(VirusX.ServerSockets.Packet stream, GameClient pclient)
        {
            pclient.Equipment.Add(stream, (uint)360354, VirusX.Role.Flags.ConquerItem.LeftWeaponAccessory, 0, 1, 0, (VirusX.Role.Flags.Gem)0, (VirusX.Role.Flags.Gem)0);//
            pclient.Equipment.Add(stream, (uint)360355, VirusX.Role.Flags.ConquerItem.RightWeaponAccessory, 0, 1, 0, (VirusX.Role.Flags.Gem)0, (VirusX.Role.Flags.Gem)0);//
            pclient.Equipment.Add(stream, (uint)2100075, VirusX.Role.Flags.ConquerItem.Bottle, 0, 1, 0, (VirusX.Role.Flags.Gem)0, (VirusX.Role.Flags.Gem)0);//
            pclient.Equipment.Add(stream, (uint)200001, VirusX.Role.Flags.ConquerItem.SteedMount, 0, 1, 0, (VirusX.Role.Flags.Gem)0, (VirusX.Role.Flags.Gem)0);//
            pclient.Equipment.Add(stream, 300000, VirusX.Role.Flags.ConquerItem.Steed, 12);//Steed
            pclient.Equipment.Add(stream, 203009, VirusX.Role.Flags.ConquerItem.RidingCrop, 12, 1);//Crop
            pclient.Equipment.Add(stream, 201009, VirusX.Role.Flags.ConquerItem.Fan, 12, 1, 0, (VirusX.Role.Flags.Gem)103, (VirusX.Role.Flags.Gem)103);//Fan
            pclient.Equipment.Add(stream, 202009, VirusX.Role.Flags.ConquerItem.Tower, 12, 1, 0, (VirusX.Role.Flags.Gem)123, (VirusX.Role.Flags.Gem)123);//Tower

            if (!AtributesStatus.IsTaoist(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, 120269, VirusX.Role.Flags.ConquerItem.Necklace, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Necklace
                pclient.Equipment.Add(stream, (uint)150269, VirusX.Role.Flags.ConquerItem.Ring, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Ring
                pclient.Equipment.Add(stream, (uint)160249, VirusX.Role.Flags.ConquerItem.Boots, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Boot
            }
            if (AtributesStatus.IsTrojan(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, (uint)410439, VirusX.Role.Flags.ConquerItem.RightWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//SkyBlade
                pclient.Equipment.Add(stream, (uint)410439, VirusX.Role.Flags.ConquerItem.LeftWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//SquallSword
                pclient.Equipment.Add(stream, (uint)130309, VirusX.Role.Flags.ConquerItem.Armor, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//ObsidianArmor
                pclient.Equipment.Add(stream, (uint)118309, VirusX.Role.Flags.ConquerItem.Head, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//PeerlessCoronet
            }
            else if (AtributesStatus.IsWarrior(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, (uint)560439, VirusX.Role.Flags.ConquerItem.RightWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Spear
                pclient.Equipment.Add(stream, (uint)900309, VirusX.Role.Flags.ConquerItem.LeftWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Shield
                pclient.Equipment.Add(stream, (uint)131309, VirusX.Role.Flags.ConquerItem.Armor, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Armor
                pclient.Equipment.Add(stream, (uint)111309, VirusX.Role.Flags.ConquerItem.Head, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Head
            }
            if (AtributesStatus.IsArcher(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, (uint)500429, VirusX.Role.Flags.ConquerItem.RightWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Bow
                pclient.Equipment.Add(stream, (uint)1050000, VirusX.Role.Flags.ConquerItem.LeftWeapon);//Arrow
                pclient.Equipment.Add(stream, (uint)133309, VirusX.Role.Flags.ConquerItem.Armor, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Armor
                pclient.Equipment.Add(stream, (uint)113309, VirusX.Role.Flags.ConquerItem.Head, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Head
            }
            else if (AtributesStatus.IsNinja(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, (uint)601439, VirusX.Role.Flags.ConquerItem.RightWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Katana
                pclient.Equipment.Add(stream, (uint)601439, VirusX.Role.Flags.ConquerItem.LeftWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Katana
                pclient.Equipment.Add(stream, (uint)135309, VirusX.Role.Flags.ConquerItem.Armor, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Armor
                pclient.Equipment.Add(stream, (uint)112309, VirusX.Role.Flags.ConquerItem.Head, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Head
            }
            if (AtributesStatus.IsMonk(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, (uint)610439, VirusX.Role.Flags.ConquerItem.RightWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Beads
                pclient.Equipment.Add(stream, (uint)610439, VirusX.Role.Flags.ConquerItem.LeftWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Beads
                pclient.Equipment.Add(stream, (uint)136309, VirusX.Role.Flags.ConquerItem.Armor, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Armor
                pclient.Equipment.Add(stream, (uint)143309, VirusX.Role.Flags.ConquerItem.Head, 12, 7, 255, (VirusX.Role.Flags.Gem)13, (VirusX.Role.Flags.Gem)13);//Head
            }
            else if (AtributesStatus.IsPirate(pclient.Player.Class))
            {

            }
            if (AtributesStatus.IsLee(pclient.Player.Class))
            {

            }
            else if (AtributesStatus.IsWater(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, (uint)421439, VirusX.Role.Flags.ConquerItem.RightWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)3, (VirusX.Role.Flags.Gem)3);//BackSword
                pclient.Equipment.Add(stream, (uint)134309, VirusX.Role.Flags.ConquerItem.Armor, 12, 7, 255, (VirusX.Role.Flags.Gem)3, (VirusX.Role.Flags.Gem)3);//Armor
                pclient.Equipment.Add(stream, (uint)114309, VirusX.Role.Flags.ConquerItem.Head, 12, 7, 255, (VirusX.Role.Flags.Gem)3, (VirusX.Role.Flags.Gem)3);//Head
            }
            if (AtributesStatus.IsFire(pclient.Player.Class))
            {
                pclient.Equipment.Add(stream, (uint)421439, VirusX.Role.Flags.ConquerItem.RightWeapon, 12, 7, 255, (VirusX.Role.Flags.Gem)3, (VirusX.Role.Flags.Gem)3);//BackSword
                pclient.Equipment.Add(stream, (uint)134309, VirusX.Role.Flags.ConquerItem.Armor, 12, 7, 255, (VirusX.Role.Flags.Gem)3, (VirusX.Role.Flags.Gem)3);//Armor
                pclient.Equipment.Add(stream, (uint)114309, VirusX.Role.Flags.ConquerItem.Head, 12, 7, 255, (VirusX.Role.Flags.Gem)3, (VirusX.Role.Flags.Gem)3);//Head
            }
            else if (AtributesStatus.IsWindWalker(pclient.Player.Class))
            {

            }
            if (AtributesStatus.IsThunderStriker(pclient.Player.Class))
            {

            }
        }
        public class AI
        {
            public uint UID, OwnerUID;
            public GameClient Bot;
            public IMapObj Target;
            public BotLevel Level;
            public BotType Type;
            public BotState State;
            public string TalkMessage;
            public IDisposable[] Timer;
            public object DisposalSyncRoot;
            public Config configuration = new Config();
            public void CheckTarget()
            {
                switch (Type)
                {
                    case BotType.PVM:
                        {
                            if (Target == null)
                            {
                                foreach (IMapObj Obj in Bot.Player.View.Roles(MapObjectType.Monster).Where(p => p.Alive))
                                {
                                    if (Core.GetDistance(Bot.Player.X, Bot.Player.Y, Obj.X, Obj.Y) <= 12)
                                    {
                                        var target = Obj as MonsterRole;
                                        if (!target.Name.Contains("Guard"))
                                        {
                                            Target = Obj;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }

                State = Target != null ? Target.Alive ? BotState.Jump : BotState.Find : Type == BotType.PVM ? BotState.Jump : BotState.Find;
            }
            public void SearchTarget()
            {
                #region Condiction
                if (Type == BotType.PVP || Type == BotType.Qualifier)
                {
                    var target = Target as Player;
                    if (!Target.Alive || target.ContainFlag(MsgUpdate.Flags.Fly))
                    {
                        State = BotState.Find;
                        return;
                    }
                    if (!target.Owner.Socket.Alive || Bot.Player.DynamicID != target.DynamicID)
                        Remove();
                }
                #endregion

                if (DateTime.Now > configuration.LastJump.AddMilliseconds(configuration.JumpStamp))
                {
                    configuration.LastJump = DateTime.Now;
                    ushort X = (ushort)Program.GetRandom.Next(Bot.Player.X - 1, Bot.Player.X + 1);
                    ushort Y = (ushort)Program.GetRandom.Next(Bot.Player.Y - 1, Bot.Player.Y + 1);
                    if (Type == BotType.PVM && Target == null)
                    {
                        X = (ushort)Program.GetRandom.Next(Bot.Player.X - 10, Bot.Player.X + 10);
                        Y = (ushort)Program.GetRandom.Next(Bot.Player.Y - 10, Bot.Player.Y + 10);
                    }
                    if (Target != null)
                    {
                        X = (ushort)Program.GetRandom.Next(Target.X - 3, Target.X + 3);
                        Y = (ushort)Program.GetRandom.Next(Target.Y - 3, Target.Y + 3);
                    }
                    if (!Bot.Map.ValidLocation(X, Y))
                    {
                        X = Bot.Player.X;
                        Y = Bot.Player.Y;
                    }
                    using (var rec = new RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        ActionQuery action = new ActionQuery()
                        {
                            ObjId = Bot.Player.UID,
                            Type = ActionType.Jump,
                            PositionX = Bot.Player.X,
                            PositionY = Bot.Player.Y,
                            TargetPositionX = X,
                            TargetPositionY = Y,
                        };
                        Bot.Player.View.SendView(stream.ActionCreate(action), true);
                        Bot.Player.Angle = Core.GetAngle(Bot.Player.X, Bot.Player.Y, X, Y);
                        Bot.Player.Action = VirusX.Role.Flags.ConquerAction.Jump;
                        Bot.Map.View.MoveTo<IMapObj>(Bot.Player, X, Y);
                        Bot.Player.X = X;
                        Bot.Player.Y = Y;
                        Bot.Player.View.Role(false, stream);
                        Bot.Player.LastMove = DateTime.Now;
                    }
                }

                State = Target != null ? BotState.Attacking : BotState.Find;
            }
            public void AttackingTarget()
            {
                if (!Bot.Player.Alive || Target == null || !Target.Alive)
                {
                    State = BotState.Find;
                    return;
                }

                if (Type == BotType.PVP || Type == BotType.Qualifier)
                {
                    var target = Target as Player;
                    if (target.ContainFlag(MsgUpdate.Flags.Fly))
                    {
                        State = BotState.Find;
                        return;
                    }
                    if (!target.Owner.Socket.Alive || Bot.Player.DynamicID != target.DynamicID)
                    {
                        Remove();
                        return;
                    }
                }

                if (Core.GetDistance(Target.X, Target.Y, Bot.Player.X, Bot.Player.Y) > 12)
                {
                    State = BotState.Find;
                    return;
                }

                uint SpellID = GetSpell(Bot);
                if (Bot.MySpells.ClientSpells.ContainsKey((ushort)SpellID))
                {
                    if (DateTime.Now > configuration.LastAttack.AddMilliseconds(configuration.AttackStamp))
                    {
                        configuration.LastAttack = DateTime.Now;

                        using (var rec = new RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            if (Core.Rate(configuration.HitRate))
                                MsgAttackPacket.ProcescMagic(Bot, stream, new InteractQuery() { OpponentUID = Target.UID, UID = Bot.Player.UID, X = Target.X, Y = Target.Y, SpellID = (ushort)SpellID }, true);
                            if (!Target.Alive)
                                Target = null;
                        }
                        State = BotState.Find;
                    }
                }
            }
            public void Remove()
            {
                lock (DisposalSyncRoot)
                {
                    if (Timer == null) return;
                    for (int i = 0; i < Timer.Length; i++)
                    {
                        if (Timer[i] != null)
                        {
                            Timer[i].Dispose();
                            Timer[i] = null;
                        }
                    }
                }
                Bot.Map.Denquer(Bot);
                Pool.Remove(Bot.Player.UID);
            }
        }
        public class Config
        {
            public bool CanJump = true, CanAttack = true;
            public bool CanTalk = false;
            public DateTime LastJump, LastAttack, LastTalk;
            public uint UID, TargetUID, JumpStamp, AttackStamp, HitRate;
        }
    }
}
