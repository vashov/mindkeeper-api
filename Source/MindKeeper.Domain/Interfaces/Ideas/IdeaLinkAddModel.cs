using System;

namespace MindKeeper.Domain.Interfaces.Ideas
{
    public struct IdeaLinkAddModel
    {
        public Guid IdeaId { get; set; }
        public Guid UserId { get; set; }

        public Guid? ParentIdea { get; set; }
        public Guid? ChildIdea { get; set; }
        public Guid? RelatesToIdea { get; set; }
        public Guid? DependsOnIdea { get; set; }
        public Guid? Country { get; set; }
        public Guid? Subdomain { get; set; }
    }
}
