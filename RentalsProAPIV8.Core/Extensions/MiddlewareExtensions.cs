using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RentalsProAPIV8.Core.Extensions
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds a global exception handling middleware.
        /// </summary>
        /// <param name="app">The IApplicationBuilder instance.</param>
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// Adds a middleware to log request and response details.
        /// </summary>
        /// <param name="app">The IApplicationBuilder instance.</param>
        public static void UseRequestResponseLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }

        /// <summary>
        /// Adds a custom header to all responses.
        /// </summary>
        /// <param name="app">The IApplicationBuilder instance.</param>
        /// <param name="headerName">The name of the header.</param>
        /// <param name="headerValue">The value of the header.</param>
        public static void UseCustomHeader(this IApplicationBuilder app, string headerName, string headerValue)
        {
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers[headerName] = headerValue;
                    return Task.CompletedTask;
                });

                await next.Invoke();
            });
        }

        /// <summary>
        /// Configures Swagger UI for multiple API versions.
        /// </summary>
        public static void AddSwaggerUI(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"API {description.GroupName.ToUpperInvariant()}");
                }
            });

            // Add ReDoc for all API versions
            foreach (var description in provider.ApiVersionDescriptions)
            {
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = $"swagger/{description.GroupName}";
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Version {description.GroupName}");
                });

                app.UseReDoc(c =>
                {
                    c.RoutePrefix = $"redoc/{description.GroupName}";
                    c.SpecUrl($"/swagger/{description.GroupName}/swagger.json");
                });
            }
        }
    }

    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the incoming request details
            _logger.LogInformation("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);

            // Call the next middleware in the pipeline
            await _next(context);

            // Log the outgoing response details
            _logger.LogInformation("Handled response: {StatusCode}", context.Response.StatusCode);
        }
    }
}
