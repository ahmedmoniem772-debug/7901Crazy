
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace VirusX.Game.MsgTournaments
{
    public class MsgEliteGuildWar
    {
        public static System.SafeDictionary<uint, Winner> ScoreList = new System.SafeDictionary<uint, Winner>();
        public Dictionary<Role.SobNpc.StaticMesh, Role.SobNpc> Furnitures;
        public Winner RoundOwner;
        public static ushort MapID = 6521;
        public ProcesType Proces;
        public class Winner
        {
            public uint GuildID;
            public uint Score;
            public string GuildName;
        }
        public MsgEliteGuildWar()
        {
            Proces = ProcesType.Dead;
            Furnitures = new Dictionary<Role.SobNpc.StaticMesh, Role.SobNpc>();
        }

        public void Load()
        {
            Furnitures.Add(Role.SobNpc.StaticMesh.Pole, Pool.ServerMaps[MapID].View.GetMapObject<Role.SobNpc>(Role.MapObjectType.SobNpc, 6539));
        }

        public void CheckUP()
        {
            DateTime Now = DateTime.Now;
            if (Proces == ProcesType.Dead && Now.Minute >= 40 && Now.Minute <= 59)
                this.Start();

            if (Proces == ProcesType.Alive)
            {
                if (Now.Minute == 55 && Now.Second <= 2)
                    MsgSchedules.SendSysMesage("EliteGuildWar will ended after 5 Minutes.", MsgServer.MsgMessage.ChatMode.Center);

                if (Now.Minute == 59 && Now.Second <= 2)
                    MsgSchedules.SendSysMesage("EliteGuildWar will ended after 1 Minutes.", MsgServer.MsgMessage.ChatMode.Center);

                if (Now.Minute == 00 && Now.Second <= 2)
                    this.End();
            }
        }

        public void Start()
        {
            RoundOwner = null;
            Proces = ProcesType.Alive;
            ScoreList = new System.SafeDictionary<uint, Winner>();
            MsgSchedules.SendSysMesage("EliteGuildWar war has began", MsgServer.MsgMessage.ChatMode.Center);
            MsgSchedules.SendSysMesage("EliteGuildWar war has began", MsgServer.MsgMessage.ChatMode.BroadcastMessage);
            MsgSchedules.SendInvitation("EliteGuildWar has begun! Would you like to join?", "CPs", 345, 327, 1002, 0, 60);
           
            Furnitures[Role.SobNpc.StaticMesh.Pole].Name = "EliteGuildWar";
        }
        public void Reset(ServerSockets.Packet stream)
        {
            ScoreList = new System.SafeDictionary<uint, Winner>();

            foreach (var npc in Furnitures.Values)
                npc.HitPoints = npc.MaxHitPoints;

            var Pole = Furnitures[Role.SobNpc.StaticMesh.Pole];
            var users = Pool.GamePoll.Values.Where(u => u != null && u.Player.Map == MapID && Role.Core.GetDistance(u.Player.X, u.Player.Y, Pole.X, Pole.Y) <= Role.SobNpc.SeedDistrance).ToArray();
            if (users != null)
            {
                foreach (var user in users)
                {
                    MsgServer.MsgUpdate upd = new MsgServer.MsgUpdate(stream, Pole.UID, 2);
                    stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.Mesh, (long)Pole.Mesh);
                    stream = upd.GetArray(stream);
                    user.Send(stream);
                    stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.Hitpoints, Pole.HitPoints);
                    stream = upd.GetArray(stream);
                    user.Send(stream);
                    user.Send(Pole.GetArray(stream, false));
                }
            }
        }
        public void FinishRound(ServerSockets.Packet stream)
        {
            SortScores(true);
            Furnitures[Role.SobNpc.StaticMesh.Pole].Name = RoundOwner.GuildName;
            MsgSchedules.SendSysMesage("The Round Ownered by guild " + RoundOwner.GuildName + ".", MsgServer.MsgMessage.ChatMode.Center);
            Reset(stream);
        }
        public void End()
        {
            if (Proces == ProcesType.Alive)
            {
                Proces = ProcesType.Dead;
                MsgSchedules.SendSysMesage(RoundOwner == null ? "EliteGuildWar has ended and there is no winner" : "Guild " + RoundOwner.GuildName + ", He is the winner of EliteGuildWar.", MsgServer.MsgMessage.ChatMode.Center);
                MsgSchedules.SendSysMesage(RoundOwner == null ? "EliteGuildWar has ended and there is no winner" : "Guild " + RoundOwner.GuildName + ", He is the winner of EliteGuildWar.", MsgServer.MsgMessage.ChatMode.BroadcastMessage);
                ScoreList.Clear();
            }
        }
        public void UpdateScore(ServerSockets.Packet stream, uint Score, Role.Instance.Guild Guild)
        {
            if (!ScoreList.ContainsKey(Guild.Info.GuildID))
                ScoreList.Add(Guild.Info.GuildID, new Winner() { GuildName = Guild.GuildName, GuildID = Guild.Info.GuildID, Score = Score });
            else
                ScoreList[Guild.Info.GuildID].Score += Score;

            SortScores();

            if (Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints < 1)
            {
                FinishRound(stream);
                return;
            }
        }
        private void SortScores(bool getwinner = false)
        {
            if (Proces != ProcesType.Dead)
            {
                var Array = ScoreList.Values.ToArray();
                var DescendingList = Array.OrderByDescending(p => p.Score).ToArray();
                for (int x = 0; x < DescendingList.Length; x++)
                {
                    var element = DescendingList[x];
                    if (x == 0 && getwinner)
                    {
                        RoundOwner = element;
                    }
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        Game.MsgServer.MsgMessage msg = new MsgServer.MsgMessage("No " + (x + 1).ToString() + ". " + element.GuildName + " (" + element.Score + ")"
                           , MsgServer.MsgMessage.MsgColor.yellow, x == 0 ? MsgServer.MsgMessage.ChatMode.FirstRightCorner : MsgServer.MsgMessage.ChatMode.ContinueRightCorner);

                        SendMapPacket(msg.GetArray(rec.GetStream()));
                    }
                    if (x == 4)
                        break;
                }
            }
        }
        public void SendMapPacket(ServerSockets.Packet packet)
        {
            foreach (var client in Pool.ServerMaps[MapID].Values)
                client.Send(packet);
        }
    }
}
