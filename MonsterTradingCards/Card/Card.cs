using MonsterTradingCards.Card.CardType;
using MonsterTradingCards.Card.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card
{
    class Card
    {
        public string GetCardName()
        {
            return elementType.GetTypeName() + cardType.GetTypeName();
        }

        public Card(string cardId, int playerId, double damage, IElementType elementType, ICardType cardType)
        {
            this.cardId = cardId;
            this.playerId = playerId;
            this.damage = damage;
            this.elementType = elementType;
            this.cardType = cardType;
        }

        public void AdjustDamageByElementType(IElementType otherCardElementType)
        {
            damage *= elementType.Compare(otherCardElementType);
        }

        public void ConsiderSpecialties(ICardType otherCardType, IElementType otherElementType)
        {
            damage *= cardType.ConsiderSpecialty(otherCardType, otherElementType);
        }

        public string cardId { get; protected set; }
        public int playerId { get; protected set; }
        public IElementType elementType { get; protected set; }
        public ICardType cardType { get; protected set; }
        public double damage { get; protected set; }
    }
}
