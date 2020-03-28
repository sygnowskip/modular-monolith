using System;
using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Payments.Contracts.Events;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments.Commands
{
    internal class StartPayment : IRequest<Result<PaymentId>>
    {
        public StartPayment(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }

    internal class StartPaymentCommandHandler : IRequestHandler<StartPayment, Result<PaymentId>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediator _mediator;

        public StartPaymentCommandHandler(IPaymentRepository paymentRepository, IMediator mediator)
        {
            _paymentRepository = paymentRepository;
            _mediator = mediator;
        }

        public async Task<Result<PaymentId>> Handle(StartPayment request, CancellationToken cancellationToken)
        {
            return await Payment.Create(request.CorrelationId)
                .OnSuccess(async payment =>
                {
                    await _paymentRepository.SaveAsync(payment);
                    
                    //TODO: Event should be on aggregate
                    await _mediator.Publish(new PaymentStarted(payment.Id, payment.CorrelationId), cancellationToken);
                    return payment.Id;
                });
        }
    }
}