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
    class RegisterCommand : IRouteCommand
    {
        private readonly IUserManager messageManager;
        public Credentials Credentials { get; private set; }

        public RegisterCommand(IUserManager messageManager, Credentials credentials)
        {
            Credentials = credentials;
            this.messageManager = messageManager;
        }

        public Response Execute()
        {
            var response = new Response();
            try
            {
                messageManager.RegisterUser(Credentials);
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
