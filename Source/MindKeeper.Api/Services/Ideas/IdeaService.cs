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
        private readonly IIdeaRepository _ideaRepository;
        private readonly IUserRepository _userRepository;

        public IdeaService(IIdeaRepository ideaRepository, IUserRepository userRepository)
        {
            _ideaRepository = ideaRepository;
            _userRepository = userRepository;
        }

        public async Task<Idea> Create(int userId, string name, string descritpion, int typeId, int parentId)
        {
            if (_userRepository.Get(userId) == null)
                throw new ApiException("Invalid user id.");

            ValidateNameAndDescription(name, descritpion);

            if (parentId != default && (await _ideaRepository.Get(parentId)) == null)
                throw new ApiException("Invalid parent idea id.");

            // TODO: validate typeId.

            var idea = await _ideaRepository.Create(userId, name, descritpion, typeId, parentId);

            return idea;
        }

        public async Task<Idea> Get(long id)
        {
            var idea = await _ideaRepository.Get(id);
            if (idea == null)
                throw new ApiException($"idea {id} not found.");

            return idea;
        }

        public async Task<List<Idea>> GetAll(IdeaFilter filter)
        {
            var ideas = await _ideaRepository.GetAll(filter);

            return ideas;
        }

        public async Task CreateLink(int parentId, int childId)
        {
            await ValidateIdeasAsFutureChildAndParent(parentId, childId);

            var created = await _ideaRepository.CreateLink(parentId, childId);
            if (!created)
                throw new ApiException(
                    $"Connection of idea (parent) {parentId} with idea (child) {childId} wasn't created.");
        }

        public async Task DeleteLink(int parentId, int childId)
        {
            await ValidateIdeasAsFutureChildAndParent(parentId, childId);

            var deleted = await _ideaRepository.DeleteLink(parentId, childId);

            if (!deleted)
                throw new ApiException(
                    $"Connection of idea (parent) {parentId} with idea (child) {childId} wasn't deleted.");
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

        private async Task ValidateIdeasExist(params int[] ideas)
        {
            var filter = new IdeaFilter
            {
                Ideas = ideas.ToList()
            };
            var ideasResult = await _ideaRepository.GetAll(filter);
            List<int> invalidIdeas = ideas.Except(ideasResult.Select(n => n.Id)).ToList();

            if (invalidIdeas.HasValues())
                throw new ApiException($"Invalid ideas with id {string.Join(",", invalidIdeas)}.");
        }

        private async Task ValidateIdeasAsFutureChildAndParent(int parentId, int childId)
        {
            if (parentId == childId)
                throw new AppValidationException("Ideas should have different ID.");

            await ValidateIdeasExist(parentId, childId);
        }
    }
}
