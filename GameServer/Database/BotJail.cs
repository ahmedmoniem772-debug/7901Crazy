using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Database
{
    public class BotJail
    {

        public class User
        {
            public uint UID;
            public uint CountJail;
            public override string ToString()
            {
                var writer = new DBActions.WriteLine('/');
                writer.Add(UID).Add(CountJail);
                return writer.Close();
            }
        }

        private static List<User> UsersPoll = new List<User>();


        public static bool TryGetObject(uint UID, out User obj)
        {
            foreach (var _obj in UsersPoll)
            {
                if (_obj.UID == UID)
                {
                    obj = _obj;
                    return true;
                }
            }
            obj = null;
            return false;
        }
        public static bool CanOutBot(Client.GameClient client)
        {
            User _user;
            if (TryGetObject(client.Player.UID, out _user))
            {
                if (_user.CountJail < 3)
                    return true;
                else
                    return false;
            }
            return true;
        }
        public static bool CanOutBotJail(Client.GameClient client)
        {
            User _user;
            if (TryGetObject(client.Player.UID, out _user))
            {
                if (_user.CountJail < 3 && client.Player.CanOut)
                    return true;
                else
                    return false;
            }
            return true;
        }
        public static void CheckUp(Client.GameClient client)
        {
            if (CanOutBot(client))
            {
                User _user;
                if (TryGetObject(client.Player.UID, out _user))
                {
                    if (client.Player.ConquerPoints >= 2000000)
                    {
                        client.Player.ConquerPoints -= 2000000;
                        client.Player.CanOut = true;
                        if (CanOutBotJail(client))
                        {
                            client.Teleport(410, 354, 1002);
                            VirusX.Game.MsgTournaments.MsgSchedules.SendSysMesage("Every One " + client.Player.Name.ToString() + " Out Jail Now Dont Forget When More Than 3 Bot Jail Your Account is Get PermanentJail", VirusX.Game.MsgServer.MsgMessage.ChatMode.Center);
                        }
                        else { client.CreateBoxDialog("You Cant out  Bot Jail Because You Dont Have 2M Cps"); }
                    }
                    else
                    {
                        client.CreateBoxDialog("You Cant out  Bot Jail Because You Dont Have 2M Cps");
                    }

                }
                else
                {
                    _user = new User();
                    _user.UID = client.Player.UID;
                    _user.CountJail++;
                    UsersPoll.Add(_user);
                    if (client.Player.ConquerPoints >= 2000000)
                    {
                        client.Player.ConquerPoints -= 2000000;
                        client.Player.CanOut = true;
                        if (CanOutBotJail(client))
                        {
                            client.Teleport(410, 354, 1002);
                            VirusX.Game.MsgTournaments.MsgSchedules.SendSysMesage("Every One " + client.Player.Name.ToString() + " Out Jail Now Dont Forget When More Than 3 Bot Jail Your Account is Get PermanentJail", VirusX.Game.MsgServer.MsgMessage.ChatMode.Center);
                        }
                        else { client.CreateBoxDialog("You Cant out  Bot Jail Because You Dont Have 2M Cps"); }
                    }
                    else
                    {
                        client.CreateBoxDialog("You Cant out  Bot Jail Because You Dont Have 2M Cps");
                    }
                }
            }

        }
        public static void JoinJail(Client.GameClient client)
        {
            User _user;
            if (TryGetObject(client.Player.UID, out _user))
            {
                _user.CountJail++;
                client.Teleport(35, 78, 10001);
                client.Player.CanOut = false;

                VirusX.Game.MsgTournaments.MsgSchedules.SendSysMesage("Every One " + client.Player.Name.ToString() + " In Jail Now Dont Forget When More Than 3 Bot Jail Your Account is Get PermanentJail", VirusX.Game.MsgServer.MsgMessage.ChatMode.Center);
            }
            else
            {
                _user = new User();
                _user.UID = client.Player.UID;
                _user.CountJail++;
                UsersPoll.Add(_user);
                client.Teleport(35, 78, 10001);
                client.Player.CanOut = false;
                VirusX.Game.MsgTournaments.MsgSchedules.SendSysMesage("Every One " + client.Player.Name.ToString() + " In Jail Now Dont Forget When More Than 3 Bot Jail Your Account is Get PermanentJail", VirusX.Game.MsgServer.MsgMessage.ChatMode.Center);
            }

        }
        public static void Save()
        {
            using (Database.DBActions.Write _wr = new Database.DBActions.Write("BotJail.txt"))
            {
                foreach (var _obj in UsersPoll)
                    _wr.Add(_obj.ToString());
                _wr.Execute(DBActions.Mode.Open);
            }
        }
        public static void Load()
        {
            using (Database.DBActions.Read r = new Database.DBActions.Read("BotJail.txt"))
            {
                if (r.Reader())
                {
                    int count = r.Count;
                    for (uint x = 0; x < count; x++)
                    {
                        Database.DBActions.ReadLine reader = new DBActions.ReadLine(r.ReadString(""), '/');
                        User user = new User();
                        user.UID = reader.Read((uint)0);
                        user.CountJail = reader.Read((uint)0);
                        UsersPoll.Add(user);
                    }
                }
            }

        }

    }
}
