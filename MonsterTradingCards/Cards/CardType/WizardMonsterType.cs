using MonsterTradingCards.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Cards.CardType
{
    internal class WizardMonsterType : ICardType
    {
        public int ConsiderSpecialty(ICardType otherType, IElementType elementType)
        {
            return 1;
        }

        public string GetTypeName()
        {
            return "Wizard";
        }
    }
}
