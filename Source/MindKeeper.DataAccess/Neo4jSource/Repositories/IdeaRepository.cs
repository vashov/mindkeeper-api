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

            var idea = await session.WriteTransactionAsync<Idea>(
                transaction => CreateIdea(transaction, model));

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

        public async Task<Idea> Get(Guid id)
        {
            using var session = _client.AsyncSession();

            string query = $@"
                    MATCH (u:{Label.User})-[r:{Relationship.CREATED_IDEA} {{CreatedAt: $CreatedAt}}]->(i:{Label.Idea} {{Id:$Id}})
                    RETURN u, r, i;
                ";

            var parameters = new
            {
                Id = id
            };

            var cursor = await session.RunAsync(query, parameters);

            var results = await cursor.ToListAsync<Idea>(BuildIdea);

            return results.FirstOrDefault();
        }

        public async Task<List<Idea>> GetAll(IdeaGetAllModel model)
        {
            using var session = _client.AsyncSession();

            string query = $@"
                    MATCH (u:{Label.User})-[r:{Relationship.CREATED_IDEA} {{CreatedAt: $CreatedAt}}]->(i:{Label.Idea})
                    RETURN u, r, i;
                ";

            var cursor = await session.RunAsync(query);

            var results = await cursor.ToListAsync<Idea>(BuildIdea);

            return results;
        }

        private async Task<Idea> CreateIdea(IAsyncTransaction transaction, IdeaCreateModel model)
        {
            var createdAt = DateTimeOffset.UtcNow;

            string createIdeaQuery = $@"
                    MATCH (u:{Label.User} {{Id: $UserId}})
                    CREATE (u)-[r:{Relationship.CREATED_IDEA} {{CreatedAt: $CreatedAt}}]->(i:{Label.Idea} {{Id: $Id, Name: $Name, Description: $Description}})
                    RETURN u, r, i;
                ";

            var createIdeaParameters = new
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                UserId = model.UserId,
                CreatedAt = createdAt
            };

            IResultCursor cursor = await transaction.RunAsync(createIdeaQuery, createIdeaParameters);

            Idea idea = (await cursor.ToListAsync<Idea>(BuildIdea)).First();

            // TODO: add Idea's relationships
            // RELATES_TO
            // DEPENDS_ON
            // COPY_OF

            if (model.ParentIdeaId.HasValue)
            {
                string createRelateWithIdea = $@"
                        MATCH (p:{Label.Idea} {{Id: $ParentIdeaId}}), (i:{Label.Idea} {{Id: $IdeaId}})
                        CREATE (p)-[r:{Relationship.PARENT_FOR} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(i)
                        ;
                    ";

                var parentParameters = new
                {
                    ParentIdeaId = model.ParentIdeaId.Value,
                    IdeaId = idea.Id,
                    ConnectedAt = createdAt,
                    ConnectedBy = model.UserId
                };

                cursor = await transaction.RunAsync(createRelateWithIdea, parentParameters);
            }

            if (model.CountryId.HasValue)
            {
                string createRelateWithCountry = $@"
                        MATCH (p:{Label.Country} {{Id: $CountryId}}), (i:{Label.Idea} {{Id: $IdeaId}})
                        CREATE (p)-[r:{Relationship.COUNTRY_OF} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(i)
                        ;
                    ";

                var parentParameters = new
                {
                    CountryId = model.CountryId.Value,
                    IdeaId = idea.Id,
                    ConnectedAt = createdAt,
                    ConnectedBy = model.UserId
                };

                cursor = await transaction.RunAsync(createRelateWithCountry, parentParameters);
            }

            if (model.SubdomainId.HasValue)
            {
                string createRelateWithSubdomain = $@"
                        MATCH (p:{Label.Subdomain} {{Id: $SubdomainId}}), (i:{Label.Idea} {{Id: $IdeaId}})
                        CREATE (p)-[r:{Relationship.CONTAINS_IDEA} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(i)
                        ;
                    ";

                var parentParameters = new
                {
                    SubdomainId = model.SubdomainId.Value,
                    IdeaId = idea.Id,
                    ConnectedAt = createdAt,
                    ConnectedBy = model.UserId
                };

                cursor = await transaction.RunAsync(createRelateWithSubdomain, parentParameters);
            }

            return idea;
        }

        private Idea BuildIdea(IRecord record)
        {
            var userNode = record["u"].As<INode>();
            var rel = record["r"].As<INode>();
            var ideaNode = record["i"].As<INode>();

            var user = userNode.ToObject<User>();
            var relationship = rel.ToObject<UserCreatedIdea>();
            var idea = ideaNode.ToObject<Idea>();

            idea.CreatedBy = user.Id;
            idea.CreatedAt = relationship.CreatedAt;

            return idea;
        }
    }
}
