using MindKeeper.Domain.Entities;
using MindKeeper.Domain.EntitiesComposed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<List<Achievement>> GetAchievements(Guid? userId = null);
        Task<StatsSystem> GetSystemStats();
        Task<StatsUser> GetUserStats(Guid userId);
    }
}
