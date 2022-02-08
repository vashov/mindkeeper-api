using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Countries
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<Country>> GetAll()
        {
            var results = await _countryRepository.GetAll();
            return results;
        }
    }
}
