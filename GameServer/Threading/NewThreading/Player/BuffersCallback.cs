using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class BuffersCallback
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
                    if (DateTime.Now > client.BuffersStamp)
                    {
                        try
                        {
                            Client.PoolProcesses.BuffersCallback(client);
                            client.BuffersStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_Buffers);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > BuffersCallback thread Failure");
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
