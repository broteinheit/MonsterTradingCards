using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Logger
{
    internal class BattleLogStringBuilder
    {
        public string GetLogString()
        {
            //if final damage differs from inital damage
            if (finalDamageP1.HasValue && finalDamageP2.HasValue 
                && (finalDamageP1.Value != initialDamageP1 || finalDamageP2 != initialDamageP2))
            {
                return $"{player1Name}: {cardP1Name} ({initialDamageP1} Damage) vs " +
                    $"{player2Name}: {cardP2Name} ({initialDamageP2} Damage) => " +
                    $"{initialDamageP1} VS {initialDamageP2} -> {finalDamageP1} VS {finalDamageP2} " +
                    $"=> {(draw ? "Draw (no action)" : (monsterFight ? $"{winnerCardName} defeats {loserCardName}" : $"{winnerCardName} wins" ))}";
            } 
            else
            {
                return $"{player1Name}: {cardP1Name} ({initialDamageP1} Damage) vs " +
                    $"{player2Name}: {cardP2Name} ({initialDamageP2} Damage) " +
                    $"=> {(draw ? "Draw (no action)" : (monsterFight ? $"{winnerCardName} defeats {loserCardName}" : $"{winnerCardName} wins"))}";
            }
        }

        public void reset()
        {
            cardP1Name = null;
            cardP2Name = null;
            initialDamageP1 = -1;
            initialDamageP2 = -1;
            finalDamageP1 = null;
            finalDamageP2 = null;
            monsterFight = false;
            winnerCardName = null;
            loserCardName = null;
            draw = false;
        }

        public string player1Name { get; set; }
        public string player2Name { get; set; }
        public string cardP1Name { get; set; }
        public string cardP2Name { get; set; }
        public double initialDamageP1 { get; set; }
        public double initialDamageP2 { get; set; }
        public double? finalDamageP1 { get; set; }
        public double? finalDamageP2 { get; set; }
        public bool monsterFight { get; set; } = false;
        public bool draw { get; set; } = false;
        public string winnerCardName { get; set; }
        public string loserCardName { get; set; }
    }
}
