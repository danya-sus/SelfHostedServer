using Microsoft.AspNetCore.Builder;

namespace SelfHostedServer.Middleware
{
    public static class BodySizeMiddlewareExtensions
    {
        public static IApplicationBuilder UseSizeLimit(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BodySizeMiddleware>();
        }
    }
}
