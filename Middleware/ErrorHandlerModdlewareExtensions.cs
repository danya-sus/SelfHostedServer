using Microsoft.AspNetCore.Builder;

namespace SelfHostedServer.Middleware
{
    public static class ErrorHandlerModdlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
