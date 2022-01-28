using MindKeeper.Api.Data.Entities;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Nodes
{
    public interface INodeService
    {
        public Task<Node> Get(long id);
        public Task<List<Node>> GetAll(NodeFilter filter);
        public Task<Node> Create(int userId, string name, string descritpion, int typeId, int parentId);
        public Task CreateLink(int parentId, int childId);
        public Task DeleteLink(int parentId, int childId);
    }
}
