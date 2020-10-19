using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Board.Core.Abstractions;
using Board.Core.Entities;
using Board.Core.Specification;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Board.Application.Companies.GetCompanies
{
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, GetCompaniesQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepository<Company> _repository;

        public GetCompaniesQueryHandler(
            IMapper mapper,
            IMemoryCache cache,
            IRepository<Company> repository)
        {
            _cache = cache;
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<GetCompaniesQueryResult> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            return await _cache.GetOrCreateAsync($"companies_page:{request.Page}_size:{request.PageSize}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                var specification = PagedCompanies(request.Page, request.PageSize);

                var companies = await _repository.ListAsync(specification);

                var results = _mapper.Map<List<GetCompaniesQueryResult.Company>>(companies);

                return new GetCompaniesQueryResult(results);
            });
        }

        private ISpecification<Company> PagedCompanies(int page, int pageSize)
        {
            return new SpecificationBuilder<Company>()
                        .OrderBy(c => -c.CreatedOn.Ticks)
                        .Paginate(page, pageSize)
                        .Build();
        }
    }
}
