using System;
using System.Collections.Generic;
using MediatR;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Contracts.Registrations
{
    public class UnpaidRegistrationDto
    {
        public UnpaidRegistrationDto(Guid id, RegistrationStatus status)
        {
            Id = id;
            Status = status;
        }

        public Guid Id { get; }
        //TODO: Problem
        public RegistrationStatus Status { get; }
    }

    public class UnpaidRegistrations : IRequest<IEnumerable<UnpaidRegistrationDto>>
    {
        
    }
}