using MediatR;

namespace Board.Application.Companies.GetCompanies
{
    public class GetCompaniesQuery : IRequest<GetCompaniesQueryResult>
    {
        public int PageSize = 50;

        public int Page { get; private set; }

        public GetCompaniesQuery(int page)
        {
            Page = page;
        }
    }
}
