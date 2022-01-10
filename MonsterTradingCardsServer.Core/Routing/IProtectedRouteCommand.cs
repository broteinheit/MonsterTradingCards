﻿using MonsterTradingCardsServer.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsServer.Core.Routing
{
    public interface IProtectedRouteCommand : IRouteCommand
    {
        IIdentity Identity { get; set; }
    }
}
