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
}
