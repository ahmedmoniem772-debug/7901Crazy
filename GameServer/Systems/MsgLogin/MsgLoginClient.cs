using VirusX.DBFunctionality;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public struct MsgLoginClient
    {

        public ushort Length;//0
        public ushort ID;//2
        public uint Key, keyax;//4
        public ulong AccountHash, accountHash;//8
        public string UserName;//12
        public string PassWord;//44
        public string ServerName;//76
        public uint HDSeriala;//108
        public string HDSerial;//108
        public string MachineName;//208
        public string MacAddress;//308
        public string MacAddress2;//326
        public string HWID;//344
        public string ProcessorID;//444
        public string MotherboredID;//544
        public string Language;
        public string LanguageStr;
        public static byte[] buffer;

        private static readonly uint[] ObfuscatedKeys =
 {
    409712393u,
    79783111u,
    1357661211u,
    325727974u
};

        private static readonly uint[] KeyMasks =
        {
    0xA5A5A5A5u,
    0x5A5A5A5Au,
    0x12345678u,
    0xCAFEBABEu
};

        public static uint DecryptIdentifier(uint encryptedIdentifier)
        {
            for (int i = 0; i < ObfuscatedKeys.Length; i++)
            {
                encryptedIdentifier ^= (ObfuscatedKeys[i] ^ KeyMasks[i]);
            }
            return encryptedIdentifier;
        }


        [PacketAttribute((ushort)10055)]
        public unsafe static void LoginGame(Client.GameClient client, ServerSockets.Packet packet)
        {
            byte[] buffer = new byte[packet.Size];
            fixed (byte* ptr = buffer)
            {
                packet.memcpy(ptr, packet.Memory, packet.Size);
            }
            packet.Seek(4);

            client.OnLogin = new MsgLoginClient()
            {
                Key = packet.ReadUInt32(),
            };
            client.ClientFlag |= Client.ServerFlag.OnLoggion;
            Database.ServerDatabase.LoginQueue.TryEnqueue(client);
        }
        public unsafe static void LoginHandler(Client.GameClient client, MsgLoginClient packet)
        {
            client.ClientFlag &= ~Client.ServerFlag.OnLoggion;
            client.LoginIP = client.Socket.RemoteIp;
            client.LoginMAC = client.OnLogin.MacAddress;
            if (client.Socket != null)
            {
                if (client.Socket.RemoteIp == "NONE")
                {

                    return;
                }
            }
            try
            {
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
                //int Count = Pool.GamePoll.Values.Count(p => p.OnLogin.HWID == client.OnLogin.HWID);
                //if (Count > 3)
                //{
                //    using (var rec = new ServerSockets.RecycledPacket())
                //    {
                //        var stream = rec.GetStream();
                //        client.Send(new MsgServer.MsgMessage("You can't have more than 3 active users", "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                //    }
                //    return;
                //}
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

                            MyConsole.WriteLine($"[NEW] {client.Player.Name} joined | {client.Socket.RemoteIp} | {client.OnLogin.MacAddress}", ConsoleColor.Blue);

                            client.Send(stream.LoginHandlerCreate(1, client.Player.Map));
                            MsgLoginHandler.LoadMap(client, stream);
                        }
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
                                MyConsole.WriteLine("Account: " + InGame.Player.Name + " (Already logged on)!", ConsoleColor.Red);

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
                                
                                client.LoginIP = client.Socket.RemoteIp;
                                client.LoginMAC = client.OnLogin.MacAddress;
                                MyConsole.WriteLine($"[+] {client.Player.Name} logged in | {client.Socket.RemoteIp} | {client.OnLogin.MacAddress}", ConsoleColor.Green);
                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));

                                MsgLoginHandler.LoadMap(client, stream);
                                //Database.ServerDatabase.LoginQueue.Enqueue("[Login] Name: " + client.Player.Name + " UID :" + client.Player.UID + " IP: " + client.Socket.RemoteIp + " MacAddress " + client.Socket.GetMACAddress() + " Time " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);

                            }
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
                           
                            client.LoginIP = client.Socket.RemoteIp;
                            client.LoginMAC = client.OnLogin.MacAddress;
                            MyConsole.WriteLine($"[+] {client.Player.Name} logged in | {client.Socket.RemoteIp} | {client.OnLogin.MacAddress}", ConsoleColor.Green);

                            client.Send(stream.LoginHandlerCreate(1, client.Player.Map));
                            MsgLoginHandler.LoadMap(client, stream);
                        }
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
                                MyConsole.WriteLine("Account: " + InGame.Player.Name + " (Already logged on)!", ConsoleColor.Red);

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
                                client.LoginIP = client.Socket.RemoteIp;
                                client.LoginMAC = client.OnLogin.MacAddress;
                                client.ClientFlag |= Client.ServerFlag.AcceptLogin;
                                MyConsole.WriteLine($"[{DateTime.Now:HH:mm:ss}] 🔓 {client.Player.Name} logged in | {client.LoginIP} | {client.LoginMAC}", ConsoleColor.Green); client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                                //Database.ServerDatabase.LoginQueue.Enqueue("[Login] Name: " + client.Player.Name + " UID :" + client.Player.UID + " IP: " + client.Socket.RemoteIp + " MacAddress " + client.Socket.GetMACAddress() + " Time " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);

                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));

                                MsgLoginHandler.LoadMap(client, stream);
                            }
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