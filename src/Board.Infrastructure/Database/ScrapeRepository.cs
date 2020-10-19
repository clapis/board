using System.Threading.Tasks;
using Board.Core.Entities;
using Board.Core.Abstractions;

namespace Board.Infrastructure.Database
{
    public class ScrapeRepository : IScrapeRepository
    {
        private readonly BoardDbContext _context;

        public ScrapeRepository(BoardDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string source, string sourceId)
        {
            var result = await _context.Scrapes.FindAsync(source, sourceId);

            return result != null;
        }


        public Task InsertAsync(string source, string sourceId, Job job)
        {
            _context.Scrapes.Add(new Scrape
            {
                Job = job,
                Source = source,
                SourceId = sourceId
            });

            return _context.SaveChangesAsync();
        }

    }
}
