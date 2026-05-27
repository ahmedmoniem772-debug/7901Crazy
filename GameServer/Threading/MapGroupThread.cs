using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX
{
    public class MapGroupThread
    {
        public Extensions.ThreadGroup.ThreadItem Thread;
        public MapGroupThread(int interval, string name)
        {
            Thread = new Extensions.ThreadGroup.ThreadItem(interval, name, OnProcess);
        }
        public void Start()
        {
            Thread.Open();
        }
        Role.GameMap _desert;
        public Role.GameMap Desert
        {
            get
            {
                if (_desert == null)
                    _desert = Pool.ServerMaps[1000];
                return _desert;
            }
        }
        Role.GameMap _bird;
        public Role.GameMap Bird
        {
            get
            {
                if (_bird == null)
                    _bird = Pool.ServerMaps[1000];
                return _bird;
            }
        }

        Role.GameMap _towerofmystery;
        public Role.GameMap TowerofMystery
        {
            get
            {
                if (_towerofmystery == null)
                    _towerofmystery = Pool.ServerMaps[3998];
                return _towerofmystery;
            }
        }
        public void OnProcess()
        {
            Extensions.Time32 clock = Extensions.Time32.Now;


            Desert.CheckUpSoldierReamins();
            Bird.CheckUpSoldierReamins();

            TowerofMystery.GenerateSectorTraps(50, 336, 1417);
            TowerofMystery.GenerateSectorTraps(59, 334, 1417);
            TowerofMystery.GenerateSectorTraps(32, 351, 1417);
            TowerofMystery.GenerateSectorTraps(30, 346, 1417);
            TowerofMystery.GenerateSectorTraps(12, 355, 1417);
            TowerofMystery.GenerateSectorTraps(22, 341, 1417);
   
        }
    }
}
