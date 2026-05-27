using VirusX.Client;
using VirusX.Database;
using VirusX.Game.MsgMonster;
using VirusX.Role;
using VirusX.Role.Instance;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirusX.Game.MsgServer
{
    public static class MsgSynFormInfo
    {

       
        [ProtoContract]
        public class MsgGuildConstruct
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2, IsRequired = true)]
            public ulong Material;
            [ProtoMember(3, IsRequired = true)]
            public uint uk3;
            [ProtoMember(4, IsRequired = true)]
            public MsgSynFormInfo.MsgGuildConstruct.Construct[] construct;

            [ProtoContract]
            public class Construct
            {
                [ProtoMember(1, IsRequired = true)]
                public uint Type;
                [ProtoMember(2, IsRequired = true)]
                public ulong Beast;
                [ProtoMember(3, IsRequired = true)]
                public uint Level;
                [ProtoMember(4, IsRequired = true)]
                public ulong Exp;
                [ProtoMember(5, IsRequired = true)]
                public ulong v2;
                [ProtoMember(6, IsRequired = true)]
                public ulong v3;
                [ProtoMember(7, IsRequired = true)]
                public ulong v4;

                public Construct(Role.Instance.Guild.Construct obj)
                {
                     Type = obj.ID;
                     Level = obj.Level;
                     Exp = obj.Exp;
                     Beast = obj.Beast;
                     v2 = obj.v2;
                     v3 = obj.v3;
                     v4 = obj.v4;
                }
            }
        }
        public static unsafe ServerSockets.Packet CreateGuildConstruct(this ServerSockets.Packet stream, MsgSynFormInfo.MsgGuildConstruct obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgSynFormInfo);
            return stream;
        }
        
    }

}
