using Hexure.Events.Serialization;
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

            builder.Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            builder.OwnsOne(entity => entity.SerializedEvent, navigationBuilder =>
            {
                navigationBuilder.Property(@event => @event.Namespace)
                    .HasColumnName(
                        $"{nameof(SerializedEventEntity.SerializedEvent)}{nameof(SerializedEvent.Namespace)}");
                navigationBuilder.Property(@event => @event.Payload)
                    .HasColumnName(
                        $"{nameof(SerializedEventEntity.SerializedEvent)}{nameof(SerializedEvent.Payload)}");
                navigationBuilder.Property(@event => @event.Type)
                    .HasColumnName(
                        $"{nameof(SerializedEventEntity.SerializedEvent)}{nameof(SerializedEvent.Type)}");
            });
            builder.Navigation(e => e.SerializedEvent).IsRequired();
        }
    }
}