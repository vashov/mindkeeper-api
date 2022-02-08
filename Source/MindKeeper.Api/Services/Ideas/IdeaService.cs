using MindKeeper.Api.Core.Exceptions;
using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces.Ideas;
using MindKeeper.Domain.Interfaces;
using MindKeeper.Shared.Extensions;
using System;
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

        public async Task<Idea> Create(IdeaCreateModel model)
        {
            if (_userRepository.Get(model.UserId) == null)
                throw new ApiException("Invalid user id.");

            ValidateNameAndDescription(model.Name, model.Description);

            if (model.ParentIdeaId.HasValue && (await _ideaRepository.Get(model.ParentIdeaId.Value)) == null)
                throw new ApiException("Invalid parent idea id.");

            // TODO: validate countryId, subdomainId, domainId.

            var idea = await _ideaRepository.Create(model);

            return idea;
        }

        public async Task<Idea> Get(Guid id)
        {
            var idea = await _ideaRepository.Get(id);
            if (idea == null)
                throw new ApiException($"idea {id} not found.");

            return idea;
        }

        public async Task<List<Idea>> GetAll(IdeaGetAllModel filter)
        {
            var ideas = await _ideaRepository.GetAll(filter);

            return ideas;
        }

        public async Task CreateLink(Guid parentId, Guid childId)
        {
            await ValidateIdeasAsFutureChildAndParent(parentId, childId);

            var created = await _ideaRepository.CreateLink(parentId, childId);
            if (!created)
                throw new ApiException(
                    $"Connection of idea (parent) {parentId} with idea (child) {childId} wasn't created.");
        }

        public async Task DeleteLink(Guid parentId, Guid childId)
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

        private async Task ValidateIdeasExist(params Guid[] ideas)
        {
            var filter = new IdeaGetAllModel
            {
                Ideas = ideas.ToList()
            };
            var ideasResult = await _ideaRepository.GetAll(filter);
            List<Guid> invalidIdeas = ideas.Except(ideasResult.Select(n => n.Id)).ToList();

            if (invalidIdeas.HasValues())
                throw new ApiException($"Invalid ideas with id {string.Join(",", invalidIdeas)}.");
        }

        private async Task ValidateIdeasAsFutureChildAndParent(Guid parentId, Guid childId)
        {
            if (parentId == childId)
                throw new AppValidationException("Ideas should have different ID.");

            await ValidateIdeasExist(parentId, childId);
        }
    }
}
