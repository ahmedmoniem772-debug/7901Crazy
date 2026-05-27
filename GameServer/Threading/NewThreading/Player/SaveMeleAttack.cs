using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class SaveMeleAttack
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
                    if (DateTime.Now > client.SaveMeleStamp && client.onSaveMele)
                    {
                        try
                        {
                            Client.PoolProcesses.SaveMeleAttack(client);
                            client.SaveMeleStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_SaveMele);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > SaveMeleAttack thread Failure");
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
