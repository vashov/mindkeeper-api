using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Filters;
using MindKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        public Task<Idea> Create(Guid userId, string name, string descritpion, Guid parentId)
        {
            throw new NotImplementedException();
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

        public Task<List<Idea>> GetAll(IdeaFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
