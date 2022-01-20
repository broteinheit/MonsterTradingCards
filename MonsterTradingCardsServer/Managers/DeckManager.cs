using MonsterTradingCards.Server.DAL.Repositories.Deck;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
{
    public class DeckManager : IDeckManager
    {
        private readonly IDeckRepository deckRepository;

        public DeckManager(IDeckRepository deckRepository)
        {
            this.deckRepository = deckRepository;
        }

        public Deck GetDeckByUsername(string username)
        {
            Deck deck = deckRepository.GetDeck(username);
            if (deck.owner == null)
            {
                deck.owner = username;
                deck.cardIds = new List<string>();
            }
            return deck;
        }

        public void SetDeckForUser(Deck deck)
        {
            if (!deckRepository.SetDeck(deck))
            {
                throw new Exception($"Could not set deck for user '{deck.owner}'!");
            }
        }
    }
}
