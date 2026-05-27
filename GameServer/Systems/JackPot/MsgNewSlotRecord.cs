//using VirusX.Client;
//using ProtoBuf;

//namespace VirusX.Game.MsgServer
//{
//    public unsafe static partial class MsgBuilder
//    {
//        public static unsafe ServerSockets.Packet CreateNewSlotRecord(this ServerSockets.Packet stream, MsgNewSlotRecord pQuery)
//        {
//            stream.InitWriter();
//            stream.ProtoBufferSerialize(pQuery);
//            stream.Finalize(GamePackets.MsgNewSlotRecord);
//            return stream;
//        }
//        public static void GetNewSlotRecord(this ServerSockets.Packet stream, out MsgNewSlotRecord pQuery)
//        {
//            pQuery = new MsgNewSlotRecord();
//            pQuery = stream.ProtoBufferDeserialize<MsgNewSlotRecord>(pQuery);
//        }


//    }
//    [ProtoContract]
//    public class MsgNewSlotRecord
//    {
//        [ProtoMember(1, IsRequired = true)]
//        public long Member1;//0
//        [ProtoMember(2, IsRequired = true)]
//        public long[] Member2;//29234
//        [ProtoMember(3, IsRequired = true)]
//        public long Member3;//20000

//        [PacketAttribute(GamePackets.MsgNewSlotRecord)]
//        private static void Process(GameClient user, ServerSockets.Packet stream)
//        {
//            MsgNewSlotRecord pQuery;
//            stream.GetNewSlotRecord(out pQuery);

//        }

//    }

//}
