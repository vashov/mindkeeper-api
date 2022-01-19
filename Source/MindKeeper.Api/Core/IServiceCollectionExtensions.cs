using Microsoft.Extensions.DependencyInjection;
using MindKeeper.Api.Services.Users;

namespace MindKeeper.Api.Core
{
    public static class IServiceCollectionExtensions
    {
        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
