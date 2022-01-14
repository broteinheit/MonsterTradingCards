using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Users
{
    internal class GetProfileCommand : ProtectedRouteCommand
    {
        private string username;
        private readonly IUserManager userManager;

        public GetProfileCommand(IUserManager userManager, string username)
        {
            this.username = username;
            this.userManager = userManager;
        }

        public override Response Execute()
        {
            Response response = new Response();
            try
            {
                if (User.Username != username)
                {
                    throw new Exception("Not Authorized!");
                }

                var info = userManager.GetUserInfo(username);
                response.Payload = JsonConvert.SerializeObject(info);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception e)
            {
                response.StatusCode = StatusCode.BadRequest;
                response.Payload = e.Message;
            }
            return response;
        }
    }
}
