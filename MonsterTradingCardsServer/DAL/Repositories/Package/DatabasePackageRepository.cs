using MonsterTradingCards.Server.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Package
{
    internal class DatabasePackageRepository : DatabaseRepository, IPackageRepository
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



        public DatabasePackageRepository(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public List<string> AcquireRandomPackage()
        {
            var packageAmount = GetPackageCount();
            if (packageAmount <= 0)
            {
                throw new ArgumentException("no Package available");
            }

            var random = new Random();
            int randomPackIdx = random.Next(0, packageAmount-1);
            string[] cardIds = new string[5];
            var conn = prepareConnection();

            try
            {
                //get Package
                var cmd = new NpgsqlCommand(SelectPackageByIndex, conn);
                cmd.Parameters.AddWithValue("offset", 0);//randomPackIdx);

                
                var res = cmd.ExecuteReader();
                res.Read();
                res.GetValues(cardIds);
                res.Close();

                //delete Package
                cmd = new NpgsqlCommand(DeletePackage, conn);
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

            conn.Close();
            return cardIds.ToList<string>();
        }

        public bool CreatePackage(Models.Package package)
        {
            int affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(InsertPackageCommand, conn);
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

            conn.Close();
            return affectedRows > 0;
        }


        private void EnsureTables()
        {
            var conn = prepareConnection();
            using var cmd = new NpgsqlCommand(CreateTableCommand, prepareConnection());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private int GetPackageCount()
        {
            var conn = prepareConnection();
            var result = new NpgsqlCommand(SelectPackageCountCommand, prepareConnection()).ExecuteScalar();
            conn.Close();
            return Convert.ToInt32(result);
        }
    }
}
