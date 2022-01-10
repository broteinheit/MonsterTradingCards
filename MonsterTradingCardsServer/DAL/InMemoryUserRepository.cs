using SWE1HttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> users = new();

        public User GetUserByAuthToken(string authToken)
        {
            return users.SingleOrDefault(u => u.Token == authToken);
        }

        public User GetUserByCredentials(string username, string password)
        {
            return users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool InsertUser(User user)
        {
            var inserted = false;

            if (GetUserByName(user.Username) == null)
            {
                users.Add(user);
                inserted = true;
            }

            return inserted;
        }

        private User GetUserByName(string username)
        {
            return users.SingleOrDefault(u => u.Username == username);
        }
    }
}
