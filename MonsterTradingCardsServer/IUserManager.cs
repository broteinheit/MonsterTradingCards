using MonsterTradingCardsServer.Models;
using System.Collections.Generic;

namespace MonsterTradingCardsServer
{
    public interface IUserManager
    {
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
    }
}