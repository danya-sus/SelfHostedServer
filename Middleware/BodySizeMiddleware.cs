using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SelfHostedServer.Middleware
{
    public class BodySizeMiddleware
    {
        private readonly RequestDelegate _next;

        public BodySizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.ContentLength > 2048)
            {
                throw new BadHttpRequestException("Request entity too large");
            }

            await _next(httpContext);
        }
    }
}
