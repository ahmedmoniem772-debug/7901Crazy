using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using System.IO;

namespace MortalConquer.Game.MsgServer
{
    public static class MsgSpiritInteractive
    {
        [ProtoContract]
        public class MsgSpiritInteractiveProto
        {
            [ProtoMember(1, IsRequired = true)]
            public byte Type;
            [ProtoMember(2)]
            public uint[] Items;
        }
        public static unsafe ServerSockets.Packet CreateSpiritInteractive(this ServerSockets.Packet stream, MsgSpiritInteractiveProto obj)
        {
            stream.InitWriter();
            stream.ProtoBufferSerialize(obj);
            stream.Finalize(GamePackets.MsgSpiritInteractive);

            return stream;
        }
        public static unsafe void GetSpiritInteractive(this ServerSockets.Packet stream, out MsgSpiritInteractiveProto pQuery)
        {
            pQuery = new MsgSpiritInteractiveProto();
            pQuery = stream.ProtoBufferDeserialize<MsgSpiritInteractiveProto>(pQuery);
        }
        [PacketAttribute(GamePackets.MsgSpiritInteractive)]
        private unsafe static void Process(Client.GameClient client, ServerSockets.Packet stream)
        {
            MsgSpiritInteractiveProto Info;
            stream.GetSpiritInteractive(out Info);
            MsgGameItem item;
            if (!client.Player.TREPIN2 && client.Player.CheckPin())
            {
                client.Player.MessageBox("Please Active Pincode", null, null, 60);
                return;
            }
            if (Info.Type == 0)//Upgrade
            {
                if (client.Inventory.TryGetItem(Info.Items[0], out item))
                {
                    if (client.Inventory.Contain(item.ITEM_ID, 2) && item.ITEM_ID % 100 < 18)
                    {
                        if (!(item.ITEM_ID >= 4200001 && item.ITEM_ID <= 4200018))
                        {
                            client.Player.MessageBox("You Hacked ?? Good Bay Ya Baby :D #52 ", null, null, 60);
                            MyConsole.WriteLine("Player " + client.Player.Name + " Banned To Bug Anima.");

                            Database.SystemBannedAccount.AddBan(client.Player.UID, client.Player.Name, 99999, "Hack Anima.");
                            client.Socket.Disconnect();
                            return;
                        }
                        if (item.ITEM_ID >= 4200001 && item.ITEM_ID <= 4200005 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200006 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200007 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200008 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200009 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200010 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200011 && Role.Core.Rate(65)
                          || item.ITEM_ID == 4200012 && Role.Core.Rate(65)
                          || item.ITEM_ID == 4200013 && Role.Core.Rate(65)
                          || item.ITEM_ID == 4200014 && Role.Core.Rate(65)
                          || item.ITEM_ID == 4200015 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200016 && Role.Core.Rate(100)
                          || item.ITEM_ID == 4200017 && Role.Core.Rate(100))
                        {
                            client.Inventory.Remove(item.ITEM_ID, 2, stream);
                            var Chances = Database.SpiritTable.SpiritRates.Where(z => z.AnimaID == item.ITEM_ID).ToArray();
                            Chances = Chances.Where(z => z.Type != Database.SpiritTable.Type.Failed).ToArray();

                            Chances = Chances.OrderBy(q => q.NewAnimaID).ToArray();

                            if (Role.Core.Rate(0))
                            {
                                var r = Pool.GetRandom.Next(0, Chances.Count());
                                Chances = Chances.Where(q => q.Index == Chances.ToArray()[r].Index).ToArray();
                                String folderN2 = DateTime.Now.Year + "-" + DateTime.Now.Month,
             Path2 = "Cache\\Logs\\[AnimaPlaing]\\Susscful-Awakened\\",
             NewPath2 = System.IO.Path.Combine(Path2, folderN2);
                                if (!File.Exists(NewPath2 + folderN2))
                                {
                                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path2, folderN2));
                                }
                                if (!File.Exists(NewPath2 + "\\" + DateTime.Now.Day + ".txt"))
                                {
                                    using (System.IO.FileStream fs = System.IO.File.Create(NewPath2 + "\\" + DateTime.Now.Day + ".txt"))
                                    {
                                        fs.Close();
                                    }
                                }
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath2 + "\\" + DateTime.Now.Day + ".txt", true))
                                {
                                    file.WriteLine("UserName[Acc] |" + client.Player.Name + "| UIDAccount |" + client.Player.UID + " | New Anima | " + Chances.FirstOrDefault().NewAnimaID + " [The Clock] " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                                    file.Close();
                                }
                            }

                            client.Inventory.Add(stream, Chances.FirstOrDefault().NewAnimaID);

                            String folderN = DateTime.Now.Year + "-" + DateTime.Now.Month,
              Path = "Cache\\Logs\\[AnimaPlaing]\\Susscful\\",
              NewPath = System.IO.Path.Combine(Path, folderN);
                            if (!File.Exists(NewPath + folderN))
                            {
                                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                            }
                            if (!File.Exists(NewPath + "\\" + DateTime.Now.Day + ".txt"))
                            {
                                using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Day + ".txt"))
                                {
                                    fs.Close();
                                }
                            }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Day + ".txt", true))
                            {
                                file.WriteLine("UserName[Acc] |" + client.Player.Name + "| UIDAccount |" + client.Player.UID + " | New Anima | " + Chances.FirstOrDefault().NewAnimaID + " [The Clock] " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                                file.Close();
                            }
                            uint index2 = Chances.FirstOrDefault().Index;
                            if (item.ITEM_ID == 4200013)
                                index2 = 71;
                            if (item.ITEM_ID == 4200014)
                                index2 = 72;
                            if (item.ITEM_ID == 4200015)
                                index2 = 73;
                            Info.Items = new uint[3] { Info.Items[0], Info.Items[1], index2 };
                        }
                        else
                        {
                            var Chances = Database.SpiritTable.SpiritRates.Where(z => z.AnimaID == item.ITEM_ID).ToArray();
                            Chances = Chances.Where(q => q.Type == Database.SpiritTable.Type.Failed).ToArray();

                            if (Chances.FirstOrDefault().IsItem && Role.MyMath.Success(50))//Lost 1
                            {
                                client.Inventory.Remove(item.ITEM_ID, 1, stream);
                                client.GainExpBall(Chances.FirstOrDefault().NewAnimaID, false, Role.Flags.ExperienceEffect.angelwing);
                            }
                            else
                            {
                                client.Inventory.Remove(item.ITEM_ID, 1, stream);
                                client.CreateBoxDialog("Failure! You Lost 1 Anima ....");
                                client.GainExpBall(Chances.FirstOrDefault().NewAnimaID, false, Role.Flags.ExperienceEffect.angelwing);
                            }

                            String folderN = DateTime.Now.Year + "-" + DateTime.Now.Month,
          Path = "Cache\\Logs\\[AnimaPlaing]\\Failed\\",
          NewPath = System.IO.Path.Combine(Path, folderN);
                            if (!File.Exists(NewPath + folderN))
                            {
                                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                            }
                            if (!File.Exists(NewPath + "\\" + DateTime.Now.Day + ".txt"))
                            {
                                using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Day + ".txt"))
                                {
                                    fs.Close();
                                }
                            }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Day + ".txt", true))
                            {
                                file.WriteLine("UserName[Acc] |" + client.Player.Name + "| UIDAccount |" + client.Player.UID + " | Anima | " + item.UID + " [The Clock] " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                                file.Close();
                            }
                            uint index = Chances.FirstOrDefault().Index;
                            if (item.ITEM_ID >= 4200013)
                                index = 60;
                            Info.Items = new uint[3] { Info.Items[0], Info.Items[1], index };
                        }
                    }

                }
            }
            else if (Info.Type == 1)//Disassemble
            {
                for (int i = 0; i < Info.Items.Length; i++)
                {
                    if (client.Inventory.TryGetItem(Info.Items[i], out item))
                    {
                        #region log
                        String folderN = DateTime.Now.Year + "-" + DateTime.Now.Month,
   Path = "Cache\\Logs\\[AnimaPlaing]\\Disassemble\\",
   NewPath = System.IO.Path.Combine(Path, folderN);
                        if (!File.Exists(NewPath + folderN))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path, folderN));
                        }
                        if (!File.Exists(NewPath + "\\" + DateTime.Now.Day + ".txt"))
                        {
                            using (System.IO.FileStream fs = System.IO.File.Create(NewPath + "\\" + DateTime.Now.Day + ".txt"))
                            {
                                fs.Close();
                            }
                        }
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(NewPath + "\\" + DateTime.Now.Day + ".txt", true))
                        {
                            file.WriteLine("UserName[Acc] |" + client.Player.Name + "| UIDAccount |" + client.Player.UID + " | Anima | " + item.ITEM_ID + "| StackSize | " + item.StackSize.ToString() + " [The Clock] " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                            file.Close();
                        }
                        #endregion
                        if (!(item.ITEM_ID >= 4200001 && item.ITEM_ID <= 4200018))
                        {
                            client.Player.MessageBox("You Hacked ?? Good Bay Ya Baby :D #52 ", null, null, 60);
                            MyConsole.WriteLine("Player " + client.Player.Name + " Banned To Bug Anima.");

                            Database.SystemBannedAccount.AddBan(client.Player.UID, client.Player.Name, 99999, "Hack Anima.");
                            client.Socket.Disconnect();
                            return;
                        }
                        if (client.Inventory.AddItemWitchStack((uint)(item.ITEM_ID - 1), 0, (byte)(item.StackSize * 2), stream, item.Bound > 0) && client.Inventory.RemoveStackItem(item.UID, item.StackSize, stream))
                            Info.Items[i] = 1;
                        else
                        {
                            Info.Items[i] = 0;
                            client.SendSysMesage("Please empty some space in your inventory.");
                        }

                    }
                }
            }
            client.Send(stream.CreateSpiritInteractive(Info));
        }
    }

}
