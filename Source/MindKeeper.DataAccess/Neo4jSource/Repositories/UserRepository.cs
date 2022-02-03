using MindKeeper.DataAccess.Neo4jSource.Extensions;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDriver _client;

        public UserRepository(IDriver client)
        {
            _client = client;
        }

        public async Task<User> Create(string mail, string passwordHash)
        {
            string normalizedMail = mail.ToLower();

            var parameters = new
            {
                Id = Guid.NewGuid(),
                Mail = mail,
                NormalizedMail = normalizedMail,
                PasswordHash = passwordHash,
                CreatedAt = DateTimeOffset.UtcNow
            };

            string query = $@"
                CREATE (user:User {{{parameters.AsProperties()}}})
                RETURN user
            ";

            using var session = _client.AsyncSession();
            var cursor = await session.RunAsync(query, parameters);

            var results = await cursor.ToListAsync<User>(r =>
            {
                var node = r["user"].As<INode>();
                var user = node.ToObject<User>();
                return user;
            });

            return results.FirstOrDefault();
        }

        public async Task<User> Get(Guid id)
        {
            var parameters = new
            {
                Id = id
            };

            string query = $@"
                    MATCH (user:User {{{parameters.AsProperties()}}})
                    RETURN user
                ";

            using var session = _client.AsyncSession();
            var cursor = await session.RunAsync(query, parameters);

            var results = await cursor.ToListAsync<User>(r =>
            {
                var node = r["user"].As<INode>();
                var user = node.ToObject<User>();
                return user;
            });

            return results.FirstOrDefault();
        }

        public async Task<User> Get(string mail, bool isNormalizedSearch = false)
        {
            object parameters;

            if (isNormalizedSearch)
            {
                var normalizedMail = mail.ToLower();
                parameters = new { NormalizedMail = normalizedMail };
            }
            else
            {
                parameters = new { Mail = mail };
            }

            string query = $@"
                    MATCH (user:User {{{parameters.AsProperties()}}})
                    RETURN user
                ";

            using var session = _client.AsyncSession();
            var cursor = await session.RunAsync(query, parameters);

            var results = await cursor.ToListAsync<User>(r =>
            {
                var node = r["user"].As<INode>();
                var user = node.ToObject<User>();
                return user;
            });

            return results.FirstOrDefault();
        }

        public async Task<List<User>> GetAll()
        {
            string query = $@"
                    MATCH (user:User)
                    RETURN user
                ";

            using var session = _client.AsyncSession();
            var cursor = await session.RunAsync(query);

            var results = await cursor.ToListAsync<User>(r =>
            {
                var node = r["user"].As<INode>();
                var user = node.ToObject<User>();
                return user;
            });

            return results;
        }
    }
}
