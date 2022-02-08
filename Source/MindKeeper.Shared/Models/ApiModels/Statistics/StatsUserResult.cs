using System;

namespace MindKeeper.Shared.Models.ApiModels.Statistics
{
    public class StatsUserResult
    {
        public Guid UserId { get; set; }
        public long IdeasCreatedCount { get; set; }
        public long AchievementsCount { get; set; }
    }
}
