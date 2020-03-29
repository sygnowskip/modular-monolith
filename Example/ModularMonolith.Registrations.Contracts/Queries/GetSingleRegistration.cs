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
        public GetSingleRegistrationDto(RegistrationId id, RegistrationStatus status, string candidateFirstName, string candidateLastName, DateTime candidateDateOfBirth)
        {
            Id = id;
            Status = status;
            CandidateFirstName = candidateFirstName;
            CandidateLastName = candidateLastName;
            CandidateDateOfBirth = candidateDateOfBirth;
        }

        public RegistrationId Id { get; }
        public RegistrationStatus Status { get; }
        public string CandidateFirstName { get; }
        public string CandidateLastName { get; }
        public DateTime CandidateDateOfBirth { get; }

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