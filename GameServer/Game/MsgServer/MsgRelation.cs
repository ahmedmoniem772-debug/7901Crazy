using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static class MsgRelation
    {
        public static unsafe ServerSockets.Packet RelationCreate(this ServerSockets.Packet stream, Role.Player Requester, Role.Player Receiver)
        {
            stream.InitWriter();
            stream.Write(Requester.UID);
            stream.Write(Receiver.UID);
            stream.Write((uint)Requester.Level);
            stream.Write(Requester.BattlePower);
            stream.Write(Receiver.SpouseUID == Requester.SpouseUID && Receiver.SpouseUID > 0);
            stream.Write(Receiver.Associate.Contain(Role.Instance.Associate.Friends, Requester.UID));
            stream.Write(Receiver.Associate.Contain(Role.Instance.Associate.Partener, Requester.UID));
            stream.Write(Receiver.Associate.Contain(Role.Instance.Associate.Mentor, Requester.UID));
            stream.Write(false);
            stream.Write(Receiver.Owner.Team != null && Receiver.Owner.Team.Members.ContainsKey(Requester.UID));
            stream.Write(Receiver.MyGuild != null && Receiver.MyGuild.Members.ContainsKey(Requester.UID));
            stream.Write(Receiver.Associate.Contain(Role.Instance.Associate.Enemy, Requester.UID));
            stream.ZeroFill(4);
            stream.Finalize(GamePackets.MsgRelation);

            return stream;
        }
    }
}