using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Ideas
{
    public interface IIdeaService
    {
        public Task<Idea> Get(long id);
        public Task<List<Idea>> GetAll(NodeFilter filter);
        public Task<Idea> Create(int userId, string name, string descritpion, int typeId, int parentId);
        public Task CreateLink(int parentId, int childId);
        public Task DeleteLink(int parentId, int childId);
    }
}
