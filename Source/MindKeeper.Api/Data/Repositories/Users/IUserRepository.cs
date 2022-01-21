﻿using MindKeeper.Api.Data.Repositories.Users.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Api.Data.Repositories.Users
{
    public interface IUserRepository
    {
        public Task<User> Get(long id);
        public Task<User> Get(string mail);
        public Task<List<User>> GetAll();
        public Task<User> Create(string mail, string passwordHash);
    }
}