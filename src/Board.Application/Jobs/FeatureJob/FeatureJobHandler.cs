using System;
using System.Threading;
using System.Threading.Tasks;
using Board.Core.Abstractions;
using Board.Core.Entities;
using Board.Core.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.Application.Jobs.FeatureJob
{
    public class FeatureJobHandler : INotificationHandler<Notification<PaymentReceivedEvent>>
    {
        private readonly IRepository<Job> _repository;
        private readonly ILogger<FeatureJobHandler> _logger;

        public FeatureJobHandler(
            IRepository<Job> repository,
            ILogger<FeatureJobHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(Notification<PaymentReceivedEvent> notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            if (!int.TryParse(@event.Reference, out var jobId))
            {
                _logger.LogError($"Unable to parse payment reference '{@event.Reference}'");
                return;
            }

            var job = await _repository.GetByIdAsync(jobId);

            if (job == null)
            {
                _logger.LogError($"Unable to match job with payment reference '{@event.Reference}'");
                return;
            }

            job.Publish(PublicationType.Featured);

            await _repository.UpdateAsync(job);
        }
    }
}
