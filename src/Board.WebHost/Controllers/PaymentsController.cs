using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using Board.WebHost.Configuration;
using Microsoft.Extensions.Options;
using Board.Application.Payments.RegisterPayment;

namespace Board.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IMediator _mediator;
        private readonly PaymentSettings _settings;


        public PaymentsController(
            ILogger<PaymentsController> logger,
            IMediator mediator,
            IOptions<PaymentSettings> settings)
        {
            _logger = logger;
            _mediator = mediator;
            _settings = settings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            using var stream = new StreamReader(Request.Body);

            var json = await stream.ReadToEndAsync();

            var @event = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _settings.Stripe.WebhookSecret);

            _logger.LogDebug($"Payment event received: {@event.ToJson()}");

            switch(@event.Type)
            {
                case Events.CheckoutSessionCompleted:
                    await HandleCheckoutSessionCompletedAsync(@event.Data.Object as Session);
                    break;
                default:
                    _logger.LogInformation($"Payment event '{@event.Type}' not handled");
                    break;
            }

            return Ok();

        }

        private async Task HandleCheckoutSessionCompletedAsync(Session session)
        {
            await _mediator.Send(new RegisterPaymentCommand(session.ClientReferenceId, session.CustomerEmail, session.ToJson()));
        }

    }
}
