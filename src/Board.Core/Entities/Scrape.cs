namespace Board.Core.Entities
{
    public class Scrape
    {
        public int JobId { get; set; }
        public Job Job { get; set; }

        public string Source { get; set; }
        public string SourceId { get; set; }
    }
}
