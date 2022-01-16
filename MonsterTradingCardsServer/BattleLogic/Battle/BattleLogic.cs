using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Server.BattleLogic.Cards;
using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Logger;

namespace MonsterTradingCards.Server.BattleLogic.Battle
{
    internal class BattleLogic
    {
        public BattleLogic(ref BattleLogStringBuilder logBuilder)
        {
            this.logBuilder = logBuilder;
        }

        public BattleRoundResult RunRound(GameCard cardP1, GameCard cardP2)
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
            else
            {
                logBuilder.monsterFight = true;
            }

            logBuilder.finalDamageP1 = cardP1.damage;
            logBuilder.finalDamageP2 = cardP2.damage;

            return CalculateWinner(cardP1, cardP2);
        }

        //PlayerA: FireSpell (10 Damage) vs PlayerB: WaterSpell (20 Damage) => 10 VS 20 -> 05 VS 40 => WaterSpell wins

        private BattleRoundResult CalculateWinner(GameCard cardP1, GameCard cardP2)
        {
            if (cardP1.damage > cardP2.damage)
            {
                //Player 1 won the round -> return 0
                return BattleRoundResult.PLAYER1_WIN;
            }
            else if (cardP2.damage > cardP1.damage)
            {
                //Player 2 won the round -> return 1
                return BattleRoundResult.PLAYER2_WIN;
            }
            else
            {
                //draw -> return -1
                return BattleRoundResult.DRAW;
            }
        }

        private BattleLogStringBuilder logBuilder;
    }
}
