using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class FloorCallback
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
                    if (DateTime.Now > client.FloorSpellStamp)
                    {
                        try
                        {
                            Client.PoolProcesses.FloorCallback(client);
                            client.FloorSpellStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_FloorSpell);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > FloorCallback thread Failure");
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
