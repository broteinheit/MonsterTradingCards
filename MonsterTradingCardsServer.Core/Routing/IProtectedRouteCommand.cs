using MonsterTradingCards.Server.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Core.Routing
{
    public interface IProtectedRouteCommand : IRouteCommand
    {
        IIdentity Identity { get; set; }
    }
}
