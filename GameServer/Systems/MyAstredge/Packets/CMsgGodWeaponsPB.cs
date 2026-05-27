using VirusX.Client;
using VirusX.Database;
using VirusX.Role.Instance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirusX.Game.MsgServer
{
    public static class CMsgGodWeapons
    {
        [ProtoContract]
        public class CMsgGodWeaponsPB
        {
            public enum ActionID : uint
            {
                Login = 1,
                Close = 2,
                Add = 3,
                Update = 4,
                Close1 = 5,
                Close2 = 6,
                Search = 7,
                View = 8,
                Practice = 9,
            }
            [ProtoMember(1, IsRequired = true)]
            public ActionID Action;

            [ProtoMember(2, IsRequired = true)]
            public List<InfoAstredge> AstredgeInfo = new List<InfoAstredge>();

            [ProtoMember(3, IsRequired = true)]
            public List<ItemProgress> ItemProgress = new List<ItemProgress>();

            [ProtoMember(4, IsRequired = true)]
            public uint TargetUID;

            [ProtoMember(5, IsRequired = true)]
            public uint Unk5;

            [ProtoMember(6, IsRequired = true)]
            public uint[] Items = new uint[3];

            [ProtoMember(7, IsRequired = true)]
            public byte AstredgeQuestcountdone;

            public static void UpdateItem(GameClient user, Astredge.Item Item)
            {

                using (ServerSockets.RecycledPacket rec = new ServerSockets.RecycledPacket())
                {
                    ServerSockets.Packet stream = rec.GetStream();
                    CMsgGodWeaponsPB obj = new CMsgGodWeaponsPB
                    {
                        Action = ActionID.Update,
                    };
                    obj.AstredgeInfo.Add(new CMsgGodWeapons.InfoAstredge()
                    {
                        Type = Item.ItemID,
                        AstredgeLevel = Item.Level,
                        AstredgeProgress = Item.Progress,
                        GUISKILL = 1,
                    });
                    obj.Unk5 = 7;
                    obj.Items[0] = 7803;
                    obj.Items[1] = 7819;
                    obj.Items[2] = 7845;
                    user.Send(stream.CreateCMsgGodWeapons(obj));
                }
            }
            public static void Add(GameClient user, Astredge.Item Item)
            {

                using (ServerSockets.RecycledPacket rec = new ServerSockets.RecycledPacket())
                {
                    ServerSockets.Packet stream = rec.GetStream();
                    CMsgGodWeaponsPB obj = new CMsgGodWeaponsPB
                    {
                        Action = ActionID.Add,
                    };
                    obj.AstredgeInfo.Add(new CMsgGodWeapons.InfoAstredge()
                    {
                        Type = Item.ItemID,
                        AstredgeLevel = Item.Level,
                        AstredgeProgress = Item.Progress,
                        GUISKILL = 1,
                    });
                    obj.Unk5 = 0;
                    obj.Items[0] = 7803;
                    obj.Items[1] = 7819;
                    obj.Items[2] = 7845;
                    user.Send(stream.CreateCMsgGodWeapons(obj));
                }
            }
        }
        [ProtoContract]
        public class InfoAstredge
        {
            [ProtoMember(1, IsRequired = true)]
            public Astredge.TypeID Type;//1 //2

            [ProtoMember(2, IsRequired = true)]
            public byte AstredgeLevel;//1 // Level //MaxLevel 20

            [ProtoMember(3, IsRequired = true)]
            public uint AstredgeProgress;//0 //Progress

            [ProtoMember(4, IsRequired = true)]
            public byte GUISKILL;//1 

            [ProtoMember(5, IsRequired = true)]
            public uint Unk5;//640 

            [ProtoMember(6, IsRequired = true)]
            public uint Unk6;//640 

            [ProtoMember(7, IsRequired = true)]
            public uint Unk7;//640 
        }
        [ProtoContract]
        public class ItemProgress
        {
            [ProtoMember(1, IsRequired = true)]
            public uint UID = 0;

            [ProtoMember(2, IsRequired = true)]
            public uint Count = 0;
        }

        public static unsafe ServerSockets.Packet CreateCMsgGodWeapons(this ServerSockets.Packet stream, CMsgGodWeapons.CMsgGodWeaponsPB obj)
        {

            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.CMsgGodWeaponsPB);
            return stream;
        }
        public static unsafe void GetCMsgGodWeapons(this ServerSockets.Packet stream, out CMsgGodWeapons.CMsgGodWeaponsPB pQuery)
        {
            pQuery = new CMsgGodWeapons.CMsgGodWeaponsPB();
            pQuery = stream.ProtoBufferDeserialize<CMsgGodWeapons.CMsgGodWeaponsPB>(pQuery);
        }

        [PacketAttribute(GamePackets.CMsgGodWeaponsPB)]
        private unsafe static void Process(GameClient client, ServerSockets.Packet stream)
        {
            if (client.InTrade || client.PokerPlayer != null || client.IsVendor)
                return;
            CMsgGodWeapons.CMsgGodWeaponsPB pQuery;
            stream.GetCMsgGodWeapons(out pQuery);
            switch (pQuery.Action)
            {
                case CMsgGodWeaponsPB.ActionID.Search:
                    {
                        GameClient user;
                        if (Pool.GamePoll.TryGetValue(pQuery.TargetUID, out user))
                        {
                            Astredge.Item[] MyAstredge = null;
                            MyAstredge = user.MyAstredge.Items.Values.Where(p => p.ItemID >= Astredge.TypeID.ViodragonClub && p.ItemID <= Astredge.TypeID.HeartLock).ToArray();
                            if (MyAstredge != null)
                            {
                                foreach (var Item in MyAstredge)
                                {
                                    pQuery.AstredgeInfo.Add(new CMsgGodWeapons.InfoAstredge()
                                    {
                                        Type = Item.ItemID,
                                        AstredgeLevel = Item.Level,
                                        AstredgeProgress = Item.Progress,
                                        GUISKILL = 1,
                                    });
                                    pQuery.Unk5 = 11;
                                    pQuery.Items = new uint[3];
                                    pQuery.Items[0] = 7804;
                                    pQuery.Items[1] = 7819;
                                    pQuery.Items[2] = 7833;
                                    client.Send(stream.CreateCMsgGodWeapons(pQuery));
                                }
                            }
                        }
                        break;
                    }
                case CMsgGodWeaponsPB.ActionID.Update:
                    {
                        Astredge.Item OBJ;
                        uint Progress = 0;
                        if (client.MyAstredge.Items.TryGetValue(pQuery.AstredgeInfo[0].Type, out OBJ))
                        {
                            MsgGameItem ITEM;
                            foreach (var items in pQuery.ItemProgress)
                            {
                                if (client.Inventory.TryGetItem(items.UID, out ITEM))
                                {
                                    if (client.Inventory.Contain(ITEM.ITEM_ID, (ushort)items.Count)
                                        || client.Inventory.Contain(ITEM.ITEM_ID, (ushort)items.Count, 1))
                                    {
                                        if (ITEM.ITEM_ID >= 3344443 && ITEM.ITEM_ID <= 3344446)
                                        {
                                            god_weapons_material.MaterialAstredge obj;
                                            if (god_weapons_material.TryGetValue(god_weapons_material.TypeExp.Material, ITEM.ITEM_ID, out obj))
                                            {
                                                Progress += obj.EXP * items.Count;
                                            }
                                        }
                                        client.Inventory.RemoveStackItem(ITEM.ITEM_ID, (ushort)items.Count, stream);

                                    }
                                }
                            }
                            OBJ.Progress += Progress;
                            byte level = 20;
                            while (OBJ.Progress >= OBJ.DBItemLevel.EXP && OBJ.Level < level)
                            {
                                ++OBJ.Level;
                            }
                            if (OBJ.Progress >= 1000000000)
                                OBJ.Progress = 1000000000;
                            CMsgGodWeaponsPB.UpdateItem(client, OBJ);
                            client.MyAstredge.GET_Skill(Astredge.TypeID.ViodragonClub);
                            client.MyAstredge.UpdateRank();

                        }
                        break;
                    }
                case CMsgGodWeaponsPB.ActionID.View:
                    {
                        client.Send(stream.CreateCMsgGodWeapons(pQuery));
                        break;
                    }
                default:
                    {
                        Console.WriteLine("[CMsgGodWeaponsPB] Unable Action  " + pQuery.Action);
                        break;
                    }
            }

        }

    }
}
