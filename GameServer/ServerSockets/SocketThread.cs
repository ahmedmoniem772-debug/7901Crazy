using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.ServerSockets
{
    public static class SocketThread
    {
        const int SOCKET_PROCESS_INTERVAL = 1;
        private static ServerSocket[] Sockets;
        public static void Creates(string GroupName, params ServerSocket[] _Sockets)
        {
            Sockets = _Sockets;
            var ThreadItem = new Extensions.ThreadGroup.ThreadItem(SOCKET_PROCESS_INTERVAL, GroupName, Process);
            ThreadItem.Open();
        }
        public static void Process()
        {
            try
            {
                foreach (var _socket in Sockets)
                {
                    try
                    {
                        if (_socket == null)//for inter server
                            continue;

                        _socket.Accept();
                    }
                    catch (Exception e)
                    {
                        MyConsole.SaveException(e);
                    }
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine(ex.ToString());
            }
        }


    }
}