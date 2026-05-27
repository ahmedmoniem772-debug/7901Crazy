using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        [ProtoContract]
        public class MsgDataProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2, IsRequired = true)]
            public int Year;
            [ProtoMember(3, IsRequired = true)]
            public int Month;
            [ProtoMember(4, IsRequired = true)]
            public int DayOfYear;
            [ProtoMember(5, IsRequired = true)]
            public int DayOfMonth;
            [ProtoMember(6, IsRequired = true)]
            public int Hour;
            [ProtoMember(7, IsRequired = true)]
            public int Minute;
            [ProtoMember(8, IsRequired = true)]
            public int Second;
            [ProtoMember(9, IsRequired = true)]
            public long Member9;//15
        }
        public static unsafe ServerSockets.Packet ServerTimerCreate(this ServerSockets.Packet stream)
        {
            stream.InitWriter();
            DateTime now = DateTime.Now;
            stream.ProtoBufferSerialize(new MsgDataProto() { Year = now.Year - 1900, Month = now.Month - 1, DayOfYear = now.DayOfYear, DayOfMonth = now.Day, Hour = now.Hour, Minute = now.Minute, Second = now.Second });

            stream.Finalize(GamePackets.MsgData);
            return stream;
        }
        public static unsafe ServerSockets.Packet DeathTimerCreate(this ServerSockets.Packet stream, uint DeathTimer)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(new MsgDataProto() { Year = (int)DeathTimer, Type = 5 });
            stream.Finalize(GamePackets.MsgData);
            return stream;
        }
        public static unsafe ServerSockets.Packet ServerInfoCreate(this ServerSockets.Packet stream, uint Vigor)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(new MsgDataProto() { Year = (int)Vigor, Type = 2});
            stream.Finalize(GamePackets.MsgData);
            return stream;
        }
        public static unsafe ServerSockets.Packet ServerVerCreate(this ServerSockets.Packet stream)
        {
            stream.InitWriter();
            stream.Write(0);//Server Type (1=Classic)
            stream.Write(0);//Potency Type
            stream.Finalize(GamePackets.MsgServerInfo);
            return stream;
        }

    }
}
