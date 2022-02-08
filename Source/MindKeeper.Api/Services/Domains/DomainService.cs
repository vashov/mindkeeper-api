using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Domains
{
    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _domainRepository;

        public DomainService(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task<List<DomainEntity>> GetAll()
        {
            var domains = await _domainRepository.GetAll();
            return domains;
        }
    }
}
