using MonsterTradingCards.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Cards.CardType
{
    internal class KrakenMonsterType : ICardType
    {
        public int ConsiderSpecialty(ICardType otherCard, IElementType elementType)
        {
            return 1;
        }

        public string GetTypeName()
        {
            return "Kraken";
        }
    }
}
