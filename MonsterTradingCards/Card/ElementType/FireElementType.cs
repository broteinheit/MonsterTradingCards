using MonsterTradingCards.Card.CardType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Card.ElementType
{
    internal class FireElementType : IElementType
    {
        public string GetTypeName()
        {
            return "Fire";
        }

        public double Compare(IElementType otherElementType)
        {
            switch (otherElementType)
            {
                case WaterElementType:
                    return 0.5;
                case NormalElementType:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
