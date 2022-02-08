using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Api.Services.Domains;
using MindKeeper.Shared.Models.ApiModels.Domains;
using MindKeeper.Shared.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Controllers
{
    [ApiController]
    [ControllerRoute]
    [Authorize]
    public class DomainController : ControllerBase
    {
        private readonly IDomainService _domainService;
        private readonly IMapper _mapper;

        public DomainController(
            IDomainService domainService,
            IMapper mapper)
        {
            _domainService = domainService;
            _mapper = mapper;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24)]
        [AllowAnonymous]
        public async Task<Response<DomainGetAllResult>> GetAll()
        {
            var result = await _domainService.GetAll();

            var domainsDto = _mapper.Map<List<DomainGetAllResult.Domain>>(result);
            var response = new DomainGetAllResult
            {
                Domains = domainsDto
            };

            return new Response<DomainGetAllResult>(response);
        }
    }
}
