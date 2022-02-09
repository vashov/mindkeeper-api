using MindKeeper.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Domain.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAll();
    }
}
