using System.Text.Json;
using ExpenseTracker.Api.Common.Exceptions;
using ExpenseTracker.Api.DTOs.Common;

namespace ExpenseTracker.Api.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = ex.StatusCode;
                await WriteError(context, ex.Message, ex.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await WriteError(context, $"Something went wrong. Please try again later.{ex}", StatusCodes.Status500InternalServerError);
            }


        }
        private static async Task WriteError(
            HttpContext context,
            string message,
            int statusCode)
        {
            context.Response.ContentType = "application/json";
            var response = new ApiResponse<object>()
            {
                Data = null,
                Error = new ApiError
                {
                    Message = message,
                    StatusCode = statusCode
                }
            };

            await context.Response.WriteAsJsonAsync(
                response
            );
        }

    }
}