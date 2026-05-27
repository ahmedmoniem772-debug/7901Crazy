using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ConquerOnline.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {
        public static unsafe void GetTeamArenaSignup(this ServerSockets.Packet stream, out MsgTeamArenaSignup.DialogType DType
            , out MsgTeamArenaSignup.DialogButton DButton)
        {
            DType = (MsgTeamArenaSignup.DialogType)stream.ReadUInt32();
            DButton = (MsgTeamArenaSignup.DialogButton)stream.ReadUInt32();
        }

        public static unsafe ServerSockets.Packet TeamArenaSignupCreate(this ServerSockets.Packet stream, MsgTeamArenaSignup.DialogType DType
            , MsgTeamArenaSignup.DialogButton DButton, Client.GameClient client)
        {
            stream.InitWriter();
            stream.Write((uint)DType);
            stream.Write((uint)DButton);
            stream.Write(client.Player.UID);
            stream.Write(client.Player.Name, 32);
            stream.Write(client.TeamArenaStatistic.Info.TodayRank);
            stream.Write((uint)client.Player.Class);
            stream.Write(0);
            stream.Write(client.TeamArenaStatistic.Info.ArenaPoints);
            stream.Write((uint)client.Player.Level);
            stream.Finalize(GamePackets.MsgTeamArenaSignup);
            return stream;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 60)]
    public unsafe struct MsgTeamArenaSignup
    {
        public enum DialogType : uint
        {
            ArenaIconOn = 0,
            ArenaIconOff = 1,
            ArenaGui = 3,
            StartCountDown = 2,
            OpponentGaveUp = 4,
            Match = 5,
            YouAreKicked = 6,
            StartTheFight = 7,
            Dialog = 8,
            Dialog2 = 9,
            Continue = 10
        }

        public enum DialogButton : uint
        {
            Lose = 3,
            Win = 1,
            DoGiveUp = 2,
            Accept = 1,
            MatchOff = 3,
            MatchOn = 5,
            SignUp = 0
        }

        [Packet(GamePackets.MsgTeamArenaSignup)]
        private static void Handler(Client.GameClient user, ServerSockets.Packet stream)
        {
            DialogType DialogID;
            DialogButton OptionID;
            stream.GetTeamArenaSignup(out DialogID, out OptionID);
            switch (DialogID)
            {
                case DialogType.OpponentGaveUp:
                    {
                        switch (OptionID)
                        {
                            case DialogButton.SignUp:
                                {
                                    MsgTournaments.MsgSchedules.TeamArena.DoQuit(user);
                                    break;
                                }
                        }
                        break;
                    }
                case DialogType.ArenaIconOn:
                    {
                        user.Send(stream.TeamArenaSignupCreate(DialogID, OptionID, user));
                        MsgTournaments.MsgSchedules.TeamArena.DoSignup(user);
                        break;
                    }
                case DialogType.ArenaIconOff:
                    {
                        user.Send(stream.TeamArenaSignupCreate(DialogID, OptionID, user));
                        MsgTournaments.MsgSchedules.TeamArena.DoQuit(user);
                        break;
                    }
                case DialogType.ArenaGui:
                    {
                        switch (OptionID)
                        {
                            case DialogButton.DoGiveUp: MsgTournaments.MsgSchedules.TeamArena.DoGiveUp(user); break;
                            case DialogButton.Accept:
                                {
                                    if (user.Team != null)
                                    {
                                        if (user.Team.ArenaState == Role.Instance.Team.StateType.WaitForBox)
                                        {
                                            user.Team.AcceptBox = true;
                                            user.Team.ArenaState = Role.Instance.Team.StateType.WaitForOther;
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case DialogType.Continue:
                    {
                        switch (OptionID)
                        {
                            case DialogButton.SignUp:
                                {
                                    MsgTournaments.MsgSchedules.TeamArena.DoSignup(user);
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
    }
}
