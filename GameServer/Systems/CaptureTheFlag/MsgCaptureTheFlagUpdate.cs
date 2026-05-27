using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CaptureTheFlagUpdateCreate(this ServerSockets.Packet stream, MsgCaptureTheFlagUpdate.Mode mode, uint DwParam, uint DwParam2 = 0)
        {
            stream.InitWriter();
            stream.Write((uint)mode);//4
            stream.Write(DwParam);//8
            stream.ZeroFill(16);//12
            stream.Write(DwParam2);//28
 

            return stream;
        }
        public static unsafe ServerSockets.Packet AddX2LocationCaptureTheFlagUpdate(this ServerSockets.Packet stream, ushort x, ushort y)
        {
            stream.Write(x);//32
            stream.Write(y);//34
            return stream;
        }
        public static unsafe ServerSockets.Packet AddItemCaptureTheFlagUpdate(this ServerSockets.Packet stream, uint dwparam1, uint dwparam2, string name)
        {
            stream.Write(dwparam1);
            stream.Write(dwparam2);
            stream.Write((uint)0);
            stream.Write(name, 32);
            stream.ZeroFill(40);
            return stream;
        }
        public static unsafe ServerSockets.Packet CaptureTheFlagUpdateFinalize(this ServerSockets.Packet stream)
        {
            stream.Finalize(GamePackets.MsgWarFlag);

            return stream;
        }
        public static unsafe void GetCaptureTheFlagUpdate(this ServerSockets.Packet stream, out  MsgCaptureTheFlagUpdate.Mode mode)
        {
            mode = (MsgCaptureTheFlagUpdate.Mode)stream.ReadUInt32();
        }
    }
    public class MsgCaptureTheFlagUpdate
    {
        public enum Mode : uint
        {
            InitializeCTF = 0,
            ScoreUpdate = 2,
            ScoreBase = 1,
            OccupiedBase = 5,
            Location = 3,
            GenerateEffect = 6,
            RemoveFlagEffect = 7,
            GenerateTimer = 8,
            InitializeRank = 11,
            X2Location = 12,
        }
        [PacketAttribute(GamePackets.MsgWarFlag)]
        private unsafe static void Poroces(Client.GameClient user, ServerSockets.Packet stream)
        {
          

            Mode ActionID;
            stream.GetCaptureTheFlagUpdate(out ActionID);

            switch (ActionID)
            {
                case Mode.RemoveFlagEffect:
                    {
                        MsgTournaments.MsgSchedules.CaptureTheFlag.PlantTheFlag(user, stream);
                        break;
                    }
                case Mode.InitializeRank:
                    {
                        if (MsgTournaments.MsgSchedules.CaptureTheFlag.Proces == MsgTournaments.ProcesType.Alive)
                        {
                            stream.CaptureTheFlagUpdateCreate(ActionID, 1, 0);
                            stream.CaptureTheFlagUpdateFinalize();
                            user.Send(stream);
                        }
                        else
                        {
                            stream.CaptureTheFlagUpdateCreate(ActionID, 0, 0);
                            stream.CaptureTheFlagUpdateFinalize();
                            user.Send(stream);
                        }
                        break;
                    }
            }
        }

    }
}
