using MonsterTradingCards.Server.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Deck
{
    public class DatabaseDeckRepository : IDeckRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS decks (ownerId VARCHAR PRIMARY KEY, cardIdOne VARCHAR, cardIdTwo VARCHAR, cardIdThree VARCHAR, " +
            "cardIdFour VARCHAR, FOREIGN KEY (ownerId) REFERENCES users(username), FOREIGN KEY (cardIdOne) REFERENCES cards(cardId), " +
            "FOREIGN KEY (cardIdTwo) REFERENCES cards(cardId), FOREIGN KEY (cardIdThree) REFERENCES cards(cardId), FOREIGN KEY (cardIdFour) REFERENCES cards(cardId))";

        private const string InsertDeckCommand = "INSERT INTO decks(ownerId, cardIdOne, cardIdTwo, cardIdThree, cardIdFour) " +
            "VALUES (@ownerId, @cardOne, @cardTwo, @cardThree, @cardFour)";
        private const string SelectDeckByOwnerId = "SELECT cardIdOne, cardIdTwo, cardIdThree, cardIdFour FROM decks WHERE ownerId=@ownerId";
        private const string UpdateDeckCommand = "UPDATE decks SET cardIdOne=@cardOne, cardIdTwo=@cardTwo, cardIdThree=@cardThree, cardIdFour=@cardFour WHERE ownerId = @ownerId";

        private readonly NpgsqlConnection _connection;

        public DatabaseDeckRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        public Models.Deck GetDeck(string username)
        {
            string[] values = new string[4];

            var cmd = new NpgsqlCommand(SelectDeckByOwnerId, _connection);
            cmd.Parameters.AddWithValue("ownerId", username);

            lock (Database.dbLock)
            {
                var res = cmd.ExecuteReader();

                if (!res.HasRows)
                {
                    res.Close();
                    return new Models.Deck();
                }

                res.Read();
                res.GetValues(values);
                res.Close();
            }

            return new Models.Deck() 
            { 
                owner = username,
                cardIds = new List<string>() { values[0], values[1], values[2], values[3]} 
            }; 
        }

        public bool SetDeck(Models.Deck deck)
        {
            if (deck.cardIds.Count != 4)
            {
                throw new Exception($"a Deck needs exactly 4 cards (given: {deck.cardIds.Count})");
            }
            else if (deck.cardIds.GroupBy(n => n).Any(c => c.Count() > 1))
            {
                throw new Exception($"any given card can only be input once in a deck");
            }

            int affectedRows = 0;
            try
            {
                if (GetDeck(deck.owner).owner == null)
                {
                    using var cmd = new NpgsqlCommand(InsertDeckCommand, _connection);
                    cmd.Parameters.AddWithValue("ownerId", deck.owner);
                    cmd.Parameters.AddWithValue("cardOne", deck.cardIds[0]);
                    cmd.Parameters.AddWithValue("cardTwo", deck.cardIds[1]);
                    cmd.Parameters.AddWithValue("cardThree", deck.cardIds[2]);
                    cmd.Parameters.AddWithValue("cardFour", deck.cardIds[3]);

                    lock (Database.dbLock)
                    {
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using var cmd = new NpgsqlCommand(UpdateDeckCommand, _connection);
                    cmd.Parameters.AddWithValue("ownerId", deck.owner);
                    cmd.Parameters.AddWithValue("cardOne", deck.cardIds[0]);
                    cmd.Parameters.AddWithValue("cardTwo", deck.cardIds[1]);
                    cmd.Parameters.AddWithValue("cardThree", deck.cardIds[2]);
                    cmd.Parameters.AddWithValue("cardFour", deck.cardIds[3]);

                    lock (Database.dbLock)
                    {
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            return affectedRows > 0;
        }
    }
}
