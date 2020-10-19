using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Board.Core.Entities;
using Board.Core.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.Application.Jobs.ScrapeJobs
{
    public class ScrapeJobsCommandHandler : AsyncRequestHandler<ScrapeJobsCommand>
    {
        private readonly IScrapeRepository _scrapes;
        private readonly IRepository<Company> _companies;
        private readonly IEnumerable<IJobsScraper> _scrapers;
        private readonly ILogger<ScrapeJobsCommandHandler> _logger;


        public ScrapeJobsCommandHandler(
            IScrapeRepository repository,
            IRepository<Company> companies,
            IEnumerable<IJobsScraper> scrapers,
            ILogger<ScrapeJobsCommandHandler> logger)
        {
            _logger = logger;
            _scrapers = scrapers;
            _companies = companies;
            _scrapes = repository;
        }


        protected override async Task Handle(ScrapeJobsCommand request, CancellationToken cancellationToken)
        {
            // TODO: Good candidate for parallelization
            foreach (var scraper in _scrapers) await ExecuteScraper(scraper);
        }

        private async Task ExecuteScraper(IJobsScraper scraper)
        {
            foreach (var scraped in await scraper.ScrapeAsync())
            {
                _logger.LogDebug($"Processing {scraped.Source}:{scraped.SourceId} {scraped.Position} at {scraped.Company}");

                // If already scrapped, ignore it - no updates for now
                if (await _scrapes.ExistsAsync(scraped.Source, scraped.SourceId)) continue;

                // Try find the company by name since we don't have external identifiers for it :(
                var company = await GetCompanyByNameAsync(scraped.Company);

                var job = new Job(scraped.Position, company, scraped.ApplyUrl);

                job.AddTags(scraped.Tags);
                job.SetLocation(scraped.Remote, scraped.Location);
                job.SetDescription(scraped.Description, desc => desc);

                job.Publish(PublicationType.Scraped); 

                await _scrapes.InsertAsync(scraped.Source, scraped.SourceId, job);
            }
        }


        private async Task<Company> GetCompanyByNameAsync(string name)
        {
            var spec = new CompanyNameSpecification(name);

            var company = await _companies.FirstOrDefaultAsync(spec);

            return company ?? new Company(name);
        }
    }
}
