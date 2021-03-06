using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;

        public UserManager(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User LoginUser(Credentials credentials)
        {
            var user = userRepository.GetUserByCredentials(credentials.Username, credentials.Password);
            return user ?? throw new UserNotFoundException();
        }

        public void AdjustGoldForUser(User user, int amount)
        {
            if (!userRepository.AdjustUserGold(user.Username, amount)) 
            {
                throw new Exception("Could not reduce gold");
            }
        }

        public void RegisterUser(Credentials credentials)
        {
            var user = new User()
            {
                Username = credentials.Username,
                Password = credentials.Password,
                Elo = 100,
                Gold = 20
            };
            if (userRepository.InsertUser(user) == false)
            {
                throw new DuplicateUserException();
            }
        }

        public UserInfo GetUserInfo(string username)
        {
            return userRepository.GetUserInfo(username);
        }

        public void EditUserInfo(UserInfo userInfo)
        {
            if (!userRepository.UpdateUserInfo(userInfo))
            {
                throw new Exception("Could not update user info");
            }
        }

        public UserStats GetUserStats(string username)
        {
            return userRepository.GetUserStats(username);
        }

        public Scoreboard GetScoreboard()
        {
            return userRepository.GetScoreboard();
        }

        public void AdjustEloForUser(User user, int amount)
        {
            if (!userRepository.AdjustUserElo(user.Username, amount))
            {
                throw new Exception("Could not reduce gold");
            }
        }

        public void AdjustWinLoseForUser(User user, WinLoseDrawColumns result)
        {
            if (!userRepository.AddToUserWinLoseStat(user.Username, result))
            {
                throw new Exception("Could not update WinLoseDraw stats");
            }
        }
    }
}
