using MonsterTradingCards.Cards.CardType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Cards.ElementType
{
    internal class NormalElementType : IElementType
    {
        public string GetTypeName()
        {
            return "Normal";
        }

        public double Compare(IElementType otherElementType)
        {
            switch (otherElementType)
            {
                case FireElementType:
                    return 0.5;
                case WaterElementType:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
