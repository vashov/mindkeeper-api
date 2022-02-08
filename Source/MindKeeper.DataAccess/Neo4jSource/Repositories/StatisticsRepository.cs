using MindKeeper.DataAccess.Neo4jSource.Tokens;
using MindKeeper.Domain.EntitiesComposed;
using MindKeeper.Domain.Interfaces;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly IDriver _client;

        public StatisticsRepository(IDriver driver)
        {
            _client = driver;
        }

        public async Task<StatsSystem> GetSystemStats()
        {
            string query = $@"
                    MATCH (u:{Label.User})
                    RETURN count(u) as count
                    UNION ALL
                    MATCH (i:{Label.Idea})
                    RETURN count(i) as count
                ";

            using var session = _client.AsyncSession();

            var cursor = await session.RunAsync(query);
            List<long> results = await cursor.ToListAsync<long>(r =>
            {
                var count = (long)r["count"];
                return count;
            });

            var result = new StatsSystem
            {
                TotalUsersCount = results[0],
                TotalIdeasCount = results[1]
            };

            return result;
        }

        public async Task<StatsUser> GetUserStats(Guid userId)
        {
            string query = $@"
                    MATCH(u: {Label.User} {{ Id: $UserId}})-[r: {Relationship.CREATED_IDEA}]->(i)
                    RETURN count(r) as count
                    UNION ALL
                    MATCH(u: {Label.User} {{ Id: $UserId}})-[r: {Relationship.HAS_ACHIEVEMENT}]->(a)
                    RETURN count(r) as count
                ";

            using var session = _client.AsyncSession();

            var parameters = new 
            { 
                UserId = userId.ToString()
            };
            var cursor = await session.RunAsync(query, parameters);
            List<long> results = await cursor.ToListAsync<long>(r =>
            {
                var count = (long)r["count"];
                return count;
            });

            var result = new StatsUser
            {
                UserId = userId,
                IdeasCreatedCount = results[0],
                AchievementsCount = results[1]
            };

            return result;
        }
    }
}
