//using VirusX.Client;
//using ProtoBuf;

//namespace VirusX.Game.MsgServer
//{
//    public unsafe static partial class MsgBuilder
//    {
//        public static unsafe ServerSockets.Packet CreateNewSlotResult(this ServerSockets.Packet stream, MsgNewSlotResult pQuery)
//        {
//            stream.InitWriter();
//            stream.ProtoBufferSerialize(pQuery);
//            stream.Finalize(GamePackets.MsgNewSlotResult);
//            return stream;
//        }
//        public static void GetNewSlotResult(this ServerSockets.Packet stream, out MsgNewSlotResult pQuery)
//        {
//            pQuery = new MsgNewSlotResult();
//            pQuery = stream.ProtoBufferDeserialize<MsgNewSlotResult>(pQuery);
//        }

//    }
//    [ProtoContract]
//    public class MsgNewSlotResult
//    {
//        [ProtoMember(1, IsRequired = true)]
//        public long Member1;//0
//        [ProtoMember(2, IsRequired = true)]
//        public long Member2;//29234
//        [ProtoMember(3, IsRequired = true)]
//        public long Member3;//12000
//        [ProtoMember(4, IsRequired = true)]
//        public long Member4;//10000
//        [ProtoMember(5, IsRequired = true)]
//        public long Member5;//0
//        [ProtoMember(6, IsRequired = true)]
//        public long Member6;//0
//        [ProtoMember(7, IsRequired = true)]
//        public ushort[] Count;
//        [ProtoMember(8, IsRequired = true)]
//        public long Member8;
//        [ProtoMember(9, IsRequired = true)]
//        public long Member9;
//        //public long[8]{6,4,4,3,4,1,2,1,}
//        [PacketAttribute(GamePackets.MsgNewSlotResult)]
//        private static void Process(GameClient user, ServerSockets.Packet stream)
//        {
//            MsgNewSlotResult pQuery;
//            stream.GetNewSlotResult(out pQuery);

//        }

//    }
//}
