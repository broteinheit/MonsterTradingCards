using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Tradings
{
    public class TradeSerializedObject
    {
        public string Id { get; set; }
        public string CardToTrade { get; set; }
        public string Type { get; set; }
        public double MinimumDamage { get; set; }
    }
}
