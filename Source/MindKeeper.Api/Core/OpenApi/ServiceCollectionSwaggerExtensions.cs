using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MindKeeper.Shared.Models;
using System;
using System.IO;
using System.Reflection;

namespace MindKeeper.Api.Core.OpenApi
{
    public static class ServiceCollectionSwaggerExtensions
    {
        public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
        {
            var securityScheme = AppSecurityScheme.Scheme;

            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MindKeeper.Api", Version = "v1" });

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.OperationFilter<BasicAuthOperationsFilter>();

                var xmlFile = $"{Assembly.GetAssembly(typeof(Shared.Wrappers.Response)).GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
