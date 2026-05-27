using System;
using VirusX.Client;
using VirusX.ServerSockets;
using ProtoBuf;
using VirusX.Game.MsgServer;
using System.Collections.Generic;
using VirusX.Role;
using VirusX.Role.Instance;
using VirusX.Database;
using VirusX.DBFunctionality;

namespace VirusX
{

    [ProtoContract]
    public class MsgTwistedFututr
    {
        [ProtoMember(1, IsRequired = true)]
        public uint Type;

        [ProtoMember(2, IsRequired = true)]
        public uint Level;

        [ProtoMember(3, IsRequired = true)]
        public uint Exp;

        [ProtoMember(4, IsRequired = true)]
        public uint SelectMaterial;

        [ProtoMember(5, IsRequired = true)]
        public ulong AirPower;

        [ProtoMember(6, IsRequired = true)]
        public uint Unlocked;

        [ProtoMember(7, IsRequired = true)]
        public Role.Instance.Archives.TwistedFututr SkillID;

        [ProtoMember(8, IsRequired = true)]
        public bool Unlocked2;

        public Archives.TwistedFututr ItemID;
        public System.Collections.Concurrent.ConcurrentDictionary<Archives.TwistedFututr, MsgTwistedFututr> Items;
        public static byte Flag;
        [Flags]
        public enum TypeID : uint
        {
            Upgrade = 3,
            Active = 6,
        }
        public static Archives.items itemse;

        public static bool CheckSpells(ushort ID)
        {
            return ID == (ushort)Flags.SpellID.Disorder
                || ID == (ushort)Flags.SpellID.ApePistol
                || ID == (ushort)Flags.SpellID.BearsCare;
        }

        public ServerSockets.Packet ToArray()
        {
            ServerSockets.Packet result;
            using (RecycledPacket rec = new RecycledPacket())
            {
                ServerSockets.Packet stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(this);
                stream.Finalize(Game.GamePackets.TwistedFututr);
                result = stream;
            }
            return result;
        }


        public static implicit operator ServerSockets.Packet(MsgTwistedFututr obj)
        {
            return obj.ToArray();
        }
        public static void Loading(GameClient client)
        {

            if (Database.AtributesStatus.IsPirate(client.Player.Class))
            {
                MsgTwistedFututr info = new MsgTwistedFututr();

                info.Type = 1u;
                info.Level = client.Player.TwistedFututrLevel;
                info.Exp = client.Player.TwistedFututrExp;
                info.AirPower = client.Player.TwistedFututrAirPower;
                info.Unlocked = 1U;
                info.Unlocked2 = true;
                using (RecycledPacket rec = new RecycledPacket())
                {
                    ServerSockets.Packet stream = rec.GetStream();
                    client.Send(info);
                }
            }
        }
        public unsafe static void Execute(Client.GameClient user, InteractQuery Attack, ServerSockets.Packet stream, Dictionary<ushort, Database.MagicType.Magic> DBSpells)
        {
            Database.MagicType.Magic DBSpell;
            MsgSpell ClientSpell;
            VirusX.Game.MsgServer.AttackHandler.CheckAttack.CanUseSpell.Verified(Attack, user, DBSpells, out ClientSpell, out DBSpell);
            {
                switch (ClientSpell.ID)
                {
                    case (ushort)Flags.SpellID.BearsCare:
                        {
                            user.Send(stream.InteractionCreate(Attack));
                            if (!user.Player.ContainFlag(MsgUpdate.Flags.BearsCare))
                            {
                                user.Player.AddSpellFlag(MsgUpdate.Flags.BearsCare, (int)5, true);
                                user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage / 10, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);
                                MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, user.Player.UID, 0, 0, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                MsgSpell.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                MsgSpell.SetStream(stream);
                                MsgSpell.Send(user);
                            }
                            break;
                        }
                    case (ushort)Flags.SpellID.Disorder:
                        {

                            user.Send(stream.InteractionCreate(Attack));

                            user.Player.AddSpellFlag(MsgUpdate.Flags.Disorder, (int)DBSpell.Second, true);
                            MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID, 0, Attack.X, Attack.Y, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                            if (!user.Player.FloorSpells.ContainsKey(ClientSpell.ID))
                            {
                                user.Player.FloorSpells.TryAdd(ClientSpell.ID, new Role.FloorSpell.ClientFloorSpells(user.Player.UID, user.Player.X, user.Player.Y, ClientSpell.SoulLevel, DBSpell, user.Map));
                            }
                            var FloorItem = new Role.FloorSpell(Game.MsgFloorItem.MsgItemPacket.Disorder, (ushort)Attack.X, (ushort)Attack.Y, 34, DBSpell, 4000, 1000);

                            FloorItem.FloorPacket.m_X = Attack.X;
                            FloorItem.FloorPacket.m_Y = Attack.Y;
                            FloorItem.FloorPacket.OwnerX = user.Player.X;
                            FloorItem.FloorPacket.OwnerY = user.Player.Y;
                            FloorItem.FloorPacket.GuildID = user.Player.GuildID;
                            FloorItem.FloorPacket.ItemOwnerUID = user.Player.UID;
                            FloorItem.FloorPacket.m_Color = 34;
                            FloorItem.FloorPacket.Name = "STR_TRAP_ID_@@" + (uint)FloorItem.FloorPacket.m_ID;
                            user.Player.FloorSpells[ClientSpell.ID].AddItem(FloorItem);
                            MsgSpell.SetStream(stream);
                            MsgSpell.Send(user);
                            break;
                        }
                    case (ushort)Role.Flags.SpellID.ApePistol:
                        {
                            if (!user.Player.ContainFlag(MsgUpdate.Flags.ApePistol))
                            {
                                user.Player.AddSpellFlag(MsgUpdate.Flags.ApePistol, (int)DBSpell.Duration, true);
                                user.Player.SendUpdate(stream, (MsgUpdate.Flags)DBSpell.Status, (uint)DBSpell.Duration, (uint)DBSpell.GDamage / 10, (uint)0, MsgUpdate.DataType.ArchiveSkill, true);
                                MsgSpellAnimation MsgSpells = new MsgSpellAnimation(user.Player.UID, user.Player.UID, 0, 0, ClientSpell.ID, ClientSpell.Level, ClientSpell.UseSpellSoul);
                                MsgSpells.Targets.Enqueue(new MsgSpellAnimation.SpellObj(user.Player.UID, 0, MsgAttackPacket.AttackEffect.None));
                                user.Player.ApePistolSkill = 0;
                                MsgSpells.SetStream(stream);

                                MsgSpells.Send(user);

                                break;
                            }
                            if (user.Player.ContainFlag(MsgUpdate.Flags.ApePistol))
                            {
                                if (user.Player.ApePistolSkill < 4)
                                {
                                    MsgSpellAnimation MsgSpell = new MsgSpellAnimation(user.Player.UID
                                                       , 0, Attack.X, Attack.Y, ClientSpell.ID
                                                       , ClientSpell.Level, ClientSpell.UseSpellSoul);

                                    VirusX.Game.MsgServer.AttackHandler.Algoritms.InLineAlgorithm Line = new VirusX.Game.MsgServer.AttackHandler.Algoritms.InLineAlgorithm(user.Player.X, Attack.X, user.Player.Y, Attack.Y, user.Map, DBSpell.Range, 0);


                                    uint Experience = 0;
                                    foreach (Role.IMapObj target in user.Player.View.Roles(Role.MapObjectType.Monster))
                                    {
                                        if (Line.InLine(target.X, target.Y, 2))
                                        {
                                            VirusX.Game.MsgMonster.MonsterRole attacked = target as VirusX.Game.MsgMonster.MonsterRole;
                                            if (VirusX.Game.MsgServer.AttackHandler.CheckAttack.CanAttackMonster.Verified(user, attacked, DBSpell))
                                            {
                                                MsgSpellAnimation.SpellObj AnimationObj;
                                                VirusX.Game.MsgServer.AttackHandler.Calculate.Physical.OnMonster(user.Player, attacked, DBSpell, out AnimationObj);

                                                Experience += VirusX.Game.MsgServer.AttackHandler.ReceiveAttack.Monster.Execute(stream, AnimationObj, user, attacked);
                                                MsgSpell.Targets.Enqueue(AnimationObj);
                                            }
                                        }
                                    }
                                    foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.Player))
                                    {
                                        if (Line.InLine(targer.X, targer.Y, 2))
                                        {
                                            var attacked = targer as Role.Player;
                                            if (VirusX.Game.MsgServer.AttackHandler.CheckAttack.CanAttackPlayer.Verified(user, attacked, DBSpell))
                                            {

                                                MsgSpellAnimation.SpellObj AnimationObj;
                                                VirusX.Game.MsgServer.AttackHandler.Calculate.Physical.OnPlayer(user.Player, attacked, DBSpell, out AnimationObj);

                                                VirusX.Game.MsgServer.AttackHandler.ReceiveAttack.Player.Execute(AnimationObj, user, attacked);
                                                MsgSpell.Targets.Enqueue(AnimationObj);


                                            }
                                        }

                                    }
                                    foreach (Role.IMapObj targer in user.Player.View.Roles(Role.MapObjectType.SobNpc))
                                    {
                                        if (Line.InLine(targer.X, targer.Y, 2))
                                        {
                                            var attacked = targer as Role.SobNpc;
                                            if (VirusX.Game.MsgServer.AttackHandler.CheckAttack.CanAttackNpc.Verified(user, attacked, DBSpell))
                                            {
                                                MsgSpellAnimation.SpellObj AnimationObj;
                                                VirusX.Game.MsgServer.AttackHandler.Calculate.Physical.OnNpcs(user.Player, attacked, DBSpell, out AnimationObj);

                                                Experience += VirusX.Game.MsgServer.AttackHandler.ReceiveAttack.Npc.Execute(stream, AnimationObj, user, attacked);
                                                MsgSpell.Targets.Enqueue(AnimationObj);
                                            }
                                        }
                                    }
                                    if (user.Player.ApePistolSkill < 4)
                                    {
                                        user.Player.ApePistolSkill += 1;
                                    }
                                    if (user.Player.ApePistolSkill >= 3)
                                    {
                                        user.Player.RemoveFlag(MsgUpdate.Flags.ApePistol);
                                    }
                                    VirusX.Game.MsgServer.AttackHandler.Updates.IncreaseExperience.Up(stream, user, Experience);
                                    VirusX.Game.MsgServer.AttackHandler.Updates.UpdateSpell.CheckUpdate(stream, user, Attack, Experience, DBSpells);
                                    MsgSpell.bomb = 2;
                                    MsgSpell.SetStream(stream);
                                    MsgSpell.SendRole(user);
                                    MsgSpell.Send(user);
                                }
                            }
                            break;
                        }
                }
            }
        }

        [Packet(Game.GamePackets.TwistedFututr)]
        private static void Process(GameClient client, ServerSockets.Packet stream)
        {

            MsgTwistedFututr Info = new MsgTwistedFututr();
            Info = stream.ProtoBufferDeserialize<MsgTwistedFututr>(Info);
            switch (Info.Type)
            {

                case 1:
                case 2:
                case 7:
                case 8:
                    {
                        client.Send(Info);
                        break;
                    }
                case (uint)MsgTwistedFututr.TypeID.Upgrade:
                    {
                        if (client.MyArchives.AirPower <= 0)
                        {
                            break;
                        }
                        if (client.Player.TwistedFututrLevel == 0 && Info.SelectMaterial < 8000)
                        {
                            client.CreateBoxDialog("You need to put 8000 AirPower Points to open 1st Stage.");
                            break;
                        }
                        #region New
                        if (client.MyArchives.AirPower >= Info.SelectMaterial)
                        {
                            client.MyArchives.AirPower -= Info.SelectMaterial;
                            Info.Exp = client.Player.TwistedFututrExp;
                            client.Player.TwistedFututrExp += Info.SelectMaterial;

                            client.Player.TwistedFututrAirPower += Info.SelectMaterial;

                            Info.Exp = client.Player.TwistedFututrExp;
                            Info.AirPower = client.Player.TwistedFututrAirPower;
                            if (client.Player.TwistedFututrExp >= 8000 && client.Player.TwistedFututrLevel == 0)
                            {
                                client.Player.TwistedFututrExp = 0;
                                client.Player.TwistedFututrLevel = 1;
                                Info.Level = 1;
                            }
                            else if (client.Player.TwistedFututrExp >= 7500 && client.Player.TwistedFututrLevel == 1)
                            {
                                client.Player.TwistedFututrExp = 0;
                                client.Player.TwistedFututrLevel = 2;
                                Info.Level = 2;
                            }
                            else if (client.Player.TwistedFututrExp >= 7500 && client.Player.TwistedFututrLevel == 2)
                            {
                                client.Player.TwistedFututrExp = 0;
                                client.Player.TwistedFututrLevel = 3;
                                Info.Level = 3;
                            }
                            else if (client.Player.TwistedFututrExp >= 7500 && client.Player.TwistedFututrLevel == 3)
                            {
                                client.Player.TwistedFututrExp = 0;
                                client.Player.TwistedFututrLevel = 4;
                                Info.Level = 4;
                            }
                            else if (client.Player.TwistedFututrExp >= 50000)
                            {
                                client.Player.TwistedFututrLevel = 5;
                                Info.Level = 5;
                            }
                            client.MyArchives.UpdatePoints(client.MyArchives.AirPower);
                            client.Send(Info);
                            Save(client);
                            MsgTwistedFututr.Loading(client);
                        }
                        #endregion
                        break;
                    }
                case (uint)MsgTwistedFututr.TypeID.Active:
                    {
                        switch (Info.SkillID)
                        {
                            case Archives.TwistedFututr.Disorder:
                                {
                                    if (!client.MySpells.ClientSpells.ContainsKey((ushort)24900))
                                    {
                                        if (client.MySpells.ClientSpells.ContainsKey((ushort)24910))
                                        {
                                            client.MySpells.Remove((ushort)24910, stream);
                                        }

                                        if (client.MySpells.ClientSpells.ContainsKey((ushort)24920))
                                        {
                                            client.MySpells.Remove((ushort)24920, stream);
                                        }

                                        client.MySpells.Add(stream, (ushort)24900);
                                        client.Send(Info);
                                        Info.Unlocked2 = true;
                                    }

                                    break;
                                }
                            case Archives.TwistedFututr.ApePistol:
                                {
                                    if (!client.MySpells.ClientSpells.ContainsKey((ushort)24910))
                                    {
                                        if (client.MySpells.ClientSpells.ContainsKey((ushort)24900))
                                        {
                                            client.MySpells.Remove((ushort)24900, stream);
                                        }

                                        if (client.MySpells.ClientSpells.ContainsKey((ushort)24920))
                                        {
                                            client.MySpells.Remove((ushort)24920, stream);
                                        }

                                        client.MySpells.Add(stream, (ushort)24910);
                                        Info.Unlocked2 = true;
                                        client.Send(Info);

                                    }

                                    break;
                                }
                            case Archives.TwistedFututr.BearsCare:
                                {
                                    if (!client.MySpells.ClientSpells.ContainsKey((ushort)24920))
                                    {
                                        if (client.MySpells.ClientSpells.ContainsKey((ushort)24910))
                                        {
                                            client.MySpells.Remove((ushort)24910, stream);
                                        }

                                        if (client.MySpells.ClientSpells.ContainsKey((ushort)24900))
                                        {
                                            client.MySpells.Remove((ushort)24900, stream);
                                        }

                                        client.MySpells.Add(stream, (ushort)24920);
                                    }
                                    client.Send(Info);
                                    Info.Unlocked2 = true;
                                    break;
                                }
                        }
                        MsgTwistedFututr.Loading(client);

                        client.Send(Info);
                        break;
                    }
            }
        }
        #region Save

        public static void Save(Client.GameClient client)
        {

            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\PlayersTwistedFututr\\" + client.Player.UID + ".ini");
            write.Write("TwistedFututr", "Flag", Flag);

            write.Write<uint>("TwistedFututr", "ActiveSkill", (uint)client.Player.TwistedFututrLevelSkill);
            write.Write("TwistedFututr", "SelectMaterial", client.Player.TwistedFututrSelectMaterial);
            write.Write("TwistedFututr", "Exp", client.Player.TwistedFututrExp);
            write.Write("TwistedFututr", "AirPower", client.Player.TwistedFututrAirPower);
            write.Write("TwistedFututr", "Level", client.Player.TwistedFututrLevel);
            write.Write("TwistedFututr", "Unlocked", client.Player.TwistedFututrUnlocked);

        }
        #endregion
        #region Load
        public static void Load(Client.GameClient client)
        {
            // var iniFile = new IniFile("\\PlayersTwistedFututr\\" + UID + ".ini");
            WindowsAPI.IniFile reader = new WindowsAPI.IniFile("\\PlayersTwistedFututr\\" + client.Player.UID + ".ini");
            Flag = reader.ReadByte("TwistedFututr", "Flag", 0);
            client.Player.TwistedFututrLevelSkill = (uint)(Archives.TwistedFututr)reader.ReadUInt32("TwistedFututr", "ActiveSkill", 0);
            client.Player.TwistedFututrSelectMaterial = reader.ReadByte("TwistedFututr", "SelectMaterial", 0);
            client.Player.TwistedFututrExp = reader.ReadUInt32("TwistedFututr", "Exp", 0);
            client.Player.TwistedFututrAirPower = reader.ReadByte("TwistedFututr", "AirPower", 0);
            client.Player.TwistedFututrLevel = reader.ReadUInt32("TwistedFututr", "Level", 0);
            client.Player.TwistedFututrUnlocked = reader.ReadUInt32("TwistedFututr", "Unlocked", 0);//bool
            //if (!Items.ContainsKey(Twisted.ItemID))
            //    Items.TryAdd(Twisted.ItemID, Twisted);
        }
        #endregion
    }
}