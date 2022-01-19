using MonsterTradingCards.Server.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Cards
{
    internal class DatabaseCardRepository : DatabaseRepository, ICardRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS cards (cardId VARCHAR PRIMARY KEY, cardName VARCHAR, damage DECIMAL, owner VARCHAR, " +
            "FOREIGN KEY (owner) REFERENCES users(username))";

        private const string InsertCardCommand = "INSERT INTO cards(cardId, cardName, damage, owner) VALUES (@cardId, @cardName, @damage, @owner)";
        private const string SelectCardById = "SELECT cardId, cardName, damage, owner FROM cards WHERE cardId=@cardId";
        private const string SelectAllUserCards = "SELECT cardId, cardName, damage, owner FROM cards WHERE owner=@username";
        private const string UpdateCardCommand = "UPDATE cards SET owner=@newOwner WHERE cardId = @cardId";

        public DatabaseCardRepository(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        private void EnsureTables()
        {
            var conn = prepareConnection();
            using var cmd = new NpgsqlCommand(CreateTableCommand, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public List<Card> GetAllUserCards(string Username)
        {
            List<Card> result = new List<Card>();
            var conn = prepareConnection();
            var cmd = new NpgsqlCommand(SelectAllUserCards, conn);
            cmd.Parameters.AddWithValue("username", Username);

            
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
            conn.Close();
            return result;
        }

        public Card GetCardById(string cardId)
        {
            object[] values = new object[4];

            var conn = prepareConnection();
            var cmd = new NpgsqlCommand(SelectCardById, conn);
            cmd.Parameters.AddWithValue("cardId", cardId);

            var res = cmd.ExecuteReader();
            res.Read();
            res.GetValues(values);
            res.Close();
            conn.Close();

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
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(InsertCardCommand, conn);
                cmd.Parameters.AddWithValue("cardId", card.Id);
                cmd.Parameters.AddWithValue("cardName", card.Name);
                cmd.Parameters.AddWithValue("damage", card.Damage);
                cmd.Parameters.Add(new NpgsqlParameter<string>("owner", card.OwnerUsername));

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            conn.Close();
            return affectedRows > 0;
        }

        public bool ChangeCardOwner(Card card)
        {
            int affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(UpdateCardCommand, conn);
                cmd.Parameters.AddWithValue("cardId", card.Id);
                cmd.Parameters.Add(new NpgsqlParameter<string>("newOwner", card.OwnerUsername));
                
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            conn.Close();
            return affectedRows > 0;
        }
    }
}
