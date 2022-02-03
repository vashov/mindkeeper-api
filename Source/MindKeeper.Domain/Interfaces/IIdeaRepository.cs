using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Domain.Interfaces
{
    public interface IIdeaRepository
    {
        public Task<Idea> Get(Guid id);
        public Task<List<Idea>> GetAll(IdeaFilter filter);
        public Task<Idea> Create(Guid userId, string name, string descritpion, Guid parentId);
        public Task<bool> CreateLink(Guid parentId, Guid childId);
        public Task<bool> DeleteLink(Guid parentId, Guid childId);
    }
}
