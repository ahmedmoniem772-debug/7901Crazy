using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe void GetQuestData(this ServerSockets.Packet stream, out uint UnKnow, out uint UID, out uint[] intents)
        {
            UnKnow = stream.ReadUInt32();//4
            UID = stream.ReadUInt32();//8 35034
            intents = new uint[1];
            intents[0] = stream.ReadUInt32();//12
        }
        public static unsafe ServerSockets.Packet MsgQuestDataCreate(this ServerSockets.Packet stream, uint UnKnow, uint UID, params uint[] intents)
        {
            stream.InitWriter();
            stream.Write((ushort)0);
            stream.Write((ushort)intents.Length);
            stream.Write(UID);
            for (int x = 0; x < intents.Length; x++)
                stream.Write(intents[x]);
            if (UID == 35024)
                for (int x = 0; x < intents.Length; x++)
                    stream.Write(intents[x]);
            stream.Write(0);
            if (UID == 35007)
            {
                stream.ZeroFill(12);
                for (int x = 0; x < intents.Length; x++)
                    stream.Write(intents[x]);
            }

            stream.Finalize(GamePackets.MsgTaskDetailInfo);
            return stream;
        }
        public static unsafe ServerSockets.Packet MsgQuestDataCreate(this ServerSockets.Packet stream, uint UnKnow, uint UID, uint OwnerUID, params uint[] intents)
        {
            stream.InitWriter();

            stream.Write(UnKnow);//4
            stream.Write(UID);//8
            for (int x = 0; x < intents.Length; x++)
                stream.Write(intents[x]);//12
            stream.ZeroFill(sizeof(uint) * Math.Min(6, (6 - intents.Length)));//14
            stream.Write(OwnerUID);//16
            stream.ZeroFill(sizeof(uint));
            stream.Finalize(GamePackets.MsgTaskDetailInfo);
            return stream;
        }
    }
    public class MsgQuestData
    {
        [PacketAttribute(GamePackets.MsgTaskDetailInfo)]
        private static unsafe void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            uint UnKnow; uint UID;
            uint[] intents;
            stream.GetQuestData(out UnKnow, out UID, out intents);
            if (user.OnInterServer)
            {
                  if (Database.QuestInfo.IsKingDomMission(UID))
                  {
                      user.Player.QuestGUI.SetKingDomQuestObjectives(stream, UID, intents);
                  }
            }
            if (Database.QuestInfo.IsKingDomMission(UID))
            {
                user.Send(stream.MsgQuestDataCreate(UnKnow, UID, user.Player.UID));
            }
            user.Send(stream.MsgQuestDataCreate(UnKnow, UID, user.Player.UID));
        }
    }
}
