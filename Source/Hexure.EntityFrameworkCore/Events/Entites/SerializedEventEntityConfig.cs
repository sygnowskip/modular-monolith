using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexure.EntityFrameworkCore.Events.Entites
{
    public class SerializedEventEntityConfig : IEntityTypeConfiguration<SerializedEventEntity>
    {
        public void Configure(EntityTypeBuilder<SerializedEventEntity> builder)
        {
            builder.ToTable(nameof(SerializedEventEntity.SerializedEvent), "events");
            builder.HasKey(entity => entity.Id);

            builder.OwnsOne(entity => entity.SerializedEvent);
        }
    }
}