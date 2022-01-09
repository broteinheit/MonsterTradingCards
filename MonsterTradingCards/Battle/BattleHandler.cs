﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Cards;
using MonsterTradingCards.Logger;

namespace MonsterTradingCards.Battle
{
    internal class BattleHandler
    {
        public BattleHandler()
        {
            battleState = BattleState.WAITING;
            battleLogger = new InMemoryLogger();
            logBuilder = new BattleLogStringBuilder();
            battleLogic = new BattleLogic(ref logBuilder);
        }

        public BattleHandler(int player1Id) : this()
        {
            ConnectPlayer(player1Id);
        }

        public BattleHandler(int player1Id, int player2Id) : this(player1Id)
        {
            ConnectPlayer(player2Id);
        }

        public bool ConnectPlayer(int playerId)
        {
            //If game already in progress or done -> return false
            if (battleState != BattleState.WAITING)
            {
                return false;
            }


            if (player1Id == -1)        //if player 1 not set
            {
                player1Id = playerId;
                return true;
            }
            else if (player2Id == -1)   //if player 2 not set
            {
                player2Id = playerId;
                return true;
            }

            return false;               //if both are already set
        }

        public bool DisconnectPlayer(int playerId)
        {
            //if game is already in progress or done -> cannot disconnect player
            if (battleState != BattleState.WAITING)
            {
                return false;
            }

            if (player1Id == playerId)          // if player 1 wants to disconnect
            {
                player1Id = -1;
                return true;
            }
            else if (player2Id == playerId)     // if player 2 wants to disconnect
            {
                player2Id = -1;
                return true;
            }

            return false;
        }

        public int StartBattle()
        {
            //if not both are connected -> throw error
            if (player1Id == -1 || player2Id == -1)
            {
                // TODO own Exception class
                throw new Exception("To start a battle, two players need to be connected");
            }

            logBuilder.player1Name = player1Id.ToString();  //TODO change to actaul name when user handling exists
            logBuilder.player2Name = player2Id.ToString();  //TODO change to actual name when user handling exists
            battleState = BattleState.IN_PROGRESS;
            //TODO fetch decks from DB/Repository

            //Start fight (while not round 100 reached and both players still have cards)
            while (round < 100 && cardsP1.Count > 0 && cardsP2.Count > 0)   //TODO change access to decks
            {
                Card cardP1 = DrawCardFromDeck(cardsP1);
                Card cardP2 = DrawCardFromDeck(cardsP2);


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
                        AddCardToDeck(cardsP1, cardP1);
                        AddCardToDeck(cardsP1, cardP2);
                        logBuilder.winnerCardName = cardP1.GetCardName();
                        logBuilder.loserCardName = cardP2.GetCardName();
                        break;
                    //Player 2 won the round -> both cards to player 2 deck
                    case BattleRoundResult.PLAYER2_WIN:
                        AddCardToDeck(cardsP2, cardP1);
                        AddCardToDeck(cardsP2, cardP2);
                        logBuilder.winnerCardName = cardP2.GetCardName();
                        logBuilder.loserCardName = cardP1.GetCardName();
                        break;
                    //draw -> both cards to their respective deck
                    case BattleRoundResult.DRAW:
                        AddCardToDeck(cardsP1, cardP1);
                        AddCardToDeck(cardsP2, cardP2);
                        logBuilder.draw = true;
                        break;
                    default:
                        //throw error -> return value out of range
                        break;
                }

                round++;
                battleLogger.AddRoundLog(logBuilder.GetLogString());
            }
            battleState = BattleState.DONE;


            if (cardsP1.Count <= 0)         //if Player 1 has 0 cards -> player 2 wins
            {
                //TODO adjust elo
                return player2Id;
            }
            else if (cardsP2.Count <= 0)    //if Player 2 has 0 cards -> player 1 wins
            {
                //TODO adjust elo
                return player1Id;
            }
            else                            //draw
            {
                return -1;
            }
        }


        //TODO Parameter List<Card> to be replace by deck class
        private Card DrawCardFromDeck(List<Card> deck)
        {
            Random r = new Random();
            Card c = deck.ElementAt(r.Next(deck.Count));
            deck.Remove(c);
            return c;
        }

        //TODO put into deck class when exists
        private void AddCardToDeck(List<Card> deck, Card c)
        {
            deck.Add(c);
        }



        public int player1Id { get; private set; } = -1;
        public int player2Id { get; private set; } = -1;
        public List<Card> cardsP1 { get; set; } = new List<Card>();     //TODO to be replaced by deck class, set property set to private
        public List<Card> cardsP2 { get; set; } = new List<Card>();     //TODO to be replaced by deck class, set property set to private
        public BattleLogic battleLogic { get; private set; }
        public BattleState battleState { get; private set; }
        public int round { get; private set; } = 0;
        public IBattleLogger battleLogger { get; private set; }
        private BattleLogStringBuilder logBuilder;
    }
}
