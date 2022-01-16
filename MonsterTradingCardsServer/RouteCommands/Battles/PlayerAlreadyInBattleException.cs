using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Battles
{
    public class PlayerAlreadInBattleException : Exception
    {
        public PlayerAlreadInBattleException()
        {
        }

        public PlayerAlreadInBattleException(string message) : base(message)
        {
        }

        public PlayerAlreadInBattleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
