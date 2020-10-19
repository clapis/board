using MediatR;

namespace Board.Application.Jobs.GetJobs
{
    public class GetJobsQuery : IRequest<GetJobsQueryResult>
    {
        public int PageSize = 20;

        public int Page { get; private set; }

        public GetJobsQuery(int page)
        {
            Page = page;
        }
    }
}
