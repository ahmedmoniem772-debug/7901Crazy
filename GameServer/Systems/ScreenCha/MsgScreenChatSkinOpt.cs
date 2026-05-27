using ProtoBuf;
using System;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateMsgScreenChatSkinOpt(this ServerSockets.Packet stream, MsgMsgScreenChatSkinOpt pQuery)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgScreenChatSkinOpt);
            return stream;
        }
        public static void GetMsgScreenChatSkinOpt(this ServerSockets.Packet stream, out MsgMsgScreenChatSkinOpt pQuery)
        {
            pQuery = new MsgMsgScreenChatSkinOpt();
            pQuery = stream.ProtoBufferDeserialize<MsgMsgScreenChatSkinOpt>(pQuery);
        }

    }

    [ProtoContract]
    public class MsgMsgScreenChatSkinOpt
    {
        [Flags]
        public enum ActionID : uint
        {
            Add = 1,


        }

        public enum ChatSkin : uint
        {

            ChatBubble1 = 10000,
            BlessedYongding = 10002,
            PoeticCharm = 10003,


        }

        [ProtoMember(1, IsRequired = true)]
        public uint TotNum;
        [ProtoMember(2, IsRequired = true)]
        public ActionID Action;

        [PacketAttribute(GamePackets.MsgScreenChatSkinOpt)]
        public unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            MsgMsgScreenChatSkinOpt Info = null;
            Info = new MsgMsgScreenChatSkinOpt();
            Info = stream.ProtoBufferDeserialize<MsgMsgScreenChatSkinOpt>(Info);
            switch (Info.Action)
            {
                case ActionID.Add:
                    {
                        user.Send(stream.CreateMsgScreenChatSkinOpt(Info));
                        user.Player.View.SendView(user.Player.GetArray(stream, false), false);

                    }
                    break;
            }

        }
    }
}
