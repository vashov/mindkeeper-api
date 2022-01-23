using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace MindKeeper.Api.Core.OpenApi
{
    /// <summary>
    /// Source: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1586#issuecomment-604104300
    /// </summary>
    internal class BasicAuthOperationsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var noAuthRequired = context.ApiDescription.CustomAttributes()
                .Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));

            if (noAuthRequired) 
                return;

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        AppSecurityScheme.Scheme,
                        new List<string>()
                    }
                }
            };
        }
    }
}
