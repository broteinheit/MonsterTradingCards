using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Tradings
{
    internal class GetTradingDealsCommand : ProtectedRouteCommand
    {
        private readonly ITradingsManager tradingsManager;
        private string id;

        public GetTradingDealsCommand(ITradingsManager tradingsManager)
        {
            this.tradingsManager = tradingsManager;
        }

        public override Response Execute()
        {
            var response = new Response();

            try
            {
                response.Payload = JsonConvert.SerializeObject(tradingsManager.GetAllTradeOffers());
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Payload = ex.Message;
                response.StatusCode = StatusCode.BadRequest;
            }

            return response;
        }
    }
}
