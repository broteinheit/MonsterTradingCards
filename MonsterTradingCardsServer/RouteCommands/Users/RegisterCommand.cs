using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Core.Routing;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Users
{
    class RegisterCommand : IRouteCommand
    {
        private readonly IUserManager userManager;
        public Credentials Credentials { get; private set; }

        public RegisterCommand(IUserManager messageManager, Credentials credentials)
        {
            Credentials = credentials;
            this.userManager = messageManager;
        }

        public Response Execute()
        {
            var response = new Response();
            try
            {
                userManager.RegisterUser(Credentials);
                response.StatusCode = StatusCode.Created;
            }
            catch (DuplicateUserException)
            {
                response.StatusCode = StatusCode.Conflict;
            }

            return response;
        }
    }
}
