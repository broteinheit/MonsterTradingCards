using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Tradings
{
    public class DeleteTradingDealCommand : ProtectedRouteCommand
    {
        private readonly ITradingsManager tradingsManager;
        private string id;

        public DeleteTradingDealCommand(ITradingsManager tradingsManager, string id)
        {
            this.tradingsManager = tradingsManager;
            this.id = id;
        }

        public override Response Execute()
        {
            var response = new Response();

            try
            {
                tradingsManager.DeleteTradeOffer(id, User.Username);
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
