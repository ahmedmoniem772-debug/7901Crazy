
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.ServerSockets
{
    public class SocketThread
    {
        private System.Threading.Thread ThreadItem;
        private ServerSocket Socket;
        public SocketThread(ServerSocket socket)
        {
            this.Socket = socket;
            ThreadItem = new System.Threading.Thread(Process);
        }
        public void Start()
        {
            ThreadItem.Start();
        }

        public void Process()
        {
            try
            {
                while (true)
                {

                    System.Threading.Thread.Sleep(1);
                }
            }
            catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
        }
        public void Close()
        {
            ThreadItem.Abort();
        }

    }
}
