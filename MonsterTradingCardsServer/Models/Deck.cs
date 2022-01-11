using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Models
{
    public class Deck
    {
        public string owner { get; set; }
        public List<string> cardIds { get; set; }
    }
}
