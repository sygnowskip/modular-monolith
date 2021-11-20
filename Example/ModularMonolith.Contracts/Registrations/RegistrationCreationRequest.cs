using System;

namespace ModularMonolith.Contracts.Registrations
{
    public class RegistrationCreationRequest
    {
        public RegistrationCreationRequest(string firstName, string lastName, DateTime dateOfBirth, long examId)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ExamId = examId;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public long ExamId { get; }
    }
}