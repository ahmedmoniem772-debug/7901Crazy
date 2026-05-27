using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusX;
public static class PacketRelic
{
    public static unsafe VirusX.ServerSockets.Packet CreateRuneResonance(this VirusX.ServerSockets.Packet stream, MsgResonanceRune.MsgResonanceProto obj)
    {
        stream.InitWriter();
        stream.ProtoBufferSerialize(obj);
        stream.Finalize((ushort)2604);
        return stream;
    }

    public static unsafe void GetRuneResonance(this VirusX.ServerSockets.Packet stream, out MsgResonanceRune.MsgResonanceProto pQuery)
    {
        pQuery = stream.ProtoBufferDeserialize<MsgResonanceRune.MsgResonanceProto>(new MsgResonanceRune.MsgResonanceProto());
    }
}