using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.Application.Jobs.ScrapeJobs
{
    public interface IJobsScraper
    {
        Task<IEnumerable<ScrapedJob>> ScrapeAsync();

    }

    public class ScrapedJob
    {
        public string Source { get; set; }

        public string SourceId { get; set; }


        public string Company { get; set; }

        public string Position { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }

        public bool Remote { get; set; }

        public string Location { get; set; }

        public string ApplyUrl { get; set; }

        public DateTime PublishedOn { get; set; }
    }

}
