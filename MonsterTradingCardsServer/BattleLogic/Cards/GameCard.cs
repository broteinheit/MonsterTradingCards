using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards
{
    class GameCard
    {
        public string GetCardName()
        {
            return elementType.GetTypeName() + cardType.GetTypeName();
        }

        public GameCard(string cardId, double damage, IElementType elementType, ICardType cardType)
        {
            this.cardId = cardId;
            this.damage = damage;
            this.elementType = elementType;
            this.cardType = cardType;
        }

        public GameCard Clone()
        {
            return (GameCard)this.MemberwiseClone();
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
        public IElementType elementType { get; protected set; }
        public ICardType cardType { get; protected set; }
        public double damage { get; protected set; }
    }
}
