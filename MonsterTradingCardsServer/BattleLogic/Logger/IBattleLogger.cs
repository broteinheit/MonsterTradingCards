using MonsterTradingCards.Server.BattleLogic.Battle;
using MonsterTradingCards.Server.BattleLogic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Logger
{
    internal interface IBattleLogger
    {
        void AddRoundLog(string log);
        string GetCompleteLog();
        string GetRoundLog(int round);
        public void AddResultLog(string winnerName, string loserName);
        public void AddResultLogDraw();
    }
}
