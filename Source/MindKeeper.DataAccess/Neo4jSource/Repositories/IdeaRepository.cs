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
        public Task<Idea> Create(int userId, string name, string descritpion, int typeId, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateLink(int parentId, int childId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteLink(int parentId, int childId)
        {
            throw new NotImplementedException();
        }

        public Task<Idea> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Idea>> GetAll(IdeaFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
