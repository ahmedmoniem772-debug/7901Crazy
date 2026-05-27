
using ProtoBuf;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreatePokerFriendAction(this ServerSockets.Packet stream, MsgPokerFriendAction obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize((object)obj);
            stream.Finalize(GamePackets.MsgPokerFriendAction);
            return stream;
        }

        public static void GetPokerFriendAction(this ServerSockets.Packet stream, out MsgPokerFriendAction pQuery)
        {
            pQuery = new MsgPokerFriendAction();
            pQuery = stream.ProtoBufferDeserialize<MsgPokerFriendAction>(pQuery);
        }

    }
    [ProtoContract]

    public class MsgPokerFriendAction
    {
        [ProtoMember(1)]
        public MsgPokerFriendAction.TypeID Type;
        [ProtoMember(2)]
        public uint ServerID;
        [ProtoMember(3)]
        public uint TargetID;
        [ProtoMember(5)]
        public uint Mesh;
        [ProtoMember(6)]
        public string TargetName;
        [ProtoMember(7)]
        public uint Un7;

        public enum TypeID : byte
        {
            Add = 0,
            AcceptFriend = 1,
            Delete = 3,
        }
    }
}
