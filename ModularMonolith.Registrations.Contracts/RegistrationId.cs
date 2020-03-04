using System;

namespace ModularMonolith.Registrations.Contracts
{
    public class RegistrationId
    {
        public RegistrationId()
        {
            Identifier = Guid.NewGuid();
        }

        public Guid Identifier { get; }
    }
}