using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.DAL.Repositories.Tradings;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
{
    public class TradingsManager : ITradingsManager
    {
        private readonly ITradingsRepository tradingsRepository;
        private readonly ICardRepository cardRepository;

        public TradingsManager(ITradingsRepository tradingsRepository, ICardRepository cardRepository)
        {
            this.tradingsRepository = tradingsRepository;
            this.cardRepository = cardRepository;
        }

        public void AcceptTradeOffer(string tradeId, string username, string cardId)
        {
            Card tradeInCard = cardRepository.GetCardById(cardId);
            //check if user owns card
            if (username != tradeInCard.OwnerUsername)
            {
                throw new Exception($"User {username} does not own card {cardId}");
            }

            var trade = tradingsRepository.GetTrade(tradeId);

            //check if user is different from trade provider
            if (trade.Username == tradeInCard.OwnerUsername)
            {
                throw new Exception("You cannot trade with yourself!");
            }

            //check conditions
            if (tradeInCard.Damage < trade.MinDamage)
            {
                throw new Exception($"Minimum Damage is not reached! (required: {trade.MinDamage}, provided: {tradeInCard.Damage})");
            }

            string[] cardNameSplit = Regex.Split(tradeInCard.Name, @"(?<!^)(?=[A-Z])");
            Type cardType = StringToCardType.convertStringToCardType(cardNameSplit.Length == 2 ? cardNameSplit[1] : cardNameSplit[0]).GetType();
            if (trade.TypeToTrade == TradeCardType.MONSTER && (!cardType.GetInterfaces().Contains(typeof(ICardType)) || cardType == typeof(SpellType)))
            {
                throw new Exception("Provided Card is not a monster card as required by the trade!");
            }
            else if (trade.TypeToTrade == TradeCardType.SPELL && cardType != typeof(SpellType))
            {
                throw new Exception("Provided Card is not a spell card as required by the trade!");
            }

            // conditions are met -> make trade
            //delete trade
            tradingsRepository.DeleteTrade(tradeId);

            //change owners
            Card providerCard = cardRepository.GetCardById(trade.CardToTradeId);
            string providerUsername = providerCard.OwnerUsername;
            string tradeinUsername = tradeInCard.OwnerUsername;

            providerCard.OwnerUsername = tradeinUsername;
            tradeInCard.OwnerUsername = providerUsername;
            //change owner trade provider card
            cardRepository.UpdateCard(providerCard);
            //change owner trade-in card
            cardRepository.UpdateCard(tradeInCard);
        }

        public void CreateTradeOffer(string id, string username, string cardId, string wantedType, double minDamage)
        {
            //check if user owns card
            if (username != cardRepository.GetCardById(cardId).OwnerUsername)
            {
                throw new Exception($"User {username} does not own card {cardId}");
            }

            //convert string to TradeCardType
            TradeCardType typeToTrade;
            switch (wantedType)
            {
                case "monster":
                    typeToTrade = TradeCardType.MONSTER;
                    break;
                case "spell":
                    typeToTrade = TradeCardType.SPELL;
                    break;
                case "none":
                    typeToTrade = TradeCardType.NONE;
                    break;
                default:
                    typeToTrade = TradeCardType.ERROR;
                    break;
            }

            var trade = new Trade()
            {
                Id = id,
                Username = username,
                CardToTradeId = cardId,
                TypeToTrade = typeToTrade,
                MinDamage = minDamage
            };

            if (!tradingsRepository.CreateTrade(trade))
            {
                throw new Exception("Could not create trade!");
            }
        }

        public void DeleteTradeOffer(string id, string username)
        {
            if (tradingsRepository.GetTrade(id).Username != username)
            {
                throw new Exception("User does not own trade!");
            }

            if (!tradingsRepository.DeleteTrade(id))
            {
                throw new Exception("Could not delete trade!");
            }
        }

        public List<Trade> GetAllTradeOffers()
        {
            return tradingsRepository.GetAllTrades();
        }
    }
}
