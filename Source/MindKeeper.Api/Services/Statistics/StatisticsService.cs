using MindKeeper.Domain.Entities;
using MindKeeper.Domain.EntitiesComposed;
using MindKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IAchievementsRepository _achievementsRepository;

        public StatisticsService(
            IStatisticsRepository statisticsRepository,
            IAchievementsRepository achievementsRepository)
        {
            _statisticsRepository = statisticsRepository;
            _achievementsRepository = achievementsRepository;
        }
        public async Task<List<Achievement>> GetAchievements(Guid? userId = null)
        {
            List<Achievement> results = await _achievementsRepository.GetAchievements(userId);
            return results;
        }

        public async Task<StatsSystem> GetSystemStats()
        {
            StatsSystem stats = await _statisticsRepository.GetSystemStats();
            return stats;
        }

        public async Task<StatsUser> GetUserStats(Guid userId)
        {
            StatsUser stats = await _statisticsRepository.GetUserStats(userId);
            return stats;
        }
    }
}
