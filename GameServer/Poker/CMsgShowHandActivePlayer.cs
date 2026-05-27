using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Poker
{
    public class CMsgShowHandActivePlayer
    {
        public static byte[] Create(PokerTable Table, ushort counter, uint uid)
        {
            ushort Action = Table.GetRequiredAction();
            MemoryStream GStream = new MemoryStream();
            BinaryWriter stream = new BinaryWriter(GStream);
            stream.Write((ushort)0);
            stream.Write((ushort)PacketsID.CMsgShowHandActivePlayer);
            stream.Write(counter);
            stream.Write(Action);
            if (Table.TableType == General.TableType.TexasHoldem)
            {
                ulong MinBet = Table.MinBet;
                if (!Table.UnLimited)
                {
                    if (Table.NumberOfRaise == 0)
                    {
                        if (Table.State == General.TableState.Pocket || Table.State == General.TableState.Flop)
                            MinBet = Table.MinBet;
                        else
                            MinBet = Table.MinBet * 2;
                    }
                    else
                    {
                        if (Table.State == General.TableState.Pocket)
                            MinBet = Table.MinBet;
                        else
                            MinBet = Table.MinBet * 2;
                    }
                }
                if (Table.UnLimited)
                {
                    if (Table.PreviousPlayer.RoundPot != 0)
                        MinBet = Table.PreviousPlayer.RoundPot;
                }
                stream.Write((ulong)MinBet);
                if (Table.CurrentPlayer != 0)
                {
                    stream.Write((ulong)((ulong)Table.Players[Table.CurrentPlayer].CurrentMoney - (ulong)Table.RequiredPot));//16
                    stream.Write(Table.Players[Table.CurrentPlayer].ServerID);
                    stream.Write(Table.Players[Table.CurrentPlayer].RealUID);
                }
            }
            else
            {
                ulong MinBet = Table.MinBet;
                if (Table.UnLimited)
                {
                    if (Table.PreviousPlayer.RoundPot != 0)
                        MinBet = Table.PreviousPlayer.RoundPot;
                }
                stream.Write((ulong)MinBet);
                if (Table.CurrentPlayer != 0)
                {
                    if (Table.Showhand == false)
                    {
                        stream.Write((ulong)(Table.LowestMoney) - Table.RequiredPot);
                        stream.Write(Table.Players[Table.CurrentPlayer].ServerID);
                        stream.Write(Table.Players[Table.CurrentPlayer].RealUID);
                    }
                    else
                    {
                        stream.Write(Table.RequiredPot);
                        stream.Write(Table.Players[Table.CurrentPlayer].ServerID);
                        stream.Write(Table.Players[Table.CurrentPlayer].RealUID);
                    }
                }


            }
            return GStream.TQServer();
        }
    }
}
