using ConquerOnline.MsgInterServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConquerOnline.MsgInterServer.Packets
{
    public unsafe static class InterArchives
    {
        public static unsafe ServerSockets.Packet InterArchivesCreate(this ServerSockets.Packet stream, ConquerOnline.Client.GameClient user)
        {
            stream.InitWriter();
            stream.Write(user.HundredWeapons.ToString());
            stream.Write(user.MyNinja.ToString());
            stream.Write(user.MyArchives.ToString());
            stream.Finalize(PacketTypes.InterServer_Archives);
            return stream;
        }
        public static unsafe void GetInterArchives(this ServerSockets.Packet stream, out string HundredWeapons, out string MyNinja, out string MyArchives)
        {
            HundredWeapons = stream.ReadStringList()[0];
            MyNinja = stream.ReadStringList()[0];
            MyArchives = stream.ReadStringList()[0];
        }
    }
}
