using System;
using System.Collections.Generic;

namespace MindKeeper.Shared.Models.ApiModels.Domains
{
    public class DomainGetAllResult
    {
        public class Subdomain
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class Domain
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public List<Subdomain> Subdomains { get; set; }
        }

        public List<Domain> Domains{ get; set; }
    }
}
