using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Models
{
    public class UserStats
    {
        public string Username { get; set; }
        public int Elo { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        public int MatchesDraw { get; set; }
    }
}
