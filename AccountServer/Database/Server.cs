// * Created by AccountServer
// * Copyright © 2020-2021
// * AccountServer - Project

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AccountServer.Database
{
    public unsafe class ServerInfo
    {
        public string Name;
        public string IP;
        public ushort Port;
        public string TransferKey;
        public string TransferSalt;
    }
    public unsafe class Server
    {
        public static Dictionary<string, ServerInfo> Servers = new Dictionary<string, ServerInfo>();
        public static void Load()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("");
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    LOADING SERVER LIST                      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

            using (var cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("Servers"))
            using (var reader = cmd.CreateReader())
            {
                int count = 0;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("");
                Console.WriteLine($"{"Server Name",-25} {"IP Address",-20} {"Port",-8}");
                Console.WriteLine(new string('-', 55));
                Console.ForegroundColor = ConsoleColor.Gray;

                while (reader.Read())
                {
                    ServerInfo serverinfo = new ServerInfo();
                    serverinfo.Name = reader.ReadString("Name");
                    serverinfo.IP = reader.ReadString("IP");
                    serverinfo.Port = reader.ReadUInt16("Port");
                    Servers.Add(serverinfo.Name, serverinfo);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{serverinfo.Name,-25} {serverinfo.IP,-20} {serverinfo.Port,-8}");
                    count++;
                }

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(new string('-', 55));
                Console.WriteLine($"Total Servers Loaded: {count}");
                Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    SERVER LIST LOADED                      ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
