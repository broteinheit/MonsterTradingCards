using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards.CardType
{
    public class DragonMonsterType : ICardType
    {
        public int ConsiderSpecialty(ICardType otherType, IElementType elementType)
        {
            switch (otherType)
            {
                case ElveMonsterType:
                    if (elementType.GetType() == typeof(FireElementType))
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
            return "Dragon";
        }
    }
}
