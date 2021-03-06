using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeasRecommendationsResult
    {
        public class IdeaRecommendation
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Guid CreatedBy { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public Guid? UpdatedBy { get; set; }
            public DateTimeOffset? UpdatedAt { get; set; }
            //public List<Guid> Parents { get; } = new List<Guid>();
            //public List<Guid> Children { get; } = new List<Guid>();
        }

        public List<IdeaRecommendation> Ideas { get; set; }
    }
}
