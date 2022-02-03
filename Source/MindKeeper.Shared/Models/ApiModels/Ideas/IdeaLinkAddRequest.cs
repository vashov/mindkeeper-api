using System;
using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeaLinkAddRequest
    {
        [Required]
        public Guid ParentId { get; set; }

        [Required]
        public Guid ChildId { get; set; }
    }
}
