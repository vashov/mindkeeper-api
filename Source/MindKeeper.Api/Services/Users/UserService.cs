using MindKeeper.Shared;
using System;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Users
{
    public class UserService : IUserService
    {
        public async Task<OperationResult> Login(string mail, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<string>> RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<long>> CreateUser(string mail, string password)
        {
            throw new NotImplementedException();
        }
    }
}
