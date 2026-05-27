using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet TeamArenaInfoPlayersCreate(this ServerSockets.Packet stream, MsgTeamArenaInfoPlayers.KindOfParticipants Mode, uint LeaderUID, uint Count)
        {
            stream.InitWriter();
            stream.Write((uint)Mode);
            stream.Write(LeaderUID);
            stream.Write(Count);
            return stream;
        }

        public static unsafe ServerSockets.Packet AddItemTeamArenaInfoPlayers(this ServerSockets.Packet stream, MsgTournaments.MsgTeamArena.User user)
        {
            stream.Write(user.UID);
            stream.Write((uint)user.Level);
            stream.Write((uint)user.Class);
            stream.Write(user.Mesh);
            stream.Write(user.Info.TodayRank);
            stream.Write(user.Info.ArenaPoints);
            stream.Write(user.Name, 32);

            return stream;
        }
        public static unsafe ServerSockets.Packet TeamArenaInfoPlayersFinalize(this ServerSockets.Packet stream)
        {
            stream.Finalize(GamePackets.MsgTeamArenaFightingMemberInfo);
            return stream;
        }
    }

    public unsafe class MsgTeamArenaInfoPlayers
    {
        public enum KindOfParticipants : uint
        {
            Neutral = 0,
            Opponents = 1
        }

        [PacketAttribute(GamePackets.MsgTeamArenaFightingMemberInfo)]
        private static void Handler(Client.GameClient user, ServerSockets.Packet stream)
        {

        }
    }
}
