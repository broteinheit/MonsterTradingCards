﻿using MonsterTradingCards.Card.CardType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card.ElementType
{
    internal class WaterElementType : IElementType
    {
        public string GetTypeName()
        {
            return "Water";
        }

        public double Compare(IElementType otherElementType)
        {
            switch (otherElementType)
            {
                case NormalElementType:
                    return 0.5;
                case FireElementType:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
