using System;

namespace Board.WebHost.Configuration
{
    public class ScrapingSettings
    {
        public bool IsEnabled { get; set; }

        public TimeSpan Interval { get; set; }
    }
}
