using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Cards.CardType
{
    public class KrakenMonsterType : ICardType
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
