using MonsterTradingCards.Server.Models;
using System.Collections.Generic;

namespace MonsterTradingCards.Server
{
    public interface IUserManager
    {
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
        void AdjustGoldForUser(User user, int amount);
    }
}