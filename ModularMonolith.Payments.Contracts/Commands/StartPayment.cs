using System;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Payments.Language;

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