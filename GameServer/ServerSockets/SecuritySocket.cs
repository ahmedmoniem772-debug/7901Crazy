using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using VirusX.WindowsAPI;

namespace VirusX.ServerSockets
{

    public unsafe class SecuritySocket
    {
        public ReceiveBuffer ReceiveBuff;
        public bool IsVirusX
        {
            get
            {
                return Crypto != null;
            }
        }
        public bool SetDHKey = false;
        public object SendRoot;
        public object SynRoot = new object();
        public Action<SecuritySocket> OnDisconnect;
        private Action<SecuritySocket, Packet> OnReceiveHandler;
        public Socket Connection;
        public object Client;
        public bool Alive = false;
        public Cryptography.TQCast5 Crypto;
        public bool OnInterServer = false;
        public string RemoteIp
        {
            get
            {
                try
                {
                    if (Connection == null)
                        return "NONE";
                    return (Connection.RemoteEndPoint as IPEndPoint).Address.ToString();
                }
                catch
                {
                    return "NONE";
                }
            }
        }
        private Queue<byte[]> OnSend;
        public bool RequestDisFinishSend = false;
        public bool OverrideTiming = false;
        public Time32 LastReceive, LastReceiveCall;
        public Client.GameClient Game;
        public ServerSocket Server;
        public bool ConnectFull = false;
        private IDisposable[] TimerSubscriptions = null;
        public SecuritySocket(ServerSocket serversocket, Action<SecuritySocket> _OnDisconnect, Action<SecuritySocket, Packet> _OnReceiveHandler)
        {
            Server = serversocket;
            OnReceiveHandler = _OnReceiveHandler;
            OnDisconnect = _OnDisconnect;
        }
        public SecuritySocket(Action<SecuritySocket> _OnDisconnect, Action<SecuritySocket, Packet> _OnReceiveHandler)
        {
            OnReceiveHandler = _OnReceiveHandler;
            OnDisconnect = _OnDisconnect;
        }
        public bool Connect(string IPAddres, ushort port, out Socket _socket)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult asyncResult = _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(IPAddres), port), null, null);
            uint count = 0;
            while (!asyncResult.IsCompleted && count < 10)
            {
                count++;
                System.Threading.Thread.Sleep(100);
            }
            if (asyncResult.IsCompleted)
            {
                _socket.Blocking = false;
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                _socket.ReceiveBufferSize = _socket.SendBufferSize = ServerSockets.ReceiveBuffer.RECV_BUFFER_SIZE;
            }
            return !((_socket.Poll(1000, SelectMode.SelectRead) && (_socket.Available == 0)) || !_socket.Connected);
        }
        internal bool CheckTerminateTimers()
        {
            if (TimerSubscriptions != null && !Alive)
            {
                foreach (var Now in TimerSubscriptions)
                    Now.Dispose();
                TimerSubscriptions = null;
                return true;
            }
            return false;
        }
        public void Create(Socket _socket)
        {
            try
            {

                ReceiveBuff = new ReceiveBuffer(Program.ServerConfig.Port_ReceiveSize);
                Connection = _socket;
                SetDHKey = false;
                Alive = true;
                LastReceive = Time32.Now;
                SendRoot = new object();
                OnSend = new Queue<byte[]>();
                Client = Crypto = null;
                TimerSubscriptions = new IDisposable[]
    {
                ThreadPoll.Subscribe<SecuritySocket>(ThreadPoll.ConnectionReceive, this, ThreadPoll.ReceivePool),
                ThreadPoll.Subscribe<SecuritySocket>(ThreadPoll.ConnectionSend, this, ThreadPoll.SendPool),
                ThreadPoll.Subscribe<SecuritySocket>(ThreadPoll.ConnectionReview, this, ThreadPoll.GenericThreadPool)
    };
            }
            catch (Exception e)
            {
                MyConsole.SaveException(e);
            }
        }
        public void SetCrypto(Cryptography.TQCast5 Crypt)
        {
            if (!Program.ServerConfig.IsInterServer)
            {
                Crypto = Crypt;
            }
        }
        public void CheckUp()
        {
            if (CheckTerminateTimers())
                return;
            if (Alive)
            {
                if (Time32.Now < LastReceiveCall.AddSeconds(2))
                    if (Time32.Now > LastReceive.AddSeconds(60))
                        Disconnect();
            }
        }
        public unsafe void Send(Packet msg)
        {
            try
            {
                if (Alive)
                {
                    lock (SendRoot)
                    {
                        byte[] _buffer = new byte[msg.Size];
                        if (Crypto != null)
                            Crypto.Encrypt(msg.Memory, 0, _buffer, 0, msg.Size);
                        else
                            fixed (byte* ptr = _buffer)
                                msg.memcpy(ptr, msg.Memory, msg.Size);
                        OnSend.Enqueue(_buffer);
                    }
                }
            }
            catch (SocketException)
            {
                Disconnect();
            }
            catch (Exception e)
            {
            }
        }
        public bool CanDequeue(out byte[] data)
        {
            data = null;
            try
            {
                if (OnSend.Count > 0)
                {
                    lock (SendRoot)
                        data = OnSend.Dequeue();
                }
                else if (Alive && RequestDisFinishSend)
                    Disconnect();
            }
            catch (Exception e)
            {
                Disconnect();
                MyConsole.WriteException(e);
            }
            return data != null;
        }
        public int nPutBytes = 0;
        public byte[] packet = null;
        public static bool TrySend(SecuritySocket wrapper)
        {
            if (wrapper.Alive)
            {
                if (!wrapper.ConnectFull)
                    return false;

                //if (wrapper.OnSend.Count > 3500)
                //{
                //    Console.WriteLine("the sync is " + wrapper.OnSend.Count + "");
                //    wrapper.OnSend.Clear();
                //    wrapper.Disconnect();
                //    return false;
                //}

                SocketError sError = SocketError.Success;
                int ret = 0;
                if (wrapper.packet != null)
                {
                    try
                    {
                        int nLen = wrapper.packet.Length;
                        ret = wrapper.Connection.Send(wrapper.packet, wrapper.nPutBytes, nLen - wrapper.nPutBytes, SocketFlags.None, out sError);
                        if (ret > 0)
                        {
                            wrapper.nPutBytes += ret;
                            if (wrapper.nPutBytes >= nLen)
                            {
                                wrapper.nPutBytes = 0;
                                wrapper.packet = null;
                            }
                            else
                            {
                                if (sError != SocketError.WouldBlock)
                                {
                                    wrapper.Disconnect();
                                    return false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                        else if (ret <= 0)
                        {
                            if (sError != SocketError.WouldBlock)
                            {
                                wrapper.Disconnect();
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        wrapper.Disconnect();
                        return false;
                    }
                }
                if (wrapper.CanDequeue(out wrapper.packet))
                {
                    try
                    {
                        int nLen = wrapper.packet.Length;
                        ret = wrapper.Connection.Send(wrapper.packet, wrapper.nPutBytes, nLen - wrapper.nPutBytes, SocketFlags.None, out sError);
                        if (ret > 0)
                        {
                            wrapper.nPutBytes += ret;
                            if (wrapper.nPutBytes < wrapper.packet.Length)
                            {
                                if (sError != SocketError.WouldBlock)
                                {
                                    wrapper.Disconnect();
                                    return false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                wrapper.packet = null;
                                wrapper.nPutBytes = 0;
                                return true;
                            }
                        }
                        else
                        {
                            if (sError != SocketError.WouldBlock)
                            {
                                wrapper.Disconnect();
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        wrapper.Disconnect();
                        return false;
                    }
                }
            }
            return false;
        }
        public ReceiveBuffer DHKeyBuffer = new ReceiveBuffer(1024, true);
        public ReceiveBuffer EncryptedDHKeyBuffer = new ReceiveBuffer(1024, true);
        public bool IsCompleteDHKey(out int type)
        {
            type = 0;
            try
            {
                if (DHKeyBuffer.Length() < 8)
                    return false;
                byte[] buffer = new byte[Packet.SealSize];
                for (int x = 0; x < buffer.Length; x++)
                    buffer[x] = DHKeyBuffer.buffer[x + (DHKeyBuffer.Length() - Packet.SealSize)];
                string Text = System.Text.ASCIIEncoding.ASCII.GetString(DHKeyBuffer.buffer);
                bool accept = Text.Contains("TQClient");
                if (!Text.EndsWith("TQClient"))
                    type = 1;
                return accept;
            }
            catch (Exception e)
            {
                MyConsole.SaveException(e);
                Disconnect();
                return false;
            }
        }
        public unsafe bool ReceiveDHKey()
        {
            try
            {
                if (Alive)
                {
                    int rec_type = 0;
                    if (!SetDHKey && Alive)
                    {
                        SocketError Socket_Error = SocketError.IsConnected;
                        int length = DHKeyBuffer.MaxLength() - DHKeyBuffer.Length();
                        if (length <= 0)
                        {
                            Disconnect();
                            return false;
                        }
                        int ret = Connection.Receive(DHKeyBuffer.buffer, DHKeyBuffer.Length(), length, SocketFlags.None, out Socket_Error);
                        if (ret > 0)
                        {
                            Buffer.BlockCopy(DHKeyBuffer.buffer, DHKeyBuffer.Length(), EncryptedDHKeyBuffer.buffer, EncryptedDHKeyBuffer.Length(), ret);
                            EncryptedDHKeyBuffer.AddLength(ret);
                            ushort PackLength = BitConverter.ToUInt16(DHKeyBuffer.buffer, 0);
                            ushort ID = BitConverter.ToUInt16(DHKeyBuffer.buffer, 2);
                            if (ID == 2562)
                                return false;
                            if (Crypto != null)
                            {
                                fixed (byte* ptr = DHKeyBuffer.buffer)
                                    Crypto.Decrypt(DHKeyBuffer.buffer, DHKeyBuffer.Length(), ptr, DHKeyBuffer.Length(), ret);
                            }
                            DHKeyBuffer.AddLength(ret);
                            if (IsCompleteDHKey(out rec_type))
                            {
                                using (var rec = new RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    stream.Seek(0);
                                    fixed (byte* ptr = DHKeyBuffer.buffer)
                                        stream.memcpy(stream.Memory, ptr, DHKeyBuffer.Length());
                                    stream.Size = DHKeyBuffer.Length();
                                    if (OnReceiveHandler != null)
                                        OnReceiveHandler.Invoke(this, stream);
                                }
                            }
                        }
                        else if (ret == 0)
                        {
                            if (Socket_Error != SocketError.WouldBlock)
                                Disconnect();
                            return false;
                        }
                        else
                        {
                            if (Socket_Error != SocketError.WouldBlock)
                                Disconnect();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Disconnect();
            }
            return false;
        }
        public bool ReceiveBuffer()
        {
            if (CheckTerminateTimers())
                return false;
            if (Alive)
            {
                LastReceiveCall = Time32.Now;
                try
                {
                    if (!ConnectFull)
                        return false;
                    if (!SetDHKey && Crypto != null)
                    {
                        ReceiveDHKey();
                    }
                    try
                    {
                        if (!Alive)
                            return false;

                        if (!Receive())
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        MyConsole.SaveException(e);
                        Disconnect();
                    }
                    return true;
                }
                catch (Exception e) { Disconnect(); Console.WriteLine(e.ToString()); }
            }
            return false;
        }
        public unsafe bool HandlerBuffer()
        {
            int counts = 30;
            while (true && counts > 0)
            {
                counts--;
                if (!Alive)
                    return false;
                try
                {
                    ushort _pid = BitConverter.ToUInt16(ReceiveBuff.buffer, 2);
                    int Length = ReceiveBuff.Length();
                    if (_pid != 1774)
                    {
                        if (!ConnectFull)
                            return false;
                        if (ReceiveBuff.Length() == 0)
                            return false;
                        Length = (int)(ReceiveBuff.ReadHead() + (IsVirusX ? 8 : 0));
                        if (Program.ServerConfig.IsInterServer || OnInterServer)
                            Length += 8;
                        if (Length < 2)
                            return false;
                        if (Length > ServerSockets.ReceiveBuffer.HeadSize)
                        {
                            Disconnect();
                            return false;
                        }
                        if (Length > ReceiveBuff.Length())
                            return false;

                    }
                    LastReceive = Time32.Now;
                    Packet Stream = PacketRecycle.Take();
                    Stream.Seek(0);
                    fixed (byte* ptr = ReceiveBuff.buffer)
                    {
                        Stream.memcpy(Stream.stream, ptr, Length);
                        if (Length < ReceiveBuff.Length())
                        {
                            fixed (void* next_buffer = &ReceiveBuff.buffer[Length])
                                Stream.memcpy(ptr, next_buffer, ReceiveBuff.Length() - Length);
                        }
                        Stream.Size = Length;
                        ReceiveBuff.DelLength(Length);
                    }
                    Stream.SeekForward(2);
                    if (OnReceiveHandler != null)
                        OnReceiveHandler.Invoke(this, Stream);
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
            }
            return false;
        }
        public bool Receive()
        {
            if (Alive)
            {
                SocketError Socket_Error = SocketError.IsConnected;
                try
                {
                    int length = ReceiveBuff.MaxLength() - ReceiveBuff.Length();
                    int ret = Connection.Receive(ReceiveBuff.buffer, ReceiveBuff.Length(), length, SocketFlags.None, out Socket_Error);
                    ushort _pid = BitConverter.ToUInt16(ReceiveBuff.buffer, 2);
                    if (ret > 0)
                    {
                        if (Crypto != null && _pid != 1774)
                        {
                            fixed (byte* ptr1 = ReceiveBuff.buffer)
                                Crypto.Decrypt(ReceiveBuff.buffer, ReceiveBuff.Length(), ptr1, ReceiveBuff.Length(), ret);
                            var ptr = new byte[ret];
                            Buffer.BlockCopy(ReceiveBuff.buffer, ReceiveBuff.Length(), ptr, 0, ret);
                            if (Program.packetsForm != null)
                            {
                                Program.packetsForm.AddPacketToClient(ReceiveBuff.buffer);
                            }
                        }

                        ReceiveBuff.AddLength(ret);
                        return true;
                    }
                    else if (ret == 0)
                    {
                        if (Socket_Error != SocketError.WouldBlock)
                            Disconnect();
                        return false;
                    }
                    else
                    {
                        if (Socket_Error != SocketError.WouldBlock)
                            Disconnect();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(Socket_Error.ToString());

                }
            }
            return false;
        }
        public void Disconnect()
        {
            lock (SynRoot)
            {
                if (Alive)
                {
                    Alive = false;
                    OnSend.Clear();
                    if (TimerSubscriptions != null)
                    {
                        for (int i = 0; i < TimerSubscriptions.Length; i++)
                            if (TimerSubscriptions[i] != null)
                                TimerSubscriptions[i].Dispose();
                    }
                    try
                    {
                        WindowsAPI.ws2_32.shutdown(Connection.Handle, WindowsAPI.ws2_32.ShutDownFlags.SD_BOTH);
                        WindowsAPI.ws2_32.closesocket(Connection.Handle);
                        Connection.Dispose();
                        GC.SuppressFinalize(Connection);
                    }
                    catch (Exception e)
                    {
                        MyConsole.SaveException(e);
                    }
                    finally
                    {
                        if (OnDisconnect != null)
                            OnDisconnect.Invoke(this);
                    }
                }
            }
        }
    }

}