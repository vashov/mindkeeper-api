using System;

namespace MindKeeper.Domain.Entities
{
    public class UpdateApprove
    {
        public int Id { get; set; }
        public int IdeaUpdateId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
