using System.Threading.Tasks;
using Board.Core.Entities;

namespace Board.Core.Abstractions
{
    public interface IScrapeRepository
    {
        Task<bool> ExistsAsync(string source, string sourceId);

        Task InsertAsync(string source, string sourceId, Job job);
    }
}
