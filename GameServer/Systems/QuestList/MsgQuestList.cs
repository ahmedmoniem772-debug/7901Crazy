using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe void GetQuestList(this ServerSockets.Packet stream, out  MsgQuestList.QuestMode Mode, out ushort Count, out uint QuestID, out MsgQuestList.QuestListItem.QuestStatus QuestMode
            , out uint QuestTimer)
        {
            Mode = (MsgQuestList.QuestMode)stream.ReadUInt16();//4
            Count = stream.ReadUInt16();//6
            stream.ReadUInt64();//8
            stream.ReadUInt64();
            QuestID = stream.ReadUInt32();//24
            QuestMode = (MsgQuestList.QuestListItem.QuestStatus)stream.ReadUInt32();//28
            QuestTimer = stream.ReadUInt32();//32

        }
        public static unsafe ServerSockets.Packet QuestListCreate(this ServerSockets.Packet stream, MsgQuestList.QuestMode Mode, ushort Count)
        {
            stream.InitWriter();

            stream.Write((ushort)Mode);//4
            stream.Write(Count);//6
            stream.ZeroFill(16);//9
            return stream;
        }
        public static unsafe ServerSockets.Packet AddItemQuestList(this ServerSockets.Packet stream, MsgQuestList.QuestListItem item)
        {
            
            stream.Write(item.UID);//24
            stream.Write((uint)item.Status);//28
            stream.Write(item.Time);//32
            return stream;
        }
        public static unsafe ServerSockets.Packet QuestListFinalize(this ServerSockets.Packet stream)
        {
            stream.Finalize(GamePackets.MsgTaskStatus);
            return stream;
        }
    }



    public unsafe struct MsgQuestList
    {
        public class QuestListItem
        {
            public enum QuestStatus : uint
            {
                Accepted = 0,
                Finished = 1,
                Available = 2
            }
            public uint UID;
            public QuestStatus Status;
            public uint Time;
            public uint[] Intentions = new uint[1];
        }
        public enum QuestMode : ushort
        {
            AcceptQuest = 1,
            QuitQuest = 2,
            Review = 3,
            FinishQuest = 4,
            ShowBountyHall = 10,
            RefreshBountyHall = 11,
            AcceptBountyHall = 12,
            CancelBountyHall = 13,
            FinishBountyHall = 15,
        }


        [PacketAttribute(GamePackets.MsgTaskStatus)]
        private static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {

            MsgQuestList.QuestMode Mode;
            ushort Count;
            uint QuestID; 
            MsgQuestList.QuestListItem.QuestStatus QuestaMode;
             uint QuestTimer;

             stream.GetQuestList(out Mode, out Count, out QuestID, out QuestaMode, out QuestTimer);

            switch (Mode)
            {
                case QuestMode.FinishBountyHall:
                    {
                        user.Player.QuestGUI.FinishQuest(QuestID);

                        break;
                    }
                case QuestMode.ShowBountyHall:
                    {
                        user.Player.QuestGUI.ShowAll(stream);

                        break;
                    }
                case QuestMode.RefreshBountyHall:
                    {
                        user.Player.QuestGUI.RefreshBountyHall(stream);
                        break;
                    }
                case QuestMode.AcceptBountyHall:
                    {
                        Database.QuestInfo.DBQuest quest;
                        if (Database.QuestInfo.AllQuests.TryGetValue(QuestID, out quest))
                        {
                            user.Player.QuestGUI.AcceptBountyHall(quest, QuestTimer);
                        }
                        break;
                    }
                case QuestMode.CancelBountyHall:
                    {
                        QuestListItem n_quest;
                        if (user.Player.QuestGUI.src.TryGetValue(QuestID, out n_quest))
                        {
                            user.Player.QuestGUI.SendSinglePacket(n_quest, Game.MsgServer.MsgQuestList.QuestMode.QuitQuest);
                            user.Player.QuestGUI.RemoveQuest(n_quest.UID);
                        }
                        
                        break;
                    }
                case QuestMode.AcceptQuest:
                    {
                        if (user.Player.QuestGUI.AllowAccept())
                        {
                            Database.QuestInfo.DBQuest quest;
                            if (Database.QuestInfo.AllQuests.TryGetValue(QuestID, out quest))
                            {
                                if (Database.QuestInfo.IsKingDomMission(quest.MissionId))
                                {
                                    QuestListItem _quest;
                                    user.Player.QuestGUI.AcceptKingDomMission(quest, 0, out _quest);
                                }
                                else
                                    user.Player.QuestGUI.Accept(quest, QuestTimer);
                            }
                        }
                        break;
                    }
                case QuestMode.FinishQuest:
                    {


                        break;
                    }
                case QuestMode.QuitQuest:
                    {
                        QuestListItem n_quest;
                        if (user.Player.QuestGUI.src.TryGetValue(QuestID, out n_quest))
                        {
                            user.Player.QuestGUI.SendSinglePacket(n_quest, Game.MsgServer.MsgQuestList.QuestMode.QuitQuest);
                            user.Player.QuestGUI.RemoveQuest(n_quest.UID);
                        }
                        
                        break;
                    }
                case QuestMode.Review:
                    {
                        if (Count < 80)
                            user.Player.QuestGUI.SendFullGUI(stream);
                        break;
                    }
            }
        }
    }
}
