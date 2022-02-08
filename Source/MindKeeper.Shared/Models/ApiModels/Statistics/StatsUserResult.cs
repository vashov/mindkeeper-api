using System;

namespace MindKeeper.Shared.Models.ApiModels.Statistics
{
    public class StatsUserResult
    {
        public Guid UserId { get; set; }
        public int IdeasCreatedCount { get; set; }
        public int AchievementsCount { get; set; }
    }
}
