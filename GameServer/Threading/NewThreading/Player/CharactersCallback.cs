using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class CharactersCallback
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
                    if (DateTime.Now > client.CheckSecoundsStamp)
                    {
                        try
                        {
                            Client.PoolProcesses.CharactersCallback(client);
                            client.CheckSecoundsStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_CheckSecounds);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > CharactersCallback thread Failure");
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
