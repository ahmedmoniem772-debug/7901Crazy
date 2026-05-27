using System;
using ConquerOnline.Client;
using ConquerOnline.Database;
using ConquerOnline.Game.MsgMonster;
using ConquerOnline.Game.MsgServer;

namespace ConquerOnline.Role
{
    public unsafe class PetInfo
    {
        public Extensions.Time32 AttackStamp = new Extensions.Time32();

        public Game.MsgMonster.MonsterFamily Family;

        public MonsterRole monster;

        public IMapObj Target { get; set; }

        public Client.GameClient Owner;

        public static sbyte[] XDir = new sbyte[] { 0, -1, -1, -1, 0, 1, 1, 1 };
        public static sbyte[] YDir = new sbyte[] { 1, 1, 0, -1, -1, -1, 0, 1 };
        public static sbyte[] XDir2 = new sbyte[] { 0, -2, -2, -2, 0, 2, 2, 2, -1, -2, -2, -1, 1, 2, 2, 1, -1, -2, -2, -1, 1, 2, 2, 1 };
        public static sbyte[] YDir2 = new sbyte[] { 2, 2, 0, -2, -2, -2, 0, 2, 2, 1, -1, -2, -2, -1, 1, 2, 2, 1, -1, -2, -2, -1, 1, 2 };

        public void PetInformation(Role.Player role, uint UID)
        {
            Owner = role.Owner;
            Owner.Pet = this;
            Family = new Game.MsgMonster.MonsterFamily();
            Family.SpellId = Server.Pets.ReadUInt16(UID.ToString(), "SpellID", 0);
            Family.Level = Server.Pets.ReadUInt16(UID.ToString(), "Level", 0);
            Family.MaxAttack = Server.Pets.ReadInt32(UID.ToString(), "Attack", 0);
            Family.MinAttack = Family.MaxAttack;
            Family.Mesh = Server.Pets.ReadUInt16(UID.ToString(), "Mesh", 0);
            Family.MaxHealth = Server.Pets.ReadInt32(UID.ToString(), "Hitpoints", 0);
            Family.Defense = Server.Pets.ReadUInt16(UID.ToString(), "Defence", 0);
            Family.AttackRange = Server.Pets.ReadSByte(UID.ToString(), "AttackRange", 0);
            Family.Name = Server.Pets.ReadString(UID.ToString(), "Name", "ERROR");
            Family.MapID = role.Map;
        }

        public PetInfo(Role.Player role, uint UID, ServerSockets.Packet stream)
        {
            PetInformation(role, UID);

            #region SendMonster
            monster = new MonsterRole(Family, Family.ID, string.Empty, Owner.Map);
            monster.ObjType = MapObjectType.Monster;
            monster.UID = 700000 + (role.UID - 1000000);
            monster.Name = Family.Name;
            monster.Level = (byte)Family.Level;
            monster.Mesh = Family.Mesh;
            monster.HitPoints = (uint)Family.MaxHealth;
            monster.X = Owner.Player.X;
            monster.Y = Owner.Player.Y;
            if (monster.HitPoints > 0)
            {
                Game.MsgServer.MsgUpdate Upd = new Game.MsgServer.MsgUpdate(stream, monster.UID, 2);
                stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.MaxHitpoints, Family.MaxHealth);
                stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.Hitpoints, monster.HitPoints);
                stream = Upd.GetArray(stream);
                Owner.Send(stream);
            }
            #endregion

            #region ReviveMonster
            ActionQuery action = new ActionQuery()
            {
                ObjId = monster.UID,
                Type = ActionType.ReviveMonster,
                PositionX = Owner.Player.X,
                PositionY = Owner.Player.Y
            };
            monster.Send(stream.ActionCreate(action));
            #endregion

            monster.Send(monster.GetArray(stream, false));
            Owner.Player.Send(stream.CreatePetInfo(UID, Family.ID, monster.PetFlag, monster.Mesh, 14, monster.X, monster.Y, monster.Name));
            Owner.Player.View.SendView(GetArray(stream, false), true);
        }
        public unsafe ServerSockets.Packet GetArray(ServerSockets.Packet stream, bool view)
        {
           
            stream.InitWriter();
            var proto = new Role.Player.SpawnPacketProto()
            {
                UID = monster.UID,
                Mesh = monster.Mesh,
                MonsterLevel = monster.Level,
                Hitpoints = monster.IsFloor ? (uint)monster.StampFloorSecounds : (uint)monster.HitPoints,
                X = monster.X,
                Y = monster.Y,
                Action = (ushort)monster.Action,
                Level = monster.Level,
                MonstersID = Family.ID,
                Names = new string[4] { monster.Name, string.Empty, string.Empty, string.Empty }
            };
            proto.Boss = monster.Boss > 0;
            
            proto.MaxLife = (uint)Family.MaxHealth;
            proto.StatusFlags = new ulong[monster.BitVector.bits.Length];
            for (int x = 0; x < monster.BitVector.bits.Length; x++)
                proto.StatusFlags[x] = monster.BitVector.bits[x];

            uint key = (uint)(Family.MaxHealth / 10000);
            if (key != 0)
                proto.Hitpoints = (uint)(proto.Hitpoints / key);
            else
                proto.Hitpoints = (uint)(proto.Hitpoints * Family.MaxHealth);
            stream.ProtoBufferSerialize(proto);
            stream.Finalize(Game.GamePackets.MsgPlayer);
            return stream;
        }
        public bool Move(Flags.ConquerAngle Direction)
        {
            ushort _X = monster.X, _Y = monster.Y;
            monster.Facing = Direction;
            int dir = ((int)Direction) % XDir.Length;
            sbyte xi = XDir[dir], yi = YDir[dir];
            _X = (ushort)(monster.X + xi);
            _Y = (ushort)(monster.Y + yi);
            Core.IncXY((Flags.ConquerAngle)dir, ref _X, ref _Y);
            if (monster.GMap.ValidLocation(_X, _Y) && !monster.GMap.MonsterOnTile(_X, _Y))
            {
                monster.GMap.SetMonsterOnTile(monster.X, monster.Y, false);
                monster.GMap.SetMonsterOnTile(_X, _Y, true);

                monster.GMap.View.MoveTo<Role.IMapObj>(monster, _X, _Y);
                monster.X = _X;
                monster.Y = _Y;
                return true;
            }
            return false;
        }

        public void RevivePet(ServerSockets.Packet stream)
        {
            Owner.Pet.monster.GMap.View.EnterMap<MonsterRole>(monster);
        }

        public void DeadPet(ServerSockets.Packet stream)
        {
            ActionQuery action = new ActionQuery()
            {
                ObjId = monster.UID,
                Type = ActionType.RemoveEntity,
            };
            Owner.Send(stream.ActionCreate(action));
            Owner.Player.View.SendView(stream.ActionCreate(action), false);
            monster.GMap.View.LeaveMap<MonsterRole>(monster);
            Owner.Pet = null;
        }
    }

}