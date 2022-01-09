using MonsterTradingCards.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Logger
{
    internal interface IBattleLogger
    {
        void AddRoundLog(string log);
        string GetCompleteLog();
        string GetRoundLog(int round);
    }
}
