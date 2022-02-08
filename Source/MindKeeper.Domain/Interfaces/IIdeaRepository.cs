using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces.Ideas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Domain.Interfaces
{
    public interface IIdeaRepository
    {
        public Task<Idea> Get(Guid id);
        public Task<List<Idea>> GetAll(IdeaGetAllModel filter);
        public Task<Idea> Create(IdeaCreateModel model);
        public Task<bool> CreateLink(IdeaLinkAddModel model);
        public Task<bool> DeleteLink(IdeaLinkDeleteModel model);
        Task AddToFavorites(Guid userId, Guid ideaId);
        Task DeleteFromFavorites(Guid userId, Guid ideaId);
        Task<List<Idea>> GetRecommendedIdeas(Guid userId);
    }
}
