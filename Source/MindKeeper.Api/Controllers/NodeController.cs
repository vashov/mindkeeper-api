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
        public async Task<OperationResult<GetNodeResponse>> Get([FromRoute] int id)
        {
            var result = await _nodeService.Get(id);

            var response = _mapper.Map<OperationResult<GetNodeResponse>>(result);
            return response;
        }

        [HttpGet]
        public async Task<OperationResult<GetAllNodesResponse>> GetAll([FromQuery] GetAllNodesRequest request)
        {
            var filter = _mapper.Map<NodeFilter>(request);

            var result = await _nodeService.GetAll(filter);

            if (!result.IsOk)
                return OperationResult<GetAllNodesResponse>.Error(result.ErrorMessage);

            var nodeDtos = _mapper.Map<List<GetAllNodesResponse.NodeResponse>>(result.Data);
            var response = new GetAllNodesResponse() 
            { 
                Nodes = nodeDtos 
            };

            return OperationResult<GetAllNodesResponse>.Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<OperationResult<CreateNodeResponse>> Create([FromBody] CreateNodeRequest request)
        {
            var userId = User.GetUserId();
            var result = await _nodeService.Create(
                userId,
                request.Name,
                request.Descritpion,
                (int)NodeTypeEnum.Common,
                request.ParentId);

            if (!result.IsOk)
                return OperationResult<CreateNodeResponse>.Error(result.ErrorMessage);

            var response = new CreateNodeResponse()
            {
                Id = result.Data.Id
            };

            return OperationResult<CreateNodeResponse>.Ok(response);
        }

        /*
        public Task<OperationResult> SetParent(int nodeId, int parentNodeId);
        public Task<OperationResult> DeleteParent(int nodeId, int parentNodeId);
        public Task<OperationResult> SetChild(int nodeId, int childNodeId);
        public Task<OperationResult> DeleteChild(int nodeId, int childNodeId);
         */
    }
}
