using System.Linq;

namespace ModularMonolith.ReadModels
{
    public interface IMonolithQueryDbContext
    {
        IQueryable<Location> Locations { get; }
        IQueryable<Subject> Subjects { get; }
    }
}