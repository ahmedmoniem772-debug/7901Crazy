using CMsgTQProtect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahmoudAli.CMsgTQProtect
{
    internal class MsgTaskMgr
    {

        [PacketAttribute((ushort)MsgTQProtect._MSG_ID._MSG_PROCESS)]
        public unsafe static void Process(Client.GameClient client, ServerSockets.Packet packet)
        {
            if (client.g_TQProtect == null)
                return;

            packet.Seek(0);
            byte[] bytes = packet.ReadBytes(packet.Size);
            var msg = new MsgTQProtect.MsgTaskMgr(bytes);

            if (msg.ActionType == MsgTQProtect.MsgTaskMgr._MSG_TASKMgr_FLAGS.TASKMgr_START)//Clean
            {
                client.OpenedProcesses.Clear();
                MyConsole.WriteLine("Someone requested " + client.Player.Name + "'s opened processes, Processing...");
            }
            else if (msg.ActionType == MsgTQProtect.MsgTaskMgr._MSG_TASKMgr_FLAGS.TASKMgr_INSERT)//Insert
            {
                foreach (var proc in msg.Processes)
                    client.OpenedProcesses.Add(proc.Key, proc.Value);
            }
            else if (msg.ActionType == MsgTQProtect.MsgTaskMgr._MSG_TASKMgr_FLAGS.TASKMgr_FINISH)//Finish
            {
                MyConsole.WriteLine(client.Player.Name + "'s " + client.OpenedProcesses.Count + " opened processes have been received successfully, Check log files.");
                string text = "==========================================================" + Environment.NewLine;
                text += "Request Time: " + DateTime.Now.ToString("dddd, dd MMMM yyyy [HH:mm:ss]") + Environment.NewLine;


                text += "Processes Instances Count: " + client.OpenedProcesses.Count() + Environment.NewLine + Environment.NewLine;
                text += "==========================================================" + Environment.NewLine;


                foreach (var proc in client.OpenedProcesses)
                {
                    try
                    {
                        //text += "---------------" + Environment.NewLine;
                        text += "PATCH: '" + proc.Key + "'" + Environment.NewLine;
                        text += "Title: '" + proc.Value + "'" + Environment.NewLine;
                        text += "---------------" + Environment.NewLine;
                    }
                    catch { }
                }
               
                text += "==========================================================" + Environment.NewLine + Environment.NewLine;
                if (!System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\TQLoader\\"))
                    System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\TQLoader\\");
                string path = System.Windows.Forms.Application.StartupPath + "\\TQLoader\\[Processes]\\";
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                path += client.Player.Name.RemoveIllegalCharacters(false, true) + ".txt";
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.WriteAllLines(path, new string[0]);
                }
                using (var SW = System.IO.File.AppendText(path))
                {
                    SW.WriteLine(text);
                    SW.Close();
                }
            }

        }
    }
}
