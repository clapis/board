namespace Board.Core.Events
{
    public class PaymentReceivedEvent : DomainEvent
    {
        public PaymentReceivedEvent(string reference)
        {
            Reference = reference;
        }

        public string Reference { get; }
    }
}
