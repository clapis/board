using Board.Core.Abstractions;
using Board.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Infrastructure
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddBoardInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<BoardDbContext>();

            services.AddTransient<IScrapeRepository, ScrapeRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));

            return services;
        }

    }
}
