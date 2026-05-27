using ProtoBuf;
using System.Linq;

namespace MortalConquer.Game.MsgServer
{
    public static class MsgHundredWeaponsInfo
    {

        [ProtoContract]
        public class MsgHundredWeaponsInfoProto
        {
            [ProtoMember(1, IsRequired = true)]
            public ActionID Type;
            [ProtoMember(2, IsRequired = true)]
            public uint OwnerUID;
            [ProtoMember(3, IsRequired = true)]
            public WeaponInfo[] Weapons;
        }
        [ProtoContract]
        public class WeaponInfo
        {
            [ProtoMember(1, IsRequired = true)]
            public Database.MagicType.WeaponsType SubType;
            [ProtoMember(2, IsRequired = true)]
            public uint AppearancePosition;
            [ProtoMember(3, IsRequired = true)]
            public uint PositionFlags;
            [ProtoMember(4, IsRequired = true)]
            public byte Level;
            [ProtoMember(5, IsRequired = true)]
            public uint Progress;
            [ProtoMember(6, IsRequired = true)]
            public int Hitpoints;
            [ProtoMember(7, IsRequired = true)]
            public int PhysicalAttack;
            [ProtoMember(8, IsRequired = true)]
            public int PhysicalDefense;
            [ProtoMember(9, IsRequired = true)]
            public int MagicAttack;
            [ProtoMember(10, IsRequired = true)]
            public int MagicDefense;
            [ProtoMember(12, IsRequired = true)]
            public uint Score;
        }
        public enum ActionID : int
        {
            None,
            Update,
            RequestInfo,
            Add
        }
        public static unsafe ServerSockets.Packet CreateHundredWeaponsInfo(this ServerSockets.Packet stream, Client.GameClient client, ActionID type)
        {
            MsgHundredWeaponsInfoProto msg = new MsgHundredWeaponsInfoProto() { Type = type };
            if (client != null)
            {
                msg.OwnerUID = client.Player.UID;
                msg.Weapons = new WeaponInfo[client.HundredWeapons.Objects.Count];
                for (int i = 0; i < msg.Weapons.Length; i++)
                {
                    var obj = client.HundredWeapons.Objects.Values.OrderBy(w => (ushort)w.WeaponSubtype).ToArray()[i];
                    msg.Weapons[i] = new WeaponInfo()
                    {
                        SubType = obj.WeaponSubtype,
                        Progress = obj.Progress,
                        Level = obj.Level,
                        Hitpoints = (byte)Database.HundredWeapons.AttributeType.Hitpoints * 10000 + (obj.Attributes[Database.HundredWeapons.AttributeType.Hitpoints] - obj.DBInfo.Attributes[Database.HundredWeapons.AttributeType.Hitpoints]),
                        PhysicalAttack = (byte)Database.HundredWeapons.AttributeType.PhysicalAttack * 10000 + (obj.Attributes[Database.HundredWeapons.AttributeType.PhysicalAttack] - obj.DBInfo.Attributes[Database.HundredWeapons.AttributeType.PhysicalAttack]),
                        PhysicalDefense = (byte)Database.HundredWeapons.AttributeType.PhysicalDefense * 10000 + (obj.Attributes[Database.HundredWeapons.AttributeType.PhysicalDefense] - obj.DBInfo.Attributes[Database.HundredWeapons.AttributeType.PhysicalDefense]),
                        MagicAttack = (byte)Database.HundredWeapons.AttributeType.MagicAttack * 10000 + (obj.Attributes[Database.HundredWeapons.AttributeType.MagicAttack] - obj.DBInfo.Attributes[Database.HundredWeapons.AttributeType.MagicAttack]),
                        MagicDefense = (byte)Database.HundredWeapons.AttributeType.MagicDefense * 10000 + (obj.Attributes[Database.HundredWeapons.AttributeType.MagicDefense] - obj.DBInfo.Attributes[Database.HundredWeapons.AttributeType.MagicDefense]),
                        PositionFlags = obj.BitVector.bits[0],
                        AppearancePosition = obj.AppearancePosition,
                        Score = obj.Score,
                    };
                }
            }
            stream.InitWriter();
            stream.ProtoBufferSerialize(msg);
            stream.Finalize(GamePackets.MsgHundredWeaponsInfo);
            return stream;
        }
        [PacketAttribute(GamePackets.MsgHundredWeaponsInfo)]
        public static unsafe void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            MsgHundredWeaponsInfoProto msg = new MsgHundredWeaponsInfoProto();
            msg = stream.ProtoBufferDeserialize<MsgHundredWeaponsInfoProto>(msg);

            switch (msg.Type)
            {
                // case ActionID.RequestInfo:
                default:
                    {
                        if (Pool.GamePoll.ContainsKey(msg.OwnerUID))
                            client.Send(stream.CreateHundredWeaponsInfo(Pool.GamePoll[msg.OwnerUID], msg.Type));
                        else
                            client.Send(stream.CreateHundredWeaponsInfo(null, msg.Type));
                        break;
                    }
            }
        }
    }
}
