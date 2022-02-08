using MindKeeper.Domain.Entities;
using MindKeeper.Domain.EntitiesComposed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        public StatisticsService(
            )
        {

        }
        public Task<List<Achievement>> GetAchievements(Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public Task<StatsSystem> GetSystemStats()
        {
            throw new NotImplementedException();
        }

        public Task<StatsUser> GetUserStats(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
