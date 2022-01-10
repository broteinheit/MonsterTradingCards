using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server
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
    }
}
