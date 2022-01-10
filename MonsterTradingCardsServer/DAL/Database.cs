using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsServer.DAL
{
    class Database
    {
        private readonly NpgsqlConnection _connection;

        public IMessageRepository MessageRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        public Database(string connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();

                // first users, then messages
                // we need this special order since messages has a foreign key to users
                UserRepository = new DatabaseUserRepository(_connection);
                MessageRepository = new DatabaseMessageRepository(_connection);
            }
            catch (NpgsqlException e)
            {
                // provide our own custom exception
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }
    }
}
