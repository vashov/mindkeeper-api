using MindKeeper.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Countries
{
    public interface ICountryService
    {
        Task<List<Country>> GetAll();
    }
}
