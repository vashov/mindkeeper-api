using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Nodes
{
    public class NodeCreateRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Descritpion { get; set; }
        public int ParentId { get; set; }
    }
}
