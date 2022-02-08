using MindKeeper.DataAccess.Neo4jSource.Extensions;
using MindKeeper.DataAccess.Neo4jSource.Tokens;
using MindKeeper.DataAccess.SeedData.Models;
using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            await ActAndLog(InsertCountries, nameof(InsertCountries));
            await ActAndLog(InsertScientificDomains, nameof(InsertScientificDomains));
            await ActAndLog(InsertAchievements, nameof(InsertAchievements));
        }

        private static async Task ActAndLog(Func<Task> function, string actionName)
        {
            var w = Stopwatch.StartNew();

            await function();
            
            Console.WriteLine($"{actionName}: {w.ElapsedMilliseconds}");
            w.Stop();
        }

        private async Task<bool> IsAnyNodeExistsWithLabel(string label)
        {
            string query = @$"
                MATCH (n:{label})
                RETURN n
                LIMIT 1
                ";

            using var session = _driver.AsyncSession();
            var cursor = await session.RunAsync(query);
            var result = await cursor.ToListAsync<Guid>(r =>
            {
                var node = r["n"].As<INode>();
                var id = Guid.Parse(node.GetValueStrict<string>("Id"));
                return id;
            });

            return result.Any();
        }

        private async Task InsertCountries()
        {
            if (await IsAnyNodeExistsWithLabel(Label.Country))
                return;

            using var fileReader = File.OpenRead(GetPathToFile(@".\SeedData\countries.json"));

            var countries = await JsonSerializer
                .DeserializeAsync<List<CountryModel>>(fileReader, _jsonSerializerOptions);

            using var session = _driver.AsyncSession();

            for (int idx = 0; idx < countries.Count; idx++)
            {
                var country = countries[idx];
                var parameters = new
                {
                    Id = Guid.NewGuid().ToString(),
                    country.Name,
                    country.Code
                };

                string nodeQuery = $"CREATE (:{Label.Country} {{{parameters.AsProperties()}}});";

                var cursor = await session.RunAsync(nodeQuery, parameters);
            }
        }

        private async Task InsertScientificDomains()
        {
            if (await IsAnyNodeExistsWithLabel(Label.Domain))
                return;

            using var fileReader = File.OpenRead(GetPathToFile(@".\SeedData\scientific_domains.json"));

            var domains = await JsonSerializer
                .DeserializeAsync<List<ScientificDomainModel>>(fileReader, _jsonSerializerOptions);

            using var session = _driver.AsyncSession();

            foreach (var domain in domains)
            {
                var domainId = Guid.NewGuid().ToString();
                var domainName = domain.Domain;

                string createDomainCommand = $@"
                     CREATE (d:{Label.Domain} {{Id:$DomainId, Name:$DomainName}})
                     RETURN d;
                ";

                await session.RunAsync(createDomainCommand,
                    new { DomainId = domainId, DomainName = domainName});

                foreach (var subdomainName in domain.Subdomains)
                {
                    var subdomainId = Guid.NewGuid().ToString();

                    var parameters = new
                    {
                        DomainId = domainId,
                        SubdomainId = subdomainId,
                        SubdomainName = subdomainName
                    };

                    string createNodesComand = $@"
                        MATCH (d:{Label.Domain} {{Id:$DomainId}})
                        CREATE p = (d)-[:{Relationship.CONTAINS_SUBDOMAIN}]->(s:{Label.Subdomain} {{Id:$SubdomainId, Name:$SubdomainName}})
                        RETURN p
                        ;";

                    var cursor = await session.RunAsync(createNodesComand, parameters);
                }
            }
        }

        private async Task InsertAchievements()
        {
            List<AchievementModel> achievements;

            using var fileReader = File.OpenRead(GetPathToFile(@".\SeedData\achievements.json"));
            {
                achievements = await JsonSerializer
                    .DeserializeAsync<List<AchievementModel>>(fileReader, _jsonSerializerOptions);
            }

            using var session = _driver.AsyncSession();

            for (var i = 0; i < achievements.Count; i++)
            {
                var a = achievements[i];

                var parameters = new
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    IsSecret = a.IsSecret
                };

                string query = $"MERGE (:{Label.Achievement} {{{parameters.AsProperties()}}});";
                var cursor = await session.RunAsync(query, parameters);
            }

        }

        private string GetPathToFile(string path)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string combinedPath = System.IO.Path.Combine(currentDirectory, path);
            string filePath = Path.GetFullPath(combinedPath);
            return filePath;
        }
    }
}
