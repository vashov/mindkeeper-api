using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Domain.Interfaces
{
    public interface IIdeaRepository
    {
        public Task<Idea> Get(long id);
        public Task<List<Idea>> GetAll(NodeFilter filter);
        public Task<Idea> Create(int userId, string name, string descritpion, int typeId, int parentId);
        public Task<bool> CreateLink(int parentId, int childId);
        public Task<bool> DeleteLink(int parentId, int childId);
    }
}
