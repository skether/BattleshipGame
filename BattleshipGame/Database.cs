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
        public string Winner { get; set; }
        public string Looser { get; set; }
        public DateTime PlayedAt { get; set; }

        public MatchResult(string winner, string looser, DateTime time)
        {
            Winner = winner;
            Looser = looser;
            PlayedAt = time;
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
                return collection.FindAll();
            }
        }
    }
}
