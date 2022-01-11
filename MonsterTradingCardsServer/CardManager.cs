using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server
{
    public class CardManager : ICardManager
    {
        private readonly ICardRepository cardRepository;

        public CardManager(ICardRepository cardRepository)
        {
            this.cardRepository = cardRepository;
        }

        public void AddCard(Card card)
        {
            var tmpCard = new Card()
            {
                Id = card.Id,
                Name = card.Name,
                Damage = card.Damage,
                OwnerUsername = null
            };
            if (!cardRepository.InsertCard(tmpCard))
            {
                throw new Exception("Card could not be added");
            }

        }

        public string GetAllCardsJSON(string username)
        {
            return JsonConvert.SerializeObject(cardRepository.GetAllUserCards(username));
        }

        public Card GetCard(string cardId)
        {
            throw new NotImplementedException();
        }

        public void ChangeOwner(Card card, string newOwner)
        {
            card.OwnerUsername = newOwner;
            if (!cardRepository.ChangeCardOwner(card))
            {
                throw new Exception($"Owner could not be changed (id: {card.Id}, newOwner: {card.OwnerUsername})");
            }
        }
    }
}
