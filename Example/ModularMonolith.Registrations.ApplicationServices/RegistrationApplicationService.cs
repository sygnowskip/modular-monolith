using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Payments.Contracts;
using ModularMonolith.Registrations.Commands;
using ModularMonolith.Registrations.Contracts;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.ApplicationServices
{
    internal class RegistrationApplicationService : IRegistrationApplicationService
    {
        private readonly IMediator _mediator;
        private readonly IPaymentsApplicationService _paymentsApplicationService;

        public RegistrationApplicationService(IMediator mediator, IPaymentsApplicationService paymentsApplicationService)
        {
            _mediator = mediator;
            _paymentsApplicationService = paymentsApplicationService;
        }

        public async Task<Result> StartPaymentAsync(RegistrationId id)
        {
            return await _paymentsApplicationService.StartPayment(id.Identifier)
                .Bind(async paymentId => await _mediator.Send(new StartPaymentForRegistration(id, paymentId)));
        }

        public async Task<Result> MarkAsPaid(RegistrationId id)
        {
            return await _mediator.Send(new MarkRegistrationAsPaid(id));
        }
    }
}