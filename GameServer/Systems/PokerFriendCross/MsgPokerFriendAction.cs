
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
        [ProtoMember(1, IsRequired = true)]
        public TypeID Type;//1

        [ProtoMember(2, IsRequired = true)]
        public ushort ServerID;//37
   
        [ProtoMember(3, IsRequired = true)]
        public uint TargetUID;//5772278 

        [ProtoMember(4, IsRequired = true)]
        public string TargetName;//Meraa

        [ProtoMember(5, IsRequired = true)]
        public uint TargetMesh;//3492007

        [ProtoMember(6, IsRequired = true)]
        public long unk;//1

        [ProtoMember(7, IsRequired = true)]
        public long unk1;//0

        [ProtoMember(9, IsRequired = true)]
        public long unk2;//1

        [ProtoMember(10, IsRequired = true)]
        public long unk3;//1

        public enum TypeID : byte
        {
            RequestAdd = 0,
            AcceptFriend = 1,
            Delete = 3,
        }
    }
}
