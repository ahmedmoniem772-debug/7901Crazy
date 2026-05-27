using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX
{
    public class MsgResonanceRune
    {
        #region Proto Packet

        [ProtoContract]
        public class MsgResonanceProto
        {
            [ProtoMember(1)]
            public ActionID Type;

            [ProtoMember(2)]
            public ulong UID;

            [ProtoMember(3)]
            public int Unk3;

            [ProtoMember(4)]
            public int Unk4;

            [ProtoMember(5)]
            public List<RuneItem> Items;
        }

        [ProtoContract]
        public class RuneItem
        {
            [ProtoMember(1)]
            public byte Slot;

            [ProtoMember(2)]
            public uint ItemID;

            [ProtoMember(3)]
            public byte Unknown;
        }

        #endregion

        public enum ActionID : int
        {
            Create = 0,
            Resonate = 1,
            Login = 2,
            unk = 3,
            unk1 = 4,
            unk2 = 5,
        }

        public enum UpdataID : int
        {
            Create = 0,
            Resonate = 1,
            Login = 2,
            unk = 3,
            unk1 = 4,
            unk2 = 5,
        }



        [PacketAttribute(2604)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            MsgResonanceRune.MsgResonanceProto info;
            stream.GetRuneResonance(out info);

            switch (info.Type)
            {
                case ActionID.Login:
                    {
                        #region Login One
                        if (client.Player.ResinanceRuneOne > 0 && client.Player.ResinanceRunefive < 5)
                        {
                            var packet = new MsgResonanceProto
                            {
                                Type = ActionID.Login,
                                UID = client.Player.UID,
                                Unk3 = 3,
                                Items = LoadRuneItemsOne(client.Player.UID)

                            };
                            client.Send(stream.CreateRuneResonance(packet));
                        }
                        #endregion
                        #region Login Tow
                        if (client.Player.ResinanceRuneTwo > 0 && client.Player.ResinanceRuneOne > 0 && client.Player.ResinanceRuneTwo > 0 && client.Player.ResinanceRunethree > 0 && client.Player.ResinanceRunefour > 0 && client.Player.ResinanceRunefive > 0)
                        {
                            var packet2 = new MsgResonanceProto
                            {
                                Type = ActionID.Login,
                                UID = client.Player.UID,
                                Unk3 = 3,
                                Items = LoadRuneItems(client.Player.UID)
                            };
                            client.Send(stream.CreateRuneResonance(packet2));
                        }
                        #endregion
                        if (client.Player.ResinanceRuneOne > 0 || client.Player.ResinanceRuneTwo > 0 || client.Player.ResinanceRunethree > 0 || client.Player.ResinanceRunefour > 0 || client.Player.ResinanceRunefive > 0)
                            break;
                        var packet3 = new MsgResonanceProto
                        {
                            Type = ActionID.Login,
                            UID = client.Player.UID,
                            Unk3 = 3,
                            Items = LoadRuneItemsFaild(client.Player.UID)
                        };
                        client.Send(stream.CreateRuneResonance(packet3));
                        break;
                    }
                case ActionID.Resonate:
                    {

                        var packet = new MsgResonanceProto
                        {
                            Type = ActionID.Resonate,
                            UID = client.Player.UID,
                            Unk3 = 3,
                            Unk4 = 2,
                            Items = LoadRuneItemsFaild(client.Player.UID)

                        };

                        client.Send(stream.CreateRuneResonance(packet));
                        break;
                    }
                case ActionID.Create:
                    {
                        switch (info.Unk4)
                        {
                            case 1:
                                {
                                    if (client.Inventory.Contain(3314254, 6) && (client.Inventory.Contain(4060001, 900) || client.Inventory.Contain(3324226, 3)))
                                    {
                                        client.Inventory.RemoveStackItem(3314254, (ushort)6, stream);
                                        client.Inventory.RemoveStackItem(4060001, (ushort)900, stream);
                                        client.Inventory.RemoveStackItem(3324226, (ushort)3, stream);
                                        if (Role.Core.Rate(2))
                                        {
                                            if (info.Items != null)
                                            {
                                                foreach (var rune in info.Items)
                                                {
                                                    switch (rune.Slot)
                                                    {
                                                        case 1:
                                                            {

                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRuneOne = 1;
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems2()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRuneTwo = 1;
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems3()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRunethree = 1;
                                                                break;
                                                            }
                                                        case 4:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems4()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRunefour = 1;
                                                                break;
                                                            }
                                                        case 5:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems5()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRunefive = 1;
                                                                break;
                                                            }
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            var packet = new MsgResonanceProto
                                            {
                                                Type = ActionID.Create,
                                                UID = client.Player.UID,
                                                Unk3 = 3,
                                            };

                                            client.Send(stream.CreateRuneResonance(packet));
                                        }
                                    }

                                    break;

                                }
                            case 2:
                                {
                                    if (client.Inventory.Contain(3314256, 3) && (client.Inventory.Contain(4040001, 900) || client.Inventory.Contain(3324226, 3)))
                                    {
                                        client.Inventory.RemoveStackItem(3314256, (ushort)3, stream);
                                        client.Inventory.RemoveStackItem(4040001, (ushort)900, stream);
                                        client.Inventory.RemoveStackItem(3324226, (ushort)3, stream);
                                        if (Role.Core.Rate(2))
                                        {
                                            if (info.Items != null)
                                            {
                                                foreach (var rune in info.Items)
                                                {
                                                    switch (rune.Slot)
                                                    {
                                                        case 1:
                                                            {

                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRuneOne = 1;
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems2()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRuneTwo = 1;
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems3()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRunethree = 1;
                                                                break;
                                                            }
                                                        case 4:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems4()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRunefour = 1;
                                                                break;
                                                            }
                                                        case 5:
                                                            {
                                                                var packet = new MsgResonanceProto
                                                                {
                                                                    Type = ActionID.Create,
                                                                    UID = client.Player.UID,
                                                                    Unk3 = 3,
                                                                    Unk4 = 2,
                                                                    Items = LoadCreatedRuneItems5()
                                                                };

                                                                client.Send(stream.CreateRuneResonance(packet));
                                                                client.Player.ResinanceRunefive = 1;
                                                                break;
                                                            }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var packet = new MsgResonanceProto
                                            {
                                                Type = ActionID.Create,
                                                UID = client.Player.UID,
                                                Unk3 = 3,
                                            };

                                            client.Send(stream.CreateRuneResonance(packet));
                                        }
                                    }
                                    break;
                                }
                        }

                        break;
                    }

                default:
                    {
                        Console.WriteLine("[ResonanceRune] Unknown ActionID: { " + info.Type + "]");
                        break;
                    }
            }
        }

        public static List<RuneItem> LoadRuneItems(ulong uid)
        {

            return new List<RuneItem>
            {
                new RuneItem { Slot = 1, ItemID = 4039609, Unknown = 0 },
                new RuneItem { Slot = 2, ItemID = 4037709, Unknown = 0 },
                new RuneItem { Slot = 3, ItemID = 4071409, Unknown = 0 },
                new RuneItem { Slot = 4, ItemID = 4035209, Unknown = 0 },
                new RuneItem { Slot = 5, ItemID = 4030309, Unknown = 0 },
                new RuneItem { Slot = 6, ItemID = 0, Unknown = 0 }
            };
        }

        public static List<RuneItem> LoadRuneItemsOne(ulong uid)
        {

            return new List<RuneItem>
            {
                new RuneItem { Slot = 1, ItemID = 4039609, Unknown = 1 },
                //new RuneItem { Slot = 2, ItemID = 4037709, Unknown = 0 },
                // new RuneItem { Slot = 3, ItemID = 4071409, Unknown = 0 },
                // new RuneItem { Slot = 4, ItemID = 4035209, Unknown = 0 },
                //  new RuneItem { Slot = 5, ItemID = 4035209, Unknown = 0 },
                 new RuneItem { Slot = 2, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 3, ItemID = 0, Unknown = 0 },
                 new RuneItem { Slot = 4, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 5, ItemID = 0, Unknown = 0 },
                 new RuneItem { Slot = 5, ItemID = 0, Unknown = 0 }
            };
        }
        public static List<RuneItem> LoadRuneItemsFaild(ulong uid)
        {

            return new List<RuneItem>
            {
                new RuneItem { Slot = 1, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 2, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 3, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 4, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 5, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 6, ItemID = 0, Unknown = 0 }
            };
        }
        private static List<RuneItem> LoadCreatedRuneItems()
        {
            return new List<RuneItem>
            {
                        new RuneItem { Slot = 1, ItemID = 4039609, Unknown = 0 },

            };
        }
        private static List<RuneItem> LoadCreatedRuneItems2()
        {
            return new List<RuneItem>
            {
                        new RuneItem { Slot = 2, ItemID = 4070709, Unknown = 0 },

            };
        }
        private static List<RuneItem> LoadCreatedRuneItems3()
        {
            return new List<RuneItem>
            {
                        new RuneItem { Slot = 3, ItemID = 4071409, Unknown = 0 },

            };
        }
        private static List<RuneItem> LoadCreatedRuneItems4()
        {
            return new List<RuneItem>
            {
                        new RuneItem { Slot = 4, ItemID = 4035209, Unknown = 0 },

            };
        }
        private static List<RuneItem> LoadCreatedRuneItems5()
        {
            return new List<RuneItem>
            {
                        new RuneItem { Slot = 5, ItemID = 4030309, Unknown = 0 },

            };
        }
        private static List<RuneItem> InitializeRuneSlots()
        {

            return new List<RuneItem>
            {
                new RuneItem { Slot = 1, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 2, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 3, ItemID = 0, Unknown = 0 },
                new RuneItem { Slot = 4, ItemID = 0, Unknown = 0 }
            };
        }
    }
}
