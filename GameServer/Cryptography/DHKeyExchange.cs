using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Cryptography
{
    public unsafe static class DHKeyExchange
    {
        public unsafe static bool GetHandshakeReplyKey2(this ServerSockets.Packet msg, out string key)
        {
            key = null;
            int SERVER_PAD_SIZE = 11, trycount = 0;
        Agen:
            msg.Seek(SERVER_PAD_SIZE);
            int offset = msg.ReadInt32() + 4 + SERVER_PAD_SIZE;
            if (offset > 0 && offset < msg.Size)
            {
                msg.Seek(offset);
                int nSize = msg.ReadInt32();
                if (nSize > 0 && nSize < msg.Size - offset)
                {
                    key = msg.ReadCString(nSize);
                }
            }
            if (key == null && trycount == 0)
            {
                SERVER_PAD_SIZE = 27;
                trycount = 1;
                goto Agen;
            }
            return (key != null);
        }
        public unsafe static bool GetHandshakeReplyKey(this ServerSockets.Packet msg, bool BlockPacket, out string key)
        {
            key = null;
            int midokey = BlockPacket ? 27 : 11;
            msg.Seek(midokey);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var streamssss = rec.GetStream();
                streamssss = msg;
                int m = streamssss.ReadInt32();
                int offset = m + 4 + midokey;
                if (offset > 0 && offset < msg.Size)
                {
                    msg.Seek(offset);
                    int nSize = msg.ReadInt32();
                    if (nSize > 0 && nSize < msg.Size - offset)
                    {
                        key = msg.ReadCString(nSize);
                    }
                }
            }
            return (key != null);
        }
        public class KeyExchange
        {
          
            public static string Str_P = "E7A69EBDF105F2A6BBDEAD7E798F76A209AD73FB466431E2E7352ED262F8C558F10BEFEA977DE9E21DCEE9B04D245F300ECCBBA03E72630556D011023F9E857F";
            public static string Str_G = "05";

            public static byte[] P;
            public static byte[] G;

            public static void CreateKeys()
            {
                P = Encoding.Default.GetBytes(Str_P);
                G = Encoding.Default.GetBytes(Str_G);
            }
        }
       

        public class ServerKeyExchange
        {
        
            public ServerSockets.Packet CreateServerKeyPacket(Cryptography.DiffieHellman user_dh)
            {

                byte[] _key = Encoding.ASCII.GetBytes(user_dh.GenerateRequest());

                using (var rec = new ServerSockets.RecycledPacket())
                    return rec.GetStream().Handshake(KeyExchange.P, KeyExchange.G, _key);
            }

            private string Hex(byte[] bytes)
            {
                char[] c = new char[bytes.Length * 2];
                byte b;
                for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
                {
                    b = ((byte)(bytes[bx] >> 4));
                    c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
                    b = ((byte)(bytes[bx] & 0x0F));
                    c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
                }
                return new string(c);
            }
            public byte[] PostProcessDHKey(byte[] key)
            {
                var hashService = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var s1 = Hex(hashService.ComputeHash(key, 0, FixKey(key)));
                var s2 = Hex(hashService.ComputeHash(Encoding.ASCII.GetBytes(String.Concat(s1, s1))));
                var sresult = String.Concat(s1, s2);

                return GetArrayPostProcessDHKey(sresult);
            }
            public byte[] GetArrayPostProcessDHKey(string sresult)
            {
                byte[] skey = new byte[sresult.Length];
                for (int x = 0; x < sresult.Length; x++)
                    skey[x] = (byte)sresult[x];
                return skey;
            }
            public int FixKey(byte[] key)
            {
                for (int x = 0; x < key.Length; x++)
                {
                    if (key[x] == 0)
                        return x;
                }
                return key.Length;
            }
            
         
        }
    }
}
