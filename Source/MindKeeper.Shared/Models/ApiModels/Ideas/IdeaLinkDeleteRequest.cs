using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeaLinkDeleteRequest
    {
        [Required]
        public int ParentId { get; set; }

        [Required]
        public int ChildId { get; set; }
    }
}
