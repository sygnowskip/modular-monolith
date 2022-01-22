using Hexure.EntityFrameworkCore.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Persistence.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable(nameof(Exam), Schemas.Exams);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IdentifierGeneratedOnAdd();
            
            builder.OwnsOne(e => e.SubjectId, navigationBuilder =>
            {
                navigationBuilder.Property(capacity => capacity.Value)
                    .HasColumnName(nameof(Exam.SubjectId));
            });
            builder.OwnsOne(e => e.LocationId, navigationBuilder =>
            {
                navigationBuilder.Property(capacity => capacity.Value)
                    .HasColumnName(nameof(Exam.LocationId));
            });
            builder.OwnsOne(e => e.Capacity, navigationBuilder =>
            {
                navigationBuilder.Property(capacity => capacity.Value)
                    .HasColumnName(nameof(Exam.Capacity));
            });
            builder.OwnsOne(e => e.Booked, navigationBuilder =>
            {
                navigationBuilder.Property(booked => booked.Value)
                    .HasColumnName(nameof(Exam.Booked));
            });
            builder.OwnsOne(e => e.RegistrationStartDate, navigationBuilder =>
            {
                navigationBuilder.Property(rsd => rsd.Value)
                    .HasColumnName(nameof(Exam.RegistrationStartDate));
            });
            builder.OwnsOne(e => e.RegistrationEndDate, navigationBuilder =>
            {
                navigationBuilder.Property(red => red.Value)
                    .HasColumnName(nameof(Exam.RegistrationEndDate));
            });
            builder.OwnsOne(e => e.ExamDateTime, navigationBuilder =>
            {
                navigationBuilder.Property(rdt => rdt.Value)
                    .HasColumnName(nameof(Exam.ExamDateTime));
            });
            builder.Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<ExamStatus>());
        }
    }
}