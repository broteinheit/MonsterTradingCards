using MonsterTradingCardsServer.Core.Response;
using MonsterTradingCardsServer.Core.Routing;
using MonsterTradingCardsServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsServer.RouteCommands.Users
{
    class LoginCommand : IRouteCommand
    {
        private readonly IUserManager messageManager;

        public Credentials Credentials { get; private set; }

        public LoginCommand(IUserManager messageManager, Credentials credentials)
        {
            Credentials = credentials;
            this.messageManager = messageManager;
        }

        public Response Execute()
        {
            User user;
            try
            {
                user = messageManager.LoginUser(Credentials);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            var response = new Response();
            if (user == null)
            {
                response.StatusCode = StatusCode.Unauthorized;
            }
            else
            {
                response.StatusCode = StatusCode.Ok;
                response.Payload = user.Token;
            }

            return response;
        }
    }
}
