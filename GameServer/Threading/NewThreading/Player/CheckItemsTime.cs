using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class CheckItemsTime
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
                    if (DateTime.Now > client.CheckItemTimeStamp)
                    {
                        try
                        {
                            Client.PoolProcesses.CheckItemsTime(client);
                            client.CheckItemTimeStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_CheckItemsTime);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > CheckItemsTime thread Failure");
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
