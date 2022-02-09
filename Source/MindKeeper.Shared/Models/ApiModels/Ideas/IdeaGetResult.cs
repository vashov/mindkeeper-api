﻿using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeaGetResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public List<Guid> Parents { get; } = new List<Guid>();
        public List<Guid> Children { get; } = new List<Guid>();
        public List<Guid> DependsOn { get; } = new List<Guid>();
        public List<Guid> RequiredFor { get; } = new List<Guid>();
        public List<Guid> RelatesTo { get; set; } = new List<Guid>();
        public List<Guid> Countries { get; set; } = new List<Guid>();
        public List<Guid> Subdomains { get; set; } = new List<Guid>();
    }
}
