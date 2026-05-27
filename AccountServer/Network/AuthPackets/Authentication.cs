/*using System;
using System.IO;
using System.Text;
using AccountServer.Network.Cryptography;

namespace AccountServer.Network.AuthPackets
{
    public unsafe class Authentication : Interfaces.IPacket
    {
        public string Username;
        public string Password;
        public string Server;
        public string Key;
        public Authentication()
        {

        }
        public void Deserialize(byte[] buffer)
        {
            try
            {
                ushort length = BitConverter.ToUInt16(buffer, 0);
                if (length == 472)
                {
                    ushort type = BitConverter.ToUInt16(buffer, 2);
                    byte[] temp = new byte[16];
                    if (type == 1942)
                    {
                        MemoryStream MS = new MemoryStream(buffer);
                        BinaryReader BR = new BinaryReader(MS);
                        BR.ReadUInt16();
                        BR.ReadUInt16();
                        byte UserLen = BR.ReadByte();
                        byte PwLen = BR.ReadByte();
                        byte ServerLen = BR.ReadByte();
                        ushort serial = BR.ReadUInt16();
                        BR.ReadByte();
                        Username = Encoding.Default.GetString(BR.ReadBytes(UserLen));
                        Username = Username.Replace("\0", "");
                        byte Size = (byte)(PwLen);
                        byte[] passord = new byte[PwLen];
                        passord = BR.ReadBytes(PwLen);
                        Password = Encoding.Default.GetString(passord).Replace("\0", "");
                        Server = Encoding.Default.GetString(BR.ReadBytes(ServerLen));
                        Server = Server.Replace("\0", "");
                        BR.Close();
                        MS.Close();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Invalid login packet.");
            }
        }
        public unsafe byte[] ToArray()
        {
            throw new NotImplementedException();
        }
    }
}*/

using System;
using System.IO;
using System.Text;
using AccountServer.Network.Cryptography;

namespace AccountServer.Network.AuthPackets
{
    public unsafe class Authentication : Interfaces.IPacket
    {
        public string Username;
        public string Password;
        public string Server;
        public string Key;
        public Authentication()
        {

        }
        public void Deserialize(byte[] buffer)
        {
            try
            {
                ushort length = BitConverter.ToUInt16(buffer, 0);
                if (length == 472)
                {
                    ushort type = BitConverter.ToUInt16(buffer, 2);
                    byte[] temp = new byte[16];
                    if (type == 1942)
                    {
                        MemoryStream MS = new MemoryStream(buffer);
                        BinaryReader BR = new BinaryReader(MS);
                        BR.ReadUInt16();
                        BR.ReadUInt16();
                        byte UserLen = BR.ReadByte();
                        byte PwLen = BR.ReadByte();
                        byte ServerLen = BR.ReadByte();
                        ushort serial = BR.ReadUInt16();
                        BR.ReadByte();
                        Username = Encoding.Default.GetString(BR.ReadBytes(UserLen));
                        Username = Username.Replace("\0", "");
                        Password = Encoding.Default.GetString(BR.ReadBytes(PwLen));
                        Password = Password.Replace("\0", "");
                        Server = Encoding.Default.GetString(BR.ReadBytes(ServerLen));
                        Server = Server.Replace("\0", "");
                        BR.Close();
                        MS.Close();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Invalid login packet.");
            }
        }
        public unsafe byte[] ToArray()
        {
            throw new NotImplementedException();
        }
    }
}