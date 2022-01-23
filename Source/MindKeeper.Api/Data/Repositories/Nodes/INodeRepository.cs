using MindKeeper.Api.Data.Entities;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Repositories.Nodes
{
    public interface INodeRepository
    {
        public Task<Node> Get(long id);
        public Task<List<Node>> GetAll(NodeFilter filter);
        public Task<Node> Create(int userId, string name, string descritpion, int typeId, int parentId);
        public Task<bool> SetParent(int nodeId, int parentNodeId);
        public Task<bool> DeleteParent(int nodeId, int parentNodeId);
        public Task<bool> SetChild(int nodeId, int childNodeId);
        public Task<bool> DeleteChild(int nodeId, int childNodeId);
    }
}
