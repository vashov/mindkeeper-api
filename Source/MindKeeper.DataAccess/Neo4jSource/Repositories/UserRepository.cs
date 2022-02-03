using MindKeeper.DataAccess.Neo4jSource.Extensions;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
                Mail = mail,
                NormalizedMail = normalizedMail,
                PasswordHash = passwordHash,
                CreatedAt = DateTimeOffset.UtcNow
            };

            using var session = _client.AsyncSession();

            string query = $@"
                CREATE (user:User {{{parameters.AsProperties()}}})
                RETURN user
            ";
            var cursor = await session.RunAsync(query, parameters);

            var results = await cursor.ToListAsync<User>(r =>
            {
                var node = r["user"].As<INode>();
                var user = node.AsEntity<User>();
                return user;
            });

                //var results = await _client.Cypher
                //.Create("(user:User {parameters})")
                //.WithParam("parameters", parameters)
                //.Return(user => user.As<User>())
                //.ResultsAsync;

            return results.FirstOrDefault();
        }

        public async Task<User> Get(long id)
        {
            //var results = await _client.Cypher
            //    .Match("(user:User)")
            //    .Where($"ID(user)={id}")
            //    .Return(user => user.As<User>())
            //    .ResultsAsync;

            //return results.FirstOrDefault();

            return null;
        }

        public async Task<User> Get(string mail, bool isNormalizedSearch = false)
        {
            using var session = _client.AsyncSession();

            string query;
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

            query = $@"
                    MATCH (user:User {{{parameters.AsProperties()}}})
                    RETURN user
                ";

            var cursor = await session.RunAsync(query, parameters);

            var results = await cursor.ToListAsync<User>(r =>
            {
                var node = r["user"].As<INode>();
                var user = node.AsEntity<User>();
                return user;
            });

            //var query = _client.Cypher
            //    .Match("(user:User {parameters})");

            //if (isNormalizedSearch)
            //{
            //    var normalizedMail = mail.ToLower();
            //    query = query.WithParam("parameters", new { NormalizedMail = normalizedMail });
            //}
            //else
            //{
            //    query = query.WithParam("parameters", new { Mail = mail });
            //}


            //var result = await query.Return(user => user.As<User>()).ResultsAsync;
            return results.FirstOrDefault();
        }

        public async Task<List<User>> GetAll()
        {
            //var results = await _client.Cypher
            //    .Match("(user:User)")
            //    .Return(user => user.As<User>())
            //    .ResultsAsync;

            //return results.ToList();
            return null;
        }
    }
}
