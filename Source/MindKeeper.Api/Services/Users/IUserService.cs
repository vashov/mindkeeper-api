using MindKeeper.Domain.Entities;
using System.Threading.Tasks;

namespace MindKeeper.Api.Services.Users
{
    public interface IUserService
    {
        public Task<User> CreateUser(string mail, string password);

        public Task<string> CreateAccessToken(string mail, string password);

        public Task<string> RefreshToken(string refreshToken);
    }
}
