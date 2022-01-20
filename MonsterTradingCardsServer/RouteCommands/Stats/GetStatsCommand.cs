using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Stats
{
    public class GetStatsCommand : ProtectedRouteCommand
    {
        private readonly IUserManager userManager;

        public GetStatsCommand(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            
            try
            {
                var stats = userManager.GetUserStats(User.Username);
                response.Payload = JsonConvert.SerializeObject(stats);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCode.publicServerError;
                response.Payload = ex.Message;
            }

            return response;
        }
    }
}
