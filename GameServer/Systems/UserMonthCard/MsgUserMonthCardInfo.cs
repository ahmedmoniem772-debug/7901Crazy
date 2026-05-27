
using VirusX.Client;
using ProtoBuf;
using System.Collections.Generic;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateUserMonthCardLogin(this ServerSockets.Packet stream)
        {
            stream.Finalize(GamePackets.MsgUserMonthCardInfo);
            return stream;
        }
        public static unsafe ServerSockets.Packet CreateUserMonthCardInfo(this ServerSockets.Packet stream, MsgUserMonthCardInfo obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgUserMonthCardInfo);
            return stream;
        }
        public static void GetUserMonthCardInfo(this ServerSockets.Packet stream, out MsgUserMonthCardInfo pQuery)
        {
            pQuery = new MsgUserMonthCardInfo();
            pQuery = stream.ProtoBufferDeserialize<MsgUserMonthCardInfo>(pQuery);
        }

    }
    [ProtoContract]
    public class MsgUserMonthCardInfo
    {
        [ProtoMember(1, IsRequired = true)]
        public CardItemPB Info;

        [ProtoContract]
        public class CardItemPB
        {
            [ProtoMember(1, IsRequired = true)]
            public ushort Type;

            [ProtoMember(2, IsRequired = true)]
            public uint TimeLastGet;

            [ProtoMember(3, IsRequired = true)]
            public string TimeOut;

            [ProtoMember(4, IsRequired = true)]
            public uint AllowBuyCount;
        }
    }
}
