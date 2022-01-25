﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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

                var responseModel = new Response() 
                { 
                    Succeeded = false,
                    Message = error?.Message 
                };

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
                        var statusCode = (int)HttpStatusCode.InternalServerError;
                        response.StatusCode = statusCode;
                        responseModel.Message = "Unknown error on WebApi";
                        responseModel.Status = statusCode;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);

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
