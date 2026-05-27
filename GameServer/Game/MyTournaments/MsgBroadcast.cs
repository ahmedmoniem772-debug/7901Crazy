using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgTournaments
{
    public class MsgBroadcast
    {
        public const int MaxBroadcasts = 10;

        public static Extensions.Time32 TimerStamp = Extensions.Time32.Now.AddMilliseconds(ThreadFunctions.BroadCastStamp);


        public static System.Counter BroadcastCounter = new System.Counter(1);

        public struct BroadcastStr
        {
            public uint EntityID;
            public uint ID;
            public string EntityName;
            public uint SpentCPs;
            public string Message;
            public uint UnionRank;
        }

        public static DateTime LastBroadcast = DateTime.Now;

        public static BroadcastStr CurrentBroadcast = new BroadcastStr() { ID = 1 };

        public static List<BroadcastStr> Broadcasts = new List<BroadcastStr>();
        public unsafe static void Work(Extensions.Time32 clock)
        {
            if (clock > TimerStamp)
            {
                DateTime Now = DateTime.Now;
                if (Now > LastBroadcast.AddSeconds(65))
                {
                    if (Now > LastBroadcast.AddMinutes(1))
                    {
                        if (Broadcasts.Count > 0)
                        {
                            CurrentBroadcast = Broadcasts[0];
                            Broadcasts.Remove(CurrentBroadcast);
                            #region AntiSpam
                            Pool.Insults.CheckInsults(ref CurrentBroadcast.Message);
                            #endregion AntiSpam
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                Server.SendGlobalPacket(new MsgServer.MsgMessage(CurrentBroadcast.Message, "ALLUSERS", CurrentBroadcast.EntityName, MsgServer.MsgMessage.MsgColor.red, MsgServer.MsgMessage.ChatMode.BroadcastMessage).GetArray(stream, CurrentBroadcast.UnionRank));
                            }
                        }
                    }
                    else
                        CurrentBroadcast.EntityID = 1;

                    LastBroadcast = DateTime.Now;
                }
                TimerStamp.Value = clock.Value + ThreadFunctions.BroadCastStamp;
            }

        }

    }
}
