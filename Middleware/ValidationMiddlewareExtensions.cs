using Microsoft.AspNetCore.Builder;

namespace SelfHostedServer.Middleware
{
    public static class ValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidationMiddlware>();
        }
    }
}
