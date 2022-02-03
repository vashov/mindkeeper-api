using System;
using System.Collections.Generic;

namespace MindKeeper.Domain.Filters
{
    public struct IdeaFilter
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        //public int CountryIdeaId { get; set; }
        //public int DomainIdeaId { get; set; }
        //public int SubdomainId { get; set; }
        public Guid ParentId { get; set; }
        public List<Guid> Ideas { get; set; }
    }
}
