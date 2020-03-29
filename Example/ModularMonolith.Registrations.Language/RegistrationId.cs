using System;

namespace ModularMonolith.Registrations.Language
{
    public class RegistrationId
    {
        private RegistrationId(Guid identifier)
        {
            Identifier = identifier;
        }

        public Guid Identifier { get; private set; }

        public static RegistrationId CreateFor(Guid id)
        {
            return new RegistrationId(id);
        }
        public static RegistrationId CreateNew()
        {
            return new RegistrationId(Guid.NewGuid());
        }
    }
}