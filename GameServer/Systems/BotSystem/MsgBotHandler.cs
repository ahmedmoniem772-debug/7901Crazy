using VirusX.Client;
using VirusX.Database;
using VirusX.Game.MsgMonster;
using VirusX.Game.MsgNpc;
using VirusX.Game.MsgServer;
using VirusX.Role;
using VirusX.Role.Instance;
using VirusX.ServerSockets;
using VirusX.WindowsAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VirusX.Game.Ai
{
    public enum Mode : byte
    {
        Off,
        On,
    }
    public class BotSystem
    {

        public static IMapObj Target = null;
        public DateTime VipTime = DateTime.Now;

        public GameClient BEntity;

        public GameClient Owner;

        public bool save;

        public object AutoHunting;

        public int _endtime;
        public static void BotsEquipment(VirusX.ServerSockets.Packet stream, GameClient pclient)
        {
            uint PerfectionLevel = 54;
            uint SoulArmor = 822071;
            uint SoulH = 820073;
            pclient.Equipment.AddEx(stream, (uint)360354, Flags.ConquerItem.LeftWeaponAccessory, 0, 1, 0, (Flags.Gem)0, (Flags.Gem)0);//
            pclient.Equipment.AddEx(stream, (uint)360355, Flags.ConquerItem.RightWeaponAccessory, 0, 1, 0, (Flags.Gem)0, (Flags.Gem)0);//

            pclient.Equipment.AddEx(stream, 300000, Flags.ConquerItem.Steed, 12, 0, 0, (Flags.Gem)0, (Flags.Gem)0, true, Role.Flags.ItemEffect.None, PerfectionLevel);//Steed
            pclient.Equipment.AddEx(stream, 203009, Flags.ConquerItem.RidingCrop, 12, 0, 0, (Flags.Gem)0, (Flags.Gem)0, true, Role.Flags.ItemEffect.None, PerfectionLevel);//Crop
            pclient.Equipment.AddEx(stream, 201009, Flags.ConquerItem.Fan, 12, 1, 0, (Flags.Gem)103, (Flags.Gem)103, true, Role.Flags.ItemEffect.None, PerfectionLevel);//Fan
            pclient.Equipment.AddEx(stream, 202009, Flags.ConquerItem.Tower, 12, 1, 0, (Flags.Gem)123, (Flags.Gem)123, true, Role.Flags.ItemEffect.None, PerfectionLevel);//Tower
            pclient.Equipment.AddEx(stream, 204009, Flags.ConquerItem.Wing, 0, 1, 0, (Flags.Gem)103, (Flags.Gem)123, true, Role.Flags.ItemEffect.None, PerfectionLevel);//Wing
            pclient.Equipment.AddEx(stream, 120269, Flags.ConquerItem.Necklace, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Necklace
            pclient.Equipment.AddEx(stream, (uint)150269, Flags.ConquerItem.Ring, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Ring
            pclient.Equipment.AddEx(stream, (uint)160249, Flags.ConquerItem.Boots, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Boot
            if (AtributesStatus.IsTrojan(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)410439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)130309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//PeerlessCoronet
            }
            else if (AtributesStatus.IsWarrior(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)560439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Spear
                pclient.Equipment.AddEx(stream, (uint)900309, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Shield
                pclient.Equipment.AddEx(stream, (uint)131309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)111309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//Head
            }
            if (AtributesStatus.IsArcher(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)500429, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Bow
                pclient.Equipment.AddEx(stream, (uint)1050000, Flags.ConquerItem.LeftWeapon);//Arrow
                pclient.Equipment.AddEx(stream, (uint)133309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)113309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//Head
            }
            else if (AtributesStatus.IsNinja(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)616439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Katana
                pclient.Equipment.AddEx(stream, (uint)616439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Katana
                pclient.Equipment.AddEx(stream, (uint)135309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)112309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//Head
            }
            if (AtributesStatus.IsMonk(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)622439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Beads
                pclient.Equipment.AddEx(stream, (uint)622439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//Beads
                pclient.Equipment.AddEx(stream, (uint)136309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)143309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//Head
            }
            else if (AtributesStatus.IsPirate(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)611439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)612439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)139309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//PeerlessCoronet
            }
            if (AtributesStatus.IsLee(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)617439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)617439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)138309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//PeerlessCoronet
            }
            else if (AtributesStatus.IsWater(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)421439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//BackSword
                pclient.Equipment.AddEx(stream, (uint)134309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)114309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//Head
            }
            if (AtributesStatus.IsFire(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)421439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//BackSword
                pclient.Equipment.AddEx(stream, (uint)134309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//Armor
                pclient.Equipment.AddEx(stream, (uint)114309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)3, (Flags.Gem)3, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//Head
            }
            else if (AtributesStatus.IsWindWalker(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)626439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)626439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)101309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//PeerlessCoronet
            }
            if (AtributesStatus.IsThunderStriker(pclient.Player.Class))
            {
                pclient.Equipment.AddEx(stream, (uint)681439, Flags.ConquerItem.RightWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SkyBlade
                pclient.Equipment.AddEx(stream, (uint)680439, Flags.ConquerItem.LeftWeapon, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0);//SquallSword
                pclient.Equipment.AddEx(stream, (uint)102309, Flags.ConquerItem.Armor, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulArmor);//ObsidianArmor
                pclient.Equipment.AddEx(stream, (uint)118309, Flags.ConquerItem.Head, 12, 7, 255, (Flags.Gem)13, (Flags.Gem)13, true, Role.Flags.ItemEffect.None, PerfectionLevel, 0, SoulH);//PeerlessCoronet
            }
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
                Power.Attributes[0] = new VirusX.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[0].Type = MsgChiInfo.ChiAttribute.HPAdd;
                Power.Attributes[0].Value = 3500;
                Power.Attributes[1] = new VirusX.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[1].Type = MsgChiInfo.ChiAttribute.AddAttack;
                Power.Attributes[1].Value = 2000;
                Power.Attributes[2] = new VirusX.Role.Instance.Chi.ChiAttribute();
                Power.Attributes[2].Type = MsgChiInfo.ChiAttribute.Immunity;
                Power.Attributes[2].Value = 2000;
                Power.Attributes[3] = new VirusX.Role.Instance.Chi.ChiAttribute();
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

        public uint OwnerUID;

        public BotSystem(GameClient client)
        {
            Owner = client;
            BEntity = new GameClient((SecuritySocket)null);
            BEntity.Player = new Player(BEntity);
            BEntity.Player.UID = Pool.ClientCounter.Next;
            BEntity.Inventory = new Inventory(BEntity);
            BEntity.Equipment = new Role.Instance.Equip(BEntity);
            BEntity.Warehouse = new Warehouse(BEntity);
            BEntity.MyProfs = new Proficiency(BEntity);
            BEntity.MySpells = new Role.Instance.Spell(BEntity);
            BEntity.Fake = true;
            BEntity.HundredWeapons = new Role.Instance.HundredWeapons(BEntity);
            BEntity.Player.SubClass = new Role.Instance.SubClass();
            BEntity.Player.MyUnion = new Role.Instance.Union();
            BEntity.Player.MyChi = new Role.Instance.Chi(BEntity.Player.UID);
            BEntity.Player.InnerPower = new Role.Instance.InnerPower(BEntity.Player.Name, BEntity.Player.UID);
            BEntity.Player.Associate = new Role.Instance.Associate.MyAsociats(BEntity.Player.UID);
            BEntity.Rune = new Role.Instance.Rune(BEntity);
            BEntity.Beasts = new Role.Instance.Beasts(BEntity);
            BEntity.Player.MyClan = new Role.Instance.Clan();
            DataCore.LoadClient(BEntity.Player);
            OwnerUID = client.Player.UID;
            Load();
        }

        #region Skills Bots Using
        private static ushort[] SkillRobotTrojan = new ushort[3] { (ushort)1045, (ushort)1046, (ushort)1115 };
        private static ushort[] SkillRobotArcher = new ushort[1] { (ushort)8001 }; private static ushort[] SkillRobotNinja = new ushort[1] { (ushort)6000 };
        private static ushort[] SkillRobotMonk = new ushort[2] { (ushort)10381, (ushort)10415 };
        private static ushort[] SkillRobotWater = new ushort[1] { (ushort)1000 };
        private static ushort[] SkillRobotFire = new ushort[1] { (ushort)1002 };
        private static ushort[] SkillRobotAttacked = new ushort[5] { (ushort)6000, (ushort)10381, (ushort)10415, (ushort)1000, (ushort)1002 };
        private static ushort[] SkillXPRobot = new ushort[2] { (ushort)1110, (ushort)6011 };
        #endregion
        public static System.Time32 IsSeat = System.Time32.Now;
        public static System.Time32 IsWatcher = System.Time32.Now;
        public static System.Time32 AllInTime = System.Time32.Now;
        public static System.Time32 JumbDoWorkBotPoker = System.Time32.Now;
        #region ThreadingBots
        public Time32 ThreadCallBack = Time32.Now;
        public Time32 StampJumbCallback = Time32.Now;
        public Time32 StampHitCallback = Time32.Now;
        public static void CheakUp()
        {
            Time32 now32 = Time32.Now;
            foreach (var @BotSystem in Pool.AIAutoHuntingPoll.Values)
            {
                try
                {
                    if (@BotSystem.EndTime <= 0)
                    {
                        @BotSystem.OutBot();
                    }
                    else
                    {
                        if (@BotSystem.EndTime > 0)
                            --@BotSystem.EndTime;
                        Time32 now = Time32.Now;
                        if (now >= @BotSystem.StampJumbCallback.AddMilliseconds(3000))
                        {
                            @BotSystem.Jumb_DoWork();
                            @BotSystem.StampJumbCallback = now;
                        }
                        if (now >= @BotSystem.StampHitCallback.AddMilliseconds(2500))
                        {
                            @BotSystem.Hit_DoWork();
                            @BotSystem.StampHitCallback = now;
                        }
                    }
                }
                catch
                {
                }

            }

        }
        public void Jumb_DoWork()
        {
            if (BEntity == null)
                return;
            bool flag = false;
            foreach (var mapObj in BEntity.Player.View.Roles(MapObjectType.Monster).Where(e => e.Alive && Core.GetDistance(e.X, e.Y, BEntity.Player.X, BEntity.Player.Y) <= (short)18))
            {
                if (Core.Rate(100))
                {
                    flag = true;
                    MonsterRole monsterRole = mapObj as MonsterRole;
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        Role.Flags.ConquerAngle angle = GetAngle(mapObj.X, mapObj.Y, BEntity.Player.X, BEntity.Player.Y);
                        ushort x = mapObj.X;
                        ushort y = mapObj.Y;
                        IncXY(angle, ref x, ref y);
                        BEntity.Teleport(x, y, BEntity.Player.Map);
                        BEntity.DirectionChange = 0;
                        BEntity.Player.LastMove = DateTime.Now;
                        break;
                    }
                }
            }
            if (flag)
            {
                var all = BEntity.Map.View.GetAllMapRoles(Role.MapObjectType.Monster).Where(e => e.Name == TargetName).ToArray();
                if (all == null)
                    return;
                var Obj = all[Program.GetRandom.Next(0, all.Length)];
                if (Obj != null)
                {
                    Role.Flags.ConquerAngle dir = GetAngle(Obj.X, Obj.Y, BEntity.Player.X, BEntity.Player.Y);
                    ushort WalkX = Obj.X; ushort WalkY = Obj.Y;
                    IncXY(dir, ref WalkX, ref WalkY);
                    if (BEntity.Map.ValidLocation(WalkX, WalkY))
                    {
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            Game.MsgServer.InterActionWalk inter = new Game.MsgServer.InterActionWalk()
                            {
                                Mode = MsgInterAction.Action.Jump,
                                X = WalkX,
                                Y = WalkY,
                                UID = BEntity.Player.UID,
                                OponentUID = 0
                            };
                            unsafe
                            {
                                BEntity.Player.View.SendView(stream.InterActionWalk(&inter), true);
                            }
                            BEntity.Player.Angle = Role.Core.GetAngle(BEntity.Player.X, BEntity.Player.Y, WalkX, WalkY);
                            BEntity.Player.Action = Role.Flags.ConquerAction.Jump;
                            BEntity.Map.View.MoveTo<Role.IMapObj>(BEntity.Player, WalkX, WalkY);
                            BEntity.Player.X = WalkX;
                            BEntity.Player.Y = WalkY;
                            BEntity.Player.LastMove = DateTime.Now;
                        }
                    }
                }
                return;
            }

        }
        public void Hit_DoWork()
        {
            if (this.BEntity == null)
                return;
            bool flag = Database.ItemType.IsBow(this.BEntity.Equipment.RightWeapon);
            foreach (var role in this.BEntity.Player.View.Roles(MapObjectType.Monster))
            {
                if (Core.GetDistance(role.X, role.Y, this.BEntity.Player.X, this.BEntity.Player.Y) <= (short)8)
                {
                    MonsterRole Attacked = role as MonsterRole;
                    if (Attacked.CanRespawn(this.BEntity.Map))
                        Attacked.Respawn(false);
                    if (!Attacked.ContainFlag(MsgUpdate.Flags.Ghost) && !Attacked.Name.Contains("Guard"))
                    {
                        ushort Skills = 0;
                        if (this.BEntity.Player.Class >= 1000 && this.BEntity.Player.Class <= 1005)
                            Skills = BotSystem.SkillRobotTrojan[Pool.GetRandom.Next(BotSystem.SkillRobotTrojan.Length)];
                        if (this.BEntity.Player.Class >= 4000 && this.BEntity.Player.Class <= 4005)
                            Skills = BotSystem.SkillRobotArcher[Pool.GetRandom.Next(BotSystem.SkillRobotArcher.Length)];
                        if (this.BEntity.Player.Class >= 5000 && this.BEntity.Player.Class <= 5005)
                            Skills = BotSystem.SkillRobotNinja[Pool.GetRandom.Next(BotSystem.SkillRobotNinja.Length)];
                        if (this.BEntity.Player.Class >= 6000 && this.BEntity.Player.Class <= 6005)
                            Skills = BotSystem.SkillRobotMonk[Pool.GetRandom.Next(BotSystem.SkillRobotMonk.Length)];
                        if (this.BEntity.Player.Class >= 13000 && this.BEntity.Player.Class <= 13005)
                            Skills = BotSystem.SkillRobotWater[Pool.GetRandom.Next(BotSystem.SkillRobotWater.Length)];
                        if (this.BEntity.Player.Class >= 14000 && this.BEntity.Player.Class <= 14005)
                            Skills = BotSystem.SkillRobotFire[Pool.GetRandom.Next(BotSystem.SkillRobotFire.Length)];
                        if (!this.BEntity.MySpells.ClientSpells.ContainsKey(Skills))
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                                this.BEntity.MySpells.Add(rec.GetStream(), Skills);
                        }
                        if (!this.BEntity.Player.ContainFlag(MsgUpdate.Flags.Cyclone) && !this.BEntity.Player.ContainFlag(MsgUpdate.Flags.FatalStrike) && this.BEntity.Player.ContainFlag(MsgUpdate.Flags.XPList))
                        {
                            List<ushort> ListSkill = new List<ushort>();
                            ushort ID = 0;
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                for (int index = 0; index < BotSystem.SkillXPRobot.Length; ++index)
                                {
                                    if (this.BEntity.MySpells.ClientSpells.ContainsKey(BotSystem.SkillXPRobot[index]))
                                    {
                                        ListSkill.Add(BotSystem.SkillXPRobot[index]);
                                    }
                                    else
                                    {
                                        this.BEntity.MySpells.Add(stream, ID);
                                        ListSkill.Add(BotSystem.SkillXPRobot[index]);
                                    }
                                }
                            }
                            if (ListSkill.Count > 0)
                            {
                                ushort Damage = ListSkill[(int)(ushort)Pool.GetRandom.Next(ListSkill.Count)];
                                if (Damage > (ushort)0)
                                {
                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = rec.GetStream();
                                        MsgAttackPacket.Process(this.BEntity, new InteractQuery()
                                        {
                                            AtkType = (ushort)24,
                                            UID = this.BEntity.Player.UID,
                                            OpponentUID = this.BEntity.Player.UID,
                                            X = this.BEntity.Player.X,
                                            Y = this.BEntity.Player.Y,
                                            Damage = (int)Damage
                                        });
                                    }
                                }
                            }
                        }
                        Dictionary<ushort, MagicType.Magic> dictionary;
                        MsgSpell msgMagicInfo;
                        MagicType.Magic magic;
                        if (!this.BEntity.Player.ContainFlag(MsgUpdate.Flags.Cyclone) 
                            && !this.BEntity.Player.ContainFlag(MsgUpdate.Flags.FatalStrike) 
                            && Pool.Magic.TryGetValue(Skills, out dictionary)
                            && this.BEntity.MySpells.ClientSpells.TryGetValue(Skills, out msgMagicInfo) 
                            && dictionary.TryGetValue(msgMagicInfo.Level, out magic)
                            && Skills != (ushort)0 && magic != null 
                            && (int)magic.UseStamina <= (int)this.BEntity.Player.Stamina
                            && (int)magic.UseMana <= (int)this.BEntity.Player.Mana 
                            && (this.BEntity.Player.Rate(50)
                            || this.BEntity.Player.Class >= 13000
                            && this.BEntity.Player.Class <= 13005
                            || this.BEntity.Player.Class >= 14000 
                            && this.BEntity.Player.Class <= 14000))
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                InteractQuery Attack = new InteractQuery();
                                Attack.AtkType = (ushort)24;
                                Attack.UID = this.BEntity.Player.UID;
                                if ((BotSystem.SkillRobotAttacked).Contains<ushort>(Skills))
                                    Attack.OpponentUID = role.UID;
                                Attack.X = role.X;
                                Attack.Y = role.Y;
                                Attack.Damage = (int)Skills;
                                Attack.SpellID = Skills;
                                MsgAttackPacket.Process(this.BEntity, Attack);
                                break;
                            }
                        }
                        else if ((Core.GetDistance(role.X, role.Y, this.BEntity.Player.X, this.BEntity.Player.Y) <= (short)2 
                            || this.BEntity.Player.ContainFlag(MsgUpdate.Flags.FatalStrike)) 
                            && this.BEntity.Player.Class != 13005
                            && this.BEntity.Player.Class != 14005)
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                InteractQuery Attack = new InteractQuery();
                                Attack.AtkType = (ushort)2;
                                if (this.BEntity.Player.Class >= 4000 && this.BEntity.Player.Class <= 4005 && flag)
                                {
                                    Attack.AtkType = (ushort)24;
                                    Attack.Damage = 8001;
                                    Attack.SpellID = (ushort)8001;
                                }
                                Attack.UID = this.BEntity.Player.UID;
                                Attack.OpponentUID = role.UID;
                                Attack.X = role.X;
                                Attack.Y = role.Y;
                                MsgAttackPacket.Process(this.BEntity, Attack);
                                break;
                            }
                        }


                    }
                }
            }
        }
        public int EndTime
        {
            get
            {
                return _endtime;
            }
            set
            {
                if (BEntity != null)
                    BEntity.BotHuntingTime = value;
                _endtime = value;
            }
        }
        #endregion

        #region Angle
        public void IncXY(Role.Flags.ConquerAngle Facing, ref ushort x, ref ushort y)
        {
            sbyte num1;
            sbyte num2 = num1 = (sbyte)0;
            switch (Facing)
            {
                case Role.Flags.ConquerAngle.SouthWest:
                    num1 = (sbyte)1;
                    break;
                case Role.Flags.ConquerAngle.West:
                    num2 = (sbyte)-1;
                    num1 = (sbyte)1;
                    break;
                case Role.Flags.ConquerAngle.NorthWest:
                    num2 = (sbyte)-1;
                    break;
                case Role.Flags.ConquerAngle.North:
                    num2 = (sbyte)-1;
                    num1 = (sbyte)-1;
                    break;
                case Role.Flags.ConquerAngle.NorthEast:
                    num1 = (sbyte)-1;
                    break;
                case Role.Flags.ConquerAngle.East:
                    num2 = (sbyte)1;
                    num1 = (sbyte)-1;
                    break;
                case Role.Flags.ConquerAngle.SouthEast:
                    num2 = (sbyte)1;
                    break;
                case Role.Flags.ConquerAngle.South:
                    num2 = (sbyte)1;
                    num1 = (sbyte)1;
                    break;
            }
            x += (ushort)num2;
            y += (ushort)num1;
        }
        public Role.Flags.ConquerAngle GetAngle(ushort X, ushort Y, ushort X2, ushort Y2)
        {
            double x = (double)((int)X2 - (int)X);
            double num = Math.Atan2((double)((int)Y2 - (int)Y), x);
            if (num < 0.0)
                num += 2.0 * Math.PI;
            return (Role.Flags.ConquerAngle)((uint)(byte)(7.0 - Math.Floor(360.0 - num * 180.0 / Math.PI) / 45.0 % 8.0 - 1.0) % 8);
        }
        #endregion

        #region AddBots&&ActiveTime&&OutBot
        public string TargetName = "";
        public bool Add(out string errorstr, string TargetName, int AddHour = 0)
        {
            errorstr = "";
            if (TargetName == "(AddHour)")
            {
                if (ActiveTime(AddHour))
                {
                    SaveData();
                    errorstr = "just addhour";
                    return false;
                }
                errorstr = "you do not have more time";
            }
            if (ActiveTime())
            {
                if (Pool.AIAutoHuntingPoll.ContainsKey(Owner.Player.UID))
                {
                    errorstr = "you can't do this right now [1]";
                    return false;
                }
                ushort x = Owner.Player.X;
                ushort y = Owner.Player.Y;
                Owner.Map.GetRandCoord(ref x, ref y, 2);
                if (Owner.Map != null && Owner.Map.IsValidFlagNpc(x, y))
                {
                    BEntity.Player.Name = Owner.Player.Name + "[Bot]";
                    BEntity.Player.Body = Owner.Player.Body;
                    BEntity.Player.HitPoints = Owner.Player.HitPoints + 5000;
                    BEntity.Status.MaxHitpoints = (uint)BEntity.Player.HitPoints;
                    BEntity.Player.Action = Owner.Player.Action;
                    BEntity.Player.Level = (ushort)140;
                    BEntity.Player.Reborn = (byte)2;
                    BEntity.Player.Face = 153;
                    BEntity.Player.Hair = 32;
                    BEntity.Player.CountryID = Owner.Player.CountryID;
                    BEntity.Player.Angle = (Role.Flags.ConquerAngle)Pool.GetRandom.Next(0, 7);
                    BEntity.Player.Class = (uint)4005;
                    BEntity.Player.FirstClass = (uint)4005;
                    BEntity.Player.SecoundeClass = (uint)4005;
                    BEntity.Player.ServerID = (ushort)GroupServerList.MyServerInfo.ID;
                    BEntity.Map = Owner.Map;
                    BEntity.Player.X = x;
                    BEntity.Player.Y = y;
                    BEntity.Player.Map = Owner.Player.Map;
                    BEntity.Player.Vitality = (ushort)(((int)BEntity.Player.Level + BEntity.Player.BattlePower) * ((int)BEntity.Player.Reborn + 1));
                    DataCore.AtributeStatus.GetStatus(BEntity.Player);
                    DataCore.SetCharacterSides(BEntity.Player);
                    DataCore.LoadClient(BEntity.Player);
                    this.TargetName = TargetName;
                    StampJumbCallback = Time32.Now;
                    StampHitCallback = Time32.Now;
                    ThreadCallBack = Time32.Now;
                    StuffBot();
                    if (Pool.AIAutoHuntingPoll.TryAdd(Owner.Player.UID, this))
                    {
                        if (Pool.GamePoll.TryAdd(BEntity.Player.UID, BEntity))
                        {
                            BEntity.Map.Enquer(BEntity);
                            return true;
                        }
                        errorstr = "you can't do this right now [5]";
                    }
                    else
                        errorstr = "you can't do this right now [3]";
                }
                else
                    errorstr = "change your place";
            }
            else
                errorstr = "you do not have more time";
            return false;
        }

        public bool ActiveTime(int hour = 0)
        {
            DateTime now = DateTime.Now;
            bool flag = false;
            if (EndTime <= 0)
            {
                if (Owner.Inventory.Contain(3100069, 1) || Owner.Inventory.Contain(3100070, 1))
                {
                    using (RecycledPacket recycledPacket = new RecycledPacket())
                    {
                        ServerSockets.Packet stream = recycledPacket.GetStream();
                        if (Owner.Inventory.Remove(3100069, 1, stream))
                        {
                            EndTime += 21600;
                            VipTime = now;
                            SaveData();
                            flag = true;
                        }
                        if (Owner.Inventory.Remove(3100070, 1, stream))
                        {
                            EndTime += 43200;
                            VipTime = now;
                            SaveData();
                            flag = true;
                        }
                    }


                }
                else
                {
                    Owner.SendSysMesage("You Dont Have Token ActiveBots  Hours ");
                    return flag;
                }
               
                if (hour > 0)
                {
                    EndTime += hour * 60 * 60;
                    SaveData();
                    flag = true;
                }
            }
            else
            {
                if (hour > 0)
                {
                    EndTime += hour * 60 * 60;
                    SaveData();
                }
                flag = true;
            }
            return flag;
        }

        public void OutBot()
        {
            try
            {
                GameClient user;
                if (!Pool.GamePoll.TryRemove(BEntity.Player.UID, out user))
                    return;
                if (Pool.AIAutoHuntingPoll.ContainsKey(Owner.Player.UID))
                {
                    Pool.AIAutoHuntingPoll.Remove<uint, BotSystem>(Owner.Player.UID);
                    SaveData();
                    GameClient gameClient2;
                    if (Pool.GamePoll.TryGetValue(OwnerUID, out gameClient2))
                    {
                        uint conquerPoints = (uint)BEntity.Player.ConquerPoints;
                        BEntity.Player.ConquerPoints = 0;
                        gameClient2.Player.ConquerPoints -= (long)conquerPoints;
                        gameClient2.CreateBoxDialog("remove bot and you got " + conquerPoints.ToString() + " ConquerPoints.");
                        gameClient2.AI.SaveData();
                        gameClient2.AI = null;
                    }
                }
                using (RecycledPacket recycledPacket = new RecycledPacket())
                {
                    ServerSockets.Packet stream = recycledPacket.GetStream();
                    if (user.MyVendor != null)
                        user.MyVendor.StopVending(stream);
                    user.Map.View.LeaveMap<IMapObj>((IMapObj)user.Player);
                    user.Player.View.Clear(stream);
                }
            }
            catch (Exception ex)
            {
                MyConsole.SaveException(ex);
            }
        }
        #endregion

        #region Stuff Bot
        public void StuffBot()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                BEntity.Equipment.AddBot(stream, 133309, Flags.ConquerItem.Armor, 12, 7, 255, Flags.Gem.SuperDragonGem, Flags.Gem.SuperDragonGem, 822071, 6);
                BEntity.Equipment.AddBot(stream, 500429, Flags.ConquerItem.RightWeapon, 12, 7, 255, Flags.Gem.SuperDragonGem, Flags.Gem.SuperDragonGem, 800618, 6);

                BEntity.Equipment.AddBot(stream, 1050002, Flags.ConquerItem.LeftWeapon);

                BEntity.Equipment.AddBot(stream, 150269, Flags.ConquerItem.Ring, 12, 7, 255, Flags.Gem.SuperDragonGem, Flags.Gem.SuperDragonGem, 823058, 6);
                BEntity.Equipment.AddBot(stream, 120269, Flags.ConquerItem.Necklace, 12, 7, 255, Flags.Gem.SuperDragonGem, Flags.Gem.SuperDragonGem, 821033, 6);
                BEntity.Equipment.AddBot(stream, 160249, Flags.ConquerItem.Boots, 12, 7, 255, Flags.Gem.SuperDragonGem, Flags.Gem.SuperDragonGem, 824018, 6);
                BEntity.Equipment.AddBot(stream, 113309, Flags.ConquerItem.Head, 12, 7, 255, Flags.Gem.SuperDragonGem, Flags.Gem.SuperDragonGem, 820073, 6);
                BEntity.Equipment.AddBot(stream, 196865, Flags.ConquerItem.Garment);
                BEntity.Equipment.AddBot(stream, 196865, Flags.ConquerItem.AleternanteGarment);
                BEntity.Equipment.AddBot(stream, 201009, Flags.ConquerItem.Fan, 12, 1, 0, Flags.Gem.SuperThunderGem, Flags.Gem.SuperThunderGem, 0, 0);
                BEntity.Equipment.AddBot(stream, 202009, Flags.ConquerItem.Tower, 12, 1, 0, Flags.Gem.SuperGloryGem, Flags.Gem.SuperGloryGem, 0, 0);
                BEntity.Equipment.AddSteed(stream, 300000, Flags.ConquerItem.Steed, 12, false, 46, 39, 142);
                BEntity.Equipment.AddBot(stream, 203009, Flags.ConquerItem.RidingCrop, 12, 1, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, 0, 0);
                BEntity.Equipment.AddBot(stream, 204009, Flags.ConquerItem.Wing, 12, 1, 0, Role.Flags.Gem.SuperThunderGem, Role.Flags.Gem.SuperGloryGem, 0, 0);
                BEntity.Equipment.QueryEquipment(false);
                BEntity.Player.View.SendView(BEntity.Player.GetArray(stream, false), false);
            }
        }
        #endregion

        #region HandleNpcBot
        public static void HandleNpc(GameClient client, ServerSockets.Packet stream, byte Option, string Input, uint id)
        {
            uint num = 10;
            Dialog dialog = new Dialog(client, stream);
            switch (Option)
            {
                case 0:
                    {
                        if (client.AI == null)
                        {
                            dialog.Text("Hey, I can help you make your hero better, Choose how I can help you.");
                            dialog.Option("BotActive.", 6);
                            dialog.AddOption("See~you~then!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                        }
                        if (client.AI != null)
                        {
                            TimeSpan timeSpan = DateTime.Now.AddSeconds((double)client.AI.EndTime) - DateTime.Now;
                            dialog.Text(string.Format("Your bot is active.\nConquerPoints : {0}\n Remove after : {1} Days : {2} Hours : {3} Minutes : {4} Seconds.", (object)client.AI.BEntity.Player.ConquerPoints, (object)timeSpan.Days, (object)timeSpan.Hours, (object)timeSpan.Minutes, (object)timeSpan.Seconds));
                            dialog.Option("Get Cps.", 4);
                            dialog.Option("Remove Bot.", 2);
                            dialog.Option("Get to Bot.", 3);
                            dialog.Option("AddHour.", 7);
                            dialog.AddOption("See~you~then!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();

                        }
                        break;
                    }
                case 1:
                    {
                        if (client.Player.Class >= 4000 && client.Player.Class <= 4005)
                        {
                            dialog.Text("Class [Archer] not qualified for  ");
                            dialog.AddOption("ok!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                            break;
                        }
                        if (client.Player.Level < (ushort)130 && client.Player.Reborn < (byte)2)
                        {
                            dialog.Text("You're not qualified for this, you need to get to level 130 and 2reborn first.");
                            dialog.AddOption("ok!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                            break;
                        }
                        if (client.Player.Level < (ushort)130 && client.Player.Reborn == (byte)2)
                        {
                            dialog.Text("You're not qualified for this, you need to get to level 130 first.");
                            dialog.AddOption("ok!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                            break;
                        }
                        if (client.Player.Reborn < (byte)2)
                        {
                            dialog.Text("You're not qualified for this, you need to get to 2reborn first.");
                            dialog.AddOption("ok!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                            break;
                        }
                        if (Pool.MonsterMap.ContainsKey(client.Player.Map))
                        {
                            List<string> monster = Pool.MonsterMap[client.Player.Map];
                            dialog.Text("Monster Target:");
                            for (int index = 0; index < monster.Count; ++index)
                                dialog.Option(monster[index], (byte)((ulong)index + (ulong)num));
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                            break;
                        }
                        dialog.Text("Can`t run bot on this map.");
                        dialog.AddOption("ok!", byte.MaxValue);
                        dialog.AddAvatar((ushort)6).FinalizeDialog();
                        break;
                    }
                case 2:
                    {
                        if (client.AI == null)
                            break;
                        client.AI.OutBot();
                        client.AI = null;
                        break;
                    }
                case 3:
                    {

                        if (client.AI == null)
                            break;
                        client.Teleport(client.AI.BEntity.Player.X, client.AI.BEntity.Player.Y, client.AI.BEntity.Player.Map);
                        break;
                    }
                case 4:
                    {
                        if (client.Player.Level < (ushort)120)
                        {
                            client.CreateBoxDialog("Please return when you reach level 110.");
                            break;
                        }
                        if (client.AI == null)
                        {
                            client.CreateBoxDialog("you don`t have any bot to claim conquerpoints.");
                            break;
                        }
                        uint conquerPoints = (uint)client.AI.BEntity.Player.ConquerPoints;
                        client.AI.BEntity.Player.ConquerPoints = 0;
                        client.Player.ConquerPoints -= (long)conquerPoints;
                        client.AI.SaveData(Mode.On);
                        client.CreateBoxDialog("You got " + conquerPoints.ToString() + " ConquerPoints.");
                        break;
                    }
                case 5:
                    {
                        uint cps1 = BotSystem.GetCPs(client.Player.UID);
                        if (cps1 <= 0 || !BotSystem.UpdateCps(client.Player.UID, 0))
                            break;
                        client.Player.ConquerPoints -= (long)cps1;
                        client.CreateBoxDialog("You got " + cps1.ToString() + " ConquerPoints.");
                        break;
                    }
                case 6:
                    {
                        BotSystem @BotSystem = null;
                        if (Pool.AIAutoHuntingPoll.TryGetValue(client.Player.UID, out @BotSystem))
                            client.AI = @BotSystem;
                        if (client.AI == null)
                        {
                            dialog.Text("You can take out your own bot to kill monsters instead of you and that helps you get more time to play and more conquerpoints.");
                            dialog.Option("Active.", 1);
                            uint cps2 = BotSystem.GetCPs(client.Player.UID);
                            if (cps2 > 0)
                                dialog.Option("Get [" + cps2.ToString() + "] Cps.", 5);
                        }
                        else if (client.AI != null)
                        {
                            TimeSpan timeSpan = DateTime.Now.AddSeconds((double)client.AI.EndTime) - DateTime.Now;
                            client.AI.SaveData(Mode.On);
                            dialog.Text(string.Format("Your bot is active.\nConquerPoints : {0}\n Remove after : {1} Days : {2} Hours : {3} Minutes : {4} Seconds.", (object)client.AI.BEntity.Player.ConquerPoints, (object)timeSpan.Days, (object)timeSpan.Hours, (object)timeSpan.Minutes, (object)timeSpan.Seconds));
                            dialog.Option("Get Cps.", 4);
                            dialog.Option("Remove Bot.", 2);
                            dialog.Option("Get to Bot.", 3);
                        }
                        dialog.AddOption("See~you~then!", byte.MaxValue);
                        dialog.AddAvatar((ushort)6).FinalizeDialog();
                        break;
                    }
                case 7:
                    {

                        dialog.Text("Hey, I can help you make your hero better, Choose how I can help you.");
                        dialog.Option("6Hours.", 8);
                        dialog.Option("12Hours.", 9);
                        dialog.AddOption("See~you~then!", byte.MaxValue);
                        dialog.AddAvatar((ushort)6).FinalizeDialog();
                        break;

                    }
                case 8:
                    {
                        if (client.Inventory.Contain(3100069, 1))
                        {
                            client.Inventory.Remove(3100069, 1, stream);
                            client.AddHourBot(6);

                        }
                        else
                        {
                            client.CreateBoxDialog("You Dont Have Item Add 6 Hours.");
                        }
                        break;
                    }
                case 9:
                    {
                        if (client.Inventory.Contain(3100070, 1))
                        {
                            client.Inventory.Remove(3100070, 1, stream);

                            client.AddHourBot(12);


                        }
                        else
                        {
                            client.CreateBoxDialog("You Dont Have Item Add 12 Hours.");
                        }
                        break;
                    }
                default:
                    {
                        if (client.Player.Level < (ushort)120)
                        {
                            dialog.Text("Please return when you reach level 120.");
                            dialog.AddOption("See~you~then!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                            break;
                        }
                        if (Pool.MonsterMap.ContainsKey(client.Player.Map))
                        {
                            List<string> monster = Pool.MonsterMap[client.Player.Map];
                            int index = (int)Option - (int)num;
                            if (index < monster.ToArray().Length)
                            {
                                string TargetName = monster[index];
                                client.AI = new BotSystem(client);
                                string errorstr = "";
                                if (client.AI.Add(out errorstr, TargetName))
                                    break;
                                client.AI = null;
                                dialog.Text(errorstr);
                                dialog.AddOption("ok!", byte.MaxValue);
                                dialog.AddAvatar((ushort)6).FinalizeDialog();
                                break;
                            }
                            dialog.Text("Please try again. [Error: 2]");
                            dialog.AddOption("ok!", byte.MaxValue);
                            dialog.AddAvatar((ushort)6).FinalizeDialog();
                            break;
                        }
                        dialog.Text("Please try again. [Error: 3]");
                        dialog.AddOption("ok!", byte.MaxValue);
                        dialog.AddAvatar((ushort)6).FinalizeDialog();
                        break;
                    }
            }
        }
        public static bool UpdateCps(uint uid, uint value)
        {
            try
            {
                if (!File.Exists(Program.ServerConfig.DbLocation + "\\Bots\\" + uid.ToString() + ".ini"))
                    return false;
                new IniFile("\\Bots\\" + uid.ToString() + ".ini").Write<uint>("Bot", "cps", 0);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static uint GetCPs(uint uid)
        {
            try
            {
                return File.Exists(Program.ServerConfig.DbLocation + "\\Bots\\" + uid.ToString() + ".ini") ? new IniFile("\\Bots\\" + uid.ToString() + ".ini").ReadUInt32("Bot", "cps", 0) : 0;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region Save&&Load
        public static void Save()
        {

            foreach (BotSystem @BotSystem in Pool.AIAutoHuntingPoll.Values)
                @BotSystem.SaveData();
        }
        public void SaveData(Mode mode = Mode.Off)
        {

            IniFile iniFile = new IniFile("\\Bots\\" + OwnerUID.ToString() + ".ini");
            iniFile.Write<uint>("Bot", "(mode)", (uint)mode);
            iniFile.Write<uint>("Bot", "cps", (uint)BEntity.Player.ConquerPoints);
            iniFile.Write<long>("Bot", "viptime", VipTime.Ticks);
            iniFile.Write<int>("Bot", "time", EndTime);

        }
        public void Load()
        {
            IniFile iniFile = new IniFile("\\Bots\\" + OwnerUID.ToString() + ".ini");
            BEntity.Player.ConquerPoints = iniFile.ReadInt32("Bot", "cps", 0);
            EndTime = iniFile.ReadInt32("Bot", "time", 0);
            if ((uint)BEntity.BotHuntingTime > 0)
                EndTime = BEntity.BotHuntingTime;
            VipTime = DateTime.FromBinary(iniFile.ReadInt64("Bot", "viptime", 0));
        }
        #endregion
    }
}