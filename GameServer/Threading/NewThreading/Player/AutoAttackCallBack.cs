using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX.Game.MsgServer;

namespace VirusX.Threading
{
    public static class AutoAttackCallBack
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
                    if (DateTime.Now > client.AttackStamp)
                    {
                        try
                        {
                            Client.PoolProcesses.AutoAttackCallback(client);
                            Client.PoolProcesses.SkyStepCallback(client);
                            client.AttackStamp = DateTime.Now.AddMilliseconds(ThreadPlayer.User_AutoAttack);
                        }
                        catch
                        {
                            MyConsole.WriteLine("PlayerCollection > AutoAttackCallback thread Failure");

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
