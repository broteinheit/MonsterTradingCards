using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards.CardType
{
    internal class KnightMonsterType : ICardType
    {
        public int ConsiderSpecialty(ICardType otherType, IElementType elementType)
        {
            switch (otherType)
            {
                case SpellType:
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
