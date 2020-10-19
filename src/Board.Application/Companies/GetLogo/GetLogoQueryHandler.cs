using System.Threading;
using System.Threading.Tasks;
using Board.Core.Abstractions;
using Board.Core.Entities;
using MediatR;

namespace Board.Application.Companies.GetLogo
{
    public class GetLogoQueryHandler : IRequestHandler<GetLogoQuery, GetLogoQueryResult>
    {
        private readonly IRepository<Company> _repository;

        public GetLogoQueryHandler(IRepository<Company> repository)
        {
            _repository = repository;
        }

        public async Task<GetLogoQueryResult> Handle(GetLogoQuery request, CancellationToken cancellationToken)
        {
            var company = await _repository.GetByIdAsync(request.Id);

            return new GetLogoQueryResult { Logo = company?.Logo };
        }
    }
}
