using MonsterTradingCards.Server.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Cards
{
    internal class DatabaseCardRepository : ICardRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS cards (cardId VARCHAR PRIMARY KEY, cardName VARCHAR, damage DECIMAL, owner VARCHAR, " +
            "FOREIGN KEY (owner) REFERENCES users(username))";

        private const string InsertCardCommand = "INSERT INTO cards(cardId, cardName, damage, owner) VALUES (@cardId, @cardName, @damage, @owner)";
        private const string SelectCardById = "SELECT cardId, cardName, damage, owner FROM cards WHERE cardId=@cardId";
        private const string SelectAllUserCards = "SELECT cardId, cardName, damage, owner FROM users WHERE owner=@username";
        private const string UpdateCardCommand = "UPDATE cards SET owner=@newOwner WHERE cardId = @cardId";

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
            throw new NotImplementedException();
        }

        public Card GetCardById(string cardId)
        {
            object[] values = new object[4];

            var cmd = new NpgsqlCommand(SelectCardById, _connection);
            cmd.Parameters.AddWithValue("cardId", cardId);
            var res = cmd.ExecuteReader();
            res.Read();
            res.GetValues(values);
            res.Close();

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
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            return affectedRows > 0;
        }

        public bool ChangeCardOwner(Card card)
        {
            int affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(UpdateCardCommand, _connection);
                cmd.Parameters.AddWithValue("cardId", card.Id);
                cmd.Parameters.Add(new NpgsqlParameter<string>("newOwner", card.OwnerUsername));
                affectedRows = cmd.ExecuteNonQuery();
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
