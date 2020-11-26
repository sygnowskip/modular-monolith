using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Payments.Contracts.Events;
using ModularMonolith.Payments.Language;

namespace ModularMonolith.Payments.Commands
{
    internal class CompletePayment : IRequest<Result>
    {
        public CompletePayment(PaymentId id)
        {
            Id = id;
        }

        public PaymentId Id { get; }
    }

    internal class CompletePaymentCommandHandler : IRequestHandler<CompletePayment, Result>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediator _mediator;

        public CompletePaymentCommandHandler(IPaymentRepository paymentRepository, IMediator mediator)
        {
            _paymentRepository = paymentRepository;
            _mediator = mediator;
        }

        public async Task<Result> Handle(CompletePayment request, CancellationToken cancellationToken)
        {
            return await _paymentRepository.GetAsync(request.Id)
                .OnSuccess(async payment =>
                {
                    payment.CompletePayment();

                    //TODO: Event should be on aggregate
                    await _mediator.Publish(new PaymentStarted(payment.Id, payment.CorrelationId), cancellationToken);
                });
        }
    }
}