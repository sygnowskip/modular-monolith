using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Persistence.Read.Entities
{
    public class Registration
    {
        public RegistrationId Id { get; set; }
        public RegistrationStatus Status { get; set; }
    }
}