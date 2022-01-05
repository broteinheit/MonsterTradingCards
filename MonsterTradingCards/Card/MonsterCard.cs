using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card
{
    class MonsterCard : Card
    {
        public MonsterCard(string cardId, string cardName, int playerId, double damage, ElementType elementType, MonsterType monsterType)
        {
            initCard(cardId, cardName, playerId, damage, elementType);
            this.monsterType = monsterType;
        }

        public MonsterType monsterType { get; private set; }

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
