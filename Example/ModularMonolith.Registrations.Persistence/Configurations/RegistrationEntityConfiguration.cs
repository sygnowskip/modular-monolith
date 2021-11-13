using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularMonolith.Registrations.Domain;
using ModularMonolith.Registrations.Language.ValueObjects;

namespace ModularMonolith.Registrations.Persistence.Configurations
{
    public class RegistrationEntityConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.ToTable(nameof(Registration), Schemas.Registrations);
            builder.HasKey(r => r.Id);
            builder.OwnsOne(r => r.Candidate, navigationBuilder =>
            {
                navigationBuilder
                    .Property(c => c.FirstName)
                    .HasColumnName($"{nameof(Candidate)}{nameof(Candidate.FirstName)}");

                navigationBuilder
                    .Property(c => c.LastName)
                    .HasColumnName($"{nameof(Candidate)}{nameof(Candidate.LastName)}");

                navigationBuilder
                    .OwnsOne(c => c.DateOfBirth, ownedNavigationBuilder =>
                    {
                        ownedNavigationBuilder
                            .Property(birth => birth.Value)
                            .HasColumnName($"{nameof(Candidate)}{nameof(DateOfBirth)}");
                    });
            });

            //TODO: Temporary
            builder.Ignore(r => r.Payment);
        }
    }
}