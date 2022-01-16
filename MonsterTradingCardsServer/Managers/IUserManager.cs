using MonsterTradingCards.Server.DAL.Repositories.Users;
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
        UserStats GetUserStats(string username);
        Scoreboard GetScoreboard();
        void AdjustEloForUser(User user, int amount);
        void AdjustWinLoseForUser(User user, WinLoseDrawColumns result);
    }
}