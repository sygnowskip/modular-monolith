using System;
using System.Collections;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MediatR;

namespace ModularMonolith.Payments.Contracts.Queries
{
    public class PaymentDto
    {
        public PaymentDto(PaymentId id, Guid correlationId, Enum status)
        {
            Id = id;
            CorrelationId = correlationId;
            Status = status;
        }

        public PaymentId Id { get; }
        public Guid CorrelationId { get; }
        //TODO: Problem
        public Enum Status { get; }
    }

    public class PaymentsInProgress : IRequest<Result<IEnumerable<PaymentDto>>>, IRequest<IEnumerable>, IRequest<IEnumerable<PaymentDto>>
    {
        
    }
}