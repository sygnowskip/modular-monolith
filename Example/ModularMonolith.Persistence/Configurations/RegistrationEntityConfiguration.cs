using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularMonolith.Registrations;

namespace ModularMonolith.Persistence.Configurations
{
    internal class RegistrationEntityConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.ToTable(nameof(Registration), Schemas.Registrations);
        }
    }
}