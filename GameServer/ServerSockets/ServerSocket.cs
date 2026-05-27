using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Concurrent;
using VirusX.WindowsAPI;

namespace VirusX.ServerSockets
{
    public class ServerSocket
    {
        private bool Alive = false;
        private Socket Connection;
        public Socket GetConnection
        {
            get { return Connection; }
        }
        public bool IsAlive
        {
            get { return Alive; }
        }
        private string ServerAddresIP = "";
        public uint SPort;
        public string ServerName = "";
        private Action<SecuritySocket> ProcessConnection;
        private Action<SecuritySocket> ProcessDisconnect;
        private Action<SecuritySocket, Packet> ProcessReceive;
        public ServerSocket(Action<SecuritySocket> _processConnection, Action<SecuritySocket, Packet> _procesreceive, Action<SecuritySocket> _processdisconnect)
        {
            ProcessConnection = _processConnection;
            ProcessReceive = _procesreceive;
            ProcessDisconnect = _processdisconnect;
            Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Initilize(ushort MaxBufferSend, ushort MaxBufferReceive, uint MaxAcceptConnections, uint MaxClientsConnections)
        {
            Connection.ReceiveBufferSize = MaxBufferReceive;
            Connection.SendBufferSize = MaxBufferSend;
        }

        public void Connect(string IpAddres, ushort port, string aServerName)
        {
            SPort = port;
            ServerAddresIP = IpAddres;
            ServerName = aServerName;
            TryConnect(aServerName);
        }
        private void TryConnect(string servername)
        {
            Connection.BeginConnect(ServerAddresIP, (int)SPort, new AsyncCallback(ConnectCallback), null);
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Connection.EndConnect(ar);
                Connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                Connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                Connection.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                Connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                Connection.Blocking = false;
                Connection.ReceiveBufferSize = 16384;
                Connection.SendBufferSize = 16384;
                Alive = Connection.Connected;
                if (Alive)
                {
                    var client = new SecuritySocket(this, ProcessDisconnect, ProcessReceive);
                    client.Create(Connection);
                    client.Connection.Blocking = false;
                    if (ProcessConnection != null)
                    {
                        ProcessConnection(client);
                    }
                }
            }
            catch
            {

                TryConnect(ServerName);
            }
        }
        public void Open(string IpAddres, ushort port, int backlog)
        {

            SPort = port;
            ServerAddresIP = IpAddres;
            Connection.Bind(new IPEndPoint(IPAddress.Any, port));
            Connection.Listen(10);
            Connection.Blocking = false;
            Connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Connection.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, true);
            Connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            Connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Alive = true;
        }
        public void Accept()
        {
            try
            {
                if (Alive)
                {
                    if (Connection.Poll(0, SelectMode.SelectRead))
                    {
                        if (Connection.Poll(1, SelectMode.SelectError))
                        {
                            var close_socket = Connection.Accept();
                            WindowsAPI.ws2_32.shutdown(close_socket.Handle, WindowsAPI.ws2_32.ShutDownFlags.SD_BOTH);
                            WindowsAPI.ws2_32.closesocket(close_socket.Handle);
                            MyConsole.WriteLine("[Sockets] Error on socket Accept().");
                            return;
                        }
                        var socket = Connection.Accept();
                        SecuritySocket user = new SecuritySocket(this, ProcessDisconnect, ProcessReceive);
                        user.Create(socket);
                        if (ProcessConnection != null)
                            ProcessConnection.Invoke(user);
                        user.ConnectFull = true;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); MyConsole.SaveException(e); }
        }
        public void CloseNewSocket(Socket socket)
        {
            ws2_32.shutdown(socket.Handle, ws2_32.ShutDownFlags.SD_BOTH);
            ws2_32.closesocket(socket.Handle);
            socket.Dispose();
            GC.SuppressFinalize(socket);
        }
        public void Close()
        {
            if (Alive)
            {
                Alive = false;
                Connection.Close(1);
            }
        }
    }

}