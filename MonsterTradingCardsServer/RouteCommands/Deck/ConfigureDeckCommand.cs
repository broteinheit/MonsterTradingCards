using MonsterTradingCards.Server.BattleLogic.Battle;
using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Deck
{
    public class ConfigureDeckCommand : ProtectedRouteCommand
    {
        private readonly IDeckManager deckManager;
        private readonly ICardManager cardManager;
        private List<string> cardIds;

        public ConfigureDeckCommand(IDeckManager deckManager, ICardManager cardManager, List<string> cardIds)
        {
            this.deckManager = deckManager;
            this.cardManager = cardManager;
            this.cardIds = cardIds;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                //check if user is in battle or queue
                if (GameManager.IsPlayerBlocked(User.Username))
                {
                    throw new Exception($"Player {User.Username} is already in a Battle Queue");
                }

                //check if player provided 4 cards
                if (cardIds.Count != 4)
                {
                    throw new Exception("a Deck needs exactly 4 cards!");
                } 

                //check if player owns all provided cards
                foreach (var c in cardIds)
                {
                    if (cardManager.GetCard(c).OwnerUsername != User.Username)
                    {
                        throw new Exception($"Card does not belong to user (user: {User.Username}, cardId: {c})");
                    }
                }

                deckManager.SetDeckForUser(new Models.Deck() { owner = User.Username, cardIds = cardIds});

                response.StatusCode = StatusCode.Created;
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
