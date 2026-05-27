using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class Monstercallback
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
                    try
                    {
                        Extensions.Time32 now = Extensions.Time32.Now;
                        client.Player.View.CheckUpMonsters(now);
                    }
                    catch
                    {
                        MyConsole.WriteLine("PlayerCollection > BuffersCallback thread Failure");
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
