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

        public async Task CreateLink(IdeaLinkAddModel model)
        {
            await ValidateIdeaLinkModel(model);

            await _ideaRepository.CreateLink(model);
        }

        public async Task DeleteLink(IdeaLinkDeleteModel model)
        {
            await ValidateIdeaLinkModel(model);

            await _ideaRepository.DeleteLink(model);
        }

        public async Task AddToFavorites(Guid userId, Guid ideaId)
        {
            await ValidateIdeasExist(ideaId);

            await _ideaRepository.AddToFavorites(userId, ideaId);
        }

        public async Task DeleteFromFavorites(Guid userId, Guid ideaId)
        {
            await ValidateIdeasExist(ideaId);

            await _ideaRepository.DeleteFromFavorites(userId, ideaId);
        }

        private async Task ValidateIdeaLinkModel(IIdeaLinkModel model)
        {
            if (model.IdeaId == default)
                throw new AppValidationException("Invalid idea Id.");
            if (model.UserId == default)
                throw new AppValidationException("Invalid user Id.");

            if (model.ParentIdea == default
                && model.ChildIdea == default
                && model.RelatesToIdea == default
                && model.DependsOnIdea == default
                && model.Country == default
                && model.Subdomain == default)
                throw new AppValidationException("Not enough data for operation.");

            if (model.IdeaId == model.ParentIdea
                || model.IdeaId == model.ChildIdea
                || model.IdeaId == model.RelatesToIdea
                || model.IdeaId == model.DependsOnIdea)
                throw new AppValidationException("Self-referencing is not allowed for this type of relationship.");

            Guid[] ideas = new Guid?[]
            {
                model.IdeaId,
                model.ParentIdea,
                model.ChildIdea,
                model.RelatesToIdea,
                model.DependsOnIdea
            }
            .Where(i => i.HasValue)
            .Select(i => i.Value)
            .ToArray();

            await ValidateIdeasExist(ideas);
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
