using VirusX.Client;
using VirusX.Database;
using VirusX.Role.Instance;
using ProtoBuf;
using System;
using System.Linq;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateArchives(this ServerSockets.Packet stream, MsgCombatGear.ProtoStructure obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgCombatGear);
            return stream;

        }

        public static void GetArchives(this ServerSockets.Packet stream, out MsgCombatGear.ProtoStructure pQuery)
        {
            pQuery = new MsgCombatGear.ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<MsgCombatGear.ProtoStructure>(pQuery);
        }

    }

    public class MsgCombatGear
    {
        [ProtoContract]
        public class ProtoStructure
        {
            [Flags]
            public enum Action
            {
                Load = 0,
                Add = 2,
                Update = 3,
                View = 4,
                DunePoints = 5,
                DuneUpdate = 6,
            }
            [ProtoMember(1, IsRequired = true)]
            public byte Type;
            [ProtoMember(2, IsRequired = true)]
            public uint UID;
            [ProtoMember(3, IsRequired = true)]
            public ConstructsProto[] Items;
            [ProtoMember(4, IsRequired = true)]
            public byte Member4;
            [ProtoMember(5, IsRequired = true)]
            public DuneStatesProto DuneStates;

            [ProtoContract]
            public class ConstructsProto
            {
                [ProtoMember(1, IsRequired = true)]
                public ushort ID;
                [ProtoMember(2, IsRequired = true)]
                public byte Level;
                [ProtoMember(3, IsRequired = true)]
                public uint Progress;
                [ProtoMember(4, IsRequired = true)]
                public byte Hash;
                [ProtoMember(5, IsRequired = true)]
                public uint dwParam;
                [ProtoMember(6, IsRequired = true)]
                public uint jade1;
                [ProtoMember(7, IsRequired = true)]
                public uint jade2;
                [ProtoMember(8, IsRequired = true)]
                public uint jade3;
                [ProtoMember(9, IsRequired = true)]
                public uint jade4;
                [ProtoMember(10, IsRequired = true)]
                public uint jade5;
                [ProtoMember(11, IsRequired = true)]
                public uint jade6;
                [ProtoMember(12, IsRequired = true)]
                public uint MasteryPoints = 0;
                [ProtoMember(13, IsRequired = true)]
                public uint Debris = 0;
                [ProtoMember(14, IsRequired = true)]
                public uint MartialPoints = 0;
            }
            [ProtoContract]
            public class DuneStatesProto
            {
                [ProtoMember(1, IsRequired = true)]
                public ushort type;
                [ProtoMember(2, IsRequired = true)]
                public ulong Points;
            }
            public static void Create(GameClient user, Archives.Item Item, Action Type)
            {
               
                using (ServerSockets.RecycledPacket rec = new ServerSockets.RecycledPacket())
                {
                    ServerSockets.Packet stream = rec.GetStream();
                    ProtoStructure obj = new ProtoStructure
                    {
                        Type = (byte)Type,
                        UID = user.Player.UID,
                        Items = new ConstructsProto[1]
                    };
                    obj.Items[0] = new ConstructsProto
                    {
                        ID = (ushort)Item.ItemID,
                        Level = (byte)Item.Level,
                        Progress = Item.Progress,
                        Hash = Item.Hash,
                        dwParam = Item.dwParam,
                        jade1 = (uint)Item.Jades[0].JadeID,
                        jade2 = (uint)Item.Jades[1].JadeID,
                        jade3 = (uint)Item.Jades[2].JadeID,
                        jade4 = (uint)Item.Jades[3].JadeID,
                        jade5 = (uint)Item.Jades[4].JadeID,
                        jade6 = (uint)Item.Jades[5].JadeID,
                        MasteryPoints = Item.MasteryPoints,
                        Debris = Item.Debris,
                        MartialPoints = Item.MartialPoints,

                    };
                    obj.DuneStates = new DuneStatesProto
                    {

                        type = (ushort)Item.ItemID,
                        Points = user.MyArchives.DunePts,

                    };
                    user.Send(stream.CreateArchives(obj));
                }
            }

            [PacketAttribute(GamePackets.MsgCombatGear)]
            private unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
            {
                ProtoStructure pQuery;
                stream.GetArchives(out pQuery);

                switch (pQuery.Type)
                {
                    case (byte)Action.View:
                        {
                            #region view
                            Client.GameClient Target;
                            if (Pool.GamePoll.TryGetValue(pQuery.UID, out Target))
                            {

                                Archives.Item[] MyArchvies = null;
                                if (AtributesStatus.IsLee(Target.Player.Class))
                                    MyArchvies = Target.MyArchives.Items.Values.Where(p => p.ItemID >= Archives.TypeID.Dragon && p.ItemID <= Archives.TypeID.Suanni).ToArray();
                                else if (AtributesStatus.IsPirate(Target.Player.Class))
                                    MyArchvies = Target.MyArchives.Items.Values.Where(p => p.ItemID >= Archives.TypeID.ThunderNut && p.ItemID <= Archives.TypeID.LavaNut).ToArray();
                                else if (AtributesStatus.IsTaoist(Target.Player.Class))
                                    MyArchvies = Target.MyArchives.Items.Values.Where(p => p.ItemID >= Archives.TypeID.Vicissitude && p.ItemID <= Archives.TypeID.Birthdeath).ToArray();
                                else if (AtributesStatus.IsArcher(Target.Player.Class))
                                    MyArchvies = Target.MyArchives.Items.Values.Where(p => p.ItemID >= Archives.TypeID.StoneCracker && p.ItemID <= Archives.TypeID.ThornCutter).ToArray();
                                else if (AtributesStatus.IsMonk(Target.Player.Class))
                                    MyArchvies = Target.MyArchives.Items.Values.Where(p => p.ItemID >= Archives.TypeID.HeavenlyTiger && p.ItemID <= Archives.TypeID.CosmicRoc).ToArray();
                                else if (AtributesStatus.IsWarrior(Target.Player.Class))
                                    MyArchvies = Target.MyArchives.Items.Values.Where(p => p.ItemID >= Archives.TypeID.Dragonhowl && p.ItemID <= Archives.TypeID.Redcurse).ToArray();

                               

                                if (MyArchvies != null)
                                {
                                    ProtoStructure obj = new ProtoStructure();
                                    obj.Type = (byte)Action.View;
                                    obj.UID = Target.Player.UID;
                                    obj.Items = new ConstructsProto[Target.MyArchives.Items.Count];
                                    int i = 0; foreach (var Item in MyArchvies)
                                    {
                                        obj.Items[i] = new ConstructsProto();
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
                                    user.Send(stream.CreateArchives(obj));

                                }
                            }
                            #endregion
                            break;

                        }

                }
            }
        }
    }
}
