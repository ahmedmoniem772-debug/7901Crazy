using VirusX.Client;
using VirusX.Database;
using VirusX.Database.DBActions;
using VirusX.Game.MsgServer;
using VirusX.Game.MsgServer.AttackHandler;
using VirusX.Game.MsgServer.AttackHandler.Calculate;
using VirusX.ServerSockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace VirusX.Role.Instance
{
    public class Astredge
    {
        [Flags]
        public enum TypeID : uint
        {
            None = 0,
            ViodragonClub = 1,
            LoveForEver = 2,
            HeartLock = 3,
        }
        public ConcurrentDictionary<TypeID, Item> Items;
        public Client.GameClient user;

        public Astredge(Client.GameClient _user)
        {
            user = _user;
            Items = new ConcurrentDictionary<TypeID, Item>();
        }
        public class Item
        {
            public uint UserID;

            public TypeID ItemID;

            public byte Level;

            public uint Progress;

            public god_weapons_exp.EXPAstredge DBItem
            {
                get
                {
                    god_weapons_exp.EXPAstredge Type;
                    god_weapons_exp.EXPAstredge Item;
                    if (!god_weapons_exp.TryGetValue((uint)this.ItemID, this.Level, out Type))
                    {
                        Item = new god_weapons_exp.EXPAstredge();
                    }
                    else
                    {
                        Item = Type;
                    }
                    return Item;
                }
            }
            public god_weapons_type.ItemsAstredge Astredge
            {
                get
                {
                    god_weapons_type.ItemsAstredge Type;
                    god_weapons_type.ItemsAstredge Item;
                    if (!god_weapons_type.TryGetValue((uint)this.ItemID, this.Level, out Type))
                    {
                        Item = new god_weapons_type.ItemsAstredge();
                    }
                    else
                    {
                        Item = Type;
                    }
                    return Item;
                }
            }
            public god_weapons_exp.EXPAstredge DBItemLevel
            {
                get
                {
                    god_weapons_exp.EXPAstredge Type;
                    god_weapons_exp.EXPAstredge Item;
                    if (!god_weapons_exp.TryGetValue((uint)this.ItemID, this.Level, out Type))
                    {
                        Item = new god_weapons_exp.EXPAstredge();
                    }
                    else
                    {
                        Item = Type;
                    }
                    return Item;
                }
            }

        }
        public void AddItem(TypeID ID, byte Level)
        {
            if (Level > 20)
            {
                Level = 20;
            }
            if (ID == TypeID.ViodragonClub || ID == TypeID.LoveForEver || ID == TypeID.HeartLock)
            {
                if (!Items.ContainsKey(ID))
                {

                    god_weapons_exp.EXPAstredge DBitem;
                    if (god_weapons_exp.TryGetValue((uint)ID, (byte)(Level > 1 ? Level - 1 : Level), out  DBitem))
                    {
                        Item item = new Item
                        {

                            UserID = user.Player.UID,
                            ItemID = ID,
                            Level = Level
                        };
                        Items.Add(ID, item);
                        GET_Skill(item.ItemID);
                        CMsgGodWeapons.CMsgGodWeaponsPB.Add(user, item);
                        using (ServerSockets.RecycledPacket rec = new ServerSockets.RecycledPacket())
                        {
                            ServerSockets.Packet stream = rec.GetStream();

                            var OpenDailog = new ActionQuery() { ObjId = user.Player.UID, dwParam = 3856, Type = 126, PositionX = user.Player.X, PositionY = user.Player.Y };
                            user.Send(stream.ActionCreate(OpenDailog));
                        }
                    }
                }
            }
        }
        public void GET_Skill(TypeID ID)
        {
            using (ServerSockets.RecycledPacket rec = new ServerSockets.RecycledPacket())
            {
                ServerSockets.Packet stream = rec.GetStream();
                if (Items.ContainsKey(ID))
                {
                    Astredge.Item[] MyAstredge = null;
                    MyAstredge = user.MyAstredge.Items.Values.Where(p => p.ItemID >= TypeID.ViodragonClub && p.ItemID <= TypeID.HeartLock).ToArray();
                    if (MyAstredge != null)
                    {
                        foreach (var Item in MyAstredge)
                        {
                            god_weapons_type.ItemsAstredge Itemss;
                            if (Database.god_weapons_type.TryGetValue((uint)Item.ItemID, Item.Level, out Itemss))
                            {
                                user.MySpells.Add(stream, (ushort)(Itemss.SkillFirst[0]), (byte)(Itemss.SkillFirst[1]));
                                user.MySpells.Add(stream, (ushort)(Itemss.SkillMele[0]), (byte)(Itemss.SkillMele[1]));
                            }
                        }
                    }
                    
                   
                }
            }
        }
        public void Close()
        {
            using (var recycledPacket = new RecycledPacket())
            {
                var stream = recycledPacket.GetStream();
                user.Send(stream.CreateCMsgGodWeapons(new CMsgGodWeapons.CMsgGodWeaponsPB()
                {
                    Action = CMsgGodWeapons.CMsgGodWeaponsPB.ActionID.Close2
                }));
                if (user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.ChaoticDance))
                    user.MySpells.Remove((ushort)Role.Flags.SpellID.ChaoticDance, stream);
                if (user.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.ChaoticDanceAttack))
                    user.MySpells.Remove((ushort)Role.Flags.SpellID.ChaoticDanceAttack, stream);
            }
        }

        public uint ViodtagonClub
        {
            get
            {
                uint Score = 0;
                foreach (var obj in Items.Values)
                {
                    if (obj.ItemID == Astredge.TypeID.ViodragonClub)
                        Score += obj.Progress;
                }
                return Score;
            }
        }
        public uint HeartLock
       {
            get
            {
                uint Score = 0;
                foreach (var obj in Items.Values)
                {
                    if (obj.ItemID == Astredge.TypeID.HeartLock)
                        Score += obj.Progress;
                }
                return Score;
            }
        }
        public uint LoveForEver
        {
            get
            {
                uint Score = 0;
                foreach (var obj in Items.Values)
                {
                    if (obj.ItemID == Astredge.TypeID.LoveForEver)
                        Score += obj.Progress;
                }
                return Score;
            }
        }
        public uint WeeklyScore
        {
            get
            {
                uint Score = 0;
                foreach (var obj in Items.Values)
                {
                    Score += obj.Progress;
                }
                return Score;
            }
        }
        
        public void UpdateRank()
        {
            #region ViodtagonClub

            if (ViodtagonClub >= 5000)
            {
                var Ranks = new AstredgeRank.Entry()
                {
                    Type = AstredgeRank.Type.ViodragonClub,
                    TotalPoints = this.ViodtagonClub,
                    UID = user.Player.UID,
                    Name = user.Player.Name,
                    Level = (byte)user.Player.Level,
                    Class = user.Player.Class,
                    Mesh = user.Player.Mesh
                };
                Ranks.AddInfo(user);
                AstredgeRank.Ranks[AstredgeRank.Type.ViodragonClub].UpdateItem(Ranks);
            }
            if (LoveForEver >= 5000)
            {
                var Ranks = new AstredgeRank.Entry()
                {
                    Type = AstredgeRank.Type.LoveForEver,
                    TotalPoints = this.LoveForEver,
                    UID = user.Player.UID,
                    Name = user.Player.Name,
                    Level = (byte)user.Player.Level,
                    Class = user.Player.Class,
                    Mesh = user.Player.Mesh
                };
                Ranks.AddInfo(user);
                AstredgeRank.Ranks[AstredgeRank.Type.LoveForEver].UpdateItem(Ranks);
            }
            if (HeartLock >= 5000)
            {
                var Ranks = new AstredgeRank.Entry()
                {
                    Type = AstredgeRank.Type.HeartLock,
                    TotalPoints = this.LoveForEver,
                    UID = user.Player.UID,
                    Name = user.Player.Name,
                    Level = (byte)user.Player.Level,
                    Class = user.Player.Class,
                    Mesh = user.Player.Mesh
                };
                Ranks.AddInfo(user);
                AstredgeRank.Ranks[AstredgeRank.Type.HeartLock].UpdateItem(Ranks);
            }
            #endregion
        }
        public void Loading()
        {
            var Item = user.MyAstredge.Items.Values.Where(p => p.ItemID >= TypeID.ViodragonClub && p.ItemID <= TypeID.LoveForEver).ToArray();
            if (Item != null)
            {
                foreach (var Itemss in Items.Values)
                {
                    #region LoadAstredge(Info)
                    if (user.Player.Reborn == 2 && user.Player.Level >= 120)
                    {
                        var Astredge = new CMsgGodWeapons.CMsgGodWeaponsPB()
                        {
                            Action = CMsgGodWeapons.CMsgGodWeaponsPB.ActionID.Login,
                            AstredgeInfo = new List<CMsgGodWeapons.InfoAstredge>(),
                        };
                        Astredge.AstredgeInfo.Add(new CMsgGodWeapons.InfoAstredge() 
                        { Type = Itemss.ItemID, AstredgeLevel = Itemss.Level, AstredgeProgress = Itemss.Progress, GUISKILL = 1, Unk5 = 0 });
                        Astredge.Items[0] = 7803;
                        Astredge.Items[1] = 7819;
                        Astredge.Items[2] = 7845;
                        Astredge.Unk5 = 11;
                        Close();
                        GET_Skill(Itemss.ItemID);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            user.Send(stream.CreateCMsgGodWeapons(Astredge));
                        }

                    }
                    else
                    {
                        Close();
                    }
                }
                #endregion
            }
        }
        public override string ToString()
        {
            WriteLine file = new WriteLine('/');
            file.Add(Items.Count);
            foreach (Item att in Items.Values)
            {
                file.Add((uint)att.ItemID);
                file.Add(att.Level);
                file.Add(att.Progress);
            }
            return file.Close();
        }
        public void Load(string line)
        {
            ReadLine reader = new ReadLine(line, '/');
            int count = reader.Read(0);
            for (int i = 0; i < count; i++)
            {
                Item item = new Item
                {
                    ItemID = (Astredge.TypeID)reader.Read((uint)0),
                    Level = reader.Read((byte)0),
                    Progress = reader.Read((uint)0),
                };
                if (!Items.ContainsKey(item.ItemID))
                {
                    Items.Add(item.ItemID, item);
                }
            }

        }
    }
}
