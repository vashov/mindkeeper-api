using MindKeeper.Api.Core.Exceptions;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Filters;
using MindKeeper.Domain.Interfaces;
using MindKeeper.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Ideas
{
    public class IdeaService : IIdeaService
    {
        private readonly IIdeaRepository _nodeRepository;
        private readonly IUserRepository _userRepository;

        public IdeaService(IIdeaRepository nodeRepository, IUserRepository userRepository)
        {
            _nodeRepository = nodeRepository;
            _userRepository = userRepository;
        }

        public async Task<Idea> Create(int userId, string name, string descritpion, int typeId, int parentId)
        {
            if (_userRepository.Get(userId) == null)
                throw new ApiException("Invalid user id.");

            ValidateNameAndDescription(name, descritpion);

            if (parentId != default && (await _nodeRepository.Get(parentId)) == null)
                throw new ApiException("Invalid parent node id.");

            // TODO: validate typeId.

            var node = await _nodeRepository.Create(userId, name, descritpion, typeId, parentId);

            return node;
        }

        public async Task<Idea> Get(long id)
        {
            var node = await _nodeRepository.Get(id);
            if (node == null)
                throw new ApiException($"Node {id} not found.");

            return node;
        }

        public async Task<List<Idea>> GetAll(NodeFilter filter)
        {
            var nodes = await _nodeRepository.GetAll(filter);

            return nodes;
        }

        public async Task CreateLink(int parentId, int childId)
        {
            await ValidateNodesAsFutureChildAndParent(parentId, childId);

            var created = await _nodeRepository.CreateLink(parentId, childId);
            if (!created)
                throw new ApiException(
                    $"Connection of node (parent) {parentId} with node (child) {childId} wasn't created.");
        }

        public async Task DeleteLink(int parentId, int childId)
        {
            await ValidateNodesAsFutureChildAndParent(parentId, childId);

            var deleted = await _nodeRepository.DeleteLink(parentId, childId);

            if (!deleted)
                throw new ApiException(
                    $"Connection of node (parent) {parentId} with node (child) {childId} wasn't deleted.");
        }

        private static void ValidateNameAndDescription(string name, string description)
        {
            const int nameMaxLen = 100;
            if (string.IsNullOrWhiteSpace(name) || name.Length > nameMaxLen)
                throw new AppValidationException($"Name length should be > 0 and <= {nameMaxLen}.");

            const int descriptionMaxLen = 1000;
            if (string.IsNullOrWhiteSpace(description) || description.Length > descriptionMaxLen)
                throw new AppValidationException($"Description length should be > 0 and <= {descriptionMaxLen}.");
        }

        private async Task ValidateNodesExist(params int[] nodes)
        {
            var filter = new NodeFilter
            {
                Nodes = nodes.ToList()
            };
            var nodesResult = await _nodeRepository.GetAll(filter);
            List<int> invalidNodes = nodes.Except(nodesResult.Select(n => n.Id)).ToList();

            if (invalidNodes.HasValues())
                throw new ApiException($"Invalid nodes with id {string.Join(",", invalidNodes)}.");
        }

        private async Task ValidateNodesAsFutureChildAndParent(int parentId, int childId)
        {
            if (parentId == childId)
                throw new AppValidationException("Nodes should have different ID.");

            await ValidateNodesExist(parentId, childId);
        }
    }
}
