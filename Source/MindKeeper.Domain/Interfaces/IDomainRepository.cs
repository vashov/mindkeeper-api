using MindKeeper.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Domain.Interfaces
{
    public interface IDomainRepository
    {
        Task<List<DomainEntity>> GetAll();
    }
}
