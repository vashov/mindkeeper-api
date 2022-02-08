using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class AchievementsRepository : IAchievementsRepository
    {
        private readonly IDriver _client;

        public AchievementsRepository(IDriver driver)
        {
            _client = driver;
        }

        public Task<List<Achievement>> GetAchievements(Guid? userId)
        {
            throw new NotImplementedException();
        }
    }
}
