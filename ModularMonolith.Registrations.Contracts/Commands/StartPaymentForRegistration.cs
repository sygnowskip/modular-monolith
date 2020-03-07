using CSharpFunctionalExtensions;
using MediatR;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts.Commands
{
    public class StartPaymentForRegistration : IRequest<Result>
    {
        public StartPaymentForRegistration(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }
}