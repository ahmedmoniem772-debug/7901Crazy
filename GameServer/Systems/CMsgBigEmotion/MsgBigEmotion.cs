using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public static class MsgBigEmotion
    {
        [ProtoContract]
        public class MsgBigEmotionProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint PlayerUID;
            [ProtoMember(2, IsRequired = true)]
            public uint EmotionID;
            [ProtoMember(3, IsRequired = true)]
            public uint Member3;
            [ProtoMember(4, IsRequired = true)]
            public uint Member4;
            [ProtoMember(5, IsRequired = true)]
            public uint Member5;
            [ProtoMember(6, IsRequired = true)]
            public uint Member6;
            [ProtoMember(7, IsRequired = true)]
            public uint Member7;
            [ProtoMember(8, IsRequired = true)]
            public uint Member8;
            [ProtoMember(9, IsRequired = true)]
            public uint Member9;
            [ProtoMember(10, IsRequired = true)]
            public uint Member10;
        }
        public static unsafe void GetBigEmotion(this ServerSockets.Packet stream, out MsgBigEmotionProto pQuery)
        {
            pQuery = new MsgBigEmotionProto();
            pQuery = stream.ProtoBufferDeserialize(pQuery);
        }
        public static unsafe ServerSockets.Packet CreateBigEmotion(this ServerSockets.Packet stream, MsgBigEmotionProto obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize((ushort)GamePackets.MsgBigEmotion);
            return stream;
        }
        [Packet((ushort)GamePackets.MsgBigEmotion)]
        private static unsafe void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            MsgBigEmotionProto Info;
            stream.GetBigEmotion(out Info);

            if (client.HairfaceStorage.Exists(Info.EmotionID, MsgHairfaceStorage.Type.Emoji))
            {
                Info.PlayerUID = client.Player.UID;
                client.Send(stream.CreateBigEmotion(Info));
                client.Player.View.SendView(stream.CreateBigEmotion(Info), true);
            }

        }
    }
}