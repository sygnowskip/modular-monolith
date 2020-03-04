using CSharpFunctionalExtensions;
using MediatR;

namespace ModularMonolith.Registrations.Contracts.Commands
{
    public class MarkRegistrationAsPaid : IRequest<Result>
    {
        public MarkRegistrationAsPaid(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }
}