using System;
using System.Collections.Generic;

namespace MindKeeper.Domain.Interfaces.Ideas
{
    public struct IdeaGetAllModel
    {
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? DomainId { get; set; }
        public Guid? SubdomainId { get; set; }
        public Guid? ParentIdeaId { get; set; }
        public IReadOnlyList<Guid> Ideas { get; set; }
    }
}
