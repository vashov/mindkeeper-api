using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Nodes
{
    public class NodeLinkAddRequest
    {
        [Required]
        public int ParentId { get; set; }

        [Required]
        public int ChildId { get; set; }
    }
}
