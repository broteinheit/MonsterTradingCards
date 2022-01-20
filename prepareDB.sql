-- drop and create database
CREATE DATABASE monster_trading_cards;

-- create tables
CREATE TABLE IF NOT EXISTS cards (cardId VARCHAR PRIMARY KEY, cardName VARCHAR, damage DECIMAL, owner VARCHAR, 
            FOREIGN KEY (owner) REFERENCES users(username));
CREATE TABLE IF NOT EXISTS decks (ownerId VARCHAR PRIMARY KEY, cardIdOne VARCHAR, cardIdTwo VARCHAR, cardIdThree VARCHAR, 
            cardIdFour VARCHAR, FOREIGN KEY (ownerId) REFERENCES users(username), FOREIGN KEY (cardIdOne) REFERENCES cards(cardId), 
            FOREIGN KEY (cardIdTwo) REFERENCES cards(cardId), FOREIGN KEY (cardIdThree) REFERENCES cards(cardId), FOREIGN KEY (cardIdFour) REFERENCES cards(cardId));
CREATE TABLE IF NOT EXISTS packages (cardOneId VARCHAR, cardTwoId VARCHAR, cardThreeId VARCHAR, cardFourId VARCHAR, cardFiveId VARCHAR,
            CONSTRAINT pk PRIMARY KEY (cardOneId, cardTwoId, cardThreeId, cardFourId, cardFiveId), 
            CONSTRAINT fk_cardOne FOREIGN KEY (cardOneId) REFERENCES cards(cardId), CONSTRAINT fk_cardTwo FOREIGN KEY (cardTwoId) REFERENCES cards(cardId),
            CONSTRAINT fk_cardThree FOREIGN KEY (cardThreeId) REFERENCES cards(cardId), CONSTRAINT fk_cardFour FOREIGN KEY (cardFourId) REFERENCES cards(cardId),
            CONSTRAINT fk_cardFive FOREIGN KEY (cardFiveId) REFERENCES cards(cardId));
CREATE TABLE IF NOT EXISTS tradings (id VARCHAR PRIMARY KEY, username VARCHAR, cardToTradeId VARCHAR, cardtype VARCHAR, minDamage DECIMAL,
            CONSTRAINT fk_cardToTrade FOREIGN KEY (cardToTradeId) REFERENCES cards(cardId), CONSTRAINT fk_username FOREIGN KEY (username) REFERENCES users(username));
CREATE TABLE IF NOT EXISTS users (username VARCHAR PRIMARY KEY, password VARCHAR, token VARCHAR, elo INTEGER, gold INTEGER, 
            name VARCHAR, bio VARCHAR, image VARCHAR, matches_won INTEGER, matches_lost INTEGER, matches_draw INTEGER)