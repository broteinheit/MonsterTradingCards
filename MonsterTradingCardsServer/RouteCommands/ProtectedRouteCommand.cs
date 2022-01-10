using MonsterTradingCardsServer.Core.Authentication;
using MonsterTradingCardsServer.Core.Response;
using MonsterTradingCardsServer.Core.Routing;
using MonsterTradingCardsServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsServer.RouteCommands
{
    abstract class ProtectedRouteCommand : IProtectedRouteCommand
    {
        public IIdentity Identity { get; set; }

        public User User => (User)Identity;

        public abstract Response Execute();
    }
}
