using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using VirusX.MsgInterServer.Packets;
using ProtoBuf;
using VirusX.DBFunctionality;
namespace VirusX.Game.MsgServer
{
    using System.IO;
    using ActionInvoker = CachedAttributeInvocation<ProcessAction, MsgDataPacket.DataAttribute, ushort>;
    using VirusX.Game.MsgFloorItem;
    using VirusX.Game.MsgNpc;
    using VirusX.Database;
    using VirusX.Role.Instance;
    using System.Runtime.InteropServices.ComTypes;

    public unsafe delegate void ProcessAction(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data);

    public class CustomCommands
    {
        public const ushort
            ExitQuestion = 1,
            Minimize = 2,
            ShowReviveButton = 1053,
            FlowerPointer = 1067,
            Enchant = 1091,
            LoginScreen = 1153,
            SelectRecipiet = 30,
            JoinGuild = 34,
            MakeFriend = 38,
            ChatWhisper = 40,
            CloseClient = 43,
            HotKey = 53,
            Furniture = 54,
            TQForum = 79,
            PathFind = 97,
            LockItem = 102,
            ShowRevive = 1053,
            HideRevive = 1054,
            StatueMaker = 1066,
            GambleOpen = 1077,
            GambleClose = 1078,
            Compose = 1086,
            Craft1 = 1088,
            Craft2 = 1089,
            Warehouse = 1090,
            ShoppingMallShow = 1100,
            ShoppingMallHide = 1101,
            NoOfflineTraining = 1117,
            Interact = 1122,
            CenterClient = 1155,
            ClaimCP = 1197,
            ClaimAmount = 1198,
            MerchantApply = 1201,
            MerchantDone = 1202,
            RedeemEquipment = 1233,
            ClaimPrize = 1234,
            RepairAll = 1239,
            FlowerIcon = 1244,
            SendFlower = 1246,
            ReciveFlower = 1248,
            WarehouseVIP = 1272,
            UseExpBall = 1288,
            HackProtection = 1298,
            HideGUI = 1307,
            Inscribe = 3059,
            BuyPrayStone = 3069,
            HonorStore = 3104,
            Opponent = 3107,
            CountDownQualifier = 3109,
            QualifierStart = 3111,
            ItemsReturnedShow = 3117,
            ItemsReturnedWindow = 3118,
            ItemsReturnedHide = 3119,
            QuestFinished = 3147,
            QuestPoint = 3148,
            QuestPointSparkle = 3164,
            StudyPointsUp = 3192,
            Updates = 3218,
            IncreaseLineage = 3227,
            HorseRacingStore = 3245,
            GuildPKTourny = 3249,
            QuitPK = 3251,
            Spectators = 3252,
            CardPlayOpen = 3270,
            CardPlayClost = 3271,
            ArtifactPurification = 3344,
            SafeguardConvoyShow = 3389,
            SafeguardConvoyHide = 3390,
            RefineryStabilization = 3392,
            ArtifactStabilization = 3398,
            SmallChat = 3406,
            NormalChat = 3407,
            Reincarnation = 3439;
    }
    public class DialogCommands
    {
        public const ushort
            Compose = 1,
            Craft = 2,
            Warehouse = 4,
            DetainRedeem = 336,
            DetainClaim = 337,
            VIPWarehouse = 341,
            Breeding = 368,
            PurificationWindow = 455,
            StabilizationRifinery = 448,
            StabilizationWindow = 459,
            TalismanUpgrade = 347,
            GemComposing = 422,
            OpenSockets = 425,
            Blessing = 426,
            TortoiseGemComposing = 438,
            RefineryStabilization = 448,

            HorseRacingStore = 464,
            Reincarnation = 485,
            ChangeName = 489,
            GarmentShop = 502,
            DegradeEquip = 506,
            Browse = 572,
            JiangHuSetName = 617,
            SendTwinCityTime = 538,
            BrowseAuction = 572,
            HowGetStudy = 595,
            TheFactionWar = 599, //. 1072 packet
            ResetSecundaryPassword = 639,
            NewLottery = 656, // Packet . 2804
            CreateUnion = 693,
            SetKingdomTitle = 700,
            Hairstyle = 741,
            ChangeNameUnion = 723,
            RuneDecompose = 904,
            EmbedRune = 930,
            TrojanWeaponArchiveLook = 978,
              GouYuArchiveArt = 1209,
            TrojanWeaponArchive = 980;

    }
    public class ActionType
    {
        public const ushort RemoveTrap = 434,
        UpdateSpell = 252,
        UpdateProf = 253,
        DragonBall = 165,
        OpenGuiNpc = 160,
        AutoPatcher = 162,
        Description = 181,
        CountDown = 159,
        HideGui = 158,
        ChangeLookface = 151,
        SetLocation = 74,
        Hotkeys = 75,
        ConfirmAssociates = 76,
        NewChatTalking = 456,
        ConfirmProficiencies = 77,
        ConfirmSpells = 78,
        ChangeDirection = 79,
        ChangeStance = 81,
        ChangeMap = 85,
        AnimaCrest = 258,
        Teleport = 86,
        Leveled = 92,
        Revive = 94,
        DeleteCharacter = 95,
        SetPkMode = 96,
        ConfirmGuild = 97,
        LocationTeamLieder = 101,
        RequestEntity = 102,
        SetMapColor = 104,
        TeamSearchForMember = 106,
        RemoveSpell = 109,
        StartVendor = 111,
        StopVending = 114,
        OpenCustom = 116,
        ViewEquipment = 117,
        EndTransformation = 118,
        EndFly = 120,
        ViewEnemyInfo = 123,
        OpenDialog = 126,
        CompleteLogin = 132,
        ReviveMonster = 134,
        RemoveEntity = 135,
        Jump = 137,
        Ghost = 145,
        ViewFriendInfo = 148,
        ChangeFace = 151,
        TradePartnerInfo = 152,
        AbortMagic = 163,
        Bulletin = 166,
        PokerTeleporter = 167,
        FlashStep = 156,
        Away = 161,
        Pick = 164,
        ClikerON = 171,
        ClikerEntry = 172,
        SetAppearanceType = 178,
        PoketTableBet = 234,
        AllowAnimation = 251,
        CreditGifts = 255,
        UpdateInventorySash = 256,
        QuerySpawn = 310,
        QueryEquipment = 408,
        BeginSteedRace = 401,
        FinishSteedRace = 402,
        SubmitGoldBrick = 436,
        AddBlackList = 440,
        RemoveBlackList = 441,
        DrawStory = 443,
        PetAttack = 447,
        DuelAction = 449,
        PortalInvite = 451,
        PortalConfirm = 452,
        SaizoNpc = 456,
        InspiredPirate = 457,
        ViewNinjaInspired = 461,
        GiveOrder = 466,
        ReturnReward = 470,
        ExchangeClan = 479,
        ShowFriend = 482,
        ShowPartner = 483,
        GetPrizeFlower = 485,
         DunePower = 521;

    }
    [ProtoContract]
    public class ActionQuery
    {
        [ProtoMember(1, IsRequired = true)]
        public uint ObjId;
        [ProtoMember(2)]
        public uint TargetUID;
        [ProtoMember(3)]
        public uint dwParam;
        [ProtoMember(4)]
        public int dwParam2;
        [ProtoMember(5)]
        public long dwParam3;
        [ProtoMember(6)]
        public bool SucDone;
        [ProtoMember(7)]
        public uint TargetPositionX;
        [ProtoMember(8)]
        public uint TargetPositionY;
        [ProtoMember(9, IsRequired = true)]
        public uint Timestamp;
        [ProtoMember(10)]
        public int NpcID;
        [ProtoMember(11)]
        public uint CheatLogData1;
        [ProtoMember(12, IsRequired = true)]
        public ushort Type;
        [ProtoMember(13, IsRequired = true)]
        public ushort Fascing;
        [ProtoMember(14, IsRequired = true)]
        public uint PositionX;
        [ProtoMember(15, IsRequired = true)]
        public uint PositionY;
        [ProtoMember(16)]
        public uint CheatLogData2;
        [ProtoMember(17)]
        public uint MapID;
        [ProtoMember(18)]
        public uint CheatLogData3;
        [ProtoMember(19)]
        public int ScheduleTime;
        [ProtoMember(20)]
        public int LinkIndex;
        [ProtoMember(21)]
        public uint CheatLogData4;
        [ProtoMember(22)]
        public int RemainTime;
        [ProtoMember(23)]
        public bool MountCharge;
        [ProtoMember(24)]
        public string[] Strings;
        
    }
    public unsafe static class MsgDataPacket
    {
        public static unsafe ServerSockets.Packet ActionPick(this ServerSockets.Packet stream, uint UID, ushort dwparam2, ushort timer, string text)
        {
            return ActionCreate(stream, new ActionQuery() { ObjId = UID, dwParam = 220, Type = ActionType.Pick, Timestamp = (uint)Time32.timeGetTime().GetHashCode(), Fascing = dwparam2, RemainTime = timer, Strings = new string[1] { text } });
        }

        public static unsafe void Action(this ServerSockets.Packet stream, out ActionQuery pQuery)
        {
            pQuery = new ActionQuery();
            pQuery = stream.ProtoBufferDeserialize<ActionQuery>(pQuery);
        }
        public static unsafe ServerSockets.Packet ActionCreate(this ServerSockets.Packet stream, ActionQuery pQuery)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgAction);
            return stream;
        }
        public class DataAttribute : Attribute
        {
            public static readonly Func<DataAttribute, ushort> Translator = (a) => a.Type;
            public ushort Type { get; private set; }
            public DataAttribute(ushort type)
            {
                this.Type = type;
            }
        }

        public static ActionInvoker invoker = new ActionInvoker(DataAttribute.Translator);

        [PacketAttribute(GamePackets.MsgAction)]
        private unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            try
            {

                ActionQuery data;
                stream.Action(out data);
                Tuple<DataAttribute, ProcessAction> processFolded;
                if (invoker.TryGetInvoker(data.Type, out processFolded))
                {
                    if (data.Type != 137)
                    { }
                    processFolded.Item2(user, stream, data);
                }
                else
                {


                }
            }
            catch (Exception e) { MyConsole.WriteException(e); }
        }
        [DataAttribute(ActionType.SubmitGoldBrick)]
        public unsafe static void SubmitGoldBrick(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.InUnion)
            {
               
            }
        }
        [DataAttribute(ActionType.Bulletin)]
        public static unsafe void Bulletin(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (!client.Player.Alive)
                return;
            if (client.Player.Map == 1017 || client.Player.Map == 1081 || client.Player.Map == 2060 || client.Player.Map == 9972
                  || client.Player.Map == 1080 || client.Player.Map == 3820 || client.Player.Map == 3954
              || client.Player.Map == 1806
                  || Game.MsgTournaments.MsgSchedules.DisCity.IsInDisCity(client.Player.Map) || client.Player.Map == 1508
                   || Game.MsgTournaments.MsgSchedules.SteedRace.InSteedRace(client.Player.Map)
          || client.Player.Map == 1768

          || client.Player.Map == 3053 || client.Player.Map == 1505 || client.Player.Map == 1506 || client.Player.Map == 12022 || client.Player.Map == 1508 || client.Player.Map == 1507)
            {
                return;
            }

            if (client.Player.Map == 1038 || client.Player.Map == MsgTournaments.MsgClassPKWar.MapID || client.Player.DynamicID != 0
                || client.Player.Map == 6001)
            {
                return;
            }
            if (client.Player.Map == 10137 || client.Player.Map == 10250)
            {
                client.SendSysMesage("Sorry, you can`t get teleported on this map");
                return;
            }
            if (Pool.Constants.BlockTeleportMap.Contains(client.Player.Map))
                return;
            switch (data.dwParam)
            {
                case 8:
                    {
                        client.Teleport(275, 484, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 1:
                    {
                        client.Teleport(392, 472, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 15:
                    {
                        client.Teleport(317, 318, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 38:
                    {
                        client.Teleport(260, 442, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 91:
                    {
                        client.Teleport(395, 344, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 68:
                    {
                        client.Teleport(275, 467, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 104:
                    {
                        client.Teleport(246, 469, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 25:
                    {
                        if (client.Player.Reborn == 2)
                        {
                            client.Teleport(453, 480, 10137);
                            client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        }
                        else client.SendSysMesage("You should reach the required level of this event to enable the teleport function.", MsgMessage.ChatMode.TopLeft, MsgMessage.MsgColor.red, false, true);
                        break;
                    }
                case 98:
                    {
                        client.Teleport(208, 373, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                case 54:
                case 14:
                case 37:
                    {
                        client.Teleport(422, 279, 1002);
                        client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "movego");
                        break;
                    }
                default:
                    {
                        if (client.ProjectManager == true)
                            client.SendSysMesage("Unkown case ID ." + data.dwParam, MsgMessage.ChatMode.Talk, MsgServer.MsgMessage.MsgColor.red);
                        break;
                    }
            }
        }
        [DataAttribute(ActionType.RemoveBlackList)]
        public unsafe static void RemoveBlackList(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            string TargetName = data.Strings.FirstOrDefault();
            foreach (var user in Pool.GamePoll.Values)
            {
                if (user.Player.Name == TargetName)
                {
                    data.ObjId = user.Player.UID;
                    break;
                }
            }
            data.dwParam = 1;
            data.Strings = new string[1] { TargetName };
            data.ObjId = client.Player.UID;
            client.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.AddBlackList)]
        public unsafe static void AddBlackList(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            string TargetName = data.Strings.FirstOrDefault();
            foreach (var user in Pool.GamePoll.Values)
            {
                if (user.Player.Name == TargetName)
                {
                    data.ObjId = user.Player.UID;
                    break;
                }
            }
            data.dwParam = 1;
            data.Strings = new string[1] { TargetName };
            data.ObjId = client.Player.UID;
            client.Send(msg.ActionCreate(data));

        }
        [DataAttribute(ActionType.PortalInvite)]
        public unsafe static void PortalInvite(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (Pool.GamePoll.ContainsKey(data.dwParam))
            {
                var owner = Pool.GamePoll[data.dwParam];
                if (owner.Player.FloorSpells.ContainsKey((ushort)Role.Flags.SpellID.SpaceLeap) && owner.Player.FloorSpells[(ushort)Role.Flags.SpellID.SpaceLeap].Spells.Count > 0)
                {
                    
                    var floor = owner.Player.FloorSpells[(ushort)Role.Flags.SpellID.SpaceLeap].Spells.FirstOrDefault();
                    ushort X = floor.FloorPacket.m_X, Y = floor.FloorPacket.m_Y;
                    Pool.ServerMaps[floor.FloorPacket.MapID].GetRandCoord(ref X, ref Y, 7);
                    client.Teleport(X, Y, floor.FloorPacket.MapID);

                    owner.Send(msg.ActionCreate(new ActionQuery() { TargetUID = owner.Player.UID, Type = ActionType.PortalConfirm, ObjId = client.Player.UID, Timestamp = (uint)Time32.timeGetTime().GetHashCode() }));
                }
            }

        }
        [DataAttribute(ActionType.PetAttack)]
        public unsafe static void PetAttack(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            foreach (var obj in client.Player.View.Roles(Role.MapObjectType.Monster))
            {
                var pet = obj as Game.MsgMonster.MonsterRole;
                if (pet.UID == data.NpcID)
                {
                    if (DateTime.Now >= pet.AttackSpeed)
                    {
                        uint Experience = 0;
                        Role.IMapObj target;
                        if (client.Player.View.TryGetValue(data.TargetUID, out target, Role.MapObjectType.Monster))
                        {
                            var DBSpell = Pool.Magic[(ushort)Role.Flags.SpellID.ThundercloudAttack][0];
                            MsgMonster.MonsterRole attacked = target as MsgMonster.MonsterRole;
                            if (MsgServer.AttackHandler.CheckAttack.CanAttackMonster.Verified(client, attacked, DBSpell))
                            {

                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(pet.UID
                                             , 0, attacked.X, attacked.Y, DBSpell.ID
                                             , DBSpell.Level, 0);


                                MsgSpellAnimation.SpellObj AnimationObj;
                                MsgServer.AttackHandler.Calculate.Range.OnMonster(client.Player, attacked, null, out AnimationObj
                                    , (byte)(pet.ContainFlag(MsgUpdate.Flags.Thunderbolt) ? 2 : 0));
                                AnimationObj.Damage = (uint)(AnimationObj.Damage * Pool.Magic[(ushort)Role.Flags.SpellID.Thundercloud][client.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.Thundercloud].Level].Damage2 / 100);
                                AnimationObj.Damage = MsgServer.AttackHandler.Calculate.Base.CalculateSoul(AnimationObj.Damage, 0);
                                Experience += MsgServer.AttackHandler.ReceiveAttack.Monster.Execute(msg, AnimationObj, client, attacked);

                                MsgSpell.Targets.Enqueue(AnimationObj);
                                MsgSpell.SetStream(msg);
                                MsgSpell.Send(client);
                                pet.AttackSpeed = DateTime.Now.AddMilliseconds(pet.Family.AttackSpeed);
                            }

                        }
                        else if (client.Player.View.TryGetValue(data.TargetUID, out target, Role.MapObjectType.Player))
                        {
                            var DBSpell = Pool.Magic[(ushort)Role.Flags.SpellID.ThundercloudAttack][0];
                            var attacked = target as Role.Player;
                            if (MsgServer.AttackHandler.CheckAttack.CanAttackPlayer.Verified(client, attacked, DBSpell))
                            {
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(pet.UID
                                            , 0, attacked.X, attacked.Y, DBSpell.ID
                                            , DBSpell.Level, 0);
                                int extra = DBSpell.Damage2;
                                MsgSpell myspell;
                                if (pet.ContainFlag(MsgUpdate.Flags.Thunderbolt) && client.MySpells.ClientSpells.TryGetValue((ushort)Role.Flags.SpellID.Thunderbolt, out myspell))
                                    extra = 10 * (myspell.Level + 1);

                                MsgSpellAnimation.SpellObj AnimationObj;
                                MsgServer.AttackHandler.Calculate.Physical.OnPlayer(client.Player, attacked, DBSpell, out AnimationObj);
                                AnimationObj.Damage = (uint)(AnimationObj.Damage * Pool.Magic[(ushort)Role.Flags.SpellID.Thundercloud][client.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.Thundercloud].Level].Damage2 / 100);
                                AnimationObj.Damage = MsgServer.AttackHandler.Calculate.Base.CalculateSoul(AnimationObj.Damage, 0);
                                MsgServer.AttackHandler.ReceiveAttack.Player.Execute(AnimationObj, client, attacked);
                                MsgSpell.Targets.Enqueue(AnimationObj);
                                MsgSpell.SetStream(msg);
                                MsgSpell.Send(client);
                                pet.AttackSpeed = DateTime.Now.AddMilliseconds(pet.Family.AttackSpeed);
                            }
                        }
                        else if (client.Player.View.TryGetValue(data.TargetUID, out target, Role.MapObjectType.SobNpc))
                        {
                            var DBSpell = Pool.Magic[(ushort)Role.Flags.SpellID.ThundercloudAttack][0];
                            var attacked = target as Role.SobNpc;
                            if (MsgServer.AttackHandler.CheckAttack.CanAttackNpc.Verified(client, attacked, DBSpell))
                            {
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(pet.UID
                                         , 0, attacked.X, attacked.Y, DBSpell.ID
                                         , DBSpell.Level, 0);

                                MsgSpellAnimation.SpellObj AnimationObj;
                                MsgServer.AttackHandler.Calculate.Range.OnNpcs(client.Player, attacked, null, out AnimationObj);
                                AnimationObj.Damage = (uint)(AnimationObj.Damage * Pool.Magic[(ushort)Role.Flags.SpellID.Thundercloud][client.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.Thundercloud].Level].Damage2 / 100);
                                Experience += MsgServer.AttackHandler.ReceiveAttack.Npc.Execute(msg, AnimationObj, client, attacked);
                                MsgSpell.Targets.Enqueue(AnimationObj);
                                MsgSpell.SetStream(msg);
                                MsgSpell.Send(client);
                                pet.AttackSpeed = DateTime.Now.AddMilliseconds(pet.Family.AttackSpeed);
                            }
                        }
                        MsgServer.AttackHandler.Updates.IncreaseExperience.Up(msg, client, Experience);
                    }
                }
            }
        }

        [DataAttribute(ActionType.ClikerEntry)]
        public unsafe static void ClikerEntry(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            //dwparam target.
            if (client.Player.QuestGUI.CheckQuest(6131, MsgQuestList.QuestListItem.QuestStatus.Accepted))
            {
                Role.IMapObj target;
                if (client.Player.View.TryGetValue(data.dwParam, out target, Role.MapObjectType.Monster))
                {
                    var monster = target as Game.MsgMonster.MonsterRole;

                    if (monster.Name.Contains("BanditL98"))
                    {
                        client.Player.QuestGUI.IncreaseQuestObjectives(msg, 6131, 0, 1);
                        client.Player.QuestGUI.IncreaseQuestObjectives(msg, 6131, 1, 1);
                    }

                    monster.HitPoints = 0;
                    monster.Dead(msg, client, client.Player.UID, client.Map);
                }
                data.ObjId = data.dwParam;
                data.dwParam = 0;
                data.Type = 174;
                client.Send(msg.ActionCreate(data));
            }
            else if (client.Player.QuestGUI.CheckQuest(1355, MsgQuestList.QuestListItem.QuestStatus.Accepted))
            {
                Role.IMapObj target;
                if (client.Player.View.TryGetValue(data.dwParam, out target, Role.MapObjectType.Monster))
                {
                    var monster = target as Game.MsgMonster.MonsterRole;
#if Arabic
                     if (monster.Name.Contains("Thunder"))
                    {
                        client.SendSysMesage("Now you can report back to Poison Master.", MsgMessage.ChatMode.System);
                        client.Player.QuestGUI.IncreaseQuestObjectives(msg, 1355, 1);
                    }
#else
                    if (monster.Name.Contains("Thunder"))
                    {
                        client.SendSysMesage("Now you can report back to Poison Master.", MsgMessage.ChatMode.System);
                        client.Player.QuestGUI.IncreaseQuestObjectives(msg, 1355, 1);
                    }
#endif

                    monster.HitPoints = 0;
                    monster.Dead(msg, client, client.Player.UID, client.Map);
                }
                data.ObjId = data.dwParam;
                data.dwParam = 0;
                data.Type = 174;
                client.Send(msg.ActionCreate(data));
            }
            else if (client.Player.QuestGUI.CheckQuest(1487, MsgQuestList.QuestListItem.QuestStatus.Accepted))
            {
                Role.IMapObj target;
                if (client.Player.View.TryGetValue(data.dwParam, out target, Role.MapObjectType.Monster))
                {
                    var monster = target as Game.MsgMonster.MonsterRole;

                    if (monster.Name == "RockMonsterL78")
                    {
#if Arabic
                            client.CreateBoxDialog("The~Level~78~Rock~Monsters~are~afraid~of~the~Cactus~and~run~away.~Hurry~to~report~to~Convoy~Vice~Leader~Ling.");
#else
                        client.CreateBoxDialog("The~Level~78~Rock~Monsters~are~afraid~of~the~Cactus~and~run~away.~Hurry~to~report~to~Convoy~Vice~Leader~Ling.");
#endif
                        monster.HitPoints = 0;
                        monster.Dead(msg, client, client.Player.UID, client.Map);
                        client.Player.QuestGUI.IncreaseQuestObjectives(msg, 1487, 1);

                    }
                }
            }
            else if (client.Player.QuestGUI.CheckQuest(1330, MsgQuestList.QuestListItem.QuestStatus.Accepted))
            {
                Role.IMapObj target;
                if (client.Player.View.TryGetValue(data.dwParam, out target, Role.MapObjectType.Monster))
                {
                    var monster = target as Game.MsgMonster.MonsterRole;
#if Arabic
                     if (monster.Name == "ThunderApe")
                    {
                        if (!client.Player.ActivePick)
                        {
                            client.Player.QuestCaptureType = 1;
                            client.Player.AddPick(msg, "Capture~Thunder~Ape.", 2);
                        }
                    }
                    else if (monster.Name == "ThunderApeL58")
                    {
                        if (!client.Player.ActivePick)
                        {
                            client.Player.QuestCaptureType = 2;
                            client.Player.AddPick(msg, "Capture~Thunder~ApeL58.", 2);
                        }
                    }
#else
                    if (monster.Name == "ThunderApe")
                    {
                        if (!client.Player.ActivePick)
                        {
                            client.Player.QuestCaptureType = 1;
                            client.Player.AddPick(msg, "Capture~Thunder~Ape.", 2);
                        }
                    }
                    else if (monster.Name == "ThunderApeL58")
                    {
                        if (!client.Player.ActivePick)
                        {
                            client.Player.QuestCaptureType = 2;
                            client.Player.AddPick(msg, "Capture~Thunder~ApeL58.", 2);
                        }
                    }
#endif

                    monster.HitPoints = 0;
                    monster.Dead(msg, client, client.Player.UID, client.Map);
                }
                data.ObjId = data.dwParam;
                data.dwParam = 0;
                data.Type = 174;
                client.Send(msg.ActionCreate(data));
            }
        }
        [DataAttribute(ActionType.PokerTeleporter)]
        public unsafe static void PokerTeleporter(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.Map == 1017 || client.Player.Map == 1081 || client.Player.Map == 2060 || client.Player.Map == 9972
                     || client.Player.Map == 1080 || client.Player.Map == 3820 || client.Player.Map == 3954
                 || client.Player.Map == 1806
                     || Game.MsgTournaments.MsgSchedules.DisCity.IsInDisCity(client.Player.Map) || client.Player.Map == 1508
                     || Game.MsgTournaments.MsgSchedules.SteedRace.InSteedRace(client.Player.Map)
             || client.Player.Map == 1768
            
             || client.Player.Map == 1505 || client.Player.Map == 1506 || client.Player.Map == 12022 || client.Player.Map == 1508 || client.Player.Map == 1507)
            {
#if Arabic
                                       client.SendSysMesage("The Poker room is not allowed on this map.");
#else
                client.SendSysMesage("The Poker room is not allowed on this map.");
#endif

                return;
            }

            if (client.Player.Map == 1038 || client.Player.Map == MsgTournaments.MsgClassPKWar.MapID || client.Player.DynamicID != 0
                || client.Player.Map == 6001)
            {
#if Arabic
                   client.SendSysMesage("The Poker room is not allowed on this map.");
#else
                client.SendSysMesage("The Poker room is not allowed on this map.");
#endif

                return;
            }
            if (Pool.Constants.BlockTeleportMap.Contains(client.Player.Map))
                return;
            switch (data.dwParam)
            {
                case 4://roulete
                    {
#if Roullete
                        client.Teleport(56, 65, 3852);
#endif
                        break;
                    }
#if Poker
                case 2://cp room
                    {
                        client.Teleport(58, 66, 1860);
                        break;
                    }
                case 1://gold room
                    {
                        client.Teleport(157, 119, 1858);
                        break;
                    }
#endif
                case 3://one bandit
                    {
                       // client.Teleport(240, 241, 1036);
                        break;
                    }
            }

        }
        [DataAttribute(ActionType.CreditGifts)]
        public unsafe static void CreditGifts(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if ((client.Player.MainFlag & Role.Player.MainFlagType.CanClaim) == Role.Player.MainFlagType.CanClaim)
            {
                if ((client.Player.MainFlag & Role.Player.MainFlagType.ClaimGift) != Role.Player.MainFlagType.ClaimGift)
                {
                    if (client.Inventory.HaveSpace(9))
                    {
                        //client.Inventory.Add(msg, 4200012, 1, 0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, true);
                        //client.Inventory.Add(msg, 4200012, 1, 0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, true);

                        //client.Player.MainFlag |= Role.Player.MainFlagType.ClaimGift;
                        //client.Player.MainFlag |= Role.Player.MainFlagType.ShowSpecialItems;
                        //client.Player.SendUpdate(msg, (uint)client.Player.MainFlag, MsgUpdate.DataType.MainFlag);

                        //client.Player.SendUpdate(msg, (uint)client.Player.MainFlag, MsgUpdate.DataType.MainFlag);
                    }
                }
            }
           
        }
        [DataAttribute(ActionType.UpdateInventorySash)]
        public unsafe static void UpdateInventorySash(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.ConquerPoints >= 90 && client.Player.InventorySashCount < 300)
            {
                if (client.InTrade) return;
                client.Player.InventorySashCount += 1;
                client.Player.ConquerPoints -= 90;
                client.Player.UpdateInventorySash(msg);
            }
            else
            {
#if Arabic
                 client.SendSysMesage("You need 90 CPs in your inventory to open 1 slot in your sash.");
#else
                client.SendSysMesage("You need 90 CPs in your inventory to open 1 slot in your sash.");
#endif

            }
        }
        [DataAttribute(ActionType.Away)]
        public unsafe static void Away(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (data.ObjId == client.Player.UID)
            {
                if (client.Player.Away == 1)
                    client.Player.Away = 0;
                else
                    client.Player.Away = 1;
            }
            else
                client.Player.Away = 0;

            client.Send(msg.ActionCreate(data));
            client.Player.View.SendView(client.Player.GetArray(msg, false), false);
        }
        [DataAttribute(ActionType.TradePartnerInfo)]
        public unsafe static void TradePartnerInfo(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            Client.GameClient Target;
            if (Pool.GamePoll.TryGetValue(data.dwParam, out Target))
            {
                client.Send(msg.TradePartnerInfoCreate(Target));
            }
        }
        [DataAttribute(ActionType.SetAppearanceType)]
        public unsafe static void SetAppearanceType(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.Player.AparenceType = data.dwParam;
            data.ObjId = client.Player.UID;
            client.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.ViewEnemyInfo)]
        public unsafe static void ViewEnemyInfo(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            Client.GameClient Target;
            if (Pool.GamePoll.TryGetValue(data.dwParam, out Target))
            {
                client.Send(msg.CreateViewRelationInfo(Target, true));
            }
        }
        [DataAttribute(ActionType.FinishSteedRace)]
        public unsafe static void FinishSteedRace(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.Map == MsgTournaments.MsgSteedRace.MAPID)
                MsgTournaments.MsgSchedules.SteedRace.FinishRace(client);
        }
        [DataAttribute(ActionType.ViewFriendInfo)]
        public unsafe static void ViewFriendInfo(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            Client.GameClient Target;
            if (Pool.GamePoll.TryGetValue(data.dwParam, out Target))
            {
                client.Send(msg.CreateViewRelationInfo(Target, false));
            }
        }
        [DataAttribute(ActionType.Ghost)]
        public unsafe static void Ghost(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.Alive == false)
            {
                client.Player.SendString(msg, MsgStringPacket.StringID.Effect, true, "ghost");
                data.ObjId = client.Player.UID;
                data.PositionX = client.Player.X;
                data.PositionY = client.Player.Y;
                client.Player.View.SendView(msg.ActionCreate(data), true);
            }
        }
        [DataAttribute(ActionType.StartVendor)]
        public unsafe static void StartVendor(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (!client.IsVendor)
            {
                bool CanVendor = false;
                foreach (var IObj in client.Player.View.Roles(VirusX.Role.MapObjectType.Npc))
                {
                    var screenObjs = IObj as Npc;
                    if (screenObjs.NpcType == (Role.Flags.NpcType)16 && client.Player.InView((ushort)(screenObjs.X + 2), screenObjs.Y, 0))
                        CanVendor = true;
                   
                }
                if (!CanVendor)
                    return;
                client.MyVendor = new Role.Instance.Vendor(client);
                if (client.MyVendor.UID.ContainsKey(client.Player.UID))
                    return;
                client.MyVendor.CreateVendor(msg);
                data.dwParam = client.MyVendor.VendorUID;
                data.PositionX = client.MyVendor.VendorNpc.X;
                data.PositionY = client.MyVendor.VendorNpc.Y;
                client.Send(msg.ActionCreate(data));
            }
        }
        [DataAttribute(ActionType.Revive)]
        public unsafe static void Revive(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            bool CTFRandomRevive = data.dwParam == 2;
            if (CTFRandomRevive)
            {
                if (client.Player.Map == 3935 && client.OnInterServer)
                {
                    foreach (var server in Database.GroupServerList.GroupServers.Values)
                    {
                        if (server.ID == client.Player.ServerID)
                        {
                            client.Teleport((ushort)server.X, (ushort)server.Y, (ushort)server.MapID);
                            break;
                        }
                    }
                    return;
                }
                return;
            }
            if (client.InQualifier())
            {
                return;
            }
            if (client.Player.ContainFlag(MsgUpdate.Flags.SoulShackle))
                return;
            if (client.Player.ContainFlag((MsgUpdate.Flags)438))
                return;
            byte deathtime = 20;
            byte itemLevel = 0;
            if (client.Rune.IsEquipped("Nirvana", ref itemLevel))
            {
                switch (itemLevel)
                {
                    case 1: deathtime -= 5; break;
                    case 2: deathtime -= 6; break;
                    case 3: deathtime -= 7; break;
                    case 4: deathtime -= 8; break;
                    case 5: deathtime -= 9; break;
                    case 6: deathtime -= 10; break;
                    case 7: deathtime -= 11; break;
                    case 8: deathtime -= 12; break;
                    case 9: deathtime -= 15; break;
                }
            }
            if (DateTime.Now > client.Player.DeadStamp.AddSeconds(deathtime) || data.dwParam==4)
            {
                if (!client.Player.Alive)
                {
                    bool StraightLife = data.dwParam == 4;
                    if (StraightLife && client.Player.HeavenBlessing > 0)
                    {
                        if (client.OnInterServer)
                        {
                            if (client.InQualifier())
                            {
                                return;
                            }
                            if (client.Player.Map == 3935)
                            {
                                foreach (var server in Database.GroupServerList.GroupServers.Values)
                                {
                                    if (server.ID == client.Player.ServerID)
                                    {
                                        client.Teleport((ushort)(server.X + +Program.GetRandom.Next(-4, 5)), (ushort)(server.Y + Program.GetRandom.Next(-4, 5)), (ushort)server.MapID);
                                        break;
                                    }
                                }
                            }
                        }
                        client.Player.Revive(msg);
                        return;
                    }
                    bool ReviveHere = data.dwParam == 1;
                    if (ReviveHere && client.Player.HeavenBlessing > 0)
                    {
                        if (client.OnInterServer)
                        {
                            if (client.InQualifier())
                            {
                                return;
                            }
                            if (client.Player.Map == 3935)
                            {
                                foreach (var server in Database.GroupServerList.GroupServers.Values)
                                {
                                    if (server.ID == client.Player.ServerID)
                                    {
                                        client.Teleport((ushort)(server.X + +Program.GetRandom.Next(-4, 5)), (ushort)(server.Y + Program.GetRandom.Next(-4, 5)), (ushort)server.MapID);
                                        break;
                                    }
                                }
                            }
                        }
                        if (client.Player.Map == 1038 || client.Player.Map == MsgTournaments.MsgClassPKWar.MapID
                            || client.Player.Map == 1357
                            || client.Player.Map == MsgTournaments.MsgCaptureTheFlag.MapID
                            || client.Player.Map == 6011)
                            return;
                        client.Player.Revive(msg);
                        return;
                    }
                    else
                    {
                        if (client.Player.Map == 4000 || client.Player.Map == 4003 || client.Player.Map == 4006 || client.Player.Map == 4008 || client.Player.Map == 4009)
                        {
                            client.Teleport(85, 75, 4020);
                            return;
                        }
                        if (client.Player.Map == 8009)
                        {
                            client.Teleport(84, 137, 8009);
                            return;
                        }
                        if (client.Player.Map == 3998)
                        {
                            client.Teleport(106, 383, 3998);
                            return;
                        }

                        if (client.OnInterServer)
                        {
                            if (client.Player.Map == 10137)
                            {
                                client.Teleport(95, 409, 10137);
                                return;
                            }
                        }
                        if (client.Player.Map == 3935 && client.OnInterServer)
                        {
                            foreach (var server in Database.GroupServerList.GroupServers.Values)
                            {
                                if (server.ID == client.Player.ServerID)
                                {
                                    client.Teleport((ushort)server.X, (ushort)server.Y, (ushort)server.MapID);
                                    break;
                                }
                            }
                            return;
                        }
                        if (client.Player.OnMyOwnServer == false)
                        {
                            client.Teleport((ushort)432, 390, 1002);
                            return;
                        }
                       

                        if (client.Player.Map == 1762 || client.Player.Map == 1927 || client.Player.Map == 1999 || client.Player.Map == 2054 || client.Player.Map == 2055)
                        {
                            client.Teleport(477, 640, 1000);
                            return;
                        }
                       
                        if (client.Player.Map == 10250)
                        {
                            client.Teleport(1014, 1288, 10250);
                            return;
                        }
                        if (client.Player.Map == 6011)
                        {
                            client.Teleport(410, 354, 1002);
                            return;
                        }
                        if (client.Player.Map == 1038)// gw map
                        {
                            client.Teleport(410, 354, 1002, 0, true, true);
                        }
                        else if (client.Player.Map == 1138)//super gw
                        {
                            if (MsgTournaments.MsgSchedules.SuperGuildWar.Proces == MsgTournaments.ProcesType.Dead)
                                client.Teleport(409, 353, 1002);
                            else
                            {
                                var map = Pool.ServerMaps[1038];
                                client.Teleport(27, 73, map.Reborn_Map);
                            }
                        }
                        else
                        {
                            if (client.Map.Reborn_Map != client.Player.Map)
                            {
                                if (client.Map.Reborn_Map == 0)
                                {
                                    client.Teleport(410, 354, 1002);
                                    return;
                                }
                                var map = Pool.ServerMaps[client.Map.Reborn_Map];
                                client.Teleport(map.Reborn_X, map.Reborn_Y, map.Reborn_Map);
                            }
                            else
                            {
                                if (client.Map.Reborn_X != 0)
                                    client.Teleport(client.Map.Reborn_X, client.Map.Reborn_Y, client.Map.Reborn_Map);
                                else
                                {
                                    var map = Pool.ServerMaps[client.Map.Reborn_Map];
                                    if (map.Reborn_X != 0)
                                        client.Teleport(map.Reborn_X, map.Reborn_Y, map.Reborn_Map);
                                    else
                                    {
                                        map = Pool.ServerMaps[map.Reborn_Map];
                                        if (map.Reborn_X != 0)
                                            client.Teleport(map.Reborn_X, map.Reborn_Y, map.Reborn_Map);
                                        else
                                            client.Teleport(410, 354, 1002);
                                    }
                                }
                            }
                        }
                        if (client.Player.X == 0 || client.Player.Y == 0)//invalid map reborn
                            client.Teleport(410, 354, 1002);
                    }
                }
            }
        }
        [DataAttribute(ActionType.EndTransformation)]
        public unsafe static void EndTransformation(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.OnTransform)
            {
                if (client.Player.TransformInfo != null)
                {
                    client.Player.TransformInfo.Stamp = DateTime.Now;
                }
            }
        }
        [DataAttribute(ActionType.UpdateSpell)]
        public unsafe static void UpdateSpell(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            MsgSpell Spell;
            if (client.MySpells.ClientSpells.TryGetValue((ushort)data.dwParam, out Spell))
            {
                Dictionary<ushort, Database.MagicType.Magic> DbSpells;
                if (Pool.Magic.TryGetValue(Spell.ID, out DbSpells))
                {
                    if (Spell.Level < DbSpells.Count - 1)
                    {
                        if (DbSpells.ContainsKey(Spell.Level))
                        {
                            decimal cpCost = DbSpells[Spell.Level].CpsCost;
                            cpCost = ((cpCost / 2) - (cpCost / 2 / 10)) / 10;

                            int max = Math.Max((int)Spell.Experience, 1);
                            int percentage = (int)Math.Ceiling((decimal)(100 - (int)(max / Math.Max((DbSpells[Spell.Level].Experience / 100), 1))));
                            cpCost = Math.Ceiling((decimal)(cpCost * percentage / 100));
                            if (cpCost < 0) cpCost = 0;
                            else if (cpCost > int.MaxValue) cpCost = int.MaxValue;
                            if (client.Player.BoundConquerPoints >= cpCost)
                            {
                                if (client.InTrade) return;
                                client.Player.BoundConquerPoints -= (int)cpCost;

                                client.MySpells.Add(msg, Spell.ID, (ushort)(Spell.Level + 1), Spell.SoulLevel, Spell.PreviousLevel, Spell.Experience);
                            }
                            else
                            {
                                if (client.Player.ConquerPoints >= cpCost)
                                {
                                    if (client.InTrade) return;
                                    client.Player.ConquerPoints -= (long)cpCost;

                                    client.MySpells.Add(msg, Spell.ID, (ushort)(Spell.Level + 1), Spell.SoulLevel, Spell.PreviousLevel, Spell.Experience);
                                }
                            }
                          
                            
                        }
                    }
                }
            }
        }
        [DataAttribute(ActionType.UpdateProf)]
        public unsafe static void UpdateProf(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.InTrade) return;
            MsgProficiency prof;
            if (client.MyProfs.ClientProf.TryGetValue(data.dwParam, out prof))
            {
                if (prof.Level < 20)
                {
                    uint cpCost = 0;
                    #region Costs
                    switch (prof.Level)
                    {
                        case 1: cpCost = 100; break;
                        case 2: cpCost = 120; break;
                        case 3: cpCost = 140; break;
                        case 4: cpCost = 160; break;
                        case 5: cpCost = 180; break;
                        case 6: cpCost = 200; break;
                        case 7: cpCost = 220; break;
                        case 8: cpCost = 240; break;
                        case 9: cpCost = 260; break;
                        case 10: cpCost = 280; break;
                        case 11: cpCost = 300; break;
                        case 12: cpCost = 320; break;
                        case 13: cpCost = 340; break;
                        case 14: cpCost = 360; break;
                        case 15: cpCost = 380; break;
                        case 16: cpCost = 400; break;
                        case 17: cpCost = 800; break;
                        case 18: cpCost = 1000; break;
                        case 19: cpCost = 2000; break;
                    }
                    #endregion
                    uint needExperience = BaseFunc.ProficiencyLevelExperience((byte)prof.Level);
                    int max = Math.Max((int)prof.Experience, 1);
                    int percentage = 100 - (int)(max / (needExperience / 100));
                    cpCost = (uint)(cpCost * percentage / 100);
                    if (client.Player.ConquerPoints >= cpCost)
                    {
                        client.Player.ConquerPoints -= (long)cpCost;

                        client.MyProfs.Add(msg, prof.ID, prof.Level + 1, prof.Experience, prof.PreviouseLevel);
                        client.SendSysMesage("You have paid " + cpCost + " Conquer Points to increase the weapon proficiency!", MsgMessage.ChatMode.TopLeftSystem, MsgMessage.MsgColor.red);
                    }
                }
            }
        }
        [DataAttribute(ActionType.StopVending)]
        public unsafe static void StopVending(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.IsVendorr = false;
            if (client.IsVendor)
                client.MyVendor.StopVending(msg);
        }
        [DataAttribute(ActionType.EndFly)]
        public unsafe static void EndFly(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.Player.RemoveFlag(MsgUpdate.Flags.Fly);
            client.Player.RemoveFlag(MsgUpdate.Flags.Infinity);
            client.Player.RemoveFlag(MsgUpdate.Flags.HeavensWrath);
            client.Player.RemoveFlag(MsgUpdate.Flags.HoverFather);
  
        }
        [DataAttribute(ActionType.TeamSearchForMember)]
        public unsafe static void TeamSearchForMember(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Team != null)
            {
                Client.GameClient Member;
                if (client.Team.TryGetMember(data.dwParam, out Member))
                {
                    if (Member.Player.Map == client.Player.Map)
                    {
                        data.PositionX = Member.Player.X;
                        data.PositionY = Member.Player.Y;
                        client.Send(msg.ActionCreate(data));
                    }
                }
            }
        }
        [DataAttribute(ActionType.SetLocation)]
        public unsafe static void SetLocation(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if ((client.ClientFlag & Client.ServerFlag.SetLocation) != Client.ServerFlag.SetLocation)
            {
                if (client.Player.Map == 3852 || client.Player.Map == 3845)
                {
                    client.Player.Map = 1002;
                    client.Player.X = 410;
                    client.Player.Y = 354;
                }
                if (!Role.GameMap.CheckMap(client.Player.Map))
                {
                    client.Player.Map = 1002;
                    client.Player.X = 410;
                    client.Player.Y = 354;
                }
            back:
                Role.GameMap.EnterMap((int)client.Player.Map);
                if (Pool.ServerMaps.TryGetValue(client.Player.Map, out client.Map))
                {

                    client.ClientFlag |= Client.ServerFlag.SetLocation;
                    client.Map.Enquer(client);

                    if (client.Player.HitPoints == 0)
                    {
                        client.Player.HitPoints = 1;

                        if (client.Player.Map == 1038)// gw map
                        {
                            if (MsgTournaments.MsgSchedules.GuildWar.Proces == MsgTournaments.ProcesType.Dead)
                                client.Teleport(410, 354, 1002);
                            else
                            {
                                var map = Pool.ServerMaps[1038];
                                client.Teleport(27, 73, map.Reborn_Map);
                            }
                        }
                        else if (client.Player.Map == 1138)//super gw
                        {
                            if (MsgTournaments.MsgSchedules.SuperGuildWar.Proces == MsgTournaments.ProcesType.Dead)
                                client.Teleport(410, 354, 1002);
                            else
                            {
                                var map = Pool.ServerMaps[1038];
                                client.Teleport(27, 73, map.Reborn_Map);
                            }
                        }
                        else
                        {
                            if (client.Player.Map == 601)
                                client.Teleport(410, 354, 1002);
                            else
                            {
                                if (client.Map.Reborn_X != 0)
                                    client.Teleport(client.Map.Reborn_X, client.Map.Reborn_Y, client.Map.Reborn_Map);
                                else
                                {
                                    Role.GameMap map;// ;= Pool.ServerMaps[client.Map.Reborn_Map];
                                    Role.GameMap.EnterMap(client.Map.Reborn_Map);
                                    if (Pool.ServerMaps.TryGetValue(client.Map.Reborn_Map, out map))
                                    {
                                        if (map.Reborn_X != 0)
                                            client.Teleport(map.Reborn_X, map.Reborn_Y, map.Reborn_Map);
                                        else
                                        {
                                            map = Pool.ServerMaps[map.Reborn_Map];
                                            if (map.Reborn_X != 0)
                                                client.Teleport(map.Reborn_X, map.Reborn_Y, map.Reborn_Map);
                                            else
                                                client.Teleport(410, 354, 1002);
                                        }
                                    }
                                    else
                                    {

                                        client.Teleport(410, 354, 1002);
                                    }
                                }
                                if (client.Player.X == 0 || client.Player.Y == 0)//invalid map reborn
                                    client.Teleport(410, 354, 1002);
                            }
                        }
                    }

                    data.ObjId = client.Map.ID;
                    if (client.Map.BaseID != 0)
                        data.dwParam = client.Map.BaseID;
                    else
                        data.dwParam = client.Player.Map;
                    data.PositionX = client.Player.X;
                    data.PositionY = client.Player.Y;
                    data.Fascing = (ushort)client.Player.Angle;
                    client.Send(msg.ActionCreate(data));




                    client.Player.ClearPreviouseCoord();
                    client.Player.View.Role();



                }
                else
                {
                    client.Player.Map = 1002;
                    client.Player.X = 410;
                    client.Player.Y = 354;
                    Pool.ServerMaps[1002].Enquer(client);
                    goto back;
                }

                client.Send(msg.MapStatusCreate(client.Map.ID, client.Map.ID, client.Map.TypeStatus));

                //client.Send(msg.WeatherCreate(MsgWeather.WeatherType.Snow, 1000, 3, 0, 0));
                if (client.Player.Map == 3846)
                {

                    ActionQuery datam = new ActionQuery()
                    {
                        ObjId = client.Player.UID,
                        Type = ActionType.SetMapColor,
                        dwParam = 16755370,
                        PositionX = client.Player.X,
                        PositionY = client.Player.Y
                    };

                    client.Send(msg.ActionCreate(datam));
                }
            }
           

        }
        [DataAttribute(ActionType.ChangeMap)]
        public unsafe static void ChangeMap(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
           
            ushort Portal_X = (ushort)data.PositionX;
            ushort Portal_Y = (ushort)data.PositionY;
            foreach (var portal in client.Map.Portals)
            {
                if (Role.Core.GetDistance(client.Player.X, client.Player.Y, portal.X, portal.Y) < 7)
                {

                    client.Teleport(portal.Destiantion_X, portal.Destiantion_Y, portal.Destiantion_MapID);
                    client.Send(msg.ActionCreate(data));
                    return;
                }
            }
            if (client.ProjectManager)
            {
                client.SendSysMesage("Invalid Portal : X = " + Portal_X + ", Y= " + Portal_Y + " Map = " + client.Player.Map + "", MsgMessage.ChatMode.System, MsgMessage.MsgColor.yellow);
            }
            client.Teleport(410, 354, 1002);
        }
        [DataAttribute(ActionType.ChangeLookface)]
        public unsafe static void ChangeLookface(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.PokerPlayer != null)
                return;
            if (client.Player.Money >= 500)
            {
                uint newface = data.dwParam;
                if (client.Player.Body > 2000)
                {
                    newface = newface < 200 ? newface + 200 : newface;
                    client.Player.Face = (ushort)newface;
                }
                else
                {
                    newface = newface > 200 ? newface - 200 : newface;
                    client.Player.Face = (ushort)newface;
                }
                client.Player.Money -= 500;
                client.Send(msg.ActionCreate(data));
                client.Player.SendUpdate(msg, client.Player.Money, MsgUpdate.DataType.Money);
            }
            else
            {
#if Arabic
                client.SendSysMesage("You do not have 500 silvers with you.", MsgMessage.ChatMode.TopLeft);
#else
                client.SendSysMesage("You do not have 500 silvers with you.", MsgMessage.ChatMode.TopLeft);
#endif

            }
        }
        [DataAttribute(ActionType.RequestEntity)]
        public unsafe static void RequestEntity(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            Role.IMapObj obj;
            if (client.Player.View.TryGetValue(data.dwParam, out obj, Role.MapObjectType.Player))
            {
                var pClient = (obj as Role.Player).Owner;
                if (pClient.IsWatching() || pClient.Player.Invisible)
                    return;
                client.Send(obj.GetArray(msg, false));
            }
            else if (client.Player.View.TryGetValue(data.dwParam, out obj, Role.MapObjectType.Monster))
            {
                client.Send(obj.GetArray(msg, false));
            }
        }

        [DataAttribute(ActionType.QuerySpawn)]
        public unsafe static void QuerySpawn(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            Client.GameClient Target;
            if (Pool.GamePoll.TryGetValue(data.dwParam, out Target))
            {

                client.Send(Target.Player.GetArray(msg, true));
            }

        }
        [DataAttribute(ActionType.ViewEquipment)]
        public unsafe static void ViewEquipment(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            Client.GameClient Target;
            if (Pool.GamePoll.TryGetValue(data.dwParam, out Target))
            {
                if (Target.Equipment == null || Target.Equipment.CurentEquip == null) return;
                MsgUserAbilityScore.UserAbilityScore info = new MsgUserAbilityScore.UserAbilityScore();
                info.type = 1;
                info.Level = Target.Player.Level;
                info.UID = Target.Player.UID;
                info.Items = new MsgUserAbilityScore.AbilityScore[Target.PrestigePoints.Length];
                for (int x = 0; x < Target.PrestigePoints.Length; x++)
                {
                    info.Items[x] = new MsgUserAbilityScore.AbilityScore();
                    info.Items[x].Position = (uint)(x + 1);
                    info.Items[x].Points = Target.PrestigePoints[x];
                }
                client.Send(msg.UserAbilityScoreCreate(info));

                foreach (var item in Target.Equipment.CurentEquip)
                {
                    if (item != null)
                    {
                        client.Send(msg.ItemViewCreate(Target.Player.UID, 0, item, MsgItemView.ActionMode.ViewEquip));
                        item.SendItemExtra(client, msg);
                    }
                }
                foreach (var Yuanshen in Target.EonspiritSystem.Values)
                {
                    if (Yuanshen.Position == 222 || Yuanshen.Position == 223)
                        client.Send(msg.ItemViewCreate(Target.Player.UID, 0, Yuanshen, MsgItemView.ActionMode.Yuanshen));
                }
                foreach (var rune in Target.Rune.Objects)
                {
                    client.Send(msg.ItemViewCreate(Target.Player.UID, 0, rune, MsgItemView.ActionMode.Rune));
                }
                Game.MsgServer.MsgStringPacket packet = new Game.MsgServer.MsgStringPacket();
                packet.ID = MsgStringPacket.StringID.ViewEquipSpouse;
                packet.UID = client.Player.UID;
                packet.Strings = new string[1] { Target.Player.Spouse };
                client.Send(msg.StringPacketCreate(packet));
                client.Send(msg.StatusCreate(Target.Status));
                Target.SendSysMesage(client.Player.Name + " is checking your equipment.", MsgMessage.ChatMode.System);
            }
        }
        [DataAttribute(ActionType.Hotkeys)]
        public unsafe static void Hotkeys(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.Send(msg.ActionCreate(data));

        }
        
        public static double CalculateJumpStamp(short distance)
        {
            return (int)(distance * 220);
        }
        [DataAttribute(ActionType.Jump)]
        public unsafe static void PlayerJump(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.IsVendorr)
            {
                client.Pullback();
                client.IsVendorr = false;
                return;
            }
            if (data.ObjId != client.Player.UID)
                return;
            if (client.Player.ContainFlag((MsgUpdate.Flags)491))
                client.Player.RemoveFlag((MsgUpdate.Flags)491);
            if (client.Player.Away == 1)
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var apacket = rec.GetStream();
                    client.Player.Away = 0;
                    client.Player.View.SendView(client.Player.GetArray(apacket, false), false);
                    client.Player.KingDomExploits = client.Player.KingDomExploits;
                }
            }
            if (client.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.ManiacDance)
                || client.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.ShurikenVortex)
                || client.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.DefensiveStance)
                || (client.Player.ContainFlag(Game.MsgServer.MsgUpdate.Flags.Dizzy)
                && client.Player.StunJump.AddMilliseconds(900) > DateTime.Now))
            {
                var Stun = new ActionQuery();
                Stun.ObjId = client.Player.UID;
                Stun.PositionX = client.Player.X;
                Stun.PositionY = client.Player.Y;
                Stun.Type = 108;
                client.Send(msg.ActionCreate(Stun));
                return;
            }
           
            client.OnAutoAttack = false;
        
            client.Player.RemoveBuffersMovements(msg);
            if (client.Player.InUseIntensify)
            {
                client.Player.InUseIntensify = false;
                using (var recycledPacket = new ServerSockets.RecycledPacket())
                {
                    var streamm = recycledPacket.GetStream();
                    client.Send(streamm.ActionCreate(new ActionQuery() { ObjId = client.Player.UID, TargetUID = client.Player.FocusClientSpell.ID, Timestamp = (uint)Time32.timeGetTime().GetHashCode(), Type = 103 }));
                }
            }
            if (!client.Player.Alive && !client.Player.ContainFlag(MsgUpdate.Flags.NeptuneCurse))
            {
                client.Map.View.MoveTo<Role.IMapObj>(client.Player, client.Player.Dead_X, client.Player.Dead_Y);
                if (client.Player.Dead_X == 0 || client.Player.Dead_Y == 0)
                {
                    client.Player.X = 410;
                    client.Player.Y = 354;
                }
                else
                {
                    client.Player.X = client.Player.Dead_X;
                    client.Player.Y = client.Player.Dead_Y;
                }

                InteractQuery action = new InteractQuery()
                {
                    X = client.Player.Dead_X,
                    AtkType = (ushort)MsgAttackPacket.AttackID.Death,
                    Y = client.Player.Dead_Y,
                    OpponentUID = client.Player.UID
                };
                client.Player.View.SendView(msg.InteractionCreate(action), true);

                return;
            }

            ushort JumpX = (ushort)data.TargetPositionX;
            ushort JumpY = (ushort)data.TargetPositionY;



            if (client.Map == null)
            {
                client.Teleport(410, 354, 1002);
                return;

            }
            if (client.Player.Map == 1038)
            {
                if (!Game.MsgTournaments.MsgSchedules.GuildWar.ValidJump(client.TerainMask, out client.TerainMask, JumpX, JumpY))
                {

                    client.SendSysMesage("Illegal jumping over the gates detected.");

                    client.Pullback();
                    return;
                }

            }
            if (client.Player.Map == 1002)
            {

                if (client.Map.Altitude[JumpX, JumpY] - client.Map.Altitude[client.Player.X, client.Player.Y] > 200)// If the target tallness is greater than current tallness by 200 point
                {
                    MyConsole.WriteLine("[WARNING] " + client.Player.Name + " has been jumped to a high place!");
                    MyConsole.WriteLine("Map: " + client.Player.Map + ", X:" + JumpX + ", Y:" + JumpY);
                    client.SendSysMesage("Illegal jumping detected.");
                    client.Pullback();

                    return;
                }
            }

            #region ObjInteraction
            if (client.Player.ObjInteraction != null)
            {

                InterActionWalk inter = new InterActionWalk()
                {
                    Mode = MsgInterAction.Action.Jump,
                    X = JumpX,
                    Y = JumpY,
                    UID = client.Player.UID,
                    OponentUID = client.Player.ObjInteraction.Player.UID
                };
                client.Player.View.SendView(msg.InterActionWalk(&inter), true);


                client.Player.Angle = Role.Core.GetAngle(client.Player.X, client.Player.Y, JumpX, JumpY);
                client.Player.Action = Role.Flags.ConquerAction.InteractionHold;
                client.Map.View.MoveTo<Role.IMapObj>(client.Player, JumpX, JumpY);
                client.Player.X = JumpX;
                client.Player.Y = JumpY;
                client.Player.View.Role(false, null);

                client.Map.View.MoveTo<Role.IMapObj>(client.Player.ObjInteraction.Player, JumpX, JumpY);
                client.Player.ObjInteraction.Player.X = JumpX;
                client.Player.ObjInteraction.Player.Y = JumpY;
                client.Player.ObjInteraction.Player.Action = Role.Flags.ConquerAction.Jump;
                client.Player.ObjInteraction.Player.Angle = Role.Core.GetAngle(client.Player.X, client.Player.Y, JumpX, JumpY);
                client.Player.ObjInteraction.Player.View.Role(false, null);

                return;
            }
            #endregion
            short Distance = Role.Core.GetDistance(client.Player.X, client.Player.Y, JumpX, JumpY);
            client.Player.lastJumpTime = DateTime.Now.AddMilliseconds(CalculateJumpStamp(Distance));
           
            if (client.Player.ContainFlag(MsgUpdate.Flags.Ride))
            {
                uint vigor = (ushort)(1.5F * (Distance / 2));
                if (client.Vigor >= vigor)
                    client.Vigor -= vigor;
                else
                    client.Vigor = 0;


                client.Send(msg.ServerInfoCreate(client.Vigor));

            }
            data.dwParam3 = client.Player.Map;
            client.Player.View.SendView(msg.ActionCreate(data), true);
            client.Player.Angle = Role.Core.GetAngle(client.Player.X, client.Player.Y, JumpX, JumpY);
            client.Player.Action = Role.Flags.ConquerAction.Jump;
            client.Map.View.MoveTo<Role.IMapObj>(client.Player, JumpX, JumpY);
            client.Player.X = JumpX;
            client.Player.Y = JumpY;
            client.Player.View.Role(false, null);
            if (MsgTournaments.MsgSchedules.CaptureTheFlag != null)
            {
                MsgTournaments.MsgSchedules.CaptureTheFlag.ChechMoveFlag(client);
                MsgTournaments.MsgSchedules.CaptureTheFlag.PlantTheFlag(client, msg);
            }
            if (MsgTournaments.MsgSchedules.GuildWar != null)
                MsgTournaments.MsgSchedules.GuildWar.ChechMove(client);


            if (client.Player.Map == 10445 || client.Player.Map == 10446 || client.Player.Map == 10447 || client.Player.Map == 10448 || client.Player.Map == 10449 || client.Player.Map == 10450 || client.Player.Map == 10451 || client.Player.Map == 10452 || client.Player.Map == 10453 || client.Player.Map == 10454 || client.Player.Map == 10455)
            {
                if (Role.Core.GetDistance(client.Player.X, client.Player.Y, 66, 90) <= 4)
                {
                    client.Teleport(58, 65, 1004);
                }
            }
            if (client.Player.Map == 10622)
            {
                if (Role.Core.GetDistance(client.Player.X, client.Player.Y, 43, 31) <= 4)
                {
                    client.Player.MessageBox("Are you sure you want return to Twin City?", p => { p.Teleport(349, 400, 1002); p.SendSysMesage(" you were teleported back to Twin City."); });
                }
            }
            #region Stage1
            if (client.Player.Map == 11223)
            {
                #region Portal1
                if (Role.Core.GetDistance(client.Player.X, client.Player.Y, 138, 164) <= 4)
                {
                    if (client.Player.Portal == 1)
                    {
                        client.Player.MessageBox("STR_ID_tArchersTask[Msg][Jump][1]@@", new Action<Client.GameClient>(p =>
                        {
                            client.Teleport(66, 141, 11223, client.Player.UID);
                        }), null);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            var map = Pool.ServerMaps[11223];
                            if (!map.ContainMobID(4226, client.Player.UID))
                            {
                                Server.AddMapMonster(stream, map, 4226, 63, 140, 18, 18, 1, client.Player.UID, true);
                            }

                        }
                    }

                }
                #endregion
                #region Portal2
                if (Role.Core.GetDistance(client.Player.X, client.Player.Y, 161, 191) <= 4)
                {
                    if (client.Player.Portal == 2)
                    {
                        client.Player.MessageBox("STR_ID_tArchersTask[Msg][Jump][2]@@", new Action<Client.GameClient>(p =>
                        {
                            client.Teleport(157, 244, 11223, client.Player.UID);
                        }), null);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            var map = Pool.ServerMaps[11223];
                            if (!map.ContainMobID(4227, client.Player.UID))
                            {
                                Server.AddMapMonster(stream, map, 4227, 156, 243, 18, 18, 1, client.Player.UID, true);
                            }

                        }
                    }
                }
                #endregion
                #region Portal3
                if (Role.Core.GetDistance(client.Player.X, client.Player.Y, 188, 153) <= 4)
                {
                    if (client.Player.Portal == 3)
                    {
                        client.Player.MessageBox("STR_ID_tArchersTask[Msg][Jump][3]@@", new Action<Client.GameClient>(p =>
                        {
                            client.Teleport(254, 147, 11223, client.Player.UID);
                        }), null);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            var map = Pool.ServerMaps[11223];
                            if (!map.ContainMobID(4228, client.Player.UID))
                            {
                                Server.AddMapMonster(stream, map, 4228, 254, 147, 18, 18, 1, client.Player.UID, true);
                            }

                        }
                    }
                }
                #endregion
                #region Portal4
                if (Role.Core.GetDistance(client.Player.X, client.Player.Y, 145, 132) <= 4)
                {
                    if (client.Player.Portal == 4)
                    {
                        client.Player.MessageBox("STR_ID_tArchersTask[Msg][Jump][4]@@", new Action<Client.GameClient>(p =>
                        {
                            client.Teleport(96, 81, 11223, client.Player.UID);
                        }), null);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            var map = Pool.ServerMaps[11223];
                            if (!map.ContainMobID(4229, client.Player.UID))
                            {
                                Server.AddMapMonster(stream, map, 4229, 96, 81, 18, 18, 1, client.Player.UID, true);
                            }

                        }
                    }
                }
                #endregion

            }
            #endregion
            if (client.Player.ActivePick)
                client.Player.RemovePick(msg);
            #region ShadowClones
            if (client.Player.MyClones.Count > 0)
            {
                foreach (var clone in client.Player.MyClones.GetValues())
                {
                    clone.Owner.Player.X = client.Player.X;
                    clone.Owner.Player.Y = client.Player.Y;
                    clone.Owner.Player.Angle = client.Player.Angle;
                }
            }
            #endregion
            if (client.Player.Map == 10428 && Role.Core.GetDistance(client.Player.X, client.Player.Y, 61, 66) <= 2)
            {
                client.Player.MessageBox("Are you sure you want to leave the alien world and return to Twin City?", p => { p.Teleport(351, 422, 1002); p.SendSysMesage("With a flash of light, you were teleported back to Twin City."); });
            }
            if (client.Player.Map == 11041 && Role.Core.GetDistance(client.Player.X, client.Player.Y, 87, 76) <= 2)
            {
                client.Player.MessageBox("Are you sure you want return to Twin City?", p => { p.Teleport(410, 354, 1002); p.SendSysMesage(" you were teleported back to Twin City."); });
            }
            if (client.Player.Map == 11223 && Role.Core.GetDistance(client.Player.X, client.Player.Y, 179, 173) <= 2)
            {
                client.Player.MessageBox("Are you sure you want return to Twin City?", p => { p.Teleport(410, 354, 1002); p.SendSysMesage(" you were teleported back to Twin City."); });
            }
            if (client.SaveMele != null)
            {
                if (client.SaveMele.OnAutoAttack)
                {
                    if (client.SaveMele.AutoAttack != null && client.SaveMele.AutoAttack.OpponentUID == client.Player.UID)
                    {
                        client.onSaveMele = true;
                        client.SaveMeleStamp = DateTime.Now.AddMilliseconds(250);
                    }
                }
            }
            else 
            {
                Client.GameClient Client;
                if (client.SaveMele != null)
                {
                    if (Pool.GamePoll.TryGetValue(client.SaveMele.Player.UID, out Client))
                    {
                        if (Client.SaveMele != null)
                        {
                            Client.SaveMele.nSaveMele = false;
                            Client.SaveMele = null;

                        }
                    }
                }
            }
        }

        [DataAttribute(ActionType.DeleteCharacter)]
        public unsafe static void DeleteCharacter(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.Player.Delete = true;
            client.Socket.Disconnect();
        } 
        [DataAttribute(ActionType.ChangeDirection)]
        public unsafe static void ChangeDirection(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.OnAutoAttack = false;
            client.Player.Angle = (Role.Flags.ConquerAngle)data.Fascing;
#if TEST
               Console.WriteLine("Direction . " + data.Fascing + " " + client.Player.Angle.ToString()+ "");
#endif
            client.Player.View.SendView(msg.ActionCreate(data), true);
            if (client.Player.ActivePick)
                client.Player.RemovePick(msg);
        }
        [DataAttribute(ActionType.ChangeStance)]
        public unsafe static void ChangeStance(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.OnAutoAttack = false;
            client.Player.Action = (Role.Flags.ConquerAction)data.dwParam;
           
            if (client.Player.Action == Role.Flags.ConquerAction.Cool)
            {
               
                if (client.Equipment.FullSuper)
                    data.dwParam = (uint)(data.dwParam | (uint)(client.Player.Class * 0x10000 + 0x1000000));
                else if (client.Equipment.SuperArmor)
                    data.dwParam = (uint)(data.dwParam | (uint)(client.Player.Class * 0x10000));
            }
            data.ObjId = client.Player.UID;
            data.dwParam = (ushort)client.Player.Action;
            if (client.Player.Action == Role.Flags.ConquerAction.Cool)
                client.Send(msg.CreateDragonSkinInfo(new MsgDragonSkin.MsgDragonSkinProto()
                {
                    Type = MsgDragonSkin.ActionID.Effect,
                    UID = client.Player.UID,
                    DragonSkinRecord = client.DragonSkin.GetStyle()
                }));
            client.Player.View.SendView(msg.ActionCreate(data), false);
            if (client.Player.Action == Role.Flags.ConquerAction.PistilAroma)
            {
                var UseIt = client.Collection.items.Values.Where(p => p.Use == 1).FirstOrDefault();
                if (UseIt != null)
                    client.Player.PistilAromaID = UseIt.ID;
                client.Player.View.SendView(msg.ActionCreate(data), false);
            }
           
            if (client.Player.ContainFlag(MsgUpdate.Flags.CastPray))
            {
                foreach (var user in client.Player.View.Roles(Role.MapObjectType.Player))
                {
                    if (Role.Core.GetDistance(client.Player.X, client.Player.Y, user.X, user.Y) <= 4)
                    {
                        data.Timestamp = user.UID;
                        client.Player.View.SendView(msg.ActionCreate(data), true);
                    }
                }
            }
            if (client.Player.ActivePick)
                client.Player.RemovePick(msg);
            if (DateTime.Now > client.Player.SpellAttackStamp.AddMilliseconds(client.Equipment.AttackSpeed(true)))
            {
                client.Player.SpellAttackStamp = DateTime.Now;
                client.Player.SpellAttackStamp.AddMilliseconds(client.Equipment.AttackSpeed(true));
            }
        }
        [DataAttribute(ActionType.SetPkMode)]
        public unsafe static void SetPkMode(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.PkMode == Role.Flags.PKMode.Jiang)
                client.SendSysMesage("You have temporarily quit the Jiang Hu!", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
            client.Player.PkMode = (Role.Flags.PKMode)data.dwParam;
            data.dwParam3 = data.dwParam;
            client.Send(msg.ActionCreate(data));
            if (client.Player.PkMode == Role.Flags.PKMode.PK)
                client.SendSysMesage("Free PK mode. You can attack monster and all players.", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
            else if (client.Player.PkMode == Role.Flags.PKMode.Capture)
                client.SendSysMesage("Capture PK mode. You can only attack monsters, black-name and blue-name players.", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
            else if (client.Player.PkMode == Role.Flags.PKMode.Peace)
                client.SendSysMesage("Peace mode. You can only attack monsters.", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
            else if (client.Player.PkMode == Role.Flags.PKMode.Team)
                client.SendSysMesage("Team PK mode. You can attack monster and all players except your teammates or guild.", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
            else if (client.Player.PkMode == Role.Flags.PKMode.Revange)
                client.SendSysMesage("Revenge PK mode. You can attack monster and all Your Enemy List Players.", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
            else if (client.Player.PkMode == Role.Flags.PKMode.Guild)
                client.SendSysMesage("Guild PK mode. You can attack monster and all Your Guild's Enemies", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
#if Jiang
            if (client.Player.PkMode == Role.Flags.PKMode.Jiang)
            {
                if (client.Player.MyJiangHu != null)
                {
                    client.Player.MyJiangHu.ActiveJiangMode(client);
                    client.SendSysMesage("You have returned to the Jiang Hu !", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
                }
            }
#endif
        }
        [DataAttribute(ActionType.ConfirmAssociates)]
        public unsafe static void ConfirmAssociates(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.ConfirmSpells)]
        public unsafe static void ConfirmSpells(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.MySpells.SendAll(msg);
            client.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.ConfirmProficiencies)]
        public unsafe static void ConfirmProficiencies(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.MyProfs.SendAll(msg);
            client.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.ConfirmGuild)]
        public unsafe static void ConfirmGuild(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.MyGuild != null && client.Player.MyGuildMember != null)
            {
                client.Player.SendString(msg, MsgStringPacket.StringID.GuildName, client.Player.MyGuild.Info.GuildID, true, client.Player.MyGuild.GuildName + " " + client.Player.MyGuild.Info.LeaderName + " " + client.Player.MyGuild.Info.Level.ToString() + " " + client.Player.MyGuild.Members.Count.ToString() + " 83");
                client.Player.MyGuild.SendThat(client.Player);
                client.Player.SendString(msg, MsgStringPacket.StringID.GuildName, client.Player.MyGuild.Info.GuildID, true, client.Player.MyGuild.GuildName + " " + client.Player.MyGuild.Info.LeaderName + " " + client.Player.MyGuild.Info.Level.ToString() + " " + client.Player.MyGuild.Members.Count.ToString() + " 83");
                client.Player.MyGuild.SendGuildAlly(msg, true, client);
                client.Player.MyGuild.SendGuilEnnemy(msg, true, client);
                client.Player.GuildBattlePower = client.Player.MyGuild.ShareMemberPotency(client.Player.MyGuildMember.Rank);
            }
            client.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.AllowAnimation)]
        public unsafe static void AllowAnimation(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            if (client.Player.InUnion)
            {
                client.Player.MyUnion.SendMyInfo(msg, client);
            }
         
            client.Equipment.QueryEquipment(client.Equipment.Alternante);
            data.ObjId = client.Player.UID;
            client.Send(msg.ActionCreate(data));
        }

        [DataAttribute(ActionType.CompleteLogin)]
        public unsafe static void CompleteLogin(Client.GameClient client, ServerSockets.Packet msg, ActionQuery data)
        {
            client.Player.CompleteLogin = true;
            DiscordInfo.Send(client, msg);
            if (client.OnInterServer)
            {
                client.Equipment.QueryEquipment(client.Equipment.Alternante);
                if (client.Player.InitTransfer == 1)
                {
                    client.Player.InitTransfer = 2;
                    Console.WriteLine("Complete Transfer for " + client.Player.Name);
                    client.Send(msg.InterServerCheckTransferCreate(3, client.Player.UID));
                    lock (Pool.NameUsed)
                    {
                        if (Pool.NameUsed.Contains(client.Player.Name.GetHashCode()))
                        {
                            if (!Pool.NameUsed.Contains(client.Player.UID.ToString().GetHashCode()))
                            {
                                Pool.NameUsed.Add(client.Player.UID.ToString().GetHashCode());
                            }
                            client.Player.Name = client.Player.UID.ToString();
                        }
                    }

                }
            }
            else
            {

                client.Player.UpdateVip(msg);
                if (client.Player.MyJiangHu != null)
                    client.Player.MyJiangHu.LoginClient(msg, client);
                else if (client.Player.Reborn == 2)
                {
                    client.Send(msg.JiangHuInfoCreate(MsgJiangHuInfo.JiangMode.IconBar, "0"));
                }
                client.MyWardrobe.SendToClient(msg);
             
                if (client.Player.GuildID == 0 && client.Player.Level > 20)
                {
                    client.SendSysMesage("Why not join a guild. and find some companions in this turbulent world!", MsgMessage.ChatMode.System, MsgMessage.MsgColor.red);
                }
                client.CreatePrestigePoints();

                MsgUserAbilityScore.UserAbilityScore info = new MsgUserAbilityScore.UserAbilityScore();
                info.type = 1;
                info.Level = client.Player.Level;
                info.UID = client.Player.UID;
                info.Items = new MsgUserAbilityScore.AbilityScore[client.PrestigePoints.Length];
                for (int x = 0; x < client.PrestigePoints.Length; x++)
                {
                    info.Items[x] = new MsgUserAbilityScore.AbilityScore();
                    info.Items[x].Position = (uint)(x + 1);
                    info.Items[x].Points = client.PrestigePoints[x];

                }
                client.Send(msg.UserAbilityScoreCreate(info));
            }
            client.Player.CompleteLogin = true;
        }
        [DataAttribute(ActionType.GiveOrder)]
        public unsafe static void GiveOrder(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
            {
                if (client.Player.Map == 1038 || client.Player.Map == 6001)
                {
                    if (client.Player.GuildID == user.Player.GuildID && client.Player.UID != user.Player.UID)
                    {
                        client.Send(msg.ActionCreate(data));
                    }
                }
            }
        }
        [DataAttribute(ActionType.ExchangeClan)]
        public unsafe static void ExchangeClan(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            uint Count = (uint)(data.dwParam3 / 130000);
            if (user.Player.Money >= data.dwParam3)
            {
                user.Player.Money -= data.dwParam3;
                user.Player.CyanJadeRing += Count;
                user.Send(msg.ActionCreate(data));
            }
            
        }
        [DataAttribute(ActionType.ShowFriend)]
        public unsafe static void ShowFriend(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            if (user.Player.Associate.Associat.ContainsKey(Role.Instance.Associate.Friends))
            {
                var Info = new MsgFriendInfo.FriendProtoStructure() { Type = 0, unk1 = 1, Items = new List<MsgFriendInfo.FriendProtoStructure.ConstructsProto>() };
                foreach (var typ in user.Player.Associate.Associat)
                {
                    foreach (var mem in typ.Value.Values)
                    {
                        if (typ.Key == Role.Instance.Associate.Friends)
                        {
                            Client.GameClient Targer;
                            if (Pool.GamePoll.TryGetValue(mem.UID, out Targer))
                                Info.Items.Add(new MsgFriendInfo.FriendProtoStructure.ConstructsProto() { UID = Targer.Player.UID, Online = 1, Mesh = Targer.Player.Mesh, Level = Targer.Player.Level, Name = Targer.Player.Name, Description = Targer.Player.Description, Frame = Targer.Player.FrameID });
                            else
                                Info.Items.Add(new MsgFriendInfo.FriendProtoStructure.ConstructsProto() { UID = mem.UID, Mesh = mem.Mesh, Level = mem.Level, Online = 0, Name = mem.Name, Description = mem.Description, Frame = mem.Frame });
                        }
                    }
                }
                user.Send(msg.CreateFriendInfo(Info));
            }
        }
        [DataAttribute(ActionType.ShowPartner)]
        public unsafe static void ShowPartner(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {

            if (user.Player.Associate.Associat.ContainsKey(Role.Instance.Associate.Partener))
            {
                var Info = new MsgFriendInfo.FriendProtoStructure() { Type = 1, unk1 = 1, Items = new List<MsgFriendInfo.FriendProtoStructure.ConstructsProto>()};
                foreach (var typ in user.Player.Associate.Associat)
                {
                    foreach (var mem in typ.Value.Values)
                    {
                        if (typ.Key == Role.Instance.Associate.Partener)
                        {
                            Client.GameClient Targer;
                            if (Pool.GamePoll.TryGetValue(mem.UID, out Targer))
                                Info.Items.Add(new MsgFriendInfo.FriendProtoStructure.ConstructsProto(){UID = Targer.Player.UID,Mesh = Targer.Player.Mesh,Level = Targer.Player.Level,Online = 1,Name = Targer.Player.Name,Description = Targer.Player.Description});
                            else
                                Info.Items.Add(new MsgFriendInfo.FriendProtoStructure.ConstructsProto(){UID = mem.UID,Mesh = mem.Mesh,Level = mem.Level,Online = 0,Name = mem.Name,Description = mem.Description});
                        }
                    }
                }
                user.Send(msg.CreateFriendInfo(Info));
            }
        }
        [DataAttribute(ActionType.Description)]
        public unsafe static void Description(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            data.ObjId = user.Player.UID;
            user.Player.Description = data.Strings[0].ToString();
            user.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.AnimaCrest)]
        public unsafe static void AnimaCrest(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            if (data.Strings.Contains("</F>User_OpenDialog</N>3833</N>3</N>0</N>4200001"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 3833;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.NpcID = 4200001;
                data.Type = 126;
                data.Fascing = 3;
                data.PositionX = user.Player.X;
                data.PositionY = user.Player.Y;
                user.Send(msg.ActionCreate(data));
            }
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_OpenMain"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 10053;
                data.Type = 258;

                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.Strings = new string[1] { "DlgVeteranBossUi_OpenMainView|{['Data']={['BossIndex']=2,['BossTime']=0,['AllBossState']={0,1,0,0,0,1,1}}}" };

                user.Send(msg.ActionCreate(data));
            }
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_SwitchInterface</N>1"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 10053;
                data.Type = 258;
                data.Fascing = 1;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.Strings = new string[1] { "DlgVeteranBossUi_OpenMainView|{['Data']={['BossIndex']=1,['BossTime']=0,['AllBossState']={0,1,0,0,0,1,1}}}" };

                user.Send(msg.ActionCreate(data));
            }
            if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_FindMonster</N>1"))//
            {
                data.ObjId = user.Player.UID;
                user.Teleport(869, 1128, 10250);

                data.Strings = new string[1] { "DlgVeteranBossUi_Close" };
                user.Send(msg.ActionCreate(data));
            }//
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_SwitchInterface</N>2"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 10053;
                data.Type = 258;
                data.Fascing = 2;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.Strings = new string[1] { "DlgVeteranBossUi_OpenMainView|{['Data']={['BossIndex']=2,['BossTime']=0,['AllBossState']={0,1,0,0,0,1,1}}}" };

                user.Send(msg.ActionCreate(data));
            }
            if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_FindMonster</N>2"))
            {
                data.ObjId = user.Player.UID;
                user.Teleport(1014, 691, 10250);

                data.Strings = new string[1] { "DlgVeteranBossUi_Close" };
                user.Send(msg.ActionCreate(data));
            }//
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_SwitchInterface</N>3"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 10053;
                data.Type = 258;
                data.Fascing = 3;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.Strings = new string[1] { "DlgVeteranBossUi_OpenMainView|{['Data']={['BossIndex']=3,['BossTime']=0,['AllBossState']={0,1,0,0,0,1,1}}}" };
                user.Send(msg.ActionCreate(data));
            }
            if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_FindMonster</N>3"))//
            {
                data.ObjId = user.Player.UID;
                user.Teleport(158, 408, 10250);

                data.Strings = new string[1] { "DlgVeteranBossUi_Close" };
                user.Send(msg.ActionCreate(data));
            }//
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_SwitchInterface</N>4"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 10053;
                data.Type = 258;
                data.Fascing = 4;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.Strings = new string[1] { "DlgVeteranBossUi_OpenMainView|{['Data']={['BossIndex']=4,['BossTime']=0,['AllBossState']={0,1,0,0,0,1,1}}}" };
                user.Send(msg.ActionCreate(data));
            }
            if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_FindMonster</N>4"))
            {
                data.ObjId = user.Player.UID;

                user.Teleport(469, 163, 10250);

                data.Strings = new string[1] { "DlgVeteranBossUi_Close" };
                user.Send(msg.ActionCreate(data));
            }//
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_SwitchInterface</N>5"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 10053;
                data.Type = 258;
                data.Fascing = 5;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.Strings = new string[1] { "DlgVeteranBossUi_OpenMainView|{['Data']={['BossIndex']=5,['BossTime']=0,['AllBossState']={0,1,0,0,0,1,1}}}" };

                user.Send(msg.ActionCreate(data));
            }
            if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_FindMonster</N>5"))
            {
                data.ObjId = user.Player.UID;
                user.Teleport(635, 594, 10250);

                data.Strings = new string[1] { "DlgVeteranBossUi_Close" };
                user.Send(msg.ActionCreate(data));
            }//
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_SwitchInterface</N>6"))
            {
                data.ObjId = user.Player.UID;
                data.dwParam = 10053;
                data.Type = 258;
                data.Fascing = 6;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.Strings = new string[1] { "DlgVeteranBossUi_OpenMainView|{['Data']={['BossIndex']=6,['BossTime']=0,['AllBossState']={0,1,0,0,0,1,1}}}" };

                user.Send(msg.ActionCreate(data));
            }
            if (data.Strings.Contains("</F>ClientManager_Callback</N>10053</S>LakeMonsterDrop_FindMonster</N>6"))
            {
                data.ObjId = user.Player.UID;
                user.Teleport(241, 215, 10250);

                data.Strings = new string[1] { "DlgVeteranBossUi_Close" };
                user.Send(msg.ActionCreate(data));
            }
            else if (data.Strings.Contains("</F>ClientManager_Callback</N>10152</S>ZongFang_ActOpen"))
            {
                data.ObjId = user.Player.UID;
                data.Timestamp = (uint)Time32.timeGetTime().GetHashCode();
                data.NpcID = 10152;
                data.Type = 126;
                data.Fascing = 3;
                data.PositionX = user.Player.X;
                data.PositionY = user.Player.Y;
                user.Send(msg.ActionCreate(data));
            }
           
        }
        [DataAttribute(ActionType.ReturnReward)]
        public unsafe static void ReturnReward(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            if (data.dwParam == 1)
            {
                if (user.Inventory.HaveSpace(2))
                {
                    user.Inventory.Add(msg, 730005, 1, 0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, true);
                    user.Inventory.Add(msg, 3322761, 1, 0, 0, 0, Role.Flags.Gem.NoSocket, Role.Flags.Gem.NoSocket, true);

                    user.Send(msg.ActionCreate(data));
                }
            }
            else
            {
                if (user.Inventory.HaveSpace(2))
                {
                    user.Inventory.AddItemWitchStack(3003126, 0, 30, msg, true);
                    user.Inventory.AddItemWitchStack(3002926, 0, 30, msg, true);
                    user.Inventory.AddItemWitchStack(3002030, 0, 30, msg, true);
                    user.Send(msg.ActionCreate(data));
                }
            }
            

        }
        [DataAttribute(ActionType.GetPrizeFlower)]
        public unsafe static void GetPrizeFlower(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            if (!user.Player.HaloTitles.Contains((MsgTitleStorage.HaloType)data.PositionX))
            {
                user.Player.AddSpecialHalo((Game.MsgServer.MsgTitleStorage.HaloType)data.PositionX, msg);
            }
            if (user.Player.HaloTitles.Contains((MsgTitleStorage.HaloType)data.PositionX))
            {
                Database.TitleStorage dbtitle;
                if (Database.TitleStorage.Titles.TryGetValue(data.PositionX, out dbtitle))
                {
                    user.Player.FairyForm = data.dwParam2;

                    user.Player.SendUpdate(msg, 0, 0, user.Player.TodayFlowerType, data.dwParam2, MsgUpdate.DataType.FlowerRank, true);

                    if (data.dwParam2 > 0)
                    {
                        user.Player.SpecialTitleScore = dbtitle.Score;
                        user.Player.SpecialHaloID = (uint)(dbtitle.ID * 10000 + dbtitle.SubID);
                        var pQuery = new MsgTitleStorage.TitleStorage();
                        pQuery.ActionID = MsgTitleStorage.Action.Equip;
                        pQuery.dwparam2 = data.PositionX;
                        pQuery.dwparam3 = data.PositionY;
                        user.Send(msg.CreateTitleStorage(pQuery));
                        pQuery.ActionID = MsgTitleStorage.Action.UseTitle;
                        pQuery.Title = new MsgTitleStorage.Title();
                        pQuery.Title.ID = pQuery.dwparam2;
                        pQuery.Title.SubId = pQuery.dwparam3;
                        pQuery.Title.dwparam1 = 1;
                        user.Send(msg.CreateTitleStorage(pQuery));
                        user.Player.View.SendView(user.Player.GetArray(msg, false), false);
                    }
                    else 
                    {

                        user.Player.SpecialTitleScore = user.Player.SpecialHaloID = 0;
                        var pQuery = new MsgTitleStorage.TitleStorage();
                        pQuery.ActionID = MsgTitleStorage.Action.UnEquip;
                        pQuery.dwparam2 = data.PositionX;
                        pQuery.dwparam3 = data.PositionY;
                        user.Send(msg.CreateTitleStorage(pQuery));
                        user.Player.View.Clear(msg);
                        user.Player.View.Role(false);
                        user.Player.View.SendView(user.Player.GetArray(msg, false), false);
                    }
                }
            }
                      
        }
        [DataAttribute(ActionType.DunePower)]
        public unsafe static void DunePower(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            if (user.Player.ContainFlag(MsgUpdate.Flags.BloodThirst))
            {
                user.Player.RemoveFlag(MsgUpdate.Flags.BloodThirst);
            }

            user.Player.AddSpellFlag(MsgUpdate.Flags.Dune, 10, true);
            user.Player.AddSpellFlag(MsgUpdate.Flags.BloodThirst, 10, true);
            user.Player.SendUpdate(msg, MsgUpdate.Flags.ConceptionVessal, 10, 1, 0, 10, 0, MsgUpdate.DataType.ArchiveSkill);
            user.Player.SendUpdate(msg, MsgUpdate.Flags.BloodThirst, 10, 65, 0, MsgUpdate.DataType.ArchiveSkill);
            user.Player.DuneEnergy = 0;
            user.Player.DuneEnergy2 = 0;
            user.Player.DuneEnergy3 = 0;
            Game.MsgServer.AttackHandler.DuneSkills.UpdateEnergy(user);
            user.Equipment.QueryEquipment(user.Equipment.Alternante);
            user.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.DuelAction)]
        public unsafe static void DuelAction(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            if (user.Player.ContainFlag(MsgUpdate.Flags.PortadorRuneDuel))
                user.Player.RemoveFlag(MsgUpdate.Flags.PortadorRuneDuel);
            var Duel = Pool.Magic[(ushort)Role.Flags.SpellID.Duel][user.MySpells.ClientSpells[(ushort)Role.Flags.SpellID.Duel].Level];
            Client.GameClient attacked;
            if (Pool.GamePoll.TryGetValue(user.Player.DuelUID, out attacked))
            {
                user.Player.DuelUID = 0;
                attacked.Player.AddFlag(MsgUpdate.Flags.Duel, (int)Duel.DamageOnHuman, true);
                attacked.Player.DuelPercent = (byte)Duel.Damage;
            }
            user.Send(msg.ActionCreate(data));
        }
        [DataAttribute(ActionType.ViewNinjaInspired)]
        public unsafe static void ViewNinjaInspired(Client.GameClient user, ServerSockets.Packet msg, ActionQuery data)
        {
            Client.GameClient Target;
            if (Pool.GamePoll.TryGetValue((uint)data.TargetUID, out Target))
            {
                if (Target.MyNinja != null)
                {
                    VirusX.Game.MsgServer.MsgGouYuInfo.MsgGouYuInfoProto.Create(user, Target.MyNinja, VirusX.Game.MsgServer.MsgGouYuInfo.MsgGouYuInfoProto.TypeID.View, Target.Player.UID);
                    MsgGouYuAptitude.MsgGouYuAptitudeProto.Create(user, Target.MyNinja, Target.Player.UID);
                }
                if (Target.HundredWeapons != null)
                {
                    if (Pool.GamePoll.ContainsKey(data.TargetUID))
                        user.Send(msg.CreateHundredWeaponsInfo(Pool.GamePoll[data.TargetUID], MsgHundredWeaponsInfo.ActionID.RequestInfo));
                    else
                        user.Send(msg.CreateHundredWeaponsInfo(null, MsgHundredWeaponsInfo.ActionID.RequestInfo));
                }
                if (Target.MyArchives != null
                    && (AtributesStatus.IsMonk(Target.Player.Class)
                    || AtributesStatus.IsThunderStriker(Target.Player.Class) 
                    || AtributesStatus.IsWindWalker(Target.Player.Class) || AtributesStatus.IsDune(Target.Player.Class)))
                {

                    Archives.Item[] MyArchvies = null;
                    MyArchvies = Target.MyArchives.Items.Values.Where(p => p.ItemID >= Archives.TypeID.Dragonhowl && p.ItemID <= Archives.TypeID.Suanni).ToArray();



                    if (MyArchvies != null)
                    {
                        VirusX.Game.MsgServer.MsgCombatGear.ProtoStructure obj = new VirusX.Game.MsgServer.MsgCombatGear.ProtoStructure();
                        obj.Type = (byte)VirusX.Game.MsgServer.MsgCombatGear.ProtoStructure.Action.View;
                        obj.UID = Target.Player.UID;
                        obj.Items = new VirusX.Game.MsgServer.MsgCombatGear.ProtoStructure.ConstructsProto[Target.MyArchives.Items.Count];
                        int i = 0; foreach (var Item in MyArchvies)
                        {
                            obj.Items[i] = new VirusX.Game.MsgServer.MsgCombatGear.ProtoStructure.ConstructsProto();
                            obj.Items[i].ID = (ushort)Item.ItemID;
                            obj.Items[i].Level = (byte)Item.Level;
                            obj.Items[i].Progress = Item.Progress;
                            obj.Items[i].Hash = Item.Hash;
                            obj.Items[i].dwParam = Item.dwParam;
                            obj.Items[i].jade1 = (uint)Item.Jades[0].JadeID;
                            obj.Items[i].jade2 = (uint)Item.Jades[1].JadeID;
                            obj.Items[i].jade3 = (uint)Item.Jades[2].JadeID;
                            obj.Items[i].jade4 = (uint)Item.Jades[3].JadeID;
                            obj.Items[i].jade5 = (uint)Item.Jades[4].JadeID;
                            obj.Items[i].jade6 = (uint)Item.Jades[5].JadeID;
                            obj.Items[i].MasteryPoints = Item.MasteryPoints;
                            obj.Items[i].Debris = Item.Debris;
                            i++;
                        }
                        user.Send(msg.CreateArchives(obj));

                    }
                }

            }
            user.Send(msg.ActionCreate(data));
        }
    }
}