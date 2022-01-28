using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Nodes
{
    public class NodesGetAllRequest
    {
        /// <summary>
        /// Filter by user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Filter by nodes whose name starts with this parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Filter by parent node.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Filter by selected node's id.
        /// </summary>
        public List<int> Nodes { get; set; }
    }
}
