using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgMonster
{
    public class MobCollection
    {
        public const byte Multiple = 3;

        public string LocationSpawn = "";

        public object SyncRoot = new object();
        public static System.Counter GenerateUid = new System.Counter(400000);


        public Role.GameMap DMap = null;

        private uint DmapID = 0;
        public MobCollection(uint Map)
        {
            DmapID = Map;
            if (Pool.ServerMaps != null)
            {
                if (Pool.ServerMaps.TryGetValue(Map, out DMap))
                    DMap.MonstersColletion = this;
            }
        }
        public bool ReadMap()
        {
            if (Pool.ServerMaps != null)
            {
                if (Pool.ServerMaps.TryGetValue(DmapID, out DMap))
                    DMap.MonstersColletion = this;
            }

            return DMap != null;
        }
        public MonsterRole Add(MonsterFamily Famili, bool RemoveOnDead = false, uint dinamicid = 0, bool justone = false)
        {
            if (DMap == null)
                ReadMap();

            MonsterRole monsterr = null;
            int count = (int)((Famili.Boss > 0) ? 1 : (int)(Math.Max(1, (int)Famili.SpawnCount) * Multiple));
            if (justone)
                count = Math.Max(1, (int)Famili.SpawnCount);


            if (DmapID == 1011 || DmapID == 1770 || DmapID == 1771 || DmapID == 1772 || DmapID == 1773 || DmapID == 1774
                || DmapID == 1775 || DmapID == 1777 || DmapID == 1782
                || DmapID == 1785 || DmapID == 1786 || DmapID == 1787 || DmapID == 1794 || DmapID == 11447)
            {
                if (count > 1 && Famili.SpawnCount > 1)
                    count = (int)(((Famili.MaxSpawnX - Famili.SpawnX) / Famili.SpawnCount) * ((Famili.MaxSpawnY - Famili.SpawnY) / Famili.SpawnCount));
            }

            for (int x = 0; x < count; x++)
            {

                MonsterRole Mob = new MonsterRole(Famili.Copy(), GenerateUid.Next, LocationSpawn, DMap);
                if (Mob.Family.ID == 900 || Mob.Family.ID == 910 || Mob.Family.ID == 7985 || Mob.Family.ID == 7483 || Mob.Family.ID == 8307 || Mob.Family.ID == 3983 || Mob.Family.MapID == 1762 || Mob.Family.MapID == 1927 || Mob.Family.MapID == 1015 || Mob.Family.MapID == 10622)
                {
                    Mob.RemoveOnDead = RemoveOnDead;
                    ushort _x = 0, _y = 0;
                    TryObtainSpawnXY(Famili, out _x, out _y);
                    if (Mob.Family.ID != 900 && Mob.Family.ID != 910 && Mob.Family.ID != 3983 && Mob.Family.ID != 8307)
                    {
                        if (!DMap.ValidLocation(_x, _y) || DMap.MonsterOnTile(_x, _y))
                            continue;
                    }
                    DMap.SetMonsterOnTile(_x, _y, true);

                    Mob.X = _x;
                    Mob.Y = _y;
                    Mob.RespawnX = _x;
                    Mob.RespawnY = _y;

                    Mob.Map = DMap.ID;
                    Mob.DynamicID = dinamicid;

                    monsterr = Mob;

                    DMap.View.EnterMap<MonsterRole>(Mob);
                }
                else
                {
                    Mob.RemoveOnDead = RemoveOnDead;
                    ushort _x = 0, _y = 0;
                    TryObtainSpawnXY(Famili, out _x, out _y);
                    if (Mob.Family.ID == 900 || Mob.Family.ID == 910 || Mob.Family.ID == 7985 || Mob.Family.ID == 7483 || Mob.Family.ID == 4355 || Mob.Family.ID == 2438 || Mob.Family.ID == 2437 || Mob.Family.ID == 2436 || Mob.Family.ID == 2435 || Mob.Family.ID == 2422 || Mob.Family.ID == 2421 || Mob.Family.ID == 2420)
                    {

                    }
                    else
                    {
                        if (!DMap.ValidLocation(_x, _y) || DMap.MonsterOnTile(_x, _y))
                            continue;
                    }
                    DMap.SetMonsterOnTile(_x, _y, true);
                    ushort xx = (ushort)Program.GetRandom.Next(_x - 7, _x + 7);
                    ushort yy = (ushort)Program.GetRandom.Next(_y - 7, _y + 7);
                    if (Mob.Family.ID == 7985 || Mob.Family.ID == 4355 || Mob.Family.ID == 2438 || Mob.Family.ID == 2437 || Mob.Family.ID == 2436 || Mob.Family.ID == 2435 || Mob.Family.ID == 2422 || Mob.Family.ID == 2421 || Mob.Family.ID == 2420)
                    {

                    }
                    else
                    {
                        if (!DMap.ValidLocation(_x, _y) || !DMap.ValidLocation(xx, yy) || DMap.MonsterOnTile(xx, yy))
                            continue;

                    }

                    if (Mob.Family.ID == 900 || Mob.Family.ID == 910 || Mob.Map == 1927 || Mob.Map == 1762 || Mob.Boss == 1 || Mob.Family.ID == 900 || Mob.Family.ID == 910 || Mob.Family.ID == 4355 || Mob.Family.ID == 2438 || Mob.Family.ID == 2437 || Mob.Family.ID == 2436 || Mob.Family.ID == 2435 || Mob.Family.ID == 2422 || Mob.Family.ID == 2421 || Mob.Family.ID == 2420)
                    {
                        xx = _x;
                        yy = _y;
                    }
                    Mob.X = xx;
                    Mob.Y = yy;
                    Mob.RespawnX = xx;
                    Mob.RespawnY = yy;

                    Mob.Map = DMap.ID;
                    Mob.DynamicID = dinamicid;

                    monsterr = Mob;

                    DMap.View.EnterMap<MonsterRole>(Mob);
                }

            }
            return monsterr;
        }

        /// <summary>
        /// Attemps to obtain a point where the monster can be re-spawned.
        /// </summary>
        /// <param name="X">The x-coordinate point.</param>
        /// <param name="Y">The y-coordinate point.</param>
        public void TryObtainSpawnXY(MonsterFamily Monster, out ushort X, out ushort Y)
        {

            X = (ushort)Pool.GetRandom.Next(Monster.SpawnX, Monster.MaxSpawnX);
            Y = (ushort)Pool.GetRandom.Next(Monster.SpawnY, Monster.MaxSpawnY);

            for (byte i = 0; i < 10; i++)
            {
                if (DMap == null)
                    break;
                if (DMap.ValidLocation(X, Y) && !DMap.MonsterOnTile(X, Y))
                    break;

                X = (ushort)Pool.GetRandom.Next(Monster.SpawnX, Monster.MaxSpawnX);
                Y = (ushort)Pool.GetRandom.Next(Monster.SpawnY, Monster.MaxSpawnY);
            }
        }
    }
}
