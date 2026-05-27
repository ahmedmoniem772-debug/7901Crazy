using ConquerOnline.Client;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static void GetTexasMatchInfo(this ServerSockets.Packet stream, out MsgTexasExMatchFieldList.TexasExMatchFieldList pQuery)
        {
            pQuery = new MsgTexasExMatchFieldList.TexasExMatchFieldList();
            pQuery = stream.ProtoBufferDeserialize<MsgTexasExMatchFieldList.TexasExMatchFieldList>(pQuery);
        }
        public static ServerSockets.Packet CreateTexasMatchInfo(this ServerSockets.Packet stream, MsgTexasExMatchFieldList.TexasExMatchFieldList obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgTexasExMatchFieldList);
            return stream;
        }

    }
    public class MsgTexasExMatchFieldList
    {
        public enum MatchFieldListAction : uint
        {
            View = 1
        }
        [ProtoContract]
        public class TexasExMatchFieldList
        {
            [ProtoMember(1, IsRequired = true)]
            public MatchFieldListAction Action;

            [ProtoMember(2, IsRequired = true)]
            public List<Room> Rooms;

        }
        [ProtoContract]
        public class Room
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ID;

            [ProtoMember(2, IsRequired = true)]
            public uint PlayersCount;
        }
    }
}
