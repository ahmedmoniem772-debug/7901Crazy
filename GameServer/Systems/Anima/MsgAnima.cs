using VirusX.Client;
using VirusX.Database;
using VirusX.Role;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
namespace VirusX.Game.MsgServer
{
    public static class MsgAnima
    {
        [ProtoContract]
        public class ProtoStructure
        {
          
            [ProtoMember(1, IsRequired = true)]
           
            public uint[] Data;

            [ProtoMember(2, IsRequired = true)]
            public string Name;
        }
        
        public static unsafe ServerSockets.Packet CreateSpiritInteractive(this ServerSockets.Packet stream, ProtoStructure obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgAnima);
            return stream;
        }
        public static unsafe void GetSpiritInteractive(this ServerSockets.Packet stream, out ProtoStructure pQuery)
        {

            pQuery = new ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<ProtoStructure>(pQuery);
        }
        
        [PacketAttribute(GamePackets.MsgAnima)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {

            ProtoStructure Info;//ماهو متشفر اهو امال انا بقول اي
            stream.GetSpiritInteractive(out Info);
            Info.Data = new uint[9];
            Info.Name = "";
            client.Send(stream.CreateSpiritInteractive(Info));
        }
    }
}