using System;
using Hexure.Identifiers.Guid;

namespace ModularMonolith.Registrations.Language
{
    public sealed class RegistrationId : Identifier
    {
        public RegistrationId(Guid value) : base(value)
        {
        }
    }
}