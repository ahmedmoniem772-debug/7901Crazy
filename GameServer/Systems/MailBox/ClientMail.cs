using VirusX.Game.MsgServer;
using System;

namespace VirusX.Game.MsgServer
{
    public struct ClientMail
    {
        public struct DBPrize
        {
            public unsafe fixed sbyte _title[64];
            public unsafe fixed sbyte _Sender_name[128];
            public unsafe fixed sbyte _content[512];
            public unsafe string title
            {
                get { fixed (sbyte* bp = _title) { return new string(bp); } }
                set
                {
                    string text = value;
                    fixed (sbyte* bp = _title)
                    {
                        for (int i = 0; i < text.Length; i++)
                            bp[i] = (sbyte)text[i];
                    }
                }
            }
            public unsafe string Sender_name
            {
                get { fixed (sbyte* bp = _Sender_name) { return new string(bp); } }
                set
                {
                    string text = value;
                    fixed (sbyte* bp = _Sender_name)
                    {
                        for (int i = 0; i < text.Length; i++)
                            bp[i] = (sbyte)text[i];
                    }
                }
            }
            public unsafe string content
            {
                get { fixed (sbyte* bp = _content) { return new string(bp); } }
                set
                {
                    string text = value;
                    fixed (sbyte* bp = _content)
                    {
                        for (int i = 0; i < text.Length; i++)
                            bp[i] = (sbyte)text[i];
                    }
                }
            }
            public uint id;
            public ulong Money;
            public uint EMoney;
            public uint itemid;
            public uint Emoney_Record_Type;
            public uint Flag;
            public long Timer;

           

          
            public DBPrize GetDBPrize(PrizeInfo obj)
            {
                title = obj.title;
                Sender_name = obj.Sender_name;
                content = obj.content;
                id = obj.id;
                Money = obj.Money;
                EMoney = obj.EMoney;
                itemid = obj.itemid;
                Emoney_Record_Type = obj.Emoney_Record_Type;
                Flag = obj.Flag;
                Timer = obj.TimeStamp.Ticks;
                return this;
            }

            public PrizeInfo GetClientPrize()
            {
                if (Timer > 0)
                {
                    if (DateTime.Now < DateTime.FromBinary(Timer))
                    {
                        PrizeInfo Mail = new PrizeInfo();
                        Mail.title = title;
                        Mail.Sender_name = Sender_name;
                        Mail.content = content;
                        Mail.id = id;
                        Mail.Money = Money;
                        Mail.EMoney = EMoney;
                        Mail.itemid = itemid;
                        Mail.Emoney_Record_Type = Emoney_Record_Type;
                        Mail.Flag = Flag;
                        Mail.TimeStamp = DateTime.FromBinary(Timer);
                        TimeSpan timeSpan = Mail.TimeStamp - DateTime.Now;
                        Mail.Expiration_Date = (uint)timeSpan.TotalMinutes * 60;
                        return Mail;
                    }
                }
                return null;
            }
        }
    }
}

