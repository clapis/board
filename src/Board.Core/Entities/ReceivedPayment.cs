using Board.Core.Events;

namespace Board.Core.Entities
{
    public class ReceivedPayment : BaseEntity
    {
        public ReceivedPayment(string reference, string customerEmail, string additionalInfo)
        {
            Reference = reference;
            CustomerEmail = customerEmail;
            AdditionalInfo = additionalInfo;

            Events.Add(new PaymentReceivedEvent(reference));
        }

        public string Reference { get; private set; }
        public string CustomerEmail { get; private set; }
        public string AdditionalInfo { get; private set; }
    }
}
