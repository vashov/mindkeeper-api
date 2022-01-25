using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Shared.Wrappers;
using MindKeeper.Api.Data.Constants;
using MindKeeper.Api.Data.Repositories.Nodes.Models;
using MindKeeper.Api.Services.Nodes;
using MindKeeper.Shared.Models;
using MindKeeper.Shared.Models.ApiModels.Nodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Controllers
{
    [ApiController]
    [ControllerRoute]
    [Authorize]
    public class NodeController : ControllerBase
    {
        private readonly ILogger<NodeController> _logger;
        private readonly INodeService _nodeService;
        private readonly IMapper _mapper;

        public NodeController(
            ILogger<NodeController> logger,
            INodeService nodeService,
            IMapper mapper)
        {
            _logger = logger;
            _nodeService = nodeService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<Response<NodeGetResponse>> Get([FromRoute] int id)
        {
            var result = await _nodeService.Get(id);

            var response = _mapper.Map<NodeGetResponse>(result);
            return new Response<NodeGetResponse>(response);
        }

        [HttpGet]
        public async Task<Response<NodesGetAllResponse>> GetAll([FromQuery] NodesGetAllRequest request)
        {
            var filter = _mapper.Map<NodeFilter>(request);

            var result = await _nodeService.GetAll(filter);

            var nodeDtos = _mapper.Map<List<NodesGetAllResponse.NodeResponse>>(result);
            var response = new NodesGetAllResponse() 
            { 
                Nodes = nodeDtos 
            };

            return new Response<NodesGetAllResponse>(response);
        }

        [HttpPost("[action]")]
        public async Task<Response<NodeCreateResponse>> Create([FromBody] NodeCreateRequest request)
        {
            var userId = User.GetUserId();
            var result = await _nodeService.Create(
                userId,
                request.Name,
                request.Descritpion,
                (int)NodeTypeEnum.Common,
                request.ParentId);

            var response = new NodeCreateResponse()
            {
                Id = result.Id
            };

            return new Response<NodeCreateResponse>(response);
        }

        [HttpPost("Link/Add")]
        public async Task<Response> AddLink([FromBody] NodeLinkAddRequest request)
        {
            await _nodeService.CreateLink(request.ParentId, request.ChildId);

            return new Response();
        }

        [HttpDelete("Link/Delete")]
        public async Task<Response> DeleteLink([FromBody] NodeLinkDeleteRequest request)
        {
            await _nodeService.DeleteLink(request.ParentId, request.ChildId);

            return new Response();
        }
    }
}
