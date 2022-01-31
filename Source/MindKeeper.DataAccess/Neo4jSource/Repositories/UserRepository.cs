using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IGraphClient _client;

        public UserRepository(IGraphClient client)
        {
            _client = client;
        }

        public async Task<User> Create(string mail, string passwordHash)
        {
            string normalizedMail = mail.ToLower();

            var parameters = new
            {
                Mail = mail,
                NormalizedMail = normalizedMail,
                PasswordHash = passwordHash,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var results = await _client.Cypher
                .Create("(user:User {parameters})")
                .WithParam("parameters", parameters)
                .Return(user => user.As<User>())
                .ResultsAsync;

            return results.FirstOrDefault();
        }

        public async Task<User> Get(int id)
        {
            var results = await _client.Cypher
                .Match("(user:User)")
                .Where($"ID(user)={id}")
                .Return(user => user.As<User>())
                .ResultsAsync;

            return results.FirstOrDefault();
        }

        public async Task<User> Get(string mail, bool isNormalizedSearch = false)
        {
            var query = _client.Cypher
                .Match("(user:User {parameters})");

            if (isNormalizedSearch)
            {
                var normalizedMail = mail.ToLower();
                query = query.WithParam("parameters", new { NormalizedMail = normalizedMail });
            }
            else
            {
                query = query.WithParam("parameters", new { Mail = mail });
            }


            var result = await query.Return(user => user.As<User>()).ResultsAsync;
            return result.FirstOrDefault();
        }

        public async Task<List<User>> GetAll()
        {
            var results = await _client.Cypher
                .Match("(user:User)")
                .Return(user => user.As<User>())
                .ResultsAsync;

            return results.ToList();
        }
    }
}
