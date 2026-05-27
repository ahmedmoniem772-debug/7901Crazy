using ConquerOnline.Client;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static void GetTexasExInteractive(this ServerSockets.Packet stream, out MsgTexasExInteractive.TexasAction pQuery)
        {
            pQuery = new MsgTexasExInteractive.TexasAction();
            pQuery = stream.ProtoBufferDeserialize<MsgTexasExInteractive.TexasAction>(pQuery);
        }
        public static ServerSockets.Packet CreateTexasExInteractive(this ServerSockets.Packet stream,   MsgTexasExInteractive.TexasAction obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgTexasExInteractive);
            return stream;
        }
    }
    public class MsgTexasExInteractive
    {
        [Flags]
        public enum TexasFlag : uint
        {
            View = 0,
            ShowMatchInfo = 1,
            ShowTournmentInfos = 2,
            JoinTournement = 3,
            //10
            Wheel = 11, 
            //14
            //20

        }
        [ProtoContract]
        public class TexasAction
        {
            [ProtoMember(1, IsRequired = true)]
            public MsgTexasExInteractive.TexasFlag Action;

            [ProtoMember(2, IsRequired = true)]
            public uint ID;

            [ProtoMember(3, IsRequired = true)]
            public uint MatchID;

            [ProtoMember(4, IsRequired = true)]
            public uint dwparam2;

            [ProtoMember(5, IsRequired = true)]
            public uint dwparam3;

            [ProtoMember(6, IsRequired = true)]
            public uint dwparam4;

            [ProtoMember(7, IsRequired = true)]
            public uint dwparam5;
        }
        [PacketAttribute(GamePackets.MsgTexasExInteractive)]
        private unsafe static void TexasExMatchFieldList(GameClient client, ServerSockets.Packet stream)
        {
             MsgTexasExInteractive.TexasAction pQuery;
             stream.GetTexasExInteractive(out pQuery);
          
             switch (pQuery.Action)
             {
                 case TexasFlag.View:
                     {
                         MsgTexasExMatchFieldList.TexasExMatchFieldList List = new MsgTexasExMatchFieldList.TexasExMatchFieldList();
                         List.Action = MsgTexasExMatchFieldList.MatchFieldListAction.View;
                         List.Rooms = new List<MsgTexasExMatchFieldList.Room>();
                         foreach (var table in Database.Poker.Tables.Values)
                         {
                             if (table.Id >= 4000 == table.Id <= 4012)
                             {
                                 List.Rooms.Add(new MsgTexasExMatchFieldList.Room() { ID = (uint)table.Id, PlayersCount = (uint)table.Players.Count });
                             }
                         }
                         client.Send(stream.CreateTexasMatchInfo(List));

                         break;
                     }
                 case TexasFlag.ShowMatchInfo:
                     {
                         client.Player.SetLocationType = 5;
                         client.Player.TableID = pQuery.ID;
                         MsgInterServer.PipeClient.Connect(client, Database.GroupServerList.InterServer.IPAddress, Database.GroupServerList.InterServer.Port);
          
                        
                         break;
                     }
                 case (TexasFlag)31:
                     {
                         var Info = new MsgNewTexasBlindsList.ProtoStructure();
                         Info.type = 5;
                         Info.Rooms = new List<MsgNewTexasBlindsList.Table>();
                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 200000, MinMoney = 78080, LastMoney = 781184, PlayerCount = 0 });

                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 1000000, MinMoney = 390528, LastMoney = 3906176, PlayerCount = 0 });

                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 5000000, MinMoney = 1953024, LastMoney = 19531136, PlayerCount = 0 });

                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 78080, MinMoney = 3906176, LastMoney = 39062400, PlayerCount = 0 });

                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 156160, MinMoney = 7812480, LastMoney = 78124928, PlayerCount = 0 });

                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 390528, MinMoney = 19531136, LastMoney = 195312384, PlayerCount = 0 });

                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 781184, MinMoney = 39062400, LastMoney = 390624896, PlayerCount = 0 });

                         Info.Rooms.Add(new MsgNewTexasBlindsList.Table() { MaxMoney = 1562496, MinMoney = 78124928, LastMoney = 781249920, PlayerCount = 0 });
                         client.Send(stream.CreateNewTexasBlindsList(Info));
                         break;
                     }
             }
             

        }
    }
}
