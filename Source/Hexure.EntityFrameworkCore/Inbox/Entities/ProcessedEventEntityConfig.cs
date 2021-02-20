using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hexure.EntityFrameworkCore.Inbox.Entities
{
    public class ProcessedEventEntityConfig : IEntityTypeConfiguration<ProcessedEventEntity>
    {
        public void Configure(EntityTypeBuilder<ProcessedEventEntity> builder)
        {
            builder.ToTable("ProcessedEvent", "events");
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .ValueGeneratedOnAdd();
        }
    }
}