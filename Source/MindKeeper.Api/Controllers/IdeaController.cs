using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Api.Services.Ideas;
using MindKeeper.Domain.Constants;
using MindKeeper.Domain.Filters;
using MindKeeper.Shared.Models.ApiModels.Ideas;
using MindKeeper.Shared.Wrappers;
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
        private readonly IIdeaService _nodeService;
        private readonly IMapper _mapper;

        public IdeaController(
            ILogger<IdeaController> logger,
            IIdeaService nodeService,
            IMapper mapper)
        {
            _logger = logger;
            _nodeService = nodeService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<Response<IdeaGetResult>> Get([FromRoute] int id)
        {
            var result = await _nodeService.Get(id);

            var response = _mapper.Map<IdeaGetResult>(result);
            return new Response<IdeaGetResult>(response);
        }

        [HttpGet]
        public async Task<Response<IdeasGetAllResult>> GetAll([FromQuery] IdeasGetAllRequest request)
        {
            var filter = _mapper.Map<NodeFilter>(request);

            var result = await _nodeService.GetAll(filter);

            var nodeDtos = _mapper.Map<List<IdeasGetAllResult.IdeaResponse>>(result);
            var response = new IdeasGetAllResult() 
            { 
                Ideas = nodeDtos 
            };

            return new Response<IdeasGetAllResult>(response);
        }

        [HttpPost("[action]")]
        public async Task<Response<IdeaCreateResult>> Create([FromBody] IdeaCreateRequest request)
        {
            var userId = User.GetUserId();
            var result = await _nodeService.Create(
                userId,
                request.Name,
                request.Descritpion,
                (int)NodeTypeEnum.Common,
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
            await _nodeService.CreateLink(request.ParentId, request.ChildId);

            return new Response();
        }

        [HttpDelete("Link/Delete")]
        public async Task<Response> DeleteLink([FromBody] IdeaLinkDeleteRequest request)
        {
            await _nodeService.DeleteLink(request.ParentId, request.ChildId);

            return new Response();
        }
    }
}
