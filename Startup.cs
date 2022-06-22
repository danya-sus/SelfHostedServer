using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SelfHostedServer.Data;
using SelfHostedServer.Middleware;
using SelfHostedServer.ModelsDTO.ModelsDto.AutoMapperProfiles;
using SelfHostedServer.Services;
using SelfHostedServer.Validation;
using System.Linq;

namespace SelfHostedServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning();
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDbContext<TicketContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("TicketContext")));

            services.AddScoped<IJsonValidator, JsonValidator>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<IJsonValidator, JsonValidator>();

            services.AddAutoMapper(typeof(SaleDtoProfile),
                                   typeof(RefundDtoProfile),
                                   typeof(PassengerDtoProfile),
                                   typeof(RouteDtoProfile));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "SelfHostedServer", Version = "v2" });
                c.ResolveConflictingActions(apiDescription => apiDescription.First());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "SelfHostedServer v2"));
            }

            app.UseRouting();

            app.UseSizeLimit();
            app.UseValidation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
