

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusX.Game.MsgTournaments
{
    public class MsgWarOfPlayers
    {
        public static System.SafeDictionary<uint, Info> WarOfPlayers = new System.SafeDictionary<uint, Info>();

        public static Info RoundOwner;

        public static ProcesType Proces = ProcesType.Dead;

        public static Dictionary<Role.SobNpc.StaticMesh, Role.SobNpc> Furnitures = new Dictionary<Role.SobNpc.StaticMesh, Role.SobNpc>();

        public static ushort MapID = 12344;

        public class Info
        {
            public uint UID;
            public uint Score;
            public string Name;
        }

        public static void Load()
        {
            Furnitures.Add(Role.SobNpc.StaticMesh.Pole, Pool.ServerMaps[MapID].View.GetMapObject<Role.SobNpc>(Role.MapObjectType.SobNpc, 99639));
        }

        public static void Start()
        {
            RoundOwner = null;
            WarOfPlayers = new System.SafeDictionary<uint, Info>();
            Proces = ProcesType.Alive;
            MsgSchedules.SendInvitation("WarOfPlayers", "CPS", 345, 327, 1002, 0, 60);
            MsgSchedules.SendSysMesage("WarOfPlayers has been begun..");
            MsgSchedules.SendSysMesage("WarOfPlayers has been begun.", MsgServer.MsgMessage.ChatMode.TopLeft);
            Furnitures[Role.SobNpc.StaticMesh.Pole].Name = "WarOfPlayers";

            //MsgSchedules.SendSysMesage("WarOfPlayers has been begun.", MsgServer.MsgMessage.ChatMode.BroadcastMessage);

        }

        public static void End()
        {
            if (Proces == ProcesType.Alive)
            {
                Proces = ProcesType.Dead;
                MsgSchedules.SendSysMesage(RoundOwner == null ? "WarOfPlayers has ended and there is no winner" : "" + RoundOwner.Name + ", He is the winner of WarOfPlayers .", MsgServer.MsgMessage.ChatMode.TopLeft);
                //MsgSchedules.SendSysMesage(RoundOwner == null ? "WarOfPlayers has ended and there is no winner" : "" + RoundOwner.Name + ", He is the winner of WarOfPlayers .", MsgServer.MsgMessage.ChatMode.BroadcastMessage);
                if (RoundOwner != null)
                {
                    //Program.DiscordAPI.Enqueue("``Player " + RoundOwner.Name + " Win WarOfPlayer ``");
                }
                WarOfPlayers.Clear();
            }
        }

        public static void CheckUP()
        {
            DateTime Now = DateTime.Now;

            if (Proces == ProcesType.Dead && Now.Minute >= 00 && Now.Minute <= 9)
                Start();

            if (Proces == ProcesType.Alive)
            {
                if (Now.Minute == 6 && Now.Second <= 20)
                    MsgSchedules.SendSysMesage("WarOfPlayers will ended after 4 Minutes.", MsgServer.MsgMessage.ChatMode.Center);

                if (Now.Minute == 8 && Now.Second <= 20)
                    MsgSchedules.SendSysMesage("WarOfPlayers will ended after 2 Minutes.", MsgServer.MsgMessage.ChatMode.Center);

                if (Now.Minute == 9 && Now.Second <= 2)
                    MsgSchedules.SendSysMesage("WarOfPlayers will ended after 1 Minutes.", MsgServer.MsgMessage.ChatMode.Center);

                if (Now.Minute == 10)
                    End();
            }
        }

        public static void Reset(ServerSockets.Packet stream)
        {
            WarOfPlayers = new System.SafeDictionary<uint, Info>();

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
        public static void FinishRound(ServerSockets.Packet stream)
        {
            SortScores(true);
            Furnitures[Role.SobNpc.StaticMesh.Pole].Name = RoundOwner.Name;
            MsgSchedules.SendSysMesage("The Round Ownered by " + RoundOwner.Name + ".", MsgServer.MsgMessage.ChatMode.TopLeft);
            Reset(stream);
        }
        public static void UpdateScore(ServerSockets.Packet stream, uint Score, Role.Player Player)
        {
            if (!WarOfPlayers.ContainsKey(Player.UID))
                WarOfPlayers.Add(Player.UID, new Info() { Name = Player.Name, UID = Player.UID, Score = Score });
            else
                WarOfPlayers[Player.UID].Score += Score;

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
        private static void SortScores(bool getwinner = false)
        {
            if (Proces != ProcesType.Dead)
            {
                var Array = WarOfPlayers.Values.ToArray();
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
                        Game.MsgServer.MsgMessage msg = new MsgServer.MsgMessage("No " + (x + 1).ToString() + ". " + element.Name + " (" + element.Score + ")"
                           , MsgServer.MsgMessage.MsgColor.yellow, x == 0 ? MsgServer.MsgMessage.ChatMode.FirstRightCorner : MsgServer.MsgMessage.ChatMode.ContinueRightCorner);

                        SendMapPacket(msg.GetArray(rec.GetStream()));
                    }
                    if (x == 4)
                        break;
                }
            }
        }
        public static void SendMapPacket(ServerSockets.Packet packet)
        {
            foreach (var client in Pool.ServerMaps[MapID].Values)
                client.Send(packet);
        }
    }
}
