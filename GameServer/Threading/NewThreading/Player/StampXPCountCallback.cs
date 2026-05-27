using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConquerOnline.Game.MsgServer;

namespace ConquerOnline.Threading
{
    public static class StampXPCountCallback
    {
        /// <summary>
        /// Controller for the player thread
        /// </summary>
        public static void Handle()
        {
            try
            {
                PlayerCollection.ForEach(client =>
                {
                    if (DateTime.Now > client.XPCountStamp)
                    {
                        try
                        {
                            Client.PoolProcesses.StampXPCountCallback(client);
                            client.XPCountStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_StampXPCount);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > StampXPCountCallback thread Failure");

                        }
                    }

                });

            }
            catch
            {
                MyConsole.WriteLine("Player Collection Failure");
            }
        }
    }
}
