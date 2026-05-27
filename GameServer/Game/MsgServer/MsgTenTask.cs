using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace MortalConquer
{
    [ProtoContract]
    public class TenTimesLotteryReward
    {
        [ProtoMember(1, IsRequired = true)]
        public uint[] ID;
    }
}
