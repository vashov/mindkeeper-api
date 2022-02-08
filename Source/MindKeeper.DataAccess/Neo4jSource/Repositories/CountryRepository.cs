using MindKeeper.DataAccess.Neo4jSource.Tokens;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDriver _client;

        public CountryRepository(IDriver client)
        {
            _client = client;
        }

        public async Task<List<Country>> GetAll()
        {
            using var session = _client.AsyncSession();

            string query = $@"
                    MATCH (c:{Label.Country}))
                    RETURN c;
                ";

            var cursor = await session.RunAsync(query);

            var results = await cursor.ToListAsync<Country>(r =>
            {
                var node = r["c"] as INode;
                var country = node.ToObject<Country>();
                return country;
            });

            return results;
        }
    }
}
