using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace ModularMonolith.Registrations.ValueObjects
{
    public static class CandidateErrors
    {
        public static Error.ErrorType FirstNameCannotBeEmpty = new Error.ErrorType(nameof(FirstNameCannotBeEmpty), "First name cannot be empty");
        public static Error.ErrorType LastNameCannotBeEmpty = new Error.ErrorType(nameof(LastNameCannotBeEmpty), "Last name cannot be empty");
    }

    public class Candidate : ValueObject
    {
        private Candidate()
        {
        }

        private Candidate(string firstName, string lastName, DateOfBirth dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public DateOfBirth DateOfBirth { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return DateOfBirth;
        }

        public static Result<Candidate> Create(string firstName, string lastName, DateOfBirth dateOfBirth)
        {
            return Result.Create(!string.IsNullOrWhiteSpace(firstName), CandidateErrors.FirstNameCannotBeEmpty.Build())
                .OnSuccess(() => Result.Create(!string.IsNullOrWhiteSpace(lastName),
                    CandidateErrors.LastNameCannotBeEmpty.Build()))
                .OnSuccess(() => new Candidate(firstName, lastName, dateOfBirth));
        }
    }
}