using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces.Ideas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Ideas
{
    public interface IIdeaService
    {
        public Task<Idea> Get(Guid id);
        public Task<List<Idea>> GetAll(IdeaGetAllModel filter);
        public Task<Idea> Create(IdeaCreateModel model);
        public Task CreateLink(IdeaLinkAddModel model);
        public Task DeleteLink(IdeaLinkDeleteModel model);
        Task AddToFavorites(Guid userId, Guid ideaId);
        Task DeleteFromFavorites(Guid userId, Guid ideaId);
        Task<List<Idea>> GetRecommendedIdeas(Guid userId);
    }
}
