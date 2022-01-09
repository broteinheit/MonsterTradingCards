using MonsterTradingCards.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Logger
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

        public string GetRoundLog(int round)
        {
            return roundLogs[round];
        }

        private List<string> roundLogs;
    }
}
