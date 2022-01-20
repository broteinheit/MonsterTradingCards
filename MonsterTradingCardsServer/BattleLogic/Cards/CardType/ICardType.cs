using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards.CardType
{
    public interface ICardType : IType
    {
        //returns multiplier for specialty as defined in specifications
        //int because it either loses or nothing changes (0, 1)
        int ConsiderSpecialty(ICardType otherCardType, IElementType elementType);
    }
}
