using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Statistics
{
    public class AchievementsResult
    {
        public class Achivement
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsSecret { get; set; }
        }

        public List<Achivement> Achievements { get; set; }
    }
}
