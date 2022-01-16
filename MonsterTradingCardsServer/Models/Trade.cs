using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Models
{
    public class Trade
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string CardToTradeId { get; set; }
        public TradeCardType TypeToTrade { get; set; }
        public double MinDamage { get; set; }
    }
}
