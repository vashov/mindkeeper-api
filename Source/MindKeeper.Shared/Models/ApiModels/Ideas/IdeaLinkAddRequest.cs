using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeaLinkAddRequest
    {
        [Required]
        public int ParentId { get; set; }

        [Required]
        public int ChildId { get; set; }
    }
}
