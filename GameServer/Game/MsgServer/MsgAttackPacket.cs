using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using VirusX.Game.MsgServer.AttackHandler.CheckAttack;
using ProtoBuf;
using VirusX.Role.Instance;
using VirusX.Game.MsgServer.AttackHandler;
using VirusX.Database;

namespace VirusX.Game.MsgServer
{
    [ProtoContract]
    public unsafe class InteractQuery
    {
        [ProtoMember(1, IsRequired = true)]
        public long unk1;

        [ProtoMember(2, IsRequired = true)]
        public long unk2;

        [ProtoMember(3, IsRequired = true)]
        public long unk3;

        [ProtoMember(4, IsRequired = true)]
        public uint UID;

        [ProtoMember(5, IsRequired = true)]
        public uint OpponentUID;

        [ProtoMember(6, IsRequired = true)]
        public long unk6;

        [ProtoMember(7, IsRequired = true)]
        public ushort X;

        [ProtoMember(8, IsRequired = true)]
        public ushort Y;

        [ProtoMember(9, IsRequired = true)]
        public long unk9;

        [ProtoMember(10, IsRequired = true)]
        public long unk10;

        [ProtoMember(11, IsRequired = true)]
        public long unk11;

        [ProtoMember(12, IsRequired = true)]
        public long unk12;

        [ProtoMember(13, IsRequired = true)]
        public ushort AtkType;

        [ProtoMember(14, IsRequired = true)]
        public long unk14;

        [ProtoMember(15, IsRequired = true)]
        public int Damage;

        [ProtoMember(16, IsRequired = true)]
        public long SpellPirate;

        [ProtoMember(17, IsRequired = true)]
        public int KilledMonster;

        [ProtoMember(18, IsRequired = true)]
        public ushort SpellLevel;

        [ProtoMember(19, IsRequired = true)]
        public ushort SpellID;

        [ProtoMember(21, IsRequired = true)]
        public uint ResponseDamage;

        [ProtoMember(22, IsRequired = true)]
        public long unk22;

        [ProtoMember(23, IsRequired = true)]
        public long unk23;

        [ProtoMember(24, IsRequired = true)]
        public long unk24;

        [ProtoMember(25, IsRequired = true)]
        public uint Effect;

        [ProtoMember(26, IsRequired = true)]
        public long Unknown;

        public static InteractQuery ShallowCopy(InteractQuery item)
        {
            return (InteractQuery)item.MemberwiseClone();
        }
    }
    public unsafe static class MsgAttackPacket
    {
        [Flags]
        public enum AttackEffect : ulong  
        {
            None = 0,
            Block = 1 << 0,
            Penetration = 1 << 1,
            CriticalStrike = 1 << 2,
            Imunity = 1 << 3,
            Break = Imunity | Penetration,
            ResistMetal = 1 << 4,
            ResistWood = 1 << 5,
            ResistWater = 1 << 6,
            ResistFire = 1 << 7,
            ResistEarth = 1 << 8,
            AddStudyPoint = 1 << 9,
            LuckyStrike = 1 << 10,
            TortoiseBreaker = 1 << 12,
            NatureShield = 1 << 13,
            Assasssin = 1 << 14,
            BurningSky = 1 << 15,
            FireCurse = 1 << 16,
            TailedBeast = 1 << 18,
            DashRate7 = 1 << 19,
            TacitStrike = 1 << 20,
            Shield2 = 1 << 21,
            UniversalShieldBarrier = 1 << 22,
            DashRate11 = 1 << 23,
            DestroyWarrior = 1 << 24,
            MudWall = 1 << 25,
            Destroy = 1 << 26,
            DashRate = 1 << 27,
            DashRate2 = 1 << 28,
            DashRate3 = 1 << 29,
            BlockNew = 1 << 30,
            Defence = 1ul << 31,
            CrackMyth1 = 1ul << 32,
            SmashFrontal = 1ul << 33,
            CrackMyth = 1ul << 34,
            DashRate13 = 1ul << 35,
            BestShield = 1ul << 36,
            DashRate15 = 1ul << 37,
            DashRat16 = 1ul << 38,
            BreakDown = 1ul << 51,
        }
        public enum AttackID : uint
        {
            None = 0x00,
            Physical = 0x02,
            Magic = 0x18,
            Archer = 0x1C,
            RequestMarriage = 0x08,
            AcceptMarriage = 0x09,
            Death = 0x0E,
            Reflect = 26,
            Dash = 27,
            UpdateHunterJar = 36,
            CounterKillSwitch = 44,
            Scapegoat = 43,
            FatalStrike = 45,
            InteractionRequest = 46,
            InteractionAccept = 47,
            InteractionRefuse = 48,
            InteractionEffect = 49,
            InteractionStopEffect = 50,
            InMoveSpell = 53,
            BlueDamage = 55,
            BackFire = 57,
            HittheWaterThreethouSandAttack = 65
        }
        public static unsafe void Interaction(this ServerSockets.Packet stream, out InteractQuery pQuery, VirusX.Client.GameClient user)
        {
            pQuery = new InteractQuery();
            pQuery = stream.ProtoBufferDeserialize<InteractQuery>(pQuery);



            if ((AttackID)pQuery.AtkType == AttackID.Magic && user.OnAutoAttack == false)
            {
                DecodeMagicAttack(pQuery, user);
            }
            if ((AttackID)pQuery.AtkType == AttackID.Physical ||
           (AttackID)pQuery.AtkType == AttackID.Archer)
            {
                DecodeMagicAttackPhysical(pQuery, user);
            }
        }

        public static unsafe ServerSockets.Packet InteractionCreate(this ServerSockets.Packet stream, InteractQuery pQuery)
        {


            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgInteract);

            return stream;
        }


        public static unsafe void EncodeMagicAttack(InteractQuery pQuery)
        {
            int magicType, magicLevel;
            BitUnfold32(pQuery.Damage, out magicType, out magicLevel);

            magicType = (ushort)(ExchangeShortBits((uint)magicType - 0x14be, 3) ^ pQuery.UID ^ 0x915d);
            magicLevel = (ushort)((magicLevel + 0x100 * (0 % 0x100)) ^ 0x3721);

            pQuery.Damage = BitFold32(magicType, magicLevel);
            pQuery.OpponentUID = (uint)ExchangeLongBits((((uint)pQuery.OpponentUID - 0x8b90b51a) ^ (uint)pQuery.UID ^ 0x5f2d2463u), 32 - 13);
            pQuery.X = (ushort)(ExchangeShortBits((uint)pQuery.X - 0xdd12, 1) ^ pQuery.UID ^ 0x2ed6);
            pQuery.Y = (ushort)(ExchangeShortBits((uint)pQuery.Y - 0x76de, 5) ^ pQuery.UID ^ 0xb99b);
        }
        private static unsafe void DecodeMagicAttackPhysical(InteractQuery pQuery, Client.GameClient user)
        {

            int magicType, magicLevel;
            BitUnfold32(pQuery.SpellID, out magicType, out magicLevel);

            magicType = (ushort)(ExchangeShortBits(((ushort)magicType ^ (uint)pQuery.UID ^ 0x915D), 16 - 3) + 0x14be);

            magicLevel = (ushort)(((byte)magicLevel) ^ 0x21);
            pQuery.SpellID = 0;
            pQuery.OpponentUID = (uint)((ExchangeLongBits((uint)pQuery.OpponentUID, 13) ^ (uint)pQuery.UID ^ 0x5f2d2463) + 0x8b90b51a);
            pQuery.X = (ushort)(ExchangeShortBits(((ushort)pQuery.X ^ (uint)pQuery.UID ^ 0x2ed6), 16 - 1) + 0xdd12);
            pQuery.Y = (ushort)(ExchangeShortBits(((ushort)pQuery.Y ^ (uint)pQuery.UID ^ 0xb99b), 16 - 5) + 0x76de);


        }
        private static unsafe void DecodeMagicAttack(InteractQuery pQuery, Client.GameClient user)
        {
            //decrypt loader

            int magicType, magicLevel;
            BitUnfold32(pQuery.SpellID, out magicType, out magicLevel);
            magicType = (ushort)(ExchangeShortBits(((ushort)magicType ^ (uint)pQuery.UID ^ 0x915d), 16 - 3) + 0x14be);

            magicLevel = (ushort)(((byte)magicLevel) ^ 0x21);
            pQuery.SpellID = (ushort)BitFold32(magicType, magicLevel);
            pQuery.OpponentUID = (uint)((ExchangeLongBits((uint)pQuery.OpponentUID, 13) ^ (uint)pQuery.UID ^ 0x5f2d2463) + 0x8b90b51a);
            pQuery.X = (ushort)(ExchangeShortBits(((ushort)pQuery.X ^ (uint)pQuery.UID ^ 0x2ed6), 16 - 1) + 0xdd12);
            pQuery.Y = (ushort)(ExchangeShortBits(((ushort)pQuery.Y ^ (uint)pQuery.UID ^ 0xb99b), 16 - 5) + 0x76de);
        }
        public static int BitFold32(int lower16, int higher16)
        {
            return (lower16) | (higher16 << 16);
        }
        public static void BitUnfold32(int bits32, out int lower16, out int upper16)
        {
            lower16 = (int)(bits32 & UInt16.MaxValue);
            upper16 = (int)(bits32 >> 16);
        }
        public static void BitUnfold64(ulong bits64, out int lower32, out int upper32)
        {
            lower32 = (int)(bits64 & UInt32.MaxValue);
            upper32 = (int)(bits64 >> 32);
        }
        private static uint ExchangeShortBits(uint data, int bits)
        {
            data &= 0xffff;
            return ((data >> bits) | (data << (16 - bits))) & 0xffff;
        }

        public static uint ExchangeLongBits(uint data, int bits)
        {
            return (data >> bits) | (data << (32 - bits));
        }
        [PacketAttribute(GamePackets.MsgInteract)]
        public static void HandlerProcess(Client.GameClient user, ServerSockets.Packet stream)
        {
            user.OnAutoAttack = false;
            user.Player.Action = Role.Flags.ConquerAction.None;
            user.Player.RemoveBuffersMovements(stream);

            var attack_obj = new AttackObj();
            InteractQuery Attack;
            stream.Interaction(out Attack, user);
            if (user.Player.ActivePick)
                user.Player.RemovePick(stream);
            try
            {

            }
            catch
            { }

            if (user.Player.InUseIntensify)
            {
                user.Player.InUseIntensify = false;
                using (var recycledPacket = new ServerSockets.RecycledPacket())
                {
                    var streamm = recycledPacket.GetStream();
                    user.Send(streamm.ActionCreate(new ActionQuery() { ObjId = user.Player.UID, TargetUID = user.Player.FocusClientSpell.ID, Timestamp = (uint)Time32.timeGetTime().GetHashCode(), Type = 103 }));
                }
            }
            if (user.Player.RemoveFlag(Game.MsgServer.MsgUpdate.Flags.Focused))
                user.Player.doFocus = true;

            Process(user, Attack, true);
            user.Player.doFocus = false;



        }
        public class AttackObj
        {
            public Client.GameClient User;
            public InteractQuery Attack;
        }

        public static void Process(Client.GameClient user, InteractQuery Attack, bool CheckTargetType = false)
        {

            using (var rec = new ServerSockets.RecycledPacket())
            {

                var stream = rec.GetStream();

                if (user.Player.Map == 2068 || (user.IsWatching() && user.Player.Map == 700 && user.InQualifier() == false))
                {
                    user.SendSysMesage("Spells are not allowed on this area.");

                    return;
                }

                if (!user.Player.Alive && !user.Player.ContainFlag(MsgUpdate.Flags.NeptuneCurse)) return;

                if (user.Player.Map != 1039)//training
                    AttackHandler.CheckAttack.CheckItems.AttackDurability(user, stream);

                if (user.Player.ContainFlag(MsgUpdate.Flags.Freeze))
                {

                    return;
                }

                switch ((AttackID)Attack.AtkType)
                {

                    case AttackID.UpdateHunterJar:
                        {
                            MsgGameItem Jar;
                            if (user.Inventory.TryGetItem(user.DemonExterminator.ItemUID, out Jar))
                            {
                                Attack.UID = user.Player.UID;
                                Attack.SpellLevel = (ushort)user.DemonExterminator.QuestTyp;
                                Attack.ResponseDamage = user.DemonExterminator.HuntKills;

                                user.Send(stream.InteractionCreate(Attack));
                            }
                            break;
                        }
                    case AttackID.InteractionRequest:
                        {

                            Role.IMapObj Target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out Target, Role.MapObjectType.Player))
                            {
                                Role.Player Opponent = Target as Role.Player;
                                if (user.Player.ObjInteraction == null && Opponent.ObjInteraction == null)
                                {
                                    Opponent.ActiveDance = user.Player.ActiveDance = (ushort)Attack.ResponseDamage;
                                    Opponent.Owner.Send(stream.InteractionCreate(Attack));
                                    Opponent.Owner.Send(stream.InteractionCreate(Attack));
                                }
                            }
                            break;
                        }
                    case AttackID.InteractionRefuse:
                        {
                            user.Player.ActiveDance = 0;
                            Role.IMapObj Target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out Target, Role.MapObjectType.Player))
                            {
                                Role.Player Opponent = Target as Role.Player;
                                Opponent.ActiveDance = 0;
                                Opponent.Owner.Send(stream.InteractionCreate(Attack));
                            }
                            break;
                        }
                    case AttackID.InteractionAccept:
                        {
                            Role.IMapObj Target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out Target, Role.MapObjectType.Player))
                            {
                                Role.Player Opponent = Target as Role.Player;
                                if (user.Player.Map == 1858 || Opponent.Map == 1858)
                                {
                                    user.SendSysMesage("You Can`t Make Interaction While Be In Booth or Poker Map");
                                    break;
                                }
                                if (user.Player.ObjInteraction == null && Opponent.ObjInteraction == null)
                                {
                                    Attack.ResponseDamage = user.Player.ActiveDance;
                                    Opponent.Owner.Send(stream.InteractionCreate(Attack));

                                    user.Send(stream.InteractionCreate(Attack));

                                    user.Player.Action = (Role.Flags.ConquerAction)Attack.Damage;
                                    Opponent.Action = (Role.Flags.ConquerAction)Attack.Damage;

                                    user.Player.ObjInteraction = Opponent.Owner;
                                    Opponent.ObjInteraction = user;

                                }
                            }
                            break;
                        }
                    case AttackID.InteractionEffect:
                        {

                            if (user.Player.ObjInteraction != null)
                            {
                                if (user.Player.ObjInteraction.Player.ObjInteraction != null)
                                {
                                    Attack.ResponseDamage = user.Player.ActiveDance;

                                    CreateInteractionEffect(Attack, user);

                                    InteractQuery user_effect = user.Player.InteractionEffect;

                                    user.Player.View.SendView(stream.InteractionCreate(user_effect), true);

                                    Attack.UID = user.Player.ObjInteraction.Player.UID;
                                    Attack.OpponentUID = user.Player.UID;

                                    CreateInteractionEffect(Attack, user.Player.ObjInteraction);
                                    user.Player.Action = Role.Flags.ConquerAction.InteractionHold;
                                    user_effect = user.Player.ObjInteraction.Player.InteractionEffect;
                                    user.Player.ObjInteraction.Player.View.SendView(stream.InteractionCreate(user_effect), true);
                                }
                            }
                            break;
                        }
                    case AttackID.InteractionStopEffect:
                        {

                            Attack.ResponseDamage = user.Player.ActiveDance;
                            user.Player.View.SendView(stream.InteractionCreate(Attack), true);

                            Attack.UID = Attack.OpponentUID;
                            Attack.OpponentUID = user.Player.UID;
                            user.Player.View.SendView(stream.InteractionCreate(Attack), true);

                            if (user.Player.ObjInteraction != null)
                            {
                                user.Player.OnInteractionEffect = false;
                                user.Player.Action = Role.Flags.ConquerAction.None;
                                user.Player.ObjInteraction.Player.OnInteractionEffect = false;
                                user.Player.ObjInteraction.Player.Action = Role.Flags.ConquerAction.None;
                                user.Player.ObjInteraction.Player.ObjInteraction = null;
                                user.Player.ObjInteraction = null;
                            }
                            break;
                        }
                    case AttackID.RequestMarriage:
                        {
                            Role.IMapObj Target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out Target, Role.MapObjectType.Player))
                            {
                                Role.Player Opponent = Target as Role.Player;
                                if (user.Player.Spouse != "None" || Opponent.Spouse != "None")
                                {
#if Arabic
                                       user.SendSysMesage("You cannot marry someone that is already married with someone else!");
#else
                                    user.SendSysMesage("You cannot marry someone that is already married with someone else!");
#endif

                                    break;
                                }
                                if (user.Player.Body % 10 == 7 && Opponent.Body % 10 == 8 || user.Player.Body % 10 == 8 && Opponent.Body % 10 == 7)
                                {
                                    Opponent.Send(stream.RelationCreate(user.Player, Opponent));

                                    Attack.X = Opponent.X;
                                    Attack.Y = Opponent.Y;

                                    Opponent.Send(stream.InteractionCreate(Attack));

                                }
                                else
                                {
#if Arabic
                                      user.SendSysMesage("You cannot marry someone of your gender!");
#else
                                    user.SendSysMesage("You cannot marry someone of your gender!");
#endif

                                }
                            }
                            break;
                        }
                    case AttackID.AcceptMarriage:
                        {
                            Role.IMapObj Target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out Target, Role.MapObjectType.Player))
                            {
                                Role.Player Opponent = Target as Role.Player;
                                if (user.Player.Spouse != "None" || Opponent.Spouse != "None")
                                {
#if Arabic
                                    user.SendSysMesage("You cannot marry someone that is already married with someone else!");
#else
                                    user.SendSysMesage("You cannot marry someone that is already married with someone else!");
#endif

                                    break;
                                }
                                if (user.Player.Body % 10 == 7 && Opponent.Body % 10 == 8 || user.Player.Body % 10 == 8 && Opponent.Body % 10 == 7)
                                {
                                    user.Player.Spouse = Opponent.Name;
                                    user.Player.SpouseUID = Opponent.UID;

                                    Opponent.Spouse = user.Player.Name;
                                    Opponent.SpouseUID = user.Player.UID;

#if Arabic
                                     MsgMessage messaj = new MsgMessage("Joy and happiness! " + user.Player.Name + " and " + Opponent.Name + " have joined together in the holy marriage. We wish them a stone house.", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
#else
                                    MsgMessage messaj = new MsgMessage("Joy and happiness! " + user.Player.Name + " and " + Opponent.Name + " have joined together in the holy marriage. We wish them a stone house.", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
#endif

                                    Server.SendGlobalPacket(messaj.GetArray(stream));

                                    user.Player.SendString(stream, MsgStringPacket.StringID.Spouse, false, new string[1] { user.Player.Spouse });
                                    Opponent.SendString(stream, MsgStringPacket.StringID.Spouse, false, new string[1] { Opponent.Spouse });
                                    user.Player.SendString(stream, MsgStringPacket.StringID.Fireworks, true, new string[1] { "1122" });
                                    //firework-2love
                                    user.Player.SendString(stream, MsgStringPacket.StringID.Effect, true, new string[1] { "firework-2love" });
                                }
                                else
                                {
#if Arabic
                                       user.SendSysMesage("You cannot marry someone of your gender!");
#else
                                    user.SendSysMesage("You cannot marry someone of your gender!");
#endif

                                }
                            }
                            break;
                        }
                    case AttackID.Archer:
                        {
                            #region Archer
                            if (!AttackHandler.CheckAttack.CheckLineSpells.CheckUp(user, Attack.SpellID))
                                break;
                            AttackHandler.Updates.GetWeaponSpell.CheckExtraEffects(user, stream);

                            DateTime Timer = DateTime.Now;
                            if (user.OnAutoAttack)
                            {
                                if (Timer < user.Player.AttackStamp.AddMilliseconds((user.Equipment.AttackSpeed(true)) * 2))
                                    return;
                            }

                            user.Player.AttackStamp = Timer;


                            MsgServer.AttackHandler.CheckAttack.CheckGemEffects.TryngEffect(user);

                            Role.IMapObj target;
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                            {
                                if (!Database.ItemType.IsArcherEpicWeapon(user.Player.RightWeaponId))
                                {
                                    if (!user.nSaveMele)
                                    {
                                        if ((user.Player.ContainFlag(MsgUpdate.Flags.NeptuneCurse) || (int)AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) > (short)2) && !user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings) && (!user.Player.ContainFlag(MsgUpdate.Flags.IronGuard) || (int)AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) > (short)7) && !user.MyArchives.isOpen(Archives.TypeID.Dragonhowl) && !user.MyArchives.isOpen(Archives.TypeID.Bloodlust) && !user.MyArchives.isOpen(Archives.TypeID.Redcurse) && !user.MyArchives.isOpen(Archives.TypeID.StoneCracker) && !user.MyArchives.isOpen(Archives.TypeID.ThornCutter) && !user.MyArchives.isOpen(Archives.TypeID.ColdMoon))
                                        {
                                            return;
                                        }
                                    }
                                }
                                if (AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 18 || user.nSaveMele)
                                {
                                    Role.Player attacked = target as Role.Player;
                                    if (AttackHandler.CheckAttack.CanAttackPlayer.Verified(user, attacked, null, true, Attack))
                                    {
                                        if (!user.nSaveMele)
                                        {
                                            if (attacked.lastJumpTime > Timer)
                                                return;
                                        }
                                        if (!AttackHandler.Updates.GetWeaponSpell.Check(Attack, stream, user, target))
                                        {
                                            if (user.Player.ContainFlag(MsgUpdate.Flags.KineticSpark))
                                            {
                                                if (AttackHandler.Calculate.Base.Rate(30))
                                                {
                                                    Dictionary<ushort, Database.MagicType.Magic> Spells;
                                                    if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.KineticSpark, out Spells))
                                                    {
                                                        Attack.SpellID = (ushort)Role.Flags.SpellID.KineticSpark;
                                                        Database.MagicType.Magic spell;
                                                        if (Spells.TryGetValue(Attack.SpellLevel, out spell))
                                                        {
                                                            AttackHandler.KineticSpark.AttackSpell(user, Attack, stream, Spells);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (user.Player.ContainFlag(MsgUpdate.Flags.ShadowofChaser))
                                            {
                                                MsgSpell clientspell;
                                                if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.ShadowofChaser, out clientspell))
                                                {
                                                    Dictionary<ushort, Database.MagicType.Magic> Spells;
                                                    if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.ShadowofChaser, out Spells))
                                                    {
                                                        Database.MagicType.Magic spell;
                                                        if (Spells.TryGetValue(clientspell.Level, out spell))
                                                        {
                                                            if (AttackHandler.Calculate.Base.Rate(spell.Rate))
                                                            {
                                                                Attack.SpellID = (ushort)Role.Flags.SpellID.ShadowofChaser;
                                                                AttackHandler.KineticSpark.AttackSpell(user, Attack, stream, Spells);
                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            AttackHandler.Calculate.Range.OnPlayer(user.Player, attacked, null, out AnimationObj);

                                            Attack.Damage = (int)AnimationObj.Damage;
                                            Attack.Effect = (uint)AnimationObj.Effect;
                                            user.Player.View.SendView(stream.InteractionCreate(Attack), true);


                                            Attack.AtkType = (ushort)AttackID.Archer;
                                            CreateAutoAtack(Attack, user, true);

                                            AttackHandler.ReceiveAttack.Player.Execute(AnimationObj, user, attacked);

                                        }
                                        else
                                            CreateAutoAtack(Attack, user, true);
                                    }
                                    else
                                        user.OnAutoAttack = false;
                                }
                                else
                                    user.OnAutoAttack = false;

                            }
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Monster))
                            {

                                if (AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 18)
                                {
                                    MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                    if (AttackHandler.CheckAttack.CanAttackMonster.Verified(user, attacked, null))
                                    {
                                        if (!AttackHandler.Updates.GetWeaponSpell.Check(Attack, stream, user, target))
                                        {
                                            if (user.Player.ContainFlag(MsgUpdate.Flags.KineticSpark))
                                            {
                                                if (AttackHandler.Calculate.Base.Rate(30))
                                                {
                                                    Dictionary<ushort, Database.MagicType.Magic> Spells;
                                                    if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.KineticSpark, out Spells))
                                                    {
                                                        Attack.SpellID = (ushort)Role.Flags.SpellID.KineticSpark;
                                                        Database.MagicType.Magic spell;
                                                        if (Spells.TryGetValue(Attack.SpellLevel, out spell))
                                                        {
                                                            AttackHandler.KineticSpark.AttackSpell(user, Attack, stream, Spells);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (user.Player.ContainFlag(MsgUpdate.Flags.ShadowofChaser))
                                            {
                                                MsgSpell clientspell;
                                                if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.ShadowofChaser, out clientspell))
                                                {
                                                    Dictionary<ushort, Database.MagicType.Magic> Spells;
                                                    if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.ShadowofChaser, out Spells))
                                                    {
                                                        Database.MagicType.Magic spell;
                                                        if (Spells.TryGetValue(clientspell.Level, out spell))
                                                        {
                                                            if (AttackHandler.Calculate.Base.Rate(spell.Rate))
                                                            {
                                                                Attack.SpellID = (ushort)Role.Flags.SpellID.ShadowofChaser;
                                                                AttackHandler.KineticSpark.AttackSpell(user, Attack, stream, Spells);
                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            AttackHandler.Calculate.Range.OnMonster(user.Player, attacked, null, out AnimationObj);



                                            Attack.Damage = (int)AnimationObj.Damage;
                                            Attack.Effect = (uint)AnimationObj.Effect;
                                            user.Player.View.SendView(stream.InteractionCreate(Attack), true);

                                            Attack.AtkType = (ushort)AttackID.Archer;
                                            AttackHandler.Updates.IncreaseExperience.Up(stream, user, AttackHandler.ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked));
                                            AttackHandler.Updates.UpdateSpell.CheckUpdate(stream, user, Attack, AnimationObj.Damage, null);
                                            CreateAutoAtack(Attack, user);
                                            AttackHandler.Updates.UpdateSpell.CheckUpdate(stream, user, Attack, AnimationObj.Damage, null);
                                        }
                                        else
                                            CreateAutoAtack(Attack, user);

                                    }
                                    else
                                        user.OnAutoAttack = false;
                                }
                                else
                                    user.OnAutoAttack = false;

                            }
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.SobNpc))
                            {

                                if (AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 18)
                                {
                                    var attacked = target as Role.SobNpc;
                                    if (AttackHandler.CheckAttack.CanAttackNpc.Verified(user, attacked, null))
                                    {
                                        if (!AttackHandler.Updates.GetWeaponSpell.Check(Attack, stream, user, target))
                                        {
                                            if (user.Player.ContainFlag(MsgUpdate.Flags.KineticSpark))
                                            {
                                                if (AttackHandler.Calculate.Base.Rate(30))
                                                {
                                                    Dictionary<ushort, Database.MagicType.Magic> Spells;
                                                    if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.KineticSpark, out Spells))
                                                    {
                                                        Attack.SpellID = (ushort)Role.Flags.SpellID.KineticSpark;
                                                        Database.MagicType.Magic spell;
                                                        if (Spells.TryGetValue(Attack.SpellLevel, out spell))
                                                        {
                                                            AttackHandler.KineticSpark.AttackSpell(user, Attack, stream, Spells);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (user.Player.ContainFlag(MsgUpdate.Flags.ShadowofChaser))
                                            {
                                                MsgSpell clientspell;
                                                if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.ShadowofChaser, out clientspell))
                                                {
                                                    Dictionary<ushort, Database.MagicType.Magic> Spells;
                                                    if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.ShadowofChaser, out Spells))
                                                    {
                                                        Database.MagicType.Magic spell;
                                                        if (Spells.TryGetValue(clientspell.Level, out spell))
                                                        {
                                                            if (AttackHandler.Calculate.Base.Rate(spell.Rate))
                                                            {
                                                                Attack.SpellID = (ushort)Role.Flags.SpellID.ShadowofChaser;
                                                                AttackHandler.KineticSpark.AttackSpell(user, Attack, stream, Spells);
                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            AttackHandler.Calculate.Range.OnNpcs(user.Player, attacked, null, out AnimationObj);

                                            Attack.Damage = (int)AnimationObj.Damage;
                                            Attack.Effect = (uint)AnimationObj.Effect;

                                            user.Player.View.SendView(stream.InteractionCreate(Attack), true);


                                            Attack.AtkType = (ushort)AttackID.Archer;
                                            CreateAutoAtack(Attack, user);
                                            AttackHandler.Updates.IncreaseExperience.Up(stream, user, AttackHandler.ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked));

                                            AttackHandler.Updates.UpdateSpell.CheckUpdate(stream, user, Attack, AnimationObj.Damage, null);

                                        }
                                        else
                                            CreateAutoAtack(Attack, user);
                                    }
                                    else
                                        user.OnAutoAttack = false;
                                }
                                else
                                    user.OnAutoAttack = false;

                            }
                            else
                                user.OnAutoAttack = false;
                            #endregion
                            break;
                        }
                    case AttackID.CounterKillSwitch:
                        {
                            Dictionary<ushort, Database.MagicType.Magic> Spells;
                            if (Pool.Magic.TryGetValue((ushort)Role.Flags.SpellID.CounterKill, out Spells))
                            {
                                MsgSpell ClientSpell;
                                if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.CounterKill, out ClientSpell))
                                {
                                    Database.MagicType.Magic spell;
                                    if (Spells.TryGetValue(ClientSpell.Level, out spell))
                                    {
                                        switch (spell.Type)
                                        {
                                            case Database.MagicType.MagicSort.CounterKill:
                                                {
                                                    Database.MagicType.Magic DBSpell;

                                                    Attack.SpellID = (ushort)Role.Flags.SpellID.CounterKill;
                                                    Attack.SpellLevel = ClientSpell.Level;

                                                    if (AttackHandler.CheckAttack.CanUseSpell.Verified(Attack, user, Spells, out ClientSpell, out DBSpell))
                                                    {
                                                        user.Player.ActivateCounterKill = !user.Player.ActivateCounterKill;
                                                        Attack.Damage = user.Player.ActivateCounterKill ? 1 : 0;
                                                        user.Send(stream.InteractionCreate(Attack));
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case AttackID.Physical:
                        {
                            if (user.Player.ContainFlag(MsgUpdate.Flags.DragonShift))
                                break;
                            DateTime Timer = DateTime.Now;
                            if (!CanAttackPlayer.CanAttack(user, Attack) || !CanAttackNpc.CanAttack(user, Attack))
                                return;
                            if (Timer < user.Player.SpellAttackStamp.AddMilliseconds(user.Equipment.AttackSpeed(true)))
                                return;
                            if (!user.Player.HitInMele)
                            {
                                if ((user.OnAutoAttack || !user.Player.HitInMele) && !user.nSaveMele)
                                {
                                    if (Timer < user.Player.AttackStamp.AddMilliseconds(user.Equipment.AttackSpeed(true)))
                                        return;
                                }
                            }
                            user.Player.AttackStamp = DateTime.Now;

                            AttackHandler.Updates.GetWeaponSpell.CheckExtraEffects(user, stream);

                            Role.IMapObj target;

                            #region Player
                            if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Player))
                            {
                                if (!user.nSaveMele)
                                {
                                    int distance = (int)AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y);
                                    
                                    bool isDistanceValid = distance <= (short)2;
                                    bool hasNeptuneCurse = user.Player.ContainFlag(MsgUpdate.Flags.NeptuneCurse);
                                    bool hasSpreadYourWings = user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings);

                                    bool hasIronGuard = user.Player.ContainFlag(MsgUpdate.Flags.IronGuard);
                                    bool isDragonhowlOpen = user.MyArchives.isOpen(Archives.TypeID.Dragonhowl);
                                    bool isBloodlustOpen = user.MyArchives.isOpen(Archives.TypeID.Bloodlust);
                                    bool isRedcurseOpen = user.MyArchives.isOpen(Archives.TypeID.Redcurse);
                                    bool isStoneCrackerOpen = user.MyArchives.isOpen(Archives.TypeID.StoneCracker);
                                    bool isThornCutterOpen = user.MyArchives.isOpen(Archives.TypeID.ThornCutter);
                                    bool isColdMoonOpen = user.MyArchives.isOpen(Archives.TypeID.ColdMoon);
                                    if ((!isDragonhowlOpen && !isBloodlustOpen && !isRedcurseOpen && !isStoneCrackerOpen && !isThornCutterOpen && !isColdMoonOpen))
                                    {
                                        if (!isDistanceValid
                                          || hasNeptuneCurse && !hasSpreadYourWings && !hasIronGuard)
                                        {
                                            return;
                                        }
                                    }
                                    
                                }
                                if (AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 14
                                    || user.nSaveMele)
                                {


                                    Role.Player attacked = target as Role.Player;


                                    if (AttackHandler.CheckAttack.CanAttackPlayer.Verified(user, attacked, null, true, Attack))
                                    {
                                        if (attacked.ContainFlag(MsgUpdate.Flags.ArmorOfImmunity))
                                            return;
                                        #region BonePulse
                                        if (Attack.SpellID == 0 || Attack.SpellID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.LightningSlashPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.SickleWindPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.WildFireballPassive && user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BonePulse))
                                        {
                                            MsgSpell user_spell = null;
                                            if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.BonePulse, out user_spell) && !user.Player.ContainFlag(MsgUpdate.Flags.BonePulse))
                                            {
                                                Database.MagicType.Magic BonePulse = Pool.Magic[user_spell.ID][user_spell.Level];
                                                if (Role.Core.Rate(BonePulse.Rate))
                                                {
                                                    user.Player.AddSpellFlag(MsgUpdate.Flags.BonePulse, (int)BonePulse.Duration, true);
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var streamm = recycledPacket.GetStream();
                                                        user.Player.SendUpdate(streamm, MsgUpdate.Flags.BonePulse, BonePulse.Duration, 0, 0, MsgUpdate.DataType.ArchiveSkill);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region HittheWaterThreethouSandAttack
                                        if (user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings))
                                        {
                                            if (Attack.Damage == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack1 || Attack.Damage == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack2 || Attack.Damage == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack3)
                                            {
                                                var AttackPaket = new Game.MsgServer.InteractQuery();
                                                AttackPaket.OpponentUID = attacked.UID;
                                                AttackPaket.UID = Attack.UID;
                                                AttackPaket.X = attacked.X;
                                                AttackPaket.Y = attacked.Y;
                                                AttackPaket.SpellID = (ushort)Attack.Damage;
                                                Game.MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);
                                            }
                                        }
                                        #endregion
                                        if (!AttackHandler.Updates.GetWeaponSpell.Check(Attack, stream, user, target))
                                        {

                                            MsgServer.AttackHandler.CheckAttack.CheckGemEffects.TryngEffect(user);

                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            AttackHandler.Calculate.Physical.OnPlayer(user.Player, attacked, null, out AnimationObj);
                                            Attack.Damage = (int)AnimationObj.Damage;
                                            Attack.Effect = (uint)AnimationObj.Effect;
                                            user.Player.View.SendView(stream.InteractionCreate(Attack), true);
                                            Attack.AtkType = (ushort)AttackID.Physical;
                                            AttackHandler.ReceiveAttack.Player.Execute(AnimationObj, user, attacked);

                                        }
                                        if (attacked.Alive)
                                            CreateAutoAtack(Attack, user, true);
                                        else
                                        {

                                            user.OnAutoAttack = false;
                                        }
                                    }
                                    else
                                        user.OnAutoAttack = false;
                                }

                            }
                            #endregion
                            #region Monster
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.Monster))
                            {

                                MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                                if ((user.Player.ContainFlag(MsgUpdate.Flags.NeptuneCurse) || (int)AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) > user.Equipment.GetAttackRange(attacked.SizeAdd)) && !user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings) && !user.Player.ContainFlag(MsgUpdate.Flags.FatalStrike) && !user.MyArchives.isOpen(Archives.TypeID.Dragonhowl) && !user.MyArchives.isOpen(Archives.TypeID.Bloodlust) && !user.MyArchives.isOpen(Archives.TypeID.Redcurse) && !user.MyArchives.isOpen(Archives.TypeID.ColdMoon) && !user.MyArchives.isOpen(Archives.TypeID.StoneCracker) && !user.MyArchives.isOpen(Archives.TypeID.ThornCutter))
                                {

                                    if (user.OnAutoAttack)
                                        user.OnAutoAttack = false;
                                    return;
                                }


                                if (AttackHandler.CheckAttack.CanAttackMonster.Verified(user, attacked, null))
                                {
                                    #region BonePulse
                                    if (Attack.SpellID == 0 || Attack.SpellID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
         || Attack.SpellID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
         || Attack.SpellID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
         || Attack.SpellID == (ushort)Role.Flags.SpellID.LightningSlashPassive
         || Attack.SpellID == (ushort)Role.Flags.SpellID.SickleWindPassive
         || Attack.SpellID == (ushort)Role.Flags.SpellID.WildFireballPassive && user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BonePulse))
                                    {
                                        MsgSpell user_spell = null;
                                        if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.BonePulse, out user_spell) && !user.Player.ContainFlag(MsgUpdate.Flags.BonePulse))
                                        {
                                            Database.MagicType.Magic BonePulse = Pool.Magic[user_spell.ID][user_spell.Level];
                                            if (Role.Core.Rate(BonePulse.Rate))
                                            {
                                                user.Player.AddSpellFlag(MsgUpdate.Flags.BonePulse, (int)BonePulse.Duration, true); using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                {
                                                    var streamm = recycledPacket.GetStream();
                                                    user.Player.SendUpdate(streamm, MsgUpdate.Flags.BonePulse, BonePulse.Duration, 0, 0, MsgUpdate.DataType.ArchiveSkill);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region HittheWaterThreethouSandAttack
                                    if (user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings))
                                    {
                                        user.Shift(attacked.X, attacked.Y, stream, false, true, (ushort)Attack.Damage);
                                        var AttackPaket = new Game.MsgServer.InteractQuery();
                                        AttackPaket.OpponentUID = attacked.UID;
                                        AttackPaket.UID = Attack.UID;
                                        AttackPaket.X = attacked.X;
                                        AttackPaket.Y = attacked.Y;
                                        AttackPaket.SpellID = (ushort)Attack.Damage;
                                        Game.MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);
                                    }
                                    #endregion
                                    if (!AttackHandler.Updates.GetWeaponSpell.Check(Attack, stream, user, target))
                                    {
                                        MsgServer.AttackHandler.CheckAttack.CheckGemEffects.TryngEffect(user);
                                        MsgSpellAnimation.SpellObj AnimationObj;


                                        if (user.Player.ContainFlag(MsgUpdate.Flags.FatalStrike))
                                        {
                                            Attack.AtkType = (ushort)AttackID.FatalStrike;
                                            user.Shift(target.X, target.Y, stream);
                                            AttackHandler.Calculate.Physical.OnMonster(user.Player, attacked, Pool.Magic[(ushort)Role.Flags.SpellID.FatalStrike][0], out AnimationObj);

                                        }
                                        else
                                            AttackHandler.Calculate.Physical.OnMonster(user.Player, attacked, null, out AnimationObj);

                                        Attack.Damage = (int)AnimationObj.Damage;
                                        Attack.Effect = (uint)AnimationObj.Effect;

                                        user.Player.View.SendView(stream.InteractionCreate(Attack), true);

                                        Attack.AtkType = (ushort)AttackID.Physical;
                                        AttackHandler.Updates.IncreaseExperience.Up(stream, user, AttackHandler.ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked));
                                        AttackHandler.Updates.UpdateSpell.CheckUpdate(stream, user, Attack, AnimationObj.Damage, null);

                                    }
                                    if (attacked.Alive)
                                        CreateAutoAtack(Attack, user);
                                }
                                else
                                    user.OnAutoAttack = false;

                            }
                            #endregion
                            #region SobNpc
                            else if (user.Player.View.TryGetValue(Attack.OpponentUID, out target, Role.MapObjectType.SobNpc))
                            {
                                if ((user.Player.ContainFlag(MsgUpdate.Flags.NeptuneCurse) || (int)AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) > 3) && !user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings) && !user.Player.ContainFlag(MsgUpdate.Flags.FatalStrike) && !user.MyArchives.isOpen(Archives.TypeID.Dragonhowl) && !user.MyArchives.isOpen(Archives.TypeID.Bloodlust) && !user.MyArchives.isOpen(Archives.TypeID.Redcurse) && !user.MyArchives.isOpen(Archives.TypeID.ColdMoon) && !user.MyArchives.isOpen(Archives.TypeID.StoneCracker) && !user.MyArchives.isOpen(Archives.TypeID.ThornCutter))
                                {
                                    if (user.OnAutoAttack)
                                        user.OnAutoAttack = false;
                                    return;
                                }



                                if (AttackHandler.Calculate.Base.GetDistance(user.Player.X, user.Player.Y, target.X, target.Y) <= 7 || user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings))
                                {
                                    var attacked = target as Role.SobNpc;
                                    if (AttackHandler.CheckAttack.CanAttackNpc.Verified(user, attacked, null))
                                    {
                                        #region BonePulse
                                        if (Attack.SpellID == 0||Attack.SpellID == (ushort)Role.Flags.SpellID.WaterPrisonPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.WhirlShurikenPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.DustDetachmentPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.LightningSlashPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.SickleWindPassive
             || Attack.SpellID == (ushort)Role.Flags.SpellID.WildFireballPassive && user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BonePulse))
                                        {
                                            MsgSpell user_spell = null;
                                            if (user.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.BonePulse, out user_spell) && !user.Player.ContainFlag(MsgUpdate.Flags.BonePulse))
                                            {
                                                Database.MagicType.Magic BonePulse = Pool.Magic[user_spell.ID][user_spell.Level];
                                                if (Role.Core.Rate(BonePulse.Rate))
                                                {
                                                    user.Player.AddSpellFlag(MsgUpdate.Flags.BonePulse, (int)BonePulse.Duration, true);
                                                    using (var recycledPacket = new ServerSockets.RecycledPacket())
                                                    {
                                                        var streamm = recycledPacket.GetStream();
                                                        user.Player.SendUpdate(streamm, MsgUpdate.Flags.BonePulse, BonePulse.Duration, 0, 0, MsgUpdate.DataType.ArchiveSkill);
                                                    }
                                                }
                                            }
                                        }
#endregion
                                        #region HittheWaterThreethouSandAttack
                                        if (user.Player.ContainFlag(MsgUpdate.Flags.Spreadyourwings))
                                        {
                                            if (Attack.Damage == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack1 || Attack.Damage == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack2 || Attack.Damage == (ushort)Role.Flags.SpellID.HittheWaterThreethouSandAttack3)
                                            {
                                                var AttackPaket = new Game.MsgServer.InteractQuery();
                                                AttackPaket.OpponentUID = attacked.UID;
                                                AttackPaket.UID = Attack.UID;
                                                AttackPaket.X = attacked.X;
                                                AttackPaket.Y = attacked.Y;
                                                AttackPaket.SpellID = (ushort)Attack.Damage;
                                                Game.MsgServer.MsgAttackPacket.ProcescMagic(user, stream, AttackPaket, true);
                                            }
                                        }
                                        #endregion
                                        if (!AttackHandler.Updates.GetWeaponSpell.Check(Attack, stream, user, target))
                                        {

                                            MsgServer.AttackHandler.CheckAttack.CheckGemEffects.TryngEffect(user);

                                            MsgSpellAnimation.SpellObj AnimationObj;
                                            AttackHandler.Calculate.Physical.OnNpcs(user.Player, attacked, null, out AnimationObj);

                                            Attack.Damage = (int)AnimationObj.Damage;
                                            Attack.Effect = (uint)AnimationObj.Effect;
                                            user.Player.View.SendView(stream.InteractionCreate(Attack), true);


                                            Attack.AtkType = (ushort)AttackID.Physical;

                                            AttackHandler.Updates.IncreaseExperience.Up(stream, user, AttackHandler.ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked));
                                            AttackHandler.Updates.UpdateSpell.CheckUpdate(stream, user, Attack, AnimationObj.Damage, null);

                                        }
                                        if (attacked.Alive)
                                            CreateAutoAtack(Attack, user);
                                    }
                                    else
                                        user.OnAutoAttack = false;
                                }

                            }
                            #endregion
                            else
                                user.OnAutoAttack = false;
                            user.Player.HitInMele = false;
                            break;
                        }
                    case AttackID.Magic:
                        {

                            if (Attack.SpellID == (ushort)Role.Flags.SpellID.ArrowBlades && user.Player.ArrowBladesPower <= 0)
                            {
                                user.Player.UpdateArrowBlades(stream, 0);
                                break;
                            }
                            else if (Attack.SpellID == (ushort)Role.Flags.SpellID.ArrowBlades && user.Player.ArrowBladesPower > 0)
                                user.Player.UpdateArrowBlades(stream, -1);

                            if (Attack.SpellID == (ushort)Role.Flags.SpellID.KunpengTrek && user.Player.PengchengMilesCount <= 0)
                            {
                                user.Player.UpdatePengchengMiles(stream, 0);
                                break;
                            }
                            else if (Attack.SpellID == (ushort)Role.Flags.SpellID.KunpengTrek && user.Player.PengchengMilesCount > 0)
                                user.Player.UpdatePengchengMiles(stream, -1);
                            ProcescMagic(user, stream, Attack);
                            break;
                        }
                }

            }

        }
        public static void ProcescMagic(Client.GameClient user, ServerSockets.Packet stream, InteractQuery Attack, bool ignoreStamp = false)
        {
            user.Player.AttackHit = false;
            if (!user.AllowUseSpellOnSteed(Attack.SpellID))
            {
                user.Player.RemoveFlag(MsgUpdate.Flags.Ride);
                return;
            }
            if (!AttackHandler.CheckAttack.CheckLineSpells.CheckUp(user, Attack.SpellID))
            {
                return;
            }
            MsgServer.AttackHandler.CheckAttack.CheckGemEffects.TryngEffect(user);
            #region ActiveWeaponHundredWeapons
            if (Attack.SpellID != (ushort)Role.Flags.SpellID.WeaponCombo)
            {
                if (user.HundredWeapons.Unlocked && user.HundredWeapons.Valid())
                {
                    if (Database.ItemType.IsTrojanEpicWeapon(user.Equipment.RightWeapon)
                           || Database.ItemType.IsScepter(user.Equipment.RightWeapon)
                           || Database.ItemType.IsClub(user.Equipment.RightWeapon)
                           || Database.ItemType.IsTrojanEpicWeapon(user.Equipment.LeftWeapon)
                           || Database.ItemType.IsScepter(user.Equipment.LeftWeapon)
                           || Database.ItemType.IsClub(user.Equipment.LeftWeapon))
                    {
                        if (user.HundredWeapons.TODO_Spells.Count == 0 && !user.Player.ContainFlag(MsgUpdate.Flags.ActiveWeapon))
                        {
                            if (user.HundredWeapons.StageActivated(1) && Role.Core.Rate((byte)user.HundredWeapons.GetWeaponRate(1).FirstOrDefault().Key) && user.HundredWeapons.CanTrigger(4))
                            {
                                user.HundredWeapons.TODO_Spells = new List<ushort>() { user.HundredWeapons.TryGetItem(1).DBSpell.ID };

                                user.Player.AddFlag(MsgUpdate.Flags.ActiveWeapon, 10, true);
                                user.Player.SendUpdate(stream, MsgUpdate.Flags.ActiveWeapon, 0, 0, ((uint)user.HundredWeapons.TryGetItem(1).WeaponSubtype) * 10000, MsgUpdate.DataType.ArchiveSkill, true);
                            }
                            if (user.HundredWeapons.TODO_Spells.Count == 0 && user.Player.ContainFlag(MsgUpdate.Flags.ActiveWeapon))
                            {
                                user.Player.RemoveFlag(MsgUpdate.Flags.ActiveWeapon);
                            }
                        }

                    }
                }
            }
            #endregion

            bool OnTGAutoAttack = true;
            Dictionary<ushort, Database.MagicType.Magic> Spells;
            if (Pool.Magic.TryGetValue(Attack.SpellID, out Spells))
            {
                MsgSpell ClientSpell;
                if (user.MySpells.ClientSpells.TryGetValue(Attack.SpellID, out ClientSpell))
                {

                    Database.MagicType.Magic spell = Pool.Magic[ClientSpell.ID][ClientSpell.Level];

                    if (spell.Passive && user.Player.RandomSpell != Attack.SpellID && Attack.SpellID != (ushort)Role.Flags.SpellID.WrathoftheEmperor && Attack.SpellID != (ushort)Role.Flags.SpellID.StarFlow && Attack.SpellID != (ushort)Role.Flags.SpellID.ArrowBlades && Attack.SpellID != (ushort)Role.Flags.SpellID.BloodTide && Attack.SpellID != (ushort)Role.Flags.SpellID.ArchShadow && Attack.SpellID != (ushort)Role.Flags.SpellID.GapingWounds && Attack.SpellID != (ushort)(Role.Flags.SpellID)22300)
                        return;
                    DateTime Timer = DateTime.Now;
                    if (spell.CoolDown == 0)
                        spell.CoolDown = 500;
                   
                    #region Superpower
                    if (user.Status.Superpower > 0)
                    {
                        MythSoulAttributes.Attribute MythInfo;
                        if (MythSoulAttributes.Attributes[MythSoulAttributes.Type.Superpower].TryGetValue(user.Status.Superpower, out MythInfo))
                        {
                            user.Player.AddFlag(MsgUpdate.Flags.SuperPower, (int)MythInfo.Seconds, true);
                            user.Player.SendUpdate(stream, MsgUpdate.Flags.SuperPower, (uint)MythInfo.Seconds, (uint)MythInfo.Damage, (uint)MythInfo.Level, MsgUpdate.DataType.ArchiveSkill);
                           
                        }
                    }
                    #endregion
                   

                    var spells = user.Player.GetSpellByID(user.Player.spellList, (ushort)spell.ID);
                    if (spells != null)
                    {
                        if (Timer < spells.SpellNow)
                        {
                            return;
                        }
                    }
                    if (ignoreStamp == false && !user.Player.HitInMele)
                    {
                        if (spell.CoolDown > 1000 && spell.CoolDown < 3000)
                            spell.CoolDown = 2500;
                        if (spell.ID == (ushort)Role.Flags.SpellID.MortalWound)
                        {
                            if (Timer < user.Player.AttackStamp.AddMilliseconds(spell.CoolDown))
                            {
                                return;
                            }
                        }
                        else if (spell.ID == (ushort)Role.Flags.SpellID.BladeFlurry)
                        {
                            if (Timer < user.Player.AttackStamp.AddMilliseconds(spell.CoolDown))
                            {
                                return;
                            }
                        }
                        else if (Timer < user.Player.AttackStamp.AddMilliseconds(spell.CoolDown)
                            && spell.ID != (ushort)Role.Flags.SpellID.FastBlader && spell.ID != (ushort)Role.Flags.SpellID.ScrenSword
                            && spell.ID != (ushort)Role.Flags.SpellID.ViperFang && spell.ID != (ushort)Role.Flags.SpellID.EagleEye)
                        {
                            return;
                        }
                        user.Player.AttackStamp = Timer;
                    }

                    if (user.OnAutoAttack == false)
                    {
                        if (user.Equipment.RightWeaponEffect == Role.Flags.ItemEffect.MP)
                        {
                            if (Attack.SpellID == (ushort)Role.Flags.SpellID.EffectMP || AttackHandler.Calculate.Base.Rate(50))
                            {
                                user.Player.RandomSpell = 1175;
                                AttackHandler.EffectMP.Execute(Attack, user, stream, Spells);
                            }
                        }
                        AttackHandler.Updates.GetWeaponSpell.CheckExtraEffects(user, stream);
                    }
                    if (spell.ID == (ushort)Role.Flags.SpellID.EagleEye)
                    {
                        if (DateTime.Now < user.Player.BlackSpotEagle.AddMilliseconds(spell.CoolDown))
                            return;
                    }
                    if (spell.ID == (ushort)Role.Flags.SpellID.GrowFromHurt || spell.ID == (ushort)Role.Flags.SpellID.FatalBlow || spell.ID == (ushort)Role.Flags.SpellID.WeaponCombo)
                    {
                        AttackHandler.SwordAncestorAttack.Execute(user, Attack, stream, Spells);
                        return;
                    }
                    
                    if (Enum.IsDefined(typeof(Role.Flags.SpellIDGolden), spell.ID))
                    {
                        AttackHandler.MonkSkills.Execute(user, Attack, stream, Spells);

                        if (user.Player.Map == 1039 && spell.Type != Database.MagicType.MagicSort.AttachStatus)
                        {
                            CreateAutoAtack(Attack, user);
                        }
                        return;
                    }
                        if (MsgTwistedFututr.CheckSpells(spell.ID))
                        {
                            MsgTwistedFututr.Execute(user, Attack, stream, Spells);
                            if (user.Player.Map == 1039 && spell.Type != Database.MagicType.MagicSort.AttachStatus)
                            {
                                CreateAutoAtack(Attack, user);
                            }
                            return;
                        }
                        if (AtributesStatus.IsDune(user.Player.Class) && AttackHandler.DuneSkills.CheckSpells(spell.ID))
                        {
                            AttackHandler.DuneSkills.Execute(user, Attack, stream, Spells);
                            if (user.Player.Map == 1039 && spell.Type != Database.MagicType.MagicSort.AttachStatus)
                            {
                                CreateAutoAtack(Attack, user);
                            }
                            return;
                        }
                        //if (MsgTwistedFututr.CheckSpells(spell.ID))
                        //{
                        //    MsgTwistedFututr.Execute(user, Attack, stream, Spells);
                        //    if (user.Player.Map == 1039 && spell.Type != Database.MagicType.MagicSort.AttachStatus)
                        //    {
                        //        CreateAutoAtack(Attack, user);
                        //    }
                        //    return;
                        //}
                        if (MsgYuanshen.CheckSpells(spell.ID))
                        {
                            YuanshenAttack.Execute(user, Attack, stream, Spells);
                            if (user.Player.Map == 1039 && spell.Type != Database.MagicType.MagicSort.AttachStatus)
                            {
                                CreateAutoAtack(Attack, user);
                            }
                            return;

                        }
                        if (Enum.IsDefined(typeof(Role.Flags.SpellIDPirate), spell.ID))
                    {
                        AttackHandler.PirateSkills.Execute(user, Attack, stream, Spells);

                        if (user.Player.Map == 1039 && spell.Type != Database.MagicType.MagicSort.AttachStatus)
                        {
                            CreateAutoAtack(Attack, user);
                        }
                        return;
                    }
                    //user.SendSysMesage("SpellID  [" + Attack.SpellID + "] && Type [" + spell.Type + "]&& Name [" + spell.Name + "]", MsgMessage.ChatMode.System);
                    switch (spell.Type)
                    {
                        case Database.MagicType.MagicSort.SupremeLeadership: AttackHandler.SupremeLeadership.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ChaoticDance: AttackHandler.ChaoticDance.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ChaoticDanceAttack: AttackHandler.ChaoticDanceAttack.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.SuanniCommand: AttackHandler.SuanniCommand.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ArrowBlades: AttackHandler.ArrowBlades.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.CrackShot: AttackHandler.CrackShot.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Type: AttackHandler.Hunter.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.CounterKill: AttackHandler.CounterKill.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.HellVortex: AttackHandler.HellVortex.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.TripleAttack: AttackHandler.TripleAttack.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.PetAttachStatus: AttackHandler.PetAttachStatus.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.RemoveStamin: AttackHandler.RemoveStamin.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.SwirlingStorm: AttackHandler.SwirlingStorm.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Omnipotence: AttackHandler.Omnipotence.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.BurntFrost: AttackHandler.BurntFrost.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Rectangle: AttackHandler.Rectangle.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.AddMana: AttackHandler.AddMana.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Strike: AttackHandler.Strike.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.AirKick: AttackHandler.AirKick.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.DragonCyclone: AttackHandler.DragonCyclone.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.FatalCross: AttackHandler.FatalCross.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.SectorBack: AttackHandler.SectorBack.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.BladeFlurry: AttackHandler.BladeFlurry.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.KineticSpark: AttackHandler.KineticSpark.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.MortalDrag: AttackHandler.MortalDrag.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ChargingVortex: AttackHandler.ChargingVortex.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.PirateXpSkill: AttackHandler.PirateXpSkill.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.AddBlackSpot: AttackHandler.AddBlackSpot.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.MoveLine: AttackHandler.MoveLine.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.BombLine: AttackHandler.BombLine.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.BlackSpot: AttackHandler.BlackSpot.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.CannonBarrage: AttackHandler.CannonBarrage.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ScurvyBomb: AttackHandler.ScurvyBomb.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.PhysicalSpells: AttackHandler.PhysicalSpells.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.WhirlwindKick: AttackHandler.WhirlwindKick.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Oblivion: AttackHandler.Oblivion.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.DispatchXp: AttackHandler.DispatchXp.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ShieldBlock: AttackHandler.ShieldBlock.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Compasion:
                        case Database.MagicType.MagicSort.Tranquility:
                        case Database.MagicType.MagicSort.RemoveBuffers:
                            {

                                AttackHandler.RemoveBuffers.Execute(user, Attack, stream, Spells);
                                break;
                            }
                        case Database.MagicType.MagicSort.Perimeter: AttackHandler.Perimeter.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Auras: AttackHandler.Auras.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.DirectAttack: AttackHandler.DirectAttack.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.DragonWhirl: AttackHandler.DragonWhirl.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.StarArrow: AttackHandler.StarArrow.ExecuteExecute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ChainBolt: AttackHandler.ChainBolt.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Spook: AttackHandler.Spook.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.WarCry: AttackHandler.WarCry.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Riding: AttackHandler.Riding.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ShurikenVortex: AttackHandler.ShurikenVortex.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Toxic: AttackHandler.Toxic.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.DecLife: AttackHandler.DecLife.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.LayTrap: AttackHandler.LayTrap.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Transform: AttackHandler.Transform.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.AttackStatus: AttackHandler.AttackStatus.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Collide: AttackHandler.Collide.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Sector: AttackHandler.Sector.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Line:
                        case (Database.MagicType.MagicSort)96: AttackHandler.Line.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Attack: AttackHandler.Attack.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.AttachStatus:
                            {
                                OnTGAutoAttack = false;
                                AttackHandler.AttachStatus.Execute(user, Attack, stream, Spells); break;
                            }
                        case Database.MagicType.MagicSort.DetachStatus: AttackHandler.DetachStatus.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Recruit: AttackHandler.Recruit.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Bomb: AttackHandler.Bomb.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.FatalSpin: AttackHandler.FatalSpin.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.CallPet: AttackHandler.CallPet.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.SectorPasive: AttackHandler.SectorPasive.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.StraightFist: AttackHandler.StraightFist.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ManiacDance: AttackHandler.ManiacDance.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Pounce: AttackHandler.Pounce.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Pitching: AttackHandler.Pitching.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Wildwind: AttackHandler.Wildwind.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.ThunderRampage: AttackHandler.ThunderRampage.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.HeavensWrath: AttackHandler.HeavensWrath.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Shock: AttackHandler.Shock.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.Kunpeng: AttackHandler.FiveStarLianzhuWater.Execute(user, Attack, stream, Spells); break;
                        case Database.MagicType.MagicSort.BeastControl: AttackHandler.BeastControl.Execute(user, Attack, stream, Spells); break;

                    }
                    if (spell.ID == 10415)
                        spell.CoolDown = 1000;
                    var UpdateSpell = user.Player.spellList.FirstOrDefault(s => s.SpellID == (ushort)spell.ID);

                    if (UpdateSpell != null)
                    {
                        UpdateSpell.SpellNow = DateTime.Now.AddMilliseconds(spell.CoolDown);
                    }
                    else
                    {
                        user.Player.spellList.Add(new VirusX.Role.Player.SpellData
                        {
                            SpellNow = DateTime.Now.AddMilliseconds(spell.CoolDown),
                            SpellID = (ushort)spell.ID
                        });
                    }
                    if (Database.AtributesStatus.IsTrojan(user.Player.Class) && !user.Player.AttackHit)
                    {
                        if (user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.BreathFocus))
                        {
                            if (!(Attack.SpellID == 9876 || Attack.SpellID == 6002 || Attack.SpellID == 10315 || Attack.SpellID == 10311 || Attack.SpellID == 10313 ||
                    Attack.SpellID == 6003 || Attack.SpellID == 10405 || Attack.SpellID == 30000 || Attack.SpellID == 10310 || Attack.SpellID == 3050 ||
                    Attack.SpellID == 3060 || Attack.SpellID == 3080 || Attack.SpellID == 3090 || Attack.SpellID == 12830))
                            {
                                Attack.SpellID = (ushort)Role.Flags.SpellID.BreathFocus;
                                if (Pool.Magic.TryGetValue(Attack.SpellID, out Spells))
                                {
                                    AttackHandler.BreathFocus.Execute(Attack, user, stream, Spells);
                                }
                                if (user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.ApePistol))
                                {
                                    user.Player.AddSpellFlag(MsgUpdate.Flags.ApePistol, (int)spell.Duration, true);
                                    MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                          , 0, user.Player.X, user.Player.Y, spell.ID
                                          , spell.Level, 0);
                                    MsgSpell.SetStream(stream);
                                    MsgSpell.Send(user);
                                    user.Player.SendUpdateSlayer(stream, MsgUpdate.DataType.ApePistol, (uint)spell.Duration, false);
                                }
                            }
                        }
                    }

                }
                
            }
            if (user.Player.Map == 1039 && OnTGAutoAttack)
            {
                CreateAutoAtack(Attack, user);
            }
           
        }
        public static void HitInMele(Role.Player player, Database.MagicType.Magic DBSpell)
        {
            if (DBSpell != null)
            {
                if (!DBSpell.Passive)
                {
                    ushort[] SpellMele = new ushort[] { 17650, 17460, 17470, 17480, 17490, 17500, 17510, 17520, 17530, 17540, 17550, 17560, 17570, 17400, 17401, 18220, 11950, 11951, 11952, 18670, 17392, 17391, 22630,16470, 16490, 16510, 16530,16430, 16410, 21670, 21680, 21690, 21700, 22070, 22080, 22090, 22100, 22110 };
                    if (!SpellMele.Contains(DBSpell.ID))
                    {
                        player.HitInMele = true;
                    }
                }
            }
        }
        public static void CreateAutoAtack(InteractQuery pQuery, Client.GameClient client, bool mele = false)
        {
            client.OnAutoAttack = true;
            client.AutoAttack = new InteractQuery();
            client.AutoAttack.AtkType = pQuery.AtkType;
            client.AutoAttack.Damage = pQuery.Damage;
            client.AutoAttack.Effect = pQuery.Effect;
            client.AutoAttack.OpponentUID = pQuery.OpponentUID;
            client.AutoAttack.ResponseDamage = pQuery.ResponseDamage;
            client.AutoAttack.SpellID = pQuery.SpellID;
            client.AutoAttack.SpellLevel = pQuery.SpellLevel;
            client.AutoAttack.UID = pQuery.UID;
            client.AutoAttack.X = pQuery.X;
            client.AutoAttack.Y = pQuery.Y;

            #region Player
            if (mele)
            {
                Role.IMapObj target;
                if (client.Player.View.TryGetValue(pQuery.OpponentUID, out target, Role.MapObjectType.Player))
                {
                    Role.Player attacked = target as Role.Player;
                    attacked.Owner.SaveMele = client;
                }
            }
            #endregion
        }
        public static void CreateInteractionEffect(InteractQuery pQuery, Client.GameClient client)
        {
            client.Player.OnInteractionEffect = true;
            client.Player.InteractionEffect = new InteractQuery();
            client.Player.InteractionEffect.AtkType = pQuery.AtkType;
            client.Player.InteractionEffect.Damage = pQuery.Damage;
            client.Player.InteractionEffect.Effect = pQuery.Effect;
            client.Player.InteractionEffect.OpponentUID = pQuery.OpponentUID;
            client.Player.InteractionEffect.ResponseDamage = pQuery.ResponseDamage;
            client.Player.InteractionEffect.SpellID = pQuery.SpellID;
            client.Player.InteractionEffect.SpellLevel = pQuery.SpellLevel;
            client.Player.InteractionEffect.UID = pQuery.UID;
            client.Player.InteractionEffect.X = pQuery.X;
            client.Player.InteractionEffect.Y = pQuery.Y;
        }
        
    }
}