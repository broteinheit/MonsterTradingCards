using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card
{
    class SpellCard : Card
    {
        public SpellCard(string cardId, string cardName, int playerId, double damage, ElementType elementType)
        {
            initCard(cardId, cardName, playerId, damage, elementType);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (cardId.Equals(((SpellCard)obj).cardId))
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
