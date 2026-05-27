using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Game.MsgServer;

namespace VirusX.Role.Instance
{
    //questid = 10 <- An~accessory in bird 
    //questid = 11 <- DesertBloodExp in desert
    public class Quests
    {



        public Role.Player Player;
        public Dictionary<uint, Game.MsgServer.MsgQuestList.QuestListItem> src;
        public Dictionary<uint, Game.MsgServer.MsgQuestList.QuestListItem> AcceptedQuests;

        public Quests(Role.Player _owner)
        {
            Player = _owner;
            AcceptedQuests = new Dictionary<uint, MsgQuestList.QuestListItem>();
            src = new Dictionary<uint, Game.MsgServer.MsgQuestList.QuestListItem>();

        }
        public unsafe void SetKingDomQuestObjectives(ServerSockets.Packet stream, uint UID, params uint[] Intentions)
        {
            if (Database.QuestInfo.IsKingDomMission(UID))
            {
                if (src.ContainsKey(UID))
                {
                    var item = src[UID];
                    if (item.Status != MsgQuestList.QuestListItem.QuestStatus.Finished)
                    {
                        if (Intentions.Length > item.Intentions.Length)//check the size.
                        {
                            var _Intentions = new uint[item.Intentions.Length];
                            for (int x = 0; x < _Intentions.Length; x++)
                                _Intentions[x] = item.Intentions[x];
                            item.Intentions = new uint[Intentions.Length];
                            for (int x = 0; x < _Intentions.Length; x++)
                                item.Intentions[x] = _Intentions[x];
                        }
                        for (int x = 0; x < Intentions.Length; x++)
                            item.Intentions[x] = Intentions[x];

                        src[UID] = item;
                        Player.Owner.Send(stream.MsgQuestDataCreate(0, UID, item.Intentions));
                    }
                }
                return;
            }
        }
        public void GetQuestObjectives(uint UID, out uint[] Intentions)
        {
            if (src.ContainsKey(UID))
            {
                Intentions = new uint[src[UID].Intentions.Length];
                for (int x = 0; x < src[UID].Intentions.Length; x++)
                    Intentions[x] = src[UID].Intentions[x];
            }
            else
                Intentions = new uint[1];
        }
        public void SetQuestObjectives(ServerSockets.Packet stream, uint UID, params uint[] Intentions)
        {
            if (src.ContainsKey(UID) && IsActiveQuest(UID))
            {
                var item = src[UID];
                if (Intentions.Length > item.Intentions.Length)//check the size.
                {
                    var _Intentions = new uint[item.Intentions.Length];
                    for (int x = 0; x < _Intentions.Length; x++)
                        _Intentions[x] = item.Intentions[x];
                    item.Intentions = new uint[Intentions.Length];
                    for (int x = 0; x < _Intentions.Length; x++)
                        item.Intentions[x] = _Intentions[x];
                }
                for (int x = 0; x < Intentions.Length; x++)
                    item.Intentions[x] = Intentions[x];

                src[UID] = item;
                Player.Owner.Send(stream.MsgQuestDataCreate(0, UID, item.Intentions));
            }
        }
        public unsafe void IncreaseQuestObjectives(ServerSockets.Packet stream, uint UID, params uint[] Intentions)
        {

            if (Database.QuestInfo.IsKingDomMission(UID))
            {
                if (src.ContainsKey(UID))
                {
                    var item = src[UID];
                    if (item.Status != MsgQuestList.QuestListItem.QuestStatus.Finished)
                    {
                        if (Intentions.Length > item.Intentions.Length)//check the size.
                        {
                            var _Intentions = new uint[item.Intentions.Length];
                            for (int x = 0; x < _Intentions.Length; x++)
                                _Intentions[x] = item.Intentions[x];
                            item.Intentions = new uint[Intentions.Length];
                            for (int x = 0; x < _Intentions.Length; x++)
                                item.Intentions[x] = _Intentions[x];
                        }
                        for (int x = 0; x < Intentions.Length; x++)
                            item.Intentions[x] += Intentions[x];

                        src[UID] = item;
                        Player.Owner.Send(stream.MsgQuestDataCreate(0, UID, item.Intentions));
                    }
                }
                return;
            }
            if (IsActiveQuest(UID) && AcceptedQuests.ContainsKey(UID))
            {
                var item = AcceptedQuests[UID];
                if (Intentions.Length > item.Intentions.Length)//check the size.
                {
                    var _Intentions = new uint[item.Intentions.Length];
                    for (int x = 0; x < _Intentions.Length; x++)
                        _Intentions[x] = item.Intentions[x];
                    item.Intentions = new uint[Intentions.Length];
                    for (int x = 0; x < _Intentions.Length; x++)
                        item.Intentions[x] = _Intentions[x];
                }
                for (int x = 0; x < Intentions.Length; x++)
                    item.Intentions[x] += Intentions[x];

                AcceptedQuests[UID] = item;
                Player.Owner.Send(stream.MsgQuestDataCreate(0, UID, item.Intentions));
            }
        }

        public unsafe void SendAutoPatcher(string Text, uint map, ushort x, ushort y, uint NpcUid)
        {
            Player.MessageBox(Text, p =>
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();

                    ActionQuery action = new ActionQuery()
                    {
                        ObjId = p.Player.UID,
                        Type = ActionType.AutoPatcher,
                        NpcID = (int)NpcUid,
                        PositionX = x,
                        PositionY = y,
                        MapID = map
                        
                    };
                    p.Send(stream.ActionCreate(action));


                }

            }, null, 0);
        }
        public unsafe void SendAutoPatcher(uint map, ushort x, ushort y, uint NpcUid)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                ActionQuery action = new ActionQuery()
                {
                    ObjId = Player.UID,
                    Type = ActionType.AutoPatcher,
                    NpcID = (int)NpcUid,
                    PositionX = x,
                    PositionY = y,
                    MapID = map
                };
                Player.Send(stream.ActionCreate(action));
            }
        }
        public void RemoveQuest(uint UID)
        {
            if (src.ContainsKey(UID))
                src.Remove(UID);
            if (AcceptedQuests.ContainsKey(UID))
                AcceptedQuests.Remove(UID);
            var n_quest = new Game.MsgServer.MsgQuestList.QuestListItem()
            {
                UID = UID,
                Status = MsgQuestList.QuestListItem.QuestStatus.Available,
                Time = 0
            };
            SendSinglePacket(n_quest, Game.MsgServer.MsgQuestList.QuestMode.CancelBountyHall);
        }
        public bool IsActiveQuest(uint UID)
        {
            return CheckQuest(UID, Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus.Accepted);
        }
        public bool CheckKingDomQuest(uint UID, params uint[] Intentions)
        {
            if (src.ContainsKey(UID))
            {
                try
                {
                    var quest = src[UID];
                    bool isdone = true;
                    for (int x = 0; x < Intentions.Length; x++)
                        if (quest.Intentions[x] < Intentions[x])
                            isdone = false;
                    return isdone;
                }
                catch
                {
                    for (int x = 0; x < 10; x++)
                        MyConsole.WriteLine("Error on quest : " + UID);
                }
              
            }
            return false;
        }
        public bool CheckObjectives(uint UID, params uint[] Intentions)
        {
            if (AcceptedQuests.ContainsKey(UID))
            {
                try
                {
                    var quest = AcceptedQuests[UID];
                    bool isdone = true;
                    for (int x = 0; x < Intentions.Length; x++)
                        if (quest.Intentions[x] < Intentions[x])
                            isdone = false;
                    return isdone;
                }
                catch
                {
                    for (int x = 0; x < 10; x++)
                        MyConsole.WriteLine("Error on quest : " + UID);
                }
                
            }
            
            return false;
           
        }
        public bool CheckQuest(uint UID, Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus status)
        {
            if (Database.QuestInfo.IsKingDomMission(UID))
            {
                return src.Values.Where((p) => p.UID == UID && p.Status == status).Count() == 1;
            }
            if (status == MsgQuestList.QuestListItem.QuestStatus.Accepted)
            {
                return AcceptedQuests.ContainsKey(UID);
            }
            return src.Values.Where((p) => p.UID == UID && p.Status == status).Count() == 1;
        }
        public bool FinishQuest(uint UID)
        {
            if (src.ContainsKey(UID))
            {
                var item = src[UID];
                if (item.Status != Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus.Finished)
                {
                    item.Status = Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus.Finished;
                    src[UID] = item;
                    if (AcceptedQuests.ContainsKey(UID))
                        AcceptedQuests.Remove(UID);
                    SendSinglePacket(item, Game.MsgServer.MsgQuestList.QuestMode.FinishQuest);
                    return true;
                }
            }
            return false;
        }

        public bool AcceptKingDomMission(Database.QuestInfo.DBQuest quest, uint time, out Game.MsgServer.MsgQuestList.QuestListItem _quest)
        {
            _quest = null;
            if (Database.QuestInfo.IsKingDomMission(quest.MissionId))
            {
                if (!src.ContainsKey(quest.MissionId))
                {
                    var n_quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                    {
                        UID = quest.MissionId,
                        Status = MsgQuestList.QuestListItem.QuestStatus.Accepted,
                        Time = time,
                        Intentions = new uint[quest.Intentions]
                    };
                    src.Add(quest.MissionId, n_quest);
                    SendSinglePacket(n_quest, Game.MsgServer.MsgQuestList.QuestMode.AcceptQuest);
                    _quest = n_quest;
                    return true;
                }
                else
                {
                    _quest = src[quest.MissionId];
                    return src[quest.MissionId].Status != MsgQuestList.QuestListItem.QuestStatus.Finished;
                }
            }
            return false;
        }


        public unsafe bool Accept(Database.QuestInfo.DBQuest quest, uint time)
        {


            if (!src.ContainsKey(quest.MissionId))
            {
                if (quest.MissionId == 1469)
                    quest.Intentions = 4;
                if (quest.MissionId == 409)
                    quest.Intentions = 3;
                var n_quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                {
                    UID = quest.MissionId,
                    Status = MsgQuestList.QuestListItem.QuestStatus.Accepted,
                    Time = time,
                    Intentions = new uint[quest.Intentions]
                };
                AcceptedQuests.Add(n_quest.UID, n_quest);
                src.Add(n_quest.UID, n_quest);
                SendSinglePacket(n_quest, Game.MsgServer.MsgQuestList.QuestMode.AcceptQuest);
            }
            return true;
        }
        public unsafe bool AcceptBountyHall(Database.QuestInfo.DBQuest quest, uint time)
        {
            if (!src.ContainsKey(quest.MissionId))
            {
                var n_quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                {
                    UID = quest.MissionId,
                    Status = MsgQuestList.QuestListItem.QuestStatus.Accepted,
                    Time = time,
                    Intentions = new uint[quest.Intentions]
                };
                AcceptedQuests.Add(n_quest.UID, n_quest);
                src.Add(n_quest.UID, n_quest);
                SendSinglePacket(n_quest, Game.MsgServer.MsgQuestList.QuestMode.AcceptBountyHall);
            }
            return true;
        }
        public unsafe void SendSinglePacket(Game.MsgServer.MsgQuestList.QuestListItem data, Game.MsgServer.MsgQuestList.QuestMode mode)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();

                stream.QuestListCreate(mode, 1);
                stream.AddItemQuestList(data);
                Player.Owner.Send(stream.QuestListFinalize());
                if (mode == MsgQuestList.QuestMode.FinishQuest)
                {
                    if (Player.Level < 130)
                        Player.Owner.GainExpBall(600, false, Flags.ExperienceEffect.None, false, false);
                }
            }
        }

        public int AcceptQuestsCount()
        {
            return src.Values.Where((p) => p.Status == Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus.Accepted).Count();
        }
        public bool AllowAccept()
        {
            return AcceptQuestsCount() < 5;
        }
        public unsafe void SendFullGUI(ServerSockets.Packet stream)
        {
            if (Database.AtributesStatus.IsLee(Player.Class))
            {
                if (!src.ContainsKey(6435))//first Quest
                {
                    var n_quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                    {
                        UID = 6435,
                        Status = Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus.Accepted,
                        Time = 0

                    };
                    AcceptedQuests.Add(n_quest.UID, n_quest);
                    src.Add(n_quest.UID, n_quest);
#if Arabic
                    SendAutoPatcher("Son of Dragon, you're destined for the Power of Dragon. Pay a visit to the previous holder, Kungfu King, for more details", 1002, 269, 217, 17222);
#else
                    SendAutoPatcher("Son of Dragon, you're destined for the Power of Dragon. Pay a visit to the previous holder, Kungfu King, for more details", 1002, 269, 217, 17222);
#endif

                }
            }
            else
            {
                if (!src.ContainsKey(804))//first Quest 804
                {
                    var n_quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                    {
                        UID = 804,
                        Status = Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus.Accepted,
                        Time = 0

                    };
                    AcceptedQuests.Add(n_quest.UID, n_quest);
                    src.Add(n_quest.UID, n_quest);
#if Arabic
                      SendAutoPatcher("Welcome!~Pay~a~visit~to~the~Kungfu~Boy~to~learn~more~about~the~world!", 1002, 270, 223, 5673);
#else
                    SendAutoPatcher("Welcome!~Pay~a~visit~to~the~Kungfu~Boy~to~learn~more~about~the~world!", 1002, 270, 223, 5673);
#endif

                }
            }
            Dictionary<int, Queue<Game.MsgServer.MsgQuestList.QuestListItem>> Collection = new Dictionary<int, Queue<Game.MsgServer.MsgQuestList.QuestListItem>>();
            Collection.Add(0, new Queue<Game.MsgServer.MsgQuestList.QuestListItem>());

            int count = 0;
            var Array = Database.QuestInfo.AllQuests.Values.ToArray();
            for (uint x = 0; x < Array.Length; x++)
            {
                if (x % 80 == 0)
                {
                    count++;
                    Collection.Add(count, new Queue<Game.MsgServer.MsgQuestList.QuestListItem>());
                }
                if (src.ContainsKey(Array[x].MissionId))
                    Collection[count].Enqueue(src[Array[x].MissionId]);
                else
                {
                    var quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                    {
                        UID = Array[x].MissionId,
                        Status = Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus.Available,
                        Time = 0
                    };
                    Collection[count].Enqueue(quest);
                }
            }



            foreach (var aray in Collection.Values)
            {
                Queue<Game.MsgServer.MsgQuestList.QuestListItem> ItemArray = aray;

                ushort CCount = (ushort)ItemArray.Count;
                stream.QuestListCreate(MsgQuestList.QuestMode.Review, CCount);
                for (byte x = 0; x < CCount; x++)
                {
                    stream.AddItemQuestList(ItemArray.Dequeue());
                }
                Player.Owner.Send(stream.QuestListFinalize());
            }
        }
        public unsafe void ShowAll(ServerSockets.Packet stream)
        {
            Dictionary<int, Queue<Game.MsgServer.MsgQuestList.QuestListItem>> Collection = new Dictionary<int, Queue<Game.MsgServer.MsgQuestList.QuestListItem>>();
            Collection.Add(0, new Queue<Game.MsgServer.MsgQuestList.QuestListItem>());

            int count = 0;
            var Array = Database.QuestInfo.AllQuests.Values.Where(p => p.MissionId >= 590000).ToArray();
            for (uint x = 0; x < Array.Length; x++)
            {
                count++;
                Collection.Add(count, new Queue<Game.MsgServer.MsgQuestList.QuestListItem>());
                if (src.ContainsKey(Array[x].MissionId))
                    Collection[count].Enqueue(src[Array[x].MissionId]);
                else
                {
                    var quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                    {
                        UID = Array[x].MissionId,
                        Status = (Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus)6,
                        Time = 0
                    };
                    Collection[count].Enqueue(quest);
                }
            }
            var info = Collection.Values.Where(p => p.Count == 1).ToArray().Take(3);
            stream.QuestListCreate(MsgQuestList.QuestMode.ShowBountyHall, 3);
            foreach (var aray in info)
            {
                stream.AddItemQuestList(aray.Dequeue());
            }
            Player.Owner.Send(stream.QuestListFinalize());
        
            //foreach(var x in Array)
            //{
            //   // Console.WriteLine("MissionId:{0} TypeId:{1} Name:{2}", x.MissionId, x.TypeId, x.Name); 
            //}

            //stream.QuestListCreate(MsgQuestList.QuestMode.ShowBountyHall, 3);
            ////TypeId=5
            ////TypeBigId=2
            ////TaskNameColor=0xFFFFFFFF
            ////CompleteFlag=2
            ////ActivityType=0
            ////MissionId=590023
            //stream.AddItemQuestList(new MsgQuestList.QuestListItem { UID = 590023, Status = (MsgQuestList.QuestListItem.QuestStatus)6 });
            //stream.AddItemQuestList(new MsgQuestList.QuestListItem { UID = 590000, Status = (MsgQuestList.QuestListItem.QuestStatus)6 });
            ////TypeId=3
            ////TypeBigId=2
            ////TaskNameColor=0xFFFFFFFF
            ////CompleteFlag=2
            ////ActivityType=0
            ////MissionId=590122
            //stream.AddItemQuestList(new MsgQuestList.QuestListItem { UID = 590122, Status = (MsgQuestList.QuestListItem.QuestStatus)6 });
            //Player.Owner.Send(stream.QuestListFinalize());

        }
        public unsafe void RefreshBountyHall(ServerSockets.Packet stream)
        {
            Dictionary<int, Queue<Game.MsgServer.MsgQuestList.QuestListItem>> Collection = new Dictionary<int, Queue<Game.MsgServer.MsgQuestList.QuestListItem>>();
            Collection.Add(0, new Queue<Game.MsgServer.MsgQuestList.QuestListItem>());

            int count = 0;
            var Array = Database.QuestInfo.AllQuests.Values.Where(p => p.MissionId >= 590000).ToArray();
            for (uint x = 0; x < Array.Length; x++)
            {
                count++;
                Collection.Add(count, new Queue<Game.MsgServer.MsgQuestList.QuestListItem>());
                if (src.ContainsKey(Array[x].MissionId))
                    Collection[count].Enqueue(src[Array[x].MissionId]);
                else
                {
                    var quest = new Game.MsgServer.MsgQuestList.QuestListItem()
                    {
                        UID = Array[x].MissionId,
                        Status = (Game.MsgServer.MsgQuestList.QuestListItem.QuestStatus)6,
                        Time = 0
                    };
                    Collection[count].Enqueue(quest);
                }
            }
            var info = Collection.Values.Where(p => p.Count == 1).ToArray().Take(3);
            stream.QuestListCreate(MsgQuestList.QuestMode.RefreshBountyHall, 3);
            foreach (var aray in info)
            {
                stream.AddItemQuestList(aray.Dequeue());
            }
            Player.Owner.Send(stream.QuestListFinalize());
        }
    }
}
