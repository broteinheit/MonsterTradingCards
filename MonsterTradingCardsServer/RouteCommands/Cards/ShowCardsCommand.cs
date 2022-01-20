using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Cards
{
    public class ShowCardsCommand : ProtectedRouteCommand
    {
        private readonly ICardManager cardManager;

        public ShowCardsCommand(ICardManager cardManager)
        {
            this.cardManager = cardManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                response.Payload = cardManager.GetAllCardsJSON(User.Username);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception e)
            {
                response.StatusCode = StatusCode.Conflict;
                response.Payload = e.Message;
            }

            return response;
        }
    }
}
