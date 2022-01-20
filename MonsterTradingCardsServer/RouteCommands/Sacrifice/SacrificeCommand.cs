using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Sacrifice
{
    public class SacrificeCommand : ProtectedRouteCommand
    {
        private readonly ICardManager cardManager;
        private readonly IDeckManager deckManager;
        private readonly Card sacrifice;
        private readonly Card receiver;


        public SacrificeCommand(ICardManager cardManager, IDeckManager deckManager, SacrificeModel sacrificeModel)
        {
            this.cardManager = cardManager;
            this.deckManager = deckManager;
            sacrifice = cardManager.GetCard(sacrificeModel.Sacrifice);
            receiver = cardManager.GetCard(sacrificeModel.Reciever);
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                //check if user owns both
                if (sacrifice.OwnerUsername != User.Username || receiver.OwnerUsername != User.Username)
                {
                    throw new Exception($"User {User.Username} does not own both cards!");
                }

                //check if sacrifice card in deck
                if (deckManager.GetDeckByUsername(User.Username).cardIds.Contains(sacrifice.Id))
                {
                    throw new Exception($"Cannot sacrifice cards that are currently in a deck! (cardId: {sacrifice.Id})");
                }

                response.Payload = JsonConvert.SerializeObject(cardManager.SacrificeCard(sacrifice, receiver));
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
