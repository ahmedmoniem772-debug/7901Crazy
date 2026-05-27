using VirusX.Client;
using VirusX.Database;
using VirusX.Role.Instance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirusX.Game.MsgServer
{
    public static class MsgTaskRank
    {
        [ProtoContract]
        public class TaskRankProto
        {
            [ProtoMember(1, IsRequired = true)]
            public long Action;//1

            [ProtoMember(2, IsRequired = true)]
            public long Member2;//50025


            [ProtoMember(3, IsRequired = true)]
            public long Member3;//295

            [ProtoMember(4, IsRequired = true)]
            public long Member4;//24

            [ProtoMember(5, IsRequired = true)]
            public long Member5;//1
            [ProtoMember(6, IsRequired = true)]
            public List<ProtoStructureInfo> Items = new List<ProtoStructureInfo>();

            [ProtoMember(7, IsRequired = true)]
            public long Member7;//6

            [ProtoMember(8, IsRequired = true)]
            public long Member8;//1

        }
        [ProtoContract]
        public class ProtoStructureInfo
        {
            [ProtoMember(1, IsRequired = true)]
            public long Member1;//1

            [ProtoMember(2, IsRequired = true)]
            public long Member2;//1004930

            [ProtoMember(3, IsRequired = true)]
            public long Member3;//90

            [ProtoMember(4, IsRequired = true)]
            public long Member4;//3632007

            [ProtoMember(5, IsRequired = true)]
            public string Member5;//>FreeBunny

            [ProtoMember(6, IsRequired = true)]
            public long Member6;//

            [ProtoMember(7, IsRequired = true)]
            public string Member7;//Bitcoin

            [ProtoMember(8, IsRequired = true)]
            public long Member8;//26

        }
        public static unsafe ServerSockets.Packet CreateTaskRank(this ServerSockets.Packet stream, TaskRankProto obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgTaskRank);
            return stream;

        }
        public static void GetTaskRank(this ServerSockets.Packet stream, out TaskRankProto pQuery)
        {
            pQuery = new TaskRankProto();
            pQuery = stream.ProtoBufferDeserialize<TaskRankProto>(pQuery);
        }
        [PacketAttribute(GamePackets.MsgTaskRank)]
        public unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            TaskRankProto Rank;
            stream.GetTaskRank(out Rank);
            switch (Rank.Action)
            {
                case 1:
                    {
                        if (Role.Core.IsGirl(user.Player.Body))
                        {
                            var Peonies = Pool.GirlsFlowersRanking.Peonies.Values.FirstOrDefault(p => p.Rank == 1 && p.Amount > 0);
                            if (Peonies != null)
                            {
                                var Info = new TaskRankProto();
                                Info.Member2 = 50025;
                                Info.Member5 = 1;
                                Info.Items = new List<ProtoStructureInfo>();
                                Info.Items.Add(new ProtoStructureInfo { Member1 = Peonies.Rank, Member2 = Peonies.UID, Member3 = Peonies.Amount, Member4 = Peonies.Mesh, Member5 = Peonies.Name, Member7 = Peonies.GuildName, Member8 = 26 });
                                user.Send(stream.CreateTaskRank(Info));

                            }
                        }
                        else if (Role.Core.IsBoy(user.Player.Body))
                        {
                            var Peonies = Pool.BoysFlowersRanking.Peonies.Values.FirstOrDefault(p => p.Rank == 1 && p.Amount > 0);
                            if (Peonies != null)
                            {
                                var Info = new TaskRankProto();
                                Info.Member2 = 50026;
                                Info.Member5 = 1;
                                Info.Items = new List<ProtoStructureInfo>();
                                Info.Items.Add(new ProtoStructureInfo { Member1 = Peonies.Rank, Member2 = Peonies.UID, Member3 = Peonies.Amount, Member4 = Peonies.Mesh, Member5 = Peonies.Name, Member7 = Peonies.GuildName, Member8 = 26 });
                                user.Send(stream.CreateTaskRank(Info));
                            }
                        }
                        break;
                    }
            }
        }
    }
}
