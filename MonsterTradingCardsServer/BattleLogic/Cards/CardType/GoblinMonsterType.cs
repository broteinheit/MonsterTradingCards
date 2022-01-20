using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards.CardType
{
    public class GoblinMonsterType : ICardType
    {
        public int ConsiderSpecialty(ICardType otherType, IElementType elementType)
        {
            switch (otherType)
            {
                case DragonMonsterType:
                    return 0;
                default:
                    return 1;
            }
        }

        public string GetTypeName()
        {
            return "Goblin";
        }
    }
}
