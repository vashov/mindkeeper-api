using System;

namespace MindKeeper.Domain.Entities
{
    public class IdeaUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdeaId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
