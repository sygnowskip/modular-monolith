using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Payments.Contracts.Commands;
using ModularMonolith.Payments.Contracts.Events;

namespace ModularMonolith.Payments.Commands
{
    public class CompletePaymentCommandHandler : IRequestHandler<CompletePayment, Result>
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
                .ToResult($"Unable to get payment with id: {request.Id}")
                .Tap(async payment =>
                {
                    payment.CompletePayment();

                    //TODO: Event should be on aggregate
                    await _mediator.Publish(new PaymentStarted(payment.Id, payment.CorrelationId), cancellationToken);
                });
        }
    }
}