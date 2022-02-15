using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MindKeeper.Shared.Wrappers;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MindKeeper.Api.Core.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = AppResponse.Error(error?.Message);

                switch (error)
                {
                    case MindKeeper.Api.Core.Exceptions.ApiException:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case MindKeeper.Api.Core.Exceptions.AppValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    default:
                        // unhandled error
                        _logger.LogError($"Unhandled error: {error}");

                        var statusCode = (int)HttpStatusCode.InternalServerError;
                        response.StatusCode = statusCode;
                        responseModel.Message = "Unknown error on WebApi";
                        responseModel.Status = statusCode;
                        break;
                }

                var jsonSerializerSettings = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var result = JsonSerializer.Serialize(responseModel, jsonSerializerSettings);

                await response.WriteAsync(result);
            }
        }
    }

    public static class ErrorHandlerMiddlewareExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
