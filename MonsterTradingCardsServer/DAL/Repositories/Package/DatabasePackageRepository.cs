using MonsterTradingCards.Server.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Package
{
    internal class DatabasePackageRepository : IPackageRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS packages (cardOneId VARCHAR, cardTwoId VARCHAR, cardThreeId VARCHAR, cardFourId VARCHAR, cardFiveId VARCHAR, " +
            "CONSTRAINT pk PRIMARY KEY (cardOneId, cardTwoId, cardThreeId, cardFourId, cardFiveId), " +
            "CONSTRAINT fk_cardOne FOREIGN KEY (cardOneId) REFERENCES cards(cardId), CONSTRAINT fk_cardTwo FOREIGN KEY (cardTwoId) REFERENCES cards(cardId)," +
            "CONSTRAINT fk_cardThree FOREIGN KEY (cardThreeId) REFERENCES cards(cardId), CONSTRAINT fk_cardFour FOREIGN KEY (cardFourId) REFERENCES cards(cardId)," +
            "CONSTRAINT fk_cardFive FOREIGN KEY (cardFiveId) REFERENCES cards(cardId))";

        private const string InsertPackageCommand = "INSERT INTO packages(cardOneId, cardTwoId, cardThreeId, cardFourId, cardFiveId) " +
            "VALUES (@cardOneId, @cardTwoId, @cardThreeId, @cardFourId, @cardFiveId)";
        private const string SelectPackageByIndex = "SELECT cardOneId, cardTwoId, cardThreeId, cardFourId, cardFiveId FROM packages LIMIT 1 OFFSET @offset;";
        private const string SelectPackageCountCommand = "SELECT count(*) FROM packages";
        private const string DeletePackage = "DELETE FROM packages WHERE cardOneId=@cardOneId AND cardTwoId=@cardTwoId AND cardThreeId=@cardThreeId AND cardFourId=@cardFourId AND cardFiveId=@cardFiveId";

        private readonly NpgsqlConnection _connection;


        public DatabasePackageRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        public List<string> AcquireRandomPackage()
        {
            var random = new Random();
            int randomPackIdx = random.Next(0, GetPackageCount()-1);
            string[] cardIds = new string[5];

            try
            {
                //get Package
                var cmd = new NpgsqlCommand(SelectPackageByIndex, _connection);
                cmd.Parameters.AddWithValue("offset", randomPackIdx);

                var res = cmd.ExecuteReader();
                res.Read();
                res.GetValues(cardIds);
                res.Close();

                //delete Package
                cmd = new NpgsqlCommand(DeletePackage, _connection);
                cmd.Parameters.AddWithValue("cardOneId", cardIds[0]);
                cmd.Parameters.AddWithValue("cardTwoId", cardIds[1]);
                cmd.Parameters.AddWithValue("cardThreeId", cardIds[2]);
                cmd.Parameters.AddWithValue("cardFourId", cardIds[3]);
                cmd.Parameters.AddWithValue("cardFiveId", cardIds[4]);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Package could not be deleted");
                }
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }

            return cardIds.ToList<string>();
        }

        public bool CreatePackage(Models.Package package)
        {
            int affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertPackageCommand, _connection);
                cmd.Parameters.AddWithValue("cardOneId", package.Cards[0].Id);
                cmd.Parameters.AddWithValue("cardTwoId", package.Cards[1].Id);
                cmd.Parameters.AddWithValue("cardThreeId", package.Cards[2].Id);
                cmd.Parameters.AddWithValue("cardFourId", package.Cards[3].Id);
                cmd.Parameters.AddWithValue("cardFiveId", package.Cards[4].Id);
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            return affectedRows > 0;
        }


        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        private int GetPackageCount()
        {
            return Convert.ToInt32(new NpgsqlCommand(SelectPackageCountCommand, _connection).ExecuteScalar());
        }
    }
}
