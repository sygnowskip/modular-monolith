using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModularMonolith.Exams.Language;
using ModularMonolith.ReadModels.Planning;

namespace ModularMonolith.ReadModels.Persistence.Planning
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToView(nameof(Exam), Schemas.Read);
            
            builder.Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<ExamStatus>());
        }
    }
}