using System;
using System.Text.Json;
using System.Threading.Tasks;
using api.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ApiError ex)
            {
                await ApiErrorHandler(context, ex);
            }
            catch (Exception ex)
            {
                await ExceptionHandler(context, ex);
            }
        }

        private static Task ApiErrorHandler(HttpContext context, ApiError exception)
        {
            var result = JsonSerializer.Serialize(new
            {
                status = (int)exception.StatusCode,
                error = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.StatusCode;

            return context.Response.WriteAsync(result);
        }
        
        private static Task ExceptionHandler(HttpContext context, Exception exception)
        {
            var result = JsonSerializer.Serialize(new
            {
                status = 500,
                error = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            return context.Response.WriteAsync(result);
        }
    }
}