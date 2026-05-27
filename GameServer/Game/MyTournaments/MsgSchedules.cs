using VirusX.Game.MsgNpc;
using VirusX.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgTournaments
{
    public class MsgSchedules
    {
        public static Extensions.Time32 Stamp = Extensions.Time32.Now.AddMilliseconds(ThreadFunctions.TournamentsStamp);
        public static Dictionary<TournamentType, ITournament> Tournaments = new Dictionary<TournamentType, ITournament>();

        public static ITournament CurrentTournament;
        internal static MsgGuildWar GuildWar;
        internal static MsgSuperGuildWar SuperGuildWar;
        internal static MsgArena Arena;
        internal static MsgClassPKWar ClassPkWar;
        internal static MsgEliteTournament ElitePkTournament;

        internal static MsgTeamPkTournament TeamPkTournament;
        internal static MsgSkillTeamPkTournament SkillTeamPkTournament;
        internal static MsgCaptureTheFlag CaptureTheFlag;
        internal static MsgClanWar ClanWar;
        internal static MsgTowerOfMystery TowerOfMystery;
        internal static MsgPkWar PkWar;
        internal static MsgSteedRace SteedRace;
        internal static MsgDisCity DisCity;
        #region Event
        //  internal static MsgTreasureThief TreasureThief;
        internal static MsgChampionsOfWarr ChampionsOfWarr;
        internal static MsgEmperorWar EmperorWar;
        internal static MsgTopWarScore TopWarScore;
        internal static MsgUnionWar UnionWar;
        internal static MsgWarOfPlayers WarOfPlayers;
        internal static MsgGuild6PoleWar6 Guild6PoleWar6;
        #endregion




        internal static MsgEliteGuildWar EliteGuildWar;

        internal static MsgMonthlyPK MonthlyPK;
        internal static MsgWeeklyPK WeeklyPK;
        internal static MsgClanTwin ClanTwin;
        internal static MsgClanApe ClanApe;
        internal static MsgClanPhoenix ClanPhoenix;
        internal static MsgClanDesert ClanDesert;
        internal static MsgClanBird ClanBird;
        internal static MsgSquidWardOctopus SquidWardOctopus;
        internal static MsgCpsCastle CpsCastle;

        internal static void Create()
        {
            Tournaments.Add(TournamentType.None, new MsgNone(TournamentType.None));
            Tournaments.Add(TournamentType.ExtremePk, new MsgBattleField(TournamentType.ExtremePk));

            Tournaments.Add(TournamentType.BattleField, new MsgBattleField(TournamentType.BattleField));




            Tournaments.Add(TournamentType.FindBox, new FindBox(TournamentType.FindBox));



            CurrentTournament = Tournaments[TournamentType.None];
            SquidWardOctopus = new MsgSquidWardOctopus();
            CpsCastle = new MsgCpsCastle();
            SuperGuildWar = new MsgSuperGuildWar();
            GuildWar = new MsgGuildWar();
            Arena = new MsgArena();
            ClassPkWar = new MsgClassPKWar(ProcesType.Dead);
            ElitePkTournament = new MsgEliteTournament();
            CaptureTheFlag = new MsgCaptureTheFlag();
            PkWar = new MsgPkWar();
            SteedRace = new MsgSteedRace();
            WeeklyPK = new MsgWeeklyPK();
            MonthlyPK = new MsgMonthlyPK();

            DisCity = new MsgDisCity();
            TowerOfMystery = new MsgTowerOfMystery();
            EliteGuildWar = new MsgEliteGuildWar();


            #region Event
            ChampionsOfWarr = new MsgChampionsOfWarr();
            TopWarScore = new MsgTopWarScore();
            UnionWar = new MsgUnionWar();
            EmperorWar = new MsgEmperorWar();
            WarOfPlayers = new MsgWarOfPlayers();

            Guild6PoleWar6 = new MsgGuild6PoleWar6();

            #endregion

            TeamPkTournament = new MsgTeamPkTournament();
            SkillTeamPkTournament = new MsgSkillTeamPkTournament();
            ClanTwin = new MsgClanTwin();
            ClanPhoenix = new MsgClanPhoenix();
            ClanApe = new MsgClanApe();
            ClanDesert = new MsgClanDesert();
            ClanBird = new MsgClanBird();

        }
        internal static void SendInvitation(string Name, string Prize, ushort X, ushort Y, ushort map, ushort DinamicID, int Secounds, Game.MsgServer.MsgStaticMessage.Messages messaj = Game.MsgServer.MsgStaticMessage.Messages.None)
        {

            string Message = " " + Name + " is about to begin! Will you join it? Prize[" + Prize + "]";


            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                {
                    var packet = new Game.MsgServer.MsgMessage(Message, MsgServer.MsgMessage.MsgColor.yellow, MsgServer.MsgMessage.ChatMode.Center).GetArray(stream);

                    if (client.Player.life)
                        return;
                    if (client.PokerPlayer != null)
                        return;
                    if (client.Player.Map == 3053 || client.Player.Map == 1860 || client.Player.Map == 10001 && !client.Player.CanOut)
                        return;
                    if (!client.Player.OnMyOwnServer || client.IsConnectedInterServer())
                        continue;
                    client.Send(packet);
                    if (map == 5040)
                        return;
                    if (
client.Player.Map == 5061 ||
client.Player.Map == 10001 ||
client.Player.Map == 5062 ||
client.Player.Map == 5063 ||
client.Player.Map == 5064 ||
client.Player.Map == 5065 ||
client.Player.Map == 5066 ||
client.Player.Map == 5051 ||
client.Player.Map == 5052 ||
client.Player.Map == 5053 ||
client.Player.Map == 5054 ||
client.Player.Map == 5055 ||
client.Player.Map == 5056 ||
client.Player.Map == 5057 ||
 client.Player.Map == 22330 ||
client.Player.Map == 5058)
                        return;
                    if (client.Player.Alive)
                    {
                        if (map == 5040)
                            return;
                        client.Player.MessageBox(Message, new Action<Client.GameClient>(user => user.Teleport(X, Y, map, DinamicID)), null, Secounds, messaj);
                    }
                }
            }
        }


        internal unsafe static void SendSysMesage(string Messaj, Game.MsgServer.MsgMessage.ChatMode ChatType = Game.MsgServer.MsgMessage.ChatMode.TopLeft
           , Game.MsgServer.MsgMessage.MsgColor color = Game.MsgServer.MsgMessage.MsgColor.red, bool SendScren = false)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                foreach (var client in Pool.GamePoll.Values.Where(p => p.Fake == false))
                {
                    var packet = new Game.MsgServer.MsgMessage(Messaj, color, ChatType).GetArray(stream);
                    client.Send(packet);

                }
            }
        }
        internal static void CheckUp()
        {
            DateTime Now64 = DateTime.Now;

            if (!Server.FullLoading)
                return;
            #region Arena
            if (Arena.Proces == ProcesType.Dead)
            {
                Arena.Proces = ProcesType.Alive;
            }
            #endregion
            try
            {

                #region Check Ups
                MsgWarOfPlayers.CheckUP();
                EmperorWar.CheckUP();
                Guild6PoleWar6.CheckUP();
                ChampionsOfWarr.CheckUP();

                TopWarScore.CheckUP();

                GuildWar.Update();
                PkWar.CheckUp();
                EliteGuildWar.CheckUP();
     
                CurrentTournament.CheckUp();
               
                #endregion
                #region UnionWar
                if (Now64.Hour == 20 && Now64.Minute == 00 && Now64.Second <= 2)
                    UnionWar.Start();
                if (Now64.Hour == 21 && Now64.Minute == 00 && Now64.Second <= 2)
                    UnionWar.CompleteUnionwar();
                #endregion
                #region Server [Tops]
         
                #region World [xx:00]
                if (Now64.Minute == 00 && Now64.Second <= 2)//359.294    359.298
                {
                    SendInvitation("World", " " + NewSystem.PrizeAllTops.AllEvent + " [CPS]", 378, 365, 1002, 0, 60);
                }
                #endregion
  
                #region SuperPk [xx:20]
                if (Now64.Minute == 20 && Now64.Second <= 2)//364.294         364.298
                {
                    SendInvitation("SuperPk", " " + NewSystem.PrizeAllTops.AllEvent + " [CPS]", 378, 365, 1002, 0, 60);
                }
                #endregion

                #region LastManStand [xx:30]
                if (Now64.Minute == 30 && Now64.Second <= 2)//359.294    359.298
                {
                    SendInvitation("LastManStand", " " + NewSystem.PrizeAllTops.AllEvent + " [CPS]", 378, 365, 1002, 0, 60);
                }
                #endregion

                #region DarkPk [xx:40]
                if (Now64.Minute == 40 && Now64.Second <= 2)//369.294        369.297
                {
                    SendInvitation("DarkPk", " " + NewSystem.PrizeAllTops.AllEvent + " [CPS]", 378, 365, 1002, 0, 60);
                }
                #endregion
           
                #endregion

             
                #region Weekly
                switch (Now64.DayOfWeek)
                {          
                    case DayOfWeek.Saturday:
                        {
                            #region CaptureTheFlag
                            if (Now64.Hour == 22 && Now64.Minute == 00 && Now64.Second == 01)
                            {
                                CaptureTheFlag.Start();
                            }
                            if (CaptureTheFlag.Proces == ProcesType.Alive)
                            {
                                CaptureTheFlag.UpdateMapScore();
                                CaptureTheFlag.CheckUpX2();
                                CaptureTheFlag.SpawnFlags();
                            }
                            if (Now64.Hour == 23 && Now64.Minute == 00 && Now64.Second == 00)
                            {
                                CaptureTheFlag.CheckFinish();
                            }
                            #endregion

                            #region Weekly PK
                            if (Now64.Hour == 23 && Now64.Minute == 00 && Now64.Second < 2)
                            {
                                WeeklyPK.Open();
                            }
                            #endregion
                            #region TeamPkTournament
                            if (Now64.Hour == 20 && Now64.Minute == 15 && Now64.Second < 2)
                            {
                                TeamPkTournament.Start();
                            }
                            #endregion
                            break;
                        }
                    case DayOfWeek.Sunday:
                        {
                            #region Guild War
                            if (Now64.Hour == 22 && Now64.Minute == 0)
                            {
                                GuildWar.Start();
                            }
                            if (Now64.Hour == 23)
                            {
                                GuildWar.CompleteEndGuildWar();
                            }
                            #endregion
                            break;
                        }
                    case DayOfWeek.Monday:
                        {
                            #region ClassPkWar
                            if (Now64.Hour == 19 && Now64.Minute == 30)
                            {
                                ClassPkWar.Start();
                            }
                            #endregion
                            break;
                        }
                    case DayOfWeek.Tuesday:
                        {
                            #region SuperGuildWar
                            if (Now64.Hour == 22 && Now64.Minute == 0)
                            {
                                if (SuperGuildWar.Proces == ProcesType.Dead)
                                    SuperGuildWar.Start();
                                if (SuperGuildWar.Proces == ProcesType.Idle)
                                {
                                    if (Now64 > SuperGuildWar.StampRound)
                                        SuperGuildWar.Began();
                                }
                                if (SuperGuildWar.Proces != ProcesType.Dead)
                                {
                                    if (DateTime.Now > SuperGuildWar.StampShuffleScore)
                                    {
                                        SuperGuildWar.ShuffleGuildScores();
                                    }
                                }
                            }
                            else if (Now64.Hour == 23)
                            {
                                if (SuperGuildWar.Proces == ProcesType.Alive || GuildWar.Proces == ProcesType.Idle)
                                    SuperGuildWar.CompleteEndGuildWar();
                                SuperGuildWar.Save();
                            }
                            #endregion
                            break;
                        }
                    case DayOfWeek.Wednesday:
                        {
                            #region SkillTeamPkTournament
                            if (Now64.Hour == 20 && Now64.Minute == 15 && Now64.Second < 2)
                            {
                                SkillTeamPkTournament.Start();
                            }
                            #endregion
                            break;
                        }
                    case DayOfWeek.Friday:
                        {

                            #region ElitePkTournament
                            if (Now64.Hour != 19 && Now64.Hour != 20 && Now64.Hour != 21)
                            {
                                if (ElitePkTournament.Proces != ProcesType.Dead)
                                {
                                    ElitePkTournament.Proces = ProcesType.Dead;
                                }
                            }
                            if (Now64.Hour == 20 && Now64.Minute == 15)
                            {
                                ElitePkTournament.Start();
                            }
                            #endregion


                            break;
                        }
                }
                #endregion
            }
            catch (Exception e)
            {
                MyConsole.SaveException(e);
            }

         
        }
    }
}