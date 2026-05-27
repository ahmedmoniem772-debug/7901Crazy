using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgServer
{
    public unsafe static partial class MsgBuilder
    {

        public static void GetHeroInfo(this ServerSockets.Packet stream, Client.GameClient Owner, out Role.Player user)
        {
            user = new Role.Player(Owner);
            user.RealUID = stream.ReadUInt32();
            user.AparenceType = stream.ReadUInt16();
            uint mesh = stream.ReadUInt32();
            user.Body = (ushort)(mesh % 10000);
            user.Face = (ushort)((mesh - user.Body) / 10000);
            user.Hair = stream.ReadUInt32();
            user.Money = stream.ReadInt64();
            user.ConquerPoints = stream.ReadUInt32();
            user.Experience = stream.ReadUInt64();
            user.ServerID = (ushort)stream.ReadUInt16();
            user.SetLocationType = (ushort)stream.ReadUInt16();
            user.SpecialTitleID = stream.ReadUInt32();
            user.SpecialWingID = stream.ReadUInt32();
            user.VirtutePoints = stream.ReadUInt32();
            user.HeavenBlessing = stream.ReadInt32();
            user.Strength = stream.ReadUInt16();
            user.Agility = stream.ReadUInt16();
            user.Vitality = stream.ReadUInt16();
            user.Spirit = stream.ReadUInt16();
            user.Atributes = stream.ReadUInt16();
            user.HitPoints = stream.ReadInt32();
            user.Mana = stream.ReadUInt16();
            user.PKPoints = stream.ReadUInt16();
            user.Level = stream.ReadUInt8();
            user.Class = stream.ReadUInt32();
            user.FirstClass = stream.ReadUInt32();
            user.SecoundeClass = stream.ReadUInt32();
            user.NobilityRank = (Role.Instance.Nobility.NobilityRank)stream.ReadUInt8();
            user.Reborn = stream.ReadUInt8();
            user.InitTransfer = stream.ReadUInt8();
            user.QuizPoints = stream.ReadUInt32();
            stream.SeekForward(sizeof(uint));
            user.Enilghten = stream.ReadUInt16();
            user.EnlightenReceive = (ushort)(stream.ReadUInt16() / 100);
            stream.SeekForward(sizeof(uint));
            user.VipLevel = (byte)stream.ReadUInt32();
            user.MyTitle = (byte)stream.ReadUInt16();
            user.BoundConquerPoints = stream.ReadInt32();
            stream.SeekForward(sizeof(byte));
            stream.SeekForward(sizeof(ulong));
            stream.SeekForward(sizeof(uint));
            stream.SeekForward(sizeof(ushort));
            stream.SeekForward(sizeof(uint));
            stream.SeekForward(sizeof(ulong));
            stream.SeekForward(sizeof(uint));
            stream.SeekForward(sizeof(uint));
            stream.SeekForward(sizeof(uint));
            string[] strs = stream.ReadStringList();
            user.Name = strs[0];
            user.Spouse = strs[1];
        }
        public static unsafe ServerSockets.Packet HeroInfo(this ServerSockets.Packet stream, Role.Player client, int inittransfer = 0)
        {
            stream.InitWriter();//0
            stream.Write(client.UID);//4
            stream.Write((ushort)client.AparenceType);//8
            stream.Write(client.Mesh);//10
            stream.Write(client.Hair);//14
            stream.Write((ulong)client.Money);//18
            stream.Write(client.ConquerPoints);//26
            stream.Write(client.Experience);//30
            stream.Write((ushort)Database.GroupServerList.MyServerInfo.ID);//38
            stream.Write(client.SetLocationType);//40
            stream.Write(client.SpecialTitleID);//42
            stream.Write(client.SpecialWingID);//46
            stream.Write(client.VirtutePoints);//50
            stream.Write(client.HeavenBlessing);//54
            stream.Write(client.Strength);//58
            stream.Write(client.Agility);//60
            stream.Write(client.Vitality);//62
            stream.Write(client.Spirit);//64
            stream.Write(client.Atributes);//66
            stream.Write(client.HitPoints);//68
            stream.Write(client.Mana);//72
            stream.Write(client.PKPoints);//74
            stream.Write((byte)client.Level);//76
            stream.Write(client.Class);//77
            stream.Write(client.FirstClass);//81
            stream.Write(client.SecoundeClass);//85
            stream.Write((byte)client.NobilityRank);//89
            stream.Write(client.Reborn);//90
            stream.Write((byte)inittransfer);//91
            stream.Write(client.QuizPoints);//92
            stream.Write((uint)client.MainFlag);//96
            stream.Write(client.Enilghten);//100
            stream.Write((ushort)(client.EnlightenReceive * 100));//102
            stream.Write((uint)0);//104
            stream.Write((uint)client.VipLevel);//108
            stream.Write((ushort)client.MyTitle);//112
            stream.Write(client.BoundConquerPoints);//114
            if (client.SubClass != null)
            {
                stream.Write((byte)client.ActiveSublass);//118
                stream.Write((ulong)client.SubClass.GetHashPoint());//119
            }
            else
                stream.ZeroFill(9);
            stream.Write(client.RacePoints);//127
            stream.Write((ushort)client.CountryID);//131
            stream.Write((uint)0);//133
            stream.Write((ulong)client.UID);//137
            stream.Write((uint)0);//145
            stream.Write((uint)0);//149
            stream.Write((uint)0);//153
            stream.Write(client.Name, "", client.Spouse);//157
            stream.Finalize(GamePackets.HeroInfo);
            return stream;
        }

    }

}
