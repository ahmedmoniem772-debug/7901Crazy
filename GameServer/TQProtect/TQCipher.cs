using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CMsgTQProtect
{
    internal class TQCipher
    {
        public static int PublicSeed = 0;
        public TQCipher(bool _IsMahmoudAli = false)
        {
            IsMahmoudAli = _IsMahmoudAli;
            PublicSeed = new System.Random().Next();
            //MsgGuardShield.Set_IV_Keys(PublicSeed);
        }
        public static int HandleBuffer(ref byte[] data, bool isDecrypt)
        {
            // Ensure the data is not null and has a length of at least 4
            if (data == null || data.Length <= 4)
                return 0;

            if (!isDecrypt)
                PadToMultipleOf(ref data);

            int length = System.BitConverter.ToUInt16(data, 0);

            if (data.Length - 8 != length && data.Length != length)
                throw new Exception("Invalid buffer length");

            if ((length - PADDING_SIZE) % 16 != 0)
                throw new Exception("Invalid buffer Padding");


            using (AesManaged aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                aes.KeySize = 128;

                var srand = new srand(TQCipher.PublicSeed);
                byte[] Key1 = new byte[16];
                byte[] Key2 = new byte[16];

                //key 1
                for (int i = 0; i < 16; i++)
                    Key1[i] = (byte)srand.rand();

                //key 2
                for (int i = 0; i < 16; i++)
                    Key2[i] = (byte)srand.rand();

                aes.Key = Key1;
                aes.BlockSize = 128;
                aes.IV = Key2;
                aes.Padding = PaddingMode.PKCS7;

                if ((data.Length  - PADDING_SIZE  - PADDING_REDUCE ) % 16 == 0)
                    aes.Padding = PaddingMode.None;

                byte[] result = isDecrypt
                    ? aes.CreateDecryptor().TransformFinalBlock(data, PADDING_SIZE, data.Length - PADDING_SIZE - PADDING_REDUCE)
                    : aes.CreateEncryptor().TransformFinalBlock(data, PADDING_SIZE, data.Length - PADDING_SIZE - PADDING_REDUCE);

                Buffer.BlockCopy(result, 0, data, PADDING_SIZE, result.Length);

            }
            return data.Length;
        }  
        private static void PadToMultipleOf(ref byte[] src, int pad = PADDING_SIZE * 4)
        {
            int OldLength = src.Length - PADDING_SIZE - PADDING_REDUCE;
            if (OldLength % pad == 0)
            {
                return;
            }
            int len = (src.Length + pad - 1) / pad * pad;
            Array.Resize(ref src, len + PADDING_REDUCE + PADDING_SIZE);
            unsafe
            {
                fixed (byte* Buffer = src)
                    *((ushort*)(Buffer + 0)) = (ushort)(src.Length - PADDING_REDUCE);
            }
        }

        private static bool IsMahmoudAli = false;
        private  const byte PADDING_SIZE = 4;
        private static readonly int TQSeal_SIZE = 8;
        private static int PADDING_REDUCE
        {
            get { return IsMahmoudAli ? TQSeal_SIZE : 0; }
        }
    }
    public class srand
    {
        private int _seed = 0;
        public short rand()
        {
            _seed *= 0x343fd;
            _seed += 0x269ec3;
            return (short)((_seed >> 0x10) & 0x7fff);
        }
        public srand(int seed)
        {
            _seed = seed;
        }
    }
}
