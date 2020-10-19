using System;
using Board.Core.Entities;
using Board.Core.Specification;

namespace Board.Application.Jobs.GetJobs
{
    public class LatestJobsSpecification : Specification<Job>
    {
        public LatestJobsSpecification(int page, int pageSize)
        {
            AddInclude(j => j.Company);
            ApplyPaginate(page, pageSize);
            ApplyOrder(j => -j.CreatedOn.Ticks);
            AddCriteria(j => j.PublishedOn > DateTime.UtcNow.AddMonths(-3));
        }
    }
}
