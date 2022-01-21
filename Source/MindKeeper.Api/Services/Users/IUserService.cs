using MindKeeper.Shared;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Users
{
    public interface IUserService
    {
        public Task<OperationResult<long>> CreateUser(string mail, string password);

        public Task<OperationResult<string>> CreateAccessToken(string mail, string password);

        public Task<OperationResult<string>> RefreshToken(string refreshToken);
    }
}
