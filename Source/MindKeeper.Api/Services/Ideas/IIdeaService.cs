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
        public Task CreateLink(Guid parentId, Guid childId);
        public Task DeleteLink(Guid parentId, Guid childId);
    }
}
