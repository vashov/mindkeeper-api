using System;

namespace MindKeeper.Domain.Interfaces.Ideas
{
    public interface IIdeaLinkModel
    {
        Guid IdeaId { get; set; }
        Guid UserId { get; set; }

        Guid? ParentIdea { get; set; }
        Guid? ChildIdea { get; set; }
        Guid? RelatesToIdea { get; set; }
        Guid? DependsOnIdea { get; set; }
        Guid? Country { get; set; }
        Guid? Subdomain { get; set; }
    }
}
