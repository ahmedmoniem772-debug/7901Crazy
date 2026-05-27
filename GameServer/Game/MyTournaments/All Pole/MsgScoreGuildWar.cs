using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX.Game.MsgTournaments
{
    public class MsgTopWarScore
    {
        public System.SafeDictionary<uint, Winner> TopWarScore = new System.SafeDictionary<uint, Winner>();
        public Dictionary<Role.SobNpc.StaticMesh, Role.SobNpc> Furnitures;
        public static ushort MapID = 10504;
        public ProcesType Proces;
        public class Winner
        {
            public uint GuildID;
            public uint Score;
            public string GuildName;
        }
        public MsgTopWarScore()
        {
            Proces = ProcesType.Dead;
            Furnitures = new Dictionary<Role.SobNpc.StaticMesh, Role.SobNpc>();
        }

        public void Load()
        {
            Furnitures.Add(Role.SobNpc.StaticMesh.Pole, Pool.ServerMaps[MapID].View.GetMapObject<Role.SobNpc>(Role.MapObjectType.SobNpc, 90051));
        }
        public void CheckUP()
        {
            DateTime Now = DateTime.Now;
            if (Proces == ProcesType.Dead && Now.Hour == 16 && Now.Minute >= 00 && Now.Minute <= 09)//44 mt8yrhash
                this.Start();

            if (Proces == ProcesType.Alive)
            {
                if (Now.Hour == 16 && Now.Minute == 07 && Now.Second <= 2)
                    MsgSchedules.SendSysMesage("GuildWarScore will ended after 3 Minutes.", MsgServer.MsgMessage.ChatMode.Center);
                if (Now.Hour == 16 && Now.Minute == 08 && Now.Second <= 2)
                    MsgSchedules.SendSysMesage("GuildWarScore will ended after 2 Minutes.", MsgServer.MsgMessage.ChatMode.Center);
                if (Now.Hour == 16 && Now.Minute == 09 && Now.Second <= 2)
                    MsgSchedules.SendSysMesage("GuildWarScore will ended after 1 Minutes.", MsgServer.MsgMessage.ChatMode.Center);

                if (Now.Hour == 16 && Now.Minute == 05 && Now.Second <= 2)
                    MsgSchedules.SendSysMesage("GuildWarScore will ended after 5 Minutes.", MsgServer.MsgMessage.ChatMode.Center);

                if (Now.Hour == 16 && Now.Minute == 10)
                    this.End();
            }
        }


        public void Start()
        {
            Proces = ProcesType.Alive;
            TopWarScore = new System.SafeDictionary<uint, Winner>();
            MsgSchedules.SendSysMesage("GuildWarScore war has began", MsgServer.MsgMessage.ChatMode.Center);
            MsgSchedules.SendInvitation("GuildWarScore has begun! Would you like to join?", "CPs", 361, 341, 1002, 0, 60);

        }
        public void Reset(ServerSockets.Packet stream)
        {
            // TopWarScore = new System.SafeDictionary<uint, Winner>();

            foreach (var npc in Furnitures.Values)
                npc.HitPoints = npc.MaxHitPoints;

            var Pole = Furnitures[Role.SobNpc.StaticMesh.Pole];
            var users = Pool.GamePoll.Values.Where(u => u != null && u.Player.Map == MapID && Role.Core.GetDistance(u.Player.X, u.Player.Y, Pole.X, Pole.Y) <= Role.SobNpc.SeedDistrance).ToArray();
            if (users != null)
            {
                foreach (var user in users)
                {
                    MsgServer.MsgUpdate upd = new MsgServer.MsgUpdate(stream, Pole.UID, 1);
                    stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.Mesh, (ulong)Pole.Mesh);
                    stream = upd.GetArray(stream);
                    user.Send(stream);
                    stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.Hitpoints, (ulong)Pole.HitPoints);
                    stream = upd.GetArray(stream);
                    user.Send(stream);
                    if ((Role.SobNpc.StaticMesh)Pole.Mesh == Role.SobNpc.StaticMesh.Pole)
                        user.Send(Pole.GetArray(stream, false));
                }
            }
        }
        public void FinishRound(ServerSockets.Packet stream)
        {
            SortScores();
            var RoundOwner = MsgSchedules.TopWarScore.TopWarScore.Values.OrderByDescending(i => i.Score).FirstOrDefault();
            Furnitures[Role.SobNpc.StaticMesh.Pole].Name = RoundOwner.GuildName;
            MsgSchedules.SendSysMesage("The Round Ownered by guild " + RoundOwner.GuildName + ".", MsgServer.MsgMessage.ChatMode.Center);
            Reset(stream);
        }
        public void End()
        {
            if (Proces == ProcesType.Alive)
            {
                Proces = ProcesType.Dead;
                var RoundOwner = MsgSchedules.TopWarScore.TopWarScore.Values.OrderByDescending(i => i.Score).FirstOrDefault();
                MsgSchedules.SendSysMesage(RoundOwner == null ? "GuildWarScore has ended and there is no winner" : "Guild " + RoundOwner.GuildName + ", He is the winner of TopWarScore.", MsgServer.MsgMessage.ChatMode.Center);
                // Program.DiscordAPI.Enqueue("``Guild " + RoundOwner.GuildName + " Win GuildWarScore, congratulations ♥ ``");
            }
        }
        public void UpdateScore(ServerSockets.Packet stream, uint Score, Role.Instance.Guild Guild)
        {
            if (!TopWarScore.ContainsKey(Guild.Info.GuildID))
                TopWarScore.Add(Guild.Info.GuildID, new Winner() { GuildName = Guild.GuildName, GuildID = Guild.Info.GuildID, Score = Score });
            else
                TopWarScore[Guild.Info.GuildID].Score += Score;

            SortScores();
            foreach (var Bas in Furnitures.Values)
            {
                if ((Role.SobNpc.StaticMesh)Bas.Mesh == Role.SobNpc.StaticMesh.Pole)
                {
                    foreach (var user in Pool.GamePoll.Values)
                    {
                        if (user.Player.Map == MapID)
                        {
                            if (Role.Core.GetDistance(user.Player.X, user.Player.Y, Bas.X, Bas.Y) <= Role.SobNpc.SeedDistrance)
                            {
                                var upd = new MsgServer.MsgUpdate(stream, Bas.UID, 2);
                                stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.Hitpoints, Bas.HitPoints);
                                stream = upd.GetArray(stream);
                                user.Send(stream);
                                stream = upd.Append(stream, MsgServer.MsgUpdate.DataType.MaxHitpoints, Bas.MaxHitPoints);
                                stream = upd.GetArray(stream);
                                user.Send(stream);
                                user.Send(Bas.GetArray(stream, false));
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            if (Furnitures[Role.SobNpc.StaticMesh.Pole].HitPoints < 1)
            {
                FinishRound(stream);
                return;
            }
        }
        private void SortScores()
        {
            if (Proces != ProcesType.Dead)
            {
                var Array = TopWarScore.Values.ToArray();
                var DescendingList = Array.OrderByDescending(p => p.Score).ToArray();
                for (int x = 0; x < DescendingList.Length; x++)
                {
                    var element = DescendingList[x];
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
