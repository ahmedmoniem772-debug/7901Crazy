using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using VirusX.Game.MsgServer;

namespace VirusX.Role.Instance
{
    public class Mail
    {
        public ConcurrentDictionary<uint, Game.MsgServer.MsgProficiency> Mails = new ConcurrentDictionary<uint, Game.MsgServer.MsgProficiency>();

        private Client.GameClient Owner;

        public Mail(Client.GameClient _own)
        {
            Owner = _own;
        }
    }
}
