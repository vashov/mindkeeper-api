using MindKeeper.DataAccess.Neo4jSource.Tokens;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        private readonly IDriver _client;

        public DomainRepository(IDriver client)
        {
            _client = client;
        }

        public async Task<List<DomainEntity>> GetAll()
        {
            using var session = _client.AsyncSession();

            string query = $@"
                    MATCH (d:{Label.Domain})-[r:{Relationship.CONTAINS_SUBDOMAIN}]->(s)
                    RETURN d,r,s;
                ";

            var cursor = await session.RunAsync(query);

            var domains = new Dictionary<Guid, DomainEntity>();

            var results = await cursor.ToListAsync<DomainEntity>(r =>
            {
                var domainNode = r["d"] as INode;
                var subdomainNode = r["s"] as INode;
                var domain = domainNode.ToObject<DomainEntity>();
                var subdomain = subdomainNode.ToObject<Subdomain>();

                if (domains.TryGetValue(domain.Id, out var existedDomain))
                    domain = existedDomain;

                domain.Subdomains ??= new List<Subdomain>();
                domain.Subdomains.Add(subdomain);
                domains[domain.Id] = domain;

                return domain;
            });

            return domains.Values.ToList(); ;
        }
    }
}
