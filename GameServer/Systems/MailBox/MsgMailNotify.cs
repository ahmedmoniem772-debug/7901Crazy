using VirusX.Client;
using VirusX.ServerSockets;

namespace VirusX.Game.MsgServer
{
    public class MsgMailNotify
    {
        public static void Loading(GameClient client)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.Write((byte)(client.PrizeInfo.Count > 0 ? 3 : 2));
                stream.Finalize(GamePackets.MsgMailNotify);
                client.Send(stream);
            }
        }
    }
}