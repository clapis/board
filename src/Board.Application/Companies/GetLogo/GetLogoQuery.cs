using MediatR;

namespace Board.Application.Companies.GetLogo
{
    public class GetLogoQuery : IRequest<GetLogoQueryResult>
    {
        public int Id { get; }

        public GetLogoQuery(int id)
        {
            Id = id;
        }
    }
}
