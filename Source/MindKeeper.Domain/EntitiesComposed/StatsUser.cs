using System;

namespace MindKeeper.Domain.EntitiesComposed
{
    public class StatsUser
    {
        public Guid UserId { get; set; }
        public long IdeasCreatedCount { get; set; }
        public long AchievementsCount { get; set; }
    }
}
