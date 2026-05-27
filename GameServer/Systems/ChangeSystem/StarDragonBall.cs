using VirusX.Client;
using VirusX.Database.DBActions;
using VirusX.Game.MsgServer;
using VirusX.ServerSockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VirusX.Database
{
    public class StarDragonBall
    {
        public class User
        {
            public uint UID;
            public string HWID;
            public DateTime Timer = new DateTime();
            public override string ToString()
            {
                WriteLine writeLine = new WriteLine('/');
                writeLine.Add(UID).Add(HWID).Add(Timer.Ticks);
                return writeLine.Close();
            }
        }
        public static List<StarDragonBall.User> UsersPoll = new List<StarDragonBall.User>();

        public static bool TryGetObject(uint UID, string HWID, out User obj)
        {
            foreach (var _obj in UsersPoll)
            {
                if (_obj.UID == UID  || _obj.HWID == HWID)
                {
                    obj = _obj;
                    return true;
                }
            }
            obj = null;
            return false;
        }
        public static bool CanClaim(Client.GameClient client)
        {
            User _user;
            if (TryGetObject(client.Player.UID ,client.OnLogin.HWID, out _user))
            {
                if (_user.Timer.AddHours(12) < DateTime.Now)
                    return true;
                else
                    return false;
            }
            return true;
        }
        public static void GetPrize(GameClient client)
        {
            bool HavePrize = false;
            using (RecycledPacket recycledPacket = new RecycledPacket())
            {

                ServerSockets.Packet stream = recycledPacket.GetStream();
                if (Role.Core.Rate(10))
                {
                    client.Inventory.AddItemWitchStack(4200013, 0, 1, stream);
                    client.CreateBoxDialog("You Received " + Pool.ItemsBase[4200013].Name + " 1");
                    HavePrize = true;
                    return;
                }
                if (Role.Core.Rate(25))
                {
                    client.Inventory.AddItemWitchStack(4200012, 0, 1, stream);
                    client.CreateBoxDialog("You Received " + Pool.ItemsBase[4200012].Name + " 1");
                    HavePrize = true;
                    return;
                }
                if (Role.Core.Rate(15))
                {
                    client.Player.ConquerPoints += 50000;
                    client.CreateBoxDialog("You Received 50000 ConquerPoints");
                    HavePrize = true;
                    return;
                }
                if (Role.Core.Rate(35))
                {
                    client.Player.BoundConquerPoints += 50000;
                    client.CreateBoxDialog("You Received 50000 BoundConquerPoints");
                    HavePrize = true;
                    return;
                }
                if (Role.Core.Rate(55))
                {
                    client.Player.Money += 1000000000;
                    client.CreateBoxDialog("You Received 1000000000 Money");
                    HavePrize = true;
                    return;
                }
                if (Role.Core.Rate(50))
                {
                    client.Player.Money += 2000000000;
                    client.CreateBoxDialog("You Received 2000000000 Money");
                    HavePrize = true;
                    return;

                }
                if (Role.Core.Rate(30))
                {
                    client.Player.Money += 3000000000;
                    client.CreateBoxDialog("You Received 3000000000 Money");
                    HavePrize = true;
                    return;
                }
                if (!HavePrize)
                {
                    client.Inventory.AddItemWitchStack(4060001, 0, 2000, stream);
                    client.CreateBoxDialog("You Received " + Pool.ItemsBase[4060001].Name + " 2000");
                    HavePrize = true;
                    return;
                }
            }
        }
        public static void CheckUp(GameClient client)
        {
            if (client.Player.CanClaim)
            {
                User _user;
                if (TryGetObject(client.Player.UID, client.OnLogin.MacAddress, out _user))
                {
                    _user.Timer = DateTime.Now;
                }
                else
                {
                    _user = new User();
                    _user.UID = client.Player.UID;
                    _user.HWID = client.OnLogin.HWID;
                    _user.Timer = DateTime.Now;
                    UsersPoll.Add(_user);
                }
                using (RecycledPacket recycledPacket = new RecycledPacket())
                {
                    ServerSockets.Packet stream = recycledPacket.GetStream();
                    if (client.Inventory.Contain(3000502, 1)
                          && client.Inventory.Contain(3000503, 1)
                          && client.Inventory.Contain(3000504, 1)
                         && client.Inventory.Contain(3000505, 1)
                         && client.Inventory.Contain(3000506, 1)
                         && client.Inventory.Contain(3000507, 1)
                         && client.Inventory.Contain(3000508, 1))
                    {
                        client.Inventory.RemoveStackItem(3000502, 1, stream);
                        client.Inventory.RemoveStackItem(3000503, 1, stream);
                        client.Inventory.RemoveStackItem(3000504, 1, stream);
                        client.Inventory.RemoveStackItem(3000505, 1, stream);
                        client.Inventory.RemoveStackItem(3000506, 1, stream);
                        client.Inventory.RemoveStackItem(3000507, 1, stream);
                        client.Inventory.RemoveStackItem(3000508, 1, stream);
                        GetPrize(client);
                        client.Player.CanClaim = false;
                        VirusX.Game.ServerLogs.StarDragonBall(client);

                    }
                }
            }

        }
        public static void Save()
        {
            using (Write write = new Write("Claim.txt"))
            {
                foreach (var user in StarDragonBall.UsersPoll)
                    write.Add(user.ToString());
                write.Execute(Database.DBActions.Mode.Open);
            }
        }

        public static void Load()
        {
            using (Read read = new Read("Claim.txt"))
            {
                if (!read.Reader())
                    return;
                int count = read.Count;
                for (uint index = 0; (long)index < (long)count; ++index)
                {
                    ReadLine readLine = new ReadLine(read.ReadString(""), '/');
                    StarDragonBall.UsersPoll.Add(new StarDragonBall.User()
                    {
                        UID = readLine.Read((uint)0),
                        HWID = readLine.Read(""),
                        Timer = DateTime.FromBinary(readLine.Read((long)0))
                    });
                }
            }
        }



    }
}
