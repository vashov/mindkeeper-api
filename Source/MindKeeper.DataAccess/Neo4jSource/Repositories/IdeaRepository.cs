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

        public async Task<Idea> Get(Guid id)
        {
            using var session = _client.AsyncSession();

            string query = $@"
                    MATCH (u:{Label.User})-[r:{Relationship.CREATED_IDEA}]->(i:{Label.Idea} {{Id:$Id}})
                    RETURN u, r, i;
                ";

            var parameters = new
            {
                Id = id.ToString()
            };

            var cursor = await session.RunAsync(query, parameters);
            var results = await cursor.ToListAsync<Idea>(BuildIdea);

            var idea = results.FirstOrDefault();
            if (idea == null)
                return null;

            await FillNodes(session, idea);
            
            return idea;
        }

        public async Task<List<Idea>> GetAll(IdeaGetAllModel model)
        {
            using var session = _client.AsyncSession();

            string query = $@"
                    MATCH (u:{Label.User})-[r:{Relationship.CREATED_IDEA}]->(i:{Label.Idea})
                    RETURN u, r, i;
                ";

            var cursor = await session.RunAsync(query);

            var results = await cursor.ToListAsync<Idea>(BuildIdea);
            
            // TODO: Use filters

            if (model.HasFilter)
            {
                foreach (var idea in results)
                    await FillNodes(session, idea);
            }

            return results;
        }

        public async Task<bool> CreateLink(IdeaLinkAddModel model)
        {
            using var session = _client.AsyncSession();
            bool result = await session.WriteTransactionAsync<bool>(async t =>
            {
                var connectedAt = DateTimeOffset.UtcNow;
                var connectedBy = model.UserId.ToString();
                var ideaId = model.IdeaId.ToString();

                if (model.ParentIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$ParentId}}), (ic:{Label.Idea} {{Id:$IdeaId}})
                        CREATE (ip)-[:{Relationship.PARENT_FOR} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(ic);
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ConnectedBy = connectedBy,
                        ConnectedAt = connectedAt,
                        ParentId = model.ParentIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.ChildIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}}), (ic:{Label.Idea} {{Id:$ChildId}})
                        CREATE (ip)-[:{Relationship.PARENT_FOR} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(ic);
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ConnectedBy = connectedBy,
                        ConnectedAt = connectedAt,
                        ChildId = model.ChildIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.RelatesToIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}}), (ic:{Label.Idea} {{Id:$RelatedId}})
                        CREATE (ip)-[:{Relationship.RELATED_TO_IDEA} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(ic);
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ConnectedBy = connectedBy,
                        ConnectedAt = connectedAt,
                        RelatedId = model.RelatesToIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.DependsOnIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}}), (ic:{Label.Idea} {{Id:$DependsOnId}})
                        CREATE (ip)-[:{Relationship.DEPENDS_ON} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(ic);
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ConnectedBy = connectedBy,
                        ConnectedAt = connectedAt,
                        DependsOnId = model.DependsOnIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.Country.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}}), (ic:{Label.Country} {{Id:$CountryId}})
                        CREATE (ip)-[:{Relationship.RELATED_TO_COUNTRY} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(ic);
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ConnectedBy = connectedBy,
                        ConnectedAt = connectedAt,
                        CountryId = model.Country.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.Subdomain.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}}), (ic:{Label.Subdomain} {{Id:$SubdomainId}})
                        CREATE (ip)-[:{Relationship.RELATED_TO_SUBDOMAIN} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(ic);
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ConnectedBy = connectedBy,
                        ConnectedAt = connectedAt,
                        SubdomainId = model.Subdomain.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                return true;
            });

            return result;
        }

        public async Task<bool> DeleteLink(IdeaLinkDeleteModel model)
        {
            using var session = _client.AsyncSession();
            bool result = await session.WriteTransactionAsync<bool>(async t =>
            {
                var ideaId = model.IdeaId.ToString();

                if (model.ParentIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$ParentId}})-[r:{Relationship.PARENT_FOR}]->(ic:{Label.Idea} {{Id:$IdeaId}})
                        DELETE r;
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ParentId = model.ParentIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.ChildIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}})-[r:{Relationship.PARENT_FOR}]->(ic:{Label.Idea} {{Id:$ChildId}})
                        DELETE r;
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        ChildId = model.ChildIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.RelatesToIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}})-[r:{Relationship.RELATED_TO_IDEA}]->(ic:{Label.Idea} {{Id:$RelatedId}})
                        DELETE r;
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        RelatedId = model.RelatesToIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.DependsOnIdea.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}})-[r:{Relationship.DEPENDS_ON}]->(ic:{Label.Idea} {{Id:$DependsOnId}})
                        DELETE r;
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        DependsOnId = model.DependsOnIdea.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.Country.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}})-[r:{Relationship.RELATED_TO_COUNTRY}]->(ic:{Label.Country} {{Id:$CountryId}})
                        DELETE r;
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        CountryId = model.Country.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                if (model.Subdomain.HasValue)
                {
                    string query = $@"
                        MATCH (ip:{Label.Idea} {{Id:$IdeaId}})-[r:{Relationship.RELATED_TO_SUBDOMAIN}]->(ic:{Label.Subdomain} {{Id:$SubdomainId}})
                        DELETE r;
                    ";

                    var parameters = new
                    {
                        IdeaId = ideaId,
                        SubdomainId = model.Subdomain.Value.ToString()
                    };

                    await t.RunAsync(query, parameters);
                }

                return true;
            });

            return result;
        }

        public async Task AddToFavorites(Guid userId, Guid ideaId)
        {
            string query = $@"
                    MATCH (u:{Label.User} {{Id: $UserId}}), (i:{Label.Idea} {{Id: $IdeaId}})
                    CREATE (u)-[r:{Relationship.ADDED_TO_FAVORITES} {{CreatedAt: $CreatedAt}}]->(i)
                    ;
                ";

            var parameters = new
            {
                UserId = userId.ToString(),
                IdeaId = ideaId.ToString(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            using var session = _client.AsyncSession();
            await session.RunAsync(query, parameters);
        }

        public async Task DeleteFromFavorites(Guid userId, Guid ideaId)
        {
            string query = $@"
                    MATCH (u:{Label.User} {{Id: $UserId}})-[r:{Relationship.ADDED_TO_FAVORITES}]->(i:{Label.Idea} {{Id: $IdeaId}})
                    DELETE r;
                ";

            var parameters = new
            {
                UserId = userId.ToString(),
                IdeaId = ideaId.ToString()
            };

            using var session = _client.AsyncSession();
            await session.RunAsync(query, parameters);
        }

        public async Task<List<Idea>> GetRecommendedIdeas(Guid userId)
        {
            string query =
                $"MATCH (user:{Label.User} {{Id: $UserId}})-[:{Relationship.ADDED_TO_FAVORITES}]->(idea:{Label.Idea})" +
                $"<-[:{Relationship.ADDED_TO_FAVORITES}]-(anotherUser:{Label.User})" +
                $"-[:{Relationship.ADDED_TO_FAVORITES}]->(i:{Label.Idea})," +
                $"(u:{Label.User})-[r:{Relationship.CREATED_IDEA}]->(i)" +
                $" WHERE idea <> i AND user <> u" +
                $" RETURN u, r, i;";

            var parameters = new
            {
                UserId = userId.ToString()
            };
            using var session = _client.AsyncSession();
            var cursor = await session.RunAsync(query, parameters);

            var results = await cursor.ToListAsync<Idea>(BuildIdea);
            return results;
        }

        private async Task<Idea> CreateIdea(IAsyncTransaction transaction, IdeaCreateModel model)
        {
            var createdAt = DateTimeOffset.UtcNow;
            string userId = model.UserId.ToString();

            string createIdeaQuery = $@"
                    MATCH (u:{Label.User} {{Id: $UserId}})
                    CREATE (u)-[r:{Relationship.CREATED_IDEA} {{CreatedAt: $CreatedAt}}]->(i:{Label.Idea} {{Id: $Id, Name: $Name, Description: $Description}})
                    RETURN u, r, i;
                ";

            var createIdeaParameters = new
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Description = model.Description,
                UserId = userId,
                CreatedAt = createdAt
            };

            IResultCursor cursor = await transaction.RunAsync(createIdeaQuery, createIdeaParameters);

            Idea idea = (await cursor.ToListAsync<Idea>(BuildIdea)).First();
            string ideaId = idea.Id.ToString();

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
                    ParentIdeaId = model.ParentIdeaId.Value.ToString(),
                    IdeaId = ideaId,
                    ConnectedAt = createdAt,
                    ConnectedBy = userId
                };

                cursor = await transaction.RunAsync(createRelateWithIdea, parentParameters);
                idea.Parents.Add(model.ParentIdeaId.Value);
            }

            if (model.CountryId.HasValue)
            {
                string createRelateWithCountry = $@"
                        MATCH (p:{Label.Country} {{Id: $CountryId}}), (i:{Label.Idea} {{Id: $IdeaId}})
                        CREATE (i)-[r:{Relationship.RELATED_TO_COUNTRY} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(p)
                        ;
                    ";

                var parentParameters = new
                {
                    CountryId = model.CountryId.Value.ToString(),
                    IdeaId = ideaId,
                    ConnectedAt = createdAt,
                    ConnectedBy = userId
                };

                cursor = await transaction.RunAsync(createRelateWithCountry, parentParameters);
                idea.Countries.Add(model.CountryId.Value);
            }

            if (model.SubdomainId.HasValue)
            {
                string createRelateWithSubdomain = $@"
                        MATCH (p:{Label.Subdomain} {{Id: $SubdomainId}}), (i:{Label.Idea} {{Id: $IdeaId}})
                        CREATE (i)-[r:{Relationship.RELATED_TO_SUBDOMAIN} {{ConnectedAt: $ConnectedAt, ConnectedBy: $ConnectedBy}}]->(p)
                        ;
                    ";

                var parentParameters = new
                {
                    SubdomainId = model.SubdomainId.Value.ToString(),
                    IdeaId = ideaId,
                    ConnectedAt = createdAt,
                    ConnectedBy = userId
                };

                cursor = await transaction.RunAsync(createRelateWithSubdomain, parentParameters);
                idea.Subdomains.Add(model.SubdomainId.Value);
            }

            return idea;
        }

        private Idea BuildIdea(IRecord record)
        {
            var userNode = record["u"].As<INode>();
            var rel = record["r"].As<IRelationship>();
            var ideaNode = record["i"].As<INode>();

            var user = userNode.ToObject<User>();
            var relationship = rel.ToObject<UserCreatedIdea>();
            var idea = ideaNode.ToObject<Idea>();

            idea.CreatedBy = user.Id;
            idea.CreatedAt = relationship.CreatedAt;

            return idea;
        }

        private async Task FillNodes(IAsyncSession session, Idea idea)
        {
            static Guid GetGuid(IRecord r) => Guid.Parse((string)r["p.Id"]);

            var parameters = new { Id = idea.Id.ToString() };

            string queryParents = $@"
                    MATCH (p:{Label.Idea})-[r:{Relationship.PARENT_FOR}]->(i:{Label.Idea} {{Id:$Id}})
                    RETURN p.Id;
                ";

            var cursor = await session.RunAsync(queryParents, parameters);
            var parents = await cursor.ToListAsync<Guid>(GetGuid);

            string queryChildrens = $@"
                    MATCH (p:{Label.Idea})<-[r:{Relationship.PARENT_FOR}]-(i:{Label.Idea} {{Id:$Id}})
                    RETURN p.Id;
                ";

            parameters = new { Id = idea.Id.ToString() };

            cursor = await session.RunAsync(queryChildrens, parameters);
            var childrens = await cursor.ToListAsync<Guid>(GetGuid);

            string queryDependsOn = $@"
                    MATCH (p:{Label.Idea})<-[r:{Relationship.DEPENDS_ON}]-(i:{Label.Idea} {{Id:$Id}})
                    RETURN p.Id;
                ";

            cursor = await session.RunAsync(queryDependsOn, parameters);
            var dependsOn = await cursor.ToListAsync<Guid>(GetGuid);

            string queryReqiredFor = $@"
                    MATCH (p:{Label.Idea})-[r:{Relationship.DEPENDS_ON}]->(i:{Label.Idea} {{Id:$Id}})
                    RETURN p.Id;
                ";

            cursor = await session.RunAsync(queryReqiredFor, parameters);
            var requiredFor = await cursor.ToListAsync<Guid>(GetGuid);

            string queryRelatesTo = $@"
                    MATCH (p:{Label.Idea})-[r:{Relationship.RELATED_TO_IDEA}]-(i:{Label.Idea} {{Id:$Id}})
                    RETURN p.Id;
                ";

            cursor = await session.RunAsync(queryRelatesTo, parameters);
            var relatesTo = await cursor.ToListAsync<Guid>(GetGuid);

            string queryCountry = $@"
                    MATCH (p:{Label.Country})<-[r:{Relationship.RELATED_TO_COUNTRY}]-(i:{Label.Idea} {{Id:$Id}})
                    RETURN p.Id;
                ";

            cursor = await session.RunAsync(queryCountry, parameters);
            var countries = await cursor.ToListAsync<Guid>(GetGuid);

            string querySubdomains = $@"
                    MATCH (p:{Label.Subdomain})<-[r:{Relationship.RELATED_TO_SUBDOMAIN}]-(i:{Label.Idea} {{Id:$Id}})
                    RETURN p.Id;
                ";

            cursor = await session.RunAsync(queryCountry, parameters);
            var subdomains = await cursor.ToListAsync<Guid>(GetGuid);

            idea.Parents.AddRange(parents);
            idea.Children.AddRange(childrens);
            idea.RelatesTo.AddRange(relatesTo);
            idea.DependsOn.AddRange(dependsOn);
            idea.RequiredFor.AddRange(requiredFor);

            idea.Subdomains.AddRange(subdomains);

            idea.Countries.AddRange(countries);
        }
    }
}
