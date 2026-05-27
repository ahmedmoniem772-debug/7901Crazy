
using ConquerOnline.Client;
using ProtoBuf;
using System.Collections.Generic;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreatePokerFriendList(this ServerSockets.Packet stream, MsgPokerFriendList pQuery)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgBetLevel);
            return stream;
        }
        public static void GetPokerFriendList(this ServerSockets.Packet stream, out MsgPokerFriendList pQuery)
        {
            pQuery = new MsgPokerFriendList();
            pQuery = stream.ProtoBufferDeserialize<MsgPokerFriendList>(pQuery);
        }

    }
    [ProtoContract]
    public class MsgPokerFriendList
    {
        public enum _Type : byte
        {
            Show = 0,
        }
        [ProtoMember(1, IsRequired = true)]
        public long Type;//0

        [ProtoMember(2, IsRequired = true)]
        public List<PlayerInfo> Items = new List<PlayerInfo>();

        [ProtoMember(3, IsRequired = true)]
        public long Member3;//1

        [ProtoContract]
        public class PlayerInfo
        {
            [ProtoMember(1, IsRequired = true)]
            public ushort ServerID;//37

            [ProtoMember(2, IsRequired = true)]
            public uint UID;//5772278

            [ProtoMember(3, IsRequired = true)]
            public string Name;//Meraa

            [ProtoMember(4, IsRequired = true)]
            public uint Mesh;//3492007

            [ProtoMember(5, IsRequired = true)]
            public byte Online;//0 //1

            [ProtoMember(6, IsRequired = true)]
            public byte Member6;//0

            [ProtoMember(7, IsRequired = true)]
            public byte Member7;//0

            [ProtoMember(8, IsRequired = true)]
            public byte Member8;//1

            [ProtoMember(9, IsRequired = true)]
            public byte Member9;//1

        }
        [PacketAttribute(GamePackets.MsgPokerFriendList)]
        private static void Process(GameClient user, ServerSockets.Packet stream)
        {
            
        }

    }
}
