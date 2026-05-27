using System.Text;

namespace AccountServer.Network.Cryptography
{
    public unsafe class LoaderEncryption
    {
        private static byte[] Key1 = Encoding.Default.GetBytes("f0HOXPCZo6pB8K3YVA3QYldAeGoW9mCC");
        private static byte[] Key2 = Encoding.Default.GetBytes("jeiQoKKV7KmZ61Ss047mvmeYasTQiTM4");
        public static string Decrypt(byte[] data, int Length)
        {
            for (int x = 0; x < Length; x++)
            {
                data[x] ^= Key1[x % Length];
                data[x] ^= Key1[(x * 24 % 32) % Length];
                data[x] ^= Key2[(x * 48 % 64) % Length];
            }
            string pass = Encoding.Default.GetString(data).Replace("\0", "");
            return pass;
        }
    }
}