using MindKeeper.DataAccess.Neo4jSource.Tokens;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class AchievementsRepository : IAchievementsRepository
    {
        private readonly IDriver _client;

        public AchievementsRepository(IDriver driver)
        {
            _client = driver;
        }

        public async Task<List<Achievement>> GetAchievements(Guid? userId)
        {
            string query = $@"
                    MATCH (u:{Label.User})-[r:{Relationship.HAS_ACHIEVEMENT}->(a:{Label.Achievement})
                    RETURN count(u) as count
                    UNION ALL
                    MATCH (i:{Label.Idea})
                    RETURN count(i) as count
                ";

            object parameters;

            if (userId.HasValue)
            {
                parameters = new
                {
                    UserId = userId
                };

                query = $@"
                    MATCH (u:{Label.User} {{Id:$UserId}})-[r:{Relationship.HAS_ACHIEVEMENT}->(a:{Label.Achievement})
                    RETURN a;
                ";
            }
            else
            {
                parameters = new { };
                query = $@"
                    MATCH (a:{Label.Achievement})
                    RETURN a;
                ";
            }

            using var session = _client.AsyncSession();

            var cursor = await session.RunAsync(query, parameters);
            List<Achievement> results = await cursor.ToListAsync<Achievement>(r =>
            {
                var node = r["a"] as INode;
                return node.ToObject<Achievement>();
            });

            return results;
        }
    }
}
