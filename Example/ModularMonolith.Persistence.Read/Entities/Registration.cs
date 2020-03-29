using System;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Persistence.Read.Entities
{
    public class Registration
    {
        public RegistrationId Id { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
    }
}