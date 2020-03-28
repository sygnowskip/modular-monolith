using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using MediatR;
using ModularMonolith.Payments.Contracts;
using ModularMonolith.Registrations.Commands;
using ModularMonolith.Registrations.Contracts;
using ModularMonolith.Registrations.Contracts.Requests;
using ModularMonolith.Registrations.Language;
using ModularMonolith.Registrations.ValueObjects;
using ModularMonolith.Time;

namespace ModularMonolith.Registrations.ApplicationServices
{
    internal class RegistrationApplicationService : IRegistrationApplicationService
    {
        private readonly IMediator _mediator;
        private readonly IPaymentsApplicationService _paymentsApplicationService;
        private readonly ISystemTimeProvider _systemTimeProvider;

        public RegistrationApplicationService(IMediator mediator, IPaymentsApplicationService paymentsApplicationService, ISystemTimeProvider systemTimeProvider)
        {
            _mediator = mediator;
            _paymentsApplicationService = paymentsApplicationService;
            _systemTimeProvider = systemTimeProvider;
        }


        public Task<Result<RegistrationId>> Create(RegistrationCreationRequest request)
        {
            return DateOfBirth.Create(request.DateOfBirth, _systemTimeProvider)
                .OnSuccess(dateOfBirth => Candidate.Create(request.FirstName, request.LastName, dateOfBirth))
                .OnSuccess(async candidate => await _mediator.Send(new CreateRegistrationCommand(candidate)));
        }

        public async Task<Result> StartPaymentAsync(RegistrationId id)
        {
            return await _paymentsApplicationService.StartPayment(id.Identifier)
                .OnSuccess(async paymentId => await _mediator.Send(new StartPaymentForRegistration(id, paymentId)));
        }

        public async Task<Result> MarkAsPaid(RegistrationId id)
        {
            return await _mediator.Send(new MarkRegistrationAsPaid(id));
        }
    }
}