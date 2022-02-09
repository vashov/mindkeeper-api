using System.Collections.Generic;

namespace MindKeeper.Domain.Entities
{
    public class DomainEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<Subdomain> Subdomains { get; set; }
    }
}
