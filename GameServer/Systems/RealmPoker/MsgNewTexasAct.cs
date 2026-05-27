using ConquerOnline.Client;
using ProtoBuf;
using System;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static void GetNewTexasAct(this ServerSockets.Packet stream, out MsgNewTexasAct.ProtoStructure pQuery)
        {
            pQuery = new MsgNewTexasAct.ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<MsgNewTexasAct.ProtoStructure>(pQuery);
        }
        public static ServerSockets.Packet CreateNewTexasAct(this ServerSockets.Packet stream, MsgNewTexasAct.ProtoStructure obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgNewTexasAct);
            return stream;
        }
    }
    public class MsgNewTexasAct
    {
       
        [ProtoContract]
        public class ProtoStructure
        {
            [ProtoMember(1, IsRequired = true)]
            public long type;

            [ProtoMember(2, IsRequired = true)]
            public long Count;
        }
        [PacketAttribute(GamePackets.MsgNewTexasAct)]
        public unsafe static void Process(GameClient client, ServerSockets.Packet stream)
        {
           
             MsgNewTexasAct.ProtoStructure pQuery;
             stream.GetNewTexasAct(out pQuery);
             switch (pQuery.type)
             {
                 case 0:
                     {
                         if (pQuery.Count < 3)
                         {
                             var Info = new MsgNewTexasBlindsList.ProtoStructure();
                             Info.type = (byte)pQuery.Count;
                             Info.Rooms = new System.Collections.Generic.List<MsgNewTexasBlindsList.Table>();
                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 20, MinMoney = 1000, LastMoney = 10000, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 200, MinMoney = 10000, LastMoney = 100000, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 2000, MinMoney = 100000, LastMoney = 1000000, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 20000, MinMoney = 1000000, LastMoney = 78080, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 200000, MinMoney = 78080, LastMoney = 781184, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 1000000, MinMoney = 390528, LastMoney = 3906176, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 5000000, MinMoney = 1953024, LastMoney = 19531136, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 78080, MinMoney = 3906176, LastMoney = 39062400, PlayerCount = 0 });
                             client.Send(stream.CreateNewTexasBlindsList(Info));
                         }
                         else
                         {
                             var Info = new MsgNewTexasBlindsList.ProtoStructure();
                             Info.type = (byte)pQuery.Count;
                             Info.Rooms = new System.Collections.Generic.List<MsgNewTexasBlindsList.Table>();
                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 200000, MinMoney = 78080, LastMoney = 781184, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 1000000, MinMoney = 390528, LastMoney = 3906176, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 5000000, MinMoney = 1953024, LastMoney = 19531136, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 78080, MinMoney = 3906176, LastMoney = 39062400, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 156160, MinMoney = 7812480, LastMoney = 78124928, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 390528, MinMoney = 19531136, LastMoney = 195312384, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 781184, MinMoney = 39062400, LastMoney = 390624896, PlayerCount = 0 });

                             Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 1562496, MinMoney = 78124928, LastMoney = 781249920, PlayerCount = 0 });
                             client.Send(stream.CreateNewTexasBlindsList(Info));
                         }
                       
                         
                         break;
                     }
             }
        }
    }
}