using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerOnline.Game.MsgServer;

namespace ConquerOnline.MsgInterServer.Instance
{
    public static class CrossCaptureTheFlag
    {
        public static System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.Guild> RegistredGuilds = new System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.Guild>();
      
    }
}
