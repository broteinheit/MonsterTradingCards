using MonsterTradingCards.Server.Models;
using System.Collections.Generic;

namespace MonsterTradingCards.Server.Managers
{
    public interface IUserManager
    {
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
        void AdjustGoldForUser(User user, int amount);
        UserInfo GetUserInfo(string username);
        void EditUserInfo(UserInfo userInfo);
    }
}