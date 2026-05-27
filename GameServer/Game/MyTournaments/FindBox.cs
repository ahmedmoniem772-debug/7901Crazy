using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgTournaments
{
    public class FindBox : ITournament
    {
        public const ushort MapID = 16252;
        public ProcesType Process { get; set; }
        public int CurrentBoxes = 0;
        public DateTime StartTimer = new DateTime();
        public DateTime BoxesStamp = new DateTime();
        Role.GameMap _map;
        public Role.GameMap Map
        {
            get
            {
                if (_map == null)
                    _map = Pool.ServerMaps[MapID];
                return _map;
            }
        }
        public TournamentType Type { get; set; }
        public FindBox(TournamentType _type)
        {
            Type = _type;
            Process = ProcesType.Dead;
        }
        public bool InTournament(Client.GameClient user)
        {
            return user.Player.Map == MapID;
        }
        public void Open()
        {
            if (Process != ProcesType.Alive)
            {
                Create();
                foreach (var user in Pool.GamePoll.Values)
                    user.Player.CurrentTreasureBoxes = 0;
                Process = ProcesType.Alive;
                StartTimer = DateTime.Now.AddMinutes(10);
                BoxesStamp = DateTime.Now.AddSeconds(30);

                MsgSchedules.SendInvitation("FindBox Started wanna join ?", "", 436, 288, 1002, 0, 60);

            }
        }
        public bool Join(Client.GameClient user, ServerSockets.Packet stream)
        {
            if (Process == ProcesType.Alive)
            {
                ushort x = 0;
                ushort y = 0;
                Map.GetRandCoord(ref x, ref y);
                user.Teleport(x, y, MapID);
                return true;
            }
            return false;
        }
        private void Create()
        {
            GenerateBoxes();
        }
        private void GenerateBoxes()
        {
            for (int i = CurrentBoxes; i < 50; i++)
            {
                byte rand = (byte)Pool.GetRandom.Next(0, 5);
                ushort x = 0;
                ushort y = 0;
                
                Map.GetRandCoord(ref x, ref y);
                Game.MsgNpc.Npc np = Game.MsgNpc.Npc.Create();
                while (true)
                {
                    np.UID = (uint)Pool.GetRandom.Next(10000, 100000);
                    if (Map.View.Contain(np.UID, x, y) == false)
                        break;
                }
                np.NpcType = Role.Flags.NpcType.Talker;
                switch (rand)
                {
                    case 0: np.Mesh = 26586; break;
                    case 1: np.Mesh = 26596; break;
                    case 2: np.Mesh = 26606; break;
                    case 3: np.Mesh = 26616; break;
                    case 4: np.Mesh = 26626; break;
                    default: np.Mesh = 26586; break;
                }
                np.Map = MapID;
                np.X = x;
                np.Y = y;
                Map.AddNpc(np);
               
            }

            CurrentBoxes = 50;
        }
       
        public void CheckUp()
        {
            if (Process == ProcesType.Alive)
            {
                DateTime Now64 = DateTime.Now;
                if (Now64.Minute == 35 && Now64.Second < 2)
                {

                  
                    foreach (var user in Map.Values)
                    {
                        user.Teleport(435, 288, 1002);
                    }
                    Process = ProcesType.Dead;
                }
                else if (DateTime.Now > BoxesStamp)
                {
                    GenerateBoxes();
                   
                    BoxesStamp = DateTime.Now.AddSeconds(30);
                }
            }
        }
        public void Reward(Client.GameClient user, Game.MsgNpc.Npc npc, ServerSockets.Packet stream)
        {
            CurrentBoxes -= 1;
        jmp:
            byte rand = (byte)Pool.GetRandom.Next(0, 5);
            switch (rand)
            {
                case 0://money
                    {
                        System.Random FoxConquer = new System.Random();
                        uint Money = (uint)FoxConquer.Next(10000000, 20000000);
                       
                        user.Player.Money += Money;
                        user.Player.SendUpdate(stream, user.Player.Money, MsgServer.MsgUpdate.DataType.Money);
                        user.CreateBoxDialog("You've received " + Money + " Money.");
                        MsgSchedules.SendSysMesage(user.Player.Name + " got " + Money.ToString() + " Money while opening the TreasureBox!", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        break;
                    }
                case 1://experience
                    {
                        if (user.Player.Level == 140)
                            goto jmp;
                        user.GainExpBall(600 * 2, true, Role.Flags.ExperienceEffect.angelwing);

                        MsgSchedules.SendSysMesage(user.Player.Name + " got 2xExpBalls while opening the TreasureBox!", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);


                        break;
                    }
                case 2://cps
                    {
                        System.Random CpsS = new System.Random();
                        uint Cps = (uint)VirusX.Pool.GetRandom.Next(10000, 20000);
                        user.Player.ConquerPoints += (long)Cps;
                        MsgSchedules.SendSysMesage(user.Player.Name + " got " + Cps.ToString() + " CPs while opening the TreasureBox!", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);
                        user.CreateBoxDialog("You've received " + Cps + " ConquerPoints.");

                        break;
                    }

                case 4://item.
                    {
                        uint[] Items = new uint[]
                        {
                          
                            Database.ItemType.DragonBall,
                            Database.ItemType.PowerExpBall,
                            3005126,
                            3005127,
                            3005125,
                            3005124,
                            (uint)(700000 + (uint)Role.Flags.Gem.SuperDragonGem),
                            (uint)(700000 + (uint)Role.Flags.Gem.SuperTortoiseGem),
                            3321098,
                            723342,
                            1200002,
                            720173,
                            Database.ItemType.DragonBall
                        };
                        uint ItemID = Items[Pool.GetRandom.Next(0, Items.Length)];
                        Database.ItemType.DBItem DBItem;
                        if (Pool.ItemsBase.TryGetValue(ItemID, out DBItem))
                        {
                            if (user.Inventory.HaveSpace(1))
                                user.Inventory.Add(stream, DBItem.ID);
                            else
                                user.Inventory.AddReturnedItem(stream, DBItem.ID);

                            MsgSchedules.SendSysMesage(user.Player.Name + " got " + DBItem.Name + " while opening the TreasureBox!", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.red);


                        }
                        break;
                    }

            }
            user.Player.CurrentTreasureBoxes += 1;
            user.Player.SendString(stream, MsgServer.MsgStringPacket.StringID.Effect, true, "accession2");
            Map.RemoveNpc(npc, stream);

            ShuffleGuildScores(stream);

        }
        public void ShuffleGuildScores(ServerSockets.Packet stream)
        {
            foreach (var user in Map.Values)
            {

                Game.MsgServer.MsgMessage msg = new MsgServer.MsgMessage("---Your Score: " + user.Player.CurrentTreasureBoxes + "---", MsgServer.MsgMessage.MsgColor.yellow, MsgServer.MsgMessage.ChatMode.FirstRightCorner);


                user.Send(msg.GetArray(stream));
            }
            var array = Map.Values.OrderByDescending(p => p.Player.CurrentTreasureBoxes).ToArray();
            for (int x = 0; x < Math.Min(10, Map.Values.Length); x++)
            {
                var element = array[x];

                Game.MsgServer.MsgMessage msg = new MsgServer.MsgMessage("No " + (x + 1).ToString() + "- " + element.Player.Name + " Opened " + element.Player.CurrentTreasureBoxes.ToString() + " Boxes!", MsgServer.MsgMessage.MsgColor.yellow, MsgServer.MsgMessage.ChatMode.ContinueRightCorner);


                Send(msg.GetArray(stream));
            }
        }
        public void Send(ServerSockets.Packet stream)
        {
            foreach (var user in Map.Values)
                user.Send(stream);
        }
    }
}
