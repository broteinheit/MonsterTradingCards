using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Card;
using MonsterTradingCards.Card.CardType;

namespace MonsterTradingCards.Battle
{
    internal class BattleLogic
    {
        //TODO: Logger

        public int runRound(Card.Card cardP1, Card.Card cardP2)
        {
            
            
            //Not Monster Fight -> consider Element
            if (cardP1.cardType.GetType() == typeof(SpellType) || cardP2.cardType.GetType() == typeof(SpellType))
            {
                adjustForElement(ref cardP1, ref cardP2);
            }


            return 0;
        }

        private int calculateWinner(Card.Card cardP1, Card.Card cardP2)
        {

            return 0;
        }

        void adjustForElement(ref Card.Card cardP1, ref Card.Card cardP2)
        {
            cardP1.adjustDamageByElementType(cardP2.elementType);
            cardP2.adjustDamageByElementType(cardP1.elementType);
        }
    }
}
