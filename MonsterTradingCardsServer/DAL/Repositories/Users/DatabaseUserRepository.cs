using Npgsql;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.DAL.Repositories.Users
{
    class DatabaseUserRepository : DatabaseRepository, IUserRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS users (username VARCHAR PRIMARY KEY, password VARCHAR, token VARCHAR, elo INTEGER, gold INTEGER, " +
            "name VARCHAR, bio VARCHAR, image VARCHAR, matches_won INTEGER, matches_lost INTEGER, matches_draw INTEGER)";

        private const string InsertUserCommand = "INSERT INTO users(username, password, token, elo, gold, matches_won, matches_lost, matches_draw) " +
            "VALUES (@username, @password, @token, @elo, @gold, 0, 0, 0)";
        private const string SelectUserByTokenCommand = "SELECT username, password, elo, gold FROM users WHERE token=@token";
        private const string SelectUserByCredentialsCommand = "SELECT username, password, elo, gold FROM users WHERE username=@username AND password=@password";
        private const string UpdateUserGoldCommand = "UPDATE users SET gold = gold + @amount WHERE username = @username";
        private const string SelectUserInfoCommand = "SELECT name, bio, image FROM users WHERE username = @username";
        private const string UpdateUserInfoCommand = "UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username";
        private const string SelectUserStatsCommand = "SELECT elo, matches_won, matches_lost, matches_draw FROM users WHERE username=@username";
        private const string SelectScoreboardCommand = "SELECT username, elo FROM users";
        private const string UpdateUserEloCommand = "UPDATE users SET elo = elo + @amount WHERE username = @username";
        private const string UpdateUserWinsCommand = "UPDATE users SET matches_won = matches_won + 1 WHERE username = @username";
        private const string UpdateUserLosesCommand = "UPDATE users SET matches_lost = matches_lost + 1 WHERE username = @username";
        private const string UpdateUserDrawsCommand = "UPDATE users SET matches_draw = matches_draw + 1 WHERE username = @username";

        public DatabaseUserRepository(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTables();
        }

        public User GetUserByAuthToken(string authToken)
        {
            User user = null;
            var conn = prepareConnection();
            using (var cmd = new NpgsqlCommand(SelectUserByTokenCommand, conn))
            {
                cmd.Parameters.AddWithValue("token", authToken);

                // take the first row, if any
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = ReadUser(reader);
                }
                reader.Close();
            }
            conn.Close();
            return user;
        }

        public User GetUserByCredentials(string username, string password)
        {
            User user = null;
            var conn = prepareConnection();
            using (var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                // take the first row, if any
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = ReadUser(reader);
                }
                reader.Close();
                
            }
            conn.Close();
            return user;
        }

        public bool InsertUser(User user)
        {
            var affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(InsertUserCommand, conn);
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
            conn.Close();
            return affectedRows > 0;
        }

        public bool AdjustUserGold(string username, int gold)
        {
            var affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(UpdateUserGoldCommand, conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("amount", gold);
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
            using var cmd = new NpgsqlCommand(CreateTableCommand, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
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

        public UserInfo GetUserInfo(string username)
        {
            var conn = prepareConnection();
            var cmd = new NpgsqlCommand(SelectUserInfoCommand, conn);
            cmd.Parameters.AddWithValue("username", username);
            
            var reader = cmd.ExecuteReader();
            var userInfo = new UserInfo() { Username = username };

            if (reader.Read())
            {
                userInfo.Name = Convert.ToString(reader["name"]);
                userInfo.Bio = Convert.ToString(reader["bio"]);
                userInfo.Image = Convert.ToString(reader["image"]);
            }
            reader.Close();
            conn.Close();
            return userInfo;
        }

        public bool UpdateUserInfo(UserInfo user)
        {
            int affected = 0;
            var conn = prepareConnection();
            try
            {
                var cmd = new NpgsqlCommand(UpdateUserInfoCommand, conn);
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("name", user.Name);
                cmd.Parameters.AddWithValue("bio", user.Bio);
                cmd.Parameters.AddWithValue("image", user.Image);

                affected = cmd.ExecuteNonQuery();
            } 
            catch (PostgresException)
            {

            }

            conn.Close();
            return affected > 0;
        }

        public UserStats GetUserStats(string username)
        {
            var conn = prepareConnection();
            var cmd = new NpgsqlCommand(SelectUserStatsCommand, conn);
            cmd.Parameters.AddWithValue("username", username);

            var reader = cmd.ExecuteReader();
            var userStats = new UserStats() { Username = username };

            if (reader.Read())
            {
                userStats.Elo = Convert.ToInt32(reader["elo"]);
                userStats.MatchesWon = Convert.ToInt32(reader["matches_won"]);
                userStats.MatchesLost = Convert.ToInt32(reader["matches_lost"]);
                userStats.MatchesDraw = Convert.ToInt32(reader["matches_draw"]);
            }
            reader.Close();
            conn.Close();
            return userStats;
        }

        public Scoreboard GetScoreboard()
        {
            var conn = prepareConnection();
            var cmd = new NpgsqlCommand(SelectScoreboardCommand, conn);

            var reader = cmd.ExecuteReader();
            var scoreboard = new Scoreboard();
            scoreboard.entries = new List<ScoreboardEntry>();

            while (reader.Read())
            {
                scoreboard.entries.Add(new ScoreboardEntry()
                {
                    Username = Convert.ToString(reader["username"]),
                    Elo = Convert.ToInt32(reader["elo"])
                });
            }
            reader.Close();

            scoreboard.entries.Sort((x, y) => y.Elo.CompareTo(x.Elo));
            conn.Close();
            return scoreboard;
        }

        public bool AdjustUserElo(string username, int elo)
        {
            var affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                using var cmd = new NpgsqlCommand(UpdateUserEloCommand, conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("amount", elo);

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

        public bool AddToUserWinLoseStat(string username, WinLoseDrawColumns result)
        {
            var affectedRows = 0;
            var conn = prepareConnection();
            try
            {
                NpgsqlCommand cmd;
                switch (result)
                {
                    case WinLoseDrawColumns.WINNER:
                        cmd = new NpgsqlCommand(UpdateUserWinsCommand, conn);
                        cmd.Parameters.AddWithValue("col", "matches_won");
                        break;
                    case WinLoseDrawColumns.LOSER:
                        cmd = new NpgsqlCommand(UpdateUserLosesCommand, conn);
                        cmd.Parameters.AddWithValue("col", "matches_lost");
                        break;
                    case WinLoseDrawColumns.DRAW:
                        cmd = new NpgsqlCommand(UpdateUserDrawsCommand, conn);
                        cmd.Parameters.AddWithValue("col", "matches_draw");
                        break;
                    default:
                        return false;
                }

                cmd.Parameters.AddWithValue("username", username);

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
