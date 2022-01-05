using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card
{
    abstract class Card
    {
        protected void initCard(string cardId, string cardName, int playerId, double damage, ElementType elementType)
        {
            this.cardId = cardId;
            this.cardName = cardName;
            this.playerId = playerId;
            this.damage = damage;
            this.elementType = elementType;
        }

        public string cardId { get; protected set; }
        public string cardName { get; protected set; }
        public int playerId { get; protected set; }
        public double damage { get; protected set; }
        public ElementType elementType { get; protected set; }
    }
}
