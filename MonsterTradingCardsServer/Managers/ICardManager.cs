using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
{
    public interface ICardManager
    {
        void AddCard(Card card);
        Card GetCard(string cardId);
        string GetAllCardsJSON(string username);
        void ChangeOwner(Card card, string newOwner);
        public Card SacrificeCard(Card sacrifice, Card reciever);
    }
}
