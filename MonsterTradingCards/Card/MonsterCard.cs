using MonsterTradingCards.Card.ElementType;
using MonsterTradingCards.Card.CardType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card
{
    class MonsterCard : Card
    {
        public MonsterCard(string cardId, int playerId, double damage, IElementType elementType, ICardType monsterType)
            : base(cardId, playerId, damage, elementType, monsterType) { }
        

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (cardId.Equals(((MonsterCard)obj).cardId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return cardId.GetHashCode();
        }
    }
}
