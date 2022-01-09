﻿using MonsterTradingCards.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Cards.CardType
{
    internal class OrkMonsterType : ICardType
    {
        public int ConsiderSpecialty(ICardType otherType, IElementType elementType)
        {
            switch (otherType)
            {
                case WizardMonsterType:
                    return 0;
                default:
                    return 1;
            }
        }

        public string GetTypeName()
        {
            return "Ork";
        }
    }
}
