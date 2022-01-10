﻿using MonsterTradingCardsServer.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsServer.Core.Authentication
{
    public interface IIdentityProvider
    {
        IIdentity GetIdentyForRequest(RequestContext request);
    }
}
