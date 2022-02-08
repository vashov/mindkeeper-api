using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Statistics
{
    public class AchievementsResult
    {
        public class Achivement
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public List<Achivement> Achivements { get; set; }
    }
}
