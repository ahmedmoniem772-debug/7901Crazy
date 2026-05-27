using ConquerOnline.Client;
using ProtoBuf;
using System;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static void GetCasinoInteractive(this ServerSockets.Packet stream, out MsgCasinoInteractive.ProtoStructure pQuery)
        {
            pQuery = new MsgCasinoInteractive.ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<MsgCasinoInteractive.ProtoStructure>(pQuery);
        }
        public static ServerSockets.Packet CreateCasinoInteractive(this ServerSockets.Packet stream, MsgCasinoInteractive.ProtoStructure obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgCasinoInteractive);
            return stream;
        }
    }
    public class MsgCasinoInteractive
    {
        [Flags]
        public enum Types : uint
        {
            Buy = 0,
        }
        [ProtoContract]
        public class ProtoStructure
        {
            [ProtoMember(1, IsRequired = true)]
            public Types type;

            [ProtoMember(2, IsRequired = true)]
            public long[] Count;
        }
        [PacketAttribute(GamePackets.MsgCasinoInteractive)]
        public unsafe static void Process(GameClient client, ServerSockets.Packet stream)
        {
            if (client.InTrade)
                return;
            if (client.PokerPlayer != null)
                return;
            if (client.Player.Money >= 9900000000)
                return;
            if (client.Player.ObjInteraction != null)
                return;
            return;
            ProtoStructure Info;
            stream.GetCasinoInteractive(out Info);
            #region Price Money
            long Price = 0;
            if (Info.Count[1] == 500000)
                Price = 9 * Info.Count[2];
            else if (Info.Count[1] == 5000000)
                Price = 99 * Info.Count[2];
            else if (Info.Count[1] == 30000000)
                Price = 599 * Info.Count[2];
            else if (Info.Count[1] == 200000000)
                Price = 3999 * Info.Count[2];
            else if (Info.Count[1] == 500000000)
                Price = 9999 * Info.Count[2];
            #endregion
            switch (Info.type)
            { 
                case Types.Buy:
                    {
                        if (client.Player.ConquerPoints >= (uint)Price)
                        {
                            client.Player.ConquerPoints -= (int)Price;
                            var Money = Info.Count[1] * Info.Count[2];
                            client.Player.Money += Money;
                            Game.ServerLogs.ChangeMoney(client, Money);
                            client.Send(stream.CreateCasinoInteractive(Info));
                        }
                        break;
                       
                    }
            }
        }
    }
}