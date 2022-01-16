using MonsterTradingCards.Server.BattleLogic.Battle;
using MonsterTradingCards.Server.BattleLogic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Logger
{
    internal class InMemoryLogger : IBattleLogger
    {
        public InMemoryLogger()
        {
            roundLogs = new List<string>();
        }

        public void AddRoundLog(string log)
        {
            roundLogs.Add(log);
        }

        public string GetCompleteLog()
        {
            string complete = "";
            foreach (string message in roundLogs)
            {
                complete += $"{message}\n";
            }
            return complete;
        }

        public void AddResultLog(string winnerName, string loserName)
        {
            roundLogs.Add($"\n####### BATTLE FINISHED #######\nWinner: {winnerName} | +3 ELO\nLoser:  {loserName} | -5 ELO");
        }

        public string GetRoundLog(int round)
        {
            return roundLogs[round];
        }

        public void AddResultLogDraw()
        {
            roundLogs.Add($"\n####### BATTLE FINISHED #######\nResult: Draw\nNo ELO changes");
        }

        private List<string> roundLogs;
    }
}
