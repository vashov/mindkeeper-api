using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MindKeeper.Api.Core;
using MindKeeper.Api.Core.Auth;
using MindKeeper.Api.Core.Middlewares;
using MindKeeper.Api.Core.OpenApi;
using MindKeeper.Api.Data.Migrations;
using MindKeeper.Shared.Wrappers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MindKeeper.Api
{
    public class Startup
    {
        private readonly string _dbConnectionString = SqlConnectionStringBuilder.Build();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(_dbConnectionString));

            services.AddRepositories();
            services.AddBusinessLogicServices();

            services.AddTransient<DbMigration>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,

                        ValidateLifetime = true,

                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState.Values
                            .SelectMany(c => c.Errors)
                            .Where(c => !string.IsNullOrEmpty(c.ErrorMessage))
                            .Select(c => c.ErrorMessage);
                        var response = new Response("One or more validation errors.")
                        {
                            Errors = errors.ToList(),
                        };
                        return new BadRequestObjectResult(response);
                    };
                });

                services.AddConfiguredSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            InitDatabase(services);

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MindKeeper.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void InitDatabase(IServiceProvider services)
        {
            var dbMigration = (DbMigration)services.GetService(typeof(DbMigration));
            dbMigration.InitDatabase().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
