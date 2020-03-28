using System;

namespace ModularMonolith.Registrations.Contracts.Requests
{
    public class RegistrationCreationRequest
    {
        public RegistrationCreationRequest(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
    }
}