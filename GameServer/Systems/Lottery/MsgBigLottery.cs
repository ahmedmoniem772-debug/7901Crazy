using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace VirusX.Game.MsgServer
{

    [ProtoContract]
    public class MsgBigLottery
    {
        [ProtoMember(1, IsRequired = true)]
        public uint Type;
        [ProtoMember(2, IsRequired = true)]
        public Item[] Items;
        [ProtoContract]
        public class Item
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Bliss;
            [ProtoMember(2, IsRequired = true)]
            public uint ID;
            [ProtoMember(3, IsRequired = true)]
            public uint ItemID;
            [ProtoMember(4, IsRequired = true)]
            public uint u4;
            [ProtoMember(5, IsRequired = true)]
            public string Name;
        }
        public static implicit operator ServerSockets.Packet(MsgBigLottery obj)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.ProtoBufferSerialize(obj);
                stream.Finalize(GamePackets.MsgBigLottery);
                return stream;
            }
        }

        [PacketAttribute(GamePackets.MsgBigLottery)]
        private unsafe static void Process(Client.GameClient user, ServerSockets.Packet stream)
        {
            LotteryTable.GenerateLotteryPrize();
            MsgBigLottery Info = null;
            Info = new MsgBigLottery();
            Info = stream.ProtoBufferDeserialize<MsgBigLottery>(Info);
            switch (Info.Type)
            {
                case 0:
                    {
                        var Array = LotteryTable.LotteryList.Values.ToArray();
                        int Counts = Array.Length;
                        for (int i = 0; i < (Array.Length / 20) + 1; i++)
                        {
                            MsgBigLottery obj = new MsgBigLottery();
                            obj.Type = 0;
                            obj.Items = new Item[Counts > 20 ? 20 : Counts];
                            for (int x = 0; x < obj.Items.Length && Counts > 0; x++)
                            {
                                obj.Items[x] = new Item();
                                obj.Items[x].Bliss = Array[Counts - 1].Bliss;
                                obj.Items[x].ID = Array[Counts - 1].Id;
                                obj.Items[x].ItemID = Array[Counts - 1].Awared;
                                obj.Items[x].u4 = 0;
                                obj.Items[x].Name = Array[Counts - 1].Name;
                                Counts--;
                            }
                            user.Send(obj);
                        }
                        break;
                    }
            }
        }

    }
}