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
using MindKeeper.DataAccess.Neo4jSource;
using MindKeeper.DataAccess.Neo4jSource.Seed;
using MindKeeper.DataAccess.PostgreSource.Seed;
using MindKeeper.Shared.Wrappers;
using Neo4j.Driver;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MindKeeper.Api
{
    public class Startup
    {
        //private readonly string _dbConnectionString = SqlConnectionStringBuilder.Build();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(_dbConnectionString));

            services.AddSingleton<IDriver>(sp => GraphDatabase.Driver(Neo4jSettings.Uri,
                    AuthTokens.Basic(Neo4jSettings.Username, Neo4jSettings.Password)));

            services.AddRepositories();
            services.AddBusinessLogicServices();

            services.AddTransient<DbMigration>();
            services.AddTransient<Neo4jDbConstraints>();
            services.AddTransient<Neo4jDbPopulation>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.Response.OnStarting(async () =>
                            {
                                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                                {
                                    var response = new Response("Unauthorized")
                                    {
                                        Status = (int)HttpStatusCode.Unauthorized
                                    };

                                    await context.Response.WriteAsJsonAsync(response);
                                }
                            });
                            return Task.CompletedTask;
                        },
                        OnForbidden = async context =>
                        {
                            var response = new Response("Forbidden")
                            {
                                Status = (int)HttpStatusCode.Forbidden
                            };

                            await context.Response.WriteAsJsonAsync(response);
                        }
                    };
                    options.Events = events;

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
            //InitDatabase(services);

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            InitNeo4jDatabase(services);

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

        private static void InitNeo4jDatabase(IServiceProvider services)
        {
            var dbConstraints = (Neo4jDbConstraints)services.GetService(typeof(Neo4jDbConstraints));
            dbConstraints.Init().ConfigureAwait(false).GetAwaiter().GetResult();

            var dbPopulation = (Neo4jDbPopulation)services.GetService(typeof(Neo4jDbPopulation));
            dbPopulation.Init().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
