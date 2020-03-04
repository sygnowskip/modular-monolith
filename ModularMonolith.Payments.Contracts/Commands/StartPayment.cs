using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace ModularMonolith.Payments.Contracts.Commands
{
    public class StartPayment : IRequest<Result<PaymentId>>
    {
        public StartPayment(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}