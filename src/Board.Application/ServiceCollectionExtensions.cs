using AutoMapper;
using Board.Core.Abstractions;
using Ganss.XSS;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBoardApplication(this IServiceCollection services)
        {
            services.AddTransient(sp => new HtmlSanitizer());

            services.AddTransient<IDomainEventDispatcher, EventDispatcher>();

            return services
                    .AddMemoryCache()
                    .AddMediatR(typeof(ServiceCollectionExtensions))
                    .AddAutoMapper(typeof(ServiceCollectionExtensions));
        }

    }
}
