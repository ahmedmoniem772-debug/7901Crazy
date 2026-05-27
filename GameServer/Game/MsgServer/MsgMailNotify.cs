using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet MailNotifyCreate(this ServerSockets.Packet stream, MailNotifyType type)
        {
            stream.InitWriter();

            stream.Write((ushort)type);
            stream.Finalize(GamePackets.MsgMailNotify);
            return stream;
        }
    }
    public enum MailNotifyType : byte
    {
        None,
        CannotDelete,//Failed to delete. Please take out the items from the mail first.
        Hidden,
        Showen,
        Unknown
    }
}