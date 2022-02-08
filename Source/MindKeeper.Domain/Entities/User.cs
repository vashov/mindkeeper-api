using System;

namespace MindKeeper.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
