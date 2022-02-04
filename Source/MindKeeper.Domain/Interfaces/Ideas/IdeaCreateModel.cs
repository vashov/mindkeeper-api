using System;

namespace MindKeeper.Domain.Interfaces.Ideas
{
    public struct IdeaCreateModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentIdeaId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? DomainId { get; set; }
        public Guid? SubdomainId { get; set; }
    }
}
