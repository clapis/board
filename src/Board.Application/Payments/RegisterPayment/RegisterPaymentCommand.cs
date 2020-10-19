using MediatR;

namespace Board.Application.Payments.RegisterPayment
{
    public class RegisterPaymentCommand : IRequest
    {
        public string Reference { get; private set; }
        public string CustomerEmail { get; private set; }
        public string AdditionalInfo { get; private set; }


        public RegisterPaymentCommand(string reference, string customerEmail, string additionalInfo)
        {
            Reference = reference;
            CustomerEmail = customerEmail;
            AdditionalInfo = additionalInfo;
        }
    }
}
