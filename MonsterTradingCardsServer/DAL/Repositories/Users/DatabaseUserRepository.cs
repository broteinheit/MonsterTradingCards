﻿using Npgsql;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Users
{
    class DatabaseUserRepository : IUserRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS users (username VARCHAR PRIMARY KEY, password VARCHAR, token VARCHAR, elo INTEGER, gold INTEGER)";

        private const string InsertUserCommand = "INSERT INTO users(username, password, token, elo, gold) VALUES (@username, @password, @token, @elo, @gold)";
        private const string SelectUserByTokenCommand = "SELECT username, password, elo, gold FROM users WHERE token=@token";
        private const string SelectUserByCredentialsCommand = "SELECT username, password, elo, gold FROM users WHERE username=@username AND password=@password";
        private const string UpdateUserGoldCommand = "UPDATE users SET gold = gold + @amount WHERE username = @username";

        private readonly NpgsqlConnection _connection;

        public DatabaseUserRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        public User GetUserByAuthToken(string authToken)
        {
            User user = null;
            using (var cmd = new NpgsqlCommand(SelectUserByTokenCommand, _connection))
            {
                cmd.Parameters.AddWithValue("token", authToken);

                // take the first row, if any
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = ReadUser(reader);
                }
                reader.Close();
            }
            return user;
        }

        public User GetUserByCredentials(string username, string password)
        {
            User user = null;
            using (var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, _connection))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                // take the first row, if any
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = ReadUser(reader);
                }
            }
            return user;
        }

        public bool InsertUser(User user)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertUserCommand, _connection);
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("token", user.Token);
                cmd.Parameters.AddWithValue("elo", user.Elo);
                cmd.Parameters.AddWithValue("gold", user.Gold);
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the user already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }
            return affectedRows > 0;
        }

        public bool AdjustUserGold(string username, int gold)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(UpdateUserGoldCommand, _connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("amount", gold);
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

        private User ReadUser(IDataRecord record)
        {
            var user = new User
            {
                Username = Convert.ToString(record["username"]),
                Password = Convert.ToString(record["password"]),
                Elo = Convert.ToInt32(record["elo"]),
                Gold = Convert.ToInt32(record["gold"])
            };
            return user;
        }
    }
}
