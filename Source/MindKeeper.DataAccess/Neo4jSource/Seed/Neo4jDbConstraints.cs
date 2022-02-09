using MindKeeper.DataAccess.Neo4jSource.Tokens;
using Neo4j.Driver;
using System;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Seed
{
    public class Neo4jDbConstraints
    {
        private readonly IDriver _driver;

        public Neo4jDbConstraints(IDriver driver)
        {
            _driver = driver;
        }

        public async Task Init()
        {
            using var session = _driver.AsyncSession();

            Func<string, string> CreateUniqueIdConstraint = 
                label => $"CREATE CONSTRAINT {label}_id_unique IF NOT EXISTS FOR (n:{label}) REQUIRE n.Id IS UNIQUE";

            IResultCursor cursor;
            cursor = await session.RunAsync(CreateUniqueIdConstraint(Label.User));
            cursor = await session.RunAsync(CreateUniqueIdConstraint(Label.Achievement));
            cursor = await session.RunAsync(CreateUniqueIdConstraint(Label.Idea));
            cursor = await session.RunAsync(CreateUniqueIdConstraint(Label.IdeaUpdate));
            cursor = await session.RunAsync(CreateUniqueIdConstraint(Label.Country));
            cursor = await session.RunAsync(CreateUniqueIdConstraint(Label.Domain));
            cursor = await session.RunAsync(CreateUniqueIdConstraint(Label.Subdomain));
        }
    }
}
