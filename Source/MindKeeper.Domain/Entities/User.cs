﻿using System;

namespace MindKeeper.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string NormalizedMail { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
