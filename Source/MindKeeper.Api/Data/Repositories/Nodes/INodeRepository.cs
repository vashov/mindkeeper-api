using MindKeeper.Api.Data.Entities;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Repositories.Nodes
{
    public interface INodeRepository
    {
        public Task<Node> Get(long id);
        public Task<Node> Get(string name);
        public Task<List<Node>> GetAll(NodeFilter filter);
        public Task<Node> Create(int userId, string name, string descritpion, int typeId, int parentId);
    }
}
