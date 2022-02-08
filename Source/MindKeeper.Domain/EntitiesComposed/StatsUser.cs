using System;

namespace MindKeeper.Domain.EntitiesComposed
{
    public class StatsUser
    {
        public Guid UserId { get; set; }
        public int IdeasCreatedCount { get; set; }
        public int AchievementsCount { get; set; }
    }
}
