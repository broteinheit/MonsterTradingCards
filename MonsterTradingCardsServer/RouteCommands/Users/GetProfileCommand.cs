using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Users
{
    internal class GetProfileCommand : ProtectedRouteCommand
    {
        private string test;

        public GetProfileCommand(string test)
        {
            this.test = test;
        }

        public override Response Execute()
        {
            Response response = new Response();
            response.StatusCode = StatusCode.Ok;
            response.Payload = test;
            return response;
        }
    }
}
