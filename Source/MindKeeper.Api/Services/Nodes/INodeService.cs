using MindKeeper.Api.Data.Entities;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using MindKeeper.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Nodes
{
    public interface INodeService
    {
        public Task<OperationResult<Node>> Get(long id);
        public Task<OperationResult<List<Node>>> GetAll(NodeFilter filter);
        public Task<OperationResult<Node>> Create(int userId, string name, string descritpion, int typeId, int parentId);
        public Task<OperationResult> SetParent(int nodeId, int parentNodeId);
        public Task<OperationResult> DeleteParent(int nodeId, int parentNodeId);
        public Task<OperationResult> SetChild(int nodeId, int childNodeId);
        public Task<OperationResult> DeleteChild(int nodeId, int childNodeId);
    }
}
