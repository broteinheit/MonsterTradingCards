using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
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
            return cardRepository.GetCardById(cardId);
        }

        public void ChangeOwner(Card card, string newOwner)
        {
            card.OwnerUsername = newOwner;
            if (!cardRepository.UpdateCard(card))
            {
                throw new Exception($"Owner could not be changed (id: {card.Id}, newOwner: {card.OwnerUsername})");
            }
        }

        public Card SacrificeCard(Card sacrifice, Card reciever)
        {
            if (sacrifice.Damage < 1.5 * reciever.Damage)
            {
                throw new Exception($"Sacrifice-Card must have at least 50% more damage than Receiver-Card! " +
                    $"(Sacrifice: {sacrifice.Damage}, Receiver: {reciever.Damage})");
            }

            //add 1/4th of sacrifice card's damage to receiver;
            reciever.Damage += Math.Floor(sacrifice.Damage * 0.25);

            //update reciever
            if (!cardRepository.UpdateCard(reciever))
            {
                throw new Exception("Receiver-Card could not be updated!");
            }

            //delete sacrifice
            if (!cardRepository.DeleteCard(sacrifice))
            {
                throw new Exception("Could not delete Sacrifice-Card!");
            }

            return reciever;
        }
    }
}
