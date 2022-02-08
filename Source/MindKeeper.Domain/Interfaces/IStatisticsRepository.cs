using MindKeeper.Domain.EntitiesComposed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindKeeper.Domain.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<StatsSystem> GetSystemStats();
        Task<StatsUser> GetUserStats(Guid userId);
    }
}
