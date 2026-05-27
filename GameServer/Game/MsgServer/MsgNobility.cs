using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {
        public static unsafe void GetNobility(this ServerSockets.Packet stream, out MsgNobility.NobilityAction mode, out ulong UID, out MsgNobility.DonationTyp donationtyp)
        {
            mode = (MsgNobility.NobilityAction)stream.ReadInt32();
            UID = stream.ReadUInt64();
            stream.SeekForward(88);
            donationtyp = (MsgNobility.DonationTyp)stream.ReadUInt8();
        }
        public static unsafe ServerSockets.Packet NobilityIconCreate(this ServerSockets.Packet stream, Role.Instance.Nobility nobility)
        {
            stream.InitWriter();

            stream.Write((uint)MsgNobility.NobilityAction.Icon);//4
            stream.Write(nobility.UID);//8

            string StrList = "" + nobility.UID + " " + nobility.Donation + " " + (byte)nobility.Rank + " " + nobility.Position + "";
            stream.ZeroFill(108);

            stream.Write(StrList);

            stream.Finalize(GamePackets.MsgPeerage);

            return stream;
        }
    }

    public unsafe struct MsgNobility
    {


        public enum NobilityAction : uint
        {
            Donate = 1,
            RankListen = 2,
            Icon = 3,
            NobilityInformarion = 4,
        }
        public enum DonationTyp : byte
        {
            Money = 0,
            ConquerPoints = 1,
            BoundCps = 2
           
        }


        [PacketAttribute(GamePackets.MsgPeerage)]
        public unsafe static void HandlerNobility(Client.GameClient user, ServerSockets.Packet stream)
        {
            NobilityAction Action;
            ulong UID;
            DonationTyp donationtyp;

            stream.GetNobility(out Action, out UID, out donationtyp);
            switch (Action)
            {
                case NobilityAction.Donate:
                    {
                        if (!user.Player.OnMyOwnServer)
                            return;
                        if (user.InTrade)
                            return;
                        if (user.PokerPlayer != null)
                            return;
                        
                        switch (donationtyp)
                        {
                            case DonationTyp.Money:
                                {
                                    if (UID > (ulong)user.Player.Money)
                                    {
                                        user.SendSysMesage(" WorldConquer#99.", MsgMessage.ChatMode.Service);
                                        return;
                                    }
                                    if (!user.Player.OnMyOwnServer)
                                        return;
                                    if (user.InTrade)
                                        return;
                                    if (user.PokerPlayer != null)
                                        return;
                                    if (UID < 0)
                                        return;
                                    if (user.Player.Money >= (long)UID)
                                    {
                                        user.Player.Money -= (long)UID;
                                        user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                        user.Player.Nobility.Donation += UID;
                                        user.Send(stream.NobilityIconCreate(user.Player.Nobility));
                                        Pool.NobilityRanking.UpdateRank(user.Player.Nobility);
                                    }

                                    else
                                    {

                                        if (user.Player.Money >= (long)UID)
                                        {
                                            user.Player.Money -= (long)UID;
                                            user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                            user.Player.Nobility.Donation += UID;
                                            user.Send(stream.NobilityIconCreate(user.Player.Nobility));
                                            Pool.NobilityRanking.UpdateRank(user.Player.Nobility);
                                        }
                                    }
                                    break;
                                }
                            case DonationTyp.ConquerPoints:
                                {
                                    if (UID < 0)
                                        return;
                                    if ((UID / 50000) > (ulong)user.Player.ConquerPoints)
                                    {
                                        user.SendSysMesage("WorldConquer#99.", MsgMessage.ChatMode.Service);
                                        return;
                                    }
                                    if (user.Player.ConquerPoints >= (int)(UID / 50000))
                                    {
                                        user.Player.ConquerPoints -= (long)(UID / 50000);
                                        user.Player.Nobility.Donation += UID;
                                        user.Send(stream.NobilityIconCreate(user.Player.Nobility));
                                        Pool.NobilityRanking.UpdateRank(user.Player.Nobility);
                                    }
                                    else
                                    {
                                        user.SendSysMesage("you don't have enough cps to donate , or less number to donate.", MsgMessage.ChatMode.Service);
                                        return;
                                    }
                                    break;
                                }
                            case DonationTyp.BoundCps:
                                {
                                    if (user.Player.BoundConquerPoints >= (uint)(UID / 50000))
                                    {
                                        user.Player.BoundConquerPoints -= (int)(UID / 50000);
                                        user.Player.Nobility.Donation += UID;
                                        user.Send(stream.NobilityIconCreate(user.Player.Nobility));
                                        Pool.NobilityRanking.UpdateRank(user.Player.Nobility);
                                    }
                                    else
                                    {
                                        user.SendSysMesage("you don't have enough cpsb to donate , or less number to donate.", MsgMessage.ChatMode.Service);
                                        return;
                                    }
                                    break;
                                }
                           
                        }
                        break;
                    }
                case NobilityAction.RankListen:
                    {
                        int page = (int)UID;
                        var info = Pool.NobilityRanking.GetArray();

                        try
                        {
                            const int max = 10;
                            int offset = page * max;
                            int count = Math.Min(max, Math.Max(0, info.Length - offset));

                            stream.InitWriter();

                            stream.Write((uint)NobilityAction.RankListen);
                            stream.Write((ushort)page);
                            stream.Write((ushort)5);
                            stream.Write((ushort)10);

                            stream.ZeroFill(102);

                            stream.Write((uint)1);

                            for (int x = 0; x < count; x++)
                            {
                                if (info.Length > offset + x)
                                {
                                    var element = info[offset + x];
                                    if (element.Position < 50)
                                    {
                                        stream.Write(element.UID);
                                        stream.Write(element.Mesh);

                                        stream.Write(element.Name, 32);
                                        stream.Write(element.Donation);
                                        stream.Write((uint)element.Rank);
                                        stream.Write((uint)element.Position);
                                    }
                                }
                            }


                            stream.Finalize(GamePackets.MsgPeerage);

                            user.Send(stream);
                        }
                        catch (Exception e) { MyConsole.WriteLine(e.ToString()); }
                        break;
                    }


                case NobilityAction.NobilityInformarion:
                    {
                        stream.InitWriter();

                        stream.Write((uint)NobilityAction.NobilityInformarion);

                        stream.Write(Pool.NobilityRanking.KnightDonation);
                        stream.Write(uint.MaxValue);
                        stream.Write((uint)Role.Instance.Nobility.NobilityRank.Knight);

                        stream.Write(Pool.NobilityRanking.KnightDonation);
                        stream.Write(uint.MaxValue);
                        stream.Write((uint)Role.Instance.Nobility.NobilityRank.Baron);

                        stream.Write(Pool.NobilityRanking.EarlDonation);
                        stream.Write(uint.MaxValue);
                        stream.Write((uint)Role.Instance.Nobility.NobilityRank.Earl);

                        stream.Write(Pool.NobilityRanking.DukeDonation);
                        stream.Write(uint.MaxValue);
                        stream.Write((uint)Role.Instance.Nobility.NobilityRank.Duke);

                        stream.Write(Pool.NobilityRanking.PrinceDonation);
                        stream.Write(uint.MaxValue);
                        stream.Write((uint)Role.Instance.Nobility.NobilityRank.Prince);

                        stream.Write(Pool.NobilityRanking.KingDonation);
                        stream.Write(uint.MaxValue);
                        stream.Write((uint)Role.Instance.Nobility.NobilityRank.King);

                        stream.Finalize(GamePackets.MsgPeerage);

                        user.Send(stream);


                        break;
                    }

            }

        }
    }
}
