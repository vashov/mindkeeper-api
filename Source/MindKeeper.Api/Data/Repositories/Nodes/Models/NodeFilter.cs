using System.Collections.Generic;

namespace MindKeeper.Api.Data.Repositories.Nodes.Models
{
    public struct NodeFilter
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int CountryNodeId { get; set; }
        public int DomainNodeId { get; set; }
        public int SubdomainId { get; set; }
        public int ParentId { get; set; }
        public List<int> ByNodes { get; set; }
    }
}
