using ConquerOnline.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Generic;
using System.Threading.Tasks;

namespace ConquerOnline.ServerSockets
{
    class Thread4K
    {
        public TimerRule<ServerSockets.SecuritySocket> ConnectionReceive;
        public StaticPool ReceivePool;
        public static StaticPool GenericThreadPool;
        public TimerRule<GameClient> MainTimer;

        public Thread4K()
        {
            GenericThreadPool = new StaticPool(32).Run();
            ReceivePool = new StaticPool(64).Run();
            ConnectionReceive = new TimerRule<ServerSockets.SecuritySocket>(connectionReceive, 1, ThreadPriority.Highest);
            MainTimer = new TimerRule<GameClient>(MainCallBack, 250);
        }
        #region Thread
        public static void MainCallBack(GameClient client, int time)
        {
            try
            {
                if (client.Player.Class >= 4001 && client.Player.Class <= 4005)
                {
                    if (Time32.Now >= client.StampAutoAttackCallback.AddMilliseconds(900 - (client.Player.Agility / 2)))
                    {
                        PoolProcesses.AutoAttackCallback(client);
                        client.StampAutoAttackCallback = Time32.Now;
                    }
                }
                else if (Time32.Now >= client.StampAutoAttackCallback.AddMilliseconds(ThreadStamp.User_AutoAttack - (client.Player.Agility / 2)))
                {
                    PoolProcesses.AutoAttackCallback(client);

                    client.StampAutoAttackCallback = Time32.Now;
                }
                if (Time32.Now >= client.StampCharactersCallback.AddMilliseconds(ThreadStamp.User_CheckSecounds))
                {
                    PoolProcesses.CharactersCallback(client);
                    client.StampCharactersCallback = Time32.Now;
                }
                if (Time32.Now >= client.StampXPCountCallback.AddMilliseconds(ThreadStamp.User_StampXPCount))
                {
                    PoolProcesses.StampXPCountCallback(client);
                    client.StampXPCountCallback = Time32.Now;
                }
                if (Time32.Now >= client.StampFloorCallback.AddMilliseconds(ThreadStamp.User_FloorSpell))
                {
                    PoolProcesses.FloorCallback(client);
                    client.StampFloorCallback = Time32.Now;
                }
                if (Time32.Now >= client.StampBuffersCallback.AddMilliseconds(ThreadStamp.User_Buffers))
                {
                    PoolProcesses.BuffersCallback(client);
                    client.StampBuffersCallback = Time32.Now;
                }
                if (Time32.Now >= client.StampMonstersCallback.AddMilliseconds(ThreadStamp.AI_Monsters))
                {
                    PoolProcesses.MonstersCallback(client);
                    client.StampMonstersCallback = Time32.Now;
                }
                if (Time32.Now >= client.SaveMeleAttack.AddMilliseconds(ThreadStamp.User_SaveMele))
                {
                    PoolProcesses.SaveMeleAttack(client);
                    client.SaveMeleAttack = Time32.Now;
                }
                if (Time32.Now >= client.StaminaCallback.AddMilliseconds(ThreadStamp.User_Stamina))
                {
                    PoolProcesses.StaminaCallback(client);
                    client.StaminaCallback = Time32.Now;
                }

            }
            catch (Exception e)
            {
                MyConsole.SaveException(e);
            }
        }
        public bool Register(GameClient client)
        {
            if (client.TimerSubscriptions == null)
            {
                client.TimerSubscriptions = new IDisposable[]
                {
                   Subscribe(MainTimer, client)
                };
                return true;
            }
            return false;
        }
        public void Unregister(GameClient client)
        {
            lock (client.TimerSyncRoot)
            {
                if (client.TimerSubscriptions != null)
                {
                    foreach (var Now in client.TimerSubscriptions)
                        Now.Dispose();
                    client.TimerSubscriptions = null;
                }
            }
        }
        public static void connectionReceive(ServerSockets.SecuritySocket wrapper, int time)
        {
            try
            {
                if (wrapper.ReceiveBuffer())
                {
                    wrapper.HandlerBuffer();
                }
            }
            catch (Exception e)
            {
                MyConsole.SaveException(e);
            }
        }
        #endregion
        #region Funcs
        public static void Execute(Action<int> action, int timeOut = 0, ThreadPriority priority = ThreadPriority.Normal)
        {
            GenericThreadPool.Subscribe(new LazyDelegate(action, timeOut, priority));
        }
        public static void Execute<T>(Action<T, int> action, T param, int timeOut = 0, ThreadPriority priority = ThreadPriority.Normal)
        {
            GenericThreadPool.Subscribe<T>(new LazyDelegate<T>(action, timeOut, priority), param);
        }
        public static IDisposable Subscribe(Action<int> action, int period = 1, ThreadPriority priority = ThreadPriority.Normal)
        {
            return GenericThreadPool.Subscribe(new TimerRule(action, period, priority));
        }
        public static IDisposable Subscribe<T>(Action<T, int> action, T param, int timeOut = 0, ThreadPriority priority = ThreadPriority.Normal)
        {
            return GenericThreadPool.Subscribe<T>(new TimerRule<T>(action, timeOut, priority), param);
        }
        public static IDisposable Subscribe<T>(TimerRule<T> rule, T param, StandalonePool pool)
        {
            return pool.Subscribe<T>(rule, param);
        }
        public static IDisposable Subscribe<T>(TimerRule<T> rule, T param, StaticPool pool)
        {
            return pool.Subscribe<T>(rule, param);
        }
        public static IDisposable Subscribe<T>(TimerRule<T> rule, T param)
        {
            return GenericThreadPool.Subscribe<T>(rule, param);
        }
        #endregion
    }
}
