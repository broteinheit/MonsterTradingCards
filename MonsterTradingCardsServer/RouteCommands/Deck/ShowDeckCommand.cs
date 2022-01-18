using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.RouteCommands;
using MonsterTradingCards.Server.Utility;
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
        private readonly ICardManager cardManager;
        private readonly string format;

        public ShowDeckCommand(IDeckManager deckManager, ICardManager cardManager, string format="json")
        {
            this.deckManager = deckManager;
            this.cardManager = cardManager;
            this.format = format;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                List<Models.Card> cards = new List<Models.Card>();

                foreach (var cardId in deckManager.GetDeckByUsername(User.Username).cardIds)
                {
                    cards.Add(cardManager.GetCard(cardId));
                }

                response.StatusCode = StatusCode.Ok;
                switch (format)
                {
                    case "json":
                        response.Payload = JsonConvert.SerializeObject(cards);
                        break;
                    case "plain":
                        foreach (var card in cards)
                        {
                            (IElementType, ICardType) types = StringToCardType.GetCardAndElementTypeFromString(card.Name);
                            response.Payload += $"Id: {card.Id}\n-> CardName: {card.Name}, Damage: {card.Damage}\n" +
                                $"-> Element: {types.Item1.GetTypeName()}, CardType: {types.Item2.GetTypeName()}\n\n";
                        }
                        break;
                    default:
                        throw new Exception($"format {format} is not supported");
                }
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
