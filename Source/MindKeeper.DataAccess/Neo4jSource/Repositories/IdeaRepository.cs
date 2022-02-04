using MindKeeper.DataAccess.Neo4jSource.Extensions;
using MindKeeper.DataAccess.Neo4jSource.Relationships;
using MindKeeper.DataAccess.Neo4jSource.Tokens;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using MindKeeper.Domain.Interfaces.Ideas;
using Neo4j.Driver;
using Neo4j.Driver.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly IDriver _client;

        public IdeaRepository(IDriver driver)
        {
            _client = driver;
        }

        public async Task<Idea> Create(IdeaCreateModel model)
        {
            using var session = _client.AsyncSession();

            var idea = await session.WriteTransactionAsync<Idea>(async t =>
            {
                var createdAt = DateTimeOffset.UtcNow;

                var ideaParameters = new
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                };

                string createIdeaQuery = $@"
                    MATCH (u:{Label.User} {{Id: $UserId}})
                    CREATE (u)-[r:{Relationship.CREATED_IDEA} {{CreatedAt: $CreatedAt}}]->(i:{Label.Idea} {{{ideaParameters.AsProperties()}}})
                    RETURN u, r, i
                ";

                var parameters = new
                {
                    Id = ideaParameters.Id,
                    Name = ideaParameters.Name,
                    Description = ideaParameters.Description,
                    UserId = model.UserId,
                    CreatedAt = createdAt
                };

                var cursor = await t.RunAsync(createIdeaQuery, parameters);

                var results = await cursor.ToListAsync<Idea>(r =>
                {
                    var userNode = r["u"].As<INode>();
                    var rel = r["r"].As<INode>();
                    var ideaNode = r["i"].As<INode>();

                    var user = userNode.ToObject<User>();
                    var relationship = rel.ToObject<UserCreatedIdea>();
                    var idea = ideaNode.ToObject<Idea>();

                    idea.CreatedBy = user.Id;
                    idea.CreatedAt = relationship.CreatedAt;

                    return idea;
                });

                return results.First();
            });

            return idea;
        }

        public Task<bool> CreateLink(Guid parentId, Guid childId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLink(Guid parentId, Guid childId)
        {
            throw new NotImplementedException();
        }

        public Task<Idea> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Idea>> GetAll(IdeaGetAllModel model)
        {
            throw new NotImplementedException();
        }
    }
}
