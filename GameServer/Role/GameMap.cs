using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using VirusX.Game.MsgServer;
using VirusX.Game.MsgFloorItem;
using VirusX.Client;


namespace VirusX.Role
{
    public class MapView
    {
        const int CELLS_PER_BLOCK = 18;

        private Extensions.Counter CounterMovement = new Extensions.Counter(1);

        public ViewPtr[,] m_setBlock;

        private int Width, Height;

        private int GetWidthOfBlock() { return (Width - 1) / CELLS_PER_BLOCK + 1; }
        private int GetHeightOfBlock() { return (Height - 1) / CELLS_PER_BLOCK + 1; }

        public MapView(int _Width, int _Height)
        {
            Width = _Width;
            Height = _Height;

            m_setBlock = new ViewPtr[GetWidthOfBlock(), GetHeightOfBlock()];
            for (int x = 0; x < GetWidthOfBlock(); x++)
                for (int y = 0; y < GetHeightOfBlock(); y++)
                    m_setBlock[x, y] = new ViewPtr();
        }

        private int Block(int nPos)
        {
            return nPos / CELLS_PER_BLOCK;
        }
        private ViewPtr BlockSet(int nPosX, int nPosY) { return m_setBlock[Block(nPosX), Block(nPosY)]; }

        public bool MoveTo<T>(T obj, int nNewPosX, int nNewPosY)
            where T : IMapObj
        {

            int nOldPosX = obj.X;
            int nOldPosY = obj.Y;
            if ((nOldPosX >= 0 && nOldPosX < Width) == false)
                return false;
            if ((nOldPosY >= 0 && nOldPosY < Height) == false)
                return false;
            if ((nNewPosX >= 0 && nNewPosX < Width) == false)
                return false;
            if ((nNewPosY >= 0 && nNewPosY < Height) == false)
                return false;

            if (Block(nOldPosX) == Block(nNewPosX) && Block(nOldPosY) == Block(nNewPosY))
                return false;

            BlockSet(nOldPosX, nOldPosY).RemoveObject<T>(obj);
            BlockSet(nNewPosX, nNewPosY).AddObject<T>(obj);

            if (obj.ObjType == MapObjectType.Player)
                obj.IndexInScreen = CounterMovement.Next;

            return true;
        }

        public bool EnterMap<T>(T obj)
           where T : IMapObj
        {
            if ((obj.X >= 0 && obj.X < Width) == false)
                return false;
            if ((obj.Y >= 0 && obj.Y < Height) == false)
                return false;

            BlockSet(obj.X, obj.Y).AddObject<T>(obj);

            if (obj.ObjType == MapObjectType.Player)
                obj.IndexInScreen = CounterMovement.Next;

            return true;
        }
        public bool LeaveMap<T>(T obj)
             where T : IMapObj
        {
            if ((obj.X >= 0 && obj.X < Width) == false)
                return false;
            if ((obj.Y >= 0 && obj.Y < Height) == false)
                return false;

            BlockSet(obj.X, obj.Y).RemoveObject<T>(obj);

            return true;
        }
        public IEnumerable<IMapObj> Roles(MapObjectType typ, int X, int Y, Predicate<IMapObj> P = null)
        {

            for (int x = Math.Max(Block(X) - 1, 0); x <= Block(X) + 1 && x < GetWidthOfBlock(); x++)
                for (int y = Math.Max(Block(Y) - 1, 0); y <= Block(Y) + 1 && y < GetHeightOfBlock(); y++)
                {
                    var list = m_setBlock[x, y].GetObjects(typ);
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i >= list.Count)
                                break;
                            var element = list[i];
                            if (element != null)
                            {
                                if (P != null)
                                {
                                    if (P(element))
                                        yield return element;
                                }
                                else if (element != null)
                                    yield return element;
                            }
                        }
                    }
                }


        }
        public int CountRoles(MapObjectType typ, int X, int Y)
        {
            int count = 0;
            for (int x = Math.Max(Block(X) - 1, 0); x <= Block(X) + 1 && x < GetWidthOfBlock(); x++)
                for (int y = Math.Max(Block(Y) - 1, 0); y <= Block(Y) + 1 && y < GetHeightOfBlock(); y++)
                {
                    var list = m_setBlock[x, y].GetObjects(typ);
                    count += list.Count;
                }
            return count;
        }
        public IEnumerable<IMapObj> GetAllMapRoles(MapObjectType typ, Predicate<IMapObj> P = null)
        {
            for (int x = 0; x < GetWidthOfBlock(); x++)
                for (int y = 0; y < GetHeightOfBlock(); y++)
                {
                    var list = m_setBlock[x, y].GetObjects(typ);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i >= list.Count)
                            break;
                        var element = list[i];
                        if (element != null)
                        {
                            if (P != null)
                            {
                                if (P(element))
                                    yield return element;
                            }
                            else if (element != null)
                                yield return element;
                        }
                    }
                }
        }
        public int GetAllMapRolesCount(MapObjectType typ, Predicate<IMapObj> P = null)
        {
            return GetAllMapRoles(typ, P).Count();
        }
        public T GetMapObject<T>(MapObjectType typ, uint UID, Predicate<IMapObj> P = null)
        {
            foreach (var obj in GetAllMapRoles(typ, P))
                if (obj.UID == UID)
                    return (T)obj;
            return default(T);
        }
        public bool MapContain(MapObjectType typ, uint UID, Predicate<IMapObj> P = null)
        {
            foreach (var obj in GetAllMapRoles(typ, P))
                if (obj.UID == UID)
                    return true;
            return false;
        }
        public void ClearMap(MapObjectType typ)
        {
            for (int x = 0; x < GetWidthOfBlock(); x++)
                for (int y = 0; y < GetHeightOfBlock(); y++)
                {
                    m_setBlock[x, y].Clear(typ);
                }
        }
        public bool TryGetObject<T>(uint UID, MapObjectType typ, int X, int Y, out T obj)
            where T : IMapObj
        {
            for (int x = Math.Max(Block(X) - 1, 0); x <= Block(X) + 1 && x < GetWidthOfBlock(); x++)
                for (int y = Math.Max(Block(Y) - 1, 0); y <= Block(Y) + 1 && y < GetHeightOfBlock(); y++)
                {
                    var list = m_setBlock[x, y];
                    if (list.TryGetObject<T>(typ, UID, out obj))
                        return true;

                }
            obj = default(T);
            return false;
        }
        public bool Contain(uint UID, int X, int Y)
        {
            for (int x = Math.Max(Block(X) - 1, 0); x <= Block(X) + 1 && x < GetWidthOfBlock(); x++)
                for (int y = Math.Max(Block(Y) - 1, 0); y <= Block(Y) + 1 && y < GetHeightOfBlock(); y++)
                {
                    var list = m_setBlock[x, y];
                    for (int i = 0; i < (int)MapObjectType.Count; i++)
                        if (list.ContainObject((MapObjectType)i, UID))
                            return true;

                }
            return false;
        }
    }
    public class ViewPtr
    {
        private Extensions.MyList<Role.IMapObj>[] Objects;
        public ViewPtr()
        {
            Objects = new Extensions.MyList<IMapObj>[(int)MapObjectType.Count];
            for (int x = 0; x < (int)MapObjectType.Count; x++)
                Objects[x] = new Extensions.MyList<IMapObj>();
        }


        public void AddObject<T>(T obj)
             where T : IMapObj
        {

            Objects[(int)obj.ObjType].Add(obj);
        }

        public void RemoveObject<T>(T obj)
            where T : IMapObj
        {
            Objects[(int)obj.ObjType].Remove(obj);
        }


        public bool ContainObject(MapObjectType obj_t, uint UID)
        {
            for (int x = 0; x < Objects[(int)obj_t].Count; x++)
            {
                var list = Objects[(int)obj_t];
                if (x >= list.Count)
                    break;
                if (list[x].UID == UID)
                    return true;
            }
            return false;
        }

        public bool TryGetObject<T>(MapObjectType obj_t, uint UID, out T obj)
        {
            for (int x = 0; x < Objects[(int)obj_t].Count; x++)
            {
                var list = Objects[(int)obj_t];
                if (x >= list.Count)
                    break;
                if (list[x].UID == UID)
                {
                    obj = (T)list[x];
                    return true;
                }
            }
            obj = default(T);
            return false;
        }
        public Extensions.MyList<IMapObj> GetObjects(MapObjectType typ)
        {
            return Objects[(int)typ];
        }

        public void Clear(MapObjectType typ)
        {
            Objects[(int)typ].Clear();
        }
    }

    public class Portal
    {
        public ushort MapID { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }

        public ushort Destiantion_MapID { get; set; }
        public ushort Destiantion_X { get; set; }
        public ushort Destiantion_Y { get; set; }
    }
    [Flags]
    public enum MapFlagType : byte
    {
        None = 0,
        Valid = 1 << 0,
        Monster = 1 << 1,
        Item = 1 << 2,
        Player = 1 << 3,
        Npc = 1 << 4

    }
    [Flags]
    public enum MapTypeFlags
    {
        Normal = 0,
        PkField = 1 << 0,
        ChangeMapDisable = 1 << 1,
        RecordDisable = 1 << 2,
        PkDisable = 1 << 3,
        BoothEnable = 1 << 4,
        TeamDisable = 1 << 5,
        TeleportDisable = 1 << 6,
        GuildMap = 1 << 7,
        PrisonMap = 1 << 8,
        FlyDisable = 1 << 9,
        Family = 1 << 10,
        MineEnable = 1 << 11,
        FreePk = 1 << 12,
        NeverWound = 1 << 13,
        DeadIsland = 1 << 14
    }
    public class GameMap
    {
        public void GuildWarItem(int type)
        {
            Tuple<ushort, ushort> tuple = RandomCoordinates();
            ushort newx = tuple.Item1;
            ushort newy = tuple.Item2;
            if (IsFlagPresent(newx, newy, MapFlagType.Item) == false && IsFlagPresent(newx, newy, MapFlagType.Valid))
            {
                var Item = new Game.MsgFloorItem.MsgItem(null, newx, newy, Game.MsgFloorItem.MsgItem.ItemType.Effect, 0, 0, ID, 0, false, this, 60 * 60 * 1000);
                Item.MsgFloor.m_ID = (uint)type;
                Item.MsgFloor.m_Color = 1;
                Item.MsgFloor.Name = "STR_TRAP_ID_" + type + "@@";
                Item.MsgFloor.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                cells[newx, newy] |= MapFlagType.Item;
                View.EnterMap<Role.IMapObj>(Item);
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    Item.SendAll(stream, MsgDropID.Effect);
                }
            }
        }
        public int ContainItem(uint Look)
        {
            int Count = 0;
            foreach (var Item in View.GetAllMapRoles(MapObjectType.Item))
            {
                Game.MsgFloorItem.MsgItem obj = Item as Game.MsgFloorItem.MsgItem;
                if (obj != null)
                {
                    if (obj.MsgFloor.m_ID == Look)
                    {
                        Count++;
                    }
                }
            }
            return Count;
        }
        public Tuple<ushort, ushort> RandomCoordinates()
        {
            int times = 10000;
            int x = Pool.GetRandom.Next(bounds.Width), y = Pool.GetRandom.Next(bounds.Height);
            while (times-- > 0)
            {
                if ((cells[x, y] & MapFlagType.Valid) != MapFlagType.Valid)
                {
                    x = Pool.GetRandom.Next(bounds.Width);
                    y = Pool.GetRandom.Next(bounds.Height);
                }
                else break;
            }
            return new Tuple<ushort, ushort>((ushort)x, (ushort)y);
        }
        public static object EnterObj = new object();
        public uint RecordSteedRace = 0;

        public static sbyte[] XDir = new sbyte[]
        {
            -1, -2, -2, -1, 1, 2, 2, 1,
             0, -2, -2, -2, 0, 2, 2, 2,
            -1, -2, -2, -1, 1, 2, 2, 1,
             0, -1, -1, -1, 0, 1, 1, 1,
        };
        public static sbyte[] YDir = new sbyte[]
        {
            2,  1, -1, -2, -2, -1, 1, 2,
            2,  2,  0, -2, -2, -2, 0, 2,
            2,  1, -1, -2, -2, -1, 1, 2,
            1,  1,  0, -1, -1, -1, 0, 1
        };

        public static bool IsGate(uint UID)
        {
            return UID == 516076 || UID == 516077 || UID == 516074 || UID == 516075 || UID == 516078 || UID == 516079 || UID == 516080;
        }
        public static bool IsFrozengrotoMaps(uint Map)
        {
            return Map == 1762 || Map == 1927 || Map == 1999 || Map == 2054 || Map == 2055 || Map == 2056;
        }
        public void Pushback(ref uint x, ref uint y, Role.Flags.ConquerAngle angle, int paces)
        {
            sbyte xi = 0, yi = 0;
            for (int i = 0; i < paces; i++)
            {
                switch (angle)
                {
                    case Role.Flags.ConquerAngle.North: xi = -1; yi = -1; break;
                    case Role.Flags.ConquerAngle.South: xi = 1; yi = 1; break;
                    case Role.Flags.ConquerAngle.East: xi = 1; yi = -1; break;
                    case Role.Flags.ConquerAngle.West: xi = -1; yi = 1; break;
                    case Role.Flags.ConquerAngle.NorthWest: xi = -1; break;
                    case Role.Flags.ConquerAngle.SouthWest: yi = 1; break;
                    case Role.Flags.ConquerAngle.NorthEast: yi = -1; break;
                    case Role.Flags.ConquerAngle.SouthEast: xi = 1; break;
                }
                if (!ValidLocation((ushort)(x + xi), (ushort)(y + yi))) break;
                x = (ushort)(x + xi);
                y = (ushort)(y + yi);
            }
        }
        private static List<ushort> UsingMaps = new List<ushort>()
        {
            601,//offlineTG
            700,//arena map, lotery map
            1000,//Desert
            1001,//MysticCastel
            1002,//TwinCity
            1004,//Prommoter
            1005,//Arena
            1006,//Steeding TC
            1008,//color you armors/heah
            1010,//bird vilage
            1011,//PhoenixCastle
            1013,//HalkingCave
            1015,//BirdIsland
            1020,//ApeMoutain
            1036,//Market
            //1039,//TrainingGrounds
            1511,//buy mobila
            1038,//GuildWar
            2068,//elitepk map
            6001,//GuildWarJaill
            1098,1099,2080,601,3024,//house id`s
            1351,1352,1353,1354,//lab`s
            1762,//fg1
            1927,//fg2
            1999,//fg3
            2054,//fg4
            2055,//fg5
            2056,//fg6
            1858,//roulette
            3846,//Nemesys Map
            1700,//2nd reborn quest !!!
            3851,//epic ninja quest
            3055,//first map nemesys
            3056,//pestera
            3846,//nemesys map
            1039,//
            6000,//jail
            3825,//trojan epic quest
            2057,
             1993,
            10364,
            Game.MsgTournaments.MsgClassPKWar.MapID,
            Game.MsgTournaments.MsgEliteGroup.WaitingAreaID
        };

        public List<Portal> Portals = new List<Portal>();

        public unsafe void SendSysMesage(string Messaj, Game.MsgServer.MsgMessage.ChatMode ChatType = Game.MsgServer.MsgMessage.ChatMode.TopLeft
           , Game.MsgServer.MsgMessage.MsgColor color = Game.MsgServer.MsgMessage.MsgColor.red)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                var Packet = new Game.MsgServer.MsgMessage(Messaj, color, ChatType).GetArray(stream);
                foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                    if (client.Player.Map == ID)
                        client.Send(Packet);
            }
        }

        public string Name = "";

        public uint BaseID = 0;
        public MapFlagType[,] cells { get; set; }
        public System.Drawing.Size bounds;
        public Game.MsgMonster.MobCollection MonstersColletion;

        public MapView View;

        public bool AddStaticRole(StaticRole role)
        {
            if (View.EnterMap<StaticRole>(role))
            {
                SetFlagNpc(role.X, role.Y);
                return true;
            }
            return false;
        }
        public bool RemoveStaticRole(Role.IMapObj obj)
        {

            if (View.LeaveMap<Role.IMapObj>(obj))
            {
                RemoveFlagNpc(obj.X, obj.Y);
                return true;
            }
            return false;
        }

        public Game.MsgNpc.Npc Magnolia = null;
        public void AddMagnolia(ServerSockets.Packet stream, uint Quality)
        {
            bool Location = false;

            if (Magnolia != null)
            {
                if (Magnolia.X == 99)
                    Location = true;
                RemoveNpc(Magnolia, stream);
            }
            Magnolia = Game.MsgNpc.Npc.Create();
            if (Location)
            {
                Magnolia.UID = 999900;
                Magnolia.X = 106;
                Magnolia.Y = 99;
            }
            else
            {
                Magnolia.UID = 999901;
                Magnolia.X = 99;
                Magnolia.Y = 112;
            }
            Magnolia.ObjType = MapObjectType.Npc;
            Magnolia.NpcType = Flags.NpcType.Talker;
            uint mesh = 0;
            if (Quality % 10 == 7)
                mesh = 10;
            else if (Quality % 10 == 8)
                mesh = 20;
            if (Quality % 10 == 9)
                mesh = 30;
            if (Quality % 10 == 0)
                mesh = 40;
            Magnolia.Mesh = (ushort)(19340 + mesh);
            Magnolia.Map = this.ID;
            AddNpc(Magnolia);
        }



        public void GenerateSectorTraps(ushort x, ushort y, int type)
        {
            if (View.CountRoles(MapObjectType.Item, x, y) < 6)
            {
                ushort newx = (ushort)Pool.GetRandom.Next(1, 18);
                ushort newy = (ushort)Pool.GetRandom.Next(1, 18);
                newx += x;
                newy += y;
                if (IsFlagPresent(newx, newy, MapFlagType.Item) == false && IsFlagPresent(newx, newy, MapFlagType.Valid))
                {
                    var Item = new Game.MsgFloorItem.MsgItem(null, newx, newy, Game.MsgFloorItem.MsgItem.ItemType.Effect, 0, 0, ID, 0, false, this, 60 * 60 * 1000);
                    Item.MsgFloor.m_ID = (uint)type;
                    Item.MsgFloor.m_Color = 2;
                    Item.MsgFloor.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                    cells[newx, newy] |= MapFlagType.Item;
                    View.EnterMap<Role.IMapObj>(Item);


                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        Item.SendAll(stream, MsgDropID.Effect);
                    }
                }
            }
        }
        public void RemoveTrap(ushort x, ushort y, Role.IMapObj item)
        {

            cells[item.X, item.Y] &= ~MapFlagType.Item;
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                var ittem = item as Game.MsgFloorItem.MsgItem;
                ittem.SendAll(stream, ittem.IsTrap() ? Game.MsgFloorItem.MsgDropID.RemoveEffect : Game.MsgFloorItem.MsgDropID.Remove);
                ittem.MsgFloor.DropType = ittem.IsTrap() ? MsgDropID.RemoveEffect : MsgDropID.Remove;
                ittem.MsgFloor.SendScreen(stream.ItemPacketCreate(ittem.MsgFloor));
                View.LeaveMap<Role.IMapObj>(item);
            }


        }
        public void RemoveTrapGuild(ushort x, ushort y, Role.IMapObj item)
        {

            cells[item.X, item.Y] &= ~MapFlagType.Item;
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                var ittem = item as Game.MsgFloorItem.MsgItem;
                ittem.SendAll(stream, ittem.IsTrap() ? Game.MsgFloorItem.MsgDropID.RemoveEffect : Game.MsgFloorItem.MsgDropID.Remove);
                ittem.MsgFloor.DropType = ittem.IsTrap() ? MsgDropID.RemoveEffect : MsgDropID.Remove;
                ittem.MsgFloor.SendScreen(stream.ItemPacketCreate(ittem.MsgFloor));
                if (item.Map == 1002)
                    View.LeaveMap<Role.IMapObj>(item);
            }


        }
        public ConcurrentDictionary<uint, Game.MsgNpc.Npc> soldierRemains = new ConcurrentDictionary<uint, Game.MsgNpc.Npc>();
        public void CheckUpSoldierReamins()
        {
            List<Game.MsgNpc.Npc> remove = new List<Game.MsgNpc.Npc>();
            foreach (var npc in soldierRemains.Values)
            {
                if (ID == 1000)
                {
                    if (DateTime.Now > npc.Respawn)
                    {
                        npc.X = (ushort)Pool.GetRandom.Next(624 - 32, 624 + 32);
                        npc.Y = (ushort)Pool.GetRandom.Next(477 - 32, 477 + 32);
                        AddNpc(npc);
                        remove.Add(npc);
                    }
                }
                else if (ID == 1015)
                {
                    if (npc.UID == 8551)
                    {
                        npc.X = (ushort)Pool.GetRandom.Next(551 - 32, 551 + 32);
                        npc.Y = (ushort)Pool.GetRandom.Next(342 - 32, 342 + 32);
                        AddNpc(npc);
                        remove.Add(npc);
                    }
                    else
                    {
                        npc.X = (ushort)Pool.GetRandom.Next(454 - 90, 454 + 90);
                        npc.Y = (ushort)Pool.GetRandom.Next(574 - 90, 574 + 90);
                        AddNpc(npc);
                        remove.Add(npc);
                    }
                }
            }
            foreach (var npc in remove)
            {
                Game.MsgNpc.Npc rem;
                soldierRemains.TryRemove(npc.UID, out rem);
            }
        }

        public void AddNpc(Game.MsgNpc.Npc npc)
        {
            if (!View.MapContain(MapObjectType.Npc, npc.UID))
            {
                View.EnterMap<Role.IMapObj>(npc);
                SetFlagNpc(npc.X, npc.Y);
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    npc.Send(stream);
                }
            }
            else
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var streamm = rec.GetStream();
                    RemoveNpc(npc, streamm);
                }
                if (!View.MapContain(MapObjectType.Npc, npc.UID))
                {
                    View.EnterMap<Role.IMapObj>(npc);
                    SetFlagNpc(npc.X, npc.Y);
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        npc.Send(stream);
                    }
                }
            }
        }
        public unsafe void RemoveNpc(SobNpc npc, ServerSockets.Packet stream)
        {
            if (View.MapContain(MapObjectType.SobNpc, npc.UID))
            {
                View.LeaveMap<Role.IMapObj>(npc);
                RemoveFlagNpc(npc.X, npc.Y);
                ActionQuery action;
                action = new ActionQuery()
                {
                    ObjId = npc.UID,
                    Type = ActionType.RemoveEntity
                };
                foreach (var client in View.Roles(MapObjectType.Player, npc.X, npc.Y))
                {
                    if (Core.GetDistance(client.X, client.Y, npc.X, npc.Y) <= Game.MsgNpc.Npc.SeedDistance)
                    {
                        client.Send(stream.ActionCreate(action));
                    }
                }
            }

        }
        public unsafe void RemoveNpc(Game.MsgNpc.Npc npc, ServerSockets.Packet stream)
        {
            if (View.MapContain(MapObjectType.Npc, npc.UID))
            {
                View.LeaveMap<Role.IMapObj>(npc);
                RemoveFlagNpc(npc.X, npc.Y);


                ActionQuery action;

                action = new ActionQuery()
                {
                    ObjId = npc.UID,
                    Type = ActionType.RemoveEntity
                };

                foreach (var client in View.Roles(MapObjectType.Player, npc.X, npc.Y))
                {
                    if (Core.GetDistance(client.X, client.Y, npc.X, npc.Y) <= Game.MsgNpc.Npc.SeedDistance)
                    {
                        client.Send(stream.ActionCreate(action));
                    }
                }
            }

        }
        public bool ValidLocation(ushort X, ushort Y)
        {
            if (ID == 1002 && Role.Core.GetDistance(509, 318, X, Y) < 50) return true;
            if (ID == 1002 && Role.Core.GetDistance(445, 445, X, Y) < 50) return true;
            if (ID == 1002 && Role.Core.GetDistance(269, 395, X, Y) < 50) return true;
            if (ID == 1002 && Role.Core.GetDistance(429, 384, X, Y) < 50) return true;
            if (ID == 1002 && Role.Core.GetDistance(432, 310, X, Y) < 50) return true;
            if (ID == 700 && Role.Core.GetDistance(50, 50, X, Y) < 20) return true;
            if (bounds.Width > X && this.bounds.Height > Y)
            {
                return (cells[X, Y] & MapFlagType.Valid) == MapFlagType.Valid;
            }
            return false;
        }
        public bool MonsterOnTile(ushort X, ushort Y)
        {
            if (bounds.Width > X && this.bounds.Height > Y)
            {
                return (cells[X, Y] & MapFlagType.Monster) == MapFlagType.Monster;
            }
            return false;
        }
        public void SetMonsterOnTile(ushort X, ushort Y, bool Value)
        {
            try
            {
                if (Value)
                    cells[X, Y] |= MapFlagType.Monster;
                else
                    cells[X, Y] &= ~MapFlagType.Monster;
            }
            catch (Exception e)
            {
                MyConsole.WriteException(e);
                MyConsole.WriteLine("Problem monsters on map " + ID.ToString());
            }
        }
        public bool SearchNpcInScreen(uint UID, ushort X, ushort Y, out Game.MsgNpc.Npc obj)
        {
            if (View.TryGetObject<Game.MsgNpc.Npc>(UID, MapObjectType.Npc, X, Y, out obj))
            {
                return Core.GetDistance(X, Y, obj.X, obj.Y) < Game.MsgNpc.Npc.SeedDistance;
            }
            obj = default(Game.MsgNpc.Npc);
            return false;
        }


        public uint ID { get; private set; }
        public GameMap(int width, int height, int m_id)
        {
            Clients = new ConcurrentDictionary<uint, Client.GameClient>();
            this.cells = new MapFlagType[width, height];
            this.bounds = new System.Drawing.Size(width, height);

            this.ID = (uint)m_id;
        }

        public static System.Counter DinamicIDS = new System.Counter(10000001);

        public uint GenerateDynamicID()
        {
            return DinamicIDS.Next;
        }
        public ushort Reborn_Map = 0;
        public ushort Reborn_X = 0;
        public ushort Reborn_Y = 0;

        public static Dictionary<int, string> MapContents = new Dictionary<int, string>();
        public static bool CheckMap(uint ID)
        {
            if (!Pool.ServerMaps.ContainsKey(ID))
            {
                try
                {
                    if (MapContents.ContainsKey((int)ID))
                        return LoadMap((int)ID, MapContents[(int)ID]);
                    else return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    MyConsole.WriteLine("MapID = " + ID + " not exist.");
                    return false;
                }
            }
            else return true;
        }
        public ulong TypeStatus { get; set; }
        public static void EnterMap(int id)
        {
            try
            {
                if (Pool.ServerMaps.Base.ContainsKey((uint)id) || id == 0) return;
                uint baseID = (uint)id;
                if (baseID >= 3830 && baseID <= 3834) baseID = 1780;
                else if (baseID >= 3826 && baseID <= 3829) baseID = 3825;
                else if (baseID == 7357) baseID = 6000;//10364

                else if (baseID == 6004) baseID = 6000;//botjail

                else if (baseID == 15757) baseID = 1993;//15757
                else if (baseID == 15758) baseID = 1993;//15758
                else if (baseID == 15759) baseID = 1993;//15759
                else if (baseID == 15760) baseID = 1993;//15759
                else if (baseID == 15761) baseID = 1993;//15759
                else if (baseID == 15762) baseID = 1993;//15759
                else if (baseID == 15763) baseID = 1993;//15759
                else if (baseID == 15764) baseID = 1993;//15759
                else if (baseID == 15765) baseID = 1993;//15759
                else if (baseID == 15766) baseID = 1993;//15759
                else if (baseID == 15767) baseID = 1993;//15759
                                                        //     else if (baseID == 11) baseID = 4003;
                else if (baseID == 16252) baseID = 2021;//15759
                else if (baseID == 10446) baseID = 10445;
                else if (baseID == 10134) baseID = 10133;
                else if (baseID == 10447) baseID = 10445;
                else if (baseID == 10448) baseID = 10445;
                else if (baseID == 10449) baseID = 10445;
                else if (baseID == 10450) baseID = 10445;
                else if (baseID == 10451) baseID = 10445;
                else if (baseID == 10452) baseID = 10445;
                else if (baseID == 10453) baseID = 10445;
                else if (baseID == 10454) baseID = 10445;
                else if (baseID == 10455) baseID = 10445;
                else if (baseID == 10) baseID = 3825;
                else if (baseID == 6241) baseID = 10723;//10364
                else if (baseID == 5342) baseID = 700;
                else if (baseID == 5339) baseID = 3851;
                else if (baseID == 2351) baseID = 1351;
                else if (baseID == 2352) baseID = 1351;
                else if (baseID == 2353) baseID = 1351;
                else if (baseID == 6011) baseID = 1021;
                else if (baseID == 9988) baseID = 8709;
                else if (baseID == 8437) baseID = 1801;
                else if (baseID == 3833) baseID = 3825;
                else if (baseID == 1518) baseID = 1508;
                else if (baseID == 1818) baseID = 1765;
                else if (baseID == 1052) baseID = 1082;
                else if (baseID == 6072) baseID = 1004;
                else if (baseID == 1782) baseID = 1004;
                else if (baseID == 1783) baseID = 1004;
                else if (baseID == 1784) baseID = 601;
                else if (baseID == 1794) baseID = 1028;
                else if (baseID == 5599) baseID = 1016;
                else if (baseID == 1792) baseID = 1014;
                else if (baseID == 1791) baseID = 1765;
                else if (baseID == 6784) baseID = 1505;
                else if (baseID >= 44455 && baseID <= 44457) baseID = 10088;
                else if (baseID >= 44460 && baseID <= 44463) baseID = 10090;
                else if (baseID == 6546) baseID = 11725;
                else if (baseID == 6546) baseID = 3873;
                else if (baseID == 6521) baseID = 2071;
                else if (baseID == 8009) baseID = 2071;
                else if (baseID == 6891) baseID = 2071;
                //  else if (baseID == 6525) baseID = 1507;
                else if (baseID == 8521) baseID = 1507;
                else if (baseID == 6729) baseID = 3825;
                else if (baseID == 5050) baseID = 6000;
                else if (baseID == 5051) baseID = 4000;
                else if (baseID == 5052) baseID = 4000;
                else if (baseID == 5053) baseID = 4000;
                else if (baseID == 5054) baseID = 4000;
                else if (baseID == 5055) baseID = 4000;
                else if (baseID == 5056) baseID = 4000;
                else if (baseID == 5057) baseID = 4000;
                else if (baseID == 5058) baseID = 4000;
                else if (baseID == 5061) baseID = 4000;
                else if (baseID == 5062) baseID = 4000;
                else if (baseID == 5063) baseID = 4000;
                else if (baseID == 5064) baseID = 4000;
                else if (baseID == 5065) baseID = 4000;
                else if (baseID == 5066) baseID = 4000;
                else if (baseID == 22330) baseID = 4000;
                else if (baseID == 10001) baseID = 6000;
                else if (baseID == 22331) baseID = 4000;
                else if (baseID == 22332) baseID = 4000;
                else if (baseID == 22334) baseID = 4000;
                else if (baseID == 22335) baseID = 4000;
                else if (baseID == 46812) baseID = 4000;
                else if (baseID == 10030) baseID = 3991;
                else if (baseID == 5000) baseID = 6000;
                else if (baseID == 20081) baseID = 1016;//TeamKiller
                else if (baseID == 20132) baseID = 1068;
                else if (baseID == 20082) baseID = 10602;//BestFighter
                else if (baseID == 20083) baseID = 10602;//OnePunch
                                                         // else if (baseID == 20084) baseID = 10770;//Lastblood
                else if (baseID == 1138) baseID = 1038;
                else if (baseID == 12020) baseID = 1779;
                else if (baseID == 12023) baseID = 1020;
                else if (baseID == 12024) baseID = 1000;
                else if (baseID == 12025) baseID = 1015;
                else if (baseID == 20084) baseID = 10602;
                else if (baseID == 20086) baseID = 10602;
                else if (baseID == 7657) baseID = 1039;

                else if (baseID == 22348) baseID = 10702;


                else if (baseID == 12344) baseID = 10723;

                else if (baseID == 45421) baseID = 10702;


                else if (baseID == 10070) baseID = 10444;

                else if (baseID == 6525) baseID = 10639;

                else if (baseID == 11) baseID = 10760;
                else if (baseID == 20) baseID = 10760;
                else if (baseID == 7) baseID = 10760;
                else if (baseID == 8) baseID = 10760;
                else if (baseID == 12) baseID = 10760;
                else if (baseID == 14) baseID = 10503;
                else if (baseID == 15) baseID = 10503;
                else if (baseID == 5) baseID = 10760;

                if (MapContents.ContainsKey((int)baseID) && LoadMap(id, MapContents[(int)baseID], id != baseID ? baseID : 0))
                {
                    Server.LoadMapName((uint)id);
                    Database.NpcServer.LoadNpcs((uint)id);
                    Database.NpcServer.LoadSobNpcs((uint)id);
                    Server.LoadPortals((uint)id);
                    Database.NpcServer.LoadServerTraps((uint)id);

                    if (id == 16785 || id == 16786 || id == 3860 || id == 15757 || id == 15758 || id == 15759 || id == 15760 || id == 15761 || id == 15762 || id == 15763 || id == 15764 || id == 15765 || id == 15766 || id == 15767 || id == 15768)
                        Server.LoadNewMonsters((uint)id);

                }
                GC.Collect();
            }
            catch (Exception e) { MyConsole.WriteException(e); }
        }
        public static void LoadMaps()
        {
            using (var gamemap = new BinaryReader(new FileStream(Path.Combine(Program.ServerConfig.CO2Folder, "ini/GameMap.dat"), FileMode.Open)))
            {
                var amount = gamemap.ReadInt32();
                for (var i = 0; i < amount; i++)
                {

                    var id = gamemap.ReadInt32();
                    var fileName = Encoding.ASCII.GetString(gamemap.ReadBytes(gamemap.ReadInt32()));
                    var puzzleSize = gamemap.ReadInt32();
                    MapContents[id] = fileName.Replace(".7z", ".dmap");
                }
                //foreach (var folded in MapContents)
                //{
                //    int id = folded.Key; var mapFile = folded.Value; LoadMap(id, mapFile);
                //    if (folded.Key == 1005)
                //        LoadMap(20084, mapFile, 1005);
                //}
            }
        }
        public uint MapColor = 0;
        public int[,] FloorType;
        public int[,] Altitude;
        public static bool LoadMap(int id, string mapFile, uint baseid = 0)
        {
            try
            {
                lock (EnterObj)
                {
                    GameMap ourInst;
                    using (var rdr = new BinaryReader(new FileStream(Path.Combine(Program.ServerConfig.CO2Folder, mapFile), FileMode.Open)))
                    {

                        rdr.ReadBytes(268);
                        ourInst = new GameMap(rdr.ReadInt32(), rdr.ReadInt32(), id);
                        ourInst.MonstersColletion = new Game.MsgMonster.MobCollection((uint)id);
                        ourInst.View = new MapView(ourInst.bounds.Width, ourInst.bounds.Height);
                        ourInst.MonstersColletion = new Game.MsgMonster.MobCollection((uint)id);
                        ourInst.BaseID = baseid;

                        ourInst.FloorType = new int[ourInst.bounds.Width, ourInst.bounds.Height];
                        ourInst.Altitude = new int[ourInst.bounds.Width, ourInst.bounds.Height];

                        for (int y = 0; y < ourInst.bounds.Height; y++)
                        {
                            for (int x = 0; x < ourInst.bounds.Width; x++)
                            {

                                ourInst.cells[x, y] = (rdr.ReadInt16() == 0) ? MapFlagType.Valid : MapFlagType.None;
                                ourInst.FloorType[x, y] = rdr.ReadInt16();
                                ourInst.Altitude[x, y] = rdr.ReadInt16();

                            }
                            rdr.ReadInt32();
                        }
                    }
                    int info = baseid != 0 ? (int)baseid : (int)id;

                    if (File.Exists(Program.ServerConfig.DbLocation + "maps\\" + info + ".ini"))
                    {
                        WindowsAPI.IniFile reader = new WindowsAPI.IniFile("\\maps\\" + info + ".ini");
                        ourInst.TypeStatus = reader.ReadUInt64("info", "type", 0);
                        ourInst.Reborn_X = reader.ReadUInt16("info", "portal0_x", 0);
                        ourInst.Reborn_Y = reader.ReadUInt16("info", "portal0_y", 0);
                        ourInst.Reborn_Map = reader.ReadUInt16("info", "reborn_map", 0);
                        ourInst.RecordSteedRace = reader.ReadUInt16("info", "race_record", 0);
                        ourInst.MapColor = reader.ReadUInt32("info", "color", 0);
                    }
                    Pool.ServerMaps.Add((uint)id, ourInst);
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                MyConsole.WriteLine("\tMap not found: " + id + " - " + mapFile + "");
            }
            catch (Exception e)
            {
                MyConsole.WriteException(e);
            }
            return false;
        }
        private bool Update = false;
        private Client.GameClient[] Users = new Client.GameClient[0];
        public Client.GameClient[] Values
        {
            get
            {
                if (Update)
                {
                    Users = Clients.Values.ToArray();
                    Update = false;
                }
                return Users;
            }
            set { }
        }
        private ConcurrentDictionary<uint, Client.GameClient> Clients;
        public void Enquer(Client.GameClient client)
        {
            if (Clients.TryAdd(client.Player.UID, client))
            {
                View.EnterMap<Role.IMapObj>(client.Player);
                client.Map = this;
                Update = true;
            }
        }
        public void Denquer(Client.GameClient client)
        {
            Client.GameClient aclient;
            if (Clients.TryRemove(client.Player.UID, out aclient))
            {
                View.LeaveMap<Role.IMapObj>(client.Player);

                Update = true;
            }
        }
        public void SetFlagNpc(ushort x, ushort y)
        {
            if (!(x > 0 && y > 0 && x < bounds.Width && y < bounds.Height)) return;
            cells[x, y] = MapFlagType.Npc;

            ushort limy = (ushort)Math.Min(this.bounds.Height - 2, y + 2);
            ushort limx = (ushort)Math.Min(this.bounds.Width - 2, x + 2);
            ushort xstart = (ushort)Math.Max(x - 2, 0);

            for (ushort ay = (ushort)Math.Max(y - 2, 0); ay <= limy; ay++)
            {
                for (ushort ax = xstart; ax <= limx; ax++)
                {
                    cells[ax, ay] = MapFlagType.Npc;
                }
            }
        }
        public void SetGateFlagNpc(ushort x, ushort y)
        {
            if (!(x > 0 && y > 0 && x < bounds.Width && y < bounds.Height)) return;
            cells[x, y] = MapFlagType.None;

            ushort limy = (ushort)Math.Min(this.bounds.Height - 2, y + 2);
            ushort limx = (ushort)Math.Min(this.bounds.Width - 2, x + 2);
            ushort xstart = (ushort)Math.Max(x - 2, 0);

            for (ushort ay = (ushort)Math.Max(y - 2, 0); ay <= limy; ay++)
            {
                for (ushort ax = xstart; ax <= limx; ax++)
                {
                    cells[ax, ay] = MapFlagType.None;
                }
            }
        }
        public void RemoveFlagNpc(ushort x, ushort y)
        {
            if (!(x > 0 && y > 0 && x < bounds.Width && y < bounds.Height)) return;
            cells[x, y] = MapFlagType.Valid;

            ushort limy = (ushort)Math.Min(this.bounds.Height - 1, y + 1);
            ushort limx = (ushort)Math.Min(this.bounds.Width - 1, x + 1);
            ushort xstart = (ushort)Math.Max(x - 1, 0);

            for (ushort ay = (ushort)Math.Max(y - 1, 0); ay <= limy; ay++)
            {
                for (ushort ax = xstart; ax <= limx; ax++)
                {
                    cells[ax, ay] = MapFlagType.Valid;
                }
            }
        }
        public bool ContainMobID(uint ID, uint Dynamic = 0, int count = 1)
        {
            foreach (var monster in View.GetAllMapRoles(MapObjectType.Monster))
            {
                var mob = monster as Game.MsgMonster.MonsterRole;
                if (mob.Family != null)
                    if (mob.Family.ID == ID && mob.Alive)
                    {
                        if (Dynamic == 0)
                            count--;
                        else if (Dynamic == monster.DynamicID)
                            count--;
                    }
            }
            return count <= 0;
        }

        public object SyncRoot = new object();
        public void GetRandCoord(ref ushort x, ref ushort y)
        {
            lock (SyncRoot)
            {
                do
                {
                    x = (ushort)Program.GetRandom.Next(20, (ushort)(bounds.Width - 1));
                    y = (ushort)Program.GetRandom.Next(20, (ushort)(bounds.Height - 1));
                }
                while ((cells[x, y] & MapFlagType.Valid) != MapFlagType.Valid);
            }
        }
        public void GetRandCoord(ref ushort _x, ref ushort _y, int radius)
        {
            int times = 10000;
            lock (SyncRoot)
            {
                _x = (ushort)Pool.GetRandom.Next(_x - radius, _x + radius);
                _y = (ushort)Pool.GetRandom.Next(_y - radius, _y + radius);
                while (times-- > 0)
                {
                    if ((cells[_x, _y] & MapFlagType.Valid) != MapFlagType.Valid)
                    {
                        _x = (ushort)Pool.GetRandom.Next(_x + (radius * -1), _x + radius);
                        _y = (ushort)Pool.GetRandom.Next(_y + (radius * -1), _y + radius);
                    }
                    else break;
                }
            }
        }

        public bool IsFlagPresent(int x, int y, MapFlagType flag)
        {
            if (x > 0 && y > 0 && x < bounds.Width && y < bounds.Height)
                return (cells[x, y] & flag) == flag;
            return false;
        }
        public bool EnqueueItem(Game.MsgFloorItem.MsgItem item)
        {
            return View.EnterMap<Role.IMapObj>(item);
        }
        public bool IsValidFlagNpc(ushort x, ushort y)
        {
            ushort limy = (ushort)Math.Min(this.bounds.Height - 1, y + 1);
            ushort limx = (ushort)Math.Min(this.bounds.Width - 1, x + 1);
            ushort xstart = (ushort)Math.Max(x - 1, 0);

            for (ushort ay = (ushort)Math.Max(y - 1, 0); ay <= limy; ay++)
            {
                for (ushort ax = xstart; ax <= limx; ax++)
                {
                    if (!this.IsFlagPresent(x, y, MapFlagType.Valid))
                        return false;
                }
            }
            return true;
        }
        public bool AddGuildTeleporterItem(ref ushort x, ref ushort y)
        {
            if (IsValidFlagNpc(x, y))
            {
                ushort limy = (ushort)Math.Min(this.bounds.Height - 6, y + 6);
                ushort limx = (ushort)Math.Min(this.bounds.Width - 6, x + 6);
                ushort xstart = (ushort)Math.Max(x - 6, 0);
                ushort ystart = (ushort)Math.Max(y - 6, 0);

                for (ushort ay = ystart; ay <= limy; ay++)
                {
                    for (ushort ax = xstart; ax <= limx; ax++)
                    {
                        if (IsValidFlagNpc(ax, ay))
                        {
                            x = ax;
                            y = ay;

                            cells[ax, ay] |= MapFlagType.Item;

                            return true;
                        }
                    }
                }
                x = 0;
                y = 0;
                return false;
            }

            cells[x, y] |= MapFlagType.Item;
            return true;
        }
        public bool AddGroundItem(ref ushort x, ref ushort y, byte Range = 0)
        {
            if (this.IsFlagPresent(x, y, MapFlagType.Item) || !this.IsFlagPresent(x, y, MapFlagType.Valid))
            {
                ushort limy = (ushort)Math.Min(this.bounds.Height - (1 + Range), y + (1 + Range));
                ushort limx = (ushort)Math.Min(this.bounds.Width - (1 + Range), x + (1 + Range));
                ushort xstart = (ushort)Math.Max(x - (1 + Range), 0);
                ushort ystart = (ushort)Math.Max(y - (1 + Range), 0);

                for (ushort ay = ystart; ay <= limy; ay++)
                {
                    for (ushort ax = xstart; ax <= limx; ax++)
                    {
                        if (!this.IsFlagPresent(ax, ay, MapFlagType.Item))
                        {
                            if (this.IsFlagPresent(ax, ay, MapFlagType.Valid))
                            {
                                x = ax;
                                y = ay;

                                cells[ax, ay] |= MapFlagType.Item;

                                return true;
                            }
                        }
                    }
                }
                x = 0;
                y = 0;
                return false;
            }

            cells[x, y] |= MapFlagType.Item;
            return true;
        }
        public unsafe void AddMapMonster(ServerSockets.Packet stream, uint ID, ushort x, ushort y, ushort max_x, ushort max_y, byte count, uint DinamicID = 0, bool RemoveOnDead = false
            , Game.MsgFloorItem.MsgItemPacket.EffectMonsters m_effect = Game.MsgFloorItem.MsgItemPacket.EffectMonsters.None, string streffect = "")
        {
            if (MonstersColletion == null)
            {
                MonstersColletion = new Game.MsgMonster.MobCollection(ID);
            }
            if (MonstersColletion.ReadMap())
            {

                Game.MsgMonster.MonsterFamily famil;
                if (Pool.MonsterFamilies.TryGetValue(ID, out famil))
                {
                    Game.MsgMonster.MonsterFamily Monster = famil.Copy();

                    Monster.SpawnX = x;
                    Monster.SpawnY = y;
                    Monster.MaxSpawnX = (ushort)(x + max_x);
                    Monster.MaxSpawnY = (ushort)(y + max_y);
                    Monster.MapID = ID;
                    Monster.SpawnCount = count;
                    Game.MsgMonster.MonsterRole rolemonster = MonstersColletion.Add(Monster, RemoveOnDead, DinamicID, true);

                    if (rolemonster == null)
                    {
                        MyConsole.WriteLine("[Error] Add Map Monster (" + ID.ToString() + ")");
                        return;
                    }
                    //   View.EnterMap<Role.IMapObj>(rolemonster);

                    Game.MsgServer.ActionQuery action = new Game.MsgServer.ActionQuery()
                    {
                        ObjId = rolemonster.UID,
                        Type = Game.MsgServer.ActionType.ReviveMonster,
                        PositionX = rolemonster.X,
                        PositionY = rolemonster.Y
                    };
                    rolemonster.Send(stream.ActionCreate(action));
                    rolemonster.Send(rolemonster.GetArray(stream, false));

                    if (streffect != null)
                    {
                        rolemonster.SendString(stream, MsgStringPacket.StringID.Effect, streffect);
                    }



                    if (m_effect != Game.MsgFloorItem.MsgItemPacket.EffectMonsters.None && rolemonster != null)
                    {
                        Game.MsgFloorItem.MsgItemPacket effect = Game.MsgFloorItem.MsgItemPacket.Create();
                        effect.m_UID = (uint)m_effect;
                        effect.m_X = rolemonster.X;
                        effect.m_Y = rolemonster.Y;
                        effect.DropType = MsgDropID.Earth;
                        rolemonster.Send(stream.ItemPacketCreate(effect));
                        rolemonster.SendString(stream, Game.MsgServer.MsgStringPacket.StringID.Effect, "glebesword");
                    }
                    if (rolemonster.HitPoints > 65535)
                    {
                        Game.MsgServer.MsgUpdate Upd = new Game.MsgServer.MsgUpdate(stream, rolemonster.UID, 1);
                        stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.MaxHitpoints, rolemonster.Family.MaxHealth);
                        stream = Upd.GetArray(stream);
                        rolemonster.Send(stream);
                        stream = Upd.Append(stream, Game.MsgServer.MsgUpdate.DataType.Hitpoints, rolemonster.HitPoints);
                        stream = Upd.GetArray(stream);
                        rolemonster.Send(stream);
                    }
                }
            }
        }
        public unsafe bool AddFloor(ServerSockets.Packet stream, uint ID, ushort x, ushort y, ushort spelllevel, Database.MagicType.Magic dbspell, Client.GameClient Owner, uint GuildID, uint OwnerUID, uint DinamicID = 0, string Name = "", bool RemoveOnDead = true)
        {
            try
            {
                if (MonstersColletion == null)
                {
                    MonstersColletion = new Game.MsgMonster.MobCollection(ID);
                }
                if (MonstersColletion.ReadMap())
                {

                    Game.MsgMonster.MonsterFamily famil;
                    if (Pool.MonsterFamilies.TryGetValue(1, out famil))
                    {
                        Game.MsgMonster.MonsterFamily Monster = famil.Copy();

                        Monster.SpawnX = x;
                        Monster.SpawnY = y;
                        Monster.MaxSpawnX = (ushort)(x + 1);
                        Monster.MaxSpawnY = (ushort)(y + 1);
                        Monster.MapID = ID;
                        Monster.SpawnCount = 1;
                        Game.MsgMonster.MonsterRole rolemonster = MonstersColletion.Add(Monster, RemoveOnDead, DinamicID, true);
                        if (rolemonster == null)
                        {
                            //invalid x ,y
                            return false;
                        }
                        rolemonster.Family.ID = ID;
                        rolemonster.IsFloor = true;
                        rolemonster.FloorStampTimer = DateTime.Now.AddSeconds(7);
                        rolemonster.Family.Settings = Game.MsgMonster.MonsterSettings.Lottus;

                        rolemonster.FloorPacket = new MsgItemPacket();
                        rolemonster.FloorPacket.m_UID = rolemonster.UID;
                        rolemonster.FloorPacket.m_ID = ID;
                        rolemonster.FloorPacket.m_X = x;
                        rolemonster.FloorPacket.m_Y = y;
                        rolemonster.FloorPacket.MaxLife = 25;
                        rolemonster.FloorPacket.Life = 25;
                        rolemonster.FloorPacket.DropType = MsgDropID.Effect;
                        rolemonster.FloorPacket.m_Color = 13;
                        rolemonster.FloorPacket.ItemOwnerUID = OwnerUID;
                        rolemonster.FloorPacket.GuildID = GuildID;
                        rolemonster.FloorPacket.FlowerType = 2;//2;
                        rolemonster.FloorPacket.Timer = Role.Core.TqTimer(rolemonster.FloorStampTimer);
                        rolemonster.FloorPacket.Name = Name;

                        rolemonster.DBSpell = dbspell;
                        rolemonster.Family.MaxHealth = 25;
                        rolemonster.HitPoints = 25;
                        rolemonster.OwnerFloor = Owner;
                        rolemonster.SpellLevel = spelllevel;


                        if (rolemonster == null)
                        {
                            Console.WriteLine("Eror monster spawn. Server.");
                            return false;
                        }
                        View.EnterMap<Role.IMapObj>(rolemonster);
                        rolemonster.Send(rolemonster.GetArray(stream, false));
                        return true;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return false;

        }


        public void RemoveAI(GameClient entity)
        {
            if (BotAttack.Pool.ContainsKey(entity.Player.UID))
            {
                GameClient remov;
                BotAttack.Pool.TryRemove(entity.Player.UID, out remov);
                View.LeaveMap<IMapObj>(entity.Player);
                Update = true;
            }
        }
        public void AddAI(GameClient entity)
        {
            if (!BotAttack.Pool.ContainsKey(entity.Player.UID))
            {
                BotAttack.Pool.TryAdd(entity.Player.UID, entity);
                Role.GameMap map;
                if (Pool.ServerMaps.TryGetValue(entity.Player.Map, out map))
                {
                    entity.Map = map;
                    View.EnterMap<IMapObj>(entity.Player);
                }

                Update = true;
            }
            else
            {
                View.LeaveMap<IMapObj>(entity.Player);
                BotAttack.Pool.TryAdd(entity.Player.UID, entity);
                View.EnterMap<IMapObj>(entity.Player);
            }
        }

        public bool SelectJump(ref ushort X, ref ushort Y, byte z)
        {

            ushort x = X;
            ushort y = Y;
            ushort XX = (ushort)Pool.GetRandom.Next((int)(x) - z, (int)(x) + z);
            ushort YY = (ushort)Pool.GetRandom.Next((int)(y) - z, (int)(y) + z);
            for (int i = 0; i < 100; i++)
            {
                if (ValidLocation((ushort)(XX), (ushort)(YY)))
                {
                    X = XX;
                    Y = YY;
                    break;
                }
                XX = (ushort)Pool.GetRandom.Next((int)(x) - z, (int)(x) + z);
                YY = (ushort)Pool.GetRandom.Next((int)(y) - z, (int)(y) + z);
            }
            return true;
        }
    }
}
