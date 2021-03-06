using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MonsterTradingCards.Server.DAL.Repositories.Users
{
    public interface IUserRepository
    {
        User GetUserByCredentials(string username, string password);

        User GetUserByAuthToken(string authToken);

        bool InsertUser(User user);

        bool AdjustUserGold(string username, int gold);

        UserInfo GetUserInfo(string username);

        bool UpdateUserInfo(UserInfo user);

        UserStats GetUserStats(string username);

        Scoreboard GetScoreboard();
        
        bool AdjustUserElo(string username, int elo);
        bool AddToUserWinLoseStat(string username, WinLoseDrawColumns result);
    }
}
