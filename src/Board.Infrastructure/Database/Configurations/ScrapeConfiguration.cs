using Board.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Database.Configurations
{
    public class ScrapeConfiguration : IEntityTypeConfiguration<Scrape>
    {
        public void Configure(EntityTypeBuilder<Scrape> builder)
        {
            builder.HasKey(c => new { c.Source, c.SourceId });
        }
    }
}
