using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public unsafe partial class MsgBuilder
    {
        [ProtoContract]
        public class MapStatusProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ID;
            [ProtoMember(2, IsRequired = true)]
            public uint BaseID;
            [ProtoMember(3, IsRequired = true)]
            public string Status;
            [ProtoMember(4)]
            public uint Weather;
            [ProtoMember(5)]
            public uint ID2;
        }
        public static unsafe ServerSockets.Packet MapStatusCreate(this ServerSockets.Packet stream, uint MapID, uint BaseID, ulong Status)
        {
            stream.InitWriter();
            if (MapID == 1002)
            {
                stream.ProtoBufferSerialize(new MsgBuilder.MapStatusProto()
                {
                    ID = 1002,
                    BaseID = 1002,
                    Status = "3,4,13,19,46,47",
                    ID2 = 1002
                });
            }
            else if (MapID == 1036)
            {
                stream.ProtoBufferSerialize(new MsgBuilder.MapStatusProto()
                {
                    ID = 1036,
                    BaseID = 1036,
                    Status = "1,2,3,4",
                    ID2 = 1036
                });
            }
            else if (MapID == 2057)
            {
                stream.ProtoBufferSerialize(new MsgBuilder.MapStatusProto()
                {
                    ID = 2057,
                    BaseID = 2057,
                    Status = "1,2,16,20,30,31,51,60",
                    ID2 = 2057
                });
            }
            else
            {
                stream.ProtoBufferSerialize(new MsgBuilder.MapStatusProto()
                {
                    ID = MapID,
                    BaseID = BaseID,
                    Status = Status.ToString(),
                    ID2 = MapID
                });
            }
            stream.Finalize(GamePackets.MsgMapInfo);
            return stream;
        }
    }
}
