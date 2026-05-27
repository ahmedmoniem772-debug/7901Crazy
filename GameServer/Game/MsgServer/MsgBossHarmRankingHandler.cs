using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateBossHarmRankList(this ServerSockets.Packet stream, MsgBossHarmRanking obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgBossHarmRanking);
            return stream;
        }
        public static unsafe void GetBossHarm(this ServerSockets.Packet stream, out MsgBossHarmRanking pQuery)
        {
            pQuery = new MsgBossHarmRanking();
            pQuery = stream.ProtoBufferDeserialize<MsgBossHarmRanking>(pQuery);
        }
        [PacketAttribute(GamePackets.MsgBossHarmRanking)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {

            MsgBossHarmRanking Info;
            stream.GetBossHarm(out Info);
            switch (Info.Type)
            {
                case (int)MsgBossHarmRanking.RankAction.Show:
                    {
                        if (client.Map.View.MapContain(Role.MapObjectType.Monster, Info.MonsterID))
                        {
                            var monster = client.Map.View.GetMapObject<MsgMonster.MonsterRole>(Role.MapObjectType.Monster, Info.MonsterID);
                            client.Send(stream.CreateBossHarmRankList(new MsgBossHarmRanking() { Type = (int)MsgBossHarmRanking.RankAction.ShowRespondForFirstThree, MonsterID = Info.MonsterID, Hunters = monster.Hunters.Values.Take(3).ToArray() }));
                            if (monster.Hunters.Count > 3)
                                client.Send(stream.CreateBossHarmRankList(new MsgBossHarmRanking() { Type = (int)MsgBossHarmRanking.RankAction.ShowRespondForTheRest, MonsterID = Info.MonsterID, Hunters = monster.Hunters.Values.Skip(3).ToArray() }));
                        }
                        break;
                    }
            }
        }
    }
    [ProtoContract]
    public struct MsgBossHarmRanking
    {
        [Flags]
        public enum RankAction : int
        {
            Show = 0,
            ShowRespondForFirstThree = 1,
            ShowRespondForTheRest = 2,
        }
        [ProtoMember(1, IsRequired = true)]
        public int Type;
        [ProtoMember(2, IsRequired = true)]
        public uint MonsterID;
        [ProtoMember(3, IsRequired = true)]
        public MsgBossHarmRankingEntry[] Hunters;
    }
    [ProtoContract]
    public class MsgBossHarmRankingEntry
    {

        [ProtoMember(1, IsRequired = true)]
        public uint Rank;
        [ProtoMember(2, IsRequired = true)]
        public uint ServerID;
        [ProtoMember(3, IsRequired = true)]
        public uint HunterUID;
        [ProtoMember(4, IsRequired = true)]
        public string HunterName;
        [ProtoMember(5, IsRequired = true)]
        public uint HunterScore;
    }
}
