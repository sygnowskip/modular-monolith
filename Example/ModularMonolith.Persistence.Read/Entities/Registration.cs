using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Persistence.Read.Entities
{
    public class Registration
    {
        public RegistrationId Id { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public string CandidateFirstName { get; private set; }
        public string CandidateLastName { get; private set; }
        public DateTime CandidateDateOfBirth { get; private set; }
    }

    internal class RegistrationEntityConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.ToTable(nameof(Registration), Schemas.Registrations);
            builder.HasKey(registration => registration.Id);
        }
    }
}