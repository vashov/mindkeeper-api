using MindKeeper.Api.Data.Repositories.Users.Models;
using MindKeeper.Shared;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Users
{
    public interface IUserService
    {
        public Task<OperationResult<User>> CreateUser(string mail, string password);

        public Task<OperationResult<string>> CreateAccessToken(string mail, string password);

        public Task<OperationResult<string>> RefreshToken(string refreshToken);
    }
}
