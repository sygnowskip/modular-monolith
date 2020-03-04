using CSharpFunctionalExtensions;
using MediatR;

namespace ModularMonolith.Payments.Contracts.Commands
{
    public class CompletePayment : IRequest<Result>
    {
        public CompletePayment(PaymentId id)
        {
            Id = id;
        }

        public PaymentId Id { get; }
    }
}