using MindKeeper.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Domains
{
    public interface IDomainService
    {
        Task<List<DomainEntity>> GetAll();
    }
}
