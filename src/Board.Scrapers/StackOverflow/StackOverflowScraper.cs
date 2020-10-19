using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Board.Application.Jobs.ScrapeJobs;

namespace Board.Scrapers.StackOverflow
{
    internal class StackOverflowScraper : IJobsScraper
    {
        const string Source = "stackoverflow";

        const string AtomRSS = "http://stackoverflow.com/jobs/feed?l=Netherlands&u=Km&d=20&tl=.net+.net-core+asp.net-core+c%23";

        public Task<IEnumerable<ScrapedJob>> ScrapeAsync()
        {
            using var reader = XmlReader.Create(AtomRSS);

            var feed = SyndicationFeed.Load(reader);

            return Task.FromResult(feed.Items.Select(Map));
        }

        private ScrapedJob Map(SyndicationItem item)
        {
            var job = new ScrapedJob();

            job.Source = Source;
            job.SourceId = item.Id;
            job.Position = item.Title.Text;
            job.Description = item.Summary.Text;
            job.Company = item.Authors.First().Name;
            job.PublishedOn = item.PublishDate.UtcDateTime;
            job.ApplyUrl = item.Links.First().Uri.ToString();
            job.Tags = item.Categories.Select(c => c.Name).ToList();
            job.Location = item.ElementExtensions
                                .First(x => x.OuterName == "location")
                                .GetObject<string>();

            // Position field follows the pattern "[JobTitle] at [Company] ([Location])"
            job.Position = job.Position.Replace($"at {job.Company} ({job.Location})", string.Empty).Trim();
            job.Location = job.Location.Replace(", Netherlands", string.Empty).Trim();

            return job;
        }
    }
}
