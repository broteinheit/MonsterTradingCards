using MonsterTradingCards.Server.Core.Authentication;
using MonsterTradingCards.Server.Core.Request;
using MonsterTradingCards.Server.DAL;
using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server
{
    class UserIdentityProvider : IIdentityProvider
    {
        private readonly IUserRepository userRepository;

        public UserIdentityProvider(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public IIdentity GetIdentyForRequest(RequestContext request)
        {
            User currentUser = null;

            if (request.Header.TryGetValue("Authorization", out string authToken))
            {
                const string prefix = "Basic ";
                if (authToken.StartsWith(prefix))
                {
                    currentUser = userRepository.GetUserByAuthToken(authToken.Substring(prefix.Length));
                }
            }

            return currentUser;
        }
    }
}
