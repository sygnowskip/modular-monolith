using System.Linq;
using ModularMonolith.ReadModels.Common;
using ModularMonolith.ReadModels.Planning;

namespace ModularMonolith.ReadModels
{
    public interface IMonolithQueryDbContext
    {
        IQueryable<Location> Locations { get; }
        IQueryable<Subject> Subjects { get; }
        IQueryable<Exam> Exams { get; }
    }
}