using System.Collections.Generic;

namespace Board.Application.Jobs.GetJobs
{
    public class GetJobsQueryResult
    {
        public List<Job> Jobs { get; set; }

        public class Job
        {
            public int Id { get; set; }

            public int CompanyId { get; set; }

            public string CompanyName { get; set; }

            public bool HasLogo { get; set; }

            public string Position { get; set; }

            public string Description { get; set; }

            public bool Remote { get; set; }

            public string Location { get; set; }

            public string ApplyUrl { get; set; }

            public List<string> Tags { get; set; }

            public bool IsFeatured { get; set; }

            public int Age { get; set; }

        }

    }

}
