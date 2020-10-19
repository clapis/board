using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Board.Core.Abstractions;
using Board.Core.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Board.Application.Jobs.GetJobs
{

    public class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, GetJobsQueryResult>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepository<Job> _repository;

        public GetJobsQueryHandler(IRepository<Job> repository, IMapper mapper, IMemoryCache cache)
        {
            _cache = cache;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<GetJobsQueryResult> Handle(GetJobsQuery request, CancellationToken cancellationToken)
        {
            return await _cache.GetOrCreateAsync($"jobs_page:{request.Page}_size:{request.PageSize}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                var specification = new LatestJobsSpecification(request.Page, request.PageSize);

                var jobs = await _repository.ListAsync(specification);

                var results = _mapper.Map<List<GetJobsQueryResult.Job>>(jobs);

                return new GetJobsQueryResult { Jobs = results };
            });
        }

    }

}
