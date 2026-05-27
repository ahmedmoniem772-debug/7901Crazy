using ProtoBuf;
using ConquerOnline.Client;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateMsgPokerInfo(this ServerSockets.Packet stream, MsgPokerInfo pQuery)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgPokerInfo);
            return stream;
        }
        public static void GetMsgPokerInfo(this ServerSockets.Packet stream, out MsgPokerInfo pQuery)
        {
            pQuery = new MsgPokerInfo();
            pQuery = stream.ProtoBufferDeserialize<MsgPokerInfo>(pQuery);
        }

    }
    [ProtoContract]
    public class MsgPokerInfo
    {
        public enum _Type : byte
        {

            Show = 0,
        }
        [ProtoMember(1, IsRequired = true)]
        public uint Type;
        [ProtoMember(2, IsRequired = true)]
        public uint Unk1;
        [ProtoMember(3, IsRequired = true)]
        public uint Unk2;
        [ProtoMember(4, IsRequired = true)]
        public uint Unk3;
        [ProtoMember(5, IsRequired = true)]
        public uint Unk4;
        [ProtoMember(6, IsRequired = true)]
        public uint Unk5;
        [PacketAttribute(GamePackets.MsgPokerInfo)]
        private static void Process(GameClient user, ServerSockets.Packet stream)
        {
            MsgPokerInfo pQuery;
            stream.GetMsgPokerInfo(out pQuery);
            switch (pQuery.Type)
            {
                case (byte)_Type.Show:
                    {
                        user.Send(stream.CreateMsgPokerInfo(pQuery));
                        break;
                    }
            }
        }

    }
}
