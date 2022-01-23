using MindKeeper.Api.Data.Entities;
using MindKeeper.Api.Data.Repositories.Nodes;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using MindKeeper.Api.Data.Repositories.Users;
using MindKeeper.Shared.Extensions;
using MindKeeper.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Nodes
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IUserRepository _userRepository;

        public NodeService(INodeRepository nodeRepository, IUserRepository userRepository)
        {
            _nodeRepository = nodeRepository;
            _userRepository = userRepository;
        }

        public async Task<OperationResult<Node>> Create(int userId, string name, string descritpion, int typeId, int parentId)
        {
            if (_userRepository.Get(userId) == null)
                return OperationResult<Node>.Error("Invalid user id.");

            if (!ValidateNameAndDescription(name, descritpion, out string errorMsg))
                return OperationResult<Node>.Error(errorMsg);

            if (parentId != default && (await _nodeRepository.Get(parentId)) == null)
                return OperationResult<Node>.Error("Invalid parent node id.");

            // TODO: validate typeId.

            // TODO: try-catch.
            var node = await _nodeRepository.Create(userId, name, descritpion, typeId, parentId);

            return OperationResult<Node>.Ok(node);
        }

        public async Task<OperationResult<Node>> Get(long id)
        {
            // TODO: try-catch.

            var node = await _nodeRepository.Get(id);

            return node == null
                ? OperationResult<Node>.Error($"Node {id} not found.")
                : OperationResult<Node>.Ok(node);
        }

        public async Task<OperationResult<List<Node>>> GetAll(NodeFilter filter)
        {
            // TODO: try-catch.

            var nodes = await _nodeRepository.GetAll(filter);

            return OperationResult<List<Node>>.Ok(nodes);
        }

        public async Task<OperationResult> CreateLink(int parentId, int childId)
        {
            // TODO: try-catch.

            OperationResult validateResult = await ValidateNodesAsFutureChildAndParent(parentId, childId);
            if (!validateResult.IsOk)
                return validateResult;

            var created = await _nodeRepository.CreateLink(parentId, childId);

            return created
                ? OperationResult.Ok()
                : OperationResult.Error(
                    $"Connection of node (parent) {parentId} with node (child) {childId} wasn't created.");
        }

        public async Task<OperationResult> DeleteLink(int parentId, int childId)
        {
            // TODO: try-catch.

            OperationResult validateResult = await ValidateNodesAsFutureChildAndParent(parentId, childId);
            if (!validateResult.IsOk)
                return validateResult;

            var deleted = await _nodeRepository.DeleteLink(parentId, childId);

            return deleted
                ? OperationResult.Ok()
                : OperationResult.Error(
                    $"Connection of node (parent) {parentId} with node (child) {childId} wasn't deleted.");
        }

        private static bool ValidateNameAndDescription(string name, string description, out string errorMsg)
        {
            const int nameMaxLen = 100;
            if (string.IsNullOrWhiteSpace(name) || name.Length > nameMaxLen)
            {
                errorMsg = $"Name length should be > 0 and <= {nameMaxLen}.";
                return false;
            }

            const int descriptionMaxLen = 1000;
            if (string.IsNullOrWhiteSpace(description) || description.Length > descriptionMaxLen)
            {
                errorMsg = $"Description length should be > 0 and <= {descriptionMaxLen}.";
                return false;
            }

            errorMsg = string.Empty;
            return true;
        }

        private async Task<OperationResult> ValidateNodesExist(params int[] nodes)
        {
            var filter = new NodeFilter
            {
                Nodes = nodes.ToList()
            };
            var nodesResult = await _nodeRepository.GetAll(filter);
            List<int> invalidNodes = nodes.Except(nodesResult.Select(n => n.Id)).ToList();

            if (invalidNodes.HasValues())
                return OperationResult.Error($"Invalid nodes with id {string.Join(",", invalidNodes)}.");

            return OperationResult.Ok();
        }

        private async Task<OperationResult> ValidateNodesAsFutureChildAndParent(int parentId, int childId)
        {
            if (parentId == childId)
                return OperationResult.Error("Nodes should have different ID.");

            return await ValidateNodesExist(parentId, childId);
        }
    }
}
