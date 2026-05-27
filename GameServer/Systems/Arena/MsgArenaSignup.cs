using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet ArenaSignupCreate(this ServerSockets.Packet stream, MsgArenaSignup.DialogType DialogID
            , MsgArenaSignup.DialogButton OptionID, Client.GameClient user)
        {
            stream.InitWriter();

            stream.Write((uint)DialogID);
            stream.Write((uint)OptionID);

            stream.Write(user.Player.UID);
            stream.Write((uint)0);
            stream.Write(user.Player.Name, 32);
            stream.Write(user.ArenaStatistic.Info.TodayRank);
            stream.Write((uint)user.Player.Class);
            stream.Write((uint)0);
            stream.Write(user.ArenaStatistic.Info.ArenaPoints);
            stream.Write((uint)user.Player.Level);

            stream.Finalize(GamePackets.MsgQualifyingInteractive);
            return stream;
        }
        public static unsafe void GetArenaSignup(this ServerSockets.Packet stream, out MsgArenaSignup.DialogType DialogID, out MsgArenaSignup.DialogButton OptionID)
        {
            DialogID = (MsgArenaSignup.DialogType)stream.ReadUInt32();
            OptionID = (MsgArenaSignup.DialogButton)stream.ReadUInt32();
            

        }
    }


    [StructLayout(LayoutKind.Explicit, Size = 60)]
    public unsafe struct MsgArenaSignup
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
            Continue = 10,
            Claim = 12,
            TierReward = 13,
            
        }
        public enum DialogButton : uint
        { 
            SignUp = 0,
            Win = 1,
            Accept = 1,
            DoGiveUp = 2,
            Lose = 3,
            MatchOff = 3,
            MatchOn = 5,
            #region Points
            Points1200 = 2,//2
            Points1400 = 3,//6
            Points1600 = 4,//14
            Points1800 = 5,//30
            Points2000 = 6,//62
            Points2200 = 7,//126
            Points2400 = 8,
            Points2600 = 9,
            Points2800 = 10,
            Points3000 = 11,
            Points3200 = 12,
            Points3400 = 13
            #endregion


        }
        [PacketAttribute(GamePackets.MsgQualifyingInteractive)]
        public static void Handler(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (user.PokerPlayer != null)
                return;
            if (user.Player.Map == 1700 || user.Player.Map == 20081 || user.Player.Map == 20082 || user.Player.Map == 20083 || user.Player.Map == 20084 || user.Player.Map == 10137|| user.Player.Map == 10250)
                return;
            DialogType DialogID;
            DialogButton OptionID;

            stream.GetArenaSignup(out DialogID, out OptionID);

            switch (DialogID)
            {
                case DialogType.OpponentGaveUp:
                    {
                        switch (OptionID)
                        {
                            case DialogButton.SignUp:
                                {
                                    Game.MsgTournaments.MsgSchedules.Arena.DoQuit(stream, user);
                                    break;
                                }
                        }
                        break;
                    }
                case DialogType.ArenaIconOn:
                    {
                        
                        user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                        Game.MsgTournaments.MsgSchedules.Arena.DoSignup(stream, user);

                        break;
                    }
                case DialogType.ArenaIconOff:
                    {
                        user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));

                        Game.MsgTournaments.MsgSchedules.Arena.DoQuit(stream, user);
                        break;
                    }
            
                case DialogType.ArenaGui:
                    {
                        switch (OptionID)
                        {
                            case DialogButton.DoGiveUp: Game.MsgTournaments.MsgSchedules.Arena.DoGiveUp(stream, user); break;
                            case DialogButton.Accept:
                                {
                                    if (user.ArenaStatistic.ArenaState == MsgTournaments.MsgArena.User.StateType.WaitForBox)
                                    {
                                        user.ArenaStatistic.AcceptBox = true;
                                        user.ArenaStatistic.ArenaState = MsgTournaments.MsgArena.User.StateType.WaitForOther;

                                        //var info = MsgTournaments.MsgSchedules.Arena.MatchesRegistered.Values.ToArray();
                                        //for (int x = 0; x < info.Length; x++)
                                        //{
                                        //    if (info[x].Players[x].Fake)
                                        //    {
                                        //        if (info[x].Players[x].ArenaStatistic.ArenaState == MsgTournaments.MsgArena.User.StateType.WaitForBox)
                                        //        {
                                        //            info[x].Players[x].ArenaStatistic.AcceptBox = true;
                                        //            info[x].Players[x].ArenaStatistic.ArenaState = MsgTournaments.MsgArena.User.StateType.WaitForOther;
                                        //            break;
                                        //        }
                                              
                                        //    }
                                        //}
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
                                    Game.MsgTournaments.MsgSchedules.Arena.DoSignup(stream, user);
                                    break;
                                }
                        }
                        break;
                    }
                case DialogType.Claim:
                    {
                        switch (OptionID)
                        {
                            case DialogButton.Points1200:
                                {
                                    user.Player.ClaimPointsArena = 1200;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points1400:
                                {
                                    user.Player.ClaimPointsArena = 1400;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points1600:
                                {
                                    user.Player.ClaimPointsArena = 1600;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points1800:
                                {
                                    user.Player.ClaimPointsArena = 1800;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points2000:
                                {
                                    user.Player.ClaimPointsArena = 2000;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points2200:
                                {
                                    user.Player.ClaimPointsArena = 2200;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points2400:
                                {
                                    user.Player.ClaimPointsArena = 2400;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points2600:
                                {
                                    user.Player.ClaimPointsArena = 2600;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points2800:
                                {
                                    user.Player.ClaimPointsArena = 2800;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points3000:
                                {
                                    user.Player.ClaimPointsArena = 3000;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points3200:
                                {
                                    user.Player.ClaimPointsArena = 3200;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                            case DialogButton.Points3400:
                                {
                                    user.Player.ClaimPointsArena = 3400;
                                    user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                                    break;
                                }
                        }
                        break;
                    }
                case DialogType.TierReward:
                    {
                        if (user.Player.ClaimPointsArena == 0)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, OptionID, user));
                        }
                        else if (user.Player.ClaimPointsArena == 1200)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, (DialogButton)2, user));
                        }
                        else if (user.Player.ClaimPointsArena == 1400)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, (DialogButton)6, user));
                        }
                        else if (user.Player.ClaimPointsArena == 1600)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, (DialogButton)14, user));
                        }
                        else if (user.Player.ClaimPointsArena == 1800)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, (DialogButton)30, user));
                        }
                        else if (user.Player.ClaimPointsArena == 2000)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, (DialogButton)62, user));
                        }
                        else if (user.Player.ClaimPointsArena == 2200)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, (DialogButton)126, user));
                        }
                        else if (user.Player.ClaimPointsArena == 2400)
                        {
                            user.Send(stream.ArenaSignupCreate(DialogID, (DialogButton)240, user));
                        }
                        break;
                    }
              
            }
        }
    }
}
