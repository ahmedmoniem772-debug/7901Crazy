using ConquerOnline.Client;
using ConquerOnline.Database;
using ConquerOnline.Game.MsgServer;
using ConquerOnline.Game.MsgServer.AttackHandler.Calculate;
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
using Time32 = System.Time32;

namespace ConquerOnline.AIB
{
    public enum BotType
    {
        MyAi,
        AI,
        MatrixAI
    }
    public enum BotLevel
    {
        Noob = 0,
        Easy = 1,
        Normal = 2,
        Medium = 3,
        Hard = 4,
        Insane = 5,
        Ketos = 6
    }
    public unsafe class ConquerAI
    {
        public static uint GetSpell(GameClient AI)
        {
            #region Trojan
            if (AtributesStatus.IsTrojan(AI.Player.Class))
            {
                uint[] Spell = { (uint)ConquerOnline.Role.Flags.SpellID.FastBlader, (ushort)ConquerOnline.Role.Flags.SpellID.ScrenSword, (ushort)ConquerOnline.Role.Flags.SpellID.Hercules };
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Warrior
            if (AtributesStatus.IsWarrior(AI.Player.Class))
            {
                uint[] Spell = { (uint)ConquerOnline.Role.Flags.SpellID.WaveofBlood, (ushort)ConquerOnline.Role.Flags.SpellID.Ironbone };
                return Spell[ConquerOnline.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Archer
            if (AtributesStatus.IsArcher(AI.Player.Class))
            {
                uint[] Spell = { (uint)ConquerOnline.Role.Flags.SpellID.ArrowBlades};
                return Spell[ConquerOnline.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Ninja
            if (AtributesStatus.IsNinja(AI.Player.Class))
            {
                uint[] Spell = { (uint)ConquerOnline.Role.Flags.SpellID.SuperTwofoldBlade};
                return Spell[ConquerOnline.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Monk
            if (AtributesStatus.IsMonk(AI.Player.Class))
            {
                uint[] Spell = { (uint)(ushort)ConquerOnline.Role.Flags.SpellID.RadiantPalm };
                return Spell[ConquerOnline.Pool.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Pirate
            if (AtributesStatus.IsPirate(AI.Player.Class))
            {
                uint[] Spell = { (ushort)ConquerOnline.Role.Flags.SpellID.BladeTempest,(ushort)ConquerOnline.Role.Flags.SpellID.SeaBurial};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Dragon-Warrior
            if (AtributesStatus.IsLee(AI.Player.Class))
            {
                uint[] Spell = {(ushort)ConquerOnline.Role.Flags.SpellID.CrackingSwipe, (ushort)ConquerOnline.Role.Flags.SpellID.SplittingSwipe, (ushort)ConquerOnline.Role.Flags.SpellID.DragonSlash};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region ThunderStrike
            if (AtributesStatus.IsThunderStriker(AI.Player.Class))
            {
                uint[] Spell = { (ushort)ConquerOnline.Role.Flags.SpellID.CrackingShock, (ushort)ConquerOnline.Role.Flags.SpellID.ThunderBlast, (ushort)ConquerOnline.Role.Flags.SpellID.Megabolt};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Water
            if (AtributesStatus.IsWater(AI.Player.Class))
            {
                uint[] Spell = { (uint)ConquerOnline.Role.Flags.SpellID.ChainBolt, (ushort)ConquerOnline.Role.Flags.SpellID.Thunder };
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Fire
            if (AtributesStatus.IsFire(AI.Player.Class))
            {
                uint[] Spell = { (ushort)ConquerOnline.Role.Flags.SpellID.FireMeteor, (ushort)ConquerOnline.Role.Flags.SpellID.FireCircle,
                    (ushort)ConquerOnline.Role.Flags.SpellID.Tornado, (ushort)ConquerOnline.Role.Flags.SpellID.Bomb, (ushort)ConquerOnline.Role.Flags.SpellID.FireofHell };
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion

            #region Windwalker
            if (AtributesStatus.IsWindWalker(AI.Player.Class))
            {
                uint[] Spell = {  (ushort)ConquerOnline.Role.Flags.SpellID.RageofWar, (ushort)ConquerOnline.Role.Flags.SpellID.BurntFrost};
                return Spell[Program.GetRandom.Next(0, Spell.Length)];
            }
            #endregion
           
            return 0;
        }

        public static Dictionary<uint, ConquerAI> Ais = new Dictionary<uint, ConquerAI>();

        public static ConcurrentDictionary<uint, GameClient> Pool = new ConcurrentDictionary<uint, GameClient>();

        public static uint[] Body = { 1008, 2007 };

        public BotLevel Level;

        public bool HeroGathering = false;

        public GameClient Bot = null;

        public IMapObj Target = null;

        public int HP;

        public Time32 LastBotJump;

        public uint Damage = 0;

        public uint UID;

        private int ShootChance = 0;

        private Time32 LastAttack;

        public void SetLevel(BotLevel Level)
        {
            switch (Level)
            {

                case BotLevel.Noob:
                    ShootChance = 30;
                    Damage = 5000;
                    break;

                case BotLevel.Easy:
                    ShootChance = 35;
                    Damage = 10000;
                    break;

                case BotLevel.Normal:
                    ShootChance = 40;
                    Damage = 15000;
                    break;
                case BotLevel.Medium:
                    ShootChance = 50;
                    Damage = 25000;
                    break;
                case BotLevel.Hard:
                    ShootChance = 60;
                    Damage = 25000;
                    break;
                case BotLevel.Insane:
                    ShootChance = 70;
                    Damage = 25000;
                    break;

                case BotLevel.Ketos:
                    ShootChance = 100;
                    Damage = 35000;
                    break;
            }
        }

        public unsafe IMapObj GetTarget()
        {
            if (Target != null)
            {
                if (Target.Map != Bot.Player.Map)
                {
                    Target = null;
                }
            }
            if (Target == null)
            {
                foreach (IMapObj Obj in Bot.Player.View.Roles(MapObjectType.Player))
                {

                    if (Core.GetDistance(Bot.Player.X, Bot.Player.Y, Obj.X, Obj.Y) <= 12)
                    {
                        if (Obj.Alive)
                        {

                            Target = Obj;
                            break;
                        }
                    }
                    else
                        return null;
                    break;
                }
            }
            return Target;
        }

        public void JumpBot()
        {
            if (Bot != null && Bot.Map != null && Bot.Player != null && Bot.Player.Map == 700 && Bot.Player.Alive)
            {
                ushort X = (ushort)Program.GetRandom.Next(Bot.Player.X - 1, Bot.Player.X + 1);
                ushort Y = (ushort)Program.GetRandom.Next(Bot.Player.Y - 1, Bot.Player.Y + 1);
                if (Target == null)
                {
                    X = (ushort)Program.GetRandom.Next(Bot.Player.X - 10, Bot.Player.X + 10);
                    Y = (ushort)Program.GetRandom.Next(Bot.Player.Y - 10, Bot.Player.Y + 10);
                }
                if (Target != null)
                {
                    X = (ushort)Program.GetRandom.Next(Target.X - 3, Target.X + 3);
                    Y = (ushort)Program.GetRandom.Next(Target.Y - 3, Target.Y + 3);
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
                    ActionQuery action = new ActionQuery()//try
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
                    Bot.Player.Action = Flags.ConquerAction.Jump;
                    Bot.Map.View.MoveTo<IMapObj>(Bot.Player, X, Y);
                    Bot.Player.X = X;
                    Bot.Player.Y = Y;
                    Bot.Player.View.Role(false, stream);
                    Bot.Player.LastMove = DateTime.Now;
                    LastBotJump = Time32.Now;
                }
            }
        }

        public void AttackerBot()
        {
            if (Bot != null && Bot.Map != null && Bot.Player.View != null && Bot.Player != null && Bot.Player.HitPoints > 0 && Bot.Player.Map == 700)
            {
                if (!Target.Alive || Target == null)
                    return;
            
               // if (Time32.Now >= Bot.MyBot.LastAttack.AddSeconds(1))
                {
                    if (Bot.Player.Alive)
                    {
                        if (Bot.Status.MaxHitpoints > Bot.Player.HitPoints + 10000)
                            Bot.Player.HitPoints += 10000;
                        else
                        {
                            Bot.Player.HitPoints = (int)Bot.Status.MaxHitpoints;
                        }
                    }
                    GameClient attacked;
                    if (ConquerOnline.Pool.GamePoll.TryGetValue(Target.UID, out attacked))
                    {
                        uint SpellID = GetSpell(Bot);
                        if (Bot.MySpells.ClientSpells.ContainsKey((ushort)SpellID))
                        {
                            using (var rec = new RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                if (!attacked.Player.AllowAttack())
                                    return;
                                if (MyMath.Success(Bot.MyBot.ShootChance))
                                {
                                    Bot.Player.Mana += 10000;
                                    Bot.Player.Stamina += 100;
                                    Bot.Player.UpdateArrowBlades(stream, 3);
                                    Bot.Player.SendUpdate(stream, Bot.Player.Stamina, MsgUpdate.DataType.Stamina);
                                    MsgAttackPacket.ProcescMagic(Bot, stream, new InteractQuery()
                                    {
                                        OpponentUID = Target.UID,
                                        UID = Bot.Player.UID,
                                        X = Target.X,
                                        Y = Target.Y,
                                        SpellID = (ushort)SpellID
                                    }, true);
                                }
                            }
                        }
                    }
                    Bot.MyBot.LastAttack = Time32.Now;
                }
            }
        }

        public void Remover()
        {
            if (HeroGathering)
                return;
            if (Target == null)
                return;
            GameClient client;
            if (!ConquerOnline.Pool.GamePoll.TryGetValue(Target.UID, out client))
            {
                using (var rec = new RecycledPacket())
                {
                    var stream = rec.GetStream();
                    Bot.Map.RemoveAI(Bot);
                }
            }
            if (ConquerOnline.Pool.GamePoll.TryGetValue(Target.UID, out client))
            {
                if (client.Player.Map != Bot.Player.Map)
                {
                    using (var rec = new RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        Bot.Map.RemoveAI(Bot);
                    }
                }
            }
        }
    }
    public unsafe class ConquerAIHandle
    {
        public static void Action()
        {
            foreach (var Bot in ConquerAI.Pool.Values)
            {
                if (Bot != null)
                {
                    Bot.MyBot.GetTarget();
                    Bot.MyBot.JumpBot();
                    Bot.MyBot.AttackerBot();
                    Bot.MyBot.Remover();
                }
            }
        }
        public static BotType Type;
        public static Counter UID1 = new Counter(3999950000);
        public static Counter UID2 = new Counter(9000000);
        public static GameClient CreateBots(ConquerOnline.ServerSockets.Packet stream, BotLevel Level, uint Map, ushort X, ushort Y, bool HeroGathering = false)
        {
            GameClient Bot = Clases();
            Bot.MyBot = new ConquerAI();
            Bot.MyBot.Bot = Bot;
            Bot.MyBot.SetLevel(Level);
            string name = "Bot" + "[MT]";
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
                        AddSkill(stream, Bot);


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

        public static void AddSkill(ConquerOnline.ServerSockets.Packet stream, GameClient pclient)
        {

            #region Trojan
            if (AtributesStatus.IsTrojan(pclient.Player.Class))
            {
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.ScrenSword))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.ScrenSword, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.FastBlader))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.FastBlader, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.Hercules))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.Hercules, 4);
                }
            }
            #endregion

            #region Warrior
            if (AtributesStatus.IsWarrior(pclient.Player.Class))
            {
                if (!pclient.MyArchives.Items.ContainsKey(Archives.TypeID.Dragonhowl))
                {
                    pclient.MyArchives.AddItem(Archives.TypeID.Dragonhowl, 1, 0, 1, 0);

                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.WaveofBlood))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.WaveofBlood, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.Ironbone))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.Ironbone, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.Hercules))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.Hercules, 4);
                }
            }
            #endregion

            #region Archer
            if (AtributesStatus.IsArcher(pclient.Player.Class))
            {
                if (!pclient.MyArchives.Items.ContainsKey(Archives.TypeID.StoneCracker))
                {
                    pclient.MyArchives.AddItem(Archives.TypeID.StoneCracker, 1, 0, 1, 0);

                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.ArrowBlades))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.ArrowBlades, 9);
                }
            }
            #endregion

            #region IsNinja
            if (AtributesStatus.IsNinja(pclient.Player.Class))
            {
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.SuperTwofoldBlade))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.SuperTwofoldBlade, 4);
                }

            }
            #endregion

            #region IsMonk
            if (AtributesStatus.IsMonk(pclient.Player.Class))
            {

                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.RadiantPalm))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.RadiantPalm, 4);
                }

            }
            #endregion

            #region Pirate
            if (AtributesStatus.IsPirate(pclient.Player.Class))
            {
                if (!pclient.MyArchives.Items.ContainsKey(Archives.TypeID.LavaNut))
                {
                    pclient.MyArchives.AddItem(Archives.TypeID.LavaNut, 1, 0, 1, 0);

                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.BladeTempest))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.BladeTempest, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.SeaBurial))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.SeaBurial, 4);
                }

            }
            #endregion

            #region Lee
            if (AtributesStatus.IsLee(pclient.Player.Class))
            {
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.CrackingSwipe))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.CrackingSwipe, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.SplittingSwipe))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.SplittingSwipe, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.DragonSlash))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.DragonSlash, 4);
                }

            }
            #endregion

            #region ThunderStriker
            if (AtributesStatus.IsThunderStriker(pclient.Player.Class))
            {
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.CrackingShock))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.CrackingShock, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.ThunderBlast))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.ThunderBlast, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.Megabolt))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.Megabolt, 4);
                }

            }
            #endregion

            #region Water
            if (AtributesStatus.IsWater(pclient.Player.Class))
            {
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.Thunder))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.Thunder, 3);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.ChainBolt))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.ChainBolt, 3);
                }
            }
            #endregion

            #region Fire
            if (AtributesStatus.IsFire(pclient.Player.Class))
            {
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.Tornado))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.Tornado, 3);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.FireofHell))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.FireofHell, 3);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.Bomb))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.Bomb, 3);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.FireMeteor))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.FireMeteor, 3);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.FireCircle))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.FireCircle, 3);
                }
            }
            #endregion

            #region Windwalker
            if (AtributesStatus.IsWindWalker(pclient.Player.Class))
            {
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.RageofWar))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.RageofWar, 4);
                }
                if (!pclient.MySpells.ClientSpells.ContainsKey((ushort)ConquerOnline.Role.Flags.SpellID.BurntFrost))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                        pclient.MySpells.Add(rec.GetStream(), (ushort)ConquerOnline.Role.Flags.SpellID.BurntFrost, 4);
                }
            }
            #endregion

        }
        public static void BotsEquipment(ConquerOnline.ServerSockets.Packet stream, GameClient pclient)
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
            pclient.Equipment.AddEx(stream, 300000, Flags.ConquerItem.Steed, 12, 0, 0, (Flags.Gem)0, (Flags.Gem)0, false, Role.Flags.ItemEffect.None, PerfectionLevel);//Steed
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
                pclient.Equipment.AddEx(stream, (uint)130309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, false, Role.Flags.ItemEffect.None, PerfectionLevel, 4200018, SoulArmor);//ObsidianArmor
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

        public static GameClient RellenarInformacion(GameClient pclient, ConquerOnline.ServerSockets.Packet stream)
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
                Power.Attributes[0].Value = 3500;
                Power.Attributes[1] = new ConquerOnline.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[1].Type = MsgChiInfo.ChiAttribute.AddAttack;
                Power.Attributes[1].Value = 2000;
                Power.Attributes[2] = new ConquerOnline.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[2].Type = MsgChiInfo.ChiAttribute.Immunity;
                Power.Attributes[2].Value = 2000;
                Power.Attributes[3] = new ConquerOnline.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[3].Type = MsgChiInfo.ChiAttribute.CriticalStrike;
                Power.Attributes[3].Value = 2000;
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
