﻿using MindKeeper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindKeeper.Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> Get(Guid id);
        public Task<User> Get(string mail, bool isNormalizedSearch = false);
        public Task<List<User>> GetAll();
        public Task<User> Create(string mail, string passwordHash);
    }
}
