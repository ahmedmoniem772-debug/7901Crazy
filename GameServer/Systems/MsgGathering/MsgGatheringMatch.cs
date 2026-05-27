
using ProtoBuf;
namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {

        public static void GetGatheringMatch(this ServerSockets.Packet stream, out MsgGatheringMatch pQuery)
        {
            pQuery = new MsgGatheringMatch();
            pQuery = stream.ProtoBufferDeserialize<MsgGatheringMatch>(pQuery);
        }

        public static ServerSockets.Packet CreateGatheringMatch(this ServerSockets.Packet stream, MsgGatheringMatch obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgEnemyInvadeArenic);
            return stream;
        }
    }
    [ProtoContract]
    public class MsgGatheringMatch
    {

        [ProtoMember(1)]
        public MsgGatheringMatch.TypeID Type;
        [ProtoMember(2)]
        public uint un2;
        [ProtoMember(3)]
        public uint un3;
        [ProtoMember(6)]
        public MsgGatheringMatch.Bot[] bot;

        public class Bot
        {
            [ProtoMember(1)]
            public uint BotID;
            [ProtoMember(2)]
            public uint BotServerID;
            [ProtoMember(3)]
            public uint BotClass;
            [ProtoMember(4)]
            public uint BotLevel;
            [ProtoMember(5)]
            public uint Bot5;
            [ProtoMember(6)]
            public uint Bot6;
            [ProtoMember(10)]
            public string BotName;
        }

        public enum TypeID : byte
        {
            EnterMatch = 1,
            JoinMatch = 2,
        }
        [PacketAttribute(GamePackets.MsgEnemyInvadeArenic)]
        public static void MsgEnemyInvadeArenic(Client.GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {
            MsgGatheringMatch pQuery;
            stream.GetGatheringMatch(out pQuery);
            MsgGatheringMatch.TypeID type = pQuery.Type;
            client.Send(stream.CreateGatheringMatch(pQuery));
        }
    }
}
