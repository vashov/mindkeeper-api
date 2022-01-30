using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeasGetAllResult
    {
        public class IdeaResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int TypeId { get; set; }
            public int CreatedBy { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public int UpdatedBy { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }
            public List<int> Parents { get; } = new List<int>();
            public List<int> Children { get; } = new List<int>();
        }

        public List<IdeaResponse> Ideas { get; set; }
    }
}
