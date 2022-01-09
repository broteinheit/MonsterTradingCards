using MonsterTradingCards.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Cards.CardType
{
    internal interface ICardType : IType
    {
        //returns multiplier for specialty as defined in specifications
        //int because it either loses or nothing changes (0, 1)
        int ConsiderSpecialty(ICardType otherCardType, IElementType elementType);
    }
}
