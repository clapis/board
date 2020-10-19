using Board.Application.Jobs.ScrapeJobs;
using Board.Scrapers.StackOverflow;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Scrapers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJobScrapers(this IServiceCollection services)
        {
            services.AddTransient<IJobsScraper, StackOverflowScraper>();

            return services;
        }
    }
}
