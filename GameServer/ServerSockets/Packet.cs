using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace VirusX.ServerSockets
{
    public unsafe class Packet : IDisposable
    {
        public const int MAX_SIZE = 16384;
        private const int TQ_SEALSIZE = 8;

        private static string seal;

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void* memset(void* dst, int val, int count);

        public static string SealString
        {
            get { return seal; }
            set
            {
                if (value == null || value.Length != TQ_SEALSIZE)
                    throw new ArgumentOutOfRangeException("SealString");
                seal = value;
            }
        }

        public static int SealSize
        {
            get { return seal != null ? TQ_SEALSIZE : 0; }
        }

        public int Size { get; set; }

        public byte* Memory;
        public byte* stream;

        private bool IsDisposed;

        public int Position
        {
            get { return (int)(stream - Memory); }
        }

        /* ===================== CONSTRUCTORS ===================== */

        public Packet(int size)
        {
            Memory = (byte*)Marshal.AllocHGlobal(size);
            stream = Memory;
            Size = 0;
        }

        public Packet()
            : this(MAX_SIZE)
        {
        }

        public Packet(byte[] buffer)
        {
            Memory = (byte*)Marshal.AllocHGlobal(MAX_SIZE);
            stream = Memory;
            Marshal.Copy(buffer, 0, (IntPtr)Memory, buffer.Length);
            Size = buffer.Length;
        }

        /* ===================== DISPOSE ===================== */

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;

            if (Memory != null)
            {
                Marshal.FreeHGlobal((IntPtr)Memory);
                Memory = null;
                stream = null;
            }
        }

        /* ===================== SEEK ===================== */

        public void Seek(int offset)
        {
            stream = Memory + offset;
        }

        public void SeekForward(int amount)
        {
            Seek(Position + amount);
        }

        public void SeekBackwards(int amount)
        {
            Seek(Position - amount);
        }

        public void InitWriter()
        {
            Seek(4);
        }

        /* ===================== WRITE ===================== */

        public void Write(byte value)
        {
            *stream = value;
            stream++;
        }

        public void Write(bool value) { Write((byte)(value ? 1 : 0)); }
        public void Write(sbyte value) { Write((byte)value); }
        public void Write(short value) { Write((ushort)value); }
        public void Write(int value) { Write((uint)value); }
        public void Write(long value) { Write((ulong)value); }

        public void Write(ushort value)
        {
            *((ushort*)stream) = value;
            stream += 2;
        }

        public void Write(uint value)
        {
            *((uint*)stream) = value;
            stream += 4;
        }

        public void Write(ulong value)
        {
            *((ulong*)stream) = value;
            stream += 8;
        }

        public void Write(string value, int length)
        {
            if (value == null) value = "";
            byte[] buf = Encoding.UTF8.GetBytes(value);

            int min = buf.Length < length ? buf.Length : length;
            for (int i = 0; i < min; i++)
                Write(buf[i]);

            ZeroFill(length - min);
        }

        public void Write(params string[] value)
        {
            Write((byte)value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value[i] ?? "");
                Write((ushort)buffer.Length);
                for (int x = 0; x < buffer.Length; x++)
                    Write(buffer[x]);
            }
        }

        public void WriteUnsafe(void* buf, int length)
        {
            memcpy(stream, buf, length);
            stream += length;
        }

        public void ZeroFill(int amount)
        {
            if (amount > 0)
                memset(stream, 0, amount);
            stream += amount;
        }

        /* ===================== READ ===================== */

        public byte ReadUInt8() { return *stream++; }
        public sbyte ReadInt8() { return (sbyte)ReadUInt8(); }

        public ushort ReadUInt16()
        {
            ushort v = *((ushort*)stream);
            stream += 2;
            return v;
        }

        public short ReadInt16() { return (short)ReadUInt16(); }

        public uint ReadUInt32()
        {
            uint v = *((uint*)stream);
            stream += 4;
            return v;
        }

        public int ReadInt32() { return (int)ReadUInt32(); }

        public ulong ReadUInt64()
        {
            ulong v = *((ulong*)stream);
            stream += 8;
            return v;
        }

        public long ReadInt64() { return (long)ReadUInt64(); }

        public byte[] ReadBytes(int size)
        {
            byte[] res = new byte[size];
            for (int i = 0; i < size; i++)
                res[i] = ReadUInt8();
            return res;
        }

        public string ReadCString(int size)
        {
            string s = Encoding.UTF8.GetString(ReadBytes(size));
            int idx = s.IndexOf('\0');
            return idx >= 0 ? s.Substring(0, idx) : s;
        }

        public string[] ReadStringList()
        {
            int count = ReadUInt8();
            string[] list = new string[count];
            for (int i = 0; i < count; i++)
                list[i] = ReadCString(ReadUInt16());
            return list;
        }

        public void ReadUnsafe(void* buf, int length)
        {
            memcpy(buf, stream, length);
            stream += length;
        }

        /* ===================== PROTOBUF ===================== */

        public void ProtoBufferSerialize(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, obj);
                byte[] arr = ms.ToArray();
                fixed (byte* p = arr)
                    memcpy(stream, p, arr.Length);
                stream += arr.Length;
            }
        }

        public T ProtoBufferDeserialize<T>(T obj)
        {
            Seek(0);
            ushort len = ReadUInt16();
            ReadUInt16();

            byte[] data = ReadBytes(len - 4);
            using (MemoryStream ms = new MemoryStream(data))
                obj = ProtoBuf.Serializer.Deserialize<T>(ms);
            return obj;
        }

        public T ProtoBufferDeserialize<T>(T obj, byte[] packet)
        {
            using (MemoryStream ms = new MemoryStream(packet))
                obj = ProtoBuf.Serializer.Deserialize<T>(ms);
            return obj;
        }

        /* ===================== FINALIZE ===================== */

        public void Finalize(ushort type)
        {
            if (SealSize > 0)
                WriteSeal();

            Size = Position;
            Seek(0);
            Write((ushort)(Size - SealSize));
            Write(type);
        }

        public void WriteSeal()
        {
            Write(SealString, TQ_SEALSIZE);
        }

        /* ===================== MEMCPY ===================== */

        public void memcpy(void* dest, void* src, int size)
        {
            byte* d = (byte*)dest;
            byte* s = (byte*)src;
            for (int i = 0; i < size; i++)
                d[i] = s[i];
        }

        public static void WriteString(string arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + arg.Length)
            {
                for (ushort num = 0; num < arg.Length; num = (ushort)(num + 1))
                {
                    buffer[(ushort)(num + offset)] = (byte)arg[num];
                }
            }
        }
        public static void WriteUInt16(ushort arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + 2)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
            }
        }
        public static void WriteUInt32(uint arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + 4)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
                buffer[offset + 2] = (byte)(arg >> 16);
                buffer[offset + 3] = (byte)(arg >> 24);
            }
        }
    }
}
