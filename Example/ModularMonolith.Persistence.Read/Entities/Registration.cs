using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularMonolith.Registrations.Language;

namespace ModularMonolith.Persistence.Read.Entities
{
    public class Registration
    {
        private Registration() { }

        private Registration(RegistrationId id, RegistrationStatus status, string candidateFirstName, string candidateLastName, DateTime candidateDateOfBirth)
        {
            Id = id;
            Status = status;
            CandidateFirstName = candidateFirstName;
            CandidateLastName = candidateLastName;
            CandidateDateOfBirth = candidateDateOfBirth;
        }

        public RegistrationId Id { get; }
        public RegistrationStatus Status { get; }
        public string CandidateFirstName { get; }
        public string CandidateLastName { get; }
        public DateTime CandidateDateOfBirth { get; }
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