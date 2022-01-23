using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Routing;
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
        public async Task<OperationResult<NodeGetResponse>> Get([FromRoute] int id)
        {
            var result = await _nodeService.Get(id);

            var response = _mapper.Map<OperationResult<NodeGetResponse>>(result);
            return response;
        }

        [HttpGet]
        public async Task<OperationResult<NodesGetAllResponse>> GetAll([FromQuery] NodesGetAllRequest request)
        {
            var filter = _mapper.Map<NodeFilter>(request);

            var result = await _nodeService.GetAll(filter);

            if (!result.IsOk)
                return OperationResult<NodesGetAllResponse>.Error(result.ErrorMessage);

            var nodeDtos = _mapper.Map<List<NodesGetAllResponse.NodeResponse>>(result.Data);
            var response = new NodesGetAllResponse() 
            { 
                Nodes = nodeDtos 
            };

            return OperationResult<NodesGetAllResponse>.Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<OperationResult<NodeCreateResponse>> Create([FromBody] NodeCreateRequest request)
        {
            var userId = User.GetUserId();
            var result = await _nodeService.Create(
                userId,
                request.Name,
                request.Descritpion,
                (int)NodeTypeEnum.Common,
                request.ParentId);

            if (!result.IsOk)
                return OperationResult<NodeCreateResponse>.Error(result.ErrorMessage);

            var response = new NodeCreateResponse()
            {
                Id = result.Data.Id
            };

            return OperationResult<NodeCreateResponse>.Ok(response);
        }

        [HttpPost("Link/Add")]
        public async Task<OperationResult> AddLink([FromBody] NodeLinkAddRequest request)
        {
            var result = await _nodeService.CreateLink(request.ParentId, request.ChildId);

            return result;
        }

        [HttpDelete("Link/Delete")]
        public async Task<OperationResult> DeleteLink([FromBody] NodeLinkDeleteRequest request)
        {
            var result = await _nodeService.DeleteLink(request.ParentId, request.ChildId);

            return result;
        }
    }
}
