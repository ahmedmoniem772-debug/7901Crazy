using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using VirusX.ServerSockets;

namespace VirusX.Game.MsgServer
{
    public static class MsgProfLevUp
    {
        [ProtoContract]
        public class MsgProfLevUpProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;

            [ProtoMember(2, IsRequired = true)]
            public uint EntityID;

            [ProtoMember(3, IsRequired = true)]
            public uint Rank;

            [ProtoMember(4, IsRequired = true)]
            public RankPromotion[] Ranking;

            [ProtoContract]
            public class RankPromotion
            {
                [ProtoMember(1, IsRequired = true)]
                public uint EntityUID;
                [ProtoMember(2, IsRequired = true)]
                public uint Rank;
                [ProtoMember(3, IsRequired = true)]
                public string Name;
            }
        
        }

        public static unsafe ServerSockets.Packet CreateProfLevUp(this ServerSockets.Packet stream, MsgProfLevUpProto obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgProfLevUp);

            return stream;
        }
        public static unsafe void GetProfLevUp(this ServerSockets.Packet stream, out MsgProfLevUpProto pQuery)
        {
            pQuery = new MsgProfLevUpProto();
            pQuery = stream.ProtoBufferDeserialize<MsgProfLevUpProto>(pQuery);
        }
        [PacketAttribute(GamePackets.MsgProfLevUp)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {

            MsgProfLevUpProto Info;
            stream.GetProfLevUp(out Info);
            if (Info.Type == 0)//Promote
            {
                if (client.Player.Class % 100 >= 5 && client.Player.Reborn < 2)
                {
                    client.CreateBoxDialog("You Reached your max promotion for reborn");
                    return;
                }

                if (client.Player.Class % 100 >= 51) return;
                var data = Database.ProfessionTable.ProfLevelUPList.Values.Where(i => i.Class == client.Player.Class).FirstOrDefault();
                if (data != null)
                {
                    if (client.MyPrestigePoints >= data.PrestigeScore && client.Player.Level >= data.Level && client.Player.ClassExperience >= data.EXP)
                    {

                        client.Player.ClassExperience -= data.EXP;
                        client.Player.Class++;         
                        Info.EntityID = client.Player.UID;
                        client.Send(stream.CreateProfLevUp(Info));
                        client.Equipment.QueryEquipment(client.Equipment.Alternante);
                    }
                }

            }
            if (Info.Type == 2)
            {
                var rank = Database.PrestigeRanking.Ranks[Database.PrestigeRanking.GetIndex(client.Player.Class)];
                if (rank != null)
                {
                    var array = rank.Items.Values.Take(3).ToArray();
                    Info.Ranking = new MsgProfLevUpProto.RankPromotion[array.Count()];
                    for (int x = 0; x < array.Count(); x++)
                    {
                        Info.Ranking[x] = new MsgProfLevUpProto.RankPromotion();
                        Info.Ranking[x].EntityUID = array[x].UID;
                        Info.Ranking[x].Name = array[x].Name;
                        Info.Ranking[x].Rank = (uint)x + 1;
                    }
                    Info.EntityID = client.Player.UID;
                    client.Send(stream.CreateProfLevUp(Info));
                }
            }
        }
    }
}
