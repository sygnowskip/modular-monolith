using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModularMonolith.ReadModels.Common;

namespace ModularMonolith.ReadModels.Persistence.Common
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable(nameof(Subject), Schemas.Read);
            builder.HasKey(subject => subject.Id);
        }
    }
}