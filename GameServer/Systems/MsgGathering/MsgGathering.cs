
using ProtoBuf;
using System;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {

        public static void GetGathering(this ServerSockets.Packet stream, out MsgGathering pQuery)
        {
            pQuery = new MsgGathering();
            pQuery = stream.ProtoBufferDeserialize<MsgGathering>(pQuery);
        }

        public static ServerSockets.Packet CreateGathering(this ServerSockets.Packet stream, MsgGathering obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize((object)obj);
            stream.Finalize(GamePackets.MsgEnemyInvadeOpt);
            return stream;
        }
    }
    [ProtoContract]
    public class MsgGathering
    {

        [ProtoMember(1, IsRequired = true)]
        public TypeID Type;

        [ProtoMember(2, IsRequired = true)]
        public uint UID = 0;//UID

        [ProtoMember(3, IsRequired = true)]
        public uint MyPoints = 0;

        [ProtoMember(4, IsRequired = true)]
        public uint StartTime = 0;

        [ProtoMember(5, IsRequired = true)]
        public uint Change = 0;

        [ProtoMember(6, IsRequired = true)]
        public uint Recovery = 0;

        [ProtoMember(7, IsRequired = true)]
        public uint unk7 = 0;

        [ProtoMember(8, IsRequired = true)]
        public uint unk8 = 0;

        [ProtoMember(9, IsRequired = true)]
        public uint unk9 = 0;

        [ProtoMember(10, IsRequired = true)]
        public uint unk10 = 0;

        [ProtoMember(11, IsRequired = true)]
        public uint unk11 = 0;

        [ProtoMember(12, IsRequired = true)]
        public Players[] ListBots;

        [ProtoContract]
        public class Players
        {
            [ProtoMember(1, IsRequired = true)]
            public uint UID;

            [ProtoMember(1, IsRequired = true)]
            public uint PrizePoint;//150

            [ProtoMember(3, IsRequired = true)]
            public uint unk12;
            
        }
        public enum TypeID : byte
        {
            Login = 0,
            Start = 1,
            Claim = 2,
            Info = 3,
            Fight = 4,
            Change = 5,
            Show = 7,
            Random = 8
        }

        [PacketAttribute(GamePackets.MsgEnemyInvadeOpt)]
        public static void MsgEnemyInvadeOpt(Client.GameClient client, ConquerOnline.ServerSockets.Packet stream)
        {
            MsgGathering pQuery;
           
            stream.GetGathering(out pQuery);
            switch (pQuery.Type)
            {

                case MsgGathering.TypeID.Start:
                    {
                      
                        client.Send(stream.CreateGathering(pQuery));
                        break;
                    }
                case MsgGathering.TypeID.Claim:
                    {
                        client.Send(stream.CreateGathering(pQuery));
                        break;
                    }

                case MsgGathering.TypeID.Info:
                    {
                        pQuery.UID = client.Player.UID;
                        pQuery.MyPoints = client.Player.Points;
                        pQuery.Change = client.Player.Change;
                        pQuery.unk10 = 1;
                        client.Send(stream.CreateGathering(pQuery));
                        break;
                    }
                case MsgGathering.TypeID.Fight:
                    {
                        
                        client.Send(stream.CreateGathering(pQuery));
                        break;
                    }
                case MsgGathering.TypeID.Change:
                    {
                        client.Send(stream.CreateGathering(pQuery));
                        break;
                    }
                case MsgGathering.TypeID.Show:
                    {
                        client.Send(stream.CreateGathering(pQuery));
                        break;
                    }
                case MsgGathering.TypeID.Random:
                    {
                        client.Send(stream.CreateGathering(pQuery));
                        break;
                    }
            }
           
        }
    }
}
