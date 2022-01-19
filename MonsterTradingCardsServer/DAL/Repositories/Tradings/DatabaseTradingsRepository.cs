using MonsterTradingCards.Server.BattleLogic.Cards.CardType;
using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.Utility;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Tradings
{
    internal class DatabaseTradingsRepository : DatabaseRepository, ITradingsRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS tradings (id VARCHAR PRIMARY KEY, username VARCHAR, cardToTradeId VARCHAR, cardtype VARCHAR, minDamage DECIMAL," +
            "CONSTRAINT fk_cardToTrade FOREIGN KEY (cardToTradeId) REFERENCES cards(cardId), CONSTRAINT fk_username FOREIGN KEY (username) REFERENCES users(username))";

        private const string InsertTradingCommand = "INSERT INTO tradings(id, username, cardToTradeId, cardtype, minDamage) " +
            "VALUES (@id, @username, @cardToTradeId, @cardtype, @minDamage)";
        private const string SelectAllTradings = "SELECT id, cardToTradeId, cardtype, minDamage FROM tradings";
        private const string SelectTradingById = "SELECT username, cardToTradeId, cardtype, minDamage FROM tradings WHERE id=@id";
        private const string DeleteTradings = "DELETE FROM tradings WHERE id=@id";

        public DatabaseTradingsRepository(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public bool CreateTrade(Trade trade)
        {
            string cardtype;
            switch (trade.TypeToTrade)
            {
                case TradeCardType.MONSTER:
                    cardtype = "monster";
                    break;
                case TradeCardType.SPELL:
                    cardtype = "spell";
                    break;
                case TradeCardType.NONE:
                    cardtype = "none";
                    break;
                default:
                    cardtype = null;
                    break;
            }

            int affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(InsertTradingCommand, conn);
                cmd.Parameters.AddWithValue("id", trade.Id);
                cmd.Parameters.AddWithValue("username", trade.Username);
                cmd.Parameters.AddWithValue("cardToTradeId", trade.CardToTradeId);
                cmd.Parameters.AddWithValue("cardtype", cardtype);
                cmd.Parameters.AddWithValue("minDamage", trade.MinDamage);

                
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

        public bool DeleteTrade(string tradeId)
        {
            int affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(DeleteTradings, conn);
                cmd.Parameters.AddWithValue("id", tradeId);
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

        public List<Trade> GetAllTrades()
        {
            var tradeList = new List<Trade>();
            var conn = prepareConnection();
            var cmd = new NpgsqlCommand(SelectAllTradings, conn);

            
            var res = cmd.ExecuteReader();

            if (!res.HasRows)
            {
                res.Close();
                return new List<Trade>();
            }

            while (res.Read())
            {
                tradeList.Add(new Trade()
                {
                    Id = Convert.ToString(res["id"]),
                    Username = null,
                    CardToTradeId = Convert.ToString(res["cardToTradeId"]),
                    TypeToTrade = ConvertToTradeCardTypeFromString((string)res["cardtype"]),
                    MinDamage = Convert.ToDouble(res["minDamage"])
                });
            }
            res.Close();
            conn.Close();

            return tradeList;
        }

        public Trade GetTrade(string tradeId)
        {
            //get trade
            object[] values = new object[4];

            var conn = prepareConnection();
            var cmd = new NpgsqlCommand(SelectTradingById, conn);
            cmd.Parameters.AddWithValue("id", tradeId);

            var res = cmd.ExecuteReader();
            res.Read();
            res.GetValues(values);
            res.Close();
            conn.Close();

            return new Trade()
            {
                Id = tradeId,
                Username = (string)values[0],
                CardToTradeId = (string)values[1],
                TypeToTrade = ConvertToTradeCardTypeFromString((string)values[2]),
                MinDamage = Convert.ToDouble(values[3])
            };
        }

        private void EnsureTables()
        {
            var conn = prepareConnection();
            using var cmd = new NpgsqlCommand(CreateTableCommand, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private TradeCardType ConvertToTradeCardTypeFromString(string str)
        {
            switch (str)
            {
                case "monster":
                    return TradeCardType.MONSTER;
                case "spell":
                    return TradeCardType.SPELL;
                case "none":
                    return TradeCardType.NONE;
                default:
                    return TradeCardType.ERROR;
            }
        }
    }
}
