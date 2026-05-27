using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Client;

namespace VirusX.Game.MsgServer
{
    public static class AgateEx
    {
        public static void GetAgate(this ServerSockets.Packet stream, out AgateEx.AgateActions action, out uint UID, out sbyte Map)
        {
            action = (AgateEx.AgateActions)stream.ReadUInt32();
            UID = stream.ReadUInt32();
            Map = stream.ReadInt8();
        }

        public enum AgateActions
        {
            Record = 1,
            Recall = 3,
            Repair = 4,
        }
    }
    public class MsgMemoryAgate
    {
        public static readonly List<uint> revnomap = new List<uint>(){(uint)1,(uint) 2,(uint) 3,2060U,7009U,2060U,1950U,1005U,7005U,7006U,7008U,6000U,6004U,6001U,6002U,6003U,1844U,7001U,1801U,1508U,1518U,7777U,8877U,3333U,1090U,1225U,1860U,700U,3073U,2064U,1038U};
        public static List<uint> BlockedMaps = new List<uint>(){2071U,1005U,1036U,2056U};

       [PacketAttribute(GamePackets.MsgSuperFlag)]
        public static void MemoryAgateHandler(GameClient client, ServerSockets.Packet stream)
        {
            AgateEx.AgateActions action;
            uint UID;
            sbyte Map;
            stream.GetAgate(out action, out UID, out Map);
            switch (action)
            {
                case AgateEx.AgateActions.Record:
                    {
                        if (!client.Player.Alive || client.Player.DeadState || client.Player.DynamicID > 0U)
                            break;
                        MsgGameItem msgItemInfo1 = null;
                        if (MsgMemoryAgate.revnomap.Contains(client.Player.Map) || client.Player.Map == 1210U || client.Player.Map == 1700U || !client.Inventory.TryGetItem(UID, out msgItemInfo1))
                            break;
                        if (msgItemInfo1.Agate_map.ContainsKey((uint)Map))
                        {
                            if (MsgMemoryAgate.BlockedMaps.Contains(client.Player.Map))
                                break;
                            Dictionary<uint, string> agateMap = msgItemInfo1.Agate_map;
                            int num1 = (int)Map;
                            string[] strArray = new string[5]
            {
              client.Player.Map.ToString(),
              "~",
              null,
              null,
              null
            };
                            ushort num2 = client.Player.X;
                            strArray[2] = num2.ToString();
                            strArray[3] = "~";
                            num2 = client.Player.Y;
                            strArray[4] = num2.ToString();
                            string str = string.Concat(strArray);
                            agateMap[(uint)num1] = str;
                            msgItemInfo1.SendAgate(client);
                            break;
                        }
                        if ((int)Map > msgItemInfo1.Agate_map.Count)
                        {
                            msgItemInfo1.Agate_map.Add((uint)(byte)msgItemInfo1.Agate_map.Count, client.Player.Map.ToString() + "~" + client.Player.X.ToString() + "~" + client.Player.Y.ToString());
                            msgItemInfo1.SendAgate(client);
                            break;
                        }
                        if (msgItemInfo1.Agate_map.ContainsKey((uint)Map) || MsgMemoryAgate.BlockedMaps.Contains(client.Player.Map))
                            break;
                        Dictionary<uint, string> agateMap1 = msgItemInfo1.Agate_map;
                        int num3 = (int)Map;
                        string[] strArray1 = new string[5]
          {
            client.Player.Map.ToString(),
            "~",
            null,
            null,
            null
          };
                        ushort num4 = client.Player.X;
                        strArray1[2] = num4.ToString();
                        strArray1[3] = "~";
                        num4 = client.Player.Y;
                        strArray1[4] = num4.ToString();
                        string str1 = string.Concat(strArray1);
                        agateMap1.Add((uint)num3, str1);
                        msgItemInfo1.SendAgate(client);
                        break;
                    }
                case AgateEx.AgateActions.Recall:
                    {
                        if (!client.Player.Alive || client.Player.DeadState || client.Player.DynamicID > 0U)
                            break;
                        MsgGameItem msgItemInfo2 = (MsgGameItem)null;
                        if (MsgMemoryAgate.revnomap.Contains(client.Player.Map) || client.Player.Map == 1210U || client.Player.Map == 1700U || !client.Inventory.TryGetItem(UID, out msgItemInfo2) || !msgItemInfo2.Agate_map.ContainsKey((uint)Map) || ushort.Parse(msgItemInfo2.Agate_map[(uint)Map].Split('~')[0].ToString()) == (ushort)1038 || ushort.Parse(msgItemInfo2.Agate_map[(uint)Map].Split('~')[0].ToString()) == (ushort)6001)
                            break;
                        ushort num5 = ushort.Parse(msgItemInfo2.Agate_map[(uint)Map].Split('~')[0].ToString());
                        if (MsgMemoryAgate.BlockedMaps.Contains((uint)num5))
                            break;
                        client.Teleport(ushort.Parse(msgItemInfo2.Agate_map[(uint)Map].Split('~')[1].ToString()), ushort.Parse(msgItemInfo2.Agate_map[(uint)Map].Split('~')[2].ToString()), (uint)ushort.Parse(msgItemInfo2.Agate_map[(uint)Map].Split('~')[0].ToString()));
                        --msgItemInfo2.Durability;
                        msgItemInfo2.SendAgate(client);
                        break;
                    }
                case AgateEx.AgateActions.Repair:
                    {
                        if (client.Player.DeadState || !client.Player.Alive)
                            break;
                        MsgGameItem msgItemInfo3 = (MsgGameItem)null;
                        if (!client.Inventory.TryGetItem(UID, out msgItemInfo3))
                            break;
                        uint num6 = ((uint)msgItemInfo3.MaximDurability - (uint)msgItemInfo3.Durability) / 2U;
                        if (num6 == 0U)
                            num6 = 1U;
                        if (client.Player.ConquerPoints > num6)
                        {
                            client.Player.ConquerPoints -= (long)num6;
                            msgItemInfo3.Durability = msgItemInfo3.MaximDurability;
                            msgItemInfo3.SendAgate(client);
                        }
                        break;
                    }
            }
        }

    }
}
