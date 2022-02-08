using Microsoft.Extensions.DependencyInjection;
using MindKeeper.Api.Services.Ideas;
using MindKeeper.Api.Services.Statistics;
using MindKeeper.Api.Services.Users;
using MindKeeper.DataAccess.Neo4jSource.Repositories;
using MindKeeper.Domain.Interfaces;

namespace MindKeeper.Api.Core
{
    public static class DependencyRegistration
    {
        public static void AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdeaService, IdeaService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIdeaRepository, IdeaRepository>();
        }
    }
}
