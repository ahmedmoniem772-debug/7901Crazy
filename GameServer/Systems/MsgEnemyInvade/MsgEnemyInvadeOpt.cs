using ConquerOnline.Client;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ConquerOnline
{
    /*
sually means the source data is corrupt
   at ProtoBuf.ProtoReader.ReadFieldHeader()
   at proto_52(Object , ProtoReader )
   at ProtoBuf.Serializers.CompiledSerializer.ProtoBuf.Serializers.IProtoSeriali
zer.Read(Object value, ProtoReader source)
   at ProtoBuf.Meta.RuntimeTypeModel.Deserialize(Int32 key, Object value, ProtoR
eader source)
   at ProtoBuf.Meta.TypeModel.DeserializeCore(ProtoReader reader, Type type, Obj
ect value, Boolean noAutoCreate)
   at ProtoBuf.Meta.TypeModel.Deserialize(Stream source, Object value, Type type
, SerializationContext context)
   at ProtoBuf.Meta.TypeModel.Deserialize(Stream source, Object value, Type type
)
   at ProtoBuf.Serializer.Deserialize[T](Stream source)
   at ConquerOnline.ServerSockets.Packet.ProtoBufferDeserialize[T](T obj) in d:\
Source[Test]\ServerSockets\Packet.cs:line 326
   at ConquerOnline.MsgEnemyInvadeOpt.op_Implicit(Packet stream) in d:\Source[Te
st]\Systems\MsgEnemyInvade\MsgEnemyInvadeOpt.cs:line 100
   at ConquerOnline.EnemyInvade.Process1(GameClient user, Packet stream) in d:\S
ource[Test]\Systems\MsgEnemyInvade\EnemyInvade.cs:line 469
   at ConquerOnline.Program.Game_Receive(SecuritySocket obj, Packet stream) in d
:\Source[Test]\Program.cs:line 982
[12:53:08]: ProtoBuf.ProtoException: Unexpected end-group in source data; this u
sually means the source data is corrupt
   at ProtoBuf.ProtoReader.ReadFieldHeader()
   at proto_52(Object , ProtoReader )
   at ProtoBuf.Serializers.CompiledSerializer.ProtoBuf.Serializers.IProtoSeriali
zer.Read(Object value, ProtoReader source)
   at ProtoBuf.Meta.RuntimeTypeModel.Deserialize(Int32 key, Object value, ProtoR
eader source)
   at ProtoBuf.Meta.TypeModel.DeserializeCore(ProtoReader reader, Type type, Obj
ect value, Boolean noAutoCreate)
   at ProtoBuf.Meta.TypeModel.Deserialize(Stream source, Object value, Type type
, SerializationContext context)
   at ProtoBuf.Meta.TypeModel.Deserialize(Stream source, Object value, Type type
)
   at ProtoBuf.Serializer.Deserialize[T](Stream source)
   at ConquerOnline.ServerSockets.Packet.ProtoBufferDeserialize[T](T obj) in d:\
Source[Test]\ServerSockets\Packet.cs:line 326
   at ConquerOnline.MsgEnemyInvadeOpt.op_Implicit(Packet stream) in d:\Source[Te
st]\Systems\MsgEnemyInvade\MsgEnemyInvadeOpt.cs:line 100
   at ConquerOnline.EnemyInvade.Process1(GameClient user, Packet stream) in d:\S
ource[Test]\Systems\MsgEnemyInvade\EnemyInvade.cs:line 469
   at ConquerOnline.Program.Game_Receive(SecuritySocket obj, Packet stream) in d
     */
    [ProtoContract]
    public class MsgEnemyInvadeOpt
    {
        public enum TypeID : byte
        {
            Login = 0,
            Start = 1,
            ClaimRewards = 1,
            Info = 3,
            Fight = 4,
            Change = 5,
            Show = 7,
        }
        [ProtoMember(1, IsRequired = true)]
        public uint Type = 0;
        [ProtoMember(2, IsRequired = true)]
        public uint UserID = 0;
        [ProtoMember(3, IsRequired = true)]
        public uint MyPoint = 0;
        [ProtoMember(4, IsRequired = true)]
        public uint StartTime = 0;
        [ProtoMember(5, IsRequired = true)]
        public uint Chance = 0;
        [ProtoMember(6, IsRequired = true)]
        public uint Recovery = 0;
        [ProtoMember(7, IsRequired = true)]
        public uint uk7 = 0;
        [ProtoMember(8, IsRequired = true)]
        public uint uk8 = 0;
        [ProtoMember(9, IsRequired = true)]
        public uint uk9 = 0;
        [ProtoMember(10, IsRequired = true)]
        public uint uk10 = 0;
        [ProtoMember(11, IsRequired = true)]
        public uint uk11 = 0;
        [ProtoMember(12, IsRequired = true)]
        public List<Player> Players = new List<Player>();
        [ProtoContract]
        public class Player
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ID = 0;
            [ProtoMember(2, IsRequired = true)]
            public uint Point = 0;
        }
        public static implicit operator MsgEnemyInvadeOpt(ConquerOnline.ServerSockets.Packet stream)
        {
            MsgEnemyInvadeOpt pQuery = new MsgEnemyInvadeOpt();
            pQuery = stream.ProtoBufferDeserialize(pQuery);
            return pQuery;
        }
        public static implicit operator ConquerOnline.ServerSockets.Packet(MsgEnemyInvadeOpt obj)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(obj);
                stream.Finalize(2433);
                return stream;
            }
        }
    }
}
