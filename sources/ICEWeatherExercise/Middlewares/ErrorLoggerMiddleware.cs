using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ICEWeatherExercise.Middlewares
{
    public class ErrorLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggerMiddleware> _logger;

        public ErrorLoggerMiddleware(RequestDelegate next, ILogger<ErrorLoggerMiddleware> logger)
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
                _logger.LogCritical(error, $"{context.Request.Method} {context.Request.Path} failed with exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            }
        }
    }
}