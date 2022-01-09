using System;
using Hexure.Identifiers.Guid;

namespace ModularMonolith.Registrations.Language
{
    public class ExternalRegistrationId : Identifier
    {
        public ExternalRegistrationId() : base(Guid.NewGuid())
        {
        }
        
        protected ExternalRegistrationId(Guid value) : base(value)
        {
        }
    }
}