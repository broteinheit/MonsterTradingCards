using MonsterTradingCards.Server.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Cards
{
    public class DatabaseCardRepository : ICardRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS cards (cardId VARCHAR PRIMARY KEY, cardName VARCHAR, damage DECIMAL, owner VARCHAR, " +
            "FOREIGN KEY (owner) REFERENCES users(username))";

        private const string InsertCardCommand = "INSERT INTO cards(cardId, cardName, damage, owner) VALUES (@cardId, @cardName, @damage, @owner)";
        private const string SelectCardById = "SELECT cardId, cardName, damage, owner FROM cards WHERE cardId=@cardId";
        private const string SelectAllUserCards = "SELECT cardId, cardName, damage, owner FROM cards WHERE owner=@username";
        private const string UpdateCardCommand = "UPDATE cards SET cardName=@cardName, damage=@damage, owner=@newOwner WHERE cardId = @cardId";
        private const string DeleteCardCommand = "DELETE FROM cards WHERE cardId=@cardId";

        private readonly NpgsqlConnection _connection;

        public DatabaseCardRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        public List<Card> GetAllUserCards(string Username)
        {
            List<Card> result = new List<Card>();
            var cmd = new NpgsqlCommand(SelectAllUserCards, _connection);
            cmd.Parameters.AddWithValue("username", Username);

            lock (Database.dbLock)
            {
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Card()
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Damage = reader.GetDouble(2),
                        OwnerUsername = reader.GetString(3),
                    });
                }
                reader.Close();
            }
            return result;
        }

        public Card GetCardById(string cardId)
        {
            object[] values = new object[4];

            var cmd = new NpgsqlCommand(SelectCardById, _connection);
            cmd.Parameters.AddWithValue("cardId", cardId);

            lock (Database.dbLock)
            {
                var res = cmd.ExecuteReader();
                res.Read();
                res.GetValues(values);
                res.Close();
            }

            return new Card()
            {
                Id = (string)values[0],
                Name = (string)values[1],
                Damage = Convert.ToDouble(values[2]),
                OwnerUsername = values[3] != null && values[3].GetType() != typeof(DBNull) ? (string)values[3] : null
            };
        }

        public bool InsertCard(Card card)
        {
            int affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertCardCommand, _connection);
                cmd.Parameters.AddWithValue("cardId", card.Id);
                cmd.Parameters.AddWithValue("cardName", card.Name);
                cmd.Parameters.AddWithValue("damage", card.Damage);
                cmd.Parameters.Add(new NpgsqlParameter<string>("owner", card.OwnerUsername));

                lock (Database.dbLock)
                {
                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            return affectedRows > 0;
        }

        public bool UpdateCard(Card card)
        {
            int affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(UpdateCardCommand, _connection);
                cmd.Parameters.AddWithValue("cardId", card.Id);
                cmd.Parameters.AddWithValue("cardName", card.Name);
                cmd.Parameters.AddWithValue("damage", card.Damage);
                cmd.Parameters.Add(new NpgsqlParameter<string>("newOwner", card.OwnerUsername));

                lock (Database.dbLock)
                {
                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            return affectedRows > 0;
        }

        public bool DeleteCard(Card card)
        {
            int affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(DeleteCardCommand, _connection);
                cmd.Parameters.AddWithValue("cardId", card.Id);

                lock (Database.dbLock)
                {
                    affectedRows = cmd.ExecuteNonQuery();
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
