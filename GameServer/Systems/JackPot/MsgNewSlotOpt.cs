//using VirusX.Client;
//using ProtoBuf;

//namespace VirusX.Game.MsgServer
//{
//    public unsafe static partial class MsgBuilder
//    {
//        public static unsafe ServerSockets.Packet CreateNewSlotOpt(this ServerSockets.Packet stream, MsgNewSlotOpt pQuery)
//        {
//            stream.InitWriter();
//            stream.ProtoBufferSerialize(pQuery);
//            stream.Finalize(GamePackets.MsgNewSlotOpt);
//            return stream;
//        }
//        public static void GetNewSlotOpt(this ServerSockets.Packet stream, out MsgNewSlotOpt pQuery)
//        {
//            pQuery = new MsgNewSlotOpt();
//            pQuery = stream.ProtoBufferDeserialize<MsgNewSlotOpt>(pQuery);
//        }
       
      

//    }
//    [ProtoContract]
//    public class MsgNewSlotOpt
//    {
//        public enum Action : byte
//        {
//            Spin = 0,
//            unk = 2,
//            Show = 5,
//            unk1 = 8,
//            unk2 = 9,
//        }
//        [ProtoMember(1, IsRequired = true)]
//        public Action Type;//5

//        [ProtoMember(2, IsRequired = true)]
//        public uint MoneyCps;//0 //1

//        [ProtoMember(3, IsRequired = true)]
//        public long Time;//-2038321080

//        [ProtoMember(4, IsRequired = true)]
//        public string NameWinner;//!kepryae!

//        [ProtoMember(5, IsRequired = true)]
//        public long PrizeWinner;//222928246
//        /*0C 00 8B 08 08 A0 9C 01 10 00 18 00*/
//        /*37 00 45 09 08 00 10 B2 E4 01 18 00 20 90 4E 28 00 30 00 38 03 38 06 38 02 38 04 38 07 38 03 38 62 38 64 38 01 38 05 38 62 38 06 38 07 38 02 38 06 48 A9 F5 C5 9A 05*/
//        /*0C 00 47 09 0A 06 08 10 10 C0 A9 07*/
//        [PacketAttribute(GamePackets.MsgNewSlotOpt)]
//        private static void Process(GameClient user, ServerSockets.Packet stream)
//        {
           
//            MsgNewSlotOpt pQuery;
//            stream.GetNewSlotOpt(out pQuery);
//            switch (pQuery.Type)
//            {
//                case Action.Spin:
//                    {
//                        var info = new MsgNewSlotRecord() { Member1 = pQuery.Time, Member3 = 0 };
//                        info.Member2 = new long[2];
//                        info.Member2[0] = 0;
//                        info.Member2[1] = 0;
//                        user.Send(stream.CreateNewSlotRecord(info));
//                        if (user.Player.Money >= pQuery.Time)
//                        {
//                            user.Player.Money -= pQuery.Time;
                           
//                            ushort i = 99;
//                            var infos = new MsgNewSlotResult() { Member1 = 0, Member2 = pQuery.MoneyCps, Member3 = 0, Member4 = 10000, Member5 = 0, Member6 = 0 };
//                            infos.Count = new ushort[15];
//                            infos.Count[0] = i;
//                            infos.Count[1] = i;
//                            infos.Count[2] = i;
//                            infos.Count[3] = i;
//                            infos.Count[4] = i;
//                            infos.Count[5] = i;
//                            infos.Count[6] = i;
//                            infos.Count[7] = i;
//                            infos.Count[8] = i;
//                            infos.Count[9] = i;
//                            infos.Count[10] = i;
//                            infos.Count[11] = i;
//                            infos.Count[12] = i;
//                            infos.Count[13] = i;
//                            infos.Count[14] = i;
//                            infos.Member9 = 1397848745;
//                            user.Send(stream.CreateNewSlotResult(infos));
//                        }
//                        break;
//                    }
//                case Action.unk:
//                    {
//                        #region Money
//                        if (pQuery.MoneyCps == 0)
//                        {
//                            System.DateTime now64 = System.DateTime.Now;
//                            int hours = now64.Hour;
//                            int minutes = now64.Minute;
//                            int secounds = now64.Second;

//                            int lefthours = 69 - hours;
//                            int leftminutes = 60 - minutes;
//                            int leftsecounds = 60 - secounds;

//                            System.TimeSpan now = new System.TimeSpan(System.DateTime.Now.Ticks);
//                            System.TimeSpan nextday = new System.TimeSpan(System.DateTime.Now.AddHours(lefthours).AddMinutes(leftminutes).AddSeconds(leftsecounds).Ticks);
//                            pQuery.Time = (uint)(nextday.TotalSeconds - now.TotalSeconds);
//                            pQuery.NameWinner = user.Player.Name;
//                            pQuery.PrizeWinner = user.Player.Money;
//                        }
//                        #endregion

//                        #region Cps
//                        else
//                        {
//                            System.DateTime now64 = System.DateTime.Now;
//                            int hours = now64.Hour;
//                            int minutes = now64.Minute;
//                            int secounds = now64.Second;

//                            int lefthours = 69 - hours;
//                            int leftminutes = 60 - minutes;
//                            int leftsecounds = 60 - secounds;

//                            System.TimeSpan now = new System.TimeSpan(System.DateTime.Now.Ticks);
//                            System.TimeSpan nextday = new System.TimeSpan(System.DateTime.Now.AddHours(lefthours).AddMinutes(leftminutes).AddSeconds(leftsecounds).Ticks);
//                            pQuery.Time = (uint)(nextday.TotalSeconds - now.TotalSeconds);
//                            pQuery.NameWinner = user.Player.Name;
//                            pQuery.PrizeWinner = user.Player.ConquerPoints;
//                        }
//                        #endregion

//                        user.Send(stream.CreateNewSlotOpt(pQuery));
//                        break;
//                    }
//                case Action.Show:
//                    {
//                        #region Money
//                        if (pQuery.MoneyCps == 0)
//                        {
//                            System.DateTime now64 = System.DateTime.Now;
//                            int hours = now64.Hour;
//                            int minutes = now64.Minute;
//                            int secounds = now64.Second;

//                            int lefthours = 69 - hours;
//                            int leftminutes = 60 - minutes;
//                            int leftsecounds = 60 - secounds;

//                            System.TimeSpan now = new System.TimeSpan(System.DateTime.Now.Ticks);
//                            System.TimeSpan nextday = new System.TimeSpan(System.DateTime.Now.AddHours(lefthours).AddMinutes(leftminutes).AddSeconds(leftsecounds).Ticks);
//                            pQuery.Time = (uint)(nextday.TotalSeconds - now.TotalSeconds);
//                            pQuery.NameWinner = user.Player.Name;
//                            pQuery.PrizeWinner = user.Player.Money;
//                        }
//                        #endregion

//                        #region Cps
//                        else
//                        {
//                            System.DateTime now64 = System.DateTime.Now;
//                            int hours = now64.Hour;
//                            int minutes = now64.Minute;
//                            int secounds = now64.Second;

//                            int lefthours = 69 - hours;
//                            int leftminutes = 60 - minutes;
//                            int leftsecounds = 60 - secounds;

//                            System.TimeSpan now = new System.TimeSpan(System.DateTime.Now.Ticks);
//                            System.TimeSpan nextday = new System.TimeSpan(System.DateTime.Now.AddHours(lefthours).AddMinutes(leftminutes).AddSeconds(leftsecounds).Ticks);
//                            pQuery.Time = (uint)(nextday.TotalSeconds - now.TotalSeconds);
//                            pQuery.NameWinner = user.Player.Name;
//                            pQuery.PrizeWinner = user.Player.ConquerPoints;
//                        }
//                        #endregion

//                        user.Send(stream.CreateNewSlotOpt(pQuery));
//                        break;
//                    }
//                case Action.unk1:
//                    {
//                        #region Money
//                        if (pQuery.MoneyCps == 0)
//                        {
//                            System.DateTime now64 = System.DateTime.Now;
//                            int hours = now64.Hour;
//                            int minutes = now64.Minute;
//                            int secounds = now64.Second;

//                            int lefthours = 69 - hours;
//                            int leftminutes = 60 - minutes;
//                            int leftsecounds = 60 - secounds;

//                            System.TimeSpan now = new System.TimeSpan(System.DateTime.Now.Ticks);
//                            System.TimeSpan nextday = new System.TimeSpan(System.DateTime.Now.AddHours(lefthours).AddMinutes(leftminutes).AddSeconds(leftsecounds).Ticks);
//                            pQuery.Time = (uint)(nextday.TotalSeconds - now.TotalSeconds);
//                            pQuery.NameWinner = user.Player.Name;
//                            pQuery.PrizeWinner = user.Player.Money;
//                        }
//                        #endregion

//                        #region Cps
//                        else
//                        {
//                            System.DateTime now64 = System.DateTime.Now;
//                            int hours = now64.Hour;
//                            int minutes = now64.Minute;
//                            int secounds = now64.Second;

//                            int lefthours = 69 - hours;
//                            int leftminutes = 60 - minutes;
//                            int leftsecounds = 60 - secounds;

//                            System.TimeSpan now = new System.TimeSpan(System.DateTime.Now.Ticks);
//                            System.TimeSpan nextday = new System.TimeSpan(System.DateTime.Now.AddHours(lefthours).AddMinutes(leftminutes).AddSeconds(leftsecounds).Ticks);
//                            pQuery.Time = (uint)(nextday.TotalSeconds - now.TotalSeconds);
//                            pQuery.NameWinner = user.Player.Name;
//                            pQuery.PrizeWinner = user.Player.ConquerPoints;
//                        }
//                        #endregion

//                        user.Send(stream.CreateNewSlotOpt(pQuery));
//                        break;
//                    }
//            }
//        }

//    }
    
  
//}
