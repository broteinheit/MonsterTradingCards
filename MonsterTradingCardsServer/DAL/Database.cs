using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.DAL.Repositories.Deck;
using MonsterTradingCards.Server.DAL.Repositories.Package;
using MonsterTradingCards.Server.DAL.Repositories.Tradings;
using MonsterTradingCards.Server.DAL.Repositories.Users;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL
{
    class Database
    {
        private readonly NpgsqlConnection _connection;

        public IUserRepository UserRepository { get; private set; }
        public ICardRepository CardRepository { get; private set; }
        public IPackageRepository PackageRepository { get; private set; }
        public IDeckRepository DeckRepository { get; private set; }
        public ITradingsRepository TradingsRepository { get; private set; }
        public static object dbLock { get; private set; } = new object();

        public Database(string connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();

                // first users, then messages
                // we need this special order since messages has a foreign key to users
                UserRepository = new DatabaseUserRepository(_connection);
                CardRepository = new DatabaseCardRepository(_connection);
                PackageRepository = new DatabasePackageRepository(_connection);
                DeckRepository = new DatabaseDeckRepository(_connection);
                TradingsRepository = new DatabaseTradingsRepository(_connection);
            }
            catch (NpgsqlException e)
            {
                // provide our own custom exception
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }
    }
}
