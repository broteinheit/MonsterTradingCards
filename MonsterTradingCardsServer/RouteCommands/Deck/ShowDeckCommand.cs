using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Deck
{
    internal class ShowDeckCommand : ProtectedRouteCommand
    {
        private readonly IDeckManager deckManager;

        public ShowDeckCommand(IDeckManager deckManager)
        {
            this.deckManager = deckManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                Models.Deck deck = deckManager.GetDeckByUsername(User.Username);
                response.StatusCode = StatusCode.Ok;
                response.Payload = JsonConvert.SerializeObject(deck.cardIds);
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
