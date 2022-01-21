using Microsoft.Extensions.DependencyInjection;
using MindKeeper.Api.Data.Repositories.Users;
using MindKeeper.Api.Services.Users;

namespace MindKeeper.Api.Core
{
    public static class DependencyRegistration
    {
        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
