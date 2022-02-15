using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindKeeper.Api.Core.Routing;
using MindKeeper.Api.Services.Countries;
using MindKeeper.Shared.Models.ApiModels.Countries;
using MindKeeper.Shared.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Controllers
{
    [ApiController]
    [ControllerRoute]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountryController(
            ICountryService countryService,
            IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24)]
        [AllowAnonymous]
        public async Task<AppResponse<CountryGetAllResult>> GetAll()
        {
            var result = await _countryService.GetAll();

            var countriesDto = _mapper.Map<List<CountryGetAllResult.Country>>(result);
            var response = new CountryGetAllResult 
            { 
                Countries = countriesDto
            };

            return AppResponse<CountryGetAllResult>.Ok(response);
        }
    }
}
