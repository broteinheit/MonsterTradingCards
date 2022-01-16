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
    internal class TradeCommand : ProtectedRouteCommand
    {
        private readonly ITradingsManager tradingsManager;
        private string tradeId;
        private string cardId;

        public TradeCommand(ITradingsManager tradingsManager, string tradeId, string cardId)
        {
            this.tradingsManager = tradingsManager;
            this.tradeId = tradeId;
            this.cardId = cardId;
        }

        public override Response Execute()
        {
            var response = new Response();

            try
            {
                tradingsManager.AcceptTradeOffer(tradeId, User.Username, cardId);
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
