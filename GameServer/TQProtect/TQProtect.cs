using MahmoudAli.Client;
using MahmoudAli.Game.MsgServer;
using MahmoudAli.ServerSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace CMsgTQProtect
{
    public class TQProtect
    {
        private GameClient client = null;
        private DateTime PingStamp = new DateTime();
        private DateTime PingPacketStamp = new DateTime();
        public DateTime TaskProcess = new DateTime();

        private uint PingValue = 0;
        private uint PingValue_2 = 0;
        private int PrivOwnerSeed = 0;


        public bool ForceQuery = false;
        public bool RequestProcess = false;
        public int StampCheckMinutes = 5;
        public int StampCheckCounter = 0;

        public TQProtect(GameClient _client)
        {
            client = _client;
            PingStamp = DateTime.Now;
            PingPacketStamp = DateTime.Now;
            TaskProcess = DateTime.Now;
            PrivOwnerSeed = new Random().Next() ^ 0x514E;
            PingValue = (uint)new Random().Next(1, 0x1000);
            PingValue_2 = (uint)new Random((int)this.PingValue).Next(1, 0x1000);
            _CreateConnect();
        }
        public enum _MSG_DATA_TYPE : ushort
        {
            CreateLogin = 900,
            CreateExchangeKey = 901,
            CreateMachineInfo = 902,
            CreateProcessRunning = 903,
            TerminateClient = 904,
        }
        public byte[] HandleOwner(byte[] data)
        {
            var srand = new srand(PrivOwnerSeed);
            byte[] Key1 = new byte[128];
            byte[] Key2 = new byte[128];

            //key 1
            for (int i = 0; i < Key1.Length; i++)
            {
                Key1[i] = (byte)srand.rand();
                Key1[i] = (byte)(i ^ Key1[i & 0x5]);
            }
            //key 2
            for (int i = 0; i < Key2.Length; i++)
            {
                Key2[i] = (byte)srand.rand();
                Key2[i] = (byte)(i ^ Key2[i & 0x8]);
            }
            //handle
            for (int x = 4; x < data.Length; x++)
            {
                data[x] = (byte)((byte)(Key1[44 * x & 28] ^ data[x]));
                data[x] = (byte)((byte)(Key2[99 * x & 31] ^ data[x]));
            }
            return data;
        }

      

        internal void _CreateConnect()
        {
            CreateDataPacket((ushort)_MSG_DATA_TYPE.CreateLogin, new uint[] { (uint)TQCipher.PublicSeed, (uint)PrivOwnerSeed });
        }

        void CreateDataPacket(ushort _type, params uint[] ints)
        {
            using (var rec = new RecycledPacket())
            {
                var stream = rec.GetStream();
                ActionQuery _action = new ActionQuery();
                _action.ObjId = client.Player.UID;
                _action.Type = (ushort)_type;
                if (ints.Length > 0)
                    _action.dwParam = ints[0];
                if (ints.Length > 1)
                    _action.dwParam2 = (int)ints[1];
                if (ints.Length > 2)
                    _action.dwParam3 = ints[2];
              
                client.Send(stream.ActionCreate(_action));
            }
        }

        internal void Disconnect(string reason = "")
        {
            if (!string.IsNullOrEmpty(reason))
                Console.WriteLine("[TQProtect] KICKOUT Name:{0} , {1}", client.Player.Name, reason);
            client.Socket.Disconnect();
        }


        internal void TerminateLoader()
        {
            CreateDataPacket((ushort)_MSG_DATA_TYPE.TerminateClient);
        }

        internal void RequestMachineInfo()
        {
            CreateDataPacket((ushort)_MSG_DATA_TYPE.CreateMachineInfo);
        }
        internal void RequestOpenedProcesses()
        {
            CreateDataPacket((ushort)_MSG_DATA_TYPE.CreateProcessRunning);
        }

    }
}
