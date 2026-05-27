

using VirusX.Client;
using VirusX.Game.MsgTournaments;
using VirusX;

using VirusX.ServerSockets;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;



namespace VirusX.Game.MsgTournaments
{
    public static class MsgVlmScoreInfo
    {
        public static Packet CreateMsgVlmScoreInfo(
          this Packet stream,
          MsgVlmScoreInfo.MsgVlmScoreInfoProto proto)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize((object)proto);
            stream.Finalize((ushort)2177);
            return stream;
        }

        [Packet(2177)]
        public static void Process(GameClient client, Packet stream)
        {
            MsgVlmScoreInfo.MsgVlmScoreInfoProto vlmScoreInfoProto = new MsgVlmScoreInfo.MsgVlmScoreInfoProto();
            MsgVlmScoreInfo.MsgVlmScoreInfoProto proto = stream.ProtoBufferDeserialize<MsgVlmScoreInfo.MsgVlmScoreInfoProto>(vlmScoreInfoProto);
            switch ((MsgVlmScoreInfo._Type)proto.Type)
            {
                case MsgVlmScoreInfo._Type.PersonalInfo:
                    if ((long)MsgSchedules.CaptureTheFlag.VlmScoreInfoList.Count >= (long)proto.Index)
                    {
                        MsgCaptureTheFlag.VlmScoreInfo vlmScoreInfo = MsgSchedules.CaptureTheFlag.VlmScoreInfoList.Values.ToArray<MsgCaptureTheFlag.VlmScoreInfo>()[(int)proto.Index - 1];
                        if (vlmScoreInfo != null)
                        {
                            proto.PersonalInfo = new MsgVlmScoreInfo.Personal();
                            proto.PersonalInfo.Name = vlmScoreInfo.Name;
                            proto.PersonalInfo.Rank = vlmScoreInfo.Rank;
                            proto.PersonalInfo.Rating = vlmScoreInfo.Rating;
                            proto.PersonalInfo.Deaths = vlmScoreInfo.Deaths;
                            proto.PersonalInfo.Flags = 0U;
                            proto.PersonalInfo.GuildName = vlmScoreInfo.GuildName;
                            proto.PersonalInfo.uk10 = new ulong[0];
                            proto.PersonalInfo.CaptureTheFlagInfo = new ulong[14]
                            {
                (ulong) vlmScoreInfo.ContributionPts,
                vlmScoreInfo.TotalKills,
                vlmScoreInfo.MaxComboKill,
                vlmScoreInfo.Revival,
                vlmScoreInfo.Shackled,
                vlmScoreInfo.UnShackled,
                vlmScoreInfo.TotalDamage,
                vlmScoreInfo.FlagsCaptured,
                vlmScoreInfo.BasesOccupied,
                vlmScoreInfo.FlagsDelivered,
                24UL,
                25UL,
                26UL,
                257UL
                            };
                        }
                        break;
                    }
                    break;
                case MsgVlmScoreInfo._Type.Ranking:
                    int val1_1 = 10;
                    MsgCaptureTheFlag.VlmScoreInfo[] array1 = ((IEnumerable<MsgCaptureTheFlag.VlmScoreInfo>)MsgSchedules.CaptureTheFlag.VlmScoreInfoList.Values.ToArray<MsgCaptureTheFlag.VlmScoreInfo>()).Where<MsgCaptureTheFlag.VlmScoreInfo>((Func<MsgCaptureTheFlag.VlmScoreInfo, bool>)(p => p != null && p.ContributionPts != 0)).OrderByDescending<MsgCaptureTheFlag.VlmScoreInfo, int>((Func<MsgCaptureTheFlag.VlmScoreInfo, int>)(p => p.ContributionPts)).ToArray<MsgCaptureTheFlag.VlmScoreInfo>();
                    int num1 = ((int)proto.Page - 1) * val1_1;
                    int length1 = Math.Min(val1_1, array1.Length);
                    proto.Count = (uint)((IEnumerable<MsgCaptureTheFlag.VlmScoreInfo>)array1).Count<MsgCaptureTheFlag.VlmScoreInfo>();
                    proto.RankingInfo = new MsgVlmScoreInfo.Ranking[length1];
                    proto.uk5 = 1U;
                    for (byte index = 0; (int)index < length1 && (int)index + num1 < array1.Length; ++index)
                    {
                        MsgCaptureTheFlag.VlmScoreInfo vlmScoreInfo = array1[(int)index + num1];
                        proto.RankingInfo[(int)index] = new MsgVlmScoreInfo.Ranking();
                        proto.RankingInfo[(int)index].Rank = (uint)(num1 + (int)index + 1);
                        proto.RankingInfo[(int)index].Rating = vlmScoreInfo.Rating;
                        proto.RankingInfo[(int)index].GuildName = vlmScoreInfo.GuildName;
                        proto.RankingInfo[(int)index].ContributionPts = (uint)vlmScoreInfo.ContributionPts;
                        proto.RankingInfo[(int)index].Name = vlmScoreInfo.Name;
                    }
                    break;
                case MsgVlmScoreInfo._Type.RankingGw:
                    int val1_2 = 10;
                    MsgGuildWar.VlmScoreInfo[] array2 = ((IEnumerable<MsgGuildWar.VlmScoreInfo>)MsgSchedules.GuildWar.VlmScoreInfoList.Values.ToArray<MsgGuildWar.VlmScoreInfo>()).Where<MsgGuildWar.VlmScoreInfo>((Func<MsgGuildWar.VlmScoreInfo, bool>)(p => p != null && p.ContributionPts != 0)).OrderByDescending<MsgGuildWar.VlmScoreInfo, int>((Func<MsgGuildWar.VlmScoreInfo, int>)(p => p.ContributionPts)).ToArray<MsgGuildWar.VlmScoreInfo>();
                    int num2 = ((int)proto.Page - 1) * val1_2;
                    int length2 = Math.Min(val1_2, array2.Length);
                    proto.Count = (uint)((IEnumerable<MsgGuildWar.VlmScoreInfo>)array2).Count<MsgGuildWar.VlmScoreInfo>();
                    proto.RankingInfo = new MsgVlmScoreInfo.Ranking[length2];
                    proto.uk5 = 1U;
                    for (byte index = 0; (int)index < length2 && (int)index + num2 < array2.Length; ++index)
                    {
                        MsgGuildWar.VlmScoreInfo vlmScoreInfo = array2[(int)index + num2];
                        proto.RankingInfo[(int)index] = new MsgVlmScoreInfo.Ranking();
                        proto.RankingInfo[(int)index].Rank = (uint)(num2 + (int)index + 1);
                        proto.RankingInfo[(int)index].Rating = vlmScoreInfo.Rating;
                        proto.RankingInfo[(int)index].GuildName = vlmScoreInfo.GuildName;
                        proto.RankingInfo[(int)index].ContributionPts = (uint)vlmScoreInfo.ContributionPts;
                        proto.RankingInfo[(int)index].Name = vlmScoreInfo.Name;
                    }
                    break;
            }
            client.Send(stream.CreateMsgVlmScoreInfo(proto));
        }

        [ProtoContract]
        public class MsgVlmScoreInfoProto
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Type;
            [ProtoMember(2, IsRequired = true)]
            public uint Count;
            [ProtoMember(3, IsRequired = true)]
            public uint Index;
            [ProtoMember(4, IsRequired = true)]
            public uint Page;
            [ProtoMember(5, IsRequired = true)]
            public uint uk5;
            [ProtoMember(6, IsRequired = true)]
            public MsgVlmScoreInfo.Personal PersonalInfo;
            [ProtoMember(7, IsRequired = true)]
            public MsgVlmScoreInfo.Ranking[] RankingInfo;
            [ProtoMember(8, IsRequired = true)]
            public MsgVlmScoreInfo.ComboKill ComboKillInfo;
        }

        [ProtoContract]
        public class ComboKill
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ComboKills;
            [ProtoMember(2, IsRequired = true)]
            public uint Kills;
        }

        [ProtoContract]
        public class Personal
        {
            [ProtoMember(1, IsRequired = true)]
            public uint Rank;
            [ProtoMember(2, IsRequired = true)]
            public uint Rating;
            [ProtoMember(3, IsRequired = true)]
            public uint Flags;
            [ProtoMember(4, IsRequired = true)]
            public uint Deaths;
            [ProtoMember(5, IsRequired = true)]
            public ulong[] CaptureTheFlagInfo;
            [ProtoMember(6, IsRequired = true)]
            public MsgVlmScoreInfo.MEkiiled MK;
            [ProtoMember(7, IsRequired = true)]
            public MsgVlmScoreInfo.MEkiiled KM;
            [ProtoMember(8, IsRequired = true)]
            public string Name;
            [ProtoMember(9, IsRequired = true)]
            public string GuildName;
            [ProtoMember(10, IsRequired = true)]
            public ulong[] uk10;
        }

        [ProtoContract]
        public class MEkiiled
        {
            [ProtoMember(1, IsRequired = true)]
            public uint KMCount;
            [ProtoMember(2, IsRequired = true)]
            public string KMName;
        }

        [ProtoContract]
        public class Ranking
        {
            [ProtoMember(1, IsRequired = true)]
            public uint ContributionPts;
            [ProtoMember(2, IsRequired = true)]
            public uint Rating;
            [ProtoMember(3, IsRequired = true)]
            public uint Rank;
            [ProtoMember(4, IsRequired = true)]
            public string Name;
            [ProtoMember(5, IsRequired = true)]
            public string GuildName;
            [ProtoMember(6, IsRequired = true)]
            public uint[] uk6;
        }

        public enum _Type : uint
        {
            PersonalInfo = 0,
            Ranking = 1,
            View = 2,
            ComboKill = 3,
            test = 4,
            test1 = 5,
            test2 = 6,
            RankingGw = 8,
            ViewGw = 9,
            ComboKillGw = 10, // 0x0000000A
        }
    }
}
