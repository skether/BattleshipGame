using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    public class MatchResult
    {
        public int Id { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
        public DateTime PlayedAt { get; set; }

        public MatchResult() { }

        public MatchResult(string winner, string looser, DateTime time)
        {
            Winner = winner;
            Looser = looser;
            PlayedAt = time;
        }
    }

    public class PlayerStatistics
    {
        public string Name { get; private set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Played { get { return Won + Lost; } }
        public double WinRate { get { return Won / (double)Played; } }

        public PlayerStatistics(string name)
        {
            Name = name;
        }
    }

    public static class Database
    {
        private const string dbFile = "history.db";

        public static void AddResult(MatchResult result)
        {
            using (LiteDatabase db = new LiteDatabase(dbFile))
            {
                LiteCollection<MatchResult> collection = db.GetCollection<MatchResult>("History");
                collection.Insert(result);
            }
        }

        public static IEnumerable<MatchResult> GetHistory()
        {
            using (LiteDatabase db = new LiteDatabase(dbFile))
            {
                LiteCollection<MatchResult> collection = db.GetCollection<MatchResult>("History");
                return collection.FindAll().OrderByDescending(x => x.PlayedAt);
            }
        }

        public static IEnumerable<PlayerStatistics> GetLeaderBoard()
        {
            Dictionary<string, PlayerStatistics> stats = new Dictionary<string, PlayerStatistics>();
            foreach (MatchResult item in GetHistory())
            {
                if (stats.ContainsKey(item.Winner)) stats[item.Winner].Won++;
                else stats.Add(item.Winner, new PlayerStatistics(item.Winner) { Won = 1});

                if (stats.ContainsKey(item.Looser)) stats[item.Looser].Lost++;
                else stats.Add(item.Looser, new PlayerStatistics(item.Looser) { Lost = 1 });
            }
            return stats.Values.ToList<PlayerStatistics>().OrderByDescending(x=> x.WinRate).ThenBy(y=> y.Name);
        }
    }
}
