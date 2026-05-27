//// Player.cs
//================
//[ProtoMember(144, IsRequired = true)]
//public uint FamePoints;

//public unsafe uint PlayerFame = 0;

//public unsafe ServerSockets.Packet GetArray(ServerSockets.Packet stream, bool WindowsView)
//proto.FamePoints = PlayerFame;

//================

////MsgFamePacket.cs
//================
using System;
using System.Collections.Generic;
using VirusX.Client;
using ProtoBuf;

namespace VirusX
{
    public static class MsgFamePacket
    {
        [ProtoContract]
        public class MsgFameHall
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Action;
            [ProtoMember(2, IsRequired = true)]
            public uint FamePoints;
            [ProtoMember(3, IsRequired = true)]
            public uint Unk3;
            [ProtoMember(4, IsRequired = true)]
            public uint TimeStamp;
            [ProtoMember(5, IsRequired = true)]
            public uint Unk5;// 3 = GUI Full View / 2 xx double the values / 0 == clear data view
            [ProtoMember(6, IsRequired = true)]
            public List<Fame> FameDetails = new List<Fame>();

            [ProtoContract]
            public class Fame
            {
                [ProtoMember(1, IsRequired = true)]
                public uint Type;// 1 >> Bounty Hall //  33 >> Killed Monster Quests
                [ProtoMember(2, IsRequired = true)]
                public uint TodayPoints;//Earned Points Today from the Quests
                [ProtoMember(3, IsRequired = true)]
                public uint DoneQuests;//Total Done Quests For Eeach Type
                [ProtoMember(4, IsRequired = true)]
                public uint Unk4;//0
            }
            public ServerSockets.Packet ToArray()
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    stream.InitWriter();
                    stream.ProtoBufferSerialize(this);
                    stream.Finalize(2317);
                    return stream;
                }
            }
            public static implicit operator ServerSockets.Packet(MsgFameHall obj)
            {
                return obj.ToArray();
            }
        }
        public static unsafe ServerSockets.Packet ActionCreate(this ServerSockets.Packet stream, MsgFameHall pQuery)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(2317);
            return stream;
        }
        public static unsafe ServerSockets.Packet AddPoints(this ServerSockets.Packet stream, uint Action, uint points)
        {
            //user.Send(stream.AddPoints(5, user.Player.PlayerFame));// Addpoints 
            return ActionCreate(stream, new MsgFameHall() { Action = Action, FamePoints = points, Unk5 = 3 });
        }
        public static unsafe ServerSockets.Packet CreateFameLogin(this ServerSockets.Packet stream, uint Action, uint points)
        {
            //user.Send(stream.CreateFameLogin(4, user.Player.PlayerFame));// LoginHandler
            return ActionCreate(stream, new MsgFameHall() { Action = Action, FamePoints = points, TimeStamp = uint.Parse(DateTime.Now.ToString("yyyyMMdd")), Unk5 = 3 });
        }
        [Packet(2317)]
        private unsafe static void Process(GameClient user, ServerSockets.Packet stream)
        {
            MsgFameHall msg = new MsgFameHall();
            msg = stream.ProtoBufferDeserialize<MsgFameHall>(msg);

            switch (msg.Action)
            {
                case 0://View >> this action is for test didn't complete it yet.....
                    {
                        var fameList = new List<MsgFameHall.Fame>();

                        fameList.Add(new MsgFameHall.Fame()
                        {
                            Type = 1,// 1 = Bounty Hall
                            TodayPoints = 111,
                            DoneQuests = 112,
                            Unk4 = 0
                        });

                        fameList.Add(new MsgFameHall.Fame()
                        {
                            Type = 33,// 33 = Beast Cultivation / Monster Fame
                            TodayPoints = 221,
                            DoneQuests = 222,
                            Unk4 = 0
                        });
                        msg.FamePoints = user.Player.TableBetDice;
                        msg.FameDetails = fameList;
                        msg.Unk5 = 3;// Apply View Data

                        user.Send(msg);
                        break;
                    }
                case 2://view yesterday events
                    {
                        user.Send(msg);
                        break;
                    }
                case 3://claim yesterday reward item
                    {
                        user.Send(msg);
                        break;
                    }
                case 4://Login
                    {
                        user.Send(stream.CreateFameLogin(4, user.Player.TableBetDice));
                        break;
                    }
                case 5://sendpoints
                    {
                        user.Send(stream.AddPoints(5, user.Player.TableBetDice));
                        //user.TableBetDic.UpdateRank();
                        break;
                    }
                default:
                    {
                        //MyConsole.WriteLine("MsgFame Can't Find Action >> msg.Action}");
                        break;
                    }
            }
        }
    }
}