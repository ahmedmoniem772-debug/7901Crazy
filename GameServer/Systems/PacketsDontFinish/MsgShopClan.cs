using ProtoBuf;
using ConquerOnline.Client;
using System.Collections.Generic;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet CreateShopClan(this ServerSockets.Packet stream, MsgShopClan.ProtoStructure pQuery)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgShopClan);
            return stream;
        }
        public static void GetExchangeShopGoodsInfo(this ServerSockets.Packet stream, out MsgShopClan.ProtoStructure pQuery)
        {
            pQuery = new MsgShopClan.ProtoStructure();
            pQuery = stream.ProtoBufferDeserialize<MsgShopClan.ProtoStructure>(pQuery);
        }

    }
  
    public class MsgShopClan
    {
        [ProtoContract]
        public class ProtoStructure
        {
            public enum Action : byte
            {
                Show = 0,
            }
            [ProtoMember(1, IsRequired = true)]
            public Action ACTion;
            [ProtoMember(2, IsRequired = true)]
            public uint ID;
            [ProtoMember(3, IsRequired = true)]
            public uint Level;
            [ProtoMember(4, IsRequired = true)]
            public uint Unk1;
            [ProtoMember(5, IsRequired = true)]
            public ulong FreeTime;
            [ProtoMember(6, IsRequired = true)]
            public uint PayNum;
            [ProtoMember(7, IsRequired = true)]
            public uint Ret;
            [ProtoMember(8, IsRequired = true)]
            public List<ITemProto> Items;
        }
        [ProtoContract]
        public class ITemProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint IDShop;
            [ProtoMember(2, IsRequired = true)]
            public uint Count;
            [ProtoMember(3, IsRequired = true)]
            public ulong Time;
        }
        [PacketAttribute(GamePackets.MsgShopClan)]
        private static void Process(GameClient user, ServerSockets.Packet stream)
        {
            MsgShopClan.ProtoStructure pQuery;
            stream.GetExchangeShopGoodsInfo(out pQuery);
            switch (pQuery.ACTion)
            {
                case ProtoStructure.Action.Show:
                    {
                        user.Send(stream.CreateShopClan(pQuery));
                        break;
                    }
            }
        }

    }
}
