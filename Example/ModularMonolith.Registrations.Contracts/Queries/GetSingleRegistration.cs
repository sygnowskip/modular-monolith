using System;
using Hexure.Results;
using MediatR;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Registrations.Contracts.Queries
{
    public class GetSingleRegistrationErrors
    {
        public static Error.ErrorType UnableToFindRegistration = new Error.ErrorType(nameof(UnableToFindRegistration), "Unable to find registration for specified identifier");
    }

    public class GetSingleRegistrationDto
    {
        public GetSingleRegistrationDto(RegistrationId id, RegistrationStatus status, string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = id;
            Status = status;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public RegistrationId Id { get; }
        public RegistrationStatus Status { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }

    }

    public class GetSingleRegistration : IRequest<Result<GetSingleRegistrationDto>>
    {
        public GetSingleRegistration(RegistrationId id)
        {
            Id = id;
        }

        public RegistrationId Id { get; }
    }
}