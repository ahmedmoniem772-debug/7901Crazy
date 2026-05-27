using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgServer
{
    public static unsafe partial class MsgBuilder
    {
        public static unsafe ServerSockets.Packet ItemViewCreate(this ServerSockets.Packet stream, uint PlayerUID, long Cost, MsgGameItem DateItem, MsgItemView.ActionMode Mode)
        {
            stream.InitWriter();
            stream.Write(DateItem.UID);//4
            stream.Write(PlayerUID);//8
            stream.Write(Cost);//12
            stream.Write(DateItem.ITEM_ID);//20
            stream.Write(DateItem.Durability);//24
            stream.Write(DateItem.MaximDurability);//26
            stream.Write((ushort)Mode);//28
            stream.Write((byte)DateItem.Position);//30
            stream.Write(DateItem.SocketProgress);//31
            stream.Write((byte)DateItem.SocketOne);//35
            stream.Write((byte)DateItem.SocketTwo);//36
            stream.Write((uint)DateItem.SpellID);//37
            stream.Write((byte)DateItem.Effect);//41
            stream.Write(DateItem.Plus);//42
            stream.Write(DateItem.Bless);//43
            stream.Write(DateItem.Bound);//44
            stream.Write(DateItem.Enchant);//45
            stream.Write((uint)DateItem.ProgresGreen);//46
            stream.Write(DateItem.Suspicious);//50
            stream.Write((byte)0);//51
            stream.Write((byte)DateItem.Locked);//52
            stream.Write((byte)0);//53
            stream.Write(DateItem.PlusProgress);//54
            if (DateItem.Activate == 0)
            {
                stream.Write(DateItem.Activate);//58
                stream.Write(DateItem.TimeLeftInMinutes);//uint.MaxValue);//active 

            }
            else
            {
                if (DateItem.Activate == 1 && DateItem.TimeLeftInMinutes > 0 && DateItem.TimeLeftInMinutes < uint.MaxValue)
                {
                    TimeSpan timeSpan = DateItem.TimeStamp - DateTime.Now;
                    stream.Write((uint)(DateItem.TimeLeftInMinutes = (uint)(timeSpan.TotalSeconds)));//46 TimeIn
                }
                else
                    stream.Write(DateItem.TimeLeftInMinutes);//uint.MaxValue);//active 
                stream.Write(0);
            }
            stream.Write(DateItem.StackSize);
            stream.Write(DateItem.Purification.PurificationItemID);
            stream.Write(DateItem.Purification.PurificationItemID);
            stream.Write(DateItem.PerfectionLevel);
            stream.Write(DateItem.PerfectionProgress);
            stream.Write(DateItem.OwnerUID);
            stream.Write(DateItem.OwnerName, 32);
            stream.Write(DateItem.Signature, 64);
            stream.Write(DateItem.RuneEXP);
            for (int i = 0; i < DateItem.RelicAttributes.Length; i++)
                stream.Write(DateItem.RelicAttributes[i]);
            stream.Write(DateItem.AnimaItemID);
            stream.Write(0);//
            stream.Write(DateItem.MythsoulEffect);//208
            stream.Write(DateItem.MythSoulID);//208
            stream.Write(DateItem.MythSoulProgress);//208
            stream.Write(0);
            stream.Write(0);//ENERGY
            stream.Write(DateItem.Mutacion);//Funcion.//232
            stream.Write(DateItem.Mutacion2);//236
            stream.Write(0);
            stream.Write(DateItem.YuanshenIndex);
            stream.Write(DateItem.YuanshenTime);
            stream.Finalize(GamePackets.MsgItemInfoEx);
            return stream;
        }

    }

    public class MsgItemView
    {
        public enum ActionMode : ushort
        {
            Gold = 1,
            CPs = 3,
            ViewEquip = 4,
            Rune = 7,
             Yuanshen = 8,
        }
    }
}
