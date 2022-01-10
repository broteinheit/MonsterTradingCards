using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server
{
    public interface ICardManager
    {
        void AddCard(Card card);
        Card GetCard(string cardId);
        List<Card> GetAllCards(string username);
    }
}
