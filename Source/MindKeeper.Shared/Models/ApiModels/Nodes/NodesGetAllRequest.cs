using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Nodes
{
    public class NodesGetAllRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public List<int> Nodes { get; set; }
    }
}
