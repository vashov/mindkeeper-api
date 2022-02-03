using System.Collections.Generic;

namespace MindKeeper.Domain.Filters
{
    public struct IdeaFilter
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        //public int CountryIdeaId { get; set; }
        //public int DomainIdeaId { get; set; }
        //public int SubdomainId { get; set; }
        public long ParentId { get; set; }
        public List<long> Ideas { get; set; }
    }
}
