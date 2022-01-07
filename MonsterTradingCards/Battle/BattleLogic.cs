using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Cards;
using MonsterTradingCards.Cards.CardType;

namespace MonsterTradingCards.Battle
{
    internal class BattleLogic
    {
        //TODO: Logger

        public int RunRound(Card cardP1, Card cardP2)
        {
            //Consider Specialties
            cardP1.ConsiderSpecialties(cardP2.cardType, cardP2.elementType);
            cardP2.ConsiderSpecialties(cardP1.cardType, cardP1.elementType);

            //Not Monster Fight -> consider Element (only when both cards have damage values > 0, otherwise winner is already known)
            if ((cardP1.cardType.GetType() == typeof(SpellType) || cardP2.cardType.GetType() == typeof(SpellType))
                && cardP1.damage > 0 && cardP2.damage > 0)
            {
                cardP1.AdjustDamageByElementType(cardP2.elementType);
                cardP2.AdjustDamageByElementType(cardP1.elementType);
            }

            return CalculateWinner(cardP1, cardP2);
        }


        private int CalculateWinner(Card cardP1, Card cardP2)
        {
            if (cardP1.damage > cardP2.damage)
            {
                //Player 1 won the round -> return 0
                return 0;
            }
            else if (cardP2.damage > cardP1.damage)
            {
                //Player 2 won the round -> return 1
                return 1;
            }
            else
            {
                //draw -> return -1
                return -1;
            }
        }
    }
}
