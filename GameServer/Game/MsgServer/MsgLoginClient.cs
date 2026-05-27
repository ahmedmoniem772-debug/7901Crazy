using ConquerOnline.DBFunctionality;
using GuardShield;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public struct MsgLoginClient
    {

        public ushort Length;
        public ushort PacketID;
        public uint AccountHash;
        public uint Key, HDSerial;
        public string MachineName, MacAddress;
        public string HWID;
        [PacketAttribute((ushort)MsgGuardShield.PacketIDs.MsgLoginGame)]
        public unsafe static void LoginGame(Client.GameClient client, ServerSockets.Packet packet)
        {
            packet.Seek(0);
            byte[] bytes = packet.ReadBytes(packet.Size);
            var msg = new MsgGuardShield.MsgLoginGame(bytes);
            client.LoaderTime = DateTime.Now;
            client.EncryptTokenSpell = msg.OwnerAttackEncrypt;
            client.OnLogin = new MsgLoginClient()
            {
                Key = msg.Key,
                AccountHash = msg.AccountHash,
                HDSerial = msg.HDSerial,
                MachineName = msg.MachineName,
                MacAddress = msg.MacAddress,
                HWID = msg.HWID,
            };
            client.ClientFlag |= Client.ServerFlag.OnLoggion;
            Database.ServerDatabase.LoginQueue.TryEnqueue(client);
        }
        public unsafe static void LoginHandler(Client.GameClient client, MsgLoginClient packet)
        {
            client.ClientFlag &= ~Client.ServerFlag.OnLoggion;

            if (client.Socket != null)
            {
                if (client.Socket.RemoteIp == "NONE")
                {
                    // MyConsole.WriteLine("Breack login client.");
                    return;
                }
            }
            try
            {
                var pool = Pool.GamePoll.Values.ToArray();
                if (pool.Count(i => i.OnLogin.MacAddress == client.OnLogin.MacAddress) >= 9)
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        client.Send(new MsgServer.MsgMessage("You can not open more than 3 accounts.", "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                    }
                   
                }
                string BanMessaje;
                if (Database.SystemBanned.IsBanned(client.Socket.RemoteIp, out BanMessaje))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        string Messaj = "You IP Address is Banned for: " + BanMessaje + " ";
                        client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));

                    }
                    return;
                }
                else if (Database.SystemBannedPC.IsBanned(client, out BanMessaje))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        client.Send(new MsgServer.MsgMessage(BanMessaje, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                    }
                    return;
                }
                if ((client.ClientFlag & Client.ServerFlag.CreateCharacterSucces) == Client.ServerFlag.CreateCharacterSucces)
                {
                    if (Database.ServerDatabase.AllowCreate(client.ConnectionUID))
                    {
                        client.ClientFlag &= ~Client.ServerFlag.CreateCharacterSucces;
                        if (client.Player.MyChi == null)
                        {
                            client.Player.MyChi = new Role.Instance.Chi(client.Player.UID);

                        }
                        if (client.Player.SubClass == null)
                            client.Player.SubClass = new Role.Instance.SubClass();
                        if (client.Player.Flowers == null)
                        {
                            client.Player.Flowers = new Role.Instance.Flowers(client.Player.UID, client.Player.Name);
                            client.Player.Flowers.FreeFlowers = 1;
                        }
                        if (client.Player.Nobility == null)
                            client.Player.Nobility = new Role.Instance.Nobility(client);
                        if (client.Player.Associate == null)
                        {
                            client.Player.Associate = new Role.Instance.Associate.MyAsociats(client.Player.UID);
                            client.Player.Associate.MyClient = client;
                            client.Player.Associate.Online = true;
                        }
                        if (client.Player.InnerPower == null)
                            client.Player.InnerPower = new Role.Instance.InnerPower(client.Player.Name, client.Player.UID);

                        Pool.GamePoll.TryAdd(client.Player.UID, client);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            client.Send(new MsgServer.MsgMessage("ANSWER_OK", "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));


                            Database.ServerDatabase.CreateCharacte(client);
                            Database.ServerDatabase.SaveClient(client);

                            client.ClientFlag |= Client.ServerFlag.AcceptLogin;

                            MyConsole.WriteLine("[NEW] Account: " + client.Player.Name + " Login.");

                            client.Send(stream.LoginHandlerCreate(1, client.Player.Map));
                            MsgLoginHandler.LoadMap(client, stream);
                        }
                        Program.CallBack.Register(client);
                        return;
                    }
                }
                if ((client.ClientFlag & Client.ServerFlag.AcceptLogin) != Client.ServerFlag.AcceptLogin)
                {

                    var login = client.OnLogin;

                    client.ConnectionUID = login.Key;


                    if (Database.SystemBannedAccount.IsBanned(client.ConnectionUID, out BanMessaje))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
#if Arabic
                             string aMessaj = "Your account is Banned for: " + BanMessaje + " ";
#else
                            string aMessaj = "Your account is Banned for: " + BanMessaje;
#endif

                            client.Send(new MsgServer.MsgMessage(aMessaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                        }
                        return;
                    }


                    string Messaj = "NEW_ROLE";

                    if (Database.ServerDatabase.AllowCreate(login.Key) == false)
                    {

                        Client.GameClient InGame = null;
                        if (Pool.GamePoll.TryGetValue((uint)login.Key, out InGame))
                        {
                            if (InGame.Player != null)
                            {
                                MyConsole.WriteLine("Account: " + InGame.Player.Name + " (Already logged on)!");

                                if (InGame.Player.UID == 0)
                                {
                                    Pool.GamePoll.TryRemove((uint)login.Key, out InGame);
                                    if (InGame != null && InGame.Player != null)
                                    {
                                        if (InGame.Map != null)
                                            InGame.Map.Denquer(InGame);
                                    }
                                }
                            }
                            InGame.Socket.Disconnect();
#if Arabic
                                     Messaj = "Sorry, you Account is online, Try Again";
#else
                            Messaj = "Your account already logged in on another pc, Try again.";
#endif

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                            }
                            if (InGame.TRyDisconnect-- == 0)
                            {
                                if (InGame.Player != null && InGame.FullLoading)
                                {
                                    InGame.ClientFlag |= Client.ServerFlag.Disconnect;
                                    Database.ServerDatabase.SaveClient(InGame);
                                }
                                Pool.GamePoll.TryRemove((uint)login.Key, out InGame);
                                if (InGame != null && InGame.Player != null)
                                {
                                    if (InGame.Map != null)
                                        InGame.Map.Denquer(InGame);
                                }
                            }
                        }
                        else
                        {
                            Pool.GamePoll.TryAdd((uint)login.Key, client);
                            Messaj = "ANSWER_OK";
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                if ((client.ClientFlag & Client.ServerFlag.CreateCharacterSucces) != Client.ServerFlag.CreateCharacterSucces)
                                {
                                    Database.ServerDatabase.LoadCharacter(client, (uint)login.Key);
                                }

                                client.ClientFlag |= Client.ServerFlag.AcceptLogin;
                                MyConsole.WriteLine("Account: " + client.Player.Name + " has been logged in successfully.");
                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));

                                MsgLoginHandler.LoadMap(client, stream);
                                //Database.ServerDatabase.LoginQueue.Enqueue("[Login] Name: " + client.Player.Name + " UID :" + client.Player.UID + " IP: " + client.Socket.RemoteIp + " MacAddress " + client.Socket.GetMACAddress() + " Time " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);

                            }
                            Program.CallBack.Register(client);
                        }
                    }
                    else//new client
                    {
                        //client.Socket.OverrideTiming = true;
                        client.ClientFlag |= Client.ServerFlag.CreateCharacter;
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                        }
                    }
                }
            }

            catch (Exception e) { MyConsole.WriteException(e); }
        }
        public unsafe static void LoginHandlers(Client.GameClient client, MsgLoginClient packet)
        {
            using (var cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("accounts").Where("ID", client.Player.UID))
            using (var rdr = new MySqlReader(cmd))
            {
                if (rdr.Read())
                {
                    client.MacAddress = rdr.ReadString("macs");
                }
            }
            client.ClientFlag &= ~Client.ServerFlag.OnLoggion;

            if (client.Socket != null)
            {
                if (client.Socket.RemoteIp == "NONE")
                {
                    //  MyConsole.WriteLine("Breack login client.");
                    return;
                }
            }
            try
            {
                if (client.OnLogin.Key > 100000000 || client.OnLogin.Key < 1000000)
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        string Messaj = "You can't do this";
                        client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));

                    }
                    return;
                }
                string BanMessaje;
                if (Database.SystemBanned.IsBanned(client.Socket.RemoteIp, out BanMessaje))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();

#if Arabic
                          string Messaj = "You IP Address is Banned for: " + BanMessaje + " ";
#else
                        string Messaj = "You IP Address is Banned for: " + BanMessaje + " ";
#endif

                        client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));

                    }
                    return;




                }
                if ((client.ClientFlag & Client.ServerFlag.CreateCharacterSucces) == Client.ServerFlag.CreateCharacterSucces)
                {
                    if (Database.ServerDatabase.AllowCreate(client.ConnectionUID))
                    {

                        client.ClientFlag &= ~Client.ServerFlag.CreateCharacterSucces;
                        if (client.Player.MyChi == null)
                        {
                            client.Player.MyChi = new Role.Instance.Chi(client.Player.UID);

                        }
                        if (client.Player.SubClass == null)
                            client.Player.SubClass = new Role.Instance.SubClass();
                        if (client.Player.Flowers == null)
                        {
                            client.Player.Flowers = new Role.Instance.Flowers(client.Player.UID, client.Player.Name);
                            client.Player.Flowers.FreeFlowers = 1;
                        }
                        if (client.Player.Nobility == null)
                            client.Player.Nobility = new Role.Instance.Nobility(client);
                        if (client.Player.Associate == null)
                        {
                            client.Player.Associate = new Role.Instance.Associate.MyAsociats(client.Player.UID);
                            client.Player.Associate.MyClient = client;
                            client.Player.Associate.Online = true;
                        }
                        if (client.Player.InnerPower == null)
                            client.Player.InnerPower = new Role.Instance.InnerPower(client.Player.Name, client.Player.UID);

                        Pool.GamePoll.TryAdd(client.Player.UID, client);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            client.Send(new MsgServer.MsgMessage("ANSWER_OK", "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));


                            Database.ServerDatabase.CreateCharacte(client);
                            Database.ServerDatabase.SaveClient(client);

                            client.ClientFlag |= Client.ServerFlag.AcceptLogin;

                            MyConsole.WriteLine("[NEW] Account: " + client.Player.Name + " Login.");
                            //Database.ServerDatabase.LoginQueue.Enqueue("[Login] Name: " + client.Player.Name + " UID :" + client.Player.UID + " IP: " + client.Socket.RemoteIp + " MacAddress " + client.Socket.GetMACAddress() + " Time " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);

                            client.Send(stream.LoginHandlerCreate(1, client.Player.Map));
                            MsgLoginHandler.LoadMap(client, stream);
                        }
                        Program.CallBack.Register(client);
                        return;
                    }
                }

                if ((client.ClientFlag & Client.ServerFlag.AcceptLogin) != Client.ServerFlag.AcceptLogin)
                {

                    var login = client.OnLogin;

                    client.ConnectionUID = login.Key;

                    if (Database.SystemBannedAccount.IsBanned(client.ConnectionUID, out BanMessaje))
                    {

                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
#if Arabic
                             string aMessaj = "Your account is Banned for: " + BanMessaje + " ";
#else
                            string aMessaj = "Your account is Banned for: " + BanMessaje;
#endif

                            client.Send(new MsgServer.MsgMessage(aMessaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                        }
                        return;
                    }

                    string Messaj = "NEW_ROLE";

                    if (Database.ServerDatabase.AllowCreate(login.Key) == false)
                    {

                        Client.GameClient InGame = null;
                        if (Pool.GamePoll.TryGetValue((uint)login.Key, out InGame))
                        {
                            if (InGame.Player != null)
                            {
                                MyConsole.WriteLine("Account: " + InGame.Player.Name + " (Already logged on)!");

                                if (InGame.Player.UID == 0)
                                {
                                    Pool.GamePoll.TryRemove((uint)login.Key, out InGame);
                                    if (InGame != null && InGame.Player != null)
                                    {
                                        if (InGame.Map != null)
                                            InGame.Map.Denquer(InGame);
                                    }
                                }
                            }
                            InGame.Socket.Disconnect();
#if Arabic
                                     Messaj = "Sorry, you Account is online, Try Again";
#else
                            Messaj = "Sorry, you Account is online, Try Again ( next login will lag the account )";
#endif

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                            }
                            if (InGame.TRyDisconnect-- == 0)
                            {
                                if (InGame.Player != null && InGame.FullLoading)
                                {
                                    InGame.ClientFlag |= Client.ServerFlag.Disconnect;
                                    Database.ServerDatabase.SaveClient(InGame);
                                }
                                Pool.GamePoll.TryRemove((uint)login.Key, out InGame);
                                if (InGame != null && InGame.Player != null)
                                {
                                    if (InGame.Map != null)
                                        InGame.Map.Denquer(InGame);
                                }
                            }
                        }
                        else
                        {
                            Messaj = "ANSWER_OK";
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                if ((client.ClientFlag & Client.ServerFlag.CreateCharacterSucces) != Client.ServerFlag.CreateCharacterSucces)
                                {
                                    Database.ServerDatabase.LoadCharacter(client, (uint)login.Key);
                                }
                                client.ClientFlag |= Client.ServerFlag.AcceptLogin;
                                MyConsole.WriteLine("Account: " + client.Player.Name + " has been logged in successfully.");
                                //Database.ServerDatabase.LoginQueue.Enqueue("[Login] Name: " + client.Player.Name + " UID :" + client.Player.UID + " IP: " + client.Socket.RemoteIp + " MacAddress " + client.Socket.GetMACAddress() + " Time " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);

                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));

                                MsgLoginHandler.LoadMap(client, stream);
                            }
                            Program.CallBack.Register(client);
                            Pool.GamePoll.TryAdd((uint)login.Key, client);
                        }
                    }
                    else//new client
                    {
                        //client.Socket.OverrideTiming = true;
                        client.ClientFlag |= Client.ServerFlag.CreateCharacter;
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                        }
                    }
                }
            }
            catch (Exception e) { MyConsole.WriteException(e); }
        }
    }
}