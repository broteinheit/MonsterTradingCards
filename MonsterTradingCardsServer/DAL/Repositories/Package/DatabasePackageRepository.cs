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
        private const string SelectPackageByIdCommand = "SELECT pkgId, cardOneId, cardTwoId, cardThreeId, cardFourId FROM packages WHERE pkgId=@pkgId";

        private readonly NpgsqlConnection _connection;


        public DatabasePackageRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        public Models.Package acquirePackage(string packageId)
        {
            throw new NotImplementedException();
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
    }
}
