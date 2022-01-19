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
                UserRepository = new DatabaseUserRepository(connectionString);
                CardRepository = new DatabaseCardRepository(connectionString);
                PackageRepository = new DatabasePackageRepository(connectionString);
                DeckRepository = new DatabaseDeckRepository(connectionString);
                TradingsRepository = new DatabaseTradingsRepository(connectionString);
            }
            catch (NpgsqlException e)
            {
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }
    }
}
