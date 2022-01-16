using MonsterTradingCards.Server.Core.Authentication;
using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Core.Routing;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands
{
    abstract class ProtectedRouteCommand : IProtectedRouteCommand
    {
        public IIdentity Identity { get; set; }

        public User User => (User)Identity;

        public abstract Response Execute();
    }
}
