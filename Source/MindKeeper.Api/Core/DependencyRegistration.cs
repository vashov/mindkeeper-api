using Microsoft.Extensions.DependencyInjection;
using MindKeeper.Api.Services.Countries;
using MindKeeper.Api.Services.Domains;
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
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IDomainService, DomainService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIdeaRepository, IdeaRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IAchievementsRepository, AchievementsRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDomainRepository, DomainRepository>();
        }
    }
}
