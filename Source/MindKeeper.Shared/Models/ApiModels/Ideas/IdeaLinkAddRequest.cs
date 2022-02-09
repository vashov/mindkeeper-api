using System;
using System.ComponentModel.DataAnnotations;

namespace MindKeeper.Shared.Models.ApiModels.Ideas
{
    public class IdeaLinkAddRequest
    {
        [Required]
        public Guid IdeaId { get; set; }

        public Guid? ParentIdea { get; set; }
        public Guid? ChildIdea { get; set; }
        public Guid? RelatesToIdea { get; set; }
        public Guid? DependsOnIdea { get; set; }
        public Guid? Country { get; set; }
        public Guid? Subdomain { get; set; }
    }
}
