using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class PlayerCollection
    {
        /// <summary>
        /// <param name="iterate">the iteration action</param>
        /// </summary>
        public static void ForEach(Action<Client.GameClient> iterate)
        {
            try
            {
                if (iterate != null)
                {
                    foreach (var player in Pool.GamePoll.Values)
                    {
                        try
                        {
                            if (!player.Fake && player != null)
                            {
                                iterate(player);
                            }
                        }
                        catch { continue; }
                    }
                }
            }
            catch
            {
                MyConsole.WriteLine("Player Collection Failure");
            }
        }
    }
}
