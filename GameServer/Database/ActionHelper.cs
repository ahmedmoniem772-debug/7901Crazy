using VirusX.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Database
{
    public static class ActionHelper
    {

        public static Action<ServerSockets.Packet, Client.GameClient> LvlAction = null;

        public static void Create()
        {
            LvlAction = new Action<ServerSockets.Packet, Client.GameClient>(Uplvl);
        }

        private static void Uplvl(ServerSockets.Packet stream, Client.GameClient client)
        {
            while (client.Player.Experience >= Pool.LevelInfo[Database.DBLevExp.Sort.User][(byte)client.Player.Level].Experience)
            {
                client.Player.Experience -= Pool.LevelInfo[Database.DBLevExp.Sort.User][(byte)client.Player.Level].Experience;
                ushort newlev = (ushort)(client.Player.Level + 1);
                client.UpdateLevel(stream, newlev);

                if (client.Player.Level >= 140)
                {
                    client.Player.Experience = 0;
                    break;
                }
            }
        }

    }
}
