using System;
using System.Collections.Generic;
using MediatR;

namespace ModularMonolith.Registrations.Contracts.Queries
{
    public class UnpaidRegistrationDto
    {
        public UnpaidRegistrationDto(RegistrationId id, Enum status)
        {
            Id = id;
            Status = status;
        }

        public RegistrationId Id { get; }
        //TODO: Problem
        public Enum Status { get; }
    }

    public class UnpaidRegistrations : IRequest<IEnumerable<UnpaidRegistrationDto>>
    {
        
    }
}