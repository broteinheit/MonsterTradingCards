using MonsterTradingCards.Server.BattleLogic.Battle;
using MonsterTradingCards.Server.BattleLogic.Cards;
using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.BattleLogic.Cards.ElementType;
using MonsterTradingCards.Server.BattleLogic.Logger;
using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace MonsterTradingCards.Server.Test
{
    [TestFixture]
    public class TestsBattleHandler
    {
        [Test]
        public void TestConnectPlayer()
        {
            // arrange
            var deckManager = new Mock<IDeckManager>();
            var cardManager = new Mock<ICardManager>();
            var battleHandler = new BattleHandler(deckManager.Object, cardManager.Object);

            var user1 = new User() { Username = "testusr1", Password = "12345678", Elo = 100, Gold = 20 };
            var user2 = new User() { Username = "testusr2", Password = "87654321", Elo = 100, Gold = 20 };

            // act
            battleHandler.ConnectPlayer(user1);
            battleHandler.ConnectPlayer(user2);

            // assert
            Assert.AreEqual(user1, battleHandler.player1);
            Assert.AreEqual(user2, battleHandler.player2);
        }

        [Test]
        public void TestDisconnectPlayer()
        {
            // arrange
            var deckManager = new Mock<IDeckManager>();
            var cardManager = new Mock<ICardManager>();
            var user1 = new User() { Username = "testusr1", Password = "12345678", Elo = 100, Gold = 20 };
            var user2 = new User() { Username = "testusr2", Password = "87654321", Elo = 100, Gold = 20 };
            var battleHandler = new BattleHandler(user1, user2, deckManager.Object, cardManager.Object);

            //check if players are in battle handler
            Assert.AreEqual(user1, battleHandler.player1);
            Assert.AreEqual(user2, battleHandler.player2);

            // act
            battleHandler.DisconnectPlayer(user1);
            battleHandler.DisconnectPlayer(user2);

            // assert
            Assert.AreEqual(null, battleHandler.player1);
            Assert.AreEqual(null, battleHandler.player2);
        }

        [Test]
        public void TestGetResult_BeforeBattle()
        {
            // arrange
            var deckManager = new Mock<IDeckManager>();
            var cardManager = new Mock<ICardManager>();
            var battleHandler = new BattleHandler(deckManager.Object, cardManager.Object);

            // act
            // assert
            Assert.AreEqual(BattleResult.ERROR, battleHandler.GetResult());
        }

        [Test]
        public void TestStartBattle()
        {
            // arrange
            var deckManager = new Mock<IDeckManager>();
            var cardManager = new Mock<ICardManager>();
            var user1 = new User() { Username = "testusr1", Password = "12345678", Elo = 100, Gold = 20 };
            var user2 = new User() { Username = "testusr2", Password = "87654321", Elo = 100, Gold = 20 };
            var battleHandler = new BattleHandler(user1, user2, deckManager.Object, cardManager.Object);

            var deck1 = new Models.Deck() { owner = user1.Username, cardIds = new List<string> { "a", "b", "c", "d" } };
            var deck2 = new Models.Deck() { owner = user2.Username, cardIds = new List<string> { "e", "f", "g", "h" } };

            var card1 = new Card() { Id = "a", Name = "WaterGoblin", Damage = 10, OwnerUsername = user1.Username };
            var card2 = new Card() { Id = "b", Name = "WaterGoblin", Damage = 15, OwnerUsername = user1.Username };
            var card3 = new Card() { Id = "c", Name = "FireGoblin", Damage = 20, OwnerUsername = user1.Username };
            var card4 = new Card() { Id = "d", Name = "FireGoblin", Damage = 25, OwnerUsername = user1.Username };
            var card5 = new Card() { Id = "e", Name = "WaterGoblin", Damage = 1, OwnerUsername = user2.Username };
            var card6 = new Card() { Id = "f", Name = "WaterGoblin", Damage = 5, OwnerUsername = user2.Username };
            var card7 = new Card() { Id = "g", Name = "NormalGoblin", Damage = 10, OwnerUsername = user2.Username };
            var card8 = new Card() { Id = "h", Name = "NormalGoblin", Damage = 15, OwnerUsername = user2.Username };

            deckManager.Setup(d => d.GetDeckByUsername(user1.Username)).Returns(deck1);
            deckManager.Setup(d => d.GetDeckByUsername(user2.Username)).Returns(deck2);

            cardManager.Setup(c => c.GetCard("a")).Returns(card1);
            cardManager.Setup(c => c.GetCard("b")).Returns(card2);
            cardManager.Setup(c => c.GetCard("c")).Returns(card3);
            cardManager.Setup(c => c.GetCard("d")).Returns(card4);
            cardManager.Setup(c => c.GetCard("e")).Returns(card5);
            cardManager.Setup(c => c.GetCard("f")).Returns(card6);
            cardManager.Setup(c => c.GetCard("g")).Returns(card7);
            cardManager.Setup(c => c.GetCard("h")).Returns(card8);

            // act
            // assert
            Assert.AreNotEqual(battleHandler.StartBattle(), BattleResult.ERROR);
        }

        [Test]
        public void TestGetResult_AfterBattle()
        {
            // arrange
            var deckManager = new Mock<IDeckManager>();
            var cardManager = new Mock<ICardManager>();
            var user1 = new User() { Username = "testusr1", Password = "12345678", Elo = 100, Gold = 20 };
            var user2 = new User() { Username = "testusr2", Password = "87654321", Elo = 100, Gold = 20 };
            var battleHandler = new BattleHandler(user1, user2, deckManager.Object, cardManager.Object);

            var deck1 = new Models.Deck() { owner = user1.Username, cardIds = new List<string> { "a", "b", "c", "d" } };
            var deck2 = new Models.Deck() { owner = user2.Username, cardIds = new List<string> { "e", "f", "g", "h" } };

            var card1 = new Card() { Id = "a", Name = "WaterGoblin", Damage = 10, OwnerUsername = user1.Username };
            var card2 = new Card() { Id = "b", Name = "WaterGoblin", Damage = 15, OwnerUsername = user1.Username };
            var card3 = new Card() { Id = "c", Name = "FireGoblin", Damage = 20, OwnerUsername = user1.Username };
            var card4 = new Card() { Id = "d", Name = "FireGoblin", Damage = 25, OwnerUsername = user1.Username };
            var card5 = new Card() { Id = "e", Name = "WaterGoblin", Damage = 1, OwnerUsername = user2.Username };
            var card6 = new Card() { Id = "f", Name = "WaterGoblin", Damage = 5, OwnerUsername = user2.Username };
            var card7 = new Card() { Id = "g", Name = "NormalGoblin", Damage = 10, OwnerUsername = user2.Username };
            var card8 = new Card() { Id = "h", Name = "NormalGoblin", Damage = 15, OwnerUsername = user2.Username };

            deckManager.Setup(d => d.GetDeckByUsername(user1.Username)).Returns(deck1);
            deckManager.Setup(d => d.GetDeckByUsername(user2.Username)).Returns(deck2);

            cardManager.Setup(c => c.GetCard("a")).Returns(card1);
            cardManager.Setup(c => c.GetCard("b")).Returns(card2);
            cardManager.Setup(c => c.GetCard("c")).Returns(card3);
            cardManager.Setup(c => c.GetCard("d")).Returns(card4);
            cardManager.Setup(c => c.GetCard("e")).Returns(card5);
            cardManager.Setup(c => c.GetCard("f")).Returns(card6);
            cardManager.Setup(c => c.GetCard("g")).Returns(card7);
            cardManager.Setup(c => c.GetCard("h")).Returns(card8);

            // act
            var result = battleHandler.StartBattle();
            // assert
            Assert.AreNotEqual(result, BattleResult.ERROR);
            Assert.AreNotEqual(battleHandler.GetResult(), BattleResult.ERROR);
            Assert.AreEqual(result, battleHandler.GetResult());
        }
    }
}