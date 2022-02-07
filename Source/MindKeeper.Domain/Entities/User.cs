using System;

namespace MindKeeper.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Mail { get; set; }
        public string NormalizedMail { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
