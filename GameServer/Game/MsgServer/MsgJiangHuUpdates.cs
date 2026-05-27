using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ConquerOnline.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {
        public static void GetJiangHuUpdates(this ServerSockets.Packet stream, out  MsgJiangHuUpdates.Action Action, out byte Star, out byte Stage, out byte High, out byte KnowledgePill)
        {
            uint freecourse = stream.ReadUInt32();//4
            High = stream.ReadUInt8();//8
            Action = (MsgJiangHuUpdates.Action)stream.ReadUInt8();//9
            Star = stream.ReadUInt8();//10
            Stage = stream.ReadUInt8();//11
            KnowledgePill = stream.ReadUInt8();//12
            stream.ReadUInt16();//13
            stream.ReadUInt8();//15
            stream.ReadUInt8();//16
        }

        public static unsafe ServerSockets.Packet JiangHuUpdatesCreate(this ServerSockets.Packet stream, uint FreeCourse, MsgJiangHuUpdates.Action Action, byte Star, byte Stage, ushort Atribut, byte FreeTimeTodeyUsed, uint RoundBuyPoints, byte High, byte KnowledgePill)
        {
            stream.InitWriter();

            stream.Write(FreeCourse);//4
            stream.Write((byte)High);//8
            stream.Write((byte)Action);//9
            stream.Write(Star);//10
            stream.Write(Stage);//11
            stream.Write(KnowledgePill);//12
            stream.Write(Atribut);//13
            stream.Write(FreeTimeTodeyUsed);//15
            stream.Write(RoundBuyPoints);//16

            stream.Finalize(GamePackets.JiangHuUpdates);

            return stream;
        }

    }
    public unsafe struct MsgJiangHuUpdates
    {
        public enum Action : byte
        {
            CreateStar = 0,
            BuyStrength = 1,
            TrainingPill = 2
        }


        [PacketAttribute(GamePackets.JiangHuUpdates)]
        private static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {

            MsgJiangHuUpdates.Action Mode;
            byte Star;
            byte Stage;
            byte Higher;
            byte KnowledgePill;
            stream.GetJiangHuUpdates(out Mode, out Star, out Stage, out Higher, out KnowledgePill);

            if (user.PokerPlayer != null)
                return;
            switch (Mode)
            {
                case Action.CreateStar:
                    {
                        if (user.Player.MyJiangHu != null)
                        {
                            if (Star > 9 || Stage > 9)
                                break;
                            if (Star > 1 && !user.Player.MyJiangHu.ArrayStages[Stage - 1].ArrayStars[Star - 2].Activate)
                                break;
                            if (user.Inventory.Contain(3002926, 1))
                            {
                                user.Player.MyJiangHu.FreeTimeToday -= 1;
                                user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                user.Player.MyJiangHu.OnJiangMode = true;
                                user.Player.MyJiangHu.SendStatus(stream, user, user);
                                user.Player.MyJiangHu.Talent -= 1;
                                user.Player.MyJiangHu.FreeCourse -= 10000;
                                user.Player.SubClass.RemoveStudy(user, 20, stream);
                                user.Inventory.Remove(3002926, 1, stream);
                                user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                            }
                           else if (user.Player.SubClass.StudyPoints >= 20 && user.Player.MyJiangHu.Talent >= 1 && user.Player.MyJiangHu.FreeCourse >= 10000 && Higher != 1 && Higher != 2)
                            {

                                user.Activeness.IncreaseTask(38);

                                user.Player.MyJiangHu.FreeTimeToday -= 1;
                                user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                user.Player.MyJiangHu.OnJiangMode = true;
                                user.Player.MyJiangHu.SendStatus(stream, user, user);
                                user.Player.MyJiangHu.Talent -= 1;
                                user.Player.MyJiangHu.FreeCourse -= 10000;
                                user.Player.SubClass.RemoveStudy(user, 20, stream);
                                user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                              
                            }
                            else if (user.Player.MyJiangHu.Talent >= 1 && user.Player.MyJiangHu.FreeCourse >= 10000 && user.Inventory.Contain(3003124, 1))
                            {
                                user.Player.MyJiangHu.FreeTimeToday -= 1;
                                user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                user.Player.MyJiangHu.OnJiangMode = true;
                                user.Player.MyJiangHu.SendStatus(stream, user, user);
                                user.Player.MyJiangHu.Talent -= 1;
                                user.Player.MyJiangHu.FreeCourse -= 10000;
                                user.Player.SubClass.RemoveStudy(user, 20, stream);
                                user.Inventory.Remove(3003124, 1, stream);
                                  user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                            }
                          
                            #region Higher
                            if (Higher == 1)
                            {
                                if (user.Inventory.Contain(3003125, 1))
                                {
                                    user.Inventory.Remove(3003125, 1, stream);
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3003125, 1) && user.Player.ConquerPoints >= 5)
                                {
                                    user.Inventory.Remove(3003125, 1, stream);
                                    user.Player.ConquerPoints -= 5;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                            #region Highst
                            if (Higher == 2)
                            {
                                if (user.Inventory.Contain(3003126, 1))
                                {
                                    user.Inventory.Remove(3003126, 1, stream);
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3003126, 1) && user.Player.ConquerPoints >= 50)
                                {
                                    user.Inventory.Remove(3003126, 1, stream);
                                    user.Player.ConquerPoints -= 50;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                            #region knowledgePill
                            if (KnowledgePill == 1)
                            {
                                if (user.Inventory.Contain(3303373, 1))
                                {
                                    user.Inventory.Remove(3303373, 1, stream);
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                    user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3303373, 1) && user.Player.ConquerPoints >= 50)
                                {
                                    user.Inventory.Remove(3303373, 1, stream);
                                    user.Player.ConquerPoints -= 50;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                    user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                        }
                        break;
                    }
             
                case Action.BuyStrength:
                    {
                        if (user.Player.MyJiangHu != null)
                        {
                            if (Star > 9 || Stage > 9)
                                break;
                            if (Star > 1 && !user.Player.MyJiangHu.ArrayStages[Stage - 1].ArrayStars[Star - 2].Activate)
                                break;
                            
                            ushort GetCpsStage = (ushort)((user.Player.MyJiangHu.RoundBuyPoints * 10) + 10);
                            ushort GetCpsHigher = (ushort)((user.Player.MyJiangHu.RoundBuyPoints * 10) + 15);
                            ushort GetCpsHighst = (ushort)((user.Player.MyJiangHu.RoundBuyPoints * 10) + 60);
                            if (user.Player.ConquerPoints >= GetCpsStage)
                            {
                                user.Activeness.IncreaseTask(38);

                                user.Player.ConquerPoints -= GetCpsStage;
                                user.Player.MyJiangHu.OnJiangMode = true;
                                user.Player.MyJiangHu.SendStatus(stream, user, user);
                                user.Player.MyJiangHu.FreeCourse += 10000;
                                user.Player.MyJiangHu.Talent = (byte)Math.Min(5, user.Player.MyJiangHu.Talent + 1);

                                user.Player.AddChampionPoints(10);
                                user.SendSysMesage("You received 10 ChampionPoints.");
                               
                                user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);

                                  user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);

                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                            }
                            else
                            {

                                user.SendSysMesage("You do not have " + GetCpsStage + " ConquerPoints with you.");


                            }
                            #region Higher
                            if (Higher == 1)
                            {
                                if (user.Inventory.Contain(3003125, 1) && user.Player.ConquerPoints >= GetCpsHigher)
                                {
                                    user.Inventory.Remove(3003125, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHigher;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                   
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3003125, 1) && user.Player.ConquerPoints >= GetCpsHigher)
                                {
                                    user.Inventory.Remove(3003125, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHigher;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                            #region Highst
                            if (Higher == 2)
                            {
                                if (user.Inventory.Contain(3003126, 1) && user.Player.ConquerPoints >= GetCpsHighst)
                                {
                                    user.Inventory.Remove(3003126, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHighst;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                   
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3003126, 1) && user.Player.ConquerPoints >= GetCpsHighst)
                                {
                                    user.Inventory.Remove(3003126, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHighst;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                   
                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                            #region knowledgePill
                            if (KnowledgePill == 1)
                            {
                                if (user.Inventory.Contain(3303373, 1))
                                {
                                    user.Inventory.Remove(3303373, 1, stream);
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                    user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3303373, 1) && user.Player.ConquerPoints >= 50)
                                {
                                    user.Inventory.Remove(3303373, 1, stream);
                                    user.Player.ConquerPoints -= 50;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                    user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                        }
                        break;
                    }
                case Action.TrainingPill:
                    {
                        if (user.Player.MyJiangHu != null)
                        {
                            if (Star > 9 || Stage > 9)
                                break;
                            if (Star > 1 && !user.Player.MyJiangHu.ArrayStages[Stage - 1].ArrayStars[Star - 2].Activate)
                                break;
                            ushort GetCpsStage = (ushort)((user.Player.MyJiangHu.RoundBuyPoints * 10) + 10);
                            ushort GetCpsHigher = (ushort)((user.Player.MyJiangHu.RoundBuyPoints * 10) + 15);
                            ushort GetCpsHighst = (ushort)((user.Player.MyJiangHu.RoundBuyPoints * 10) + 60);
                            #region Higher
                            if (Higher == 1)
                            {
                                if (user.Inventory.Contain(3003125, 1) && user.Player.ConquerPoints >= GetCpsHigher)
                                {
                                    user.Inventory.Remove(3003125, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHigher;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;

                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3003125, 1) && user.Player.ConquerPoints >= GetCpsHigher)
                                {
                                    user.Inventory.Remove(3003125, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHigher;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;

                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                            #region Highst
                            if (Higher == 2)
                            {
                                if (user.Inventory.Contain(3003126, 1) && user.Player.ConquerPoints >= GetCpsHighst)
                                {
                                    user.Inventory.Remove(3003126, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHighst;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;

                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3003126, 1) && user.Player.ConquerPoints >= GetCpsHighst)
                                {
                                    user.Inventory.Remove(3003126, 1, stream);
                                    user.Player.ConquerPoints -= GetCpsHighst;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;

                                      user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                            #region knowledgePill
                            if (KnowledgePill == 1)
                            {
                                if (user.Inventory.Contain(3303373, 1))
                                {
                                    user.Inventory.Remove(3303373, 1, stream);
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                    user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                                else if (!user.Inventory.Contain(3303373, 1) && user.Player.ConquerPoints >= 50)
                                {
                                    user.Inventory.Remove(3303373, 1, stream);
                                    user.Player.ConquerPoints -= 50;
                                    user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                    user.Player.MyJiangHu.OnJiangMode = true;
                                    user.Player.MyJiangHu.SendStatus(stream, user, user);
                                    user.Player.MyJiangHu.FreeTimeToday -= 1;
                                    user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                    user.Player.MyJiangHu.Talent -= 1;
                                    user.Player.MyJiangHu.FreeCourse -= 10000;
                                    user.Player.SubClass.RemoveStudy(user, 20, stream);
                                    user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                    user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                    user.Player.SubClass.UpdateStundyPoints(user, 20, stream);
                                }
                            }
                            #endregion
                            else if (!user.Inventory.Contain(3003124, 1))
                            {
                                user.Player.ConquerPoints -= GetCpsStage;
                                user.Player.MyJiangHu.RoundBuyPoints = (byte)Math.Min(49, user.Player.MyJiangHu.RoundBuyPoints + 1);
                                user.Player.MyJiangHu.OnJiangMode = true;
                                user.Player.MyJiangHu.SendStatus(stream, user, user);
                                user.Player.MyJiangHu.FreeTimeToday -= 1;
                                user.Player.MyJiangHu.Level = (byte)user.Player.Level;
                                user.Player.MyJiangHu.Talent -= 1;
                                user.Player.MyJiangHu.FreeCourse += 10000;

                                  user.Player.MyJiangHu.CreateRollValue(stream, user, Star, Stage, Higher, KnowledgePill);
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTime, false, user.Player.MyJiangHu.FreeCourse.ToString(), user.Player.MyJiangHu.Time.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateStar, false, Stage.ToString(), Star.ToString());
                                user.Player.MyJiangHu.SendInfo(user, MsgJiangHuInfo.JiangMode.UpdateTalent, true, user.Player.UID.ToString(), user.Player.MyJiangHu.Talent.ToString());
                                user.Player.SubClass.UpdateStundyPoints(user, 20, stream);

                            }
                          
                        }
                        break;
                    }
                default: Console.WriteLine("[Jiang] Unknown Action " + Mode); break;
            }

        }
    }
}
