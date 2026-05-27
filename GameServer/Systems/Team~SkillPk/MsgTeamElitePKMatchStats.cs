using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {//MsgTeamPKArenicScore
        public static unsafe ServerSockets.Packet TeamElitePKMatchStatsCreate(this ServerSockets.Packet stream, ushort ID, MsgTournaments.MsgTeamEliteGroup.Match match)
        {
            stream.InitWriter();
            if (match.TeamsFighting.Length > 0)
            {
                var team = match.TeamsFighting[0];
                if (ID == GamePackets.MsgTeamPKArenicScore)
                    stream.Write(0);
                stream.Write(team.Leader.Player.UID);//4
                stream.Write(team.UID);//8
                stream.Write(team.Leader.Player.Name, 32);//12
                stream.Write(team.TeamName, 64);//44
                stream.Write(team.PKStats.Points);//108
            }
            else
                stream.ZeroFill(108);

            if (match.TeamsFighting.Length > 1)
            {
                var team = match.TeamsFighting[1];
                if (ID == GamePackets.MsgTeamPKArenicScore)
                    stream.Write(0);
                stream.Write(team.Leader.Player.UID);//112
                stream.Write(team.UID);//116
                stream.Write(team.Leader.Player.Name, 32);//120
                stream.Write(team.TeamName, 64);//152
                stream.Write(team.PKStats.Points);//216
            }
            else
                stream.ZeroFill(108);
            stream.Finalize(ID);
            return stream;
        }
    }
}
