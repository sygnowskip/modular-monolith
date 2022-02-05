using Hexure.Results;

namespace ModularMonolith.Registrations.Domain
{
    public static class RegistrationErrors
    {
        public static Error.ErrorType CandidateCannotBeEmpty = new Error.ErrorType(nameof(CandidateCannotBeEmpty), "Candidate cannot be empty");
        
        public static class Actions
        {
            public static readonly Error.ErrorType UnableToCancel =
                new Error.ErrorType(nameof(UnableToCancel), "Unable to open for registration");

            public static readonly Error.ErrorType UnableToMarkAsPaid =
                new Error.ErrorType(nameof(UnableToMarkAsPaid), "Unable to close registration");
        }
    }
}