using System;

namespace MindKeeper.Domain.Entities
{
    public class UpdateApprove : BaseEntity
    {
        public int IdeaUpdateId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
