using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.DAL.Repositories.Tradings;
using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MonsterTradingCards.Server.Test
{
    [TestFixture]
    public class TestsTradingsManager
    {
        [Test]
        public void TestCreateTradeOffer_ValidParams()
        {
            // arrange
            var tradingsRepo = new Mock<ITradingsRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var tradingsManager = new TradingsManager(tradingsRepo.Object, cardRepo.Object);

            var card = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = "testusr" };
            var trading = new Trade() { CardToTradeId = card.Id, Id = "trade-a", MinDamage = 15, 
                TypeToTrade = TradeCardType.SPELL, Username="testusr" };

            cardRepo.Setup(c => c.GetCardById(card.Id)).Returns(card);
            tradingsRepo.Setup(t => t.CreateTrade(It.Is<Trade>(tr => tr.Id == trading.Id && tr.CardToTradeId == card.Id 
                && tr.MinDamage == trading.MinDamage && tr.TypeToTrade == trading.TypeToTrade && tr.Username == trading.Username))).Returns(true);

            // act
            // assert
            Assert.DoesNotThrow(() => tradingsManager.CreateTradeOffer("trade-a", "testusr", card.Id, "spell", 15), "CreateTradeOffer did not work!");
        }

        [Test]
        public void TestAcceptTrade()
        {
            // arrange
            var tradingsRepo = new Mock<ITradingsRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var tradingsManager = new TradingsManager(tradingsRepo.Object, cardRepo.Object);

            var card = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = "testusr" };
            var trading = new Trade()
            {
                CardToTradeId = card.Id,
                Id = "trade-a",
                MinDamage = 15,
                TypeToTrade = TradeCardType.SPELL,
                Username = "testusr"
            };
            var tradeinCard = new Card() { Id = "efgh", Name = "WaterSpell", Damage = 15, OwnerUsername = "testusr2" };

            cardRepo.Setup(c => c.GetCardById(tradeinCard.Id)).Returns(tradeinCard);
            cardRepo.Setup(c => c.GetCardById(card.Id)).Returns(card);
            tradingsRepo.Setup(t => t.GetTrade(trading.Id)).Returns(trading);

            // act
            // assert
            Assert.Throws<Exception>(() => tradingsManager.AcceptTradeOffer("trade-a", "testusr", tradeinCard.Id), "AcceptTradeOffer not work (but should not)!");
            Assert.Throws<Exception>(() => tradingsManager.AcceptTradeOffer("trade-a", "testusr", card.Id), "AcceptTradeOffer not work (but should not)!");
            Assert.DoesNotThrow(() => tradingsManager.AcceptTradeOffer("trade-a", "testusr2", tradeinCard.Id), "AcceptTradeOffer did not work!");
        }
    }
}