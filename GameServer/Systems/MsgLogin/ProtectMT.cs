using ConquerOnline.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquerOnline
{
    public class ReConnectAccount
    {
        public DateTime Time;
        public uint UID;
        public Client.GameClient User;
        public ReConnectAccount(Client.GameClient _user)
        {
            UID = _user.OnLogin.Key;
            User = _user;
            Time = DateTime.Now.AddMinutes(15);
        }
        public bool CheakUP(DateTime _Time)
        {
            if (_Time > Time)
                return true;
            return false;
        }
    }
    public class ProtectMT
    {
        public static void JumpProtect(GameClient client)
        {
            return;
            lock (client.JumpLock)
            {
                double Mill = (client.Player.JumpingStamp - client.Player.JumpingPrevious).TotalMilliseconds;
                if (Mill > 100 && Mill <= 550)
                {
                    if (!MsgLoader.MsgLoader.UpdateSpeedFlags(client) && !client.Player.OnTransform && client.Player.TransformationID != 207 && client.Player.TransformationID != 267)
                    {
                        client.Player.lastClientJumpTime++;
                        if (client.Player.lastClientJumpTime >= 5)
                        {
                            client.Player.lastClientJumpTime = 0;
                            client.Socket.Disconnect();
                        }
                    }
                }
                else
                {
                    if (client.Player.lastClientJumpTime > 0)
                        client.Player.lastClientJumpTime--;

                }
                client.Player.JumpingPrevious = client.Player.JumpingStamp;
                client.Player.JumpingStamp = DateTime.Now;
            }

        }
        public static void MovementProtect(GameClient client)
        {
            return;
            lock (client.JumpLock)
            {
                double Mill = (client.Player.MoveingStamp - client.Player.MovePrevious).TotalMilliseconds;
                if (Mill > 0 && Mill <= 150)
                {
                    if (!MsgLoader.MsgLoader.UpdateSpeedFlags(client) && !client.Player.OnTransform && client.Player.TransformationID != 207 && client.Player.TransformationID != 267)
                    {
                        client.Player.CountMoveHack++;
                        if (client.Player.CountMoveHack >= 5)
                        {
                            client.Player.CountMoveHack = 0;
                            client.Socket.Disconnect();
                        }
                    }
                }
                else
                {
                    if (client.Player.CountMoveHack > 0)
                        client.Player.CountMoveHack--;

                }
                client.Player.MovePrevious = client.Player.MoveingStamp;
                client.Player.MoveingStamp = DateTime.Now;

            }
        }
    }
}
