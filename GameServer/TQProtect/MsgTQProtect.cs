using MahmoudAli.Game.MsgServer;
using MahmoudAli.ServerSockets;
using OpenSSL;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMsgTQProtect
{
    public static class MsgTQProtect
    {
        public static bool HdKeyVaild = true;

        private static uint[] ConquerHashes = new uint[2], DLLHash = new uint[2], MagicTypeHash = new uint[2], MagicEffectHash = new uint[2], UStrResHash = new uint[2], DataServersHash = new uint[2];

        public static void Load(bool _IsGameServer = false)
        {
            new TQCipher(_IsGameServer);
            if (_IsGameServer)
            {
               

                DLLHash[0] = CalculateMD5(@"TQLoader\Files\Env_DX8\d3d8.dll");
                DLLHash[1] = CalculateMD5(@"TQLoader\Files\Env_DX9\d3d9.dll");


                MagicTypeHash[0] = CalculateMD5(@"TQLoader\Files\ini\magictype.dat");
                MagicEffectHash[0] = CalculateMD5(@"TQLoader\Files\ini\MagicEffect.ini");

                UStrResHash[0] = CalculateMD5(@"TQLoader\Files\ini\UStrRes.ini");
               // DataServersHash[0] = CalculateMD5(@"TQLoader\Files\Guard.dat");
            }
        }
        public static void ReportLogg(string Name, string reason)
        {
            DateTime timer = DateTime.Now;
            string logs = string.Format("CHEAT [Player] {0} -- REASON: {1}", Name, !String.IsNullOrEmpty(reason) ? reason : "Abnormal operation");
            OnDequeue(logs, timer.Millisecond);
        }
        private static void OnDequeue(object obj, int time)
        {
            try
            {
                if (obj is string)
                {
                    string text = obj as string;
                    string identifier = text.Substring(0, text.IndexOf("]") + 1);
                    string UnhandledExceptionsPath = Application.StartupPath + "\\TQLoader\\";
                    if (!Directory.Exists(UnhandledExceptionsPath))
                        Directory.CreateDirectory(UnhandledExceptionsPath);
                    UnhandledExceptionsPath += "[Logs]\\";
                    if (!Directory.Exists(UnhandledExceptionsPath))
                        Directory.CreateDirectory(UnhandledExceptionsPath);
                    UnhandledExceptionsPath += identifier + "\\";
                    if (!Directory.Exists(UnhandledExceptionsPath))
                        Directory.CreateDirectory(UnhandledExceptionsPath);
                    string fileName = UnhandledExceptionsPath + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";
                    if (!File.Exists(fileName))
                        File.WriteAllLines(fileName, new string[0]);
                    using (var SW = File.AppendText(fileName))
                    {
                        SW.WriteLine(text.Replace(identifier, DateTime.Now.ToString("[hh:mm:ss tt]:")));
                        SW.Close();
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        private static uint CalculateMD5(string filename)
        {
            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    using (var stream = System.IO.File.OpenRead(filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        string H = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                        return (uint)uint.Parse(H.Substring(0, 8), NumberStyles.HexNumber);
                    }
                }
            }
            catch
            {
                Console.WriteLine(string.Format("{0} Not Find Hash", filename));
            }
            return 0;
        }
        public static bool Validated(uint conquer, uint magicType, uint magicEffect, uint c3_WDB, uint dLL_Hash, uint UStrRes, uint Data_Servers, out string filechanged)
        {
            filechanged = "";
            return true;
            if (conquer != ConquerHashes[0] && conquer != ConquerHashes[1])
                filechanged += " Conquer.exe";

            if (magicType != MagicTypeHash[0])
                filechanged += " magictype.dat";
            if (magicEffect != MagicEffectHash[0])
                filechanged += " MagicEffect.ini";
            if (dLL_Hash != DLLHash[0] && dLL_Hash != DLLHash[1])
                filechanged += " d3d8-9.dll";
            if (UStrRes != UStrResHash[0])
                filechanged += " UStrRes.ini";
            //if (Data_Servers != DataServersHash[0])
             //   filechanged += " Guard.dat";
            return filechanged == "" ? true : false;
        }

        public enum _MSG_ID : ushort
        {
            _MSG_NONE = 0,
            _MSG_GENERAL = 0x2328,
            _MSG_ACCOUNT,
            _MSG_CONNECT,
            _MSG_DATA,
            _MSG_MACHINE,
            _MSG_PROCESS,
            _MSG_STRING,
            _MSG_CHEATING,
        }

        public class MsgTaskMgr
        {
            public enum _MSG_TASKMgr_FLAGS : byte
            {
                TASKMgr_START = 1,
                TASKMgr_INSERT,
                TASKMgr_FINISH,
            }

            public _MSG_TASKMgr_FLAGS ActionType;
            public Dictionary<string, string> Processes;

            public MsgTaskMgr(byte[] Buffer)
            {
                if (Buffer != null && Buffer.Length >= 4 + 1 && BitConverter.ToUInt16(Buffer, 2) == (ushort)_MSG_ID._MSG_PROCESS)
                {
                    if (HdKeyVaild)
                    {

                        if (TQCipher.HandleBuffer(ref Buffer, true) == 0)
                            return;

                        BinaryReader rdr = new BinaryReader(new MemoryStream(Buffer));
                        rdr.BaseStream.Seek(4, SeekOrigin.Current);
                        var SwitchType = rdr.ReadByte();
                        if (!Enum.IsDefined(typeof(_MSG_TASKMgr_FLAGS), SwitchType))
                        {
                            Console.WriteLine("Erorr Defined OpenedProcesses > ActionType " + SwitchType);
                            rdr.Close();
                            return;
                        }
                        ActionType = (_MSG_TASKMgr_FLAGS)SwitchType;
                        if (ActionType == _MSG_TASKMgr_FLAGS.TASKMgr_INSERT)
                        {
                            int count = rdr.ReadUInt16();
                            Processes = new Dictionary<string, string>(count);
                            for (int i = 0; i < count; i++)
                            {
                                var proc = Encoding.Default.GetString(rdr.ReadBytes(rdr.ReadByte())).Replace("\0", "");
                                var title = Encoding.Default.GetString(rdr.ReadBytes(rdr.ReadByte())).Replace("\0", "");
                                Processes.Add(proc, title);
                            }
                        }
                        rdr.Close();
                    }
                }
            }
        }
    }
}
