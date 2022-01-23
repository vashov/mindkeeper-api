using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Nodes
{
    public class CreateNodeRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Descritpion { get; set; }
        public int ParentId { get; set; }
    }
}
