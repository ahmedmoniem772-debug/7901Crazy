using CMsgTQProtect;
using MahmoudAli.ServerSockets;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MahmoudAli.CMsgTQProtect
{
    internal class MsgCheating
    {

        public enum _MSG_CHEAT_FLAGS : byte
        {
            None = 0,
            MemoryChanged = 0x10,      // Detects any changes in the game's memory
            AutoHunting,               // Automated hunting or bot behavior
            AutoClick,                 // Detects basic automated mouse clicking
            ScriptAutoClick,           // Detects scripted or advanced automated clicking
            AutoHotkey,                // Detects the use of AutoHotkey scripts for automation
            ScriptAutoHotkey,          // Detects scripted AutoHotkey usage
            ClientFilesScanning,       // Scanning for modifications to client files
            SpeedHack,                 // Detects speeding up or slowing down the game
            FunctionChanged,           // Detects if critical game functions are altered or replaced
            InjectDll,                 // Detects DLL injection into the game process
            InjectCode,                // Detects code injection into the game process
            Aimbot,                    // Auto-aiming at targets (common in FPS games)
            ZoomHack,                  // Altering camera zoom levels beyond normal limits
            DebuggerPresent,           // Detects the presence of a debugger
            SuspendThreads,            // Detects if threads are suspended (used to pause or manipulate gameplay)
            AbnormalOperation,         // Detects abnormal or unexpected game operations
        };

        [ProtoContract]
        public unsafe class CheatQuery
        {
            [ProtoMember(1, IsRequired = true)]
            public uint TickCount;
            [ProtoMember(2, IsRequired = true)]
            public uint CryptoKey;
            [ProtoMember(3, IsRequired = true)]
            public _MSG_CHEAT_FLAGS Flag;
            [ProtoMember(4)]
            public uint ConquerHash;
            [ProtoMember(5)]
            public uint GuardHash;
            [ProtoMember(6)]
            public uint MagicTypeHash;
            [ProtoMember(7)]
            public uint MagicEffectHash;
            [ProtoMember(8)]
            public uint UstrResHash;
            [ProtoMember(9)]
            public uint C3WdbHash;
            [ProtoMember(10)]
            public uint GuardDatHash;
            [ProtoMember(11)]
            public uint GuardVersion;
            [ProtoMember(12)]
            public string[] StrParam;
            [ProtoMember(13)]
            public uint QuerySize;
        }



        [PacketAttribute((ushort)MsgTQProtect._MSG_ID._MSG_CHEATING)]
        public unsafe static void Process(Client.GameClient client, ServerSockets.Packet packet)
        {
            packet.Seek(0);
            byte[] bytesArray = packet.ReadBytes(packet.Size);

            if (client.g_TQProtect == null || TQCipher.HandleBuffer(ref bytesArray, true) == 0)
            {
                client.g_TQProtect.Disconnect("Invalid g_TQProtect or HandleBuffer on " + Assembly.GetExecutingAssembly().FullName);
                return;
            }

            var QueryLength = BitConverter.ToInt16(bytesArray, 4);
            using (var ms = new System.IO.MemoryStream(bytesArray, 6, QueryLength))
            {
                var pQuery = ProtoBuf.Serializer.Deserialize<CheatQuery>(ms);
                if(pQuery.QuerySize != QueryLength)
                {
                    client.g_TQProtect.Disconnect("Invalid Query Size on "+ Assembly.GetExecutingAssembly().FullName);
                    return;
                }
                switch (pQuery.Flag)
                {
                    case _MSG_CHEAT_FLAGS.ClientFilesScanning:
                        {
                            string invalidFiles = "";
                            if(!MsgTQProtect.Validated(pQuery.ConquerHash, pQuery.MagicTypeHash,pQuery.MagicEffectHash,pQuery.C3WdbHash, pQuery.GuardHash, pQuery.UstrResHash, pQuery.GuardDatHash, out  invalidFiles))
                            {
                                Console.WriteLine("[TQProtect]Player {0} Detection ChangeFiles {1}", client.Player.Name, invalidFiles);
                                client.g_TQProtect.Disconnect();
                            }
                           break;
                        }
                    default:
                        {
                            Console.WriteLine("[TQProtect]Player {0} Detection Using {1}", client.Player.Name, pQuery.Flag.ToString());
                            MsgTQProtect.ReportLogg(client.Player.Name, pQuery.Flag.ToString());
                            client.g_TQProtect.Disconnect();
                            break;
                        }
                }
            }
        }
    }
}
