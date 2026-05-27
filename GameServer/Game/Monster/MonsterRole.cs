using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Game.MsgServer;
using VirusX.Game.MsgFloorItem;
using VirusX.MsgInterServer.Packets;
using VirusX.Game.MsgNpc;
using VirusX.WindowsAPI;
using VirusX.Role;

namespace VirusX.Game.MsgMonster
{
    public unsafe class MonsterRole : Role.IMapObj
    {


        public DateTime RemoveFloor = DateTime.Now;
        public int StampFloorSecounds = 0;
        public static List<uint> SpecialMonsters = new List<uint>()
        {
            3130,
            3134,
            4171,
            4212,
            4220,
            29300,
            29360,
            29363,
            29370,
            6819,
            60979,
            2751,
            5355,
            3970,
            7985,
            3971,
            7483,
            3976,
            3977,
            3978,
            3979,
            3981,
            3982,
            3983,
            3985,
            3986,
            2984
        };

        public Client.GameClient AttackerScarofEarthl;
        public Database.MagicType.Magic ScarofEarthl;

        public int ExtraDamage { get { return Family.extra_damage; } }
        public int BattlePower { get { return Family.extra_battlelev; } }
        public bool AllowDynamic { get; set; }
        public bool IsTrap() { return false; }
        public uint IndexInScreen { get; set; }

        public Client.GameClient OwnerFloor;
        public Database.MagicType.Magic DBSpell;
        public ushort SpellLevel = 0;
        public DateTime FloorStampTimer = new DateTime();
        public bool IsFloor = false;
        public Game.MsgFloorItem.MsgItemPacket FloorPacket;

        public string Name { get; set; }
        public bool BlackSpot = false;
        public DateTime Stamp_BlackSpot = new DateTime();


        public int SizeAdd { get { return Family.AttackRange; } }

        public byte PoisonLevel = 0;

        private DateTime DeadStamp = new DateTime();
        private DateTime FadeAway = new DateTime();
        public DateTime RespawnStamp = new DateTime();
        public DateTime MoveStamp = new DateTime();
        public object scoreObj = new object();
        public Dictionary<uint, MsgBossHarmRankingEntry> Hunters = new Dictionary<uint, MsgBossHarmRankingEntry>();
        public bool CanRespawn(Role.GameMap map)
        {
            DateTime Now = DateTime.Now;
            if (Now > RespawnStamp)
            {
                if (!map.MonsterOnTile(RespawnX, RespawnY))
                {
                    return true;
                }
            }
            return false;

        }
        public bool logged = false;
        public uint GetScorePosition(Client.GameClient killer)
        {
            lock (scoreObj)
            {
                if (this.Hunters == null) return 0;

                var a = Hunters.Values.OrderByDescending(p => p.HunterScore).ToArray();
                if (!Alive && !logged)
                {
                    string log = "[Bosses] Name: " + Name + " | Killer: " + killer.Player.Name + " | Hunters: ";
                    foreach (var hunter in a.Take(5))
                        log += hunter.HunterName + "     |      ";
                    Database.ServerDatabase.LoginQueue.Enqueue(log);
                    logged = true;
                }
                for (int x = 0; x < a.Length; x++)
                {
                    if (a[x].HunterUID == killer.Player.UID)
                        return (uint)(x + 1);
                }
                return 0;
            }
        }
        public void UpdateScore(ServerSockets.Packet stream, Client.GameClient killer, uint Damage)
        {
            try
            {
                lock (scoreObj)
                {
                    if (this.Hunters == null)
                        this.Hunters = new Dictionary<uint, MsgBossHarmRankingEntry>();
                    if (this.Hunters.ContainsKey(killer.Player.UID))
                        this.Hunters[killer.Player.UID].HunterScore += Damage;
                    else
                        this.Hunters.Add(killer.Player.UID, new MsgBossHarmRankingEntry() { HunterUID = killer.Player.UID, HunterScore = Damage, Rank = 0, HunterName = killer.Player.Name, ServerID = killer.Player.ServerID });

                    var array = this.Hunters.Values.OrderByDescending(p => p.HunterScore).ToArray();
                    for (int x = 0; x < array.Length; x++)
                    {
                        array[x].Rank = (uint)(x + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteException(ex);
                MyConsole.SaveException(ex);
            }
        }
        public void Respawn(bool SendEffect = true)
        {
            using (var rev = new ServerSockets.RecycledPacket())
            {
                var stream = rev.GetStream();

                ClearFlags(false);

                HitPoints = (uint)Family.MaxHealth;
                State = MobStatus.Idle;

                Game.MsgServer.ActionQuery action;

                action = new MsgServer.ActionQuery()
                {
                    ObjId = UID,
                    Type = MsgServer.ActionType.RemoveEntity
                };

                Send(stream.ActionCreate(action));

                Send(GetArray(stream, false));

                if (SendEffect)
                {
                    action.Type = ActionType.ReviveMonster;
                    Send(stream.ActionCreate(action));
                }

                if (Family.MaxHealth > ushort.MaxValue)
                {
                    Game.MsgServer.MsgUpdate Upd = new Game.MsgServer.MsgUpdate(stream, UID, 1);
                    stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.MaxHitpoints, Family.MaxHealth);
                    Send(Upd.GetArray(stream));
                    stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.Hitpoints, HitPoints);
                    Send(Upd.GetArray(stream));
                }

            }
        }
        public void SendSysMesage(string Messaj, Game.MsgServer.MsgMessage.ChatMode ChatType = Game.MsgServer.MsgMessage.ChatMode.TopLeft
          , Game.MsgServer.MsgMessage.MsgColor color = Game.MsgServer.MsgMessage.MsgColor.red)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(Messaj, color, ChatType).GetArray(stream));
            }
        }
        public void SendBossSysMesage(string KillerName, int StudyPoints, Game.MsgServer.MsgMessage.ChatMode ChatType = Game.MsgServer.MsgMessage.ChatMode.Center
          , Game.MsgServer.MsgMessage.MsgColor color = Game.MsgServer.MsgMessage.MsgColor.red)
        {
            SendSysMesage("The " + Name.ToString() + " has been destroyed by the team " + KillerName.ToString() + "`s ! All team members received " + StudyPoints.ToString() + " Study Points!", ChatType, color);

        }
        public void GetBossReward(Client.GameClient killer, ServerSockets.Packet stream)
        {
            if (Map == 10250 || Map == 10137 || Map == 3954)
            {
                if (Family.ID == 3970)
                {
                    if (Role.MyMath.Success(90))
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 14, X + 14);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 14, Y + 14);
                        DropItem(stream, killer.Player.UID, killer.Map, 4200010, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    else
                    {
                        var Items = Pool.ItemsBase.Values.Where((p => p.ID / 10000 == 430 || p.ID / 10000 == 431 || p.ID / 10000 == 432 || p.ID / 10000 == 433)).ToArray();
                        uint ItemID = (uint)Pool.GetRandom.Next(0, Items.Length);
                        ushort xx = (ushort)Program.GetRandom.Next(X - 5, X + 5);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 5, Y + 5);
                        DropItem(stream, killer.Player.UID, killer.Map, ItemID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    for (int x = 0; x < 5; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 7, X + 7);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 7, Y + 7);
                        DropItem(stream, killer.Player.UID, killer.Map, 3310699, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }

                    for (int x = 0; x < 7; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 6, X + 6);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 6, Y + 6);
                        DropItem(stream, killer.Player.UID, killer.Map, 3310698, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    for (int x = 0; x < 3; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 5, X + 5);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 5, Y + 5);
                        DropItem(stream, killer.Player.UID, killer.Map, 3324226, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    for (int x = 0; x < 7; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 8, X + 8);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 8, Y + 8);
                        DropItem(stream, killer.Player.UID, killer.Map, 720836, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    for (int x = 0; x < 5; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 6, X + 6);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 6, Y + 6);
                        DropItem(stream, killer.Player.UID, killer.Map, 3325989, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    killer.Player.ConquerPoints += 50000;
                    killer.Player.WorldPoints += 5;
                    foreach (var user in Pool.GamePoll.Values)
                    {
                        user.Send(new MsgMessage(killer.Player.Name, " killed  the " + Family.Name + " and  Received [TONS of CPs] ", 3, 0).GetArray(stream));
                    }
                }
                else
                {
                    for (int x = 0; x < 1; x++)
                    {
                        int[] itemIds = { 4200007, 4200008, 4200009 };

                        int randomIndex = Program.GetRandom.Next(0, itemIds.Length);
                        int selectedItemId = itemIds[randomIndex];

                        ushort xx = (ushort)Program.GetRandom.Next(X - 8, X + 8);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 8, Y + 8);

                        DropItem(stream, killer.Player.UID, killer.Map, (uint)selectedItemId, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                   
                    for (int x = 0; x < 1; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 8, X + 8);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 8, Y + 8);
                        DropItem(stream, killer.Player.UID, killer.Map, 3310699, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }

                    for (int x = 0; x < 2; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 7, X + 7);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 7, Y + 7);
                        DropItem(stream, killer.Player.UID, killer.Map, 3310698, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    for (int x = 0; x < 3; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 7, X + 7);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 7, Y + 7);
                        DropItem(stream, killer.Player.UID, killer.Map, 720836, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }
                    for (int x = 0; x < 5; x++)
                    {
                        ushort xx = (ushort)Program.GetRandom.Next(X - 6, X + 6);
                        ushort yy = (ushort)Program.GetRandom.Next(Y - 6, Y + 6);
                        DropItem(stream, killer.Player.UID, killer.Map, 3325756, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                    }

                    killer.Player.ConquerPoints += 50000;
                    killer.Player.WorldPoints += 1;
                    foreach (var user in Pool.GamePoll.Values)
                    {
                        user.Send(new MsgMessage(killer.Player.Name, " killed  the " + Family.Name + " and  Received [TONS of CPs] ", 3, 0).GetArray(stream));
                    }
                }
                foreach (var hunter in Hunters.Values)
                {
                    if (Pool.GamePoll.ContainsKey(hunter.HunterUID))
                    {
                        var client = Pool.GamePoll[hunter.HunterUID];
                        switch (this.GetScorePosition(client))
                        {
                            case 1:
                                {
                                    switch (Family.ID)
                                    {
                                        default:
                                            {
                                                
                                                new PrizeInfo(client, "Boss Reward", "Prize Rank", "You killer " + Family.Name + " This Prize For Rank 1", 30 * 24 * 60 * 60, 0, 10000, 0, 0, 0);
                                                break;
                                            }


                                    }

                                }
                                break;
                            case 2:
                                {
                                    switch (Family.ID)
                                    {
                                        default:
                                            {
                                                
                                                new PrizeInfo(client, "Boss Reward", "Prize Rank", "You killer " + Family.Name + " This Prize For Rank 2", 30 * 24 * 60 * 60, 0, 5000, 0, 2, 0);
                                                break;
                                            }


                                    }

                                }
                                break;
                            case 3:
                                {
                                    switch (Family.ID)
                                    {
                                        default:
                                            {
                                                
                                                new PrizeInfo(client, "Boss Reward", "Prize Rank", "You killer " + Family.Name + " This Prize For Rank 3", 30 * 24 * 60 * 60, 0, 2500, 0, 3, 0);
                                                break;
                                            }

                                    }

                                }
                                break;

                        }
                    }

                }
                
                foreach (var user in Pool.GamePoll.Values.Where(p => p.Player.Event && p.Player.EndTele))
                    user.Player.End2Tele = true;
                if (DateTime.Now.DayOfWeek != DayOfWeek.Friday)
                {
                    if (Database.RankMonster.HuntCoinsMap.ContainsKey(killer.Player.UID))
                        Database.RankMonster.HuntCoinsMap[killer.Player.UID].KillMonster += 1;
                    else Database.RankMonster.HuntCoinsMap.Add(killer.Player.UID, new Database.RankMonster.HuntCoins() { UID = killer.Player.UID, Name = killer.Player.Name, KillMonster = 1 });
                }
            }
        }
        public void Dead(ServerSockets.Packet stream, Client.GameClient killer, uint aUID, Role.GameMap GameMap)
        {
            if (Alive)
            {
                if (IsFloor)
                {

                    FloorPacket.DropType = MsgFloorItem.MsgDropID.RemoveEffect;
                    if (FloorPacket.m_ID == Game.MsgFloorItem.MsgItemPacket.Thundercloud)
                    {
                        ActionQuery _action;

                        _action = new ActionQuery()
                        {
                            ObjId = this.FloorPacket.m_UID,
                            Type = ActionType.RemoveEntity
                        };

                        this.View.SendScreen(stream.ActionCreate(_action), this.GMap);


                        GMap.View.LeaveMap<Role.IMapObj>(this);
                        HitPoints = 0;
                        GameMap.SetMonsterOnTile(X, Y, false);
                    }
                    else if (FloorPacket.m_ID == Game.MsgFloorItem.MsgItemPacket.AuroraLotus)
                    {
                        byte revivers = SpellLevel >= 6 ? (byte)2 : (byte)1;
                        foreach (var user in View.Roles(GameMap, Role.MapObjectType.Player))
                        {
                            if (revivers == 0)
                                break;
                            if (user.Alive == false)
                            {
                                if (Role.Core.GetDistance(user.X, user.Y, X, Y) < 5)
                                {
                                    revivers--;
                                    var player = user as Role.Player;
                                    if (player.ContainFlag(MsgUpdate.Flags.SoulShackle) == false)
                                        player.Revive(stream);

                                }
                            }
                            user.Send(GetArray(stream, false));
                        }
                        GMap.View.LeaveMap<Role.IMapObj>(this);
                    }
                    else if (FloorPacket.m_ID == Game.MsgFloorItem.MsgItemPacket.FlameLotus)
                    {
                        FloorPacket.DropType = MsgFloorItem.MsgDropID.RemoveEffect;
                        int counter = 1;
                        foreach (var user in View.Roles(GameMap, Role.MapObjectType.Player))
                        {
                            if (user.UID != OwnerFloor.Player.UID && Role.Core.GetDistance(user.X, user.Y, this.X, this.Y) < 5)
                            {
                                var player = user as Role.Player;

                                Game.MsgServer.MsgSpellAnimation.SpellObj AnimationObj;
                                Game.MsgServer.AttackHandler.Calculate.Magic.OnPlayer(this.OwnerFloor.Player, player, this.DBSpell, out AnimationObj);
                                Game.MsgServer.AttackHandler.ReceiveAttack.Player.Execute(AnimationObj, this.OwnerFloor, player);
                                AnimationObj.Damage = Game.MsgServer.AttackHandler.Calculate.Base.CalculateSoul(AnimationObj.Damage, 0);

                                InteractQuery Attack = new InteractQuery();
                                Attack.UID = this.UID;
                                Attack.OpponentUID = player.UID;
                                Attack.Damage = (int)AnimationObj.Damage;
                                Attack.Effect = (uint)AnimationObj.Effect;
                                Attack.X = player.X;
                                Attack.Y = player.Y;
                                Attack.AtkType = (ushort)MsgAttackPacket.AttackID.Physical;

                                stream.InteractionCreate(Attack);

                                player.View.SendView(stream, true);

                                MsgSpell fireCurse;
                                if (OwnerFloor.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.FireCurse, out fireCurse))
                                {
                                    Database.MagicType.Magic fireCurseDB = Pool.Magic[(ushort)Role.Flags.SpellID.FireCurse][OwnerFloor.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.FireCurse].Level];
                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                    {
                                        var streamm = recycledPacket.GetStream();
                                        MsgSpellAnimation FireCurseMsgSpell = new MsgSpellAnimation(this.UID
                         , player.UID, player.X, player.Y, fireCurse.ID
                         , fireCurse.Level, fireCurse.UseSpellSoul);
                                        FireCurseMsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj() { Damage = (uint)(fireCurseDB.DamageOnHuman * counter), Effect = MsgAttackPacket.AttackEffect.FireCurse, Hit = 1, UID = player.UID });
                                        Game.MsgServer.AttackHandler.ReceiveAttack.Player.Execute(FireCurseMsgSpell.Targets.FirstOrDefault(), this.OwnerFloor, player);
                                        FireCurseMsgSpell.SetStream(streamm); FireCurseMsgSpell.Send(player.Owner);
                                        counter++;
                                    }
                                }

                            }
                            user.Send(this.GetArray(stream, false));
                        }
                        foreach (var obj in View.Roles(GameMap, Role.MapObjectType.Monster))
                        {
                            if (obj.UID != this.UID && Role.Core.GetDistance(obj.X, obj.Y, this.X, this.Y) < 5)
                            {
                                var monster = obj as Game.MsgMonster.MonsterRole;

                                Game.MsgServer.MsgSpellAnimation.SpellObj AnimationObj;
                                Game.MsgServer.AttackHandler.Calculate.Magic.OnMonster(this.OwnerFloor.Player, monster, this.DBSpell, out AnimationObj);
                                Game.MsgServer.AttackHandler.ReceiveAttack.Monster.Execute(stream, AnimationObj, monster.OwnerFloor, monster);
                                AnimationObj.Damage = Game.MsgServer.AttackHandler.Calculate.Base.CalculateSoul(AnimationObj.Damage, 0);

                                InteractQuery Attack = new InteractQuery();
                                Attack.UID = this.UID;
                                Attack.OpponentUID = this.UID;
                                Attack.Damage = (int)AnimationObj.Damage;
                                Attack.Effect = (uint)AnimationObj.Effect;
                                Attack.X = this.X;
                                Attack.Y = this.Y;
                                Attack.AtkType = (ushort)MsgAttackPacket.AttackID.Physical;

                                stream.InteractionCreate(Attack);


                                monster.View.SendScreen(stream, this.OwnerFloor.Map);

                                MsgSpell fireCurse;
                                if (OwnerFloor.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.FireCurse, out fireCurse))
                                {
                                    Database.MagicType.Magic fireCurseDB = Pool.Magic[(ushort)Role.Flags.SpellID.FireCurse][OwnerFloor.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.FireCurse].Level];
                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                    {
                                        var streamm = recycledPacket.GetStream();
                                        MsgSpellAnimation FireCurseMsgSpell = new MsgSpellAnimation(this.UID
                         , monster.UID, monster.X, monster.Y, fireCurse.ID
                         , fireCurse.Level, fireCurse.UseSpellSoul);
                                        FireCurseMsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj() { Damage = (uint)(fireCurseDB.DamageOnHuman * counter), Effect = MsgAttackPacket.AttackEffect.FireCurse, Hit = 1, UID = monster.UID });
                                        Game.MsgServer.AttackHandler.ReceiveAttack.Monster.Execute(streamm, FireCurseMsgSpell.Targets.FirstOrDefault(), this.OwnerFloor, monster);
                                        FireCurseMsgSpell.SetStream(streamm); FireCurseMsgSpell.Send(monster);
                                        counter++;
                                    }
                                }
                            }

                        }
                        GMap.View.LeaveMap<Role.IMapObj>(this);
                    }
                    HitPoints = 0;
                    GameMap.SetMonsterOnTile(X, Y, false);
                    return;
                }


                RespawnStamp = DateTime.Now.AddSeconds(8 + Family.RespawnTime);

                if (BlackSpot)
                {
                    Send(stream.BlackspotCreate(false, UID));
                    BlackSpot = false;
                }
                ClearFlags(false);
                HitPoints = 0;
                AddFlag(MsgServer.MsgUpdate.Flags.Dead, Role.StatusFlagsBigVector32.PermanentFlag, true);
                DeadStamp = DateTime.Now;

                InteractQuery action = new InteractQuery()
                {
                    UID = aUID,
                    KilledMonster = 1,
                    X = this.X,
                    Y = this.Y,
                    AtkType = (ushort)MsgAttackPacket.AttackID.Death,
                    OpponentUID = UID
                };


                if (killer != null && killer.Player != null)
                {

                    #region XpSkill
                    if (killer.Player.OnXPSkill() == MsgUpdate.Flags.SuperCyclone || killer.Player.OnXPSkill() == MsgUpdate.Flags.Cyclone
                               || killer.Player.OnXPSkill() == MsgUpdate.Flags.Superman)
                    {
                        killer.Player.XPCount++;
                        killer.Player.KillCounter++;

                        if (killer.Player.OnXPSkill() != MsgServer.MsgUpdate.Flags.Normal)
                        {

                            action.SpellLevel = killer.Player.KillCounter;
                            killer.Player.UpdateXpSkill();
                        }

                    }
                    else if (killer.Player.OnXPSkill() == MsgUpdate.Flags.Normal)
                    {
                        killer.Player.XPCount++;
                    }
                    else if (killer.Player.OnXPSkill() != MsgUpdate.Flags.BladeFlurry)
                    {
                        if (killer.Player.OnXPSkill() != MsgUpdate.Flags.Omnipotence)
                        {
                            killer.Player.KillCounter++;
                            if (killer.Player.KillCounter % 4 == 0)
                                killer.Player.XPCount++;
                        }
                    }


                    #endregion

                    Send(stream.InteractionCreate(action));
                    if (RemoveOnDead)
                    {
                        AddFlag(MsgUpdate.Flags.FadeAway,1, false);
                        GMap.View.LeaveMap<Role.IMapObj>(this);
                        if (GMap.IsFlagPresent(X, Y, Role.MapFlagType.Monster))
                            GMap.cells[X, Y] &= ~Role.MapFlagType.Monster;



                    }
                    if (Map == 3935)
                    {
                        #region StoneBoundRealm
                        if (MyMath.Success(5))
                        {
                            if (!killer.Inventory.HaveSpace(4))
                            {
                                killer.SendSysMesage("You Dont Have Space");
                                return;
                            }
                            uint ItemId = 3337906;

                            killer.Inventory.AddItemWitchStack(ItemId, 0, 1, stream, true);
                            killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!", MsgServer.MsgMessage.ChatMode.CrossServerIcon, MsgServer.MsgMessage.MsgColor.red);
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                            return;

                        }
                        #endregion
                        #region StoneBoundRealm
                        if (MyMath.Success(0.1))
                        {
                            if (!killer.Inventory.HaveSpace(4))
                            {
                                killer.SendSysMesage("You Dont Have Space");
                                return;
                            }
                            uint[] ItemsIDS = new uint[] { 730001, 730002, 730003, 730004, 3009001, 3009002 };

                            uint ItemId = ItemsIDS[Program.GetRandom.Next(0, ItemsIDS.Length)];

                            killer.Inventory.AddItemWitchStack(ItemId, 0, 1, stream, true);
                            killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!", MsgServer.MsgMessage.ChatMode.CrossServerIcon, MsgServer.MsgMessage.MsgColor.red);
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                            return;

                        }
                        #endregion
                        #region BigBoSs
                        if (Family.ID == 7884 || Family.ID == 7971 || Family.ID == 7972 || Family.ID == 7973)
                        {
                            if (!killer.Inventory.HaveSpace(4))
                            {
                                killer.SendSysMesage("You Dont Have Space");
                                return;
                            }

                            uint ItemId = 3009003;
                            killer.Inventory.AddItemWitchStack(ItemId, 0, 1, stream, true);
                            killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!", MsgServer.MsgMessage.ChatMode.CrossServerIcon, MsgServer.MsgMessage.MsgColor.red);
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                            return;

                        }
                        #endregion
                        return;
                    }
                    if (Role.MyMath.Success(0.59))
                    {
                        if (killer.Player.Reborn >= 2)
                        {
                            killer.Beasts.FruitToday++;
                            killer.Inventory.AddItemWitchStack(3009100, 0, 1, stream);
                            killer.SendSysMesage("You recived " + Pool.ItemsBase[3009100].Name + " in Inventory.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        }
                    }
                    
                    #region DropMonster
                   

                    #region DailyQuest(Level110)
                    else if (killer.Player.Map == 3998)
                    {
                        if (Family.ID == 2758)
                        {
                            if (killer.Player.QuestGUI.CheckQuest(3634, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                killer.Player.QuestGUI.IncreaseQuestObjectives(stream, 3634, 1);
                                if (killer.Player.QuestGUI.CheckObjectives(3634, 10))
                                {
                                    killer.CreateBoxDialog("You've eliminated enough number of Anger Rats. Hurry and report back to Chong Yan Elder!");
                                }
                            }
                        }
                        else if (Family.ID == 2759)
                        {
                            if (killer.Player.QuestGUI.CheckQuest(3636, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                if (Role.Core.Rate(45))
                                {
                                    var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)MsgNpc.NpcID.BrokenForgeFurnace, killer.Player.Class, 3636);
                                    if (killer.Inventory.HaveSpace(1))
                                    {
                                        if (killer.Inventory.Contain(3008752, 8) == false)
                                        {
                                            killer.Inventory.AddItemWitchStack(3008752, 0, 1, stream);
                                        }
                                        if (killer.Inventory.Contain(3008752, 8))
                                            killer.Player.QuestGUI.SendAutoPatcher("You've collected enough number of Rune Fragments. Go and try to complete the runes on the Forge Furnace.", ActiveQuest.FinishNpcId.Map, ActiveQuest.FinishNpcId.X, ActiveQuest.FinishNpcId.Y, ActiveQuest.FinishNpcId.ID);
                                    }
                                    else
                                        killer.CreateBoxDialog("Please make 1 more space in your inventory.");
                                }
                            }
                        }
                        else if (Family.ID == 2760)
                        {

                            if (killer.Player.QuestGUI.CheckQuest(3638, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                if (killer.Inventory.HaveSpace(1))
                                {
                                    var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)MsgNpc.NpcID.BrokenForgeFurnace, killer.Player.Class, 3638);
                                    if (!killer.Inventory.Contain(3008750, 1))
                                        killer.Inventory.Add(stream, 3008750);
                                    killer.Player.QuestGUI.SendAutoPatcher("The Violet Bat King fell down and dropped an ancient-style hammer.Hurry and take this hammer to the Forge Furnace.", ActiveQuest.FinishNpcId.Map, ActiveQuest.FinishNpcId.X, ActiveQuest.FinishNpcId.Y, ActiveQuest.FinishNpcId.ID);
                                }
                                else
                                    killer.CreateBoxDialog("Please make 1 more space in your inventory.");
                            }
                        }
                        else if (Family.ID == 2761)
                        {
                            if (killer.Player.QuestGUI.CheckQuest(3641, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                killer.Player.QuestGUI.IncreaseQuestObjectives(stream, 3641, 1);

                                if (killer.Player.QuestGUI.CheckObjectives(3641, 15))
                                    killer.Player.QuestGUI.SendAutoPatcher("You've defeat enough number of Lava Scorpions. Now, you can appease the sacrificed Bright people.", 3998, 220, 294, 0);

                            }

                        }
                        else if (Family.ID == 2762)
                        {
                            if (killer.Player.QuestGUI.CheckQuest(3644, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                if (killer.Inventory.HaveSpace(1))
                                {
                                    var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)MsgNpc.NpcID.FlameAltar, killer.Player.Class, 3644);
                                    if (!killer.Inventory.Contain(3008742, 50))
                                        killer.Inventory.AddItemWitchStack(3008742, 0, 1, stream);
                                    else
                                        killer.Player.QuestGUI.SendAutoPatcher("You`ve collect enough number of Building Stones. Go and try to restore the ruined altar.", ActiveQuest.FinishNpcId.Map, ActiveQuest.FinishNpcId.X, ActiveQuest.FinishNpcId.Y, ActiveQuest.FinishNpcId.ID);
                                }
                                else
                                    killer.CreateBoxDialog("Please make 1 more space in your inventory.");
                            }
                        }
                        else if (Family.ID == 2765)
                        {
                            if (killer.Player.QuestGUI.CheckQuest(3648, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                if (killer.Inventory.HaveSpace(1))
                                {
                                    var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)MsgNpc.NpcID.FlameAltar, killer.Player.Class, 3648);
                                    if (!killer.Inventory.Contain(3008748, 100))
                                        killer.Inventory.AddItemWitchStack(3008748, 0, 1, stream);
                                    else
                                        killer.Player.QuestGUI.SendAutoPatcher("You`ve collected enough number of Star Ores. Go and try to extract the Essence of Star at the altar.", ActiveQuest.FinishNpcId.Map, ActiveQuest.FinishNpcId.X, ActiveQuest.FinishNpcId.Y, ActiveQuest.FinishNpcId.ID);
                                }
                                else
                                    killer.CreateBoxDialog("Please make 1 more space in your inventory.");
                            }

                        }
                        else if (Family.ID == 2763)
                        {
                            if (killer.Player.QuestGUI.CheckQuest(3645, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                if (killer.Inventory.HaveSpace(1))
                                {
                                    if (Role.Core.Rate(40))
                                    {//3008744
                                        var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)MsgNpc.NpcID.FlameAltar, killer.Player.Class, 3645);
                                        if (!killer.Inventory.Contain(3008743, 1))
                                            killer.Inventory.AddItemWitchStack(3008743, 0, 1, stream);
                                        else
                                            killer.Player.QuestGUI.SendAutoPatcher("You`ve retrieved the Wheel of Nature from the Clawed Rock Devil. Hurry and take the next action at the altar.", ActiveQuest.FinishNpcId.Map, ActiveQuest.FinishNpcId.X, ActiveQuest.FinishNpcId.Y, ActiveQuest.FinishNpcId.ID);
                                    }
                                }
                                else
                                    killer.CreateBoxDialog("Please make 1 more space in your inventory.");
                            }
                        }
                        else if (Family.ID == 2764)
                        {
                            if (killer.Player.QuestGUI.CheckQuest(3646, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                            {
                                if (killer.Inventory.HaveSpace(1))
                                {
                                    if (Role.Core.Rate(40))
                                    {
                                        var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)MsgNpc.NpcID.FlameAltar, killer.Player.Class, 3646);
                                        if (!killer.Inventory.Contain(3008745, 1))
                                        {

                                            if (!killer.Inventory.Contain(3008754, 5))
                                                killer.Inventory.AddItemWitchStack(3008754, 0, 1, stream);
                                            if (killer.Inventory.Contain(3008754, 5))
                                            {
                                                killer.Inventory.Remove(3008754, 5, stream);
                                                killer.Inventory.Add(stream, 3008745, 1);
                                                killer.Player.QuestGUI.SendAutoPatcher("You received the Earth Force! Hurry and transform it into Metal Force through the Wheel of Nature!", ActiveQuest.FinishNpcId.Map, ActiveQuest.FinishNpcId.X, ActiveQuest.FinishNpcId.Y, ActiveQuest.FinishNpcId.ID);
                                            }
                                        }
                                        else
                                            killer.Player.QuestGUI.SendAutoPatcher("You received the Earth Force! Hurry and transform it into Metal Force through the Wheel of Nature!", ActiveQuest.FinishNpcId.Map, ActiveQuest.FinishNpcId.X, ActiveQuest.FinishNpcId.Y, ActiveQuest.FinishNpcId.ID);
                                    }
                                }
                                else
                                    killer.CreateBoxDialog("You received the Earth Force! Hurry and transform it into Metal Force through the Wheel of Nature!");
                            }


                        }


                        //3008754
                    }

                    #endregion
                    #region SprirtDailyQuest
                    if (killer.Player.QuestGUI.CheckQuest(2375, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                    {
                        uint spirites = 0;
                        if (Level < 70)
                            spirites = 1;
                        else if (Level >= 70 && Level <= 99)
                            spirites = 2;
                        else if (Level >= 100 && Level <= 119)
                            spirites = 3;
                        else if (Level >= 120 && Level < 140)
                            spirites = 4;
                        else if (Boss == 1 && Family.MaxHealth >= 1000000)
                            spirites = 1000;

                        killer.Player.DailySpiritBeadCount += spirites;
#if Arabic
                          killer.SendSysMesage("You received " + spirites + " spirites.", MsgMessage.ChatMode.System);
#else
                        killer.SendSysMesage("You received " + spirites + " spirites.", MsgMessage.ChatMode.System);
#endif

                        if (Game.MsgNpc.NpcHandler.GetDailySpiritBeadKills(killer) <= killer.Player.DailySpiritBeadCount)
                        {
                            if (!killer.Player.QuestGUI.CheckObjectives(2375, 1))
                            {
                                killer.Player.QuestGUI.IncreaseQuestObjectives(stream, 2375, 1, 1);
#if Arabic
                                 killer.CreateBoxDialog("You`ve~collected~enough~spirits,~and~you~can~use~the~bead~to~claim~a~reward,~now!");
#else
                                killer.CreateBoxDialog("You`ve~collected~enough~spirits,~and~you~can~use~the~bead~to~claim~a~reward,~now!");
#endif

                            }
                        }
                    }
                    #endregion
                    #region DisCity
                    if (Map == 2022)
                    {
                        killer.Player.KillersDisCity += 1;
                    }

                    if (Map == 2024)
                    {
                        if (Family.ID == 66432)//ultimate pluto
                        {

                            MsgServer.MsgGameItem DataItem = new MsgServer.MsgGameItem();
                            DataItem.ITEM_ID = 790001;
                            Database.ItemType.DBItem DBItem;
                            if (Pool.ItemsBase.TryGetValue(0, out DBItem))
                            {
                                DataItem.Durability = DataItem.MaximDurability = DBItem.Durability;
                            }
                            DataItem.Color = Role.Flags.Color.Red;
                            ushort xx = X;
                            ushort yy = Y;
                            if (killer.Map.AddGroundItem(ref xx, ref yy, 3))
                            {
                                MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(DataItem, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, DynamicID, Map, killer.Player.UID, true, GMap);

                                if (killer.Map.EnqueueItem(DropItem))
                                {
                                    DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Visible);
                                }
                            }
                            MsgTournaments.MsgSchedules.DisCity.KillTheUltimatePluto(killer);

                        }
                    }
                    #endregion
                    #endregion
                    #region Poker Tables
                    foreach (var t in Poker.PokerLoad.Tables.Values)
                        PokerHandler.PokerTablesCallback(t, 0);
                    #endregion
                    #region BattleField
                    if (Map == 1081)
                    {
                        if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Type == MsgTournaments.TournamentType.BattleField
                            && Game.MsgTournaments.MsgSchedules.CurrentTournament.Process == MsgTournaments.ProcesType.Alive)
                            killer.Player.BattleFieldPoints += 10;

                        killer.SendSysMesage("You received 10 BattlePoints.");


                    }
                    if (Map == 1080)
                    {
                        if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Type == MsgTournaments.TournamentType.BattleField
                            && Game.MsgTournaments.MsgSchedules.CurrentTournament.Process == MsgTournaments.ProcesType.Alive)
                            killer.Player.BattleFieldPoints += 20;

                        killer.SendSysMesage("You received 20 BattlePoints.");


                    }
                    if (Map == 2060)
                    {
                        if (Game.MsgTournaments.MsgSchedules.CurrentTournament.Type == MsgTournaments.TournamentType.BattleField
                             && Game.MsgTournaments.MsgSchedules.CurrentTournament.Process == MsgTournaments.ProcesType.Alive)
                            killer.Player.BattleFieldPoints += 30;

                        killer.SendSysMesage("You received 30 BattlePoints.");


                    }
                    #endregion
                    if (Map == 10428 && Family.ID == 2984)//Ancient Dragon
                    {
                        if (killer.Inventory.Contain(killer.Player.DragonFurnace, 2))
                        {
                            if (Role.Core.Rate(BaseFunc.AnimaUpgradeRate(killer.Player.DragonFurnace)))
                            {
                                killer.Inventory.Remove(killer.Player.DragonFurnace, 2, stream);
                            again:
                                var Chances = Database.SpiritTable.SpiritRates.Values.Where(z => z.Type == killer.Player.DragonFurnace).ToArray();
                            Chances = Chances.Where(z => z.AnimaID != 0).ToArray();
                                Chances = Chances.OrderBy(q => q.Prizet1).ToArray();//This is new

                                if (Chances.FirstOrDefault().AnimaID == 2) goto again;
                                killer.Inventory.Add(stream, Chances.FirstOrDefault().Prizet1, 1);
                                killer.CreateBoxDialog("+------- Fusing Succeeded -------+\n" +
                                                       "              Congratulations! \n" +
                                                    "        You received a " + Pool.ItemsBase[Chances.FirstOrDefault().Prizet1].Name + "!\n" +
                                     "+----------------------------------+");
                            }
                            else
                            {
                                var Chances = Database.SpiritTable.SpiritRates.Values.Where(p => p.Type == 1 && p.AnimaID == killer.Player.DragonFurnace && p.rank == 0).ToArray();
                                    
                                if (MyMath.Success(50))
                                {
                                    killer.Inventory.Remove(killer.Player.DragonFurnace, 1, stream);
                                    killer.CreateBoxDialog("+------- Fusing Failed -------+\n" +
                                                           "What a pity, you lost 1 " + Pool.ItemsBase[killer.Player.DragonFurnace].Name + ".\n" +
                                         "+----------------------------------+");
                                }
                                else
                                {
                                    killer.Inventory.Remove(killer.Player.DragonFurnace, 2, stream);
                                    killer.IncreaseExperience(stream, Chances.FirstOrDefault().Prizet1);
                                    killer.CreateBoxDialog("+------- Fusing Failed -------+\n" +
                                                           "What a pity, you lost 2 " + Pool.ItemsBase[killer.Player.DragonFurnace].Name + ".\n" +
                                         "+----------------------------------+");
                                }
                            }
                        }
                        killer.Teleport(410, 354, 1002);
                    }
                    #region DragonIsland

                    if (Map == 10137)
                    {
                        #region NemesisTyrant (DragonISland
                        if (Family.ID == 4220)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();


                            return;
                        }
                        #endregion

                        #region Snow Banshee
                        if (Family.ID == 4171)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();

                            return;
                        }
                        #endregion

                        #region ThrillingSpook
                        if (Family.ID == 4212)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();
                            

                            return;
                        }
                        #endregion
                        #region DropMap

                        if (MyMath.Success(1))
                        {
                            uint[] ItemsIDS = new uint[] { 3009001, 3009002, 3313368, 3310998, 3009002 };

                            uint ItemId = ItemsIDS[Program.GetRandom.Next(0, ItemsIDS.Length)];

                            DropItemID(killer, ItemId, stream);
                            killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!");
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                            return;

                        }
                        #endregion

                    }

                    #endregion
                    #region DeityLand
                    if (killer.Player.Map == 10250)
                    { 
                        if (MyMath.Success(1))
                        {
                            if (!killer.Inventory.HaveSpace(1))
                            {
                                killer.SendSysMesage("You Dont Have Space");
                                return;
                            }
                            uint ItemId = 3307083;

                            killer.Inventory.AddItemWitchStack(ItemId, 0, 1, stream, true);
                            killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!", MsgServer.MsgMessage.ChatMode.CrossServerIcon, MsgServer.MsgMessage.MsgColor.red);
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                            return;

                        }
                        #region QueenofEvil
                        if (Family.ID == 3970)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();
                        }
                        #endregion

                        #region NetherTyrant
                        if (Family.ID == 3978)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();
                        }
                        #endregion

                        #region BloodyBanshee
                        if (Family.ID == 3976)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();
                        }
                        #endregion

                        #region ChillingSpook
                        if (Family.ID == 3977)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();

                        }
                        #endregion

                        #region DragonWraith
                        if (Family.ID == 3971)
                        {
                            GetBossReward(killer, stream);
                            Hunters.Clear();

                        }
                        #endregion

                        #region Quest
                        if (killer.Player.QuestGUI.CheckQuest(3944, MsgQuestList.QuestListItem.QuestStatus.Available) || !killer.Player.QuestGUI.src.ContainsKey(3944))
                            killer.Player.QuestGUI.Accept(Database.QuestInfo.GetFinishQuest(21972, killer.Player.Class, 3944), 0);
                        if (killer.Player.QuestGUI.CheckQuest(3944, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                        {
                            killer.Player.QuestGUI.IncreaseQuestObjectives(stream, 3944, 1);
                            uint[] Intentions;
                            killer.Player.QuestGUI.GetQuestObjectives(3944, out Intentions);
                            if (Intentions.Length > 0 && Intentions[0] >= 100)
                            {
                                killer.SendSysMesage("You have killed 100 devils in the DeityLand! Go claim your rewards from the Victoy Chest!");
                            }
                        }
                        #endregion

                        #region MeteorDove
                        if (Name == "MeteorDove")
                        {
                            for (ushort x = 0; x < 5; x++)
                                DropItemID(killer, 1088001, stream);
                        }
                        #endregion

                        #region Sprite
                        if (Name.Contains("Sprite"))
                        {
                            uint item = 0;
                            if (Name == "MeteorSprite") item = 1088001;
                            else if (Name == "DBSprite") item = 3303975;
                            else if (Name == "ChiSprite") item = 3008305;
                            else if (Name == "DragonSoulSprite") item = 3005133;
                            else if (Name == "SilverSprite")
                            {
                                DropItem(stream, killer.Player.UID, killer.Map, Database.ItemType.MoneyItemID((uint)50000), X, Y, MsgFloorItem.MsgItem.ItemType.Money, 50000, false, 0);
                            }
                            if (item > 0)
                                DropItemID(killer, item, stream);
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                        }
                        #endregion

                        #region DropMap

                        if (MyMath.Success(1))
                        {
                            uint[] ItemsIDS = new uint[] { 3009001, 3009002, 3313368, 3310998, 3009002 };

                            uint ItemId = ItemsIDS[Program.GetRandom.Next(0, ItemsIDS.Length)];

                            DropItemID(killer, ItemId, stream);
                            killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!");
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                            return;

                        }
                        #endregion


                    }

                    #region DropMap
                    if (Role.MyMath.Success(0.8))
                    {
                        uint[] ItemsIDS = new uint[] { Database.ItemType.Meteor };

                        uint ItemId = ItemsIDS[Program.GetRandom.Next(0, ItemsIDS.Length)];

                        DropItemID(killer, ItemId, stream);

                        killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!");
                        killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                        return;

                    }
                    if (Role.MyMath.Success(0.1))
                    {
                        uint[] ItemsIDS = new uint[] { Database.ItemType.DragonBall};

                        uint ItemId = ItemsIDS[Program.GetRandom.Next(0, ItemsIDS.Length)];

                        DropItemID(killer, ItemId, stream);

                        killer.SendSysMesage(Name + " that you killed dropped 1 " + (Pool.ItemsBase[ItemId].Name) + "(s)!");
                        killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "angelwing");
                        return;

                    }
                    
                    #endregion
                    #endregion
                
                    #region Quest Epic&&Archives Archer
                    #region stage1
                    if (Map == 11223)
                    {
                        #region stage
                        #region Portal1
                        if (Family.ID == 4226)
                        {
                            ActionQuery naction = new ActionQuery()
                            {
                                ObjId = killer.Player.UID,
                                dwParam3 = 182,//158 //186
                                Type = 456, //137
                                PositionX = killer.Player.X,
                                PositionY = killer.Player.Y,

                            };
                            killer.Send(stream.ActionCreate(naction));
                            killer.Teleport(171, 165, 11223, killer.Player.UID);
                            var Item = new Game.MsgFloorItem.MsgItem(null, 161, 191, Game.MsgFloorItem.MsgItem.ItemType.Effect, 0, killer.Player.UID, 11223, 0, false, Pool.ServerMaps[11223], 60 * 60 * 1000);
                            Item.MsgFloor.m_ID = 3616;
                            Item.MsgFloor.m_Color = 2;
                            Item.MsgFloor.DropType = Game.MsgFloorItem.MsgDropID.Effect;

                            Item.GMap.View.EnterMap<Role.IMapObj>(Item);
                            killer.Player.Portal = 2;
                            killer.Player.QuestGUI.SendAutoPatcher("STR_ID_tArchersTask[Msg][Enter][5][6]@@", 11223, 160, 189, 0);


                        }
                        #endregion
                        #region Portal2
                        if (Family.ID == 4227)
                        {
                            ActionQuery naction = new ActionQuery()
                            {
                                ObjId = killer.Player.UID,
                                dwParam3 = 183,
                                Type = 456,
                                PositionX = killer.Player.X,
                                PositionY = killer.Player.Y,

                            };
                            killer.Send(stream.ActionCreate(naction));
                            killer.Teleport(171, 165, 11223, killer.Player.UID);
                            var Item = new Game.MsgFloorItem.MsgItem(null, 190, 153, Game.MsgFloorItem.MsgItem.ItemType.Effect, 0, killer.Player.UID, 11223, 0, false, Pool.ServerMaps[11223], 60 * 60 * 1000);
                            Item.MsgFloor.m_ID = 3616;
                            Item.MsgFloor.m_Color = 2;
                            Item.MsgFloor.DropType = Game.MsgFloorItem.MsgDropID.Effect;

                            Item.GMap.View.EnterMap<Role.IMapObj>(Item);

                            killer.Player.QuestGUI.SendAutoPatcher("STR_ID_tArchersTask[Msg][Enter][5][8]@@", 11223, 190, 153, 0);

                            killer.Player.Portal = 3;



                        }
                        #endregion
                        #region Portal3
                        if (Family.ID == 4228)
                        {
                            ActionQuery naction = new ActionQuery()
                            {
                                ObjId = killer.Player.UID,
                                dwParam3 = 184,
                                Type = 456,
                                PositionX = killer.Player.X,
                                PositionY = killer.Player.Y,

                            };
                            killer.Send(stream.ActionCreate(naction));
                            killer.Teleport(171, 165, 11223, killer.Player.UID);
                            var Item = new Game.MsgFloorItem.MsgItem(null, 146, 132, Game.MsgFloorItem.MsgItem.ItemType.Effect, 0, killer.Player.UID, 11223, 0, false, Pool.ServerMaps[11223], 60 * 60 * 1000);
                            Item.MsgFloor.m_ID = 3616;
                            Item.MsgFloor.m_Color = 2;
                            Item.MsgFloor.DropType = Game.MsgFloorItem.MsgDropID.Effect;

                            Item.GMap.View.EnterMap<Role.IMapObj>(Item);

                            killer.Player.QuestGUI.SendAutoPatcher("STR_ID_tArchersTask[Msg][Enter][5][10]@@", 11223, 146, 131, 0);

                            killer.Player.Portal = 4;



                        }
                        #endregion
                        #region Portal4
                        if (Family.ID == 4229)
                        {
                            ActionQuery naction = new ActionQuery()
                            {
                                ObjId = killer.Player.UID,
                                dwParam3 = 162,
                                Type = 456,
                                PositionX = killer.Player.X,
                                PositionY = killer.Player.Y,

                            };
                            killer.Send(stream.ActionCreate(naction));
                            killer.Teleport(170, 164, 11223);

                            killer.Player.QuestGUI.SendAutoPatcher("STR_ID_tArchersTask[Msg][Find][8]@@", 11223, 166, 161, (uint)NpcID.LuBan);
                            killer.Player.Portal = 0;
                            killer.Player.FinishStage1 = 1;



                        }
                        #endregion
                        #endregion

                        #region stage1
                        #region LuBan
                        if (Family.ID == 4230)
                        {
                            killer.MyArchives.AddItem(Role.Instance.Archives.TypeID.StoneCracker, 1, 0, 1, 0);
                            killer.Teleport(170, 164, 11223);
                            killer.Player.HaveArchives = 1;
                        }
                        #endregion
                        #endregion

                        #region stage2
                        #region MasterYan
                        if (Family.ID == 4233)
                        {
                            killer.MyArchives.AddItem(Role.Instance.Archives.TypeID.ColdMoon, 1, 0, 1, 0);
                            killer.Teleport(170, 164, 11223);
                            killer.Player.HaveArchives = 2;
                            killer.Player.QuestGUI.SendAutoPatcher("STR_ID_tArchersTask[Msg][KuiLeiBOSS]@@", 11223, 169, 174, (uint)NpcID.MasterYan);
                        }
                        #endregion
                        #endregion

                        #region MoDi
                        if (Family.ID == 4234)
                        {
                            killer.MyArchives.AddItem(Role.Instance.Archives.TypeID.ThornCutter, 1, 0, 1, 0);
                            killer.Teleport(170, 164, 11223);
                            killer.Player.HaveArchives = 3;

                        }
                        #endregion

                    }
                    #endregion

                    #endregion

                    #region Quest Archives Warrior

                    if (Map == 11040)
                    {

                        #region HuaMulan
                        if (Family.ID == 7130)
                        {
                            killer.MyArchives.AddItem(Role.Instance.Archives.TypeID.Dragonhowl, 1, 0, 1, 0);
                            killer.Teleport(104, 68, 11041);
                            killer.Player.HuaMulan = 1;
                            killer.Player.Archives = 1;

                        }
                        #endregion
                        #region Tyre
                        if (Family.ID == 7132)
                        {
                            killer.Player.MonsterTyre += 1;
                            var map = Pool.ServerMaps[11040];
                            if (!map.ContainMobID(7132, killer.Player.UID + 1))
                            {
                                Server.AddMapMonster(stream, map, 7132, 88, 72, 18, 18, 1, killer.Player.UID + 1, true);
                            }
                            if (killer.Player.MonsterTyre == 2)
                            {
                                killer.Teleport(104, 68, 11041);
                                killer.Player.Tyre = 2;
                                killer.MyArchives.AddItem(Role.Instance.Archives.TypeID.Bloodlust, 1, 0, 1, 0);
                                killer.Player.Archives = 2;

                            }

                        }
                        #endregion
                        #region Ares
                        if (Family.ID == 7131)
                        {
                            killer.Teleport(104, 68, 11041);
                            killer.MyArchives.AddItem(Role.Instance.Archives.TypeID.Redcurse, 1, 0, 1, 0);
                            killer.Player.Ares = 2;

                        }
                        #endregion

                    }


                    #endregion
                    
                    #region Quest TrojanArchives

                    if (Map == 10446)
                    {
                        killer.HundredWeapons.Unlocked = true;
                        killer.Send(stream.CreateHundredWeaponsInfo(killer, MsgHundredWeaponsInfo.ActionID.RequestInfo));
                        killer.Send(stream.ActionCreate(new ActionQuery()
                        {
                            Type = ActionType.OpenDialog,
                            ObjId = killer.Player.UID,
                            Timestamp = (uint)Time32.timeGetTime().GetHashCode(),
                            dwParam = DialogCommands.TrojanWeaponArchive,
                            PositionX = killer.Player.X,
                            PositionY = killer.Player.Y
                        }));
                        ActionQuery actions = new ActionQuery()
                        {
                            Type = 456,
                            ObjId = killer.Player.UID,
                            dwParam3 = 7,
                            PositionX = killer.Player.X,
                            PositionY = killer.Player.Y,

                        };
                        killer.Send(stream.ActionCreate(actions));
                        killer.HundredWeapons.LoadPowerFoucs(killer);
                    }


                    #endregion

                    #region LavaBeast
                    if (Boss > 0 && Family.ID == 20055)
                    {
                        const ushort GetStudyPoints = 20;



                        if (killer.Team != null)
                        {
                            foreach (var member in killer.Team.Temates)
                            {
                                if (member != null && member.client != null && member.client.Player != null && member.client.Player.SubClass != null)
                                {
                                    if (member.client.Player.UID != killer.Player.UID)
                                    {
                                        member.client.Player.SubClass.AddStudyPoints(member.client, GetStudyPoints, stream);
                                    }
                                }
                            }
                        }
                        if (killer.Player.SubClass != null)
                            killer.Player.SubClass.AddStudyPoints(killer, GetStudyPoints, stream);
                        SendBossSysMesage(killer.Player.Name, GetStudyPoints);

                        return;
                        #endregion

                        #region SwordMaster
                    }
                    if (Boss > 0 && Family.ID == 6643)
                    {
                        List<uint> DropIems = Family.ItemGenerator.GenerateSoulsItems(3);
                        foreach (var ids in DropIems)
                        {
                            MsgServer.MsgGameItem DataItem = new MsgServer.MsgGameItem();
                            DataItem.ITEM_ID = ids;
                            Database.ItemType.DBItem DBItem;
                            if (Pool.ItemsBase.TryGetValue(ids, out DBItem))
                            {
                                DataItem.Durability = DataItem.MaximDurability = DBItem.Durability;
                            }
                            DataItem.Color = Role.Flags.Color.Red;
                            ushort xx = X;
                            ushort yy = Y;
                            if (killer.Map.AddGroundItem(ref xx, ref yy, 3))
                            {
                                MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(DataItem, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, DynamicID, Map, killer.Player.UID, true, GMap);

                                if (killer.Map.EnqueueItem(DropItem))
                                {
                                    DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Visible);
                                }
                            }
                        }
                        return;
                    }
                    #endregion

                    #region terato dragon
                    if (Boss > 0 && Family.ID == 20060)//terato dragon
                    {

                        GetBossReward(killer, stream);

                        for (int x = 0; x < 5; x++)
                            DropItemID(killer, Database.ItemType.DragonBall, stream, 5);
                        for (int x = 0; x < 1; x++)
                            DropItemID(killer, 3004181, stream, 1);
                        for (int x = 0; x < 1; x++)
                            DropItemID(killer, 722057, stream, 1);
                        const ushort GetStudyPoints = 100;
                        List<uint> DropIems = Family.ItemGenerator.GenerateSoulsItems(6);
                        int xsoul = 0;
                        foreach (var ids in DropIems)
                        {
                            if (xsoul >= 1)
                                break;
                            MsgServer.MsgGameItem DataItem = new MsgServer.MsgGameItem();
                            DataItem.ITEM_ID = ids;
                            Database.ItemType.DBItem DBItem;
                            if (Pool.ItemsBase.TryGetValue(ids, out DBItem))
                            {
                                DataItem.Durability = DataItem.MaximDurability = DBItem.Durability;
                            }
                            DataItem.Color = Role.Flags.Color.Red;
                            ushort xx = X;
                            ushort yy = Y;
                            if (killer.Map.AddGroundItem(ref xx, ref yy, 3))
                            {
                                MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(DataItem, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, DynamicID, Map, killer.Player.UID, true, GMap);

                                if (killer.Map.EnqueueItem(DropItem))
                                {
                                    killer.HeroRewards.AddGoal(709);
                                    DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Visible);
                                }
                            }
                            xsoul += 1;
                        }
                        if (killer.Team != null)
                        {
                            foreach (var member in killer.Team.Temates)
                            {
                                if (member != null && member.client != null && member.client.Player != null && member.client.Player.SubClass != null)
                                {
                                    if (member.client.Player.UID != killer.Player.UID)
                                    {
                                        member.client.Player.SubClass.AddStudyPoints(member.client, GetStudyPoints, stream);
                                    }
                                }
                            }
                        }
                        if (killer.Player.SubClass != null)
                            killer.Player.SubClass.AddStudyPoints(killer, GetStudyPoints, stream);

                        SendBossSysMesage(killer.Player.Name, GetStudyPoints);
                        return;


                    }
                    #endregion

                    #region QuestVip
                    if (Map != 11568 && Map == 1700)
                    {
                        ushort rand = (ushort)(killer.Player.MyRandom.Next() % 1000);
                        byte count = 1;
                        if (rand > 700)
                        {
                            ushort xx = X;
                            ushort yy = Y;
                            for (byte i = 0; i < count; i++)
                            {

                                Database.ItemType.DBItem DbItem = null;
                                byte ID_Quality;
                                bool ID_Special;
                                uint ID = Family.ItemGenerator.GenerateItem2(Map, out ID_Quality, out ID_Special, out DbItem);
                                if (ID != 0)
                                {
                                    bool drop = true;

                                    #region Moss
                                    if (ID == 722723 && killer != null)
                                        if (killer.Player.VipLevel >= 6)
                                        {
                                            if (killer.Inventory.HaveSpace(1))
                                            {
                                                killer.Inventory.AddItemWitchStack(722723, 0, 1, stream);

                                                drop = false;

                                            }



                                        }
                                    #endregion
                                    #region SoulAroma
                                    if (ID == 722724 && killer != null)
                                        if (killer.Player.VipLevel >= 6)
                                        {
                                            if (killer.Inventory.HaveSpace(1))
                                            {
                                                killer.Inventory.AddItemWitchStack(722724, 0, 1, stream);

                                                drop = false;

                                            }



                                        }
                                    #endregion
                                    #region DreamGras
                                    if (ID == 722725 && killer != null)
                                        if (killer.Player.VipLevel >= 6)
                                        {
                                            if (killer.Inventory.HaveSpace(1))
                                            {
                                                killer.Inventory.AddItemWitchStack(722725, 0, 1, stream);

                                                drop = false;

                                            }



                                        }
                                    #endregion
                                    if (killer.Map.AddGroundItem(ref xx, ref yy) && drop)
                                    {
                                        DropItem(stream, killer.Player.UID, killer.Map, ID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, ID_Special, ID_Quality, killer, DbItem);
                                        if (ID_Special)
                                            break;
                                    }

                                }
                            }
                        }

                    }
                    if (Family.ID == 7204)
                    {
                        ushort rand = (ushort)(killer.Player.MyRandom.Next() % 1000);
                        byte count = 1;

                        {
                            ushort xx = X;
                            ushort yy = Y;
                            for (byte i = 0; i < count; i++)
                            {

                                Database.ItemType.DBItem DbItem = null;
                                byte ID_Quality;
                                bool ID_Special;
                                uint ID = Family.ItemGenerator.GenerateItem1(Map, out ID_Quality, out ID_Special, out DbItem);
                                if (ID != 0)
                                {

                                    if (killer.Map.AddGroundItem(ref xx, ref yy))
                                    {

                                        #region PinkEgg
                                        if (ID == 711726 && killer != null)
                                        {
                                            killer.SendSysMesage("A monster you killed has dropped a PinkEgg at (" + xx + "," + yy + ")!", MsgMessage.ChatMode.System);

                                        }
                                        #endregion
                                        if (killer.Map.AddGroundItem(ref xx, ref yy))
                                        {
                                            DropItem(stream, killer.Player.UID, killer.Map, ID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, ID_Special, ID_Quality, killer, DbItem);
                                            if (ID_Special)
                                                break;
                                        }
                                    }
                                }

                            }
                        }



                    }
                    if (Family.ID == 7283)
                    {
                        ushort rand = (ushort)(killer.Player.MyRandom.Next() % 1000);
                        byte count = 1;

                        {
                            ushort xx = X;
                            ushort yy = Y;
                            for (byte i = 0; i < count; i++)
                            {

                                Database.ItemType.DBItem DbItem = null;
                                byte ID_Quality;
                                bool ID_Special;
                                uint ID = Family.ItemGenerator.GenerateItem3(Map, out ID_Quality, out ID_Special, out DbItem);
                                if (ID != 0)
                                {

                                    if (killer.Map.AddGroundItem(ref xx, ref yy))
                                    {
                                        #region BlueEgg
                                        if (ID == 710062 && killer != null)
                                        {
                                            killer.SendSysMesage("A monster you killed has dropped a BlueEgg at (" + xx + "," + yy + ")!", MsgMessage.ChatMode.System);

                                        }
                                        #endregion
                                        DropItem(stream, killer.Player.UID, killer.Map, ID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, ID_Special, ID_Quality, killer, DbItem);
                                        if (ID_Special)
                                            break;
                                    }
                                }

                            }
                        }


                    }
                    #endregion

                    #region DropItem

                    if (Family.Boss == 0)
                    {

                        ushort xx = X;
                        ushort yy = Y;
                        #region Item
                        if (Role.MyMath.Success(5))
                        {
                            Database.ItemType.DBItem DbItem = null;
                            byte ID_Quality;
                            bool ID_Special;
                            uint ID = Family.ItemGenerator.GenerateItem(Map, out ID_Quality, out ID_Special, out DbItem);
                            if (ID != 0)
                            {
                                bool drop = true;

                                if (killer.Map.AddGroundItem(ref xx, ref yy) && drop)
                                {
                                    DropItem(stream, killer.Player.UID, killer.Map, ID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, ID_Special, ID_Quality, killer, DbItem);

                                }
                            }

                        }

                        #endregion
                    }
                    #endregion

                    #region NinjaEpic
                    if (Boss > 0 && Family.ID == 20100)//NightmareCaptain
                    {
                        uint ItemID = 3004465;
                        if (Role.Core.Rate(20))
                        {
                            ItemID = 3004463;
#if Arabic
                               killer.CreateBoxDialog("You've found an YinYangSeal.");
#else
                            killer.CreateBoxDialog("You've found an YinYangSeal.");
#endif

                        }
                        else
                        {
#if Arabic
                               killer.CreateBoxDialog("You wasn`t very lucky today .. I hope you will have a better luck next time ^^.");
#else
                            killer.CreateBoxDialog("You wasn`t very lucky today .. I hope you will have a better luck next time ^^.");
#endif

                        }
                        ushort xx = X;
                        ushort yy = Y;
                        if (killer.Map.AddGroundItem(ref xx, ref yy))
                        {
                            DropItem(stream, killer.Player.UID, killer.Map, ItemID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                        }
                        return;
                    }
                    if (Boss > 0 && Family.ID == 20101 && Map == 3851)//PurpleBanshee
                    {
                        uint ItemID = 3004465;
                        if (Role.Core.Rate(20))
                        {
                            ItemID = 3004464;
#if Arabic
                                  killer.CreateBoxDialog("You've found an Life`sEye.");
                            SendSysMesage("The PurpleBanshee has been destroyed by " + killer.Player.Name.ToString() + ", he got the Life`sEye", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                       
#else
                            killer.CreateBoxDialog("You've found an Life`sEye.");
                            SendSysMesage("The PurpleBanshee has been destroyed by " + killer.Player.Name.ToString() + ", he got the Life`sEye", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);

#endif
                        }
                        else
                        {
#if Arabic
                             killer.CreateBoxDialog("You wasn`t very lucky today .. I hope you will have a better luck next time ^^.");
#else
                            killer.CreateBoxDialog("You wasn`t very lucky today .. I hope you will have a better luck next time ^^.");
#endif

                        }
                        ushort xx = X;
                        ushort yy = Y;
                        if (killer.Map.AddGroundItem(ref xx, ref yy))
                        {
                            DropItem(stream, killer.Player.UID, killer.Map, ItemID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                        }
                        return;
                    }
                    if (Boss > 0 && Family.ID == 20101 && Map == 3825)//PurpleBanshee
                    {
                        //3003340 SolarBlade
                        uint ItemID = 3003340;
#if Arabic
                         killer.CreateBoxDialog("You've found an SolarBlade.");
                        SendSysMesage("The PurpleBanshee has been destroyed by " + killer.Player.Name.ToString() + ", he got the SolarBlade", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                     
#else
                        killer.CreateBoxDialog("You've found an SolarBlade.");
                        SendSysMesage("The PurpleBanshee has been destroyed by " + killer.Player.Name.ToString() + ", he got the SolarBlade", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);

#endif


                        ushort xx = X;
                        ushort yy = Y;
                        if (killer.Map.AddGroundItem(ref xx, ref yy, 5))
                        {
                            DropItem(stream, killer.Player.UID, killer.Map, ItemID, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, false, 0);
                        }
                        return;
                    }
                    #endregion

                    #region Quest EpicTrojan
                    if (killer.Player.Map == 3831)
                    {
                        if (Family.ID == 7484)
                        {
                            var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)NpcID.MonkMisery, 0, 3272);
                            var ActiveQuest2 = Database.QuestInfo.GetFinishQuest((uint)NpcID.GeneralPakMap2, 0, 3273);
                            killer.Player.QuestGUI.FinishQuest(ActiveQuest.MissionId);
                            killer.Player.StageEpicTrojanQuest = 40;
                            killer.Player.QuestGUI.Accept(ActiveQuest2, 0);
                            killer.Player.QuestGUI.SendAutoPatcher("The~Evil~Monk~Misery`s~death~laugh~sounds~weird.~Something~may~happen~to~the~Flame~Devastator.~Go~report~to~General~Pak!", 3831, 154, 130, (uint)NpcID.GeneralPakMap2);
                        }
                    }
                    else if (killer.Player.Map == 3834)
                    {
                        if (Family.ID == 7483)
                        {


                            var ActiveQuest = Database.QuestInfo.GetFinishQuest((uint)NpcID.GeneralPak, 0, 3271);
                            var ActiveQuest2 = Database.QuestInfo.GetFinishQuest((uint)NpcID.GeneralPak, 0, 3277);
                            killer.Player.StageEpicTrojanQuest = 30;
                            killer.Player.EpicTrijanKillGhostReaver = 1;
                            killer.Player.EpicTrojanMrMirrorPrograss += 1;



                            MsgServer.MsgGameItem item = new MsgServer.MsgGameItem();
                            item.Color = (Role.Flags.Color)2;
                            item.ITEM_ID = 1182;
                            MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(item, 163, 228, MsgFloorItem.MsgItem.ItemType.Effect, 0, 0, killer.Player.Map
                                   , 0, false, killer.Map, 4);

                            if (killer.Map.EnqueueItem(DropItem))
                                DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Effect);

                            if (killer.Player.EpicTrojanMrMirrorPrograss >= 18)
                            {
                                killer.Player.QuestGUI.FinishQuest(ActiveQuest.MissionId);
                                killer.Player.QuestGUI.Accept(ActiveQuest2, 0);
                                killer.Player.MessageBox("All~the~18~Ghost~Reavers~in~the~Flame~Devastator`s~army~have~been~eliminated.~Report~back~to~General~Pak!", new Action<Client.GameClient>(p =>
                                {
                                    killer.Teleport(162, 218, 3830);
                                    killer.Player.QuestGUI.SendAutoPatcher(3830, 154, 130, 10581);

                                }), null);
                            }
                            else
                            {
                                killer.Player.MessageBox("A~Ghost~Reaver~fell~down~and~vanished.~So~far,~you~still~have~" + (18 - killer.Player.EpicTrojanMrMirrorPrograss) + "~Ghost~Reaver(s)~to~deal~with.", new Action<Client.GameClient>(p =>
                                {

                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var pstream = rec.GetStream();
                                        killer.ActiveNpc = (uint)Game.MsgNpc.NpcID.MrMirror2;
                                        Game.MsgNpc.NpcHandler.MrMirror2(killer, pstream, 0, "", 0);
                                    }
                                }), null);
                            }
                        }
                    }

                    else if (killer.Player.Map == 10)
                    {
                        if (Family.ID == 7485)
                        {
                            killer.Player.StageEpicTrojanQuest = 51;
                            if (!killer.Map.ContainMobID(7486))
                            {
                                killer.Map.AddMapMonster(stream, 7486, killer.Player.X, killer.Player.Y, 3, 3, 1, killer.Player.DynamicID, false, MsgFloorItem.MsgItemPacket.EffectMonsters.None, "ride_screen");
                            }

                            killer.SendSysMesage("The~earth~is~shaking,~and~the~Flame~Devastator~revived!");
                        }
                        else if (Family.ID == 7486)
                        {
                            killer.Player.StageEpicTrojanQuest = 52;
                            killer.Player.AddMapEffect(stream, 45, 45, "xidag_bafi");
                            if (!killer.Map.ContainMobID(7487))
                            {
                                killer.Map.AddMapMonster(stream, 7487, killer.Player.X, killer.Player.Y, 3, 3, 1, killer.Player.DynamicID, false, MsgFloorItem.MsgItemPacket.EffectMonsters.None, "xidag_bafi");
                            }
                        }
                        else if (Family.ID == 7487)
                        {

                            killer.CreateBoxDialog("Holding~the~Epic~Weapon~infused~with~the~blade~spirit,~you~beheaded~the~Flame~Devastator~and~burned~it~into~ashes.");
                            killer.Teleport(154, 132, 3832);
                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "role-select1");
                            bool finish = false;
                            foreach (var item in killer.Equipment.ClientItems.Values)
                            {
                                if (item.IsWeapon && !Database.ItemType.IsTwoHand(item.ITEM_ID) && !Database.ItemType.IsTrojanEpicWeapon(item.ITEM_ID))
                                {
                                    uint UpdateToEpic = (item.ITEM_ID % 1000) + 614000;
                                    item.ITEM_ID = UpdateToEpic;
                                    item.Mode = Role.Flags.ItemMode.Update;
                                    item.Send(killer, stream);
                                    finish = true;
                                    break;
                                }
                            }
                            if (finish == false)
                            {
                                foreach (var item in killer.Equipment.ClientItems.Values)
                                {
                                    if (item.IsWeapon && !Database.ItemType.IsTwoHand(item.ITEM_ID) && !Database.ItemType.IsTrojanEpicWeapon(item.ITEM_ID))
                                    {
                                        uint UpdateToEpic = (item.ITEM_ID % 1000) + 614000;
                                        item.ITEM_ID = UpdateToEpic;
                                        item.Mode = Role.Flags.ItemMode.Update;
                                        item.Send(killer, stream);
                                        finish = true;
                                        break;
                                    }
                                }
                            }
                            if (finish)
                            {
                                killer.Player.ResetEpicTrojan();


                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + "~successfully~prevented~Twin~City~from~an~olden~massacre,~and~obtained~an~Epic~Weapon!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.System).GetArray(stream));
                                killer.Inventory.Remove(3003340, 1, stream);
                            }
                        }
                    }
                    #endregion

                    #region SquidwardOctopus
                    else if (killer.Player.Map == 3071)
                    {
                        if (Family.ID == 2700)
                        {
                            DropItemID(killer, 1088001, stream, 6);
                            DropItemID(killer, 1088001, stream, 6);
                            DropItemID(killer, 1088001, stream, 6);
                            DropItemID(killer, 723341, stream, 6);
                            DropItemID(killer, 723341, stream, 6);
                            DropItemID(killer, 723341, stream, 6);

                            if (Role.Core.Rate(5, 100))
                                DropItemID(killer, 710212, stream, 6);
                            else if (Role.Core.Rate(30, 100))
                                DropItemID(killer, 720128, stream, 6);
                            else if (Role.Core.Rate(30, 100))
                                DropItemID(killer, 720128, stream, 6);
                            else if (Role.Core.Rate(20, 100))
                                DropItemID(killer, 728917, stream, 6);
                            else if (Role.Core.Rate(20, 100))
                                DropItemID(killer, 727306, stream, 6);
                            else if (Role.Core.Rate(20, 100))
                                DropItemID(killer, 728918, stream, 6);
                            else if (Role.Core.Rate(10, 100))
                                DropItemID(killer, 710214, stream, 6);
                            else if (Role.Core.Rate(20, 100))
                                DropItemID(killer, Database.ItemType.ExpBall2, stream, 6);
                            else
                            {
                                if (killer.Inventory.HaveSpace(1))
                                {
                                    killer.Inventory.Add(stream, 711609);
                                    killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "dispel4");
                                    killer.CreateBoxDialog("You~received~a~Gold~Coin!");
                                }
                                else
                                {
                                    killer.CreateBoxDialog("Your~inventory~is~full!");
                                }
                            }

                        }
                        else if (Family.ID == 2699)
                        {
                            if (killer.Inventory.HaveSpace(1))
                            {
                                killer.Inventory.Add(stream, 711610);
                                killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "dispel7");
                                killer.SendSysMesage("You~received~a~Silver~Coin!", MsgMessage.ChatMode.System);
                            }
                            else
                            {
                                killer.CreateBoxDialog("Your~inventory~is~full!");
                            }
                        }
                        else if (Family.ID == 7022)
                        {
                            if (Role.Core.Rate(1, 30))//to check !!!
                            {
                                if (Role.Core.Rate(1, 200))
                                {
                                    if (killer.Inventory.HaveSpace(1))
                                    {
                                        killer.Inventory.Add(stream, 711609);
                                        killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "dispel4");
                                        killer.CreateBoxDialog("You~received~a~Gold~Coin!");
                                    }
                                    else
                                    {
                                        killer.CreateBoxDialog("Your~inventory~is~full!");
                                    }
                                }
                                else
                                {
                                    if (Role.Core.Rate(2, 199))
                                    {
                                        if (killer.Inventory.HaveSpace(1))
                                        {
                                            killer.Inventory.Add(stream, 711610);
                                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "dispel7");
                                            killer.SendSysMesage("You~received~a~Silver~Coin!", MsgMessage.ChatMode.System);
                                        }
                                        else
                                        {
                                            killer.CreateBoxDialog("Your~inventory~is~full!");
                                        }
                                    }
                                    else
                                    {

                                        if (killer.Inventory.HaveSpace(1))
                                        {
                                            killer.Inventory.Add(stream, 711611);
                                            killer.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "dispel5");
                                            killer.SendSysMesage("You~received~a~Copper~Coin!", MsgMessage.ChatMode.System);
                                        }
                                        else
                                        {
                                            killer.CreateBoxDialog("Your~inventory~is~full!");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Tower of Mystery
                    if (killer.Player.TOM_StartChallenge)
                    {
                        bool finished = true;
                        foreach (var mob in killer.Player.View.Roles(Role.MapObjectType.Monster))
                            if (mob.Alive)
                                finished = false;
                        if (finished)
                        {

                            if (killer.Player.TOMChallengeToday == 0)
                            {
                                if (killer.Player.MyTowerOfMysteryLayer <= killer.Player.JoinTowerOfMysteryLayer)
                                    killer.Player.MyTowerOfMysteryLayer = (byte)Math.Min(killer.Player.MyTowerOfMysteryLayer + 1, 9);
                            }
                            else
                            {

                                if (killer.Player.MyTowerOfMysteryLayerElite <= killer.Player.JoinTowerOfMysteryLayer)
                                    killer.Player.MyTowerOfMysteryLayerElite = (byte)Math.Min(killer.Player.MyTowerOfMysteryLayerElite + 1, 9);
                            }
                            killer.CreateBoxDialog("You`ve successfully defeated the devil on Tower of Mystery " + (killer.Player.JoinTowerOfMysteryLayer + 1).ToString() + "F. Hurry and go claim the Bright Tribe`s reward for you.");
                            killer.Player.TOM_FinishChallenge = true;
                            foreach (var npc in killer.Player.View.Roles(Role.MapObjectType.Npc))
                                killer.Player.View.SendView(stream.NpcCreate(npc as Npc, 40150), true);
                        }
                    }
                    #endregion

                    #region DemonBox
                    else if (killer.MyHouse != null && killer.Player.DynamicID == killer.Player.UID)
                    {
                        #region HeavenDemonBox
                        if (Family.ID == 2435)//HeavenDemonBox
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                DropItemID(killer, 720679, stream);
                                killer.CreateBoxDialog("You killed a Heaven Demon and found a Frost CP Pack (69000CPs)!");
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Frost CP Pack (69000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 720678, stream);
                                    killer.CreateBoxDialog("You killed a Heaven Demon and found a Life CP Pack (13500CPs)!");
                                    Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Life CP Pack (13500CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                                }
                                else
                                {
                                    if (Role.Core.Rate(3700, 9989))
                                    {
                                        DropItemID(killer, 720677, stream);
                                        killer.CreateBoxDialog("You killed a Heaven Demon and found a Blood CP Pack (1000CPs)!");
                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1289, 6289))
                                        {
                                            DropItemID(killer, 720676, stream);
                                            killer.CreateBoxDialog("You killed a Heaven Demon and found a Soul CP Pack (500CPs)!");
                                        }
                                        else
                                        {
                                            if (Role.Core.Rate(1000, 5000))
                                            {
                                                DropItemID(killer, 720675, stream);

                                                killer.CreateBoxDialog("You killed a Heaven Demon and found a Ghost CP Pack (250CPs)!");
                                            }
                                            else
                                            {
                                                DropItemID(killer, 720680, stream);
                                                killer.CreateBoxDialog("You killed a Heaven Demon and found a Heaven Pill equal to the EXP of 2 and a half EXP Balls!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region ChaosDemonBox
                        else if (Family.ID == 2436)//ChaosDemonBox
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                DropItemID(killer, 720685, stream);

                                killer.CreateBoxDialog("You killed a Chaos Demon and found a Nimbus CP Pack (138000CPs)!");
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Nimbus CP Pack (138000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 720684, stream);


                                    killer.CreateBoxDialog("You killed a Chaos Demon and found a Butterfly CP Pack (27000CPs)!");
                                    Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Butterfly CP Pack (27000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                                }
                                else
                                {
                                    if (Role.Core.Rate(3700, 9989))
                                    {
                                        DropItemID(killer, 720683, stream);
                                        killer.CreateBoxDialog("You killed a Chaos Demon and found a Heart CP Pack (2000CPs)!");
                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1289, 6289))
                                        {
                                            DropItemID(killer, 720682, stream);
                                            killer.CreateBoxDialog("You killed a Chaos Demon and found a Flower CP Pack (1000CPs)!");
                                        }
                                        else
                                        {
                                            if (Role.Core.Rate(1000, 5000))
                                            {
                                                DropItemID(killer, 720681, stream);
                                                killer.CreateBoxDialog("You killed a Chaos Demon and found a Deity CP Pack (500CPs)!");
                                            }
                                            else
                                            {
                                                DropItemID(killer, 720686, stream);
                                                killer.CreateBoxDialog("You killed a Chaos Demon and found a Mystery Pill equal to the EXP of 2 and 1/3 EXP Balls!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Sacred Demon
                        else if (Family.ID == 2437)//sacreddemon
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                DropItemID(killer, 720691, stream);
                                killer.CreateBoxDialog("You killed a Sacred Demon and found a Kylin CP Pack (276000CPs)");
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Kylin CP Pack (276000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 720690, stream);
                                    killer.CreateBoxDialog("You killed a Sacred Demon and found a Rainbow CP Pack (54000CPs)!");
                                    Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Rainbow CP Pack (54000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                                }
                                else
                                {
                                    if (Role.Core.Rate(3700, 9989))
                                    {
                                        DropItemID(killer, 720689, stream);

                                        killer.CreateBoxDialog("You killed a Sacred Demon and found a Shadow CP Pack (4000CPs)!");
                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1289, 6289))
                                        {
                                            DropItemID(killer, 720688, stream);
                                            killer.CreateBoxDialog("You killed a Sacred Demon and found a Jewel CP Pack (2000CPs)!");
                                        }
                                        else
                                        {
                                            if (Role.Core.Rate(1000, 5000))
                                            {
                                                DropItemID(killer, 720687, stream);

                                                killer.CreateBoxDialog("You killed a Sacred Demon and found a Cloud CP Pack (1000CPs)!");
                                            }
                                            else
                                            {
                                                DropItemID(killer, 720692, stream);
                                                killer.CreateBoxDialog("You killed a Sacred Demon and found a Wind Pill equal to the EXP of 5 EXP Balls!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region TaiChiDemonBox
                        else if (Family.ID == 4355)//TaiChiDemonBox
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                DropItemID(killer, 3303370, stream);

                                killer.CreateBoxDialog("You killed an TaiChiDemonBox and found a Pilgrim CP Pack (1380000CPs)!");
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " got a Pilgrim CP Pack (1380000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 3303369, stream);
                                    killer.CreateBoxDialog("You killed an TaiChiDemonBox and found a Zephyr CP Pack (270000CPs)!");
                                    Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Zephyr CP Pack (270000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                                }
                                else
                                {
                                    if (Role.Core.Rate(3700, 9989))
                                    {
                                        DropItemID(killer, 3303368, stream);
                                        killer.CreateBoxDialog("You killed an TaiChiDemonBox and found an Earth CP Pack (20000CPs)!");
                                        Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found an Earth CP Pack (20000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1289, 6289))
                                        {
                                            DropItemID(killer, 3303367, stream);
                                            killer.CreateBoxDialog("You killed an TaiChiDemonBox and found a Moon CP Pack (10000CPs)!");
                                        }
                                        else
                                        {
                                            if (Role.Core.Rate(1000, 5000))
                                            {
                                                DropItemID(killer, 3303366, stream);
                                                killer.CreateBoxDialog("You killed an TaiChiDemonBox and found a Fog CP Pack (5000CPs)!");
                                            }
                                            else
                                            {
                                                DropItemID(killer, 720698, stream);
                                                killer.CreateBoxDialog("You killed an TaiChiDemonBox and got a Wind Pill equal to the EXP of 8 and 1/3 EXP Balls!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Aurorademonbox

                        else if (Family.ID == 2438)//aurorademonbox
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                DropItemID(killer, 720697, stream);

                                killer.CreateBoxDialog("You killed an Aurora Demon and found a Pilgrim CP Pack (690000CPs)!");
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " got a Pilgrim CP Pack (690000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 720696, stream);
                                    killer.CreateBoxDialog("You killed an Aurora Demon and found a Zephyr CP Pack (135000CPs)!");
                                    Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Zephyr CP Pack (135000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                                }
                                else
                                {
                                    if (Role.Core.Rate(3700, 9989))
                                    {
                                        DropItemID(killer, 720695, stream);
                                        killer.CreateBoxDialog("You killed an Aurora Demon and found an Earth CP Pack (10000CPs)!");
                                        Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found an Earth CP Pack (10000CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1289, 6289))
                                        {
                                            DropItemID(killer, 720694, stream);
                                            killer.CreateBoxDialog("You killed an Aurora Demon and found a Moon CP Pack (5000CPs)!");
                                        }
                                        else
                                        {
                                            if (Role.Core.Rate(1000, 5000))
                                            {
                                                DropItemID(killer, 720693, stream);
                                                killer.CreateBoxDialog("You killed an Aurora Demon and found a Fog CP Pack (2500CPs)!");
                                            }
                                            else
                                            {
                                                DropItemID(killer, 720698, stream);
                                                killer.CreateBoxDialog("You killed an Aurora Demon and got a Wind Pill equal to the EXP of 8 and 1/3 EXP Balls!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Demon


                        else if (Family.ID == 2420)
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                DropItemID(killer, 720654, stream);

                                killer.CreateBoxDialog("You killed a Demon and found a Joy CP Pack (1380CPs)!");
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Joy CP Pack (1380CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 720653, stream);

                                    killer.CreateBoxDialog("You killed a Demon and found a Dream CP Pack (270CPs)!");
                                }
                                else
                                {
                                    if (Role.Core.Rate(3700, 9989))
                                    {
                                        DropItemID(killer, 720655, stream);
                                        killer.CreateBoxDialog("You killed a Demon and found a Mammon CP Pack (20CPs)!");
                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1289, 6289))
                                        {
                                            DropItemID(killer, 720656, stream);
                                            killer.CreateBoxDialog("You killed a Demon and found a Mascot CP Pack (10CPs)!");
                                        }
                                        else
                                        {
                                            if (Role.Core.Rate(1000, 5000))
                                            {
                                                DropItemID(killer, 720657, stream);
                                                killer.CreateBoxDialog("You killed a Demon and found a Hope CP Pack (5CPs)!");
                                            }
                                            else
                                            {
                                                DropItemID(killer, 720668, stream);
                                                killer.CreateBoxDialog("You killed a Demon and found a Magic Ball equal to the EXP of 1/6 of an EXP Ball!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Ancient Demon
                        else if (Family.ID == 2421)//ancient
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                killer.CreateBoxDialog("You killed a Ancient Demon and found a Mystic CP Pack (6900CPs)!");
                                DropItemID(killer, 720662, stream);
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Mystic CP Pack (6900CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));

                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 720661, stream);

                                    killer.CreateBoxDialog("You killed a Ancient Demon and found a Pure CP Pack (1350CPs)!");
                                }
                                else
                                {
                                    if (Role.Core.Rate(3700, 9989))
                                    {
                                        DropItemID(killer, 720660, stream);
                                        killer.CreateBoxDialog("You killed a Ancient Demon and found a Legend CP Pack (100CPs)!");
                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1289, 6289))
                                        {
                                            DropItemID(killer, 720659, stream);

                                            killer.CreateBoxDialog("You killed a Ancient Demon and found a Sweet CP Pack (50CPs)!");
                                        }
                                        else
                                        {
                                            if (Role.Core.Rate(1000, 5000))
                                            {
                                                DropItemID(killer, 720658, stream);

                                                killer.CreateBoxDialog("You killed a Ancient Demon and found a Festival CP Pack (25CPs)!");
                                            }
                                            else
                                            {
                                                DropItemID(killer, 720669, stream);
                                                killer.CreateBoxDialog("You killed the Ancient Demon and found a Super Ball equal to the EXP of 5/6 of an EXP Ball!");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Flood Demon
                        else if (Family.ID == 2422)
                        {
                            if (Role.Core.Rate(1, 10000))
                            {
                                DropItemID(killer, 720667, stream);
                                killer.CreateBoxDialog("You killed a Flood Demon and found a Fantasy CP Pack (13800CPs)!");
                                Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Fantasy CP Pack (13800CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                            }
                            else
                            {
                                if (Role.Core.Rate(10, 9999))
                                {
                                    DropItemID(killer, 720666, stream);

                                    killer.CreateBoxDialog("You killed a Flood Demon and found a Star CP Pack (2700CPs)!");
                                    Server.SendGlobalPacket(new MsgMessage("" + killer.Player.Name + " found a Star CP Pack (2700CPs)!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                                }
                                else
                                {

                                    if (Role.Core.Rate(1289, 6289))
                                    {
                                        DropItemID(killer, 721019, stream);

                                        killer.CreateBoxDialog("You killed a Flood Demon and found a Flare CP Pack (100CPs)!");
                                    }
                                    else
                                    {
                                        if (Role.Core.Rate(1000, 5000))
                                        {
                                            DropItemID(killer, 721016, stream);
                                            killer.CreateBoxDialog("You killed a Flood Demon and found a Violet CP Pack (50CPs)!");
                                        }
                                        else
                                        {
                                            DropItemID(killer, 720670, stream);
                                            killer.CreateBoxDialog("You killed the Flood Demon and found an Ultra Ball equal to EXP worth 1 and 2/3 EXP Balls!");
                                        }
                                    }
                                    //}
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region DemonExterminator
                    if (Family.ID == 4212)
                    {
                        if (killer.Team != null)
                        {
                            foreach (var user in killer.Team.Temates)
                            {
                                if (user.client.Player.Map == killer.Player.Map && user.client.Player.DynamicID == killer.Player.DynamicID)
                                {
                                    if (user.client.Player.QuestGUI.CheckQuest(6126, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                                    {
                                        user.client.Player.QuestGUI.IncreaseQuestObjectives(stream, 6126, 1);
                                    }
                                }
                            }
                        }
                        if (killer.Player.QuestGUI.CheckQuest(6126, MsgQuestList.QuestListItem.QuestStatus.Accepted))
                        {
                            killer.Player.QuestGUI.IncreaseQuestObjectives(stream, 6126, 1);
                        }
                    }
                    if (killer.DemonExterminator != null)
                        killer.DemonExterminator.UppdateJar(killer, Family.ID);
                    #endregion

                    #region Drop Bag CPS

                    killer.Player.ConquerPoints += 5;
                    killer.SendSysMesage("You received " + 5 + " cps in your Inventory.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);

                    if (killer.Player.VipLevel <= 3)
                    {
                        if (Role.MyMath.Success(0.015))
                        {
                            DropItemID(killer, 721039, stream, 6);
                            killer.SendSysMesage("You Drop " + Pool.ItemsBase[721039].Name + ".", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        }
                        if (Role.MyMath.Success(0.003))
                        {
                            DropItemID(killer, 721049, stream, 6);
                            killer.SendSysMesage("You Drop " + Pool.ItemsBase[721049].Name + ".", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        }
                        if (Role.MyMath.Success(0.0007))
                        {
                            DropItemID(killer, 721057, stream, 6);
                            killer.SendSysMesage("You Drop " + Pool.ItemsBase[721057].Name + ".", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        }


                    }
                    else
                    {
                        if (Role.MyMath.Success(0.015))
                        {
                            killer.Player.ConquerPoints += 1000;
                            killer.SendSysMesage("[Lucky]You received " + 1000 + " cps in your Inventory.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        }
                        if (Role.MyMath.Success(0.003))
                        {
                            killer.Player.ConquerPoints += 5000;
                            killer.SendSysMesage("[Lucky]You received " + 5000 + " cps in your Inventory.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        }
                        if (Role.MyMath.Success(0.0007))
                        {
                            killer.Player.ConquerPoints += 10000;
                            killer.SendSysMesage("[Lucky]You received " + 10000 + " cps in your Inventory.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        }

                    }
                   
                    #region EXPBALL
                    if (Role.MyMath.Success(0.5))
                    {
                        DropItemID(killer, 722136, stream, 6);
                        killer.SendSysMesage("You Drop " + Pool.ItemsBase[722136].Name + ".", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                    }
                    #endregion
                    #endregion
                }

            }

        }
        private void DropItem(ServerSockets.Packet stream, uint OwnerItem, Role.GameMap map, uint ItemID, ushort XX, ushort YY, MsgFloorItem.MsgItem.ItemType typ
                 , uint amount, bool special, byte ID_Quality, Client.GameClient user = null, Database.ItemType.DBItem DBItem = null)
        {
            MsgServer.MsgGameItem DataItem = new MsgServer.MsgGameItem();

            DataItem.ITEM_ID = ItemID;
            if (DataItem.Durability > 100)
            {
                DataItem.Durability = (ushort)Program.GetRandom.Next(100, DataItem.Durability / 10);
                DataItem.MaximDurability = DataItem.Durability;
            }

            else
            {
                DataItem.Durability = (ushort)Program.GetRandom.Next(1, 10);
                DataItem.MaximDurability = 10;
            }

            DataItem.Color = Role.Flags.Color.Red;
            if (typ == MsgFloorItem.MsgItem.ItemType.Item)
            {
                byte sockets;
                bool lucky = false;
                if (DataItem.IsEquip)
                {
                    if (!special)
                    {

                        lucky = (ID_Quality > 7); // q>unique
                        if (!lucky)
                            lucky = (DataItem.Plus = Family.ItemGenerator.GeneratePurity()) != 0;
                        if (!lucky)
                            lucky = (DataItem.Bless = Family.ItemGenerator.GenerateBless()) != 0;
                        if (!lucky)
                        {
                            if (DataItem.IsWeapon)
                            {
                                sockets = Family.ItemGenerator.GenerateSocketCount(DataItem.ITEM_ID);

                                if (sockets >= 1)
                                    DataItem.SocketOne = Role.Flags.Gem.EmptySocket;
                                else if (sockets == 2)
                                    DataItem.SocketTwo = Role.Flags.Gem.EmptySocket;
                            }
                        }
                    }
                    if (DBItem != null)
                    {
                        DataItem.Durability = (ushort)Program.GetRandom.Next(1, DBItem.Durability / 10 + 10);
                        DataItem.MaximDurability = (ushort)Program.GetRandom.Next(DataItem.Durability, DBItem.Durability);
                    }
                }
                else
                {
                    if (DBItem != null)
                        DataItem.Durability = DataItem.MaximDurability = DBItem.Durability;
                }
            }
            if (user != null)
            {
                if (user.Player.VipLevel >= 4)
                {
                    if (DataItem.ITEM_ID == 1088000)
                    {
                        user.Inventory.Update(DataItem, Role.Instance.AddMode.ADD, stream);
                        if (user.Inventory.Contain(1088000, 10) && user.Player.VipLevel == 6)
                        {
                            user.Inventory.Remove(1088000, 10, stream);
                            user.Inventory.Add(stream, 720028, 1);
                        }
                        return;
                    }
                }
            }

            MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(DataItem, XX, YY, typ, amount, DynamicID, Map, OwnerItem, true, map);

            if (map.EnqueueItem(DropItem))
            {
                DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Visible);
            }
        }
        public void AddFadeAway(int time, Role.GameMap map)
        {
            if (!Alive)
            {
                DateTime timer = DateTime.Now;
                if (timer > DeadStamp.AddSeconds(5))
                {
                    if (AddFlag(MsgServer.MsgUpdate.Flags.FadeAway, Role.StatusFlagsBigVector32.PermanentFlag, true))
                    {
                        FadeAway = timer;

                    }
                }
            }
        }
        public unsafe bool RemoveView(int time, Role.GameMap map)
        {
            if (ContainFlag(MsgServer.MsgUpdate.Flags.FadeAway) && State != MobStatus.Respawning)
            {
                DateTime timer = DateTime.Now;
                if (timer > FadeAway.AddSeconds(3))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();

                        ActionQuery action;

                        action = new ActionQuery()
                        {
                            ObjId = UID,
                            Type = ActionType.RemoveEntity
                        };

                        Send(stream.ActionCreate(action));
                    }

                    State = MobStatus.Respawning;

                    map.View.MoveTo<Role.IMapObj>(this, RespawnX, RespawnY);

                    X = RespawnX;
                    Y = RespawnY;
                    Target = null;

                    return true;
                }
            }
            return false;
        }

        public void DropItemID(Client.GameClient killer, uint itemid, ServerSockets.Packet stream, byte range = 3, int mins = 0)
        {
            MsgServer.MsgGameItem DataItem = new MsgServer.MsgGameItem();
            DataItem.ITEM_ID = itemid;
            Database.ItemType.DBItem DBItem;
            if (itemid == 4034801 || itemid == 4035101 || itemid == 4035009 || itemid == 4034201 || itemid == 4034301)
            {
                return;
            }
            if (Pool.ItemsBase.TryGetValue(itemid, out DBItem))
            {
                DataItem.Durability = DataItem.MaximDurability = DBItem.Durability;
            }
            DataItem.Color = Role.Flags.Color.Red;
            ushort xx = (ushort)Program.GetRandom.Next(X - 5, X + 5);
            ushort yy = (ushort)Program.GetRandom.Next(Y - 4, Y + 4);
            if (killer.Map.AddGroundItem(ref xx, ref yy, range))
            {
                MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(DataItem, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, DynamicID, Map, killer.Player.UID, true, GMap);

                if (killer.Map.EnqueueItem(DropItem))
                {
                    DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Visible);
                }
            }
        }
        public void DropItemID2(Client.GameClient killer, uint itemid, ServerSockets.Packet stream, byte range = 3, int mins = 0)
        {
            MsgServer.MsgGameItem DataItem = new MsgServer.MsgGameItem();
            DataItem.ITEM_ID = itemid;
            Database.ItemType.DBItem DBItem;
            if (itemid == 4034801 || itemid == 4035101 || itemid == 4035009 || itemid == 4034201 || itemid == 4034301)
            {
                return;
            }
            if (Pool.ItemsBase.TryGetValue(itemid, out DBItem))
            {
                DataItem.Durability = DataItem.MaximDurability = DBItem.Durability;
            }
            DataItem.Color = Role.Flags.Color.Red;
            ushort xx = (ushort)Program.GetRandom.Next(X - 14, X + 14);
            ushort yy = (ushort)Program.GetRandom.Next(Y - 14, Y + 14);
            if (killer.Map.AddGroundItem(ref xx, ref yy, range))
            {
                MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(DataItem, xx, yy, MsgFloorItem.MsgItem.ItemType.Item, 0, DynamicID, Map, killer.Player.UID, true, GMap);

                if (killer.Map.EnqueueItem(DropItem))
                {
                    DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Visible);
                }
            }
        }
        public MonsterFamily Family;
        public MonsterView View;
        public MobStatus State;
        public Role.Player Target = null;
        public DateTime AttackSpeed = new DateTime();

        public Role.StatusFlagsBigVector32 BitVector;
        public void AddSpellFlag(Game.MsgServer.MsgUpdate.Flags Flag, int Secounds, bool RemoveOnDead, int SecoundStamp = 0)
        {
            if (BitVector.ContainFlag((int)Flag))
                BitVector.TryRemove((int)Flag);
            AddFlag(Flag, Secounds, RemoveOnDead, SecoundStamp);
        }
        public bool AddFlag(Game.MsgServer.MsgUpdate.Flags Flag, int Secounds, bool RemoveOnDead, int StampSecounds = 0)
        {
            if (!BitVector.ContainFlag((int)Flag))
            {
                BitVector.TryAdd((int)Flag, Secounds, RemoveOnDead, StampSecounds);
                UpdateFlagOffset();
                return true;
            }
            return false;
        }
        public bool RemoveFlag(Game.MsgServer.MsgUpdate.Flags Flag, Role.GameMap map)
        {
            if (BitVector.ContainFlag((int)Flag))
            {
                BitVector.TryRemove((int)Flag);
                UpdateFlagOffset();
                return true;
            }
            return false;
        }
        public bool UpdateFlag(Game.MsgServer.MsgUpdate.Flags Flag, int Secounds, bool SetNewTimer, int MaxTime)
        {
            return BitVector.UpdateFlag((int)Flag, Secounds, SetNewTimer, MaxTime);
        }
        public void ClearFlags(bool SendScreem = false)
        {
            BitVector.GetClear();
            UpdateFlagOffset(SendScreem);
        }
        public bool ContainFlag(Game.MsgServer.MsgUpdate.Flags Flag)
        {
            return BitVector.ContainFlag((int)Flag);
        }
        private unsafe void UpdateFlagOffset(bool SendScreem = true)
        {
            if (SendScreem)
                SendUpdate(BitVector.bits, Game.MsgServer.MsgUpdate.DataType.StatusFlag);
        }

        public byte OpenBoss = 0;
        public uint Map { get; set; }
        public uint DynamicID { get; set; }

        public short GetMyDistance(ushort X2, ushort Y2)
        {
            return Role.Core.GetDistance(X, Y, X2, Y2);
        }
        public short OldGetDistance(ushort X2, ushort Y2)
        {
            return Role.Core.GetDistance(PX, PY, X2, Y2);
        }
        public bool InView(ushort X2, ushort Y2, byte distance)
        {
            return (!(OldGetDistance(X2, Y2) < distance) && GetMyDistance(X2, Y2) < distance);
        }


        public unsafe void Send(ServerSockets.Packet msg)
        {
            View.SendScreen(msg, GMap);
        }
        public void UpdateMonsterView(Role.RoleView Target, ServerSockets.Packet stream)
        {
            foreach (var player in View.Roles(GMap, Role.MapObjectType.Player))
            {
                if (InView(player.X, player.Y, MonsterView.ViewThreshold))
                    player.Send(GetArray(stream, false));
            }
        }
        public bool UpdateMapCoords(ushort New_X, ushort New_Y, Role.GameMap _map)
        {
            if (!_map.IsFlagPresent(New_X, New_Y, Role.MapFlagType.Monster))
            {
                _map.SetMonsterOnTile(X, Y, false);
                _map.SetMonsterOnTile(New_X, New_Y, true);
                _map.View.MoveTo<MonsterRole>(this, New_X, New_Y);
                X = New_X;
                Y = New_Y;
                return true;
            }
            return false;
        }
        public void RemoveRole(Role.IMapObj obj)
        {

        }
        public Role.MapObjectType ObjType { get; set; }

        public byte Boss = 0;
        public uint Mesh = 0;
        public uint UID { get; set; }
        public byte Level = 0;
        public uint HitPoints;
        public uint OwnerCount = 0;
        public uint OwnerFlag = 0;
        public uint OwnerUID = 0;
        public ushort RespawnX;
        public ushort RespawnY;


        public ushort PX = 0;
        public ushort PY = 0;
        public ushort _xx;
        public ushort _yy;

        public ushort X { get { return _xx; } set { PX = _xx; _xx = value; } }
        public ushort Y { get { return _yy; } set { PY = _yy; _yy = value; } }
        public Role.Flags.ConquerAction Action = Role.Flags.ConquerAction.None;
        public Role.Flags.ConquerAngle Facing = Role.Flags.ConquerAngle.East;
        public string LocationSpawn = "";
        public Role.GameMap GMap;
        public bool RemoveOnDead = false;
        public uint PetFlag = 0;


        public unsafe void SendString(ServerSockets.Packet stream, Game.MsgServer.MsgStringPacket.StringID id, params string[] args)
        {
            Game.MsgServer.MsgStringPacket packet = new Game.MsgServer.MsgStringPacket();
            packet.ID = id;
            packet.UID = UID;

            packet.Strings = args;
            Send(stream.StringPacketCreate(packet));
        }
        public MonsterRole(MonsterFamily Famil, uint _UID, string locationspawn, Role.GameMap _map)
        {
            AllowDynamic = false;
            GMap = _map;
            LocationSpawn = locationspawn;
            ObjType = Role.MapObjectType.Monster;
            Name = Famil.Name;
            Family = Famil;
            UID = _UID;
            Mesh = Famil.Mesh;
            Level = (byte)Famil.Level;
            HitPoints = (uint)Famil.MaxHealth;
            View = new MonsterView(this);
            State = MobStatus.Idle;
            BitVector = new Role.StatusFlagsBigVector32(450);
            Boss = Family.Boss;
            Facing = (Role.Flags.ConquerAngle)Pool.GetRandom.Next(0, 8);

        }
        public bool Alive { get { return HitPoints > 0; } }



        public unsafe ServerSockets.Packet GetArray(ServerSockets.Packet stream, bool view)
        {
            if (IsFloor && Mesh != 980)
            {
                return stream.ItemPacketCreate(this.FloorPacket);

            }
            stream.InitWriter();
            var proto = new Role.Player.SpawnPacketProto()
            {
                UID = UID,
                Mesh = Mesh,
                MonsterLevel = Level,
                Hitpoints = IsFloor ? (uint)StampFloorSecounds : (uint)HitPoints,
                X = X,
                Y = Y,
                Action = (ushort)Action,
                Level = Level,
                MonstersID = Family.ID,
                Names = new string[4] { Name, string.Empty, string.Empty, string.Empty }
            };
            proto.Boss = Boss > 0;
            if (Mesh == 980)
            {
                proto.OwnerPet = (uint)3;
                proto.Facing = (byte)this.OwnerFloor.Player.Angle;
                proto.OwnerUID = this.OwnerFloor.Player.UID;
                proto.GuildID = this.OwnerFloor.Player.GuildID;
                proto.OwnerPet1 = (uint)15;
                proto.MonstersID = (uint)4264;
            }
            if (this.Mesh == (uint)138 || this.Mesh == (uint)137 || this.Mesh == (uint)848)
            {
                proto.OwnerPet = this.PetFlag;
                proto.Facing = (byte)this.OwnerFloor.Player.Angle;
                proto.OwnerUID = this.OwnerFloor.Player.UID;
                proto.OwnerPet1 = (uint)15;
                proto.MonstersID = this.Family.ID;
            }
            proto.MaxLife = (uint)Family.MaxHealth;
            proto.StatusFlags = new ulong[BitVector.bits.Length];
            for (int x = 0; x < BitVector.bits.Length; x++)
                proto.StatusFlags[x] = BitVector.bits[x];
            if (!IsFloor && Boss > 0)
            {
                uint key = (uint)(Family.MaxHealth / 10000);
                if (key != 0)
                    proto.Hitpoints = (uint)(proto.Hitpoints / key);
                else
                    proto.Hitpoints = (uint)(proto.Hitpoints * Family.MaxHealth);
            }

            stream.ProtoBufferSerialize(proto);
            stream.Finalize(Game.GamePackets.MsgPlayer);
            return stream;
        }
        public unsafe void SendUpdate(uint[] Value, Game.MsgServer.MsgUpdate.DataType datatype)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                Game.MsgServer.MsgUpdate packet = new Game.MsgServer.MsgUpdate(stream, UID, 1);
                stream = packet.Append(stream, datatype, Value);
                stream = packet.GetArray(stream);
                Send(stream);
            }
        }
    }
}