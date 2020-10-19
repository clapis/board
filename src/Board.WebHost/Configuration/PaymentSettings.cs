namespace Board.WebHost.Configuration
{
    public class PaymentSettings
    {
        public StripeSettings Stripe { get; set; }
    }

    public class StripeSettings
    {
        public string WebhookSecret { get; set; }
    }
}
