using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Models
{
    public class ScoreboardEntry
    {
        public string Username { get; set; }
        public int Elo { get; set; }
    }
}
