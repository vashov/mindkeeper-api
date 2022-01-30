using MindKeeper.Domain.Entities;
using MindKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindKeeper.DataAccess.Neo4jSource.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> Create(string mail, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(string mail, bool isNormalizedSearch = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
