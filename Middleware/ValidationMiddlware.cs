using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json.Schema;
using SelfHostedServer.Validation;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedServer.Middleware
{
    public class ValidationMiddlware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IJsonValidator validator)
        {
            if (!httpContext.Request.HasJsonContentType())
            {
                throw new NotSupportedException("Media type should be JSON");
            }

            var sync = httpContext.Features.Get<IHttpBodyControlFeature>().AllowSynchronousIO = true;

            var request = httpContext.Request;
            request.EnableBuffering();
            request.Body.Seek(0, SeekOrigin.Begin);

            var schema = request.Path.ToString().Replace("/", " ").Trim().Split(" ");

            var json = "";
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, false, 2048, true))
            {
                json = reader.ReadToEnd();
                httpContext.Items.Add("request_body", json);
            }
            request.Body.Seek(0, SeekOrigin.Begin);

            if (!validator.IsValid(json, schema[schema.Length - 1]))
            {
                throw new JSchemaValidationException("JSON is not valid");
            }

            await _next(httpContext);
        }
    }
}
