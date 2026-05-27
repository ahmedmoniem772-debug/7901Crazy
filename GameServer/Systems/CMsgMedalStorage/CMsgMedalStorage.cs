using VirusX.Game;
using VirusX.Client;
using VirusX.Game.MsgServer;
using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace VirusX
{
    [Flags]
    public enum MsgMedalStorageType : uint
    {
        Load = 0,
        Create = 1,
        Equip = 3,
        UnEquip = 4,
        View = 6,
    }
    [ProtoContract]
    public class MsgMedalStorage
    {
        [ProtoMember(1, IsRequired = true)]
        public MsgMedalStorageType Type;
        [ProtoMember(2, IsRequired = true)]
        public uint Id;
        [ProtoMember(3, IsRequired = true)]
        public uint OK;//?
        [ProtoMember(4, IsRequired = true)]
        public List<Medal> Medals = new List<Medal>();
        [ProtoContract]
        public class Medal
        {
            [ProtoMember(1, IsRequired = true)]
            public uint u1;
            [ProtoMember(2, IsRequired = true)]
            public uint Type;
            [ProtoMember(3, IsRequired = true)]
            public bool Activate;
            [ProtoMember(4, IsRequired = true)]
            public ulong DateTime;
            [ProtoMember(5, IsRequired = true)]
            public uint DelTime;
        }
        public ServerSockets.Packet ToArray()
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(this);
                stream.Finalize(Game.GamePackets.MsgMedalStorage);
                return stream;
            }
        }
        public static implicit operator ServerSockets.Packet(MsgMedalStorage obj)
        {
            return obj.ToArray();
        }
    }
    public class MedalStorage
    {
        public GameClient User;
        public MedalStorage(GameClient user)
        {
            User = user;
        }
        public class Medal
        {
            public uint UserId;
            public uint u1;
            public uint Type;
            public bool Activate;
            public ulong DateTime;
            public uint DelTime;
        }
        public ConcurrentDictionary<uint, Medal> Medals = new ConcurrentDictionary<uint, Medal>();
        public MsgMedalStorage GetMsgMedals(MsgMedalStorageType type, uint UserId, params Medal[] Medals)
        {
            MsgMedalStorage msg = new MsgMedalStorage()
            {
                Type = type,
                Id = UserId,
            };
            foreach (var Medal in Medals)
            {
                msg.Medals.Add(new MsgMedalStorage.Medal()
                {
                    Activate = Medal.Activate,
                    DelTime = (uint)Math.Max(0, (Medal.DelTime - (ulong)Timestamp())),
                    DateTime = Medal.DateTime,
                    Type = Medal.Type,
                    u1 = Medal.u1,
                });
            }
            return msg;
        }
        public void Loading()
        {
            //free
            Create(1, (7 * 24 * 60 * 60));
            Create(2, (7 * 24 * 60 * 60));
            Create(3, (7 * 24 * 60 * 60));
            Create(4, (7 * 24 * 60 * 60));
            Create(5, (7 * 24 * 60 * 60));
            Create(6, (7 * 24 * 60 * 60));
            Create(7, (7 * 24 * 60 * 60));
            Create(8, (7 * 24 * 60 * 60));
            Create(9, (7 * 24 * 60 * 60));
            Create(10, (7 * 24 * 60 * 60));
            Create(11, (7 * 24 * 60 * 60));
            User.Send(GetMsgMedals(MsgMedalStorageType.Load, 0, Medals.Values.ToArray()));
        }
        public uint Immunity;
        public uint Resit;
        public uint Counteraction;
        public static int Timestamp()
        {
            return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime()).TotalSeconds);
        }
        public void Update()
        {
            Immunity = 0;
            Resit = 0;
            Counteraction = 0;
            Medal medal;
            if (Medals.TryGetValue(2, out medal))
            {
                if (medal.Activate)
                {
                    Counteraction += 100;
                    Immunity += 100;
                    Resit += 500;
                }
            }
        }
        public void Update(MsgStatus status)
        {
            status.Counteraction += Counteraction;
            status.Immunity += Immunity;
            status.HitRate += Resit;
        }
        public void Create(uint type, uint delTime = 0)
        {
            if (type >= 1 && type <= 11)
            {
                if (!Medals.ContainsKey(type))
                {
                    Medal Medal = new Medal()
                    {
                        UserId = User.Player.UID,
                        Type = type,
                        DateTime = ulong.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                        DelTime = (uint)(Timestamp() + delTime),
                    };
                    Medals.TryAdd(Medal.Type, Medal);
                    User.Send(GetMsgMedals(MsgMedalStorageType.Create, User.Player.UID, Medal));
                    Update();
                }
            }
        }
        public void UnEquip(MsgMedalStorage msg)
        {
            Medal medal;
            if (Medals.TryGetValue(msg.Id, out medal))
            {
                User.Player.Medal = 0;
                medal.Activate = false;
                msg.OK = 1;
                User.Send(msg);
                Update();
                User.Equipment.QueryEquipment(User.Equipment.Alternante);
            }
        }
        public void Equip(MsgMedalStorage msg)
        {
            foreach (var clear in Medals.Values)
            {
                if (clear.Activate)
                {
                    User.Player.Medal = 0;
                    clear.Activate = false;
                }
            }
            Medal medal;
            if (Medals.TryGetValue(msg.Id, out medal))
            {
                User.Player.Medal = msg.Id;
                medal.Activate = true;
                msg.OK = 1;
                User.Send(msg);
            }

            msg.OK = 1;
            User.Send(msg);
            Update();
            User.Equipment.QueryEquipment(User.Equipment.Alternante);
        }
        public void View(MsgMedalStorage msg)
        {
            Client.GameClient Target;
            if (Pool.GamePoll.TryGetValue(msg.Id, out Target))
            {
                User.Send(GetMsgMedals(MsgMedalStorageType.View, Target.Player.UID, Target.MedalStorage.Medals.Values.ToArray()));
            }
        }
        public void Load(string line)
        {
            Database.DBActions.ReadLine reader = new Database.DBActions.ReadLine(line, '/');
            int nCount = reader.Read((int)0);
            for (int count = 0; count < nCount; count++)
            {

                uint nUserId = reader.Read((uint)0);
                uint nu1 = reader.Read((uint)0);
                uint nType = reader.Read((uint)0);
                bool nActivate = reader.Read((byte)0) == 1;
                ulong nDateTime = reader.Read((ulong)0);
                uint nDelTime = reader.Read((uint)0);
                if (nDelTime >= Timestamp())
                {
                    if (nActivate)
                    {
                        User.Player.Medal = nType;
                    }
                    Medals.Add(nType, new Medal()
                    {
                        UserId = nUserId,
                        u1 = nu1,
                        Type = nType,
                        Activate = nActivate,
                        DateTime = nDateTime,
                        DelTime = nDelTime,
                    });
                }
            };
            Update();
        }
        public override string ToString()
        {
            var file = new Database.DBActions.WriteLine('/');
            file.Add(Medals.Count);
            foreach (var att in Medals.Values)
            {
                file.Add(att.UserId);
                file.Add(att.u1);
                file.Add(att.Type);
                file.Add((byte)(att.Activate ? 1 : 0));
                file.Add(att.DateTime);
                file.Add(att.DelTime);
            }
            return file.Close();
        }
        [Packet(Game.GamePackets.MsgMedalStorage)]
        private unsafe static void Process(GameClient user, ServerSockets.Packet stream)
        {
            MsgMedalStorage msg = new MsgMedalStorage();
            msg = stream.ProtoBufferDeserialize<MsgMedalStorage>(msg);
            switch (msg.Type)
            {
                case MsgMedalStorageType.View: user.MedalStorage.View(msg); break;
                case MsgMedalStorageType.Equip: user.MedalStorage.Equip(msg); break;
                case MsgMedalStorageType.UnEquip: user.MedalStorage.UnEquip(msg); break;
                default: Console.WriteLine("MsgAstredge Type: " + msg.Type + ""); break;
            }
        }
    }
}
