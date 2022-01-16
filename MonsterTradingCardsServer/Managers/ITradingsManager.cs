using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
{
    public interface ITradingsManager
    {
        public void CreateTradeOffer(string id, string username, string cardId, string wantedType, double minDamage);
        public void DeleteTradeOffer(string id, string username);
        public List<Trade> GetAllTradeOffers();
        public void AcceptTradeOffer(string tradeId, string username, string cardId);
    }
}
