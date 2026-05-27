using ConquerOnline.Client;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static void GetNewTexasBlindsList(this ServerSockets.Packet stream, out MsgNewTexasBlindsList.ProtoStructure pQuery)
        {
            pQuery = new MsgNewTexasBlindsList.ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<MsgNewTexasBlindsList.ProtoStructure>(pQuery);
        }
        public static ServerSockets.Packet CreateNewTexasBlindsList(this ServerSockets.Packet stream, MsgNewTexasBlindsList.ProtoStructure obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgNewTexasBlindsList);
            return stream;
        }
    }
    public class MsgNewTexasBlindsList
    {

        [ProtoContract]
        public class ProtoStructure
        {
            [ProtoMember(1, IsRequired = true)]
            public byte type;
            [ProtoMember(2, IsRequired = true)]
            public List<Table> Rooms;
        }
        [ProtoContract]
        public class Table
        {
            [ProtoMember(1, IsRequired = true)]
            public long MaxMoney;
            [ProtoMember(2, IsRequired = true)]
            public long MinMoney;
            [ProtoMember(3, IsRequired = true)]
            public long LastMoney;
            [ProtoMember(4, IsRequired = true)]
            public long PlayerCount;
        }
        [PacketAttribute(GamePackets.MsgNewTexasBlindsList)]
        public unsafe static void Process(GameClient client, ServerSockets.Packet stream)
        {

        }
    }
}