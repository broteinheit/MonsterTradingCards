using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Scoreboard
{
    internal class ScoreboardCommand : ProtectedRouteCommand
    {
        private readonly IUserManager userManager;

        public ScoreboardCommand(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public override Response Execute()
        {
            var response = new Response();

            try
            {
                var scoreboard = userManager.GetScoreboard();
                response.Payload = JsonConvert.SerializeObject(scoreboard.entries);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCode.InternalServerError;
                response.Payload = ex.Message;
            }


            return response;
        }
    }
}
