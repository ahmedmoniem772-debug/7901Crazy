
using ConquerOnline.Client;
using ProtoBuf;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static ServerSockets.Packet CreatePokerFriendList(this ServerSockets.Packet stream, MsgPokerFriendList obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize((object)obj);
            stream.Finalize(GamePackets.MsgPokerFriendList);
            return stream;
        }

        public static void GetPokerFriendList(this ServerSockets.Packet stream, out MsgPokerFriendList pQuery)
        {
            pQuery = new MsgPokerFriendList();
            pQuery = stream.ProtoBufferDeserialize<MsgPokerFriendList>(pQuery);
        }
    }
    public class MsgPokerFriendList
    {
        public enum TypeID : byte
        {
            Add = 0,
            Show = 1,
            Delete = 3,
        }
        [ProtoMember(1)]
        public uint Un1;
        [ProtoMember(2)]
        public MsgPokerFriendList.List[] Li;
        [ProtoMember(3)]
        public uint Un3;
        public class List
        {
            [ProtoMember(1)]
            public uint ServerID;
            [ProtoMember(2)]
            public uint TargetID;
            [ProtoMember(3)]
            public string TargetName;
            [ProtoMember(4)]
            public uint Mesh;
            [ProtoMember(5)]
            public uint Un5;
            [ProtoMember(6)]
            public uint Un6;
            [ProtoMember(7)]
            public uint Un7;
        }

        [ConquerOnline.Packet(2398)]
        public static void PokerFriendList(GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {
            MsgPokerFriendList pQuery;
            stream.GetPokerFriendList(out pQuery);
            client.Send(stream.CreatePokerFriendList(pQuery));
        }




    }
}
