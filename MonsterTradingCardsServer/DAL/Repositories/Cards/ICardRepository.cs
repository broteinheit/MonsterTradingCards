using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Server.Models;

namespace MonsterTradingCards.Server.DAL.Repositories.Cards
{
    public interface ICardRepository
    {
        bool InsertCard(Card card);
        Card GetCardById(string cardId);
        List<Card> GetAllUserCards(string Username);
        bool ChangeCardOwner(Card card);
    }
}
