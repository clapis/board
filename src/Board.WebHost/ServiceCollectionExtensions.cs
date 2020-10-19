using Board.Scrapers;
using Board.WebHost.HostedServices;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Board.WebHost
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBoardWebHost(this IServiceCollection services)
        {
            services.AddJobScrapers();
            services.AddHostedService<ScrapingHostedService>();

            services.AddControllers()
                .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining(typeof(Application.ServiceCollectionExtensions)));

            services.AddResponseCaching();

            services.AddCors(opts =>
                opts.AddDefaultPolicy(p =>
                    p.AllowAnyHeader()
                     .AllowAnyOrigin()
                     .WithMethods("GET", "POST", "PUT")
                )
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            return services;
        }
    }
}
