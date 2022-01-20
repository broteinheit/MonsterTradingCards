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
    public class TestsBattleLogic
    {
        [Test]
        public void TestRunRound_MonsterFight()
        {
            // arrange
            var logger = new BattleLogStringBuilder();
            var battleLogic = new BattleLogic.Battle.BattleLogic(ref logger);
            var card1 = new GameCard("asdf", 10, new WaterElementType(), new GoblinMonsterType());
            var card2 = new GameCard("qwer", 15, new FireElementType(), new GoblinMonsterType());

            // act
            // assert
            Assert.AreEqual(battleLogic.RunRound(card1, card2), BattleRoundResult.PLAYER2_WIN);
        }

        [Test]
        public void TestRunRound_SpellFight()
        {
            // arrange
            var logger = new BattleLogStringBuilder();
            var battleLogic = new BattleLogic.Battle.BattleLogic(ref logger);
            var card1 = new GameCard("asdf", 10, new WaterElementType(), new SpellType());
            var card2 = new GameCard("qwer", 15, new FireElementType(), new SpellType());

            // act
            // assert
            Assert.AreEqual(battleLogic.RunRound(card1, card2), BattleRoundResult.PLAYER1_WIN);
        }

        [Test]
        public void TestRunRound_MixedFight()
        {
            // arrange
            var logger = new BattleLogStringBuilder();
            var battleLogic = new BattleLogic.Battle.BattleLogic(ref logger);
            var card1 = new GameCard("asdf", 10, new WaterElementType(), new GoblinMonsterType());
            var card2 = new GameCard("qwer", 15, new FireElementType(), new SpellType());

            // act
            // assert
            Assert.AreEqual(battleLogic.RunRound(card1, card2), BattleRoundResult.PLAYER1_WIN);
        }

        [Test]
        public void TestRunRound_Draw()
        {
            // arrange
            var logger = new BattleLogStringBuilder();
            var battleLogic = new BattleLogic.Battle.BattleLogic(ref logger);
            var card1 = new GameCard("asdf", 10, new WaterElementType(), new GoblinMonsterType());
            var card2 = new GameCard("qwer", 10, new FireElementType(), new GoblinMonsterType());

            // act
            // assert
            Assert.AreEqual(battleLogic.RunRound(card1, card2), BattleRoundResult.DRAW);
        }

        [Test]
        public void TestRunRound_Specialty()
        {
            // arrange
            var logger = new BattleLogStringBuilder();
            var battleLogic = new BattleLogic.Battle.BattleLogic(ref logger);
            var card1 = new GameCard("asdf", 50, new NormalElementType(), new KnightMonsterType());
            var card2 = new GameCard("qwer", 15, new WaterElementType(), new SpellType());

            // act
            // assert
            Assert.AreEqual(battleLogic.RunRound(card1, card2), BattleRoundResult.PLAYER2_WIN);
        }
    }
}