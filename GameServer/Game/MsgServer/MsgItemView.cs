using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
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
                    stream.Write((uint)(DateItem.TimeLeftInMinutes));

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
            stream.Write(DateItem.MythSoulID); //216
            stream.Write(DateItem.MythSoulProgress); //220
            stream.Write(0);
            stream.Write(0);//ENERGY
            stream.Write(DateItem.Mutacion);//Funcion.
            stream.Finalize(GamePackets.ItemView);
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
            Rune = 7
        }
    }
}
