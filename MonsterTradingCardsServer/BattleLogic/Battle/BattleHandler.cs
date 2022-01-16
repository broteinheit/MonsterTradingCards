using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Server.BattleLogic.Cards;
using MonsterTradingCards.Server.BattleLogic.Logger;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;

namespace MonsterTradingCards.Server.BattleLogic.Battle
{
    internal class BattleHandler
    {
        public BattleHandler(IDeckManager deckManager, ICardManager cardManager)
        {
            battleState = BattleState.WAITING;
            battleLogger = new InMemoryLogger();
            logBuilder = new BattleLogStringBuilder();
            battleLogic = new BattleLogic(ref logBuilder);
            this.deckManager = deckManager;
            this.cardManager = cardManager;
        }

        public BattleHandler(User player1, IDeckManager deckManager, ICardManager cardManager) 
            : this(deckManager, cardManager)
        {
            ConnectPlayer(player1);
        }

        public BattleHandler(User player1, User player2, IDeckManager deckManager, ICardManager cardManager) 
            : this(player1, deckManager, cardManager)
        {
            ConnectPlayer(player2);
        }

        public bool ConnectPlayer(User player)
        {
            //If game already in progress or done -> return false
            if (battleState != BattleState.WAITING)
            {
                return false;
            }


            if (player1 == null)        //if player 1 not set
            {
                player1 = player;
                return true;
            }
            else if (player2 == null && player != player1)   //if player 2 not set and player-to-connect is not already player 1
            {
                player2 = player;
                return true;
            }

            return false;               //if both are already set
        }

        public bool DisconnectPlayer(User player)
        {
            //if game is already in progress or done -> cannot disconnect player
            if (battleState != BattleState.WAITING)
            {
                return false;
            }

            if (player1 == player)          // if player 1 wants to disconnect
            {
                player1 = null;
                return true;
            }
            else if (player2 == player)     // if player 2 wants to disconnect
            {
                player2 = null;
                return true;
            }

            return false;
        }

        public BattleResult StartBattle()
        {
            //if not both are connected -> throw error
            if (player1 == null || player2 == null)
            {
                // TODO own Exception class
                throw new Exception("To start a battle, two players need to be connected!");
            }

            if (battleState != BattleState.WAITING)
            {
                throw new Exception("Game not in waiting state!");
            }

            logBuilder.player1Name = player1.Username;
            logBuilder.player2Name = player2.Username;
            battleState = BattleState.IN_PROGRESS;
            Cards.Deck deckP1 = new Cards.Deck(deckManager.GetDeckByUsername(player1.Username), cardManager);
            Cards.Deck deckP2 = new Cards.Deck(deckManager.GetDeckByUsername(player2.Username), cardManager);

            //Start fight (while not round 100 reached and both players still have cards)
            while (round < 100 && deckP1.GetCardCount() > 0 && deckP2.GetCardCount() > 0)   //TODO change access to decks
            {
                GameCard cardP1 = deckP1.DrawRandomCard();
                GameCard cardP2 = deckP2.DrawRandomCard();


                logBuilder.reset();
                logBuilder.cardP1Name = cardP1.GetCardName();
                logBuilder.cardP2Name = cardP2.GetCardName();
                logBuilder.initialDamageP1 = cardP1.damage;
                logBuilder.initialDamageP2 = cardP2.damage;

                BattleRoundResult roundWinner = battleLogic.RunRound(cardP1.Clone(), cardP2.Clone());

                switch (roundWinner)
                {
                    //Player 1 won the round -> both cards to player 1 deck
                    case BattleRoundResult.PLAYER1_WIN:
                        deckP1.AddCard(cardP1);
                        deckP1.AddCard(cardP2);
                        logBuilder.winnerCardName = cardP1.GetCardName();
                        logBuilder.loserCardName = cardP2.GetCardName();
                        break;
                    //Player 2 won the round -> both cards to player 2 deck
                    case BattleRoundResult.PLAYER2_WIN:
                        deckP2.AddCard(cardP1);
                        deckP2.AddCard(cardP2);
                        logBuilder.winnerCardName = cardP2.GetCardName();
                        logBuilder.loserCardName = cardP1.GetCardName();
                        break;
                    //draw -> both cards to their respective deck
                    case BattleRoundResult.DRAW:
                        deckP1.AddCard(cardP1);
                        deckP2.AddCard(cardP2);
                        logBuilder.draw = true;
                        break;
                    default:
                        //throw error -> return value out of range
                        break;
                }

                round++;
                battleLogger.AddRoundLog(logBuilder.GetLogString());
            }

            if (deckP1.GetCardCount() <= 0)         //if Player 1 has 0 cards -> player 2 wins
            {
                battleResult = BattleResult.PLAYER_TWO;
                battleLogger.AddResultLog(player2.Username, player1.Username);
            }
            else if (deckP2.GetCardCount() <= 0)    //if Player 2 has 0 cards -> player 1 wins
            {
                battleResult = BattleResult.PLAYER_ONE;
                battleLogger.AddResultLog(player1.Username, player2.Username);
            }
            else                            //draw
            {
                battleResult = BattleResult.DRAW;
                battleLogger.AddResultLogDraw();
            }

            battleState = BattleState.DONE;
            return battleResult;
        }

        public BattleResult GetResult()
        {
            if (battleState == BattleState.DONE)
            {
                return battleResult;
            }
            else
            {
                return BattleResult.ERROR;
            }
        }


        public User player1 { get; private set; } = null;
        public User player2 { get; private set; } = null;
        public BattleLogic battleLogic { get; private set; }
        public BattleState battleState { get; private set; }
        public int round { get; private set; } = 0;
        public IBattleLogger battleLogger { get; private set; }
        
        private BattleLogStringBuilder logBuilder;
        private BattleResult battleResult;
        private IDeckManager deckManager;
        private ICardManager cardManager;
    }
}
