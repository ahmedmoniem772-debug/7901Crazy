using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using ConquerOnline.Role.Instance.Poker;
namespace ConquerOnline.Game.MsgServer
{
    public class Packet
    {
        public static void WriteString(string arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + arg.Length)
            {
                for (ushort num = 0; num < arg.Length; num = (ushort)(num + 1))
                {
                    buffer[(ushort)(num + offset)] = (byte)arg[num];
                }
            }
        }

        public static void WriteByte(byte arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1)
            {
                buffer[offset] = arg;
            }
        }

        public static void WriteBoolean(bool arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1)
            {
                WriteByte((byte)(arg ? 1 : 0), offset, buffer);
            }
        }

        public static void WriteUInt16(ushort arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + 2)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
            }
        }

        public static void WriteUInt32(uint arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + 4)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
                buffer[offset + 2] = (byte)(arg >> 16);
                buffer[offset + 3] = (byte)(arg >> 24);
            }
        }

        public static void WriteUInt64(ulong arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= buffer.Length - 1 && buffer.Length >= offset + 8)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
                buffer[offset + 2] = (byte)(arg >> 16);
                buffer[offset + 3] = (byte)(arg >> 24);
                buffer[offset + 4] = (byte)(arg >> 32);
                buffer[offset + 5] = (byte)(arg >> 40);
                buffer[offset + 6] = (byte)(arg >> 48);
                buffer[offset + 7] = (byte)(arg >> 56);
            }
        }
    }
    public class General
    {
        

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ActivePlayerTypes
        {
            public static byte Bet = 1;

            public static byte Call = 2;

            public static byte Fold = 4;

            public static byte Check = 8;

            public static byte Raise = 16;

            public static byte Allin = 32;
        }




        public static byte[] MsgShowHandActivePlayer(Role.Instance.PokerTable table, ushort counter, uint uid)
        {
            ushort requiredAction;
            byte[] array;
            if (table.TableType == Role.Flags.TableType.TexasHoldem)
            {
                requiredAction = table.GetRequiredAction();
                array = new byte[40];
                Packet.WriteUInt16((ushort)(array.Length - 8), 0, array);
                Packet.WriteUInt16(GamePackets.MsgShowHandActivePlayer, 2, array);
                Packet.WriteUInt16(counter, 4, array);
                Packet.WriteUInt16(requiredAction, 6, array);
                Packet.WriteUInt64(table.MinBet, 8, array);
                if (!table.UnLimited)
                {
                    if (table.NumberOfRaise == 0)
                    {
                        if (table.State == Role.Flags.TableState.Pocket || table.State == Role.Flags.TableState.Flop)
                        {
                            Packet.WriteUInt64(table.MinBet, 8, array);
                        }
                        else
                        {
                            Packet.WriteUInt64(table.MinBet * 2, 8, array);
                        }
                    }
                    else if (table.State == Role.Flags.TableState.Pocket)
                    {
                        Packet.WriteUInt64(table.MinBet, 8, array);
                    }
                    else
                    {
                        Packet.WriteUInt64(table.MinBet * 2, 8, array);
                    }
                }
                if (table.UnLimited && table.PreviousPlayer.RoundPot != 0L)
                {
                    Packet.WriteUInt64(table.PreviousPlayer.RoundPot, 8, array);
                }
                if (table.CurrentPlayer != 0)
                {
                    Packet.WriteUInt64((ulong)(table.Players[table.CurrentPlayer].CurrentMoney - (long)table.RequiredPot), 16, array);
                    Packet.WriteUInt32(table.Players[table.CurrentPlayer].ServerID, 24, array);
                    Packet.WriteUInt32(table.Players[table.CurrentPlayer].Uid, 28, array);
                }
                return array;
            }
            else
            {
                requiredAction = table.GetRequiredAction();
                array = new byte[40];
                Packet.WriteUInt16((ushort)(array.Length - 8), 0, array);
                Packet.WriteUInt16(GamePackets.MsgShowHandActivePlayer, 2, array);
                Packet.WriteUInt16(counter, 4, array);
                Packet.WriteUInt16(requiredAction, 6, array);
                Packet.WriteUInt64(table.MinBet, 8, array);
                if (table.UnLimited && table.PreviousPlayer.RoundPot != 0L)
                {
                    Packet.WriteUInt64(table.PreviousPlayer.RoundPot, 8, array);
                }
                if (table.CurrentPlayer != 0)
                {
                    if (!table.Showhand)
                    {
                        Packet.WriteUInt64((ulong)(table.LowestMoney - (long)table.RequiredPot), 16, array);
                        Packet.WriteUInt32(table.Players[table.CurrentPlayer].ServerID, 24, array);
                        Packet.WriteUInt32(table.Players[table.CurrentPlayer].Uid, 28, array);
                    }
                    else
                    {
                        Packet.WriteUInt64(table.RequiredPot, 16, array);
                        Packet.WriteUInt32(table.Players[table.CurrentPlayer].ServerID, 24, array);
                        Packet.WriteUInt32(table.Players[table.CurrentPlayer].Uid, 28, array);
                    }
                }
            }
            return array;
        }
  
        public static byte[] MsgShowHandGameResult(Role.Instance.PokerTable table)
        {
            lock (table.TableSyncRoot)
            {

                int count = table.Players.Values.Where((Player p) => p.IsPlaying).ToList().Count;
                byte[] array = new byte[8 + 8 + 32 * count];
                Packet.WriteUInt16((ushort)((int)array.Length - 8), 0, array);
                Packet.WriteUInt16(GamePackets.MsgShowHandGameResult, 2, array);
                Packet.WriteUInt16(10, 4, array);
                Packet.WriteUInt16((ushort)count, 6, array);
                int Offset = 8;
                foreach (Player item in table.Players.Values.Where((Player p) => p.IsPlaying))
                {
                    if (item.CurrentMoney < table.MinBet * 10)
                    {
                        array[Offset] = 1; Offset++;//39
                    }
                    else
                    {
                        array[Offset] = 0;//8
                        Offset++;
                    }
                    if (!item.Fold)
                    {
                        if (item.Lose >= 0L)
                        {
                            if (item.Lose > 0L)
                            {
                                Packet.WriteUInt16(1, Offset, array); //9
                                Offset += 2;
                            }
                        }
                        else
                        {
                            Packet.WriteUInt16(255, Offset, array); //9
                            Offset += 2;
                        }
                    }
                    else
                    {
                        Packet.WriteUInt16(3, Offset, array);//9
                        Offset += 2;
                    } 
                    Packet.WriteUInt32(item.ServerID, Offset, array);//11
                    Offset += 4;
                    Packet.WriteUInt32(item.Uid, Offset, array);//15
                    Offset += 4;
                    Packet.WriteUInt64((ulong)item.Lose, Offset, array);//19
                    Offset += 20;
                    
                }
                return array;
               
            }
        }
    }
}