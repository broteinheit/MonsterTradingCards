using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards
{
    public class Deck
    {
        public Deck(Models.Deck dbDeck, ICardManager cardManager)
        {
            cards = new List<GameCard>();
            foreach (var cardId in dbDeck.cardIds)
            {
                Models.Card card = cardManager.GetCard(cardId);
                (IElementType, ICardType) types = StringToCardType.GetCardAndElementTypeFromString(card.Name);
                cards.Add(new GameCard(cardId, card.Damage, types.Item1, types.Item2));
            }
        }

        public GameCard DrawRandomCard()
        {
            Random r = new Random();
            GameCard c = cards.ElementAt(r.Next(cards.Count));
            cards.Remove(c);
            return c;
        }

        public void AddCard(GameCard c)
        {
            cards.Add(c);
        }

        public int GetCardCount()
        {
            return cards.Count;
        }


        private List<GameCard> cards { get; set; }
    }
}
