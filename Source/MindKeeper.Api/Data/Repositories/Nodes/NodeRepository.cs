using MindKeeper.Api.Data.Entities;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Repositories.Nodes
{
    public class NodeRepository : INodeRepository
    {
        public Task<Node> Create(int userId, string name, string descritpion, int typeId, int parentId)
        {
            throw new NotImplementedException();
        }

        public Task<Node> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Node> Get(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Node>> GetAll(NodeFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
