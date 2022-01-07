using MonsterTradingCards.Card.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card.CardType
{
    internal class KnightMonsterType : ICardType
    {
        public int ConsiderSpecialty(ICardType otherType, IElementType elementType)
        {
            switch (otherType)
            {
                case SpellCard:
                    if (elementType.GetType() == typeof(WaterElementType))
                    {
                        return 0;
                    }
                    return 1;
                default:
                    return 1;
            }
        }

        public string GetTypeName()
        {
            return "Knight";
        }
    }
}
