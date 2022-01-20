using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MonsterTradingCards.Server.Test
{
    [TestFixture]
    public class TestsCardManager
    {
        [Test]
        public void TestAddCard_ValidCard()
        {
            // arrange
            var cardRepo = new Mock<ICardRepository>();
            var cardManager = new CardManager(cardRepo.Object);
            var card = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = null };

            cardRepo.Setup(u => u.InsertCard(It.Is<Card>(c => c.Id == card.Id && c.Name == card.Name
                && c.Damage == card.Damage && c.OwnerUsername == null))).Returns(true);

            // act
            // assert
            Assert.DoesNotThrow(() => cardManager.AddCard(card), "AddCard did not work!");
        }

        [Test]
        public void TestAddCard_InvalidCard()
        {
            // arrange
            var cardRepo = new Mock<ICardRepository>();
            var cardManager = new CardManager(cardRepo.Object);
            var card = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = null };

            //adding card did not work -> InsertCard returns false
            cardRepo.Setup(u => u.InsertCard(It.Is<Card>(c => c.Id == card.Id && c.Name == card.Name
                && c.Damage == card.Damage && c.OwnerUsername == null))).Returns(false);

            // act
            // assert
            Assert.Throws<Exception>(() => cardManager.AddCard(card), "AddCard did work (but is not supposed to)!");
        }

        [Test]
        public void TestGetAllCardsJSON()
        {
            //arrange
            var cardRepo = new Mock<ICardRepository>();
            var cardManager = new CardManager(cardRepo.Object);
            var card = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = null };

            cardRepo.Setup(u => u.GetAllUserCards("testusr")).Returns(new List<Card>() { card });

            //act
            //assert
            Assert.AreEqual(JsonConvert.SerializeObject(new List<Card>() { card }), cardManager.GetAllCardsJSON("testusr"), 
                "JSON strings are not equal!");
        }

        [Test]
        public void TestUpdateCard_ValidCard()
        {
            // arrange
            var cardRepo = new Mock<ICardRepository>();
            var cardManager = new CardManager(cardRepo.Object);
            var card = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = null };

            cardRepo.Setup(u => u.UpdateCard(It.Is<Card>(c => c.Id == card.Id && c.Name == card.Name
                && c.Damage == card.Damage && c.OwnerUsername == "testusr"))).Returns(true);

            // act
            // assert
            Assert.DoesNotThrow(() => cardManager.ChangeOwner(card, "testusr"), "UpdateCard did not work!");
        }

        [Test]
        public void TestUpdateCard_InvalidCard()
        {
            // arrange
            var cardRepo = new Mock<ICardRepository>();
            var cardManager = new CardManager(cardRepo.Object);
            var card = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = null };

            //updating card did not work -> UpdateCard returns false
            cardRepo.Setup(u => u.UpdateCard(It.Is<Card>(c => c.Id == card.Id && c.Name == card.Name
                && c.Damage == card.Damage && c.OwnerUsername == "testusr"))).Returns(false);

            // act
            // assert
            Assert.Throws<Exception>(() => cardManager.ChangeOwner(card, "testusr"), "UpdateCard did work (but is not supposed to)!");
        }

        [Test]
        public void TestSacrificeCard_ValidCards()
        {
            // arrange
            var cardRepo = new Mock<ICardRepository>();
            var cardManager = new CardManager(cardRepo.Object);
            var cardReceiver = new Card() { Id = "abcd", Name = "WaterGoblin", Damage = 10, OwnerUsername = "testusr" };
            var cardSacrifice = new Card() { Id = "efgh", Name = "WaterGoblin", Damage = 15, OwnerUsername = "testusr" };

            cardRepo.Setup(u => u.UpdateCard(It.Is<Card>(c => c.Id == cardReceiver.Id))).Returns(true);
            cardRepo.Setup(u => u.DeleteCard(It.Is<Card>(c => c.Id == cardSacrifice.Id))).Returns(true);

            // act
            Card buffedCard = cardManager.SacrificeCard(cardSacrifice, cardReceiver);

            // assert
            Assert.AreEqual(buffedCard.Id, cardReceiver.Id);
            Assert.AreEqual(buffedCard.Name, cardReceiver.Name);
            Assert.AreEqual(buffedCard.Damage, 10 + Math.Floor(0.25 * 15));
            Assert.AreEqual(buffedCard.OwnerUsername, cardReceiver.OwnerUsername);
        }
    }
}