using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Server.Models;

namespace MonsterTradingCards.Server.DAL.Repositories.Tradings
{
    public interface ITradingsRepository
    {
        bool CreateTrade(Trade trade);
        List<Trade> GetAllTrades();
        bool DeleteTrade(string tradeId);
        public Trade GetTrade(string tradeId);
    }
}
