using System.Threading;
using System.Threading.Tasks;
using Board.Core.Abstractions;
using Board.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Board.Application.Payments.RegisterPayment
{
    public class RegisterPaymentCommandHandler : AsyncRequestHandler<RegisterPaymentCommand>
    {
        private readonly IRepository<ReceivedPayment> _repository;
        private readonly ILogger<RegisterPaymentCommandHandler> _logger;

        public RegisterPaymentCommandHandler(
            IRepository<ReceivedPayment> repository,
            ILogger<RegisterPaymentCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        protected override async Task Handle(RegisterPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new ReceivedPayment(request.Reference, request.CustomerEmail, request.AdditionalInfo);

            _logger.LogInformation($"Payment with reference '{request.Reference}' received.");

            await _repository.AddAsync(payment);
        }
    }
}
