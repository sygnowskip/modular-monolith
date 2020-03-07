using System.Collections.Generic;
using MediatR;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts.Queries
{
    public class UnpaidRegistrationDto
    {
        public UnpaidRegistrationDto(RegistrationId id, RegistrationStatus status)
        {
            Id = id;
            Status = status;
        }

        public RegistrationId Id { get; }
        //TODO: Problem
        public RegistrationStatus Status { get; }
    }

    public class UnpaidRegistrations : IRequest<IEnumerable<UnpaidRegistrationDto>>
    {
        
    }
}