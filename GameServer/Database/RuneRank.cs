using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Database
{
    public class RuneRank
    {
        public const int MaxPlayers = 100;
        public class Entry
        {
            public uint TotalPoints = 0;
            public string Name = "";
        }
        public static Dictionary<uint, Entry> RankingList;
        public static object SynRoot = new object();

        public static void Load()
        {
            lock (SynRoot)
            {
                RankingList = new Dictionary<uint, Entry>();
                using (var reader = new DBActions.Read("RuneRanking.txt", false))
                {
                    if (reader.Reader())
                    {
                        int count = reader.Count;
                        for (int x = 0; x < count; x++)
                        {
                            var line = new DBActions.ReadLine(reader.ReadString("/"), '/');
                            var item = new Entry();
                            item.Name = line.Read("");
                            item.TotalPoints = line.Read((uint)0);
                            RankingList.Add(line.Read((uint)0), item);
                        }
                    }
                }
                RankingList = RankingList.Where(i => i.Value.TotalPoints >= 10000 && !i.Value.Name.Contains("[Bot]") && !i.Value.Name.Contains("[Shop]")  && !i.Value.Name.Contains("[GM]") && !i.Value.Name.Contains("[PM]")).OrderByDescending(i => i.Value.TotalPoints).Take(MaxPlayers).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }
         public static void Update(Client.GameClient client)
        {
            lock (SynRoot)
            {
                if (RankingList.ContainsKey(client.Player.UID))
                {
                    RankingList[client.Player.UID].TotalPoints = client.Rune.Score;
                    RankingList[client.Player.UID].Name = client.Player.Name;
                }
                else RankingList.Add(client.Player.UID, new Entry()
                {
                    Name = client.Player.Name,
                    TotalPoints = client.Rune.Score
                });
                RankingList = RankingList.Where(i => i.Value.TotalPoints >= 10000 && !i.Value.Name.Contains("[Bot]") && !i.Value.Name.Contains("[Shop]")  && !i.Value.Name.Contains("[GM]") && !i.Value.Name.Contains("[PM]")).OrderByDescending(i => i.Value.TotalPoints).Take(MaxPlayers).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }
        public static void Save()
        {
            lock (SynRoot)
                RankingList = RankingList.Where(i => i.Value.TotalPoints >= 10000 && !i.Value.Name.Contains("[Bot]") && !i.Value.Name.Contains("[Shop]") && !i.Value.Name.Contains("[GM]") && !i.Value.Name.Contains("[PM]")).OrderByDescending(i => i.Value.TotalPoints).Take(MaxPlayers).ToDictionary(pair => pair.Key, pair => pair.Value);
            using (var writer = new DBActions.Write("RuneRanking.txt"))
            {
                foreach (var rank in RankingList)
                {
                    if (rank.Key >= 3999900001)
                        continue;
                    var line = new DBActions.WriteLine('/');
                    if (!rank.Value.Name.Contains("[Bot]")  && !rank.Value.Name.Contains("[Shop]") && !rank.Value.Name.Contains("[GM]") && !rank.Value.Name.Contains("[PM]"))
                    {
                        line.Add(rank.Value.Name).Add(rank.Value.TotalPoints).Add(rank.Key);
                        writer.Add(line.Close());
                    }
                }
                writer.Execute(DBActions.Mode.Open);
            }
        }
        public static int GetPlayerRank(uint playerUID)
        {
            lock (SynRoot)
            {
                if (RankingList == null || !RankingList.ContainsKey(playerUID))
                    return 0;

                // Get the position (rank) of the player
                int rank = 1;
                foreach (var entry in RankingList)
                {
                    if (entry.Key == playerUID)
                        return rank;
                    rank++;
                }
                return 0;
            }
        }

        // Optional: Get rank by player name
        public static int GetPlayerRank(string playerName)
        {
            lock (SynRoot)
            {
                if (RankingList == null)
                    return 0;

                int rank = 1;
                foreach (var entry in RankingList)
                {
                    if (entry.Value.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase))
                        return rank;
                    rank++;
                }
                return 0;
            }
        }
    }
}
