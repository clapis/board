using System.Threading;
using System.Threading.Tasks;
using Board.Core.Entities;
using Board.Core.Abstractions;
using Ganss.XSS;
using MediatR;

namespace Board.Application.Jobs.PostJob
{
    public class PostJobCommandHandler : IRequestHandler<PostJobCommand, int>
    {
        private readonly IRepository<Job> _repository;
        private readonly HtmlSanitizer _htmlSanitizer;

        public PostJobCommandHandler(
            IRepository<Job> repository,
            HtmlSanitizer htmlSanitizer)
        {
            _repository = repository;
            _htmlSanitizer = htmlSanitizer;
        }

        public async Task<int> Handle(PostJobCommand request, CancellationToken cancellationToken)
        {
            var job = CreateJob(request);

            job.Publish(PublicationType.Free);

            await _repository.AddAsync(job);

            return job.Id;
        }


        private Job CreateJob(PostJobCommand request)
        {
            var company = CreateCompany(request);

            var job = new Job(request.Position, company, request.ApplyUrl);

            job.AddTags(request.Tags);
            job.SetLocation(request.Remote, request.Location);
            job.SetDescription(request.Description, desc => _htmlSanitizer.Sanitize(desc));

            return job;

        }

        private Company CreateCompany(PostJobCommand request)
        {
            return new Company(request.CompanyName)
            {
                Logo = request.Logo,
                Website = request.Website
            };
        }

    }
}
