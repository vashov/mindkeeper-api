using MindKeeper.DataAccess.Neo4jSource.Extensions;
using MindKeeper.DataAccess.Neo4jSource.Tokens;
using MindKeeper.DataAccess.SeedData.Models;
using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Seed
{
    public class Neo4jDbPopulation
    {
        private readonly IDriver _driver;

        private readonly JsonSerializerOptions _jsonSerializerOptions =
            new () { PropertyNameCaseInsensitive = true };

        public Neo4jDbPopulation(IDriver driver)
        {
            _driver = driver;
        }

        public async Task Init()
        {
            await InsertCountries();
            await InsertScientificDomains();
        }

        private async Task<bool> IsAnyNodeExistsWithLabel(string label)
        {
            string query = @$"
                MATCH (n: {{{label}}})
                RETURN n
                LIMIT 1
                ";

            using var session = _driver.AsyncSession();
            var cursor = await session.RunAsync(query);
            var result = await cursor.ToListAsync<Guid>(r =>
            {
                var node = r["n"].As<INode>();
                var id = node.GetValueStrict<Guid>("Id");
                return id;
            });

            return result.Any();
        }

        private async Task InsertCountries()
        {
            if (!await IsAnyNodeExistsWithLabel(Label.Country))
                return;

            using var fileReader = File.OpenRead(@".\SeedData\countries.json");

            var countries = await JsonSerializer
                .DeserializeAsync<List<CountryModel>>(fileReader, _jsonSerializerOptions);

            using var session = _driver.AsyncSession();

            for (int idx = 0; idx < countries.Count; idx++)
            {
                var country = countries[idx];
                var parameters = new
                {
                    Id = Guid.NewGuid(),
                    country.Name,
                    country.Code
                };

                string nodeQuery = $"CREATE (:{Label.Country} {{{parameters.AsProperties()}}});";

                var cursor = await session.RunAsync(nodeQuery, parameters);
            }
        }

        private async Task InsertScientificDomains()
        {
            if (!await IsAnyNodeExistsWithLabel(Label.Domain))
                return;

            using var fileReader = File.OpenRead(@".\SeedData\scientific_domains.json");

            var domains = await JsonSerializer
                .DeserializeAsync<List<ScientificDomainModel>>(fileReader, _jsonSerializerOptions);

            using var session = _driver.AsyncSession();

            foreach (var domain in domains)
            {
                var domainId = Guid.NewGuid();
                var domainName = domain.Domain;

                foreach (var subdomainName in domain.Subdomains)
                {
                    var subdomainId = Guid.NewGuid();

                    var parameters = new
                    {
                        DomainId = domainId,
                        DomainName = domainName,
                        SubdomainId = subdomainId,
                        SubdomainName = subdomainName
                    };

                    string createNodesComand = $@"
                        CREATE p = (domain:{Label.Domain} {{Id:$DomainId, Name:$DomainName}})-[:{Relationship.CONTAINS_SUBDOMAIN}]->(subdomain:{Label.Subdomain} {{Id:$SubdomainId, Name:$SubdomainName}})
                        RETURN p
                        ;";

                    var cursor = await session.RunAsync(createNodesComand, parameters);
                }
            }
        }
    }
}
