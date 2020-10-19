using Board.Core.Entities;
using Board.Core.Specification;

namespace Board.Application.Jobs.ScrapeJobs
{
    public class CompanyNameSpecification : Specification<Company>
    {
        public CompanyNameSpecification(string name)
        {
            AddCriteria(c => c.Name == name);
        }
    }
}
