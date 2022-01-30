using System.Collections.Generic;

namespace MindKeeper.Domain.Filters
{
    public struct IdeaFilter
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        //public int CountryIdeaId { get; set; }
        //public int DomainIdeaId { get; set; }
        //public int SubdomainId { get; set; }
        public int ParentId { get; set; }
        public List<int> Ideas { get; set; }
    }
}
