using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularMonolith.ReadModels.Common;

namespace ModularMonolith.ReadModels.Persistence.Common
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable(nameof(Location), Schemas.Read);
            builder.HasKey(location => location.Id);
        }
    }
}