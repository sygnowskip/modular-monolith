using CSharpFunctionalExtensions;
using MediatR;

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