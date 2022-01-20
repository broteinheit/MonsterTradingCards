using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Tradings
{
    public class CreateTradingDealCommand : ProtectedRouteCommand
    {
        private readonly ITradingsManager tradingsManager;
        private TradeSerializedObject trade;

        public CreateTradingDealCommand(ITradingsManager tradingsManager, TradeSerializedObject trade)
        {
            this.tradingsManager = tradingsManager;
            this.trade = trade;
        }

        public override Response Execute()
        {
            var response = new Response();

            try
            {
                tradingsManager.CreateTradeOffer(trade.Id, User.Username, trade.CardToTrade, trade.Type, trade.MinimumDamage);
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
