using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConquerOnline.Game.MsgServer;

namespace ConquerOnline.Threading
{
    public static class StaminaCallback
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
                    if (DateTime.Now > client.StaminStamp)
                    {
                        try
                        {
                            Client.PoolProcesses.StaminaCallback(client);
                            client.StaminStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_Stamina);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > StaminaCallback thread Failure");
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
