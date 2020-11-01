using Microsoft.EntityFrameworkCore;
using ModularMonolith.Exams.Domain;

namespace ModularMonolith.Exams.Persistence
{
    public interface IExamDbContext
    {
        DbSet<Exam> Exams { get; }
    }
}