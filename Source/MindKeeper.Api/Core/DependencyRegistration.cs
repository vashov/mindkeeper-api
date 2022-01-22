using Microsoft.Extensions.DependencyInjection;
using MindKeeper.Api.Data.Repositories.Nodes;
using MindKeeper.Api.Data.Repositories.Users;
using MindKeeper.Api.Services.Nodes;
using MindKeeper.Api.Services.Users;

namespace MindKeeper.Api.Core
{
    public static class DependencyRegistration
    {
        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INodeService, NodeService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INodeRepository, NodeRepository>();
        }
    }
}
