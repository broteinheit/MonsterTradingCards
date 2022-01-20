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
    public class TestsGameCard
    {
        [Test]
        public void TestGetCardName()
        {
            // arrange
            var card = new GameCard("asdf", 10, new WaterElementType(), new GoblinMonsterType());

            // act
            // assert
            Assert.AreEqual(card.GetCardName(), "WaterGoblin");
        }

        [Test]
        public void TestClone()
        {
            // arrange
            var card = new GameCard("asdf", 10, new WaterElementType(), new GoblinMonsterType());

            // act
            var clone = card.Clone();

            // assert
            Assert.AreEqual(card.cardId, clone.cardId);
            Assert.AreEqual(card.cardType, clone.cardType);
            Assert.AreEqual(card.elementType, clone.elementType);
            Assert.AreEqual(card.damage, clone.damage);
            Assert.AreNotEqual(card, clone);
        }

        [Test]
        public void TestAdjustDamageByElementType_Water()
        {
            // arrange
            var card = new GameCard("asdf", 10, new WaterElementType(), new GoblinMonsterType());
            var cardCompareWater = card.Clone();
            var cardCompareFire = card.Clone();
            var cardCompareNormal = card.Clone();

            // act
            cardCompareWater.AdjustDamageByElementType(new WaterElementType());
            cardCompareFire.AdjustDamageByElementType(new FireElementType());
            cardCompareNormal.AdjustDamageByElementType(new NormalElementType());

            // assert
            Assert.AreEqual(card.damage, cardCompareWater.damage);
            Assert.AreEqual(card.damage * 2, cardCompareFire.damage);
            Assert.AreEqual(card.damage / 2, cardCompareNormal.damage);
        }

        [Test]
        public void TestAdjustDamageByElementType_Fire()
        {
            // arrange
            var card = new GameCard("asdf", 10, new FireElementType(), new GoblinMonsterType());
            var cardCompareWater = card.Clone();
            var cardCompareFire = card.Clone();
            var cardCompareNormal = card.Clone();

            // act
            cardCompareWater.AdjustDamageByElementType(new WaterElementType());
            cardCompareFire.AdjustDamageByElementType(new FireElementType());
            cardCompareNormal.AdjustDamageByElementType(new NormalElementType());

            // assert
            Assert.AreEqual(card.damage / 2, cardCompareWater.damage);
            Assert.AreEqual(card.damage, cardCompareFire.damage);
            Assert.AreEqual(card.damage * 2, cardCompareNormal.damage);
        }

        [Test]
        public void TestAdjustDamageByElementType_Normal()
        {
            // arrange
            var card = new GameCard("asdf", 10, new NormalElementType(), new GoblinMonsterType());
            var cardCompareWater = card.Clone();
            var cardCompareFire = card.Clone();
            var cardCompareNormal = card.Clone();

            // act
            cardCompareWater.AdjustDamageByElementType(new WaterElementType());
            cardCompareFire.AdjustDamageByElementType(new FireElementType());
            cardCompareNormal.AdjustDamageByElementType(new NormalElementType());

            // assert
            Assert.AreEqual(card.damage * 2, cardCompareWater.damage);
            Assert.AreEqual(card.damage / 2, cardCompareFire.damage);
            Assert.AreEqual(card.damage, cardCompareNormal.damage);
        }

        [Test]
        public void TestConsiderSpecialty_Knight()
        {
            // arrange
            var card = new GameCard("asdf", 10, new WaterElementType(), new KnightMonsterType());

            // act
            card.ConsiderSpecialties(new SpellType(), new WaterElementType());

            // assert
            Assert.Zero(card.damage);
        }

        [Test]
        public void TestConsiderSpecialty_Dragon()
        {
            // arrange
            var card = new GameCard("asdf", 10, new FireElementType(), new DragonMonsterType());

            // act
            card.ConsiderSpecialties(new ElveMonsterType(), new FireElementType());

            // assert
            Assert.Zero(card.damage);
        }
    }
}