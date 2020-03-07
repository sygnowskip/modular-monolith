using System;

namespace ModularMonolith.Registrations.Language
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