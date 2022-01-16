using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using MonsterTradingCards.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards
{
    internal class Deck
    {
        public Deck(Models.Deck dbDeck, ICardManager cardManager)
        {
            cards = new List<GameCard>();
            foreach (var cardId in dbDeck.cardIds)
            {
                Models.Card card = cardManager.GetCard(cardId);
                string[] nameSplit = Regex.Split(card.Name, @"(?<!^)(?=[A-Z])");

                if (nameSplit.Length != 2)
                {
                    cards.Add(new GameCard(cardId, card.Damage, new NormalElementType(), convertStringToCardType(nameSplit[0])));
                } 
                else
                {
                    cards.Add(new GameCard(cardId, card.Damage, convertStringToElementType(nameSplit[0]), convertStringToCardType(nameSplit[1])));
                }
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





        private IElementType convertStringToElementType(string elementTypeString)
        {
            if (elementTypeString == "Regular")
            {
                elementTypeString = "Normal";
            }

            var elementType = typeof(IElementType).Assembly.GetTypes().Where(t => t.Name == elementTypeString + "ElementType").FirstOrDefault();
            return (IElementType)Activator.CreateInstance(elementType);
        }
        
        private ICardType convertStringToCardType(string cardTypeString)
        {
            if (cardTypeString == "Spell")
            {
                return new SpellType();
            }

            var elementType = typeof(ICardType).Assembly.GetTypes().Where(t => t.Name == cardTypeString + "MonsterType").FirstOrDefault();
            return (ICardType)Activator.CreateInstance(elementType);
        }

        private List<GameCard> cards { get; set; }
    }
}
