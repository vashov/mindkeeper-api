using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Api.Services.Ideas;
using MindKeeper.Domain.Constants;
using MindKeeper.Domain.Interfaces.Ideas;
using MindKeeper.Shared.Models.ApiModels.Ideas;
using MindKeeper.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Controllers
{
    [ApiController]
    [ControllerRoute]
    [Authorize]
    public class IdeaController : ControllerBase
    {
        private readonly ILogger<IdeaController> _logger;
        private readonly IIdeaService _ideaService;
        private readonly IMapper _mapper;

        public IdeaController(
            ILogger<IdeaController> logger,
            IIdeaService ideaService,
            IMapper mapper)
        {
            _logger = logger;
            _ideaService = ideaService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<Response<IdeaGetResult>> Get([FromRoute] Guid id)
        {
            var result = await _ideaService.Get(id);

            var response = _mapper.Map<IdeaGetResult>(result);
            return new Response<IdeaGetResult>(response);
        }

        [HttpGet]
        public async Task<Response<IdeasGetAllResult>> GetAll([FromQuery] IdeasGetAllRequest request)
        {
            var filter = _mapper.Map<IdeaGetAllModel>(request);

            var result = await _ideaService.GetAll(filter);

            var ideaDtos = _mapper.Map<List<IdeasGetAllResult.IdeaResponse>>(result);
            var response = new IdeasGetAllResult() 
            { 
                Ideas = ideaDtos 
            };

            return new Response<IdeasGetAllResult>(response);
        }

        [HttpPost("[action]")]
        public async Task<Response<IdeaCreateResult>> Create([FromBody] IdeaCreateRequest request)
        {
            var userId = User.GetUserId();
            var result = await _ideaService.Create(
                userId,
                request.Name,
                request.Descritpion,
                request.ParentId);

            var response = new IdeaCreateResult()
            {
                Id = result.Id
            };

            return new Response<IdeaCreateResult>(response);
        }

        [HttpPost("Link/Add")]
        public async Task<Response> AddLink([FromBody] IdeaLinkAddRequest request)
        {
            await _ideaService.CreateLink(request.ParentId, request.ChildId);

            return new Response();
        }

        [HttpDelete("Link/Delete")]
        public async Task<Response> DeleteLink([FromBody] IdeaLinkDeleteRequest request)
        {
            await _ideaService.DeleteLink(request.ParentId, request.ChildId);

            return new Response();
        }
    }
}
