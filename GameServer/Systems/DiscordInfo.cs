using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX
{
    public static class DiscordInfo
    {
        public static void Send(Client.GameClient client, ServerSockets.Packet stream)
        {
            string info1 = client.Player.Name;
            string info2 = "Lev: (" + client.Player.Level + ")";
            stream.InitWriter();
            stream.Write(0);
            stream.Write(info1.Length);
            stream.Write(info1, info1.Length);
            stream.Write(info2.Length);
            stream.Write(info2, info2.Length);
            stream.Write(0);
            stream.Finalize(8711);
            client.Send(stream);
        }
    }
}
