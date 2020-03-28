using System;
using System.Threading.Tasks;
using Hexure.Results;
using MediatR;
using ModularMonolith.Payments.Commands;
using ModularMonolith.Payments.Contracts;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments.ApplicationServices
{
    public class PaymentsApplicationService : IPaymentsApplicationService
    {
        private readonly IMediator _mediator;

        public PaymentsApplicationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<PaymentId>> StartPayment(Guid correlationId)
        {
            return await _mediator.Send(new StartPayment(correlationId));
        }

        public async Task<Result> CompletePayment(PaymentId id)
        {
            return await _mediator.Send(new CompletePayment(id));
        }
    }
}
