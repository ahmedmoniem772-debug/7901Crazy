using CMsgTQProtect;
using MahmoudAli.ServerSockets;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MahmoudAli.CMsgTQProtect
{
    internal class MsgMachine
    {

        [ProtoContract]
        public unsafe class MachineQuery
        {
            [ProtoMember(1, IsRequired = true)]
            public uint TickCount;
            [ProtoMember(2, IsRequired = true)]
            public uint CryptoKey;
            [ProtoMember(3)]
            public string MachineName;
            [ProtoMember(4)]
            public string MachineHWID;
            [ProtoMember(5)]
            public string MachineProductID;
            [ProtoMember(6)]
            public string MachineId;
            [ProtoMember(7)]
            public string[] MachineMacAdress;
            [ProtoMember(8, IsRequired = true)]
            public uint QuerySize;
        }



        [PacketAttribute((ushort)MsgTQProtect._MSG_ID._MSG_MACHINE)]
        public unsafe static void Process(Client.GameClient client, ServerSockets.Packet packet)
        {
            packet.Seek(0);
            byte[] bytesArray = packet.ReadBytes(packet.Size);

            if (client.g_TQProtect == null || TQCipher.HandleBuffer(ref bytesArray, true) == 0)
            {
                client.g_TQProtect.Disconnect("Invalid g_TQProtect or HandleBuffer on " + Assembly.GetExecutingAssembly().FullName);
                return;
            }

            var QueryLength = BitConverter.ToInt16(bytesArray, 4);
            using (var ms = new System.IO.MemoryStream(bytesArray, 6, QueryLength))
            {
                var pQuery = ProtoBuf.Serializer.Deserialize<MachineQuery>(ms);
                if(pQuery.QuerySize != QueryLength)
                {
                    client.g_TQProtect.Disconnect("Invalid Query Size on "+ Assembly.GetExecutingAssembly().FullName);
                    return;
                }
                Console.WriteLine("[TQProtect] Player {0} PC HardWareInfo", client.Player.Name);
                Console.WriteLine("MachineName : {0}", pQuery.MachineName);
                Console.WriteLine("MacAdress : {0}", pQuery.MachineMacAdress);
                Console.WriteLine("ProductID : {0}", pQuery.MachineProductID);
                Console.WriteLine("HWID : {0}\n\n", pQuery.MachineHWID);

            }
        }
    }
}
