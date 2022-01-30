using Microsoft.Extensions.DependencyInjection;
using MindKeeper.DataAccess.PostgreSource.Repositories;
using MindKeeper.Domain.Interfaces;
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
