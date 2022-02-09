using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool HasFilter
        {
            get
            {
                return UserId.HasValue
                    || !string.IsNullOrWhiteSpace(Name)
                    || CountryId.HasValue
                    || DomainId.HasValue
                    || SubdomainId.HasValue
                    || ParentIdeaId.HasValue
                    || (Ideas != null && Ideas.Any());
            }
        }
    }
}
