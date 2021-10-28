using System;
using System.Threading.Tasks;
using CommonLib.Models.ErrorModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Gateway.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (ErrorDetails ex)
            {

               // _logger.Information(ex, "A known error has occurred.");
                await HandleAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                //_logger.Error(ex, "An unknown error has occured.");
                await HandleAsync(httpContext, ex);
            }
        }

        public async Task HandleAsync(HttpContext httpContext, ErrorDetails exception)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = exception.StatusCode;
            
            await response.WriteAsync(exception.ToString());

        }

        public async Task HandleAsync(HttpContext httpContext, System.Exception exception)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = 500;
            await response.WriteAsync(exception.Message);
        }
    }

    public static class ErrorHandlingMiddleWareExtension
    {
        public static IApplicationBuilder UseCustomErrorHandler(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
    }

}