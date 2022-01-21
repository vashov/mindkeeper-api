using MindKeeper.Api.Data.Repositories.Users.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task<long> Create(string mail, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(string mail)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
