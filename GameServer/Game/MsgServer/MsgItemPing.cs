using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateItemPing(this ServerSockets.Packet stream, uint Action, uint Time)
        {
            stream.InitWriter();
            stream.Write((uint)Action);
            stream.Write((Time != 0) ? Time : (uint)System.DateTime.Now.Value());
            stream.Finalize((ushort)GamePackets.MsgItemPing);
            return stream;
        }
        public static unsafe void GetItemPing(this ServerSockets.Packet stream, out uint Action, out uint Time)
        {
            Action = stream.ReadUInt32();
            Time = stream.ReadUInt32();
        }
    }
    public class MsgItemPing
    {
        [Packet((ushort)GamePackets.MsgItemPing)]
        private static void Handler(Client.GameClient client, ServerSockets.Packet stream)
        {
            uint Action;
            uint Time;
            stream.GetItemPing(out Action, out Time);
            client.Send(stream.CreateItemPing(Action, Time));
        }
    }
}
