using ProtoBuf;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet UpdateProfExperienceCreate(this ServerSockets.Packet stream, uint Experience, uint NeededExperience, uint ID)
        {
            //stream.InitWriter();

            //stream.Write(Experience);
            //stream.Write(NeededExperience);
            //stream.Write(ID);
            //stream.ZeroFill(8);//unknow
            var pQuery = new FlushExp()
            {
                EXP = Experience,
                NeededEXP = NeededExperience,
                ProfID = ID,
                unk4 = 0,
            };
            stream.InitWriter();
            stream.ProtoBufferSerialize(pQuery);
            stream.Finalize(GamePackets.MsgFlushExp);
            return stream;
        }
    }
    [ProtoContract]
    public class FlushExp
    {
        [ProtoMember(1, IsRequired = true)]
        public uint EXP;
        [ProtoMember(2)]
        public uint NeededEXP;
        [ProtoMember(3)]
        public uint ProfID;
        [ProtoMember(4)]
        public uint unk4;
    }
}
