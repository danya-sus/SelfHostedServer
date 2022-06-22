using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Schema;
using Npgsql;
using System;
using System.Net;
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
                httpContext.Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync("Request entity too large");
                return;
            }

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException argumentNullException && argumentNullException.Message == "Media type should be JSON")
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync("Media type should be JSON");
                }

                if (ex is NotSupportedException notSupportedException && notSupportedException.Message == "Media type should be JSON")
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync("Media type should be JSON");
                }

                if (ex is JSchemaValidationException jSchemaValidationException && jSchemaValidationException.Message == "JSON is not valid")
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync("JSON is not valid");
                }

                if (ex is BadHttpRequestException badRequestException && badRequestException.Message == "Request body too large.")
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync("Request entity too large");
                }

                if (ex is DbUpdateException dbUpdateException && dbUpdateException.InnerException is PostgresException)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync("Conflict: a duplicate key value violates the uniqueness constraint");
                }

                if (ex is DbUpdateException dbConflictException && dbConflictException.Message == "Operation execution conflict")
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync("Conflict: this operation cannot be performed");
                }

                if (ex is TimeoutException)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync("Request timeout");
                }
                throw;
            }
        }
    }
}
