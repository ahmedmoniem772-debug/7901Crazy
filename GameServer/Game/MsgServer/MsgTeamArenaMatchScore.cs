using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet TeamArenaMatchScoreCreate(this ServerSockets.Packet stream, uint UidOne
            , uint RankOne, string nameone, uint damageone
            , uint UidTwo, uint RankTwo,string nametwo, uint damagetwo)
        {
            stream.InitWriter();

            stream.Write(UidOne);
            stream.Write(RankOne);
            stream.Write(nameone, 32);
            stream.Write(damageone);


            stream.Write(UidTwo);
            stream.Write(RankTwo);
            stream.Write(nametwo, 32);
            stream.Write(damagetwo);

            stream.Write((uint)1);

            stream.Finalize(GamePackets.MsgTeamArenaMatchScore);
            return stream;
        }

    }
}
