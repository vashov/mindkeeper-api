using Dapper;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.PostgreSource.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<User> Create(string mail, string passwordHash)
        {
            string normalizedMail = mail.ToLower();

            const string createCommand = @"
                INSERT INTO users (mail, normalized_mail, password_hash)
                VALUES (@mail, @normalizedMail, @passwordHash)
                RETURNING *;
            ";
            var user = await _connection.QuerySingleOrDefaultAsync<User>(createCommand,
                new { mail, normalizedMail, passwordHash});

            return user;
        }

        public async Task<User> Get(int id)
        {
            const string query = "SELECT * FROM users WHERE id = @id;";

            var user = await _connection.QuerySingleOrDefaultAsync<User>(query, new { id });
            return user;
        }

        public async Task<User> Get(string mail, bool isNormalizedSearch = false)
        {
            string query;
            if (isNormalizedSearch)
            {
                query = "SELECT * FROM users WHERE normalized_mail = @mail;";
                mail = mail.ToLower();
            }
            else
            {
                query = "SELECT * FROM users WHERE mail = @mail;";
            }

            var user = await _connection.QuerySingleOrDefaultAsync<User>(query, new { mail });
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            const string query = "SELECT * FROM users;";

            var users = await _connection.QueryAsync<User>(query);
            return users.ToList();
        }
    }
}
